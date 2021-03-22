using DiscordRfid.Models;

namespace DiscordRfid.Filters
{
    public class EmployeeFilter : BaseFilter<Employee>
    {
        public EmployeeFilter()
        {
            OrderBy = "emp.Id DESC";
        }
    }
}
