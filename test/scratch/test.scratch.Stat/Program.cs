
namespace ConsoleApplication1
{
    using global::Recls;
    using Recls = global::Recls.Api;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IEntry      e;
                IEntry2_1   e2;

                e   =   Recls.Stat("Z:/does-not-exist.ext");

                e2 = (IEntry2_1)Recls.Stat("Z:/does-not-exist.ext", SearchOptions.StatInfoForNonexistentPath);

                e2 = (IEntry2_1)Recls.Stat("Z:/does-not-exist.ext", SearchOptions.StatInfoForNonexistentPath | SearchOptions.Files);

                Console.WriteLine("'{0}' is a {1} and does {2}exist", e2, e2.IsFile ? "file" : "directory", e2.Existed ? "" : "not ");

                e2 = (IEntry2_1)Recls.Stat("Z:/does-not-exist/file.ext", SearchOptions.StatInfoForNonexistentPath | SearchOptions.Files);

                Console.WriteLine("'{0}' is a {1} and does {2}exist", e2, e2.IsFile ? "file" : "directory", e2.Existed ? "" : "not ");

                e2 = (IEntry2_1)Recls.Stat("Z:/does-not-exist/file.ext", SearchOptions.StatInfoForNonexistentPath | SearchOptions.Directories);

                Console.WriteLine("'{0}' is a {1} and does {2}exist", e2, e2.IsFile ? "file" : "directory", e2.Existed ? "" : "not ");

                e2 = (IEntry2_1)Recls.Stat("Z:/does-not-exist/file.ext", SearchOptions.StatInfoForNonexistentPath | SearchOptions.Directories | SearchOptions.Files);



                e2 = (IEntry2_1)Recls.Stat("J:/does-not-exist.ext", SearchOptions.StatInfoForNonexistentPath | SearchOptions.Files);
            }
            catch(Exception x)
            {
                Console.WriteLine(x);
            }
        }
    }
}
