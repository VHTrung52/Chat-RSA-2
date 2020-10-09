using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Client
{
    public class RSA_Crypto
    {
        //Initializes a new instance of the RSACryptoServiceProvider class with a random key pair of the specified key size.
        public static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
        //RSAParameters _privateKey;
        //RSAParameters _publicKey;

        public RSA_Crypto()
        {
            // _privateKey = csp.ExportParameters(true);
            // _publicKey = csp.ExportParameters(false);

        }
        // Hàm in ra public key
        /*public string PublicKeyString()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);
            return sw.ToString();
        }*/

        public byte[] Encrypt(string plainText, string publicKey)
        {
            /*csp = new RSACryptoServiceProvider();
            csp.ImportParameters(_publicKey);

            var data = Encoding.Unicode.GetBytes(plaintext);
            var cypher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);*/
            var rsaClient = new RSACryptoServiceProvider(2048);
            rsaClient.FromXmlString(publicKey);
            //csp.FromXmlString(publicKey);
            var data = Encoding.UTF8.GetBytes(plainText);
            var cypherText = rsaClient.Encrypt(data, false);
            return cypherText;
        }
        public string Decrypt(byte[] cypherText)
        {
            /*var dataBytes = Convert.FromBase64String(cypherText);
            csp.ImportParameters(_privateKey);
            var plaintext = csp.Decrypt(dataBytes, false);
            return Encoding.Unicode.GetString(plaintext);*/
            //var data = Encoding.UTF8.GetBytes(cypherText);
            var plainText = csp.Decrypt(cypherText, false);
            return Encoding.UTF8.GetString(plainText);
        }
        public string PublicKeyString()
        {
            //var rsaServer = new RSACryptoServiceProvider(1024);
            //var publicKeyXml = rsaServer.ToXmlString(false);
            //Console.WriteLine(publicKeyXml.ToString());
            var str = csp.ToXmlString(false);
            return str;
        }
        public byte[] EncryptImage(byte[] imagebyte, string publicKey)
        {
            var rsaClient = new RSACryptoServiceProvider(2048);
            rsaClient.FromXmlString(publicKey);
            //csp.FromXmlString(publicKey);
            //var data = Encoding.UTF8.GetBytes(plaintext);
            var cypherText = rsaClient.Encrypt(imagebyte, false);
            return cypherText;
        }
        public byte[] DecryptImage(byte[] cypherText)
        {
            var plainText = csp.Decrypt(cypherText, false);
            return plainText;
        }
        public bool VerifyData(string publicKey,string plainText,byte[] signature)
        {
            bool result;
            var converter = new ASCIIEncoding();
            var rsaRead = new RSACryptoServiceProvider();
            rsaRead.FromXmlString(publicKey);
            byte[] plainText_b = converter.GetBytes(plainText);
            ///byte[] signature_b = converter.GetBytes(signature);
            if (rsaRead.VerifyData(plainText_b,new SHA1CryptoServiceProvider(),signature))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public byte[] SignatureGen(string plainText)
        {
            var converter = new ASCIIEncoding();
            byte[] plainText_b = converter.GetBytes(plainText);
            byte[] signature_b = csp.SignData(plainText_b, new SHA1CryptoServiceProvider());
            return signature_b;
        }
    }
}
