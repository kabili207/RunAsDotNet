using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;

namespace RunAsDotNet
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static string AppDataPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
					+ "\\RunAsDotNet";
			}
		}

		protected override void OnStartup(StartupEventArgs e)
		{

			bool doShutDown = false;
			if (e.Args != null && e.Args.Count() > 1)
			{
				try
				{
					string sProfile = e.Args[0];
					string sPath = e.Args[1];

					string path = App.AppDataPath + "\\Profiles.dat";
					ProfileCollection profiles = null;
					using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
					{
						profiles = ProfileCollection.FromStream(fs);
					}
					if (profiles != null)
					{
						Profile profile = profiles.GetByName(sProfile);
						if (profile != null)
						{
							ProgramEntry entry = profile.Entries.GetByPath(sPath);
							if (entry == null)
							{
								profile.LaunchProgram(sPath);
							}
							else
							{
								profile.LaunchProgram(entry);
							}
							doShutDown = true;
						}
					}
					if (doShutDown)
					{
						using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
						{
							profiles.ToStream(fs);
						}
						profiles.CreateJumpTasks(new System.Windows.Shell.JumpList());
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error launching application: " + ex.Message);
					doShutDown = true;
				}
			}

			if (doShutDown)
			{
				Shutdown(1);
				return;
			}
			else
			{
				this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
			}

			base.OnStartup(e);
		}
	}
}
