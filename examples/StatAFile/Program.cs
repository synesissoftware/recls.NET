
namespace StatAFile
{
	using Recls;

	using System;
	using System.Linq;
	using System.Reflection;

	class Program
	{
		static void Main(string[] args)
		{
			string path = Assembly.GetEntryAssembly().Location;

			if(0 != args.Length)
			{
				if("--help" == args[0])
				{
					Console.Out.WriteLine("USAGE: StatAFile [<path>]");
					Environment.Exit(0);
				}
				else
				{
					path = args[0];
				}
			}

			// 4. Get an entry representing a given path.

			IEntry entry = FileSearcher.Stat(path);

			if(null == entry)
			{
				Console.Error.Write("file not found");
			}
			else
			{
				Console.WriteLine("{0,20}:\t{1}", "Path", entry.Path);
				Console.WriteLine("{0,20}:\t{1}", "SearchRelativePath", entry.SearchRelativePath);
				Console.WriteLine("{0,20}:\t{1}", "Drive", entry.Drive);
				Console.WriteLine("{0,20}:\t{1}", "DirectoryPath", entry.DirectoryPath);
				Console.WriteLine("{0,20}:\t{1}", "Directory", entry.Directory);
				Console.WriteLine("{0,20}:\t{1}", "SearchDirectory", entry.SearchDirectory);
				Console.WriteLine("{0,20}:\t{1}", "UncDrive", entry.UncDrive);
				Console.WriteLine("{0,20}:\t{1}", "File", entry.File);
				Console.WriteLine("{0,20}:\t{1}", "FileName", entry.FileName);
				Console.WriteLine("{0,20}:\t{1}", "FileExtension", entry.FileExtension);
				Console.WriteLine("{0,20}:\t{1}", "CreationTime", entry.CreationTime);
				Console.WriteLine("{0,20}:\t{1}", "ModificationTime", entry.ModificationTime);
				Console.WriteLine("{0,20}:\t{1}", "LastAccessTime", entry.LastAccessTime);
				Console.WriteLine("{0,20}:\t{1}", "LastStatusChangeTime", entry.LastStatusChangeTime);
				Console.WriteLine("{0,20}:\t{1}", "Size", entry.Size);
				Console.WriteLine("{0,20}:\t{1}", "Attributes", entry.Attributes);
				Console.WriteLine("{0,20}:\t{1}", "IsReadOnly", entry.IsReadOnly);
				Console.WriteLine("{0,20}:\t{1}", "IsDirectory", entry.IsDirectory);
				Console.WriteLine("{0,20}:\t{1}", "IsUnc", entry.IsUnc);
				Console.WriteLine("{0,20}:\t[{1}]", "DirectoryParts", String.Join(", ", entry.DirectoryParts.ToArray()));
			}
		}
	}
}