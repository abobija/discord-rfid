using DiscordRfid.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    public class ActivityListView : BaseListView
    {
        /// <summary>
        /// When the threshold is exceeded, each new activity will push (delete) the oldest activity from the list
        /// </summary>
        public int Threshold = 30;

        public ActivityListView()
        {
            Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader { Text = "Tag", Width = 200 },
                new ColumnHeader { Text = "CameAt", Width = 160 },
                new ColumnHeader { Text = "LeftAt", Width = 160 },
                new ColumnHeader { Text = "Duration", Width = 150 }
            });
        }

        public new void Clear()
        {
            Items.Clear();
        }

        public void Add(RfidTagActivity activity)
        {
            if((Items.Count + 1) > Threshold)
            {
                Items.RemoveAt(Items.Count - 1); // Remove from the back
            }

            Items.Add(new ActivityListViewItem(activity));
        }

        public void AddRange(RfidTagActivity[] activities)
        {
            SuspendLayout();
            foreach (var a in activities)
            {
                Add(a);
            }
            ResumeLayout();
        }

        public void Remove(RfidTagActivity activity)
        {
            ActivityListViewItem item = null;

            foreach (var li in Items)
            {
                var ai = li as ActivityListViewItem;

                if (ai.Activity == activity)
                {
                    item = ai;
                    break;
                }
            }

            if (item != null)
            {
                Items.Remove(item);
            }
        }
    }

    public class ActivityListViewItem : ListViewItem
    {
        public RfidTagActivity Activity { get; private set; }

        public ActivityListViewItem(RfidTagActivity activity) : base(new string[4])
        {
            Activity = activity;

            SubItems[0].Text = Activity.Tag.ToString();
            SubItems[1].Text = Activity.CameAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss");

            if(Activity.LeftAt != null)
            {
                SubItems[2].Text = ((DateTime) Activity.LeftAt).ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss");
            }

            SubItems[3].Text = Activity.Duration;

            BackColor = activity.Present ? Color.LightGreen : Color.LemonChiffon;
        }
    }
}
