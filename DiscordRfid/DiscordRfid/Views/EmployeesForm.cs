using DiscordRfid.Models;
using DiscordRfid.Views.Controls;
using System.Windows.Forms;

namespace DiscordRfid.Views
{
    public class EmployeesForm : ModelGridDialog<Employee>
    {
        public EmployeesForm()
        {
            Toolbox.Add(new ModelGridButton("RFID Tags", OnRfidTagsClick)
            { 
                DisableIfModelNotSelected = true
            });
        }

        protected virtual void OnRfidTagsClick(MouseEventArgs e)
        {

        }
    }
}
