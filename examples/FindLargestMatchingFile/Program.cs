
namespace FindLargestMatchingFile
{
	using Recls;

	using System;
	using System.Collections.Generic;
	using System.IO;

	class Program
	{
		private static void ShowUsageAndQuit(int exitCode)
		{
			Console.Out.WriteLine("USAGE: FindLargestMatchingFile [ ... options ... ] [<directory> [<pattern-1> [ ... <pattern-N>]]]");
			Console.Out.WriteLine();
			Console.Out.WriteLine("where options are:");
			Console.Out.WriteLine();
			Console.Out.WriteLine("\t--help\n\t\tShow this help and quit");
			Console.Out.WriteLine();

			Environment.Exit(exitCode);
		}

		static void Main(string[] args)
		{
			string directory = null;
			List<string> patterns = new List<string>();

			foreach(string arg in args)
			{
				if(0 != arg.Length && '-' == arg[0])
				{
					switch(arg)
					{
						case "--help":
							ShowUsageAndQuit(0);
							break;
						default:
							Console.Error.WriteLine("FindLargestMatchingFile: unrecognised argument {0}; use --help for usage", arg);
							break;
					}
				}
				else
				{
					if(null == directory && arg.IndexOfAny(new char[] { '?', '*' }) < 0)
					{
						directory = arg;
					}
					else
					{
						if(arg.IndexOfAny(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar } ) >= 0)
						{
							Console.Error.WriteLine("invalid pattern: {0}", arg);
							Environment.Exit(1);
						}
						else
						{
							patterns.Add(arg);
						}
					}
				}
			}

			if(0 == patterns.Count)
			{
				patterns.Add(FileSearcher.WildcardsAll);
			}

			IEntry largest = null;

			foreach(IEntry entry in FileSearcher.Search(directory, String.Join("|", patterns.ToArray())))
			{
				if(null == largest || largest.Size < entry.Size)
				{
					largest = entry;
				}
			}

			if(null == largest)
			{
				Console.Out.WriteLine("no matching entries found");
			}
			else
			{
				Console.Out.WriteLine("largest entry is {0}, which is {1} bytes", largest.SearchRelativePath, largest.Size);
			}
		}
	}
}
