using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUdp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (UdpNetMgr.Instance == null)
        {
            GameObject obj = new GameObject("UdpNet");
            obj.AddComponent<UdpNetMgr>();
        }

        UdpNetMgr.Instance.StartClient("127.0.0.1", 8080);
    }
}
