using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    [ToolboxItem(false)]
    public class ModelGrid<T> : DataGridView where T : BaseModel
    {
        public bool IsModelSelected => SelectedRows.Count > 0;

        public T SelectedModel
        {
            get
            {
                var srows = SelectedRows;

                if (srows.Count <= 0)
                    return null;

                return srows[0].DataBoundItem as T;
            }
        }

        public ModelGrid()
        {
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            BorderStyle = BorderStyle.None;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MultiSelect = false;
            ReadOnly = true;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            BackgroundColor = Color.White;
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            ParentGridDialog.Toolbox.Buttons.SetEnableStateOfAllModelNotSelected(IsModelSelected);
        }

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            if(Columns[e.ColumnIndex].ValueType == typeof(DateTime) && e.Value != null)
            {
                e.Value = ((DateTime) e.Value).ToLocalTime();
            }
        }

        public void Reload()
        {
            try
            {
                using (var con = Database.Instance.CreateConnection())
                {
                    con.Open();
                    DataSource = Reflector<T>.GetController(con).Get(ParentGridDialog.ModelFilter).ToList();
                    
                    var lastColumn = Columns[ColumnCount - 1];
                    lastColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    lastColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch(Exception ex)
            {
                ParentGridDialog.Error(ex);
            }
        }

        private ModelGridDialog<T> _parentGridDialog;
        protected ModelGridDialog<T> ParentGridDialog
        {
            get
            {
                if(_parentGridDialog == null)
                {
                    var ptr = this as Control;

                    while (ptr != null && !(ptr is ModelGridDialog<T>))
                    {
                        ptr = ptr.Parent;
                    }

                    _parentGridDialog = ptr as ModelGridDialog<T>;
                }

                return _parentGridDialog;
            }
        }
    }
}
