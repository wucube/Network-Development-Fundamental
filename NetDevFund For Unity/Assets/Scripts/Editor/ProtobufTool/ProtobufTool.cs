using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEditor;
using UnityEngine;

public enum OutputType
{
    csharp_out,
    cpp_out,
    java_out
}

public class ProtobufTool
{
    //协议配置文件夹所在路径
    private static string Proto_Path = @"E:\CodeProject\Unity\Network Development Fundamental\Protobuf\Proto";
    //协议生成可执行文件的路径
    private static string Protoc_Path = @"E:\CodeProject\Unity\Network Development Fundamental\Protobuf\protoc.exe";
    //C#文件生成的路径
    private static string CSharp_Path = @"E:\CodeProject\Unity\Network Development Fundamental\Protobuf\GeneratedCSharp";

    [MenuItem("ProtobufTool/GenerateCSharp")]
    private static void GenerateCSharp()
    {
        GenerateCode(OutputType.csharp_out, CSharp_Path);
    }


    /// <summary>
    /// 生成对应语言的脚本类
    /// </summary>
    private static void GenerateCode(OutputType outputType, string outDir)
    {
        string resultType = outputType.ToString();

        //第一步：遍历对应协议配置文件夹 得到所有文件 
        DirectoryInfo directoryInfo = Directory.CreateDirectory(Proto_Path);
        //获取对应文件夹下所有文件信息
        FileInfo[] files = directoryInfo.GetFiles();
        //遍历所有的文件 为其生成协议脚本
        for (int i = 0; i < files.Length; i++)
        {
            //后缀判断 只有配置文件才能用于生成
            if (files[i].Extension == ".proto")
            {
                //第二步：根据文件内容 来生成对应的C#脚本
                Process process = new Process();

                //protoc.exe的路径
                process.StartInfo.FileName = $"\"{Protoc_Path}\"";//exe执行文件的路径不能有空格，空格无法被识别;若路径包含空格，要将整个路径名用双引号包裹一层
                process.StartInfo.Arguments = $"-I=\"{Proto_Path}\" --{resultType}=\"{outDir}\" \"{files[i]}\"";
                //执行
                process.Start();
                //告诉外部 某一个文件 生成结束
                UnityEngine.Debug.Log(files[i] + "生成结束");
            }
        }

        UnityEngine.Debug.Log("所有内容生成结束");
    }
}
