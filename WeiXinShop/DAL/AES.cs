using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

/// <summary>
///AES 的摘要说明
/// </summary>
public class AES
{
    /// <summary>
    /// MD5　32位加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Md5(string str)
    {
        //string b = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        //return b;
        byte[] result = Encoding.Default.GetBytes(str);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] output = md5.ComputeHash(result);
        string encryptResult = BitConverter.ToString(output).Replace("-", "");
        return encryptResult;
    }
    /// <summary>
    /// AES加密
    /// </summary>
    /// <param name="toEncrypt"></param>
    /// <returns></returns>
    public static string Encrypt(string toEncrypt, string key)
    {
        string str = Md5(key);
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(str);
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="toDecrypt"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string Decrypt(string toDecrypt, string key)
    {
        string str = Md5(key);
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(str);
        byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return UTF8Encoding.UTF8.GetString(resultArray);
    }
}