
namespace DiscordRfid
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.LblBotName = new System.Windows.Forms.ToolStripStatusLabel();
            this.LblServerName = new System.Windows.Forms.ToolStripStatusLabel();
            this.LblState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolBtnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.PnlMain = new System.Windows.Forms.Panel();
            this.ToolBtnEmployees = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolBtnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolLblClock = new System.Windows.Forms.ToolStripLabel();
            this.communicationMonitor1 = new DiscordRfid.Com.Ctrl.CommunicationMonitor();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.PnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LblBotName,
            this.LblServerName,
            this.LblState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 411);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(712, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LblBotName
            // 
            this.LblBotName.Name = "LblBotName";
            this.LblBotName.Size = new System.Drawing.Size(57, 17);
            this.LblBotName.Text = "BotName";
            // 
            // LblServerName
            // 
            this.LblServerName.Name = "LblServerName";
            this.LblServerName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LblServerName.Size = new System.Drawing.Size(71, 17);
            this.LblServerName.Text = "ServerName";
            // 
            // LblState
            // 
            this.LblState.Name = "LblState";
            this.LblState.Size = new System.Drawing.Size(33, 17);
            this.LblState.Text = "State";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.ToolBtnEmployees,
            this.toolStripDropDownButton2,
            this.ToolLblClock});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(712, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolBtnExit});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // ToolBtnExit
            // 
            this.ToolBtnExit.Name = "ToolBtnExit";
            this.ToolBtnExit.Size = new System.Drawing.Size(93, 22);
            this.ToolBtnExit.Text = "Exit";
            this.ToolBtnExit.Click += new System.EventHandler(this.ToolBtnExit_Click);
            // 
            // PnlMain
            // 
            this.PnlMain.Controls.Add(this.communicationMonitor1);
            this.PnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlMain.Location = new System.Drawing.Point(0, 25);
            this.PnlMain.Name = "PnlMain";
            this.PnlMain.Size = new System.Drawing.Size(712, 386);
            this.PnlMain.TabIndex = 3;
            // 
            // ToolBtnEmployees
            // 
            this.ToolBtnEmployees.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolBtnEmployees.Image = ((System.Drawing.Image)(resources.GetObject("ToolBtnEmployees.Image")));
            this.ToolBtnEmployees.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolBtnEmployees.Name = "ToolBtnEmployees";
            this.ToolBtnEmployees.Size = new System.Drawing.Size(68, 22);
            this.ToolBtnEmployees.Text = "Employees";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolBtnAbout});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton2.Text = "Help";
            // 
            // ToolBtnAbout
            // 
            this.ToolBtnAbout.Name = "ToolBtnAbout";
            this.ToolBtnAbout.Size = new System.Drawing.Size(107, 22);
            this.ToolBtnAbout.Text = "About";
            this.ToolBtnAbout.Click += new System.EventHandler(this.ToolBtnAbout_Click);
            // 
            // ToolLblClock
            // 
            this.ToolLblClock.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolLblClock.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.ToolLblClock.Name = "ToolLblClock";
            this.ToolLblClock.Size = new System.Drawing.Size(36, 22);
            this.ToolLblClock.Text = "Clock";
            // 
            // communicationMonitor1
            // 
            this.communicationMonitor1.Dock = System.Windows.Forms.DockStyle.Right;
            this.communicationMonitor1.Location = new System.Drawing.Point(496, 0);
            this.communicationMonitor1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.communicationMonitor1.Name = "communicationMonitor1";
            this.communicationMonitor1.Size = new System.Drawing.Size(216, 386);
            this.communicationMonitor1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(712, 433);
            this.Controls.Add(this.PnlMain);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Discord RFID";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.PnlMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel LblState;
        private System.Windows.Forms.ToolStripStatusLabel LblBotName;
        private System.Windows.Forms.ToolStripStatusLabel LblServerName;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem ToolBtnExit;
        private System.Windows.Forms.ToolStripButton ToolBtnEmployees;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem ToolBtnAbout;
        private System.Windows.Forms.Panel PnlMain;
        private System.Windows.Forms.ToolStripLabel ToolLblClock;
        private Com.Ctrl.CommunicationMonitor communicationMonitor1;
    }
}

