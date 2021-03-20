using Microsoft.Data.Sqlite;

namespace DiscordRfid.Models
{
    public abstract class BaseModel : IModel
    {
        public abstract string TableName { get; }

        public abstract void CreateTable(SqliteConnection connection);

        public void CreateTableIfNotExists(SqliteConnection connection)
        {
            if(!TableExists(connection))
            {
                CreateTable(connection);
            }
        }

        public bool TableExists(SqliteConnection connection)
        {
            return Database.Instance.TableExists(TableName, connection);
        }
    }
}
