namespace OrderShipping
{
    partial class Shipping
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
            this.txtBookingID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_UpdateStatus = new System.Windows.Forms.Button();
            this.cmbOrderStatus = new System.Windows.Forms.ComboBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSignedBy = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBookingID
            // 
            this.txtBookingID.Location = new System.Drawing.Point(147, 42);
            this.txtBookingID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBookingID.Name = "txtBookingID";
            this.txtBookingID.Size = new System.Drawing.Size(319, 26);
            this.txtBookingID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Booking#";
            // 
            // btn_UpdateStatus
            // 
            this.btn_UpdateStatus.Location = new System.Drawing.Point(147, 336);
            this.btn_UpdateStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_UpdateStatus.Name = "btn_UpdateStatus";
            this.btn_UpdateStatus.Size = new System.Drawing.Size(210, 52);
            this.btn_UpdateStatus.TabIndex = 2;
            this.btn_UpdateStatus.Text = "Update Status";
            this.btn_UpdateStatus.UseVisualStyleBackColor = true;
            this.btn_UpdateStatus.Click += new System.EventHandler(this.btn_UpdateStatus_Click);
            // 
            // cmbOrderStatus
            // 
            this.cmbOrderStatus.Items.AddRange(new object[] {
            "Order Picked",
            "Order In Transit",
            "Order Delivered"});
            this.cmbOrderStatus.Location = new System.Drawing.Point(147, 194);
            this.cmbOrderStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbOrderStatus.Name = "cmbOrderStatus";
            this.cmbOrderStatus.Size = new System.Drawing.Size(319, 28);
            this.cmbOrderStatus.TabIndex = 3;
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(147, 96);
            this.txtDesc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(319, 75);
            this.txtDesc.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Desc";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Signed By";
            // 
            // txtSignedBy
            // 
            this.txtSignedBy.Location = new System.Drawing.Point(147, 260);
            this.txtSignedBy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSignedBy.Name = "txtSignedBy";
            this.txtSignedBy.Size = new System.Drawing.Size(319, 26);
            this.txtSignedBy.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(144, 448);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(112, 29);
            this.lblStatus.TabIndex = 10;
            // 
            // Shipping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 523);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSignedBy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.cmbOrderStatus);
            this.Controls.Add(this.btn_UpdateStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBookingID);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Shipping";
            this.Text = "Shipping";
            this.Load += new System.EventHandler(this.Shipping_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBookingID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_UpdateStatus;
        private System.Windows.Forms.ComboBox cmbOrderStatus;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSignedBy;
        private System.Windows.Forms.Label lblStatus;
    }
}

