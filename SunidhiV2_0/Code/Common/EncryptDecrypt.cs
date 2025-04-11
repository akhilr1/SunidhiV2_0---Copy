using System;
using System.Security.Cryptography;
using System.Text;

namespace SunidhiV2_0.Code.Common
{
    public class EncryptDecrypt
    {
        public string Encryptionkey()
        {
            string strEncryptionKey = "49BEC4B828C3D2BA07BF6C44437AA95E80212615CFB7767C0FE65E0D0C460F12";
            return strEncryptionKey;
        }
        public string InitializationVector()
        {
            string strInitializationVector = "88C9E981118EDF735AD5615CBA7AF6AE";
            return strInitializationVector;
        }

        public string EncryptData(string textData, string Encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            //set the mode for operation of the algorithm   
            objrij.Mode = CipherMode.CBC;
            //set the padding mode used in the algorithm.   
            objrij.Padding = PaddingMode.PKCS7;
            //set the size, in bits, for the secret key.   
            objrij.KeySize = 0x80;
            //set the block size in bits for the cryptographic operation.    
            objrij.BlockSize = 0x80;
            //set the symmetric key that is used for encryption & decryption.    
            byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);
            //set the initialization vector (IV) for the symmetric algorithm    
            //byte[] EncryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            string strIV = InitializationVector();
            byte[] EncryptionkeyBytes = StringToByteArray(strIV);

            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionkeyBytes, len);

            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;

            //Creates symmetric AES object with the current key and initialization vector IV.    
            ICryptoTransform objtransform = objrij.CreateEncryptor();
            byte[] textDataByte = Encoding.UTF8.GetBytes(textData);
            //Final transform the test string.  
            return Convert.ToBase64String(objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length));
        }

        public string DecryptData(string EncryptedText, string Encryptionkey)
        {

                if (EncryptedText.Length >= 24)
                {
                    RijndaelManaged objrij = new RijndaelManaged();
                    objrij.Mode = CipherMode.CBC;
                    objrij.Padding = PaddingMode.PKCS7;

                    objrij.KeySize = 0x80;
                    objrij.BlockSize = 0x80;
                    byte[] encryptedTextByte = Convert.FromBase64String(EncryptedText);
                    byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);
                    byte[] EncryptionkeyBytes = new byte[0x10];
                    int len = passBytes.Length;
                    if (len > EncryptionkeyBytes.Length)
                    {
                        len = EncryptionkeyBytes.Length;
                    }
                    Array.Copy(passBytes, EncryptionkeyBytes, len);
                    objrij.Key = EncryptionkeyBytes;
                    objrij.IV = EncryptionkeyBytes;
                    byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
                    return Encoding.UTF8.GetString(TextByte);  //it will return readable string  
                }
                else
                {
                    return "Not Available";
                }

        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}