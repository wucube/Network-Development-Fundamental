using GamePlayerTest;
using UnityEngine;

public class Lesson41Exercise : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestMsg msg = new TestMsg();
        msg.ListInt.Add(1);
        msg.TestBool = false;
        msg.TestD = 5.5;
        msg.TestInt32 = 99;
        msg.TestMap.Add(1, "唐老狮");
        msg.TestMsg2 = new TestMsg2();
        msg.TestMsg2.TestInt32 = 88;
        msg.TestMsg3 = new TestMsg.Types.TestMsg3();
        msg.TestMsg3.TestInt32 = 66;

        msg.TestHeart = new GameSystemTest.HeartMsg();
        msg.TestHeart.Time = 7777;

        byte[] bytes = NetProtobufTool.GetProtoBytes(msg);

        TestMsg msg2 = NetProtobufTool.GetProtoMsg<TestMsg>(bytes);
        print(msg2.TestMap[1]);
        print(msg2.ListInt[0]);
        print(msg2.TestD);
        print(msg2.TestMsg2.TestInt32);
        print(msg2.TestMsg3.TestInt32);
        print(msg2.TestHeart.Time);
    }

}
