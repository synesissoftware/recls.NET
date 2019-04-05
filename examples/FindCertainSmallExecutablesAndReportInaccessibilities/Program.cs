
namespace FindCertainSmallExecutablesAndReportInaccessibilities
{
	using Recls;

	using System;

	class Program
	{
		static void Main(string[] args)
		{
			// 2. Search for all writeable program and DLL files (including hidden)
			// smaller than 10k in the current directory and all
			// sub-directories up to 3 deep, reporting on any entries that
			// cannot be enumerated.

			var files = FileSearcher.DepthFirst.Search( // search in depth-first manner
				null,									// search current directory
				"*.exe|*.dll",							// all programs; all DLLs
				SearchOptions.IncludeHidden,			// include hidden files/directories
				3,										// descend at most 3 directories
				null,									// don't require progress
				delegate(object context, string path, Exception x)
				{
					// report on any entries that could not be enumerated, but ...
					Console.Error.WriteLine("could not enumerate {0}: {1}", path, x.Message);
					// ... continue the enumeration
					return ExceptionHandlerResult.ConsumeExceptionAndContinue;
				}
			);

			var results = from file in files
						  where file.Size < 10240 && !file.IsReadOnly
						  select file.SearchRelativePath;

			foreach(var path in results)
			{
				Console.WriteLine("entry: {0}", path);
			}
		}
	}
}
