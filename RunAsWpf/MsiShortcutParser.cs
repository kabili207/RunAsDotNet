using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Wsh = IWshRuntimeLibrary;

namespace RunAsDotNet
{
	// This class was downloaded and modified from the source available at
	// http://www.geektieguy.com/2007/11/19/how-to-parse-special-lnk-files-aka-msi-shortcuts-aka-windows-installer-advertised-shortcuts-using-c/
	public class ShortcutParser
	{
		/*
		UINT MsiGetShortcutTarget(
			LPCTSTR szShortcutTarget,
			LPTSTR szProductCode,
			LPTSTR szFeatureId,
			LPTSTR szComponentCode
		);
		*/
		[DllImport("msi.dll", CharSet = CharSet.Auto)]
		static extern uint MsiGetShortcutTarget(string targetFile, [Out] StringBuilder productCode,
			[Out] StringBuilder featureID, [Out] StringBuilder componentCode);

		public enum InstallState
		{
			NotUsed = -7,
			BadConfig = -6,
			Incomplete = -5,
			SourceAbsent = -4,
			MoreData = -3,
			InvalidArg = -2,
			Unknown = -1,
			Broken = 0,
			Advertised = 1,
			Removed = 1,
			Absent = 2,
			Local = 3,
			Source = 4,
			Default = 5
		}

		public const int MaxFeatureLength = 38;
		public const int MaxGuidLength = 38;
		public const int MaxPathLength = 1024;

		/*
		INSTALLSTATE MsiGetComponentPath(
		  LPCTSTR szProduct,
		  LPCTSTR szComponent,
		  LPTSTR lpPathBuf,
		  DWORD* pcchBuf
		);
		*/
		[DllImport("msi.dll", CharSet = CharSet.Auto)]
		static extern InstallState MsiGetComponentPath(string productCode, string componentCode, 
			[Out] StringBuilder componentPath, ref int componentPathBufferSize);

		/// <summary>
		/// Parses the shortcut specifed and returns the path to the executable it references
		/// </summary>
		/// <param name="file">The path of the shortcut</param>
		/// <returns>
		/// The path of the executable or <c>null</c> if the shortcut could not be parsed
		/// </returns>
		public static string Parse(string file)
		{
			StringBuilder product = new StringBuilder(MaxGuidLength + 1);
			StringBuilder feature = new StringBuilder(MaxFeatureLength + 1);
			StringBuilder component = new StringBuilder(MaxGuidLength + 1);

			MsiGetShortcutTarget(file, product, feature, component);

			int pathLength = MaxPathLength;
			StringBuilder path = new StringBuilder(pathLength);

			InstallState installState = MsiGetComponentPath(product.ToString(), component.ToString(), path, ref pathLength);
			if (installState == InstallState.Local)
			{
				return path.ToString();
			}
			else
			{
				Wsh.WshShell shell = new Wsh.WshShell(); //Create a new WshShell Interface
				Wsh.IWshShortcut link = (Wsh.IWshShortcut)shell.CreateShortcut(file);
				if (link != null)
					return link.TargetPath; //Link the interface to our shortcut
				return null;
			}
		}
	}
}
