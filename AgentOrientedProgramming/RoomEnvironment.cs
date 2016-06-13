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
        public TableLayoutPanel Display;
        public Agent DCAgent; //in environment's coordination.
        public List<RoomObject> UpdatableObject;

        public RoomEnvironment(TableLayoutPanel d)
        {
            Display = d;
            UpdatableObject = new List<RoomObject>();
        }

        public void SetMap(int width, int height) {
            Map = new RoomObject[width, height];
            bgColors = new Color[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    new RoomObject(new Point(i, j), this);
                    bgColors[i, j] = Map[i, j].color;
                }
            }
        }

        public void UpdateRoomObject(Point p, RoomObject RObject)
        {
            Map[p.X, p.Y] = RObject;
            RObject.position = p;
            bgColors[p.X, p.Y] = RObject.color;
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
            Display.Refresh();
        }

        public void Remove_Agent()
        {
            Label CellLabel = (Label)Display.GetControlFromPosition(DCAgent.position.X, DCAgent.position.Y);
            if (CellLabel != null)
            {
                CellLabel.Dispose();
            }
            new RoomObject(DCAgent.position, this);
            UpdatableObject.Remove(DCAgent);
            DCAgent = null;
        }
    }
}
