using System;

namespace TeachUdpServerExercises
{
    class Program
    {
        public static ServerSocket serverSocket;
        static void Main(string[] args)
        {
            #region UDP服务器要求
            //如同TCP通信一样让UDP服务端可以服务多个客户端
            //需要具备的功能有：
            //1.区分消息类型（不需要处理分包、黏包）
            //2.能够接受多个客户端的消息
            //3.能够主动给发送过消息给自己的客户端发消息（记录客户端信息）
            //4.主动记录上一次收到客户端消息的时间，如果长时间没有收到消息，主动移除记录的客户端信息

            //分析：
            //1.UDP是无连接的，我们如何记录连入的客户端？
            //2.UDP收发消息都是通过一个Socket来进行处理，我们应该如何处理收发消息？
            //3.如果不使用心跳消息，我们如何记录上次收到消息的时间？

            serverSocket = new ServerSocket();
            serverSocket.Start("127.0.0.1", 8080);

            Console.WriteLine("UDP服务器启动了");

            while (true)
            {
                string input = Console.ReadLine();
                if(input.Substring(0,2) == "B:")
                {
                    PlayerMsg msg = new PlayerMsg();
                    msg.playerData = new PlayerData();
                    msg.playerID = 1001;
                    msg.playerData.name = "唐老狮的UDP服务器";
                    msg.playerData.atk = 88;
                    msg.playerData.lev = 66;
                    serverSocket.Broadcast(msg);
                }
            }

            #endregion
        }
    }
}
