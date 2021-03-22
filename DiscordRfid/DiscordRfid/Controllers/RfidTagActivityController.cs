using DiscordRfid.Filters;
using DiscordRfid.Models;
using Serilog;
using System.Data.Common;

namespace DiscordRfid.Controllers
{
    public class RfidTagActivityController : BaseController<RfidTagActivity>
    {
        public override string TableName => "RfidTagActivity";

        public RfidTagActivityController(DbConnection connection) 
            : base(connection) { }

        public override RfidTagActivity[] Get(IFilter<RfidTagActivity> filter)
        {
            throw new System.NotImplementedException();
        }

        public override void CreateSchema()
        {
            Log.Debug($"Creating {TableName} table");

            var tagCtrl = new RfidTagController(Connection);

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"CREATE TABLE {TableName} (" +
                    "   Id         INTEGER  PRIMARY KEY AUTOINCREMENT" +
                    @", CreatedAt  DATETIME NOT NULL DEFAULT (DateTime('now'))" +
                    $", TagId      INTEGER  NOT NULL REFERENCES {tagCtrl.TableName}(Id) ON UPDATE CASCADE ON DELETE CASCADE" +
                    ",  Present    BOOLEAN  NOT NULL DEFAULT 0" +
                ")";

                cmd.ExecuteNonQuery();
            }
        }

        public override RfidTagActivity Create(RfidTagActivity model)
        {
            throw new System.NotImplementedException();
        }

        public override RfidTagActivity Update(RfidTagActivity model)
        {
            throw new System.NotImplementedException();
        }
    }
}
