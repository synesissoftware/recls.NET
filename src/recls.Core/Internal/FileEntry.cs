
namespace Recls.Internal
{
	using System.IO;

	internal sealed class FileEntry
		: Entry
	{
		#region Construction
		internal FileEntry(FileInfo info, string searchRoot, SearchOptions options)
			: base(info, searchRoot, options)
		{}
		#endregion

		#region IEntry Members
		public override long Size
		{
			get { return ((FileInfo)Info_).Length; }
		}

		public override bool IsReadOnly
		{
			get { return ((FileInfo)Info_).IsReadOnly; }
		}

		public override bool IsDirectory
		{
			get { return false; }
		}
		#endregion
	}
}
