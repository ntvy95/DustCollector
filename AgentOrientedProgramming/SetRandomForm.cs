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
                if (NumberOfRandom_number > ParentForm.RoomDisplay.ColumnCount * ParentForm.RoomDisplay.RowCount)
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
            Point? Position = null;
            while (N > 0)
            {
                Position = Random_ColumnRow(AllowedCell);
                if (Position == null)
                {
                    break;
                }
                ParentForm.Environment_Click(ParentForm.Processing, Position);
                N = N - 1;
            }
        }

        private Point? Random_ColumnRow(HashSet<Process> AllowedCell = null)
        {
            Random r = new Random();
            int Column, Row;
            bool[,] found = new bool[ParentForm.RoomDisplay.ColumnCount, ParentForm.RoomDisplay.RowCount];
            int countleft = ParentForm.RoomDisplay.ColumnCount * ParentForm.RoomDisplay.RowCount;
            do
            {
                Column = r.Next(0, ParentForm.RoomDisplay.ColumnCount);
                Row = r.Next(0, ParentForm.RoomDisplay.RowCount);
                if (!found[Column, Row])
                {
                    if (ParentForm.Room.Map[Column, Row] == null
                        || (ParentForm.Room.Map[Column, Row] != null
                        && AllowedCell != null && AllowedCell.Contains(ParentForm.Room.Map[Column, Row].type) == true))
                    {
                        return new Point(Column, Row);
                    }
                    countleft = countleft - 1;
                    found[Column, Row] = true;
                }
            } while (countleft > 0);
            return null;
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
