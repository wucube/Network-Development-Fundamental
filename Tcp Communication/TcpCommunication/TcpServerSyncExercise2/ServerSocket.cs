﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TcpServerSyncExercise2
{
    class ServerSocket
    {
        //服务端Socket
        public Socket socket;
        //客户端连接的所有Socket
        public Dictionary<int, ClientSocket> clientDic = new Dictionary<int, ClientSocket>();

        private bool isClose;

        //开启服务器端
        public void Start(string ip, int port, int num)
        {
            isClose = false;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket.Bind(ipPoint);
            socket.Listen(num);
            ThreadPool.QueueUserWorkItem(Accept);
            ThreadPool.QueueUserWorkItem(Receive);
        }

        //关闭服务器端
        public void Close()
        {
            isClose = true;
            foreach (ClientSocket client in clientDic.Values)
            {
                client.Close();
            }
            clientDic.Clear();

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }

        //接受客户端连入
        private void Accept(object obj)
        {
            while (!isClose)
            {
                try
                {
                    //连入一个客户端
                    Socket clientSocket = socket.Accept();
                    ClientSocket client = new ClientSocket(clientSocket);
                    client.Send("欢迎连入服务器");
                    clientDic.Add(client.clientID, client);
                }
                catch (Exception e)
                {
                    Console.WriteLine("客户端连入报错" + e.Message);
                }
            }
        }
        //接收客户端消息
        private void Receive(object obj)
        {
            while (!isClose)
            {
                if (clientDic.Count > 0)
                {
                    foreach (ClientSocket client in clientDic.Values)
                    {
                        client.Receive();
                    }
                }
            }
        }

        public void Broadcast(string info)
        {
            foreach (ClientSocket client in clientDic.Values)
            {
                client.Send(info);
            }
        }
    }
}
