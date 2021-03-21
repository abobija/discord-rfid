using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;

namespace DiscordRfid.Views.Controls
{
    public class ModelForm<T> : DialogForm where T : BaseModel
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

        protected override void OnDialogSave(object sender, EventArgs e)
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
                base.OnDialogSave(sender, e);
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
                    var ctrl = BaseController<T>.FromModelType(con);
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
    }
}
