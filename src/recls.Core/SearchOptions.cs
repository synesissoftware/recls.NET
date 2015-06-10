
namespace Recls
{
	using System;

	/// <summary>
	///  Constants that control the enumeration of file-system entries.
	/// </summary>
	/// <remarks>
	///  <para>
	///   If neither <see cref="Files"/> nor <see cref="Directories"/> is
	///   specified, then <see cref="Files"/> is assumed.
	///  </para>
	///  
	///  <para>
	///   If <see cref="MarkDirectories"/> is specified, the
	///   <see cref="IEntry.Path"/>,
	///   <see cref="IEntry.SearchRelativePath"/>
	///   and
	///   <see cref="IEntry.File"/>
	///   properties include a trailing slash character.
	///  </para>
	/// </remarks>
	/// 
	/// <seealso cref="Recls.FileSearcher.DepthFirst.Search(String, String, SearchOptions, int)">DepthFirst.Search(String, String, SearchOptions, int)</seealso>
	/// <seealso cref="Recls.FileSearcher.BreadthFirst.Search(String, String, SearchOptions, int)">BreadthFirst.Search(String, String, SearchOptions, int)</seealso>
	/// <seealso cref="Recls.FileSearcher.Search(String, String, SearchOptions, int)">Search(String, String, SearchOptions, int)</seealso>
	[Flags]

	public enum SearchOptions : int
	{
		/// <summary>
		///  No options specified.
		/// </summary>
		None							=	0x00000000,
		/// <summary>
		///  Include files in search. Included by default if neither
		///  <see cref="Files"/> nor <see cref="Directories"/> specified.
		/// </summary>
		Files							=	0x00000001,
		/// <summary>
		///  Include directories in search.
		/// </summary>
		Directories						=	0x00000002,
		/// <summary>
		///  Causes the search to ignore any inaccessible nodes. This is
		///  ignored if an error handler is specified.
		/// </summary>
		IgnoreInaccessibleNodes			=	0x00100000,
		/// <summary>
		///  Marks directory entries with a trailing slash. The
		///  <see cref="IEntry.Path"/>,
		///  <see cref="IEntry.SearchRelativePath"/>,
		///  and
		///  <see cref="IEntry.File"/>,
		///  properties of directory entries
		///  are marked with a trailing back-slash.
		/// </summary>
		///
		/// <seealso cref="IEntry.Path"/>
		/// <seealso cref="IEntry.SearchRelativePath"/>
		/// <seealso cref="IEntry.File"/>
		MarkDirectories					=	0x00200000,
		/// <summary>
		///  Includes hidden files/directories in the search processing and
		///  results.
		/// </summary>
		IncludeHidden					=	0x00000100,
		/// <summary>
		///  Includes system files/directories in the search processing and
		///  results.
		/// </summary>
		IncludeSystem					=	0x00000200,

		/// <summary>
		///  Unless specified, platform-specific path separators -
		///  <c>':'</c> on UNIX; <c>';'</c> on Windows - will be recognised,
		///  and translated into <c>'|'</c> before processing.
		/// </summary>
		DoNotTranslatePathSeparators	=	0x00002000,
	}
}
