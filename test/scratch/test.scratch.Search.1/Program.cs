namespace test.scratch.Search._1
{
	using Recls;

	using SynSoft.Text;

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Main_(args);
			}
			catch (Exception x)
			{
				Console.Error.WriteLine("Exception: {0} ({1}", x.Message, x.GetType());

				System.Environment.Exit(1);
			}
		}

		private static void Main_(string[] args)
		{
			try
			{
				FileSearcher.Search("abc", null);
			}
			catch
			{
			}

			PlayWithGetDirectoryPath();

			PlayWithCanonicalizePath();

			PlayWithDeriveRelativePath();

			PlayWithStat();

#if NONE2
			{
				foreach(IEntry entry in FileSearcher.Search(null, "bin/*.dll"))
				{
					Console.WriteLine(entry.Path);
					Console.WriteLine(entry.SearchDirectory);
					Console.WriteLine(entry.SearchRelativePath);
				}

				return;
			}

			{
				//Environment.CurrentDirectory = @"C:\Documents and Settings\matthew.SYNESISSOFTWARE";
				//Environment.CurrentDirectory = @"\\192.168.0.8\vss\exchange";
				//Environment.CurrentDirectory = @"H:\STLSoft\Releases";

				//string p;
				//p = System.IO.Path.GetDirectoryName(null);
				//p = System.IO.Path.GetDirectoryName("<>");
				//p = System.IO.Path.GetDirectoryName("");

				DisplayEntry("H:");
				DisplayEntry("C:");

				DisplayEntry(@"\\192.168.0.8\vss");
				DisplayEntry(@"\\192.168.0.8\vss\");
				DisplayEntry(@"\\192.168.0.8\vss/exchange");

				DisplayEntry(null);
				DisplayEntry("/");
				DisplayEntry(@"H:\dev\prefixes.txt");
				DisplayEntry(@"H:\dev\");
				DisplayEntry(@"H:/dev\");
				DisplayEntry(@"H:\dev");
				DisplayEntry(@"H:/dev");
				DisplayEntry(@"H:\");
				DisplayEntry(@"H:/");
				DisplayEntry("/");

				return;
			}

#if NONE
			{
				foreach(IEntry entry in FileSearcher.BreadthFirst.Search(
					@"\\192.168.0.8\vss\synesis",
					null,
					SearchOptions.Directories | SearchOptions.IgnoreInaccessibleNodes,
					0
				))
				{
					Console.WriteLine("{0} : {1}", entry, FileSearcher.CalculateDirectorySize(entry.Path));
				}

				return;
			}
#endif

#if NONE
			{
				FileSearcher.BreadthFirst.Search(
					null,
					null,
					SearchOptions.Directories | SearchOptions.IgnoreInaccessibleNodes,
					0
				)
					.ForEach((d) => Console.WriteLine("{0} : {1}", d.Path, FileSearcher.CalculateDirectorySize(d.Path)));

				return;
			}
#endif

#if NONE
			{
				FileSearcher.BreadthFirst.Search(
					null,
					null,
					SearchOptions.Directories | SearchOptions.MarkDirectories | SearchOptions.IgnoreInaccessibleNodes,
					0
				)
					.ForEach((d) => Console.WriteLine("{0} : {1}", d.Path, FileSearcher.CalculateDirectorySize(d.Path, FileSearcher.UnrestrictedDepth)));

				return;
			}
#endif

#if NONE
			{
				foreach(IEntry entry in FileSearcher.BreadthFirst.Search(null, null, SearchOptions.Directories | SearchOptions.IgnoreInaccessibleNodes, 0))
				{
					Console.WriteLine("{0} : {1}", entry.Path, FileSearcher.CalculateDirectorySize(entry, FileSearcher.UnrestrictedDepth));
				}

				return;
			}
#endif // NONE
#endif // NONE2

			string directory   =   @"H:\freelibs\recls\main\100\recls.NET";
			string patterns    =   "*.cs";///"*.cs|*.csproj";

			directory = @"H:\STLSoft\Releases\1.9\STLSoft\include";
			patterns = "*.hpp|*.h";
			patterns = "*stl*";

			Console.Out.WriteLine();


			var files = FileSearcher.Search(directory, patterns);

			var expr = from file in files
					   where file.IsReadOnly
					   select file.SearchRelativePath;

			foreach(var path in expr)
			{
				Console.WriteLine(path);
			}

			Console.Out.WriteLine();

			int 				count	=	0;
			const int			Depth	=	3;
			const SearchOptions options =	SearchOptions.Directories | SearchOptions.Files;

			Console.Out.WriteLine();
			Console.Out.WriteLine("Depth-first search:");

			count = 0;
			foreach(IEntry entry in FileSearcher.DepthFirst.Search(directory, patterns, options, Depth))
			{
				//Console.Out.WriteLine("\t{0}", entry.Path);
				DisplayEntry(entry.Path);
				++count;
			}
			Console.Out.WriteLine();
			Console.Out.WriteLine("count (DFS): {0}\n", count);


			Console.Out.WriteLine();
			Console.Out.WriteLine("Breadth-first search:");

			count = 0;
			foreach(IEntry entry in FileSearcher.BreadthFirst.Search(directory, patterns, options, Depth))
			{
				Console.Out.WriteLine("\t{0}", entry.SearchRelativePath);
				++count;
			}
			Console.Out.WriteLine();
			Console.Out.WriteLine("count (BFS): {0}\n", count);

			//return;


			Console.Out.WriteLine();
			Console.Out.WriteLine("Breadth-first search (directories-only):");

			directory = @"\\galibier\H$\STLSoft\Releases\1.9\STLSoft\include";

			foreach(IEntry entry in FileSearcher.BreadthFirst.Search(directory, patterns, SearchOptions.Directories | SearchOptions.MarkDirectories, Depth))
			{
#if !NONE
				Console.Out.WriteLine("\tToString()\t{0}", entry.ToString());
				Console.Out.WriteLine("\tSearchDirectory\t{0}", entry.SearchDirectory);
				Console.Out.WriteLine("\tDrive\t{0}", entry.Drive);
				Console.Out.WriteLine("\tDirectory\t{0}", entry.Directory);
				Console.Out.WriteLine("\tDirectoryPath\t{0}", entry.DirectoryPath);
				Console.Out.WriteLine("\tFileName\t{0}", entry.FileName);
				Console.Out.WriteLine("\tFileExt\t{0}", entry.FileExtension);
				Console.Out.WriteLine();

				Console.Out.WriteLine("\tPath\t{0}", entry.Path);
				Console.Out.WriteLine("\tSearchRelativePath\t{0}", entry.SearchRelativePath);
				Console.Out.WriteLine("\tFile\t{0}", entry.File);
				Console.Out.WriteLine();
				Console.Out.WriteLine();
#endif // NONE

				Console.Out.WriteLine("\tSearchRelativePath\t{0}", entry.SearchRelativePath);
			}

			directory = @"\\galibier\H$\STLSoft\Releases\1.9\STLSoft\include";
			patterns = "*.hpp|*.h";

#if NONE
			Console.Out.WriteLine();
			Console.Out.WriteLine("Breadth-first search (read only entries):");

			foreach(IEntry entry in Filters.ReadOnly(FileSearcher.BreadthFirst.Search(directory, patterns)))
			{
				Console.Out.WriteLine("\t{0}", entry.SearchRelativePath);
			}

			Console.Out.WriteLine();
			Console.Out.WriteLine("Breadth-first search (entries > 1999 bytes and < 2200 bytes):");

			foreach(IEntry entry in Filters.SizeGreaterThan(1999, Filters.SizeLessThan(2200, FileSearcher.BreadthFirst.Search(directory, patterns))))
			{
				Console.Out.WriteLine("\t{0}", entry.SearchRelativePath);
			}

			Console.Out.WriteLine();
			Console.Out.WriteLine("Breadth-first search with composite lambda:");

			foreach(IEntry entry in Filters.Select(FileSearcher.BreadthFirst.Search(directory, patterns), (entry) => entry.Size > 10000 && !entry.IsReadOnly ))
			{
				Console.Out.WriteLine("\t{0}", entry.SearchRelativePath);
			}

			Console.Out.WriteLine();
			Console.Out.WriteLine("Breadth-first search via ForEach:");

			FileSearcher.BreadthFirst.Search(directory, patterns).ForEach((entry) => Console.Out.WriteLine(entry));
#endif // NONE

			Console.Out.WriteLine();
			Console.Out.WriteLine("Breadth-first search via GetEnumerator");

			IEnumerator en = FileSearcher.BreadthFirst.Search(directory, patterns).GetEnumerator();
			while(en.MoveNext())
			{
				IEntry entry = (IEntry)en.Current;

				if(!entry.IsReadOnly)
				{
					Console.Out.WriteLine(entry.SearchRelativePath);
				}
			}

			Console.Out.WriteLine();
			Console.Out.WriteLine("Breadth-first search with explicit filtering:");

			foreach(IEntry entry in FileSearcher.BreadthFirst.Search(directory, patterns))
			{
				if(!entry.IsReadOnly)
				{
					Console.Out.WriteLine(entry.SearchRelativePath);
				}
			}

			Console.Out.WriteLine();
			Console.Out.WriteLine("Breadth-first search via Filter and ForEach:");

			//FileSearcher.BreadthFirst.Search(directory, patterns)
			//    .Where((entry) => !entry.IsReadOnly)
			//    .Select((entry) => entry.SearchRelativePath)
			//    ..ForEach((path) => Console.Out.WriteLine(path));
			foreach(string path in FileSearcher.BreadthFirst.Search(directory, patterns).Where((e) => !e.IsReadOnly).Select((e) => e.SearchRelativePath))
			{
				Console.WriteLine(path);
			}
		}

		private static void PlayWithGetDirectoryPath()
		{
			string s;

			s = PathUtil.GetDirectoryPath(@"\\server\share/abc/def/");

			PathUtil.DeriveRelativePath(@"C:/abc", @"C:/abc/def");

			s = PathUtil.GetDirectoryPath(@"\abc");
			s = PathUtil.GetDirectoryPath(@"\");
			s = PathUtil.GetDirectoryPath(@"\\server\share\abc");
			s = PathUtil.GetDirectoryPath(@"\\server\share\");

			try
			{
				s = PathUtil.GetDirectoryPath("");
			}
			catch(Exception)
			{ }
		}

		private static void PlayWithCanonicalizePath()
		{
			string s;

			s = PathUtil.CanonicalizePath(null);
			s = PathUtil.CanonicalizePath(@"");

			s = PathUtil.CanonicalizePath(@"C:\abc\def\");
			s = PathUtil.CanonicalizePath(@"C:\abc\def");
			s = PathUtil.CanonicalizePath(@"C:\abc\");
			s = PathUtil.CanonicalizePath(@"C:\abc");
			s = PathUtil.CanonicalizePath(@"C:\");
			s = PathUtil.CanonicalizePath(@"C:/");
			s = PathUtil.CanonicalizePath(@"C:");
			s = PathUtil.CanonicalizePath(@"\");
			s = PathUtil.CanonicalizePath(@"/");

			s = PathUtil.CanonicalizePath(@"abc\def\");
			s = PathUtil.CanonicalizePath(@"abc\def");
			s = PathUtil.CanonicalizePath(@"abc\");
			s = PathUtil.CanonicalizePath(@"abc");

			s = PathUtil.CanonicalizePath(@"\\server\share\dir1\file.ext");
			s = PathUtil.CanonicalizePath(@"\\server\share\dir1\");
			s = PathUtil.CanonicalizePath(@"\\server\share\dir1");
			s = PathUtil.CanonicalizePath(@"\\server\share\");
			s = PathUtil.CanonicalizePath(@"\\server\share");

			try
			{
				s = PathUtil.CanonicalizePath(@"\\server\");
			}
			catch(Exception)
			{ }
			try
			{
				s = PathUtil.CanonicalizePath(@"\\server");
			}
			catch(Exception)
			{ }
			try
			{
				s = PathUtil.CanonicalizePath(@"\\");
			}
			catch(Exception)
			{ }
		}

		private static void PlayWithDeriveRelativePath()
		{
			string s;

			s = PathUtil.DeriveRelativePath(null, @"C:\abc");
			s = PathUtil.DeriveRelativePath("", @"C:\abc");
			s = PathUtil.DeriveRelativePath(@"H:\", @"C:\abc");
			s = PathUtil.DeriveRelativePath(@"H:\abc", @"C:\abc");
			s = PathUtil.DeriveRelativePath(@"\\server\share", @"C:\abc");
			s = PathUtil.DeriveRelativePath(@"\\server\share\", @"C:\abc");
			s = PathUtil.DeriveRelativePath(@"\\server\share\abc", @"C:\abc");

			s = PathUtil.DeriveRelativePath(@"C:\abc", @"C:\abc");
			s = PathUtil.DeriveRelativePath(@"C:\", @"C:\abc");
			s = PathUtil.DeriveRelativePath(@"C:\abc", @"C:\");
			s = PathUtil.DeriveRelativePath(@"C:\abc\def", @"C:\abc");
			s = PathUtil.DeriveRelativePath(@"C:\abc\def", @"C:\abc\");
			s = PathUtil.DeriveRelativePath(@"C:\abc\def\", @"C:\abc");

			s = PathUtil.DeriveRelativePath(@"C:\abc", @"C:\abc\def");
			s = PathUtil.DeriveRelativePath(@"C:\", @"C:\abc\def");

			s = PathUtil.DeriveRelativePath(@"C:\abc\def", @"C:\abc\ghi");
			s = PathUtil.DeriveRelativePath(@"C:\abc\def\", @"C:\abc\..\abc\ghi");
			s = PathUtil.DeriveRelativePath(@"C:\abc\def", @"C:\abc\..\abc\ghi");
			s = PathUtil.DeriveRelativePath(@"C:\abc\", @"C:\abc\..\abc\ghi");
			s = PathUtil.DeriveRelativePath(@"C:\abc", @"C:\abc\..\abc\ghi");
			s = PathUtil.DeriveRelativePath(@"C:\", @"C:\abc\..\abc\ghi");

			s = PathUtil.DeriveRelativePath(@"C:\does-not-exist", @"C:\does-not-exist");
			s = PathUtil.DeriveRelativePath(@"C:\does-not-exist\", @"C:\does-not-exist\");
			s = PathUtil.DeriveRelativePath(@"/abc/def", @"/abc/ghi");

			s = PathUtil.DeriveRelativePath(@"C:\does-not-exist", @"C:\does-not-exist\def");
			s = PathUtil.DeriveRelativePath(@"C:\does-not-exist\def", @"C:\does-not-exist");
		}

		private static void PlayWithStat()
		{
			IEntry entry;

			entry = FileSearcher.Stat(@"C:\does-not-exist\abc.def");
			DisplayEntry(entry);

			entry = FileSearcher.Stat(@"C:\");
			DisplayEntry(entry);

			entry = FileSearcher.Stat(@"/");
			DisplayEntry(entry);

			entry = FileSearcher.Stat(@"\\no-server\noshare\");
			DisplayEntry(entry);

			entry = FileSearcher.Stat(@"\\galibier\H$\");
			DisplayEntry(entry);

			entry = FileSearcher.Stat(@"\\no-server\noshare");
			DisplayEntry(entry);

			entry = FileSearcher.Stat(@"\\galibier\H$");
			DisplayEntry(entry);
		}

		private static void DisplayEntry(string path)
		{
			Console.WriteLine("path:\t{0}", path);

			IEntry entry = FileSearcher.Stat(path);

			DisplayEntry(entry);
		}

		private static void DisplayEntry(IEntry entry)
		{
			if(null == entry)
			{
				Console.WriteLine("no matching entry");
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine("IEntry:");
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
#if USE_ARRAY_FOR_DIRECTORY_PARTS
				Console.WriteLine("{0,20}:\t[{1}]", "DirectoryParts", String.Join(", ", entry.DirectoryParts));
#else
				Console.WriteLine("{0,20}:\t[{1}]", "DirectoryParts", StringUtil.Join(", ", entry.DirectoryParts));
#endif

				Console.WriteLine();
				Console.WriteLine(entry.Directory);
				Console.WriteLine(StringUtil.Join("", entry.DirectoryParts));

				Console.WriteLine();
				Console.WriteLine(entry.DirectoryPath);
				Console.WriteLine(entry.Drive + entry.Directory);

				Console.WriteLine();
				Console.WriteLine(entry.Path);
				Console.WriteLine(entry.Drive + entry.Directory + entry.File);
			}
			Console.WriteLine();
		}
	}
}
