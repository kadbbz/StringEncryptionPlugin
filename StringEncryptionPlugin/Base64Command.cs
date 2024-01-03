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
    public class Base64Command : Command
    {
        [ComboProperty]
        [DisplayName("操作方向")]
        [OrderWeight(1)]
        public OperationEnum Operation { get; set; } = OperationEnum.Encrypt;

        [ComboProperty]
        [DisplayName("编码类型")]
        [OrderWeight(10)]
        public Base64EncodingEnum Encoding { get; set; } = Base64EncodingEnum.Latin;

        [FormulaProperty]
        [DisplayName("原始字符串")]
        [OrderWeight(100)]
        public object InputString { get; set; }

        //[ComboProperty]
        //[DisplayName("编码")]
        //[OrderWeight(10)]
        //public EncodingEnum Encoding { get; set; } = EncodingEnum.UTF8;

        [ResultToProperty]
        [DisplayName("将结果返回到变量")]
        [Description("本插件采用第三方类库实现，如需详情请在github搜索：js-base64")]
        [OrderWeight(999)]
        public string ResultTo { get; set; } = "结果";


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
            return CommandScope.ClientSide;
        }
    }


    public enum Base64EncodingEnum {
        [Description("Latin：默认编码，兼容性好")]
        Latin,
        [Description("UTF8：避免中文出现乱码")]
        UTF8
    }

}
