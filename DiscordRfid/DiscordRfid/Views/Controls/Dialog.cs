using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DiscordRfid.Views.Controls
{
    [ToolboxItem(false)]
    public class DialogButton : Button
    {
        public DialogButton(string text)
        {
            Text = text;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Cursor = Cursors.Hand;
            Padding = new Padding(5, 3, 5, 3);
        }
    }

    [ToolboxItem(false)]
    public class DialogFlowPanel : FlowLayoutPanel
    {
        public DialogFlowPanel()
        {
            AutoSize = true;
            BackColor = Color.Gainsboro;
            Padding = new Padding(5);
        }
    }

    public enum DialogFormButtons
    {
        SaveCancel,
        Close
    }

    public class Dialog : Form
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

        private readonly Panel ControlsPanel;
        private readonly FlowLayoutPanel ButtonsPanel;

        private readonly Button ButtonDialogSave;
        private readonly Button ButtonDialogCancel;
        private readonly Button ButtonDialogClose;

        public Dialog()
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

            ControlsPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            ButtonsPanel = new DialogFlowPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft
            };

            ButtonDialogSave = new DialogButton("Save");
            ButtonDialogCancel = new DialogButton("Cancel");
            ButtonDialogClose = new DialogButton("Close");

            ButtonDialogSave.Click += OnButtonSaveClick;
            ButtonDialogCancel.Click += OnButtonCancelClick;
            ButtonDialogClose.Click += OnButtonCloseClick;

            SuspendLayout();
            Controls.Add(ControlsPanel);
            Controls.Add(ButtonsPanel);
            UpdateButtons();
            ResumeLayout();
            PerformLayout();
        }

        protected void AddControl(Control control)
        {
            ControlsPanel.Controls.Add(control);
        }

        private void UpdateButtons()
        {
            ButtonsPanel.SuspendLayout();
            ButtonsPanel.Controls.Clear();

            switch (DialogButtons)
            {
                case DialogFormButtons.SaveCancel:
                    ButtonsPanel.Controls.Add(ButtonDialogCancel);
                    ButtonsPanel.Controls.Add(ButtonDialogSave);
                    break;

                case DialogFormButtons.Close:
                    ButtonsPanel.Controls.Add(ButtonDialogClose);
                    break;
            }

            ButtonsPanel.ResumeLayout();
            ButtonsPanel.PerformLayout();
        }

        protected virtual void OnButtonCloseClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        protected virtual void OnButtonCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        protected virtual void OnButtonSaveClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
