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

namespace AgentOrientedProgramming
{
    public partial class MainForm : Form
    {
        public TableLayoutPanel Environment;
        public Process Processing;
        public Color[,] bgColors;
        public Dictionary<Process, Color> CellColorDictionary;
        private RoomSizeForm RSForm;
        private SetRandomForm SRForm;
        private Point AgentPosition;
        private delegate void Procedure();
        private delegate bool Condition();
        
        public MainForm()
        {
            InitializeComponent();
            this.Text = "Dust Collector";
            RSForm = new RoomSizeForm(this);
            SRForm = new SetRandomForm(this);
            Processing = Process.None;

            Environment = new TableLayoutPanel();
            Environment.Parent = MainPanel;
            Environment.Dock = DockStyle.Fill;
            Environment.CellPaint += Environment_CellPaint;

            CellColorDictionary = new Dictionary<Process, Color>();
            CellColorDictionary.Add(Process.None, Color.White);
            CellColorDictionary.Add(Process.SetAgent, Color.LightBlue);
            CellColorDictionary.Add(Process.SetDust, Color.LightGray);
            CellColorDictionary.Add(Process.SetObstacles, Color.Black);
        }

        void Environment_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            using (var b = new SolidBrush(bgColors[e.Column, e.Row]))
            {
                e.Graphics.FillRectangle(b, e.CellBounds);
            }
        }
        private void Set_Click(Process P, Condition Cond, Procedure StartProcedure, Procedure EndProcedure)
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
                            SetManually_Agent_Click(null, null);
                            break;
                        case Process.SetDust:
                            SetManually_Dust_Click(null, null);
                            break;
                        case Process.SetObstacles:
                            SetManually_Obstacles_Click(null, null);
                            break;
                    }
                }
            }
        }
        private void SetManually_Click(Process P, ToolStripMenuItem MItem, EventHandler EHandler, Procedure StartProcedure = null, Procedure EndProcedure = null)
        {
            Set_Click(P,
                () =>
                {
                    return Environment.RowCount > 0 && Environment.ColumnCount > 0;
                },
                () =>
                {
                    Processing = P;
                    Environment.Click += EHandler;
                    MItem.Text = "Stop setting manually";
                    if (StartProcedure != null)
                        StartProcedure();
                },
                () =>
                {
                    Processing = Process.None;
                    Environment.Click -= EHandler;
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
        private void Click_To_Dispose(Process P, Control Main, Control Sub)
        {
            Main.Click += (object s, EventArgs ev) =>
            {
                Sub.Dispose();
                bgColors[AgentPosition.X, AgentPosition.Y] = CellColorDictionary[Process.None];
                SetManually_Agent_Direction.Enabled = false;
                Environment.Refresh();
            };
        }
        public Point Environment_Click(Process P, Point? Position = null)
        {
            if (Position == null)
            {
                Position = Environment.GetCellPositionFromCursorPosition();
            }
            if (bgColors[Position.Value.X, Position.Value.Y] == CellColorDictionary[P])
            {
                bgColors[Position.Value.X, Position.Value.Y] = CellColorDictionary[Process.None];
            }
            else
            {
                string CellText = null;
                switch (P)
                {
                    case Process.SetAgent:
                        if (bgColors[Position.Value.X, Position.Value.Y] == CellColorDictionary[Process.SetDust])
                        {
                            CellText = "[Environment : DIRTY] [Agent : UP] [Action : NONE]";
                        }
                        else
                        {
                            CellText = "[Environment : CLEAN] [Agent : UP] [Action : NONE]";
                        }
                        break;
                }

                bgColors[Position.Value.X, Position.Value.Y] = CellColorDictionary[P];
                Label CellLabel = (Label)Environment.GetControlFromPosition(Position.Value.X, Position.Value.Y);
                if (CellText != null)
                {
                    if (CellLabel == null)
                    {
                        CellLabel = new Label();
                        CellLabel.AutoSize = false;
                        CellLabel.Dock = DockStyle.Fill;
                        CellLabel.TextAlign = ContentAlignment.MiddleCenter;
                        CellLabel.BackColor = Color.Transparent;
                        Click_To_Dispose(P, CellLabel, CellLabel);
                        Environment.Controls.Add(CellLabel, Position.Value.X, Position.Value.Y);
                    }
                    CellLabel.Text = CellText;
                }
            }
            Environment.Refresh();
            return Position.Value;
        }
        private void Environment_Click_Obstacles(object sender, EventArgs e)
        {
            Environment_Click(Process.SetObstacles);
        }
        private void Environment_Click_Dust(object sender, EventArgs e)
        {
            Environment_Click(Process.SetDust);
        }
        private void Environment_Click_Agent(object sender, EventArgs e)
        {
            Agent_Remove();
            AgentPosition = Environment_Click(Process.SetAgent);
            SetManually_Agent_Direction.Enabled = true;
        }
        private void SetManually_Obstacles_Click(object sender, EventArgs e)
        {
            SetManually_Click(Process.SetObstacles,
                SetManually_Obstacles, Environment_Click_Obstacles);
        }
        private void SetManually_Dust_Click(object sender, EventArgs e)
        {
            SetManually_Click(Process.SetDust,
                SetManually_Dust, Environment_Click_Dust);
        }

        private void SetManually_Agent_Click(object sender, EventArgs e)
        {
            SetManually_Click(Process.SetAgent,
                SetManually_Agent, Environment_Click_Agent,
                () =>
                {
                    for (int i = 0; i < Environment.Controls.Count; i++)
                    {
                        if (Environment.Controls[i].BackColor == CellColorDictionary[Process.SetDust])
                        {
                            Environment.Controls[i].Click += Environment_Click_Agent;
                        }
                    }
                },
                () =>
                {
                    for (int i = 0; i < Environment.Controls.Count; i++)
                    {
                        if (Environment.Controls[i].BackColor == CellColorDictionary[Process.SetDust])
                        {
                            Environment.Controls[i].Click -= Environment_Click_Agent;
                        }
                    }
                    SetManually_Agent.Text = "Set up";
                });
        }

        private void Agent_Remove()
        {
            if (AgentPosition != null)
            {
                Label CellLabel = (Label)Environment.GetControlFromPosition(AgentPosition.X, AgentPosition.Y);
                if (CellLabel != null)
                {
                    //if (CellLabel.Text.Contains("[Environment : DIRTY]"))
                    //{
                    //    bgColors[AgentPosition.X, AgentPosition.Y] = CellColorDictionary[Process.SetDust];
                    //}
                    //else
                    //{
                    //    bgColors[AgentPosition.X, AgentPosition.Y] = CellColorDictionary[Process.None];
                    //}
                    bgColors[AgentPosition.X, AgentPosition.Y] = CellColorDictionary[Process.None];
                    CellLabel.Dispose();
                }
                SetManually_Agent_Direction.Enabled = false;
            }
        }

        private void Agent_UpdateDirection(string Direction)
        {
            Environment.GetControlFromPosition(AgentPosition.X, AgentPosition.Y).Text
            = Environment.GetControlFromPosition(AgentPosition.X, AgentPosition.Y).Text
                .Replace("[Agent : UP]", Direction)
                .Replace("[Agent : DOWN]", Direction)
                .Replace("[Agent : LEFT]", Direction)
                .Replace("[Agent : RIGHT]", Direction);
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
            SetManually_Agent.Text = "Set up";
            Agent_Remove();
            AgentPosition = SRForm.Random(1, new HashSet<Color> { CellColorDictionary[Process.SetDust] });
            SetManually_Agent_Direction.Enabled = true;
            Processing = Process.None;
        }

        private void Cell_Remove_Color(Color c)
        {
            for (int i = 0; i < Environment.ColumnCount; i++)
            {
                for (int j = 0; j < Environment.RowCount; j++)
                {
                    if (bgColors[i, j] == c)
                    {
                        bgColors[i, j] = Color.White;
                    }
                }
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("DUST COLLECTOR");
        }
    }
    public enum Process
    {
        None, SetRoomSize, SetObstacles, SetAgent, SetDust
    }
    public class Cell
    {
        Color Color;
        Process Type;
        bool Movable;
    }
}
