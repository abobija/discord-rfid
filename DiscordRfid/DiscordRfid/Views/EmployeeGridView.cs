using DiscordRfid.Models;
using DiscordRfid.Views.Controls;

namespace DiscordRfid.Views
{
    public class EmployeeGridView : ModelGridDialog<Employee>
    {
        public EmployeeGridView()
        {
            Toolbox.Add(new ModelGridButton("RFID Tags"));
        }
    }
}
