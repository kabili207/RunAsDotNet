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
	[Serializable]
	public class ProgramEntry : INotifyPropertyChanged
	{
		private string _name;
		private string _path;

		[NonSerialized]
		private ImageSource _image;

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		public string Path
		{
			get { return _path; }
			set
			{
				_path = value;
				OnPropertyChanged("Path");
			}
		}

		public ImageSource Image
		{
			get { return _image; }
			set
			{
				_image = value;
				OnPropertyChanged("Image");
			}
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

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

		public static Icon IconFromFilePath(string filePath)
		{
			Icon result = null;
			try
			{
				result = Icon.ExtractAssociatedIcon(filePath);
			}
			catch
			{
				// swallow and return nothing. You could supply a default Icon here as well
			}
			return result;
		}

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
