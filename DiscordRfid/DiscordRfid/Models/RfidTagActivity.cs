using System;

namespace DiscordRfid.Models
{
    public class RfidTagActivity : BaseModel
    {
        public bool Present { get; set; }
        public RfidTag Tag { get; set; }
        public DateTime CameAt { get; set; }
        public DateTime? LeftAt { get; set; }
    }
}
