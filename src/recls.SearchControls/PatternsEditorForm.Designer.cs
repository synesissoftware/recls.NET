
namespace Recls.SearchControls
{
	partial class PatternsEditorForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lvwPatterns = new System.Windows.Forms.ListView();
			this.Pattern = new System.Windows.Forms.ColumnHeader();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbtnSemiColon = new System.Windows.Forms.RadioButton();
			this.rbtnPipe = new System.Windows.Forms.RadioButton();
			this.edtbarPatterns = new SynSoft.Windows.Forms.Controls.EditBarControl();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lvwPatterns
			// 
			this.lvwPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvwPatterns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.Pattern});
			this.lvwPatterns.LabelEdit = true;
			this.lvwPatterns.Location = new System.Drawing.Point(13, 41);
			this.lvwPatterns.MultiSelect = false;
			this.lvwPatterns.Name = "lvwPatterns";
			this.lvwPatterns.Size = new System.Drawing.Size(406, 223);
			this.lvwPatterns.TabIndex = 1;
			this.lvwPatterns.UseCompatibleStateImageBehavior = false;
			this.lvwPatterns.View = System.Windows.Forms.View.Details;
			// 
			// Pattern
			// 
			this.Pattern.Width = 380;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(263, 290);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(344, 290);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.rbtnSemiColon);
			this.groupBox1.Controls.Add(this.rbtnPipe);
			this.groupBox1.Location = new System.Drawing.Point(13, 270);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(176, 43);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Separator";
			// 
			// rbtnSemiColon
			// 
			this.rbtnSemiColon.AutoSize = true;
			this.rbtnSemiColon.Location = new System.Drawing.Point(73, 19);
			this.rbtnSemiColon.Name = "rbtnSemiColon";
			this.rbtnSemiColon.Size = new System.Drawing.Size(87, 17);
			this.rbtnSemiColon.TabIndex = 1;
			this.rbtnSemiColon.TabStop = true;
			this.rbtnSemiColon.Text = "Se&mi-colon \';\'";
			this.rbtnSemiColon.UseVisualStyleBackColor = true;
			// 
			// rbtnPipe
			// 
			this.rbtnPipe.AutoSize = true;
			this.rbtnPipe.Location = new System.Drawing.Point(11, 19);
			this.rbtnPipe.Name = "rbtnPipe";
			this.rbtnPipe.Size = new System.Drawing.Size(55, 17);
			this.rbtnPipe.TabIndex = 0;
			this.rbtnPipe.TabStop = true;
			this.rbtnPipe.Text = "P&ipe \'|\'";
			this.rbtnPipe.UseVisualStyleBackColor = true;
			// 
			// edtbarPatterns
			// 
			this.edtbarPatterns.AddEnabled = true;
			this.edtbarPatterns.AddLabel = "&Add";
			this.edtbarPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.edtbarPatterns.ButtonEnableMask = ((SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions)(((((SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.Add | SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.Edit)
						| SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.Delete)
						| SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.MoveUp)
						| SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.MoveDown)));
			this.edtbarPatterns.ButtonMask = ((SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions)(((((SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.Add | SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.Edit)
						| SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.Delete)
						| SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.MoveUp)
						| SynSoft.Windows.Forms.Controls.EditBarControl.ButtonOptions.MoveDown)));
			this.edtbarPatterns.DeleteEnabled = true;
			this.edtbarPatterns.DeleteLabel = "&Delete";
			this.edtbarPatterns.EditEnabled = true;
			this.edtbarPatterns.EditLabel = "&Edit";
			this.edtbarPatterns.Label = "&Patterns:";
			this.edtbarPatterns.Location = new System.Drawing.Point(12, 12);
			this.edtbarPatterns.MoveDownEnabled = true;
			this.edtbarPatterns.MoveDownLabel = "Do&wn";
			this.edtbarPatterns.MoveUpEnabled = true;
			this.edtbarPatterns.MoveUpLabel = "&Up";
			this.edtbarPatterns.Name = "edtbarPatterns";
			this.edtbarPatterns.Size = new System.Drawing.Size(407, 23);
			this.edtbarPatterns.TabIndex = 0;
			// 
			// PatternsEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(431, 325);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.edtbarPatterns);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lvwPatterns);
			this.Name = "PatternsEditorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Patterns";
			this.Load += new System.EventHandler(this.PatternsEditorForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvwPatterns;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private SynSoft.Windows.Forms.Controls.EditBarControl edtbarPatterns;
		private System.Windows.Forms.ColumnHeader Pattern;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbtnPipe;
		private System.Windows.Forms.RadioButton rbtnSemiColon;
	}
}