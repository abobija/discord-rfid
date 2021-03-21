using System;

namespace DiscordRfid.Models
{
    public class RfidTagActivity : BaseModel
    {
        public RfidTag Tag;
        public bool Present;
    }
}
