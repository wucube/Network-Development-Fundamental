using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson33 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点二 自定义上传数据UploadHandler相关类
        //注意：
        //由于UnityWebRequest类的常用操作中
        //上传数据相关内容已经封装完善，能方便的上传参数和文件
        //使用常用操作已经能够满足常用需求了
        //所以以下内容主要做了解

        //UploadHandler相关类
        //1.UploadHandlerRaw  用于上传字节数组
        StartCoroutine(Upload());
        //2.UploadHandlerFile 用于上传文件

        //其中比较重要的变量是
        //contentType 内容类型，如果不设置，模式是 application/octet-stream 2进制流的形式
        #endregion
    }

    IEnumerator Upload()
    {
        UnityWebRequest uwr = new UnityWebRequest("http://192.168.10.9:8080/HTTP_Server/", UnityWebRequest.kHttpVerbPOST);

        //1.UploadHandlerRaw 用于上传字节数组
        //byte[] bytes = Encoding.UTF8.GetBytes("124121231223213");
        //uwr.uploadHandler = new UploadHandlerRaw(bytes);
        //uwr.uploadHandler.contentType = "类型/细分类型";

        //2.UploadHandlerFile 用于上传文件
        uwr.uploadHandler = new UploadHandlerFile(Application.streamingAssetsPath + "/test.png");

        yield return uwr.SendWebRequest();

        print(uwr.result);
    }
}
