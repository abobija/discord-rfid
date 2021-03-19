
namespace DiscordRfid.Com.Ctrl
{
    partial class CommunicationMonitorListItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LblSerialNumber = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LblType = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblSerialNumber
            // 
            this.LblSerialNumber.AutoSize = true;
            this.LblSerialNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblSerialNumber.Location = new System.Drawing.Point(40, 0);
            this.LblSerialNumber.Name = "LblSerialNumber";
            this.LblSerialNumber.Size = new System.Drawing.Size(195, 33);
            this.LblSerialNumber.TabIndex = 2;
            this.LblSerialNumber.Text = "Serial Number";
            this.LblSerialNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.LblSerialNumber, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.LblType, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(238, 33);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // LblType
            // 
            this.LblType.AutoSize = true;
            this.LblType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblType.Location = new System.Drawing.Point(3, 0);
            this.LblType.Name = "LblType";
            this.LblType.Size = new System.Drawing.Size(31, 33);
            this.LblType.TabIndex = 3;
            this.LblType.Text = "Type";
            this.LblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CommunicationMonitorListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "CommunicationMonitorListItem";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(244, 39);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label LblSerialNumber;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label LblType;
    }
}
