using System.Windows.Forms;

namespace DiscordRfid.Com.Ctrl
{
    public partial class CommunicationMonitor : UserControl
    {
        private readonly CommunicationMonitorList List;

        public CommunicationMonitor()
        {
            InitializeComponent();

            List = new CommunicationMonitorList
            {
                Dock = DockStyle.Fill
            };

            Controls.Add(List);
        }
    }
}
