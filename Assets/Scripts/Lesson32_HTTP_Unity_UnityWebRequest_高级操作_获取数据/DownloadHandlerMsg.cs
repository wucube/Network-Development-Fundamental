using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadHandlerMsg : DownloadHandlerScript
{
    //最终想要的消息对象
    private BaseMsg msg;
    //缓存收到的字节数组
    private byte[] cacheBytes;
    private int index = 0;

    public DownloadHandlerMsg(): base(){ }

    public T GetMsg<T>() where T : BaseMsg
    {
        return msg as T;
    }

    protected override byte[] GetData()
    {
        return cacheBytes;
    }

    protected override bool ReceiveData(byte[] data,int dataLength)
    {
        //将收到的数据拷贝到容器里，到最后一起处理
        data.CopyTo(cacheBytes, index);
        index += dataLength;
        return true;
    }

    protected override void ReceiveContentLengthHeader(ulong contentLength)
    {
        cacheBytes = new byte[contentLength];
    }

    protected override void CompleteContent()
    {
        //默认服务器下发的是继承BaseMsg的消息，在完成时解析它
        index = 0;
        int msgID = BitConverter.ToInt32(cacheBytes, index);
        index += 4;
        int msgLength = BitConverter.ToInt32(cacheBytes, index);
        index += 4;
        switch (msgID)
        {
            case 1001:
                msg = new PlayerMsg();
                msg.Reading(cacheBytes, index);
                break;
        }
        if (msg == null)
            Debug.Log("对应ID" + msgID + "没有处理");
        else
            Debug.Log("消息处理完毕");
    }
}
