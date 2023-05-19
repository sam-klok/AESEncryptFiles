using System.IO;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Reflection.Metadata;

string path = Directory.GetCurrentDirectory() + @"\TestFiles";

var commandLineArgs = Environment.GetCommandLineArgs();
if (commandLineArgs.Length > 1) {
    path = commandLineArgs[1];
}

Console.WriteLine($"Reading files from {path}");
//var directoryInfo = new DirectoryInfo(path);

string[] files = Directory.GetFiles(path, "*.*");


if (files == null || files.Length == 0) 
{
    Console.WriteLine("No files to process.");
    Console.WriteLine("Please press any key..");
    Console.ReadKey();
}

var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
var config = builder.Build();
var aesKey = config["AesKey"];
var aesIV = config["AesIV"];
Console.WriteLine($"AES Key={aesKey} , IV={aesIV}");

var pathEncrypted = path + @"\Encrypted";
Directory.CreateDirectory(pathEncrypted);

foreach (string fileName in files)
{
    EncryptFile(fileName, pathEncrypted, aesKey, aesIV);
}

void EncryptFile(string inputFileName, string pathEncrypted, string aesKey, string aesIV)
{
    #pragma warning disable SYSLIB0021
    using (var aes = new AesManaged())
    #pragma warning restore SYSLIB0021
    {

        var fileInfo = new FileInfo(inputFileName);
        var outputFileName = pathEncrypted + @"\" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + fileInfo.Extension;

        using (FileStream fStream = new FileStream(outputFileName, FileMode.Create))
        {
            byte[] key = Encoding.UTF8.GetBytes(aesKey);
            byte[] IV = Encoding.UTF8.GetBytes(aesIV);

            using (ICryptoTransform aesEncrypt = aes.CreateEncryptor(key, IV))
            {
                using(var cryptoStream = new CryptoStream(fStream, aesEncrypt, CryptoStreamMode.Write))
                {
                    using (FileStream fileInputStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
                    {
                        int data;
                        while ((data = fileInputStream.ReadByte()) != -1)
                        {
                            cryptoStream.WriteByte((byte)data);
                        }

                    }
                }
            }

        }

        Console.WriteLine($"{Path.GetFileName(inputFileName)} \\Encrypted\\{Path.GetFileName(outputFileName)}");
    }
    
}

Console.WriteLine("Please press any key..");
Console.ReadKey();