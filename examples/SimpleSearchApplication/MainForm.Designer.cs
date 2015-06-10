
namespace SimpleSearchApplication
{
	partial class MainForm
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
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.labelMain = new System.Windows.Forms.ToolStripStatusLabel();
			this.labelSummary = new System.Windows.Forms.ToolStripStatusLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.lvwResults = new System.Windows.Forms.ListView();
			this.ColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ColumnDirectory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ColumnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ColumnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnSearch = new System.Windows.Forms.Button();
			this.searchSpec = new Recls.SearchControls.SimpleSearchSpecificationControl();
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.labelMain,
			this.labelSummary});
			this.statusStrip.Location = new System.Drawing.Point(0, 416);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(436, 22);
			this.statusStrip.TabIndex = 4;
			this.statusStrip.Text = "statusStrip1";
			// 
			// labelMain
			// 
			this.labelMain.Name = "labelMain";
			this.labelMain.Size = new System.Drawing.Size(421, 17);
			this.labelMain.Spring = true;
			this.labelMain.Text = "Ready";
			this.labelMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelSummary
			// 
			this.labelSummary.Name = "labelSummary";
			this.labelSummary.Size = new System.Drawing.Size(0, 17);
			this.labelSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 241);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "R&esults:";
			// 
			// lvwResults
			// 
			this.lvwResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lvwResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.ColumnName,
			this.ColumnDirectory,
			this.ColumnType,
			this.ColumnSize});
			this.lvwResults.Location = new System.Drawing.Point(12, 257);
			this.lvwResults.Name = "lvwResults";
			this.lvwResults.Size = new System.Drawing.Size(412, 126);
			this.lvwResults.TabIndex = 2;
			this.lvwResults.UseCompatibleStateImageBehavior = false;
			this.lvwResults.View = System.Windows.Forms.View.Details;
			// 
			// ColumnName
			// 
			this.ColumnName.Text = "Name";
			this.ColumnName.Width = 100;
			// 
			// ColumnDirectory
			// 
			this.ColumnDirectory.Tag = "Directory";
			this.ColumnDirectory.Text = "Directory";
			this.ColumnDirectory.Width = 120;
			// 
			// ColumnType
			// 
			this.ColumnType.Text = "Type";
			// 
			// ColumnSize
			// 
			this.ColumnSize.Text = "Size";
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(349, 389);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 23);
			this.btnSearch.TabIndex = 3;
			this.btnSearch.Text = "Se&arch";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// searchSpec
			// 
			this.searchSpec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.searchSpec.Directory = "";
			this.searchSpec.Location = new System.Drawing.Point(12, 12);
			this.searchSpec.MaxDirectoryHistoryDepth = 10;
			this.searchSpec.MaxPatternsHistoryDepth = 10;
			this.searchSpec.MaxSearchDepth = -1;
			this.searchSpec.Name = "searchSpec";
			this.searchSpec.Patterns = "";
			this.searchSpec.SearchOptions = Recls.SearchOptions.Files;
			this.searchSpec.ShowSearchDepth = true;
			this.searchSpec.ShowSearchOptions = true;
			this.searchSpec.Size = new System.Drawing.Size(412, 220);
			this.searchSpec.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(436, 438);
			this.Controls.Add(this.searchSpec);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.lvwResults);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.statusStrip);
			this.Name = "MainForm";
			this.Text = "Simple Search Application";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel labelMain;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lvwResults;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.ColumnHeader ColumnName;
		private System.Windows.Forms.ColumnHeader ColumnDirectory;
		private System.Windows.Forms.ColumnHeader ColumnType;
		private System.Windows.Forms.ColumnHeader ColumnSize;
		private Recls.SearchControls.SimpleSearchSpecificationControl searchSpec;
		private System.Windows.Forms.ToolStripStatusLabel labelSummary;
	}
}

