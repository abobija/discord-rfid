using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordRfid
{
    public class Bot
    {
        protected static Bot Singletone { get; set; }

        private DiscordSocketClient Client { get; set; }

        protected Bot()
        {
            Client = new DiscordSocketClient();
        }

        public Task LoginAsync(string token)
        {
            return Client.LoginAsync(TokenType.Bot, token);
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
