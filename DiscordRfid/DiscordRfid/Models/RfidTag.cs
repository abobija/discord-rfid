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

            }
        }
    }
}
