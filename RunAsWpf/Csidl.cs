using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunAsDotNet
{
	/// <summary>
	/// CSIDL (constant special item ID list) values provide a unique system-independent way
	/// to identify special folders used frequently by applications, but which may not have
	/// the same name or location on any given system. For example, the system folder may
	/// be "C:\Windows" on one system and "C:\Winnt" on another.
	/// </summary>
	[Flags]
	public enum Csidl : int
	{
		/// <summary>&lt;desktop&gt;</summary>
		Desktop = 0x0000,
		/// <summary>Internet Explorer (icon on desktop)</summary>
		Internet = 0x0001,
		/// <summary>Start Menu\Programs</summary>
		Programs = 0x0002,
		/// <summary>My Computer\Control Panel</summary>
		Controls = 0x0003,
		/// <summary>My Computer\Printers</summary>
		Printers = 0x0004,
		/// <summary>Another name for "My Documents"</summary>
		Personal = 0x0005,
		/// <summary>&lt;user name>\Favorites</summary>
		Favorites = 0x0006,
		/// <summary>Start Menu\Programs\Startup</summary>
		StartUp = 0x0007,
		/// <summary>&lt;user name&gt;\Recent</summary>
		Recent = 0x0008,
		/// <summary>&lt;user name&gt;\SendTo</summary>
		SendTo = 0x0009,
		/// <summary>&lt;desktop&gt;\Recycle Bin</summary>
		BitBucket = 0x000a,
		/// <summary>&lt;user name&gt;\Start Menu</summary>
		StartMenu = 0x000b,
		/// <summary>"My Documents"</summary>
		MyDocuments = Personal,
		/// <summary>"My Music" folder</summary>
		MyMusic = 0x000d,
		/// <summary>"My Videos" folder</summary>
		MyVideo = 0x000e,
		/// <summary>&lt;user name&gt;\Desktop</summary>
		DesktopDirectory = 0x0010,
		/// <summary>My Computer</summary>
		Drives = 0x0011,
		/// <summary>Network Neighborhood (My Network Places)</summary>
		Network = 0x0012,
		/// <summary>&lt;user name&gt;\nethood</summary>
		NetHood = 0x0013,
		/// <summary>windows\fonts</summary>
		Fonts = 0x0014,
		/// <summary></summary>
		Templates = 0x0015,
		/// <summary>All Users\Start Menu</summary>
		CommonStartMenu = 0x0016,
		/// <summary>All Users\Start Menu\Programs</summary>
		CommonPrograms = 0x0017,
		/// <summary>All Users\Startup</summary>
		CommonStartUp = 0x0018,
		/// <summary>All Users\Desktop</summary>
		CommonDesktopDirectory = 0x0019,
		/// <summary>&lt;user name&gt;\Application Data</summary>
		AppData = 0x001a,
		/// <summary>&lt;user name&gt;\PrintHood</summary>
		PrintHood = 0x001b,
		/// <summary>&lt;user name&gt;\Local Settings\Applicaiton Data (non roaming)</summary>
		LocalAppData = 0x001c,
		/// <summary>non localized startup</summary>
		AltStartUp = 0x001d,
		/// <summary>non localized common startup</summary>
		CommonAltStartUp = 0x001e,
		/// <summary></summary>
		CommonFavorites = 0x001f,
		/// <summary></summary>
		InternetCache = 0x0020,
		/// <summary></summary>
		Cookies = 0x0021,
		/// <summary></summary>
		History = 0x0022,
		/// <summary>All Users\Application Data</summary>
		CommonAppData = 0x0023,
		/// <summary>GetWindowsDirectory()</summary>
		Windows = 0x0024,
		/// <summary>GetSystemDirectory()</summary>
		System = 0x0025,
		/// <summary>C:\Program Files</summary>
		ProgramFiles = 0x0026,
		/// <summary>"My Pictures" folder</summary>
		MyPictures = 0x0027,
		/// <summary>%USERPROFILE%</summary>
		Profile = 0x0028,
		/// <summary>x86 system directory on RISC</summary>
		SystemX86 = 0x0029,
		/// <summary>x86 C:\Program Files on RISC</summary>
		ProgramFilesX86 = 0x002a,
		/// <summary>C:\Program Files\Common</summary>
		ProgramFilesCommon = 0x002b,
		/// <summary>x86 Program Files\Common on RISC</summary>
		ProgramFilesCommonX86 = 0x002c,
		/// <summary>All Users\Templates</summary>
		CommonTemplates = 0x002d,
		/// <summary>All Users\Documents</summary>
		CommonDocuments = 0x002e,
		/// <summary>All Users\Start Menu\Programs\Administrative Tools</summary>
		CommonAdminTools = 0x002f,
		/// <summary>&lt;user name&gt;\Start Menu\Programs\Administrative Tools</summary>
		AdminTools = 0x0030,
		/// <summary>Network and Dial-up Connections</summary>
		Connections = 0x0031,
		/// <summary>All Users\My Music</summary>
		CommonMusic = 0x0035,
		/// <summary>All Users\My Pictures</summary>
		CommonPictures = 0x0036,
		/// <summary>All Users\My Video</summary>
		CommonVideo = 0x0037,
		/// <summary>Resource Direcotry</summary>
		Resources = 0x0038,
		/// <summary>Localized Resource Direcotry</summary>
		ResourcesLocalized = 0x0039,
		/// <summary>Links to All Users OEM specific apps</summary>
		CommonOemLinks = 0x003a,
		/// <summary>%USERPROFILE%\Local Settings\Application Data\Microsoft\CD Burning</summary>
		CDBurnArea = 0x003b,
		/// <summary>Computers Near Me (computered from Workgroup membership)</summary>
		ComputersNearMe = 0x003d,
		/// <summary>Combine with another CSIDL to force the creation of the associated folder if it does not exist.</summary>
		FlagCreate = 0x8000,
		/// <summary>Combine with another CSIDL constant, except for CSIDL_FLAG_CREATE, to return an unverified folder path with no attempt to create or initialize the folder.</summary>
		FlagDontVerify = 0x4000,
		/// <summary>Combine with another CSIDL constant to ensure the expansion of environment variables.</summary>
		FlagDontUnexpand = 0x2000,
		/// <summary>Combine with another CSIDL constant to ensure the retrieval of the true system path for the folder, free of any aliased placeholders such as %USERPROFILE%, returned by SHGetFolderLocation. This flag has no effect on paths returned by SHGetFolderPath.</summary>
		FlagNoAlias = 0x1000,
		/// <summary>combine with CSIDL_ value to indicate per-user init (eg. upgrade)</summary>
		FlagPerUserInit = 0x0800
	}
}
