
/* /////////////////////////////////////////////////////////////////////////
 * File:        FileSearcher.cs
 *
 * Created:     30th June 2009
 * Updated:     24th September 2017
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
	using Recls.Internal;

	using System;
	using System.Collections.Generic;

	/// <summary>
	///  Provides methods for enumerating file-system entities.
	/// </summary>
	/// <remarks>
	///  This class defines the API for file-system entity enumeration
	///  and inspection.
	/// </remarks>
	public static class FileSearcher
	{
		#region fields and Constants
		private const int UNRESTRICTED_DETPH = int.MaxValue;
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
			get { return UNRESTRICTED_DETPH; }
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
			get { return Util.WildcardsAll; }
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
				options = Util.ValidateOptions(options);
				directory = Util.ValidateDirectory(directory, options);
				patterns = Util.ValidatePatterns(patterns, options);
				exceptionHandler = Util.ValidateExceptionHandler(exceptionHandler, options);
				progressHandler = Util.ValidateProgressHandler(progressHandler, options);

				return new DepthFirstFileSearcher(directory, patterns, options, depth, exceptionHandler, progressHandler, null);
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
				options = Util.ValidateOptions(options);
				directory = Util.ValidateDirectory(directory, options);
				patterns = Util.ValidatePatterns(patterns, options);
				exceptionHandler = Util.ValidateExceptionHandler(exceptionHandler, options);
				progressHandler = Util.ValidateProgressHandler(progressHandler, options);

				return new BreadthFirstFileSearcher(directory, patterns, options, depth, exceptionHandler, progressHandler, null);
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
		#endregion
		#endregion

		#region utility operations
		/// <summary>
		///  Returns an entry representing the given path.
		/// </summary>
		/// <param name="path">
		///  The path of the entry to be evaluated.
		/// </param>
		/// <returns>
		///  An instance of <see cref="IEntry"/> corresponding to the
		///  file-system entry, or <b>null</b> if no such entry exists.
		/// </returns>
		/// <remarks>
		///  If no file-system entry exists, <b>null</b> is returned. In all
		///  other error cases, an exception is thrown.
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		///  If <paramref name="path"/> is <b>null</b>.
		/// </exception>
		/// <exception cref="System.Security.SecurityException">
		///  If the caller does not have the required permission.
		/// </exception>
		/// <exception cref="ArgumentException">
		///  If <paramref name="path"/> is empty, contains only white
		///  spaces, or contains invalid characters.
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
			return Util.Stat(path, true);
		}

		/// <summary>
		///  Calculates the size of the given <paramref name="directory"/> as
		///  a sum of the sizes of all files within it, to the given
		///  sub-directory <paramref name="depth"/>.
		/// </summary>
		/// <param name="directory">
		///  The directory whose size will be calculated.
		/// </param>
		/// <param name="depth">
		///  The maximum depth of the calculation search. To search all
		///  sub-directories without depth limit, specify
		///  <see cref="Recls.FileSearcher.UnrestrictedDepth">UnrestrictedDepth</see>.
		/// </param>
		/// <returns>
		///  The size of all files in the given directory and in all
		///  sub-directories (up to the given depth).
		/// </returns>
		public static long CalculateDirectorySize(string directory, int depth)
		{
			long size = 0;

			checked
			{
				foreach(IEntry entry in Search(directory, Util.WildcardsAll, SearchOptions.None, depth))
				{
					size += entry.Size;
				}
			}

			return size;
		}
		/// <summary>
		///  Calculates the size of the given <paramref name="directory"/>
		///  as a sum of the sizes of all files within it and all its
		///  sub-directories.
		/// </summary>
		/// <param name="directory">
		///  The directory whose size will be calculated.
		/// </param>
		/// <returns>
		///  The size of all files in the given directory and all its
		///  sub-directories.
		/// </returns>
		public static long CalculateDirectorySize(string directory)
		{
			return CalculateDirectorySize(directory, UnrestrictedDepth);
		}

		/// <summary>
		///  Convenience alias for
		///  <see cref="Recls.FileSearcher.CalculateDirectorySize(string, int)"/>
		///  that takes an <see cref="Recls.IEntry">IEntry</see> reference
		///  to a directory entry.
		/// </summary>
		/// <param name="directory">
		///  The directory whose size will be calculated.
		/// </param>
		/// <param name="depth">
		///  The maximum depth of the calculation search. To search all
		///  sub-directories without depth limit, specify
		///  <see cref="Recls.FileSearcher.UnrestrictedDepth">UnrestrictedDepth</see>.
		/// </param>
		/// <returns>
		///  The size of all files in the given directory and in all
		///  sub-directories (up to the given depth).
		/// </returns>
		public static long CalculateDirectorySize(IEntry directory, int depth)
		{
			if(null != directory)
			{
				if(!directory.IsDirectory)
				{
					throw new ArgumentException("entry is not a directory", "directory");
				}
			}

			return CalculateDirectorySize(directory.Path, depth);
		}
		/// <summary>
		///  Convenience alias for
		///  <see cref="Recls.FileSearcher.CalculateDirectorySize(string)"/>
		///  that takes an <see cref="Recls.IEntry">IEntry</see> reference
		///  to a directory entry.
		/// </summary>
		/// <param name="directory">
		///  The directory whose size will be calculated.
		/// </param>
		/// <returns>
		///  The size of all files in the given directory and all its
		///  sub-directories.
		/// </returns>
		public static long CalculateDirectorySize(IEntry directory)
		{
			return CalculateDirectorySize(directory, UnrestrictedDepth);
		}
		#endregion
	}
}

/* ///////////////////////////// end of file //////////////////////////// */

