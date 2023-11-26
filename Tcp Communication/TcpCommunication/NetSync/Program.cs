using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NetSync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region 知识点一 回顾服务端需要做的事情
            //1.创建套接字Socket
            //2.用Bind方法将套接字与本地地址绑定
            //3.用Listen方法监听
            //4.用Accept方法等待客户端连接
            //5.建立连接，Accept返回新套接字
            //6.用Send和Receive相关方法收发数据
            //7.用Shutdown方法释放连接
            //8.关闭套接字
            #endregion

            #region 知识点二 实现服务端基本逻辑
            //1.创建套接字Socket（TCP）
            Socket socketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //2.用Bind方法将套接字与本地地址绑定
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                socketTcp.Bind(ipPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine("绑定报错" + e.Message);
                return;
            }
            //3.用Listen方法监听
            socketTcp.Listen(1024);
            Console.WriteLine("服务端绑定监听结束，等待客户端连入");
            //4.用Accept方法等待客户端连接
            //5.建立连接，Accept返回新套接字
            Socket socketClient = socketTcp.Accept();
            Console.WriteLine("有客户端连入了");
            //6.用Send和Receive相关方法收发数据
            //发送
            socketClient.Send(Encoding.UTF8.GetBytes("欢迎连入服务端"));
            //接受
            byte[] result = new byte[1024];
            //返回值为接受到的字节数
            int receiveNum = socketClient.Receive(result);
            Console.WriteLine("接受到了{0}发来的消息：{1}",
                socketClient.RemoteEndPoint.ToString(),
                Encoding.UTF8.GetString(result, 0, receiveNum));

            //7.用Shutdown方法释放连接
            socketClient.Shutdown(SocketShutdown.Both);
            //8.关闭套接字
            socketClient.Close();
            #endregion

            #region 总结
            //1.服务端开启的流程每次都是相同的
            //2.服务端的 Accept、Send、Receive是会阻塞主线程的，要等到执行完毕才会继续执行后面的内容
            //抛出问题：
            //如何让服务端可以服务n个客户端？
            //我们会在之后的综合练习题进行讲解
            #endregion

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
    }
    
}
