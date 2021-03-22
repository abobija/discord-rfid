﻿using DiscordRfid.Filters;
using DiscordRfid.Models;
using Serilog;
using System;
using System.Data.Common;

namespace DiscordRfid.Controllers
{
    public class RfidTagActivityController : BaseController<RfidTagActivity>
    {
        public override string TableName => "RfidTagActivity";

        public RfidTagActivityController(DbConnection connection) 
            : base(connection) { }

        public override RfidTagActivity[] Get(BaseFilter<RfidTagActivity> filter)
        {
            var empCtrl = new EmployeeController(Connection);
            var tagCtr = new RfidTagController(Connection);

            return GetModels(
                new string[] {
                    $"{TableAlias}.Id AS act_Id",
                    $"{TableAlias}.CreatedAt AS act_CreatedAt",
                    $"{TableAlias}.Present AS act_Present",
                    $"{tagCtr.TableAlias}.Id AS tag_Id",
                    $"{tagCtr.TableAlias}.CreatedAt AS tag_CreatedAt",
                    $"{tagCtr.TableAlias}.SerialNumber AS tag_SerialNumber",
                    $"{empCtrl.TableAlias}.Id AS emp_Id",
                    $"{empCtrl.TableAlias}.CreatedAt AS emp_CreatedAt",
                    $"{empCtrl.TableAlias}.FirstName AS emp_FirstName",
                    $"{empCtrl.TableAlias}.LastName AS emp_LastName",
                    $"{empCtrl.TableAlias}.Present AS emp_Present",
                    $"(SELECT COUNT(*) FROM {tagCtr.TableName} _{tagCtr.TableAlias} WHERE _{tagCtr.TableAlias}.EmployeeId = {empCtrl.TableAlias}.Id) AS emp_TagsCount"
                },
                filter,
                new string[]
                {
                    $"LEFT JOIN {tagCtr.TableName} {tagCtr.TableAlias} ON {tagCtr.TableAlias}.Id = {TableAlias}.TagId",
                    $"LEFT JOIN {empCtrl.TableName} {empCtrl.TableAlias} ON {empCtrl.TableAlias}.Id = {tagCtr.TableAlias}.EmployeeId"
                }
            );
        }

        public override RfidTagActivity GetFromDataReader(DbDataReader reader)
        {
            return new RfidTagActivity
            {
                Id = (int)reader.GetInt32ByName("act_Id"),
                CreatedAt = (DateTime)reader.GetDateTimeByName("act_CreatedAt"),
                Present = reader.GetBooleanByName("act_Present"),
                Tag = new RfidTagController(Connection).GetFromDataReader(reader)
            };
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
                ");";

                cmd.ExecuteNonQuery();
            }

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "CREATE TRIGGER ModifyActivityPresentStateTrigger" +
                    " AFTER INSERT" +
                    $" ON {TableName}" +
                    " BEGIN" +
                        $" UPDATE {TableName} SET Present = (" +
                            " SELECT NOT IFNULL((SELECT Present" +
                                    $" FROM {TableName}" +
                                " WHERE TagId = NEW.TagId AND" +
                                    " Id != NEW.Id" +
                                " ORDER BY Id DESC" +
                                " LIMIT 1" +
                            " ), 0)" +
                        " ) WHERE Id = NEW.Id" +
                    " ; END";

                cmd.ExecuteNonQuery();
            }
        }

        public override RfidTagActivity Create(RfidTagActivity activity)
        {
            return Create($"(TagId) VALUES(@TagId)",
                    cmd => cmd
                    .AddParameter("@TagId", activity.Tag.Id)
                );
        }

        public override RfidTagActivity Update(RfidTagActivity activity)
        {
            return Update(activity, "Present = @Present",
                cmd => cmd
                .AddParameter("@Present", activity.Present)
            );
        }
    }
}