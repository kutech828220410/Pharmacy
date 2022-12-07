
namespace 智能藥庫系統
{
    partial class Dialog_輸入備註
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
            this.rJ_Button_確認 = new MyUI.RJ_Button();
            this.rJ_Button_取消 = new MyUI.RJ_Button();
            this.rJ_TextBox_備註 = new MyUI.RJ_TextBox();
            this.SuspendLayout();
            // 
            // rJ_Button_確認
            // 
            this.rJ_Button_確認.AutoResetState = false;
            this.rJ_Button_確認.BackColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_確認.BackgroundColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_確認.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_確認.BorderRadius = 5;
            this.rJ_Button_確認.BorderSize = 0;
            this.rJ_Button_確認.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_確認.FlatAppearance.BorderSize = 0;
            this.rJ_Button_確認.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_確認.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.rJ_Button_確認.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_確認.Location = new System.Drawing.Point(393, 15);
            this.rJ_Button_確認.Name = "rJ_Button_確認";
            this.rJ_Button_確認.Size = new System.Drawing.Size(80, 37);
            this.rJ_Button_確認.State = false;
            this.rJ_Button_確認.TabIndex = 31;
            this.rJ_Button_確認.Text = "確認";
            this.rJ_Button_確認.TextColor = System.Drawing.Color.White;
            this.rJ_Button_確認.UseVisualStyleBackColor = false;
            // 
            // rJ_Button_取消
            // 
            this.rJ_Button_取消.AutoResetState = false;
            this.rJ_Button_取消.BackColor = System.Drawing.Color.Gray;
            this.rJ_Button_取消.BackgroundColor = System.Drawing.Color.Gray;
            this.rJ_Button_取消.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_取消.BorderRadius = 5;
            this.rJ_Button_取消.BorderSize = 0;
            this.rJ_Button_取消.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_取消.FlatAppearance.BorderSize = 0;
            this.rJ_Button_取消.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_取消.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Button_取消.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_取消.Location = new System.Drawing.Point(479, 15);
            this.rJ_Button_取消.Name = "rJ_Button_取消";
            this.rJ_Button_取消.Size = new System.Drawing.Size(80, 37);
            this.rJ_Button_取消.State = false;
            this.rJ_Button_取消.TabIndex = 30;
            this.rJ_Button_取消.Text = "取消";
            this.rJ_Button_取消.TextColor = System.Drawing.Color.White;
            this.rJ_Button_取消.UseVisualStyleBackColor = false;
            // 
            // rJ_TextBox_備註
            // 
            this.rJ_TextBox_備註.BackColor = System.Drawing.SystemColors.Window;
            this.rJ_TextBox_備註.BorderColor = System.Drawing.Color.RoyalBlue;
            this.rJ_TextBox_備註.BorderFocusColor = System.Drawing.Color.HotPink;
            this.rJ_TextBox_備註.BorderRadius = 0;
            this.rJ_TextBox_備註.BorderSize = 2;
            this.rJ_TextBox_備註.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_TextBox_備註.ForeColor = System.Drawing.Color.DimGray;
            this.rJ_TextBox_備註.Location = new System.Drawing.Point(12, 12);
            this.rJ_TextBox_備註.Multiline = false;
            this.rJ_TextBox_備註.Name = "rJ_TextBox_備註";
            this.rJ_TextBox_備註.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.rJ_TextBox_備註.PassWordChar = false;
            this.rJ_TextBox_備註.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.rJ_TextBox_備註.PlaceholderText = "寫入備註....";
            this.rJ_TextBox_備註.ShowTouchPannel = false;
            this.rJ_TextBox_備註.Size = new System.Drawing.Size(351, 42);
            this.rJ_TextBox_備註.TabIndex = 29;
            this.rJ_TextBox_備註.TextAlgin = System.Windows.Forms.HorizontalAlignment.Left;
            this.rJ_TextBox_備註.Texts = "";
            this.rJ_TextBox_備註.UnderlineStyle = false;
            // 
            // Dialog_輸入備註
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(571, 66);
            this.ControlBox = false;
            this.Controls.Add(this.rJ_Button_確認);
            this.Controls.Add(this.rJ_Button_取消);
            this.Controls.Add(this.rJ_TextBox_備註);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Dialog_輸入備註";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Dialog_輸入備註_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MyUI.RJ_Button rJ_Button_確認;
        private MyUI.RJ_Button rJ_Button_取消;
        private MyUI.RJ_TextBox rJ_TextBox_備註;
    }
}