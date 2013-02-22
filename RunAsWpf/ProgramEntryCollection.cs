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


		public ProgramEntryCollection()
			: base()
		{

		}

		public ProgramEntryCollection(IEnumerable<ProgramEntry> collection)
			: base(collection)
		{

		}

		[OnDeserialized]
		private void SetValuesOnDeserialized(StreamingContext context)
		{

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
	}
}
