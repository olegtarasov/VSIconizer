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
            this.cbShowText = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tHMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tVMargin)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHMargin
            // 
            this.lblHMargin.AutoSize = true;
            this.lblHMargin.Location = new System.Drawing.Point(8, 16);
            this.lblHMargin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHMargin.Name = "lblHMargin";
            this.lblHMargin.Size = new System.Drawing.Size(91, 13);
            this.lblHMargin.TabIndex = 0;
            this.lblHMargin.Text = "Horizontal margin:";
            // 
            // lblVMargin
            // 
            this.lblVMargin.AutoSize = true;
            this.lblVMargin.Location = new System.Drawing.Point(8, 43);
            this.lblVMargin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVMargin.Name = "lblVMargin";
            this.lblVMargin.Size = new System.Drawing.Size(79, 13);
            this.lblVMargin.TabIndex = 1;
            this.lblVMargin.Text = "Vertical margin:";
            // 
            // tHMargin
            // 
            this.tHMargin.Location = new System.Drawing.Point(117, 15);
            this.tHMargin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tHMargin.Name = "tHMargin";
            this.tHMargin.Size = new System.Drawing.Size(60, 20);
            this.tHMargin.TabIndex = 1;
            this.tHMargin.ValueChanged += new System.EventHandler(this.tHMargin_ValueChanged);
            this.tHMargin.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tHMargin_KeyUp);
            // 
            // tVMargin
            // 
            this.tVMargin.Location = new System.Drawing.Point(117, 42);
            this.tVMargin.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tVMargin.Name = "tVMargin";
            this.tVMargin.Size = new System.Drawing.Size(60, 20);
            this.tVMargin.TabIndex = 0;
            this.tVMargin.ValueChanged += new System.EventHandler(this.tVMargin_ValueChanged);
            this.tVMargin.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tVMargin_KeyUp);
            // 
            // cbShowText
            // 
            this.cbShowText.AutoSize = true;
            this.cbShowText.Location = new System.Drawing.Point(11, 70);
            this.cbShowText.Name = "cbShowText";
            this.cbShowText.Size = new System.Drawing.Size(131, 17);
            this.cbShowText.TabIndex = 2;
            this.cbShowText.Text = "Show text next to icon";
            this.cbShowText.UseVisualStyleBackColor = true;
            this.cbShowText.CheckedChanged += new System.EventHandler(this.cbShowText_CheckedChanged);
            // 
            // IconizerOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbShowText);
            this.Controls.Add(this.tVMargin);
            this.Controls.Add(this.tHMargin);
            this.Controls.Add(this.lblVMargin);
            this.Controls.Add(this.lblHMargin);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "IconizerOptionsControl";
            this.Size = new System.Drawing.Size(494, 473);
            ((System.ComponentModel.ISupportInitialize)(this.tHMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tVMargin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHMargin;
        private System.Windows.Forms.Label lblVMargin;
        private System.Windows.Forms.NumericUpDown tHMargin;
        private System.Windows.Forms.NumericUpDown tVMargin;
        private System.Windows.Forms.CheckBox cbShowText;
    }
}
