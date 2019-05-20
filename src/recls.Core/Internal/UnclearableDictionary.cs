
/* /////////////////////////////////////////////////////////////////////////
 * File:        Internal/UnclearableDictionary.cs
 *
 * Created:     20th May 2019
 * Updated:     20th May 2019
 *
 * Home:        http://recls.net/
 *
 * Copyright (c) 2019, Matthew Wilson and Synesis Software
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


namespace Recls.Internal
{
	using global::System.Collections.Generic;

	internal class UnclearableDictionary<T_key, T_value>
		: IDictionary<T_key, T_value>
	{
		#region fields

		private readonly IDictionary<T_key, T_value>    m_dict;
		#endregion

		#region construction

		internal UnclearableDictionary()
		{
			m_dict = new Dictionary<T_key, T_value>();
		}
		#endregion

		#region IDictionary<>

		void IDictionary<T_key, T_value>.Add(T_key key, T_value value)
		{
			m_dict.Add(key, value);
		}

		bool IDictionary<T_key, T_value>.ContainsKey(T_key key)
		{
			return m_dict.ContainsKey(key);
		}

		ICollection<T_key> IDictionary<T_key, T_value>.Keys
		{
			get
			{
				return m_dict.Keys;
			}
		}

		bool IDictionary<T_key, T_value>.Remove(T_key key)
		{
			return m_dict.Remove(key);
		}

		bool IDictionary<T_key, T_value>.TryGetValue(T_key key, out T_value value)
		{
			return m_dict.TryGetValue(key, out value);
		}

		ICollection<T_value> IDictionary<T_key, T_value>.Values
		{
			get
			{
				return m_dict.Values;
			}
		}

		T_value IDictionary<T_key, T_value>.this[T_key key]
		{
			get
			{
				return m_dict[key];
			}
			set
			{
				m_dict[key] = value;
			}
		}

		void ICollection<KeyValuePair<T_key, T_value>>.Add(KeyValuePair<T_key, T_value> item)
		{
			m_dict.Add(item);
		}

		void ICollection<KeyValuePair<T_key, T_value>>.Clear()
		{
		}

		bool ICollection<KeyValuePair<T_key, T_value>>.Contains(KeyValuePair<T_key, T_value> item)
		{
			return m_dict.Contains(item);
		}

		void ICollection<KeyValuePair<T_key, T_value>>.CopyTo(KeyValuePair<T_key, T_value>[] array, int arrayIndex)
		{
			m_dict.CopyTo(array, arrayIndex);
		}

		int ICollection<KeyValuePair<T_key, T_value>>.Count
		{
			get
			{
				return m_dict.Count;
			}
		}

		bool ICollection<KeyValuePair<T_key, T_value>>.IsReadOnly
		{
			get
			{
				return m_dict.IsReadOnly;
			}
		}

		bool ICollection<KeyValuePair<T_key, T_value>>.Remove(KeyValuePair<T_key, T_value> item)
		{
			return m_dict.Remove(item);
		}

		IEnumerator<KeyValuePair<T_key, T_value>> IEnumerable<KeyValuePair<T_key, T_value>>.GetEnumerator()
		{
			return m_dict.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return m_dict.GetEnumerator();
		}
		#endregion
	}
}
