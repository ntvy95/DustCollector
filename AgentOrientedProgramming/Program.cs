using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        //public static Point GetCellPositionFrom(this String str)
        //{
        //    return str.Split(new char[] { ' ', '.', '?' },
        //                     StringSplitOptions.RemoveEmptyEntries).Length;
        //}
    }
}
