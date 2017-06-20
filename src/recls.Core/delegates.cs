
namespace Recls
{
	using System;

	#region Delegates
	/// <summary>
	///  Delegate defining function to be called on enumeration failure.
	/// </summary>
	/// <param name="path">
	///  The full path of the file/directory.
	/// </param>
	/// <param name="exception">
	///  The exception thrown to indicate the enumeration failure.
	/// </param>
	/// <returns>
	///  <b>true</b> to ignore the failure and allow enumeration of
	///  remaining entries to proceed; <b>false</b> to cancel the
	///  search.
	/// </returns>
	/// <remarks>
	///  The delegate may be invoked for files and/or directories for
	///  which enumeration has failed.
	/// </remarks>
	/// 
	/// <seealso cref="Recls.FileSearcher.Search(string, string, SearchOptions, int, OnProgress, OnException)"/>
	/// <seealso cref="Recls.FileSearcher.BreadthFirst.Search(string, string, SearchOptions, int, OnProgress, OnException)"/>
	/// <seealso cref="Recls.FileSearcher.DepthFirst.Search(string, string, SearchOptions, int, OnProgress, OnException)"/>
	public delegate ExceptionHandlerResult OnException(string path, Exception exception);
	#endregion

	/// <summary>
	///  Delegate defining function to be called for each directory
	///  traversed in a search.
	/// </summary>
	/// <param name="directory">
	///  The directory searched, or <b>null</b> to indicate that
	///  the search has been successfully completed.
	/// </param>
	/// <param name="depth">
	///  The search depth corresponding to the directory.
	/// </param>
	/// <returns>
	///  A <see cref="Recls.ProgressHandlerResult">value</see> that
	///  determines how the search should proceed.
	/// </returns>
	/// 
	/// <seealso cref="Recls.FileSearcher.Search(string, string, SearchOptions, int, OnProgress, OnException)"/>
	/// <seealso cref="Recls.FileSearcher.BreadthFirst.Search(string, string, SearchOptions, int, OnProgress, OnException)"/>
	/// <seealso cref="Recls.FileSearcher.DepthFirst.Search(string, string, SearchOptions, int, OnProgress, OnException)"/>
	public delegate ProgressHandlerResult OnProgress(string directory, int depth);
}
