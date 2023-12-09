using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息处理器基类，用于处理消息的逻辑
/// </summary>
public abstract class BaseHandler 
{
    /// <summary>
    /// 要处理的消息
    /// </summary>
    public BaseMsg message;

    /// <summary>
    /// 处理消息的方法
    /// </summary>
    public abstract void MsgHandle();
}
