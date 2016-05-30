using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AgentOrientedProgramming
{
    public class RoomObject
    {
        public static Color objcolor = Color.White;
        public static Process objtype = Process.None;
        public Color color;
        public Process type;
        public Point position;
        public RoomEnvironment room;
        public RoomObject Below;

        public RoomObject(Point p, RoomEnvironment r)
        {
            position = p;
            room = r;
            color = RoomObject.objcolor;
            type = RoomObject.objtype;
            if (room.Map != null)
            {
                Below = room.Map[p.X, p.Y];
                room.UpdateRoomObject(p, this);
                if (Below != null && (Below.type == Process.SetAgent || Below.type == Process.SetObstacles))
                {
                    Below = new RoomObject(position, room);
                }
            }
        }

        public void Move(Point p)
        {
            room.UpdateRoomObject(position, Below);
            Below = room.Map[p.X, p.Y];
            room.UpdateRoomObject(p, this);
        }
    }

    public class Dust : RoomObject
    {
        new public static Color objcolor = Color.LightGray;
        new public static Process objtype = Process.SetDust;
        public Dust(Point p, RoomEnvironment r)
            : base(p, r)
        {
            base.color = Dust.objcolor;
            base.type = Dust.objtype;
            room.SynchronizeColor(p);
        }
    }

    public class Obstacle : RoomObject
    {
        new public static Color objcolor = Color.Black;
        new public static Process objtype = Process.SetObstacles;
        public bool isMovable;
        public Obstacle(Point p, RoomEnvironment r, bool i)
            : base(p, r)
        {
            base.color = Obstacle.objcolor;
            base.type = Obstacle.objtype;
            isMovable = i;
            room.SynchronizeColor(p);
        }
        public void Update()
        {
            if (isMovable)
            {
                Random r = new Random();
                int movex, movey, nextx, nexty;
                do
                {
                    movex = r.Next(-1, 2);
                    movey = r.Next(-1, 2);
                    nextx = base.position.X + movex;
                    nexty = base.position.Y + movey;
                } while (nextx < 0 || nexty < 0
                    || base.room.Map[nextx, nexty].type == Process.SetObstacles
                    || base.room.Map[nextx, nexty].type == Process.SetAgent);
                base.room.Map[base.position.X, base.position.Y] = Below;
                Below = base.room.Map[nextx, nexty];
                base.room.Map[nextx, nexty] = this;
                base.position.X = nextx;
                base.position.Y = nexty;
            }
        }
        public string getObstacleState()
        {
            if (isMovable)
            {
                return "[Obstacle : MOVABLE]";
            }
            else
            {
                return "[Obstacle : IMMOVABLE]";
            }
        }
    }

    public class Agent : RoomObject
    {
        new public static Color objcolor = Color.LightBlue;
        new public static Process objtype = Process.SetAgent;
        public string direction;
        public string action;
        public Point InnerPosition; //Agent's position in inner coordination.
        public int a;
        public Agent(Point p, RoomEnvironment r, string d)
            : base(p, r)
        {
            if (base.room.DCAgent != null)
            {
                new RoomObject(base.room.DCAgent.position, r);
            }
            base.color = Agent.objcolor;
            base.type = Agent.objtype;
            base.room.DCAgent = this;
            this.a = 0;
            this.direction = d;
            this.action = "[Action : NONE]";
            room.SynchronizeColor(p);
        }
        public string Turn90()
        {
            this.a = this.a + 90;
            switch (direction)
            {
                case "[Agent : UP]":
                    return "[Agent : LEFT]";
                case "[Agent : LEFT]":
                    return "[Agent : DOWN]";
                case "[Agent : DOWN]":
                    return "[Agent : RIGHT]";
                case "[Agent : RIGHT]":
                    return "[Agent : UP]";
            }
            return null;
        }
        public string getEnvironment()
        {
            if (Below.type == Process.SetDust)
            {
                return "[Environment : DIRTY]";
            }
            return "[Environment : CLEAN]";
        }
        public string getAgentState()
        {
            return getEnvironment() + " " + direction + " " + action;
        }
    }
}
