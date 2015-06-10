
namespace Recls.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Text;
	using System.Text.RegularExpressions;

	internal sealed class Patterns
	{
		#region Construction
		internal Patterns(string patterns)
		{
			Debug.Assert(null != patterns);

			m_patterns = CreatePatterns_(SplitPatterns_(patterns));
		}
		#endregion

		#region Operations
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

		#region Internal Classes
		private interface IPattern
		{
			bool Match(string path);
		}

		private class SimplePattern
			: IPattern
		{
			#region Construction
			internal SimplePattern(string pattern)
			{
				m_pattern = pattern;
			}
			#endregion

			#region IPattern Members
			public bool Match(string path)
			{
				return 0 == String.Compare(m_pattern, path, Util.StringComparison);
			}
			#endregion

			#region Fields
			readonly string m_pattern;
			#endregion
		}

#if UNUSED
		private class WildcardsAllPattern
			: IPattern
		{
			#region Construction
			internal WildcardsAllPattern()
			{
			}
			#endregion

			#region IPattern Members
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
			#region Construction
			internal RegexPattern(Regex pattern)
			{
				m_pattern = pattern;
			}
			#endregion

			#region IPattern Members
			public bool Match(string path)
			{
				Match m = m_pattern.Match(path);

				return m.Success;
			}
			#endregion

			#region Fields
			readonly Regex m_pattern;
			#endregion
		}
		#endregion

		#region Implementation
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

		#region Fields
		readonly IPattern[] m_patterns;
		#endregion
	}
}
