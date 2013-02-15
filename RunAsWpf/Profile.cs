using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RunAsDotNet
{
	[Serializable]
	public class Profile : INotifyPropertyChanged
	{
		public ObservableCollection<ProgramEntry> Entries { get; set; }

		private string _name;
		private string _domain;
		private string _userName;
		private string _password;

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		public string Domain
		{
			get { return _domain; }
			set
			{
				_domain = value;
				OnPropertyChanged("Domain");
			}
		}

		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				OnPropertyChanged("UserName");
			}
		}

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
			Entries = new ObservableCollection<ProgramEntry>();
		}

		private void OnPropertyChanged(string prop)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		public void LaunchProgram(ProgramEntry entry)
		{
			LaunchProgram(entry.Path);
		}

		public void LaunchProgram(string path)
		{
			string pass = new SimpleAES().Decrypt(_password);
			Win32.LaunchCommand(path, _domain, _userName, pass, Win32.LogonFlags.LOGON_NETCREDENTIALS_ONLY);
		}

	}
}
