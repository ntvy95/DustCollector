using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SbsSW.SwiPlCs;
using System.Windows.Forms;

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
        public bool isMovable;

        public RoomObject(Point p, RoomEnvironment r, bool i = false)
        {
            position = p;
            room = r;
            color = RoomObject.objcolor;
            type = RoomObject.objtype;
            UpdateMovableState(i);
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

        public void UpdateMovableState(bool i)
        {
            if (isMovable && !i)
            {
                room.UpdatableObject.Remove(this);
            }
            else if (i)
            {
                room.UpdatableObject.Add(this);
            }
            isMovable = i;
        }

        public virtual void Update()
        {

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
        public Obstacle(Point p, RoomEnvironment r, bool i)
            : base(p, r, i)
        {
            base.color = Obstacle.objcolor;
            base.type = Obstacle.objtype;
            room.SynchronizeColor(p);
        }
        public override void Update()
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
                Label CellLabel = (Label)room.Display.GetControlFromPosition(position.X, position.Y);
                room.Display.Controls.Add(CellLabel, nextx, nexty);
                base.Move(new Point(nextx, nexty));
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
        public int m;
        public int leftmoves;
        public List<Weight> weight;
        public List<Point> discover;
        public Agent(Point p, RoomEnvironment r, string d)
            : base(p, r, true)
        {
            if (base.room.DCAgent != null)
            {
                new RoomObject(base.room.DCAgent.position, r);
            }
            base.color = Agent.objcolor;
            base.type = Agent.objtype;
            base.room.DCAgent = this;
            this.a = 0;
            this.m = this.a;
            this.leftmoves = 1;
            this.weight = new List<Weight>() { new Weight(this.InnerPosition.X, this.InnerPosition.Y, 1) };
            this.discover = new List<Point>() { new Point(this.position.X, this.position.Y) };
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
        public override void Update()
        {

        }
        public IEnumerable<string> infer()
        {
            if (!PlEngine.IsInitialized)
            {
                String[] param = { "-q" };  // suppressing informational and banner messages
                PlEngine.Initialize(param);
                PlQuery.PlCall("consult('HutBuiDegToRad')");
                PlQuery.PlCall("assert(do(" + this.action + "))");
                PlQuery.PlCall("assert(in(" + this.InnerPosition.X + "," + this.InnerPosition.Y + "))");
                PlQuery.PlCall("assert(facing(" + this.a + "))");
                PlQuery.PlCall("assert(choose(" + this.m + "))");
                if (this.isDirty())
                {
                    PlQuery.PlCall("assert(dirty(" + this.InnerPosition.X + "," + this.InnerPosition.Y + "))");
                }
                if (this.isObstacle())
                {
                    PlQuery.PlCall("assert(obstacle(" + this.upFront().X + "," + this.upFront().Y + "))");
                }
                PlQuery.PlCall("assert(leftmoves(" + this.leftmoves + "))");
                foreach (Weight w in this.weight)
                {
                    PlQuery.PlCall("assert(weight(" + w.position.X + "," + w.position.Y + "," + w.weight + "))");
                }
                foreach (Point d in this.discover)
                {
                    PlQuery.PlCall("assert(discover(" + d.X + "," + d.Y + "))");
                }
                using (var q = new PlQuery("do(A)"))
                {
                    PlQuery.PlCall("retract(do(" + this.action + "))");
                    this.action = q.Variables["A"].ToString();
                    yield return this.action;
                    PlQuery.PlCall("assert(do(" + this.action + "))");
                }
                PlEngine.PlCleanup();
            }
        }
        public bool isDirty()
        {
            return Below.type == Process.SetDust;
        }
        public bool isObstacle()
        {
            Point upfront = upFront();
            return upfront.X < 0 || upfront.X >= room.Map.GetLength(0)
                || upfront.Y < 0 || upfront.Y >= room.Map.GetLength(1)
                || room.Map[upfront.X, upfront.Y].type == Process.SetObstacles;
        }
        public Point upFront()
        {
            Point upfront = new Point(position.X, position.Y);
            switch (this.direction)
            {
                case "[Agent : UP]":
                    upfront.Y = upfront.Y - 1;
                    break;
                case "[Agent : LEFT]":
                    upfront.X = upfront.X - 1;
                    break;
                case "[Agent : DOWN]":
                    upfront.Y = upfront.Y + 1;
                    break;
                case "[Agent : RIGHT]":
                    upfront.X = upfront.X + 1;
                    break;
            }
            return upfront;
        }
        public Point Inner2Outer(Point Inner)
        {
            return new Point(position.X - InnerPosition.X + Inner.X, position.Y - InnerPosition.Y + Inner.Y);
        }
    }
    public class Weight
    {
        public Point position;
        public int weight;

        public Weight(int X, int Y, int W)
        {
            position = new Point(X, Y);
            weight = W;
        }
    }
}
