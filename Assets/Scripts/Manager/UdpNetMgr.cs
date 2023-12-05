using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine;

public class UdpNetMgr : MonoBehaviour
{
    private static UdpNetMgr instance;
    public static UdpNetMgr Instance => instance;

    private EndPoint serverIpPoint;

    private Socket socket;

    //客户端socket是否关闭
    private bool isClose = true;

    //两个容器 队列
    //接受和发送消息的队列 在多线程里面可以操作
    private Queue<BaseMsg> sendQueue = new Queue<BaseMsg>();
    private Queue<BaseMsg> receiveQueue = new Queue<BaseMsg>();

    private byte[] cacheBytes = new byte[512];

    // Start is called before the first frame update
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
            BaseMsg baseMsg = receiveQueue.Dequeue();
            switch (baseMsg)
            {
                case PlayerMsg msg:
                    print(msg.playerID);
                    print(msg.playerData.name);
                    print(msg.playerData.atk);
                    print(msg.playerData.lev);
                    break;
            }
        }
    }

    /// <summary>
    /// 启动客户端socket相关的方法
    /// </summary>
    /// <param name="ip">远端服务器的IP</param>
    /// <param name="port">远端服务器的port</param>
    public void StartClient(string ip, int port)
    {
        //如果当前是开启状态 就不用再开了
        if (!isClose)
            return;

        //先记录服务器地址，一会发消息时会使用 
        serverIpPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        IPEndPoint clientIpPort = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(clientIpPort);
            isClose = false;
            print("客户端网络启动");
            ThreadPool.QueueUserWorkItem(ReceiveMsg);
            ThreadPool.QueueUserWorkItem(SendMsg);
        }
        catch (System.Exception e)
        {
            print("启动Socket出问题" + e.Message);
        }
    }

    private void ReceiveMsg(object obj)
    {
        EndPoint tempIpPoint = new IPEndPoint(IPAddress.Any, 0);
        int nowIndex;
        int msgID;
        int msgLength;
        while (!isClose)
        {
            if (socket != null && socket.Available > 0)
            {
                try
                {
                    socket.ReceiveFrom(cacheBytes, ref tempIpPoint);
                    //为了避免处理 非服务器发来的 骚扰消息
                    if (!tempIpPoint.Equals(serverIpPoint))
                        continue;//如果发现 发消息给你的 不是服务器 那么证明是骚扰消息 就不用处理

                    //处理服务器发来的消息
                    nowIndex = 0;
                    //解析ID
                    msgID = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;
                    //解析长度
                    msgLength = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;
                    //解析消息体
                    BaseMsg msg = null;
                    switch (msgID)
                    {
                        case 1001:
                            msg = new PlayerMsg();
                            //反序列化消息体
                            msg.Reading(cacheBytes, nowIndex);
                            break;
                    }
                    if (msg != null)
                        receiveQueue.Enqueue(msg);
                }
                catch (SocketException s)
                {
                    print("接受消息出问题" + s.SocketErrorCode + s.Message);
                }
                catch (Exception e)
                {
                    print("接受消息出问题(非网络问题)" + e.Message);
                }
            }
        }
    }

    private void SendMsg(object obj)
    {
        while (!isClose)
        {
            if (socket != null && sendQueue.Count > 0)
            {
                try
                {
                    socket.SendTo(sendQueue.Dequeue().Writing(), serverIpPoint);
                }
                catch (SocketException s)
                {
                    print("发送消息出错" + s.SocketErrorCode + s.Message);
                }
            }
        }
    }

    //发送消息
    public void Send(BaseMsg msg)
    {
        sendQueue.Enqueue(msg);
    }

    //关闭socket
    public void Close()
    {
        if (socket != null)
        {
            isClose = true;
            QuitMsg msg = new QuitMsg();
            //发送一个退出消息给服务器 让其移除记录
            socket.SendTo(msg.Writing(), serverIpPoint);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }

    }

    private void OnDestroy()
    {
        Close();
    }
}
