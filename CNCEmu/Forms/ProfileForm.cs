﻿using System;
using System.IO;
using System.Windows.Forms;

namespace CNCEmu.Forms
{
    public partial class ProfileForm : Form
    {
        public ProfileForm()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            RefreshProfiles();
        }

        private void RefreshProfiles()
        {
            Profiles.Refresh();
            listBox1.Items.Clear();
            foreach (Profile p in Profiles.profiles)
                listBox1.Items.Add(p.ToString());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string name = toolStripTextBox1.Text;
            string mail = toolStripTextBox2.Text;
            Profiles.Refresh();
            Profiles.Create(name, mail);
            Profiles.Refresh();
            RefreshProfiles();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1)
                return;
            Profile p = Profiles.profiles[n];
            string path = Profiles.getProfilePath(p.id);
            if (File.Exists(path))
                File.Delete(path);
            Profiles.Refresh();
            RefreshProfiles();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            RefreshProfiles();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
