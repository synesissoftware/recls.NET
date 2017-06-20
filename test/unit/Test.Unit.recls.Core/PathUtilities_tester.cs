
// Updated: 20th June 2017

namespace Test.Unit.recls.Core
{
	using Recls;

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

	[TestClass]
	public class PathUtilities_tester
	{
		[TestMethod]
		public void test_PathUtilities_type_exists()
		{
			Assert.IsNotNull(typeof(PathUtilities));
		}

		[TestMethod]
		public void test_DeriveRelativePath()
		{
			Assert.AreEqual(@"C:\abc", PathUtilities.DeriveRelativePath(null, @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtilities.DeriveRelativePath("", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtilities.DeriveRelativePath(@"H:\", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtilities.DeriveRelativePath(@"H:\abc", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtilities.DeriveRelativePath(@"\\server\share", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtilities.DeriveRelativePath(@"\\server\share\", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtilities.DeriveRelativePath(@"\\server\share\abc", @"C:\abc"));

			Assert.AreEqual(@".", PathUtilities.DeriveRelativePath(@"C:\abc", @"C:\abc"));

			Assert.AreEqual(@"def", PathUtilities.DeriveRelativePath(@"C:\abc", @"C:\abc\def"));

#if PSEUDO_UNIX
		Assert.AreEqual(@"../ghi", PathUtilities.DeriveRelativePath(@"C:/abc/def", @"C:/abc/ghi"));
		Assert.AreEqual(@"../../ghi/mno", PathUtilities.DeriveRelativePath(@"C:/abc/def/jkl", @"C:/abc/ghi/mno"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"..\ghi", PathUtilities.DeriveRelativePath(@"C:\abc\def", @"C:\abc\ghi"));
			Assert.AreEqual(@"..\..\ghi\mno", PathUtilities.DeriveRelativePath(@"C:\abc\def\jkl", @"C:\abc\ghi\mno"));
#endif // PSEUDO_UNIX

			Assert.AreEqual(@".", PathUtilities.DeriveRelativePath(@"C:\abc", @"C:\abc"));
			Assert.AreEqual(@"..", PathUtilities.DeriveRelativePath(@"C:\abc\\def", @"C:\abc"));
			//Assert.AreEqual(@"def", PathUtilities.DeriveRelativePath(@"C:\abc", @"C:\abc\\def"));
		}

		[TestMethod]
		public void test_CanonicalizePath()
		{
			Assert.AreEqual(null, PathUtilities.CanonicalizePath(null));
			Assert.AreEqual(@"", PathUtilities.CanonicalizePath(@""));
			Assert.AreEqual(@".", PathUtilities.CanonicalizePath(@"."));
			Assert.AreEqual(@"..", PathUtilities.CanonicalizePath(@".."));
			Assert.AreEqual(@"abc", PathUtilities.CanonicalizePath(@"abc"));
#if PSEUDO_UNIX
		Assert.AreEqual(@"C:/", PathUtilities.CanonicalizePath(@"C:/"));
		Assert.AreEqual(@"C:/abc", PathUtilities.CanonicalizePath(@"C:/abc"));
		Assert.AreEqual(@"C:/abc", PathUtilities.CanonicalizePath(@"C:/abc/"));
		Assert.AreEqual(@"C:/abc/def", PathUtilities.CanonicalizePath(@"C:/abc/def"));
		Assert.AreEqual(@"C:/abc/def", PathUtilities.CanonicalizePath(@"C:/abc/def/"));
		Assert.AreEqual(@"abc/def", PathUtilities.CanonicalizePath(@"abc/def/"));
		Assert.AreEqual(@"/", PathUtilities.CanonicalizePath(@"/"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"C:\", PathUtilities.CanonicalizePath(@"C:\"));
			Assert.AreEqual(@"C:\abc", PathUtilities.CanonicalizePath(@"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtilities.CanonicalizePath(@"C:\abc\"));
			Assert.AreEqual(@"C:\abc\def", PathUtilities.CanonicalizePath(@"C:\abc\def"));
			Assert.AreEqual(@"C:\abc\def", PathUtilities.CanonicalizePath(@"C:\abc\def\"));
			Assert.AreEqual(@"abc\def", PathUtilities.CanonicalizePath(@"abc\def\"));
			Assert.AreEqual(@"\", PathUtilities.CanonicalizePath(@"\"));
#endif // PSEUDO_UNIX

			Assert.AreEqual(@"", PathUtilities.CanonicalizePath(@""));
			Assert.AreEqual(@".", PathUtilities.CanonicalizePath(@"."));
			Assert.AreEqual(@"..", PathUtilities.CanonicalizePath(@".."));
			Assert.AreEqual(@"abc", PathUtilities.CanonicalizePath(@"abc"));
#if PSEUDO_UNIX
		Assert.AreEqual(@"C:/", PathUtilities.CanonicalizePath(@"C:/"));
		Assert.AreEqual(@"C:/abc", PathUtilities.CanonicalizePath(@"C:/abc"));
		Assert.AreEqual(@"C:/abc", PathUtilities.CanonicalizePath(@"C:/abc/"));
		Assert.AreEqual(@"C:/abc/def", PathUtilities.CanonicalizePath(@"C:/abc/def"));
		Assert.AreEqual(@"C:/abc/def", PathUtilities.CanonicalizePath(@"C:/abc/def/"));
		Assert.AreEqual(@"abc/def", PathUtilities.CanonicalizePath(@"abc/def/"));
		Assert.AreEqual(@"/", PathUtilities.CanonicalizePath(@"/"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"C:\", PathUtilities.CanonicalizePath(@"C:/"));
			Assert.AreEqual(@"C:\abc", PathUtilities.CanonicalizePath(@"C:/abc"));
			Assert.AreEqual(@"C:\abc", PathUtilities.CanonicalizePath(@"C:/abc/"));
			Assert.AreEqual(@"C:\abc\def", PathUtilities.CanonicalizePath(@"C:/abc/def"));
			Assert.AreEqual(@"C:\abc\def", PathUtilities.CanonicalizePath(@"C:/abc/def/"));
			Assert.AreEqual(@"abc\def", PathUtilities.CanonicalizePath(@"abc/def/"));
			Assert.AreEqual(@"\", PathUtilities.CanonicalizePath(@"/"));
#endif // PSEUDO_UNIX


			Assert.AreEqual(@"abc", PathUtilities.CanonicalizePath(@"abc\def\.."));
			Assert.AreEqual(@"abc", PathUtilities.CanonicalizePath(@"abc\."));
#if PSEUDO_UNIX
		Assert.AreEqual(@"abc/def", PathUtilities.CanonicalizePath(@"abc/def/."));
		Assert.AreEqual(@"abc/def", PathUtilities.CanonicalizePath(@"abc/./././././def/./././././ghi/././././.."));
		Assert.AreEqual(@"abc/def", PathUtilities.CanonicalizePath(@"./abc/def"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"abc\def", PathUtilities.CanonicalizePath(@"abc\def\."));
			Assert.AreEqual(@"abc\def", PathUtilities.CanonicalizePath(@"abc\.\.\.\.\.\def\.\.\.\.\.\ghi\.\.\.\.\.."));
			Assert.AreEqual(@"abc\def", PathUtilities.CanonicalizePath(@".\abc\def"));
#endif // PSEUDO_UNIX
		}

		[TestMethod]
		public void test_GetDirectoryPath()
		{
#if PSEUDO_UNIX
		Assert.AreEqual(@"C:/abc/", PathUtilities.GetDirectoryPath(@"C:/abc/def"));
		Assert.AreEqual(@"C:/abc/def/", PathUtilities.GetDirectoryPath(@"C:/abc/def/"));
		Assert.AreEqual(@"C:/", PathUtilities.GetDirectoryPath(@"C:/abc"));
		Assert.AreEqual(@"C:/", PathUtilities.GetDirectoryPath(@"C:/"));

		Assert.AreEqual(@"\\server\share/abc/", PathUtilities.GetDirectoryPath(@"\\server\share/abc/def"));
		Assert.AreEqual(@"\\server\share/abc/def/", PathUtilities.GetDirectoryPath(@"\\server\share/abc/def/"));
		Assert.AreEqual(@"\\server\share/", PathUtilities.GetDirectoryPath(@"\\server\share/abc"));
		Assert.AreEqual(@"\\server\share/", PathUtilities.GetDirectoryPath(@"\\server\share/"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"C:\abc\", PathUtilities.GetDirectoryPath(@"C:\abc\def"));
			Assert.AreEqual(@"C:\abc\def\", PathUtilities.GetDirectoryPath(@"C:\abc\def\"));
			Assert.AreEqual(@"C:\", PathUtilities.GetDirectoryPath(@"C:\abc"));
			Assert.AreEqual(@"C:\", PathUtilities.GetDirectoryPath(@"C:\"));

			Assert.AreEqual(@"\\server\share\abc\", PathUtilities.GetDirectoryPath(@"\\server\share\abc\def"));
			Assert.AreEqual(@"\\server\share\abc\def\", PathUtilities.GetDirectoryPath(@"\\server\share\abc\def\"));
			Assert.AreEqual(@"\\server\share\", PathUtilities.GetDirectoryPath(@"\\server\share\abc"));
			Assert.AreEqual(@"\\server\share\", PathUtilities.GetDirectoryPath(@"\\server\share\"));
#endif // PSEUDO_UNIX
		}

		[TestMethod]
		[ExpectedException(typeof(Recls.Exceptions.IllformedUncPathException))]
		public void test_GetDirectoryPath_exception_1()
		{
			PathUtilities.GetDirectoryPath(@"\\");
		}

		[TestMethod]
		[ExpectedException(typeof(Recls.Exceptions.IllformedUncPathException))]
		public void test_GetDirectoryPath_exception_2()
		{
			PathUtilities.GetDirectoryPath(@"\\abc");
		}

		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public void test_GetDirectoryPath_exception_3()
		{
			PathUtilities.GetDirectoryPath(@"");
		}

		[TestMethod]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void test_GetDirectoryPath_exception_4()
		{
			PathUtilities.GetDirectoryPath(null);
		}
	}
}
