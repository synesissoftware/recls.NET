
namespace Recls.SearchControls
{
	using Recls;

	using SynSoft.Collections;

	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	//using System.Data;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;
	using System.Windows.Forms.Design;

	public partial class SimpleSearchSpecificationControl
		: UserControl
	{
		#region types

		struct Map
		{
			public Map(CheckBox control, SearchOptions flag)
			{
				Control = control;
				Flag = flag;
			}

			public CheckBox 		Control;
			public SearchOptions	Flag;
		}
		#endregion

		#region fields

		Map[]   m_maps;
		bool    m_showSearchOptions;
		bool    m_showSearchDepth;
		#endregion

		#region Construction
		public SimpleSearchSpecificationControl()
		{
			InitializeComponent();

			m_maps = new Map[]
			{
				new Map(chkFiles, SearchOptions.Files),
				new Map(chkDirectories, SearchOptions.Directories),
				new Map(chkMarkDirectories, SearchOptions.MarkDirectories),
				new Map(chkIncludeHidden, SearchOptions.IncludeHidden),
				new Map(chkIncludeSystem, SearchOptions.IncludeSystem),
				new Map(chkIgnoreInaccessibleDirectories, SearchOptions.IgnoreInaccessibleNodes),
			};

			m_showSearchOptions = true;
			m_showSearchDepth = true;

			NotifiedListAdaptor<string> directoryHistory = new NotifiedListAdaptor<string>(new HistoryList<string>());
			NotifiedListAdaptor<string> patternsHistory = new NotifiedListAdaptor<string>(new HistoryList<string>());

			DirectoryHistory = directoryHistory;
			PatternsHistory = patternsHistory;

			directoryHistory.CollectionChangedHandler += new CollectionChanged<string>(directoryHistory_CollectionChangedHandler);
			patternsHistory.CollectionChangedHandler += new CollectionChanged<string>(patternsHistory_CollectionChangedHandler);

			MaxDirectoryHistoryDepth = 10;
			MaxPatternsHistoryDepth = 10;
		}

		void patternsHistory_CollectionChangedHandler(ICollection<string> collection, NotifiedCollectionAction action, string item, int index)
		{
			switch(action)
			{
				case NotifiedCollectionAction.ItemAdded:
				case NotifiedCollectionAction.ItemInserted:
				case NotifiedCollectionAction.ItemRemoved:
				case NotifiedCollectionAction.ItemReplaced:
				case NotifiedCollectionAction.ItemMoved:
					cbxPatterns.Items.Clear();
					cbxPatterns.Items.AddRange(collection.Take(MaxPatternsHistoryDepth).ToArray());
					break;
				case NotifiedCollectionAction.CollectionCleared:
					cbxPatterns.Items.Clear();
					break;
			}
		}

		void directoryHistory_CollectionChangedHandler(ICollection<string> collection, NotifiedCollectionAction action, string item, int index)
		{
			switch(action)
			{
				case NotifiedCollectionAction.ItemAdded:
				case NotifiedCollectionAction.ItemInserted:
				case NotifiedCollectionAction.ItemRemoved:
				case NotifiedCollectionAction.ItemReplaced:
				case NotifiedCollectionAction.ItemMoved:
					cbxDirectory.Items.Clear();
					cbxDirectory.Items.AddRange(collection.Take(MaxDirectoryHistoryDepth).ToArray());
					break;
				case NotifiedCollectionAction.CollectionCleared:
					cbxDirectory.Items.Clear();
					break;
			}
		}
		#endregion

		#region properties

		public string Directory
		{
			get
			{
				return cbxDirectory.Text;
			}
			set
			{
				cbxDirectory.Text = value;
			}
		}
		public string Patterns
		{
			get
			{
				return cbxPatterns.Text;
			}
			set
			{
				cbxPatterns.Text = value;
			}
		}
		public IList<string> DirectoryHistory { get; private set; }
		public IList<string> PatternsHistory { get; private set; }
		public SearchOptions SearchOptions
		{
			get
			{
				SearchOptions options = SearchOptions.None;

				foreach(Map map in m_maps)
				{
					if(map.Control.Checked)
					{
						options |= map.Flag;
					}
				}

				return options;
			}
			set
			{
				foreach(Map map in m_maps)
				{
					map.Control.Checked = 0 != (map.Flag & value);
				}
			}
		}
		public int MaxSearchDepth
		{
			get
			{
				if(chkLimitDepth.Checked)
				{
					return (int)nudDepth.Value;
				}
				else
				{
					return -1;
				}
			}
			set
			{
				if(value < 0 || value == FileSearcher.UnrestrictedDepth)
				{
					chkLimitDepth.Checked = false;
				}
				else
				{
					chkLimitDepth.Checked = true;
					nudDepth.Value = value;
				}
			}
		}
		public int MaxDirectoryHistoryDepth { get; set; }
		public int MaxPatternsHistoryDepth { get; set; }
		public bool ShowSearchOptions
		{
			get
			{
				return m_showSearchOptions;
			}
			set
			{
				if(value != m_showSearchOptions)
				{
					grpOptions.Enabled = grpOptions.Visible = m_showSearchOptions = value;
				}
			}
		}
		public bool ShowSearchDepth
		{
			get
			{
				return m_showSearchDepth;
			}
			set
			{
				if(value != m_showSearchDepth)
				{
					grpDepth.Enabled = grpOptions.Visible = m_showSearchDepth = value;
				}
			}
		}
		#endregion

		#region event handlers

		private void SimpleSearchSpecificationControl_Load(object sender, EventArgs e)
		{
			if(null != DirectoryHistory)
			{
				foreach(string directory in DirectoryHistory)
				{
					cbxDirectory.Items.Add(directory);
				}
			}

			if(null != PatternsHistory)
			{
				foreach(string patterns in PatternsHistory)
				{
					cbxPatterns.Items.Add(patterns);
				}
			}
		}

		private void btnDirectoryBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog form = new FolderBrowserDialog();

			form.Description = "Select the search directory";
			form.SelectedPath = cbxDirectory.Text;
			form.ShowNewFolderButton = false;

			if(DialogResult.OK == form.ShowDialog(this))
			{
				Directory = form.SelectedPath;
			}
		}

		private void btnPatternEdit_Click(object sender, EventArgs e)
		{
			PatternsEditorForm form = new PatternsEditorForm(cbxPatterns.Text);

			if(DialogResult.OK == form.ShowDialog(this))
			{
				cbxPatterns.Text = form.Patterns;
			}
		}
		#endregion

		private void chkLimitDepth_CheckedChanged(object sender, EventArgs e)
		{
			nudDepth.Enabled = chkLimitDepth.Checked;
		}

		private void nudDepth_ValueChanged(object sender, EventArgs e)
		{
			if(nudDepth.Value < 0)
			{
				chkLimitDepth.Checked = false;
			}
		}
	}
}
