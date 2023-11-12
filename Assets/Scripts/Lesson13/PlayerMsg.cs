using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMsg : BaseMsg
{
    public int playerID;
    public PlayerData playerData;
    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        //先写消息ID
        WriteInt(bytes, GetID(), ref index);
        //写这个消息的成员变量
        WriteInt(bytes, playerID, ref index);
        WriteData(bytes, playerData, ref index);
        return bytes;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        //反序列化不需要去解析ID 因为在这一步之前 就应该把ID反序列化出来
        //用来判断到底使用哪一个自定义类来反序化
        int index = beginIndex;
        playerID = ReadInt(bytes, ref index);
        playerData = ReadData<PlayerData>(bytes, ref index);
        return index - beginIndex;
    }

    public override int GetBytesNum()
    {
        return 4 + //消息ID的长度
             4 + //playerID的字节数组长度
             playerData.GetBytesNum();//playerData的字节数组长度
    }

    /// <summary>
    /// 自定义的消息ID 主要用于区分是哪一个消息类
    /// </summary>
    /// <returns></returns>
    public override int GetID()
    {
        return 1001;
    }
}
