using Microsoft.Data.Sqlite;
using Serilog;
using System;

namespace DiscordRfid.Models
{
    public class RfidTag : BaseModel
    {
        public override string TableName => "RfidTag";

        public int Id;
        public DateTime CreatedAt;
        public ulong SerialNumber;
        public Employee Employee;

        public override void CreateTable(SqliteConnection connection)
        {
            Log.Debug($"Creating {TableName} table");

            using (var cmd = connection.CreateCommand())
            {
                var emp = new Employee();

                cmd.CommandText = $"CREATE TABLE {TableName} (" +
                    "Id            INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "CreatedAt     DATETIME NOT NULL DEFAULT (DateTime('now')), " +
                    "SerialNumber  INTEGER NOT NULL, " +
                    $"EmployeeId    INTEGER NOT NULL REFERENCES {emp.TableName}(Id) ON UPDATE CASCADE ON DELETE CASCADE" +
                ")";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
