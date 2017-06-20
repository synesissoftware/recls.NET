
/* /////////////////////////////////////////////////////////////////////////
 * File:        IErrorHandler.cs
 *
 * Created:     24th July 2009
 * Updated:     20th June 2017
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

/* ///////////////////////////// end of file //////////////////////////// */

