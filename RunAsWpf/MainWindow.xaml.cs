﻿using System;
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
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Windows.Shell;

namespace RunAsDotNet
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private JumpList _jumpList = new JumpList();

		ProfileCollection _profiles = null;

		public MainWindow()
		{
			InitializeComponent();

			try
			{
				LoadProfiles();
			}
			catch { }
			if (_profiles == null)
			{
				_profiles = new ProfileCollection();
				_profiles.Add(new Profile()
				{
					Name = "Default"
				});
				_profiles.DefaultProfile = "Default";
			}
			cmbProfiles.ItemsSource = _profiles;
			//cmbProfiles.SelectedIndex = 0;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			cmbProfiles.SelectedIndex = 0;
			if (!string.IsNullOrWhiteSpace(_profiles.DefaultProfile))
			{
				Profile profile = _profiles.GetByName(_profiles.DefaultProfile);
				if (profile != null)
					cmbProfiles.SelectedItem = profile; 
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveProfiles();
		}

		private void btnDeleteProgram_Click(object sender, RoutedEventArgs e)
		{
			ProgramEntry entry = lstPrograms.SelectedItem as ProgramEntry;
			if (entry != null)
			{
				Profile profile = cmbProfiles.SelectedItem as Profile;
				profile.Entries.Remove(entry);
				SaveProfiles();
				CreateJumpTasks();
			}
		}

		private void btnBrowse_Click(object sender, RoutedEventArgs e)
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			if (profile != null)
			{
				var _programs = profile.Entries;
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.Filter = "Programs (*.lnk, *.exe)|*.lnk;*.exe";
				ofd.DereferenceLinks = false;
				ofd.Multiselect = true;
				ofd.InitialDirectory = Win32.GetStartMenuPath();
				if (ofd.ShowDialog() == true)
				{
					AddProgram(ofd.FileNames);
				}
			}
		}

		private void AddProgram(string files)
		{
			AddProgram(new string[] { files });
		}

		private void AddProgram(IEnumerable<string> files)
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			if (profile != null)
			{
				List<string> failedFiles = new List<string>();
				var _programs = profile.Entries;
				foreach (string file in files)
				{
					ProgramEntry entry = new ProgramEntry();

					string realPath = file;
					FileInfo info = new FileInfo(realPath);
					if (info.Extension.ToLower() == ".lnk")
						realPath = MsiShortcutParser.ParseShortcut(realPath);
					entry.Path = realPath;
					entry.Name = file;

					if (_programs.Count(x => x.Path == realPath) > 0)
					{
						failedFiles.Add(realPath);
						continue;
					}

					FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(realPath);

					if (!string.IsNullOrWhiteSpace(versionInfo.FileDescription))
						entry.Name = versionInfo.FileDescription;

					entry.SetImage(ProgramEntry.IconFromFilePath(realPath));
					_programs.Insert(0, entry);
				}
				if (failedFiles.Count > 0)
				{
					MessageBox.Show("The program(s) have already been added:" + 
						string.Join(Environment.NewLine, failedFiles.ToArray()),
						"Failed to add program", MessageBoxButton.OK, MessageBoxImage.Error);
				}

				lstPrograms.SelectedIndex = 0;
				SaveProfiles();
				CreateJumpTasks();
			}
		}

		private void lstPrograms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				btnRun_Click(sender, e);
			}
		}

		private void btnRun_Click(object sender, RoutedEventArgs e)
		{
			ProgramEntry entry = lstPrograms.SelectedItem as ProgramEntry;
			if (entry != null)
			{
				Profile profile = cmbProfiles.SelectedItem as Profile;
				int index = profile.Entries.IndexOf(entry);
				profile.Entries.Move(index, 0);

				CreateJumpTasks();

				string user = txtUserName.Text;
				string domain = txtDomain.Text;
				string password = txtPassword.Password;
				string program = entry.Path;

				try
				{
					Win32.LaunchCommand(program, domain, user, password, Win32.LogonFlags.LOGON_NETCREDENTIALS_ONLY);
				}
				catch (Exception ex)
				{
					MessageBox.Show("LaunchCommand error: " + ex.Message);
				}
			}
		}

		private void cmbProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			this.DataContext = profile;
			if (profile != null)
			{
				SimpleAES aes = new SimpleAES();
				if (string.IsNullOrWhiteSpace(profile.Password))
					txtPassword.Password = "";
				else
					txtPassword.Password = aes.Decrypt(profile.Password);
			}
		}

		private void SaveProfiles()
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			if (profile != null)
				_profiles.DefaultProfile = profile.Name;
			string path = App.AppDataPath;
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			path += "\\Profiles.dat";

			using (FileStream fs = new FileStream(path, FileMode.Create))
			{
				_profiles.ToStream(fs);
			}
		}

		private void CreateJumpTasks()
		{
			_profiles.CreateJumpTasks(_jumpList);
		}

		private void LoadProfiles()
		{
			string path = App.AppDataPath + "\\Profiles.dat";

			using (FileStream fs = new FileStream(path, FileMode.Open))
			{
				_profiles = ProfileCollection.FromStream(fs);
			}
		}

		private void btnRenameProfile_Click(object sender, RoutedEventArgs e)
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			if (profile != null)
			{
				RenameProfile form = new RenameProfile(profile);
				form.Owner = this;
				if (form.ShowDialog() == true)
				{
					SaveProfiles();
					_profiles.CreateJumpTasks(_jumpList);
				}

			}
		}

		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			SaveProfiles();
		}

		private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			if (profile != null)
			{
				SimpleAES aes = new SimpleAES();
				profile.Password = aes.Encrypt(txtPassword.Password);
				TextBox_LostFocus(sender, e);
			}
		}

		private void btnAddProfile_Click(object sender, RoutedEventArgs e)
		{
			Profile profile = new Profile();
			RenameProfile form = new RenameProfile(profile);
			form.Owner = this;
			form.Title = "Add Profile";
			if (form.ShowDialog() == true)
			{
				_profiles.Add(profile);
				cmbProfiles.SelectedItem = profile;
			}
		}

		private void btnDeleteProfile_Click(object sender, RoutedEventArgs e)
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			if (MessageBox.Show("Are you sure you want to delete this profile?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				_profiles.Remove(profile);
				if (_profiles.Count == 0)
				{
					_profiles.Add(new Profile()
					{
						Name = "Default"
					});
				}
				cmbProfiles.SelectedIndex = 0;
				SaveProfiles();
				_profiles.CreateJumpTasks(_jumpList);
			}
		}

		private void lstPrograms_DragEnter(object sender, DragEventArgs e)
		{

		}

		private void lstPrograms_DragOver(object sender, DragEventArgs e)
		{
			e.Effects = DragDropEffects.None;
			if (e.Data.GetDataPresent("FileDrop", true))
			{
				string[] data = (string[])e.Data.GetData("FileDrop");
				bool accept = data.Any(x => x.ToLower().EndsWith(".lnk") || x.ToLower().EndsWith(".exe"));
				e.Effects = accept ? DragDropEffects.Link : DragDropEffects.None;
			}
			e.Handled = true;
		}

		private void lstPrograms_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("FileDrop", true))
			{
				string[] datas = (string[])e.Data.GetData("FileDrop");
				foreach (string data in datas)
				{
					if (data.ToLower().EndsWith(".lnk") || data.ToLower().EndsWith(".exe"))
					{
						AddProgram(data);
					}
				}
				e.Handled = true;
			}
		}
	}
}
