
namespace FindEmptyDirectories
{
	using Recls;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	class Program
	{
		static void Main(string[] args)
		{
			SearchOptions all = SearchOptions.IncludeHidden | SearchOptions.IncludeSystem | SearchOptions.IgnoreInaccessibleNodes;

			foreach(IEntry directory in FileSearcher.Search(null, null, SearchOptions.Directories | all))
			{
				bool fileFound = false;

				foreach(IEntry file in FileSearcher.Search(directory.Path, null, SearchOptions.Files | all))
				{
					fileFound = true;
					break;
				}

				if(!fileFound)
				{
					Console.Out.WriteLine(directory);
				}
			}
		}
	}
}
