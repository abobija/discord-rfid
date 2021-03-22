using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.Windows.Forms;

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

        protected override void OnDialogSaveClick(MouseEventArgs e)
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
                base.OnDialogSaveClick(e);
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }
        }

        protected virtual void PopulateForm() { }

        protected virtual void SaveModel(T validatedModel)
        {
            using (var con = Database.Instance.CreateConnection())
            {
                con.Open();
                var ctrl = Reflector<T>.GetController(con).SetState(Model);
                NewModel = Model == null ? ctrl.Create(validatedModel) : ctrl.Update(validatedModel);
            }
        }

        protected virtual T ConstructModel()
        {
            throw new NotImplementedException();
        }
    }
}
