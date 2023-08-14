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
    public class DESServerCommand : BaseEncryptServerCommand, ICommandExecutableInServerSideAsync
    {
        [ComboProperty]
        [DisplayName("操作方向")]
        [OrderWeight(1)]
        public OperationEnum Operation { get; set; } = OperationEnum.Encrypt;

        [FormulaProperty]
        [DisplayName("密钥（Key）")]
        [Description("长度为8个字符，只能使用英文字母和数字")]
        [OrderWeight(101)]
        public object Key { get; set; }

        [FormulaProperty]
        [DisplayName("初始向量（IV）")]
        [Description("长度为8个字符，只能使用英文字母和数字")]
        [OrderWeight(102)]
        public object IV { get; set; }


        public async Task<ExecuteResult> ExecuteAsync(IServerCommandExecuteContext dataContext)
        {
            var input = (await dataContext.EvaluateFormulaAsync(InputString)).ToString();
            var key = (await dataContext.EvaluateFormulaAsync(Key)).ToString();
            var iv = (await dataContext.EvaluateFormulaAsync(IV)).ToString();

            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(regularKeyString(key, 8));
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(regularKeyString(iv, 8));

            string output = input;

            if (Operation == OperationEnum.Encrypt)
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                int i = cryptoProvider.KeySize;
                MemoryStream ms = new MemoryStream();
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

                StreamWriter sw = new StreamWriter(cst);
                sw.Write(input);
                sw.Flush();
                cst.FlushFinalBlock();
                sw.Flush();

                output = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
            else
            {

                byte[] byEnc;
                try
                {
                    byEnc = Convert.FromBase64String(input);
                }
                catch
                {
                    return null;
                }

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream(byEnc);
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cst);
                output = sr.ReadToEnd();
            }

            dataContext.Parameters[ResultTo] = output;

            return new ExecuteResult();
        }

        public override string ToString()
        {
            if (null == InputString)
            {
                return "DES加解密";
            }
            else
            {
                return "DES" + ((Operation == OperationEnum.Encrypt) ? "加" : "解") + "密：" + ResultTo;
            }
        }

        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }
    }

}
