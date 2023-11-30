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
    private byte[] receiveBytes = new byte[1024 * 1024];
    //返回收到的字节数
    private int receiveNum;

    //是否连接
    private bool isConnected = false;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (receiveQueue.Count > 0)
        {
            BaseMsg msg = receiveQueue.Dequeue();
            if (msg is PlayerMsg)
            {
                PlayerMsg playerMsg = (msg as PlayerMsg);
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
                receiveNum = socket.Receive(receiveBytes);
                //首先把收到字节数组的前4个字节  读取出来得到ID
                int msgID = BitConverter.ToInt32(receiveBytes, 0);
                BaseMsg baseMsg = null;
                switch (msgID)
                {
                    case 1001:
                        PlayerMsg msg = new PlayerMsg();
                        msg.Reading(receiveBytes, 4);
                        baseMsg = msg;
                        break;
                }
                //如果消息为空 那证明是不知道类型的消息 没有解析
                if (baseMsg == null)
                    continue;
                //收到消息 解析消息为字符串 并放入公共容器
                receiveQueue.Enqueue(baseMsg);
            }
        }
    }

    public void Close()
    {
        if (socket != null)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            isConnected = false;
        }
    }

    private void OnDestroy()
    {
        Close();
    }
}
