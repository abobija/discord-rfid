using System;

namespace DiscordRfid.Models
{
    public class RfidTag
    {
        public int Id;
        public DateTime CreatedAt;
        public ulong SerialNumber;
        public Employee Employee;
    }
}
