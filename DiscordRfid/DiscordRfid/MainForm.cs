using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordRfid
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Shown += (o, e) => StartupCheckAsync().ContinueWithNoop();
        }

        private async Task StartupCheckAsync()
        {
            var config = Configuration.Instance;
            var bot = Bot.Instance;
            string token = config.Token;
            bool check = true;

            while(check)
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
                    await bot.LoginAsync(token);
                    config.Token = token;
                    check = false;
                }
                catch (Exception ex)
                {
                    config.Token = token = null;

                    MessageBox.Show(string.Format("Invalid token.{0}{1}{0}{0}Please try again.", Environment.NewLine, ex.Message),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
