
namespace Recls
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	/// <summary>
	///  Represents a file-system entry.
	/// </summary>
	/// <remarks>
	///  This interface is used to provide information regarding a
	///  file-system entry - file or directory - that has been
	///  enumerated via the
	///  <see cref="Recls.FileSearcher.Search(String, String, SearchOptions, int)">FileSearcher.Search()</see>
	///  (or
	///  <see cref="Recls.FileSearcher.BreadthFirst.Search(String, String, SearchOptions, int)">FileSearcher.BreadthFirst.Search()</see>
	///  or
	///  <see cref="Recls.FileSearcher.DepthFirst.Search(String, String, SearchOptions, int)">FileSearcher.DepthFirst.Search()</see>)
	///  methods.
	/// </remarks>
	public interface IEntry
	{
		/// <summary>
		///  Gets the full path of the entry.
		/// </summary>
		/// <value>
		///  The full path of the entry.
		/// </value>
		/// <remarks>
		///  If <see cref="SearchOptions"/>.<see cref="SearchOptions.MarkDirectories"/>
		///  is specified, the returned value includes a trailing slash
		///  character.
		/// </remarks>
		string Path { get; }

		/// <summary>
		///  Gets the search relative path.
		/// </summary>
		/// <value>
		///  The path of the entry relative to the search directory.
		/// </value>
		/// <remarks>
		///  If <see cref="SearchOptions"/>.<see cref="SearchOptions.MarkDirectories"/>
		///  is specified, the returned value includes a trailing slash
		///  character.
		/// </remarks>
		string SearchRelativePath { get; }

		/// <summary>
		///  Gets the entry's drive.
		/// </summary>
		/// <value>
		///  The drive part of the entry's path, which may be either a
		///  volume (e.g. <c>H:</c>)
		///  or a Universal Naming
		///  Convention (UNC) share (e.g. <c>\\SERVER\share</c>).
		/// </value>
		string Drive { get; }

		/// <summary>
		///  Gets the full path of the entry's directory.
		/// </summary>
		/// <value>
		///  The directory + drive of the entry's path.
		/// </value>
		/// <remarks>
		///  Always contains a trailing path name separator.
		/// </remarks>
		/// 
		/// <seealso cref="Directory"/>
		string DirectoryPath { get; }

		/// <summary>
		///  Gets the entry's directory.
		/// </summary>
		/// <value>
		///  The directory of the entry's path, excluding
		///  the <see cref="Drive"/>.
		/// </value>
		/// <remarks>
		///  Always contains a trailing path name separator.
		/// </remarks>
		/// 
		/// <seealso cref="DirectoryPath"/>
		string Directory { get; }

		/// <summary>
		///  Gets the search directory.
		/// </summary>
		/// <value>
		///  The directory from which the search was conducted.
		/// </value>
		string SearchDirectory { get; }

		/// <summary>
		///  Gets the UNC drive component of the entry's path.
		/// </summary>
		/// <value>
		///  The Universal Naming Convention (UNC) share, or
		///  <b>null</b> if the path begins with a volume drive.
		/// </value>
		/// <remarks>
		///  Consider the following examples:
		/// </remarks>
		/// <example>
		///  <code>
		///   IEntry entry = FileSearch.Stat(@"\\myserver\myshare\dir1");
		///   
		///   Debug.Assert(@"\\myserver\myshare\" == entry.UncDrive);
		///  </code>
		/// </example>
		/// <example>
		///  <code>
		///   IEntry entry = FileSearch.Stat(@"H:\dir1");
		///   
		///   Debug.Assert(null == entry.UncDrive);
		///  </code>
		/// </example>
		/// 
		/// <seealso cref="Drive"/>
		string UncDrive { get; }

		/// <summary>
		///  Gets the full file name of the entry's path.
		/// </summary>
		/// <value>
		///  The file (file name + extension) part of the entry's path.
		/// </value>
		/// <remarks>
		///  If <see cref="SearchOptions"/>.<see cref="SearchOptions.MarkDirectories"/>
		///  is specified, the returned value includes a trailing slash
		///  character.
		/// </remarks>
		/// 
		/// <seealso cref="FileName"/>
		/// <seealso cref="FileExtension"/>
		string File { get; }

		/// <summary>
		///  Gets the file name without extension.
		/// </summary>
		/// <value>
		///  The file part of the entry's path without extension.
		/// </value>
		/// 
		/// <seealso cref="File"/>
		/// <seealso cref="FileExtension"/>
		string FileName { get; }

		/// <summary>
		///  Gets the file extension.
		/// </summary>
		/// <value>
		///  The file extension of the entry's path.
		/// </value>
		/// 
		/// <seealso cref="File"/>
		/// <seealso cref="FileName"/>
		string FileExtension { get; }

		/// <summary>
		///  Gets the entry's creation time.
		/// </summary>
		/// <value>
		///  The time that the entry was created.
		/// </value>
		DateTime CreationTime { get; }

		/// <summary>
		///  Gets the entry's modification time.
		/// </summary>
		/// <value>
		///  The time that the entry was last modified.
		/// </value>
		DateTime ModificationTime { get; }

		/// <summary>
		///  Gets the entry's last access time.
		/// </summary>
		/// <value>
		///  The time that the entry was last accessed.
		/// </value>
		DateTime LastAccessTime { get; }

		/// <summary>
		///  Gets the entry's modification time.
		/// </summary>
		/// <value>
		///  The time that the entry was last modified.
		/// </value>
		DateTime LastStatusChangeTime { get; }

		/// <summary>
		///  Gets the entry's size.
		/// </summary>
		/// <value>
		///  The size of the entry if it is a file; <c>0</c> if it is a
		///  directory.
		/// </value>
		/// <remarks>
		///  Users should not use a size of 0 to determine whether an entry
		///  is a file or a directory, since some files may have zero size.
		///  Use <see cref="Recls.IEntry.IsDirectory"/> instead.
		/// </remarks>
		/// 
		/// <seealso cref="Recls.IEntry.IsDirectory"/>
		long Size { get; }

		/// <summary>
		///  Gets the entry's attributes.
		/// </summary>
		/// <value>
		///  The <see cref="System.IO.FileAttributes">attributes</see> of the
		///  entry.
		/// </value>
		/// <remarks>
		///  This property gets the attributes of the entry at the time of
		///  enumeration. To get the current settings, use
		///  <see cref="System.IO.File.GetAttributes">System.IO.File.GetAttributes</see>.
		/// </remarks>
		FileAttributes Attributes { get; }

		/// <summary>
		///  Indicates whether the entry is read only.
		/// </summary>
		/// <value>
		///  <b>true</b> if the entry is read only; <b>false</b> otherwise.
		/// </value>
		/// <remarks>
		///  This value is always <b>false</b> for directories. To get more
		///  specific information about a directory's characteristics, use
		///  the <see cref="Recls.IEntry.Attributes"/>
		/// </remarks>
		bool IsReadOnly { get; }

		/// <summary>
		///  Indicates whether the entry is a directory.
		/// </summary>
		/// <value>
		///  <b>true</b> if the entry is a directory; <b>false</b>
		///  otherwise.
		/// </value>
		bool IsDirectory { get; }

		/// <summary>
		///  Indicates whether the entry path is UNC.
		/// </summary>
		/// <value>
		///  <b>true</b> if the entry's drive is a Universal Naming
		///  Convention (UNC) share; <b>false</b> otherwise
		/// </value>
		bool IsUnc { get; }

		/// <summary>
		///  Gets all parts of the entry's
		///  <see cref="DirectoryPath">directory path</see>.
		/// </summary>
		/// <value>
		///  An array of all parts of the directory.
		/// </value>
		/// <remarks>
		///  Each element of the directory parts array has a trailing
		///  separator. For example, if the path is
		///  <c>C:\Windows\System32\user32.dll</c>, then the directory parts
		///  array will consist of three elements:
		///  <c>\</c>, <c>Windows\</c> and <c>System32\</c>
		/// </remarks>
#if USE_ARRAY_FOR_DIRECTORY_PARTS
		string[] DirectoryParts { get; }
#elif USE_ILIST_FOR_DIRECTORY_PARTS
		IList<string> DirectoryParts { get; }
#else
		IDirectoryParts DirectoryParts { get; }
#endif
	}
}
