﻿
// Updated: 10th June 2015

#if PSEUDO_UNIX
#else // PSEUDO_UNIX
#endif // PSEUDO_UNIX

namespace Test.Unit.recls.Core
{
	using Recls;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using System;
	using System.Collections.Generic;
	using System.IO;

	[TestClass]
	public class FileSearcher_Stat_tester
	{
		private string cwd	=	null;
		private string root =	null;
		//private string dir1	=	null;

		[TestInitialize]
		public void setup()
		{
			cwd = Environment.CurrentDirectory;
			root = Path.GetPathRoot(cwd);

#if PSEUDO_UNIX
			cwd = PathUtil.CanonicalizePath(cwd);
			root = PathUtil.CanonicalizePath(root);
#endif // PSEUDO_UNIX
		}

		[TestMethod]
		public void test_Root()
		{
			IEntry	e = FileSearcher.Stat("/");

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
			Assert.IsTrue(e.IsDirectory);
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
				if(null != knownSharePath)
				{
					Assert.Inconclusive("share path test inconclusive because environment variable RECLS_NET_TESTING_KNOWN_SHARE_PATH not defined");
				}
			}
			else
			{
				try
				{
					string	sharePath	=	knownSharePath;
					IEntry	e			=	FileSearcher.Stat(sharePath);

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
						Assert.IsTrue(e.IsDirectory);
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
				catch(System.UnauthorizedAccessException x)
				{
					Assert.Inconclusive("test involving sharing of '{0}' inconclusive: {1}", knownSharePath, x);
				}
			}
		}

		[TestMethod]
		public void test_Cwd()
		{
			IEntry	e = FileSearcher.Stat(cwd);

			Assert.AreEqual(FileAttributes.Directory, (FileAttributes.Directory & e.Attributes));
			//e.CreationTime);
			//Assert.AreEqual(cwd.Substring(e.Drive.Length), e.Directory);
			//Assert.AreEqual(1, e.DirectoryParts.Length);
			//Assert.AreEqual(cwd, e.DirectoryPath);
			//Assert.AreEqual(cwd, e.Drive);
			//Assert.AreEqual("", e.File);
			//Assert.AreEqual("", e.FileExtension);
			//Assert.AreEqual("", e.FileName);
			Assert.IsTrue(e.IsDirectory);
			//e.IsReadOnly);
			//e.IsUnc);
			//e.LastAccessTime);
			//e.LastStatusChangeTime);
			//e.ModificationTime);
			//Assert.AreEqual(root, e.Path);
			//Assert.AreEqual(root, e.SearchDirectory);
			//Assert.AreEqual("", e.SearchRelativePath);
			Assert.AreEqual(0, e.Size);
			Assert.AreEqual(null, e.UncDrive);

			Assert.AreEqual(e.Path, e.DirectoryPath + e.File);
			Assert.AreEqual(e.DirectoryPath, e.Drive + e.Directory);
			Assert.AreEqual(e.Directory, String.Join("", new List<string>(e.DirectoryParts).ToArray()));
			Assert.AreEqual(e.File, e.FileName + e.FileExtension);
		}
	}
}
