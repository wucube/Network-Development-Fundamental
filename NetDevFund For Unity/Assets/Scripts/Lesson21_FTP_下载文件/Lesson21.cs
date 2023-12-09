using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Lesson21 : MonoBehaviour
{
    void Start()
    {
        #region 知识点一 使用FTP下载文件关键点
        //1.通信凭证
        //  进行Ftp连接操作时需要的账号密码
        //2.操作命令 WebRequestMethods.Ftp
        //  设置你想要进行的Ftp操作
        //3.文件流相关 FileStream 和 Stream
        //  上传和下载时都会使用的文件流
        //  下载文件流使用FtpWebResponse类获取
        //4.保证FTP服务器已经开启
        //  并且能够正常访问
        #endregion

        #region 知识点二 FTP下载
        try
        {
            //1.创建一个Ftp连接
            //这里和上传不同，上传的文件名 是自己定义的  下载的文件名 一定是资源服务器上有的
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://192.168.50.49/实战就业路线.jpg")) as FtpWebRequest;
            //2.设置通信凭证(如果不支持匿名 就必须设置这一步)
            req.Credentials = new NetworkCredential("MrTang", "MrTang123");
            //请求完毕后 是否关闭控制连接，如果要进行多次操作 可以设置为false
            req.KeepAlive = false;
            //3.设置操作命令
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            //4.指定传输类型
            req.UseBinary = true;
            //代理设置为空
            req.Proxy = null;
            //5.得到用于下载的流对象
            //相当于把请求发送给FTP服务器 返回值 就会携带我们想要的信息
            FtpWebResponse res = req.GetResponse() as FtpWebResponse;
            //这就是下载的流
            Stream downLoadStream = res.GetResponseStream();

            //6.开始下载
            print(Application.persistentDataPath);
            using (FileStream fileStream = File.Create(Application.persistentDataPath + "/MrTang112233.jpg"))
            {
                byte[] bytes = new byte[1024];
                //读取下载下来的流数据
                int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                //一点一点的 下载到本地流中
                while (contentLength != 0)
                {
                    //把读取出来的字节数组 写入到本地文件流中
                    fileStream.Write(bytes, 0, contentLength);
                    //那我们继续读
                    contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                }
                //下载结束 关闭流
                downLoadStream.Close();
                fileStream.Close();
            }
            print("下载结束");
        }
        catch (Exception e)
        {
            print("下载出错" + e.Message);
        }
        #endregion

        #region 总结
        //C#已经把Ftp相关操作封装的很好了
        //我们只需要熟悉API，直接使用他们进行FTP下载即可
        //我们主要做的操作是
        //把下载文件的FTP流读出字节数据写入到本地文件流中
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
