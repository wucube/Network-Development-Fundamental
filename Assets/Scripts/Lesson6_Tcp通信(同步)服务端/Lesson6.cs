using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class Lesson6 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 回顾客户端需要做的事情
        //1.创建套接字Socket
        //2.用Connect方法与服务端相连
        //3.用Send和Receive相关方法收发数据
        //4.用Shutdown方法释放连接
        //5.关闭套接字
        #endregion

        #region 知识点二 实现客户端基本逻辑
        //1.创建套接字Socket
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //2.用Connect方法与服务端相连
        //确定服务端的IP和端口
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        try
        {
            socket.Connect(ipPoint);
        }
        catch (SocketException e)
        {
            if (e.ErrorCode == 10061)
                print("服务器拒绝连接");
            else
                print("连接服务器失败" + e.ErrorCode);
            return;
        }
        //3.用Send和Receive相关方法收发数据

        //接收数据
        byte[] receiveBytes = new byte[1024];
        int receiveNum = socket.Receive(receiveBytes);
        print("收到服务端发来的消息：" + Encoding.UTF8.GetString(receiveBytes, 0, receiveNum));

        //发送数据
        socket.Send(Encoding.UTF8.GetBytes("你好，我是唐老狮的客户端"));

        //4.用Shutdown方法释放连接
        socket.Shutdown(SocketShutdown.Both);
        //5.关闭套接字
        socket.Close();
        #endregion

        #region 总结
        //1.客户端连接的流程每次都是相同的
        //2.客户端的 Connect、Send、Receive是会阻塞主线程的，要等到执行完毕才会继续执行后面的内容
        //抛出问题：
        //如何让客户端的Socket不影响主线程，并且可以随时收发消息？
        //我们会在之后的综合练习题讲解
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
