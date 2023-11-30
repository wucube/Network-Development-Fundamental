namespace TcpServerSyncExercise2
{

    /*使用多线程，并基于面向对象的思想
    * 允许多个客户端连入服务端
    * 并且服务端可以分别和多个客户端进行通信
    */
    class Program
    {
        static void Main(string[] args)
        {
            ServerSocket socket = new ServerSocket();
            socket.Start("127.0.0.1", 8080, 1024);
            Console.WriteLine("服务器开启成功");
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "Quit")
                {
                    socket.Close();
                }
                else if (input.Substring(0, 2) == "B:")
                {
                    if (input.Substring(2) == "1001")
                    {
                        PlayerMsg msg = new PlayerMsg();
                        msg.playerID = 9876;
                        msg.playerData = new PlayerData();
                        msg.playerData.name = "服务器端发来的消息";
                        msg.playerData.lev = 99;
                        msg.playerData.atk = 80;
                        socket.Broadcast(msg);
                    }
                }
            }
        }
    }
}
