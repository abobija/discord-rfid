using DiscordRfid.Services;
using Microsoft.Data.Sqlite;
using Serilog;

namespace DiscordRfid.Controllers
{
    public class RfidTagController : BaseController
    {
        public RfidTagController(SqliteConnection connection)
            : base(connection) { }

        public void CreateTable()
        {
            Log.Debug($"Creating {Database.RfidTagTableName} table");

            using (var cmd = Connection.CreateCommand())
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

        public void CreateActivityTable()
        {
            Log.Debug($"Creating {Database.RfidTagActivityTableName} table");

            using (var cmd = Connection.CreateCommand())
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
