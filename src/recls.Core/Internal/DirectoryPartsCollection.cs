
// Created: 24th September 2003
// Updated: 7th November 2009

namespace Recls.Internal
{
	using System;
	using System.Collections.Generic;

	internal sealed class DirectoryPartsCollection
		: IDirectoryPartsCollection
	{
		#region Fields
		readonly IList<string>	m_parts;
		#endregion

		#region Construction
		internal DirectoryPartsCollection(IList<string> parts)
		{
			m_parts = parts;
		}
		#endregion

		#region IDirectoryPartsCollection Members
		int IDirectoryPartsCollection.Count
		{
			get
			{
				return m_parts.Count;
			}
		}

		string IDirectoryPartsCollection.this[int index]
		{
			get
			{
				return m_parts[index];
			}
		}

		bool IDirectoryPartsCollection.Contains(string item)
		{
			return m_parts.Contains(item);
		}

		void IDirectoryPartsCollection.CopyTo(string[] array, int index)
		{
			m_parts.CopyTo(array, index);
		}
		#endregion

		#region IEnumerable<string> Members
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			return m_parts.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return m_parts.GetEnumerator();
		}
		#endregion
	}
}
