using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAsync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (NetAsyncMgr.Instance == null)
        {
            GameObject obj = new GameObject("NetAsync");
            obj.AddComponent<NetAsyncMgr>();
        }

        NetAsyncMgr.Instance.Connect("127.0.0.1", 8080);
    }
}
