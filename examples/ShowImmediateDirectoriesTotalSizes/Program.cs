
namespace ShowImmediateDirectoriesTotalSizes
{
	using Recls;

	using System;
	using System.Linq;

	class Program
	{
		static void Main(string[] args)
		{
			// 3. Display the names and sizes of all the immediate
			// sub-directories of the current directory.

			FileSearcher.BreadthFirst.Search(
				null,                        // search current directory
				null,                        // all names
				SearchOptions.Directories,   // only want dirs; don't worry about inaccessible entries
				0                            // do not recurse
			)
				 .ForEach((e) => Console.WriteLine("{0} : {1}", e, FileSearcher.CalculateDirectorySize(e)));


			// 3. This can also be expressed in a more conventional syntax.

			foreach(IEntry entry in FileSearcher.BreadthFirst.Search(null, null, SearchOptions.Directories, 0))
			{
				Console.WriteLine("{0} : {1}", entry, FileSearcher.CalculateDirectorySize(entry));
			}
		}
	}
}
