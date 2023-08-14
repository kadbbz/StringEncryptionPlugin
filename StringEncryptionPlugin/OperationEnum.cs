using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEncryptionPlugin
{
    public enum OperationEnum
    {
        [Description("加密")]
        Encrypt,
        [Description("解密")]
        Decrypt
    }
}
