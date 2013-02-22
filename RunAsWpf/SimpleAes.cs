using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RunAsDotNet
{
	// This code was taken from http://stackoverflow.com/questions/165808/simple-2-way-encryption-for-c-sharp
	public class SimpleAES
	{
		private static readonly byte[] key =
		{
			152, 151, 205, 8, 98, 172, 107, 177, 245, 53, 251,
			32, 77, 40, 84, 6, 159, 95, 37, 211, 149, 102, 184,
			100, 195, 225, 148, 255, 121, 185, 15, 144
		};
		private static readonly byte[] vector =
		{ 
			201, 76, 133, 104, 169, 63, 37, 250, 148, 188, 31, 127, 8, 52, 41, 70
		};

		private ICryptoTransform encryptor, decryptor;
		private UTF8Encoding encoder;

		public SimpleAES()
		{
			RijndaelManaged rm = new RijndaelManaged();
			encryptor = rm.CreateEncryptor(key, vector);
			decryptor = rm.CreateDecryptor(key, vector);
			encoder = new UTF8Encoding();
		}

		public string Encrypt(string unencrypted)
		{
			return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
		}

		public string Decrypt(string encrypted)
		{
			return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
		}

		public byte[] Encrypt(byte[] buffer)
		{
			return Transform(buffer, encryptor);
		}

		public byte[] Decrypt(byte[] buffer)
		{
			return Transform(buffer, decryptor);
		}

		protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
		{
			MemoryStream stream = new MemoryStream();
			using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
			{
				cs.Write(buffer, 0, buffer.Length);
			}
			return stream.ToArray();
		}
	}
}
