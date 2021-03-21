﻿using DiscordRfid.Models;
using DiscordRfid.Services;
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

        public virtual T FromDataReader(DbDataReader reader)
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
                        list.Add(FromDataReader(reader));
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
                    return reader.Read() ? FromDataReader(reader) : null;
                }
            }
        }

        protected T Create(string sql, Action<DbCommand> addParameters)
        {
            int lastId;

            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"{sql}; SELECT last_insert_rowid()";
                addParameters(cmd);
                lastId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            T newModel = GetById(lastId);
            Database.Instance.FireModelCreated(newModel);

            return newModel;
        }

        public virtual T Create(T model)
        {
            throw new NotImplementedException($"Method Save not implemented for controller of model {typeof(T).Name}");
        }

        public virtual T Update(T model)
        {
            throw new NotImplementedException($"Method Update not implemented for controller of model {typeof(T).Name}");
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
