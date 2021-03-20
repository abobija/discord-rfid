
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
            this.ToolBtnEmployees = new System.Windows.Forms.ToolStripButton();
            this.ToolLblClock = new System.Windows.Forms.ToolStripLabel();
            this.PnlMain = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.PackagesListView = new DiscordRfid.PackagesListView();
            this.ToolBtnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.PnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LblBotName,
            this.LblServerName,
            this.LblState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 466);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(904, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LblBotName
            // 
            this.LblBotName.BackColor = System.Drawing.Color.Transparent;
            this.LblBotName.Name = "LblBotName";
            this.LblBotName.Size = new System.Drawing.Size(57, 17);
            this.LblBotName.Text = "BotName";
            // 
            // LblServerName
            // 
            this.LblServerName.BackColor = System.Drawing.Color.Transparent;
            this.LblServerName.Name = "LblServerName";
            this.LblServerName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LblServerName.Size = new System.Drawing.Size(71, 17);
            this.LblServerName.Text = "ServerName";
            // 
            // LblState
            // 
            this.LblState.BackColor = System.Drawing.Color.Transparent;
            this.LblState.Name = "LblState";
            this.LblState.Size = new System.Drawing.Size(33, 17);
            this.LblState.Text = "State";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.ToolBtnEmployees,
            this.ToolLblClock});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(904, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolBtnAbout,
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
            this.ToolBtnExit.Size = new System.Drawing.Size(180, 22);
            this.ToolBtnExit.Text = "Exit";
            this.ToolBtnExit.Click += new System.EventHandler(this.ToolBtnExit_Click);
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
            // ToolLblClock
            // 
            this.ToolLblClock.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolLblClock.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.ToolLblClock.Name = "ToolLblClock";
            this.ToolLblClock.Size = new System.Drawing.Size(36, 22);
            this.ToolLblClock.Text = "Clock";
            // 
            // PnlMain
            // 
            this.PnlMain.Controls.Add(this.panel1);
            this.PnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlMain.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PnlMain.Location = new System.Drawing.Point(0, 25);
            this.PnlMain.Name = "PnlMain";
            this.PnlMain.Size = new System.Drawing.Size(904, 441);
            this.PnlMain.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.PackagesListView);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(598, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.panel1.Size = new System.Drawing.Size(306, 441);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Recent packages";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PackagesListView
            // 
            this.PackagesListView.BackColor = System.Drawing.Color.Gainsboro;
            this.PackagesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PackagesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PackagesListView.FullRowSelect = true;
            this.PackagesListView.GridLines = true;
            this.PackagesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.PackagesListView.HideSelection = false;
            this.PackagesListView.Location = new System.Drawing.Point(3, 32);
            this.PackagesListView.MultiSelect = false;
            this.PackagesListView.Name = "PackagesListView";
            this.PackagesListView.Size = new System.Drawing.Size(303, 409);
            this.PackagesListView.TabIndex = 1;
            this.PackagesListView.UseCompatibleStateImageBehavior = false;
            this.PackagesListView.View = System.Windows.Forms.View.Details;
            // 
            // ToolBtnAbout
            // 
            this.ToolBtnAbout.Name = "ToolBtnAbout";
            this.ToolBtnAbout.Size = new System.Drawing.Size(180, 22);
            this.ToolBtnAbout.Text = "About";
            this.ToolBtnAbout.Click += new System.EventHandler(this.ToolBtnAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(904, 488);
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
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.Panel PnlMain;
        private System.Windows.Forms.ToolStripLabel ToolLblClock;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private PackagesListView PackagesListView;
        private System.Windows.Forms.ToolStripMenuItem ToolBtnAbout;
    }
}

