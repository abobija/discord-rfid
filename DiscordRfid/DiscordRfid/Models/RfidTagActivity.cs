using Microsoft.Data.Sqlite;
using Serilog;
using System;

namespace DiscordRfid.Models
{
    public class RfidTagActivity
    {
        public int Id;
        public DateTime DateTime;
        public RfidTag Tag;
        public bool Present;

        public static void CreateTable(SqliteConnection connection)
        {
            Log.Debug($"Creating {Database.RfidTagActivityTableName} table");

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"CREATE TABLE {Database.RfidTagActivityTableName} (" +
                    "Id            INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "\"DateTime\"  DATETIME NOT NULL DEFAULT (DateTime('now')), " +
                    $"TagId        INTEGER NOT NULL REFERENCES {Database.RfidTagTableName}(Id) ON UPDATE CASCADE ON DELETE CASCADE, " +
                    "Present       BOOLEAN NOT NULL DEFAULT 0" +
                ")";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
