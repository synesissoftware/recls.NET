﻿
namespace ListInaccessibleDirectories
{
	using Recls;

	using System;
	using System.Diagnostics;
	using System.Linq;

	class Program
	{
		static void Main(string[] argv)
		{
			FileSearcher.Search(null, null,
				SearchOptions.Directories | SearchOptions.IncludeHidden | SearchOptions.IncludeSystem,
				FileSearcher.UnrestrictedDepth,
				(object context, string directory, int depth) =>
				{
					Trace.WriteLine("searching " + directory + " [" + depth + "]");
					return ProgressHandlerResult.Continue;
				},
				(context, path, x) =>
				{
					Console.WriteLine("could not access {0}: {1}", path, x.Message);
					return ExceptionHandlerResult.ConsumeExceptionAndContinue;
				}
			).ForEach((e) => e = null);
		}
	}
}
