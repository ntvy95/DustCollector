namespace AgentOrientedProgramming
{
    partial class SetRandomForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NumberOfRandom = new System.Windows.Forms.TextBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NumberOfRandom
            // 
            this.NumberOfRandom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumberOfRandom.BackColor = System.Drawing.SystemColors.Window;
            this.NumberOfRandom.Location = new System.Drawing.Point(12, 12);
            this.NumberOfRandom.Name = "NumberOfRandom";
            this.NumberOfRandom.Size = new System.Drawing.Size(310, 20);
            this.NumberOfRandom.TabIndex = 0;
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.Location = new System.Drawing.Point(247, 38);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // SetRandomForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(334, 67);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.NumberOfRandom);
            this.MaximumSize = new System.Drawing.Size(350, 106);
            this.MinimumSize = new System.Drawing.Size(350, 106);
            this.Name = "SetRandomForm";
            this.Text = "Random";
            this.Activated += new System.EventHandler(this.SetRandomForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetRandomForm_FormClosing);
            this.Load += new System.EventHandler(this.SetRandomForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NumberOfRandom;
        private System.Windows.Forms.Button button_OK;
    }
}