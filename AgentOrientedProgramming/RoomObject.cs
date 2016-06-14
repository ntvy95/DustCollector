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
                    Below = null;
                }
            }
        }

        public void Move(Point p)
        {
            Control c = room.Display.GetControlFromPosition(position.X, position.Y);
            if (c != null)
            {
                room.Display.Controls.Remove(c);
                room.Display.Controls.Add(c, p.X, p.Y);
            }
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
                int movex = r.Next(-1, 2),
                    movey = r.Next(-1, 2),
                    nextx = base.position.X + movex,
                    nexty = base.position.Y + movey;
                if (!(nextx < 0 || nexty < 0
                    || nextx >= base.room.Map.GetLength(0)
                    || nexty >= base.room.Map.GetLength(1)
                    || (base.room.Map[nextx, nexty] != null
                    && (base.room.Map[nextx, nexty].type == Process.SetObstacles
                    || base.room.Map[nextx, nexty].type == Process.SetAgent))))
                {
                    base.Move(new Point(nextx, nexty));
                }
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
        public int timePassed;
        public Dictionary<Point, int> weight;
        public List<Point> discover;
        public int RelativeDirectionType;
        public Agent(Point p, RoomEnvironment r, string d)
            : base(p, r, true)
        {
            if (base.room.DCAgent != null)
            {
                r.Remove_Agent();
            }
            base.color = Agent.objcolor;
            base.type = Agent.objtype;
            base.room.DCAgent = this;
            this.InnerPosition = new Point(0, 0);
            this.direction = d;
            this.action = "start";
            room.SynchronizeColor(p);
        }
        public void Turn90()
        {
            switch (direction)
            {
                case "[Agent : UP]":
                    this.direction = "[Agent : LEFT]";
                    break;
                case "[Agent : LEFT]":
                    this.direction = "[Agent : DOWN]";
                    break;
                case "[Agent : DOWN]":
                    this.direction = "[Agent : RIGHT]";
                    break;
                case "[Agent : RIGHT]":
                    this.direction = "[Agent : UP]";
                    break;
            }
        }
        public void Forward()
        {
            Move(this.upFront());
        }
        public void Suck()
        {
            Below = null;
        }
        public string getEnvironment()
        {
            if (isDirty())
            {
                return "[Environment : DIRTY]";
            }
            return "[Environment : CLEAN]";
        }
        public string getAgentState()
        {
            return getEnvironment() + " " + direction + " [ ACTION : " + action.ToUpper() + " ]";
        }
        public void Start()
        {
            switch (this.direction)
            {
                case "[Agent : UP]":
                    this.RelativeDirectionType = 0;
                    break;
                case "[Agent : LEFT]":
                    this.RelativeDirectionType = 1;
                    break;
                case "[Agent : DOWN]":
                    this.RelativeDirectionType = 2;
                    break;
                case "[Agent : RIGHT]":
                    this.RelativeDirectionType = 3;
                    break;
            }
            this.InnerPosition = new Point(0, 0);
            this.weight = new Dictionary<Point, int>();
            this.discover = new List<Point>();
            this.action = "start";
            this.UpdateInternalState();
        }
        public override void Update()
        {
            Console.WriteLine("------------------------------------------");
            this.Decide();
            switch (this.action)
            {
                case "turn90":
                    this.Turn90();
                    break;
                case "forward":
                    this.Forward();
                    break;
                case "suck":
                    this.Suck();
                    break;
                case "start":
                    this.Start();
                    break;
            }
            if (this.action != "start")
            {
                this.UpdateInternalState();
            }
            Console.WriteLine("------------------------------------------");
        }
        public void UpdateInternalState()
        {
            if (!PlEngine.IsInitialized)
            {
                string[] param = { "-q" };
                PlEngine.Initialize(param);
                PlQuery.PlCall("consult('HutBuiUpdate')");
                PlQuery.PlCall("assert(done(" + this.action + "))");
                Console.WriteLine("assert(done(" + this.action + "))");
                if (this.action != "start")
                {
                    PlQuery.PlCall("assert(wasin(" + this.InnerPosition.X + "," + this.InnerPosition.Y + "))");
                    PlQuery.PlCall("assert(wasfacing(" + this.a + "))");
                    PlQuery.PlCall("assert(wastimePassed(" + this.timePassed + "))");
                    PlQuery.PlCall("assert(wasleftmoves(" + this.leftmoves + "))");

                    Console.WriteLine("assert(wasin(" + this.InnerPosition.X + "," + this.InnerPosition.Y + "))");
                    Console.WriteLine("assert(wasfacing(" + this.a + "))");
                    Console.WriteLine("assert(wastimePassed(" + this.timePassed + "))");
                    Console.WriteLine("assert(wasleftmoves(" + this.leftmoves + "))");
                    foreach (Point w in this.weight.Keys)
                    {
                        PlQuery.PlCall("assert(wasweight(" + w.X + "," + w.Y + "," + this.weight[w] + "))");
                        Console.WriteLine("assert(wasweight(" + w.X + "," + w.Y + "," + this.weight[w] + "))");
                    }
                    foreach (Point d in this.discover)
                    {
                        PlQuery.PlCall("assert(discovered(" + d.X + "," + d.Y + "))");
                        Console.WriteLine("assert(discovered(" + d.X + "," + d.Y + "))");
                    }
                }
                using (var q = new PlQuery("in(X, Y)"))
                {
                    if (q.Solutions.Count() > 0)
                    {
                        PlQueryVariables v = q.SolutionVariables.ElementAt(0);
                        this.InnerPosition.X = Int32.Parse(v["X"].ToString());
                        this.InnerPosition.Y = Int32.Parse(v["Y"].ToString());
                        Console.WriteLine("in(" + this.InnerPosition.X + ", " + this.InnerPosition.Y + ")");
                    }
                }
                using (var q = new PlQuery("facing(X)"))
                {
                    if (q.Solutions.Count() > 0)
                    {
                        PlQueryVariables v = q.SolutionVariables.ElementAt(0);
                        this.a = Int32.Parse(v["X"].ToString());
                        Console.WriteLine("facing(" + this.a + ")");
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    string OsbtacleType = this.TypeOfObstacle(this.upFront());
                    if (OsbtacleType != null)
                    {
                        Point upfront = this.Outer2Inner(this.upFront());
                        Console.WriteLine("assert(obstacle(" + upfront.X + "," + upfront.Y + ",both))");
                        Console.WriteLine("assert(obstacle(" + upfront.X + "," + upfront.Y + "," + OsbtacleType + "))");
                        PlQuery.PlCall("assert(obstacle(" + upfront.X + "," + upfront.Y + ",both))");
                        PlQuery.PlCall("assert(obstacle(" + upfront.X + "," + upfront.Y + "," + OsbtacleType + "))");
                    }
                    this.Turn90();
                }
                using (var q = new PlQuery("weight(X, Y, W)"))
                {
                    if (q.Solutions.Count() > 0)
                    {
                        PlQueryVariables v = q.SolutionVariables.ElementAt(0);
                        int W;
                        if (Int32.TryParse(v["W"].ToString(), out W))
                        {
                            this.weight[new Point(Int32.Parse(v["X"].ToString()), Int32.Parse(v["Y"].ToString()))] = W;
                            Console.WriteLine("weight(" + Int32.Parse(v["X"].ToString()) + ", " + Int32.Parse(v["Y"].ToString()) + ", " + W + ")");
                        }
                    }
                }
                using (var q = new PlQuery("discover(X, Y)"))
                {
                    foreach(PlQueryVariables v in q.SolutionVariables)
                    {
                        this.discover.Add(new Point(Int32.Parse(v["X"].ToString()), Int32.Parse(v["Y"].ToString())));
                        Console.WriteLine("discover(" + Int32.Parse(v["X"].ToString()) + ", " + Int32.Parse(v["Y"].ToString()) + ")");
                    }
                }
                using (var q = new PlQuery("leftmoves(X)"))
                {
                    foreach (PlQueryVariables v in q.SolutionVariables)
                    {
                        this.leftmoves = Int32.Parse(v["X"].ToString());
                        Console.WriteLine("leftmoves(" + Int32.Parse(v["X"].ToString()) + ")");
                    }
                }
                using (var q = new PlQuery("timePassed(X)"))
                {
                    if (q.Solutions.Count() > 0)
                    {
                        PlQueryVariables v = q.SolutionVariables.ElementAt(0);
                        this.timePassed = Int32.Parse(v["X"].ToString());
                        Console.WriteLine("timePassed(" + Int32.Parse(v["X"].ToString()) + ")");
                    }
                }
                PlEngine.PlCleanup();
            }
        }
        public void Decide()
        {
            if (!PlEngine.IsInitialized)
            {
                string[] param = { "-q" };
                PlEngine.Initialize(param);
                PlQuery.PlCall("consult('HutBuiAct')");
                PlQuery.PlCall("assert(done(" + this.action + "))");
                PlQuery.PlCall("assert(in(" + this.InnerPosition.X + "," + this.InnerPosition.Y + "))");
                PlQuery.PlCall("assert(facing(" + this.a + "))");
                PlQuery.PlCall("assert(timePassed(" + this.timePassed + "))");
                PlQuery.PlCall("assert(leftmoves(" + this.leftmoves + "))");

                Console.WriteLine("assert(done(" + this.action + "))");
                Console.WriteLine("assert(in(" + this.InnerPosition.X + "," + this.InnerPosition.Y + "))");
                Console.WriteLine("assert(facing(" + this.a + "))");
                Console.WriteLine("assert(timePassed(" + this.timePassed + "))");
                Console.WriteLine("assert(leftmoves(" + this.leftmoves + "))");
                if (this.isDirty())
                {
                    PlQuery.PlCall("assert(dirty(" + this.InnerPosition.X + "," + this.InnerPosition.Y + "))");
                    Console.WriteLine("assert(dirty(" + this.InnerPosition.X + "," + this.InnerPosition.Y + "))");
                }
                for (int i = 0; i < 4; i++)
                {
                    string OsbtacleType = this.TypeOfObstacle(this.upFront());
                    if (OsbtacleType != null)
                    {
                        Point upfront = this.Outer2Inner(this.upFront());
                        Console.WriteLine("assert(obstacle(" + upfront.X + "," + upfront.Y + ",both))");
                        Console.WriteLine("assert(obstacle(" + upfront.X + "," + upfront.Y + "," + OsbtacleType + "))");
                        PlQuery.PlCall("assert(obstacle(" + upfront.X + "," + upfront.Y + ",both))");
                        PlQuery.PlCall("assert(obstacle(" + upfront.X + "," + upfront.Y + "," + OsbtacleType + "))");
                    }
                    this.Turn90();
                }
                foreach (Point w in this.weight.Keys)
                {
                    PlQuery.PlCall("assert(wasweight(" + w.X + "," + w.Y + "," + this.weight[w] + "))");
                    Console.WriteLine("assert(wasweight(" + w.X + "," + w.Y + "," + this.weight[w] + "))");
                }
                using (var q = new PlQuery("do(A)"))
                {
                    PlQueryVariables v = q.SolutionVariables.ElementAt(0);
                    this.action = v["A"].ToString();
                    Console.WriteLine("do(" + this.action + ")");
                }
                PlEngine.PlCleanup();
            }
        }
        public bool isDirty()
        {
            return Below != null && Below.type == Process.SetDust;
        }
        public string TypeOfObstacle(Point POS)
        {
            //Console.WriteLine("env_obstacle(" + POS.X + "," + POS.Y + ")");
            if (POS.X < 0 || POS.X >= room.Map.GetLength(0)
                || POS.Y < 0 || POS.Y >= room.Map.GetLength(1))
            {
                return "static";
            }
            else if (room.Map[POS.X, POS.Y] != null && room.Map[POS.X, POS.Y].type == Process.SetObstacles)
            {
                if (room.Map[POS.X, POS.Y].isMovable)
                {
                    return "dynamic";
                }
                else
                {
                    return "static";
                }
            }
            return null;
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
        public Point Outer2Inner(Point Outer)
        {
            switch (this.RelativeDirectionType)
            {
                case 0:
                    return new Point(-Outer.Y + position.Y + InnerPosition.X, -Outer.X + position.X + InnerPosition.Y);
                case 1:
                    return new Point(-Outer.X + position.X + InnerPosition.X, Outer.Y - position.Y + InnerPosition.Y);
                case 2:
                    return new Point(Outer.Y - position.Y + InnerPosition.X, Outer.X - position.X + InnerPosition.Y);
                case 3:
                    return new Point(Outer.X - position.X + InnerPosition.X, -Outer.Y + position.Y + InnerPosition.Y);
            }
            return Outer;
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
