using CNCEmu.Services;
using CNCEmu.Services.Network;
using System;
using System.Windows.Forms;

namespace CNCEmu.Forms
{
    public partial class ProfileForm : Form
    {
        public ProfileForm() =>
            InitializeComponent();

        private void ProfileForm_Load(object sender, EventArgs e) =>
            RefreshProfiles();

        private void RefreshSB_Click(object sender, EventArgs e) =>
            RefreshProfiles();

        private void AddSB_Click(object sender, EventArgs e)
        {
            try
            {
                ProfileService.Instance.Add(UsernameTB.Text, EmailTB.Text);
            }
            catch (Exception ex)
            {
                BlazeServer.LogError("ProfileForm - AddSB_Click", ex);
            }
            RefreshProfiles();
        }

        private void RemoveSB_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == -1)
                return;

            try
            {
                ProfileService.Instance.RemoveAt(index);
            }
            catch (Exception ex)
            {
                BlazeServer.LogError("ProfileForm - RemoveSB_Click", ex);
            }
            RefreshProfiles();
        }

        private void RefreshProfiles()
        {
            listBox1.Items.Clear();
            var profiles = ProfileService.Instance.GetAll();
            profiles.ForEach(p =>
                listBox1.Items.Add(p.ToString()));
            BlazeServer.Log($"[MAIN] Loaded {profiles.Count} player profiles");
        }
    }
}
