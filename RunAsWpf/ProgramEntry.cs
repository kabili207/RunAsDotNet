using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace RunAsDotNet
{
	/// <summary>
	/// Represents an individual program entry in a profile
	/// </summary>
	[Serializable]
	public class ProgramEntry : INotifyPropertyChanged
	{
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		private string _name;
		private string _path;

		[OptionalField(VersionAdded = 2)]
		private DateTime? _lastRan;

		[NonSerialized]
		private BitmapSource _image;

		/// <summary>
		/// Gets or sets the display name of this program entry
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
		/// Gets or sets the file path of the program
		/// </summary>
		public string Path
		{
			get { return _path; }
			set
			{
				_path = value;
				OnPropertyChanged("Path");
			}
		}

		/// <summary>
		/// Gets or sets the icon of this program
		/// </summary>
		public BitmapSource Image
		{
			get { return _image; }
			set
			{
				_image = value;
				OnPropertyChanged("Image");
			}
		}

		/// <summary>
		/// Gets or sets the date and time this program was last ran
		/// </summary>
		public DateTime? LastRan
		{
			get { return _lastRan; }
			set
			{
				_lastRan = value;
				OnPropertyChanged("LastRan");
			}
		}


		private void OnPropertyChanged(string prop)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}
		}

		[OnDeserialized]
		private void SetValuesOnDeserialized(StreamingContext context)
		{
			SetImage(IconFromFilePath(Path));
		}

		/// <summary>
		/// Gets the icon associated with the specified file
		/// </summary>
		/// <param name="filePath">The path of the icon</param>
		/// <returns>The icon associated with the file</returns>
		public static Icon IconFromFilePath(string filePath)
		{
			Icon result = null;
			try
			{
				result = Icon.ExtractAssociatedIcon(filePath);
			}
			catch
			{
				// We could supply a default Icon here...
			}
			return result;
		}

		/// <summary>
		/// Sets the image of the program using the specified <see cref="System.Drawing.Icon"/>
		/// </summary>
		/// <param name="icon">The icon</param>
		public void SetImage(Icon icon)
		{
			if (icon == null)
			{
				Image = null;
			}
			else
			{

				using (MemoryStream memory = new MemoryStream())
				{
					icon.ToBitmap().Save(memory, ImageFormat.Png);
					memory.Position = 0;
					BitmapImage bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.StreamSource = memory;
					bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapImage.EndInit();
					Image = bitmapImage;
				}
			}
		}
	}
}
