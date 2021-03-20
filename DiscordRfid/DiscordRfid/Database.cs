using DiscordRfid.Models;
using Microsoft.Data.Sqlite;
using Serilog;
using System;
using System.IO;

namespace DiscordRfid
{
    public class EmployeeCounters
    {
        public int Total;
        public int Present;
        public int Absent;
    }

    public class Database
    {
        public static string EmployeeTableName = "Employee";
        public static string RfidTagTableName = "RfidTag";
        public static string RfidTagActivityTableName = "RfidTagActivity";

        private static Database Singletone;
        public bool Inited { get; private set; } = false;

        protected string ConnectionString => $"Data Source={Path.Combine(Environment.CurrentDirectory, "rfid.db")}";

        protected Database()
        {
            Log.Debug("Database contructor");
        }

        public void Init()
        {
            Log.Debug("Initializing database");

            if (Inited)
            {
                Log.Debug("Database has already initialized. Ignoring");
                return;
            }
            
            InitSchema();
        }

        public EmployeeCounters GetEmployeeCounters()
        {
            using (var con = CreateConnection())
            using (var cmd = con.CreateCommand())
            {
                con.Open();

                cmd.CommandText = "SELECT " +
                    "COUNT(*) AS Total" +
                    ", IFNULL(SUM(Present), 0) AS Present" +
                    ", IFNULL(SUM(NOT Present), 0) AS Absent" +
                    $" FROM {EmployeeTableName}";

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    return new EmployeeCounters
                    {
                        Total = reader.GetInt32ByName("Total"),
                        Present = reader.GetInt32ByName("Present"),
                        Absent = reader.GetInt32ByName("Absent")
                    };
                }
            }
        }

        protected void InitSchema()
        {
            using (var con = CreateConnection())
            {
                con.Open();

                if (!TableExists(EmployeeTableName, con))
                {
                    Employee.CreateTable(con);
                }

                if (!TableExists(RfidTagTableName, con))
                {
                    RfidTag.CreateTable(con);
                }

                if (!TableExists(RfidTagActivityTableName, con))
                {
                    RfidTagActivity.CreateTable(con);
                }
            }
        }

        public bool TableExists(string tableName, SqliteConnection connection)
        {
            using(var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT name FROM sqlite_master WHERE name='{tableName}'";
                var name = cmd.ExecuteScalar();
                return name != null && name.ToString() == tableName;
            }
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        public static Database Instance
        {
            get
            {
                if(Singletone == null)
                {
                    Singletone = new Database();
                }

                return Singletone;
            }
        }
    }
}
