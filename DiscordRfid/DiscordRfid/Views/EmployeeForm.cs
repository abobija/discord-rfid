using DiscordRfid.Models;
using DiscordRfid.Views.Controls;

namespace DiscordRfid.Views
{
    public partial class EmployeeForm : ModelDialog<Employee>
    {
        public EmployeeForm(Employee employee = null)
        {
            InitializeComponent();
            Model = employee;
        }

        protected override void PopulateForm()
        {
            TxtFirstName.Text = Model.FirstName;
            TxtLastName.Text = Model.LastName;
            ChckPresent.Checked = Model.Present;
        }

        protected override Employee ConstructModel()
        {
            return new Employee
            {
                FirstName = string.IsNullOrWhiteSpace(TxtFirstName.Text) ? null : TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim(),
                Present = ChckPresent.Checked
            };
        }
    }
}
