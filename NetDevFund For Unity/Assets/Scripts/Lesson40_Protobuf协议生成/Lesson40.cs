using GamePlayerTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson40 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 利用protoc.exe编译器生成脚本文件
        //1.打开cmd窗口
        //2.进入protoc.exe所在文件夹（也可以直接将exe文件拖入cmd窗口中）
        //3.输入转换指令
        //protoc.exe -I=配置路径 --csharp_out=输出路径 配置文件名

        //注意：路径不要有中文和特殊符号，避免生成失败
        #endregion

        #region 知识点二 测试生成对象是否能使用
        TestMsg msg = new TestMsg();
        msg.TestBool = true;
        //对应的和List以及Dictionary使用方式一样的 数组和字典对象
        msg.ListInt.Add(1);
        print(msg.ListInt[0]);
        msg.TestMap.Add(1, "唐老狮");
        print(msg.TestMap[1]);

        //枚举
        msg.TestEnum = TestEnum.Boss;
        //内部枚举
        msg.TestEnum2 = TestMsg.Types.TestEnum2.Boss;

        //其它类对象
        msg.TestMsg2 = new TestMsg2();
        msg.TestMsg2.TestInt32 = 99;
        //其它内部类对象
        msg.TestMsg3 = new TestMsg.Types.TestMsg3();
        msg.TestMsg3.TestInt32 = 55;
        //在另一个生成的脚本当中的类 如果命名空间不同 需要命名空间点出来使用
        msg.TestHeart = new GameSystemTest.HeartMsg();
        #endregion

        #region 总结
        //Protobuf 通过配置生成脚本文件
        //主要使用的就是 protoc.exe可执行文件
        //我们需要记住对应的生成指令
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
