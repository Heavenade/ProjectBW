﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using UnityEngine;
using System;
using System.Text;

public class DataEncryption
{
    /* 암호화를 하기 위한 클래스 */

    // 대칭키 암호화(AES 암호화)
    public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
    {
        byte[] encryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.Close();
                }
                encryptedBytes = ms.ToArray();
            }
        }
        return encryptedBytes;
    }

    // 대칭키 복호화(AES 복호화)
    public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
    {
        byte[] decryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    cs.Close();
                }
                decryptedBytes = ms.ToArray();
            }
        }

        return decryptedBytes;
    }

    //public void EncryptDataFile(string path, string password)
    //{
    //    byte[] baPwd = Encoding.UTF8.GetBytes(password);

    //    // Hash the password with SHA256
    //    byte[] baPwdHash = SHA256.Create().ComputeHash(baPwd);

    //    byte[] baToBeEncrypted = File.ReadAllBytes(path);

    //    byte[] baSalt = GetRandomBytes();
    //    byte[] baEncrypted = new byte[baSalt.Length + baToBeEncrypted.Length];

    //    // Combine Salt + Text
    //    for (int i = 0; i < baSalt.Length; i++)
    //        baEncrypted[i] = baSalt[i];
    //    for (int i = 0; i < baToBeEncrypted.Length; i++)
    //        baEncrypted[i + baSalt.Length] = baToBeEncrypted[i];

    //    baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);

    //    File.WriteAllBytes(path, baEncrypted);
    //}

    public string EncryptString(string text, string password)
    {
        byte[] baPwd = Encoding.UTF8.GetBytes(password);

        // Hash the password with SHA256
        byte[] baPwdHash = SHA256.Create().ComputeHash(baPwd);

        byte[] baText = Encoding.UTF8.GetBytes(text);

        byte[] baSalt = GetRandomBytes();
        byte[] baEncrypted = new byte[baSalt.Length + baText.Length];

        // Combine Salt + Text
        for (int i = 0; i < baSalt.Length; i++)
            baEncrypted[i] = baSalt[i];
        for (int i = 0; i < baText.Length; i++)
            baEncrypted[i + baSalt.Length] = baText[i];

        baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);

        string result = Convert.ToBase64String(baEncrypted);
        return result;
    }

    public string DecryptString(string text, string password)
    {
        byte[] baPwd = Encoding.UTF8.GetBytes(password);

        // Hash the password with SHA256
        byte[] baPwdHash = SHA256.Create().ComputeHash(baPwd);

        byte[] baText = Convert.FromBase64String(text);

        byte[] baDecrypted = AES_Decrypt(baText, baPwdHash);

        // Remove salt
        int saltLength = GetSaltLength();
        byte[] baResult = new byte[baDecrypted.Length - saltLength];
        for (int i = 0; i < baResult.Length; i++)
            baResult[i] = baDecrypted[i + saltLength];

        string result = Encoding.UTF8.GetString(baResult);
        return result;
    }

    public string[] DecryptString(string text, string text2, string password)
    {
        byte[] baPwd = Encoding.UTF8.GetBytes(password);

        // Hash the password with SHA256
        byte[] baPwdHash = SHA256.Create().ComputeHash(baPwd);

        byte[] baText = Convert.FromBase64String(text);
        byte[] baText2 = Convert.FromBase64String(text2);

        byte[] baDecrypted = AES_Decrypt(baText, baPwdHash);
        byte[] baDecrypted2 = AES_Decrypt(baText2, baPwdHash);

        // Remove salt
        int saltLength = GetSaltLength();
        byte[] baResult = new byte[baDecrypted.Length - saltLength];
        byte[] baResult2 = new byte[baDecrypted2.Length - saltLength];

        for (int i = 0; i < baResult.Length; i++)
            baResult[i] = baDecrypted[i + saltLength];

        for (int i = 0; i < baResult2.Length; i++)
            baResult2[i] = baDecrypted2[i + saltLength];

        string[] result = new string[2];
        result[0] = Encoding.UTF8.GetString(baResult);
        result[1] = Encoding.UTF8.GetString(baResult2);

        return result;
    }

    public static byte[] GetRandomBytes()
    {
        int saltLength = GetSaltLength();
        byte[] ba = new byte[saltLength];
        RNGCryptoServiceProvider.Create().GetBytes(ba);
        return ba;
    }

    public static int GetSaltLength()
    {
        return 8;
    }

}
