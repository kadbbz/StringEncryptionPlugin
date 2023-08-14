using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace StringEncryptionPlugin
{
    [Icon("pack://application:,,,/StringEncryptionPlugin;component/Resources/Icon.png")]
    [Category("字符串加解密")]
    [OrderWeight(200)]
    public class Base64ServerCommand : BaseEncryptServerCommand, ICommandExecutableInServerSideAsync
    {
        [ComboProperty]
        [DisplayName("操作方向")]
        [OrderWeight(1)]
        public OperationEnum Operation { get; set; } = OperationEnum.Encrypt;

        public async Task<ExecuteResult> ExecuteAsync(IServerCommandExecuteContext dataContext)
        {
            var input = (await dataContext.EvaluateFormulaAsync(InputString)).ToString();

            string output = input;

            if (Operation == OperationEnum.Encrypt)
            {
                output = Convert.ToBase64String(getEncoding().GetBytes(input));
            }
            else
            {
                var raw = Convert.FromBase64String(input);

                output = getEncoding().GetString(raw);

            }

            dataContext.Parameters[ResultTo] = output;

            return new ExecuteResult();
        }

        public override string ToString()
        {
            if (null == InputString)
            {
                return "BASE64加解密";
            }
            else
            {
                return "BASE64" + ((Operation == OperationEnum.Encrypt) ? "加" : "解") + "密：" + ResultTo;
            }

        }

        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }
    }

}
