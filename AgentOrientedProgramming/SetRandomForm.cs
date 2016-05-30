using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgentOrientedProgramming
{
    public partial class SetRandomForm : Form
    {
        MainForm ParentForm;
        public SetRandomForm(MainForm pf)
        {
            InitializeComponent();
            ParentForm = pf;
        }

        private void SetRandomForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ParentForm.Processing = Process.None;
            Hide();
            e.Cancel = true;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            try
            {
                int NumberOfRandom_number = Int32.Parse(NumberOfRandom.Text);
                if (NumberOfRandom_number > ParentForm.Environment.ColumnCount * ParentForm.Environment.RowCount)
                {
                    throw new Exception("The number cannot exceed the size of the room!");
                }
                Random(NumberOfRandom_number);
                ParentForm.Processing = Process.None;
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Random(int N, HashSet<Process> AllowedCell = null)
        {
            Point Position = new Point();
            while (N > 0)
            {
                Position = Random_ColumnRow(AllowedCell);
                ParentForm.Environment_Click(ParentForm.Processing, Position);
                N = N - 1;
            }
        }

        private Point Random_ColumnRow(HashSet<Process> AllowedCell = null)
        {
            Random r = new Random();
            int Column, Row;
            do
            {
                Column = r.Next(0, ParentForm.Environment.ColumnCount);
                Row = r.Next(0, ParentForm.Environment.RowCount);
                if (ParentForm.Room.Map[Column, Row].type == Process.None
                    || (AllowedCell != null && AllowedCell.Contains(ParentForm.Room.Map[Column, Row].type) == true))
                {
                    break;
                }
            } while (true);
            return new Point(Column, Row);
        }

        private void SetRandomForm_Load(object sender, EventArgs e)
        {
            this.Text = "Random : " + ParentForm.Processing.ToString();
        }

        private void SetRandomForm_Activated(object sender, EventArgs e)
        {
            this.Text = "Random : " + ParentForm.Processing.ToString();
        }
    }
}
