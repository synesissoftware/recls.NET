
namespace Recls
{
	using System;

	/// <summary>
	///  Interface providing control when enumeration fails.
	/// </summary>
	/// <remarks>
	///  <para>
	///   The methods of this class are never called when
	///   <see cref="System.OutOfMemoryException"/> is thrown, because
	///   in such conditions it is not appropriate to attempt to continue
	///   the search operation. Users are strongly advised to return
	///   <b><see cref="ExceptionHandlerResult.PropagateException"/></b>
	///   when invoked with any other practically-unrecoverable
	///   exception type. The implementation may change in the future to
	///   exclude other exceptions from the error-handler callback.
	///  </para>
	/// </remarks>
	public interface IExceptionHandler
	{
		/// <summary>
		///  Called when enumeration in a directory entry fails.
		/// </summary>
		/// <param name="path">
		///  The full path of the directory in which enumeration was
		///  attempted.
		/// </param>
		/// <param name="exception">
		///  The exception thrown to indicate the enumeration failure.
		/// </param>
		/// <returns>
		///  A <see cref="Recls.ExceptionHandlerResult">value</see> that
		///  determines how the search should proceed. Return
		///  <b><see cref="ExceptionHandlerResult.ConsumeExceptionAndContinue"/></b>
		///  to ignore the failure and allow enumeration of
		///  remaining entries to proceed; return
		///  <b><see cref="ExceptionHandlerResult.PropagateException"/></b>
		///  to cancel the search.
		/// </returns>
		ExceptionHandlerResult OnException(string path, Exception exception);
	}
}
