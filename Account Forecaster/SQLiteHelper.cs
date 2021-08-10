using Account_Forecaster;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo
{
    public class SQLiteHelper
    {
        public static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection("Data Source= database.db; Version = 3; New = True; Compress = True; ");
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        public static void CreateMainDataTable(SQLiteConnection conn)
        {
            try
            {
                SQLiteCommand sqlite_cmd;
                string Createsql = "CREATE TABLE " +
                    "SampleTable " +
                    "(Description VARCHAR(20), " +
                    "IsIncome INT)";
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = Createsql;
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
        }

        public static List<AccountingRowItem> ReadAllData(SQLiteConnection conn)
        {
            var returnList = new List<AccountingRowItem>();

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM SampleTable";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                Console.WriteLine(myreader);
                var dataRowValues = sqlite_datareader.GetValues();
                foreach (var key in dataRowValues.AllKeys)
                {
                    var value = dataRowValues[key];
                    //returnList.Add(myreader);
                }
                
            }
            conn.Close();

            return returnList;
        }

        public static void Insert(SQLiteConnection conn, AccountingRowItem item)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void Update(SQLiteConnection conn, AccountingRowItem newItem)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "";
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void Delete(SQLiteConnection conn, AccountingRowItem item)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "DELETE FROM * WHERE ID=";
            sqlite_cmd.ExecuteNonQuery();
        }
    }
}