using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    public enum DialogFormButtons
    {
        SaveCancel,
        Close
    }

    [ToolboxItem(false)]
    public class DialogButton : Button
    {
        public DialogButton(string text)
        {
            Text = text;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
    }

    public class DialogForm : Form
    {
        private DialogFormButtons _dialogButtons;
        public DialogFormButtons DialogButtons
        {
            get => _dialogButtons;
            set
            {
                _dialogButtons = value;
                UpdateButtons();
            }
        }

        protected FlowLayoutPanel ButtonsPanel;

        protected Button BtnDialogSave;
        protected Button BtnDialogCancel;
        protected Button BtnDialogClose;

        public DialogForm()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;

            ButtonsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                BackColor = Color.Gainsboro,
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(5)
            };

            BtnDialogSave = new DialogButton("Save");
            BtnDialogCancel = new DialogButton("Cancel");
            BtnDialogSave.Click += OnDialogSave;
            BtnDialogCancel.Click += OnDialogCancel;

            BtnDialogClose = new DialogButton("Close");
            BtnDialogClose.Click += OnDialogClose;

            SuspendLayout();
            Controls.Add(ButtonsPanel);
            UpdateButtons();
            ResumeLayout();
            PerformLayout();
        }

        private void UpdateButtons()
        {
            ButtonsPanel.SuspendLayout();
            ButtonsPanel.Controls.Clear();

            switch (DialogButtons)
            {
                case DialogFormButtons.SaveCancel:
                    ButtonsPanel.Controls.Add(BtnDialogCancel);
                    ButtonsPanel.Controls.Add(BtnDialogSave);
                    break;

                case DialogFormButtons.Close:
                    ButtonsPanel.Controls.Add(BtnDialogClose);
                    break;
            }

            ButtonsPanel.ResumeLayout();
            ButtonsPanel.PerformLayout();
        }

        private void OnDialogClose(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        protected virtual void OnDialogCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        protected virtual void OnDialogSave(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
