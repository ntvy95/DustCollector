using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using SbsSW.SwiPlCs;

namespace AgentOrientedProgramming
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Environment.SetEnvironmentVariable("SWI_HOME_DIR", @"C:\Program Files\swipl\");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        public static double Deg2Rad(double degrees) {
            return Math.PI * degrees / 180.0;
        }
    }
}

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static Point GetCellPositionFromCursorPosition(this TableLayoutPanel tlp)
        {
            Point Position = tlp.PointToClient(Cursor.Position);
            int Column = Convert.ToInt32(Math.Floor(
                         Convert.ToDouble(Position.X) / tlp.Width * tlp.ColumnCount)),
                Row = Convert.ToInt32(Math.Floor(
                      Convert.ToDouble(Position.Y) / tlp.Height * tlp.RowCount));
            return new Point(Column, Row);
        }
        public static void RemoveClickEvent(this Control b)
        {
            FieldInfo f1 = typeof(Control).GetField("EventClick",
                BindingFlags.Static | BindingFlags.NonPublic);
            object obj = f1.GetValue(b);
            PropertyInfo pi = b.GetType().GetProperty("Events",
                BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
            list.RemoveHandler(obj, list[obj]);
        }
    }
}
