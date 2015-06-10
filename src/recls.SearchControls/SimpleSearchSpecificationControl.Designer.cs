
namespace Recls.SearchControls
{
	partial class SimpleSearchSpecificationControl
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
			this.lblDirectory = new System.Windows.Forms.Label();
			this.cbxDirectory = new System.Windows.Forms.ComboBox();
			this.btnDirectoryBrowse = new System.Windows.Forms.Button();
			this.lblPatterns = new System.Windows.Forms.Label();
			this.cbxPatterns = new System.Windows.Forms.ComboBox();
			this.btnPatternEdit = new System.Windows.Forms.Button();
			this.grpOptions = new System.Windows.Forms.GroupBox();
			this.chkIgnoreInaccessibleDirectories = new System.Windows.Forms.CheckBox();
			this.chkIncludeSystem = new System.Windows.Forms.CheckBox();
			this.chkIncludeHidden = new System.Windows.Forms.CheckBox();
			this.chkMarkDirectories = new System.Windows.Forms.CheckBox();
			this.chkDirectories = new System.Windows.Forms.CheckBox();
			this.chkFiles = new System.Windows.Forms.CheckBox();
			this.grpDepth = new System.Windows.Forms.GroupBox();
			this.nudDepth = new System.Windows.Forms.NumericUpDown();
			this.chkLimitDepth = new System.Windows.Forms.CheckBox();
			this.grpOptions.SuspendLayout();
			this.grpDepth.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDepth)).BeginInit();
			this.SuspendLayout();
			// 
			// lblDirectory
			// 
			this.lblDirectory.AutoSize = true;
			this.lblDirectory.Location = new System.Drawing.Point(-3, 0);
			this.lblDirectory.Name = "lblDirectory";
			this.lblDirectory.Size = new System.Drawing.Size(52, 13);
			this.lblDirectory.TabIndex = 0;
			this.lblDirectory.Text = "&Directory:";
			// 
			// cbxDirectory
			// 
			this.cbxDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbxDirectory.DropDownHeight = 150;
			this.cbxDirectory.FormattingEnabled = true;
			this.cbxDirectory.IntegralHeight = false;
			this.cbxDirectory.Location = new System.Drawing.Point(55, 0);
			this.cbxDirectory.Name = "cbxDirectory";
			this.cbxDirectory.Size = new System.Drawing.Size(227, 21);
			this.cbxDirectory.TabIndex = 1;
			// 
			// btnDirectoryBrowse
			// 
			this.btnDirectoryBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDirectoryBrowse.Location = new System.Drawing.Point(288, 0);
			this.btnDirectoryBrowse.Name = "btnDirectoryBrowse";
			this.btnDirectoryBrowse.Size = new System.Drawing.Size(32, 21);
			this.btnDirectoryBrowse.TabIndex = 2;
			this.btnDirectoryBrowse.Text = "&...";
			this.btnDirectoryBrowse.UseVisualStyleBackColor = true;
			this.btnDirectoryBrowse.Click += new System.EventHandler(this.btnDirectoryBrowse_Click);
			// 
			// lblPatterns
			// 
			this.lblPatterns.AutoSize = true;
			this.lblPatterns.Location = new System.Drawing.Point(-3, 34);
			this.lblPatterns.Name = "lblPatterns";
			this.lblPatterns.Size = new System.Drawing.Size(49, 13);
			this.lblPatterns.TabIndex = 3;
			this.lblPatterns.Text = "&Patterns:";
			// 
			// cbxPatterns
			// 
			this.cbxPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbxPatterns.DropDownHeight = 150;
			this.cbxPatterns.FormattingEnabled = true;
			this.cbxPatterns.IntegralHeight = false;
			this.cbxPatterns.Location = new System.Drawing.Point(55, 34);
			this.cbxPatterns.Name = "cbxPatterns";
			this.cbxPatterns.Size = new System.Drawing.Size(227, 21);
			this.cbxPatterns.TabIndex = 4;
			// 
			// btnPatternEdit
			// 
			this.btnPatternEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPatternEdit.Location = new System.Drawing.Point(288, 34);
			this.btnPatternEdit.Name = "btnPatternEdit";
			this.btnPatternEdit.Size = new System.Drawing.Size(32, 21);
			this.btnPatternEdit.TabIndex = 5;
			this.btnPatternEdit.Text = "&...";
			this.btnPatternEdit.UseVisualStyleBackColor = true;
			this.btnPatternEdit.Click += new System.EventHandler(this.btnPatternEdit_Click);
			// 
			// grpOptions
			// 
			this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpOptions.Controls.Add(this.chkIgnoreInaccessibleDirectories);
			this.grpOptions.Controls.Add(this.chkIncludeSystem);
			this.grpOptions.Controls.Add(this.chkIncludeHidden);
			this.grpOptions.Controls.Add(this.chkMarkDirectories);
			this.grpOptions.Controls.Add(this.chkDirectories);
			this.grpOptions.Controls.Add(this.chkFiles);
			this.grpOptions.Location = new System.Drawing.Point(0, 64);
			this.grpOptions.Name = "grpOptions";
			this.grpOptions.Size = new System.Drawing.Size(320, 96);
			this.grpOptions.TabIndex = 6;
			this.grpOptions.TabStop = false;
			this.grpOptions.Text = "Options";
			// 
			// chkIgnoreInaccessibleDirectories
			// 
			this.chkIgnoreInaccessibleDirectories.AutoSize = true;
			this.chkIgnoreInaccessibleDirectories.Location = new System.Drawing.Point(142, 68);
			this.chkIgnoreInaccessibleDirectories.Name = "chkIgnoreInaccessibleDirectories";
			this.chkIgnoreInaccessibleDirectories.Size = new System.Drawing.Size(168, 17);
			this.chkIgnoreInaccessibleDirectories.TabIndex = 5;
			this.chkIgnoreInaccessibleDirectories.Text = "I&gnore inaccessible directories";
			this.chkIgnoreInaccessibleDirectories.UseVisualStyleBackColor = true;
			// 
			// chkIncludeSystem
			// 
			this.chkIncludeSystem.AutoSize = true;
			this.chkIncludeSystem.Location = new System.Drawing.Point(142, 44);
			this.chkIncludeSystem.Name = "chkIncludeSystem";
			this.chkIncludeSystem.Size = new System.Drawing.Size(130, 17);
			this.chkIncludeSystem.TabIndex = 4;
			this.chkIncludeSystem.Text = "Include &system entries";
			this.chkIncludeSystem.UseVisualStyleBackColor = true;
			// 
			// chkIncludeHidden
			// 
			this.chkIncludeHidden.AutoSize = true;
			this.chkIncludeHidden.Location = new System.Drawing.Point(142, 20);
			this.chkIncludeHidden.Name = "chkIncludeHidden";
			this.chkIncludeHidden.Size = new System.Drawing.Size(130, 17);
			this.chkIncludeHidden.TabIndex = 3;
			this.chkIncludeHidden.Text = "Include &hidden entries";
			this.chkIncludeHidden.UseVisualStyleBackColor = true;
			// 
			// chkMarkDirectories
			// 
			this.chkMarkDirectories.AutoSize = true;
			this.chkMarkDirectories.Location = new System.Drawing.Point(11, 68);
			this.chkMarkDirectories.Name = "chkMarkDirectories";
			this.chkMarkDirectories.Size = new System.Drawing.Size(101, 17);
			this.chkMarkDirectories.TabIndex = 2;
			this.chkMarkDirectories.Text = "&Mark directories";
			this.chkMarkDirectories.UseVisualStyleBackColor = true;
			// 
			// chkDirectories
			// 
			this.chkDirectories.AutoSize = true;
			this.chkDirectories.Location = new System.Drawing.Point(11, 44);
			this.chkDirectories.Name = "chkDirectories";
			this.chkDirectories.Size = new System.Drawing.Size(76, 17);
			this.chkDirectories.TabIndex = 1;
			this.chkDirectories.Text = "Di&rectories";
			this.chkDirectories.UseVisualStyleBackColor = true;
			// 
			// chkFiles
			// 
			this.chkFiles.AutoSize = true;
			this.chkFiles.Location = new System.Drawing.Point(11, 20);
			this.chkFiles.Name = "chkFiles";
			this.chkFiles.Size = new System.Drawing.Size(47, 17);
			this.chkFiles.TabIndex = 0;
			this.chkFiles.Text = "&Files";
			this.chkFiles.UseVisualStyleBackColor = true;
			// 
			// grpDepth
			// 
			this.grpDepth.Controls.Add(this.nudDepth);
			this.grpDepth.Controls.Add(this.chkLimitDepth);
			this.grpDepth.Location = new System.Drawing.Point(0, 167);
			this.grpDepth.Name = "grpDepth";
			this.grpDepth.Size = new System.Drawing.Size(321, 50);
			this.grpDepth.TabIndex = 7;
			this.grpDepth.TabStop = false;
			this.grpDepth.Text = "Depth";
			// 
			// nudDepth
			// 
			this.nudDepth.Enabled = false;
			this.nudDepth.Location = new System.Drawing.Point(109, 20);
			this.nudDepth.Maximum = new decimal(new int[] {
			1000,
			0,
			0,
			0});
			this.nudDepth.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			-2147483648});
			this.nudDepth.Name = "nudDepth";
			this.nudDepth.Size = new System.Drawing.Size(74, 20);
			this.nudDepth.TabIndex = 1;
			this.nudDepth.ValueChanged += new System.EventHandler(this.nudDepth_ValueChanged);
			// 
			// chkLimitDepth
			// 
			this.chkLimitDepth.AutoSize = true;
			this.chkLimitDepth.Location = new System.Drawing.Point(11, 20);
			this.chkLimitDepth.Name = "chkLimitDepth";
			this.chkLimitDepth.Size = new System.Drawing.Size(92, 17);
			this.chkLimitDepth.TabIndex = 0;
			this.chkLimitDepth.Text = "Limit d&epth to:";
			this.chkLimitDepth.UseVisualStyleBackColor = true;
			this.chkLimitDepth.CheckedChanged += new System.EventHandler(this.chkLimitDepth_CheckedChanged);
			// 
			// SimpleSearchSpecificationControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpDepth);
			this.Controls.Add(this.grpOptions);
			this.Controls.Add(this.cbxPatterns);
			this.Controls.Add(this.lblPatterns);
			this.Controls.Add(this.btnPatternEdit);
			this.Controls.Add(this.btnDirectoryBrowse);
			this.Controls.Add(this.cbxDirectory);
			this.Controls.Add(this.lblDirectory);
			this.Name = "SimpleSearchSpecificationControl";
			this.Size = new System.Drawing.Size(320, 220);
			this.Load += new System.EventHandler(this.SimpleSearchSpecificationControl_Load);
			this.grpOptions.ResumeLayout(false);
			this.grpOptions.PerformLayout();
			this.grpDepth.ResumeLayout(false);
			this.grpDepth.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDepth)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblDirectory;
		private System.Windows.Forms.ComboBox cbxDirectory;
		private System.Windows.Forms.Button btnDirectoryBrowse;
		private System.Windows.Forms.Label lblPatterns;
		private System.Windows.Forms.ComboBox cbxPatterns;
		private System.Windows.Forms.Button btnPatternEdit;
		private System.Windows.Forms.GroupBox grpOptions;
		private System.Windows.Forms.CheckBox chkIgnoreInaccessibleDirectories;
		private System.Windows.Forms.CheckBox chkIncludeHidden;
		private System.Windows.Forms.CheckBox chkMarkDirectories;
		private System.Windows.Forms.CheckBox chkDirectories;
		private System.Windows.Forms.CheckBox chkFiles;
		private System.Windows.Forms.CheckBox chkIncludeSystem;
		private System.Windows.Forms.GroupBox grpDepth;
		private System.Windows.Forms.NumericUpDown nudDepth;
		private System.Windows.Forms.CheckBox chkLimitDepth;
	}
}
