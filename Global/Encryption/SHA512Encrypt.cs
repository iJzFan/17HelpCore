using System.Security.Cryptography;
using System.Text;

namespace HELP.GlobalFile.Global.Encryption
{
	public class SHA512Encrypt : IEncrypt
	{
		private const string _salt = "ASPNETCORE";

		/// <summary>
		/// the ONLY usage is make encryted string can't be recovered
		/// do NOT use it as security key, etc.
		/// </summary>
		public virtual string Encrypt(string source)
		{
			//add salt to input to improve security
			source = source + _salt;

			// Create a new instance of the MD5CryptoServiceProvider object.
			SHA512 sha512 = SHA512.Create();

			// Convert the input string to a byte array and compute the hash.
			byte[] data = sha512.ComputeHash(Encoding.Default.GetBytes(source));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}
	}
}