using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;

namespace OPD.Models
{
    public static class CommonUtilities
    {
        public static AuthKey EncryptAesManaged(string raw)
        {
            AuthKey auth = new AuthKey();
            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {

                    byte[] encrypted = Encrypt(raw, aes.Key, aes.IV);
                    string result = Convert.ToBase64String(encrypted);
                    auth.encryptedKey = result;
                    auth.IV = Convert.ToBase64String(aes.IV);
                    auth.Key = Convert.ToBase64String(aes.Key);
                    string decrypted = Decrypt(encrypted, Convert.FromBase64String(auth.Key), Convert.FromBase64String(auth.IV));
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            Console.ReadKey();

            return auth;
        }
        public static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        public static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

        public static void FnWriteErrorLog(string FunctionName, string strWrite)
        {
            try
            {
                string ErrorFilePath = "D:\\ErrorFile\\";
                string errorFileName = "ErrorFile_" + DateTime.Now.ToString("dd_MM_yyyy");
                errorFileName = ErrorFilePath + errorFileName + ".txt";
                string NewStrString = FunctionName + "\n";
                NewStrString += "----------------------------" + DateTime.Now + "--------------------------------------------------\n";
                NewStrString += strWrite + "\n";

                if (!File.Exists(errorFileName))
                {

                    File.Create(errorFileName);
                    using (var w = new StreamWriter(errorFileName, true))
                    {
                        w.WriteLine(NewStrString);
                        w.Flush();
                    }
                }
                else
                {
                    using (var w = new StreamWriter(errorFileName, true))
                    {
                        w.WriteLine(NewStrString);
                        w.Flush();
                    }
                }
            }
            catch (Exception ex)
            {

                //FnWriteErrorLog("FnWriteErrorLog", ex.StackTrace);
                //string stroutput = "Fail---" + ex.Message.ToString();
            }

        }
        public static void fnStoreErrorLog(string Mode, string functionName, string Error, string empid)
        {
            try
            {
                DBHelper dBHelper = new DBHelper();
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@type", "AddErrorLog"));
                paramList.Add(new SqlParameter("@empid", empid));
                paramList.Add(new SqlParameter("@module", Mode + "_" + functionName));
                paramList.Add(new SqlParameter("@error_description", Error));

                dBHelper.ExecuteNonQuery("SP_SaveErrorLog", paramList.ToArray());
            }
            catch (Exception ex)
            {

            }
        }
        public static Tuple<string, string> validation(string Bankname, string Bankcode)
        {
            string banknameV = "";
            string bankcodeV = "";
           
            try
            {
                Regex bankr = new Regex("[a-zA-Z\\s]");
                Regex banks = new Regex("[a-zA-Z0-9\\s]");
                
                if (bankr.IsMatch(Bankname))
                {
                    banknameV = "";
                }
                else
                {
                    banknameV = "special character found";
                }
                if (banks.IsMatch(Bankcode))
                {
                    bankcodeV = "";
                }
                else
                {
                    bankcodeV = "special Character are  found ";

                }

            }
            catch (Exception ex)
            {


            }
            return Tuple.Create(banknameV, bankcodeV);
            
            
        }
    }






    public class AuthKey
    {
        public string Key { get; set; }
        public string IV { get; set; }
        public string encryptedKey { get; set; }
    }

}