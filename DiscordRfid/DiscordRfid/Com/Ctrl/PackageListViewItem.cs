using System.Windows.Forms;

namespace DiscordRfid.Com.Ctrl
{
    public class PackageListViewItem : ListViewItem
    {
        public Package Package { get; private set; }

        public PackageListViewItem(Package package) : base(new string[3])
        {
            Package = package;

            SubItems[0].Text = package.Time?.ToString("dd.MM.yy HH:mm");
            SubItems[1].Text = package.Type.ToString();

            if(package.Type == PackageType.Tag)
            {
                SubItems[2].Text = package.SerialNumber.ToString();
            }
        }
    }
}
