using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RunAsDotNet
{
	/// <summary>
	/// Represents a profile configuration 
	/// </summary>
	[Serializable]
	public class Profile : INotifyPropertyChanged
	{
		public ProgramEntryCollection Entries { get; set; }

		private string _name;
		private string _domain;
		private string _userName;
		private string _password;

		/// <summary>
		/// Gets or sets the name of the profile
		/// </summary>
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		/// <summary>
		/// Gets or sets the domain to authenticate against
		/// </summary>
		public string Domain
		{
			get { return _domain; }
			set
			{
				_domain = value;
				OnPropertyChanged("Domain");
			}
		}

		/// <summary>
		/// Gets or sets the user's login name
		/// </summary>
		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				OnPropertyChanged("UserName");
			}
		}

		/// <summary>
		/// Gets or sets the user's password
		/// </summary>
		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				OnPropertyChanged("Password");
			}
		}


		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		public Profile()
		{
			Entries = new ProgramEntryCollection();
		}

		private void OnPropertyChanged(string prop)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		/// <summary>
		/// Launches the specified program entry
		/// </summary>
		/// <param name="entry">The program entry to launch</param>
		public void LaunchProgram(ProgramEntry entry)
		{
			LaunchProgram(entry.Path);
			entry.LastRan = DateTime.Now;

			// TODO: Remove this
			int index = Entries.IndexOf(entry);
			Entries.Move(index, 0);
		}

		/// <summary>
		/// Launches the specifed command path
		/// </summary>
		/// <param name="path">The path of the command</param>
		public void LaunchProgram(string path)
		{
			string pass = new SimpleAES().Decrypt(_password);
			Win32.LaunchCommand(path, _domain, _userName, pass, Win32.LogonFlags.NetCredentialsOnly);
		}

	}
}
