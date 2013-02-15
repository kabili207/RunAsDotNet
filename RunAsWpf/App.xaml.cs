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
				string sProfile = e.Args[0];
				string sPath = e.Args[1];

				string path = App.AppDataPath + "\\Profiles.dat";

				using (FileStream fs = new FileStream(path, FileMode.Open))
				{
					ProfileCollection profiles = ProfileCollection.FromStream(fs);
					Profile profile = profiles.FirstOrDefault(x => x.Name == sProfile);
					if (profile != null)
					{
						profile.LaunchProgram(sPath);
						doShutDown = true;
					}
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
