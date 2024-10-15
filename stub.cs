
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;

class Stub
{
    private static readonly byte[] key = Encoding.UTF8.GetBytes("12345678901234567890123456789012"); // Chave de criptografia de 32 bytes

    private static byte[] GenerateRandomKey(int size)
    {
        byte[] key = new byte[size];
        RandomNumberGenerator.Fill(key);
        return key;
    }

    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Uso: stub <encrypt/decrypt> <file>");
            return;
        }

        string command = args[0];
        string filePath = args[1];

        if (command == "encrypt")
        {
            EncryptFile(filePath);
        }
        else if (command == "decrypt")
        {
            DecryptFile(filePath);
        }
        else
        {
            Console.WriteLine("Comando inválido. Use 'encrypt' ou 'decrypt'.");
        }
    }

    private static void EncryptFile(string filePath)
    {
        byte[] fileBytes = File.ReadAllBytes(filePath);
        byte[] encryptedBytes = Encrypt(fileBytes, key);
        File.WriteAllBytes(filePath + ".enc", encryptedBytes);
        Console.WriteLine("Arquivo criptografado com sucesso: " + filePath + ".enc");
    }

    private static void DecryptFile(string filePath)
    {
        byte[] encryptedBytes = File.ReadAllBytes(filePath);
        byte[] decryptedBytes = Decrypt(encryptedBytes, key);
        File.WriteAllBytes(filePath.Replace(".enc", ""), decryptedBytes);
        Console.WriteLine("Arquivo descriptografado com sucesso: " + filePath.Replace(".enc", ""));
    }

    private static byte[] Encrypt(byte[] data, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = new byte[16]; // Vetor de inicialização (IV) zerado

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                }
                return ms.ToArray();
            }
        }
    }

    private static byte[] Decrypt(byte[] data, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = new byte[16]; // Vetor de inicialização (IV) zerado

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.Close();
                }
                return ms.ToArray();
            }
        }
    }
}
