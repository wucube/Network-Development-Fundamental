using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
  
public class HttpMgr
{
    private static HttpMgr instance = new HttpMgr();
    public static HttpMgr Instance => instance;

    private string HTTP_PATH = "http://192.168.10.4:8080/HTTP_Server/";
    private string USER_NAME = "cube";
    private string PASS_WORD = "cube";

    /// <summary>
    /// 下载指定文件到本地指定路径中
    /// </summary>
    /// <param name="fileName">远程文件名</param>
    /// <param name="loacFilePath">本地路径</param>
    /// <param name="action">下载结束后的回调函数</param>
    public async void DownLoadFileAsync(string fileName, string loacFilePath, UnityAction<HttpStatusCode> action)
    {
        HttpStatusCode result = HttpStatusCode.OK;
        await Task.Run(() =>
        {
            try
            {
                //判断文件是否存在 Head 
                //1.创建HTTP连接对象
                HttpWebRequest req = HttpWebRequest.Create(HTTP_PATH + fileName) as HttpWebRequest;
                //2.设置请求类型 和 其它相关参数
                req.Method = WebRequestMethods.Http.Head;
                req.Timeout = 2000;
                //3.发送请求
                HttpWebResponse res = req.GetResponse() as HttpWebResponse;

                //存在才下载
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    res.Close();
                    //下载
                    //1.创建HTTP连接对象
                    req = HttpWebRequest.Create(HTTP_PATH + fileName) as HttpWebRequest;
                    //2.设置请求类型 和 其它相关参数
                    req.Method = WebRequestMethods.Http.Get;
                    req.Timeout = 2000;
                    //3.发送请求
                    res = req.GetResponse() as HttpWebResponse;
                    //4.存储数据到本地
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        //存储数据
                        using (FileStream fileStream = File.Create(loacFilePath))
                        {
                            Stream stream = res.GetResponseStream();
                            byte[] bytes = new byte[4096];
                            int contentLength = stream.Read(bytes, 0, bytes.Length);

                            while (contentLength != 0)
                            {
                                fileStream.Write(bytes, 0, contentLength);
                                contentLength = stream.Read(bytes, 0, bytes.Length);
                            }

                            fileStream.Close();
                            stream.Close();
                        }
                        result = HttpStatusCode.OK;
                    }
                    else
                    {
                        result = res.StatusCode;
                    }
                }
                else
                {
                    result = res.StatusCode;
                }

                res.Close();
            }
            catch (WebException w)
            {
                result = HttpStatusCode.InternalServerError;
                Debug.Log("下载出错" + w.Message + w.Status);
            }
        });

        action?.Invoke(result);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="fileName">传到远端服务器上的文件名</param>
    /// <param name="loacalFilePath">本地的文件路径</param>
    /// <param name="action">上传结束后的回调函数</param>
    public async void UpLoadFileAsync(string fileName, string loacalFilePath, UnityAction<HttpStatusCode> action)
    {
        HttpStatusCode result = HttpStatusCode.BadRequest;
        await Task.Run(() =>
        {
            try
            {
                HttpWebRequest req = HttpWebRequest.Create(HTTP_PATH) as HttpWebRequest;
                req.Method = WebRequestMethods.Http.Post;
                req.ContentType = "multipart/form-data;boundary=MrTang";
                req.Timeout = 500000;
                req.Credentials = new NetworkCredential(USER_NAME, PASS_WORD);
                req.PreAuthenticate = true;

                //拼接字符串 头部
                string head = "--MrTang\r\n" +
                "Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\n" +
                "Content-Type:application/octet-stream\r\n\r\n";
                //替换文件名
                head = string.Format(head, fileName);//替换 head字符串的{0} 

                byte[] headBytes = Encoding.UTF8.GetBytes(head);

                //尾部的边界字符串
                byte[] endBytes = Encoding.UTF8.GetBytes("\r\n--MrTang--\r\n");

                using (FileStream localStream = File.OpenRead(loacalFilePath))
                {
                    //设置长度
                    req.ContentLength = headBytes.Length + localStream.Length + endBytes.Length;
                    //写入流
                    Stream upLoadStream = req.GetRequestStream();
                    //写入头部
                    upLoadStream.Write(headBytes, 0, headBytes.Length);
                    //写入上传文件
                    byte[] bytes = new byte[4096];
                    int contentLenght = localStream.Read(bytes, 0, bytes.Length);
                    while (contentLenght != 0)
                    {
                        upLoadStream.Write(bytes, 0, contentLenght);
                        contentLenght = localStream.Read(bytes, 0, bytes.Length);
                    }
                    //写入尾部
                    upLoadStream.Write(endBytes, 0, endBytes.Length);

                    upLoadStream.Close();
                    loacalFilePath.Clone();
                }

                HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                //让外部去处理结果 
                result = res.StatusCode;
                res.Close();
            }
            catch (WebException w)
            {
                Debug.Log("上传出错" + w.Status + w.Message);
            }
        });

        action?.Invoke(result);//在Task执行结束后调用回调委托，避免在多线程中访问Unity对象
    }
}