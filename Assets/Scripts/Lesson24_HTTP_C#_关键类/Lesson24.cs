using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Lesson24 : MonoBehaviour
{
    void Start()
    {
        #region 知识点一 HttpWebRequest类
        //命名空间：System.Net
        //HttpWebRequest是主要用于发送客户端请求的类
        //主要用于：发送HTTP客户端请求给服务器，可以进行消息通信、上传、下载等等操作

        //重要方法
        //1.Create 创建新的WebRequest，用于进行HTTP相关操作
        HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.50.109:8000/Http_Server/")) as HttpWebRequest;
        //2.Abort  如果正在进行文件传输，用此方法可以终止传输 
        //req.Abort();
        //3.GetRequestStream  获取用于上传的流
        Stream s = req.GetRequestStream();
        //4.GetResponse  返回HTTP服务器响应
        HttpWebResponse res = req.GetResponse() as HttpWebResponse;
        //5.Begin/EndGetRequestStream 异步获取用于上传的流
        //req.BeginGetRequestStream()
        //6.Begin/EndGetResponse 异步获取返回的HTTP服务器响应
        //req.BeginGetResponse()

        //重要成员
        //1.Credentials 通信凭证，设置为NetworkCredential对象
        req.Credentials = new NetworkCredential("", "");
        //2.PreAuthenticate 是否随请求发送一个身份验证标头,一般需要进行身份验证时需要将其设置为true
        req.PreAuthenticate = true;

        //3.Headers 构成标头的名称/值对的集合
        //req.Headers
        //4.ContentLength 发送信息的字节数 上传信息时需要先设置该内容长度
        req.ContentLength = 100;
        //5.ContentType 在进行POST请求时，需要对发送的内容进行内容类型的设置
        //6.Method  操作命令设置
        //  WebRequestMethods.Http类中的操作命令属性
        //  Get     获取请求，一般用于获取数据
        //  Post    提交请求，一般用于上传数据，同时可以获取
        //  Head    获取和Get一致的内容，只是只会返回消息头，不会返回具体内容
        //  Put     向指定位置上传最新内容
        //  Connect 表示与代理一起使用的 HTTP CONNECT 协议方法，该代理可以动态切换到隧道
        //  MkCol   请求在请求 URI（统一资源标识符）指定的位置新建集合

        //了解该类的更多信息
        //https://docs.microsoft.com/zh-cn/dotnet/api/system.net.httpwebrequest?view=net-6.0
        #endregion

        #region 知识点二 HttpWebResponse类
        //命名空间：System.Net
        //它主要用于获取服务器反馈信息的类
        //我们可以通过HttpWebRequest对象中的GetResponse()方法获取
        //当使用完毕时，要使用Close释放

        //重要方法：
        //1.Close:释放所有资源
        //2.GetResponseStream：返回从FTP服务器下载数据的流

        //重要成员：
        //1.ContentLength:接受到数据的长度
        //2.ContentType：接受数据的类型
        //3.StatusCode:HTTP服务器下发的最新状态码
        //4.StatusDescription:HTTP服务器下发的状态代码的文本
        //5.BannerMessage:登录前建立连接时HTTP服务器发送的消息
        //6.ExitMessage:HTTP会话结束时服务器发送的消息
        //7.LastModified:HTTP服务器上的文件的上次修改日期和时间

        //了解该类的更多信息
        //https://docs.microsoft.com/zh-cn/dotnet/api/system.net.httpwebresponse?view=net-6.0
        #endregion

        #region 知识点三 NetworkCredential、Uri、Stream、FileStream类
        //这些类我们在学习Ftp时已经使用过了
        //在HTTP通讯时使用方式不变
        #endregion

        #region 总结
        //Http相关通讯类的使用和Ftp非常类似
        //只有一些细节上的区别
        //之后我们在学习上传下载时再来着重讲解
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
