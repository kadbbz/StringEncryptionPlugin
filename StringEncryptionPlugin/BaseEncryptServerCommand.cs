using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace StringEncryptionPlugin
{
    public abstract class BaseEncryptServerCommand : Command
    {
        public BaseEncryptServerCommand() {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [FormulaProperty]
        [DisplayName("原始字符串")]
        [OrderWeight(100)]
        public object InputString { get; set; }


        [ResultToProperty]
        [DisplayName("将结果返回到变量")]
        [OrderWeight(999)]
        public string ResultTo { get; set; } = "结果";

        internal Encoding getEncoding()
        {
            return Encoding.UTF8;
        }

        /// <summary>
        /// 对输出内容进行编码后返回为字符串，如果与格式不匹配，会显示为乱码
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        internal string encodeOutput(Stream stm, EncodingFormatEnum Format)
        {
            return encodeOutput(structureBytes(stm), Format);
        }

        /// <summary>
        /// 对输出内容进行编码后返回为字符串，如果与格式不匹配，会显示为乱码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal string encodeOutput(byte[] data, EncodingFormatEnum Format)
        {
            string output = "";
            switch (Format)
            {
                case EncodingFormatEnum.ASCII:
                    {
                        output = Encoding.ASCII.GetString(data);
                        break;
                    }
                case EncodingFormatEnum.GB2312:
                    {
                        output = Encoding.GetEncoding("gb2312").GetString(data);
                        break;
                    }
                case EncodingFormatEnum.GB18030:
                    {
                        output = Encoding.GetEncoding("GB18030").GetString(data);
                        break;
                    }
                case EncodingFormatEnum.BIG5:
                    {
                        output = Encoding.GetEncoding("big5").GetString(data);
                        break;
                    }
                case EncodingFormatEnum.SHIFT_JIS:
                    {
                        output = Encoding.GetEncoding("shift_jis").GetString(data);
                        break;
                    }
                case EncodingFormatEnum.UTF8:
                    {
                        output = Encoding.UTF8.GetString(data);
                        break;
                    }
                case EncodingFormatEnum.Unicode:
                    {
                        output = Encoding.Unicode.GetString(data);
                        break;
                    }
                case EncodingFormatEnum.HEX:
                    {
                        output = toHexString(data);
                        break;
                    }
                case EncodingFormatEnum.BASE64:
                    {
                        output = Convert.ToBase64String(data);
                        break;
                    }
            }

            return output;
        }

        /// <summary>
        /// 将输入的字符串转换为byte数组，如果与格式不匹配，直接报错
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal byte[] decodeInput(string data, EncodingFormatEnum Format)
        {
            byte[] output = null;

            switch (Format)
            {
                case EncodingFormatEnum.ASCII:
                    {
                        output = Encoding.ASCII.GetBytes(data);
                        break;
                    }
                case EncodingFormatEnum.GB2312:
                    {
                        output = Encoding.GetEncoding("gb2312").GetBytes(data);
                        break;
                    }
                case EncodingFormatEnum.GB18030:
                    {
                        output = Encoding.GetEncoding("GB18030").GetBytes(data);
                        break;
                    }
                case EncodingFormatEnum.BIG5:
                    {
                        output = Encoding.GetEncoding("big5").GetBytes(data);
                        break;
                    }
                case EncodingFormatEnum.SHIFT_JIS:
                    {
                        output = Encoding.GetEncoding("shift_jis").GetBytes(data);
                        break;
                    }
                case EncodingFormatEnum.UTF8:
                    {
                        output = Encoding.UTF8.GetBytes(data);
                        break;
                    }
                case EncodingFormatEnum.Unicode:
                    {
                        output = Encoding.Unicode.GetBytes(data);
                        break;
                    }
                case EncodingFormatEnum.HEX:
                    {
                        output = toByteArray(data);
                        break;
                    }
                case EncodingFormatEnum.BASE64:
                    {
                        output = Convert.FromBase64String(data);
                        break;
                    }
            }

            return output;
        }

        private byte[] structureBytes(Stream stream)
        {
            // 超过int上限，直接报错
            int bufferSize = (int)stream.Length;
            long writtenBytes = 0L;
            var byteArrs = new List<byte[]>();
            while (true)
            {
                byte[] buffer = new BinaryReader(stream).ReadBytes(bufferSize);
                writtenBytes += buffer.Length;
                byteArrs.Add(buffer);
                // If the end of the report is reached, break out of the 
                // loop.
                if (buffer.Length != bufferSize)
                {
                    break;
                }
            }

            byte[] bytes = new byte[writtenBytes];
            int times = (int)(writtenBytes / 10240);
            for (var i = 0; i <= times; i++)
            {
                if (i < times)
                {
                    Buffer.BlockCopy(byteArrs[i], 0, bytes, i * 10240, 10240);
                    continue;
                }
                if ((int)(writtenBytes % 10240) == 0) break;
                Buffer.BlockCopy(byteArrs[i], 0, bytes, i * 10240, (int)(writtenBytes % 10240));
            }
            return bytes;
        }

        internal byte[] toByteArray(string hexString)
        {
            hexString = hexString.ToLower();
            hexString = hexString.Replace(" ", "");//移除空格
            hexString = hexString.Replace("-", "");//-
            hexString = hexString.Replace("0x", "");//0x

            //create a byte array the length of the
            //divided by 2 (Hex is 2 characters in length)
            byte[] comBuffer = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                //convert each set of 2 characters to a byte and add to the array
                comBuffer[i / 2] = (byte)Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return comBuffer;
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

        internal string regularKeyString(string input, int length)
        {
            if (string.IsNullOrEmpty(input))
            {
                input = "huozigedefaultkeyandivforstringencryption";
            }

            if (input.Length > length)
            {
                return input.Substring(0, length);
            }
            else
            {
                return input.PadRight(length);
            }
        }

        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }


    }

}
