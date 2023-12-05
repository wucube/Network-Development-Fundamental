using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class Lesson16 : MonoBehaviour
{
    private byte[] cacheBytes = new byte[512];
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 Socket UDP通信中的异步方法
        //通过之前的学习，UDP用到的通信相关方法主要就是
        //SendTo和ReceiveFrom
        //所以在讲解UDP异步通信时也主要是围绕着收发消息相关方法来讲解
        #endregion

        #region 知识点二 UDP通信中Begin相关异步方法
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //BeginSendTo
        byte[] bytes = Encoding.UTF8.GetBytes("123123lkdsajlfjas");
        EndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        socket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, ipPoint, SendToOver, socket);

        //BeginReceiveFrom
        socket.BeginReceiveFrom(cacheBytes, 0, cacheBytes.Length, SocketFlags.None, ref ipPoint, ReceiveFromOver, (socket, ipPoint));
        #endregion

        #region 知识点三 UDP通信中Async相关异步方法
        //SendToAsync
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        //设置要发送的数据 
        args.SetBuffer(bytes, 0, bytes.Length);
        //设置完成事件
        args.Completed += SendToAsync;
        socket.SendToAsync(args);

        //ReceiveFromAsync
        SocketAsyncEventArgs args2 = new SocketAsyncEventArgs();
        //这是设置接受消息的容器
        args2.SetBuffer(cacheBytes, 0, cacheBytes.Length);
        args2.Completed += ReceiveFromAsync;
        socket.ReceiveFromAsync(args2);
        #endregion

        #region 总结
        //由于学习了TCP相关的知识点
        //所以UDP的相关内容的学习就变得简单了
        //他们异步通信的唯一的区别就是API不同，使用规则都是一致的
        #endregion
    }

    private void SendToOver(IAsyncResult result)
    {
        try
        {
            Socket s = result.AsyncState as Socket;
            s.EndSendTo(result);
            print("发送成功");
        }
        catch (SocketException s)
        {
            print("发送失败" + s.SocketErrorCode + s.Message);
        }
    }

    private void ReceiveFromOver(IAsyncResult result)
    {
        try
        {
            (Socket s, EndPoint ipPoint) info = ((Socket, EndPoint))result.AsyncState;
            //返回值 就是接收了多少个 字节数
            int num = info.s.EndReceiveFrom(result, ref info.ipPoint);
            //处理消息

            //处理完消息 又继续接受消息
            info.s.BeginReceiveFrom(cacheBytes, 0, cacheBytes.Length, SocketFlags.None, ref info.ipPoint, ReceiveFromOver, info);
        }
        catch (SocketException s)
        {
            print("接受消息出问题" + s.SocketErrorCode + s.Message);
        }
    }


    private void SendToAsync(object s, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            print("发送成功");
        }
        else
        {
            print("发送失败");
        }
    }

    private void ReceiveFromAsync(object s, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            print("接收成功");
            //具体收了多少个字节
            //args.BytesTransferred
            //可以通过以下两种方式获取到收到的字节数组内容
            //args.Buffer
            //cacheBytes
            //解析消息

            Socket socket = s as Socket;
            //只需要设置 从第几个位置开始接 能接多少
            args.SetBuffer(0, cacheBytes.Length);
            socket.ReceiveFromAsync(args);
        }
        else
        {
            print("接收失败");
        }
    }
}
