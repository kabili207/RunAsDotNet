using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using System.Collections.ObjectModel;

namespace RunAsDotNet
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ObservableCollection<Profile> _profiles = new ObservableCollection<Profile>();

		public MainWindow()
		{
			InitializeComponent();
			_profiles.Add(new Profile()
			{
				Name = "Default"
			});
			cmbProfiles.ItemsSource = _profiles;
		}

		public Icon IconFromFilePath(string filePath)
		{
			Icon result = null;
			try
			{
				result = System.Drawing.Icon.ExtractAssociatedIcon(filePath);
			}
			catch
			{
				// swallow and return nothing. You could supply a default Icon here as well
			}
			return result;
		}

		private void btnBrowse_Click(object sender, RoutedEventArgs e)
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			if (profile != null)
			{
				var _programs = profile.Entries;
				OpenFileDialog ofd = new OpenFileDialog();
				//ofd.Filter = "Solutions files (*.sln)|*.sln";
				ofd.DereferenceLinks = false;
				ofd.InitialDirectory = Win32.GetStartMenuPath();
				if (ofd.ShowDialog() == true)
				{

					ProgramEntry entry = new ProgramEntry();

					string realPath = MsiShortcutParser.ParseShortcut(ofd.FileName);
					entry.Path = realPath;
					entry.Name = ofd.SafeFileName;

					FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(realPath);

					if (!string.IsNullOrWhiteSpace(versionInfo.FileDescription))
						entry.Name = versionInfo.FileDescription;

					Icon programIcon = IconFromFilePath(realPath);
					if (programIcon == null)
					{
						entry.Image = null;
					}
					else
					{

						using (MemoryStream memory = new MemoryStream())
						{
							programIcon.ToBitmap().Save(memory, ImageFormat.Png);
							memory.Position = 0;
							BitmapImage bitmapImage = new BitmapImage();
							bitmapImage.BeginInit();
							bitmapImage.StreamSource = memory;
							bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
							bitmapImage.EndInit();
							entry.Image = bitmapImage;
						}
					}
					_programs.Add(entry);
				}
			}
		}

		private void btnRun_Click(object sender, RoutedEventArgs e)
		{
			string user = txtUserName.Text; //"anagle";
			string domain = txtDomain.Text; // "leeds_pdc";
			string password = txtPassword.Password; // "**********";
			string program = @"C:\Program Files\Microsoft Office\Office14\MSACCESS.EXE";

			try
			{
				Win32.LaunchCommand(program, domain, user, password, Win32.LogonFlags.LOGON_NETCREDENTIALS_ONLY);
			}
			catch (Exception ex)
			{
				Console.WriteLine("LaunchCommand error: " + ex.Message);
			}
		}

		private void cmbProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			this.DataContext = profile;
			txtPassword.Password = profile.Password;
		}

	}
}
