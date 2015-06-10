
namespace Recls
{
	using System.Runtime.CompilerServices;

	/// <summary>
	///  <para>
	///   The
	///   <a href="#{9F534D7B-49E3-4471-A12D-0B2735FBAB54}">Recls namespace</a>
	///   defines types for recursive search.
	///  </para>
	///  <para>
	///   <strong><u>History</u></strong><br/><br/>
	///   The <a ref="http://www.recls.org/">recls</a> -
	///   <em>rec</em>ursive <em>ls</em> - project began in 2003, as an
	///   open-source C/C++ library, forked from an internal project of
	///   <a href="http://www.synesis.com.au">Synesis Software Pty Ltd</a>.
	///   As time went on, the library evolved new features, and also
	///   added mappings for other languages, including C#, Ch, D, Java,
	///   Python and Ruby.
	///  </para>
	///  <para>
	///   Between 2003 and 2006 the library was used as the exemplar
	///   project for
	///   <a href="http://www.synesis.com.au/publications.html">Matthew Wilson</a>'s
	///   <strong>Positive Integration</strong> column, for
	///   <a href="http://www.ddj.com/">Dr. Dobb's Journal</a>
	///   (formerly in C/C++ User's Journal). The column instalments
	///   discussed many of the design and implementation techniques of the
	///   core library, and the mappings to other languages, through
	///   versions 1.0 to 1.6.
	///  </para>
	///  <para>
	///   As of November 2009, the latest publicly released version of the
	///   library is
	///   <a href="http://synesis.com.au/software/recls/downloads.html#recls_1_8_13">1.8.13</a>.
	///  </para>
	///  <para>
	///   The last version of "classic" recls will be 1.9, which is
	///   expected to be released in November or December 2009.
	///  </para>
	///  <para>
	///   The next phase for recls will be <strong>recls 100%</strong>.
	///  </para>
	///  <para>
	///   <strong><u>recls 100%</u></strong><br/><br/>
	///   The <strong>recls 100%</strong> stream aims to implement recls
	///   functionality for each language purely in that language.
	///  </para>
	///  <para>
	///   The first language to be covered in the recls 100% stream is
	///   C# (.NET). The resulting library, known as <i>recls 100% .NET</i>,
	///   is based around the static <see cref="Recls.FileSearcher"/> class,
	///   which defines methods, constants and delegates for file-system
	///   enumeration. File-system entries are represented by the
	///   <see cref="Recls.IEntry"/> interface, which defines a number
	///   of properties representing the entry's characteristics.
	///  </para>
	///  <para>
	///   The version numbers for the recls 100% stream will start from
	///   <strong>1.100</strong>.
	///  </para>
	///  <para>
	///   <strong><u>Distribution and Support</u></strong><br/><br/>
	///   <i>recls 100% .NET</i> is a
	///   <a href="http://en.wikipedia.org/wiki/Donationware">donationware</a>
	///   product. It is written to production-quality standards, yet is
	///   available for free, and may be used without restriction in
	///   commercial and/or non-commercial software systems. Users are asked
	///   to <a href="http://recls.org/donate.html">make a donation to the recls project</a>
	///   in order to ensure that the project is actively maintained and
	///   improved.
	///  </para>
	///  <para>
	///   <strong><u>Examples</u></strong><br/><br/>
	///   A few examples to give you a feel for how <i>recls 100% .NET</i>
	///   provides recursive file-system enumeration.
	///  </para>
	///  <para>
	///   <strong>Example 1</strong><br/><br/>
	///   List all font files in the windows directory or any of its
	///   subdirectories.
	///   <code>
	///    foreach(IEntry entry in FileSearcher.Search(@"C:\windows", "*.fon|*.ttf"))
	///    {
	///        Console.WriteLine(entry.Path);
	///    }
	///   </code>
	///  </para>
	///  <para>
	///   <strong>Example 2</strong><br/><br/>
	///   Search for all program and DLL files (including hidden) smaller
	///   than 10k in the current directory and all sub-directories up to
	///   3 deep, reporting on any entries that cannot be enumerated. This
	///   example uses an anonymous delegate for the error handling, and
	///   LINQ for filtering and selecting the entry members.
	///   <code>
	///    var files = FileSearcher.DepthFirst.Search( // search in depth-first manner
	///        null,                                   // search current directory
	///        "m*.exe|n*.dll",                        // all programs beginning with m; all DLLs beginning with n
	///        SearchOptions.IncludeHidden,            // include hidden files/directories
	///        3,                                      // descend at most 3 directories
	///        null,                                   // don't need progress callback
	///        delegate(string path, Exception x)
	///        {
	///            // report on any entries that could not be enumerated, but ...
	///            Console.Error.WriteLine("could not enumerate {0}: {1}", path, x.Message);
	///            // ... continue the enumeration
	///            return ExceptionHandlerResult.ConsumeExceptionAndContinue;
	///        }
	///    );
	///
	///    var results = from file in files
	///                  where file.Size &lt; 10240
	///                  select file.SearchRelativePath;
	///
	///    foreach(var path in results)
	///    {
	///        Console.WriteLine("entry: {0}", path);
	///    }
	///   </code>
	///  </para>
	///  <para>
	///   <strong>Example 3</strong><br/><br/>
	///   Display the names and sizes of all the immediate sub-directories
	///   of the current directory. The example uses the Recls extension
	///   method ForEach() in combination with a lambda expression.
	///   <code>
	///    FileSearcher.BreadthFirst.Search(
	///        null,                        // search current directory
	///        null,                        // all names
	///        SearchOptions.Directories | SearchOptions.IgnoreInaccessibleNodes, // only want dirs; don't worry about inaccessible entries
	///        0							// do not recurse
	///     )
	///         .ForEach((d) => Console.WriteLine("{0} : {1}", d.Path, FileSearcher.CalculateDirectorySize(d.Path, FileSearcher.UnrestrictedDepth)));
	///   </code>
	///   This can also be expressed in a more conventional syntax.
	///   <code>
	///   	foreach(IEntry entry in FileSearcher.BreadthFirst.Search(null, null, SearchOptions.Directories | SearchOptions.IgnoreInaccessibleNodes, 0))
	///   	{
	///   	    Console.WriteLine("{0} : {1}", entry.Path, FileSearcher.CalculateDirectorySize(entry, FileSearcher.UnrestrictedDepth));
	///   	}
	///   </code>
	///  </para>
	///  <para>
	///   <strong>Example 4</strong><br/><br/>
	///   Get an entry representing a given path.
	///   <code>
	///    IEntry entry = FileSearcher.Stat(@"H:\freelibs\recls\100\recls.net\recls.100.sln");
	///
	///    if(null == entry)
	///    {
	///        Console.Error.Write("file not found");
	///    }
	///    else
	///    {
	///        Console.WriteLine("{0,20}:\t{1}", "Path", entry.Path);
	///        Console.WriteLine("{0,20}:\t{1}", "SearchRelativePath", entry.SearchRelativePath);
	///        Console.WriteLine("{0,20}:\t{1}", "Drive", entry.Drive);
	///        Console.WriteLine("{0,20}:\t{1}", "DirectoryPath", entry.DirectoryPath);
	///        Console.WriteLine("{0,20}:\t{1}", "Directory", entry.Directory);
	///        Console.WriteLine("{0,20}:\t{1}", "SearchDirectory", entry.SearchDirectory);
	///        Console.WriteLine("{0,20}:\t{1}", "UncDrive", entry.UncDrive);
	///        Console.WriteLine("{0,20}:\t{1}", "File", entry.File);
	///        Console.WriteLine("{0,20}:\t{1}", "FileName", entry.FileName);
	///        Console.WriteLine("{0,20}:\t{1}", "FileExtension", entry.FileExtension);
	///        Console.WriteLine("{0,20}:\t{1}", "CreationTime", entry.CreationTime);
	///        Console.WriteLine("{0,20}:\t{1}", "ModificationTime", entry.ModificationTime);
	///        Console.WriteLine("{0,20}:\t{1}", "LastAccessTime", entry.LastAccessTime);
	///        Console.WriteLine("{0,20}:\t{1}", "LastStatusChangeTime", entry.LastStatusChangeTime);
	///        Console.WriteLine("{0,20}:\t{1}", "Size", entry.Size);
	///        Console.WriteLine("{0,20}:\t{1}", "Attributes", entry.Attributes);
	///        Console.WriteLine("{0,20}:\t{1}", "IsReadOnly", entry.IsReadOnly);
	///        Console.WriteLine("{0,20}:\t{1}", "IsDirectory", entry.IsDirectory);
	///        Console.WriteLine("{0,20}:\t{1}", "IsUnc", entry.IsUnc);
	///        Console.WriteLine("{0,20}:\t[{1}]", "DirectoryParts", String.Join(", ", entry.DirectoryParts));
	///    }
	///   </code>
	///   Gives the following results:
	///  </para>
	///  <para>
	///   <pre>
	///                    Path:   H:\freelibs\recls\100\recls.net\recls.100.sln
	///      SearchRelativePath:   recls.100.sln
	///                   Drive:   H:
	///           DirectoryPath:   H:\freelibs\recls\100\recls.net\
	///               Directory:   \freelibs\recls\100\recls.net\
	///         SearchDirectory:   H:\freelibs\recls\100\recls.net\
	///                UncDrive:
	///                    File:   recls.100.sln
	///                FileName:   recls.100
	///           FileExtension:   .sln
	///            CreationTime:   31/07/2009 11:40:44 AM
	///        ModificationTime:   20/08/2009 3:40:00 PM
	///          LastAccessTime:   20/08/2009 3:46:33 PM
	///    LastStatusChangeTime:   20/08/2009 3:40:00 PM
	///                    Size:   34157
	///              Attributes:   ReadOnly, Archive, Compressed
	///              IsReadOnly:   True
	///             IsDirectory:   False
	///                   IsUnc:   False
	///          DirectoryParts:   [\, freelibs\, recls\, 100\, recls.net\]
	///   </pre>
	///  </para>
	///  <para>
	///   <br/><br/>
	///  </para>
	///  
	/// <a name="#{9F534D7B-49E3-4471-A12D-0B2735FBAB54}"></a>
	/// <strong><u>The Recls namespace</u></strong>
	/// </summary>
	[CompilerGenerated]
	static class NamespaceDoc
	{}
}
