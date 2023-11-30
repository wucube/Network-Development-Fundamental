using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * Socket客户端的网络连接不影响主线程，客户端可随时和服务端通信
*/
public class Lesson7 : MonoBehaviour
{
    public Button btn;
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
