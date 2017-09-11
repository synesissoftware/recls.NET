
/* /////////////////////////////////////////////////////////////////////////
 * File:        Internal/Util.cs
 *
 * Created:     5th June 2009
 * Updated:     20th June 2017
 *
 * Home:        http://recls.net/
 *
 * Copyright (c) 2009-2017, Matthew Wilson and Synesis Software
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 * - Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * - Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 * - Neither the name(s) of Matthew Wilson and Synesis Software nor the
 *   names of any contributors may be used to endorse or promote products
 *   derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
 * IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * ////////////////////////////////////////////////////////////////////// */


namespace Recls.Internal
{
	using Recls.Exceptions;

	// TODO: sort this, probably using global::

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	//using System.IO;
	using System.Text;

	using DirectoryInfo = System.IO.DirectoryInfo;
	using DirectoryNotFoundException = System.IO.DirectoryNotFoundException;
	using File = System.IO.File;
	using FileAttributes = System.IO.FileAttributes;
	using FileInfo = System.IO.FileInfo;
	using FileNotFoundException = System.IO.FileNotFoundException;
	using FileSystemInfo = System.IO.FileSystemInfo;
	using IOException = System.IO.IOException;

#if PSEUDO_UNIX
	internal static class Path
	{
		public static string GetFullPath(string path)
		{
			return Util.ConvertPathToPseudoUNIX(System.IO.Path.GetFullPath(path));
		}
		public static string GetDirectoryName(string path)
		{
			return Util.ConvertPathToPseudoUNIX(System.IO.Path.GetDirectoryName(path));
		}
		public static string GetFileName(string path)
		{
			return Util.ConvertPathToPseudoUNIX(System.IO.Path.GetFileName(path));
		}
	}
#else // PSEUDO_UNIX
	using Path = System.IO.Path;
#endif // PSEUDO_UNIX

	internal enum PathRelativity
	{
		/// <summary>
		///  Path is null or empty.
		/// </summary>
		None,
		/// <summary>
		///  Path contains invalid characters.
		/// </summary>
		Invalid,
		/// <summary>
		///  Is relative.
		/// </summary>
		Relative,
		/// <summary>
		///  Is an absolute path that is rooted.
		/// </summary>
		AbsoluteDrive,
		/// <summary>
		///  Is an absolute path that is a UNC drive.
		/// </summary>
		AbsoluteUnc,
		/// <summary>
		///  Is a rooted path, beginning with a path name separator.
		/// </summary>
		Rooted,
	}


	// TODO: Split this file, to separate:
	//
	// - Operating System independent constants
	// - Operating System dependent constants
	// - search validation functions
	// - search functions (including Stat)
	// - path functions
	internal static class Util
	{
		#region constants
		internal static readonly char[] WildcardCharacters = { '?', '*' };
		internal static readonly char[] PathNameSeparatorCharacters = PathNameSeparatorCharacters_;
		internal static readonly bool IsPathComparisonCaseSensitive = IsPathComparisonCaseSensitive_;
		internal static readonly string WildcardsAll = WildcardsAll_;
		internal static readonly char[] StrictPathSeparatorCharacters = new char[] { '|' };
		internal static readonly char[] PathSeparatorCharacters = PathSeparatorCharacters_;
		internal static StringComparison StringComparison = StringComparison_;
		internal static readonly bool HasDrives = HasDrives_;

		private static StringComparison StringComparison_
		{
			get { return IsPathComparisonCaseSensitive ? StringComparison.CurrentCulture : StringComparison.OrdinalIgnoreCase; }
		}

		private static bool IsPathComparisonCaseSensitive_
		{
			get
			{
				switch(Api.DeducedOperatingSystem.Platform)
				{
					case PlatformID.Unix:
					case PlatformID.MacOSX:
					default:
						return true;
					case PlatformID.Win32NT:
					case PlatformID.Win32S:
					case PlatformID.Win32Windows:
					case PlatformID.WinCE:
					case PlatformID.Xbox:
						return false;
				}
			}
		}

		private static string WildcardsAll_
		{
			get
			{
				switch(Api.DeducedOperatingSystem.Platform)
				{
					case PlatformID.Unix:
					case PlatformID.MacOSX:
					default:
						return "*";
					case PlatformID.Win32NT:
					case PlatformID.Win32S:
					case PlatformID.Win32Windows:
					case PlatformID.WinCE:
					case PlatformID.Xbox:
						return "*.*";
				}
			}
		}

		private static char[] PathNameSeparatorCharacters_
		{
			get
			{
				switch(Api.DeducedOperatingSystem.Platform)
				{
					case PlatformID.Unix:
					case PlatformID.MacOSX:
					default:
#if PSEUDO_UNIX
						return new char[] { '/', '\\' };
#else
						return new char[] { '/' };
#endif // PSEUDO_UNIX
					case PlatformID.Win32NT:
					case PlatformID.Win32S:
					case PlatformID.Win32Windows:
					case PlatformID.WinCE:
					case PlatformID.Xbox:
						return new char[] { '\\', '/' };
				}
			}
		}

		private static char[] PathSeparatorCharacters_
		{
			get
			{
				switch(Api.DeducedOperatingSystem.Platform)
				{
					case PlatformID.Unix:
					case PlatformID.MacOSX:
					default:
						return new char[] { '|', ':' };
					case PlatformID.Win32NT:
					case PlatformID.Win32S:
					case PlatformID.Win32Windows:
					case PlatformID.WinCE:
					case PlatformID.Xbox:
						return new char[] { '|', ';' };
				}
			}
		}

		private static bool HasDrives_
		{
			get
			{
				switch(Api.DeducedOperatingSystem.Platform)
				{
					case PlatformID.Unix:
					case PlatformID.MacOSX:
					default:
						return false;
					case PlatformID.Win32NT:
					case PlatformID.Win32S:
					case PlatformID.Win32Windows:
					case PlatformID.WinCE:
					case PlatformID.Xbox:
						return true;
				}
			}
		}
		#endregion

		#region search validation operations
		internal static bool consume_options_(SearchOptions options)
		{
			return 0 == options;
		}

		private static bool EntryMatchesOptions(FileSystemInfo info, SearchOptions options)
		{
			Debug.Assert(0 != (options & (SearchOptions.Files | SearchOptions.Directories)));

			if(0 == (info.Attributes & FileAttributes.Directory))
			{
				// File
				if(0 == (options & SearchOptions.Files))
				{
					return false;
				}
			}
			else
			{
				// Directory
				if(0 == (options & SearchOptions.Directories))
				{
					return false;
				}
			}

			if(0 != (info.Attributes & FileAttributes.Hidden))
			{
				// Hidden
				if(0 == (options & SearchOptions.IncludeHidden))
				{
					return false;
				}
			}

			if(0 != (info.Attributes & FileAttributes.System))
			{
				// System
				if(0 == (options & SearchOptions.IncludeSystem))
				{
					return false;
				}
			}

			return true;
		}

		internal static string ValidateDirectory(string directory, SearchOptions options)
		{
			consume_options_(options);

			if(String.IsNullOrEmpty(directory))
			{
				directory = null;
			}

			if(null == directory)
			{
				directory = ".";
			}

			directory = GetFullPath(directory);

			return EnsureDirEnd(directory);
		}

		internal static string ValidatePatterns(string patterns, SearchOptions options)
		{
			if(null == patterns)
			{
				patterns = WildcardsAll;
			}
			else
			{
				if(0 == (options & SearchOptions.DoNotTranslatePathSeparators))
				{
					patterns = patterns.Replace(PathSeparatorCharacters[1], PathSeparatorCharacters[0]);
				}
			}

			return patterns;
		}

		internal static SearchOptions ValidateOptions(SearchOptions options)
		{
			if(0 == ((SearchOptions.Files | SearchOptions.Directories) & options))
			{
				options |= SearchOptions.Files;
			}

			return options;
		}

		internal static IExceptionHandler ValidateExceptionHandler(IExceptionHandler exceptionHandler, SearchOptions options)
		{
			if(null == exceptionHandler)
			{
				if(0 != (options & SearchOptions.IgnoreInaccessibleNodes))
				{
					exceptionHandler = new IgnoreAllExceptionHandler();
				}
				else
				{
					exceptionHandler = new FailAllExceptionHandler();
				}
			}

			return exceptionHandler;
		}

		internal static IProgressHandler ValidateProgressHandler(IProgressHandler progressHandler, SearchOptions options)
		{
			consume_options_(options);

			if(null == progressHandler)
			{
				progressHandler = new ContinueAllProgressHandler();
			}

			return progressHandler;
		}

		internal static IExceptionHandler GetDelegateHandler(OnException exceptionHandler)
		{
			return (null == exceptionHandler) ? null : new DelegateExceptionHandler(exceptionHandler);
		}

		internal static IProgressHandler GetDelegateProgress(OnProgress progressHandler)
		{
			return (null == progressHandler) ? null : new DelegateProgressHandler(progressHandler);
		}
		#endregion

		#region search operations
		internal static FileSystemInfo[] GetEntriesByPatterns(IExceptionHandler exceptionHandler, DirectoryInfo di, Patterns patterns, SearchOptions options)
		{
			Debug.Assert(null != exceptionHandler);
			Debug.Assert(null != patterns);

			FileSystemInfo[] entries = null;

			try
			{
				entries = di.GetFileSystemInfos();
			}
			catch(OutOfMemoryException)
			{
				throw;
			}
			catch(Exception x)
			{
				if(ExceptionHandlerResult.ConsumeExceptionAndContinue == exceptionHandler.OnException(di.FullName, x))
				{
					return new FileSystemInfo[0];
				}
				else
				{
					throw;
				}
			}

			List<FileSystemInfo> newEntries = new List<FileSystemInfo>(entries.Length);

			foreach(FileSystemInfo fi in entries)
			{
				if(EntryMatchesOptions(fi, options))
				{
					if(patterns.MatchPath(fi.Name))
					{
						newEntries.Add(fi);
					}
				}
			}

			return newEntries.ToArray();
		}

		internal static DirectoryInfo[] GetSubdirectories(IExceptionHandler exceptionHandler, DirectoryInfo di, SearchOptions options)
		{
			Debug.Assert(null != exceptionHandler);

			options = (options & ~SearchOptions.Files) | SearchOptions.Directories;

			DirectoryInfo[] directories = null;

			try
			{
				directories = di.GetDirectories();
			}
			catch(OutOfMemoryException)
			{
				throw;
			}
			catch(Exception x)
			{
				if(ExceptionHandlerResult.ConsumeExceptionAndContinue == exceptionHandler.OnException(di.FullName, x))
				{
					return new DirectoryInfo[0];
				}
				else
				{
					throw;
				}
			}

			List<DirectoryInfo> newDirectories = new List<DirectoryInfo>(directories.Length);

			foreach(DirectoryInfo di2 in directories)
			{
				if(EntryMatchesOptions(di2, options))
				{
					newDirectories.Add(di2);
				}
			}

			return newDirectories.ToArray();
		}
		#endregion

		#region path elicitation operations
		// <summary>
		//	Evaluates the UNC drive part of a given path.
		// </summary>
		// <param name="path">
		//	The path to be tested.
		// </param>
		// <returns>
		//	<b>-1</b> if the path does not contain a UNC drive; the length
		//	of the UNC drive part of the given path otherwise.
		// </returns>
		internal static int GetUncDriveLength(string path)
		{
			Debug.Assert(null != path, "path cannot be null");

			// Check that it's leading with "\\", and then look
			// for the end of the server via another '\', and
			// for either '\' or '/' or '\0'

			if(path.Length >= 5)
			{
				if( '\\' == path[0] &&
					'\\' == path[1])
				{
					int sep0 = path.IndexOf('\\', 2);

					if(sep0 >= 0)
					{
						int sep1 = path.IndexOfAny(Util.PathNameSeparatorCharacters, sep0 + 1);

						if(sep1 >= 0)
						{
							return sep1;
						}
						else
						{
							if(sep0 + 1 == path.Length)
							{
								return -1;
							}
							else
							{
								return path.Length;
							}
						}
					}
				}
			}

			return -1;
		}

#if UNUSED
		// <summary>
		//	Evaluates the UNC drive+directory part of a given path
		// </summary>
		// <param name="path">
		//	The path to be tested
		// </param>
		// <returns>
		//	<b>-1</b> if the path does not contain a UNC drive/directory;
		//	the length of the UNC drive+directory part of the given path
		//	otherwise.
		// </returns>
		internal static int GetUncDirectoryPathLength(string path)
		{
			Debug.Assert(null != path, "path cannot be null");

			int driveLen = GetUncDriveLength(path);

			if(driveLen < 0)
			{
				return driveLen;
			}
			else if(path.Length < driveLen)
			{
				return -1;
			}
			else if(path.Length == driveLen)
			{
				return driveLen;
			}
			{
				Debug.Assert('/' == path[driveLen] || '\\' == path[driveLen]);

				int sep = path.IndexOfAny(PathNameSeparatorCharacters, driveLen + 1);

				if(sep >= 0)
				{
					return sep;
				}
				else
				{
					return path.Length;
				}
			}
		}
#endif

		internal static bool IsBadUncPath(string path)
		{
			if(null != path)
			{
				if(path.Length >= 2)
				{
					if( '\\' == path[0] &&
						'\\' == path[1])
					{
						if(GetUncDriveLength(path) < 0)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		internal static bool IsVolumeBasedPath(string path)
		{
			if( path.Length >= 2 &&
				Char.IsLetter(path[0]) &&
				':' == path[1])
			{
				return true;
			}

			return false;
		}
		#endregion

		#region path manipulation operations
		// <summary>
		//	Doesn't correct "\\server\share", without fixing a directory
		//	
		//	Doesn't remove the 
		// </summary>
		// <param name="path"></param>
		// <returns></returns>
		internal static string GetFullPath(string path)
#if PSEUDO_UNIX
		{
			return ConvertPathToPseudoUNIX(GetFullPath_nonUNIX_(path));
		}
		internal static string GetFullPath_nonUNIX_(string path)
#endif // PSEUDO_UNIX
		{
			if(!IsPathAbsolute(path))
			{
				path = Path.GetFullPath(path);
			}

			int n = GetUncDriveLength(path);

			if( n == path.Length ||
				n + 1 == path.Length)
			{
				// Special case of "\\server\share", which must
				// have a trailing separator to be a
				// meaningful path

				return EnsureDirEnd(path);
			}
			else if(IsVolumeBasedPath(path) &&
					3 == path.Length)
			{
				return path;
			}
			else
			{
				return RemoveDirEnd(path);
			}
		}

		// <summary>
		//	System.IO.Path.GetDirectoryName() returns null for a root
		//	directory - whether Volume-based or UNC - so we need to return
		//	the root directory.
		//	
		//	It also throws if passed "" - though the documentation
		//	contradicts this - whereas we want null back. 
		// </summary>
		// <param name="path"></param>
		// <returns></returns>
		internal static string GetDirectoryPath(string path)
#if PSEUDO_UNIX
		{
			return ConvertPathToPseudoUNIX(GetDirectoryPath_nonUNIX_(path));
		}
		internal static string GetDirectoryPath_nonUNIX_(string path)
#endif // PSEUDO_UNIX
		{
			if(null == path)
			{
				throw new ArgumentNullException("path");
			}
			if(0 == path.Length)
			{
				throw new ArgumentException("invalid path", "path");
			}
			if(IsBadUncPath(path))
			{
				throw new IllformedUncPathException("path is invalid", path);
			}

			string dir = Path.GetDirectoryName(path);

			if(null == dir)
			{
				dir = path;
			}

			return EnsureDirEnd(dir);
		}

		// <summary>
		//	System.IO.Path.GetFileName() incorrectly handles paths with
		//	only a UNC drive - e.g. \\server\share - and so we need to
		//	handle this for it
		// </summary>
		// <param name="path"></param>
		// <returns></returns>
		internal static string GetFile(string path)
#if PSEUDO_UNIX
		{
			return ConvertPathToPseudoUNIX(GetFile_nonUNIX_(path));
		}
		internal static string GetFile_nonUNIX_(string path)
#endif // PSEUDO_UNIX
		{
			if(String.IsNullOrEmpty(path))
			{
				return path;
			}
			else
			{
				if(IsBadUncPath(path))
				{
					throw new IllformedUncPathException("path is invalid", path);
				}

				int len = GetUncDriveLength(path);

				if(len < path.Length)
				{
					return Path.GetFileName(path);
				}
				else
				{
					return "";
				}
			}
		}

		internal static string GetDrive(string path)
#if PSEUDO_UNIX
		{
			return ConvertPathToPseudoUNIX(GetDrive_nonUNIX_(path));
		}
		internal static string GetDrive_nonUNIX_(string path)
#endif // PSEUDO_UNIX
		{
			if(null == path)
			{
				throw new ArgumentNullException("path");
			}
			if(0 == path.Length)
			{
				throw new ArgumentException("empty path", "path");
			}
			if(!IsPathAbsolute(path))
			{
				throw new ArgumentException("path is not absolute", "path");
			}
			if(IsBadUncPath(path))
			{
				throw new IllformedUncPathException("path is invalid", path);
			}

			int driveLen = Util.GetUncDriveLength(path);

			if(driveLen < 0)
			{
				driveLen = 2;
			}

			return path.Substring(0, driveLen);
		}

		// <summary>
		//	Canonicalises a path
		// </summary>
		// <param name="path">
		//	The path to canonicalise
		// </param>
		// <returns></returns>
		internal static string CanonicalisePath(string path)
#if PSEUDO_UNIX
		{
			return ConvertPathToPseudoUNIX(CanonicalisePath_nonUNIX_(path));
		}
		internal static string CanonicalisePath_nonUNIX_(string path)
#endif // PSEUDO_UNIX
		{
			if(String.IsNullOrEmpty(path))
			{
				return path;
			}
			else if(IsBadUncPath(path))
			{
				throw new IllformedUncPathException("path is invalid", path);
			}
			else
			{
				int n = path.IndexOfAny(PathNameSeparatorCharacters);

				if(n < 0)
				{
					return path;
				}
				else
				{
					int u = GetUncDriveLength(path);
					string[] parts;

					if(u > 2)
					{
						return path;
					}

					if(u < 0)
					{
						parts = path.Split(PathNameSeparatorCharacters, StringSplitOptions.RemoveEmptyEntries);
					}
					else
					{
						List<string> splits = new List<string>();

						splits.Add(path.Substring(0, u));
						splits.AddRange(path.Substring(u).Split(PathNameSeparatorCharacters, StringSplitOptions.RemoveEmptyEntries));

						parts = splits.ToArray();
					}

					if(parts.Length < 2)
					{
						if( 0 == parts.Length ||
							IsPathRooted(path))
						{
							if(PathNameSeparatorCharacters.Length > 1)
							{
								return path.Replace(PathNameSeparatorCharacters[1], PathNameSeparatorCharacters[0]);
							}
							else
							{
								return path;
							}
						}
						else
						{
							return parts[0];
						}
					}
					else
					{
						List<string> results = new List<string>();

						for(int i = 0; i != parts.Length; ++i)
						{
							if("." == parts[i])
							{
								; // Ignore
							}
							else if(".." == parts[i] && i != 0)
							{
								results.RemoveAt(results.Count - 1);
							}
							else
							{
								results.Add(parts[i]);
							}
						}

						StringBuilder sb = new StringBuilder(200);

						if(u >= 0)
						{
							sb.Append(path.Substring(0, u));
						}
						foreach(string result in results)
						{
							if(sb.Length > 0)
							{
								sb.Append(PathNameSeparatorCharacters[0]);
							}
							sb.Append(result);
						}

						return sb.ToString();
					}
				}
			}
		}

		internal static bool HasDirEnd(string path)
		{
			Debug.Assert(null != path);

			if(0 == path.Length)
			{
				return false;
			}
			else
			{
				char last = path[path.Length - 1];

				foreach(char ch in PathNameSeparatorCharacters)
				{
					if(last == ch)
					{
						return true;
					}
				}

				return false;
			}
		}

		internal static string EnsureDirEnd(string path)
		{
			Debug.Assert(null != path);

			if( 0 == path.Length ||
				HasDirEnd(path))
			{
				return path;
			}
			else
			{
				return path + PathNameSeparatorCharacters[0];
			}
		}

		internal static string RemoveDirEnd(string path)
		{
			Debug.Assert(null != path);

			if(!HasDirEnd(path))
			{
				return path;
			}
			else
			{
				return path.Substring(0, path.Length - 1);
			}
		}
		#endregion

		#region path test functions
		private static bool IsLatinLetter(char ch)
		{
			switch(ch)
			{
				case 'A':
				case 'B':
				case 'C':
				case 'D':
				case 'E':
				case 'F':
				case 'G':
				case 'H':
				case 'I':
				case 'J':
				case 'K':
				case 'L':
				case 'M':
				case 'N':
				case 'O':
				case 'P':
				case 'R':
				case 'S':
				case 'T':
				case 'U':
				case 'V':
				case 'X':
				case 'Y':
				case 'Z':
				case 'a':
				case 'b':
				case 'c':
				case 'd':
				case 'e':
				case 'f':
				case 'g':
				case 'h':
				case 'i':
				case 'j':
				case 'k':
				case 'l':
				case 'm':
				case 'n':
				case 'o':
				case 'p':
				case 'r':
				case 's':
				case 't':
				case 'u':
				case 'v':
				case 'x':
				case 'y':
				case 'z':
					return true;
				default:
					return false;
			}
		}

		internal static PathRelativity GetPathRelativity(string path)
		{
			if(String.IsNullOrEmpty(path))
			{
				return PathRelativity.None;
			}
			if(path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
			{
				return PathRelativity.Invalid;
			}
			if(path.Length > 2)
			{
				if( '\\' == path[0] &&
					'\\' == path[1])
				{
					return PathRelativity.AbsoluteUnc;
				}
				if( ':' == path[1] &&
					IsLatinLetter(path[0]))
				{
					foreach(char ch in PathNameSeparatorCharacters)
					{
						if(ch == path[2])
						{
							return PathRelativity.AbsoluteDrive;
						}
					}
				}
			}
			foreach(char ch in PathNameSeparatorCharacters)
			{
				if(ch == path[0])
				{
					return PathRelativity.Rooted;
				}
			}

			return PathRelativity.Relative;
		}

		internal static bool IsPathAbsolute(string path)
		{
			// An absolute path must begin with a drive, or a slash (of one
			// direction or another

			switch(GetPathRelativity(path))
			{
				case PathRelativity.AbsoluteDrive:
				case PathRelativity.AbsoluteUnc:
					return true;
				case PathRelativity.Rooted:
					return !HasDrives;
				default:
					return false;
			}
		}

		internal static bool IsPathRooted(string path)
		{
			switch(GetPathRelativity(path))
			{
				case PathRelativity.AbsoluteDrive:
				case PathRelativity.AbsoluteUnc:
				case PathRelativity.Rooted:
					return true;
				default:
					return false;
			}
		}
		#endregion

		#region stat operations
		internal static IEntry Stat(string path, bool verifiesThatItExists)
		{
			try
			{
				// To process the path, we need to canonicalise it:
				//
				// 1. Handle null/empty
				// 2. Make full path
				// 3. Canonicalise
				// 4. Append path-name-separator if directory

				// 1. Handle null/empty
				if(String.IsNullOrEmpty(path))
				{
					path = Environment.CurrentDirectory;

					verifiesThatItExists = true;
				}

				// 2. Make full path
				path = GetFullPath(path);

				// 3. Canonicalise
				path = CanonicalisePath(path);

				// 4. Append path-name-separator if directory
				string directory = GetDirectoryPath(path);

				FileAttributes attributes;

				if(verifiesThatItExists)
				{
					attributes = File.GetAttributes(path);
				}
				else
				{
					if(HasDirEnd(path))
					{
						attributes = FileAttributes.Directory;
					}
					else
					{
						attributes = FileAttributes.Normal;
					}
				}

				if(0 != (FileAttributes.Directory & attributes))
				{
					DirectoryInfo info = new DirectoryInfo(path);

					return new DirectoryEntry(info, directory, SearchOptions.None, null);
				}
				else
				{
					FileInfo info = new FileInfo(path);

					return new FileEntry(info, directory, SearchOptions.None, null);
				}
			}
			catch(DirectoryNotFoundException)
			{
				return null;
			}
			catch(FileNotFoundException)
			{
				return null;
			}
			catch(IOException x)
			{
				// When a network path cannot be found, the stupid FCL
				// throws an instance of IOException. Unhelpful!
				//
				// We need to be able to detect whether the IOException is
				// for bad network path, or for some other reason
				//
				// There are two strategies:
				//
				// 1. Check that the message string is the same as that
				//	obtained from FormatMessage() for the error code
				//	0x80070035
				// 2. Use reflection to look into the protected HResult
				//	property, to see if it is 0x80070035

				// 1.
				System.ComponentModel.Win32Exception w32x = new System.ComponentModel.Win32Exception(unchecked((int)0x80070035));

				string m1 = w32x.Message.Trim().Trim('.');
				string m2 = x.Message.Trim().Trim('.');

				if(0 == String.CompareOrdinal(m1, m2))
				{
					return null;
				}

				throw;
			}
		}

		internal static string DeriveRelativePath(string origin, string target)
		{
			if(String.IsNullOrEmpty(origin))
			{
				return target;
			}
			else if(String.IsNullOrEmpty(target))
			{
				return origin;
			}
			else if(IsBadUncPath(origin))
			{
				throw new IllformedUncPathException("origin is invalid", origin);
			}
			else if(IsBadUncPath(target))
			{
				throw new IllformedUncPathException("target is invalid", target);
			}
			else if(0 == String.Compare(origin, target, StringComparison))
			{
				//if(HasDirEnd(origin))
				//{
				//	  return String.Format(".{0}", PathNameSeparatorCharacters[0]);
				//}
				//else
				//{
					return ".";
				//}
			}
			else
			{
				IEntry	entryOrigin = Stat(origin, false);
				IEntry	entryTarget = Stat(target, false);

				if(0 != String.Compare(entryTarget.Drive, entryOrigin.Drive, StringComparison))
				{
					return target;
				}
				else
				{
					string[]	allPartsOrigin	=	CalculateAllParts(entryOrigin.Path);
					string[]	allPartsTarget	=	CalculateAllParts(entryTarget.Path);
					int 		numSameDirParts =	CalculateNumSameDirectoryParts(allPartsOrigin, allPartsTarget);

					StringBuilder sb = new StringBuilder(200);

					for(int i = numSameDirParts; i != allPartsOrigin.Length; ++i)
					{
						if(0 != sb.Length)
						{
							sb.Append(PathNameSeparatorCharacters[0]);
						}
						sb.Append(@"..");
					}
					for(int i = numSameDirParts; i != allPartsTarget.Length; ++i)
					{
						if(0 != sb.Length)
						{
							sb.Append(PathNameSeparatorCharacters[0]);
						}
						sb.Append(allPartsTarget[i]);
					}

					return sb.ToString();
				}
			}
		}

		private static string[] CalculateAllParts(string path)
		{
			return path.Split(PathNameSeparatorCharacters, StringSplitOptions.RemoveEmptyEntries);
		}

#if UNUSED
		private static bool PathContains(string path1, string path2)
		{
			Debug.Assert(null != path1);
			Debug.Assert(null != path2);

			if(path1.Length < path2.Length)
			{
				return false;
			}
			else
			{
				string t = path1.Substring(0, path2.Length);

				return 0 == String.Compare(t, path2, StringComparison);
			}
		}
#endif

		private static int CalculateNumSameDirectoryParts(string[] partsOrigin, string[] partsTarget)
		{
			Debug.Assert(null != partsOrigin);
			Debug.Assert(null != partsTarget);

			int i = 0;

			for(; i != partsOrigin.Length && i != partsTarget.Length; ++i)
			{
				if(0 != String.Compare(partsOrigin[i], partsTarget[i], StringComparison))
				{
					break;
				}
			}

			return i;
		}

#if PSEUDO_UNIX
		internal static string ConvertPathToPseudoUNIX(string path)
		{
			if(String.IsNullOrEmpty(path))
			{
				return path;
			}
			int n = GetUncDriveLength(path);

			if(n < 0)
			{
				path = path.Replace('\\', '/');
			}
			else
			{
				path = path.Substring(0, n) + path.Substring(n).Replace('\\', '/');
			}

			return path;
		}
#endif // PSEUDO_UNIX

		#endregion
	}
}

/* ///////////////////////////// end of file //////////////////////////// */

