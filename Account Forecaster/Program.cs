using SQLiteDemo;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Account_Forecaster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = SQLiteHelper.CreateConnection();
            SQLiteHelper.CreateTable(sqlite_conn);
            SQLiteHelper.InsertData(sqlite_conn);
            SQLiteHelper.ReadData(sqlite_conn);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
