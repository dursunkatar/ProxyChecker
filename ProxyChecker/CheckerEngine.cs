using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProxyChecker
{
    public class CheckerEngine
    {
        public delegate void FinishEvent();
        public static FinishEvent Finish { get; set; }

        public static void Start(ListViewItem lvi)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            lvi.SubItems[2].Text = "Test ediliyor...";
            var check = Checker.ProxyCheck(lvi.Text, int.Parse(lvi.SubItems[1].Text));
            stopwatch.Stop();
            if (check)
            {
                lvi.SubItems[2].Text = String.Format("{0:0.##}", (stopwatch.Elapsed.TotalMilliseconds / 1000.0)) + " sn";
                lvi.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lvi.ForeColor = System.Drawing.Color.Red;
                lvi.SubItems[2].Text = "Hatalı!";

            }
            Finish();
        }
    }
}
