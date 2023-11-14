using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson31 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<IMultipartFormSection> dataList= new List<IMultipartFormSection>();

        //子类数据
        //MultipartFormDataSection
        //1.二进制字节数组
        dataList.Add(new MultipartFormDataSection(Encoding.UTF8.GetBytes("123123123123123")));
        //2.字符串
        dataList.Add(new MultipartFormDataSection("12312312312312312dsfasdf"));
        //3.参数名，参数值（字节数组，字符串），编码类型，资源类型（常用）
        dataList.Add(new MultipartFormDataSection("Name", "MrTang", Encoding.UTF8, "application/...."));
        dataList.Add(new MultipartFormDataSection("Msg", new byte[1024], "appl....."));

        //MultipartFormFileSection
        //1.字节数组
        dataList.Add(new MultipartFormFileSection(File.ReadAllBytes(Application.streamingAssetsPath + "/test.png")));

        //2.文件名，字节数组（常用）
        dataList.Add(new MultipartFormFileSection("上传的文件.png", File.ReadAllBytes(Application.streamingAssetsPath + "/test.png")));
        //3.字符串数据，文件名（常用）
        dataList.Add(new MultipartFormFileSection("12312313212312", "test.txt"));
        //4.字符串数据，编码格式，文件名（常用）
        dataList.Add(new MultipartFormFileSection("12312313212312", Encoding.UTF8, "test.txt"));

        //5.表单名，字节数组，文件名，文件类型
        dataList.Add(new MultipartFormFileSection("file", new byte[1024], "test.txt", ""));
        //6.表单名，字符串数据，编码格式，文件名
        dataList.Add(new MultipartFormFileSection("file", "123123123", Encoding.UTF8, "test.txt"));

        StartCoroutine(UpLoad());

    }

    IEnumerator UpLoad()
    {
        //准备上传的数据 
        List<IMultipartFormSection> data= new List<IMultipartFormSection>();
        //键值对相关的 信息字段数据
        data.Add(new MultipartFormDataSection("Name", "MrTang"));
        //添加一些文件上传
        //传2进制文件
        data.Add(new MultipartFormFileSection("TestTest123.png", File.ReadAllBytes(Application.streamingAssetsPath + "/test.png")));
        //传文本文件
        data.Add(new MultipartFormFileSection("12312312312321", "Test123.txt"));

        UnityWebRequest uwr = new UnityWebRequest("http://192.168.10.9:8080/HTTP_Server/");
        uwr.SendWebRequest();

        while (!uwr.isDone)
        {
            print(uwr.uploadProgress);
            print(uwr.uploadedBytes);

            yield return null;
        }

        print(uwr.uploadProgress);
        print(uwr.uploadedBytes);

        if(uwr.result == UnityWebRequest.Result.Success)
        {
            print("上传成功");
        }
        else
        {
            print("上传失败" + uwr.error + uwr.responseCode + uwr.result);
        }
    }

    IEnumerator UpLoadPut()
    {
        //Put发送数据
        UnityWebRequest uwr = UnityWebRequest.Put("http://192.168.10.9:8080/HTTP_Server/",File.ReadAllBytes(Application.streamingAssetsPath+"/test.png"));
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            print("Put 上传成功");
        }
        else
        {

        }
    }

}
