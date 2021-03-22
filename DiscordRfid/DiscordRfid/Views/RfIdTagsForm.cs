using DiscordRfid.Controllers;
using DiscordRfid.Filters;
using DiscordRfid.Models;
using DiscordRfid.Views.Controls;
using System.Windows.Forms;

namespace DiscordRfid.Views
{
    public class RfIdTagsForm : ModelGridDialog<RfidTag>
    {
        public Employee Employee { get; private set; }

        public RfIdTagsForm(Employee employee)
        {
            Employee = employee;
            Text += $" - {Employee}";

            var ctrl = new RfidTagController(null);

            ModelFilter = new RfidTagFilter
            {
                Where = $"{ctrl.TableAlias}.EmployeeId = {employee.Id}"
            };
        }

        protected override void OnModelNewClick(MouseEventArgs e)
        {
            using (var dlg = new RfidTagForm(null) { NewTagEmployee = Employee })
            {
                dlg.ShowDialog();
            }
        }
    }
}
