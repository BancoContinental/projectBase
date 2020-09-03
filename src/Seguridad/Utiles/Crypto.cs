using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Continental.API.Seguridad.Utiles
{
	class Crypto
	{
		/// <summary>
		/// Metodo que devuelte GetSHA1.
		/// </summary>
		/// <param name="texto">PARAMETRO.</param>
		/// <returns>Retorna GetSHA1.</returns>
		public static string GetSHA1 ( string texto )
		{
			SHA1 sha1              = SHA1Managed.Create();
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] stream          = null;
			StringBuilder sb       = new StringBuilder();
			stream                 = sha1.ComputeHash ( encoding.GetBytes ( texto ) );

			for ( int i = 0 ; i < stream.Length ; i++ )
			{
				sb.AppendFormat ( "{0:x2}" , stream [ i ] );
			}

			return sb.ToString ();
		}

		/// <summary>
		/// Gets or Sets Salt.
		/// </summary>
		private static byte[] salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

		/// <summary>
		/// Encrypt the given string using AES.  The string can be decrypted using 
		/// DecryptStringAES().  The sharedSecret parameters must match.
		/// </summary>
		/// <param name="plainText">The text to encrypt.</param>
		/// <param name="sharedSecret">A password used to generate a key for encryption.</param>
		/// <returns>retorna.</returns>
		public static string EncryptStringAES ( string plainText , string sharedSecret )
		{
			if ( string.IsNullOrEmpty ( plainText ) )
			{
				throw new ArgumentNullException ( "plainText" );
			}

			if ( string.IsNullOrEmpty ( sharedSecret ) )
			{
				throw new ArgumentNullException ( "sharedSecret" );
			}

			string outStr          = null;                       // Encrypted string to return
			RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

			try
			{
				// generate the key from the shared secret and the salt
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);

				// Create a RijndaelManaged object
				// with the specified key and IV.
				aesAlg     = new RijndaelManaged ();
				aesAlg.Key = key.GetBytes ( aesAlg.KeySize / 8 );
				aesAlg.IV  = key.GetBytes ( aesAlg.BlockSize / 8 );

				// Create a decrytor to perform the stream transform.
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for encryption.
				using ( MemoryStream msEncrypt = new MemoryStream () )
				{
					using ( CryptoStream csEncrypt = new CryptoStream ( msEncrypt , encryptor , CryptoStreamMode.Write ) )
					{
						using ( StreamWriter swEncrypt = new StreamWriter ( csEncrypt ) )
						{
							swEncrypt.Write ( plainText );
						}
					}

					outStr = Convert.ToBase64String ( msEncrypt.ToArray () );
				}
			}
			finally
			{
				// Clear the RijndaelManaged object.
				if ( aesAlg != null )
				{
					aesAlg.Clear ();
				}
			}

			// Return the encrypted bytes from the memory stream.
			return outStr;
		}

		/// <summary>
		/// Decrypt the given string.  Assumes the string was encrypted using.
		/// EncryptStringAES(), using an identical sharedSecret.
		/// </summary>
		/// <param name="cipherText">The text to decrypt.</param>
		/// <param name="sharedSecret">A password used to generate a key for decryption.</param>
		/// <returns>retorna.</returns>
		public static string DecryptStringAES ( string cipherText , string sharedSecret )
		{
			if ( string.IsNullOrEmpty ( cipherText ) )
			{
				throw new ArgumentNullException ( "cipherText" );
			}

			if ( string.IsNullOrEmpty ( sharedSecret ) )
			{
				throw new ArgumentNullException ( "sharedSecret" );
			}

			// Declare the RijndaelManaged object
			// used to decrypt the data.
			RijndaelManaged aesAlg = null;

			// Declare the string used to hold
			// the decrypted text.
			string plaintext = null;

			try
			{
				// generate the key from the shared secret and the salt
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, salt);

				// Create a RijndaelManaged object
				// with the specified key and IV.
				aesAlg     = new RijndaelManaged ();
				aesAlg.Key = key.GetBytes ( aesAlg.KeySize / 8 );
				aesAlg.IV  = key.GetBytes ( aesAlg.BlockSize / 8 );

				// Create a decrytor to perform the stream transform.
				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for decryption.
				byte[] bytes = Convert.FromBase64String(cipherText);
				using ( MemoryStream msDecrypt = new MemoryStream ( bytes ) )
				{
					using ( CryptoStream csDecrypt = new CryptoStream ( msDecrypt , decryptor , CryptoStreamMode.Read ) )
					{
						using ( StreamReader srDecrypt = new StreamReader ( csDecrypt ) )
						{
							// Read the decrypted bytes from the decrypting stream
							// and place them in a string.
							plaintext = srDecrypt.ReadToEnd ();
						}
					}
				}
			}
			finally
			{
				// Clear the RijndaelManaged object.
				if ( aesAlg != null )
				{
					aesAlg.Clear ();
				}
			}

			return plaintext;
		}

		static string key = "1000:dIFATM3Ou3Y2qT5C6QwDIn6VljIyNoLj:4Dh0+wnXqrjCiFyEJwM+kmqcR+CoITen";

		/// <summary>
		/// Metodo de Encrypt.
		/// </summary>
		/// <param name="toEncrypt">1.</param>
		/// <param name="useHashing">2.</param>
		/// <returns>retorno.</returns>
		public static string Encrypt ( string toEncrypt , bool useHashing )
		{
			byte[] keyArray;
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
			if ( useHashing )
			{
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray                         = hashmd5.ComputeHash ( UTF8Encoding.UTF8.GetBytes ( key ) );
				hashmd5.Clear ();
			}
			else
			{
				keyArray = UTF8Encoding.UTF8.GetBytes ( key );
			}

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
			{
				Key     = keyArray,
				Mode    = CipherMode.ECB,
				Padding = PaddingMode.PKCS7,
			};
			ICryptoTransform cTransform = tdes.CreateEncryptor();
			byte[] resultArray          = cTransform
				.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

			tdes.Clear ();
			return Convert.ToBase64String ( resultArray , 0 , resultArray.Length );
		}

		/// <summary>
		/// Metodo de Decypt.
		/// </summary>
		/// <param name="cipherString">1.</param>
		/// <param name="useHashing">2.</param>
		/// <returns>retorna la clave desencriptada.</returns>
		public static string Decrypt ( string cipherString , bool useHashing )
		{
			try
			{
				byte[] keyArray;
				byte[] toEncryptArray = Convert.FromBase64String(cipherString);
				if ( useHashing )
				{
					MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
					keyArray                         = hashmd5.ComputeHash ( UTF8Encoding.UTF8.GetBytes ( key ) );
					hashmd5.Clear ();
				}
				else
				{
					keyArray = UTF8Encoding.UTF8.GetBytes ( key );
				}

				TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
				{
					Key     = keyArray,
					Mode    = CipherMode.ECB,
					Padding = PaddingMode.PKCS7,
				};
				ICryptoTransform cTransform = tdes.CreateDecryptor();
				byte[] resultArray          = cTransform.TransformFinalBlock(
									 toEncryptArray, 0, toEncryptArray.Length);
				tdes.Clear ();

				return UTF8Encoding.UTF8.GetString ( resultArray );
			}
			catch ( Exception ex )
			{
				throw ex;
			}
		}

        /// <summary>
        /// Encrypt() Method
        /// encripta una string y devuelve una hashed string encriptada
        /// </summary>
        /// <param name="cleanString">Cadena a encriptar</param>
        /// <returns>Devuelta la cadena cifrada</returns>
        public static string Encrypt(string cleanString)
        {
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanString);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}
