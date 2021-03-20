using Microsoft.Data.Sqlite;
using Serilog;
using System;

namespace DiscordRfid.Models
{
    public class Employee : BaseModel
    {
        public override string TableName => "Employee";

        public int Id;
        public DateTime CreatedAt;
        public string FirstName;
        public string LastName;
        public bool Present;

        public override void CreateTable(SqliteConnection connection)
        {
            Log.Debug($"Creating {TableName} table");

            using (var cmd = connection.CreateCommand())
            {

            }
        }
    }
}
