using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

public class Lesson12 : MonoBehaviour
{
    private byte[] resultBytes = new byte[1024];
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 异步方法和同步方法的区别
        //同步方法：
        //方法中逻辑执行完毕后，再继续执行后面的方法
        //异步方法：
        //方法中逻辑可能还没有执行完毕，就继续执行后面的内容

        //异步方法的本质
        //往往异步方法当中都会使用多线程执行某部分逻辑
        //因为我们不需要等待方法中逻辑执行完毕就可以继续执行下面的逻辑了

        //注意：Unity中的协同程序中的某些异步方法，有的使用的是多线程有的使用的是迭代器分步执行
        //关于协同程序可以回顾Unity基础当中讲解协同程序原理的知识点
        #endregion

        #region 知识点二 举例说明异步方法原理
        //我们以一个异步倒计时方法举例
        //1.线程回调
        //CountDownAsync(5, ()=> {
        //    print("倒计时结束");
        //});
        //print("异步执行后的逻辑");

        //2.async和await 会等待线程执行完毕 继续执行后面的逻辑
        //相对第一种方式 可以让函数分步执行
        CountDownAsync(5);
        print("异步执行后的逻辑2");
        #endregion

        #region 知识点三 Socket TCP通信中的异步方法（Begin开头方法）
        //回调函数参数IAsyncResult
        //AsyncState 调用异步方法时传入的参数 需要转换
        //AsyncWaitHandle 用于同步等待

        Socket socketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //服务器相关
        //BeginAccept
        //EndAccept
        socketTcp.BeginAccept(AcceptCallBack, socketTcp);

        //客户端相关
        //BeginConnect
        //EndConnect
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        socketTcp.BeginConnect(ipPoint, (result) =>
        {
            Socket s = result.AsyncState as Socket;
            try
            {
                s.EndConnect(result);
                print("连接成功");
            }
            catch (SocketException e)
            {
                print("连接出错" + e.SocketErrorCode + e.Message);
            }

        }, socketTcp);


        //服务器客户端通用
        //接收消息
        //BeginReceive
        //EndReceive
        socketTcp.BeginReceive(resultBytes, 0, resultBytes.Length, SocketFlags.None, ReceiveCallBack, socketTcp);

        //发送消息
        //BeginSend
        //EndSend
        byte[] bytes = Encoding.UTF8.GetBytes("1231231231223123123");
        socketTcp.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, (result) =>
        {
            try
            {
                socketTcp.EndSend(result);
                print("发送成功");
            }
            catch (SocketException e)
            {
                print("发送错误" + e.SocketErrorCode + e.Message);
            }
        }, socketTcp);
        #endregion

        #region 知识点四 Socket TCP通信中的异步方法2（Async结尾方法）
        //关键变量类型
        //SocketAsyncEventArgs
        //它会作为Async异步方法的传入值
        //我们需要通过它进行一些关键参数的赋值

        //服务器端
        //AcceptAsync
        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        e.Completed += (socket, args) =>
        {
            //首先判断是否成功
            if (args.SocketError == SocketError.Success)
            {
                //获取连入的客户端socket
                Socket clientSocket = args.AcceptSocket;

                (socket as Socket).AcceptAsync(args);
            }
            else
            {
                print("连入客户端失败" + args.SocketError);
            }
        };
        socketTcp.AcceptAsync(e);

        //客户端
        //ConnectAsync
        SocketAsyncEventArgs e2 = new SocketAsyncEventArgs();
        e2.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                //连接成功
            }
            else
            {
                //连接失败
                print(args.SocketError);
            }
        };
        socketTcp.ConnectAsync(e2);

        //服务端和客户端
        //发送消息
        //SendAsync
        SocketAsyncEventArgs e3 = new SocketAsyncEventArgs();
        byte[] bytes2 = Encoding.UTF8.GetBytes("123123的就是拉法基萨克两地分居");
        e3.SetBuffer(bytes2, 0, bytes2.Length);
        e3.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                print("发送成功");
            }
            else
            {

            }
        };
        socketTcp.SendAsync(e3);

        //接受消息
        //ReceiveAsync
        SocketAsyncEventArgs e4 = new SocketAsyncEventArgs();
        //设置接受数据的容器，偏移位置，容量
        e4.SetBuffer(new byte[1024 * 1024], 0, 1024 * 1024);
        e4.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                //收取存储在容器当中的字节
                //Buffer是容器
                //BytesTransferred是收取了多少个字节
                Encoding.UTF8.GetString(args.Buffer, 0, args.BytesTransferred);

                args.SetBuffer(0, args.Buffer.Length);
                //接收完消息 再接收下一条
                (socket as Socket).ReceiveAsync(args);
            }
            else
            {

            }
        };
        socketTcp.ReceiveAsync(e4);
        #endregion

        #region 总结
        //C#中网络通信 异步方法中 主要提供了两种方案
        //1.Begin开头的API
        //内部开多线程，通过回调形式返回结果，需要和End相关方法 配合使用

        //2.Async结尾的API
        //内部开多线程，通过回调形式返回结果，依赖SocketAsyncEventArgs对象配合使用
        //可以让我们更加方便的进行操作
        #endregion
    }

    private void AcceptCallBack(IAsyncResult result)
    {
        try
        {
            //获取传入的参数
            Socket s = result.AsyncState as Socket;
            //通过调用EndAccept就可以得到连入的客户端Socket
            Socket clientSocket = s.EndAccept(result);

            s.BeginAccept(AcceptCallBack, s);
        }
        catch (SocketException e)
        {
            print(e.SocketErrorCode);
        }
    }

    private void ReceiveCallBack(IAsyncResult result)
    {
        try
        {
            Socket s = result.AsyncState as Socket;
            //这个返回值是你受到了多少个字节
            int num = s.EndReceive(result);
            //进行消息处理
            Encoding.UTF8.GetString(resultBytes, 0, num);

            //我还要继续接受
            s.BeginReceive(resultBytes, 0, resultBytes.Length, SocketFlags.None, ReceiveCallBack, s);
        }
        catch (SocketException e)
        {
            print("接受消息处问题" + e.SocketErrorCode + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CountDownAsync(int second, UnityAction callBack)
    {
        Thread t = new Thread(() =>
        {
            while (true)
            {
                print(second);
                Thread.Sleep(1000);
                --second;
                if (second == 0)
                    break;
            }
            callBack?.Invoke();
        });
        t.Start();

        print("开始倒计时");
    }

    public async void CountDownAsync(int second)
    {
        print("倒计时开始");

        await Task.Run(() =>
        {
            while (true)
            {
                print(second);
                Thread.Sleep(1000);
                --second;
                if (second == 0)
                    break;
            }
        });

        print("倒计时结束");
    }
}
