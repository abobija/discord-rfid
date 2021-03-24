using DiscordRfid.Communication;
using DiscordRfid.Controllers;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    public class PackagesListView : BaseListView
    {
        /// <summary>
        /// When the threshold is exceeded, each new package will push (delete) the oldest package from the list
        /// </summary>
        public int Threshold = 30;

        public PackageListViewItem SelectedPackageItem
        {
            get
            {
                if (SelectedItems.Count > 0)
                    return SelectedItems[0] as PackageListViewItem;

                return null;
            }
        }

        private ContextMenu PackageContextMenu { get; set; }
        private MenuItem AttachTagMenuItem { get; set; }

        public PackagesListView()
        {
            Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader { Text = "", Width = 20 },
                new ColumnHeader { Text = "Type", Width = 45 },
                new ColumnHeader { Text = "Content", Width = 110 },
                new ColumnHeader { Text = "Time", Width = 110 }
            });

            AttachTagMenuItem = new MenuItem("Attach Tag to employee...");
            AttachTagMenuItem.Click += AttachTagClick;

            PackageContextMenu = new ContextMenu(new MenuItem[] { AttachTagMenuItem });
        }

        private void AttachTagClick(object sender, System.EventArgs e)
        {
            var packageItem = SelectedPackageItem;

            if (packageItem == null)
                return;

            Employee employee = null;

            using (var dlg = new EmployeeQuickChoose())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    employee = dlg.Employee;
                }
            }

            if(employee == null)
                return;

            try
            {
                RfidTag tag = null;

                using (var con = Database.Instance.CreateConnection())
                {
                    con.Open();
                    tag = new RfidTag
                    {
                        SerialNumber = (ulong)packageItem.Package.SerialNumber,
                        Employee = new Employee
                        {
                            Id = employee.Id
                        }
                    };
                    tag = new RfidTagController(con).Create(tag);
                }

                FindForm().Information($"Tag with serial number {tag.SerialNumber} has been successfully attached to employee {tag.Employee}");
            }
            catch(Exception ex)
            {
                FindForm().Error(ex);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                var packageItem = SelectedPackageItem;

                if(packageItem != null && packageItem.Package.Type == PackageType.Tag)
                {
                    AttachTagMenuItem.Enabled = packageItem.TagFound != null && ! (bool) packageItem.TagFound;
                    PackageContextMenu.Show(this, PointToClient(Cursor.Position));
                }
            }
        }

        public PackageListViewItem AddPackage(Package package)
        {
            if ((Items.Count + 1) > Threshold)
            {
                Items.RemoveAt(Items.Count - 1); // Remove from the back
            }

            PackageListViewItem item = new PackageListViewItem(package);
            Items.Insert(0, item);
            return item;
        }

        public PackageListViewItem AddPackageSafe(Package package)
        {
            PackageListViewItem item = null;

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => item = AddPackage(package)));
            }
            else
            {
                AddPackage(package);
            }

            return item;
        }

        public void AddPackageRangeSafe(Package[] packages)
        {
            void add()
            {
                SuspendLayout();
                foreach (var p in packages)
                {
                    AddPackage(p);
                }
                ResumeLayout();
            }

            if(InvokeRequired)
            {
                Invoke(new MethodInvoker(add));
            }
            else
            {
                add();
            }
        }
    }

    public class PackageListViewItem : ListViewItem
    {
        private bool? _tagFound;
        public bool? TagFound
        {
            get => _tagFound;
            set
            {
                if(value != _tagFound)
                {
                    _tagFound = value;
                    UpdateState();
                }
            }
        }
        public Package Package { get; private set; }

        public PackageListViewItem(Package package) : base(new string[4])
        {
            Package = package;
            UpdateState();
        }

        protected void UpdateState()
        {
            UseItemStyleForSubItems = false;
            SubItems[0].Text = "⬝";
            SubItems[1].Text = Package.Type.ToString();

            if (Package.Type == PackageType.Tag)
            {
                SubItems[2].Text = Package.SerialNumber.ToString();

                if(TagFound != null)
                {
                    SubItems[2].ForeColor = (bool) TagFound ? Color.Green : Color.OrangeRed;
                }
            }

            SubItems[3].Text = Package.Time?.ToLocalTime().ToString("dd.MM.yy HH:mm");
        }
    }
}
