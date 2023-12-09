using GamePlayerTest;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Lesson41 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 序列化存储为本地文件
        //主要使用
        //1.生成的类中的 WriteTo方法
        //2.文件流FileStream对象
        TestMsg msg  = new TestMsg();
        msg.ListInt.Add(1);
        msg.TestBool = false;
        msg.TestD = 5.5;
        msg.TestInt32 = 99;
        msg.TestMap.Add(1, "唐老师");
        msg.TestMsg2 = new TestMsg2();
        msg.TestMsg2.TestInt32 = 88;
        msg.TestMsg3 = new TestMsg.Types.TestMsg3();
        msg.TestMsg3.TestInt32 = 66;

        msg.TestHeart = new GameSystemTest.HeartMsg();
        msg.TestHeart.Time = 7777;

        print(Application.persistentDataPath);
        using (FileStream fs = File.Create(Application.persistentDataPath + "/TestMsg.tang"))
        {
            msg.WriteTo(fs);
     
        }
        #endregion

        #region 知识点二 反序列化本地文件
        //主要使用
        //1.生成的类中的 Parser.ParseFrom方法
        //2.文件流FileStream对象
        using (FileStream fs = File.OpenRead(Application.persistentDataPath + "/TestMsg.tang"))
        {
            TestMsg msg2 = null;
            msg2 = TestMsg.Parser.ParseFrom(fs);
            print(msg2.TestMap[1]);
            print(msg2.ListInt[0]);
            print(msg2.TestD);
            print(msg2.TestMsg2.TestInt32);
            print(msg2.TestMsg3.TestInt32);
            print(msg2.TestHeart.Time);
        }
        #endregion

        #region 知识点三 得到序列化后的字节数组
        //主要使用
        //1.生成的类中的 WriteTo方法
        //2.内存流MemoryStream对象
        byte[] bytes = null;
        using(MemoryStream ms = new MemoryStream())
        {
            msg.WriteTo(ms);
            bytes = ms.ToArray();
            print("字节数组长度" + bytes.Length);
        }
        #endregion

        #region 知识点四 从字节数组反序列化
        //主要使用
        //1.生成的类中的 Parser.ParseFrom方法
        //2.内存流MemoryStream对象
        using (MemoryStream ms=new MemoryStream(bytes))
        {
            print("内存流当中反序列化的内容");
            TestMsg msg2 = TestMsg.Parser.ParseFrom(ms);
            print(msg2.TestMap[1]);
            print(msg2.ListInt[0]);
            print(msg2.TestD);
            print(msg2.TestMsg2.TestInt32);
            print(msg2.TestMsg3.TestInt32);
            print(msg2.TestHeart.Time);
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
