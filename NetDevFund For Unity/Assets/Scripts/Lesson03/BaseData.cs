using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

interface IValueConverter<T> where T : struct
{
    byte[] FromValue(T value);
}

class ValueConverter : IValueConverter<int>,IValueConverter<short>,IValueConverter<float>,
    IValueConverter<long>,IValueConverter<bool>,IValueConverter<byte>
{
    byte[] IValueConverter<bool>.FromValue(bool value)
    {
        return BitConverter.GetBytes(value);
    }

    byte[] IValueConverter<int>.FromValue(int value)
    {
        return BitConverter.GetBytes(value);
    }

    byte[] IValueConverter<short>.FromValue(short value)
    {
        return BitConverter.GetBytes(value);
    }

    byte[] IValueConverter<float>.FromValue(float value)
    {
        return BitConverter.GetBytes(value);
    }

    byte[] IValueConverter<byte>.FromValue(byte value)
    {
        return BitConverter.GetBytes(value);
    }

    byte[] IValueConverter<long>.FromValue(long value)
    {
        return BitConverter.GetBytes(value);
    }
}

public class GenericsBitConverter<T> where T : struct
{
    T _value;

    IValueConverter<T> converter = new ValueConverter() as IValueConverter<T>;

    public void SetValue(T value)
    {
        this._value = value;
    }

    public byte[] GetBytes()
    {
        if (converter == null)
        {
            throw new InvalidOperationException("Unsuported type");
        }

        return converter.FromValue(this._value);
    }
}


public abstract class BaseData
{
    /// <summary>
    /// 用于子类重写的 获取字节数组容器大小的方法
    /// </summary>
    /// <returns></returns>
    public abstract int GetBytesNum();

    /// <summary>
    /// 把成员变量 序列化为 对应的字节数组
    /// </summary>
    /// <returns></returns>
    public abstract byte[] Writing();

    /// <summary>
    /// 把2进制字节数组 反序列化到 成员变量当中
    /// </summary>
    /// <param name="bytes">反序列化使用的字节数组</param>
    /// <param name="beginIndex">从该字节数组的第几个位置开始解析 默认是0</param>
    public abstract int Reading(byte[] bytes, int beginIndex = 0);


    //private void WriteValue<T>(byte[] bytes, T value, ref int index) where T : struct
    //{
    //    T valueType;
    //    GenericsBitConverter<T> genericsBitConverter = new GenericsBitConverter<T>();
    //    genericsBitConverter.GetBytes().CopyTo(bytes, index);
    //    index += sizeof(T);
    //}


    /// <summary>
    /// 存储int类型变量到指定的字节数组当中
    /// </summary>
    /// <param name="bytes">指定字节数组</param>
    /// <param name="value">具体的int值</param>
    /// <param name="index">每次存储后用于记录当前索引位置的变量</param>
    protected void WriteInt(byte[] bytes, int value, ref int index)
    {
        //TODO:使用反射避免大量重复代码
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(int);
    }
    protected void WriteShort(byte[] bytes, short value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(short);
    }
    protected void WriteLong(byte[] bytes, long value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(long);
    }
    protected void WriteFloat(byte[] bytes, float value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(float);
    }
    protected void WriteByte(byte[] bytes, byte value, ref int index)
    {
        bytes[index] = value;
        index += sizeof(byte);
    }
    protected void WriteBool(byte[] bytes, bool value, ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes, index);
        index += sizeof(bool);
    }

    //protected void WriteValue<T>(byte[] bytes,T value,ref int index) where T : struct
    //{
    //    byte[] valueBytes = BitConverter.GetBytes(value);
    //    valueBytes.CopyTo(bytes, index);
    //    index += Marshal.SizeOf(typeof(T));
    //}


    protected void WriteString(byte[] bytes, string value, ref int index)
    {
        //先存储string字节数组的长度
        byte[] strBytes = Encoding.UTF8.GetBytes(value);
        //BitConverter.GetBytes(strBytes.Length).CopyTo(bytes, index);
        //index += sizeof(int);
        WriteInt(bytes, strBytes.Length, ref index);
        //再存 string字节数组
        strBytes.CopyTo(bytes, index);
        index += strBytes.Length;
    }
    protected void WriteData(byte[] bytes, BaseData data, ref int index)
    {
        data.Writing().CopyTo(bytes, index);
        index += data.GetBytesNum();
    }

    /// <summary>
    /// 根据字节数组 读取整形
    /// </summary>
    /// <param name="bytes">字节数组</param>
    /// <param name="index">开始读取的索引数</param>
    /// <returns></returns>
    protected int ReadInt(byte[] bytes, ref int index)
    {
        int value = BitConverter.ToInt32(bytes, index);
        index += sizeof(int);
        return value;
    }
    protected short ReadShort(byte[] bytes, ref int index)
    {
        short value = BitConverter.ToInt16(bytes, index);
        index += sizeof(short);
        return value;
    }
    protected long ReadLong(byte[] bytes, ref int index)
    {
        long value = BitConverter.ToInt64(bytes, index);
        index += sizeof(long);
        return value;
    }
    protected float ReadFloat(byte[] bytes, ref int index)
    {
        float value = BitConverter.ToSingle(bytes, index);
        index += sizeof(float);
        return value;
    }
    protected byte ReadByte(byte[] bytes, ref int index)
    {
        byte value = bytes[index];
        index += sizeof(byte);
        return value;
    }
    protected bool ReadBool(byte[] bytes, ref int index)
    {
        bool value = BitConverter.ToBoolean(bytes, index);
        index += sizeof(bool);
        return value;
    }
    protected string ReadString(byte[] bytes, ref int index)
    {
        //首先读取长度
        int length = ReadInt(bytes, ref index);
        //再读取string
        string value = Encoding.UTF8.GetString(bytes, index, length);
        index += length;
        return value;
    }
    protected T ReadData<T>(byte[] bytes, ref int index) where T : BaseData, new()
    {
        T value = new T();
        index += value.Reading(bytes, index);
        return value;
    }
}
