using DiscordRfid.Controllers;
using DiscordRfid.Filters;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    [ToolboxItem(false)]
    public abstract class ModelGridDialog<T> : Dialog where T : BaseModel
    {
        public BaseFilter<T> ModelFilter { get; protected set; }

        public ModelToolbox Toolbox { get; private set; }
        private readonly Panel GridPanel;
        public ModelGrid<T> Grid { get; private set; }

        public bool ModelSelected => Grid.IsModelSelected;

        public T SelectedModel => Grid.SelectedModel;

        public ModelGridDialog()
        {
            DialogButtons = DialogFormButtons.Close;
            Text = $"{typeof(T).Name}s";
            Size = new Size(700, 400);

            ModelFilter = Reflector<T>.GetFilter();

            var main = new Panel
            {
                Dock = DockStyle.Fill
            };

            Toolbox = new ModelToolbox(new ModelToolboxGroup(new ModelGridButton[]{
                new ModelGridButton("New", OnModelNewClick),
                new ModelGridButton("Edit", OnModelEditClick) { DisableIfModelNotSelected = true },
                new ModelGridButton("Delete", OnModelDeleteClick) { DisableIfModelNotSelected = true }
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

            Grid.ModelsAdded += OnModelsAddedToGrid;

            GridPanel.Controls.Add(Grid);

            main.Controls.Add(GridPanel);
            main.Controls.Add(Toolbox);

            SuspendLayout();
            AddControl(main);
            ResumeLayout();
            PerformLayout();
        }

        protected virtual void OnModelsAddedToGrid(ICollection<T> models) { }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Toolbox.Buttons.SetEnableStateOfAllModelNotSelected(false);
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

        protected virtual void OnModelNewClick(MouseEventArgs e)
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

        protected virtual void OnModelEditClick(MouseEventArgs e)
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

        protected virtual void OnModelDeleteClick(MouseEventArgs e)
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

    [ToolboxItem(false)]
    public class ModelGridButton : DialogButton
    {
        private bool _disableIfNoModelSelected;

        public bool DisableIfModelNotSelected
        {
            get => _disableIfNoModelSelected;
            set
            {
                if(value != _disableIfNoModelSelected)
                {
                    _disableIfNoModelSelected = true;
                }
            }
        }

        public ModelGridButton(string text, Action<MouseEventArgs> click = null)
            : base(text, click)
        { }
    }

    public class ModelToolboxButtonsCollection : IReadOnlyCollection<ModelGridButton>
    {
        private readonly ICollection<ModelGridButton> Buttons;

        public int Count => Buttons.Count;

        public ModelToolboxButtonsCollection(ICollection<ModelGridButton> buttons)
            => Buttons = buttons;

        public void SetEnableStateOfAllModelNotSelected(bool enabledState)
        {
            foreach (var button in Buttons)
            {
                if (button.DisableIfModelNotSelected)
                {
                    button.Enabled = enabledState;
                }
            }
        }

        public IEnumerator<ModelGridButton> GetEnumerator()
            => Buttons.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Buttons.GetEnumerator();
    }

    [ToolboxItem(false)]
    public class ModelToolbox : DialogFlowPanel
    {
        /// <summary>
        /// All buttons from all groups
        /// </summary>
        public ModelToolboxButtonsCollection Buttons
        {
            get
            {
                var list = new List<ModelGridButton>();

                foreach(ModelToolboxGroup group in Controls)
                {
                    foreach(ModelGridButton button in group.Controls)
                    {
                        list.Add(button);
                    }
                }

                return new ModelToolboxButtonsCollection(list);
            }
        }

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

    [ToolboxItem(false)]
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
}
