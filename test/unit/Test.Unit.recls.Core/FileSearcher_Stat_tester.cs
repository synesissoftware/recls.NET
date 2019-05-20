
// Created: 31st July 2009
// Updated: 19th November 2017

#if PSEUDO_UNIX
#else // PSEUDO_UNIX
#endif // PSEUDO_UNIX

namespace Test.Unit.recls.Core
{
	using Recls;
	using IEntry2 = global::Recls.IEntry2_1;

#if NUNIT
	using Assert = global::NUnit.Framework.Assert;
	using TestClass = global::NUnit.Framework.TestFixtureAttribute;
	using TestInitialize = global::NUnit.Framework.SetUpAttribute;
	using TestMethod = global::NUnit.Framework.TestAttribute;
	using ExpectedException = global::NUnit.Framework.ExpectedExceptionAttribute;
#else
	using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;

	[TestClass]
	public class FileSearcher_Stat_tester
	{
		#region types

		internal static class Util
		{
#if PSEUDO_UNIX
			private const char				Slash	=	'/';
			private static readonly char[]	Slashes =	new char[] { '/' };
#else // ? PSEUDO_UNIX
			private const char				Slash	=	'\\';
			private static readonly char[]	Slashes =	new char[] { '\\', '/' };
#endif // PSEUDO_UNIX

			internal static string EnsureDirEnd(string s)
			{
				Debug.Assert(!String.IsNullOrEmpty(s));

				foreach(char slash in Slashes)
				{
					if(slash == s[s.Length - 1])
					{
						return s;
					}
				}

				return s + Slash;
			}
		}
		#endregion

		#region fields

		private string	cwd 		=	null;
		private string	parent_dir	=	null;
		private string	root		=	null;
		private string	basename	=	null;
		private string	basename_nx =	null;
		#endregion

		[TestInitialize]
		public void setup()
		{
			cwd 		=	Environment.CurrentDirectory;
			parent_dir	=	Path.GetDirectoryName(cwd);
			root		=	Path.GetPathRoot(cwd);
			basename	=	Path.GetFileName(cwd);
			basename_nx =	Path.GetFileNameWithoutExtension(cwd);

#if PSEUDO_UNIX
			cwd = PathUtilities.CanonicalizePath(cwd);
			parent_dir = PathUtilities.CanonicalizePath(parent_dir);
			root = PathUtilities.CanonicalizePath(root);
#endif // PSEUDO_UNIX
			root = PathUtilities.CanonicalizePath(root);
		}

		[TestMethod]
		public void test_Root()
		{
			IEntry	e = FileSearcher.Stat("/");
			IEntry2 e2 = e as IEntry2;

			Assert.AreEqual(FileAttributes.Directory, (FileAttributes.Directory & e.Attributes));
			//e.CreationTime);
#if PSEUDO_UNIX
			Assert.AreEqual("/", e.Directory);
#else // PSEUDO_UNIX
			Assert.AreEqual("\\", e.Directory);
#endif // PSEUDO_UNIX
			Assert.AreEqual(1, e.DirectoryParts.Count);
			Assert.AreEqual(root, e.DirectoryPath);
			Assert.AreEqual(root[0], e.Drive[0]);
			Assert.AreEqual(root.Length, e.Drive.Length + 1);
			Assert.AreEqual("", e.File);
			Assert.AreEqual("", e.FileExtension);
			Assert.AreEqual("", e.FileName);
			Assert.IsTrue(e2.Existed);
			Assert.IsTrue(e.IsDirectory);
			Assert.IsFalse(e2.IsFile);
			//e.IsReadOnly);
			//e.IsUnc);
			//e.LastAccessTime);
			//e.LastStatusChangeTime);
			//e.ModificationTime);
			Assert.AreEqual(root, e.Path);
			Assert.AreEqual(root, e.SearchDirectory);
			Assert.AreEqual("", e.SearchRelativePath);
			Assert.AreEqual(0, e.Size);
			Assert.AreEqual(null, e.UncDrive);
		}

		[TestMethod]
		public void test_Share()
		{
			string knownSharePath = Environment.GetEnvironmentVariable("RECLS_NET_TESTING_KNOWN_SHARE_PATH");

			if(String.IsNullOrWhiteSpace(knownSharePath))
			{
				Assert.Inconclusive("share path test inconclusive because environment variable RECLS_NET_TESTING_KNOWN_SHARE_PATH not defined");
			}
			else if(!Directory.Exists(knownSharePath))
			{
				Assert.Inconclusive(String.Format("share path test inconclusive because directory specified by environment variable RECLS_NET_TESTING_KNOWN_SHARE_PATH	- '{0}' - does not exist", knownSharePath));
			}
			else
			{
				try
				{
					string	sharePath	=	knownSharePath;
					IEntry	e			=	FileSearcher.Stat(sharePath);
					IEntry2 e2          =   e as IEntry2;

					if(null != e)
					{
						Assert.AreEqual(FileAttributes.Directory, (FileAttributes.Directory & e.Attributes));
						//e.CreationTime);
#if PSEUDO_UNIX
						Assert.AreEqual("/", e.Directory);
#else // PSEUDO_UNIX
						Assert.AreEqual("\\", e.Directory);
#endif // PSEUDO_UNIX
						Assert.AreEqual(1, e.DirectoryParts.Count);
						Assert.AreEqual(sharePath, e.DirectoryPath);
						Assert.AreEqual(sharePath.Substring(0, sharePath.Length - 1), e.Drive);
						Assert.AreEqual("", e.File);
						Assert.AreEqual("", e.FileExtension);
						Assert.AreEqual("", e.FileName);
						Assert.IsTrue(e2.Existed);
						Assert.IsTrue(e.IsDirectory);
						Assert.IsFalse(e2.IsFile);
						//e.IsReadOnly);
						Assert.IsTrue(e.IsUnc);
						//e.LastAccessTime);
						//e.LastStatusChangeTime);
						//e.ModificationTime);
						Assert.AreEqual(sharePath, e.Path);
						Assert.AreEqual(sharePath, e.SearchDirectory);
						Assert.AreEqual("", e.SearchRelativePath);
						Assert.AreEqual(0, e.Size);
						Assert.AreEqual(e.Drive, e.UncDrive);
					}
				}
				catch(OutOfMemoryException)
				{
					throw;
				}
				catch(System.UnauthorizedAccessException x)
				{
					Assert.Inconclusive("test involving sharing of '{0}' inconclusive: {1}", knownSharePath, x);
				}
			}
		}

		[TestMethod]
		public void test_Cwd()
		{
			IEntry	e   =   FileSearcher.Stat(cwd);
			IEntry2 e2  =   e as IEntry2;

			Assert.AreEqual(FileAttributes.Directory, (FileAttributes.Directory & e.Attributes));
			//e.CreationTime);
			//Assert.AreEqual(cwd.Substring(e.Drive.Length), e.Directory);
			//Assert.AreEqual(1, e.DirectoryParts.Length);
			Assert.AreEqual(Util.EnsureDirEnd(parent_dir), e.DirectoryPath);
			//Assert.AreEqual(root, e.Drive);
			Assert.AreEqual(basename, e.File);
			//Assert.AreEqual("", e.FileExtension);
			Assert.AreEqual(basename_nx, e.FileName);
			Assert.IsTrue(e2.Existed);
			Assert.IsTrue(e.IsDirectory);
			Assert.IsFalse(e2.IsFile);
			//e.IsReadOnly);
			Assert.IsFalse(e.IsUnc);
			//e.LastAccessTime);
			//e.LastStatusChangeTime);
			//e.ModificationTime);
			Assert.AreEqual(cwd, e.Path);
			//Assert.AreEqual(root, e.SearchDirectory);
			//Assert.AreEqual("", e.SearchRelativePath);
			Assert.AreEqual(0, e.Size);
			Assert.AreEqual(null, e.UncDrive);

			Assert.AreEqual(e.Path, e.DirectoryPath + e.File);
			Assert.AreEqual(e.DirectoryPath, e.Drive + e.Directory);
			Assert.AreEqual(e.Directory, String.Join("", new List<string>(e.DirectoryParts).ToArray()));
			Assert.AreEqual(e.File, e.FileName + e.FileExtension);
		}

		[TestMethod]
		public void test_non_existent_directory_from_path_with_1_param_Stat()
		{
			string tempRoot = Path.GetTempPath();
			string createdDir = null;

			try
			{
				for(int i = 1; ; ++i)
				{
					createdDir = Path.Combine(tempRoot, new string('a', i));

					if(!File.Exists(createdDir))
					{
						Directory.CreateDirectory(createdDir);

						break;
					}
				}

				string nonExistingDir = Path.Combine(createdDir, @"abc\");

				IEntry  e   =   Api.Stat(nonExistingDir);

				Assert.IsNull(e);
			}
			catch(PathTooLongException x)
			{
				Assert.Inconclusive("could not create temporary directory: {0}", x.Message);
			}
			finally
			{
				Directory.Delete(createdDir);
			}
		}

		[TestMethod]
		public void test_non_existent_directory_from_path_with_trailing_slash()
		{
			string tempRoot = Path.GetTempPath();
			string createdDir = null;

			try
			{
				for(int i = 1; ; ++i)
				{
					createdDir = Path.Combine(tempRoot, new string('a', i));

					if(!File.Exists(createdDir))
					{
						Directory.CreateDirectory(createdDir);

						break;
					}
				}

				string nonExistingDir = Path.Combine(createdDir, @"abc\");

				IEntry  e   =   Api.Stat(nonExistingDir, SearchOptions.StatInfoForNonexistentPath);
				IEntry2 e2  =   e as IEntry2;

				Assert.IsNotNull(e);
				Assert.IsNotNull(e2);

				Assert.IsTrue(e.IsDirectory);
				Assert.IsFalse(e2.IsFile);
				Assert.IsFalse(e2.Existed);
			}
			catch(PathTooLongException x)
			{
				Assert.Inconclusive("could not create temporary directory: {0}", x.Message);
			}
			finally
			{
				Directory.Delete(createdDir);
			}
		}

		[TestMethod]
		public void test_non_existent_directory_from_path_without_trailing_slash_with_None_flag()
		{
			string tempRoot = Path.GetTempPath();
			string createdDir = null;

			try
			{
				for(int i = 1; ; ++i)
				{
					createdDir = Path.Combine(tempRoot, new string('a', i));

					if(!File.Exists(createdDir))
					{
						Directory.CreateDirectory(createdDir);

						break;
					}
				}

				string nonExistingDir = Path.Combine(createdDir, @"abc\");

				IEntry  e   =   Api.Stat(nonExistingDir, SearchOptions.None);

				Assert.IsNull(e);
			}
			catch(PathTooLongException x)
			{
				Assert.Inconclusive("could not create temporary directory: {0}", x.Message);
			}
			finally
			{
				Directory.Delete(createdDir);
			}
		}

		[TestMethod]
		public void test_non_existent_directory_from_path_without_trailing_slash_with_Directories_flag()
		{
			string tempRoot = Path.GetTempPath();
			string createdDir = null;

			try
			{
				for(int i = 1; ; ++i)
				{
					createdDir = Path.Combine(tempRoot, new string('a', i));

					if(!File.Exists(createdDir))
					{
						Directory.CreateDirectory(createdDir);

						break;
					}
				}

				string nonExistingDir = Path.Combine(createdDir, @"abc");

				IEntry  e   =   Api.Stat(nonExistingDir, SearchOptions.StatInfoForNonexistentPath | SearchOptions.Directories);
				IEntry2 e2  =   e as IEntry2;

				Assert.IsNotNull(e);
				Assert.IsNotNull(e2);

				Assert.IsTrue(e.IsDirectory);
				Assert.IsFalse(e2.IsFile);
				Assert.IsFalse(e2.Existed);
			}
			catch(PathTooLongException x)
			{
				Assert.Inconclusive("could not create temporary directory: {0}", x.Message);
			}
			finally
			{
				Directory.Delete(createdDir);
			}
		}

		[TestMethod]
		public void test_non_existent_directory_from_path_without_trailing_slash_with_Files_flag()
		{
			string tempRoot = Path.GetTempPath();
			string createdDir = null;

			try
			{
				for(int i = 1; ; ++i)
				{
					createdDir = Path.Combine(tempRoot, new string('a', i));

					if(!File.Exists(createdDir))
					{
						Directory.CreateDirectory(createdDir);

						break;
					}
				}

				string nonExistingDir = Path.Combine(createdDir, @"abc");

				IEntry  e   =   Api.Stat(nonExistingDir, SearchOptions.StatInfoForNonexistentPath | SearchOptions.Files);
				IEntry2 e2  =   e as IEntry2;

				Assert.IsNotNull(e);
				Assert.IsNotNull(e2);

				Assert.IsFalse(e.IsDirectory);
				Assert.IsTrue(e2.IsFile);
				Assert.IsFalse(e2.Existed);
			}
			catch(PathTooLongException x)
			{
				Assert.Inconclusive("could not create temporary directory: {0}", x.Message);
			}
			finally
			{
				Directory.Delete(createdDir);
			}
		}

		[TestMethod]
		public void test_non_existent_directory_from_path_without_trailing_slash_with_Directories_and_Files_flags()
		{
			string tempRoot = Path.GetTempPath();
			string createdDir = null;

			try
			{
				for(int i = 1; ; ++i)
				{
					createdDir = Path.Combine(tempRoot, new string('a', i));

					if(!File.Exists(createdDir))
					{
						Directory.CreateDirectory(createdDir);

						break;
					}
				}

				string nonExistingDir = Path.Combine(createdDir, @"abc\");

				IEntry  e   =   Api.Stat(nonExistingDir, SearchOptions.Directories | SearchOptions.Files);

				Assert.IsNull(e);
			}
			catch(PathTooLongException x)
			{
				Assert.Inconclusive("could not create temporary directory: {0}", x.Message);
			}
			finally
			{
				Directory.Delete(createdDir);
			}
		}
	}
}
