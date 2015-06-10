
namespace FindWindowsFontFiles
{
	using Recls;

	using System;
    using System.Diagnostics;

	class Program
	{
		static void Main(string[] args)
		{
			// 1. List all font files in the windows directory or any of its subdirectories.

            foreach(IEntry entry in FileSearcher.Search(@"C:\windows", "*.fon|*.ttf|*.otf", SearchOptions.None, FileSearcher.UnrestrictedDepth, null, delegate(string path, Exception x) {

                Debug.WriteLine(String.Format("could not enumerate {0}: {1}", path, x.Message));

                return ExceptionHandlerResult.ConsumeExceptionAndContinue;
            }))
            {
                Console.Out.WriteLine(entry.Path);
            }
		}
	}
}
