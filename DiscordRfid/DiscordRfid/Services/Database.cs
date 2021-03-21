using DiscordRfid.Controllers;
using Microsoft.Data.Sqlite;
using Serilog;
using System;
using System.IO;

namespace DiscordRfid.Services
{
    public class Database
    {
        protected string ConnectionString => $"Data Source={Path.Combine(Environment.CurrentDirectory, "rfid.db")}";

        protected Database()
        {
            Log.Debug("Database contructor");
            Init();
        }

        private void Init()
        {
            Log.Debug("Initializing database");
            InitSchema();
        }

        protected void InitSchema()
        {
            using (var con = CreateConnection())
            {
                con.Open();

                new EmployeeController(con).CreateSchemaIfNotExists();
                new RfidTagController(con).CreateSchemaIfNotExists();
            }
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        #region Singletone
        private static Database _instance;

        public static Database Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Database();
                }

                return _instance;
            }
        }
        #endregion
    }
}
