using DiscordRfid.Controllers;
using DiscordRfid.Filters;
using DiscordRfid.Models;
using DiscordRfid.Views.Controls;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace DiscordRfid.Services
{
    public static class Reflector<T> where T : BaseModel
    {
        public static BaseController<T> GetController(DbConnection connection)
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

        public static ModelDialog<T> GetView(T model = null)
        {
            var dialogType = Assembly.GetExecutingAssembly()
                    .DefinedTypes
                    .FirstOrDefault(t => t.BaseType == typeof(ModelDialog<T>)
                            && t.BaseType.GenericTypeArguments[0] == typeof(T)
                        );

            if (dialogType == null)
            {
                throw new TypeLoadException($"Unable to find ModelDialog for {typeof(T).Name} model");
            }

            var dialog =  Activator.CreateInstance(dialogType, model) as ModelDialog<T>;

            return dialog;
        }

        public static BaseFilter<T> GetFilter()
        {
            var filterType = Assembly.GetExecutingAssembly()
                    .DefinedTypes
                    .FirstOrDefault(t => t.BaseType == typeof(BaseFilter<T>)
                            && t.BaseType.GenericTypeArguments[0] == typeof(T)
                        );

            return filterType == null ? null : Activator.CreateInstance(filterType) as BaseFilter<T>;
        }
    }
}
