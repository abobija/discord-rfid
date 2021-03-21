using DiscordRfid.Models;
using DiscordRfid.Views.Controls;

namespace DiscordRfid.Views
{
    public partial class RfidTagForm : ModelDialog<RfidTag>
    {
        public RfidTagForm(RfidTag tag = null)
        {
            InitializeComponent();
            Model = tag;
        }

        protected override void PopulateForm()
        {
            TxtSerialNumber.Text = Model.SerialNumber.ToString();
        }

        protected override RfidTag ConstructModel()
        {
            return new RfidTag(TxtSerialNumber.Text.Trim())
            {
                Employee = new Employee { Id = 1 } // TODO: For testing. Fix later
            };
        }
    }
}
