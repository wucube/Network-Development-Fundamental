using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson20Exercise_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FtpMgr.Instance.UpLoadFile("MrTangPic.png", Application.streamingAssetsPath + "/test.png", () =>
        {
            print("上传结束 调用委托函数");
        });

        FtpMgr.Instance.UpLoadFile("MrTangPic2.png", Application.streamingAssetsPath + "/test.png", () =>
        {
            print("上传结束 调用委托函数");
        });

        FtpMgr.Instance.UpLoadFile("MrTangPic3.png", Application.streamingAssetsPath + "/test.png", () =>
        {
            print("上传结束 调用委托函数");
        });

        print("测试测试");
    }
}
