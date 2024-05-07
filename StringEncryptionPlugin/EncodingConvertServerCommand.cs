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
    [OrderWeight(900)]
    public class EncodingConvertServerCommand : BaseEncryptServerCommand, ICommandExecutableInServerSideAsync
    {
        [ComboProperty]
        [DisplayName("输入内容的编码格式")]
        [OrderWeight(100)]
        public EncodingFormatEnum InputFormat { get; set; } 

        [ComboProperty]
        [DisplayName("输出内容的编码格式")]
        [OrderWeight(110)]
        public EncodingFormatEnum OutputFormat { get; set; }

        public async Task<ExecuteResult> ExecuteAsync(IServerCommandExecuteContext dataContext)
        {
            var input = (await dataContext.EvaluateFormulaAsync(InputString)).ToString();

            string output = encodeOutput(decodeInput(input, InputFormat), OutputFormat);

            dataContext.Parameters[ResultTo] = output;

            return new ExecuteResult();
        }

        public override string ToString()
        {
            if (null == InputString)
            {
                return "字符编码转换";
            }
            else
            {
                return "字符编码转换（" + InputFormat + "->" + OutputFormat + "）：" + ResultTo;
            }

        }

        public override CommandScope GetCommandScope()
        {
            return CommandScope.ExecutableInServer;
        }
    }

}
