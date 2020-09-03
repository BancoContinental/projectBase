using System;
using System.Security.Cryptography;
using System.Text;

namespace Continental.API.Core.Helpers
{
    public static class EncryptionHelper
    {
        private const string _key = "1000:dIFATM3Ou3Y2qT5C6QwDIn6VljIyNoLj:4Dh0+wnXqrjCiFyEJwM+kmqcR+CoITen";

        public static string Desencriptar(string text)
        {
            try
            {
                var toEncryptArray = Convert.FromBase64String(text);
                var hashmd5        = new MD5CryptoServiceProvider();
                var keyArray       = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(_key));
                hashmd5.Clear();

                var tdes = new TripleDESCryptoServiceProvider
                {
                    Key     = keyArray,
                    Mode    = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7,
                };

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                    toEncryptArray, 0, toEncryptArray.Length);

                tdes.Clear();

                return Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
