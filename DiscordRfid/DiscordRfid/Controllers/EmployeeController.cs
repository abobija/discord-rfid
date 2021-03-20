using DiscordRfid.Dtos;
using DiscordRfid.Services;
using Microsoft.Data.Sqlite;
using Serilog;

namespace DiscordRfid.Controllers
{
    public class EmployeeController : BaseController
    {
        public EmployeeController(SqliteConnection connection)
            : base(connection) { }

        public EmployeeCounters GetCounters()
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT " +
                    "COUNT(*) AS Total" +
                    ", IFNULL(SUM(Present), 0) AS Present" +
                    ", IFNULL(SUM(NOT Present), 0) AS Absent" +
                    $" FROM {Database.EmployeeTableName}";

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

        public void CreateTable()
        {
            Log.Debug($"Creating {Database.EmployeeTableName} table");

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"CREATE TABLE {Database.EmployeeTableName} (" +
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
