using DiscordRfid.Models;

namespace DiscordRfid.Filters
{
    public interface IFilter<T> where T : BaseModel
    {
        string OrderBy { get; set; }
    }
}
