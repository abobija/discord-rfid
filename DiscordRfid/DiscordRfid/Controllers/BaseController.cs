using System.Data.Common;

namespace DiscordRfid.Controllers
{
    public abstract class BaseController
    {
        protected DbConnection Connection;
        public abstract string TableName { get; }

        public BaseController(DbConnection connection)
        {
            Connection = connection;
        }

        protected bool TableExists(string tableName)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT name FROM sqlite_master WHERE name='{tableName}'";
                var name = cmd.ExecuteScalar();
                return name != null && name.ToString() == tableName;
            }
        }

        public virtual bool SchemaExists()
        {
            return TableExists(TableName);
        }

        public abstract void CreateSchema();

        public void CreateSchemaIfNotExists()
        {
            if(!SchemaExists())
            {
                CreateSchema();
            }
        }
    }
}
