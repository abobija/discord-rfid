using DiscordRfid.Com;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace DiscordRfid
{
    public partial class MainForm : Form
    {
        protected string State { set => LblState.Text = $"Is: {value}"; }
        protected string BotName { set => LblBotName.Text = $"Bot: {value}"; }
        protected string ServerName { set => LblServerName.Text = $"@ Server: {value}"; }
        
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

            bot.Connected += () =>
            {
                State = "Checking environment...";
                BotName = Bot.Instance.Name;
            };

            bot.NewPackage += AddPackage;

            bot.Ready += async () =>
            {
                ServerName = Bot.Instance.Guild.Name;
                State = "Loading recent packages...";

                AddPackages(await bot.LoadRecentPackagesAsync());

                State = "Ready";
            };
        }

        private void AddPackage(Package package)
        {
            if(InvokeRequired)
            {
                Invoke(new MethodInvoker(() => CommunicationMonitor.Packages.Add(package)));
            }
            else
            {
                CommunicationMonitor.Packages.Add(package);
            }
        }

        private void AddPackages(ICollection<Package> pckgs)
        {
            Invoke(new MethodInvoker(() =>
            {
                foreach (var pckg in pckgs)
                {
                    AddPackage(pckg);
                }
            }));
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
                rolesQuestion += $"{Environment.NewLine}- {Configuration.MasterRoleName}";
            }

            if (bot.SlaveRole == null)
            {
                rolesQuestion += $"{Environment.NewLine}- {Configuration.SlaveRoleName}";
            }

            rolesQuestion += $"{Environment.NewLine}{Environment.NewLine}" +
                "Do you want to create them now?";

            return this.Question(rolesQuestion) == DialogResult.Yes;
        }

        private bool ChannelCreationPrompt()
        {
            var question = $"Discord server \"{Bot.Instance.Guild.Name}\" does not contain textual channel with name \"{Configuration.ChannelName}\" which " +
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
