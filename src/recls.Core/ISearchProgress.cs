
namespace Recls
{
	/// <summary>
	///  Interface providing search progress notifications and control.
	/// </summary>
	public interface IProgressHandler
	{
		/// <summary>
		///  Invoked for each directory traversed in a search.
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
		///  determines how the search should proceed. Return
		///  <b><see cref="ProgressHandlerResult.Continue"/></b>
		///  to allow enumeration of remaining entries to proceed; return
		///  <b><see cref="ProgressHandlerResult.CancelDirectory"/></b>
		///  to skip the given <paramref name="directory"/> and all its
		///  sub-directories; return
		///  <b><see cref="ProgressHandlerResult.CancelSearch"/></b>
		///  to cancel the search.
		/// </returns>
		ProgressHandlerResult OnProgress(string directory, int depth);
	}
}
