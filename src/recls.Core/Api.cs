
/* /////////////////////////////////////////////////////////////////////////
 * File:        Api.cs
 *
 * Created:     30th June 2009
 * Updated:     19th November 2017
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


namespace Recls
{
	using global::Recls.Internal;

	using global::System.Collections.Generic;

	/// <summary>
	///  Main API for <b>recls</b>
	/// </summary>
	public static class Api
	{
		#region constants

		#endregion

		#region properties

		/// <summary>
		///  A sentinel value that may be passed as the <c>depth</c>
		///  parameter to request a search of unrestricted depth.
		/// </summary>
		/// <remarks>
		///  <para>
		///   The current implementation defines this as the largest
		///   possible positive value of type <c>int</c>, on the
		///   assumption that no file-system will ever be able to
		///   provide paths with more than 2 billion directory parts.
		///  </para>
		///  <para>
		///   Notwithstanding, clients should use this read-only property,
		///   rather than specifying <c>int.MaxValue</c>, or any
		///   other large positive value, because the algorithm used
		///   to perform depth-limited search may change in a future
		///   version.
		///  </para>
		/// </remarks>
		public static int UnrestrictedDepth
		{
			get
			{
				return FileSearcher.UnrestrictedDepth;
			}
		}

		/// <summary>
		///  An array of platform-specific path name separator characters
		/// </summary>
		/// <remarks>
		///  On UNIX systems, this will comprise the character <c>'/'</c>;
		///  on Windows systems it will be <c>'\\'</c> and <c>'/'</c>.
		/// </remarks>
		public static char[] PathNameSeparatorCharacters
		{
			get
			{
				return Util.PathNameSeparatorCharacters;
			}
		}

		/// <summary>
		///  An array of platform-specific path separator characters
		/// </summary>
		/// <remarks>
		///  On UNIX systems, this will comprise the characters <c>':'</c>
		///  and <c>'|'</c>; on Windows systems it will be <c>';'</c> and
		///  <c>'|'</c>.
		/// </remarks>
		public static char[] PathSeparatorCharacters
		{
			get
			{
				return Util.PathSeparatorCharacters;
			}
		}

		/// <summary>
		///  The wildcard pattern specifying "all files/directories" for the
		///  given platform.
		/// </summary>
		/// <remarks>
		///  On UNIX systems, this will be <c>"*"</c>; on Windows systems it
		///  will be <c>"*.*"</c>.
		/// </remarks>
		public static string WildcardsAll
		{
			get
			{
				return Util.WildcardsAll;
			}
		}

		/// <summary>
		///  Gets an <see cref="System.OperatingSystem"/> object that
		///  represents the deduced operation system.
		/// </summary>
		/// <remarks>
		///  This property exists primarily for testing of the <b>recls</b>
		///  libraries. In all deployed scenarios it supplies exactly the
		///  same result as <see cref="System.Environment.OSVersion"/>
		/// </remarks>
		public static System.OperatingSystem DeducedOperatingSystem
		{
			get
			{
				System.OperatingSystem actualOS = System.Environment.OSVersion;

#if PSEUDO_UNIX
				return new System.OperatingSystem(System.PlatformID.Unix, actualOS.Version);
#else
				return actualOS;
#endif // PSEUDO_UNIX
			}
		}
		#endregion

		#region operations

		#region depth-first search operations

		/// <summary>
		///  Depth-first search operations.
		/// </summary>
		public static class DepthFirst
		{
			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  searched in a depth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns)
			{
				return Search(directory, patterns, 0, UnrestrictedDepth, null, (IExceptionHandler)null);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  searched in a depth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the
			///  search.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options)
			{
				return Search(directory, patterns, options, UnrestrictedDepth, null, (IExceptionHandler)null);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a depth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the
			///  search.
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive
			///  search.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth)
			{
				return Search(directory, patterns, options, depth, null, (IExceptionHandler)null);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  using the given <paramref name="progressHandler"/>
			///   and <paramref name="exceptionHandler"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a depth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive search.
			/// </param>
			/// <param name="progressHandler">
			///  A <see cref="IProgressHandler">progress handler</see> instance. 
			/// </param>
			/// <param name="exceptionHandler">
			///  An <see cref="IExceptionHandler">error handler</see> instance.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, IProgressHandler progressHandler, IExceptionHandler exceptionHandler)
			{
				return Search(directory, patterns, options, depth, progressHandler, exceptionHandler, null);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  using the given <paramref name="progressHandler"/>
			///   and <paramref name="exceptionHandler"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a depth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive search.
			/// </param>
			/// <param name="progressHandler">
			///  A <see cref="IProgressHandler">progress handler</see> instance. 
			/// </param>
			/// <param name="exceptionHandler">
			///  An <see cref="IExceptionHandler">error handler</see> instance.
			/// </param>
			/// <param name="context">
			///  Caller-supplied context value, that will be accessible on
			///  every search entry via the
			///  <see cref="Recls.IEntry.Context"/> property.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, IProgressHandler progressHandler, IExceptionHandler exceptionHandler, object context)
			{
				options = Util.ValidateOptions(options);
				directory = Util.ValidateDirectory(directory, options);
				patterns = Util.ValidatePatterns(patterns, options);
				exceptionHandler = Util.ValidateExceptionHandler(exceptionHandler, options);
				progressHandler = Util.ValidateProgressHandler(progressHandler, options);

				return new DepthFirstFileSearcher(directory, patterns, options, depth, exceptionHandler, progressHandler, context);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  using the given <paramref name="progressHandler"/>
			///   and <paramref name="exceptionHandler"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a depth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive search.
			/// </param>
			/// <param name="progressHandler">
			///  A <see cref="Recls.OnProgress">progress handler</see> delegate.
			/// </param>
			/// <param name="exceptionHandler">
			///  An <see cref="Recls.OnException">error handler</see> delegate.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, OnProgress progressHandler, OnException exceptionHandler)
			{
				return Search(directory, patterns, options, depth, Util.GetDelegateProgress(progressHandler), Util.GetDelegateHandler(exceptionHandler));
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  using the given <paramref name="progressHandler"/>
			///   and <paramref name="exceptionHandler"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a depth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive search.
			/// </param>
			/// <param name="progressHandler">
			///  A <see cref="Recls.OnProgress">progress handler</see> delegate.
			/// </param>
			/// <param name="exceptionHandler">
			///  An <see cref="Recls.OnException">error handler</see> delegate.
			/// </param>
			/// <param name="context">
			///  Caller-supplied context value, that will be accessible on
			///  every search entry via the
			///  <see cref="Recls.IEntry.Context"/> property.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, OnProgress progressHandler, OnException exceptionHandler, object context)
			{
				return Search(directory, patterns, options, depth, Util.GetDelegateProgress(progressHandler), Util.GetDelegateHandler(exceptionHandler), context);
			}
		}
		#endregion

		#region breadth-first search operations

		/// <summary>
		///  Breadth-first search operations.
		/// </summary>
		public static class BreadthFirst
		{
			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  searched in a breadth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns)
			{
				return Search(directory, patterns, 0, UnrestrictedDepth, null, (IExceptionHandler)null);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  searched in a breadth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options)
			{
				return Search(directory, patterns, options, UnrestrictedDepth, null, (IExceptionHandler)null);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a breadth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive search.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth)
			{
				return Search(directory, patterns, options, depth, null, (IExceptionHandler)null);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  using the given <paramref name="progressHandler"/>
			///   and <paramref name="exceptionHandler"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a breadth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive search.
			/// </param>
			/// <param name="progressHandler">
			///  A <see cref="IProgressHandler">progress handler</see> instance. 
			/// </param>
			/// <param name="exceptionHandler">
			///  An <see cref="IExceptionHandler">error handler</see> instance.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, IProgressHandler progressHandler, IExceptionHandler exceptionHandler)
			{
				return Search(directory, patterns, options, depth, progressHandler, exceptionHandler, null);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  using the given <paramref name="progressHandler"/>
			///   and <paramref name="exceptionHandler"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a breadth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive search.
			/// </param>
			/// <param name="progressHandler">
			///  A <see cref="IProgressHandler">progress handler</see> instance. 
			/// </param>
			/// <param name="exceptionHandler">
			///  An <see cref="IExceptionHandler">error handler</see> instance.
			/// </param>
			/// <param name="context">
			///  Caller-supplied context value, that will be accessible on
			///  every search entry via the
			///  <see cref="Recls.IEntry.Context"/> property.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, IProgressHandler progressHandler, IExceptionHandler exceptionHandler, object context)
			{
				options = Util.ValidateOptions(options);
				directory = Util.ValidateDirectory(directory, options);
				patterns = Util.ValidatePatterns(patterns, options);
				exceptionHandler = Util.ValidateExceptionHandler(exceptionHandler, options);
				progressHandler = Util.ValidateProgressHandler(progressHandler, options);

				return new BreadthFirstFileSearcher(directory, patterns, options, depth, exceptionHandler, progressHandler, context);
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  using the given <paramref name="progressHandler"/>
			///   and <paramref name="exceptionHandler"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a breadth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive search.
			/// </param>
			/// <param name="progressHandler">
			///  A <see cref="Recls.OnProgress">progress handler</see> delegate.
			/// </param>
			/// <param name="exceptionHandler">
			///  An <see cref="Recls.OnException">error handler</see> delegate.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, OnProgress progressHandler, OnException exceptionHandler)
			{
				return Search(directory, patterns, options, depth, Util.GetDelegateProgress(progressHandler), Util.GetDelegateHandler(exceptionHandler));
			}

			/// <summary>
			///  Returns an enumerable collection of <see cref="IEntry"/>
			///  instances representing all file-system entries
			///  under <paramref name="directory"/>
			///  matching the given <paramref name="patterns"/>,
			///  according to the given <paramref name="options"/>,
			///  using the given <paramref name="progressHandler"/>
			///   and <paramref name="exceptionHandler"/>,
			///  to the given maximum <paramref name="depth"/>,
			///  searched in a breadth-first manner.
			/// </summary>
			/// <param name="directory">
			///  The directory in which to search; the local directory if
			///  <b>null</b> or empty.
			/// </param>
			/// <param name="patterns">
			///  One or more search patterns, separated by the <c>|</c>
			///  character; searches for all if <b>null</b> or
			///  <see cref="FileSearcher.WildcardsAll"/>.
			/// </param>
			/// <param name="options">
			///  Combination of <see cref="SearchOptions"/> to moderate the search
			/// </param>
			/// <param name="depth">
			///  The maximum search depth; 0 to perform a non-recursive search.
			/// </param>
			/// <param name="progressHandler">
			///  A <see cref="Recls.OnProgress">progress handler</see> delegate.
			/// </param>
			/// <param name="exceptionHandler">
			///  An <see cref="Recls.OnException">error handler</see> delegate.
			/// </param>
			/// <param name="context">
			///  Caller-supplied context value, that will be accessible on
			///  every search entry via the
			///  <see cref="Recls.IEntry.Context"/> property.
			/// </param>
			/// <returns>
			///  An instance of a type exhibiting the
			///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
			///  interface.
			/// </returns>
			public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, OnProgress progressHandler, OnException exceptionHandler, object context)
			{
				return Search(directory, patterns, options, depth, Util.GetDelegateProgress(progressHandler), Util.GetDelegateHandler(exceptionHandler), context);
			}
		}
		#endregion

		#region priority-agnostic search operations

		/// <summary>
		///  Returns an enumerable collection of <see cref="IEntry"/>
		///  instances representing all file-system entries
		///  under <paramref name="directory"/>
		///  matching the given <paramref name="patterns"/>,
		///  searched in an implementation-defined manner.
		/// </summary>
		/// <param name="directory">
		///  The directory in which to search; the local directory if
		///  <b>null</b> or empty.
		/// </param>
		/// <param name="patterns">
		///  One or more search patterns, separated by the <c>|</c>
		///  character; searches for all if <b>null</b> or
		///  <see cref="FileSearcher.WildcardsAll"/>.
		/// </param>
		/// <returns>
		///  An instance of a type exhibiting the
		///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
		///  interface.
		/// </returns>
		public static IEnumerable<IEntry> Search(string directory, string patterns)
		{
			return DepthFirst.Search(directory, patterns);
		}
		/// <summary>
		///  Returns an enumerable collection of <see cref="IEntry"/>
		///  instances representing all file-system entries
		///  under <paramref name="directory"/>
		///  matching the given <paramref name="patterns"/>,
		///  according to the given <paramref name="options"/>,
		///  searched in an implementation-defined manner.
		/// </summary>
		/// <param name="directory">
		///  The directory in which to search; the local directory if
		///  <b>null</b> or empty.
		/// </param>
		/// <param name="patterns">
		///  One or more search patterns, separated by the <c>|</c>
		///  character; searches for all if <b>null</b> or
		///  <see cref="FileSearcher.WildcardsAll"/>.
		/// </param>
		/// <param name="options">
		///  Combination of <see cref="SearchOptions"/> to moderate the search
		/// </param>
		/// <returns>
		///  An instance of a type exhibiting the
		///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
		///  interface.
		/// </returns>
		public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options)
		{
			return DepthFirst.Search(directory, patterns, options);
		}

		/// <summary>
		///  Returns an enumerable collection of <see cref="IEntry"/>
		///  instances representing all file-system entries
		///  under <paramref name="directory"/>
		///  matching the given <paramref name="patterns"/>,
		///  according to the given <paramref name="options"/>,
		///  to the given maximum <paramref name="depth"/>,
		///  searched in an implementation-defined manner.
		/// </summary>
		/// <param name="directory">
		///  The directory in which to search; the local directory if
		///  <b>null</b> or empty.
		/// </param>
		/// <param name="patterns">
		///  One or more search patterns, separated by the <c>|</c>
		///  character; searches for all if <b>null</b> or
		///  <see cref="FileSearcher.WildcardsAll"/>.
		/// </param>
		/// <param name="options">
		///  Combination of <see cref="SearchOptions"/> to moderate the search
		/// </param>
		/// <param name="depth">
		///  The maximum search depth; 0 to perform a non-recursive search.
		/// </param>
		/// <returns>
		///  An instance of a type exhibiting the
		///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
		///  interface.
		/// </returns>
		public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth)
		{
			return DepthFirst.Search(directory, patterns, options, depth);
		}

		/// <summary>
		///  Returns an enumerable collection of <see cref="IEntry"/>
		///  instances representing all file-system entries
		///  under <paramref name="directory"/>
		///  matching the given <paramref name="patterns"/>,
		///  according to the given <paramref name="options"/>,
		///  using the given <paramref name="progressHandler"/>
		///   and <paramref name="exceptionHandler"/>,
		///  to the given maximum <paramref name="depth"/>,
		///  searched in an implementation-defined manner.
		/// </summary>
		/// <param name="directory">
		///  The directory in which to search; the local directory if
		///  <b>null</b> or empty.
		/// </param>
		/// <param name="patterns">
		///  One or more search patterns, separated by the <c>|</c>
		///  character; searches for all if <b>null</b> or
		///  <see cref="FileSearcher.WildcardsAll"/>.
		/// </param>
		/// <param name="options">
		///  Combination of <see cref="SearchOptions"/> to moderate the search
		/// </param>
		/// <param name="depth">
		///  The maximum search depth; 0 to perform a non-recursive search.
		/// </param>
		/// <param name="progressHandler">
		///  A <see cref="IProgressHandler">progress handler</see> instance. 
		/// </param>
		/// <param name="exceptionHandler">
		///  An <see cref="IExceptionHandler">error handler</see> instance.
		/// </param>
		/// <returns>
		///  An instance of a type exhibiting the
		///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
		///  interface.
		/// </returns>
		public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, IProgressHandler progressHandler, IExceptionHandler exceptionHandler)
		{
			return DepthFirst.Search(directory, patterns, options, depth, progressHandler, exceptionHandler);
		}

		/// <summary>
		///  Returns an enumerable collection of <see cref="IEntry"/>
		///  instances representing all file-system entries
		///  under <paramref name="directory"/>
		///  matching the given <paramref name="patterns"/>,
		///  according to the given <paramref name="options"/>,
		///  using the given <paramref name="progressHandler"/>
		///   and <paramref name="exceptionHandler"/>,
		///  to the given maximum <paramref name="depth"/>,
		///  searched in an implementation-defined manner.
		/// </summary>
		/// <param name="directory">
		///  The directory in which to search; the local directory if
		///  <b>null</b> or empty.
		/// </param>
		/// <param name="patterns">
		///  One or more search patterns, separated by the <c>|</c>
		///  character; searches for all if <b>null</b> or
		///  <see cref="FileSearcher.WildcardsAll"/>.
		/// </param>
		/// <param name="options">
		///  Combination of <see cref="SearchOptions"/> to moderate the search
		/// </param>
		/// <param name="depth">
		///  The maximum search depth; 0 to perform a non-recursive search.
		/// </param>
		/// <param name="progressHandler">
		///  A <see cref="IProgressHandler">progress handler</see> instance. 
		/// </param>
		/// <param name="exceptionHandler">
		///  An <see cref="IExceptionHandler">error handler</see> instance.
		/// </param>
		/// <param name="context">
		///  Caller-supplied context value, that will be accessible on
		///  every search entry via the
		///  <see cref="Recls.IEntry.Context"/> property.
		/// </param>
		/// <returns>
		///  An instance of a type exhibiting the
		///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
		///  interface.
		/// </returns>
		public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, IProgressHandler progressHandler, IExceptionHandler exceptionHandler, object context)
		{
			return DepthFirst.Search(directory, patterns, options, depth, progressHandler, exceptionHandler, context);
		}

		/// <summary>
		///  Returns an enumerable collection of <see cref="IEntry"/>
		///  instances representing all file-system entries
		///  under <paramref name="directory"/>
		///  matching the given <paramref name="patterns"/>,
		///  according to the given <paramref name="options"/>,
		///  using the given <paramref name="progressHandler"/>
		///   and <paramref name="exceptionHandler"/>,
		///  to the given maximum <paramref name="depth"/>,
		///  searched in an implementation-defined manner.
		/// </summary>
		/// <param name="directory">
		///  The directory in which to search; the local directory if
		///  <b>null</b> or empty.
		/// </param>
		/// <param name="patterns">
		///  One or more search patterns, separated by the <c>|</c>
		///  character; searches for all if <b>null</b> or
		///  <see cref="FileSearcher.WildcardsAll"/>.
		/// </param>
		/// <param name="options">
		///  Combination of <see cref="SearchOptions"/> to moderate the search
		/// </param>
		/// <param name="depth">
		///  The maximum search depth; 0 to perform a non-recursive search.
		/// </param>
		/// <param name="progressHandler">
		///  A <see cref="Recls.OnProgress">progress handler</see> delegate.
		/// </param>
		/// <param name="exceptionHandler">
		///  An <see cref="Recls.OnException">error handler</see> delegate.
		/// </param>
		/// <returns>
		///  An instance of a type exhibiting the
		///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
		///  interface.
		/// </returns>
		public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, OnProgress progressHandler, OnException exceptionHandler)
		{
			return DepthFirst.Search(directory, patterns, options, depth, progressHandler, exceptionHandler);
		}

		/// <summary>
		///  Returns an enumerable collection of <see cref="IEntry"/>
		///  instances representing all file-system entries
		///  under <paramref name="directory"/>
		///  matching the given <paramref name="patterns"/>,
		///  according to the given <paramref name="options"/>,
		///  using the given <paramref name="progressHandler"/>
		///   and <paramref name="exceptionHandler"/>,
		///  to the given maximum <paramref name="depth"/>,
		///  searched in an implementation-defined manner.
		/// </summary>
		/// <param name="directory">
		///  The directory in which to search; the local directory if
		///  <b>null</b> or empty.
		/// </param>
		/// <param name="patterns">
		///  One or more search patterns, separated by the <c>|</c>
		///  character; searches for all if <b>null</b> or
		///  <see cref="FileSearcher.WildcardsAll"/>.
		/// </param>
		/// <param name="options">
		///  Combination of <see cref="SearchOptions"/> to moderate the search
		/// </param>
		/// <param name="depth">
		///  The maximum search depth; 0 to perform a non-recursive search.
		/// </param>
		/// <param name="progressHandler">
		///  A <see cref="Recls.OnProgress">progress handler</see> delegate.
		/// </param>
		/// <param name="exceptionHandler">
		///  An <see cref="Recls.OnException">error handler</see> delegate.
		/// </param>
		/// <param name="context">
		///  Caller-supplied context value, that will be accessible on
		///  every search entry via the
		///  <see cref="Recls.IEntry.Context"/> property.
		/// </param>
		/// <returns>
		///  An instance of a type exhibiting the
		///  <see name="System.Collections.IEnumerable{T}">IEnumerable</see>&lt;<see cref="IEntry"/>&gt;
		///  interface.
		/// </returns>
		public static IEnumerable<IEntry> Search(string directory, string patterns, SearchOptions options, int depth, OnProgress progressHandler, OnException exceptionHandler, object context)
		{
			return DepthFirst.Search(directory, patterns, options, depth, progressHandler, exceptionHandler, context);
		}
		#endregion

		#region utility operations

		/// <summary>
		///  Returns an entry representing the given path, or <b>null</b>.
		/// </summary>
		/// <param name="path">
		///  The path of the entry to be evaluated. May not be <b>null</b>,
        ///  or <b>empty</b> (or <b>whitespace</b>).
		/// </param>
		/// <returns>
		///  An instance of <see cref="IEntry"/> corresponding to the
		///  file-system entry, or <b>null</b> if no such entry exists.
		/// </returns>
		/// <remarks>
		///  If no file-system entry exists, <b>null</b> is returned. In all
		///  other error cases, an exception is thrown.
		/// </remarks>
		/// <exception cref="System.Security.SecurityException">
		///  If the caller does not have the required permission.
		/// </exception>
		/// <exception cref="System.UnauthorizedAccessException">
		///  If access to the file is denied.
		/// </exception>
		/// <exception cref="System.IO.PathTooLongException">
		///  If the specified path exceeds the system-defined maximum
		///  length.
		/// </exception>
		public static IEntry Stat(string path)
		{
			return Util.Stat(path, SearchOptions.None);
		}

        /// <summary>
		///  Returns an entry representing the given path, or <b>null</b>.
        /// </summary>
        /// <param name="path">
		///  The path of the entry to be evaluated. May not be <b>null</b>,
        ///  or <b>empty</b> (or <b>whitespace</b>).
        /// </param>
        /// <param name="options">
        ///  The following enumerators from the
        ///  <see cref="Recls.SearchOptions"/> enumeration are recognised:
        ///  <list type="bullet">
        ///   <item>
        ///    <description>
        ///     <see cref="Recls.SearchOptions.Directories"/>
        ///    </description>
        ///    <description>
        ///     <see cref="Recls.SearchOptions.Files"/>
        ///    </description>
        ///    <description>
        ///     <see cref="Recls.SearchOptions.StatInfoForNonexistentPath"/>
        ///    </description>
        ///    <description>
        ///     <see cref="Recls.SearchOptions.MarkDirectories"/>
        ///    </description>
        ///    <description>
        ///     <see cref="Recls.SearchOptions.DoNotTranslatePathSeparators"/>
        ///    </description>
        ///   </item>
        ///  </list>
        ///  <para>
        ///   <b>NOTE</b>: other enumerator values are ignored, but their
        ///   presence does result in a diagnostics trace warning.
        ///  </para>
        /// </param>
		/// <returns>
		///  An instance of <see cref="IEntry"/> corresponding to the
		///  file-system entry, or <b>null</b> if no such entry exists.
		/// </returns>
        ///
		/// <remarks>
		///  If no file-system entry exists, <b>null</b> is returned unless
        ///  the parameter <paramref name="options"/>
        ///  contains the enumerator
        ///  <see cref="Recls.SearchOptions.StatInfoForNonexistentPath"/>,
        ///  in which case a "non-existing" entry is returned, according to
        ///  the following table:
        ///  
        ///  <list type="table">
        ///   <listheader>
        ///    <term>
        ///     <paramref name="path"/> ends with path-name separator
        ///    </term>
        ///    <term>
        ///     Additional
        ///     <see cref="Recls.SearchOptions"/>
        ///     enumerators in <paramref name="options"/>
        ///    </term>
        ///    <term>Result</term>
        ///   </listheader>
        ///   <item>
        ///    <term>
        ///     yes
        ///    </term>
        ///    <term> any permutation </term>
        ///    <term>
        ///     non-existing entry assumed to be a directory
        ///    </term>
        ///   </item>
        ///   <item>
        ///    <term>
        ///     no
        ///    </term>
        ///    <term>
        ///     <c><see cref="Recls.SearchOptions.Directories"/></c>
        ///    </term>
        ///    <term>
        ///     non-existing entry assumed to be a directory
        ///    </term>
        ///   </item>
        ///   <item>
        ///    <term>
        ///     no
        ///    </term>
        ///    <term>
        ///     <c><see cref="Recls.SearchOptions.Files"/></c>
        ///    </term>
        ///    <term>
        ///     non-existing entry assumed to be a file
        ///    </term>
        ///   </item>
        ///   <item>
        ///    <term>
        ///     no
        ///    </term>
        ///    <term>
        ///     <c><see cref="Recls.SearchOptions.Directories"/> | <see cref="Recls.SearchOptions.Files"/></c>
        ///    </term>
        ///    <term>
        ///     <b>null</b> is returned (and a trace warning is emitted)
        ///    </term>
        ///   </item>
        ///   <item>
        ///    <term>
        ///     no
        ///    </term>
        ///    <term>
        ///     <c>0</c>
        ///    </term>
        ///    <term>
        ///     <b>null</b> is returned (and a trace warning is emitted)
        ///    </term>
        ///   </item>
        ///  </list>
        ///
        /// <para>
        ///  If such a "non-existing" entry is returned, then
        ///  its <see cref="Recls.IEntry.Size"/> property will be <b>0</b>,
        ///  its <see cref="Recls.IEntry2_1.Existed"/> property will be
        ///  false, and
        ///  its <see cref="Recls.IEntry.Attributes"/> property will be
        ///  equal to <b>-1</b> (of type <c>int</c>).
        /// </para>
        ///
        ///  <para>
        ///   <b>NOTE</b>: other enumerator values are ignored, but their
        ///   presence does result in a diagnostics trace warning.
        ///  </para>
		/// </remarks>
        ///
		/// <remarks>
		///  If no file-system entry exists, <b>null</b> is returned. In all
		///  other error cases, an exception is thrown.
		/// </remarks>
		/// <exception cref="System.Security.SecurityException">
		///  If the caller does not have the required permission.
		/// </exception>
		/// <exception cref="System.UnauthorizedAccessException">
		///  If access to the file is denied.
		/// </exception>
		/// <exception cref="System.IO.PathTooLongException">
		///  If the specified path exceeds the system-defined maximum
		///  length.
		/// </exception>
        public static IEntry Stat(string path, SearchOptions options)
        {
            return Util.Stat(path, options);
        }
        #endregion
		#endregion
	}
}

/* ///////////////////////////// end of file //////////////////////////// */

