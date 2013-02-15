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
			} catch {}
			if (_profiles == null)
			{
				_profiles = new ProfileCollection();
				_profiles.Add(new Profile()
				{
					Name = "Default"
				});
			}
			cmbProfiles.ItemsSource = _profiles;
			//cmbProfiles.SelectedIndex = 0;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			cmbProfiles.SelectedIndex = 0;
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
				//ofd.Filter = "Solutions files (*.sln)|*.sln";
				ofd.DereferenceLinks = false;
				ofd.InitialDirectory = Win32.GetStartMenuPath();
				if (ofd.ShowDialog() == true)
				{

					ProgramEntry entry = new ProgramEntry();

					string realPath = ofd.FileName;
					FileInfo info = new FileInfo(realPath);
					if(info.Extension == ".lnk")
						realPath = MsiShortcutParser.ParseShortcut(ofd.FileName);
					entry.Path = realPath;
					entry.Name = ofd.SafeFileName;

					if (_programs.Count(x => x.Path == realPath) > 0)
					{
						MessageBox.Show("This program has already been added");
						return;
					}

					FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(realPath);

					if (!string.IsNullOrWhiteSpace(versionInfo.FileDescription))
						entry.Name = versionInfo.FileDescription;

					entry.SetImage(ProgramEntry.IconFromFilePath(realPath));
					_programs.Add(entry);
					lstPrograms.SelectedItem = entry;
					SaveProfiles();
					CreateJumpTasks();
				}
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
				string user = txtUserName.Text; //"anagle";
				string domain = txtDomain.Text; // "leeds_pdc";
				string password = txtPassword.Password; // "**********";
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
				txtPassword.Password = aes.Decrypt(profile.Password);
			}
		}

		private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
		{
			Profile profile = cmbProfiles.SelectedItem as Profile;
			if (profile != null)
			{
				SimpleAES aes = new SimpleAES();
				profile.Password = aes.Encrypt(txtPassword.Password);
				SaveProfiles();
			}
		}

		private void SaveProfiles()
		{
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
			_jumpList.JumpItems.Clear();
			foreach (Profile profile in _profiles)
			{
				foreach (ProgramEntry entry in profile.Entries)
				{
					// Configure a new JumpTask.
					JumpTask jumpTask1 = new JumpTask();
					// Get the path to Calculator and set the JumpTask properties.
					jumpTask1.ApplicationPath = System.Reflection.Assembly.GetEntryAssembly().Location;
					jumpTask1.IconResourcePath = entry.Path;
					jumpTask1.Title = entry.Name;
					//jumpTask1.Description = "Open Calculator.";
					jumpTask1.CustomCategory = profile.Name;
					jumpTask1.Arguments = string.Format("\"{0}\" \"{1}\"", profile.Name, entry.Path);
					_jumpList.JumpItems.Add(jumpTask1);
				}
			}
			_jumpList.Apply();
		}

		private void LoadProfiles()
		{
			string path = App.AppDataPath + "\\Profiles.dat";

			using (FileStream fs = new FileStream(path, FileMode.Open))
			{
				_profiles = ProfileCollection.FromStream(fs);
			}
		}
	}
}
