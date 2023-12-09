using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lesson30 : MonoBehaviour
{
    public RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 UnityWebRequest是什么？
        //UnityWebRequest是一个Unity提供的一个模块化的系统类
        //用于构成HTTP请求和处理HTTP响应
        //它主要目标是让Unity游戏和Web服务端进行交互
        //它将之前WWW的相关功能都集成在了其中
        //所以新版本中都建议使用UnityWebRequest类来代替WWW类

        //它在使用上和WWW很类似
        //主要的区别就是UnityWebRequest把下载下来的数据处理单独提取出来了
        //我们可以根据自己的需求选择对应的数据处理对象来获取数据

        //注意：
        //1.UnityWebRequest和WWW一样，需要配合协同程序使用
        //2.UnityWebRequest和WWW一样，支持http、ftp、file协议下载或加载资源
        //3.UnityWebRequest能够上传文件到HTTP资源服务器
        #endregion

        #region 知识点二 UnityWebRequest类的常用操作
        //1.使用Get请求获取文本或二进制数据
        //2.使用Get请求获取纹理数据
        //3.使用Get请求获取AB包数据
        //4.使用Post请求发送数据
        //5.使用Put请求上传数据
        #endregion

        //1.获取文本或2进制
        StartCoroutine(LoadText());
        //2.获取纹理
        StartCoroutine(LoadTexture());
        //3.获取AB包
        StartCoroutine(LoadAB());

    }

    IEnumerator LoadText()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("http://192.168.10.4:8080/HTTP_Server/test.txt");
        //等待服务器端响应后，断开连接后，再继续执行后面的内容
        yield return uwr.SendWebRequest();

        //如果处理成功，结果就是成功枚举
        if (uwr.result == UnityWebRequest.Result.Success)
        {
            //文件字符串
            print(uwr.downloadHandler.text);
            //字节数组
            byte[] bytes = uwr.downloadHandler.data;
            print("字节数组长度" + bytes.Length);
        }
        else
        {
            print("获取失败:" + uwr.result + uwr.error + uwr.responseCode);
        }
    }

    IEnumerator LoadTexture()
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("http://192.168.10.4:8080/HTTP_Server/技能的本质.png");
        //UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("ftp://127.0.0.1/技能的本质.png");
        //UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("file://"+Application.streamingAssetsPath+"/test.png");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            //(uwr.downloadHandler as DownloadHandlerTexture).texture;

            //DownloadHandlerTexture.GetContent(uwr);

            //rawImage.texture = (uwr.downloadHandler as DownloadHandlerTexture).texture;

            rawImage.texture = DownloadHandlerTexture.GetContent(uwr);
        }
        else
            print("获取失败" + uwr.error + uwr.result + uwr.responseCode);

    }

    IEnumerator LoadAB()
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("http://192.168.10.4:8080/HTTP_Server/lua");
        uwr.SendWebRequest();

        while (!uwr.isDone)
        {
            print(uwr.downloadProgress);
            print(uwr.downloadedBytes);

            yield return null;
        }

        //yield return uwr.SendWebRequest();

        print(uwr.downloadProgress);
        print(uwr.downloadedBytes);

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            //AssetBundle ab = (uwr.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(uwr);
            print(ab.name);
        }
        else
            print("获取失败" + uwr.error + uwr.result + uwr.responseCode);

    }
}
