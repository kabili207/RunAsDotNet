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
		/// <summary>
		/// Gets or sets the name of the default profile
		/// </summary>
		public string DefaultProfile { get; set; }

		public ProfileCollection()
			: base()
		{

		}

		public ProfileCollection(IEnumerable<Profile> collection)
			: base(collection)
		{

		}

		/// <summary>
		/// Creates a ProfileCollection from the specified stream
		/// </summary>
		/// <param name="stream">The stream</param>
		/// <returns>The profile collection</returns>
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
				//Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
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
		
		/// <summary>
		/// Gets the default profile if one exists.
		/// </summary>
		/// <returns>The default profile or <c>null</c> if one does not exist.</returns>
		public Profile GetDefaultProfile()
		{
			return GetByName(DefaultProfile);
		}

		/// <summary>
		/// Gets the profile with the specifed <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of the profile</param>
		/// <returns>The profile who's name matches the one specified</returns>
		public Profile GetByName(string name)
		{
			return this.FirstOrDefault(x => x.Name == name); ;
		}

		/// <summary>
		/// Re-creates the jump tasks associated with the specified JumpList
		/// </summary>
		/// <param name="jumpList">The JumpList to re-create</param>
		public void CreateJumpTasks(JumpList jumpList)
		{
			jumpList.JumpItems.Clear();
			foreach (Profile profile in this)
			{
				foreach (ProgramEntry entry in profile.SortedView)
				{
					JumpTask jumpTask = new JumpTask();

					jumpTask.ApplicationPath = System.Reflection.Assembly.GetEntryAssembly().Location;
					jumpTask.IconResourcePath = entry.Path;
					jumpTask.Title = entry.Name;
					//jumpTask.Description = "Open Calculator.";
					jumpTask.CustomCategory = profile.Name;
					jumpTask.Arguments = string.Format("\"{0}\" \"{1}\"", profile.Name, entry.Path);
					jumpList.JumpItems.Add(jumpTask);
				}
			}
			jumpList.Apply();
		}
	}
}
