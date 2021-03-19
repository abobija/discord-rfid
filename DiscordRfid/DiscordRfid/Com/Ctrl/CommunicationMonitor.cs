using Serilog;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DiscordRfid.Com.Ctrl
{
    public partial class CommunicationMonitor : UserControl
    {
        public ObservableCollection<Package> Packages;
        private Label LblEmpty;

        private bool Empty
        {
            set
            {
                PackagesContainer.Visible = !value;
                LblEmpty.Visible = value;
            }
        }

        public CommunicationMonitor()
        {
            InitializeComponent();
            PackagesContainer.Dock = DockStyle.Bottom;

            Packages = new ObservableCollection<Package>();
            Packages.CollectionChanged += Packages_Changed;

            LblEmpty = new Label
            {
                Text = $"It's pretty quiet.{Environment.NewLine}Waiting for RFID packages...",
                ForeColor = Color.DimGray,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(LblEmpty);
            Packages.Clear(); // trigger for initial draw
        }

        private void RemovePackageFromContainer(Package package)
        {
            CommunicationMonitorListItem item = null;

            foreach(var i in PackagesContainer.Controls.Cast<CommunicationMonitorListItem>())
            {
                item = i;
            }

            if(item != null)
            {
                PackagesContainer.Controls.Remove(item);
            }
        }

        private void Packages_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            Empty = Packages.Count == 0;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var pckg in e.NewItems.Cast<Package>())
                    {
                        PackagesContainer.Controls.Add(new CommunicationMonitorListItem(pckg)
                        {
                            AutoSize = true,
                            Margin = new Padding(0, 1, 0, 0),
                            BackColor = Color.LightGray
                        });
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var pckg in e.OldItems.Cast<Package>())
                    {
                        RemovePackageFromContainer(pckg);
                    }
                    break;
                default:
                    Log.Debug($"{GetType().Name} Unhandled changed action");
                    break;
            }
        }
    }
}
