using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.WebUtilities;

namespace TicketingApi.Utils
{
    public static class AncDecUtil
    {
        public static string Encrypt(string text, string key, bool toURL = false)
        {
            var _key = Encoding.UTF8.GetBytes(key);
 
            using (var aes = Aes.Create())
            {
                using (var encryptor = aes.CreateEncryptor(_key, aes.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(text);
                            }
                        }
 
                        var iv = aes.IV;
 
                        var encrypted = ms.ToArray();
 
                        var result = new byte[iv.Length + encrypted.Length];
 
                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);
                        if(toURL){
                            return Base64UrlTextEncoder.Encode(result);
                        }
                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
 
       public static string DecryptString(string cipherText, string keyString, bool toURL = false)
       { 
            var fullCipher = new byte[0];
            if(toURL){
                fullCipher = Base64UrlTextEncoder.Decode(cipherText);
            }
            else{
                fullCipher = Convert.FromBase64String(cipherText); 
            }

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length]; //changes here

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            // Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length); // changes here
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                    // if(toURL){
                    //     return Encoding.Default.GetString(Base64UrlTextEncoder.Decode(result));
                    // }
                    return result;
                }
            }
         }
    }
}