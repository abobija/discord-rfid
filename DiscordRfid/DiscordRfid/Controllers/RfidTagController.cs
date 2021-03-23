using DiscordRfid.Filters;
using DiscordRfid.Models;
using Serilog;
using System;
using System.Collections.Generic;
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
            var empCtrl = new EmployeeController(Connection);

            return GetModels(
                new string[] {
                    $"{TableAlias}.Id AS tag_Id",
                    $"{TableAlias}.CreatedAt AS tag_CreatedAt",
                    $"{TableAlias}.SerialNumber AS tag_SerialNumber",
                    $"{empCtrl.TableAlias}.Id AS emp_Id",
                    $"{empCtrl.TableAlias}.CreatedAt AS emp_CreatedAt",
                    $"{empCtrl.TableAlias}.FirstName AS emp_FirstName",
                    $"{empCtrl.TableAlias}.LastName AS emp_LastName",
                    $"{empCtrl.TableAlias}.Present AS emp_Present",
                    $"(SELECT COUNT(*) FROM {TableName} _{TableAlias} WHERE _{TableAlias}.EmployeeId = {empCtrl.TableAlias}.Id) AS emp_TagsCount"
                },
                filter,
                new string[]
                {
                    $"LEFT JOIN {empCtrl.TableName} {empCtrl.TableAlias} ON {empCtrl.TableAlias}.Id = {TableAlias}.EmployeeId"
                }
            );
        }

        public RfidTag GetBySerialNumber(ulong serialNumber)
        {
            var results = Get(new RfidTagFilter
            {
                Where = $"{TableAlias}.SerialNumber = {serialNumber}"
            });

            return results.Length > 0 ? results[0] : null;
        }

        public override RfidTag GetFromDataReader(DbDataReader reader)
        {
            return new RfidTag
            {
                Id = (int)reader.GetInt32ByName("tag_Id"),
                CreatedAt = (DateTime)reader.GetDateTimeByName("tag_CreatedAt"),
                SerialNumber = Convert.ToUInt64(reader.GetValue(reader.GetOrdinal("tag_SerialNumber"))),
                Employee = new EmployeeController(Connection).GetFromDataReader(reader)
            };
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
            return Update(tag, "SerialNumber = @SerialNumber",
                cmd => cmd
                .AddParameter("@SerialNumber", tag.SerialNumber)
            );
        }
    }
}
