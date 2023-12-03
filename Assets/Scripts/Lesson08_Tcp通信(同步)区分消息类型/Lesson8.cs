using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson8 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 如何发送之前的自定义类的2进制信息
        //1.继承BaseData类
        //2.实现其中的序列化、反序列化、获取字节数等相关方法
        //3.发送自定义类数据时 序列化
        //4.接受自定义类数据时 反序列化

        //抛出问题：
        //当将序列化的2进制数据发送给对象时，对方如何区分？
        //举例：
        //PlayerInfo:玩家信息
        //ChatInfo:聊天信息
        //LoginInfo:登录信息
        //等等
        //这些数据对象序列化后是长度不同的字节数组
        //将它们发送给对象后，对方如何区分出他们分别是什么消息
        //如何选择对应的数据类反序列化它们？
        #endregion

        #region 知识点二 如何区分消息
        //解决方案：
        //为发送的信息添加标识，比如添加消息ID
        //在所有发送的消息的头部加上消息ID(int、short、byte、long都可以，根据实际情况选择)

        //举例说明：
        //消息构成
        //如果选用int类型作为消息ID的类型
        //前4个字节为消息ID
        //后面的字节为数据类的内容
        //####***************************
        //这样每次收到消息时，先把前4个字节取出来解析为消息ID
        //再根据ID进行消息反序列化即可
        #endregion

        #region 知识点三 实践
        //实践步骤
        //1.创建消息基类,基类继承BaseData,基类添加获取消息ID的方法或者属性
        //2.让想要被发送的消息继承该类，实现序列化反序列化方法
        //3.修改客户端和服务端收发消息的逻辑
        #endregion

        #region 总结
        //区分消息的关键点，是在数据字节数组头部加上消息ID
        //只要前后端定义好统一的规则
        //那么我们可以通过ID来决定如何反序列化消息
        //并且可以决定我们应该如何处理该消息
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
