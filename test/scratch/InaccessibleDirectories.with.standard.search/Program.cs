using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InaccessibleDirectories.with.standard.search
{
	class Program
	{
		static void Main(string[] args)
		{
			test_multipart_patterns();

			test_inaccessible();

		}

		private static void test_multipart_patterns()
		{
			try
			{
				DirectoryInfo di = new DirectoryInfo(@"H:\dev\bin");

				foreach(var v in di.GetFiles("*.dll;*.exe"))
				{
					Console.Out.WriteLine("entry: {0}", v);
				}
			}
			catch(Exception x)
			{
				Console.Error.WriteLine("exception: {0}", x.Message);
			}

			try
			{
				DirectoryInfo di = new DirectoryInfo(@"H:\dev\bin");

				foreach(var v in di.GetFiles("*.dll|*.exe"))
				{
					Console.Out.WriteLine("entry: {0}", v);
				}
			}
			catch(Exception x)
			{
				Console.Error.WriteLine("exception: {0}", x.Message);
			}

			try
			{
				DirectoryInfo di = new DirectoryInfo(@"H:\dev\bin");

				foreach(var v in di.GetFiles("*.dll"))
				{
					Console.Out.WriteLine("entry: {0}", v);
				}
			}
			catch(Exception x)
			{
				Console.Error.WriteLine("exception: {0}", x.Message);
			}

			try
			{
				DirectoryInfo di = new DirectoryInfo(@"H:\dev\bin");

				foreach(var v in di.GetFiles("*.exe"))
				{
					Console.Out.WriteLine("entry: {0}", v);
				}
			}
			catch(Exception x)
			{
				Console.Error.WriteLine("exception: {0}", x.Message);
			}
		}

		static void test_inaccessible()
		{
			try
			{
				int n = 0;
				foreach(string path in Directory.GetFiles(@"H:\dev\bin", "*.*", SearchOption.TopDirectoryOnly))
				{
					Console.WriteLine(path);
					++n;
				}
				Console.WriteLine("  total: {0}", n);
			}
			catch(Exception x)
			{
				Console.Error.WriteLine("exception: {0}", x.Message);
			}
		}
	}
}
