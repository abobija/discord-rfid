using System;

namespace DiscordRfid.Models
{
    public class RfidTagActivity : BaseModel
    {
        public int Id;
        public DateTime CreatedAt;
        public RfidTag Tag;
        public bool Present;
    }
}
