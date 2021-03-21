using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    public class DialogForm : Form
    {
        protected Button BtnDialogSave;
        protected Button BtnDialogCancel;

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

            BtnDialogSave = new Button
            {
                Text = "Save",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };

            BtnDialogCancel = new Button
            {
                Text = "Cancel",
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };

            BtnDialogSave.Click += OnDialogSave;
            BtnDialogCancel.Click += OnDialogCancel;

            var buttonsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                BackColor = Color.Gainsboro,
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(5)
            };

            buttonsPanel.Controls.Add(BtnDialogCancel);
            buttonsPanel.Controls.Add(BtnDialogSave);

            buttonsPanel.SuspendLayout();
            Controls.Add(buttonsPanel);
            buttonsPanel.ResumeLayout();
            buttonsPanel.PerformLayout();
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
