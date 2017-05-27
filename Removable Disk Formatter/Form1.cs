using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Removable_Disk_Formatter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = DriveInfo.GetDrives()
                                   .Where(x => x.DriveType == DriveType.Removable)
                                   .ToList();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string drive_letter = comboBox1.Text.Substring(0, 1);
            DriveInfo di = new DriveInfo(drive_letter);
          
            if (di.IsReady)
            {
                fs.Text = di.DriveFormat;
                long toGB = di.TotalSize/ 1073741824;
                size.Text = toGB + "GB (Approx)";
                dname.Text = di.VolumeLabel;
            }
            else
            {
                fs.Text = "--";
                size.Text = "--";
                dname.Text = "--";
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            comboBox1.DataSource = DriveInfo.GetDrives()
                                  .Where(x => x.DriveType == DriveType.Removable)
                                  .ToList();
            comboBox1.SelectedIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process dp = new Process();
            dp.StartInfo.UseShellExecute = false;
            dp.StartInfo.RedirectStandardOutput = true;
            dp.StartInfo.FileName = @"C:\Windows\System32\diskpart.exe";
            dp.StartInfo.RedirectStandardInput = true;
            dp.Start();

            dp.StandardInput.WriteLine("select disk 1"); //need immediate attention fixing this! Never use "0" though.
            progressBar1.Value = 20;
            dp.StandardInput.WriteLine("clean");
            progressBar1.Value = 40;
            dp.StandardInput.WriteLine("create partition primary");
            progressBar1.Value = 60;
            dp.StandardInput.WriteLine("select partition 1");
            dp.StandardInput.WriteLine("format fs=" + comboBox2.SelectedItem.ToString() + " quick");
            progressBar1.Value = 95;
            dp.StandardInput.WriteLine("exit");
            progressBar1.Value = 100;
            string output = dp.StandardOutput.ReadToEnd();
            dp.WaitForExit();

        }
    }
}
