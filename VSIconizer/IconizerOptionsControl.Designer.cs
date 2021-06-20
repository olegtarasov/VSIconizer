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
			this.tabColorsEditor = new System.Windows.Forms.DataGridView();
			this.colTabText = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colColorValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colColorBtn = new System.Windows.Forms.DataGridViewButtonColumn();
			this.iconTextSpacing = new System.Windows.Forms.NumericUpDown();
			this.modeCmb = new System.Windows.Forms.ComboBox();
			this.iconTextSpacingLbl = new System.Windows.Forms.Label();
			this.rotateChk = new System.Windows.Forms.CheckBox();
			this.useTabColorsChk = new System.Windows.Forms.CheckBox();
			this.tabColorEditorLbl = new System.Windows.Forms.Label();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			((System.ComponentModel.ISupportInitialize)(this.tHMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tVMargin)).BeginInit();
			this.layout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabColorsEditor)).BeginInit();
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
			this.lblHMargin.TabIndex = 2;
			this.lblHMargin.Text = "Horizontal margin";
			// 
			// lblVMargin
			// 
			this.lblVMargin.AutoSize = true;
			this.lblVMargin.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblVMargin.Location = new System.Drawing.Point(2, 56);
			this.lblVMargin.Margin = new System.Windows.Forms.Padding(2);
			this.lblVMargin.Name = "lblVMargin";
			this.lblVMargin.Size = new System.Drawing.Size(100, 23);
			this.lblVMargin.TabIndex = 4;
			this.lblVMargin.Text = "Vertical margin";
			// 
			// tHMargin
			// 
			this.tHMargin.Location = new System.Drawing.Point(104, 27);
			this.tHMargin.Margin = new System.Windows.Forms.Padding(0);
			this.tHMargin.Name = "tHMargin";
			this.tHMargin.Size = new System.Drawing.Size(60, 20);
			this.tHMargin.TabIndex = 3;
			this.tHMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// tVMargin
			// 
			this.tVMargin.Location = new System.Drawing.Point(104, 54);
			this.tVMargin.Margin = new System.Windows.Forms.Padding(0);
			this.tVMargin.Name = "tVMargin";
			this.tVMargin.Size = new System.Drawing.Size(60, 20);
			this.tVMargin.TabIndex = 5;
			this.tVMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// modeLbl
			// 
			this.modeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modeLbl.Location = new System.Drawing.Point(2, 4);
			this.modeLbl.Margin = new System.Windows.Forms.Padding(2, 4, 2, 2);
			this.modeLbl.Name = "modeLbl";
			this.modeLbl.Size = new System.Drawing.Size(100, 21);
			this.modeLbl.TabIndex = 0;
			this.modeLbl.Text = "Appearance";
			// 
			// layout
			// 
			this.layout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.layout.ColumnCount = 2;
			this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.layout.Controls.Add(this.tabColorsEditor, 1, 6);
			this.layout.Controls.Add(this.iconTextSpacing, 1, 3);
			this.layout.Controls.Add(this.modeLbl, 0, 0);
			this.layout.Controls.Add(this.modeCmb, 1, 0);
			this.layout.Controls.Add(this.tVMargin, 1, 2);
			this.layout.Controls.Add(this.lblHMargin, 0, 1);
			this.layout.Controls.Add(this.lblVMargin, 0, 2);
			this.layout.Controls.Add(this.tHMargin, 1, 1);
			this.layout.Controls.Add(this.iconTextSpacingLbl, 0, 3);
			this.layout.Controls.Add(this.rotateChk, 1, 4);
			this.layout.Controls.Add(this.useTabColorsChk, 1, 5);
			this.layout.Controls.Add(this.tabColorEditorLbl, 0, 6);
			this.layout.Location = new System.Drawing.Point(0, 0);
			this.layout.Name = "layout";
			this.layout.RowCount = 7;
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.layout.Size = new System.Drawing.Size(427, 541);
			this.layout.TabIndex = 4;
			// 
			// tabColorsEditor
			// 
			this.tabColorsEditor.AllowUserToResizeRows = false;
			this.tabColorsEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabColorsEditor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.tabColorsEditor.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.tabColorsEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tabColorsEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.tabColorsEditor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTabText,
            this.colColorValue,
            this.colColorBtn});
			this.tabColorsEditor.EnableHeadersVisualStyles = false;
			this.tabColorsEditor.Location = new System.Drawing.Point(107, 165);
			this.tabColorsEditor.MultiSelect = false;
			this.tabColorsEditor.Name = "tabColorsEditor";
			this.tabColorsEditor.RowHeadersWidth = 25;
			this.tabColorsEditor.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.tabColorsEditor.Size = new System.Drawing.Size(296, 373);
			this.tabColorsEditor.TabIndex = 10;
			// 
			// colTabText
			// 
			this.colTabText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colTabText.HeaderText = "Tool tab text";
			this.colTabText.MinimumWidth = 75;
			this.colTabText.Name = "colTabText";
			this.colTabText.Width = 91;
			// 
			// colColorValue
			// 
			this.colColorValue.HeaderText = "Color";
			this.colColorValue.Name = "colColorValue";
			this.colColorValue.Width = 56;
			// 
			// colColorBtn
			// 
			this.colColorBtn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colColorBtn.HeaderText = "Picker";
			this.colColorBtn.MinimumWidth = 75;
			this.colColorBtn.Name = "colColorBtn";
			this.colColorBtn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colColorBtn.UseColumnTextForButtonValue = true;
			this.colColorBtn.Width = 75;
			// 
			// iconTextSpacing
			// 
			this.iconTextSpacing.Location = new System.Drawing.Point(104, 81);
			this.iconTextSpacing.Margin = new System.Windows.Forms.Padding(0);
			this.iconTextSpacing.Name = "iconTextSpacing";
			this.iconTextSpacing.Size = new System.Drawing.Size(60, 20);
			this.iconTextSpacing.TabIndex = 7;
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
			this.modeCmb.TabIndex = 1;
			// 
			// iconTextSpacingLbl
			// 
			this.iconTextSpacingLbl.AutoSize = true;
			this.iconTextSpacingLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.iconTextSpacingLbl.Location = new System.Drawing.Point(2, 83);
			this.iconTextSpacingLbl.Margin = new System.Windows.Forms.Padding(2);
			this.iconTextSpacingLbl.Name = "iconTextSpacingLbl";
			this.iconTextSpacingLbl.Size = new System.Drawing.Size(100, 23);
			this.iconTextSpacingLbl.TabIndex = 6;
			this.iconTextSpacingLbl.Text = "Icon-Text spacing";
			// 
			// rotateChk
			// 
			this.rotateChk.AutoSize = true;
			this.rotateChk.Location = new System.Drawing.Point(107, 111);
			this.rotateChk.Name = "rotateChk";
			this.rotateChk.Size = new System.Drawing.Size(141, 17);
			this.rotateChk.TabIndex = 8;
			this.rotateChk.Text = "Rotate vertical tab icons";
			this.rotateChk.UseVisualStyleBackColor = true;
			// 
			// useTabColorsChk
			// 
			this.useTabColorsChk.AutoSize = true;
			this.useTabColorsChk.Location = new System.Drawing.Point(107, 138);
			this.useTabColorsChk.Name = "useTabColorsChk";
			this.useTabColorsChk.Size = new System.Drawing.Size(183, 17);
			this.useTabColorsChk.TabIndex = 9;
			this.useTabColorsChk.Text = "Customize tab background colors";
			this.useTabColorsChk.UseVisualStyleBackColor = true;
			// 
			// tabColorEditorLbl
			// 
			this.tabColorEditorLbl.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabColorEditorLbl.Location = new System.Drawing.Point(2, 164);
			this.tabColorEditorLbl.Margin = new System.Windows.Forms.Padding(2);
			this.tabColorEditorLbl.Name = "tabColorEditorLbl";
			this.tabColorEditorLbl.Size = new System.Drawing.Size(100, 23);
			this.tabColorEditorLbl.TabIndex = 11;
			this.tabColorEditorLbl.Text = "Tool tab colors";
			// 
			// IconizerOptionsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.layout);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "IconizerOptionsControl";
			this.Size = new System.Drawing.Size(427, 541);
			((System.ComponentModel.ISupportInitialize)(this.tHMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tVMargin)).EndInit();
			this.layout.ResumeLayout(false);
			this.layout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabColorsEditor)).EndInit();
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
        private System.Windows.Forms.CheckBox useTabColorsChk;
        private System.Windows.Forms.DataGridView tabColorsEditor;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTabText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColorValue;
        private System.Windows.Forms.DataGridViewButtonColumn colColorBtn;
        private System.Windows.Forms.Label tabColorEditorLbl;
    }
}
