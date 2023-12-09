using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lesson32 : MonoBehaviour
{
    public RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownLoadTexture());

        //StartCoroutine(DownLoadAB());

        StartCoroutine(DownLoadCustomHandler());
    }
    IEnumerator DownLoadTexture()
    {
        UnityWebRequest uwr = new UnityWebRequest("http://192.168.10.9:8080/HTTP_Server/技能的本质.png", UnityWebRequest.kHttpVerbGET);
        //uwr.method = UnityWebRequest.kHttpVerbGET;

        //1.DownloadHandlerBuffer
        //DownloadHandlerBuffer bufferHandler = new DownloadHandlerBuffer();
        //uwr.downloadHandler = bufferHandler;

        //2.DownloadHandlerFile
        //print(Application.persistentDataPath);
        //uwr.downloadHandler = new DownloadHandlerFile(Application.persistentDataPath + "/downloadFile.png");

        //3.DownloadHandlerTexture
        DownloadHandlerTexture textureHandler = new DownloadHandlerTexture();
        uwr.downloadHandler = textureHandler;

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            //获取字节数组
            //bufferHandler.data;

            //textureHandler.texture;
            rawImage.texture = textureHandler.texture;
        }
        else
        {
            print("获取数据失败" + uwr.result + uwr.error + uwr.responseCode);
        }
    }

    IEnumerator DownLoadAB()
    {
        UnityWebRequest uwr = new UnityWebRequest("http://192.168.10.9:8080/HTTP_Server/lua", UnityWebRequest.kHttpVerbGET);
        //参数二：需要已知的校检码才能校验完整性， 如果不知道只能传0，不进行校验。
        //通常在AB包热更新时，服务器发送对应的文件列表中包含验证码才能校验
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(uwr.url, 0);
        uwr.downloadHandler = handler;

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            AssetBundle ab = handler.assetBundle;
            print(ab.name);
        }
        else
        {
            print("获取数据失败" + uwr.result + uwr.error + uwr.responseCode);
        }
    }

    IEnumerator DownLoadAudioClip()
    {
        UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("http://192.168.50.109:8000/Http_Server/音效名.mp3",AudioType.MPEG);
        yield return uwr.SendWebRequest();

        if(uwr.result==UnityWebRequest.Result.Success)
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(uwr);
        }
        else
        {
            print("获取数据失败" + uwr.result + uwr.error + uwr.responseCode);
        }
    }

    IEnumerator DownLoadCustomHandler()
    {
        UnityWebRequest uwr = new UnityWebRequest("http://192.168.10.9:8080/HTTP_Server/简易的三消小游戏.mp4", UnityWebRequest.kHttpVerbGET);

        //使用自定义的下载处理对象 来处理获取到的2进制字节数组:将视频存储到本地
        print(Application.persistentDataPath);
        uwr.downloadHandler = new CustomDownLoadFileHandler(Application.persistentDataPath + "/三消小游戏.mp4");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            print("存储本地成功");
        }
        else
        {
            print("获取数据失败" + uwr.result + uwr.error + uwr.responseCode);
        }
    }

}

public class CustomDownLoadFileHandler : DownloadHandlerScript
{
    private string savePath;

    private byte[] cacheBytes;

    private int index = 0;

    public CustomDownLoadFileHandler() : base() { }

    public CustomDownLoadFileHandler(byte[] bytes):base(bytes) { }

    public CustomDownLoadFileHandler(string path) : base() { savePath = path; }

    protected override byte[] GetData() { return cacheBytes; }//返回字节数组

    /// <summary>
    /// 从服务器接收数据后，每帧自动调用
    /// </summary>
    /// <param name="data"></param>
    /// <param name="dataLength"></param>
    /// <returns></returns>
    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        Debug.Log("收到数据长度: "+data.Length);
        Debug.Log("收到数据长度dataLength: " + dataLength);
        data.CopyTo(cacheBytes, index);
        index+= dataLength;
        return true;
    }

    /// <summary>
    /// 从服务器收到 COntent-Length标头时  会自动调用的方法
    /// </summary>
    /// <param name="contentLength"></param>
    protected override void ReceiveContentLengthHeader(ulong contentLength)
    {
        //base.ReceiveContentLengthHeader(contentLength);

        Debug.Log("收到数据长度：" + contentLength);
        //根据收到的标头 决定字节数组容器的大小
        cacheBytes = new byte[contentLength];
    }

    /// <summary>
    ///  消息收完后会自动调用的方法
    /// </summary>
    protected override void CompleteContent()
    {
        Debug.Log("消息收完");
        //把收到的字节数组进行自定义处理:这里将收到的字节数组存储到本地
        File.WriteAllBytes(savePath, cacheBytes);
    }
}
