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
		/// <summary>
		/// The virtual folder that represents the Windows desktop, the root of the namespace.
		/// </summary>
		Desktop = 0x0000,
		/// <summary>
		/// A virtual folder for Internet Explorer.
		/// </summary>
		Internet = 0x0001,
		/// <summary>
		/// The file system directory that contains the user's program groups 
		/// (which are themselves file system directories). A typical path is 
		/// C:\Documents and Settings\username\Start Menu\Programs.
		/// </summary>
		Programs = 0x0002,
		/// <summary>
		/// The virtual folder that contains icons for the Control Panel applications.
		/// </summary>
		Controls = 0x0003,
		/// <summary>
		/// The virtual folder that contains installed printers.
		/// </summary>
		Printers = 0x0004,
		/// <summary>
		/// The virtual folder that represents the My Documents desktop item. This is equivalent 
		/// to <see cref="Csidl.MyDocuments"/>.
		/// </summary>
		/// <remarks>
		/// Previous to Version 6.0: The file system directory used to physically store a user's
		/// common repository of documents. A typical path is
		/// C:\Documents and Settings\username\My Documents. This should be distinguished from
		/// the virtual My Documents folder in the namespace.
		/// </remarks>
		Personal = 0x0005,
		/// <summary>
		/// The file system directory that serves as a common repository for the user's
		/// favorite items. A typical path is C:\Documents and Settings\username\Favorites.
		/// </summary>
		Favorites = 0x0006,
		/// <summary>
		/// The file system directory that corresponds to the user's Startup program
		/// group. The system starts these programs whenever any user logs on. A typical path
		/// is C:\Documents and Settings\username\Start Menu\Programs\Startup.
		/// </summary>
		StartUp = 0x0007,
		/// <summary>
		/// The file system directory that contains shortcuts to the user's most recently used
		/// documents. A typical path is C:\Documents and Settings\username\My Recent Documents.
		/// </summary>
		/// <remarks>To create a shortcut in this folder, use SHAddToRecentDocs. In addition to
		/// creating the shortcut, this function updates the Shell's list of recent documents
		/// and adds the shortcut to the My Recent Documents submenu of the Start menu.
		/// </remarks>
		Recent = 0x0008,
		/// <summary>
		/// The file system directory that contains Send To menu items. A typical path
		/// is C:\Documents and Settings\username\SendTo.
		/// </summary>
		SendTo = 0x0009,
		/// <summary>
		/// The virtual folder that contains the objects in the user's Recycle Bin.
		/// </summary>
		BitBucket = 0x000a,
		/// <summary>
		/// The file system directory that contains Start menu items. A typical path is
		/// C:\Documents and Settings\username\Start Menu.
		/// </summary>
		StartMenu = 0x000b,
		/// <summary>
		/// The virtual folder that represents the My Documents desktop item. This
		/// value is equivalent to <see cref="Csidl.Personal"/>.
		/// </summary>
		MyDocuments = Personal,
		/// <summary>
		/// The file system directory that serves as a common repository for music files.
		/// A typical path is C:\Documents and Settings\User\My Documents\My Music.
		/// </summary>
		MyMusic = 0x000d,
		/// <summary>
		/// The file system directory that serves as a common repository for video files.
		/// A typical path is C:\Documents and Settings\username\My Documents\My Videos.
		/// </summary>
		MyVideo = 0x000e,
		/// <summary>
		/// The file system directory used to physically store file objects on the
		/// desktop (not to be confused with the desktop folder itself). A typical path
		/// is C:\Documents and Settings\username\Desktop.
		/// </summary>
		DesktopDirectory = 0x0010,
		/// <summary>
		/// The virtual folder that represents My Computer, containing everything on the
		/// local computer: storage devices, printers, and Control Panel. The folder
		/// can also contain mapped network drives.
		/// </summary>
		Drives = 0x0011,
		/// <summary>
		/// A virtual folder that represents Network Neighborhood, the root of the network
		/// namespace hierarchy.
		/// </summary>
		Network = 0x0012,
		/// <summary>
		/// A file system directory that contains the link objects that may exist in the
		/// My Network Places virtual folder. It is not the same as <see cref="Csidl.Network"/>,
		/// which represents the network namespace root. A typical path is
		/// C:\Documents and Settings\username\NetHood.
		/// </summary>
		NetHood = 0x0013,
		/// <summary>
		/// A virtual folder that contains fonts. A typical path is C:\Windows\Fonts.
		/// </summary>
		Fonts = 0x0014,
		/// <summary>
		/// The file system directory that serves as a common repository for document templates.
		/// A typical path is C:\Documents and Settings\username\Templates.
		/// </summary>
		Templates = 0x0015,
		/// <summary>
		/// The file system directory that contains the programs and folders that appear on
		/// the Start menu for all users. A typical path is
		/// C:\Documents and Settings\All Users\Start Menu.
		/// </summary>
		CommonStartMenu = 0x0016,
		/// <summary>
		/// The file system directory that contains the directories for the common program groups
		/// that appear on the Start menu for all users. A typical path is
		/// C:\Documents and Settings\All Users\Start Menu\Programs.
		/// </summary>
		CommonPrograms = 0x0017,
		/// <summary>
		/// The file system directory that contains the programs that appear in the Startup folder
		/// for all users. A typical path is
		/// C:\Documents and Settings\All Users\Start Menu\Programs\Startup.
		/// </summary>
		CommonStartUp = 0x0018,
		/// <summary>
		/// The file system directory that contains files and folders that appear on the desktop
		/// for all users. A typical path is C:\Documents and Settings\All Users\Desktop.
		/// </summary>
		CommonDesktopDirectory = 0x0019,
		/// <summary>
		/// The file system directory that serves as a common repository for application-specific
		/// data. A typical path is C:\Documents and Settings\username\Application Data.
		/// </summary>
		AppData = 0x001a,
		/// <summary>
		/// The file system directory that contains the link objects that can exist in the Printers
		/// virtual folder. A typical path is C:\Documents and Settings\username\PrintHood.
		/// </summary>
		PrintHood = 0x001b,
		/// <summary>
		/// The file system directory that serves as a data repository for local (nonroaming) applications.
		/// A typical path is C:\Documents and Settings\username\Local Settings\Application Data.
		/// </summary>
		LocalAppData = 0x001c,
		/// <summary>
		/// The file system directory that corresponds to the user's nonlocalized Startup program group.
		/// This value is recognized in Windows Vista for backward compatibility, but the folder
		/// itself no longer exists.
		/// </summary>
		AltStartUp = 0x001d,
		/// <summary>
		/// The file system directory that corresponds to the nonlocalized Startup program group for
		/// all users. This value is recognized in Windows Vista for backward compatibility,
		/// but the folder itself no longer exists.
		/// </summary>
		CommonAltStartUp = 0x001e,
		/// <summary>
		/// The file system directory that serves as a common repository for favorite items common
		/// to all users.
		/// </summary>
		CommonFavorites = 0x001f,
		/// <summary>
		/// The file system directory that serves as a common repository for temporary Internet files.
		/// A typical path is C:\Documents and Settings\username\Local Settings\Temporary Internet Files.
		/// </summary>
		InternetCache = 0x0020,
		/// <summary>
		/// The file system directory that serves as a common repository for Internet cookies.
		/// A typical path is C:\Documents and Settings\username\Cookies.
		/// </summary>
		Cookies = 0x0021,
		/// <summary>
		/// The file system directory that serves as a common repository for Internet history items.
		/// </summary>
		History = 0x0022,
		/// <summary>
		/// The file system directory that contains application data for all users.
		/// A typical path is C:\Documents and Settings\All Users\Application Data.
		/// </summary>
		/// <remarks>
		/// This folder is used for application data that is not user specific.
		/// For example, an application can store a spell-check dictionary, a database
		/// of clip art, or a log file in the <see cref="Csidl.CommonAppData"/> folder.
		/// This information will not roam and is available to anyone using the computer.
		/// </remarks>
		CommonAppData = 0x0023,
		/// <summary>
		/// The Windows directory or SYSROOT. This corresponds to the %windir% or
		/// %SYSTEMROOT% environment variables. A typical path is C:\Windows.
		/// </summary>
		Windows = 0x0024,
		/// <summary>
		/// The Windows System folder. A typical path is C:\Windows\System32.
		/// </summary>
		System = 0x0025,
		/// <summary>
		/// The Program Files folder. A typical path is C:\Program Files.
		/// </summary>
		ProgramFiles = 0x0026,
		/// <summary>
		/// The file system directory that serves as a common repository for image files.
		/// A typical path is C:\Documents and Settings\username\My Documents\My Pictures.
		/// </summary>
		MyPictures = 0x0027,
		/// <summary>
		/// The user's profile folder. A typical path is C:\Users\username.
		/// </summary>
		/// <remarks>
		/// Applications should not create files or folders at this level; they should put
		/// their data under the locations referred to by <see cref="Csidl.AppData"/> or
		/// <see cref="Csidl.LocalAppData"/>. However, if you are creating a new Known Folder
		/// the profile root referred to by <see cref="Csidl.Profile"/> is appropriate.
		/// </remarks>
		Profile = 0x0028,
		/// <summary>
		/// The 32bit System directory when running on a 64bit machine.
		/// </summary>
		SystemX86 = 0x0029,
		/// <summary>
		/// The 32bit Program Files directory when running on a 64bit machine.
		/// A typical path is C:\Program Files (x86).
		/// </summary>
		ProgramFilesX86 = 0x002a,
		/// <summary>
		/// A folder for components that are shared across applications.
		/// A typical path is C:\Program Files\Common. Valid only for Windows XP.
		/// </summary>
		ProgramFilesCommon = 0x002b,
		/// <summary>
		/// The 32bit version of <see cref="Csidl.ProgramFilesCommon"/> when running on a 64bit machine.
		/// A typical path is C:\Program Files (x86)\Common.
		/// </summary>
		ProgramFilesCommonX86 = 0x002c,
		/// <summary>
		/// The file system directory that contains the templates that are available to all users.
		/// A typical path is C:\Documents and Settings\All Users\Templates.
		/// </summary>
		CommonTemplates = 0x002d,
		/// <summary>
		/// The file system directory that contains documents that are common to all users.
		/// A typical path is C:\Documents and Settings\All Users\Documents.
		/// </summary>
		CommonDocuments = 0x002e,
		/// <summary>
		/// The file system directory that contains administrative tools for all users of the computer.
		/// </summary>
		CommonAdminTools = 0x002f,
		/// <summary>
		/// The file system directory that is used to store administrative tools for an individual user.
		/// The MMC will save customized consoles to this directory, and it will roam with the user.
		/// </summary>
		AdminTools = 0x0030,
		/// <summary>
		/// The virtual folder that represents Network Connections, that contains network and
		/// dial-up connections.
		/// </summary>
		Connections = 0x0031,
		/// <summary>
		/// The file system directory that serves as a repository for music files common to all users.
		/// A typical path is C:\Documents and Settings\All Users\Documents\My Music.
		/// </summary>
		CommonMusic = 0x0035,
		/// <summary>
		/// The file system directory that serves as a repository for image files common to all users.
		/// A typical path is C:\Documents and Settings\All Users\Documents\My Pictures.
		/// </summary>
		CommonPictures = 0x0036,
		/// <summary>
		/// The file system directory that serves as a repository for video files common to all users.
		/// A typical path is C:\Documents and Settings\All Users\Documents\My Videos.
		/// </summary>
		CommonVideo = 0x0037,
		/// <summary>
		/// Windows Vista. The file system directory that contains resource data.
		/// A typical path is C:\Windows\Resources.
		/// </summary>
		Resources = 0x0038,
		/// <summary>
		/// Localized Resource Direcotry
		/// </summary>
		ResourcesLocalized = 0x0039,
		/// <summary>
		/// This value is recognized in Windows Vista for backward compatibility, but the
		/// folder itself is no longer used.
		/// </summary>
		CommonOemLinks = 0x003a,
		/// <summary>
		/// The file system directory that acts as a staging area for files waiting to be written to a CD.
		/// A typical path is
		/// C:\Documents and Settings\username\Local Settings\Application Data\Microsoft\CD Burning.
		/// </summary>
		CDBurnArea = 0x003b,
		/// <summary>
		/// The folder that represents other computers in your workgroup.
		/// </summary>
		ComputersNearMe = 0x003d,

		/*** FLAGS ***/
		/// <summary>
		/// Combine with another CSIDL to force the creation of the associated folder if it does not exist.
		/// </summary>
		FlagCreate = 0x8000,
		/// <summary>
		/// Combine with another CSIDL constant, except for <see cref="Csidl.FlagCreate"/>, to return
		/// an unverified folder path with no attempt to create or initialize the folder.
		/// </summary>
		FlagDontVerify = 0x4000,
		/// <summary>
		/// Combine with another CSIDL constant to ensure the expansion of environment variables.
		/// </summary>
		FlagDontUnexpand = 0x2000,
		/// <summary>
		/// Combine with another CSIDL constant to ensure the retrieval of the true system path
		/// for the folder, free of any aliased placeholders such as %USERPROFILE%, returned by
		/// SHGetFolderLocation. This flag has no effect on paths returned by SHGetFolderPath.
		/// </summary>
		FlagNoAlias = 0x1000,
		/// <summary>
		/// combine with CSIDL_ value to indicate per-user init (eg. upgrade)
		/// </summary>
		FlagPerUserInit = 0x0800
	}
}
