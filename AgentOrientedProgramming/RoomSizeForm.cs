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
            try
            {
                RoomWidth = Int32.Parse(textbox_Width.Text);
                RoomHeight = Int32.Parse(textbox_Height.Text);
                if (RoomWidth == 0 || RoomHeight == 0)
                {
                    throw new Exception("Width and height of the room must be greater than zero!");
                }
                ParentForm.Environment.Controls.Clear();
                ParentForm.Environment.RowStyles.Clear();
                ParentForm.Environment.ColumnStyles.Clear();
                ParentForm.bgColors = new Color[RoomWidth, RoomHeight];
                for (int i = 0; i < RoomWidth; i++)
                {
                    for (int j = 0; j < RoomHeight; j++)
                    {
                        ParentForm.bgColors[i, j] = ParentForm.CellColorDictionary[Process.None];
                    }
                }
                for (int i = 0; i < RoomWidth; i++)
                {
                    ParentForm.Environment.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / RoomWidth));
                }
                ParentForm.Environment.ColumnCount = RoomWidth;
                for (int i = 0; i < RoomHeight; i++)
                {
                    ParentForm.Environment.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / ParentForm.Height));
                }
                ParentForm.Environment.RowCount = RoomHeight;
                ParentForm.Environment.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                ParentForm.Environment.Refresh();
                ParentForm.Processing = Process.None;
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RoomSizeForm_Activated(object sender, EventArgs e)
        {
            textbox_Width.Text = RoomWidth.ToString();
            textbox_Height.Text = RoomHeight.ToString();
        }
    }
}
