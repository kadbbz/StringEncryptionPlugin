using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Security.Cryptography;
using System.Xml;

namespace StringEncryptionPlugin
{
    public class RSAHelper
    {
        /// <summary>
        /// RSA私钥，从Java格式转.net格式(不依赖第三方包)
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string RSAPrivateKeyJava2DotNet(string privateKey)
        {
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
              Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
              Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
              Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
              Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
              Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
              Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
              Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
              Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>
        /// RSA公钥，从Java格式转.net格式(不依赖第三方包)
        /// </summary>
        /// <param name="publikKey"></param>
        /// <returns></returns>
        public static string RSAPublicKeyJava2DotNet(string publicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
              Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
              Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        /// <summary>
        /// 重写FromXmlString方法
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider FromXmlString(string xmlString)
        {
            var rsa = new RSACryptoServiceProvider();
            RSAParameters parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
            return rsa;
        }
    }
}
