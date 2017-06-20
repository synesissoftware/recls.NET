
// Updated: 20th June 2017

#if PSEUDO_UNIX
#else // PSEUDO_UNIX
#endif // PSEUDO_UNIX

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
	public class PathUtilities_DeriveRelativePath_tester
	{
		#region null Tests
		[TestMethod]
		public void Test_null_1()
		{
			Assert.AreEqual(null, PathUtilities.DeriveRelativePath(null, null));
		}
		[TestMethod]
		public void Test_null_2()
		{
			Assert.AreEqual(null, PathUtilities.DeriveRelativePath(@"", null));
		}
		#endregion

		#region empty Tests
		[TestMethod]
		public void Test_empty_1()
		{
			Assert.AreEqual(@"", PathUtilities.DeriveRelativePath(null, @""));
		}
		[TestMethod]
		public void Test_empty_2()
		{
			Assert.AreEqual(@"", PathUtilities.DeriveRelativePath(@"", @""));
		}
		#endregion

		#region relative Tests
		[TestMethod]
		public void Test_relative_1()
		{
			Assert.AreEqual(@"def", PathUtilities.DeriveRelativePath(@"abc", @"abc\def"));
		}
		[TestMethod]
		public void Test_relative_2()
		{
			Assert.AreEqual(@"def", PathUtilities.DeriveRelativePath(@"abc", @"abc\def\"));
		}
		[TestMethod]
		public void Test_relative_3()
		{
			Assert.AreEqual(@"..", PathUtilities.DeriveRelativePath(@"abc\def", @"abc"));
		}
		#endregion

		#region absolute (same drive) Tests
		[TestMethod]
		public void Test_absolute_samedrive_1()
		{
#if PSEUDO_UNIX
		Assert.AreEqual(@"def", PathUtilities.DeriveRelativePath(@"C:/abc", @"C:/abc/def"));
#else // PSEUDO_UNIX
			Assert.AreEqual(@"def", PathUtilities.DeriveRelativePath(@"C:\abc", @"C:\abc\def"));
#endif // PSEUDO_UNIX
		}
		[TestMethod]
		public void Test_absolute_samedrive_2()
		{
			Assert.AreEqual(@"..", PathUtilities.DeriveRelativePath(@"C:\abc\def", @"C:\abc"));
		}
		[TestMethod]
		public void Test_absolute_samedrive_3()
		{
			Assert.AreEqual(@"..", PathUtilities.DeriveRelativePath(@"C:\abc\def\", @"C:\abc"));
		}
		[TestMethod]
		public void Test_absolute_samedrive_4()
		{
			Assert.AreEqual(@"..", PathUtilities.DeriveRelativePath(@"C:\abc\def", @"C:\abc\"));
		}
		[TestMethod]
		public void Test_absolute_samedrive_5()
		{
			Assert.AreEqual(@"..", PathUtilities.DeriveRelativePath(@"C:\abc\def\", @"C:\abc\"));
		}
		[TestMethod]
		public void Test_absolute_samedrive_6()
		{
			Assert.AreEqual(@"abc", PathUtilities.DeriveRelativePath(@"C:\", @"C:\abc\"));
		}
		#endregion

		#region absolute (different drive) Tests
		[TestMethod]
		public void Test_absolute_differentdrive_1()
		{
			Assert.AreEqual(@"D:\abc\def", PathUtilities.DeriveRelativePath(@"C:\abc", @"D:\abc\def"));
		}
		[TestMethod]
		public void Test_absolute_differentdrive_2()
		{
			Assert.AreEqual(@"D:\abc", PathUtilities.DeriveRelativePath(@"C:\abc\def", @"D:\abc"));
		}
		[TestMethod]
		public void Test_absolute_differentdrive_3()
		{
			Assert.AreEqual(@"D:\abc", PathUtilities.DeriveRelativePath(@"C:\abc\def\", @"D:\abc"));
		}
		[TestMethod]
		public void Test_absolute_differentdrive_4()
		{
			Assert.AreEqual(@"D:\abc\", PathUtilities.DeriveRelativePath(@"C:\abc\def", @"D:\abc\"));
		}
		[TestMethod]
		public void Test_absolute_differentdrive_5()
		{
			Assert.AreEqual(@"D:\abc\", PathUtilities.DeriveRelativePath(@"C:\abc\def\", @"D:\abc\"));
		}
		#endregion

	}
}
