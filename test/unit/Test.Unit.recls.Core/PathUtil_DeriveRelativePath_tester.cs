
namespace Test.Unit.recls.Core
{
    using Recls;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;

    [TestClass]
    public class PathUtil_DeriveRelativePath_tester
    {
        #region null Tests
        [TestMethod]
        public void Test_null_1()
        {
            Assert.AreEqual(null, PathUtil.DeriveRelativePath(null, null));
        }
        [TestMethod]
        public void Test_null_2()
        {
            Assert.AreEqual(null, PathUtil.DeriveRelativePath(@"", null));
        }
        #endregion

        #region empty Tests
        [TestMethod]
        public void Test_empty_1()
        {
            Assert.AreEqual(@"", PathUtil.DeriveRelativePath(null, @""));
        }
        [TestMethod]
        public void Test_empty_2()
        {
            Assert.AreEqual(@"", PathUtil.DeriveRelativePath(@"", @""));
        }
        #endregion

        #region relative Tests
        [TestMethod]
        public void Test_relative_1()
        {
            Assert.AreEqual(@"def", PathUtil.DeriveRelativePath(@"abc", @"abc\def"));
        }
        [TestMethod]
        public void Test_relative_2()
        {
            Assert.AreEqual(@"def", PathUtil.DeriveRelativePath(@"abc", @"abc\def\"));
        }
        [TestMethod]
        public void Test_relative_3()
        {
            Assert.AreEqual(@"..", PathUtil.DeriveRelativePath(@"abc\def", @"abc"));
        }
        #endregion

        #region absolute (same drive) Tests
        [TestMethod]
        public void Test_absolute_samedrive_1()
        {
#if PSEUDO_UNIX
		Assert.AreEqual(@"def", PathUtil.DeriveRelativePath(@"C:/abc", @"C:/abc/def"));
#else // PSEUDO_UNIX
            Assert.AreEqual(@"def", PathUtil.DeriveRelativePath(@"C:\abc", @"C:\abc\def"));
#endif // PSEUDO_UNIX
        }
        [TestMethod]
        public void Test_absolute_samedrive_2()
        {
            Assert.AreEqual(@"..", PathUtil.DeriveRelativePath(@"C:\abc\def", @"C:\abc"));
        }
        [TestMethod]
        public void Test_absolute_samedrive_3()
        {
            Assert.AreEqual(@"..", PathUtil.DeriveRelativePath(@"C:\abc\def\", @"C:\abc"));
        }
        [TestMethod]
        public void Test_absolute_samedrive_4()
        {
            Assert.AreEqual(@"..", PathUtil.DeriveRelativePath(@"C:\abc\def", @"C:\abc\"));
        }
        [TestMethod]
        public void Test_absolute_samedrive_5()
        {
            Assert.AreEqual(@"..", PathUtil.DeriveRelativePath(@"C:\abc\def\", @"C:\abc\"));
        }
        [TestMethod]
        public void Test_absolute_samedrive_6()
        {
            Assert.AreEqual(@"abc", PathUtil.DeriveRelativePath(@"C:\", @"C:\abc\"));
        }
        #endregion

        #region absolute (different drive) Tests
        [TestMethod]
        public void Test_absolute_differentdrive_1()
        {
            Assert.AreEqual(@"D:\abc\def", PathUtil.DeriveRelativePath(@"C:\abc", @"D:\abc\def"));
        }
        [TestMethod]
        public void Test_absolute_differentdrive_2()
        {
            Assert.AreEqual(@"D:\abc", PathUtil.DeriveRelativePath(@"C:\abc\def", @"D:\abc"));
        }
        [TestMethod]
        public void Test_absolute_differentdrive_3()
        {
            Assert.AreEqual(@"D:\abc", PathUtil.DeriveRelativePath(@"C:\abc\def\", @"D:\abc"));
        }
        [TestMethod]
        public void Test_absolute_differentdrive_4()
        {
            Assert.AreEqual(@"D:\abc\", PathUtil.DeriveRelativePath(@"C:\abc\def", @"D:\abc\"));
        }
        [TestMethod]
        public void Test_absolute_differentdrive_5()
        {
            Assert.AreEqual(@"D:\abc\", PathUtil.DeriveRelativePath(@"C:\abc\def\", @"D:\abc\"));
        }
        #endregion

    }
}
