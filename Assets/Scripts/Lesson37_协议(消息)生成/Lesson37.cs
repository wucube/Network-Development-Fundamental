using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson37 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GamePlayer.PlayerMsg msg = new GamePlayer.PlayerMsg();
        msg.playerID = 999;
        msg.data = new GamePlayer.PlayerData();
        msg.data.id = 888;
        msg.data.atk = 10;
        msg.data.sex = true;
        msg.data.lev = 77;
        msg.data.arrays = new int[] { 1, 2, 3, 4 };
        msg.data.list = new List<int>() { 4, 3, 2, 1 };
        msg.data.dict = new Dictionary<int, string>() {
            { 1, "123"},
            { 2, "唐老狮"},
            { 3, "好好学习"},
        };
        msg.data.heroType = GamePlayer.HeroType.Main;


        //序列化
        byte[] bytes = msg.Writing();
        int index = 0;
        int msgID = BitConverter.ToInt32(bytes, index);
        index += 4;
        int msgLength = BitConverter.ToInt32(bytes, index);
        index += 4;

        GamePlayer.PlayerMsg msgR = new GamePlayer.PlayerMsg();
        msgR.Reading(bytes, index);
        print(msgR.playerID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
