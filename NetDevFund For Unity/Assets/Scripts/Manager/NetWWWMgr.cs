using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetWWWMgr : MonoBehaviour
{
    private static NetWWWMgr instance;
    public static NetWWWMgr Instance => instance;

    private string HTTP_SERVER_PATH = "http://192.168.10.9:8080/HTTP_Server/";

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 提供给外部加载资源用的方法
    /// </summary>
    /// <typeparam name="T">资源的类型</typeparam>
    /// <param name="path">资源的路径 http ftp file都支持</param>
    /// <param name="action">加载结束后的回调函数 因为WWW是通过结合协同程序异步加载的 所以不能马上获取结果 需要回调获取</param>
    public void LoadRes<T>(string path, UnityAction<T> action) where T : class
    {
        StartCoroutine(LoadResAsync<T>(path, action));
    }
    private IEnumerator LoadResAsync<T>(string path, UnityAction<T> action) where T : class
    {
        //声明www对象 用于下载或加载
        WWW www = new WWW(path);
        //等待下载或者加载结束（异步）
        yield return www;
        //如果没有错误 证明加载成功
        if (www.error == null)
        {
            //根据T泛型的类型  决定使用哪种类型的资源 传递给外部
            if (typeof(T) == typeof(AssetBundle))
            {
                action?.Invoke(www.assetBundle as T);
            }
            else if (typeof(T) == typeof(Texture))
            {
                action?.Invoke(www.texture as T);
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                action?.Invoke(www.GetAudioClip() as T);
            }
            else if (typeof(T) == typeof(string))
            {
                action?.Invoke(www.text as T);
            }
            else if (typeof(T) == typeof(byte[]))
            {
                action?.Invoke(www.bytes as T);
            }
            else if (typeof(T) == typeof(Sprite)) //自定义一些类型 可能需要将bytes 转换成对应的类型来使用
            {
                //先创建一个Texture2D对象，用于把流数据转成Texture2D
                Texture2D texture = new Texture2D(200,100);
                texture.LoadImage(www.bytes);//流数据转换成Texture2D
                //创建一个Sprite,以Texture2D对象为基础
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                action?.Invoke(sprite as T);
            }
     
        }
        //如果错误 就提示别人
        else
        {
            Debug.LogError("www加载资源出错" + www.error);
        }
    }

    /// <summary>
    /// 使用WWW类异步发送消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msg"></param>
    /// <param name="action"></param>
    public void SendMsg<T>(BaseMsg msg, UnityAction<T> action) where T : BaseMsg
    {
        StartCoroutine(SendMsgAsync<T>(msg, action));
    }

    private IEnumerator SendMsgAsync<T>(BaseMsg msg, UnityAction<T> action) where T : BaseMsg
    {
        //消息发送
        WWWForm data = new WWWForm();
        //准备要发送的消息数据
        data.AddBinaryData("Msg", msg.Writing());

        WWW www = new WWW(HTTP_SERVER_PATH, data);
        //也可以直接传递 2进制字节数组 只要和后端定好规则 怎么传都是可以的
        //WWW www = new WWW("HTTP_SERVER_PATH", msg.Writing());

        //异步等待 发送结束 才会继续执行后面的代码
        yield return www;

        //发送完毕过后 收到响应 
        //这里模拟：认为后端发回来的内容 也是一个继承自BaseMsg类的一个字节数组对象
        if (www.error == null)
        {
            //先解析 ID和消息长度
            int index = 0;
            int msgID = BitConverter.ToInt32(www.bytes, index);
            index += 4;
            int msgLength = BitConverter.ToInt32(www.bytes, index);
            index += 4;
            //反序列化 BaseMsg
            BaseMsg baseMsg = null;
            switch (msgID)
            {
                case 1001:
                    baseMsg = new PlayerMsg();
                    baseMsg.Reading(www.bytes, index);
                    break;
            }
            if (baseMsg != null)
                action?.Invoke(baseMsg as T);
        }
        else
            Debug.LogError("发消息出问题" + www.error);
    }

    /// <summary>
    /// 使用 UnityWebRequest 类 异步上传文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="localPath"></param>
    /// <param name="action"></param>
    public void UploadFile(string fileName,string localPath,UnityAction<UnityWebRequest.Result> action)
    {
        StartCoroutine(UploadFileAsync(fileName, localPath, action));
    }

    private IEnumerator UploadFileAsync(string fileName,string localPath,UnityAction<UnityWebRequest.Result> action)
    {
        //添加要上传文件的数据
        List<IMultipartFormSection> dataList = new List<IMultipartFormSection>();
        dataList.Add(new MultipartFormFileSection(fileName, File.ReadAllBytes(localPath)));

        UnityWebRequest uwr = UnityWebRequest.Post(HTTP_SERVER_PATH, dataList);

        yield return uwr.SendWebRequest();

        action?.Invoke(uwr.result);

        //如果不成功
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("上传出现问题" + uwr.error + uwr.responseCode);
        }
    }

    /// <summary>
    /// 通过UnityWebRequest异步获取数据
    /// </summary>
    /// <typeparam name="T">byte[]、Texture、AssetBundle、AudioClip、object（自定义规则：如果是object证明要保存到本地）</typeparam>
    /// <param name="path">远端或者本地数据路径 http ftp file</param>
    /// <param name="action">获取成功后的回调函数</param>
    /// <param name="localPath">如果是下载到本地 需要传第3个参数</param>
    /// <param name="type">如果是下载 音效切片文件 需要穿音效类型</param>
    public void UnityWebRequestLoad<T>(string path,UnityAction<T> action,string localPath="",AudioType type = AudioType.MPEG) where T : class
    {
        StartCoroutine(UnityWebRequestLoadAsync<T>(path, action, localPath, type));
    }

    private IEnumerator UnityWebRequestLoadAsync<T>(string path,UnityAction<T> action,string localPath="",AudioType type = AudioType.MPEG) where T : class
    {
        UnityWebRequest uwr = new UnityWebRequest(path, UnityWebRequest.kHttpVerbGET);

        if (typeof(T) == typeof(byte[]))
            uwr.downloadHandler = new DownloadHandlerBuffer();
        else if(typeof(T)==typeof(TextAsset))
            uwr.downloadHandler= new DownloadHandlerBuffer();
        else if (typeof(T) == typeof(Texture))
            uwr.downloadHandler = new DownloadHandlerTexture();
        else if (typeof(T) == typeof(AssetBundle))
            uwr.downloadHandler = new DownloadHandlerAssetBundle(uwr.url, 0);//有AB包的的校验码就不传入0
        else if (typeof(T) == typeof(object))
            uwr.downloadHandler = new DownloadHandlerFile(localPath);
        else if (typeof(T) == typeof(AudioClip))
            uwr = UnityWebRequestMultimedia.GetAudioClip(path, type);
        else//如果出现没有的类型  就不用继续往下执行了
        {
            Debug.LogWarning("未知类型" + typeof(T));
            yield break;
        }

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            if (typeof(T) == typeof(byte[]))
                action?.Invoke(uwr.downloadHandler.data as T);
            else if (typeof(T) == typeof(TextAsset))
                action?.Invoke(uwr.downloadHandler.data as T);
            else if (typeof(T) == typeof(Texture))
                //action?.Invoke((req.downloadHandler as DownloadHandlerTexture).texture as T);
                action?.Invoke(DownloadHandlerTexture.GetContent(uwr) as T);
            else if (typeof(T) == typeof(AssetBundle))
                action?.Invoke((uwr.downloadHandler as DownloadHandlerAssetBundle).assetBundle as T);
            else if (typeof(T) == typeof(object))
                action?.Invoke(null);
            else if (typeof(T) == typeof(AudioClip))
                action?.Invoke(DownloadHandlerAudioClip.GetContent(uwr) as T);
        }
        else
        {
            Debug.LogWarning("获取数据失败" + uwr.result + uwr.error + uwr.responseCode);
        }
    }

}
