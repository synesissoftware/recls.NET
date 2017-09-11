
namespace Recls
{
	using global::Recls.Internal;

	using global::System.Collections.Generic;

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
		#endregion
		#endregion
	}
}
