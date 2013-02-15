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
			LOGON_WITH_PROFILE = 0x00000001,
			LOGON_NETCREDENTIALS_ONLY = 0x00000002
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
		static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner,
		   [Out] StringBuilder lpszPath, int nFolder, bool fCreate);

		#endregion

		#region "FUNCTIONS"

		public static void LaunchCommand(string strCommand, string strDomain, string strName, string strPassword)
		{
			LaunchCommand(strCommand, strDomain, strName, strPassword, (LogonFlags)0);
		}

		public static void LaunchCommand(string strCommand, string strDomain, string strName, string strPassword, LogonFlags logonType)
		{
			// Variables
			PROCESS_INFORMATION processInfo = new PROCESS_INFORMATION();
			STARTUPINFO startInfo = new STARTUPINFO();
			bool bResult = false;
			UInt32 uiResultWait = WAIT_FAILED;

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

				// Wait for process to end
				/*uiResultWait = WaitForSingleObject(processInfo.hProcess, INFINITE);
				if (uiResultWait == WAIT_FAILED)
				{
					throw new Exception("WaitForSingleObject error #" + Marshal.GetLastWin32Error());
				}*/

			}
			finally
			{
				// Close all handles
				CloseHandle(processInfo.hProcess);
				CloseHandle(processInfo.hThread);
			}
		}
		
		public static string GetStartMenuPath()
		{
			StringBuilder path = new StringBuilder(260);
			SHGetSpecialFolderPath(IntPtr.Zero, path, (int)Csidl.CommonPrograms, false);
			return path.ToString();
		}

		#endregion
	}
}