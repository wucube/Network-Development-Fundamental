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
            if (input.text != "")
                NetMgr.Instance.Send(input.text);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
