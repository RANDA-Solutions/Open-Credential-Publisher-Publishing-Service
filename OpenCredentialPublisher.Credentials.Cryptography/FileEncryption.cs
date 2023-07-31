using System;
using System.IO;
using System.Security.Cryptography;

namespace OpenCredentialPublisher.Credentials.Cryptography
{
    public class FileEncryption
    {
        private const int _keyIterations = 50000;
        private const int _blockSize = 128;
        private const int _keySize = 256;
        private const byte _bitsInByte = 8;
        private const int _bufferSize = 1048576;
        private const byte _saltLength = 32;
        private const PaddingMode _paddingMode = PaddingMode.PKCS7;

        /// <summary>
        /// Creates a random salt that will be used to encrypt the file. 
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[_saltLength];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }

            return data;
        }


        /// <summary>
        /// Encrypts a file from a stream and protects it with a password.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="password"></param>
        public Byte[] FileEncrypt(Stream inputStream, string password)
        {

            // generate a random salt
            byte[] salt = GenerateRandomSalt();

            // create an in-memory stream
            using (MemoryStream fsCrypt = new MemoryStream())
            {
                // convert password string to byte arrray
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

                var key = new Rfc2898DeriveBytes(passwordBytes, salt, _keyIterations);

                using (var aesCryptoServiceProvider = AesCryptoServiceProvider.Create())
                {
                    aesCryptoServiceProvider.BlockSize = _blockSize;
                    aesCryptoServiceProvider.KeySize = _keySize;
                    aesCryptoServiceProvider.Padding = _paddingMode;
                    aesCryptoServiceProvider.Key = key.GetBytes(aesCryptoServiceProvider.KeySize / _bitsInByte);
                    aesCryptoServiceProvider.IV = key.GetBytes(aesCryptoServiceProvider.BlockSize / _bitsInByte);

                    // write salt to the begining of the output file, so in this case can be random every time
                    fsCrypt.Write(salt, 0, _saltLength);

                    using (CryptoStream cs = new CryptoStream(fsCrypt, aesCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // create a buffer to reduce the amount of memory allocated for reading the file
                        byte[] buffer = new byte[_bufferSize];
                        int read;

                        try
                        {
                            while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cs.Write(buffer, 0, read);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        finally
                        {
                            inputStream.Close();
                        }
                    }
                    return fsCrypt.ToArray();
                }
            }
        }

        /// <summary>
        /// Decrypts an encrypted file with the FileEncrypt method through its path and the plain password.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="password"></param>
        public byte[] FileDecrypt(string inputFile, string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[_saltLength];

            using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
            {
                fsCrypt.Read(salt, 0, _saltLength);
                var key = new Rfc2898DeriveBytes(passwordBytes, salt, _keyIterations);
                
                using (var aesCryptoServiceProvider = AesCryptoServiceProvider.Create())
                {
                    aesCryptoServiceProvider.BlockSize = _blockSize;
                    aesCryptoServiceProvider.KeySize = _keySize;
                    aesCryptoServiceProvider.Padding = _paddingMode;
                    aesCryptoServiceProvider.Key = key.GetBytes(aesCryptoServiceProvider.KeySize / _bitsInByte);
                    aesCryptoServiceProvider.IV = key.GetBytes(aesCryptoServiceProvider.BlockSize / _bitsInByte);

                    using (CryptoStream cs = new CryptoStream(fsCrypt, aesCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (MemoryStream fsOut = new MemoryStream())
                        {
                            int read;
                            byte[] buffer = new byte[_bufferSize];

                            try
                            {
                                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fsOut.Write(buffer, 0, read);
                                }
                                return fsOut.ToArray();
                            }
                            catch (CryptographicException ex_CryptographicException)
                            {
                                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.Message);
                            }
                            return null;
                        }
                    }
                }
            }
        }
    }
}
