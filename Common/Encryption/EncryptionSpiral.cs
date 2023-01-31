using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Encryption
{
	public static class EncryptionSpiral
	{
		private const string IVSpiral = "dbeac042c0764ed2941eba808370aef8";
		private const string IVAES = "40f5cc676d9a43d1b0ce509f1308aeb2";
		private const string IVCipher = "c0b1dbd2431d459fbb03eef443f78096";
		public static string SpiralEncrypt(string Text, string Key = "")
		{
			var MicrosoftEncrypt = new Encrypt().EncryptData(Text);
			//var Cipher = StringCipher.Encrypt(MicrosoftEncrypt, IVCipher);
			return AesHelper.Encrypt(MicrosoftEncrypt);
		}

		public static string SpiralDecrypt(string Text, string Key = "")
		{
			var Aes = AesHelper.Decrypt(Text);
			//var Cipher = StringCipher.Decrypt(Aes, IVCipher);
			return new Encrypt().DecryptData(Aes);
		}

		public static string AesEncode(this string password)
		{
			return AesHelper.Encrypt(password);
		}

		public static string AesDecode(this string password)
		{
			return AesHelper.Decrypt(password);
		}

		public static string EncryptOneSpiral(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return new Encrypt().EncryptData(value);
			}
			else
			{
				return string.Empty;
			}
		}
		public static string DecryptOneSpiral(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return new Encrypt().DecryptData(value);
			}
			else
			{
				return string.Empty;
			}
		}

		public static string EncryptCipherSpiral(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return StringCipher.Encrypt(value, "4951-8787");
			}
			else
			{
				return string.Empty;
			}
		}
		public static string DecryptCipherSpiral(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return StringCipher.Decrypt(value, "4951-8787");
			}
			else
			{
				return string.Empty;
			}
		}
	}
	public static class AesHelper
	{
		private const string IV = "lXhL3c65e1b9e90c";
		private const string KEY = "8a95da3bac434d0a9d4d68993edb40b9";
		public static string Encrypt(this string data)
		{
			byte[] buffer = null;

			Aes aes = Aes.Create();
			aes.IV = Encoding.UTF8.GetBytes(IV);
			aes.Key = Encoding.UTF8.GetBytes(KEY);

			ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
			using (MemoryStream ms = new MemoryStream())
			{
				using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
				{
					using (StreamWriter sw = new StreamWriter(cs))
					{
						sw.Write(data);
					}
				}
				buffer = ms.ToArray();
			}
			return Convert.ToBase64String(buffer);
		}

		public static string Decrypt(this string data)
		{
			byte[] buffer = Convert.FromBase64String(data);
			string result = null;

			Aes aes = Aes.Create();
			aes.IV = Encoding.UTF8.GetBytes(IV);
			aes.Key = Encoding.UTF8.GetBytes(KEY);

			ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV);
			using (MemoryStream ms = new MemoryStream(buffer))
			{
				using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
				{
					using (StreamReader sr = new StreamReader(cs))
					{
						result = sr.ReadToEnd();
					}
				}
			}

			return result;
		}
	}
	public class Encrypt : IDisposable
	{
		protected string passphrase;

		public string Passphrase
		{
			get { return passphrase; }
		}

		public Encrypt(bool standart = false)
		{
			passphrase = "808e30fd-6a18-4951-8787-fe3735e09340";
		}

		public string EncryptData(string value)
		{
			byte[] Results;
			System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
			MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
			byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
			TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
			TDESAlgorithm.Key = TDESKey;
			TDESAlgorithm.Mode = CipherMode.ECB;
			TDESAlgorithm.Padding = PaddingMode.PKCS7;
			try
			{
				byte[] DataToEncrypt = UTF8.GetBytes(value);
				ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
				Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
			}
			finally
			{
				TDESAlgorithm.Clear();
				HashProvider.Clear();
			}
			return Convert.ToBase64String(Results);
		}

		public string DecryptData(string value)
		{
			byte[] Results;
			System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
			MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
			byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
			TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
			TDESAlgorithm.Key = TDESKey;
			TDESAlgorithm.Mode = CipherMode.ECB;
			TDESAlgorithm.Padding = PaddingMode.PKCS7;
			try
			{
				byte[] DataToDecrypt = Convert.FromBase64String(value);
				ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
				Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
			}
			finally
			{
				TDESAlgorithm.Clear();
				HashProvider.Clear();
			}
			return UTF8.GetString(Results);
		}

		public void Dispose()
		{
		}
	}
	public static class StringCipher
	{
		private const int Keysize = 256;
		private const int DerivationIterations = 1000;
		public static string Encrypt(string plainText, string passPhrase)
		{
			var saltStringBytes = Generate256BitsOfRandomEntropy();
			var ivStringBytes = Generate256BitsOfRandomEntropy();
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
			{
				var keyBytes = password.GetBytes(Keysize / 8);
				using (var symmetricKey = new RijndaelManaged())
				{
					symmetricKey.BlockSize = 256;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
					{
						using (var memoryStream = new MemoryStream())
						{
							using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
							{
								cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
								cryptoStream.FlushFinalBlock();
								var cipherTextBytes = saltStringBytes;
								cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
								cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
								memoryStream.Close();
								cryptoStream.Close();
								return Convert.ToBase64String(cipherTextBytes);
							}
						}
					}
				}
			}
		}

		public static string Decrypt(string cipherText, string passPhrase)
		{
			var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
			var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
			var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
			var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();
			using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
			{
				var keyBytes = password.GetBytes(Keysize / 8);
				using (var symmetricKey = new RijndaelManaged())
				{
					symmetricKey.BlockSize = 256;
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.Padding = PaddingMode.PKCS7;
					using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
					{
						using (var memoryStream = new MemoryStream(cipherTextBytes))
						{
							using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
							{
								var plainTextBytes = new byte[cipherTextBytes.Length];
								var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
								memoryStream.Close();
								cryptoStream.Close();
								return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
							}
						}
					}
				}
			}
		}
		private static byte[] Generate256BitsOfRandomEntropy()
		{
			var randomBytes = new byte[32];
			using (var rngCsp = new RNGCryptoServiceProvider())
			{
				rngCsp.GetBytes(randomBytes);
			}
			return randomBytes;
		}
	}
}
