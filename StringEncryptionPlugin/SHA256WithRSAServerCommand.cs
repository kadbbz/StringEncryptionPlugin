using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;

namespace StringEncryptionPlugin
{
    [Icon("pack://application:,,,/StringEncryptionPlugin;component/Resources/Icon.png")]
    [Category("字符串加解密")]
    [OrderWeight(100)]
    public abstract class SHA256WithRSAServerCommand : BaseEncryptServerCommand, ICommandExecutableInServerSideAsync
    {
        [ComboProperty]
        [DisplayName("哈希算法")]
        [OrderWeight(100)]
        public RSA_SHAEnum RSA_Mode { get; set; } = RSA_SHAEnum.SHA256;


        [FormulaProperty]
        [DisplayName("密钥（PrivateKey）")]
        [Description("Java格式的RSA Base64字符串，包含Modulus、Exponent等信息")]
        [OrderWeight(101)]
        public object Key { get; set; }


        public async Task<ExecuteResult> ExecuteAsync(IServerCommandExecuteContext dataContext)
        {
            var input = (await dataContext.EvaluateFormulaAsync(InputString)).ToString();
            var key = (await dataContext.EvaluateFormulaAsync(Key)).ToString();

            string output = input;

            //实现方案参考： https://www.jianshu.com/p/9ba4662a9941

            var netKey = RSAHelper.RSAPrivateKeyJava2DotNet(key); //转换成适用于.net的私钥

            var rsa = RSAHelper.FromXmlString(netKey); //.net core2.2及其以下版本使用，重写FromXmlString(string)方法

            var rsaClear = new RSACryptoServiceProvider();
            var paras = rsa.ExportParameters(true);
            rsaClear.ImportParameters(paras); //签名返回
            using (var halg = getCurrentHalg())
            {
                var signData = rsa.SignData(Encoding.UTF8.GetBytes(input), halg);
                output = Convert.ToBase64String(signData);
            }

            dataContext.Parameters[ResultTo] = output;

            return new ExecuteResult();
        }

        private IDisposable getCurrentHalg() {

            switch (RSA_Mode) { 
            
                case RSA_SHAEnum.SHA256: {

                        return new SHA256CryptoServiceProvider();
                    }
                case RSA_SHAEnum.SHA1:
                    {

                        return new SHA1CryptoServiceProvider();
                    }
                case RSA_SHAEnum.SHA384:
                    {

                        return new SHA384CryptoServiceProvider();
                    }
                case RSA_SHAEnum.SHA512:
                    {

                        return new SHA512CryptoServiceProvider();
                    }
                default:
                    {
                        return new MD5CryptoServiceProvider();
                    }
            }
        }

        public override string ToString()
        {
            if (null == InputString)
            {
                return RSA_Mode + "WithRSA签名（Beta）";
            }
            else
            {
                return RSA_Mode + "WithRSA签名（Beta）：" + ResultTo;
            }
        }

        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }

    }

    public enum RSA_SHAEnum
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

}
