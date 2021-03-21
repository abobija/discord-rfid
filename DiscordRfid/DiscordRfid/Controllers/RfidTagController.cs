using DiscordRfid.Models;
using Serilog;
using System.Data.Common;

namespace DiscordRfid.Controllers
{
    public class RfidTagController : BaseController<RfidTag>
    {
        public override string TableName => "RfidTag";

        public RfidTagController(DbConnection connection)
            : base(connection) { }

        public override void CreateSchema()
        {
            Log.Debug($"Creating {TableName} table");

            var actCtrl = new RfidTagActivityController(Connection);

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"CREATE TABLE {TableName} (" +
                    "   Id            INTEGER  PRIMARY KEY AUTOINCREMENT" +
                    ",  CreatedAt     DATETIME NOT NULL DEFAULT (DateTime('now'))" +
                    ",  SerialNumber  INTEGER  NOT NULL" +
                    $", EmployeeId    INTEGER  NOT NULL REFERENCES {actCtrl.TableName}(Id) ON UPDATE CASCADE ON DELETE CASCADE" +
                    ",  UNIQUE(EmployeeId, SerialNumber)" +
                ")";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
