using Microsoft.Data.Sqlite;
using Serilog;
using System;

namespace DiscordRfid.Models
{
    public class Employee
    {
        public int Id;
        public DateTime CreatedAt;
        public string FirstName;
        public string LastName;
        public bool Present;

        public static void CreateTable(SqliteConnection connection)
        {
            Log.Debug($"Creating {Database.EmployeeTableName} table");

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"CREATE TABLE {Database.EmployeeTableName} (" +
                    "Id         INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "CreatedAt  DATETIME NOT NULL DEFAULT (DateTime('now')), " +
                    "FirstName  TEXT, " +
                    "LastName   TEXT NOT NULL, " +
                    "Present    BOOLEAN NOT NULL DEFAULT 0" +
                ")";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
