
namespace Recls
{
	/// <summary>
	///  Value to be returned by 
	///  <see cref="Recls.IProgressHandler.OnProgress">IProgressHandler.OnProgress()</see>
	///  to control further processing.
	/// </summary>
	///
	/// <seealso cref="Recls.OnProgress"/>
	/// <seealso cref="Recls.IProgressHandler.OnProgress"/>
	public enum ProgressHandlerResult
	{
		/// <summary>
		///  The search should continue.
		/// </summary>
		Continue = 0,
		/// <summary>
		///  All search within the current directory, or any of its
		///  subdirectories, is cancelled. Searching in other directories is
		///  not affected.
		/// </summary>
		CancelDirectory,
		/// <summary>
		///  The entire search is cancelled. Subsequent use of the search
		///  enumerator will fail (until it is reset).
		/// </summary>
		CancelSearch,
	}
}
