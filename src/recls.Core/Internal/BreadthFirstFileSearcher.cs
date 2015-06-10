
namespace Recls.Internal
{
	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Diagnostics;

	internal class BreadthFirstFileSearcher
		: IEnumerable<IEntry>
	{
		#region Construction
		internal BreadthFirstFileSearcher(string directory, string patterns, SearchOptions options, int maxDepth, IExceptionHandler exceptionHandler, IProgressHandler progressHandler)
		{
			Debug.Assert(null != directory);
			Debug.Assert(Util.HasDirEnd(directory), "path must end in terminator");
			Debug.Assert(Util.IsPathAbsolute(directory), "path must be absolute");
			Debug.Assert(null != patterns);
			Debug.Assert(0 != ((SearchOptions.Directories | SearchOptions.Files) & options));
			Debug.Assert(null != exceptionHandler);
			Debug.Assert(maxDepth >= 0, "maximum depth cannot be less than 0");

			m_directory = directory;
			m_patterns = new Patterns(patterns);
			m_options = options;
			m_maxDepth = maxDepth;
			m_exceptionHandler = exceptionHandler;
			m_progressHandler = progressHandler;
		}
		#endregion

		#region IEnumerable<IEntry> Members
		System.Collections.Generic.IEnumerator<IEntry> System.Collections.Generic.IEnumerable<IEntry>.GetEnumerator()
		{
			return new Enumerator(m_directory, m_patterns, m_options, m_maxDepth, m_exceptionHandler, m_progressHandler);
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((System.Collections.Generic.IEnumerable<IEntry>)this).GetEnumerator();
		}
		#endregion

		private class Enumerator
			: IEnumerator<IEntry>
		{
			#region Construction
			internal Enumerator(string directory, Patterns patterns, SearchOptions options, int maxDepth, IExceptionHandler exceptionHandler, IProgressHandler progressHandler)
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

				Reset_(false);
			}
			#endregion Construction

			#region IEnumerator<IEntry> Members
			IEntry IEnumerator<IEntry>.Current
			{
				get { return GetCurrent_(); }
			}
			#endregion

			#region IDisposable Members
			void IDisposable.Dispose()
			{
				m_currentEntry = null;
				m_entries.Clear();
				m_entryIndex = 0;
				m_subdirectories.Clear();

				GC.SuppressFinalize(this);
			}
			#endregion

			#region IEnumerator Members
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
								m_currentEntry = new FileEntry(fi, m_directory, m_options);
							}
							else
							{
								m_currentEntry = new DirectoryEntry((DirectoryInfo)fsi, m_directory, m_options);
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
							switch(m_progressHandler.OnProgress(Util.EnsureDirEnd(di.FullName), m_depth))
							{
								case ProgressHandlerResult.Continue:
									nextEntries.AddRange(Util.GetEntriesByPatterns(m_exceptionHandler, di, m_patterns, m_options));
									nextSubdirectories.AddRange(Util.GetSubdirectories(m_exceptionHandler, di, m_options));
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

			#region Implementation
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
					switch(m_progressHandler.OnProgress(m_directory, m_depth))
					{
						case ProgressHandlerResult.Continue:
							m_entries.AddRange(Util.GetEntriesByPatterns(m_exceptionHandler, m_di, m_patterns, m_options));
							m_subdirectories.AddRange(Util.GetSubdirectories(m_exceptionHandler, m_di, m_options));
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

			#region Fields
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
			#endregion
		}

		#region Fields
		readonly string 			m_directory;
		readonly Patterns			m_patterns;
		readonly SearchOptions		m_options;
		readonly int				m_maxDepth;
		readonly IExceptionHandler	m_exceptionHandler;
		readonly IProgressHandler	m_progressHandler;
		#endregion
	}
}
