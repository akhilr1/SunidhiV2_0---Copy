using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
//using OpenSSL.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;
using System.Web;
using System.IO;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;

namespace SunidhiV2_0.Code.Utils
{
    public class CryptoUtils
    {
        public string SHA256HashPassword(string password)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(password);

            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }

            var hashedPassword = hex;

            return hashedPassword;
        }

        public string Decrypt(string encrypted)
        {
            byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var privateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQDPRQhk0OBFv7Qdngc3xd/5Ofn2PeM7z5cjTrbddnBPAXE2JIhU
7ku5gvUDrIxIQlNM3u8eTZArkekjN+QnX1A6Y9WA+5/ccQFZwK5JwMjWS+k5GZeb
tTbA0GJ/VO6J1zLLEQdxci2CUU4R9LNEIZmrVUIFE5b1AGAQtlrFT0lVmwIDAQAB
AoGAT0mLEnXMtSHGVuYSwMUTDpWdBxNZj5GzsS/1R/Z8s9xLYHjNJiAaG5b6/zLM
Xv/wOaG2OxNUqTYhxvUkc+NEHsFNbGTbdknt29BGv7E8Zimv2LDFrdGIn/VfKNbe
Dxnc4xtPhdxzPaJB+CNixOWE6ehJQafiPJCLovlzepjoyUECQQD3f7Y+jPRjnC+/
LV05DSTjD6V2u7BFGiqukVMb04zEOAEXp89uoWxi8erwKH/yUAiUMJAQ6e2uNlDo
IZtscdBxAkEA1mOUmEX+HXyh7WGi8mvI39+9lAx6pphf0+73rMVlmVIwT5JkW47s
b+KxokyOOYV6/8ZXsStmgPMfhCUmFV/MywJAUy82hqGDhnM5TQ0nNwSm8DYTC+wb
W65myBOKFmEs/8o++79oJavVrQ/YQe2qyzuBFqWX1kd5nkMdQFgDUBiU0QJBAKdu
6AswhrCiuJf669amRSBKzfVE9bkMQbLz66ycEaf3X1GVxBkZeyA6jH5ziZP04kXF
spDkk05cEh4TITYKNpcCQBhRs6QkawL93Ywg9BltFZxdtvJusdW+BQTWALpZiTak
S8kc0uY/6L/MXfzhRFBkj1B9/5K4MO/+Ce4X84Lw0IY=
-----END RSA PRIVATE KEY-----";

            //CipherContext aes = new CipherContext(Cipher.AES_128_CBC);
            //byte[] decrypted = aes.Crypt(System.Text.ASCIIEncoding.ASCII.GetBytes(encrypted), System.Text.ASCIIEncoding.ASCII.GetBytes(privateKey), iv, false);
            //string val = System.Text.ASCIIEncoding.Default.GetString(decrypted);
            //return val;
            var bytesToDecrypt = Convert.FromBase64String(encrypted);


            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            PemReader pr = new PemReader(new StringReader(privateKey));
            AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
            RSAParameters rsa = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);
            decryptEngine.Init(false, KeyPair.Private);
            var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
            return decrypted;
        }

        public static string EncryptPassword(string password)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(password);

            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }

            return hex;
        }        

        // Generate a random string with a given size and case.   
        // If second parameter is true, the return string is lowercase 
        public static string RandomPasswordGenerator(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}