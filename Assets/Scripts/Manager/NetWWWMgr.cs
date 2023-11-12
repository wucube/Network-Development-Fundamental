using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetWWWMgr : MonoBehaviour
{
    private static NetWWWMgr instance;
    public static NetWWWMgr Instance => instance;

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
}
