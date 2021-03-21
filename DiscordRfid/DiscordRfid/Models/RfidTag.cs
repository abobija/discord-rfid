using System;

namespace DiscordRfid.Models
{
    public class RfidTag : BaseModel
    {
        public ulong SerialNumber;
        public Employee Employee;
    }
}
