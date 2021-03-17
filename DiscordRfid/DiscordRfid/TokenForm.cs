using System.Windows.Forms;

namespace DiscordRfid
{
    public partial class TokenForm : Form
    {
        public string Token => string.IsNullOrWhiteSpace(TxtBoxToken.Text) ? null : TxtBoxToken.Text.Trim();

        public string Message
        {
            get => LblMessage.Text;
            set { LblMessage.Text = value; }
        }

        public TokenForm()
        {
            InitializeComponent();

            BtnCancel.Click += (o, e) => { DialogResult = DialogResult.Cancel; };

            BtnOk.Click += (o, e) => Submit();

            TxtBoxToken.KeyDown += (o, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Submit();
                }
            };
        }

        private void Submit()
        {
            if (Token == null)
            {
                MessageBox.Show("Token cannot be empty", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
