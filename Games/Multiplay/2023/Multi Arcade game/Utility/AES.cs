using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

/// <summary>
/// AES 암복호화
/// </summary>

public static class AES 
{
    static string AES_KEY = "dnddkqja@fkrxhvlt";

    public static string USER_ID = "userId";
    public static string USER_PASS = "userPass";

    public static string Save_ID = "SaveID";
    public static string Auto_Login = "AutoLogin";

    // 암호화
    public static string EncryptString(string str)
    {
        RijndaelManaged RijndaelCipher = new RijndaelManaged();
        byte[] PlainText = Encoding.Unicode.GetBytes(str);
        byte[] Salt = Encoding.ASCII.GetBytes(AES_KEY.Length.ToString());
        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(AES_KEY, Salt);

        ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

        cryptoStream.Write(PlainText, 0, PlainText.Length);
        cryptoStream.FlushFinalBlock();

        byte[] CipherBytes = memoryStream.ToArray();

        memoryStream.Close();
        cryptoStream.Close();

        string EncryptedData = Convert.ToBase64String(CipherBytes);
        return EncryptedData;
    }

    // 복호화
    public static string DecryptString(string str)
    {
        RijndaelManaged RijndaelCipher = new RijndaelManaged();

        byte[] EncryptedData = Convert.FromBase64String(str);
        byte[] Salt = Encoding.ASCII.GetBytes(AES_KEY.Length.ToString());

        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(AES_KEY, Salt);
        ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
        MemoryStream memoryStream = new MemoryStream(EncryptedData);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

        byte[] PlainText = new byte[EncryptedData.Length];
        int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

        memoryStream.Close();
        cryptoStream.Close();

        string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
        return DecryptedData;
    }


    public static void SaveData(string key, string getValue)
    {
        PlayerPrefs.SetString(key, EncryptString(getValue));
    }

    public static string LoadData(string key)
    {
        if (PlayerPrefs.GetString(key, "").Equals(""))
        {
            return "";
        }

        if (!PlayerPrefs.GetString(key, "").Equals(""))
        {
            return DecryptString(PlayerPrefs.GetString(key, ""));
        }
        else
        {
            return "";
        }
    }
}
