using Serilog;
using System.Data.Common;

namespace DiscordRfid.Controllers
{
    public class RfidTagController : BaseController
    {
        public override string TableName => "RfidTag";
        public string ActivityTableName => "RfidTagActivity";

        public RfidTagController(DbConnection connection)
            : base(connection) { }

        public override bool SchemaExists()
        {
            return TableExists(TableName) && TableExists(ActivityTableName);
        }

        public override void CreateSchema()
        {
            if(!TableExists(TableName)) // maybe tag exists but activity don't
            {
                Log.Debug($"Creating {TableName} table");

                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = $"CREATE TABLE {TableName} (" +
                        "Id            INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "CreatedAt     DATETIME NOT NULL DEFAULT (DateTime('now')), " +
                        "SerialNumber  INTEGER NOT NULL, " +
                        $"EmployeeId   INTEGER NOT NULL REFERENCES {ActivityTableName}(Id) ON UPDATE CASCADE ON DELETE CASCADE" +
                    ")";

                    cmd.ExecuteNonQuery();
                }
            }
            
            if(!TableExists(ActivityTableName)) // maybe activity exists but rfid don't
            {
                Log.Debug($"Creating {ActivityTableName} table");

                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = $"CREATE TABLE {ActivityTableName} (" +
                        "Id            INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "\"DateTime\"  DATETIME NOT NULL DEFAULT (DateTime('now')), " +
                        $"TagId        INTEGER NOT NULL REFERENCES {TableName}(Id) ON UPDATE CASCADE ON DELETE CASCADE, " +
                        "Present       BOOLEAN NOT NULL DEFAULT 0" +
                    ")";

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
