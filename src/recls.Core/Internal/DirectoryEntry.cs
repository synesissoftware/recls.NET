
namespace Recls.Internal
{
	using System;
	using System.Diagnostics;
	using System.IO;

	internal sealed class DirectoryEntry
		: Entry
	{
		#region Construction
		internal DirectoryEntry(DirectoryInfo info, string searchRoot, SearchOptions options)
			: base(info, searchRoot, options)
		{
			if(0 != (options & SearchOptions.MarkDirectories))
			{
				m_path = Util.EnsureDirEnd(RawPath);
			}
			else
			{
				m_path = RawPath;
			}
		}
		#endregion

		#region IEntry Members
		public override string Path
		{
			get
			{
				return m_path;
			}
		}

		public override long Size
		{
			get { return 0; }
		}

		public override bool IsReadOnly
		{
			get { return false; }
		}

		public override bool IsDirectory
		{
			get { return true; }
		}
		#endregion

		#region Fields
		private readonly string m_path;
		#endregion
	}
}
