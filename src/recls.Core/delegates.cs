
/* /////////////////////////////////////////////////////////////////////////
 * File:        delegates.cs
 *
 * Created:     30th June 2009
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

	#region delegates
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

/* ///////////////////////////// end of file //////////////////////////// */

