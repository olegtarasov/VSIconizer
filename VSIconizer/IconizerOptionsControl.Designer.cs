namespace VSIconizer
{
    partial class IconizerOptionsControl
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
            if (disposing && (components != null))
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
			this.lblHMargin = new System.Windows.Forms.Label();
			this.lblVMargin = new System.Windows.Forms.Label();
			this.tHMargin = new System.Windows.Forms.NumericUpDown();
			this.tVMargin = new System.Windows.Forms.NumericUpDown();
			this.modeLbl = new System.Windows.Forms.Label();
			this.layout = new System.Windows.Forms.TableLayoutPanel();
			this.iconTextSpacing = new System.Windows.Forms.NumericUpDown();
			this.modeCmb = new System.Windows.Forms.ComboBox();
			this.iconTextSpacingLbl = new System.Windows.Forms.Label();
			this.rotateChk = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.tHMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tVMargin)).BeginInit();
			this.layout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.iconTextSpacing)).BeginInit();
			this.SuspendLayout();
			// 
			// lblHMargin
			// 
			this.lblHMargin.AutoSize = true;
			this.lblHMargin.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHMargin.Location = new System.Drawing.Point(2, 29);
			this.lblHMargin.Margin = new System.Windows.Forms.Padding(2);
			this.lblHMargin.Name = "lblHMargin";
			this.lblHMargin.Size = new System.Drawing.Size(100, 23);
			this.lblHMargin.TabIndex = 0;
			this.lblHMargin.Text = "Horizontal margin";
			// 
			// lblVMargin
			// 
			this.lblVMargin.AutoSize = true;
			this.lblVMargin.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblVMargin.Location = new System.Drawing.Point(3, 57);
			this.lblVMargin.Margin = new System.Windows.Forms.Padding(3);
			this.lblVMargin.Name = "lblVMargin";
			this.lblVMargin.Size = new System.Drawing.Size(98, 21);
			this.lblVMargin.TabIndex = 1;
			this.lblVMargin.Text = "Vertical margin";
			// 
			// tHMargin
			// 
			this.tHMargin.Location = new System.Drawing.Point(104, 27);
			this.tHMargin.Margin = new System.Windows.Forms.Padding(0);
			this.tHMargin.Name = "tHMargin";
			this.tHMargin.Size = new System.Drawing.Size(60, 20);
			this.tHMargin.TabIndex = 1;
			this.tHMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// tVMargin
			// 
			this.tVMargin.Location = new System.Drawing.Point(104, 54);
			this.tVMargin.Margin = new System.Windows.Forms.Padding(0);
			this.tVMargin.Name = "tVMargin";
			this.tVMargin.Size = new System.Drawing.Size(60, 20);
			this.tVMargin.TabIndex = 0;
			this.tVMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// modeLbl
			// 
			this.modeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modeLbl.Location = new System.Drawing.Point(2, 2);
			this.modeLbl.Margin = new System.Windows.Forms.Padding(2);
			this.modeLbl.Name = "modeLbl";
			this.modeLbl.Size = new System.Drawing.Size(100, 23);
			this.modeLbl.TabIndex = 3;
			this.modeLbl.Text = "Appearance";
			// 
			// layout
			// 
			this.layout.ColumnCount = 2;
			this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.layout.Controls.Add(this.iconTextSpacing, 1, 3);
			this.layout.Controls.Add(this.modeLbl, 0, 0);
			this.layout.Controls.Add(this.modeCmb, 1, 0);
			this.layout.Controls.Add(this.tVMargin, 1, 2);
			this.layout.Controls.Add(this.lblHMargin, 0, 1);
			this.layout.Controls.Add(this.lblVMargin, 0, 2);
			this.layout.Controls.Add(this.tHMargin, 1, 1);
			this.layout.Controls.Add(this.iconTextSpacingLbl, 0, 3);
			this.layout.Controls.Add(this.rotateChk, 1, 4);
			this.layout.Location = new System.Drawing.Point(0, 0);
			this.layout.Name = "layout";
			this.layout.RowCount = 5;
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.Size = new System.Drawing.Size(403, 174);
			this.layout.TabIndex = 4;
			// 
			// iconTextSpacing
			// 
			this.iconTextSpacing.Location = new System.Drawing.Point(104, 81);
			this.iconTextSpacing.Margin = new System.Windows.Forms.Padding(0);
			this.iconTextSpacing.Name = "iconTextSpacing";
			this.iconTextSpacing.Size = new System.Drawing.Size(60, 20);
			this.iconTextSpacing.TabIndex = 6;
			this.iconTextSpacing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// modeCmb
			// 
			this.modeCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.modeCmb.FormattingEnabled = true;
			this.modeCmb.Location = new System.Drawing.Point(104, 0);
			this.modeCmb.Margin = new System.Windows.Forms.Padding(0);
			this.modeCmb.Name = "modeCmb";
			this.modeCmb.Size = new System.Drawing.Size(121, 21);
			this.modeCmb.TabIndex = 4;
			// 
			// iconTextSpacingLbl
			// 
			this.iconTextSpacingLbl.AutoSize = true;
			this.iconTextSpacingLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.iconTextSpacingLbl.Location = new System.Drawing.Point(3, 84);
			this.iconTextSpacingLbl.Margin = new System.Windows.Forms.Padding(3);
			this.iconTextSpacingLbl.Name = "iconTextSpacingLbl";
			this.iconTextSpacingLbl.Size = new System.Drawing.Size(98, 21);
			this.iconTextSpacingLbl.TabIndex = 5;
			this.iconTextSpacingLbl.Text = "Icon-Text spacing";
			// 
			// rotateChk
			// 
			this.rotateChk.AutoSize = true;
			this.rotateChk.Location = new System.Drawing.Point(107, 111);
			this.rotateChk.Name = "rotateChk";
			this.rotateChk.Size = new System.Drawing.Size(141, 17);
			this.rotateChk.TabIndex = 7;
			this.rotateChk.Text = "Rotate vertical tab icons";
			this.rotateChk.UseVisualStyleBackColor = true;
			// 
			// IconizerOptionsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.layout);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "IconizerOptionsControl";
			this.Size = new System.Drawing.Size(427, 208);
			((System.ComponentModel.ISupportInitialize)(this.tHMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tVMargin)).EndInit();
			this.layout.ResumeLayout(false);
			this.layout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.iconTextSpacing)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHMargin;
        private System.Windows.Forms.Label lblVMargin;
        private System.Windows.Forms.NumericUpDown tHMargin;
        private System.Windows.Forms.NumericUpDown tVMargin;
        private System.Windows.Forms.Label modeLbl;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.ComboBox modeCmb;
        private System.Windows.Forms.NumericUpDown iconTextSpacing;
        private System.Windows.Forms.Label iconTextSpacingLbl;
        private System.Windows.Forms.CheckBox rotateChk;
    }
}
