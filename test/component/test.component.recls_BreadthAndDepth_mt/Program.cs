
#if COMPILE_AS_PROGRAM
[assembly: Pantheios.UseLogService(typeof(Pantheios.Services.TraceLogService), FilterThreshold=Pantheios.ApplicationLayer.Severity.Informational)]
#endif


namespace test.component.recls_BreadthAndDepth_mt
{
	using Recls;

#if !COMPILE_AS_PROGRAM
	using NUnit.Framework;
#endif

#if COMPILE_AS_PROGRAM
	using Pantheios.ApplicationLayer;
	using Pantheios.Runtime;

	using SynSoft.Environment;
#endif

	using System;
	using System.Collections.Generic;
	using System.Threading;

#if !COMPILE_AS_PROGRAM
	[TestFixture]
#endif
	class Program
	{
		class SearchInfo
		{
			private readonly List<IEntry> m_results;
			private readonly Semaphore m_semaphore;

			public SearchInfo(IEnumerable<IEntry> search, Semaphore semaphore)
			{
				Search = search;
				m_results = new List<IEntry>();
				m_semaphore = semaphore;

				m_semaphore.WaitOne();
			}

			public IEnumerable<IEntry>	Search { get; private set; }
			public List<IEntry> Results
			{
				get
				{
					return m_results;
				}
			}

			public void Run()
			{
				Pantheios.Api.Log(Severity.Informational, "SearchInfo.Run() - entry");

				try
				{
					foreach(IEntry entry in Search)
					{
						m_results.Add(entry);
					}
				}
				finally
				{
					m_semaphore.Release(1);

					Pantheios.Api.Log(Severity.Informational, "SearchInfo.Run() - exit");
				}
			}
		}

		static void SearchProc(object obj)
		{
			SearchInfo info = (SearchInfo)obj;

			info.Run();
		}

		static void Main(string[] argv)
		{
			string directory = null;
			string patterns = null;
			int depth = FileSearcher.UnrestrictedDepth;

			switch(argv.Length)
			{
				case 0:
					break;
				case 1:
					directory = argv[0];
					if(directory.IndexOfAny(new char[] { '?', '*', '|' }) >= 0)
					{
						patterns = directory;
						directory = null;
					}
					break;
				case 2:
					directory = argv[0];
					patterns = argv[1];
					break;
				case 3:
					directory = argv[0];
					patterns = argv[1];
					depth = int.Parse(argv[2]);
					break;
				default:
					Console.Error.WriteLine("USAGE: {0} [<directory>]", EnvironmentUtil.TryEvaluateProcessName(null));
					Environment.Exit(ExitCode.Failure);
					break;
			}

			DoSearch(directory, patterns, depth);
		}

		private static void DoSearch(string directory, string patterns, int depth)
		{
			Pantheios.Api.Flog(Severity.Informational, "Searching directory '{0}' with patterns '{1}' to depth {2}", directory, patterns, depth);

			Semaphore semaphore = new Semaphore(3, 3);

			SearchInfo defaultSearch = new SearchInfo(FileSearcher.Search(directory, patterns, SearchOptions.Files, depth), semaphore);
			SearchInfo depthSearch = new SearchInfo(FileSearcher.DepthFirst.Search(directory, patterns, SearchOptions.Files, depth), semaphore);
			SearchInfo breadthSearch = new SearchInfo(FileSearcher.BreadthFirst.Search(directory, patterns, SearchOptions.Files, depth), semaphore);

			Pantheios.Api.QueueLoggedThreadPoolWorkItem(SearchProc, defaultSearch);
			Pantheios.Api.QueueLoggedThreadPoolWorkItem(SearchProc, depthSearch);
			Pantheios.Api.QueueLoggedThreadPoolWorkItem(SearchProc, breadthSearch);

			Pantheios.Api.Log(Severity.Informational, "waiting for search threads to complete");

			semaphore.WaitOne();
			semaphore.WaitOne();
			Pantheios.Api.Log(Severity.Informational, "waiting for last search thread to complete");
			semaphore.WaitOne();
			Pantheios.Api.Log(Severity.Informational, "search threads complete");

			Console.WriteLine("default: #items: {0}", defaultSearch.Results.Count);
			Console.WriteLine("depth: #items: {0}", depthSearch.Results.Count);
			Console.WriteLine("breadth: #items: {0}", breadthSearch.Results.Count);

			if(defaultSearch.Results.Count == depthSearch.Results.Count && depthSearch.Results.Count == breadthSearch.Results.Count)
			{
				defaultSearch.Results.Sort((x, y) => String.CompareOrdinal(x.Path, y.Path));
				depthSearch.Results.Sort((x, y) => String.CompareOrdinal(x.Path, y.Path));
				breadthSearch.Results.Sort((x, y) => String.CompareOrdinal(x.Path, y.Path));

				for(int i = 0; i != depthSearch.Results.Count; ++i)
				{
					IEntry defaultEntry = defaultSearch.Results[i];
					IEntry depthEntry = depthSearch.Results[i];
					IEntry breadthEntry = breadthSearch.Results[i];

					if(defaultEntry.Path != depthEntry.Path || depthEntry.Path != breadthEntry.Path)
					{
						Console.WriteLine("different paths at index {1}{0}\tdefault:\t{2}{0}\tdepth:\t{3}{0}\tbreadth:\t{4}{0}", Environment.NewLine, i, defaultEntry, depthEntry, breadthEntry);
					}
					if(defaultEntry.Size != depthEntry.Size || depthEntry.Size != breadthEntry.Size)
					{
						Console.WriteLine("different sizes at index {1}{0}\tdefault:\t{2}{0}\tdepth:\t{3}{0}\tbreadth:\t{4}{0}", Environment.NewLine, i, defaultEntry, depthEntry, breadthEntry);
					}
				}
			}
		}
	}
}
