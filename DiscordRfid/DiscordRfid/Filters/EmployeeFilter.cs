using DiscordRfid.Models;

namespace DiscordRfid.Filters
{
    public class EmployeeFilter : IFilter<Employee>
    {
        private string _orderBy;

        public string OrderBy { 
            get => _orderBy; 
            set => _orderBy = value;
        }

        public EmployeeFilter()
        {
            OrderBy = "emp.Id DESC";
        }
    }
}
