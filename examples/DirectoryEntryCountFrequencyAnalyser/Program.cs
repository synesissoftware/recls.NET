
namespace DirectoryEntryCountFrequencyAnalyser
{
	using Recls;

	using System;
	using System.Collections.Generic;

	class Program
	{
		private static void ShowUsageAndQuit(int exitCode)
		{
			Console.Out.WriteLine("USAGE: DirectoryEntryCountFrequencyAnalyser [ ... options ... ] [<directory>]");
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
							Console.Error.WriteLine("DirectoryEntryCountFrequencyAnalyser: unrecognised argument {0}; use --help for usage", arg);
							break;
					}
				}
				else
				{
					if(null == directory)
					{
						directory = arg;
					}
					else
					{
						Console.Error.WriteLine("directory already specified; use --help for usage");
						Environment.Exit(1);
					}
				}
			}

			foreach(IEntry dir in FileSearcher.Search(directory, null, SearchOptions.Directories))
			{
				int n = 0;
				foreach(IEntry file in FileSearcher.Search(dir.Path, null, SearchOptions.Files, 0))
				{
					++n;
				}
				Console.Out.WriteLine("{0} has {1} file(s)", dir.SearchRelativePath, n);
			}
		}
	}
}
