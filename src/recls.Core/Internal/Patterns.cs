
/* /////////////////////////////////////////////////////////////////////////
 * File:        Internal/Patterns.cs
 *
 * Created:     5th June 2009
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


namespace Recls.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Text;
	using System.Text.RegularExpressions;

	internal sealed class Patterns
	{
		#region construction
		internal Patterns(string patterns)
		{
			Debug.Assert(null != patterns);

			m_patterns = CreatePatterns_(SplitPatterns_(patterns));
		}
		#endregion

		#region operations
		internal bool MatchPath(string path)
		{
			Debug.Assert(null != path);

			if(0 == m_patterns.Length)
			{
				return true;
			}

			foreach(IPattern pattern in m_patterns)
			{
				if(pattern.Match(path))
				{
					return true;
				}
			}

			return false;
		}
		#endregion

		#region internal classes
		private interface IPattern
		{
			bool Match(string path);
		}

		private class SimplePattern
			: IPattern
		{
			#region construction
			internal SimplePattern(string pattern)
			{
				m_pattern = pattern;
			}
			#endregion

			#region IPattern members
			public bool Match(string path)
			{
				return 0 == String.Compare(m_pattern, path, Util.StringComparison);
			}
			#endregion

			#region fields
			readonly string m_pattern;
			#endregion
		}

#if UNUSED
		private class WildcardsAllPattern
			: IPattern
		{
			#region construction
			internal WildcardsAllPattern()
			{
			}
			#endregion

			#region IPattern members
			public bool Match(string path)
			{
				return true;
			}
			#endregion
		}
#endif

		private class RegexPattern
			: IPattern
		{
			#region construction
			internal RegexPattern(Regex pattern)
			{
				m_pattern = pattern;
			}
			#endregion

			#region IPattern members
			public bool Match(string path)
			{
				Match m = m_pattern.Match(path);

				return m.Success;
			}
			#endregion

			#region fields
			readonly Regex m_pattern;
			#endregion
		}
		#endregion

		#region implementation
		private static IPattern[] CreatePatterns_(string[] patterns)
		{
			Debug.Assert(null != patterns);

			List<IPattern>	patternObjects = new List<IPattern>(patterns.Length);

			foreach(string pattern in patterns)
			{
				if(pattern.IndexOfAny(Util.WildcardCharacters) < 0)
				{
					patternObjects.Add(new SimplePattern(pattern));
				}
				else
				{
					if(0 == String.CompareOrdinal(Util.WildcardsAll, pattern))
					{
						return new IPattern[0];
					}
					else
					{
						RegexOptions options = RegexOptions.Compiled;

						if(!Util.IsPathComparisonCaseSensitive)
						{
							options |= RegexOptions.IgnoreCase;
						}

						StringBuilder sb = new StringBuilder("^", pattern.Length);

						foreach(char c in pattern)
						{
							switch(c)
							{
								case '.':
									sb.Append(@"\.");
									break;
								case '?':
									sb.Append('.');
									break;
								case '*':
									sb.Append(".*");
									break;
								default:
									sb.Append(c);
									break;
							}
						}

						sb.Append("$");

						patternObjects.Add(new RegexPattern(new Regex(sb.ToString(), options)));
					}
				}
			}

			return patternObjects.ToArray();
		}

		private static string[] SplitPatterns_(string patterns)
		{
			Debug.Assert(null != patterns);

			return patterns.Split(Util.StrictPathSeparatorCharacters);
		}
		#endregion

		#region fields
		readonly IPattern[] m_patterns;
		#endregion
	}
}

/* ///////////////////////////// end of file //////////////////////////// */

