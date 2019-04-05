
namespace Recls.SearchControls
{
	using SynSoft.Windows.Forms.Controls;

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;

	public partial class PatternsEditorForm
		: Form
	{
		#region fields

		private ListViewItem m_newEditedItem;
		#endregion

		#region properties

		public string Patterns { get; private set; }
		#endregion

		#region construction

		public PatternsEditorForm(string patterns)
		{
			InitializeComponent();

			Patterns = patterns;
		}
		#endregion

		#region implementation

		ListViewItem GetOnlySelectedItem()
		{
			if(1 == lvwPatterns.SelectedItems.Count)
			{
				return lvwPatterns.SelectedItems[0];
			}

			return null;
		}

		int GetOnlySelectedIndex()
		{
			if(1 == lvwPatterns.SelectedIndices.Count)
			{
				return lvwPatterns.SelectedIndices[0];
			}

			return -1;
		}

		private void SwapItems(int index0, int index1)
		{
			ListViewItem item0 = lvwPatterns.Items[index0];
			ListViewItem item1 = lvwPatterns.Items[index1];

			//PropertySwap(item0, item1, (item) => item.Text, (item, value) => item.Text = value);
			string t = item0.Text;
			item0.Text = item1.Text;
			item1.Text = t;
		}

		//static void PropertySwap<T>(T item0, T item1, (item) => item.Text, (item, value) => item.Text = value);

		private void Swap(string p, string p_2)
		{
			throw new NotImplementedException();
		}

		void RemoveUneditedItem(ListView listview, int index)
		{
			listview.Items.RemoveAt(index);
		}

		private void UpdateButtonMask()
		{
			int sel = GetOnlySelectedIndex();

			if(sel < 0)
			{
				edtbarPatterns.ButtonEnableMask = EditBarControl.ButtonOptions.Add;
			}
			else
			{
				EditBarControl.ButtonOptions mask = EditBarControl.ButtonOptions.Add | EditBarControl.ButtonOptions.Edit | EditBarControl.ButtonOptions.Delete;

				if(sel > 0)
				{
					mask |= EditBarControl.ButtonOptions.MoveUp;
				}
				if(sel + 1 < lvwPatterns.Items.Count)
				{
					mask |= EditBarControl.ButtonOptions.MoveDown;
				}

				edtbarPatterns.ButtonEnableMask = mask;
			}
		}

		delegate void RemoveDelegate(ListView listview, int index);
		#endregion

		#region event handlers

		private void PatternsEditorForm_Load(object sender, EventArgs e)
		{
			AcceptButton = btnOK;
			CancelButton = btnCancel;

			edtbarPatterns.AddButtonPressed += new EditBarControl.ButtonPressHandler(edtbarPatterns_AddButtonPress);
			edtbarPatterns.EditButtonPressed += new EditBarControl.ButtonPressHandler(edtbarPatterns_EditButtonPress);
			edtbarPatterns.DeleteButtonPressed += new EditBarControl.ButtonPressHandler(edtbarPatterns_DeleteButtonPress);
			edtbarPatterns.MoveUpButtonPressed += new EditBarControl.ButtonPressHandler(edtbarPatterns_MoveUpButtonPress);
			edtbarPatterns.MoveDownButtonPressed += new EditBarControl.ButtonPressHandler(edtbarPatterns_MoveDownButtonPress);

			lvwPatterns.AfterLabelEdit += new LabelEditEventHandler(lvwPatterns_AfterLabelEdit);

			lvwPatterns.SelectedIndexChanged += new EventHandler(lvwPatterns_SelectedIndexChanged);

			foreach(string pattern in Patterns.Split(new char[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries))
			{
				lvwPatterns.Items.Add(pattern);
			}

			int numSemiColons = ((IEnumerable<char>)Patterns).Count((c) => ';' == c);
			int numPipes = ((IEnumerable<char>)Patterns).Count((c) => '|' == c);

			if(numPipes > numSemiColons)
			{
				rbtnPipe.Checked = true;
			}
			else
			{
				rbtnSemiColon.Checked = true;
			}

			UpdateButtonMask();
		}

		void lvwPatterns_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateButtonMask();
		}

		void lvwPatterns_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if(null != m_newEditedItem)
			{
				try
				{
					if(null == e.Label)
					{
						BeginInvoke(new RemoveDelegate(RemoveUneditedItem), lvwPatterns, e.Item);
					}
				}
				finally
				{
					m_newEditedItem = null;
				}
			}
		}

		void edtbarPatterns_AddButtonPress(object sender, EditBarControl.EditBarButtonEventArgs e)
		{
			ListViewItem lvi = new ListViewItem();

			lvi.Text = "New Pattern";
			lvi.Name = null;
			lvi.Tag = null;

			lvwPatterns.Items.Add(lvi);

			m_newEditedItem = lvi;

			lvi.BeginEdit();
		}

		void edtbarPatterns_EditButtonPress(object sender, EditBarControl.EditBarButtonEventArgs e)
		{
			ListViewItem lvi = GetOnlySelectedItem();

			if(null != lvi)
			{
				lvi.BeginEdit();
			}
		}

		void edtbarPatterns_DeleteButtonPress(object sender, EditBarControl.EditBarButtonEventArgs e)
		{
			ListViewItem lvi = GetOnlySelectedItem();

			if(null != lvi)
			{
				lvwPatterns.Items.Remove(lvi);
			}
		}

		void edtbarPatterns_MoveUpButtonPress(object sender, EditBarControl.EditBarButtonEventArgs e)
		{
			int index = GetOnlySelectedIndex();

			if(index > 0)
			{
				SwapItems(index, index - 1);

				ListViewItem lvi = lvwPatterns.Items[index - 1];

				lvi.Selected = true;
				lvi.Focused = true;
			}
		}

		void edtbarPatterns_MoveDownButtonPress(object sender, EditBarControl.EditBarButtonEventArgs e)
		{
			int index = GetOnlySelectedIndex();

			if(index + 1 < lvwPatterns.Items.Count)
			{
				SwapItems(index, index + 1);

				ListViewItem lvi = lvwPatterns.Items[index + 1];

				lvi.Selected = true;
				lvi.Focused = true;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if(null != m_newEditedItem)
			{
				lvwPatterns.Items.Remove(m_newEditedItem);
			}

			Patterns = String.Join(rbtnSemiColon.Checked ? ";" : "|", lvwPatterns.Items.Cast<ListViewItem>().Select((lvi) => lvi.Text).ToArray());

			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
		#endregion
	}
}
