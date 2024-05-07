using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEncryptionPlugin
{

    public enum EncodingFormatEnum
    {
        [Description("ASCII（非Unicode程序中的默认语言编码）")]
        ASCII,
        [Description("UTF8（活字格的默认编码）")]
        UTF8,
        [Description("Unicode")]
        Unicode,
        [Description("GBK/GB2312（简体中文）")]
        GB2312,
        [Description("GB18030（简体中文）")]
        GB18030,
        [Description("BIG5（繁体中文）")]
        BIG5,
        [Description("SHIFT_JIS（日文）")]
        SHIFT_JIS,
        [Description("HEX（16进制字符串）")]
        HEX,
        [Description("Base64")]
        BASE64
    }
}
