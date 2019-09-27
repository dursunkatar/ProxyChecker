using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace ProxyChecker
{
    public partial class FrmMain : Form
    {
        private int index = 0;
        private static volatile object obj = new object();

        public FrmMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            CheckerEngine.Finish = threadFinishEvent;
        }

        private void threadFinishEvent()
        {
            lock (obj)
            {
                if (index < listView.Items.Count)
                {
                    var lvi = listView.Items[index++];
                    var th = new Thread(() =>
                    {
                        CheckerEngine.Start(lvi);
                    });
                    th.IsBackground = true;
                    th.Start();
                }
            }
        }
        private void AddLviItem(string proxy)
        {
            try
            {
                var sp = proxy.Split(':');
                var lvi = new ListViewItem();
                lvi.Text = sp[0];
                lvi.SubItems.Add(sp[1]);
                lvi.SubItems.Add("");
                listView.Items.Add(lvi);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddLviItem(txtProxy.Text);
        }

        private void menuProxyListLoad_Click(object sender, EventArgs e)
        {
            var file = new OpenFileDialog();
            file.Multiselect = false;
            file.Filter = "Txt Dosyası |*.txt";
            file.Title = "Select txt";

            if (file.ShowDialog() == DialogResult.OK)
            {
                string[] rows = File.ReadAllLines(file.FileName, Encoding.Default);
                foreach (string row in rows)
                {
                    if (!string.IsNullOrWhiteSpace(row))
                    {
                        AddLviItem(row);
                    }
                }
            }
            file.Dispose();
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
        }

        private void clear()
        {
            foreach (ListViewItem lvi in listView.Items)
            {
                lvi.ForeColor = Color.Black;
                lvi.SubItems[2].Text = "";
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            clear();
            int finishIndex = listView.Items.Count < 5 ? listView.Items.Count : 5;
            for (index = 0; index < finishIndex; index++)
            {
                var lvi = listView.Items[index];
                var th = new Thread(() =>
                {
                    CheckerEngine.Start(lvi);
                });
                th.IsBackground = true;
                th.Start();
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            var list = new List<string>();
            foreach (ListViewItem lvi in listView.Items)
            {
                if (lvi.ForeColor == Color.Green)
                {
                    list.Add(lvi.Text + ":" + lvi.SubItems[1].Text);
                }
            }
            File.WriteAllLines("result.txt", list, Encoding.Default);
            MessageBox.Show("Kaydedildi", "Ben");
        }
    }
}
