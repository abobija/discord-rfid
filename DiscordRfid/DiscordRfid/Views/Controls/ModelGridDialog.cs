using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    public class ModelGridButton : DialogButton
    {
        public ModelGridButton(string text) : base(text)
        { }
    }

    public class ModelToolbox : DialogFlowPanel
    {
        public ModelToolbox()
        {
            Dock = DockStyle.Top;
        }

        public ModelToolbox(ICollection<ModelToolboxGroup> groups) : this()
        {
            Controls.AddRange(groups.ToArray());
        }

        public ModelToolbox(ModelToolboxGroup group) : this()
        {
            Add(group);
        }

        public ModelToolbox Add(ModelToolboxGroup group)
        {
            Controls.Add(group);
            return this;
        }

        public ModelToolbox Add(ModelGridButton button)
        {
            return Add(new ModelToolboxGroup(button));
        }
    }

    public class ModelToolboxGroup : FlowLayoutPanel
    {
        public ModelToolboxGroup()
        {
            AutoSize = true;
            Padding = new Padding(5);
        }

        public ModelToolboxGroup(ICollection<ModelGridButton> buttons) : this()
        {
            Controls.AddRange(buttons.ToArray());
        }

        public ModelToolboxGroup(ModelGridButton button) : this()
        {
            Add(button);
        }

        public ModelToolboxGroup Add(ModelGridButton button)
        {
            Controls.Add(button);
            return this;
        }
    }

    public class ModelGridDialog<T> : Dialog where T : BaseModel
    {
        protected ModelToolbox Toolbox { get; private set; }
        private readonly Panel GridPanel;
        private readonly ModelGrid<T> Grid;

        private readonly ModelGridButton ButtonModelNew;
        private readonly ModelGridButton ButtonModelEdit;
        private readonly ModelGridButton ButtonModelDelete;

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

            ButtonModelNew = new ModelGridButton("New");
            ButtonModelEdit = new ModelGridButton("Edit");
            ButtonModelDelete = new ModelGridButton("Delete");

            ButtonModelNew.Click += OnButtonModelNewClick;
            ButtonModelEdit.Click += OnButtonModelEditClick;
            ButtonModelDelete.Click += OnButtonModelDeleteClick;

            Toolbox = new ModelToolbox(new ModelToolboxGroup(new ModelGridButton[]{
                ButtonModelNew, ButtonModelEdit, ButtonModelDelete
            }));

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
            main.Controls.Add(Toolbox);

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
