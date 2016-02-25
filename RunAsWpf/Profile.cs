using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Data;

namespace RunAsDotNet
{
	/// <summary>
	/// Represents a profile configuration 
	/// </summary>
	[Serializable]
	public class Profile : INotifyPropertyChanged
	{
		public enum SortMethod { Recent, Alphabetical, Frequent }

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		private string _name;
		private string _domain;
		private string _userName;
		private string _password;

		[OptionalField(VersionAdded = 2)]
		private CircularBuffer<ProgramEntry> _recentApps = new CircularBuffer<ProgramEntry>(20);

		[OptionalField(VersionAdded = 2)]
		private SortMethod _sortOrder = SortMethod.Recent;

		[OptionalField(VersionAdded = 3)]
		private bool _netOnly = false;

		[OptionalField(VersionAdded = 3)]
		private bool _noProfile = false;

		[NonSerialized]
		ListCollectionView _sortedView;

		public ProgramEntryCollection Entries { get; set; }

		/// <summary>
		/// Gets or sets the sort order of the program entries.
		/// This value is not yet used.
		/// </summary>
		public SortMethod SortOrder
		{
			get { return _sortOrder; }
			set
			{
				_sortOrder = value;
				ResortView();
				OnPropertyChanged("SortOrder");
			}
		}

		public CollectionView SortedView
		{
			get { return _sortedView; }
		}


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

		/// <summary>
		/// Gets or sets a value indicating whether this is a network-only profile.
		/// </summary>
		public bool NetOnly
		{
			get { return _netOnly; }
			set
			{
				_netOnly = value;
				OnPropertyChanged("NetOnly");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if we should create a local profile or not.
		/// </summary>
		public bool NoProfile
		{
			get { return _noProfile; }
			set
			{
				_noProfile = value;
				OnPropertyChanged("NoProfile");
			}
		}

		public CircularBuffer<ProgramEntry> RecentApps
		{
			get { return _recentApps; }
		}

		public Profile()
		{
			Entries = new ProgramEntryCollection();
			_sortedView = new ListCollectionView(Entries);
			ResortView();
		}


		[OnDeserialized]
		private void SetValuesOnDeserialized(StreamingContext context)
		{
			if (_recentApps == null || _recentApps.Capacity == 0)
				_recentApps = new CircularBuffer<ProgramEntry>(20);
			_recentApps.AllowOverflow = true;
			_sortedView = new ListCollectionView(Entries);
			ResortView();
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
			RecentApps.Put(entry);

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
			Win32.LogonFlags flags =
				_netOnly ? Win32.LogonFlags.NetCredentialsOnly :
				_noProfile ? Win32.LogonFlags.None : Win32.LogonFlags.WithProfile;

			Win32.LaunchCommand(path, _domain, _userName, pass, flags);
		}

		private void ResortView()
		{
			_sortedView.SortDescriptions.Clear();
			_sortedView.CustomSort = null;
			switch (SortOrder)
			{
				case SortMethod.Alphabetical:
					_sortedView.SortDescriptions.Add(
						new SortDescription("Name", ListSortDirection.Ascending));
					break;
				case SortMethod.Frequent:
					_sortedView.CustomSort = new RecentComparer(_recentApps);
					break;
				default:
				case SortMethod.Recent:
					_sortedView.SortDescriptions.Add(
						new SortDescription("LastRan", ListSortDirection.Descending));
					break;
			}
		}

		private class RecentComparer : System.Collections.IComparer
		{
			CircularBuffer<ProgramEntry> _recent = null;

			public RecentComparer(CircularBuffer<ProgramEntry> recent)
			{
				_recent = recent;
			}

			public int Compare(object objX, object objY)
			{
				ProgramEntry x = objX as ProgramEntry;
				ProgramEntry y = objY as ProgramEntry;
				if (x == null || y == null)
					throw new ArgumentException("Both objects must be of type ProgramEntry");
				int countX = _recent.Count(a => a == x);
				int countY = _recent.Count(a => a == y);
				return countY.CompareTo(countX);
			}
		}

	}
}
