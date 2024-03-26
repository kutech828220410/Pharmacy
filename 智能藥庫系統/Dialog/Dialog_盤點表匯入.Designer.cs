
namespace 智能藥庫系統
{
    partial class Dialog_盤點表匯入
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.plC_RJ_Button_返回 = new MyUI.PLC_RJ_Button();
            this.stepViewer1 = new MyUI.StepViewer();
            this.openFileDialog_LoadExcel = new System.Windows.Forms.OpenFileDialog();
            this.plC_RJ_Button_瀏覽 = new MyUI.PLC_RJ_Button();
            this.rJ_Lable_狀態 = new MyUI.RJ_Lable();
            this.sqL_DataGridView_盤點單 = new SQLUI.SQL_DataGridView();
            this.plC_RJ_Button_上傳 = new MyUI.PLC_RJ_Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.plC_RJ_Button_上傳);
            this.panel1.Controls.Add(this.rJ_Lable_狀態);
            this.panel1.Controls.Add(this.plC_RJ_Button_返回);
            this.panel1.Controls.Add(this.plC_RJ_Button_瀏覽);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(4, 808);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(992, 88);
            this.panel1.TabIndex = 5;
            // 
            // plC_RJ_Button_返回
            // 
            this.plC_RJ_Button_返回.AutoResetState = false;
            this.plC_RJ_Button_返回.BackgroundColor = System.Drawing.Color.Gray;
            this.plC_RJ_Button_返回.Bool = false;
            this.plC_RJ_Button_返回.BorderColor = System.Drawing.Color.Thistle;
            this.plC_RJ_Button_返回.BorderRadius = 20;
            this.plC_RJ_Button_返回.BorderSize = 0;
            this.plC_RJ_Button_返回.but_press = false;
            this.plC_RJ_Button_返回.buttonType = MyUI.RJ_Button.ButtonType.Toggle;
            this.plC_RJ_Button_返回.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.plC_RJ_Button_返回.DisenableColor = System.Drawing.Color.Gray;
            this.plC_RJ_Button_返回.Dock = System.Windows.Forms.DockStyle.Right;
            this.plC_RJ_Button_返回.FlatAppearance.BorderSize = 0;
            this.plC_RJ_Button_返回.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plC_RJ_Button_返回.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.plC_RJ_Button_返回.GUID = "";
            this.plC_RJ_Button_返回.Icon = System.Windows.Forms.MessageBoxIcon.Warning;
            this.plC_RJ_Button_返回.Image_padding = new System.Windows.Forms.Padding(0);
            this.plC_RJ_Button_返回.Location = new System.Drawing.Point(831, 0);
            this.plC_RJ_Button_返回.Name = "plC_RJ_Button_返回";
            this.plC_RJ_Button_返回.OFF_文字內容 = "返回";
            this.plC_RJ_Button_返回.OFF_文字字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.plC_RJ_Button_返回.OFF_文字顏色 = System.Drawing.Color.White;
            this.plC_RJ_Button_返回.OFF_背景顏色 = System.Drawing.Color.Gray;
            this.plC_RJ_Button_返回.ON_BorderSize = 5;
            this.plC_RJ_Button_返回.ON_文字內容 = "返回";
            this.plC_RJ_Button_返回.ON_文字字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold);
            this.plC_RJ_Button_返回.ON_文字顏色 = System.Drawing.Color.White;
            this.plC_RJ_Button_返回.ON_背景顏色 = System.Drawing.Color.Gray;
            this.plC_RJ_Button_返回.ProhibitionBorderLineWidth = 1;
            this.plC_RJ_Button_返回.ProhibitionLineWidth = 4;
            this.plC_RJ_Button_返回.ProhibitionSymbolSize = 30;
            this.plC_RJ_Button_返回.ShadowColor = System.Drawing.Color.DimGray;
            this.plC_RJ_Button_返回.ShadowSize = 3;
            this.plC_RJ_Button_返回.ShowLoadingForm = false;
            this.plC_RJ_Button_返回.Size = new System.Drawing.Size(161, 88);
            this.plC_RJ_Button_返回.State = false;
            this.plC_RJ_Button_返回.TabIndex = 10;
            this.plC_RJ_Button_返回.Text = "返回";
            this.plC_RJ_Button_返回.TextColor = System.Drawing.Color.White;
            this.plC_RJ_Button_返回.TextHeight = 0;
            this.plC_RJ_Button_返回.Texts = "返回";
            this.plC_RJ_Button_返回.UseVisualStyleBackColor = false;
            this.plC_RJ_Button_返回.字型鎖住 = false;
            this.plC_RJ_Button_返回.按鈕型態 = MyUI.PLC_RJ_Button.StatusEnum.保持型;
            this.plC_RJ_Button_返回.按鍵方式 = MyUI.PLC_RJ_Button.PressEnum.Mouse_左鍵;
            this.plC_RJ_Button_返回.文字鎖住 = false;
            this.plC_RJ_Button_返回.背景圖片 = null;
            this.plC_RJ_Button_返回.讀取位元反向 = false;
            this.plC_RJ_Button_返回.讀寫鎖住 = false;
            this.plC_RJ_Button_返回.音效 = false;
            this.plC_RJ_Button_返回.顯示 = false;
            this.plC_RJ_Button_返回.顯示狀態 = false;
            // 
            // stepViewer1
            // 
            this.stepViewer1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.stepViewer1.CurrentStep = 0;
            this.stepViewer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.stepViewer1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.stepViewer1.LineWidth = 100;
            this.stepViewer1.ListDataSource = null;
            this.stepViewer1.Location = new System.Drawing.Point(4, 34);
            this.stepViewer1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.stepViewer1.Name = "stepViewer1";
            this.stepViewer1.Size = new System.Drawing.Size(992, 116);
            this.stepViewer1.TabIndex = 7;
            // 
            // openFileDialog_LoadExcel
            // 
            this.openFileDialog_LoadExcel.DefaultExt = "txt";
            this.openFileDialog_LoadExcel.Filter = "Excel File (*.xls)|*.xls|txt File (*.txt)|*.txt;";
            this.openFileDialog_LoadExcel.Multiselect = true;
            // 
            // plC_RJ_Button_瀏覽
            // 
            this.plC_RJ_Button_瀏覽.AutoResetState = false;
            this.plC_RJ_Button_瀏覽.BackgroundColor = System.Drawing.Color.Black;
            this.plC_RJ_Button_瀏覽.Bool = false;
            this.plC_RJ_Button_瀏覽.BorderColor = System.Drawing.Color.Thistle;
            this.plC_RJ_Button_瀏覽.BorderRadius = 20;
            this.plC_RJ_Button_瀏覽.BorderSize = 0;
            this.plC_RJ_Button_瀏覽.but_press = false;
            this.plC_RJ_Button_瀏覽.buttonType = MyUI.RJ_Button.ButtonType.Toggle;
            this.plC_RJ_Button_瀏覽.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.plC_RJ_Button_瀏覽.DisenableColor = System.Drawing.Color.Gray;
            this.plC_RJ_Button_瀏覽.FlatAppearance.BorderSize = 0;
            this.plC_RJ_Button_瀏覽.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plC_RJ_Button_瀏覽.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.plC_RJ_Button_瀏覽.GUID = "";
            this.plC_RJ_Button_瀏覽.Icon = System.Windows.Forms.MessageBoxIcon.Warning;
            this.plC_RJ_Button_瀏覽.Image_padding = new System.Windows.Forms.Padding(0);
            this.plC_RJ_Button_瀏覽.Location = new System.Drawing.Point(10, 11);
            this.plC_RJ_Button_瀏覽.Name = "plC_RJ_Button_瀏覽";
            this.plC_RJ_Button_瀏覽.OFF_文字內容 = "瀏覽";
            this.plC_RJ_Button_瀏覽.OFF_文字字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.plC_RJ_Button_瀏覽.OFF_文字顏色 = System.Drawing.Color.White;
            this.plC_RJ_Button_瀏覽.OFF_背景顏色 = System.Drawing.Color.Black;
            this.plC_RJ_Button_瀏覽.ON_BorderSize = 5;
            this.plC_RJ_Button_瀏覽.ON_文字內容 = "瀏覽";
            this.plC_RJ_Button_瀏覽.ON_文字字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold);
            this.plC_RJ_Button_瀏覽.ON_文字顏色 = System.Drawing.Color.White;
            this.plC_RJ_Button_瀏覽.ON_背景顏色 = System.Drawing.Color.Black;
            this.plC_RJ_Button_瀏覽.ProhibitionBorderLineWidth = 1;
            this.plC_RJ_Button_瀏覽.ProhibitionLineWidth = 4;
            this.plC_RJ_Button_瀏覽.ProhibitionSymbolSize = 30;
            this.plC_RJ_Button_瀏覽.ShadowColor = System.Drawing.Color.DimGray;
            this.plC_RJ_Button_瀏覽.ShadowSize = 3;
            this.plC_RJ_Button_瀏覽.ShowLoadingForm = false;
            this.plC_RJ_Button_瀏覽.Size = new System.Drawing.Size(119, 63);
            this.plC_RJ_Button_瀏覽.State = false;
            this.plC_RJ_Button_瀏覽.TabIndex = 11;
            this.plC_RJ_Button_瀏覽.Text = "瀏覽";
            this.plC_RJ_Button_瀏覽.TextColor = System.Drawing.Color.White;
            this.plC_RJ_Button_瀏覽.TextHeight = 0;
            this.plC_RJ_Button_瀏覽.Texts = "瀏覽";
            this.plC_RJ_Button_瀏覽.UseVisualStyleBackColor = false;
            this.plC_RJ_Button_瀏覽.字型鎖住 = false;
            this.plC_RJ_Button_瀏覽.按鈕型態 = MyUI.PLC_RJ_Button.StatusEnum.保持型;
            this.plC_RJ_Button_瀏覽.按鍵方式 = MyUI.PLC_RJ_Button.PressEnum.Mouse_左鍵;
            this.plC_RJ_Button_瀏覽.文字鎖住 = false;
            this.plC_RJ_Button_瀏覽.背景圖片 = null;
            this.plC_RJ_Button_瀏覽.讀取位元反向 = false;
            this.plC_RJ_Button_瀏覽.讀寫鎖住 = false;
            this.plC_RJ_Button_瀏覽.音效 = false;
            this.plC_RJ_Button_瀏覽.顯示 = false;
            this.plC_RJ_Button_瀏覽.顯示狀態 = false;
            // 
            // rJ_Lable_狀態
            // 
            this.rJ_Lable_狀態.BackColor = System.Drawing.Color.White;
            this.rJ_Lable_狀態.BackgroundColor = System.Drawing.Color.White;
            this.rJ_Lable_狀態.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Lable_狀態.BorderRadius = 10;
            this.rJ_Lable_狀態.BorderSize = 0;
            this.rJ_Lable_狀態.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Lable_狀態.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Lable_狀態.ForeColor = System.Drawing.Color.Black;
            this.rJ_Lable_狀態.GUID = "";
            this.rJ_Lable_狀態.Location = new System.Drawing.Point(135, 20);
            this.rJ_Lable_狀態.Name = "rJ_Lable_狀態";
            this.rJ_Lable_狀態.ShadowColor = System.Drawing.Color.DimGray;
            this.rJ_Lable_狀態.ShadowSize = 0;
            this.rJ_Lable_狀態.Size = new System.Drawing.Size(171, 40);
            this.rJ_Lable_狀態.TabIndex = 12;
            this.rJ_Lable_狀態.Text = "---------------";
            this.rJ_Lable_狀態.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rJ_Lable_狀態.TextColor = System.Drawing.Color.Black;
            // 
            // sqL_DataGridView_盤點單
            // 
            this.sqL_DataGridView_盤點單.AutoSelectToDeep = true;
            this.sqL_DataGridView_盤點單.backColor = System.Drawing.Color.Silver;
            this.sqL_DataGridView_盤點單.BorderColor = System.Drawing.Color.Transparent;
            this.sqL_DataGridView_盤點單.BorderRadius = 0;
            this.sqL_DataGridView_盤點單.BorderSize = 0;
            this.sqL_DataGridView_盤點單.cellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.sqL_DataGridView_盤點單.cellStylBackColor = System.Drawing.Color.DarkGray;
            this.sqL_DataGridView_盤點單.cellStyleFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_盤點單.cellStylForeColor = System.Drawing.Color.Black;
            this.sqL_DataGridView_盤點單.columnHeaderBackColor = System.Drawing.Color.DarkGray;
            this.sqL_DataGridView_盤點單.columnHeaderFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_盤點單.columnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            this.sqL_DataGridView_盤點單.columnHeadersHeight = 18;
            this.sqL_DataGridView_盤點單.columnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sqL_DataGridView_盤點單.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqL_DataGridView_盤點單.ImageBox = false;
            this.sqL_DataGridView_盤點單.Location = new System.Drawing.Point(4, 150);
            this.sqL_DataGridView_盤點單.Name = "sqL_DataGridView_盤點單";
            this.sqL_DataGridView_盤點單.OnlineState = SQLUI.SQL_DataGridView.OnlineEnum.Online;
            this.sqL_DataGridView_盤點單.Password = "user82822040";
            this.sqL_DataGridView_盤點單.Port = ((uint)(3306u));
            this.sqL_DataGridView_盤點單.rowHeaderBackColor = System.Drawing.Color.Gray;
            this.sqL_DataGridView_盤點單.rowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            this.sqL_DataGridView_盤點單.RowsColor = System.Drawing.SystemColors.Control;
            this.sqL_DataGridView_盤點單.RowsHeight = 10;
            this.sqL_DataGridView_盤點單.SaveFileName = "SQL_DataGridView";
            this.sqL_DataGridView_盤點單.Server = "127.0.0.0";
            this.sqL_DataGridView_盤點單.Size = new System.Drawing.Size(992, 658);
            this.sqL_DataGridView_盤點單.SSLMode = MySql.Data.MySqlClient.MySqlSslMode.None;
            this.sqL_DataGridView_盤點單.TabIndex = 8;
            this.sqL_DataGridView_盤點單.UserName = "root";
            this.sqL_DataGridView_盤點單.可拖曳欄位寬度 = false;
            this.sqL_DataGridView_盤點單.可選擇多列 = false;
            this.sqL_DataGridView_盤點單.單格樣式 = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.sqL_DataGridView_盤點單.自動換行 = true;
            this.sqL_DataGridView_盤點單.表單字體 = new System.Drawing.Font("新細明體", 9F);
            this.sqL_DataGridView_盤點單.邊框樣式 = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sqL_DataGridView_盤點單.顯示CheckBox = false;
            this.sqL_DataGridView_盤點單.顯示首列 = true;
            this.sqL_DataGridView_盤點單.顯示首行 = true;
            this.sqL_DataGridView_盤點單.首列樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            this.sqL_DataGridView_盤點單.首行樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;
            // 
            // plC_RJ_Button_上傳
            // 
            this.plC_RJ_Button_上傳.AutoResetState = false;
            this.plC_RJ_Button_上傳.BackgroundColor = System.Drawing.Color.Black;
            this.plC_RJ_Button_上傳.Bool = false;
            this.plC_RJ_Button_上傳.BorderColor = System.Drawing.Color.Thistle;
            this.plC_RJ_Button_上傳.BorderRadius = 20;
            this.plC_RJ_Button_上傳.BorderSize = 0;
            this.plC_RJ_Button_上傳.but_press = false;
            this.plC_RJ_Button_上傳.buttonType = MyUI.RJ_Button.ButtonType.Toggle;
            this.plC_RJ_Button_上傳.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.plC_RJ_Button_上傳.DisenableColor = System.Drawing.Color.Gray;
            this.plC_RJ_Button_上傳.Enabled = false;
            this.plC_RJ_Button_上傳.FlatAppearance.BorderSize = 0;
            this.plC_RJ_Button_上傳.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plC_RJ_Button_上傳.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.plC_RJ_Button_上傳.GUID = "";
            this.plC_RJ_Button_上傳.Icon = System.Windows.Forms.MessageBoxIcon.Warning;
            this.plC_RJ_Button_上傳.Image_padding = new System.Windows.Forms.Padding(0);
            this.plC_RJ_Button_上傳.Location = new System.Drawing.Point(271, 11);
            this.plC_RJ_Button_上傳.Name = "plC_RJ_Button_上傳";
            this.plC_RJ_Button_上傳.OFF_文字內容 = "上傳";
            this.plC_RJ_Button_上傳.OFF_文字字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.plC_RJ_Button_上傳.OFF_文字顏色 = System.Drawing.Color.White;
            this.plC_RJ_Button_上傳.OFF_背景顏色 = System.Drawing.Color.Black;
            this.plC_RJ_Button_上傳.ON_BorderSize = 5;
            this.plC_RJ_Button_上傳.ON_文字內容 = "上傳";
            this.plC_RJ_Button_上傳.ON_文字字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold);
            this.plC_RJ_Button_上傳.ON_文字顏色 = System.Drawing.Color.White;
            this.plC_RJ_Button_上傳.ON_背景顏色 = System.Drawing.Color.Black;
            this.plC_RJ_Button_上傳.ProhibitionBorderLineWidth = 1;
            this.plC_RJ_Button_上傳.ProhibitionLineWidth = 4;
            this.plC_RJ_Button_上傳.ProhibitionSymbolSize = 30;
            this.plC_RJ_Button_上傳.ShadowColor = System.Drawing.Color.DimGray;
            this.plC_RJ_Button_上傳.ShadowSize = 3;
            this.plC_RJ_Button_上傳.ShowLoadingForm = false;
            this.plC_RJ_Button_上傳.Size = new System.Drawing.Size(119, 63);
            this.plC_RJ_Button_上傳.State = false;
            this.plC_RJ_Button_上傳.TabIndex = 13;
            this.plC_RJ_Button_上傳.Text = "上傳";
            this.plC_RJ_Button_上傳.TextColor = System.Drawing.Color.White;
            this.plC_RJ_Button_上傳.TextHeight = 0;
            this.plC_RJ_Button_上傳.Texts = "上傳";
            this.plC_RJ_Button_上傳.UseVisualStyleBackColor = false;
            this.plC_RJ_Button_上傳.字型鎖住 = false;
            this.plC_RJ_Button_上傳.按鈕型態 = MyUI.PLC_RJ_Button.StatusEnum.保持型;
            this.plC_RJ_Button_上傳.按鍵方式 = MyUI.PLC_RJ_Button.PressEnum.Mouse_左鍵;
            this.plC_RJ_Button_上傳.文字鎖住 = false;
            this.plC_RJ_Button_上傳.背景圖片 = null;
            this.plC_RJ_Button_上傳.讀取位元反向 = false;
            this.plC_RJ_Button_上傳.讀寫鎖住 = false;
            this.plC_RJ_Button_上傳.音效 = false;
            this.plC_RJ_Button_上傳.顯示 = false;
            this.plC_RJ_Button_上傳.顯示狀態 = false;
            // 
            // Dialog_盤點表匯入
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.plC_RJ_Button_返回;
            this.CanResize = true;
            this.CaptionFont = new System.Drawing.Font("Microsoft JhengHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.CaptionHeight = 30;
            this.ClientSize = new System.Drawing.Size(1000, 900);
            this.CloseBoxSize = new System.Drawing.Size(32, 24);
            this.ControlBox = true;
            this.Controls.Add(this.sqL_DataGridView_盤點單);
            this.Controls.Add(this.stepViewer1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = true;
            this.MaxSize = new System.Drawing.Size(32, 24);
            this.MiniSize = new System.Drawing.Size(32, 24);
            this.Name = "Dialog_盤點表匯入";
            this.ShowInTaskbar = true;
            this.ShowSystemMenu = true;
            this.Text = "盤點單匯入";
            this.TitleOffset = new System.Drawing.Point(20, 5);
            this.TitleSuitColor = true;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MyUI.PLC_RJ_Button plC_RJ_Button_返回;
        private MyUI.StepViewer stepViewer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog_LoadExcel;
        private MyUI.PLC_RJ_Button plC_RJ_Button_瀏覽;
        private MyUI.RJ_Lable rJ_Lable_狀態;
        private SQLUI.SQL_DataGridView sqL_DataGridView_盤點單;
        private MyUI.PLC_RJ_Button plC_RJ_Button_上傳;
    }
}