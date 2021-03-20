using DiscordRfid.Models;
using Microsoft.Data.Sqlite;
using Serilog;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace DiscordRfid
{
    public class Database
    {
        private static Database Singletone;
        private bool Inited = false;

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

            try
            {
                InitSchema();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Fail to boot up database");
            }
        }

        protected void InitSchema()
        {
            using (var con = CreateConnection())
            {
                con.Open();

                new Employee().CreateTableIfNotExists(con);
                new RfidTag().CreateTableIfNotExists(con);
                new RfidTagActivity().CreateTableIfNotExists(con);
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
            try
            {
                return new SqliteConnection(ConnectionString);
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Fail to create database connection");
                throw ex;
            }
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
