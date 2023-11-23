using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson39 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #region 知识点一 回顾自定义协议生成工具中的配置文件
        //我们在自定义协议配置工具相关知识点中
        //使用的是xml文件进行配置
        //我们只需要基于xml的规则 
        //按照一定规则配置协议信息
        //之后获取xml数据用于生成代码文件

        //在Protobuf中原理是一样的
        //只不过Protobuf中有自己的配置规则
        //也自定义了对应的配置文件后缀格式
        #endregion

        #region 知识点二 配置后缀
        //Protobuf中配置文件的后缀统一使用
        //.proto

        //可以通过多个后缀为.proto的配置文件进行配置
        #endregion

        #region 知识点三 配置规则

        #region 规则1 注释方式
        //方式1
        /*方式2*/
        #endregion

        #region 规则2 第一行版本号
        //syntax = "proto3";
        //如果不写 默认使用proto2
        #endregion

        #region 规则3 命名空间
        //package 命名空间名;
        #endregion

        #region 规则4 消息类
        //message 类名{
        //字段声明
        //}
        #endregion

        #region 规则5 成员类型和 唯一编号
        //浮点数:
        //float、double
        //整数:
        //变长编码-int32,int64,uint32,uint64,
        //固定字节数-fixed32,fixed64,sfixed32,sfixed64
        //其它类型:
        //bool,string,bytes

        //唯一编号 配置成员时 需要默认给他们一个编号 从1开始
        //这些编号用于标识中的字段消息二进制格式

        #endregion

        #region 规则6 特殊标识
        //1:required 必须赋值的字段
        //2:optional 可以不赋值的字段
        //3:repeated 数组
        #endregion

        #region 规则7 枚举
        //enum 枚举名{
        //    常量1 = 0;//第一个常量必须映射到0
        //    常量2 = 1;
        //}
        #endregion

        #region 规则8 默认值
        //string-空字符串
        //bytes-空字节
        //bool-false
        //数值-0
        //枚举-0
        //message-取决于语言 C#为空
        #endregion

        #region 规则9 允许嵌套

        #endregion

        #region 规则10 保留字段
        //如果修改了协议规则 删除了部分内容
        //为了避免更新时 重新使用 已经删除了的编号
        //我们可以利用 reserved 关键字来保留字段
        //这些内容就不能再被使用了
        //message Foo {
        //    reserved 2, 15, 9 to 11;
        //    reserved "foo", "bar";
        //}
        #endregion

        #region 规则11 导入定义
        //import "配置文件路径";
        //如果你在某一个配置中 使用了另一个配置的类型
        //则需要导入另一个配置文件名
        #endregion

        #endregion

        #region 总结
        //我们需要掌握Protobuf的配置规则
        //之后才能使用工具将其转为C#脚本文件
        #endregion
    }
}
