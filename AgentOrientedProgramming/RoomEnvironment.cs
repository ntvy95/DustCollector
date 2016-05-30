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

        public RoomEnvironment(TableLayoutPanel d)
        {
            Display = d;
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

        public Point ConvertCoordination_Agent2Env(Point a)
        {
            return new Point(a.X - DCAgent.position.X, a.Y - DCAgent.position.Y);
        }
    }
}
