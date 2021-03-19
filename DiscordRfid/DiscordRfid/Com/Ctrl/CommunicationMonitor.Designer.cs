
namespace DiscordRfid.Com.Ctrl
{
    partial class CommunicationMonitor
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
            this.PackagesContainer = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // PackagesContainer
            // 
            this.PackagesContainer.AutoSize = true;
            this.PackagesContainer.ColumnCount = 1;
            this.PackagesContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PackagesContainer.Location = new System.Drawing.Point(14, 131);
            this.PackagesContainer.Name = "PackagesContainer";
            this.PackagesContainer.RowCount = 1;
            this.PackagesContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.PackagesContainer.Size = new System.Drawing.Size(265, 66);
            this.PackagesContainer.TabIndex = 0;
            // 
            // CommunicationMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.PackagesContainer);
            this.Name = "CommunicationMonitor";
            this.Size = new System.Drawing.Size(302, 211);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel PackagesContainer;
    }
}
