using DiscordRfid.Controllers;
using DiscordRfid.Dtos;
using DiscordRfid.Models;
using DiscordRfid.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordRfid.Views
{
    public partial class MainForm : Form
    {
        protected string State { set => LblState.Text = $"Is: {value}"; }
        protected string BotName { set => LblBotName.Text = $"Bot: {value}"; }
        protected string ServerName { set => LblServerName.Text = $"@ Server: {value}"; }

        private EmployeeCounters _employeeCounters;
        private EmployeeCounters EmployeeCounters
        {
            get => _employeeCounters;
            set
            {
                _employeeCounters = value;
                LblCounterEmployeesTotal.Text = _employeeCounters.Total.ToString();

                PresentAbsentVisible = _employeeCounters.Total > 0;
                LblCounterEmployeesPresent.Text = _employeeCounters.Present.ToString();
                LblCounterEmployeesAbsent.Text = _employeeCounters.Absent.ToString();
            }
        }

        private bool PresentAbsentVisible
        {
            set => LblPresent.Visible = LblCounterEmployeesPresent.Visible =
                LblAbsent.Visible = LblCounterEmployeesAbsent.Visible =
                value;
        }

        public MainForm()
        {
            InitializeComponent();
            BotName = "";
            ServerName = "";
            PresentAbsentVisible = false;

            InitClock();
            InitBot();

            Shown += async (o, e) => await OnShownAsync(o, e);
        }

        private async Task OnShownAsync(object sender, EventArgs e)
        {
            LoadAndUpdateEmployeeCounters();

            Database.Instance.ModelCreated += model =>
            {
                if(model is Employee)
                {
                    LoadAndUpdateEmployeeCounters();
                }
            };

            try
            {
                State = "Connecting...";
                await Bot.Instance.ConnectAsync();
            }
            catch (Exception ex)
            {
                this.Error(ex);
                Close();
            }
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

            bot.NewPackage += PackagesListView.AddPackageSafe;

            bot.Ready += async () =>
            {
                ServerName = Bot.Instance.Guild.Name;
                State = "Loading recent packages...";

                var recentPackages = await bot.LoadRecentPackagesAsync();
                recentPackages.Reverse();
                PackagesListView.AddPackageRangeSafe(recentPackages.ToArray());

                State = "Ready";
            };
        }

        private void LoadAndUpdateEmployeeCounters()
        {
            try
            {
                using (var con = Database.Instance.CreateConnection())
                {
                    con.Open();
                    EmployeeCounters = new EmployeeController(con).GetCounters();
                }
            }
            catch(Exception ex)
            {
                this.Error(ex);
            }
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

        private void ToolBtnNewEmployee_Click(object sender, EventArgs e)
        {
            using(var frm = new EmployeeForm())
            {
                frm.ShowDialog();
            }
        }

        private void LinkLblEmployees_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var frm = new EmployeesForm())
            {
                frm.ShowDialog();
            }
        }
    }
}
