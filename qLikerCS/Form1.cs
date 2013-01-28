using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace qLikerCS
{
    public partial class Form1 : Form
    {
        string _exeFileName = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void loadTestFunction()
        {
            process1.StartInfo.FileName = _exeFileName;
            process1.StartInfo.RedirectStandardOutput = true;
            process1.StartInfo.CreateNoWindow = true;
            process1.StartInfo.UseShellExecute = false;
            process1.StartInfo.Arguments = @" -functions";

            process1.Start();
            process1.WaitForExit();

            string stdoutput = process1.StandardOutput.ReadToEnd();
            string[] separator = {System.Environment.NewLine};
            string[] funcList = stdoutput.Split( separator, StringSplitOptions.RemoveEmptyEntries );

            foreach ( string funcName in funcList )
            {
                string item = funcName.Replace( @"()", @"" );
                listView1.Items.Add( item );
            }
        }

        private void loadToolStripMenuItem_Click( object sender, EventArgs e )
        {
            DialogResult ret = openFileDialog1.ShowDialog();
            if ( ret == DialogResult.OK )
            {
                _exeFileName = openFileDialog1.FileName;

                toolStripStatusLabel1.Text = System.IO.Path.GetFileName( _exeFileName );

                loadTestFunction();
            }
        }

        private void runBotton_Click( object sender, EventArgs e )
        {
            process1.StartInfo.Arguments = string.Empty;
            process1.Start();
            process1.WaitForExit();

            string stdoutput = process1.StandardOutput.ReadToEnd();

            richTextBox1.Text = stdoutput;
        }
    }
}
