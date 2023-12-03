using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson11 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 什么是心跳消息？
        //所谓心跳消息，就是在长连接中，客户端和服务端之间定期发送的一种特殊的数据包
        //用于通知对方自己还在线，以确保长连接的有效性

        //由于其发送的时间间隔往往是固定的持续的，就像是心跳一样一直存在
        //所以我们称之为心跳消息
        #endregion

        #region 知识点二 为什么需要心跳消息？
        //1.避免非正常关闭客户端时，服务器无法正常收到关闭连接消息
        //通过心跳消息我们可以自定义超时判断，如果超时没有收到客户端消息，证明客户端已经断开连接

        //2.避免客户端长期不发送消息，防火墙或者路由器会断开连接，我们可以通过心跳消息一直保持活跃状态
        #endregion

        #region 知识点三 实现心跳消息
        //客户端
        //主要功能：定时发送消息

        //服务器
        //主要功能：不停检测上次收到某客户端消息的时间，如果超时则认为连接已经断开
        #endregion

        #region 总结
        //心跳消息是长连接项目中必备的一套逻辑规则
        //通过它可以帮助我们在服务器端及时的释放掉失效的socket
        //可以有效避免当客户端非正常关闭时，服务器端不能及时判断连接已断开
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
