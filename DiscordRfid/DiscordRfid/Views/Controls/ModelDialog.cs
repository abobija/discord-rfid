using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.Linq;
using System.Reflection;

namespace DiscordRfid.Views.Controls
{
    public class ModelDialog<T> : Dialog where T : BaseModel
    {
        private T _model;

        public T Model
        {
            get => _model;
            protected set
            {
                if (_model != value)
                {
                    _model = value;
                    PopulateForm();
                }
            }
        }

        public T NewModel { get; private set; }

        protected override void OnButtonSaveClick(object sender, EventArgs e)
        {
            try
            {
                var model = ConstructModel();
                if(Model != null)
                {
                    model.Id = Model.Id;
                }
                model.Validate();
                SaveModel(model);
                base.OnButtonSaveClick(sender, e);
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }
        }

        protected virtual void PopulateForm() { }

        protected virtual void SaveModel(T validatedModel)
        {
            try
            {
                using (var con = Database.Instance.CreateConnection())
                {
                    con.Open();
                    var ctrl = BaseController<T>.FromModelType(con).SetState(Model);
                    NewModel = Model == null ? ctrl.Create(validatedModel) : ctrl.Update(validatedModel);
                }
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }
        }

        protected virtual T ConstructModel()
        {
            throw new NotImplementedException();
        }

        public static ModelDialog<T> FromModelType(T model = null)
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

            return Activator.CreateInstance(dialogType, model) as ModelDialog<T>;
        }
    }
}
