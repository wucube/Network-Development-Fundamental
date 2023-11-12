using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson28 : MonoBehaviour
{
    public RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        //下载HTTP服务器上的内容
        //StartCoroutine(DownLoadHttp());
        //StartCoroutine(DownLoadFtp());
        StartCoroutine(DownLoadLocal());
    }

    IEnumerator DownLoadHttp()
    {
        #region 知识点一 WWW类的作用
        //WWW是Unity提供给我们简单的访问网页的类
        //我们可以通过该类下载和上传一些数据
        //在使用http协议时，默认的请求类型是Get，如果想要Post上传，需要配合下节课学习的WWWFrom类使用
        //它主要支持的协议
        //1.http://和https:// 超文本传输协议
        //2.ftp:// 文件传输协议（但仅限于匿名下载）
        //3.file:// 本地文件传输协议，可以使用该协议异步加载本地文件（PC、IOS、Android都支持）
        //我们本节课主要学习利用WWW来进行数据的下载或加载

        //注意：
        //1.该类一般配合协同程序使用
        //2.该类在较新Unity版本中会提示过时，但是仍可以使用，新版本将其功能整合进了UnityWebRequest类（之后讲解）
        #endregion

        #region 知识点二 WWW类的常用方法和变量
        #region 常用方法
        //1.WWW：构造函数，用于创建一个WWW请求
        //WWW www = new WWW("http://192.168.50.109:8000/Http_Server/实战就业路线.jpg");
        //2.GetAudioClip：从下载数据返回一个音效切片AudioClip对象
        //www.GetAudioClip()
        //3.LoadImageIntoTexture：用下载数据中的图像来替换现有的一个Texture2D对象
        //Texture2D tex = new Texture2D(100, 100);
        //www.LoadImageIntoTexture(tex);
        //4.LoadFromCacheOrDownload：从缓存加载AB包对象，如果该包不在缓存则自动下载存储到缓存中，以便以后直接从本地缓存中加载
        //WWW.LoadFromCacheOrDownload("http://192.168.50.109:8000/Http_Server/test.assetbundle", 1);
        #endregion

        #region 常用变量
        //1.assetBundle：如果加载的数据是AB包，可以通过该变量直接获取加载结果
        //www.assetBundle
        //2.audioClip：如果加载的数据是音效切片文件，可以通过该变量直接获取加载结果
        //www.GetAudioClip
        //3.bytes：以字节数组的形式获取加载到的内容
        //www.bytes
        //4.bytesDownloaded：过去已下载的字节数
        //www.bytesDownloaded
        //5.error：返回一个错误消息，如果下载期间出现错误，可以通过它获取错误信息
        //www.error != null
        //6.isDone：判断下载是否已经完成
        //www.isDone
        //7.movie：如果下载的视频，可以获取一个MovieTexture类型结果
        //www.GetMovieTexture()
        //8.progress:下载进度
        //www.progress
        //9.text：如果下载的数据是字符串，以字符串的形式返回内容
        //www.text
        //10.texture：如果下载的数据是图片，以Texture2D的形式返回加载结果
        //www.texture
        #endregion
        #endregion

        //1.创建WWW对象
        //WWW www = new WWW("http://192.168.10.4:8080/HTTP_Server/技能的本质.png");
        WWW www = new WWW("https://webcdn01.aboutcg.org/5a39792e8be24338a0ab9263493c6efe?imageMogr2/quality/85");

        //2.等待加载结束
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return null;
        }

        print(www.bytesDownloaded);
        print(www.progress);

        //3.使用加载结束后的资源
        if (www.error == null)
        {
            rawImage.texture = www.texture;
        }
        else
        {
            print(www.error);
        }
    }


    IEnumerator DownLoadFtp()
    {
        //1.创建WWW对象
        WWW www = new WWW("ftp://127.0.0.1/技能的本质.jpg");

        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return null;
        }

        print(www.bytesDownloaded);
        print(www.progress);

        if (www.error == null)
        {
            rawImage.texture = www.texture;
        }
        else
        {
            print(www.error);
        }
    }

    IEnumerator DownLoadLocal()
    {
        //1.创建WWW对象
        WWW www = new WWW("file://" + Application.streamingAssetsPath + "/test.png");

        //2.就是等待加载结束
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return null;
        }

        print(www.bytesDownloaded);
        print(www.progress);

        //3.使用加载结束后的资源
        if (www.error == null)
        {
            rawImage.texture = www.texture;
        }
        else
            print(www.error);
    }
}
