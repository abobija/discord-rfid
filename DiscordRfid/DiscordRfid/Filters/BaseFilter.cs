using DiscordRfid.Models;

namespace DiscordRfid.Filters
{
    public abstract class BaseFilter<T> where T : BaseModel
    {
        public virtual string OrderBy { get; set; }
        public virtual string Where { get; set; }
        public virtual int? Limit { get; set; }
    }
}
