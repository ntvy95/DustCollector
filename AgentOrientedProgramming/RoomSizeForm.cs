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
    public partial class RoomSizeForm : Form
    {
        MainForm ParentForm;
        int RoomWidth, RoomHeight;
        public RoomSizeForm(MainForm pf)
        {
            InitializeComponent();
            ParentForm = pf;
        }

        private void RoomSizeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ParentForm.Processing = Process.None;
            Hide();
            e.Cancel = true;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            //try
            //{
                RoomWidth = Int32.Parse(textbox_Width.Text);
                RoomHeight = Int32.Parse(textbox_Height.Text);
                if (RoomWidth == 0 || RoomHeight == 0)
                {
                    throw new Exception("Width and height of the room must be greater than zero!");
                }
                ParentForm.RoomDisplay.Controls.Clear();
                ParentForm.RoomDisplay.RowStyles.Clear();
                ParentForm.RoomDisplay.ColumnStyles.Clear();
                ParentForm.Room = new RoomEnvironment(ParentForm.RoomDisplay, ParentForm);
                ParentForm.Room.SetMap(RoomWidth, RoomHeight);
                for (int i = 0; i < RoomWidth; i++)
                {
                    ParentForm.RoomDisplay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / RoomWidth));
                }
                ParentForm.RoomDisplay.ColumnCount = RoomWidth;
                for (int i = 0; i < RoomHeight; i++)
                {
                    ParentForm.RoomDisplay.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / ParentForm.Height));
                }
                ParentForm.RoomDisplay.RowCount = RoomHeight;
                ParentForm.RoomDisplay.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                ParentForm.RoomDisplay.Refresh();
                ParentForm.Processing = Process.None;
                Hide();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void RoomSizeForm_Activated(object sender, EventArgs e)
        {
            textbox_Width.Text = RoomWidth.ToString();
            textbox_Height.Text = RoomHeight.ToString();
        }
    }
}
