
namespace Recls.Internal
{
	using System;

	#region IExceptionHandler classes
	internal class IgnoreAllExceptionHandler
		: IExceptionHandler
	{
		#region IExceptionHandler Members
		ExceptionHandlerResult IExceptionHandler.OnException(string path, Exception x)
		{
			return ExceptionHandlerResult.ConsumeExceptionAndContinue;
		}
		#endregion
	}

	internal class FailAllExceptionHandler
		: IExceptionHandler
	{
		#region IExceptionHandler Members
		ExceptionHandlerResult IExceptionHandler.OnException(string path, Exception x)
		{
			return ExceptionHandlerResult.PropagateException;
		}
		#endregion
	}

	internal class DelegateExceptionHandler
		: IExceptionHandler
	{
		#region Fields
		private OnException m_dg;
		#endregion

		#region Construction
		public DelegateExceptionHandler(OnException dg)
		{
			m_dg = dg;
		}
		#endregion

		#region IExceptionHandler Members
		ExceptionHandlerResult IExceptionHandler.OnException(string path, Exception x)
		{
			return m_dg(path, x);
		}
		#endregion
	}
	#endregion

	#region IProgressHandler classes
	internal class ContinueAllProgressHandler
		: IProgressHandler
	{
		#region IProgressHandler Members
		ProgressHandlerResult IProgressHandler.OnProgress(string directory, int depth)
		{
			return ProgressHandlerResult.Continue;
		}
		#endregion
	}

	internal class DelegateProgressHandler
		: IProgressHandler
	{
		#region Fields
		private OnProgress	m_dg;
		#endregion

		#region Construction
		public DelegateProgressHandler(OnProgress dg)
		{
			m_dg = dg;
		}
		#endregion

		#region IProgressHandler Members
		ProgressHandlerResult IProgressHandler.OnProgress(string directory, int depth)
		{
			return m_dg(directory, depth);
		}
		#endregion
	}
	#endregion
}
