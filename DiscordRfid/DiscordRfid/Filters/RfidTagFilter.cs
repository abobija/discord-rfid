using DiscordRfid.Controllers;
using DiscordRfid.Models;

namespace DiscordRfid.Filters
{
    public class RfidTagFilter : BaseFilter<RfidTag>
    {
        public RfidTagFilter()
        {
            var ctrl = new RfidTagController(null);
            OrderBy = $"{ctrl.TableAlias}.Id DESC";
        }
    }
}
