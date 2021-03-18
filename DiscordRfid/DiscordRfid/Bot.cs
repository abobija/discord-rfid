using Discord.WebSocket;

namespace DiscordRfid
{
    public class Bot
    {
        protected static Bot Singletone { get; set; }

        public DiscordSocketClient Client { get; set; }

        protected Bot()
        {
            Client = new DiscordSocketClient();
        }

        public static Bot Instance
        {
            get
            {
                if (Singletone == null)
                {
                    Singletone = new Bot();
                }

                return Singletone;
            }
        }
    }
}
