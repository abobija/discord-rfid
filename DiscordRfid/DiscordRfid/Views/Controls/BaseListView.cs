using System.ComponentModel;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    [ToolboxItem(false)]
    public abstract class BaseListView : ListView
    {
        public BaseListView()
        {
            View = View.Details;
            BorderStyle = BorderStyle.None;
            FullRowSelect = true;
            GridLines = true;
            HeaderStyle = ColumnHeaderStyle.Nonclickable;
            MultiSelect = false;
        }
    }
}
