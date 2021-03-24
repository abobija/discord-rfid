using DiscordRfid.Communication;
using Serilog;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    public class PackagesListView : BaseListView
    {
        public ObservableCollection<Package> Packages;

        /// <summary>
        /// When the threshold is exceeded, each new package will push (delete) the oldest package from the list
        /// </summary>
        public int Threshold = 30;

        public PackagesListView()
        {
            Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader { Text = "Time", Width = 110 },
                new ColumnHeader { Text = "Type", Width = 60 },
                new ColumnHeader { Text = "Content", Width = 150 }
            });

            Packages = new ObservableCollection<Package>();
            Packages.CollectionChanged += Packages_Changed;
        }

        public void AddPackageSafe(Package package)
        {
            AddPackageRangeSafe(new Package[] { package });
        }

        public void AddPackageRangeSafe(Package[] packages)
        {
            void add()
            {
                foreach (var p in packages)
                {
                    Packages.Add(p);
                }
            }

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(add));
            }
            else
            {
                add();
            }
        }

        private void RemovePackage(Package package)
        {
            PackageListViewItem item = null;

            foreach (var li in Items)
            {
                var pi = li as PackageListViewItem;

                if(pi.Package == package)
                {
                    item = pi;
                    break;
                }
            }

            if (item != null)
            {
                Items.Remove(item);
            }
        }

        private void Packages_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var thresholdExceed = Items.Count + e.NewItems.Count - Threshold;

                    while (thresholdExceed > 0)
                    {
                        Items.RemoveAt(Items.Count - 1); // Remove from the back
                        thresholdExceed--;
                    }

                    SuspendLayout();
                    foreach (var pckg in e.NewItems.Cast<Package>())
                    {
                        Items.Insert(0, new PackageListViewItem(pckg));
                    }
                    ResumeLayout();

                    break;

                case NotifyCollectionChangedAction.Remove:
                    SuspendLayout();
                    foreach (var pckg in e.OldItems.Cast<Package>())
                    {
                        RemovePackage(pckg);
                    }
                    ResumeLayout();
                    break;
                default:
                    Log.Debug($"{GetType().Name} Unhandled changed action");
                    break;
            }
        }
    }

    public class PackageListViewItem : ListViewItem
    {
        public Package Package { get; private set; }

        public PackageListViewItem(Package package) : base(new string[3])
        {
            Package = package;

            SubItems[0].Text = package.Time?.ToString("dd.MM.yy HH:mm");
            SubItems[1].Text = package.Type.ToString();

            if (package.Type == PackageType.Tag)
            {
                SubItems[2].Text = package.SerialNumber.ToString();
            }
        }
    }
}
