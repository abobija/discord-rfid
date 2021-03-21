﻿using DiscordRfid.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DiscordRfid.Controllers
{
    public abstract class BaseController<T> where T : BaseModel
    {
        protected DbConnection Connection;
        public abstract string TableName { get; }

        public static event Action<T> ModelCreated;
        public static event Action<T> ModelUpdated;
        public static event Action<T> ModelDeleted;

        public BaseController(DbConnection connection)
        {
            Connection = connection;
        }

        public static BaseController<T> FromModelType(DbConnection connection)
        {
            var ctrlType = Assembly.GetExecutingAssembly()
                    .DefinedTypes
                    .FirstOrDefault(t => t.BaseType == typeof(BaseController<T>)
                            && t.BaseType.GenericTypeArguments[0] == typeof(T)
                        );

            if (ctrlType == null)
            {
                throw new TypeLoadException($"Unable to find controller for {typeof(T).Name} model");
            }

            return Activator.CreateInstance(ctrlType, connection) as BaseController<T>;
        }

        public virtual T GetModelFromDataReader(DbDataReader reader)
        {
            throw new NotImplementedException($"Method FromDataReader not implemented for controller of model {typeof(T).Name}");
        }

        public T[] Get(string orderBy = null)
        {
            var list = new List<T>();

            var queryBuilder = new StringBuilder().AppendLine($"SELECT * FROM {TableName}");

            if(orderBy != null)
            {
                queryBuilder.AppendLine($"ORDER BY {orderBy}");
            }

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = queryBuilder.ToString();

                using (var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(GetModelFromDataReader(reader));
                    }
                }
            }

            return list.ToArray();
        }

        public virtual T GetById(int id)
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT * FROM {TableName} Where Id = @Id";
                cmd.AddParameter("@Id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? GetModelFromDataReader(reader) : null;
                }
            }
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
                ModelUpdated?.Invoke(newModel);
                return newModel;
            }

            return null;
        }

        public virtual T Create(T model)
        {
            throw new NotImplementedException($"Method Save not implemented for controller of model {typeof(T).Name}");
        }

        public virtual T Update(T model)
        {
            throw new NotImplementedException($"Method Update not implemented for controller of model {typeof(T).Name}");
        }

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
