using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class Lesson26 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //关键点：将Content-Type设置为 application/x-www-form-urlencoded 键值对类型
        HttpWebRequest req = HttpWebRequest.Create("http://192.168.10.4:8080/HTTP_Server/") as HttpWebRequest;
        req.Method = WebRequestMethods.Http.Post;
        req.Timeout = 2000;

        //设置上传的内容的类型
        req.ContentType = "application/x-www-form-urlencoded";

        //要上传的数据
        string str = "Name=MrTang&ID=2";
        byte[] bytes=Encoding.UTF8.GetBytes(str);

        //上传之前一定要设置内容的长度
        req.ContentLength = bytes.Length;

        //上传数据
        Stream stream  = req.GetRequestStream();
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        //发送数据 得到响应结果
        HttpWebResponse res = req.GetResponse() as HttpWebResponse;
        print(res.StatusCode);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
