using System.Net.Sockets;
using System.Net;
using System.Text;

namespace UdpServerSync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 实现UDP服务端通信 收发字符串
            //1.创建套接字
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //2.绑定本机地址
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
            socket.Bind(ipPoint);
            Console.WriteLine("服务器开启");
            //3.接受消息
            byte[] bytes = new byte[512];
            //这个变量主要是用来记录 谁发的信息给你 传入函数后 在内部 它会帮助我们进行赋值
            EndPoint remoteIpPoint2 = new IPEndPoint(IPAddress.Any, 0);
            int length = socket.ReceiveFrom(bytes, ref remoteIpPoint2);
            Console.WriteLine("IP:" + (remoteIpPoint2 as IPEndPoint).Address.ToString() +
                "port:" + (remoteIpPoint2 as IPEndPoint).Port +
                "发来了" +
                Encoding.UTF8.GetString(bytes, 0, length));

            //4.发送到指定目标
            //由于我们先收 所以 我们已经知道谁发了消息给我 我直接发给它就行了
            socket.SendTo(Encoding.UTF8.GetBytes("欢迎发送消息给服务器"), remoteIpPoint2);

            //5.释放关闭
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            #endregion

            Console.ReadKey();
        }
    }
}
