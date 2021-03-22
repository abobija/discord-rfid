using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    public class ModelGridDialog<T> : Dialog where T : BaseModel
    {
        private readonly DialogFlowPanel CrudPanel;
        private readonly Panel GridPanel;
        private readonly ModelGrid<T> Grid;

        private readonly DialogButton ButtonModelNew;
        private readonly DialogButton ButtonModelEdit;
        private readonly DialogButton ButtonModelDelete;

        public bool ModelSelected => Grid.ModelSelected;

        public T SelectedModel => Grid.SelectedModel;

        public ModelGridDialog()
        {
            DialogButtons = DialogFormButtons.Close;
            Text = $"{typeof(T).Name}s";
            Size = new Size(700, 400);

            var main = new Panel
            {
                Dock = DockStyle.Fill
            };

            CrudPanel = new DialogFlowPanel
            {
                Dock = DockStyle.Top
            };

            ButtonModelNew = new DialogButton("New");
            ButtonModelEdit = new DialogButton("Edit");
            ButtonModelDelete = new DialogButton("Delete");

            ButtonModelNew.Click += OnButtonModelNewClick;
            ButtonModelEdit.Click += OnButtonModelEditClick;
            ButtonModelDelete.Click += OnButtonModelDeleteClick;

            CrudPanel.Controls.Add(ButtonModelNew);
            CrudPanel.Controls.Add(ButtonModelEdit);
            CrudPanel.Controls.Add(ButtonModelDelete);

            GridPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };

            Grid = new ModelGrid<T>
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };

            GridPanel.Controls.Add(Grid);

            main.Controls.Add(GridPanel);
            main.Controls.Add(CrudPanel);

            SuspendLayout();
            AddControl(main);
            ResumeLayout();
            PerformLayout();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Grid.Reload();

            BaseController<T>.ModelCreated += OnModelCreated;
            BaseController<T>.ModelUpdated += OnModelUpdated;
            BaseController<T>.ModelDeleted += OnModelDeleted;
        }

        private void OnModelCreated(T model) => Grid.Reload();
        private void OnModelUpdated(T oldState, T newState) => Grid.Reload();
        private void OnModelDeleted(T model) => Grid.Reload();

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            BaseController<T>.ModelCreated -= OnModelCreated;
            BaseController<T>.ModelUpdated -= OnModelUpdated;
            BaseController<T>.ModelDeleted -= OnModelDeleted;
        }

        protected virtual void OnButtonModelNewClick(object sender, EventArgs e)
        {
            try
            {
                using (var dlg = Reflector<T>.GetView())
                {
                    dlg.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                this.Error(ex);
            }
        }

        protected virtual void OnButtonModelEditClick(object sender, EventArgs e)
        {
            if (!ModelSelected)
                return;

            try
            {
                using (var dlg = Reflector<T>.GetView(SelectedModel))
                {
                    dlg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }
        }

        protected virtual void OnButtonModelDeleteClick(object sender, EventArgs e)
        {
            if (!ModelSelected)
                return;

            var type = typeof(T);
            var model = SelectedModel;

            if(MessageBox.Show(
                $"Are you sure you want to delete selected {type.Name}?{Environment.NewLine}{Environment.NewLine}{type.Name}: {model}",
                $"Delete {type.Name}?",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (var con = Database.Instance.CreateConnection())
                {
                    con.Open();
                    Reflector<T>.GetController(con).Delete(model);
                }
            }
            catch(Exception ex)
            {
                this.Error(ex);
            }
        }
    }
}
