using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace StringEncryptionPlugin
{
    [Icon("pack://application:,,,/StringEncryptionPlugin;component/Resources/Icon.png")]
    [Category("字符串加解密")]
    [OrderWeight(100)]
    public class AESServerCommand : BaseEncryptServerCommand, ICommandExecutableInServerSideAsync
    {
        [ComboProperty]
        [DisplayName("操作方向")]
        [OrderWeight(1)]
        public OperationEnum Operation { get; set; } = OperationEnum.Encrypt;

        [FormulaProperty]
        [DisplayName("密钥（Key）")]
        [Description("长度为16个字符，只能使用英文字母和数字")]
        [OrderWeight(101)]
        public object Key { get; set; }


        public async Task<ExecuteResult> ExecuteAsync(IServerCommandExecuteContext dataContext)
        {
            var input = (await dataContext.EvaluateFormulaAsync(InputString)).ToString();
            var key = (await dataContext.EvaluateFormulaAsync(Key)).ToString();
            key = regularKeyString(key, 16);

            string output = input;

            if (Operation == OperationEnum.Encrypt)
            {

                Byte[] toEncryptArray = getEncoding().GetBytes(input);

                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = getEncoding().GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateEncryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                output = Convert.ToBase64String(resultArray);

            }
            else
            {
                Byte[] toEncryptArray = Convert.FromBase64String(input);

                RijndaelManaged rm = new RijndaelManaged
                {
                    Key = getEncoding().GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                ICryptoTransform cTransform = rm.CreateDecryptor();
                Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                output = getEncoding().GetString(resultArray);

            }

            dataContext.Parameters[ResultTo] = output;

            return new ExecuteResult();
        }

        public override string ToString()
        {
            if (null == InputString)
            {
                return "AES加解密";
            }
            else
            {
                return "AES" + ((Operation == OperationEnum.Encrypt) ? "加" : "解") + "密：" + ResultTo;
            }
        }

        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }
    }

}
