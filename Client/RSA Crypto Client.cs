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
        public string serverPubKey = "<RSAKeyValue><Modulus>no43x3D9BQ85lLSZ142RmTVp+kIoRDPE9PM9eIV8N5oUCHHm7xVnZzhrohgSy6L77mo8pgVwCIZtlYWBWuegc2PTw9tznqQ3XFmrugX2amMM1lHcLU/V1KSbD7Sol2NlaE/Br3AfmxesPhT5PNht5FqyOyoEs+7pwsqCYTJ+DHBawSp4Hi8o7ElZ3yYUkUm6jSACxm7mGAjv/uwqO5IFqtnX8T1HQMZi/NBU55TyajeZBVevZV6rPwXbYH9V1OPgOyYyvH4fVz3ZZOTVG81mqLfIsxcm4/5WpzcfJr+l5H5vnoqcDGbSeiRseoZJEzpXUoubgDQ3GzkgsQGRblYWQQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        public RSA_Crypto(){}
        // mã hoá text
        public byte[] Encrypt(string plainText, string publicKey)
        {
            var rsaClient = new RSACryptoServiceProvider(2048);
            rsaClient.FromXmlString(publicKey);
            var data = Encoding.UTF8.GetBytes(plainText);
            var cypherText = rsaClient.Encrypt(data, false);
            return cypherText;
        }
        // giải mã text
        public string Decrypt(byte[] cypherText)
        {
            var plainText = csp.Decrypt(cypherText, false);
            return Encoding.UTF8.GetString(plainText);
        }
        // trả về khoá public
        public string PublicKeyString()
        {
            var str = csp.ToXmlString(false);
            return str;
        }
        // mã hoá ảnh
        public byte[] EncryptImage(byte[] imagebyte, string publicKey)
        {
            var rsaClient = new RSACryptoServiceProvider(2048);
            rsaClient.FromXmlString(publicKey);
            var cypherText = rsaClient.Encrypt(imagebyte, false);
            return cypherText;
        }
        // giải mã ảnh
        public byte[] DecryptImage(byte[] cypherText)
        {
            var plainText = csp.Decrypt(cypherText, false);
            return plainText;
        }
        // xác thực chữ kí với tin nhắn
        public bool VerifyData(string publicKey,string plainText,byte[] signature)
        {
            bool result;
            var converter = new ASCIIEncoding();
            var rsaRead = new RSACryptoServiceProvider();
            rsaRead.FromXmlString(publicKey);
            byte[] plainText_b = converter.GetBytes(plainText);
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
        // tạo chữ kí
        public byte[] SignatureGen(string plainText)
        {
            var converter = new ASCIIEncoding();
            byte[] plainText_b = converter.GetBytes(plainText);
            byte[] signature_b = csp.SignData(plainText_b, new SHA1CryptoServiceProvider());
            return signature_b;
        }
    }
}
