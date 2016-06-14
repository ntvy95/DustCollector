using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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

        public RoomEnvironment(DBLayoutPanel d, MainForm MF)
        {
            Display = d;
            MForm = MF;
            UpdatableObject = new List<RoomObject>();
        }

        public void SetMap(int width, int height) {
            Map = new RoomObject[width, height];
            bgColors = new Color[width, height];
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
            MForm.SRForm.Random(r.Next(0, 2), null);
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
