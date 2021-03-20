using DiscordRfid.Dtos;
using DiscordRfid.Models;
using Serilog;
using System.Data.Common;

namespace DiscordRfid.Controllers
{
    public class EmployeeController : BaseController<Employee>
    {
        public override string TableName => "Employee";

        public EmployeeController(DbConnection connection)
            : base(connection) { }

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
                        Total = reader.GetInt32ByName("Total"),
                        Present = reader.GetInt32ByName("Present"),
                        Absent = reader.GetInt32ByName("Absent")
                    };
                }
            }
        }
    }
}
