using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


public static class AESUtil 
{
    private static string AES_KEY = "musicen@1234";


    public static string USER_ID = "user_id";
    public static string USER_PASS = "user_pass";

    public static string AutoLogin = "AutoLogin";
    public static string SaveID = "SaveID";

    public static string SetUserAgent = "SetUserAgent";
    public static string SetCameraAndMic = "SetCameraAndMic";


    public static string EncryptString(string str)
    {
        RijndaelManaged RijndaelCipher = new RijndaelManaged();
        byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(str);
        byte[] Salt = System.Text.Encoding.ASCII.GetBytes(AES_KEY.Length.ToString());
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
}
