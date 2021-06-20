
namespace VSIconizerOptionsTestHost
{
	partial class TestForm
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
			if (disposing && ( components != null ))
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
			this.iconizerOptionsControl1 = new VSIconizer.IconizerOptionsControl();
			this.applyList = new System.Windows.Forms.ListBox();
			this.tabColorsCsv = new System.Windows.Forms.TextBox();
			this.tabColorsCsvLbl = new System.Windows.Forms.Label();
			this.applyHistoryLbl = new System.Windows.Forms.Label();
			this.reloadBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// iconizerOptionsControl1
			// 
			this.iconizerOptionsControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.iconizerOptionsControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.iconizerOptionsControl1.Location = new System.Drawing.Point(11, 11);
			this.iconizerOptionsControl1.Margin = new System.Windows.Forms.Padding(2);
			this.iconizerOptionsControl1.Name = "iconizerOptionsControl1";
			this.iconizerOptionsControl1.Size = new System.Drawing.Size(427, 428);
			this.iconizerOptionsControl1.TabIndex = 0;
			// 
			// applyList
			// 
			this.applyList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.applyList.FormattingEnabled = true;
			this.applyList.IntegralHeight = false;
			this.applyList.Location = new System.Drawing.Point(443, 27);
			this.applyList.Name = "applyList";
			this.applyList.Size = new System.Drawing.Size(275, 411);
			this.applyList.TabIndex = 1;
			// 
			// tabColorsCsv
			// 
			this.tabColorsCsv.AcceptsReturn = true;
			this.tabColorsCsv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabColorsCsv.Location = new System.Drawing.Point(724, 27);
			this.tabColorsCsv.Multiline = true;
			this.tabColorsCsv.Name = "tabColorsCsv";
			this.tabColorsCsv.Size = new System.Drawing.Size(324, 411);
			this.tabColorsCsv.TabIndex = 2;
			// 
			// tabColorsCsvLbl
			// 
			this.tabColorsCsvLbl.AutoSize = true;
			this.tabColorsCsvLbl.Location = new System.Drawing.Point(721, 9);
			this.tabColorsCsvLbl.Name = "tabColorsCsvLbl";
			this.tabColorsCsvLbl.Size = new System.Drawing.Size(82, 13);
			this.tabColorsCsvLbl.TabIndex = 3;
			this.tabColorsCsvLbl.Text = "Tab Colors CSV";
			// 
			// applyHistoryLbl
			// 
			this.applyHistoryLbl.AutoSize = true;
			this.applyHistoryLbl.Location = new System.Drawing.Point(443, 11);
			this.applyHistoryLbl.Name = "applyHistoryLbl";
			this.applyHistoryLbl.Size = new System.Drawing.Size(76, 13);
			this.applyHistoryLbl.TabIndex = 4;
			this.applyHistoryLbl.Text = "Calls to Apply()";
			// 
			// reloadBtn
			// 
			this.reloadBtn.Location = new System.Drawing.Point(973, 1);
			this.reloadBtn.Name = "reloadBtn";
			this.reloadBtn.Size = new System.Drawing.Size(75, 23);
			this.reloadBtn.TabIndex = 5;
			this.reloadBtn.Text = "Reload";
			this.reloadBtn.UseVisualStyleBackColor = true;
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1141, 450);
			this.Controls.Add(this.reloadBtn);
			this.Controls.Add(this.applyHistoryLbl);
			this.Controls.Add(this.tabColorsCsvLbl);
			this.Controls.Add(this.tabColorsCsv);
			this.Controls.Add(this.applyList);
			this.Controls.Add(this.iconizerOptionsControl1);
			this.Name = "TestForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        #endregion

        private VSIconizer.IconizerOptionsControl iconizerOptionsControl1;
        private System.Windows.Forms.ListBox applyList;
        private System.Windows.Forms.TextBox tabColorsCsv;
        private System.Windows.Forms.Label tabColorsCsvLbl;
        private System.Windows.Forms.Label applyHistoryLbl;
        private System.Windows.Forms.Button reloadBtn;
    }
}

