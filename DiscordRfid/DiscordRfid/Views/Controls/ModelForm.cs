using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    public class ModelForm<T> : Form where T : BaseModel
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

        protected Button BtnModelSave;
        protected Button BtnModelCancel;

        public ModelForm()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;

            BtnModelSave = new Button
            {
                Text = "Save",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };

            BtnModelCancel = new Button
            {
                Text = "Cancel",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };

            BtnModelSave.Click += OnModelSaveClick;
            BtnModelCancel.Click += OnModelCancelClick;

            var buttonsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                BackColor = Color.Gainsboro,
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(5)
            };

            buttonsPanel.Controls.Add(BtnModelCancel);
            buttonsPanel.Controls.Add(BtnModelSave);

            buttonsPanel.SuspendLayout();
            Controls.Add(buttonsPanel);
            buttonsPanel.ResumeLayout();
            buttonsPanel.PerformLayout();
        }

        private void OnModelCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void OnModelSaveClick(object sender, EventArgs e)
        {
            try
            {
                var model = ConstructModel();
                model.Validate();
                SaveModel(model);
                DialogResult = DialogResult.OK;
            }
            catch(Exception ex)
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
