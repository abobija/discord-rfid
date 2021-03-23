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
            var sn = SerialNumber.ToString();

            if(sn.Length > 5)
            {
                sn = sn.Substring(0, 3) + "..." + sn.Substring(sn.Length - 2);
            }

            return $"{$"{Employee.FirstName} {Employee.LastName}".Trim()} ({sn})";
        }
    }
}
