using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public static class NetProtobufTool
{
    /// <summary>
    /// 序列化Protobuf生成的对象
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] GetProtoBytes(IMessage msg)
    {
        //基础写法
        //byte[] bytes = null;
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    msg.WriteTo(ms);
        //    bytes = ms.ToArray();
        //}
        //return bytes;

        //通过该拓展方法 就可以直接获取对应对象的 字节数组
        return msg.ToByteArray();
    }

    /// <summary>
    /// 反序列化字节数组为Protobuf相关的对象
    /// </summary>
    /// <typeparam name="T">想要获取的消息类型</typeparam>
    /// <param name="bytes">对应的字节数组 用于反序列化</param>
    /// <returns></returns>
    public static T GetProtoMsg<T>(byte[] bytes) where T : class,IMessage
    {
        //得到对应消息的类型 通过反射得到内部的静态成员 然后得到其中的 对应方法
        Type type = typeof(T);
        //通过反射，得到对应的静态成员属性对象
        PropertyInfo propertyInfo = type.GetProperty("Parser");
        object parserObj = propertyInfo.GetValue(null, null);

        //已经得到了对象 就可以得到该对象中的 对应方法 
        Type parserType = parserObj.GetType();
        //指定得到某一个重载函数
        MethodInfo methodInfo = parserType.GetMethod("ParseFrom", new Type[] { typeof(byte[]) });
        //调用对应的方法 反序列化为指定的对象
        object msg = methodInfo.Invoke(parserObj, new object[] { bytes });
        return msg as T;
    } 
}
