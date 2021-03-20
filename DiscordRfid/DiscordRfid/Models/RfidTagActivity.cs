using Microsoft.Data.Sqlite;
using Serilog;
using System;

namespace DiscordRfid.Models
{
    public class RfidTagActivity : BaseModel
    {
        public override string TableName => "RfidTagActivity";

        public int Id;
        public RfidTag Tag;
        public DateTime DateTime;
        public bool Present;

        public override void CreateTable(SqliteConnection connection)
        {
            Log.Debug($"Creating {TableName} table");

            using (var cmd = connection.CreateCommand())
            {

            }
        }
    }
}
