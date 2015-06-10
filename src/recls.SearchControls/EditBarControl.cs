
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace recls._100.net.SearchControls
{
	[DefaultEvent("ButtonPress")]
	public partial class EditBarControl
		: UserControl
	{
		#region Member Types
		[Flags]
		public enum ButtonOptions : int
		{
			/// <summary>
			///  Shows no buttons.
			/// </summary>
			None = 0,
			/// <summary>
			///  Shows the Add button, and fires the
			///  <see cref="EditBarControl.AddButtonPress">AddButtonPress</see>
			///  event.
			/// </summary>
			Add = 0x0001,
			/// <summary>
			///  Shows the Edit button, and fires the
			///  <see cref="EditBarControl.EditButtonPress">EditButtonPress</see>
			///  event.
			/// </summary>
			Edit = 0x0002,
			/// <summary>
			///  Shows the Delete button, and fires the
			///  <see cref="EditBarControl.DeleteButtonPress">DeleteButtonPress</see>
			///  event.
			/// </summary>
			Delete = 0x0004,
			MoveUp = 0x0008,
			MoveDown = 0x0010,
			Move = MoveUp | MoveDown,
			All = Add | Edit | Delete | Move,
		}

		public class EditBarButtonEventArgs
			: EventArgs
		{
			internal EditBarButtonEventArgs(EditBarControl editBar, ButtonOptions button)
			{
				EditBar = editBar;
				Button = button;
			}

			public EditBarControl EditBar { get; private set;}
			public ButtonOptions Button { get; private set; }
		}

		public delegate void ButtonPressHandler(object sender, EditBarButtonEventArgs e);
		#endregion

		#region Member Variables
		ButtonOptions m_options;
		#endregion

		#region Construction
		public EditBarControl()
		{
			InitializeComponent();

			ButtonMask = ButtonOptions.All;
		}
		#endregion

		#region Properties
		/// <summary>
		///  Gets or sets the button options.
		/// </summary>
		public ButtonOptions ButtonMask
		{
			get
			{
				return m_options;
			}
			set
			{
				if(value != m_options)
				{
					m_options = value;
					ReorganiseLayout();
				}
			}
		}

		/// <summary>
		///  Gets or sets the mask representing the enabled state of the
		///  buttons.
		/// </summary>
		public ButtonOptions ButtonEnableMask
		{
			get
			{
				ButtonOptions mask = ButtonOptions.None;

				if(btnAdd.Enabled)
				{
					mask |= ButtonOptions.Add;
				}
				if(btnEdit.Enabled)
				{
					mask |= ButtonOptions.Edit;
				}
				if(btnDelete.Enabled)
				{
					mask |= ButtonOptions.Delete;
				}
				if(btnMoveUp.Enabled)
				{
					mask |= ButtonOptions.MoveUp;
				}
				if(btnMoveDown.Enabled)
				{
					mask |= ButtonOptions.MoveDown;
				}

				return mask;
			}
			set
			{
				btnAdd.Enabled = 0 != (value & ButtonOptions.Add);
				btnEdit.Enabled = 0 != (value & ButtonOptions.Edit);
				btnDelete.Enabled = 0 != (value & ButtonOptions.Delete);
				btnMoveUp.Enabled = 0 != (value & ButtonOptions.MoveUp);
				btnMoveDown.Enabled = 0 != (value & ButtonOptions.MoveDown);
			}
		}

		/// <summary>
		///  Gets or sets the enabled state of the Add button.
		/// </summary>
		public bool AddEnabled
		{
			get
			{
				return btnAdd.Enabled;
			}
			set
			{
				btnAdd.Enabled = value;
			}
		}

		/// <summary>
		///  Gets or sets the enabled state of the Edit button.
		/// </summary>
		public bool EditEnabled
		{
			get
			{
				return btnEdit.Enabled;
			}
			set
			{
				btnEdit.Enabled = value;
			}
		}

		/// <summary>
		///  Gets or sets the enabled state of the Delete button.
		/// </summary>
		public bool DeleteEnabled
		{
			get
			{
				return btnDelete.Enabled;
			}
			set
			{
				btnDelete.Enabled = value;
			}
		}

		/// <summary>
		///  Gets or sets the enabled state of the MoveUp button.
		/// </summary>
		public bool MoveUpEnabled
		{
			get
			{
				return btnMoveUp.Enabled;
			}
			set
			{
				btnMoveUp.Enabled = value;
			}
		}

		/// <summary>
		///  Gets or sets the enabled state of the MoveDown button.
		/// </summary>
		public bool MoveDownEnabled
		{
			get
			{
				return btnMoveDown.Enabled;
			}
			set
			{
				btnMoveDown.Enabled = value;
			}
		}

		/// <summary>
		///  Gets or sets the label.
		/// </summary>
		public string Label
		{
			get
			{
				return lblLabel.Text;
			}
			set
			{
				lblLabel.Text = value;
			}
		}

		/// <summary>
		///  The text of the Add button.
		/// </summary>
		public string AddLabel
		{
			get
			{
				return btnAdd.Text;
			}
			set
			{
				btnAdd.Text = value;
			}
		}

		/// <summary>
		///  The text of the Edit button.
		/// </summary>
		public string EditLabel
		{
			get
			{
				return btnEdit.Text;
			}
			set
			{
				btnEdit.Text = value;
			}
		}

		/// <summary>
		///  The text of the Delete button.
		/// </summary>
		public string DeleteLabel
		{
			get
			{
				return btnDelete.Text;
			}
			set
			{
				btnDelete.Text = value;
			}
		}

		/// <summary>
		///  The text of the MoveUp button.
		/// </summary>
		public string MoveUpLabel
		{
			get
			{
				return btnMoveUp.Text;
			}
			set
			{
				btnMoveUp.Text = value;
			}
		}

		/// <summary>
		///  The text of the MoveDown button.
		/// </summary>
		public string MoveDownLabel
		{
			get
			{
				return btnMoveDown.Text;
			}
			set
			{
				btnMoveDown.Text = value;
			}
		}
		#endregion

		#region Events
		public event ButtonPressHandler ButtonPress;
		public event ButtonPressHandler AddButtonPress;
		public event ButtonPressHandler EditButtonPress;
		public event ButtonPressHandler DeleteButtonPress;
		public event ButtonPressHandler MoveUpButtonPress;
		public event ButtonPressHandler MoveDownButtonPress;
		#endregion

		#region Message Handlers
		private void EditBarControl_Load(object sender, EventArgs e)
		{
			ReorganiseLayout();
		}

		private void ReorganiseLayout()
		{
			Size upSize = btnMoveUp.Size;
			Size deleteSize = btnDelete.Size;
			Size editSize = btnEdit.Size;
			Size addSize = btnAdd.Size;

			int gap = btnMoveDown.Location.X - (btnMoveUp.Location.X + upSize.Width);
			int rhs = this.Size.Width;
			bool shiftRequired = false;

			if(0 != (ButtonMask & ButtonOptions.MoveDown))
			{
				rhs -= btnMoveDown.Size.Width + gap;
			}
			else
			{
				btnMoveDown.Visible = false;
				btnMoveDown.Enabled = false;
				shiftRequired = true;
			}
			if(0 != (ButtonMask & ButtonOptions.MoveUp))
			{
				if(shiftRequired)
				{
					btnMoveUp.Location = new Point(rhs - upSize.Width, btnMoveUp.Location.Y);
				}

				rhs -= upSize.Width + gap;
			}
			else
			{
				btnMoveUp.Visible = false;
				btnMoveUp.Enabled = false;
				shiftRequired = true;
			}
			if(0 != (ButtonMask & ButtonOptions.Delete))
			{
				if(shiftRequired)
				{
					btnDelete.Location = new Point(rhs - deleteSize.Width, btnDelete.Location.Y);
				}

				rhs -= deleteSize.Width + gap;
			}
			else
			{
				btnDelete.Visible = false;
				btnDelete.Enabled = false;
				shiftRequired = true;
			}
			if(0 != (ButtonMask & ButtonOptions.Edit))
			{
				if(shiftRequired)
				{
					btnEdit.Location = new Point(rhs - editSize.Width, btnEdit.Location.Y);
				}

				rhs -= editSize.Width + gap;
			}
			else
			{
				btnEdit.Visible = false;
				btnEdit.Enabled = false;
				shiftRequired = true;
			}
			if(0 != (ButtonMask & ButtonOptions.Add))
			{
				if(shiftRequired)
				{
					btnAdd.Location = new Point(rhs - addSize.Width, btnAdd.Location.Y);
				}

				rhs -= addSize.Width + gap;
			}
			else
			{
				btnAdd.Visible = false;
				btnAdd.Enabled = false;
				shiftRequired = true;
			}

			lblLabel.Size = new Size(rhs, lblLabel.Height);

			this.Refresh();
		}

		private void btn_Click(object sender, EventArgs e)
		{
			ButtonOptions btnId = ButtonOptions.None;
			ButtonPressHandler ev = null;

			if(sender == btnAdd)
			{
				btnId = ButtonOptions.Add;
				ev = AddButtonPress;
			}
			else if(sender == btnEdit)
			{
				btnId = ButtonOptions.Edit;
				ev = EditButtonPress;
			}
			else if(sender == btnDelete)
			{
				btnId = ButtonOptions.Delete;
				ev = DeleteButtonPress;
			}
			else if(sender == btnMoveUp)
			{
				btnId = ButtonOptions.MoveUp;
				ev = MoveUpButtonPress;
			}
			else if(sender == btnMoveDown)
			{
				btnId = ButtonOptions.MoveDown;
				ev = MoveDownButtonPress;
			}
			else
			{
				Debug.Assert(false, "invalid state");
			}

			if(null != ev)
			{
				ev.Invoke(this, new EditBarButtonEventArgs(this, btnId));
			}

			ev = ButtonPress;

			if(null != ev)
			{
				ev.Invoke(this, new EditBarButtonEventArgs(this, btnId));
			}
		}
		#endregion
	}
}
