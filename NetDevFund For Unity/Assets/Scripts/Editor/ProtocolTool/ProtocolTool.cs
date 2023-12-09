using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class ProtocolTool
{
    //配置文件所在路径
    private static string ProtoInfoPath = Application.dataPath + "/Scripts/Editor/ProtocolTool/ProtocolInfo.xml";

    private static GenerateCSharp generateCSharp = new GenerateCSharp();

    [MenuItem("ProtocolTool/GenerateCSharp")]
    private static void GenerateCSharp()
    {
        //1.读取xml相关的信息
        //XmlNodeList list = GetNodes("enum");
        //2.根据这些信息 去拼接字符串 生成对应的脚本

        //生成对应的C#枚举脚本
        generateCSharp.GenerateEnum(GetNodes("enum"));
        //生成对应的C#数据结构类成员
        generateCSharp.GenerateData(GetNodes("data"));
        //生成对应的消息类脚本
        generateCSharp.GenerateMsg(GetNodes("message"));

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取指定名称的所有子节点的List
    /// </summary>
    /// <param name="nodeName"></param>
    /// <returns></returns>
    private static XmlNodeList GetNodes(string nodeName)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(ProtoInfoPath);
        XmlNode root = xml.SelectSingleNode("messages");
        return root.SelectNodes(nodeName);
    }


}
