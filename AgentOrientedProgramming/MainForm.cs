using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtensionMethods;
using System.Diagnostics;
using System.IO;

namespace AgentOrientedProgramming
{
    public partial class MainForm : Form
    {
        private int timePassed;
        public RoomEnvironment Room;
        public DBLayoutPanel RoomDisplay;
        public Process Processing;
        private RoomSizeForm RSForm;
        public SetRandomForm SRForm;
        private SetForwardForm SFForm;
        private delegate void Procedure();
        private delegate bool Condition();
        
        public MainForm()
        {
            InitializeComponent();
            this.Text = "Dust Collector";
            RSForm = new RoomSizeForm(this);
            SRForm = new SetRandomForm(this);
            SFForm = new SetForwardForm(this);
            Processing = Process.None;

            RoomDisplay = new DBLayoutPanel();
            RoomDisplay.Parent = MainPanel;
            RoomDisplay.Dock = DockStyle.Fill;
            RoomDisplay.CellPaint += Environment_CellPaint;
            RoomDisplay.BackColor = Color.White;

            Room = new RoomEnvironment(RoomDisplay, this);
            this.timePassed = 0;
        }

        public Color ColorByProcess(Process p)
        {
            switch (p)
            {
                case Process.None:
                    return RoomObject.objcolor;
                case Process.SetAgent:
                    return Agent.objcolor;
                case Process.SetDust:
                    return Dust.objcolor;
                case Process.SetObstacles:
                    return Obstacle.objcolor;
            }
            return Color.White;
        }

        void Environment_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Column >= 0 && e.Row >=0
                && e.Column < Room.bgColors.GetLength(0)
                && e.Row < Room.bgColors.GetLength(1))
            {
                using (var b = new SolidBrush(Room.bgColors[e.Column, e.Row]))
                {
                    e.Graphics.FillRectangle(b, e.CellBounds);
                }
            }
        }
        private void Set_Click(Process P, Condition Cond = null,
            Procedure StartProcedure = null, Procedure EndProcedure = null)
        {
            while (true && (Cond == null || Cond()))
            {
                if (Processing == Process.None)
                {
                    if (StartProcedure != null)
                        StartProcedure();
                    return;
                }
                else if (Processing == P)
                {
                    if (EndProcedure != null)
                        EndProcedure();
                    return;
                }
                else
                {
                    switch (Processing)
                    {
                        case Process.SetAgent:
                            SetManually_Agent_Click();
                            break;
                        case Process.SetDust:
                            SetManually_Dust_Click();
                            break;
                        case Process.SetObstacles:
                            SetManually_Obstacles_Click();
                            break;
                    }
                }
            }
        }
        private void SetManually_Click(Process P, ToolStripMenuItem MItem, MouseEventHandler EHandler, Procedure StartProcedure = null, Procedure EndProcedure = null)
        {
            Set_Click(P,
                () =>
                {
                    return RoomDisplay.RowCount > 0 && RoomDisplay.ColumnCount > 0;
                },
                () =>
                {
                    Processing = P;
                    RoomDisplay.MouseDown += EHandler;
                    MItem.Text = "Stop setting manually";
                    if (StartProcedure != null)
                        StartProcedure();
                },
                () =>
                {
                    Processing = Process.None;
                    RoomDisplay.MouseDown -= EHandler;
                    MItem.Text = "Set manually";
                    if (EndProcedure != null)
                        EndProcedure();
                }
                );
        }
        private void SetRoomSize_Click(object sender, EventArgs e)
        {
            Set_Click(Process.SetRoomSize, null,
                () =>
            {
                Processing = Process.SetRoomSize;
                RSForm.Show();
            }, () =>
            {
                RSForm.Focus();
            });
        }
        private void Click_CellLabel(Point P, Control CellLabel)
        {
            CellLabel.MouseDown += (object s, MouseEventArgs ev) =>
            {
                if (Room.Map[P.X, P.Y] == null)
                {
                    CellLabel.Dispose();
                }
                else if (Processing != Process.None)
                {
                    switch (Room.Map[P.X, P.Y].type)
                    {
                        case Process.SetAgent:
                            if (Processing == Process.SetAgent)
                            {
                                Room.EmptyFloor(P);
                                Room.DCAgent = null;
                                SetManually_Agent_Direction.Enabled = false;
                                CellLabel.Dispose();
                            }
                            break;
                        case Process.SetObstacles:
                            if (Processing == Process.SetObstacles)
                            {
                                bool isMovable = false;
                                if (ev.Button == MouseButtons.Right)
                                {
                                    isMovable = true;
                                }
                                if (Room.Map[P.X, P.Y].isMovable == isMovable)
                                {
                                    Room.EmptyFloor(P);
                                    CellLabel.Dispose();
                                }
                                else
                                {
                                    Room.Map[P.X, P.Y].UpdateMovableState(isMovable);
                                    CellLabel.Text = ((Obstacle)Room.Map[P.X, P.Y]).ToString();
                                }
                            }
                            break;
                    }
                }
                RoomDisplay.Refresh();
            };
        }
        public Label Create_CellLabel(Point p)
        {
            Label CellLabel = new Label();
            CellLabel.AutoSize = false;
            CellLabel.Dock = DockStyle.Fill;
            CellLabel.TextAlign = ContentAlignment.MiddleCenter;
            CellLabel.BackColor = Color.Transparent;
            RoomDisplay.Controls.Add(CellLabel, p.X, p.Y);
            return CellLabel;
        }
        public Label Create_CellLabel(RoomObject RObject)
        {
            Label CellLabel = Create_CellLabel(RObject.position);
            Click_CellLabel(RObject.position, CellLabel);
            Update_CellLabel(CellLabel, RObject);
            return CellLabel;
        }
        public void Update_CellLabel(Label CellLabel, RoomObject RObject)
        {
            CellLabel.Text = RObject.ToString();
            switch (RObject.type)
            {
                case Process.SetObstacles:
                    CellLabel.ForeColor = Color.White;
                    break;
                case Process.SetAgent:
                    CellLabel.ForeColor = Color.Black;
                    break;
            }
        }
        public Point Environment_Click(Process P, Point? Position = null, bool isMovable = false)
        {
            if (Position == null)
            {
                Position = RoomDisplay.GetCellPositionFromCursorPosition();
            }
            if (Room.Map[Position.Value.X, Position.Value.Y] != null
                && Room.Map[Position.Value.X, Position.Value.Y].type == P)
            {
                Room.EmptyFloor(Position.Value);
            }
            else
            {
                RoomObject RObject = null;
                switch (P)
                {
                    case Process.SetAgent:
                        RObject = new Agent(Position.Value, Room, "[Agent : UP]");
                        break;
                    case Process.SetDust:
                        RObject = new Dust(Position.Value, Room);
                        break;
                    case Process.SetObstacles:
                        RObject = new Obstacle(Position.Value, Room, isMovable);
                        break;
                }
                if (RObject != null && RObject.ToString() != null)
                {
                    Label CellLabel = (Label)RoomDisplay.GetControlFromPosition(Position.Value.X, Position.Value.Y);
                    if (CellLabel == null)
                    {
                        CellLabel = Create_CellLabel(RObject);
                    }
                    else
                    {
                        Update_CellLabel(CellLabel, RObject);
                    }
                }
            }
            RoomDisplay.Refresh();
            return Position.Value;
        }
        private void Environment_Click_Obstacles(object sender, MouseEventArgs e)
        {
            bool isMovable = false;
            if (e.Button == MouseButtons.Right)
            {
                isMovable = true;
            }
            Environment_Click(Process.SetObstacles, null, isMovable);
        }
        private void Environment_Click_Dust(object sender, MouseEventArgs e)
        {
            Environment_Click(Process.SetDust);
        }
        private void Environment_Click_Agent(object sender, MouseEventArgs e)
        {
            Agent_Remove();
            Environment_Click(Process.SetAgent);
            SetManually_Agent_Direction.Enabled = true;
        }
        private void SetManually_Obstacles_Click(object sender = null, EventArgs e = null)
        {
            SetManually_Click(Process.SetObstacles,
                SetManually_Obstacles, Environment_Click_Obstacles);
        }
        private void SetManually_Dust_Click(object sender = null, EventArgs e = null)
        {
            SetManually_Click(Process.SetDust,
                SetManually_Dust, Environment_Click_Dust);
        }

        private void SetManually_Agent_Click(object sender = null, EventArgs e = null)
        {
            SetManually_Click(Process.SetAgent,
                SetManually_Agent, Environment_Click_Agent);
        }

        private void Agent_Remove()
        {
            if (Room.DCAgent != null)
            {
                Room.Remove_Agent();
                SetManually_Agent_Direction.Enabled = false;
            }
        }

        private void Agent_UpdateDirection(string Direction)
        {
            Room.DCAgent.direction = Direction;
            RoomDisplay.GetControlFromPosition(Room.DCAgent.position.X, Room.DCAgent.position.Y).Text
            = Room.DCAgent.ToString();
        }
        private void SetManually_Agent_UP_Click(object sender, EventArgs e)
        {
            Agent_UpdateDirection("[Agent : UP]");
        }

        private void SetManually_Agent_DOWN_Click(object sender, EventArgs e)
        {
            Agent_UpdateDirection("[Agent : DOWN]");
        }

        private void SetManually_Agent_LEFT_Click(object sender, EventArgs e)
        {
            Agent_UpdateDirection("[Agent : LEFT]");
        }

        private void SetManually_Agent_RIGHT_Click(object sender, EventArgs e)
        {
            Agent_UpdateDirection("[Agent : RIGHT]");
        }

        private void SetRandom()
        {
            if (SRForm.IsAccessible)
            {
                SRForm.Focus();
            }
            else
            {
                SRForm.Show();
            }
        }

        private void SetRandom_Obstacles_Click(object sender, EventArgs e)
        {
            Processing = Process.SetObstacles;
            SetManually_Obstacles.Text = "Set manually";
            SetRandom();
        }

        private void SetRandom_Dust_Click(object sender, EventArgs e)
        {
            Processing = Process.SetDust;
            SetManually_Dust.Text = "Set manually";
            SetRandom();
        }

        private void SetRandom_Agent_Click(object sender, EventArgs e)
        {
            Processing = Process.SetAgent;
            SetManually_Agent.Text = "Set manually";
            Agent_Remove();
            SRForm.Random(1, new HashSet<Process> { Process.SetDust });
            SetManually_Agent_Direction.Enabled = true;
            Processing = Process.None;
        }

        private void Cell_Remove_Color(Color c)
        {
            for (int i = 0; i < RoomDisplay.ColumnCount; i++)
            {
                for (int j = 0; j < RoomDisplay.RowCount; j++)
                {
                    if (Room.bgColors[i, j] == c)
                    {
                        Room.bgColors[i, j] = Color.White;
                    }
                }
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("DUST COLLECTOR\nGiáo viên hướng dẫn:\nThs. HUỲNH THỊ THANH THƯƠNG\nNhóm sinh viên:\n13520006 - LÊ KHẮC AN\n13520280 - ĐINH QUANG HÌNH\n13520803 - HUỲNH THANH THẢO\n13521064 - NGUYỄN THỤY VY");
        }

        public void oneNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Room.UpdateMap();
            RoomDisplay.GetControlFromPosition(Room.DCAgent.position.X, Room.DCAgent.position.Y).Text
            = Room.DCAgent.ToString() + "[Weight : " + Room.AgentWeight[Room.DCAgent.position.X, Room.DCAgent.position.Y] + "]";
            UpdateEnvTime();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RoomDisplay.RowCount <= 0 || RoomDisplay.ColumnCount <= 0)
            {
                SetRoomSize_Click(null, null);
            }
            if (Room.DCAgent == null)
            {
                SetRandom_Agent_Click(null, null);
            }
            Set_Click(Process.None);
            RoomDisplay.RemoveClickEvent();
            editToolStripMenuItem.Enabled = false;
            startToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = true;
            oneNextToolStripMenuItem.Enabled = true;
            forwardToToolStripMenuItem.Enabled = true;
            Room.DCAgent.Start();
        }

        private void UpdateEnvTime()
        {
            this.timePassed = this.timePassed + 1;
            time0ToolStripMenuItem.Text = "Time: " + this.timePassed;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editToolStripMenuItem.Enabled = true;
            startToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = false;
            oneNextToolStripMenuItem.Enabled = false;
            forwardToToolStripMenuItem.Enabled = false;
            SFForm.Hide();
            this.timePassed = 0;
        }

        private void forwardToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SFForm.IsAccessible)
            {
                SFForm.Focus();
            }
            else
            {
                SFForm.Show();
            }
        }

        private void selectPROLOGDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                Environment.SetEnvironmentVariable("SWI_HOME_DIR", folderBrowserDialog1.SelectedPath);
            }
        }
    }
    public enum Process
    {
        None, SetRoomSize, SetObstacles, SetAgent, SetDust
    }
}
