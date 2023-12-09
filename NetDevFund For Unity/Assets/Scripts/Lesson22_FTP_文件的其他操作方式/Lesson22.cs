using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson22 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 其它操作指什么？
        //除了上传和下载，我们可能会对FTP服务器上的内容进行其它操作
        //比如：
        //1.删除文件
        //2.获取文件大小
        //3.创建文件夹
        //4.获取文件列表
        //等等
        #endregion

        #region 知识点二 进行其它操作
        //1.删除文件
        //FtpMgr.Instance.DeleteFile("测试测试.txt", (result) =>
        //{
        //    print(result ? "删除成功" : "删除失败");
        //});
        //2.获取文件大小
        FtpMgr.Instance.GetFileSize("实战就业路线.jpg", (size) =>
        {
            print("文件大小为：" + size);
        });
        //3.创建文件夹
        FtpMgr.Instance.CreateDirectory("唐老狮", (result) =>
        {
            print(result ? "创建成功" : "创建失败");
        });
        //4.获取文件列表
        FtpMgr.Instance.GetFileList("", (list) =>
        {
            if (list == null)
            {
                print("获取文件列表失败");
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                print(list[i]);
            }
        });
        #endregion

        #region 总结
        //FTP对于我们的作用
        //1.游戏当中的一些上传和下载功能
        //2.原生AB包上传下载
        //3.上传下载一些语音内容

        //只要是上传下载相关的功能 都可以使用Ftp来完成
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
