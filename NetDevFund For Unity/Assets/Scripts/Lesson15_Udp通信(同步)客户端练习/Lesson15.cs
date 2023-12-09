using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson15 : MonoBehaviour
{
    public Button btnSend;
    // Start is called before the first frame update
    void Start()
    {
        btnSend.onClick.AddListener(() =>
        {
            PlayerMsg msg = new PlayerMsg();
            msg.playerData = new PlayerData();
            msg.playerID = 1;
            msg.playerData.name = "唐老狮的客户端发的消息";
            msg.playerData.atk = 888;
            msg.playerData.lev = 666;
            UdpNetMgr.Instance.Send(msg);
        });
    }
}
