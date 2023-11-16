using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Lesseon32Exercise : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    IEnumerator GetMsg()
    {
        UnityWebRequest uwr = new UnityWebRequest("web服务器地址", UnityWebRequest.kHttpVerbPOST);
        DownloadHandlerMsg msgHandler= new DownloadHandlerMsg();
        uwr.downloadHandler = msgHandler;

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            PlayerMsg msg = msgHandler.GetMsg<PlayerMsg>();
            //使用消息对象处理相应的逻辑
        }
    }
}
