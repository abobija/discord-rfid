using Discord;
using System;
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

            Bot.Instance.Client.GuildAvailable += g => OnGuildAvailable();

            return Extensions.NoopTask();
        }

        private Task OnGuildAvailable()
        {
            try
            {
                ServerName = $"@ {Bot.Instance.Guild.Name}";

                CheckRolesAsync().GetAwaiter().GetResult();
                CheckChannelAsync().GetAwaiter().GetResult();

                State = "Ready";
            }
            catch (Exception ex)
            {
                this.Error(ex);
                this.SafeClose();
            }

            return Extensions.NoopTask();
        }

        private async Task CheckRolesAsync()
        {
            var bot = Bot.Instance;

            if (bot.MasterRole == null || bot.SlaveRole == null)
            {
                var rolesQuestion = $"Discord server \"{bot.Guild.Name}\" does not have required roles which is used for differentiate RFID master and slaves:" +
                Environment.NewLine;

                if (bot.MasterRole == null)
                {
                    rolesQuestion += $"{Environment.NewLine}- {Constants.MasterRoleName}";
                }

                if (bot.SlaveRole == null)
                {
                    rolesQuestion += $"{Environment.NewLine}- {Constants.SlaveRoleName}";
                }

                rolesQuestion += $"{Environment.NewLine}{Environment.NewLine}" +
                    "Do you want to create them now?";

                if (this.Question(rolesQuestion) != DialogResult.Yes)
                {
                    throw new Exception("Roles are required");
                }

                await bot.CreateRolesAsync();
            }

            await bot.SelfAssignMasterRoleAsync();
        }

        private async Task CheckChannelAsync()
        {
            var bot = Bot.Instance;
            
            if(bot.Channel == null)
            {
                var question = $"Discord server \"{bot.Guild.Name}\" does not contain textual channel with name \"{Constants.ChannelName}\" which " +
                    "is used for communication with RFID slaves." +
                    $"{Environment.NewLine}{Environment.NewLine}" +
                    "Would you like to create this channel now?";

                if(this.Question(question) != DialogResult.Yes)
                {
                    throw new Exception($"Textual channel \"{Constants.ChannelName}\" is required");
                }

                bot.Channel = await bot.Guild.CreateTextChannelAsync(Constants.ChannelName);
            }

            await bot.SetChannelPermissionsAsync();
        }
    }
}
