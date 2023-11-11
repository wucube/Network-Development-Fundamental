using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Lesson25ExerciseTest: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //print(Application.persistentDataPath);
        //HttpMgr.Instance.DownLoadFileAsync("技能的本质.png", Application.persistentDataPath + "/技能的本质http.jpg", (code) =>
        //{
        //    if (code == HttpStatusCode.OK)
        //        print("下载成功");
        //    else
        //        print("下载失败" + code);
        //});

        HttpMgr.Instance.UpLoadFileAsync("封装后上传.png", Application.streamingAssetsPath + "/test.png", (code) =>
        {
            if (code == HttpStatusCode.OK)
                print("上传指令成功");
            else
                print("上传指令失败" + code);
        });
    }
}
