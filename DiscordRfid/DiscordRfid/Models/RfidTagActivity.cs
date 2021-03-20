using Microsoft.Data.Sqlite;
using Serilog;
using System;

namespace DiscordRfid.Models
{
    public class RfidTagActivity : BaseModel
    {
        public override string TableName => "RfidTagActivity";

        public int Id;
        public DateTime DateTime;
        public RfidTag Tag;
        public bool Present;

        public override void CreateTable(SqliteConnection connection)
        {
            Log.Debug($"Creating {TableName} table");

            using (var cmd = connection.CreateCommand())
            {
                var tag = new RfidTag();

                cmd.CommandText = $"CREATE TABLE {TableName} (" +
                    "Id            INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "\"DateTime\"  DATETIME NOT NULL DEFAULT (DateTime('now')), " +
                    $"TagId         INTEGER NOT NULL REFERENCES {tag.TableName}(Id) ON UPDATE CASCADE ON DELETE CASCADE, " +
                    "Present       BOOLEAN NOT NULL DEFAULT 0" +
                ")";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
