
namespace Recls
{
	/// <summary>
	///  Value to be returned by 
	///  <see cref="Recls.IExceptionHandler.OnException">IExceptionHandler.OnException()</see>
	///  to control further processing.
	/// </summary>
	///
	/// <seealso cref="Recls.OnException"/>
	/// <seealso cref="Recls.IExceptionHandler.OnException"/>
	public enum ExceptionHandlerResult
	{
		/// <summary>
		///  Allows the exception to propagate to the caller, causing the
		///  search to be terminated.
		/// </summary>
		PropagateException = 0,

		/// <summary>
		///  Causes the exception to be consumed and the search to continue.
		/// </summary>
		ConsumeExceptionAndContinue,
	}
}
