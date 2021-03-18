using Serilog;
using System;
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

            var bot = Bot.Instance;

            bot.TokenProvider = TokenProvider;
            bot.RolesCreationPrompter = RolesCreationPrompt;
            bot.ChannelCreationPrompter = ChannelCreationPrompt;

            bot.ConnectError += OnConnectError;

            bot.EnvironmentCreationError += ex =>
            {
                this.Error(ex);
                this.SafeClose();
            };

            bot.Client.Connected += () =>
            {
                BotName = Bot.Instance.Name;
                State = "Checking environment...";
                return Extensions.NoopTask();
            };

            bot.Client.GuildAvailable += g =>
            {
                ServerName = $"@ {Bot.Instance.Guild.Name}";
                Log.Information($"Connected to discord server");
                return Extensions.NoopTask();
            };

            bot.Ready += () => State = "Ready";

            Shown += async (o, e) =>
            {
                State = "Connecting...";

                try
                {
                    await bot.ConnectAsync();
                }
                catch(Exception ex)
                {
                    Log.Error(ex, "Fail to connect. Exiting");
                    this.Error(ex);
                    Close();
                }
            };
        }

        private string TokenProvider()
        {
            string token = null;

            using (var dlg = new TokenForm { Message = "Please enter token of Discord bot" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    token = dlg.Token;
                }
            }

            return token;
        }

        private void OnConnectError(Exception ex)
        {
            this.Error(string.Format("{0}{1}{1}Please try again.", ex.Message, Environment.NewLine));
        }

        private bool RolesCreationPrompt()
        {
            var bot = Bot.Instance;
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

            return this.Question(rolesQuestion) == DialogResult.Yes;
        }

        private bool ChannelCreationPrompt()
        {
            var question = $"Discord server \"{Bot.Instance.Guild.Name}\" does not contain textual channel with name \"{Constants.ChannelName}\" which " +
                "is used for communication with RFID slaves." +
                $"{Environment.NewLine}{Environment.NewLine}" +
                "Would you like to create this channel now?";

            return this.Question(question) == DialogResult.Yes;
        }
    }
}
