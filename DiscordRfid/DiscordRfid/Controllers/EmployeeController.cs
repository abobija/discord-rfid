using DiscordRfid.Dtos;
using DiscordRfid.Filters;
using DiscordRfid.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DiscordRfid.Controllers
{
    public class EmployeeController : BaseController<Employee>
    {
        public override string TableName => "Employee";

        public EmployeeController(DbConnection connection)
            : base(connection) { }

        public override Employee[] Get(BaseFilter<Employee> filter = null)
        {
            var tagCtrl = new RfidTagController(Connection);

            return GetModels(
                "emp",
                new List<string>{
                    "emp.Id AS emp_Id",
                    "emp.CreatedAt AS emp_CreatedAt",
                    "emp.FirstName AS emp_FirstName",
                    "emp.LastName AS emp_LastName",
                    "emp.Present AS emp_Present",
                    $"(SELECT COUNT(*) FROM {tagCtrl.TableName} tag WHERE tag.EmployeeId = emp.Id) AS emp_TagsCount"
                },
                filter
            );
        }

        public override Employee GetFromDataReader(DbDataReader reader)
        {
            return new Employee
            {
                Id = (int)reader.GetInt32ByName("emp_Id"),
                CreatedAt = (DateTime)reader.GetDateTimeByName("emp_CreatedAt"),
                FirstName = reader.GetStringByName("emp_FirstName"),
                LastName = reader.GetStringByName("emp_LastName"),
                Present = reader.GetBooleanByName("emp_Present"),
                TagsCount = (int) reader.GetInt32ByName("emp_TagsCount")
            };
        }

        public EmployeeCounters GetCounters()
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT " +
                    "COUNT(*) AS Total" +
                    ", IFNULL(SUM(Present), 0) AS Present" +
                    ", IFNULL(SUM(NOT Present), 0) AS Absent" +
                    $" FROM {TableName}";

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    return new EmployeeCounters
                    {
                        Total = (int)reader.GetInt32ByName("Total"),
                        Present = (int)reader.GetInt32ByName("Present"),
                        Absent = (int)reader.GetInt32ByName("Absent")
                    };
                }
            }
        }

        public override Employee Create(Employee employee)
        {
            return Create($"(FirstName, LastName, Present) VALUES(@FirstName, @LastName, @Present)",
                    cmd => cmd
                    .AddParameter("@FirstName", employee.FirstName)
                    .AddParameter("@LastName", employee.LastName)
                    .AddParameter("@Present", employee.Present)
                );
        }

        public override Employee Update(Employee model)
        {
            return Update(model, "FirstName = @FirstName, LastName = @LastName, Present = @Present",
                cmd => cmd
                .AddParameter("@FirstName", model.FirstName)
                .AddParameter("@LastName", model.LastName)
                .AddParameter("@Present", model.Present)
            );
        }

        public override void CreateSchema()
        {
            Log.Debug($"Creating {TableName} table");

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"CREATE TABLE {TableName} (" +
                    "Id         INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "CreatedAt  DATETIME NOT NULL DEFAULT (DateTime('now')), " +
                    "FirstName  TEXT, " +
                    "LastName   TEXT NOT NULL, " +
                    "Present    BOOLEAN NOT NULL DEFAULT 0" +
                ")";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
