
namespace 智能藥庫系統
{
    partial class Dialog_盤點單合併_設定
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
            this.comboBox_庫別 = new System.Windows.Forms.ComboBox();
            this.rJ_Lable2 = new MyUI.RJ_Lable();
            this.rJ_Lable1 = new MyUI.RJ_Lable();
            this.panel1 = new System.Windows.Forms.Panel();
            this.plC_RJ_Button_確認 = new MyUI.PLC_RJ_Button();
            this.rJ_DatePicker_盤點日 = new MyUI.RJ_DatePicker();
            this.panel_盤點日選擇_controls = new System.Windows.Forms.Panel();
            this.plC_RJ_Pannel_盤點日選擇 = new MyUI.PLC_RJ_Pannel();
            this.rJ_RatioButton_盤點日選擇 = new MyUI.RJ_RatioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimeIntervelPicker_消耗量起始及結束日期 = new MyUI.DateTimeIntervelPicker();
            this.rJ_Lable3 = new MyUI.RJ_Lable();
            this.panel1.SuspendLayout();
            this.panel_盤點日選擇_controls.SuspendLayout();
            this.plC_RJ_Pannel_盤點日選擇.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_庫別
            // 
            this.comboBox_庫別.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_庫別.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_庫別.FormattingEnabled = true;
            this.comboBox_庫別.Items.AddRange(new object[] {
            "藥庫",
            "藥局"});
            this.comboBox_庫別.Location = new System.Drawing.Point(428, 37);
            this.comboBox_庫別.Name = "comboBox_庫別";
            this.comboBox_庫別.Size = new System.Drawing.Size(121, 32);
            this.comboBox_庫別.TabIndex = 12;
            // 
            // rJ_Lable2
            // 
            this.rJ_Lable2.BackColor = System.Drawing.Color.White;
            this.rJ_Lable2.BackgroundColor = System.Drawing.Color.White;
            this.rJ_Lable2.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Lable2.BorderRadius = 8;
            this.rJ_Lable2.BorderSize = 0;
            this.rJ_Lable2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Lable2.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Lable2.ForeColor = System.Drawing.Color.Transparent;
            this.rJ_Lable2.GUID = "";
            this.rJ_Lable2.Location = new System.Drawing.Point(356, 36);
            this.rJ_Lable2.Name = "rJ_Lable2";
            this.rJ_Lable2.ShadowColor = System.Drawing.Color.DimGray;
            this.rJ_Lable2.ShadowSize = 0;
            this.rJ_Lable2.Size = new System.Drawing.Size(83, 36);
            this.rJ_Lable2.TabIndex = 11;
            this.rJ_Lable2.Text = "庫別";
            this.rJ_Lable2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_Lable2.TextColor = System.Drawing.Color.Black;
            // 
            // rJ_Lable1
            // 
            this.rJ_Lable1.BackColor = System.Drawing.Color.White;
            this.rJ_Lable1.BackgroundColor = System.Drawing.Color.White;
            this.rJ_Lable1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Lable1.BorderRadius = 8;
            this.rJ_Lable1.BorderSize = 0;
            this.rJ_Lable1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Lable1.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Lable1.ForeColor = System.Drawing.Color.Transparent;
            this.rJ_Lable1.GUID = "";
            this.rJ_Lable1.Location = new System.Drawing.Point(17, 36);
            this.rJ_Lable1.Name = "rJ_Lable1";
            this.rJ_Lable1.ShadowColor = System.Drawing.Color.DimGray;
            this.rJ_Lable1.ShadowSize = 0;
            this.rJ_Lable1.Size = new System.Drawing.Size(83, 36);
            this.rJ_Lable1.TabIndex = 9;
            this.rJ_Lable1.Text = "盤點日";
            this.rJ_Lable1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_Lable1.TextColor = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.plC_RJ_Button_確認);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(4, 458);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(636, 88);
            this.panel1.TabIndex = 8;
            // 
            // plC_RJ_Button_確認
            // 
            this.plC_RJ_Button_確認.AutoResetState = false;
            this.plC_RJ_Button_確認.BackgroundColor = System.Drawing.Color.Black;
            this.plC_RJ_Button_確認.Bool = false;
            this.plC_RJ_Button_確認.BorderColor = System.Drawing.Color.Thistle;
            this.plC_RJ_Button_確認.BorderRadius = 20;
            this.plC_RJ_Button_確認.BorderSize = 0;
            this.plC_RJ_Button_確認.but_press = false;
            this.plC_RJ_Button_確認.buttonType = MyUI.RJ_Button.ButtonType.Toggle;
            this.plC_RJ_Button_確認.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.plC_RJ_Button_確認.DisenableColor = System.Drawing.Color.Gray;
            this.plC_RJ_Button_確認.Dock = System.Windows.Forms.DockStyle.Right;
            this.plC_RJ_Button_確認.FlatAppearance.BorderSize = 0;
            this.plC_RJ_Button_確認.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plC_RJ_Button_確認.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.plC_RJ_Button_確認.GUID = "";
            this.plC_RJ_Button_確認.Icon = System.Windows.Forms.MessageBoxIcon.Warning;
            this.plC_RJ_Button_確認.Image_padding = new System.Windows.Forms.Padding(0);
            this.plC_RJ_Button_確認.Location = new System.Drawing.Point(475, 0);
            this.plC_RJ_Button_確認.Name = "plC_RJ_Button_確認";
            this.plC_RJ_Button_確認.OFF_文字內容 = "確認";
            this.plC_RJ_Button_確認.OFF_文字字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.plC_RJ_Button_確認.OFF_文字顏色 = System.Drawing.Color.White;
            this.plC_RJ_Button_確認.OFF_背景顏色 = System.Drawing.Color.Black;
            this.plC_RJ_Button_確認.ON_BorderSize = 5;
            this.plC_RJ_Button_確認.ON_文字內容 = "確認";
            this.plC_RJ_Button_確認.ON_文字字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold);
            this.plC_RJ_Button_確認.ON_文字顏色 = System.Drawing.Color.White;
            this.plC_RJ_Button_確認.ON_背景顏色 = System.Drawing.Color.Black;
            this.plC_RJ_Button_確認.ProhibitionBorderLineWidth = 1;
            this.plC_RJ_Button_確認.ProhibitionLineWidth = 4;
            this.plC_RJ_Button_確認.ProhibitionSymbolSize = 30;
            this.plC_RJ_Button_確認.ShadowColor = System.Drawing.Color.DimGray;
            this.plC_RJ_Button_確認.ShadowSize = 3;
            this.plC_RJ_Button_確認.ShowLoadingForm = false;
            this.plC_RJ_Button_確認.Size = new System.Drawing.Size(161, 88);
            this.plC_RJ_Button_確認.State = false;
            this.plC_RJ_Button_確認.TabIndex = 10;
            this.plC_RJ_Button_確認.Text = "確認";
            this.plC_RJ_Button_確認.TextColor = System.Drawing.Color.White;
            this.plC_RJ_Button_確認.TextHeight = 0;
            this.plC_RJ_Button_確認.Texts = "確認";
            this.plC_RJ_Button_確認.UseVisualStyleBackColor = false;
            this.plC_RJ_Button_確認.字型鎖住 = false;
            this.plC_RJ_Button_確認.按鈕型態 = MyUI.PLC_RJ_Button.StatusEnum.保持型;
            this.plC_RJ_Button_確認.按鍵方式 = MyUI.PLC_RJ_Button.PressEnum.Mouse_左鍵;
            this.plC_RJ_Button_確認.文字鎖住 = false;
            this.plC_RJ_Button_確認.背景圖片 = null;
            this.plC_RJ_Button_確認.讀取位元反向 = false;
            this.plC_RJ_Button_確認.讀寫鎖住 = false;
            this.plC_RJ_Button_確認.音效 = false;
            this.plC_RJ_Button_確認.顯示 = false;
            this.plC_RJ_Button_確認.顯示狀態 = false;
            // 
            // rJ_DatePicker_盤點日
            // 
            this.rJ_DatePicker_盤點日.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_DatePicker_盤點日.BorderSize = 0;
            this.rJ_DatePicker_盤點日.CalendarFont = new System.Drawing.Font("新細明體", 15.75F);
            this.rJ_DatePicker_盤點日.Font = new System.Drawing.Font("新細明體", 15.75F);
            this.rJ_DatePicker_盤點日.Location = new System.Drawing.Point(97, 36);
            this.rJ_DatePicker_盤點日.MinimumSize = new System.Drawing.Size(250, 35);
            this.rJ_DatePicker_盤點日.Name = "rJ_DatePicker_盤點日";
            this.rJ_DatePicker_盤點日.PickerFont = new System.Drawing.Font("新細明體", 15.75F);
            this.rJ_DatePicker_盤點日.PickerFore = System.Drawing.SystemColors.ControlText;
            this.rJ_DatePicker_盤點日.Size = new System.Drawing.Size(250, 35);
            this.rJ_DatePicker_盤點日.SkinColor = System.Drawing.Color.DimGray;
            this.rJ_DatePicker_盤點日.TabIndex = 10;
            this.rJ_DatePicker_盤點日.TextColor = System.Drawing.Color.White;
            // 
            // panel_盤點日選擇_controls
            // 
            this.panel_盤點日選擇_controls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_盤點日選擇_controls.Controls.Add(this.plC_RJ_Pannel_盤點日選擇);
            this.panel_盤點日選擇_controls.Location = new System.Drawing.Point(33, 77);
            this.panel_盤點日選擇_controls.Name = "panel_盤點日選擇_controls";
            this.panel_盤點日選擇_controls.Padding = new System.Windows.Forms.Padding(3);
            this.panel_盤點日選擇_controls.Size = new System.Drawing.Size(516, 165);
            this.panel_盤點日選擇_controls.TabIndex = 13;
            // 
            // plC_RJ_Pannel_盤點日選擇
            // 
            this.plC_RJ_Pannel_盤點日選擇.BackColor = System.Drawing.Color.White;
            this.plC_RJ_Pannel_盤點日選擇.BackgroundColor = System.Drawing.Color.Transparent;
            this.plC_RJ_Pannel_盤點日選擇.BorderColor = System.Drawing.Color.SkyBlue;
            this.plC_RJ_Pannel_盤點日選擇.BorderRadius = 0;
            this.plC_RJ_Pannel_盤點日選擇.BorderSize = 1;
            this.plC_RJ_Pannel_盤點日選擇.Controls.Add(this.rJ_RatioButton_盤點日選擇);
            this.plC_RJ_Pannel_盤點日選擇.Dock = System.Windows.Forms.DockStyle.Top;
            this.plC_RJ_Pannel_盤點日選擇.ForeColor = System.Drawing.Color.White;
            this.plC_RJ_Pannel_盤點日選擇.IsSelected = false;
            this.plC_RJ_Pannel_盤點日選擇.Location = new System.Drawing.Point(3, 3);
            this.plC_RJ_Pannel_盤點日選擇.Name = "plC_RJ_Pannel_盤點日選擇";
            this.plC_RJ_Pannel_盤點日選擇.Padding = new System.Windows.Forms.Padding(3);
            this.plC_RJ_Pannel_盤點日選擇.ShadowColor = System.Drawing.Color.DimGray;
            this.plC_RJ_Pannel_盤點日選擇.ShadowSize = 0;
            this.plC_RJ_Pannel_盤點日選擇.Size = new System.Drawing.Size(508, 45);
            this.plC_RJ_Pannel_盤點日選擇.TabIndex = 0;
            this.plC_RJ_Pannel_盤點日選擇.Visible = false;
            // 
            // rJ_RatioButton_盤點日選擇
            // 
            this.rJ_RatioButton_盤點日選擇.AutoSize = true;
            this.rJ_RatioButton_盤點日選擇.CheckColor = System.Drawing.Color.Green;
            this.rJ_RatioButton_盤點日選擇.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rJ_RatioButton_盤點日選擇.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_RatioButton_盤點日選擇.ForeColor = System.Drawing.Color.Black;
            this.rJ_RatioButton_盤點日選擇.Location = new System.Drawing.Point(3, 3);
            this.rJ_RatioButton_盤點日選擇.MinimumSize = new System.Drawing.Size(0, 21);
            this.rJ_RatioButton_盤點日選擇.Name = "rJ_RatioButton_盤點日選擇";
            this.rJ_RatioButton_盤點日選擇.Size = new System.Drawing.Size(249, 39);
            this.rJ_RatioButton_盤點日選擇.TabIndex = 0;
            this.rJ_RatioButton_盤點日選擇.TabStop = true;
            this.rJ_RatioButton_盤點日選擇.Text = "------------------------------";
            this.rJ_RatioButton_盤點日選擇.UncheckColor = System.Drawing.Color.Gray;
            this.rJ_RatioButton_盤點日選擇.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rJ_Lable1);
            this.groupBox1.Controls.Add(this.panel_盤點日選擇_controls);
            this.groupBox1.Controls.Add(this.rJ_DatePicker_盤點日);
            this.groupBox1.Controls.Add(this.comboBox_庫別);
            this.groupBox1.Controls.Add(this.rJ_Lable2);
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(21, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(589, 266);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "庫存截止日";
            // 
            // dateTimeIntervelPicker_消耗量起始及結束日期
            // 
            this.dateTimeIntervelPicker_消耗量起始及結束日期.BackColor = System.Drawing.Color.Transparent;
            this.dateTimeIntervelPicker_消耗量起始及結束日期.EndTime = new System.DateTime(2024, 4, 12, 0, 0, 0, 0);
            this.dateTimeIntervelPicker_消耗量起始及結束日期.Location = new System.Drawing.Point(21, 357);
            this.dateTimeIntervelPicker_消耗量起始及結束日期.Name = "dateTimeIntervelPicker_消耗量起始及結束日期";
            this.dateTimeIntervelPicker_消耗量起始及結束日期.Size = new System.Drawing.Size(306, 83);
            this.dateTimeIntervelPicker_消耗量起始及結束日期.StartTime = new System.DateTime(2024, 4, 12, 0, 0, 0, 0);
            this.dateTimeIntervelPicker_消耗量起始及結束日期.TabIndex = 15;
            // 
            // rJ_Lable3
            // 
            this.rJ_Lable3.BackColor = System.Drawing.Color.White;
            this.rJ_Lable3.BackgroundColor = System.Drawing.Color.White;
            this.rJ_Lable3.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Lable3.BorderRadius = 8;
            this.rJ_Lable3.BorderSize = 0;
            this.rJ_Lable3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Lable3.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Lable3.ForeColor = System.Drawing.Color.Transparent;
            this.rJ_Lable3.GUID = "";
            this.rJ_Lable3.Location = new System.Drawing.Point(17, 318);
            this.rJ_Lable3.Name = "rJ_Lable3";
            this.rJ_Lable3.ShadowColor = System.Drawing.Color.DimGray;
            this.rJ_Lable3.ShadowSize = 0;
            this.rJ_Lable3.Size = new System.Drawing.Size(218, 36);
            this.rJ_Lable3.TabIndex = 16;
            this.rJ_Lable3.Text = "消耗量起始及結束日期";
            this.rJ_Lable3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_Lable3.TextColor = System.Drawing.Color.Black;
            // 
            // Dialog_盤點單合併_設定
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 550);
            this.ControlBox = true;
            this.Controls.Add(this.rJ_Lable3);
            this.Controls.Add(this.dateTimeIntervelPicker_消耗量起始及結束日期);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "Dialog_盤點單合併_設定";
            this.Text = "設定";
            this.panel1.ResumeLayout(false);
            this.panel_盤點日選擇_controls.ResumeLayout(false);
            this.plC_RJ_Pannel_盤點日選擇.ResumeLayout(false);
            this.plC_RJ_Pannel_盤點日選擇.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MyUI.PLC_RJ_Button plC_RJ_Button_確認;
        private MyUI.RJ_Lable rJ_Lable1;
        private MyUI.RJ_DatePicker rJ_DatePicker_盤點日;
        private MyUI.RJ_Lable rJ_Lable2;
        private System.Windows.Forms.ComboBox comboBox_庫別;
        private System.Windows.Forms.Panel panel_盤點日選擇_controls;
        private MyUI.PLC_RJ_Pannel plC_RJ_Pannel_盤點日選擇;
        private MyUI.RJ_RatioButton rJ_RatioButton_盤點日選擇;
        private System.Windows.Forms.GroupBox groupBox1;
        private MyUI.DateTimeIntervelPicker dateTimeIntervelPicker_消耗量起始及結束日期;
        private MyUI.RJ_Lable rJ_Lable3;
    }
}