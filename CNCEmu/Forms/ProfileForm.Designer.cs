namespace CNCEmu.Forms
{
    partial class ProfileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileForm));
            this.ToolbarMS = new System.Windows.Forms.ToolStrip();
            this.UsernameSL = new System.Windows.Forms.ToolStripLabel();
            this.UsernameTB = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.EmailSL = new System.Windows.Forms.ToolStripLabel();
            this.EmailTB = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AddSB = new System.Windows.Forms.ToolStripButton();
            this.RemoveSB = new System.Windows.Forms.ToolStripButton();
            this.RefreshSB = new System.Windows.Forms.ToolStripButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ToolbarMS.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolbarMS
            // 
            this.ToolbarMS.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolbarMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UsernameSL,
            this.UsernameTB,
            this.toolStripSeparator2,
            this.EmailSL,
            this.EmailTB,
            this.toolStripSeparator1,
            this.AddSB,
            this.RemoveSB,
            this.RefreshSB});
            this.ToolbarMS.Location = new System.Drawing.Point(0, 0);
            this.ToolbarMS.Name = "ToolbarMS";
            this.ToolbarMS.Size = new System.Drawing.Size(514, 25);
            this.ToolbarMS.TabIndex = 0;
            this.ToolbarMS.Text = "toolStrip1";
            // 
            // UsernameSL
            // 
            this.UsernameSL.Name = "UsernameSL";
            this.UsernameSL.Size = new System.Drawing.Size(63, 22);
            this.UsernameSL.Text = "Username:";
            // 
            // UsernameTB
            // 
            this.UsernameTB.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.UsernameTB.Name = "UsernameTB";
            this.UsernameTB.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // EmailSL
            // 
            this.EmailSL.Name = "EmailSL";
            this.EmailSL.Size = new System.Drawing.Size(39, 22);
            this.EmailSL.Text = "Email:";
            // 
            // EmailTB
            // 
            this.EmailTB.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.EmailTB.Name = "EmailTB";
            this.EmailTB.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // AddSB
            // 
            this.AddSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AddSB.Image = ((System.Drawing.Image)(resources.GetObject("AddSB.Image")));
            this.AddSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddSB.Name = "AddSB";
            this.AddSB.Size = new System.Drawing.Size(33, 22);
            this.AddSB.Text = "Add";
            this.AddSB.Click += new System.EventHandler(this.AddSB_Click);
            // 
            // RemoveSB
            // 
            this.RemoveSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RemoveSB.Image = ((System.Drawing.Image)(resources.GetObject("RemoveSB.Image")));
            this.RemoveSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveSB.Name = "RemoveSB";
            this.RemoveSB.Size = new System.Drawing.Size(54, 22);
            this.RemoveSB.Text = "Remove";
            this.RemoveSB.Click += new System.EventHandler(this.RemoveSB_Click);
            // 
            // RefreshSB
            // 
            this.RefreshSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.RefreshSB.Image = ((System.Drawing.Image)(resources.GetObject("RefreshSB.Image")));
            this.RefreshSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RefreshSB.Name = "RefreshSB";
            this.RefreshSB.Size = new System.Drawing.Size(50, 22);
            this.RefreshSB.Text = "Refresh";
            this.RefreshSB.Click += new System.EventHandler(this.RefreshSB_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 25);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(514, 355);
            this.listBox1.TabIndex = 1;
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 380);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.ToolbarMS);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProfileForm";
            this.Text = "Player Profile Tool";
            this.Load += new System.EventHandler(this.ProfileForm_Load);
            this.ToolbarMS.ResumeLayout(false);
            this.ToolbarMS.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolbarMS;
        private System.Windows.Forms.ToolStripTextBox UsernameTB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton AddSB;
        private System.Windows.Forms.ToolStripButton RemoveSB;
        private System.Windows.Forms.ToolStripButton RefreshSB;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripLabel EmailSL;
        private System.Windows.Forms.ToolStripLabel UsernameSL;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox EmailTB;
    }
}