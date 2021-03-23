using DiscordRfid.Controllers;
using DiscordRfid.Models;

namespace DiscordRfid.Filters
{
    public class RfidTagActivityFilter : BaseFilter<RfidTagActivity>
    {
        public RfidTagActivityFilter()
        {
            var ctrl = new RfidTagActivityController(null);
            OrderBy = $"{ctrl.TableAlias}.Id DESC";
            Limit = 20;
        }
    }
}
