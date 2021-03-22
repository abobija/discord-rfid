
namespace DiscordRfid.Views
{
    partial class RfidTagForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtSerialNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LblEmployee = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(21, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial number";
            // 
            // TxtSerialNumber
            // 
            this.TxtSerialNumber.Location = new System.Drawing.Point(135, 30);
            this.TxtSerialNumber.Name = "TxtSerialNumber";
            this.TxtSerialNumber.Size = new System.Drawing.Size(170, 29);
            this.TxtSerialNumber.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(51, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Employee";
            // 
            // LblEmployee
            // 
            this.LblEmployee.AutoSize = true;
            this.LblEmployee.Location = new System.Drawing.Point(135, 74);
            this.LblEmployee.Name = "LblEmployee";
            this.LblEmployee.Size = new System.Drawing.Size(78, 21);
            this.LblEmployee.TabIndex = 5;
            this.LblEmployee.Text = "Employee";
            // 
            // RfidTagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 173);
            this.Controls.Add(this.LblEmployee);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtSerialNumber);
            this.Controls.Add(this.label1);
            this.Name = "RfidTagForm";
            this.Text = "RFID Tag";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.TxtSerialNumber, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.LblEmployee, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtSerialNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LblEmployee;
    }
}