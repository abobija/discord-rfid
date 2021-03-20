using System;

namespace DiscordRfid.Models
{
    public class RfidTag : BaseModel
    {
        public int Id;
        public DateTime CreatedAt;
        public ulong SerialNumber;
        public Employee Employee;
    }
}
