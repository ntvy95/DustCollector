using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace AgentOrientedProgramming
{
    public partial class SetForwardForm : Form
    {
        MainForm ParentForm;
        public SetForwardForm(MainForm MForm)
        {
            InitializeComponent();
            ParentForm = MForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Enabled = false;
                int ForwardTime = Int32.Parse(textBox1.Text);
                while (ForwardTime > 0)
                {
                    ParentForm.oneNextToolStripMenuItem_Click(null, null);
                    ParentForm.Refresh();
                    Thread.Sleep(1000);
                    ForwardTime = ForwardTime - 1;
                }
                textBox1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
