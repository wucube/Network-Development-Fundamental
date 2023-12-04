using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson13 : MonoBehaviour
{
    public Button btnSend;
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        btnSend.onClick.AddListener(() =>
        {
            if (input.text != "")
                NetAsyncMgr.Instance.Send(input.text);
        });
    }
}
