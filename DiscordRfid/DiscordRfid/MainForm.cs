using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordRfid
{
    public partial class MainForm : Form
    {
        protected string State { set => LblState.Text = value; }
        protected string BotName { set => LblBotName.Text = value; }
        protected string ServerName { set => LblServerName.Text = value; }

        public MainForm()
        {
            InitializeComponent();
            BotName = "";
            ServerName = "";
            Shown += (o, e) => ConnectAsync().ContinueWithNoop();
        }

        private async Task ConnectAsync()
        {
            var config = Configuration.Instance;
            string token = config.Token;
            bool check = true;

            State = "Connecting...";
            Bot.Instance.Client.Connected += () => OnConnected();

            while (check)
            {
                if (token == null)
                {
                    using (var dlg = new TokenForm { Message = "Please enter token of Discord bot" })
                    {
                        if (dlg.ShowDialog() != DialogResult.OK)
                        {
                            Close();
                            return;
                        }

                        token = dlg.Token;
                    }
                }

                try
                {
                    await Bot.Instance.Client.LoginAsync(TokenType.Bot, token);
                    await Bot.Instance.Client.StartAsync();
                    config.Token = token;
                    check = false;
                }
                catch (Exception ex)
                {
                    config.Token = token = null;
                    this.Error(string.Format("Invalid token.{0}{1}{0}{0}Please try again.", Environment.NewLine, ex.Message));
                }
            }
        }

        protected Task OnConnected()
        {
            BotName = Bot.Instance.Client.CurrentUser.Username;
            State = "Checking environment...";

            Bot.Instance.Client.GuildAvailable += e => OnGuilAvailable();

            return Extensions.NoopTask();
        }

        private Task OnGuilAvailable()
        {
            try
            {
                var bot = Bot.Instance.Client;

                if (bot.Guilds.Count != 1) // Bot can be only in one guild
                {
                    throw new Exception("Bot can be only in one guild");
                }

                var guild = bot.Guilds.ElementAt(0);
                ServerName = $"@ {guild.Name}";

                CheckRolesAsync(guild).GetAwaiter().GetResult();

                State = "Ready";
            }
            catch (Exception ex)
            {
                this.Error(ex);
                this.SafeClose();
            }

            return Extensions.NoopTask();
        }

        private async Task CheckRolesAsync(SocketGuild guild)
        {
            IRole masterRole = guild.Roles.FirstOrDefault(r => r.Name == Constants.MasterRoleName);
            IRole slaveRole = guild.Roles.FirstOrDefault(r => r.Name == Constants.SlaveRoleName);

            var rolesQuestion = $"Discord server \"{guild.Name}\" does not have required roles which is used for differentiate between RFID master and slaves:" +
                Environment.NewLine;

            if (masterRole == null)
            {
                rolesQuestion += $"{Environment.NewLine}- {Constants.MasterRoleName}";
            }

            if (slaveRole == null)
            {
                rolesQuestion += $"{Environment.NewLine}- {Constants.SlaveRoleName}";
            }

            rolesQuestion += $"{Environment.NewLine}{Environment.NewLine}" +
                "Do you want to create them now?";

            if (masterRole == null || slaveRole == null)
            {
                if (this.Question(rolesQuestion) != DialogResult.Yes)
                {
                    throw new Exception("Roles are required.");
                }

                var perms = new GuildPermissions(
                    viewChannel: true,
                    readMessageHistory: true,
                    sendMessages: true,
                    addReactions: true,
                    attachFiles: true
                    );

                if (masterRole == null)
                {
                    masterRole = await guild.CreateRoleAsync(Constants.MasterRoleName, perms, null, false, false, null);
                }

                if (slaveRole == null)
                {
                    slaveRole = await guild.CreateRoleAsync(Constants.SlaveRoleName, perms, null, false, false, null);
                }
            }

            var self = guild.GetUser(Bot.Instance.Client.CurrentUser.Id);
            var masterRoleAssigned = self.Roles.FirstOrDefault(r => r.Id == masterRole.Id) != null;

            if(! masterRoleAssigned)
            {
                await self.AddRoleAsync(masterRole);
            }
        }
    }
}
