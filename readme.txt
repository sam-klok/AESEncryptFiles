Setting up two .Net 6.0 console applications to AES encrypt and decrypt files..

See examples of normal and encrypted files in folders \TestFiles and \AesEcryptedFiles

Some documentation is here:
AesManaged Class - System.Security.Cryptography
https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aesmanaged?view=net-7.0


Note: second version should probably contain MAC, and be written with newer version of the library, like below.
AES-Encrypt-then-MAC a large file with .NET
https://stackoverflow.com/questions/38623335/aes-encrypt-then-mac-a-large-file-with-net