﻿
namespace Test.Component.SearchingInNonexistentDirectory
{
	using global::Recls;

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Text;
	using System.Threading.Tasks;

	class Program
	{
		static int Main(string[] args)
		{
			try
			{
				string tempDir = Path.GetTempPath();

				Guid guid = Guid.NewGuid();

				string directory = Path.Combine(tempDir, guid.ToString());

				SearchOptions options = SearchOptions.None;

				options |= SearchOptions.TreatMissingDirectoryAsEmpty;

				foreach (IEntry entry in FileSearcher.Search(directory, FileSearcher.WildcardsAll, options))
				{
					Console.Out.WriteLine("\t{0}", entry);
				}

				return 0;
			}
			catch (Exception x)
			{
				Console.Error.WriteLine("exception ({0}): {1}", x.GetType().FullName, x.Message);

				return 1;
			}
		}
	}
}
