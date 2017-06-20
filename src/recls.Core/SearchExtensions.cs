
/* /////////////////////////////////////////////////////////////////////////
 * File:        SearchExtensions.cs
 *
 * Created:     3rd July 2009
 * Updated:     20th June 2017
 *
 * Home:        http://recls.net/
 *
 * Copyright (c) 2009-2017, Matthew Wilson and Synesis Software
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are
 * met:
 *
 * - Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * - Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following disclaimer in the
 *   documentation and/or other materials provided with the distribution.
 * - Neither the name(s) of Matthew Wilson and Synesis Software nor the
 *   names of any contributors may be used to endorse or promote products
 *   derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
 * IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
 * PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * ////////////////////////////////////////////////////////////////////// */


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

/* ///////////////////////////// end of file //////////////////////////// */

