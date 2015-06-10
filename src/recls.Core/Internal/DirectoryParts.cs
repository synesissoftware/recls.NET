
// Created: 24th September 2003
// Updated: 7th November 2009

namespace Recls.Internal
{
	using System;
	using System.Collections.Generic;

	internal sealed class DirectoryParts
		: IDirectoryParts
	{
		#region Fields
		readonly IList<string>	m_parts;
		#endregion

		#region Construction
		internal DirectoryParts(IList<string> parts)
		{
			m_parts = parts;
		}
		#endregion

		#region IDirectoryParts Members
		int IDirectoryParts.Count
		{
			get
			{
				return m_parts.Count;
			}
		}

		string IDirectoryParts.this[int index]
		{
			get
			{
				return m_parts[index];
			}
		}

		bool IDirectoryParts.Contains(string item)
		{
			return m_parts.Contains(item);
		}

		void IDirectoryParts.CopyTo(string[] array, int index)
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
