
namespace Test.Scratch.recls.Core
{
	using global::Recls;

	using System;
	using System.IO;

	class Program
	{
		static int Main(string[] args)
		{
			try
			{
				if (2 != args.Length)
				{
					Console.Error.WriteLine("USAGE: {0} <directory> <patterns-separated-by-comma>", System.Diagnostics.Process.GetCurrentProcess().ProcessName);

					return 1;
				}

				string directory	=	args[0];
				string patterns		=	args[1];

				Console.Out.WriteLine("Searching in '{0}' for files matching pattern(s) '{1}'", directory, patterns);

				var search = Api.Search(directory, patterns);

				foreach (IEntry entry in search)
				{
					Console.Out.WriteLine("\t{0}", entry.SearchRelativePath);
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
