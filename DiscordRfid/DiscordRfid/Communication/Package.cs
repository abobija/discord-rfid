using Discord;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Text;

namespace DiscordRfid.Communication
{
    public enum PackageType
    {
        Unknown,
        Tag,
        Ping,
        Pong
    }

    public class Package
    {
        public PackageType Type;
        public DateTime? Time;
        public Device Device;
        public ulong? SerialNumber;

        public Package() { }

        public Package(PackageType type, Device device, DateTime? time = null, ulong? serialNumber = null)
        {
            Type = type;
            Time = time;
            Device = device;
            SerialNumber = serialNumber;
        }

        public static Package FromDiscordMessage(IMessage message)
        {
            if(message.Type != MessageType.Default)
            {
                throw new Exception("Only discord messages with default type can be parsed to package");
            }

            var pckg = JsonConvert.DeserializeObject<Package>(
                Encoding.UTF8.GetString(
                    Convert.FromBase64String(message.Content)
                    )
                );

            pckg.Time = message.Timestamp.DateTime;

            return pckg;
        }
    }
}
