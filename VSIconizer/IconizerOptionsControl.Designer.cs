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
            ((System.ComponentModel.ISupportInitialize)(this.tHMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tVMargin)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHMargin
            // 
            this.lblHMargin.AutoSize = true;
            this.lblHMargin.Location = new System.Drawing.Point(16, 30);
            this.lblHMargin.Name = "lblHMargin";
            this.lblHMargin.Size = new System.Drawing.Size(186, 25);
            this.lblHMargin.TabIndex = 0;
            this.lblHMargin.Text = "Horizontal margin:";
            // 
            // lblVMargin
            // 
            this.lblVMargin.AutoSize = true;
            this.lblVMargin.Location = new System.Drawing.Point(16, 83);
            this.lblVMargin.Name = "lblVMargin";
            this.lblVMargin.Size = new System.Drawing.Size(161, 25);
            this.lblVMargin.TabIndex = 1;
            this.lblVMargin.Text = "Vertical margin:";
            // 
            // tHMargin
            // 
            this.tHMargin.Location = new System.Drawing.Point(234, 28);
            this.tHMargin.Name = "tHMargin";
            this.tHMargin.Size = new System.Drawing.Size(120, 31);
            this.tHMargin.TabIndex = 1;
            this.tHMargin.ValueChanged += new System.EventHandler(this.tHMargin_ValueChanged);
            this.tHMargin.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tHMargin_KeyUp);
            // 
            // tVMargin
            // 
            this.tVMargin.Location = new System.Drawing.Point(234, 81);
            this.tVMargin.Name = "tVMargin";
            this.tVMargin.Size = new System.Drawing.Size(120, 31);
            this.tVMargin.TabIndex = 0;
            this.tVMargin.ValueChanged += new System.EventHandler(this.tVMargin_ValueChanged);
            this.tVMargin.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tVMargin_KeyUp);
            // 
            // IconizerOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tVMargin);
            this.Controls.Add(this.tHMargin);
            this.Controls.Add(this.lblVMargin);
            this.Controls.Add(this.lblHMargin);
            this.Name = "IconizerOptionsControl";
            this.Size = new System.Drawing.Size(987, 910);
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
    }
}
