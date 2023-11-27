using System.Net.Sockets;
using System.Net;
using System.Text;

/*使用多线程
 * 允许多个客户端连入服务端
 * 并且服务端可以分别和多个客户端进行通信
 */
namespace TcpServerSyncExercise1
{
    public class Program
    {
        static Socket socket;
        //用于存储 客户端连入的 Socket 之后可以获取他们来进行通信
        static List<Socket> clientSockets = new List<Socket>();

        static bool isClose = false;
        static void Main(string[] args)
        {
            //1.建立Socket 绑定 监听 
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            socket.Bind(ipPoint);
            socket.Listen(1024);

            //2.等待客户端连接（这节课需要特别处理的地方）
            Thread acceptThread = new Thread(AcceptClientConnect);
            acceptThread.Start();

            //3.收发消息（这节课需要特别处理的地方）
            Thread receiveThread = new Thread(ReceiveMsg);
            receiveThread.Start();

            //4.关闭相关
            while (true)
            {
                string input = Console.ReadLine();
                //定义一个规则 关闭服务器 断开所有连接
                if (input == "Quit")
                {
                    isClose = true;
                    for (int i = 0; i < clientSockets.Count; i++)
                    {
                        clientSockets[i].Shutdown(SocketShutdown.Both);
                        clientSockets[i].Close();
                    }
                    clientSockets.Clear();
                    break;
                }
                //定义一个规则 广播消息 就是让所有客户端收到服务端发送的消息
                else if (input.Substring(0, 2) == "B:")
                {
                    for (int i = 0; i < clientSockets.Count; i++)
                    {
                        clientSockets[i].Send(Encoding.UTF8.GetBytes(input.Substring(2)));
                    }
                }
            }
        }

        static void AcceptClientConnect()
        {
            while (!isClose)
            {
                Socket clientSocket = socket.Accept();
                clientSockets.Add(clientSocket);
                clientSocket.Send(Encoding.UTF8.GetBytes("欢迎你连入服务端"));
            }
        }

        static void ReceiveMsg()
        {
            Socket clientSocket;
            byte[] result = new byte[1024 * 1024];
            int receiveNum;
            int i;
            while (!isClose)
            {
                for (i = 0; i < clientSockets.Count; i++)
                {
                    clientSocket = clientSockets[i];
                    //判断 该socket是否有可以接收的消息 返回值就是字节数
                    if (clientSocket.Available > 0)
                    {
                        //客户端即使没有发消息过来 这句代码也会执行
                        receiveNum = clientSocket.Receive(result);
                        //如果直接在这收到消息 就处理 可能造成问题
                        //不能够即使的处理别人的消息
                        //为了不影响别人消息的处理 我们把消息处理 交给新的线程，为了节约线程相关的开销 我们使用线程池
                        ThreadPool.QueueUserWorkItem(HandleMsg, (clientSocket, Encoding.UTF8.GetString(result, 0, receiveNum)));
                    }

                }
            }
        }

        static void HandleMsg(object obj)
        {
            (Socket s, string str) info = ((Socket s, string str))obj;
            Console.WriteLine("收到客户端{0}发来的信息：{1}", info.s.RemoteEndPoint, info.str);
        }
    }
}
