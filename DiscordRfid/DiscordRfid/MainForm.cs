using DiscordRfid.Com;
using System;
using System.Reflection;
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

            InitClock();
            InitBot();

            Shown += async (o, e) =>
            {
                State = "Connecting...";

                try
                {
                    await Bot.Instance.ConnectAsync();
                }
                catch(Exception ex)
                {
                    this.Error(ex);
                    Close();
                }
            };
        }

        private void InitClock()
        {
            UpdateClock();
            var tmr = new System.Timers.Timer(1000) { AutoReset = true };
            tmr.Elapsed += (o, e) => { try { Invoke(new MethodInvoker(UpdateClock)); } catch { } };
            tmr.Enabled = true;
        }

        private void UpdateClock()
        {
            ToolLblClock.Text = DateTime.Now.ToString("HH:mm:ss, MM.dd.yyyy");
        }

        private void InitBot()
        {
            var bot = Bot.Instance;

            bot.RolesCreationPrompter = RolesCreationPrompt;
            bot.ChannelCreationPrompter = ChannelCreationPrompt;

            bot.AuthenticationError += OnAuthenticationError;

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
                return Extensions.NoopTask();
            };

            bot.Ready += () => State = "Ready";
        }

        private string OnAuthenticationError(string invalidToken, Exception ex)
        {
            string validToken = null;

            if(invalidToken != null)
            {
                this.Error(string.Format("Authentication error:{1}{0}{1}{1}Please try again.", ex.Message, Environment.NewLine));
            }

            using (var dlg = new TokenForm { Message = "Please enter token of Discord bot" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    validToken = dlg.Token;
                }
            }

            return validToken;
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

        private void ToolBtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolBtnAbout_Click(object sender, EventArgs e)
        {
            var asm = Assembly.GetExecutingAssembly();

            this.Information(
                    $"{Application.ProductName} {Application.ProductVersion}" +
                    $"{Environment.NewLine}{asm.GetCustomAttribute<AssemblyDescriptionAttribute>().Description}" +
                    Environment.NewLine + Environment.NewLine +
                    $"Author:" +
                    $"{Environment.NewLine}Alija Bobija ({Application.CompanyName})" +
                    Environment.NewLine + Environment.NewLine +
                    "Code:" +
                    $"{Environment.NewLine}https://github.com/abobija/discord-rfid"
                );
        }
    }
}
