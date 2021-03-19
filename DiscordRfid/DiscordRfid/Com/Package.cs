using System;

namespace DiscordRfid.Com
{
    public class Package
    {
        public DateTime Time;
        public ulong SerialNumber;
        public Device Device;

        public Package(ulong serialNumber)
        {
            Time = DateTime.Now;
            SerialNumber = serialNumber;
        }
    }
}
