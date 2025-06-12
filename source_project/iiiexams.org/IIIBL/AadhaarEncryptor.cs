using System;
//using System.Collections.Generic;
//using System.Text;
using System.Security.Cryptography;
using System.Text;

namespace IIIBL
{
    public class AadhaarEncryptorDecryptor
    {
        public static System.Byte[] GenerateHash(Byte[] Input, HashAlgorithm Algorithm)
        {
            Byte[] Output = null;
            switch (Algorithm)
            {
                case HashAlgorithm.SHA1:
                    Output = (new System.Security.Cryptography.SHA1Managed()).ComputeHash(Input);
                    break;
                case HashAlgorithm.SHA256:
                    Output = (new System.Security.Cryptography.SHA256Managed()).ComputeHash(Input);
                    break;
                case HashAlgorithm.SHA384:
                    Output = (new System.Security.Cryptography.SHA384Managed()).ComputeHash(Input);
                    break;
                case HashAlgorithm.SHA512:
                    Output = (new System.Security.Cryptography.SHA512Managed()).ComputeHash(Input);
                    break;
                case HashAlgorithm.MD5:
                    Output = (new System.Security.Cryptography.MD5CryptoServiceProvider()).ComputeHash(Input);
                    break;
                default:
                    break;
            }
            return Output;
        }
        public static System.Byte[] Encrypt(Byte[] Input, EncryptionAlgorithm Algorithm, Byte[] Key, Byte[] IV)
        {
            Byte[] Output = null;
            System.Security.Cryptography.ICryptoTransform Encryptor = null;
            KeySizes[] Sz = null;
            Int32 KeyLength = 0;
            switch (Algorithm)
            {
                case EncryptionAlgorithm.AES:
                    System.Security.Cryptography.AesCryptoServiceProvider obj1 = new System.Security.Cryptography.AesCryptoServiceProvider();
                    Sz = obj1.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj1.Key = Key;
                        obj1.IV = IV;

                        Encryptor = obj1.CreateEncryptor();
                        Output = Encryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                case EncryptionAlgorithm.DES:
                    System.Security.Cryptography.DESCryptoServiceProvider obj2 = new System.Security.Cryptography.DESCryptoServiceProvider();
                    Sz = obj2.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj2.Key = Key;
                        obj2.IV = IV;

                        Encryptor = obj2.CreateEncryptor();
                        Output = Encryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                case EncryptionAlgorithm.Rijndael:
                    System.Security.Cryptography.RijndaelManaged obj3 = new System.Security.Cryptography.RijndaelManaged();
                    Sz = obj3.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj3.Key = Key;
                        obj3.IV = IV;

                        Encryptor = obj3.CreateEncryptor();
                        Output = Encryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                case EncryptionAlgorithm.TripleDES:
                    System.Security.Cryptography.TripleDESCryptoServiceProvider obj4 = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
                    Sz = obj4.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj4.Key = Key;
                        obj4.IV = IV;

                        Encryptor = obj4.CreateEncryptor();
                        Output = Encryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                case EncryptionAlgorithm.RC2:
                    System.Security.Cryptography.RC2CryptoServiceProvider obj5 = new System.Security.Cryptography.RC2CryptoServiceProvider();
                    Sz = obj5.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj5.Key = Key;
                        obj5.IV = IV;

                        Encryptor = obj5.CreateEncryptor();
                        Output = Encryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                default:
                    break;
            }
            return Output;
        }
        public static System.Byte[] Decrypt(Byte[] Input, EncryptionAlgorithm Algorithm, Byte[] Key, Byte[] IV)
        {
            Byte[] Output = null;
            System.Security.Cryptography.ICryptoTransform Decryptor = null;
            KeySizes[] Sz = null;
            Int32 KeyLength = 0;

            switch (Algorithm)
            {
                case EncryptionAlgorithm.AES:
                    System.Security.Cryptography.AesCryptoServiceProvider obj1 = new System.Security.Cryptography.AesCryptoServiceProvider();
                    Sz = obj1.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj1.Key = Key;
                        obj1.IV = IV;

                        Decryptor = obj1.CreateDecryptor();
                        Output = Decryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                case EncryptionAlgorithm.DES:
                    System.Security.Cryptography.DESCryptoServiceProvider obj2 = new System.Security.Cryptography.DESCryptoServiceProvider();
                    Sz = obj2.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj2.Key = Key;
                        obj2.IV = IV;

                        Decryptor = obj2.CreateDecryptor();
                        Output = Decryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                case EncryptionAlgorithm.Rijndael:
                    System.Security.Cryptography.RijndaelManaged obj3 = new System.Security.Cryptography.RijndaelManaged();
                    Sz = obj3.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj3.Key = Key;
                        obj3.IV = IV;

                        Decryptor = obj3.CreateDecryptor();
                        Output = Decryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                case EncryptionAlgorithm.TripleDES:
                    System.Security.Cryptography.TripleDESCryptoServiceProvider obj4 = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
                    Sz = obj4.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj4.Key = Key;
                        obj4.IV = IV;

                        Decryptor = obj4.CreateDecryptor();
                        Output = Decryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                case EncryptionAlgorithm.RC2:
                    System.Security.Cryptography.RC2CryptoServiceProvider obj5 = new System.Security.Cryptography.RC2CryptoServiceProvider();
                    Sz = obj5.LegalKeySizes;
                    KeyLength = Key.Length * 8; // Converted to bits
                    if (KeyLength < Sz[0].MinSize || KeyLength > Sz[0].MaxSize)
                    {
                        throw new Exception(String.Format("Invalid Key Length. Min {0} - Max {1} bit", Sz[0].MinSize, Sz[0].MaxSize));
                    }
                    else
                    {
                        obj5.Key = Key;
                        obj5.IV = IV;

                        Decryptor = obj5.CreateDecryptor();
                        Output = Decryptor.TransformFinalBlock(Input, 0, Input.Length);
                    }
                    break;

                default:
                    break;

            }
            return Output;
        }

        public static string EncryptAadhaar(String Aadhaar, Byte[] Key, Byte[] IV)
        {
            //Byte[] key = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AKey"].ToString());
            //Byte[] IV = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AIV"].ToString());

            Byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(Aadhaar);

            Byte[] byteEncrypt = IIIBL.AadhaarEncryptorDecryptor.Encrypt(toEncryptArray, IIIBL.EncryptionAlgorithm.TripleDES, Key, IV);
            String encryptedvalue = Convert.ToBase64String(byteEncrypt);
            return encryptedvalue;
        }

        public static string DecryptAadhaar(String Aadhaar, Byte[] Key, Byte[] IV)
        {
            //Byte[] key = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AKey"].ToString());
            //Byte[] IV = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AIV"].ToString());

            Byte[] toDecryptArray = Convert.FromBase64String(Aadhaar);

            Byte[] byteDecrypt = IIIBL.AadhaarEncryptorDecryptor.Decrypt(toDecryptArray, IIIBL.EncryptionAlgorithm.TripleDES, Key, IV);
            String Decryptedvalue = UTF8Encoding.UTF8.GetString(byteDecrypt);
            return Decryptedvalue;
        }

    }

    public enum HashAlgorithm
    {
        SHA1,
        SHA256,
        SHA384,
        SHA512,
        MD5
    }
    public enum EncryptionAlgorithm
    {
        AES,
        DES,
        TripleDES,
        Rijndael,
        RC2,
        RSA
    }
}
