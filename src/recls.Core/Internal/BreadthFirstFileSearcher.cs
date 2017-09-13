
/* /////////////////////////////////////////////////////////////////////////
 * File:        Internal/BreadthFirstFileSearcher.cs
 *
 * Created:     5th June 2009
 * Updated:     11th September 2017
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
	using System.IO;
	using System.Collections.Generic;
	using System.Diagnostics;

	internal class BreadthFirstFileSearcher
		: IEnumerable<IEntry>
		, IDisposable
	{
		#region construction

		internal BreadthFirstFileSearcher(string directory, string patterns, SearchOptions options, int maxDepth, IExceptionHandler exceptionHandler, IProgressHandler progressHandler, object context)
		{
			Debug.Assert(null != directory);
			Debug.Assert(Util.HasDirEnd(directory), "path must end in terminator");
			Debug.Assert(Util.IsPathAbsolute(directory), "path must be absolute");
			Debug.Assert(null != patterns);
			Debug.Assert(0 != ((SearchOptions.Directories | SearchOptions.Files) & options));
			Debug.Assert(null != exceptionHandler);
			Debug.Assert(maxDepth >= 0, "maximum depth cannot be less than 0");

			Util.CheckDirectoryExistsOrThrow(directory, options, out m_stubEnumerator, out m_lockFile, out m_lockFileInfo);

			m_directory = directory;
			m_patterns = new Patterns(patterns);
			m_options = options;
			m_maxDepth = maxDepth;
			m_exceptionHandler = exceptionHandler;
			m_progressHandler = progressHandler;
			m_context = context;
		}

		void IDisposable.Dispose()
		{
			m_lockFile.Dispose();
		}
		#endregion

		#region IEnumerable<IEntry> members

		System.Collections.Generic.IEnumerator<IEntry> System.Collections.Generic.IEnumerable<IEntry>.GetEnumerator()
		{
			if (null != m_stubEnumerator)
			{
				return m_stubEnumerator;
			}

			return new Enumerator(m_directory, m_patterns, m_options, m_maxDepth, m_exceptionHandler, m_progressHandler, m_context, m_lockFileInfo);
		}
		#endregion

		#region IEnumerable members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((System.Collections.Generic.IEnumerable<IEntry>)this).GetEnumerator();
		}
		#endregion

		#region types

		private class Enumerator
			: IEnumerator<IEntry>
		{
			#region construction

			internal Enumerator(string directory, Patterns patterns, SearchOptions options, int maxDepth, IExceptionHandler exceptionHandler, IProgressHandler progressHandler, object context, FileInfo lockFileInfo)
			{
				Debug.Assert(null != directory);
				Debug.Assert(Util.HasDirEnd(directory), "path must end in terminator");
				Debug.Assert(Util.IsPathAbsolute(directory), "path must be absolute");
				Debug.Assert(null != patterns);
				Debug.Assert(0 != ((SearchOptions.Directories | SearchOptions.Files) & options));
				Debug.Assert(maxDepth >= 0, "maximum depth cannot be less than 0");

				m_directory = directory;
				m_maxDepth = maxDepth;
				m_di = new DirectoryInfo(directory);
				m_patterns = patterns;
				m_entries = new List<FileSystemInfo>();
				m_subdirectories = new List<DirectoryInfo>();
				m_options = options;
				m_exceptionHandler = exceptionHandler;
				m_progressHandler = progressHandler;

				m_context = context;
				m_lockFileInfo = lockFileInfo;

				Reset_(false);
			}
			#endregion construction

			#region IDisposable members

			void IDisposable.Dispose()
			{
				m_currentEntry = null;
				m_entries.Clear();
				m_entryIndex = 0;
				m_subdirectories.Clear();

				GC.SuppressFinalize(this);
			}
			#endregion

			#region IEnumerator<IEntry> members

			IEntry IEnumerator<IEntry>.Current
			{
				get { return GetCurrent_(); }
			}
			#endregion

			#region IEnumerator members
			object System.Collections.IEnumerator.Current
			{
				get { return GetCurrent_(); }
			}

			bool System.Collections.IEnumerator.MoveNext()
			{
				if(!m_searchCancelled)
				{
					for(; 0 != m_entries.Count || 0 != m_subdirectories.Count; )
					{
						Debug.Assert(m_entryIndex <= m_entries.Count);
						if(m_entryIndex < m_entries.Count)
						{
							FileSystemInfo	fsi =	m_entries[m_entryIndex];
							FileInfo		fi	=	fsi as FileInfo;

							if(null != fi)
							{
								m_currentEntry = new FileEntry(fi, m_directory, m_options, m_context);
							}
							else
							{
								m_currentEntry = new DirectoryEntry((DirectoryInfo)fsi, m_directory, m_options, m_context);
							}

							++m_entryIndex;

							return true;
						}

						// Descend a level
						if(m_depth++ >= m_maxDepth)
						{
							break;
						}

						List<FileSystemInfo> nextEntries = new List<FileSystemInfo>();
						List<DirectoryInfo> nextSubdirectories = new List<DirectoryInfo>();

						foreach(DirectoryInfo di in m_subdirectories)
						{
							switch(m_progressHandler.OnProgress(m_context, Util.EnsureDirEnd(di.FullName), m_depth))
							{
								case ProgressHandlerResult.Continue:
									nextEntries.AddRange(Util.GetEntriesByPatterns(m_context, m_exceptionHandler, di, m_patterns, m_options, m_lockFileInfo));
									nextSubdirectories.AddRange(Util.GetSubdirectories(m_context, m_exceptionHandler, di, m_options));
									break;
								case ProgressHandlerResult.CancelDirectory:
									break;
								case ProgressHandlerResult.CancelSearch:
									m_searchCancelled = true;
									return false;
							}
						}

						m_entryIndex = 0;
						m_entries = nextEntries;
						m_subdirectories = nextSubdirectories;
					}
				}

				return false;
			}

			void System.Collections.IEnumerator.Reset()
			{
				Reset_(false);
			}
			#endregion

			#region implementation
			// Need a separate worker method, as cannot call System.Collections.IEnumerator.Reset() without a cast
			void Reset_(bool disposing)
			{
				m_searchCancelled = false;
				m_currentEntry = null;
				m_entries.Clear();
				m_entryIndex = 0;
				m_subdirectories.Clear();
				m_depth = 0;
				if(!disposing)
				{
					switch(m_progressHandler.OnProgress(m_context, m_directory, m_depth))
					{
						case ProgressHandlerResult.Continue:
							m_entries.AddRange(Util.GetEntriesByPatterns(m_context, m_exceptionHandler, m_di, m_patterns, m_options, m_lockFileInfo));
							m_subdirectories.AddRange(Util.GetSubdirectories(m_context, m_exceptionHandler, m_di, m_options));
							break;
						case ProgressHandlerResult.CancelDirectory:
							break;
						case ProgressHandlerResult.CancelSearch:
							m_searchCancelled = true;
							break;
					}
				}
			}

			private IEntry GetCurrent_()
			{
				return m_currentEntry;
			}
			#endregion

			#region fields
			readonly string 			m_directory;
			readonly DirectoryInfo		m_di;
			readonly Patterns			m_patterns;
			readonly SearchOptions		m_options;
			readonly int				m_maxDepth;
			readonly IExceptionHandler	m_exceptionHandler;
			readonly IProgressHandler	m_progressHandler;
			IEntry						m_currentEntry;
			List<FileSystemInfo>		m_entries;
			int 						m_entryIndex;
			List<DirectoryInfo> 		m_subdirectories;
			int 						m_depth;
			bool						m_searchCancelled;
			readonly object				m_context;
			readonly FileInfo			m_lockFileInfo;
			#endregion
		}
		#endregion

		#region fields

		readonly string 				m_directory;
		readonly Patterns				m_patterns;
		readonly SearchOptions			m_options;
		readonly int					m_maxDepth;
		readonly IExceptionHandler		m_exceptionHandler;
		readonly IProgressHandler		m_progressHandler;
		readonly object					m_context;
		readonly IDisposable			m_lockFile;
		readonly FileInfo				m_lockFileInfo;
		readonly IEnumerator<IEntry>	m_stubEnumerator;
		#endregion
	}
}

/* ///////////////////////////// end of file //////////////////////////// */

