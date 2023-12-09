using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    
    void Start()
    {
        if (NetMgr.Instance == null)
        {
            GameObject obj = new GameObject("Net");
            obj.AddComponent<NetMgr>();
        }

        NetMgr.Instance.Connect("127.0.0.1", 8080);
    }
}
