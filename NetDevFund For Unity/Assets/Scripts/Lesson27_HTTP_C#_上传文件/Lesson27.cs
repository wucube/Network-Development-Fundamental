using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class Lesson27 : MonoBehaviour
{
    #region 知识点一 上传文件到HTTP资源服务器需要遵守的规则
    //上传文件时内容的必备规则
    //  1:ContentType = "multipart/form-data; boundary=边界字符串";

    //  2:上传的数据必须按照格式写入流中
    //  --边界字符串
    //  Content-Disposition: form-data; name="字段名字，之后写入的文件2进制数据和该字段名对应";filename="传到服务器上使用的文件名"
    //  Content-Type:application/octet-stream（由于我们传2进制文件 所以这里使用2进制）
    //  空一行
    //  （这里直接写入传入的内容）
    //  --边界字符串--

    //  3:保证服务器允许上传
    //  4:写入流前需要先设置ContentLength内容长度
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        //1.创建HttpWebRequest对象
        HttpWebRequest req = HttpWebRequest.Create("http://192.168.10.4:8080/HTTP_Server/") as HttpWebRequest;
        //2.相关设置（请求类型，内容类型，超时，身份验证等）
        req.Method = WebRequestMethods.Http.Post;
        req.ContentType = "multipart/form-data;boundary=MrTang";
        req.Timeout = 50000;
        req.Credentials = new NetworkCredential("cube","cube");
        req.PreAuthenticate = true;//先验证身份 再上传数据

        //3.按格式拼接字符串并且转为字节数组之后用于上传
        //3-1.文件数据前的头部信息
        //  --边界字符串
        //  Content-Disposition: form-data; name="字段名字，之后写入的文件2进制数据和该字段名对应";filename="传到服务器上使用的文件名"
        //  Content-Type:application/octet-stream（由于我们传2进制文件 所以这里使用2进制）
        //  空一行
        string head = "--MrTang\r\n" +
            "Content-Disposition:form-data;name=\"file\";filename=\"http上传的文件.png\"\r\n" +
            "Content-Type:application/octet-stream\r\n\r\n";
        //头部拼接字符串规则信息的字节数组
        byte[] headBytes= Encoding.UTF8.GetBytes(head);

        //3-2.结束的边界信息
        //  --边界字符串--
        byte[] endBytes = Encoding.UTF8.GetBytes("\r\n--MrTang--\r\n");

        //4.写入上传流
        using (FileStream localFileStream=File.OpenRead(Application.streamingAssetsPath + "/test.png"))
        {
            //4-1.设置上传长度
            //总长度 是前部分字符串 + 文件本身有多大 + 后部分边界字符串
            req.ContentLength = headBytes.Length + localFileStream.Length + endBytes.Length;

            //用于上传的流
            Stream upLoadStream = req.GetRequestStream();
            //4-2.先写入前部分头部信息
            upLoadStream.Write(headBytes, 0, headBytes.Length);
            //4-3.再写入文件数据
            byte[] bytes=new byte[2048];
            int contentLength = localFileStream.Read(bytes, 0, bytes.Length);
            while (contentLength != 0)
            {
                upLoadStream.Write(bytes, 0, contentLength);
                contentLength = localFileStream.Read(bytes, 0, bytes.Length);
            }

            //4-4.在写入结束的边界信息
            upLoadStream.Write(endBytes, 0, endBytes.Length);

            upLoadStream.Close();
            localFileStream.Close();
        }
        //5.上传数据，获取响应
        HttpWebResponse res = req.GetResponse() as HttpWebResponse;
        if (res.StatusCode == HttpStatusCode.OK)
        {
            print("上传通信成功");
        }
        else
        {
            print("上传失败"+res.StatusCode);
        }

    }
}
