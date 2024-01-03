using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEncryptionPlugin
{
    public abstract class BaseEncryptServerCommand: Command
    {
        [FormulaProperty]
        [DisplayName("原始字符串")]
        [OrderWeight(100)]
        public object InputString { get; set; }

        [ResultToProperty]
        [DisplayName("将结果返回到变量")]
        [OrderWeight(999)]
        public string ResultTo { get; set; } = "结果";

        /// <summary>
        /// 获取字符串编码
        /// </summary>
        /// <returns></returns>
        internal Encoding getEncoding()
        {

            //if (Encoding == EncodingEnum.UTF8)
            //{
            //    return System.Text.Encoding.UTF8;
            //}
            //else if (Encoding == EncodingEnum.UNICODE)
            //{
            //    return System.Text.Encoding.Unicode;
            //}
            //else if (Encoding == EncodingEnum.ASCII)
            //{
            //    return System.Text.Encoding.ASCII;
            //}
            //else
            //{
            //    return System.Text.Encoding.Default;
            //}

            return Encoding.UTF8; // 暂时全都返回UTF8
        }

        internal string toHexString(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        internal string regularKeyString(string input, int length) {
            if (string.IsNullOrEmpty(input)) {
                input = "huozigedefaultkeyandivforstringencryption";
            }

            if (input.Length > length)
            {
                return input.Substring(0, length);
            }
            else { 
                return input.PadRight(length);
            }
        }

        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }
    }

    public enum EncodingEnum
    {
        ASCII,
        UNICODE,
        UTF8,
        Default
    }

}
