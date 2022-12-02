namespace Update
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox53 = new System.Windows.Forms.GroupBox();
            this.button_Update = new System.Windows.Forms.Button();
            this.ftp_DounloadUI1 = new MyFtpUI.Ftp_DounloadUI();
            this.groupBox53.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox53
            // 
            this.groupBox53.Controls.Add(this.button_Update);
            this.groupBox53.Controls.Add(this.ftp_DounloadUI1);
            this.groupBox53.Location = new System.Drawing.Point(12, 12);
            this.groupBox53.Name = "groupBox53";
            this.groupBox53.Size = new System.Drawing.Size(641, 148);
            this.groupBox53.TabIndex = 19;
            this.groupBox53.TabStop = false;
            this.groupBox53.Text = "系統更新";
            // 
            // button_Update
            // 
            this.button_Update.BackgroundImage = global::Update.Properties.Resources.Update;
            this.button_Update.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Update.Location = new System.Drawing.Point(572, 89);
            this.button_Update.Name = "button_Update";
            this.button_Update.Size = new System.Drawing.Size(63, 53);
            this.button_Update.TabIndex = 13;
            this.button_Update.UseVisualStyleBackColor = true;
            this.button_Update.Click += new System.EventHandler(this.button_Update_Click);
            // 
            // ftp_DounloadUI1
            // 
            this.ftp_DounloadUI1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ftp_DounloadUI1.FileName = "Setup.msi";
            this.ftp_DounloadUI1.FTP_Groupbox_要顯示 = false;
            this.ftp_DounloadUI1.FTP_Server = "ftp://103.1.221.188//高雄榮總(屏東分院)/藥品補給系統";
            this.ftp_DounloadUI1.FTP_Server_要顯示 = false;
            this.ftp_DounloadUI1.Location = new System.Drawing.Point(6, 26);
            this.ftp_DounloadUI1.Name = "ftp_DounloadUI1";
            this.ftp_DounloadUI1.Password = "66437068";
            this.ftp_DounloadUI1.Password_要顯示 = false;
            this.ftp_DounloadUI1.Size = new System.Drawing.Size(431, 111);
            this.ftp_DounloadUI1.TabIndex = 12;
            this.ftp_DounloadUI1.Username = "user";
            this.ftp_DounloadUI1.Username_要顯示 = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 172);
            this.Controls.Add(this.groupBox53);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Update";
            this.groupBox53.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox53;
        private System.Windows.Forms.Button button_Update;
        private MyFtpUI.Ftp_DounloadUI ftp_DounloadUI1;
    }
}

