using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
  
public class HttpMgr
{
    private static HttpMgr instance = new HttpMgr();
    public static HttpMgr Instance => instance;

    private string HTTP_PATH = "http://192.168.10.4:8080/HTTP_Server/";

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
}