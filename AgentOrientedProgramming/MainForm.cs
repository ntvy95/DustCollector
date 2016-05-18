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
    public enum Process
    {
        None, SetRoomSize, SetObstacles, SetAgent
    }
    public partial class MainForm : Form
    {
        public TableLayoutPanel Environment;
        public Process Processing;
        RoomSizeForm RSForm;
        
        public MainForm()
        {
            InitializeComponent();
            this.Text = "Dust Collector";
            Environment = new TableLayoutPanel();
            RSForm = new RoomSizeForm(this);
            Processing = Process.None;

            Environment.Parent = MainPanel;
            Environment.Dock = DockStyle.Fill;
        }

        private void setRoomsSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Processing == Process.None)
            {
                Processing = Process.SetRoomSize;
                RSForm.Show();
            }
        }

        private void setObstaclesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Processing == Process.None)
            {
                Processing = Process.SetObstacles;
                Environment.Click += Environment_Click;
                setObstaclesToolStripMenuItem.Text = "Stop setting obstacles";
            }
            else if (Processing == Process.SetObstacles)
            {
                Environment.Click -= Environment_Click;
                setObstaclesToolStripMenuItem.Text = "Set obstacles";
                Processing = Process.None;
            }
        }
        private void Environment_Click(object sender, EventArgs e)
        {
            Point Position = Environment.PointToClient(Cursor.Position);
            int Column = Convert.ToInt32(Math.Floor(
                         Convert.ToDouble(Position.X) / Environment.Width * Environment.ColumnCount)),
                Row = Convert.ToInt32(Math.Floor(
                      Convert.ToDouble(Position.Y) / Environment.Height * Environment.RowCount));
            Panel CellPanel = new Panel();
            CellPanel.BackColor = Color.LightGray;
            CellPanel.Dock = DockStyle.Fill;
            Environment.Controls.Add(CellPanel, Column, Row);
            Environment.GetCe
        }
        private Point GetCellPosition(Environment)
    }
}
