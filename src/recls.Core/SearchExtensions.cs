
namespace Recls
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	///  Defines extended search operations to be applied to the results of
	///  <see cref="FileSearcher"/>'s search operations.
	/// </summary>
	public static class SearchExtensions
	{
		/// <summary>
		///  Applies the given action to each element in the given sequence.
		/// </summary>
		/// <param name="sequence">
		///  The sequence whose entries are to be enumerated.
		/// </param>
		/// <param name="action">
		///  The action to be applied to each element in the sequence.
		/// </param>
		public static void ForEach(this IEnumerable<IEntry> sequence, Action<IEntry> action)
		{
			foreach(IEntry entry in sequence)
			{
				action(entry);
			}
		}

		/// <summary>
		///  Applies the given transformation to each element in the given
		///  sequence to form a sequence of the transformed type.
		/// </summary>
		/// <typeparam name="TTarget">
		///  Type of the elements in the target sequence.
		/// </typeparam>
		/// <param name="sequence">
		///  The sequence whose entries are to be transformed.
		/// </param>
		/// <param name="function">
		///  The function used to translate the entries into the target type.
		/// </param>
		/// <returns>
		///  A sequence containing values obtained by applying the given transformation function
		/// </returns>
		public static IEnumerable<TTarget> Select<TTarget>(this IEnumerable<IEntry> sequence, Func<IEntry, TTarget> function)
		{
			foreach(IEntry entry in sequence)
			{
				yield return function(entry);
			}
		}

		/// <summary>
		///  Filters the entries in the source sequence based on the
		///  filtering predicate, to form a resulting sequence of filtered
		///  entries.
		/// </summary>
		/// <param name="sequence">
		///  The sequence whose entries are to be filtered.
		/// </param>
		/// <param name="predicate">
		///  The predicate that determines which entries are selected into
		///  the result sequence.
		/// </param>
		/// <returns>
		///  A sequence containing only entries that match the given
		///  predicate.
		/// </returns>
#if ANONYMOUS_DELEGATES_RECOGNISE_PREDICATE
		public static IEnumerable<IEntry> Where(this IEnumerable<IEntry> sequence, Predicate<IEntry> predicate) // This clashes with Linq's Enumerable.Where
#else
		public static IEnumerable<IEntry> Where(this IEnumerable<IEntry> sequence, Func<IEntry, bool> predicate)
#endif
		{
			foreach(IEntry entry in sequence)
			{
				if(predicate(entry))
				{
					yield return entry;
				}
			}
		}
	}
}
