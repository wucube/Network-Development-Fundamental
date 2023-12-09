using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Lesson20 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 使用FTP上传文件关键点
        //1.通信凭证
        //  进行Ftp连接操作时需要的账号密码
        //2.操作命令 WebRequestMethods.Ftp
        //  设置你想要进行的Ftp操作
        //3.文件流相关 FileStream 和 Stream
        //  上传和下载时都会使用的文件流
        //4.保证FTP服务器已经开启
        //  并且能够正常访问
        #endregion

        #region 知识点二 FTP上传
        try
        {
            //1.创建一个Ftp连接
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://192.168.50.49/pic.png")) as FtpWebRequest;
            //2.设置通信凭证(如果不支持匿名 就必须设置这一步)
            //将代理相关信息置空 避免 服务器同时有http相关服务 造成冲突
            req.Proxy = null;
            NetworkCredential n = new NetworkCredential("MrTang", "MrTang123");
            req.Credentials = n;
            //请求完毕后 是否关闭控制连接，如果想要关闭，可以设置为false
            req.KeepAlive = false;
            //3.设置操作命令
            req.Method = WebRequestMethods.Ftp.UploadFile;//设置命令操作为 上传文件
            //4.指定传输类型
            req.UseBinary = true;
            //5.得到用于上传的流对象
            Stream upLoadStream = req.GetRequestStream();

            //6.开始上传
            using (FileStream file = File.OpenRead(Application.streamingAssetsPath + "/test.png"))
            {
                //我们可以一点一点的把这个文件中的字节数组读取出来 然后存入到 上传流中
                byte[] bytes = new byte[1024];

                //返回值 是真正从文件中读了多少个字节
                int contentLength = file.Read(bytes, 0, bytes.Length);
                //不停的去读取文件中的字节 除非读取完毕了 不然一直读 并且写入到上传流中
                while (contentLength != 0)
                {
                    //写入上传流中
                    upLoadStream.Write(bytes, 0, contentLength);
                    //写完了继续读
                    contentLength = file.Read(bytes, 0, bytes.Length);
                }
                //除了循环就证明 写完了 
                file.Close();
                upLoadStream.Close();
                //上传完毕
                print("上传结束");
            }
        }
        catch (Exception e)
        {
            print("上传出错 失败" + e.Message);
        }
        #endregion

        #region 总结
        //C#已经把Ftp相关操作封装的很好了
        //我们只需要熟悉API，直接使用他们进行FTP上传即可
        //我们主要做的操作是
        //把本地文件流读出字节数据写入到要上传的FTP流中

        //FTP上传相关API也有异步方法
        //使用上和以前的TCP相关类似
        //这里不赘述
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
