using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Runtime.Serialization;

namespace RunAsDotNet
{
	[Serializable]
	public class ProgramEntryCollection : ObservableCollection<ProgramEntry>
	{
		public enum SortMethod { Recent, Alphabetical, Frequent }

		private SortMethod _sortOrder = SortMethod.Recent;

		[NonSerialized]
		CollectionView _sortedView;

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

		public ProgramEntryCollection()
			: base()
		{
			_sortedView = new ListCollectionView(this);
			ResortView();
		}

		public ProgramEntryCollection(IEnumerable<ProgramEntry> collection)
			: base(collection)
		{
			_sortedView = new ListCollectionView(this);
			ResortView();
		}

		[OnDeserialized]
		private void SetValuesOnDeserialized(StreamingContext context)
		{
			_sortedView = new ListCollectionView(this);
			ResortView();
		}

		private void OnPropertyChanged(string prop)
		{
			base.OnPropertyChanged(new PropertyChangedEventArgs(prop));
		}

		/// <summary>
		/// Gets the ProgramEntry with the specified name
		/// </summary>
		/// <param name="name">The name of the program</param>
		/// <returns>The program who's name matches the one specified</returns>
		public ProgramEntry GetByName(string name)
		{
			return this.FirstOrDefault(x => x.Name == name); ;
		}

		/// <summary>
		/// Gets the ProgramEntry with the path specified
		/// </summary>
		/// <param name="path">The path of the program</param>
		/// <returns>The program who's path matches the one specified</returns>
		public ProgramEntry GetByPath(string path)
		{
			return this.FirstOrDefault(x => x.Path == path); ;
		}

		private void ResortView()
		{
			_sortedView.SortDescriptions.Clear();

			switch (SortOrder)
			{
				case ProgramEntryCollection.SortMethod.Alphabetical:
					_sortedView.SortDescriptions.Add(
						new SortDescription("Name", ListSortDirection.Ascending));
					break;
				case ProgramEntryCollection.SortMethod.Frequent:
					// TODO: Code for frequent
				default:
				case ProgramEntryCollection.SortMethod.Recent:
					_sortedView.SortDescriptions.Add(
						new SortDescription("LastRan", ListSortDirection.Descending));
					break;
			}
		}
	}
}
