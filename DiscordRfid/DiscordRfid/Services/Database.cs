using DiscordRfid.Controllers;
using DiscordRfid.Models;
using Microsoft.Data.Sqlite;
using Serilog;
using System;
using System.IO;

namespace DiscordRfid.Services
{
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

        protected void InitSchema()
        {
            using (var con = CreateConnection())
            {
                con.Open();

                if (!TableExists(EmployeeTableName, con))
                {
                    new EmployeeController(con).CreateTable();
                }

                var rfidCtrl = new RfidTagController(con);

                if (!TableExists(RfidTagTableName, con))
                {
                    rfidCtrl.CreateTable();
                }

                if (!TableExists(RfidTagActivityTableName, con))
                {
                    rfidCtrl.CreateActivityTable();
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
