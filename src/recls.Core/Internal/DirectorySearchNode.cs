
namespace Recls.Internal
{
	using System;
	using System.Diagnostics;
	using System.IO;

	internal sealed class DirectorySearchNode
		: IDirectorySearchNode
	{
		#region Construction
		internal DirectorySearchNode(string searchRoot, string directory, Patterns patterns, SearchOptions options, IExceptionHandler exceptionHandler, IProgressHandler progressHandler, int depth)
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

			switch(m_progressHandler.OnProgress(m_directory, m_depth))
			{
				case ProgressHandlerResult.Continue:
					m_entries = Util.GetEntriesByPatterns(m_exceptionHandler, di, m_patterns, m_options);
					m_subdirectories = Util.GetSubdirectories(m_exceptionHandler, di, m_options);
					break;
				case ProgressHandlerResult.CancelDirectory:
					m_entries = new FileSystemInfo[0];
					m_subdirectories = new DirectoryInfo[0];
					break;
				case ProgressHandlerResult.CancelSearch:
					m_searchCancelled = true;
					break;
			}
		}

		internal DirectorySearchNode Restart()
		{
			return new DirectorySearchNode(m_searchRoot, m_directory, m_patterns, m_options, m_exceptionHandler, m_progressHandler, m_depth);
		}
		#endregion

		#region IDirectorySearchNode Members
		IDirectorySearchNode IDirectorySearchNode.GetNextNode()
		{
			if(!m_searchCancelled)
			{
				if(m_subdirectoryIndex < m_subdirectories.Length)
				{
					DirectoryInfo di = m_subdirectories[m_subdirectoryIndex++];

					return new DirectorySearchNode(m_searchRoot, Util.EnsureDirEnd(di.FullName), m_patterns, m_options, m_exceptionHandler, m_progressHandler, m_depth + 1);
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
						entry = new FileEntry(fi, m_searchRoot, m_options);
					}
					else
					{
						entry = new DirectoryEntry((DirectoryInfo)fsi, m_searchRoot, m_options);
					}

					++m_entryIndex;

					return entry;
				}
			}

			return null;
		}
		#endregion

		#region Fields
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
		#endregion
	}
}
