using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StringEncryptionPlugin
{
    [Icon("pack://application:,,,/StringEncryptionPlugin;component/Resources/Icon.png")]
    [Category("字符串加解密")]
    [OrderWeight(300)]
    public class MD5ServerCommand : BaseEncryptServerCommand, ICommandExecutableInServerSideAsync
    {

        public async Task<ExecuteResult> ExecuteAsync(IServerCommandExecuteContext dataContext)
        {
            var input = (await dataContext.EvaluateFormulaAsync(InputString)).ToString();

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = getEncoding().GetBytes(input);
            byte[] encryptdata = md5.ComputeHash(bytes);
            string output = Convert.ToBase64String(encryptdata);

            dataContext.Parameters[ResultTo] = output;

            return new ExecuteResult();
        }

        public override string ToString()
        {
            if (null == InputString)
            {
                return "MD5加密";
            }
            else
            {
                return "MD5加密：" + ResultTo;
            }

        }

        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }
    }

}
