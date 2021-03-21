using DiscordRfid.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    [ToolboxItem(false)]
    public class ModelGrid<T> : DataGridView where T : BaseModel
    {
        public bool ModelSelected => SelectedRows.Count > 0;

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

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            if(Columns[e.ColumnIndex].ValueType == typeof(DateTime) && e.Value != null)
            {
                e.Value = ((DateTime) e.Value).ToLocalTime();
            }
        }
    }
}
