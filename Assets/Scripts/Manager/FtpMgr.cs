using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class FtpMgr
{
    private static FtpMgr instance = new FtpMgr();

    public static FtpMgr Instance => instance;

    //远端FTP服务器的地址
    private string FTP_PATH = "ftp://192.168.50.49/";
    //用户名和密码
    private string USER_NAME = "MrTang";
    private string PASSWORD = "MrTang123";

    /// <summary>
    /// 上传文件到Ftp服务器（异步）
    /// </summary>
    /// <param name="fileName">FTP上的文件名</param>
    /// <param name="localPath">本地文件路径</param>
    /// <param name="action">上传完毕后想要做什么的委托函数</param>
    public async void UpLoadFile(string fileName, string localPath, UnityAction action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                //通过一个线程执行这里面的逻辑 那么就不会影响主线程了
                //1.创建一个Ftp连接
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                //2.进行一些设置
                //凭证
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                //是否操作结束后 关闭 控制连接
                req.KeepAlive = false;
                //传输类型
                req.UseBinary = true;
                //操作类型
                req.Method = WebRequestMethods.Ftp.UploadFile;
                //代理设置为空
                req.Proxy = null;
                //3.上传
                Stream upLoadStream = req.GetRequestStream();
                //开始上传
                using (FileStream fileStream = File.OpenRead(localPath))
                {
                    byte[] bytes = new byte[1024];
                    //返回值 为具体读取了多少个字节
                    int contentLength = fileStream.Read(bytes, 0, bytes.Length);
                    //有数据就上传
                    while (contentLength != 0)
                    {
                        //读了多少就写(上传)多少
                        upLoadStream.Write(bytes, 0, contentLength);
                        //继续从本地文件中读取数据
                        contentLength = fileStream.Read(bytes, 0, bytes.Length);
                    }
                    //上传结束
                    fileStream.Close();
                    upLoadStream.Close();
                }
                Debug.Log("上传成功");
            }
            catch (Exception e)
            {
                Debug.Log("上传文件出错" + e.Message);
            }
        });
        //上传结束后 你想在外部做的事情
        action?.Invoke();
    }

    /// <summary>
    /// 下载文件从Ftp服务器当中（异步）
    /// </summary>
    /// <param name="fileName">FTP上想要下载的文件名</param>
    /// <param name="localPath">存储的本地文件路径</param>
    /// <param name="action">下载完毕后想要做什么的委托函数</param>
    public async void DownLoadFile(string fileName, string localPath, UnityAction action = null)
    {
        await Task.Run(() => {
            try
            {
                //1.创建一个Ftp连接
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                //2.进行一些设置
                //凭证
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                //是否操作结束后 关闭 控制连接
                req.KeepAlive = false;
                //传输类型
                req.UseBinary = true;
                //操作类型
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                //代理设置为空
                req.Proxy = null;
                //3.下载
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                Stream downLoadStream = res.GetResponseStream();
                //写入到本地文件中
                using (FileStream fileStream = File.Create(localPath))
                {
                    byte[] bytes = new byte[1024];
                    //读取数据
                    int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    //一点一点的写入
                    while (contentLength != 0)
                    {
                        //读多少 写多少
                        fileStream.Write(bytes, 0, contentLength);
                        //继续读
                        contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    }
                    fileStream.Close();
                    downLoadStream.Close();
                }

                Debug.Log("下载成功");
            }
            catch (Exception e)
            {
                Debug.Log("下载失败" + e.Message);
            }
        });

        //如果下载结束有想做的事情 在这里调用外部传入的委托函数
        action?.Invoke();
    }
}
