
namespace Recls.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	internal class DepthFirstFileSearcher
		: IEnumerable<IEntry>
	{
		#region Construction
		internal DepthFirstFileSearcher(string directory, string patterns, SearchOptions options, int maxDepth, IExceptionHandler exceptionHandler, IProgressHandler progressHandler)
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

		#region Member Types
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

				m_maxDepth = maxDepth;
				m_rootNode = new DirectorySearchNode(directory, directory, patterns, options, exceptionHandler, progressHandler, 0);
				m_nodes = new Stack<IDirectorySearchNode>();
				m_nodes.Push(m_rootNode);
			}
            #endregion Construction

			#region IDisposable Members
			void IDisposable.Dispose()
			{
				Reset_(true);

				GC.SuppressFinalize(this);
			}
			#endregion

            #region IEnumerator<Entry> Members
			IEntry System.Collections.Generic.IEnumerator<IEntry>.Current
            {
                get { return GetCurrent_(); }
            }
            #endregion

            #region IEnumerator Members
            object System.Collections.IEnumerator.Current
            {
                get { return GetCurrent_(); }
            }

			bool System.Collections.IEnumerator.MoveNext()
            {
					// The nodes are processed in a stack of directory search
					// nodes.
					//
					// The top of the stack is processed in each iteration. When
					// all the files are processed from the head node, it is
					// queried for its next (sub) search node. If one exists,
					// then it is pushed onto the stack, and will be processed
					// on the next iteration. If there are no more sub search
					// nodes, then the top is popped off the stack.
					//
					// When the stack is empty, the processing is complete.
				for(; 0 != m_nodes.Count; )
				{
					IDirectorySearchNode	node	=	m_nodes.Peek();
					IEntry					entry	=	node.GetNextEntry();

					if(null != entry)
					{
						m_currentEntry = entry;

						return true;
					}
					else
					{
						IDirectorySearchNode nextNode = node.GetNextNode();

						if(null != nextNode)
						{
							if(m_nodes.Count <= m_maxDepth)
							{
								m_nodes.Push(nextNode);
							}
						}
						else
						{
							Debug.Assert(m_nodes.Count > 0);

							m_nodes.Pop();
						}
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
				m_currentEntry = null;
				m_nodes.Clear();
				if(!disposing)
				{
					m_rootNode = m_rootNode.Restart();
					m_nodes.Push(m_rootNode);
				}
			}

			private IEntry GetCurrent_()
            {
                return m_currentEntry;
            }
            #endregion

            #region Member Variables
			readonly int				m_maxDepth;
			IEntry						m_currentEntry;
			DirectorySearchNode			m_rootNode;
			Stack<IDirectorySearchNode> m_nodes;
            #endregion Member Variables
		}
		#endregion

		#region Member Variables
        readonly string				m_directory;
        readonly Patterns			m_patterns;
		readonly SearchOptions		m_options;
		readonly int				m_maxDepth;
		readonly IExceptionHandler	m_exceptionHandler;
		readonly IProgressHandler	m_progressHandler;
        #endregion Member Variables
	}
}
