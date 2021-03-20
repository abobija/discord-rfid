using Microsoft.Data.Sqlite;
using Serilog;
using System;

namespace DiscordRfid.Models
{
    public class RfidTag
    {
        public int Id;
        public DateTime CreatedAt;
        public ulong SerialNumber;
        public Employee Employee;

        public static void CreateTable(SqliteConnection connection)
        {
            Log.Debug($"Creating {Database.RfidTagTableName} table");

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"CREATE TABLE {Database.RfidTagTableName} (" +
                    "Id            INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "CreatedAt     DATETIME NOT NULL DEFAULT (DateTime('now')), " +
                    "SerialNumber  INTEGER NOT NULL, " +
                    $"EmployeeId   INTEGER NOT NULL REFERENCES {Database.EmployeeTableName}(Id) ON UPDATE CASCADE ON DELETE CASCADE" +
                ")";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
