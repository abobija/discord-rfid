using System.Windows.Forms;

namespace DiscordRfid
{
    public partial class MainForm : Form
    {
        protected Configuration Config { get; set; }

        public MainForm()
        {
            InitializeComponent();
            Config = Configuration.Load();

            Shown += (o, e) => Startup();
        }

        private void Startup()
        {
            string token = Config.Token;

            if(token == null)
            {
                using(var dlg = new TokenForm { Message = "Please enter token of Discord bot" })
                {
                    if(dlg.ShowDialog() != DialogResult.OK)
                    {
                        Close();
                        return;
                    }
                    
                    token = dlg.Token;
                }
            }

            // check token...
        }
    }
}
