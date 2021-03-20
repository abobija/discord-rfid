using DiscordRfid.Models;
using DiscordRfid.Views.Controls;

namespace DiscordRfid.Views
{
    public partial class EmployeeForm : ModelForm<Employee>
    {
        public EmployeeForm(Employee employee = null)
        {
            InitializeComponent();
            Model = employee;
        }

        protected override Employee ConstructModel()
        {
            var emp = new Employee
            {
                FirstName = TxtFirstName.Text.Trim(),
                LastName = TxtLastName.Text.Trim()
            };

            emp.Validate();

            return emp;
        }

        protected override void PopulateForm()
        {
            TxtFirstName.Text = Model.FirstName;
            TxtLastName.Text = Model.LastName;
        }
    }
}
