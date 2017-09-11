
/* /////////////////////////////////////////////////////////////////////////
 * File:        Internal/DirectorySearchNode.cs
 *
 * Created:     5th June 2009
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


namespace Recls.Internal
{
	using System;
	using System.Diagnostics;
	using System.IO;

	internal sealed class DirectorySearchNode
		: IDirectorySearchNode
	{
		#region construction
		internal DirectorySearchNode(string searchRoot, string directory, Patterns patterns, SearchOptions options, IExceptionHandler exceptionHandler, IProgressHandler progressHandler, int depth, object context)
		{
			Debug.Assert(Util.HasDirEnd(searchRoot), "path must end in terminator");
			Debug.Assert(Util.HasDirEnd(directory), "path must end in terminator");
			Debug.Assert(Util.IsPathAbsolute(searchRoot), "path must be absolute");
			Debug.Assert(Util.IsPathAbsolute(directory), "path must be absolute");
			Debug.Assert(null != exceptionHandler);
			Debug.Assert(null != progressHandler);
			Debug.Assert(m_depth >= 0);

			DirectoryInfo di = new DirectoryInfo(directory);

			m_searchRoot = searchRoot;
			m_directory = directory;
			m_patterns = patterns;
			m_options = options;
			m_exceptionHandler = exceptionHandler;
			m_progressHandler = progressHandler;
			m_depth = depth;
			m_searchCancelled = false;

			switch(m_progressHandler.OnProgress(m_context, m_directory, m_depth))
			{
				case ProgressHandlerResult.Continue:
					m_entries = Util.GetEntriesByPatterns(m_context, m_exceptionHandler, di, m_patterns, m_options);
					m_subdirectories = Util.GetSubdirectories(m_context, m_exceptionHandler, di, m_options);
					break;
				case ProgressHandlerResult.CancelDirectory:
					m_entries = new FileSystemInfo[0];
					m_subdirectories = new DirectoryInfo[0];
					break;
				case ProgressHandlerResult.CancelSearch:
					m_searchCancelled = true;
					break;
			}

			m_context = context;
		}

		internal DirectorySearchNode Restart()
		{
			return new DirectorySearchNode(m_searchRoot, m_directory, m_patterns, m_options, m_exceptionHandler, m_progressHandler, m_depth, m_context);
		}
		#endregion

		#region IDirectorySearchNode members
		IDirectorySearchNode IDirectorySearchNode.GetNextNode()
		{
			if(!m_searchCancelled)
			{
				if(m_subdirectoryIndex < m_subdirectories.Length)
				{
					DirectoryInfo di = m_subdirectories[m_subdirectoryIndex++];

					return new DirectorySearchNode(m_searchRoot, Util.EnsureDirEnd(di.FullName), m_patterns, m_options, m_exceptionHandler, m_progressHandler, m_depth + 1, m_context);
				}
			}

			return null;
		}
		IEntry IDirectorySearchNode.GetNextEntry()
		{
			if(!m_searchCancelled)
			{
				Debug.Assert(m_entryIndex <= m_entries.Length);
				if(m_entryIndex < m_entries.Length)
				{
					IEntry			entry	=	null;
					FileSystemInfo	fsi 	=	m_entries[m_entryIndex];
					FileInfo		fi		=	fsi as FileInfo;

					if(null != fi)
					{
						entry = new FileEntry(fi, m_searchRoot, m_options, m_context);
					}
					else
					{
						entry = new DirectoryEntry((DirectoryInfo)fsi, m_searchRoot, m_options, m_context);
					}

					++m_entryIndex;

					return entry;
				}
			}

			return null;
		}
		#endregion

		#region fields
		readonly string 			m_searchRoot;
		readonly string 			m_directory;
		readonly Patterns			m_patterns;
		readonly SearchOptions		m_options;
		readonly IExceptionHandler	m_exceptionHandler;
		readonly IProgressHandler	m_progressHandler;
		readonly int				m_depth;
		readonly DirectoryInfo[]	m_subdirectories;
		int 						m_subdirectoryIndex;
		readonly FileSystemInfo[]	m_entries;
		int 						m_entryIndex;
		bool						m_searchCancelled;
		readonly object				m_context;
		#endregion
	}
}

/* ///////////////////////////// end of file //////////////////////////// */

