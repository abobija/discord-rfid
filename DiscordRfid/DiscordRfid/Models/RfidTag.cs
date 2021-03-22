using System.Activities;

namespace DiscordRfid.Models
{
    public class RfidTag : BaseModel
    {
        public ulong SerialNumber { get; set; }
        public Employee Employee { get; set; }

        public RfidTag() { }

        public RfidTag(string serialNumber)
        {
            ulong.TryParse(serialNumber, out ulong sn);
            SerialNumber = sn;
        }

        public override void Validate()
        {
            if (SerialNumber == 0)
            {
                throw new ValidationException("Invalid SerialNumber");
            }

            if(Employee == null)
            {
                throw new ValidationException("Missing Employee");
            }
        }

        public override string ToString()
        {
            return $"{SerialNumber} ({$"{Employee.FirstName} {Employee.LastName}".Trim()})";
        }
    }
}
