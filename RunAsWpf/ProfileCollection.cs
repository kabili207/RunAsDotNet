using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Windows.Shell;

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

		public Profile GetByName(string name)
		{
			return this.FirstOrDefault(x => x.Name == name); ;
		}

		public void CreateJumpTasks(JumpList jumpList)
		{
			jumpList.JumpItems.Clear();
			foreach (Profile profile in this)
			{
				foreach (ProgramEntry entry in profile.Entries)
				{
					// Configure a new JumpTask.
					JumpTask jumpTask1 = new JumpTask();
					// Get the path to Calculator and set the JumpTask properties.
					jumpTask1.ApplicationPath = System.Reflection.Assembly.GetEntryAssembly().Location;
					jumpTask1.IconResourcePath = entry.Path;
					jumpTask1.Title = entry.Name;
					//jumpTask1.Description = "Open Calculator.";
					jumpTask1.CustomCategory = profile.Name;
					jumpTask1.Arguments = string.Format("\"{0}\" \"{1}\"", profile.Name, entry.Path);
					jumpList.JumpItems.Add(jumpTask1);
				}
			}
			jumpList.Apply();
		}
	}
}
