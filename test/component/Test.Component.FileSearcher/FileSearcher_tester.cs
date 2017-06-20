
// Created: 20th August 2009
// Updated: 20th June 2017

namespace Test.Component.FileSearcher
{
    using global::Recls;

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

    [TestClass]
    public class FileSearcher_tester
    {
        string						m_searchRoot;
        Dictionary<string, IEntry>	m_files				=	new Dictionary<string, IEntry>();
        Dictionary<string, IEntry>	m_directories		=	new Dictionary<string, IEntry>();
        Dictionary<string, IEntry>	m_markedDirectories	=	new Dictionary<string, IEntry>();

        [TestInitialize]
        public void Setup()
        {
            m_searchRoot = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            foreach(IEntry entry in FileSearcher.Search(m_searchRoot, null, SearchOptions.IgnoreInaccessibleNodes | SearchOptions.Files))
            {
                m_files.Add(entry.Path, entry);
            }

            foreach(IEntry entry in FileSearcher.Search(m_searchRoot, null, SearchOptions.IgnoreInaccessibleNodes | SearchOptions.Directories))
            {
                m_directories.Add(entry.Path, entry);
            }

            foreach(IEntry entry in FileSearcher.Search(m_searchRoot, null, SearchOptions.IgnoreInaccessibleNodes | SearchOptions.Directories | SearchOptions.MarkDirectories))
            {
                m_markedDirectories.Add(entry.Path, entry);
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            m_files.Clear();
            m_directories.Clear();
            m_markedDirectories.Clear();
        }

        [TestMethod]
        public void Test_Stat_1()
        {
            IEntry entry = FileSearcher.Stat(m_searchRoot);

            Assert.AreEqual(m_searchRoot, entry.Path);
            Assert.AreEqual(entry.Path, entry.Drive + entry.Directory + entry.File);
            Assert.AreEqual(entry.Path, entry.DirectoryPath + entry.File);
        }

        [TestMethod]
        public void Test_SearchRelativePath_1()
        {
            foreach(IEntry entry in m_files.Values)
            {
                Assert.AreEqual(entry.Path, System.IO.Path.Combine(m_searchRoot, entry.SearchRelativePath));

                Assert.AreEqual(entry.Path, entry.DirectoryPath + entry.File);
                Assert.AreEqual(entry.DirectoryPath, entry.Drive + entry.Directory);
                Assert.AreEqual(entry.File, entry.FileName + entry.FileExtension);

                Assert.AreEqual(entry.IsDirectory, 0 != (entry.Attributes & System.IO.FileAttributes.Directory));
            }
        }

        [TestMethod]
        public void Test_MarkedDirectories_1()
        {
            foreach(KeyValuePair<string, IEntry> pair in m_directories)
            {
                IEntry entry = pair.Value;
                IEntry markedEntry = m_markedDirectories[entry.Path + "\\"];

                Assert.AreEqual(entry.SearchRelativePath + "\\", markedEntry.SearchRelativePath);
                Assert.AreEqual(entry.Drive, markedEntry.Drive);
                Assert.AreEqual(entry.DirectoryPath, markedEntry.DirectoryPath);
                Assert.AreEqual(entry.Directory, markedEntry.Directory);
                Assert.AreEqual(entry.SearchDirectory, markedEntry.SearchDirectory);
                Assert.AreEqual(entry.UncDrive, markedEntry.UncDrive);
                Assert.AreEqual(entry.File + "\\", markedEntry.File);
                Assert.AreEqual(entry.FileName, markedEntry.FileName);
                Assert.AreEqual(entry.FileExtension, markedEntry.FileExtension);
                Assert.AreEqual(entry.CreationTime, markedEntry.CreationTime);
                Assert.AreEqual(entry.Size, markedEntry.Size);
                Assert.AreEqual(entry.Attributes, markedEntry.Attributes);
                Assert.AreEqual(entry.IsReadOnly, markedEntry.IsReadOnly);
                Assert.AreEqual(entry.IsDirectory, markedEntry.IsDirectory);
                Assert.AreEqual(entry.IsUnc, markedEntry.IsUnc);
                Assert.AreEqual(entry.DirectoryParts.Count, markedEntry.DirectoryParts.Count);
                for(int i = 0; i != entry.DirectoryParts.Count; ++i)
                {
                    Assert.AreEqual(entry.DirectoryParts[i], markedEntry.DirectoryParts[i]);
                }
                Assert.AreEqual(0, markedEntry.Size);
                Assert.IsTrue(markedEntry.IsDirectory);

                Assert.AreEqual(entry.IsDirectory, 0 != (entry.Attributes & System.IO.FileAttributes.Directory));

                //Assert.AreEqual(entry.Path, System.IO.Path.Combine(m_searchRoot, entry.SearchRelativePath));
            }
        }
    }
}
