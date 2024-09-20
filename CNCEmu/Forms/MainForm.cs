using BlazeLibWV;
using BlazeLibWV.Models;
using BlazeLibWV.Struct;
using CNCEmu.Extensions;
using CNCEmu.Services.Network;
using CNCEmu.Utils.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CNCEmu.Forms
{
    public partial class MainForm : Form
    {
        public readonly object _sync = new object();
        public List<Packet> packets = new List<Packet>();
        public int packetCount = 0;
        public List<Tdf> inlist;
        public int inlistcount;
        public int clientcount;

        public MainForm() =>
            InitializeComponent();

        private void MainForm_Load(object sender, EventArgs e)
        {
            BackendLog.Clear();
            CleanPackets.Clean();
            Logger.box = LogRTB;
            //BlazeServer.box = LogRTB;
            RedirectorServer.box = LogRTB;

            OnlyHighMenuItem.Checked =
            HighAndMediumMenuItem.Checked =
            AllMenuItem.Checked = false;

            Config.Credits();
            Config.Init();
            Config.InitialConfig();

            switch (Logger.LogLevel)
            {
                case LogPriority.high:
                    OnlyHighMenuItem.Checked = true;
                    break;
                case LogPriority.medium:
                    HighAndMediumMenuItem.Checked = true;
                    break;
                case LogPriority.low:
                    AllMenuItem.Checked = true;
                    break;
            }

            VersionLbl.Text = Assembly
                .GetExecutingAssembly()
                .GetCustomAttribute<AssemblyFileVersionAttribute>()
                .Version;
        }

        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            if (RedirectorServer._exit) RedirectorServer.Stop();
            BlazeServer.Instance.Stop();
            Application.Exit();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e) => 
            Close();

        private void OnlyHighMenuItem_Click(object sender, EventArgs e)
        {
            AllMenuItem.Checked =
            HighAndMediumMenuItem.Checked = false;
            OnlyHighMenuItem.Checked = true;
            Logger.LogLevel = LogPriority.high;
        }

        private void HighAndMediumMenuItem_Click(object sender, EventArgs e)
        {
            OnlyHighMenuItem.Checked =
            AllMenuItem.Checked = false;
            HighAndMediumMenuItem.Checked = true;
            Logger.LogLevel = LogPriority.medium;
        }

        private void AllMenuItem_Click(object sender, EventArgs e)
        {
            OnlyHighMenuItem.Checked =
            HighAndMediumMenuItem.Checked = false;
            AllMenuItem.Checked = true;
            Logger.LogLevel = LogPriority.low;
        }

        private void GenerateProviderIDMenuItem_Click(object sender, EventArgs e)
        {
            var d = new SaveFileDialog
            {
                Filter = "ProviderID.dat|ProviderID.dat"
            };
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Random rnd = new Random();
                uint[] values = new uint[5];
                long sum = 0;
                for (int i = 0; i < 4; i++)
                    sum += (values[i] = (uint)rnd.Next());
                sum = (uint)(sum & 0xFFFFFFFF);
                if (sum < 0xC18E65EF)
                    values[4] = (uint)(0xC18E65EF - sum);
                else
                    values[4] = (uint)(0x1C18E65EF - sum);
                MemoryStream m = new MemoryStream();
                for (int i = 0; i < 5; i++)
                    m.Write(BitConverter.GetBytes(values[i]), 0, 4);
                File.WriteAllBytes(d.FileName, m.ToArray());

                MessageBox.Show("Done.");
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            SearchInLog();
        }

        private void SearchTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LogRTB.Focus();
                SearchInLog();
            }
        }

        private void RefreshSB_Click(object sender, EventArgs e)
        {
            RefreshPackets();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1 || n >= packets.Count)
                return;

            Packet p;

            lock (_sync)
            {
                if (packets == null || n >= packets.Count)
                    return;

                p = packets[n];
            }

            tv1.Nodes.Clear();
            inlistcount = 0;
            inlist = new List<Tdf>();

            if (p != null)
            {
                List<Tdf> Fields = Blaze.ReadPacketContent(p);
                foreach (Tdf tdf in Fields)
                {
                    tv1.Nodes.Add(TdfToTree(tdf));
                }
            }
        }

        private void LoadFolderSB_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = Directory.GetFiles(d.SelectedPath, "*.bin");
                foreach (string file in files)
                    File.Copy(file, "logs\\packets\\" + Path.GetFileName(file), true);
                RefreshPackets();
            }
        }

        private void LabelByteSB_Click(object sender, EventArgs e)
        {
            try
            {
                string text = toolStripTextBox2.Text;
                byte[] buff = text.LabelToBytes();
                StringBuilder sb = new StringBuilder();
                foreach (byte b in buff)
                    sb.Append(b.ToString("X2"));
                toolStripTextBox3.Text = sb.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void ByteLabelSB_Click(object sender, EventArgs e)
        {
            try
            {
                string text = toolStripTextBox3.Text.Replace(" ", "").ToUpper();
                MemoryStream m = new MemoryStream();
                for (int i = 0; i < text.Length / 2; i++)
                    m.WriteByte(Convert.ToByte(text.Substring(i * 2, 2), 16));
                toolStripTextBox2.Text = m.ToArray().BytesToLabel();
            }
            catch (Exception)
            {
            }
        }

        private void CompressIntSB_Click(object sender, EventArgs e)
        {
            try
            {
                string text = toolStripTextBox4.Text.Replace(" ", "").ToUpper();
                long l = Convert.ToInt64(text, 16);
                var m = new MemoryStream();
                m.WriteCompressedInteger(l);
                toolStripTextBox5.Text = m.ToArray().ByteArrayToHexString();
            }
            catch (Exception)
            {
            }
        }

        private void DecompressIntSB_Click(object sender, EventArgs e)
        {
            try
            {
                string text = toolStripTextBox5.Text.Replace(" ", "").ToUpper();
                MemoryStream m = new MemoryStream(text.HexStringToByteArray());
                m.Seek(0, 0);
                toolStripTextBox4.Text = m.ReadCompressedInteger().ToString("X");
            }
            catch (Exception)
            {
            }
        }

        private void PlayerProfileMenuItem_Click(object sender, EventArgs e)
        {
            ProfileForm f = new ProfileForm();
            f.ShowDialog();
        }

        private void EditConfigMenuItem_Click(object sender, EventArgs e)
        {
            Helper.RunShell("notepad.exe", Config.ConfigFile);
        }

        private void Tv1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode t = e.Node;
            if (t != null && t.Name != "")
            {
                int n = Convert.ToInt32(t.Name);
                Tdf tdf = inlist[n];
                string s;
                switch (tdf.Type)
                {
                    case 0:
                        TdfInteger ti = (TdfInteger)tdf;
                        rtb2.Text = "0x" + ti.Value.ToString("X");
                        if (ti.Label == "IP  ")
                        {
                            rtb2.Text += Environment.NewLine + "(" + Blaze.GetStringFromIP(ti.Value) + ")";
                        }
                        break;
                    case 1:
                        rtb2.Text = ((TdfString)tdf).Value.ToString();
                        break;
                    case 2:
                        rtb2.Text = "Length: " + ((TdfBlob)tdf).Data.Length.ToString();
                        rtb2.Text += Environment.NewLine + Blaze.HexDump(((TdfBlob)tdf).Data);
                        break;
                    case 4:
                        TdfList l = (TdfList)tdf;
                        s = "";
                        for (int i = 0; i < l.Count; i++)
                        {
                            switch (l.SubType)
                            {
                                case 0:
                                    s += "{" + ((List<long>)l.List)[i] + "} ";
                                    break;
                                case 1:
                                    s += "{" + ((List<string>)l.List)[i] + "} ";
                                    break;
                                case 9:
                                    TrippleVal t2 = ((List<TrippleVal>)l.List)[i];
                                    s += "{" + t2.v1.ToString("X") + "; " + t2.v2.ToString("X") + "; " + t2.v3.ToString("X") + "} ";
                                    break;
                            }
                        }
                        rtb2.Text = s;
                        break;
                    case 5:
                        s = "";
                        TdfDoubleList ll = (TdfDoubleList)tdf;
                        for (int i = 0; i < ll.Count; i++)
                        {
                            s += "{";
                            switch (ll.SubType1)
                            {
                                case 0:
                                    List<long> l1 = (List<long>)ll.List1;
                                    s += l1[i].ToString("X");
                                    break;
                                case 1:
                                    List<string> l2 = (List<string>)ll.List1;
                                    s += l2[i];
                                    break;
                                case 0xA:
                                    List<float> lf1 = (List<float>)ll.List1;
                                    s += lf1[i].ToString();
                                    break;
                                default:
                                    s += "(see List1[" + i + "])";
                                    break;
                            }
                            s += " ; ";
                            switch (ll.SubType2)
                            {
                                case 0:
                                    List<long> l1 = (List<long>)ll.List2;
                                    s += l1[i].ToString("X");
                                    break;
                                case 1:
                                    List<string> l2 = (List<string>)ll.List2;
                                    s += l2[i];
                                    break;
                                case 0xA:
                                    List<float> lf1 = (List<float>)ll.List2;
                                    s += lf1[i].ToString();
                                    break;
                                default:
                                    s += "(see List2[" + i + "])";
                                    break;
                            }
                            s += "}\n";
                        }
                        rtb2.Text = s;
                        break;
                    case 6:
                        rtb2.Text = "Type: 0x" + ((TdfUnion)tdf).UnionType.ToString("X2");
                        break;
                    case 7:
                        TdfIntegerList til = (TdfIntegerList)tdf;
                        s = "";
                        for (int i = 0; i < til.Count; i++)
                        {
                            s += til.List[i].ToString("X");
                            if (i < til.Count - 1)
                                s += "; ";
                        }
                        rtb2.Text = s;
                        break;
                    case 8:
                        TdfDoubleVal dval = (TdfDoubleVal)tdf;
                        rtb2.Text = "0x" + dval.Value.v1.ToString("X") + " 0x" + dval.Value.v2.ToString("X");
                        break;
                    case 9:
                        TdfTrippleVal tval = (TdfTrippleVal)tdf;
                        rtb2.Text = "0x" + tval.Value.v1.ToString("X") + " 0x" + tval.Value.v2.ToString("X") + " 0x" + tval.Value.v3.ToString("X");
                        break;
                    default:
                        rtb2.Text = "";
                        break;
                }
            }
        }

        private void StartBlazeServerBtn_Click(object sender, EventArgs e)
        {
            BlazeServer.Instance.Start();
        }

        private void StartRedirectorServerBtn_Click(object sender, EventArgs e)
        {
            if (!RedirectorServer._exit) RedirectorServer.Start();
        }

        private void HelpMenuItem_Click(object sender, EventArgs e) =>
            Helper.OpenUrl("https://github.com/Xevrac/cnc_backend/wiki");

        private void AboutMenuItem_Click(object sender, EventArgs e) =>
            Helper.OpenUrl("https://github.com/Xevrac/cnc_backend/wiki/About");

        private void CacheSB_Click(object sender, EventArgs e) =>
            LogService.ClearPackets();

        private void SearchInLog()
        {
            int pos = LogRTB.SelectionStart + 1;
            if (pos < 0 || pos >= LogRTB.Text.Length)
                pos = 0;
            int next = LogRTB.Text.IndexOf(SearchTB.Text, pos);
            if (next != -1)
            {
                LogRTB.SelectionStart = next;
                LogRTB.SelectionLength = SearchTB.Text.Length;
                LogRTB.ScrollToCaret();
            }
        }

        public void RefreshPackets()
        {
            listBox1.Items.Clear();
            string[] files = Directory.GetFiles("logs\\packets\\", "*.bin");
            foreach (string file in files)
            {
                listBox1.Items.Add(Path.GetFileName(file));
                AddPacket(FileToByteArray(file));
            }
        }

        public byte[] FileToByteArray(string fileName)
        {
            byte[] fileData = null;

            using (FileStream fs = File.OpenRead(fileName))
            {
                using (BinaryReader binaryReader = new BinaryReader(fs))
                {
                    fileData = binaryReader.ReadBytes((int)fs.Length);
                }
            }
            return fileData;
        }

        public void AddPacket(byte[] data)
        {
            MemoryStream m = new MemoryStream(data);
            m.Seek(0, 0);
            List<Packet> result = Blaze.FetchAllBlazePackets(m);
            lock (_sync)
            {
                packets.AddRange(result);
            }
        }

        private TreeNode TdfToTree(Tdf tdf)
        {
            TreeNode t, t2, t3;
            switch (tdf.Type)
            {
                case 3:
                    t = tdf.ToTree();
                    TdfStruct str = (TdfStruct)tdf;
                    if (str.startswith2)
                        t.Text += " (Starts with 2)";
                    foreach (Tdf td in str.Values)
                        t.Nodes.Add(TdfToTree(td));
                    t.Name = (inlistcount++).ToString();
                    inlist.Add(tdf);
                    return t;
                case 4:
                    t = tdf.ToTree();
                    TdfList l = (TdfList)tdf;
                    if (l.SubType == 3)
                    {
                        List<TdfStruct> l2 = (List<TdfStruct>)l.List;
                        for (int i = 0; i < l2.Count; i++)
                        {
                            t2 = new TreeNode("Entry #" + i);
                            if (l2[i].startswith2)
                                t2.Text += " (Starts with 2)";
                            List<Tdf> l3 = l2[i].Values;
                            for (int j = 0; j < l3.Count; j++)
                                t2.Nodes.Add(TdfToTree(l3[j]));
                            t.Nodes.Add(t2);
                        }
                    }
                    t.Name = (inlistcount++).ToString();
                    inlist.Add(tdf);
                    return t;
                case 5:
                    t = tdf.ToTree();
                    TdfDoubleList ll = (TdfDoubleList)tdf;
                    t2 = new TreeNode("List 1");
                    if (ll.SubType1 == 3)
                    {
                        List<TdfStruct> l2 = (List<TdfStruct>)ll.List1;
                        for (int i = 0; i < l2.Count; i++)
                        {
                            t3 = new TreeNode("Entry #" + i);
                            if (l2[i].startswith2)
                                t2.Text += " (Starts with 2)";
                            List<Tdf> l3 = l2[i].Values;
                            for (int j = 0; j < l3.Count; j++)
                                t3.Nodes.Add(TdfToTree(l3[j]));
                            t2.Nodes.Add(t3);
                        }
                        t.Nodes.Add(t2);
                    }
                    t2 = new TreeNode("List 2");
                    if (ll.SubType2 == 3)
                    {
                        List<TdfStruct> l2 = (List<TdfStruct>)ll.List2;
                        for (int i = 0; i < l2.Count; i++)
                        {
                            t3 = new TreeNode("Entry #" + i);
                            if (l2[i].startswith2)
                                t2.Text += " (Starts with 2)";
                            List<Tdf> l3 = l2[i].Values;
                            for (int j = 0; j < l3.Count; j++)
                                t3.Nodes.Add(TdfToTree(l3[j]));
                            t2.Nodes.Add(t3);
                        }
                        t.Nodes.Add(t2);
                    }
                    t.Name = (inlistcount++).ToString();
                    inlist.Add(tdf);
                    return t;
                case 6:
                    t = tdf.ToTree();
                    TdfUnion tu = (TdfUnion)tdf;
                    if (tu.UnionType != 0x7F)
                    {
                        t.Nodes.Add(TdfToTree(tu.UnionContent));
                    }
                    t.Name = (inlistcount++).ToString();
                    inlist.Add(tdf);
                    return t;
                default:
                    t = tdf.ToTree();
                    t.Name = (inlistcount++).ToString();
                    inlist.Add(tdf);
                    return t;
            }
        }
    }
}
