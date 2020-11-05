using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class RSA_Crypto_Server
    {
        //Initializes a new instance of the RSACryptoServiceProvider class with a random key pair of the specified key size.
        public static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
        private string key = "<RSAKeyValue><Modulus>no43x3D9BQ85lLSZ142RmTVp+kIoRDPE9PM9eIV8N5oUCHHm7xVnZzhrohgSy6L77mo8pgVwCIZtlYWBWuegc2PTw9tznqQ3XFmrugX2amMM1lHcLU/V1KSbD7Sol2NlaE/Br3AfmxesPhT5PNht5FqyOyoEs+7pwsqCYTJ+DHBawSp4Hi8o7ElZ3yYUkUm6jSACxm7mGAjv/uwqO5IFqtnX8T1HQMZi/NBU55TyajeZBVevZV6rPwXbYH9V1OPgOyYyvH4fVz3ZZOTVG81mqLfIsxcm4/5WpzcfJr+l5H5vnoqcDGbSeiRseoZJEzpXUoubgDQ3GzkgsQGRblYWQQ==</Modulus><Exponent>AQAB</Exponent><P>wMK9GZmRadt+ZBTcFQvd3rw51axamQw+MnqvRxdhcVJHk7U22mFvLV+YEsYV/BiopUWL61jX5I3Y2+OoqIzzJtQa6FOTKfAtk3FW9iaF8PQfuwBq+QU8dwclP+sa14qQinBeewyHvpsuT8HUksjhOmumx7wgvOu+0bl5s9S9qYM=</P><Q>0pK2vMh+F4zf7f4K7aOGLswhO8lDwlAak7D8sSzU0Cl6AMQzB/sLfC8uO4lRRY44rwdDwI+IkI7IRtD3SdwSabM3yVQChJTiwLzJlPDwlElCRUrRPs+Dcpw5h76X9iOt9O0AeIPwSDs+s1QHYDw41m2nTcV4Y2nGp7z6QPccqes=</Q><DP>BkeRz36wPQmYgXwoe0sKrFHndG2L+gZrFEvo3+EEHKP4jvhNVCnubET7+EiZuL+45sFwQHeBeQGh+e/ds7f7x90LD1yBo9EeOvprQdOZc3MQgZAO7BPel2YVMZLgKftqoAfvHAUZev0lMhX3bdy7caLGKSWYz27CpaRsMY6Rqy8=</DP><DQ>FoHiEGmpuKCxKsIVrvLkm8nr33USzgk73f9z8wgksx9L0BsojTjTMHHmkiyKSFH8UxEGt+Vfpnvsgk1BsV5ZhXzHgLh/EGbNmeZT8ZyTSnZOJBPsK7eIiE6Ug6LmoUONC3ntq/QGUeIrQIXByBKHzLupv1FVHeMnfbRgV2NSaAM=</DQ><InverseQ>pJip1SNOTq5YfT1Iq3PjV8VAg0fdMlpIRun6mfZRu+lUZHSDlhzdRV6nXjUNw4KZ2Jq+QOlcZN307VUdUxjrloCxqHiJtSmw2GsOuPXRsbcV5PQHJU3JKUvmPsTVKXLSjU4yAfnjvbZdZmvY2LfrDzqaUiy4xnNytETPiKp650Y=</InverseQ><D>foarCGKn5oOxxXMhWRvF1mcDfQuSKmrNf1Mj+Ks85vnzhpGuYZfI/lvNuHpRdWNIe4cXgrzQHF150VnKUgqIxcwhzyeuaihNbUq5eScM7vS/579HXnlY/+mjO7NE3hoZcFZFD4UAE7meX6RJCD8F4rZliEUNn7DFpgYIaRvr7mvwg4ooIoF4IhNKE7k4wfWQTkAC+iSQsRnxJn0R4ny+X36TSGnPF0c+YAtsG8BKUHblPAR+FB6ggG2z2KPZKNks7SmY1mBut6pPpdaZ4kCafhbkZ8wT/SwpvmnD1yyK1S/Mv5ThTJTQuILCfexX48npsxJffT+xum6UwmsDVb5BSQ==</D></RSAKeyValue>";
        public RSA_Crypto_Server() 
        {
            csp.FromXmlString(key);
        }

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
        /*public string PublicKeyString()
        {
            var str = csp.ToXmlString(false);
            return str;
        }*/
        // mã hoá ảnh
        /*public byte[] EncryptImage(byte[] imagebyte, string publicKey)
        {
            var rsaClient = new RSACryptoServiceProvider(2048);
            rsaClient.FromXmlString(publicKey);
            var cypherText = rsaClient.Encrypt(imagebyte, false);
            return cypherText;
        }*/
        // giải mã ảnh
        /*public byte[] DecryptImage(byte[] cypherText)
        {
            var plainText = csp.Decrypt(cypherText, false);
            return plainText;
        }*/
        // xác thực chữ kí với tin nhắn
        public bool VerifyData(string publicKey, string plainText, byte[] signature)
        {
            bool result;
            var converter = new ASCIIEncoding();
            var rsaRead = new RSACryptoServiceProvider();
            rsaRead.FromXmlString(publicKey);
            byte[] plainText_b = converter.GetBytes(plainText);
            if (rsaRead.VerifyData(plainText_b, new SHA1CryptoServiceProvider(), signature))
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
