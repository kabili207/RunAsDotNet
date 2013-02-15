using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RunAsDotNet
{
	[Serializable]
	public class ProgramEntryCollection : ObservableCollection<ProgramEntry>
	{
		public enum SortMethod { Recent, Alphabetical, Frequent }

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		private SortMethod _sortOrder = SortMethod.Recent;


		public SortMethod SortOrder
		{
			get { return _sortOrder; }
			set
			{
				_sortOrder = value;
				OnPropertyChanged("SortOrder");
			}
		}

		public ProgramEntryCollection()
			: base()
		{

		}

		public ProgramEntryCollection(IEnumerable<ProgramEntry> collection)
			: base(collection)
		{

		}

		private void OnPropertyChanged(string prop)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		public ProgramEntry GetByName(string name)
		{
			return this.FirstOrDefault(x => x.Name == name); ;
		}

		public ProgramEntry GetByPath(string path)
		{
			return this.FirstOrDefault(x => x.Path == path); ;
		}
	}
}
