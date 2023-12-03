using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


/*
 * Socket客户端的网络连接不影响主线程，客户端可随时和服务端通信
*/
public class Lesson7 : MonoBehaviour
{
    public Button btn;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        //服务端为 Tcp同步通信服务端练习02

        btn.onClick.AddListener(() =>
        {
            PlayerMsg ms = new PlayerMsg();
            ms.playerID = 1111;
            ms.playerData = new PlayerData();
            ms.playerData.name = "唐老狮客户端发送的信息";
            ms.playerData.atk = 22;
            ms.playerData.lev = 10;
            NetMgr.Instance.Send(ms);
        });

        //黏包测试
        btn1.onClick.AddListener(() =>
        {
            PlayerMsg msg = new PlayerMsg();
            msg.playerID = 1001;
            msg.playerData = new PlayerData();
            msg.playerData.name = "唐老狮1";
            msg.playerData.atk = 1;
            msg.playerData.lev = 1;

            PlayerMsg msg2 = new PlayerMsg();
            msg2.playerID = 1002;
            msg2.playerData = new PlayerData();
            msg2.playerData.name = "唐老狮2";
            msg2.playerData.atk = 2;
            msg2.playerData.lev = 2;
            //黏包
            byte[] bytes = new byte[msg.GetBytesNum() + msg2.GetBytesNum()];
            msg.Writing().CopyTo(bytes, 0);
            msg2.Writing().CopyTo(bytes, msg.GetBytesNum());
            NetMgr.Instance.SendTest(bytes);
        });
        //分包测试
        btn2.onClick.AddListener(async () =>
        {
            PlayerMsg msg = new PlayerMsg();
            msg.playerID = 1003;
            msg.playerData = new PlayerData();
            msg.playerData.name = "唐老狮1";
            msg.playerData.atk = 3;
            msg.playerData.lev = 3;

            byte[] bytes = msg.Writing();
            //分包
            byte[] bytes1 = new byte[10];
            byte[] bytes2 = new byte[bytes.Length - 10];
            //分成第一个包
            Array.Copy(bytes, 0, bytes1, 0, 10);
            //第二个包
            Array.Copy(bytes, 10, bytes2, 0, bytes.Length - 10);

            NetMgr.Instance.SendTest(bytes1);
            await Task.Delay(500);
            NetMgr.Instance.SendTest(bytes2);
        });
        //分包、黏包测试
        btn3.onClick.AddListener(async () =>
        {
            PlayerMsg msg = new PlayerMsg();
            msg.playerID = 1001;
            msg.playerData = new PlayerData();
            msg.playerData.name = "唐老狮1";
            msg.playerData.atk = 1;
            msg.playerData.lev = 1;

            PlayerMsg msg2 = new PlayerMsg();
            msg2.playerID = 1002;
            msg2.playerData = new PlayerData();
            msg2.playerData.name = "唐老狮2";
            msg2.playerData.atk = 2;
            msg2.playerData.lev = 2;

            byte[] bytes1 = msg.Writing();//消息A
            byte[] bytes2 = msg2.Writing();//消息B

            byte[] bytes2_1 = new byte[10];
            byte[] bytes2_2 = new byte[bytes2.Length - 10];
            //分成第一个包
            Array.Copy(bytes2, 0, bytes2_1, 0, 10);
            //第二个包
            Array.Copy(bytes2, 10, bytes2_2, 0, bytes2.Length - 10);

            //消息A和消息B前一段的 黏包
            byte[] bytes = new byte[bytes1.Length + bytes2_1.Length];
            bytes1.CopyTo(bytes, 0);
            bytes2_1.CopyTo(bytes, bytes1.Length);

            NetMgr.Instance.SendTest(bytes);
            await Task.Delay(500);
            NetMgr.Instance.SendTest(bytes2_2);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
