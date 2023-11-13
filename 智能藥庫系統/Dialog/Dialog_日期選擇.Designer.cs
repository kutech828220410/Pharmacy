
namespace 智能藥庫系統
{
    partial class Dialog_日期選擇
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
            this.dateTimeComList = new MyUI.DateTimeComList();
            this.rJ_Button_確認 = new MyUI.RJ_Button();
            this.SuspendLayout();
            // 
            // dateTimeComList
            // 
            this.dateTimeComList.BackColor = System.Drawing.SystemColors.Window;
            this.dateTimeComList.Day = 13;
            this.dateTimeComList.End_Year = 2030;
            this.dateTimeComList.Location = new System.Drawing.Point(28, 19);
            this.dateTimeComList.mFont = new System.Drawing.Font("標楷體", 18F);
            this.dateTimeComList.Month = 11;
            this.dateTimeComList.Name = "dateTimeComList";
            this.dateTimeComList.Size = new System.Drawing.Size(314, 75);
            this.dateTimeComList.Start_Year = 2022;
            this.dateTimeComList.TabIndex = 0;
            this.dateTimeComList.Value = new System.DateTime(2023, 11, 13, 0, 0, 0, 0);
            this.dateTimeComList.Year = 2023;
            // 
            // rJ_Button_確認
            // 
            this.rJ_Button_確認.AutoResetState = false;
            this.rJ_Button_確認.BackColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_確認.BackgroundColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_確認.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_確認.BorderRadius = 10;
            this.rJ_Button_確認.BorderSize = 0;
            this.rJ_Button_確認.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_確認.FlatAppearance.BorderSize = 0;
            this.rJ_Button_確認.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_確認.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Button_確認.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_確認.GUID = "";
            this.rJ_Button_確認.Location = new System.Drawing.Point(367, 19);
            this.rJ_Button_確認.Name = "rJ_Button_確認";
            this.rJ_Button_確認.ShowLoadingForm = false;
            this.rJ_Button_確認.Size = new System.Drawing.Size(103, 55);
            this.rJ_Button_確認.State = false;
            this.rJ_Button_確認.TabIndex = 1;
            this.rJ_Button_確認.Text = "確認";
            this.rJ_Button_確認.TextColor = System.Drawing.Color.White;
            this.rJ_Button_確認.UseVisualStyleBackColor = false;
            // 
            // Dialog_日期選擇
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(506, 89);
            this.Controls.Add(this.rJ_Button_確認);
            this.Controls.Add(this.dateTimeComList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Dialog_日期選擇";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }

        #endregion

        private MyUI.DateTimeComList dateTimeComList;
        private MyUI.RJ_Button rJ_Button_確認;
    }
}