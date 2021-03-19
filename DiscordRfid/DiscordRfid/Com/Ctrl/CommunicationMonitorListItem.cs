using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordRfid.Com.Ctrl
{
    [ToolboxItem(false)]
    public partial class CommunicationMonitorListItem : UserControl
    {
        public Package Package { get; private set; }

        public CommunicationMonitorListItem(Package package)
        {
            Package = package;
            InitializeComponent();
            Dock = DockStyle.Bottom;
            
            LblSerialNumber.Text = Package.SerialNumber.ToString();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size
            {
                Height = Padding.Vertical + Font.Height
            };
        }
    }
}
