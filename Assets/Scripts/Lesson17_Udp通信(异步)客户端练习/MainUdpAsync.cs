using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUdpAsync : MonoBehaviour
{
    void Start()
    {
        if (UdpNetAsyncMgr.Instance == null)
        {
            GameObject obj = new GameObject("UdpNet");
            obj.AddComponent<UdpNetAsyncMgr>();
        }

        UdpNetAsyncMgr.Instance.StartClient("127.0.0.1", 8080);

    }

}
