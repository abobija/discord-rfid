using DiscordRfid.Filters;
using DiscordRfid.Models;
using Serilog;
using System;
using System.Data.Common;

namespace DiscordRfid.Controllers
{
    public class RfidTagController : BaseController<RfidTag>
    {
        public override string TableName => "RfidTag";

        public RfidTagController(DbConnection connection)
            : base(connection) { }

        public override RfidTag[] Get(BaseFilter<RfidTag> filter)
        {
            throw new NotImplementedException();
        }

        public override void CreateSchema()
        {
            Log.Debug($"Creating {TableName} table");

            var empCtrl = new EmployeeController(Connection);

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"CREATE TABLE {TableName} (" +
                    "   Id            INTEGER  PRIMARY KEY AUTOINCREMENT" +
                    ",  CreatedAt     DATETIME NOT NULL DEFAULT (DateTime('now'))" +
                    ",  SerialNumber  INTEGER  NOT NULL" +
                    $", EmployeeId    INTEGER  NOT NULL REFERENCES {empCtrl.TableName}(Id) ON UPDATE CASCADE ON DELETE CASCADE" +
                    ",  UNIQUE(SerialNumber)" +
                ")";

                cmd.ExecuteNonQuery();
            }
        }

        public override RfidTag GetFromDataReader(DbDataReader reader)
        {
            return new RfidTag
            {
                Id = (int)reader.GetInt32ByName("Id"),
                CreatedAt = (DateTime)reader.GetDateTimeByName("CreatedAt"),
                SerialNumber = (ulong) reader.GetInt64ByName("SerialNumber")
            };
        }

        public override RfidTag Create(RfidTag tag)
        {
            return Create($"(SerialNumber, EmployeeId) VALUES(@SerialNumber, @EmployeeId)",
                    cmd => cmd
                    .AddParameter("@SerialNumber", tag.SerialNumber)
                    .AddParameter("@EmployeeId", tag.Employee.Id)
                );
        }

        public override RfidTag Update(RfidTag tag)
        {
            return Update(tag, "SerialNumber = @SerialNumber, EmployeeId = @EmployeeId",
                cmd => cmd
                .AddParameter("@SerialNumber", tag.SerialNumber)
                .AddParameter("@EmployeeId", tag.Employee.Id)
            );
        }
    }
}
