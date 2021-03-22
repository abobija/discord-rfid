using DiscordRfid.Filters;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace DiscordRfid.Controllers
{
    public abstract class BaseController<T> where T : BaseModel
    {
        protected DbConnection Connection;
        public abstract string TableName { get; }

        public T State { get; private set; }

        public static event Action<T> ModelCreated;
        public static event Action<T, T> ModelUpdated; // OldState, NewState
        public static event Action<T> ModelDeleted;

        public BaseController(DbConnection connection)
        {
            Connection = connection;
        }

        public BaseController<T> SetState(T state)
        {
            State = state;
            return this;
        }

        public virtual T GetFromDataReader(DbDataReader reader)
        {
            throw new NotImplementedException($"Method GetFromDataReader not implemented for controller of model {typeof(T).Name}");
        }

        protected T[] GetModels(string tableAlias, ICollection<string> sqlSelects, BaseFilter<T> filter = null)
        {
            var list = new List<T>();

            var query = new StringBuilder()
                .AppendLine($"SELECT");

            query.AppendLine(string.Join(",", sqlSelects));
            query.AppendLine($"FROM {TableName} {tableAlias}");

            if(filter != null)
            {
                if(filter.Where != null)
                {
                    query.AppendLine($"WHERE {filter.Where}");
                }

                if(filter.OrderBy != null)
                {
                    query.AppendLine($"ORDER BY {filter.OrderBy}");
                }
            }

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = query.ToString();

                using (var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(GetFromDataReader(reader));
                    }
                }
            }

            return list.ToArray();
        }

        public T GetById(int id)
        {
            var filter = Reflector<T>.GetFilter();
            filter.Where = $"Id = {id}";
            return Get(filter)?[0];
        }

        protected T Create(string sql, Action<DbCommand> addParameters)
        {
            int lastId;

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"INSERT INTO {TableName}{sql}; SELECT last_insert_rowid()";
                addParameters(cmd);
                lastId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            T newModel = GetById(lastId);
            ModelCreated?.Invoke(newModel);

            return newModel;
        }

        protected T Update(T model, string sql, Action<DbCommand> addParameters)
        {
            T oldModel = model;

            int rows = 0;
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"UPDATE {TableName} SET {sql} WHERE Id = @Id";
                cmd.AddParameter("@Id", model.Id);
                addParameters(cmd);
                rows = cmd.ExecuteNonQuery();
            }

            if (rows > 0)
            {
                T newModel = GetById(model.Id);
                ModelUpdated?.Invoke(State, newModel);
                return newModel;
            }

            return null;
        }

        public abstract T[] Get(BaseFilter<T> filter = null);

        public abstract T Create(T model);

        public abstract T Update(T model);

        public virtual int Delete(T model)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"DELETE FROM {TableName} WHERE Id = @Id";
                cmd.AddParameter("@Id", model.Id);
                int rows = cmd.ExecuteNonQuery();

                if(rows > 0)
                {
                    ModelDeleted?.Invoke(model);
                }

                return rows;
            }
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
