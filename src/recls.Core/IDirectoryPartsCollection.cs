
// Created: 24th September 2003
// Updated: 7th November 2009

namespace Recls
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	///  Defines methods and properties for querying directory parts.
	/// </summary>
	public interface IDirectoryPartsCollection
		: IEnumerable<string>
	{
		/// <summary>
		///  The number of parts
		/// </summary>
		int Count { get; }

		/// <summary>
		///  Gets the part at the given <paramref name="index"/>.
		/// </summary>
		/// <param name="index">
		///  The index. Must be in the <b>range [0, <see cref="Count"/>)</b>.
		/// </param>
		/// <returns>
		///  The part corresponding to the given index.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///  If the index is out of range.
		/// </exception>
		string this[int index] { get; }

		/// <summary>
		///  Indicates whether the given part is contained in the
		///  parts collection.
		/// </summary>
		/// <param name="item">
		///  The item.
		/// </param>
		/// <returns>
		///  <b>true</b> if the item is contained; <b>false</b> otherwise.
		/// </returns>
		bool Contains(string item);

		/// <summary>
		///  Copies the parts to the given array.
		/// </summary>
		/// <param name="array">
		/// </param>
		/// <param name="index">
		/// </param>
		void CopyTo(string[] array, int index);
	}
}
