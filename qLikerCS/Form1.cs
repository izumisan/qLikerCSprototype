using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace qLikerCS
{
    public partial class Form1 : Form
    {
        [DllImport( "kernel32.dll" )]
        extern static bool SetEnvironmentVariable( string name, string val );

        string _exeFileName = null;

        public Form1()
        {
            InitializeComponent();

            setupProcessEnvironment();
        }

        private void setupProcessEnvironment()
        {
            string path = System.Environment.GetEnvironmentVariable( @"PATH" );
            string envPath = getDataFromConfigFile();
            if ( envPath != string.Empty )
            {
                SetEnvironmentVariable( @"PATH", envPath + @";" + path );
            }
        }

        private string getDataFromConfigFile()
        {
            string result = string.Empty;
            try
            {
                string configFile = @"config.ini";
                System.IO.StreamReader reader = new System.IO.StreamReader( configFile );
                result = reader.ReadLine();
            }
            catch ( Exception e )
            {
                //MessageBox.Show( e.Message, e.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            return result;
        }

        private void loadTestFunction()
        {
            listView1.Items.Clear();
            richTextBox1.Text = string.Empty;

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
            openFileDialog1.Filter = @"QTestLib実行ファイル(*.exe)|*.exe";
            DialogResult ret = openFileDialog1.ShowDialog();
            if ( ret == DialogResult.OK )
            {
                _exeFileName = openFileDialog1.FileName;

                toolStripStatusLabel1.Text = System.IO.Path.GetFileName( _exeFileName );

                loadTestFunction();
            }
        }

        private void exitToolStripMenuItem_Click( object sender, EventArgs e )
        {
            this.Close();
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
