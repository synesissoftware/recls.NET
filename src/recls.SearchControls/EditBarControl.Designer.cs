namespace recls._100.net.SearchControls
{
	partial class EditBarControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lblLabel = new System.Windows.Forms.Label();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnMoveUp = new System.Windows.Forms.Button();
			this.btnMoveDown = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnEdit.Location = new System.Drawing.Point(103, 0);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(48, 23);
			this.btnEdit.TabIndex = 1;
			this.btnEdit.Text = "&Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btn_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(52, 0);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(48, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "&Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btn_Click);
			// 
			// lblLabel
			// 
			this.lblLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblLabel.BackColor = System.Drawing.SystemColors.Control;
			this.lblLabel.Location = new System.Drawing.Point(0, 0);
			this.lblLabel.Name = "lblLabel";
			this.lblLabel.Size = new System.Drawing.Size(43, 23);
			this.lblLabel.TabIndex = 5;
			this.lblLabel.Text = "LABEL:";
			this.lblLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnDelete.Location = new System.Drawing.Point(154, 0);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(48, 23);
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btn_Click);
			// 
			// btnMoveUp
			// 
			this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveUp.Location = new System.Drawing.Point(205, 0);
			this.btnMoveUp.Name = "btnMoveUp";
			this.btnMoveUp.Size = new System.Drawing.Size(48, 23);
			this.btnMoveUp.TabIndex = 3;
			this.btnMoveUp.Text = "&Up";
			this.btnMoveUp.UseVisualStyleBackColor = true;
			this.btnMoveUp.Click += new System.EventHandler(this.btn_Click);
			// 
			// btnMoveDown
			// 
			this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveDown.Location = new System.Drawing.Point(256, 0);
			this.btnMoveDown.Name = "btnMoveDown";
			this.btnMoveDown.Size = new System.Drawing.Size(48, 23);
			this.btnMoveDown.TabIndex = 4;
			this.btnMoveDown.Text = "Do&wn";
			this.btnMoveDown.UseVisualStyleBackColor = true;
			this.btnMoveDown.Click += new System.EventHandler(this.btn_Click);
			// 
			// EditBarControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnMoveDown);
			this.Controls.Add(this.btnMoveUp);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.lblLabel);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnAdd);
			this.Name = "EditBarControl";
			this.Size = new System.Drawing.Size(304, 23);
			this.Load += new System.EventHandler(this.EditBarControl_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Label lblLabel;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnMoveUp;
		private System.Windows.Forms.Button btnMoveDown;
	}
}
