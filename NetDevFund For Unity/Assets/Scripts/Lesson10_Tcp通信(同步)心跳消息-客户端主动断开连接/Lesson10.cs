using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson10 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 目前的客户端主动断开连接
        //目前在客户端主动退出时
        //我们会调用socket的 ShutDown和Close方法
        //但是通过调用这两个方法后 服务器端无法得知客户端已经主动断开

        //举例说明
        #endregion

        #region 知识点二 解决目前断开不及时的问题
        //1.客户端尝试使用Disconnect方法主动断开连接
        //Socket当中有一个专门在客户端使用的方法
        //Disconect方法
        //客户端调用该方法和服务器端断开连接
        //看是否是因为之前直接Close而没有调用Disconet造成服务器端无法及时获取状态

        //主要修改的逻辑：
        //客户端：
        //主动断开连接

        //服务端：
        //1.收发消息时判断socket是否已经断开
        //2.处理删除记录的socket的相关逻辑（会用到线程锁）


        //2.自定义退出消息
        //让服务器端收到该消息就知道是客户端想要主动断开
        //然后服务器端处理释放socket相关工作
        #endregion

        #region 总结
        //客户端可以通过Disconnect方法主动和服务器端断开连接
        //服务器端可以通过Conected属性判断连接状态决定是否释放Socket

        //但是由于服务器端Conected变量表示的是上一次收发消息是否成功
        //所以服务器端无法准确判断客户端的连接状态
        //因此 我们需要自定义一条退出消息 用于准确断开和客户端之间的连接
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
