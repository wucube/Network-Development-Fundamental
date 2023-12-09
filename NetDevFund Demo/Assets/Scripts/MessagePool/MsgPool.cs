using GamePlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息池类 主要是用于 注册 ID和消息类型以及消息处理器类型的映射关系
/// 方便获取对象 进行反序列化和消息逻辑处理
/// </summary>
//public class MsgPool
//{
//    //记录消息类型和ID的映射关系
//    private Dictionary<int,Type> messageDict = new Dictionary<int,Type>();
//    //记录消息处理器类型和ID的映射关系
//    private Dictionary<int,Type> handlerDict = new Dictionary<int,Type>();

//    public MsgPool()
//    {
//        //在构造函数中 注册映射关系
//        Register(1001, typeof(PlayerMsg), typeof(PlayerMsgHandler));
//    }

//    private void Register(int id,Type messageType,Type handlerType)
//    {
//        messageDict.Add(id, messageType);
//        handlerDict.Add(id, handlerType);
//    }

//    /// <summary>
//    /// 根据ID 得到一个指定的消息类对象
//    /// </summary>
//    /// <param name="id"></param>
//    /// <returns></returns>
//    public BaseMsg GetMessage(int id)
//    {
//        if(messageDict.TryGetValue(id,out Type msg))
//            return Activator.CreateInstance(msg) as BaseMsg;
//        return null;
//    }

//    /// <summary>
//    /// 根据ID 得到一个指定的消息处理类对象
//    /// </summary>
//    /// <param name="id"></param>
//    /// <returns></returns>
//    public BaseHandler GetHandler(int id)
//    {
//        if(handlerDict.TryGetValue(id,out Type msg))
//            return Activator.CreateInstance(msg) as BaseHandler;
//        return null;
//    }
//}
