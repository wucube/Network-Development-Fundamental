using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Lesson25 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 检测资源可用性
        try
        {
            //利用Head请求类型，获取信息
            //1.创建HTTP通讯用连接对象HttpWebRequest对象
            HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.50.49:8000/Http_Server/实战就业路线.jpg")) as HttpWebRequest;
            //2.设置请求类型 或 其它相关参数
            req.Method = WebRequestMethods.Http.Head;
            req.Timeout = 2000;
            //3.发送请求，获取响应结果HttpWebResponse对象
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.OK)
            {
                print("文件存在且可用");
                print(res.ContentLength);
                print(res.ContentType);

                res.Close();
            }
            else
                print("文件不能用" + res.StatusCode);
        }
        catch (WebException w)
        {
            print("获取出错" + w.Message + w.Status);
        }
        #endregion

        #region 知识点二 下载资源
        //利用Get请求类型，下载资源
        try
        {
            //1.创建HTTP通讯用连接对象HttpWebRequest对象
            HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.50.49:8000/Http_Server/实战就业路线.jpg")) as HttpWebRequest;
            //2.设置请求类型 或 其它相关参数
            req.Method = WebRequestMethods.Http.Get;
            req.Timeout = 3000;
            //3.发送请求，获取响应结果HttpWebResponse对象
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            //4.获取响应数据流，写入本地路径
            if (res.StatusCode == HttpStatusCode.OK)
            {
                print(Application.persistentDataPath);
                using (FileStream fileStream = File.Create(Application.persistentDataPath + "/httpDownLoad.jpg"))
                {
                    Stream downLoadStream = res.GetResponseStream();
                    byte[] bytes = new byte[2048];
                    //读取数据
                    int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    //一点一点的写入本地
                    while (contentLength != 0)
                    {
                        fileStream.Write(bytes, 0, contentLength);
                        contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    }
                    fileStream.Close();
                    downLoadStream.Close();
                    res.Close();
                }
                print("下载成功");
            }
            else
                print("下载失败" + res.StatusCode);
        }
        catch (WebException w)
        {
            print("下载出错" + w.Status + w.Message);
        }
        #endregion

        #region 知识点三 Get请求类型携带额外信息
        //我们在进行HTTP通信时，可以在地址后面加一些额外参数传递给服务端
        //一般在和短连接游戏服务器通讯时，需要携带额外信息
        //举例：
        //http://www.aspxfans.com:8080/news/child/index.asp?boardID=5&ID=24618&page=1
        //这个链接可以分成几部分
        //1.协议部分：取决于服务器端使用的哪种协议
        //http://  —— 普通的http超文本传输协议
        //https:// —— 加密的超文本传输协议

        //2.域名部分：
        //www.aspxfans.com
        //也可以填写服务器的公网IP地址

        //3.端口部分：
        //8080
        //可以不写，如果不写默认为80

        //4.虚拟目录部分：
        //news/child/
        //域名后的/开始，到最后一个/之前的部分

        //5.文件名部分：
        //index.asp
        //？之前的最后一个/后的部分

        //6.参数部分：
        //boardID=5&ID=24618&page=1
        //？之后的部分就是参数部分，多个参数一&分隔开
        //这里有三个参数
        //boardID = 5
        //ID = 24618
        //page = 1

        //我们在和服务端进行通信时，只要按照这种规则格式进行通信，就可以传递参数给对象
        //主要可用于：
        //1.web网站服务器
        //2.游戏短连接服务器
        //等
        #endregion

        #region 总结
        //1.Head请求类型
        //主要用于获取文件的一些基础信息 可以用于确定文件是否存在

        //2.Get请求类型 主要用于传递信息给服务器，用于获取具体信息
        //  服务器返回的信息，可以通过Response中的流来获取
        //  用Get请求时，可以在连接中携带一些额外参数(在链接后面加上 ?参数名=参数值&参数名=参数值&参数名=参数值&。。。。)
        //  正常的http服务器应用程序，都会去解析Get请求时连接中的参数进行逻辑处理（后端程序的工作）
        //  我们主要要掌握的知识点：
        //  1.额外参数按格式书写
        //  2.通过response对象中的流来获取返回的数据（数据的类型多种多样，可以是文件、自定义消息等等，我们按照规则解析即可）

        //3.在和http服务器通信时，我们经常会使用额外参数的形式传递信息，特别是以后和一些运营平台对接时

        //4.文件下载功能和Ftp非常类似，只是其中使用的类、协议、请求类型不同而已
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }
}