namespace DiscordRfid.Views
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
            this.ToolBtnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolBtnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolLblClock = new System.Windows.Forms.ToolStripLabel();
            this.ToolBtnNewEmployee = new System.Windows.Forms.ToolStripButton();
            this.PnlMain = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.LinkLblEmployees = new System.Windows.Forms.LinkLabel();
            this.LblCounterEmployeesTotal = new System.Windows.Forms.Label();
            this.LblAbsent = new System.Windows.Forms.Label();
            this.LblCounterEmployeesAbsent = new System.Windows.Forms.Label();
            this.LblPresent = new System.Windows.Forms.Label();
            this.LblCounterEmployeesPresent = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PackagesListView = new DiscordRfid.Views.Controls.PackagesListView();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.PnlMain.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.ToolLblClock,
            this.ToolBtnNewEmployee});
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
            // ToolBtnAbout
            // 
            this.ToolBtnAbout.Name = "ToolBtnAbout";
            this.ToolBtnAbout.Size = new System.Drawing.Size(107, 22);
            this.ToolBtnAbout.Text = "About";
            this.ToolBtnAbout.Click += new System.EventHandler(this.ToolBtnAbout_Click);
            // 
            // ToolBtnExit
            // 
            this.ToolBtnExit.Name = "ToolBtnExit";
            this.ToolBtnExit.Size = new System.Drawing.Size(107, 22);
            this.ToolBtnExit.Text = "Exit";
            this.ToolBtnExit.Click += new System.EventHandler(this.ToolBtnExit_Click);
            // 
            // ToolLblClock
            // 
            this.ToolLblClock.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolLblClock.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.ToolLblClock.Name = "ToolLblClock";
            this.ToolLblClock.Size = new System.Drawing.Size(36, 22);
            this.ToolLblClock.Text = "Clock";
            // 
            // ToolBtnNewEmployee
            // 
            this.ToolBtnNewEmployee.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolBtnNewEmployee.Image = ((System.Drawing.Image)(resources.GetObject("ToolBtnNewEmployee.Image")));
            this.ToolBtnNewEmployee.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolBtnNewEmployee.Name = "ToolBtnNewEmployee";
            this.ToolBtnNewEmployee.Size = new System.Drawing.Size(90, 22);
            this.ToolBtnNewEmployee.Text = "New employee";
            this.ToolBtnNewEmployee.Click += new System.EventHandler(this.ToolBtnNewEmployee_Click);
            // 
            // PnlMain
            // 
            this.PnlMain.Controls.Add(this.flowLayoutPanel1);
            this.PnlMain.Controls.Add(this.panel1);
            this.PnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlMain.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PnlMain.Location = new System.Drawing.Point(0, 25);
            this.PnlMain.Name = "PnlMain";
            this.PnlMain.Size = new System.Drawing.Size(904, 441);
            this.PnlMain.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.LinkLblEmployees);
            this.flowLayoutPanel1.Controls.Add(this.LblCounterEmployeesTotal);
            this.flowLayoutPanel1.Controls.Add(this.LblAbsent);
            this.flowLayoutPanel1.Controls.Add(this.LblCounterEmployeesAbsent);
            this.flowLayoutPanel1.Controls.Add(this.LblPresent);
            this.flowLayoutPanel1.Controls.Add(this.LblCounterEmployeesPresent);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(21, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(375, 20);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // LinkLblEmployees
            // 
            this.LinkLblEmployees.AutoSize = true;
            this.LinkLblEmployees.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F);
            this.LinkLblEmployees.Location = new System.Drawing.Point(3, 0);
            this.LinkLblEmployees.Name = "LinkLblEmployees";
            this.LinkLblEmployees.Size = new System.Drawing.Size(82, 20);
            this.LinkLblEmployees.TabIndex = 5;
            this.LinkLblEmployees.TabStop = true;
            this.LinkLblEmployees.Text = "Employees";
            this.LinkLblEmployees.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLblEmployees_LinkClicked);
            // 
            // LblCounterEmployeesTotal
            // 
            this.LblCounterEmployeesTotal.AutoSize = true;
            this.LblCounterEmployeesTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F);
            this.LblCounterEmployeesTotal.Location = new System.Drawing.Point(91, 0);
            this.LblCounterEmployeesTotal.Name = "LblCounterEmployeesTotal";
            this.LblCounterEmployeesTotal.Size = new System.Drawing.Size(17, 20);
            this.LblCounterEmployeesTotal.TabIndex = 8;
            this.LblCounterEmployeesTotal.Text = "5";
            // 
            // LblAbsent
            // 
            this.LblAbsent.AutoSize = true;
            this.LblAbsent.Location = new System.Drawing.Point(161, 0);
            this.LblAbsent.Margin = new System.Windows.Forms.Padding(50, 0, 3, 0);
            this.LblAbsent.Name = "LblAbsent";
            this.LblAbsent.Size = new System.Drawing.Size(55, 20);
            this.LblAbsent.TabIndex = 7;
            this.LblAbsent.Text = "Absent";
            // 
            // LblCounterEmployeesAbsent
            // 
            this.LblCounterEmployeesAbsent.AutoSize = true;
            this.LblCounterEmployeesAbsent.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F);
            this.LblCounterEmployeesAbsent.Location = new System.Drawing.Point(222, 0);
            this.LblCounterEmployeesAbsent.Name = "LblCounterEmployeesAbsent";
            this.LblCounterEmployeesAbsent.Size = new System.Drawing.Size(17, 20);
            this.LblCounterEmployeesAbsent.TabIndex = 10;
            this.LblCounterEmployeesAbsent.Text = "2";
            // 
            // LblPresent
            // 
            this.LblPresent.AutoSize = true;
            this.LblPresent.Location = new System.Drawing.Point(292, 0);
            this.LblPresent.Margin = new System.Windows.Forms.Padding(50, 0, 3, 0);
            this.LblPresent.Name = "LblPresent";
            this.LblPresent.Size = new System.Drawing.Size(57, 20);
            this.LblPresent.TabIndex = 6;
            this.LblPresent.Text = "Present";
            // 
            // LblCounterEmployeesPresent
            // 
            this.LblCounterEmployeesPresent.AutoSize = true;
            this.LblCounterEmployeesPresent.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F);
            this.LblCounterEmployeesPresent.Location = new System.Drawing.Point(355, 0);
            this.LblCounterEmployeesPresent.Name = "LblCounterEmployeesPresent";
            this.LblCounterEmployeesPresent.Size = new System.Drawing.Size(17, 20);
            this.LblCounterEmployeesPresent.TabIndex = 9;
            this.LblCounterEmployeesPresent.Text = "3";
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
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Recent RFID packages";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.PnlMain.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.Panel PnlMain;
        private System.Windows.Forms.ToolStripLabel ToolLblClock;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private DiscordRfid.Views.Controls.PackagesListView PackagesListView;
        private System.Windows.Forms.ToolStripMenuItem ToolBtnAbout;
        private System.Windows.Forms.Label LblCounterEmployeesAbsent;
        private System.Windows.Forms.Label LblCounterEmployeesPresent;
        private System.Windows.Forms.Label LblCounterEmployeesTotal;
        private System.Windows.Forms.Label LblAbsent;
        private System.Windows.Forms.Label LblPresent;
        private System.Windows.Forms.LinkLabel LinkLblEmployees;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStripButton ToolBtnNewEmployee;
    }
}

