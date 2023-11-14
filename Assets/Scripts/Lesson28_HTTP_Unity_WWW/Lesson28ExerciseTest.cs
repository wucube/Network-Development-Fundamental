using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lesson28ExerciseTest : MonoBehaviour
{
    public RawImage rawImage;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        //只要保证一运行时 进行该判断 进行动态创建
        if (NetWWWMgr.Instance == null)
        {
            GameObject obj = new GameObject("WWW");
            obj.AddComponent<NetWWWMgr>();
        }

        ////在任何地方使用NetWWWMgr都没有问题
        //NetWWWMgr.Instance.LoadRes<Texture>("http://192.168.10.4:8080/HTTP_Server/技能的本质.png", (obj) =>
        //{
        //    //直接使用加载结束后的资源
        //    rawImage.texture = obj;
        //});

        //NetWWWMgr.Instance.LoadRes<Sprite>("http://192.168.10.4:8080/HTTP_Server/动作技能编辑器的需求.png", (obj) =>
        //{
        //    //直接使用加载结束后的资源
        //    image.sprite = obj;
        //});

        //NetWWWMgr.Instance.LoadRes<byte[]>("http://192.168.10.4:8080/HTTP_Server/封装后上传.png", (obj) =>
        //{
        //    //将加载结束后得到的资源存储到本地，之后再使用
        //    print(Application.persistentDataPath);
        //    File.WriteAllBytes(Application.persistentDataPath + "/www图片.png", obj);
        //});

        //NetWWWMgr.Instance.LoadRes<string>("http://192.168.10.4:8080/HTTP_Server/test.txt", (str) =>
        //{
        //    print(str);
        //});

        NetWWWMgr.Instance.UploadFile("UnityWebRequest异步上传文件.png", Application.streamingAssetsPath + "/test.png", (result) =>
        {
            if (result == UnityWebRequest.Result.Success)
            {
                print("上传成功");
            }
            else
                print("上传失败" + result);
        });
    }
}
