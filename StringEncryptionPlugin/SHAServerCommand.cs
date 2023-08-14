using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StringEncryptionPlugin
{
    [Icon("pack://application:,,,/StringEncryptionPlugin;component/Resources/Icon.png")]
    [Category("字符串加解密")]
    [OrderWeight(301)]
    public class SHAServerCommand : BaseEncryptServerCommand, ICommandExecutableInServerSideAsync
    {
        [ComboProperty]
        [DisplayName("加密等级")]
        [OrderWeight(101)]
        public SHAEnum Mode { get; set; }

        public async Task<ExecuteResult> ExecuteAsync(IServerCommandExecuteContext dataContext)
        {
            var input = (await dataContext.EvaluateFormulaAsync(InputString)).ToString();
            string output = "";

            switch (Mode)
            {

                case SHAEnum.SHA1:
                    {
                        var bytes = getEncoding().GetBytes(input);
                        var SHA = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                        var encryptbytes = SHA.ComputeHash(bytes);
                        output = Convert.ToBase64String(encryptbytes);
                        break;
                    }
                case SHAEnum.SHA256:
                    {
                        var bytes = getEncoding().GetBytes(input);
                        var SHA = new System.Security.Cryptography.SHA256CryptoServiceProvider();
                        var encryptbytes = SHA.ComputeHash(bytes);
                        output = Convert.ToBase64String(encryptbytes);
                        break;
                    }
                case SHAEnum.SHA384:
                    {
                        var bytes = getEncoding().GetBytes(input);
                        var SHA = new System.Security.Cryptography.SHA384CryptoServiceProvider();
                        var encryptbytes = SHA.ComputeHash(bytes);
                        output = Convert.ToBase64String(encryptbytes);
                        break;
                    }
                case SHAEnum.SHA512:
                    {
                        var bytes = getEncoding().GetBytes(input);
                        var SHA = new System.Security.Cryptography.SHA512CryptoServiceProvider();
                        var encryptbytes = SHA.ComputeHash(bytes);
                        output = Convert.ToBase64String(encryptbytes);
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
                return "SHA加密";
            }
            else
            {
                return Mode +"加密：" + ResultTo;
            }

        }


        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }
    }

    public enum SHAEnum
    {
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

}
