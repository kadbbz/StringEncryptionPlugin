using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StringEncryptionPlugin
{
    [Icon("pack://application:,,,/StringEncryptionPlugin;component/Resources/Icon.png")]
    [Category("字符串加解密")]
    [OrderWeight(400)]
    public class HMACServerCommand : BaseEncryptServerCommand, ICommandExecutableInServerSideAsync
    {
        [ComboProperty]
        [DisplayName("消息摘要算法")]
        [OrderWeight(101)]
        public HMAC_SHAEnum HMAC_Mode { get; set; }

        [ComboProperty]
        [DisplayName("输出格式")]
        [OrderWeight(103)]
        public HMACOutputEnum OutputMode { get; set; } = HMACOutputEnum.Base64;


        [FormulaProperty]
        [DisplayName("密钥（SignKey，如时间戳）")]
        [Description("只能使用英文字母和数字和可见的ASCII符号")]
        [OrderWeight(105)]
        public object Key { get; set; }


        public async Task<ExecuteResult> ExecuteAsync(IServerCommandExecuteContext dataContext)
        {
            var input = (await dataContext.EvaluateFormulaAsync(InputString)).ToString();
            var signKey = (await dataContext.EvaluateFormulaAsync(Key)).ToString();
            string output = "";

            switch (HMAC_Mode)
            {

                case HMAC_SHAEnum.SHA1:
                    {
                        using (HMACSHA1 mac = new HMACSHA1(getEncoding().GetBytes(signKey)))
                        {
                            byte[] hash = mac.ComputeHash(getEncoding().GetBytes(input));


                            output = OutputMode == HMACOutputEnum.Base64 ? Convert.ToBase64String(hash) : toHexString(hash);
                        }

                        break;
                    }
                case HMAC_SHAEnum.SHA256:
                    {
                        using (HMACSHA256 mac = new HMACSHA256(getEncoding().GetBytes(signKey)))
                        {
                            byte[] hash = mac.ComputeHash(getEncoding().GetBytes(input));


                            output = OutputMode == HMACOutputEnum.Base64 ? Convert.ToBase64String(hash) : toHexString(hash);
                        }
                        break;
                    }
                case HMAC_SHAEnum.SHA384:
                    {
                        using (HMACSHA384 mac = new HMACSHA384(getEncoding().GetBytes(signKey)))
                        {
                            byte[] hash = mac.ComputeHash(getEncoding().GetBytes(input));


                            output = OutputMode == HMACOutputEnum.Base64 ? Convert.ToBase64String(hash) : toHexString(hash);
                        }
                        break;
                    }
                case HMAC_SHAEnum.SHA512:
                    {
                        using (HMACSHA512 mac = new HMACSHA512(getEncoding().GetBytes(signKey)))
                        {
                            byte[] hash = mac.ComputeHash(getEncoding().GetBytes(input));


                            output = OutputMode == HMACOutputEnum.Base64 ? Convert.ToBase64String(hash) : toHexString(hash);
                        }
                        break;
                    }
                default:
                    {
                        using (HMACMD5 mac = new HMACMD5(getEncoding().GetBytes(signKey)))
                        {
                            byte[] hash = mac.ComputeHash(getEncoding().GetBytes(input));


                            output = OutputMode == HMACOutputEnum.Base64 ? Convert.ToBase64String(hash) : toHexString(hash);
                        }

                        break;
                    }
            }

            dataContext.Parameters[ResultTo] = output;

            return new ExecuteResult();
        }

        public override string ToString()
        {
            if (null == InputString)
            {
                return "HMAC加密";
            }
            else
            {
                return "HMAC-" + HMAC_Mode + "加密：" + ResultTo;
            }

        }


        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }

    }


    public enum HMACOutputEnum
    {

        Base64,
        HexString
    }

    public enum HMAC_SHAEnum
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }
}
