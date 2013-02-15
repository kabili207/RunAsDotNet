using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace RunAsDotNet
{
	[Serializable]
	public class ProfileCollection : ObservableCollection<Profile>
	{
		public ProfileCollection()
			: base()
		{

		}

		public ProfileCollection(IEnumerable<Profile> collection)
			: base(collection)
		{

		}

		public static ProfileCollection FromStream(Stream stream)
		{
			try
			{
				BinaryFormatter formatter = new BinaryFormatter();

				// Deserialize the hashtable from the file and 
				// assign the reference to the local variable.
				object col = formatter.Deserialize(stream);
				if (col is ProfileCollection)
					return (ProfileCollection)col;
				else
					return new ProfileCollection((IEnumerable<Profile>)col);
			}
			catch (SerializationException e)
			{
				Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
				throw;
			}
		}

		public void ToStream(Stream stream)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			try
			{
				formatter.Serialize(stream, this);
			}
			catch (SerializationException e)
			{
				Console.WriteLine("Failed to serialize. Reason: " + e.Message);
				throw;
			}
		}
	}
}
