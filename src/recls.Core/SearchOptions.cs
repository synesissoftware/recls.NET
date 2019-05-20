
/* /////////////////////////////////////////////////////////////////////////
 * File:        SearchOptions.cs
 *
 * Created:     30th June 2009
 * Updated:     20th May 2019
 *
 * Home:        http://recls.net/
 *
 * Copyright (c) 2009-2019, Matthew Wilson and Synesis Software
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
		Directories 					=	0x00000002,
		/// <summary>
		///  Causes the search to ignore any inaccessible nodes. This is
		///  ignored if an error handler is specified.
		/// </summary>
		IgnoreInaccessibleNodes 		=	0x00100000,
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
		MarkDirectories 				=	0x00200000,
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

		/// <summary>
		///  [Version 2+] Unless specified, the directory in a search will
		///  be verified at the creation of a search and locked until the
		///  search is disposed.
		/// </summary>
		DoNotLockDirectory				=	0x00400000,

		/// <summary>
		///  [Version 2+] If specified, a missing search directory will
		///  not result in a thrown exception, but will instead be
		///  treated as if it is empty, yielding an enumerator with no
		///  results.
		/// </summary>
		/// <remarks>
		///  Specifying this flag has two effects: (i) the effects of
		///  <see cref="DoNotLockDirectory"/> are effectively ignored; and (ii)
		///  the check is done at the creation of the search, and if the directory
		///  is caused to exist between the creation of the search and the
		///  enumeration of its first element, the caller will still see an empty
		///  results set.
		/// </remarks>
		TreatMissingDirectoryAsEmpty    =   0x00800000,

		/// <summary>
		///  .
		/// </summary>
		/// <remarks>
		///  <b>NOTE</b>: Only used by
		///  <see cref="Recls.Api.Stat(System.String, SearchOptions)"/>.
		/// </remarks>
		StatInfoForNonexistentPath      =   0x00080000,
	}
}

/* ///////////////////////////// end of file //////////////////////////// */

