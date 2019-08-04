using System;
using System.Collections;

namespace BattleServer.Protocol
{
    //字符串协议模型
    //形式 名称,参数1,参数2,参数3
    public class ProtocolStr : ProtocolBase
    {
        //传输的字符串
        public string str;

        //解码器
        public override ProtocolBase Decode(byte[] readbuff, int start, int length)
        {
            ProtocolStr protocol = new ProtocolStr();
            protocol.str = System.Text.Encoding.UTF8.GetString(readbuff, start, length);
            return (ProtocolBase)protocol;
        }

        //编码器
        public override byte[] Encode()
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(str);
            return b;
        }

        //协议名称
        public override string GetName()
        {
            if (str.Length == 0) return "";
            return str.Split(',')[0];
        }

        //协议描述
        public override string GetDesc()
        {
            return str;
        }
    }
}