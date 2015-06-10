
namespace SimpleSearchApplication
{
	using SynSoft.Collections;

	using Recls;

	using Pantheios;
	using Pantheios.ApplicationLayer;
	using Pantheios.ApplicationLayer.Inserters;
	using Pantheios.ApplicationLayer.Scopes;
	using Pantheios.Util;

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading;
	using System.Windows.Forms;

	public partial class MainForm
		: Form
	{
		#region Types
		class EntryListViewItem
			: ListViewItem
		{
			public EntryListViewItem(IEntry entry)
				: base(entry.File)
			{
				SubItems.Add(entry.DirectoryPath);
				SubItems.Add(entry.IsDirectory ? "directory" : "file");
				SubItems.Add(entry.Size.ToString());
			}
		}
		class SearchState
			: IProgressHandler
			, IExceptionHandler
		{
			internal SearchState(MainForm form, string directory, string patterns, SearchOptions options, int maxSearchDepth)
			{
				Form			=	form;
				HandleProgress	=	new OnProgress(form.HandleProgress);
				HandleEntry 	=	new OnEntry(form.HandleEntry);
				HandleException =	new OnException(form.HandleException);

				Entries = FileSearcher.Search(
					directory,
					patterns,
					options,
					maxSearchDepth,
					this,
					(0 == (options & SearchOptions.IgnoreInaccessibleNodes)) ? this : null
				);
			}

			#region Fields
			public readonly MainForm			Form;
			public readonly OnProgress			HandleProgress;
			public readonly OnEntry 			HandleEntry;
			public readonly OnException 		HandleException;
			public readonly IEnumerable<IEntry> Entries;
			#endregion

			#region IProgressHandler Members
			ProgressHandlerResult IProgressHandler.OnProgress(string directory, int depth)
			{
				return (ProgressHandlerResult)Form.Invoke(HandleProgress, directory, depth);
			}
			#endregion

			#region IExceptionHandler Members
			ExceptionHandlerResult IExceptionHandler.OnException(string path, Exception x)
			{
				return (ExceptionHandlerResult)Form.Invoke(HandleException, path, x);
			}
			#endregion
		}
		#endregion

		#region Fields
		SearchState m_searchState;
		int 		m_numFiles;
		int 		m_numDirectories;
		#endregion

		public MainForm()
		{
			Api.Log(Severity.Debug, "MainForm.MainForm()");

			InitializeComponent();
		}

		private ProgressHandlerResult HandleProgress(string directory, int depth)
		{
			Api.Log(Severity.Debug, "MainForm.HandleProgress(directory=", directory, ", depth=", depth, ")");

			labelMain.Text = directory;

			if(null == m_searchState)
			{
				return ProgressHandlerResult.CancelSearch;
			}

			return ProgressHandlerResult.Continue;
		}
		private void HandleEntry(IEntry entry)
		{
			Api.Log(Severity.Debug, "MainForm.HandleEntry(entry=", entry, ")");

			if(null == entry)
			{
				ResetSearch();
			}
			else
			{
				if(entry.IsDirectory)
				{
					++m_numDirectories;
				}
				else
				{
					++m_numFiles;
				}

				//lvwResults.BeginUpdate();
				//try
				//{
					lvwResults.Items.Add(new EntryListViewItem(entry));
				//}
				//finally
				//{
				//	  lvwResults.EndUpdate();
				//}
			}
		}

		private void ResetSearch()
		{
			// End the search
			m_searchState			=	null;
			labelMain.Text			=	"Ready";
			btnSearch.Text			=	"Se&arch";
			btnSearch.Enabled		=	true;
			labelSummary.Visible	=	true;

			switch(searchSpec.SearchOptions & (SearchOptions.Files | SearchOptions.Directories))
			{
				case SearchOptions.Files:
				case 0:
					labelSummary.Text = String.Format("{0} {1}", m_numFiles, (1 == m_numFiles) ? "file" : "files");
					break;
				case SearchOptions.Directories:
					labelSummary.Text = String.Format("{0} {1}", m_numDirectories, (1 == m_numDirectories) ? "directory" : "directories");
					break;
				case SearchOptions.Files | SearchOptions.Directories:
					labelSummary.Text = String.Format("{0} {1}, {2} {3}", m_numFiles, (1 == m_numFiles) ? "file" : "files", m_numDirectories, (1 == m_numDirectories) ? "directory" : "directories");
					break;
			}
			searchSpec.Enabled = true;
		}
		private ExceptionHandlerResult HandleException(string path, Exception x)
		{
			Api.Log(Severity.Debug, "MainForm.HandleException(path=", path, ", x=", Api.Insert.Exception(x), ")");

			switch(MessageBox.Show(this, String.Format("The search failed during search of {0}: {1}\n\nDo you wish to proceed (Yes) or cancel the search (No)?", path, x.Message), null, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
			{
				case DialogResult.Yes:
					return ExceptionHandlerResult.ConsumeExceptionAndContinue;
				default:
				case DialogResult.No:
					ResetSearch();
					return ExceptionHandlerResult.PropagateException;
			}
		}

		delegate ProgressHandlerResult OnProgress(string directory, int depth);
		delegate void OnEntry(IEntry entry);
		delegate ExceptionHandlerResult OnException(string path, Exception x);

		private static void SearchProc(object state)
		{
			using(new ThreadNameScope("Worker"))
			{
				Api.Log(Severity.Debug, "MainForm.SearchProc(state=", state, ")");

				SearchState info = (SearchState)state;

				try
				{
					foreach(IEntry entry in info.Entries)
					{
						info.Form.Invoke(info.HandleEntry, entry);
					}
				}
				catch(Exception x)
				{
					Api.Log(Severity.Critical, "search failed: ", Api.Insert.Exception(x));
				}
				finally
				{
					info.Form.Invoke(info.HandleEntry, (IEntry)null);
				}
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			AcceptButton = btnSearch;

			//// Attempt to prevent flicker - failed
			//ControlStyles newStyle = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
			//Type type = lvwResults.GetType();
			//MethodInfo mi = type.GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			//mi.Invoke(lvwResults, new object[] { newStyle });
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = (null != m_searchState);
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			if(null != m_searchState)
			{
				m_searchState = null;
				btnSearch.Enabled = false;
			}
			else
			{
				searchSpec.DirectoryHistory.Add(searchSpec.Directory);
				searchSpec.PatternsHistory.Add(searchSpec.Patterns);

				m_numFiles = 0;
				m_numDirectories = 0;
				lvwResults.Items.Clear();

				int depth = searchSpec.MaxSearchDepth;

				if(depth < 0)
				{
					depth = FileSearcher.UnrestrictedDepth;
				}

				m_searchState			=	new SearchState(this, searchSpec.Directory, searchSpec.Patterns, searchSpec.SearchOptions, depth);
				btnSearch.Text			=	"C&ancel";
				labelSummary.Visible	=	false;
				searchSpec.Enabled		=	false;

				ThreadPool.QueueUserWorkItem(SearchProc, m_searchState);
			}
		}
	}
}
