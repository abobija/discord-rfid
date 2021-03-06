using DiscordRfid.Controllers;
using DiscordRfid.Models;

namespace DiscordRfid.Filters
{
    public class EmployeeFilter : BaseFilter<Employee>
    {
        public EmployeeFilter()
        {
            var ctrl = new EmployeeController(null);
            OrderBy = $"{ctrl.TableAlias}.Id DESC";
        }
    }
}
