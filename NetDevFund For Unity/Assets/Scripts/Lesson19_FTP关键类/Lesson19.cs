using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Lesson19 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 NetworkCredential类
        //命名空间：System.Net
        //NetworkCredential通信凭证类
        //用于在Ftp文件传输时，设置账号密码
        NetworkCredential n = new NetworkCredential("MrTang", "MrTang123");
        #endregion

        #region 知识点二 FtpWebRequest类
        //命名空间：System.Net
        //Ftp文件传输协议客户端操作类
        //主要用于：上传、下载、删除服务器上的文件

        //重要方法
        //1.Create 创建新的WebRequest，用于进行Ftp相关操作
        FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/Test.txt")) as FtpWebRequest;
        //2.Abort  如果正在进行文件传输，用此方法可以终止传输
        req.Abort();
        //3.GetRequestStream  获取用于上传的流
        Stream s = req.GetRequestStream();
        //4.GetResponse  返回FTP服务器响应
        //FtpWebResponse res = req.GetResponse() as FtpWebResponse;

        //重要成员
        //1.Credentials 通信凭证，设置为NetworkCredential对象
        req.Credentials = n;
        //2.KeepAlive bool值，当完成请求时是否关闭到FTP服务器的控制连接（默认为true，不关闭）
        req.KeepAlive = false;
        //3.Method  操作命令设置
        //  WebRequestMethods.Ftp类中的操作命令属性
        //  DeleteFile  删除文件
        //  DownloadFile    下载文件    
        //  ListDirectory   获取文件简短列表
        //  ListDirectoryDetails    获取文件详细列表
        //  MakeDirectory   创建目录
        //  RemoveDirectory 删除目录
        //  UploadFile  上传文件
        req.Method = WebRequestMethods.Ftp.DownloadFile;
        //4.UseBinary 是否使用2进制传输
        req.UseBinary = true;
        //5.RenameTo    重命名
        //req.RenameTo = "myTest.txt";
        #endregion

        #region 知识点三 FtpWebResponse类
        //命名空间：System.Net
        //它是用于封装FTP服务器对请求的响应
        //它提供操作状态以及从服务器下载数据
        //我们可以通过FtpWebRequest对象中的GetResponse()方法获取
        //当使用完毕时，要使用Close释放

        //通过它来真正的从服务器获取内容
        FtpWebResponse res = req.GetResponse() as FtpWebResponse;

        //重要方法：
        //1.Close:释放所有资源
        res.Close();
        //2.GetResponseStream：返回从FTP服务器下载数据的流
        Stream stream = res.GetResponseStream();

        //重要成员：
        //1.ContentLength:接受到数据的长度
        print(res.ContentLength);
        //2.ContentType：接受数据的类型
        print(res.ContentType);
        //3.StatusCode:FTP服务器下发的最新状态码
        print(res.StatusCode);
        //4.StatusDescription:FTP服务器下发的状态代码的文本
        print(res.StatusDescription);
        //5.BannerMessage:登录前建立连接时FTP服务器发送的消息
        print(res.BannerMessage);
        //6.ExitMessage:FTP会话结束时服务器发送的消息
        //7.LastModified:FTP服务器上的文件的上次修改日期和时间
        #endregion

        #region 总结
        //通过C#提供的这3个类
        //我们便可以完成客户端向FTP服务器
        //操作文件的需求，比如
        //上传、下载、删除文件
        #endregion
    }
}
