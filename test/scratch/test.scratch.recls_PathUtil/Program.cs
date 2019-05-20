namespace test.scratch.recls_PathUtil
{
	using Recls;

	using System;
	using System.IO;

	class Program
	{
		static void Main(string[] argv)
		{
			Console.WriteLine("PathUtil scratch test {0}{0}", Environment.NewLine);

			string path0 = @"H:";
			string path1 = @"H:\";
			string path2 = @"H:\directory1\directory2\";
			string path3 = @"H:\directory1\directory2\file3.ext";
			string path4 = @"\\server\share";
			string path5 = @"\\server\share\directory1";
			string path6 = @"\\server\share\directory1\directory2\file3.ext";
			string path7 = @"file3.ext";
			string path8 = @"directory1\directory2\";
			string path9 = @"directory1\directory2\file3.ext";


			// 1. Path.GetFullPath / PathUtil.GetAbsolutePath()
			Console.WriteLine("Example 1. Path.GetFullPath(){0}", Environment.NewLine);

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path0, Path.GetFullPath(path0));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path0, PathUtil.GetAbsolutePath(path0));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path1, Path.GetFullPath(path1));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path1, PathUtil.GetAbsolutePath(path1));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path2, Path.GetFullPath(path2));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path2, PathUtil.GetAbsolutePath(path2));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path3, Path.GetFullPath(path3));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path3, PathUtil.GetAbsolutePath(path3));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path4, Path.GetFullPath(path4));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path4, PathUtil.GetAbsolutePath(path4));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path5, Path.GetFullPath(path5));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path5, PathUtil.GetAbsolutePath(path5));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path6, Path.GetFullPath(path6));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path6, PathUtil.GetAbsolutePath(path6));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path7, Path.GetFullPath(path7));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path7, PathUtil.GetAbsolutePath(path7));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path8, Path.GetFullPath(path8));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path8, PathUtil.GetAbsolutePath(path8));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFullPath(\"{0}\"):\t{1}", path9, Path.GetFullPath(path9));
			Console.WriteLine("\tRecls.PathUtil.GetAbsolutePath(\"{0}\"):\t{1}", path9, PathUtil.GetAbsolutePath(path9));
			Console.WriteLine();


			// 2. Path.GetDirectoryName / PathUtil.GetDirectoryPath()
			Console.WriteLine("Example 2. Path.GetDirectoryName(){0}", Environment.NewLine);

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path0, Path.GetDirectoryName(path0));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path0, PathUtil.GetDirectoryPath(path0));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path1, Path.GetDirectoryName(path1));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path1, PathUtil.GetDirectoryPath(path1));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path2, Path.GetDirectoryName(path2));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path2, PathUtil.GetDirectoryPath(path2));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path3, Path.GetDirectoryName(path3));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path3, PathUtil.GetDirectoryPath(path3));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path4, Path.GetDirectoryName(path4));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path4, PathUtil.GetDirectoryPath(path4));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path5, Path.GetDirectoryName(path5));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path5, PathUtil.GetDirectoryPath(path5));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path6, Path.GetDirectoryName(path6));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path6, PathUtil.GetDirectoryPath(path6));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path7, Path.GetDirectoryName(path7));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path7, PathUtil.GetDirectoryPath(path7));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path8, Path.GetDirectoryName(path8));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path8, PathUtil.GetDirectoryPath(path8));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetDirectoryName(\"{0}\"):\t{1}", path9, Path.GetDirectoryName(path9));
			Console.WriteLine("\tRecls.PathUtil.GetDirectoryPath(\"{0}\"):\t{1}", path9, PathUtil.GetDirectoryPath(path9));
			Console.WriteLine();


			// 3. Path.GetFileName / PathUtil.GetFile()
			Console.WriteLine("Example 3. Path.GetFileName(){0}", Environment.NewLine);

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path0, Path.GetFileName(path0));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path0, PathUtil.GetFile(path0));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path1, Path.GetFileName(path1));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path1, PathUtil.GetFile(path1));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path2, Path.GetFileName(path2));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path2, PathUtil.GetFile(path2));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path3, Path.GetFileName(path3));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path3, PathUtil.GetFile(path3));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path4, Path.GetFileName(path4));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path4, PathUtil.GetFile(path4));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path5, Path.GetFileName(path5));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path5, PathUtil.GetFile(path5));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path6, Path.GetFileName(path6));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path6, PathUtil.GetFile(path6));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path7, Path.GetFileName(path7));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path7, PathUtil.GetFile(path7));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path8, Path.GetFileName(path8));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path8, PathUtil.GetFile(path8));
			Console.WriteLine();

			Console.WriteLine("\tSystem.IO.Path.GetFileName(\"{0}\"):\t{1}", path9, Path.GetFileName(path9));
			Console.WriteLine("\tRecls.PathUtil.GetFile(\"{0}\"):\t{1}", path9, PathUtil.GetFile(path9));
			Console.WriteLine();


			// 4. Path.GetFileName / PathUtil.GetFile()
			Console.WriteLine("Example 4. PathUtil.GetDrive(){0}", Environment.NewLine);

			//Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path0, PathUtil.GetDrive(path0));
			//Console.WriteLine();

			Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path1, PathUtil.GetDrive(path1));
			Console.WriteLine();

			Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path2, PathUtil.GetDrive(path2));
			Console.WriteLine();

			Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path3, PathUtil.GetDrive(path3));
			Console.WriteLine();

			Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path4, PathUtil.GetDrive(path4));
			Console.WriteLine();

			Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path5, PathUtil.GetDrive(path5));
			Console.WriteLine();

			Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path6, PathUtil.GetDrive(path6));
			Console.WriteLine();

			//Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path7, PathUtil.GetDrive(path7));
			//Console.WriteLine();

			//Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path8, PathUtil.GetDrive(path8));
			//Console.WriteLine();

			//Console.WriteLine("\tRecls.PathUtil.GetDrive(\"{0}\"):\t{1}", path9, PathUtil.GetDrive(path9));
			//Console.WriteLine();

		}
	}
}
