﻿
namespace 智能藥庫系統
{
    partial class Dialog_盤點處方量
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dialog_盤點處方量));
            this.sqL_DataGridView_處方量 = new SQLUI.SQL_DataGridView();
            this.rJ_Button_確認 = new MyUI.RJ_Button();
            this.rJ_Button_取消 = new MyUI.RJ_Button();
            this.plC_RJ_Button_上傳Excel = new MyUI.PLC_RJ_Button();
            this.openFileDialog_LoadExcel = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // sqL_DataGridView_處方量
            // 
            this.sqL_DataGridView_處方量.AutoSelectToDeep = true;
            this.sqL_DataGridView_處方量.backColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_處方量.BorderColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_處方量.BorderRadius = 10;
            this.sqL_DataGridView_處方量.BorderSize = 2;
            this.sqL_DataGridView_處方量.cellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.sqL_DataGridView_處方量.cellStylBackColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_處方量.cellStyleFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_處方量.cellStylForeColor = System.Drawing.Color.Black;
            this.sqL_DataGridView_處方量.columnHeaderBackColor = System.Drawing.Color.SkyBlue;
            this.sqL_DataGridView_處方量.columnHeaderFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_處方量.columnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            this.sqL_DataGridView_處方量.columnHeadersHeight = 26;
            this.sqL_DataGridView_處方量.columnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sqL_DataGridView_處方量.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_處方量.Columns"))));
            this.sqL_DataGridView_處方量.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_處方量.Columns1"))));
            this.sqL_DataGridView_處方量.Dock = System.Windows.Forms.DockStyle.Top;
            this.sqL_DataGridView_處方量.Font = new System.Drawing.Font("新細明體", 9F);
            this.sqL_DataGridView_處方量.ImageBox = false;
            this.sqL_DataGridView_處方量.Location = new System.Drawing.Point(0, 0);
            this.sqL_DataGridView_處方量.Name = "sqL_DataGridView_處方量";
            this.sqL_DataGridView_處方量.OnlineState = SQLUI.SQL_DataGridView.OnlineEnum.Online;
            this.sqL_DataGridView_處方量.Password = "user82822040";
            this.sqL_DataGridView_處方量.Port = ((uint)(3306u));
            this.sqL_DataGridView_處方量.rowHeaderBackColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_處方量.rowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            this.sqL_DataGridView_處方量.RowsColor = System.Drawing.Color.White;
            this.sqL_DataGridView_處方量.RowsHeight = 40;
            this.sqL_DataGridView_處方量.SaveFileName = "SQL_DataGridView";
            this.sqL_DataGridView_處方量.Server = "127.0.0.0";
            this.sqL_DataGridView_處方量.Size = new System.Drawing.Size(1392, 820);
            this.sqL_DataGridView_處方量.SSLMode = MySql.Data.MySqlClient.MySqlSslMode.None;
            this.sqL_DataGridView_處方量.TabIndex = 3;
            this.sqL_DataGridView_處方量.UserName = "root";
            this.sqL_DataGridView_處方量.可拖曳欄位寬度 = false;
            this.sqL_DataGridView_處方量.可選擇多列 = false;
            this.sqL_DataGridView_處方量.單格樣式 = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.sqL_DataGridView_處方量.自動換行 = true;
            this.sqL_DataGridView_處方量.表單字體 = new System.Drawing.Font("新細明體", 9F);
            this.sqL_DataGridView_處方量.邊框樣式 = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sqL_DataGridView_處方量.顯示CheckBox = false;
            this.sqL_DataGridView_處方量.顯示首列 = true;
            this.sqL_DataGridView_處方量.顯示首行 = true;
            this.sqL_DataGridView_處方量.首列樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            this.sqL_DataGridView_處方量.首行樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
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
            this.rJ_Button_確認.Dock = System.Windows.Forms.DockStyle.Right;
            this.rJ_Button_確認.FlatAppearance.BorderSize = 0;
            this.rJ_Button_確認.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_確認.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.rJ_Button_確認.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_確認.GUID = "";
            this.rJ_Button_確認.Location = new System.Drawing.Point(1112, 820);
            this.rJ_Button_確認.Name = "rJ_Button_確認";
            this.rJ_Button_確認.Size = new System.Drawing.Size(140, 111);
            this.rJ_Button_確認.State = false;
            this.rJ_Button_確認.TabIndex = 141;
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
            this.rJ_Button_取消.Dock = System.Windows.Forms.DockStyle.Right;
            this.rJ_Button_取消.FlatAppearance.BorderSize = 0;
            this.rJ_Button_取消.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_取消.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Button_取消.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_取消.GUID = "";
            this.rJ_Button_取消.Location = new System.Drawing.Point(1252, 820);
            this.rJ_Button_取消.Name = "rJ_Button_取消";
            this.rJ_Button_取消.Size = new System.Drawing.Size(140, 111);
            this.rJ_Button_取消.State = false;
            this.rJ_Button_取消.TabIndex = 140;
            this.rJ_Button_取消.Text = "取消";
            this.rJ_Button_取消.TextColor = System.Drawing.Color.White;
            this.rJ_Button_取消.UseVisualStyleBackColor = false;
            // 
            // plC_RJ_Button_上傳Excel
            // 
            this.plC_RJ_Button_上傳Excel.AutoResetState = false;
            this.plC_RJ_Button_上傳Excel.BackgroundColor = System.Drawing.Color.RoyalBlue;
            this.plC_RJ_Button_上傳Excel.Bool = false;
            this.plC_RJ_Button_上傳Excel.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.plC_RJ_Button_上傳Excel.BorderRadius = 5;
            this.plC_RJ_Button_上傳Excel.BorderSize = 0;
            this.plC_RJ_Button_上傳Excel.but_press = false;
            this.plC_RJ_Button_上傳Excel.buttonType = MyUI.RJ_Button.ButtonType.Toggle;
            this.plC_RJ_Button_上傳Excel.FlatAppearance.BorderSize = 0;
            this.plC_RJ_Button_上傳Excel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plC_RJ_Button_上傳Excel.Font = new System.Drawing.Font("微軟正黑體", 14F);
            this.plC_RJ_Button_上傳Excel.GUID = "";
            this.plC_RJ_Button_上傳Excel.Icon = System.Windows.Forms.MessageBoxIcon.Warning;
            this.plC_RJ_Button_上傳Excel.Location = new System.Drawing.Point(12, 826);
            this.plC_RJ_Button_上傳Excel.Name = "plC_RJ_Button_上傳Excel";
            this.plC_RJ_Button_上傳Excel.OFF_文字內容 = "上傳Excel";
            this.plC_RJ_Button_上傳Excel.OFF_文字字體 = new System.Drawing.Font("微軟正黑體", 14F);
            this.plC_RJ_Button_上傳Excel.OFF_文字顏色 = System.Drawing.Color.White;
            this.plC_RJ_Button_上傳Excel.OFF_背景顏色 = System.Drawing.SystemColors.Control;
            this.plC_RJ_Button_上傳Excel.ON_BorderSize = 5;
            this.plC_RJ_Button_上傳Excel.ON_文字內容 = "上傳Excel";
            this.plC_RJ_Button_上傳Excel.ON_文字字體 = new System.Drawing.Font("微軟正黑體", 14F);
            this.plC_RJ_Button_上傳Excel.ON_文字顏色 = System.Drawing.Color.Black;
            this.plC_RJ_Button_上傳Excel.ON_背景顏色 = System.Drawing.SystemColors.Control;
            this.plC_RJ_Button_上傳Excel.Size = new System.Drawing.Size(139, 89);
            this.plC_RJ_Button_上傳Excel.State = false;
            this.plC_RJ_Button_上傳Excel.TabIndex = 139;
            this.plC_RJ_Button_上傳Excel.Text = "上傳Excel";
            this.plC_RJ_Button_上傳Excel.TextColor = System.Drawing.Color.White;
            this.plC_RJ_Button_上傳Excel.Texts = "上傳Excel";
            this.plC_RJ_Button_上傳Excel.UseVisualStyleBackColor = false;
            this.plC_RJ_Button_上傳Excel.字型鎖住 = false;
            this.plC_RJ_Button_上傳Excel.按鈕型態 = MyUI.PLC_RJ_Button.StatusEnum.保持型;
            this.plC_RJ_Button_上傳Excel.按鍵方式 = MyUI.PLC_RJ_Button.PressEnum.Mouse_左鍵;
            this.plC_RJ_Button_上傳Excel.文字鎖住 = false;
            this.plC_RJ_Button_上傳Excel.讀取位元反向 = false;
            this.plC_RJ_Button_上傳Excel.讀寫鎖住 = false;
            this.plC_RJ_Button_上傳Excel.音效 = true;
            this.plC_RJ_Button_上傳Excel.顯示 = false;
            this.plC_RJ_Button_上傳Excel.顯示狀態 = false;
            // 
            // openFileDialog_LoadExcel
            // 
            this.openFileDialog_LoadExcel.DefaultExt = "txt";
            this.openFileDialog_LoadExcel.Filter = "Excel File (*.xlsx)|*.xlsx|txt File (*.txt)|*.txt;";
            // 
            // Dialog_盤點處方量
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1392, 931);
            this.ControlBox = false;
            this.Controls.Add(this.rJ_Button_確認);
            this.Controls.Add(this.rJ_Button_取消);
            this.Controls.Add(this.plC_RJ_Button_上傳Excel);
            this.Controls.Add(this.sqL_DataGridView_處方量);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Dialog_盤點處方量";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        private SQLUI.SQL_DataGridView sqL_DataGridView_處方量;
        private MyUI.RJ_Button rJ_Button_確認;
        private MyUI.RJ_Button rJ_Button_取消;
        private MyUI.PLC_RJ_Button plC_RJ_Button_上傳Excel;
        private System.Windows.Forms.OpenFileDialog openFileDialog_LoadExcel;
    }
}