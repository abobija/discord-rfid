using System;

namespace DiscordRfid.Models
{
    public class RfidTagActivity
    {
        public int Id;
        public DateTime DateTime;
        public RfidTag Tag;
        public bool Present;
    }
}
