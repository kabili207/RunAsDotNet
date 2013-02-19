using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;

namespace RunAsDotNet
{
	public class Win32
	{
		#region "CONTS"

		const UInt32 INFINITE = 0xFFFFFFFF;
		const UInt32 WAIT_FAILED = 0xFFFFFFFF;

		#endregion

		#region "ENUMS"
		
		[Flags]
		public enum CreationFlags
		{
			CREATE_SUSPENDED = 0x00000004,
			CREATE_NEW_CONSOLE = 0x00000010,
			CREATE_NEW_PROCESS_GROUP = 0x00000200,
			CREATE_UNICODE_ENVIRONMENT = 0x00000400,
			CREATE_SEPARATE_WOW_VDM = 0x00000800,
			CREATE_DEFAULT_ERROR_MODE = 0x04000000,
		}

		[Flags]
		public enum LogonFlags
		{
			None = 0,
			/// <summary>
			/// Log on, then load the user profile in the HKEY_USERS registry key.
			/// The function returns after the profile is loaded.
			/// </summary>
			/// <remarks>
			/// Loading the profile can be time-consuming, so it is best to use this value
			/// only if you must access the information in the HKEY_CURRENT_USER registry key. 
			/// <para>
			/// Windows Server 2003: 
			/// The profile is unloaded after the new process is terminated, whether or not
			/// it has created child processes.
			/// </para>
			/// <para>
			/// Windows XP:
			/// The profile is unloaded after the new process and all child processes it has
			/// created are terminated.
			/// </para>
			/// </remarks>
			WithProfile = 0x00000001,
			/// <summary>
			/// Log on, but use the specified credentials on the network only.
			/// </summary>
			/// <remarks>
			/// <para>
			/// The new process uses the same token as the caller, but the system creates a new
			/// logon session within LSA, and the process uses the specified credentials as the
			/// default credentials.
			/// </para>
			/// <para>
			/// This value can be used to create a process that uses a different set of
			/// credentials locally than it does remotely. This is useful in inter-domain scenarios
			/// where there is no trust relationship.
			/// </para>
			/// <para>
			/// The system does not validate the specified credentials. Therefore, the process can
			/// start, but it may not have access to network resources.
			/// </para>
			/// </remarks>
			NetCredentialsOnly = 0x00000002
		}

		#endregion

		#region "STRUCTS"

		[StructLayout(LayoutKind.Sequential)]
		public struct STARTUPINFO
		{
			public Int32 cb;
			public String lpReserved;
			public String lpDesktop;
			public String lpTitle;
			public Int32 dwX;
			public Int32 dwY;
			public Int32 dwXSize;
			public Int32 dwYSize;
			public Int32 dwXCountChars;
			public Int32 dwYCountChars;
			public Int32 dwFillAttribute;
			public Int32 dwFlags;
			public Int16 wShowWindow;
			public Int16 cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PROCESS_INFORMATION
		{
			public IntPtr hProcess;
			public IntPtr hThread;
			public Int32 dwProcessId;
			public Int32 dwThreadId;
		}

		#endregion

		#region "FUNCTIONS (P/INVOKE)"
		
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern Boolean CreateProcessWithLogonW
		(
			String lpszUsername,
			String lpszDomain,
			String lpszPassword,
			Int32 dwLogonFlags,
			String applicationName,
			String commandLine,
			Int32 creationFlags,
			IntPtr environment,
			String currentDirectory,
			ref STARTUPINFO sui,
			out PROCESS_INFORMATION processInfo
		);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern UInt32 WaitForSingleObject
		(
			IntPtr hHandle,
			UInt32 dwMilliseconds
		);

		[DllImport("kernel32", SetLastError = true)]
		public static extern Boolean CloseHandle(IntPtr handle);

		[DllImport("shell32.dll")]
		public static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner,
		   [Out] StringBuilder lpszPath, int nFolder, bool fCreate);

		#endregion

		#region "FUNCTIONS"

		/// <summary>
		/// Launches a command using the specified credentials
		/// </summary>
		/// <param name="strCommand">The command string to run</param>
		/// <param name="strDomain">The domain name to use</param>
		/// <param name="strName">The username to use</param>
		/// <param name="strPassword">The password to use</param>
		public static void LaunchCommand(string strCommand, string strDomain, string strName, string strPassword)
		{
			LaunchCommand(strCommand, strDomain, strName, strPassword, (LogonFlags)0);
		}

		/// <summary>
		/// Launches a command using the specified credentials and <see cref="LogonFlags"/>
		/// </summary>
		/// <param name="strCommand">The command string to run</param>
		/// <param name="strDomain">The domain name to use</param>
		/// <param name="strName">The username to use</param>
		/// <param name="strPassword">The password to use</param>
		/// <param name="logonType">The flags to use</param>
		public static void LaunchCommand(string strCommand, string strDomain, string strName, string strPassword, LogonFlags logonType)
		{
			// Variables
			PROCESS_INFORMATION processInfo = new PROCESS_INFORMATION();
			STARTUPINFO startInfo = new STARTUPINFO();
			bool bResult = false;

			try
			{
				// Create process
				startInfo.cb = Marshal.SizeOf(startInfo);

				bResult = CreateProcessWithLogonW(
					strName,
					strDomain,
					strPassword,
					(int)logonType, // Logon flags
					null,
					strCommand,
					0, // Creation flags
					IntPtr.Zero,
					null,
					ref startInfo,
					out processInfo
				);
				if (!bResult)
				{
					throw new Exception("CreateProcessWithLogonW error #" + Marshal.GetLastWin32Error().ToString());
				}

			}
			finally
			{
				// Close all handles
				CloseHandle(processInfo.hProcess);
				CloseHandle(processInfo.hThread);
			}
		}
		
		/// <summary>
		/// Gets the path of the start menu
		/// </summary>
		/// <returns></returns>
		public static string GetStartMenuPath()
		{
			return GetSpecialFolderPath(Csidl.CommonPrograms);
		}

		/// <summary>
		/// Gets the path of the specified <paramref name="folder"/>
		/// </summary>
		/// <param name="folder">The folder to get</param>
		/// <returns></returns>
		public static string GetSpecialFolderPath(Csidl folder)
		{
			StringBuilder path = new StringBuilder(260);
			SHGetSpecialFolderPath(IntPtr.Zero, path, (int)folder, false);
			return path.ToString();
		}

		#endregion
	}
}