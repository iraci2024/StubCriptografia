
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

class Stub
{
    private static byte[] key;

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
        Console.WriteLine($"Comando: {command}, Caminho do arquivo: {filePath}");

        if (command == "encrypt")
        {
            EncryptFile(filePath);
        }
        else if (command == "decrypt")
        {
            DecryptFile(filePath);
        }
        else if (command == "convert_to_pdf")
        {
            ConvertToPdf(filePath);
        }
        else
        {
            Console.WriteLine("Comando inválido. Use 'encrypt', 'decrypt' ou 'convert_to_pdf'.");
        }
    }

    private static void EncryptFile(string filePath)
    {
        key = GenerateRandomKey(32);
        byte[] fileBytes = File.ReadAllBytes(filePath);
        byte[] compressedBytes = Compress(fileBytes);
        byte[] encryptedBytes = Encrypt(compressedBytes, key);
        File.WriteAllBytes(filePath + ".enc", encryptedBytes);
        File.WriteAllBytes(filePath + ".key", key);
        Console.WriteLine("Arquivo criptografado com sucesso: " + filePath + ".enc");
    }

    private static void DecryptFile(string filePath)
    {
        string keyPath = filePath.Replace(".enc", ".key");
        key = File.ReadAllBytes(keyPath);
        byte[] encryptedBytes = File.ReadAllBytes(filePath);
        byte[] decryptedBytes = Decrypt(encryptedBytes, key);
        byte[] decompressedBytes = Decompress(decryptedBytes);
        File.WriteAllBytes(filePath.Replace(".enc", ""), decompressedBytes);
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

    private static byte[] Compress(byte[] data)
    {
        using (var compressedStream = new MemoryStream())
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
        {
            zipStream.Write(data, 0, data.Length);
            zipStream.Close();
            return compressedStream.ToArray();
        }
    }

    private static byte[] Decompress(byte[] data)
    {
        using (var compressedStream = new MemoryStream(data))
        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
        using (var resultStream = new MemoryStream())
        {
            zipStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }
    }

    private static void ConvertToPdf(string filePath)
    {
        string pdfPath = filePath.Replace(".txt", ".pdf");
        using (var writer = new iText.Kernel.Pdf.PdfWriter(pdfPath))
        {
            using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            {
                var document = new iText.Layout.Document(pdf);
                string content = File.ReadAllText(filePath);
                document.Add(new iText.Layout.Element.Paragraph(content));
            }
        }
        Console.WriteLine("Arquivo convertido para PDF com sucesso: " + pdfPath);
    }
}
