using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AgentOrientedProgramming
{
    public class RoomEnvironment
    {
        public Color[,] bgColors;
        public RoomObject[,] Map;
        public DBLayoutPanel Display;
        public Agent DCAgent; //in environment's coordination.
        public List<RoomObject> UpdatableObject;
        public MainForm MForm;
        public double[,] AgentWeight;

        public RoomEnvironment(DBLayoutPanel d, MainForm MF)
        {
            Display = d;
            MForm = MF;
            UpdatableObject = new List<RoomObject>();
        }

        public void SetMap(int width, int height) {
            Map = new RoomObject[width, height];
            bgColors = new Color[width, height];
            AgentWeight = new double[width, height];
        }

        public void EmptyFloor(Point position)
        {
            if (Map[position.X, position.Y] != null)
            {
                bgColors[position.X, position.Y] = RoomObject.objcolor;
                if (Map[position.X, position.Y].isMovable)
                {
                    UpdatableObject.Remove(Map[position.X, position.Y]);
                }
                Map[position.X, position.Y] = null;
            }
        }

        public void UpdateRoomObject(Point p, RoomObject RObject)
        {
            Map[p.X, p.Y] = RObject;
            if (RObject != null)
            {
                RObject.position = p;
                bgColors[p.X, p.Y] = RObject.color;
            }
            else
            {
                bgColors[p.X, p.Y] = RoomObject.objcolor;
            }
            UpdateAgentWeight(p);
        }

        public void UpdateAgentWeight(Point p)
        {
            Label CellLabel = (Label)Display.GetControlFromPosition(p.X, p.Y);
            if (AgentWeight[p.X, p.Y] > 0)
            {
                if (CellLabel == null)
                {
                    CellLabel = MForm.Create_CellLabel(p);
                    Display.Controls.Add(CellLabel, p.X, p.Y);
                }
                if (CellLabel.Text.Contains("Weight"))
                {
                    Regex regex = new Regex(@"\[Weight : ([0-9]+)\]");
                    CellLabel.Text = regex.Replace(CellLabel.Text, "[Weight : " + AgentWeight[p.X, p.Y].ToString() + "]");
                }
                else
                {
                    CellLabel.Text = CellLabel.Text + " [Weight : " + AgentWeight[p.X, p.Y].ToString() + "]";
                }
            }
            else if (CellLabel != null)
            {
                Regex regex = new Regex(@"\[Weight : ([0-9]+)\]");
                CellLabel.Text = regex.Replace(CellLabel.Text, "");
            }
        }

        public void ResetAgentWeight()
        {
            AgentWeight = new double[AgentWeight.GetLength(0),AgentWeight.GetLength(1)];
            for (int i = 0; i < bgColors.GetLength(0); i++)
            {
                for (int j = 0; j < bgColors.GetLength(1); j++)
                {
                    Label CellLabel = (Label)Display.GetControlFromPosition(i, j);
                    if (CellLabel != null)
                    {
                        Regex regex = new Regex(@"\[Weight : ([0-9]+)\]");
                        CellLabel.Text = regex.Replace(CellLabel.Text, "");
                    }
                }
            }
        }

        public void SynchronizeColor(Point p)
        {
            bgColors[p.X, p.Y] = Map[p.X, p.Y].color;
        }

        public void UpdateMap()
        {
            foreach (RoomObject RObj in UpdatableObject)
            {
                RObj.Update();
            }
            MForm.Processing = Process.SetDust;
            Random r = new Random();
            MForm.SRForm.Random(r.Next(-10, 2), null);
            MForm.Processing = Process.None;
            Display.Refresh();
        }

        public void Remove_Agent()
        {
            Label CellLabel = (Label)Display.GetControlFromPosition(DCAgent.position.X, DCAgent.position.Y);
            if (CellLabel != null)
            {
                CellLabel.Dispose();
            }
            EmptyFloor(DCAgent.position);
            UpdatableObject.Remove(DCAgent);
            DCAgent = null;
        }
    }
}
