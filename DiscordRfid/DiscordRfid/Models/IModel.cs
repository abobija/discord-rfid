using Microsoft.Data.Sqlite;

namespace DiscordRfid.Models
{
    public interface IModel
    {
        string TableName { get; }
        bool TableExists(SqliteConnection connection);
        void CreateTable(SqliteConnection connection);
        void CreateTableIfNotExists(SqliteConnection connection);
    }
}
