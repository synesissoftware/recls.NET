
namespace Recls.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.IO;
	using System.Threading;

	internal abstract class Entry
		: IEntry
	{
		#region Construction
		protected Entry(FileSystemInfo info, string searchRoot, SearchOptions options)
		{
			Debug.Assert(null != info);
			Debug.Assert(null != searchRoot);
			Debug.Assert(0 != searchRoot.Length);
			Debug.Assert(Util.HasDirEnd(searchRoot), "path must end in terminator");

			m_info = info;
			m_searchRoot = searchRoot;
			//m_options = options;
			Util.consume_options_(options);
			m_fullPath = info.FullName;

			Debug.Assert(Util.IsPathAbsolute(m_fullPath));
			int unc = Util.GetUncDriveLength(m_fullPath);

			if(unc < 0)
			{
				m_lengthOfDrive = 2;
			}
			else
			{
				m_lengthOfDrive = unc;
			}

			// Next, get the file
			if(Util.HasDirEnd(m_fullPath))
			{
				m_endOfDirectory = m_fullPath.Length;
				m_endOfFileName = m_fullPath.Length;
			}
			else
			{
				int lastSlash = m_fullPath.LastIndexOfAny(Util.PathNameSeparatorCharacters);

				Debug.Assert(lastSlash >= 0);

				m_endOfDirectory = lastSlash + 1;

				int dot = m_fullPath.LastIndexOf('.');

				if(dot < m_endOfDirectory)
				{
					m_endOfFileName = m_fullPath.Length;
				}
				else
				{
					m_endOfFileName = dot;
				}
			}

			m_partsSpin = 0;
			m_directoryParts = null;
		}
		#endregion

		#region Internal Properties
		//protected SearchOptions SearchOptions_
		//{
		//    get { return m_options; }
		//}
		#endregion

		#region IEntry Members
		public virtual string Path
		{
			get { return RawPath; }
		}

		public string SearchRelativePath
		{
			get { return Path.Substring(m_searchRoot.Length); }
		}

		public string Drive
		{
			get
			{
				string drive = RawPath.Substring(0, m_lengthOfDrive);

#if PSEUDO_UNIX
				drive = Util.ConvertPathToPseudoUNIX(drive);
#endif // PSEUDO_UNIX

				return drive;
			}
		}

		public string DirectoryPath
		{
			get
			{
				return RawPath.Substring(0, m_endOfDirectory);
			}
		}

		public string Directory
		{
			get
			{
				string dir = RawPath.Substring(m_lengthOfDrive, m_endOfDirectory - m_lengthOfDrive);

#if PSEUDO_UNIX
				dir = Util.ConvertPathToPseudoUNIX(dir);
#endif // PSEUDO_UNIX

				return dir;
			}
		}

		public string SearchDirectory
		{
			get { return m_searchRoot; }
		}

		public string UncDrive
		{
			get
			{
				int index;

				if(2 != m_lengthOfDrive)
				{
					index = m_lengthOfDrive;
				}
				else
				{
					index = Util.GetUncDriveLength(DirectoryPath);
				}

				if(index < 0)
				{
					return null;
				}
				else
				{
					return DirectoryPath.Substring(0, index);
				}
			}
		}

		public virtual string File
		{
			get
			{
				return Path.Substring(m_endOfDirectory);
			}
		}

		public string FileName
		{
			get
			{
				return RawPath.Substring(m_endOfDirectory, m_endOfFileName - m_endOfDirectory);
			}
		}

		public string FileExtension
		{
			get
			{
				return RawPath.Substring(m_endOfFileName);
			}
		}

		public DateTime CreationTime
		{
			get { return Info_.CreationTime; }
		}

		public DateTime ModificationTime
		{
			get { return Info_.LastWriteTime; }
		}

		public DateTime LastAccessTime
		{
			get { return Info_.LastAccessTime; }
		}

		public DateTime LastStatusChangeTime
		{
			get { return Info_.LastWriteTime; }
		}

		public abstract long Size { get; }

		public FileAttributes Attributes
		{
			get { return Info_.Attributes; }
		}

		public abstract bool IsReadOnly { get; }

		public abstract bool IsDirectory { get; }

		public bool IsUnc
		{
			get
			{
				return (2 != m_lengthOfDrive);
			}
		}

#if USE_ARRAY_FOR_DIRECTORY_PARTS
		public string[] DirectoryParts
#elif USE_ILIST_FOR_DIRECTORY_PARTS
		public IList<string> DirectoryParts
#else
		IDirectoryParts IEntry.DirectoryParts
#endif
		{
			get
			{
				for(;;)
				{
					if(1 == Interlocked.Increment(ref m_partsSpin))
					{
						break;
					}
					else
					{
						Interlocked.Decrement(ref m_partsSpin);
					}
				}
				try
				{
					if(null == m_directoryParts)
					{
#if USE_ARRAY_FOR_DIRECTORY_PARTS
						m_directoryParts = MakeDirectoryParts().ToArray();
#elif USE_ILIST_FOR_DIRECTORY_PARTS
						m_directoryParts = new ReadOnlyCollection<string>(MakeDirectoryParts());
#else
						m_directoryParts = new DirectoryParts(MakeDirectoryParts());
#endif
					}

					return m_directoryParts;
				}
				finally
				{
					Interlocked.Decrement(ref m_partsSpin);
				}
			}
		}
		#endregion

		#region Object Overrides
		public override string ToString()
		{
			return Path;
		}
		#endregion

		#region Implementation
		protected FileSystemInfo Info_
		{
			get { return m_info; }
		}

		protected string RawPath
		{
			get
			{
				return m_fullPath;
			}
		}

		private List<string> MakeDirectoryParts()
		{
			Debug.Assert(0 != m_partsSpin);

			int				i1;
			int				i2;
			List<string>	parts = new List<string>(10);

			for(i1 = i2 = m_lengthOfDrive; i1 != m_endOfDirectory; ++i1)
			{
				switch(m_fullPath[i1])
				{
					case '\\':
					case '/':
						parts.Add(m_fullPath.Substring(i2, (i1 - i2) + 1));
						i2 = i1 + 1;
						break;
					default:
						break;
				}
			}
			if(i1 != i2)
			{
				parts.Add(m_fullPath.Substring(i2, i1 - i2));
			}

			return parts;
		}
		#endregion

		#region Member Variables
		private readonly FileSystemInfo		m_info;
		//private readonly SearchOptions	m_options;
		private readonly string				m_searchRoot;
		private readonly string				m_fullPath;
		private readonly int				m_lengthOfDrive;
		private readonly int				m_endOfDirectory;
		private readonly int				m_endOfFileName;
		private int							m_partsSpin;
#if USE_ARRAY_FOR_DIRECTORY_PARTS
		private string[]					m_directoryParts;
#elif USE_ILIST_FOR_DIRECTORY_PARTS
		private IList<string>				m_directoryParts;
#else
		private IDirectoryParts				m_directoryParts;
#endif
		#endregion
	}
}
