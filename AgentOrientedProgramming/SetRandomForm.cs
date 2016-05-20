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

        public Point Random(int N, HashSet<Color> AllowedCell = null)
        {
            Point Position = new Point();
            while (N > 0)
            {
                Position = Random_ColumnRow(AllowedCell);
                ParentForm.Environment_Click(ParentForm.Processing, Position);
                N = N - 1;
            }
            return Position;
        }

        private Point Random_ColumnRow(HashSet<Color> AllowedCell = null)
        {
            Random r = new Random();
            int Column, Row;
            do
            {
                Column = r.Next(0, ParentForm.Environment.ColumnCount - 1);
                Row = r.Next(0, ParentForm.Environment.RowCount - 1);
                Panel CellPanel = (Panel)ParentForm.Environment.GetControlFromPosition(Column, Row);
                if (CellPanel == null || (AllowedCell != null && AllowedCell.Contains(CellPanel.BackColor) == true))
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
