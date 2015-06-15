
// Updated: 10th June 2015

namespace Test.Unit.recls.Core
{
	using Recls;

#if NUNIT
    using global::NUnit.Framework;

    using TestClass = global::NUnit.Framework.TestFixtureAttribute;
    using TestMethod = global::NUnit.Framework.TestAttribute;
    using ExpectedException = global::NUnit.Framework.ExpectedExceptionAttribute;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

	using System;

	[TestClass]
	public class PathUtil_tester
	{
		[TestMethod]
		public void test_PathUtil_type_exists()
		{
			Assert.IsNotNull(typeof(PathUtil));
		}

		[TestMethod]
		public void test_DeriveRelativePath()
		{
			Assert.AreEqual(@"C:\abc", PathUtil.DeriveRelativePath(null, @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtil.DeriveRelativePath("", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtil.DeriveRelativePath(@"H:\", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtil.DeriveRelativePath(@"H:\abc", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtil.DeriveRelativePath(@"\\server\share", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtil.DeriveRelativePath(@"\\server\share\", @"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtil.DeriveRelativePath(@"\\server\share\abc", @"C:\abc"));

			Assert.AreEqual(@".", PathUtil.DeriveRelativePath(@"C:\abc", @"C:\abc"));

			Assert.AreEqual(@"def", PathUtil.DeriveRelativePath(@"C:\abc", @"C:\abc\def"));

#if PSEUDO_UNIX
		Assert.AreEqual(@"../ghi", PathUtil.DeriveRelativePath(@"C:/abc/def", @"C:/abc/ghi"));
		Assert.AreEqual(@"../../ghi/mno", PathUtil.DeriveRelativePath(@"C:/abc/def/jkl", @"C:/abc/ghi/mno"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"..\ghi", PathUtil.DeriveRelativePath(@"C:\abc\def", @"C:\abc\ghi"));
			Assert.AreEqual(@"..\..\ghi\mno", PathUtil.DeriveRelativePath(@"C:\abc\def\jkl", @"C:\abc\ghi\mno"));
#endif // PSEUDO_UNIX

			Assert.AreEqual(@".", PathUtil.DeriveRelativePath(@"C:\abc", @"C:\abc"));
			Assert.AreEqual(@"..", PathUtil.DeriveRelativePath(@"C:\abc\\def", @"C:\abc"));
			//Assert.AreEqual(@"def", PathUtil.DeriveRelativePath(@"C:\abc", @"C:\abc\\def"));
		}

		[TestMethod]
		public void test_CanonicalizePath()
		{
			Assert.AreEqual(null, PathUtil.CanonicalizePath(null));
			Assert.AreEqual(@"", PathUtil.CanonicalizePath(@""));
			Assert.AreEqual(@".", PathUtil.CanonicalizePath(@"."));
			Assert.AreEqual(@"..", PathUtil.CanonicalizePath(@".."));
			Assert.AreEqual(@"abc", PathUtil.CanonicalizePath(@"abc"));
#if PSEUDO_UNIX
		Assert.AreEqual(@"C:/", PathUtil.CanonicalizePath(@"C:/"));
		Assert.AreEqual(@"C:/abc", PathUtil.CanonicalizePath(@"C:/abc"));
		Assert.AreEqual(@"C:/abc", PathUtil.CanonicalizePath(@"C:/abc/"));
		Assert.AreEqual(@"C:/abc/def", PathUtil.CanonicalizePath(@"C:/abc/def"));
		Assert.AreEqual(@"C:/abc/def", PathUtil.CanonicalizePath(@"C:/abc/def/"));
		Assert.AreEqual(@"abc/def", PathUtil.CanonicalizePath(@"abc/def/"));
		Assert.AreEqual(@"/", PathUtil.CanonicalizePath(@"/"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"C:\", PathUtil.CanonicalizePath(@"C:\"));
			Assert.AreEqual(@"C:\abc", PathUtil.CanonicalizePath(@"C:\abc"));
			Assert.AreEqual(@"C:\abc", PathUtil.CanonicalizePath(@"C:\abc\"));
			Assert.AreEqual(@"C:\abc\def", PathUtil.CanonicalizePath(@"C:\abc\def"));
			Assert.AreEqual(@"C:\abc\def", PathUtil.CanonicalizePath(@"C:\abc\def\"));
			Assert.AreEqual(@"abc\def", PathUtil.CanonicalizePath(@"abc\def\"));
			Assert.AreEqual(@"\", PathUtil.CanonicalizePath(@"\"));
#endif // PSEUDO_UNIX

			Assert.AreEqual(@"", PathUtil.CanonicalizePath(@""));
			Assert.AreEqual(@".", PathUtil.CanonicalizePath(@"."));
			Assert.AreEqual(@"..", PathUtil.CanonicalizePath(@".."));
			Assert.AreEqual(@"abc", PathUtil.CanonicalizePath(@"abc"));
#if PSEUDO_UNIX
		Assert.AreEqual(@"C:/", PathUtil.CanonicalizePath(@"C:/"));
		Assert.AreEqual(@"C:/abc", PathUtil.CanonicalizePath(@"C:/abc"));
		Assert.AreEqual(@"C:/abc", PathUtil.CanonicalizePath(@"C:/abc/"));
		Assert.AreEqual(@"C:/abc/def", PathUtil.CanonicalizePath(@"C:/abc/def"));
		Assert.AreEqual(@"C:/abc/def", PathUtil.CanonicalizePath(@"C:/abc/def/"));
		Assert.AreEqual(@"abc/def", PathUtil.CanonicalizePath(@"abc/def/"));
		Assert.AreEqual(@"/", PathUtil.CanonicalizePath(@"/"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"C:\", PathUtil.CanonicalizePath(@"C:/"));
			Assert.AreEqual(@"C:\abc", PathUtil.CanonicalizePath(@"C:/abc"));
			Assert.AreEqual(@"C:\abc", PathUtil.CanonicalizePath(@"C:/abc/"));
			Assert.AreEqual(@"C:\abc\def", PathUtil.CanonicalizePath(@"C:/abc/def"));
			Assert.AreEqual(@"C:\abc\def", PathUtil.CanonicalizePath(@"C:/abc/def/"));
			Assert.AreEqual(@"abc\def", PathUtil.CanonicalizePath(@"abc/def/"));
			Assert.AreEqual(@"\", PathUtil.CanonicalizePath(@"/"));
#endif // PSEUDO_UNIX


			Assert.AreEqual(@"abc", PathUtil.CanonicalizePath(@"abc\def\.."));
			Assert.AreEqual(@"abc", PathUtil.CanonicalizePath(@"abc\."));
#if PSEUDO_UNIX
		Assert.AreEqual(@"abc/def", PathUtil.CanonicalizePath(@"abc/def/."));
		Assert.AreEqual(@"abc/def", PathUtil.CanonicalizePath(@"abc/./././././def/./././././ghi/././././.."));
		Assert.AreEqual(@"abc/def", PathUtil.CanonicalizePath(@"./abc/def"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"abc\def", PathUtil.CanonicalizePath(@"abc\def\."));
			Assert.AreEqual(@"abc\def", PathUtil.CanonicalizePath(@"abc\.\.\.\.\.\def\.\.\.\.\.\ghi\.\.\.\.\.."));
			Assert.AreEqual(@"abc\def", PathUtil.CanonicalizePath(@".\abc\def"));
#endif // PSEUDO_UNIX
		}

		[TestMethod]
		public void test_GetDirectoryPath()
		{
#if PSEUDO_UNIX
		Assert.AreEqual(@"C:/abc/", PathUtil.GetDirectoryPath(@"C:/abc/def"));
		Assert.AreEqual(@"C:/abc/def/", PathUtil.GetDirectoryPath(@"C:/abc/def/"));
		Assert.AreEqual(@"C:/", PathUtil.GetDirectoryPath(@"C:/abc"));
		Assert.AreEqual(@"C:/", PathUtil.GetDirectoryPath(@"C:/"));

		Assert.AreEqual(@"\\server\share/abc/", PathUtil.GetDirectoryPath(@"\\server\share/abc/def"));
		Assert.AreEqual(@"\\server\share/abc/def/", PathUtil.GetDirectoryPath(@"\\server\share/abc/def/"));
		Assert.AreEqual(@"\\server\share/", PathUtil.GetDirectoryPath(@"\\server\share/abc"));
		Assert.AreEqual(@"\\server\share/", PathUtil.GetDirectoryPath(@"\\server\share/"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"C:\abc\", PathUtil.GetDirectoryPath(@"C:\abc\def"));
			Assert.AreEqual(@"C:\abc\def\", PathUtil.GetDirectoryPath(@"C:\abc\def\"));
			Assert.AreEqual(@"C:\", PathUtil.GetDirectoryPath(@"C:\abc"));
			Assert.AreEqual(@"C:\", PathUtil.GetDirectoryPath(@"C:\"));

			Assert.AreEqual(@"\\server\share\abc\", PathUtil.GetDirectoryPath(@"\\server\share\abc\def"));
			Assert.AreEqual(@"\\server\share\abc\def\", PathUtil.GetDirectoryPath(@"\\server\share\abc\def\"));
			Assert.AreEqual(@"\\server\share\", PathUtil.GetDirectoryPath(@"\\server\share\abc"));
			Assert.AreEqual(@"\\server\share\", PathUtil.GetDirectoryPath(@"\\server\share\"));
#endif // PSEUDO_UNIX
		}

		[TestMethod]
		[ExpectedException(typeof(Recls.Exceptions.IllformedUncPathException))]
		public void test_GetDirectoryPath_exception_1()
		{
			PathUtil.GetDirectoryPath(@"\\");
		}

		[TestMethod]
		[ExpectedException(typeof(Recls.Exceptions.IllformedUncPathException))]
		public void test_GetDirectoryPath_exception_2()
		{
			PathUtil.GetDirectoryPath(@"\\abc");
		}

		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public void test_GetDirectoryPath_exception_3()
		{
			PathUtil.GetDirectoryPath(@"");
		}

		[TestMethod]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void test_GetDirectoryPath_exception_4()
		{
			PathUtil.GetDirectoryPath(null);
		}
	}
}
