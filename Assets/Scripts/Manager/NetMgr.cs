using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using System;

public class NetMgr : MonoBehaviour
{
    private static NetMgr instance;
    public static NetMgr Instance => instance;

    //客户端Socket
    private Socket socket;
    //用于发送消息的队列 公共容器 主线程往里面放 发送线程从里面取
    private Queue<BaseMsg> sendMsgQueue = new Queue<BaseMsg>();
    //用于接收消息的对象 公共容器 子线程往里面放 主线程从里面取
    private Queue<BaseMsg> receiveQueue = new Queue<BaseMsg>();

    //用于收消息的水桶（容器）
    //private byte[] receiveBytes = new byte[1024 * 1024];
    //返回收到的字节数
    //private int receiveNum;

    //用于处理分包时 缓存的 字节数组 和 字节数组长度
    private byte[] cacheBytes = new byte[1024 * 1024];
    private int cacheNum = 0;

    //是否连接
    private bool isConnected = false;

    //发送心跳消息的间隔时间
    private int SEND_HEART_MSG_TIME = 2;
    private HeartMsg hearMsg = new HeartMsg();

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        //客户端循环定时给服务端发送心跳消息
        InvokeRepeating("SendHeartMsg", 0, SEND_HEART_MSG_TIME);
    }

    private void SendHeartMsg()
    {
        if (isConnected)
            Send(hearMsg);
    }

    // Update is called once per frame
    void Update()
    {
        if (receiveQueue.Count > 0)
        {
            BaseMsg msg = receiveQueue.Dequeue();
            if (msg is PlayerMsg)
            {
                PlayerMsg playerMsg = msg as PlayerMsg;
                print(playerMsg.playerID);
                print(playerMsg.playerData.name);
                print(playerMsg.playerData.lev);
                print(playerMsg.playerData.atk);
            }
        }
    }

    //连接服务端
    public void Connect(string ip, int port)
    {
        //如果是连接状态 直接返回
        if (isConnected)
            return;

        if (socket == null)
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //连接服务端
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        try
        {
            socket.Connect(ipPoint);
            isConnected = true;
            //开启发送线程
            ThreadPool.QueueUserWorkItem(SendMsg);
            //开启接收线程
            ThreadPool.QueueUserWorkItem(ReceiveMsg);
        }
        catch (SocketException e)
        {
            if (e.ErrorCode == 10061)
                print("服务器拒绝连接");
            else
                print("连接失败" + e.ErrorCode + e.Message);
        }
    }

    //发送消息
    public void Send(BaseMsg msg)
    {
        sendMsgQueue.Enqueue(msg);
    }

    /// <summary>
    /// 用于测试 直接发字节数组的方法
    /// </summary>
    /// <param name="bytes"></param>
    public void SendTest(byte[] bytes)
    {
        socket.Send(bytes);
    }

    private void SendMsg(object obj)
    {
        while (isConnected)
        {
            if (sendMsgQueue.Count > 0)
            {
                socket.Send(sendMsgQueue.Dequeue().Writing());
            }
        }
    }

    //不停的接受消息
    private void ReceiveMsg(object obj)
    {
        while (isConnected)
        {
            if (socket.Available > 0)
            {
                byte[] receiveBytes = new byte[1024 * 1024];
                int receiveNum = socket.Receive(receiveBytes);
                HandleReceiveMsg(receiveBytes, receiveNum);
                ////首先把收到字节数组的前4个字节  读取出来得到ID
                //int msgID = BitConverter.ToInt32(receiveBytes, 0);
                //BaseMsg baseMsg = null;
                //switch (msgID)
                //{
                //    case 1001:
                //        PlayerMsg msg = new PlayerMsg();
                //        msg.Reading(receiveBytes, 4);
                //        baseMsg = msg;
                //        break;
                //}
                ////如果消息为空 那证明是不知道类型的消息 没有解析
                //if (baseMsg == null)
                //    continue;
                ////收到消息 解析消息为字符串 并放入公共容器
                //receiveQueue.Enqueue(baseMsg);
            }
        }
    }

    /// <summary>
    /// 处理接受消息 分包、黏包问题的方法
    /// </summary>
    /// <param name="receiveBytes"></param>
    /// <param name="receiveNum"></param>
    private void HandleReceiveMsg(byte[] receiveBytes, int receiveNum)
    {
        int msgID = 0;
        int msgLength = 0;
        int nowIndex = 0;

        //收到消息时 应该看看 之前有没有缓存的 如果有的话 我们直接拼接到后面
        receiveBytes.CopyTo(cacheBytes, cacheNum);
        cacheNum += receiveNum;

        while (true)
        {
            //每次将长度设置为-1 是避免上一次解析的数据 影响这一次的判断
            msgLength = -1;
            //处理解析一条消息
            if (cacheNum - nowIndex >= 8)
            {
                //解析ID
                msgID = BitConverter.ToInt32(cacheBytes, nowIndex);
                nowIndex += 4;
                //解析长度
                msgLength = BitConverter.ToInt32(cacheBytes, nowIndex);
                nowIndex += 4;
            }

            if (cacheNum - nowIndex >= msgLength && msgLength != -1)
            {
                //解析消息体
                BaseMsg baseMsg = null;
                switch (msgID)
                {
                    case 1001:
                        PlayerMsg msg = new PlayerMsg();
                        msg.Reading(cacheBytes, nowIndex);
                        baseMsg = msg;
                        break;
                }
                if (baseMsg != null)
                    receiveQueue.Enqueue(baseMsg);
                nowIndex += msgLength;
                if (nowIndex == cacheNum)
                {
                    cacheNum = 0;
                    break;
                }
            }
            else
            {
                //如果不满足 证明有分包 
                //那么我们需要把当前收到的内容 记录下来
                //有待下次接受到消息后 再做处理
                //receiveBytes.CopyTo(cacheBytes, 0);
                //cacheNum = receiveNum;
                //如果进行了 id和长度的解析 但是 没有成功解析消息体 那么我们需要减去nowIndex移动的位置
                if (msgLength != -1)
                    nowIndex -= 8;
                //就是把剩余没有解析的字节数组内容 移到前面来 用于缓存下次继续解析
                Array.Copy(cacheBytes, nowIndex, cacheBytes, 0, cacheNum - nowIndex);
                cacheNum = cacheNum - nowIndex;
                break;
            }
        }

    }

    public void Close()
    {
        if (socket != null)
        {
            print("客户端主动断开连接");

            //主动发送一条断开连接的消息给服务端
            //QuitMsg msg = new QuitMsg();
            //socket.Send(msg.Writing());
            //socket.Shutdown(SocketShutdown.Both);
            //socket.Disconnect(false);
            //socket.Close();
            socket = null;

            isConnected = false;
        }
    }

    private void OnDestroy()
    {
        Close();
    }
}
