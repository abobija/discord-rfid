using DiscordRfid.Models;
using DiscordRfid.Views.Controls;

namespace DiscordRfid.Views
{
    public partial class RfidTagForm : ModelDialog<RfidTag>
    {
        private Employee _newTagEmployee;

        public Employee NewTagEmployee
        {
            get => _newTagEmployee;
            set
            {
                _newTagEmployee = value;
                LblEmployee.Text = _newTagEmployee == null ? "-" : _newTagEmployee.ToString();
            }
        }

        public RfidTagForm(RfidTag tag = null)
        {
            InitializeComponent();
            Model = tag;
        }

        protected override void PopulateForm()
        {
            TxtSerialNumber.Text = Model.SerialNumber.ToString();
            LblEmployee.Text = Model.Employee.ToString();
        }

        protected override RfidTag ConstructModel()
        {
            return new RfidTag(TxtSerialNumber.Text.Trim())
            {
                Employee = Model != null ? Model.Employee : NewTagEmployee
            };
        }
    }
}
