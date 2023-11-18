using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace GamePlayer
{
    public enum ENUM_NAME
    {
        MAIN = 1,
        OTHER,
    }

    public class PlayerData : BaseData
    {
        public int id;
        public float atk;
        public long lev;
        public int[] arrays;
        public List<int> list;
        public Dictionary<int, string> dic;

        public override int GetBytesNum()
        {
            throw new System.NotImplementedException();
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            throw new System.NotImplementedException();
        }

        public override byte[] Writing()
        {
            throw new System.NotImplementedException();
        }
    }

    public class PlayerMsg : BaseMsg
    {
        public override int GetID()
        {
            return base.GetID();
        }
    }
}


public class Lesson35 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //1.读取xml文件信息
        XmlDocument xml = new XmlDocument();
        xml.Load(Application.dataPath + "/Scripts/Lesson35_协议(消息)配置/Lesson35.xml");

        //2.读取各节点元素
        //2-1: 跟节点读取
        XmlNode root = xml.SelectSingleNode("messages");

        //2-2: 读取出所有枚举结构类节点
        XmlNodeList enumList = root.SelectNodes("enum");
        foreach (XmlNode enumNode in enumList) 
        {
            print("**************");
            print("******枚举******");
            print("枚举名称：" + enumNode.Attributes["name"].Value);
            print("枚举所在的命名空间：" + enumNode.Attributes["namespace"].Value);
            print("******枚举成员******");
            XmlNodeList fields = enumNode.SelectNodes("field");
            foreach (XmlNode field in fields)
            {
                string str = field.Attributes["name"].Value;
                if(string.IsNullOrEmpty(field.InnerText))
                {
                    str += " = " + field.InnerText;
                }
                str += ",";
                print(str);
            }
        }

        //2-3: 读取出所有数据结构类节点
        XmlNodeList dataList = root.SelectNodes("data");
        foreach (XmlNode data in dataList)
        {
            print("**************");
            print("******数据结构类******");
            print("数据类名：" + data.Attributes["name"].Value);
            print("数据类所在命名空间：" + data.Attributes["namespace"].Value);
            print("******数据类成员******");

            XmlNodeList fields = data.SelectNodes("field");
            foreach (XmlNode field in fields)
            {
                print(field.Attributes["type"].Value + " " + field.Attributes["name"].Value + ";");

            }
        }
        //2-4: 读取出所有消息节点
        XmlNodeList msgList = root.SelectNodes("message");
        foreach (XmlNode msg in msgList)
        {
            print("**************");
            print("******消息类******");
            print("消息类名：" + msg.Attributes["name"].Value);
            print("消息类所在命名空间：" + msg.Attributes["namespace"].Value);
            print("消息ID：" + msg.Attributes["id"].Value);
            print("******数据类成员******");

            XmlNodeList fields = msg.SelectNodes("field");
            foreach (XmlNode field in fields)
            {
                print(field.Attributes["type"].Value + " " + field.Attributes["name"].Value + ";");
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
