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
using System.Windows.Shapes;

namespace RunAsDotNet
{
	/// <summary>
	/// Interaction logic for RenameProfile.xaml
	/// </summary>
	public partial class RenameProfile : Window
	{
		Profile _profile;
		public RenameProfile(Profile profile)
		{
			InitializeComponent();
			_profile = profile;
			txtProfileName.Text = profile.Name;
			txtProfileName.Focus();
			txtProfileName.SelectAll();
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtProfileName.Text))
			{
				MessageBox.Show("A profile name is required");
				return;
			}
			_profile.Name = txtProfileName.Text;
			DialogResult = true;
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}
