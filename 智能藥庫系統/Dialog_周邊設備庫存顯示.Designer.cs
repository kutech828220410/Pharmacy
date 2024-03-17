
namespace 智能藥庫系統
{
    partial class Dialog_周邊設備庫存顯示
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
            this.rJ_Lable1 = new MyUI.RJ_Lable();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rJ_Button_返回 = new MyUI.RJ_Button();
            this.sqL_DataGridView_庫存查詢 = new SQLUI.SQL_DataGridView();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rJ_Lable1
            // 
            this.rJ_Lable1.BackColor = System.Drawing.Color.White;
            this.rJ_Lable1.BackgroundColor = System.Drawing.Color.Silver;
            this.rJ_Lable1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Lable1.BorderRadius = 10;
            this.rJ_Lable1.BorderSize = 0;
            this.rJ_Lable1.Dock = System.Windows.Forms.DockStyle.Top;
            this.rJ_Lable1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Lable1.Font = new System.Drawing.Font("微軟正黑體", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Lable1.ForeColor = System.Drawing.Color.Transparent;
            this.rJ_Lable1.GUID = "";
            this.rJ_Lable1.Location = new System.Drawing.Point(4, 28);
            this.rJ_Lable1.Name = "rJ_Lable1";
            this.rJ_Lable1.ShadowColor = System.Drawing.Color.DimGray;
            this.rJ_Lable1.ShadowSize = 0;
            this.rJ_Lable1.Size = new System.Drawing.Size(842, 96);
            this.rJ_Lable1.TabIndex = 0;
            this.rJ_Lable1.Text = "庫 存 查 詢";
            this.rJ_Lable1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_Lable1.TextColor = System.Drawing.Color.White;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rJ_Button_返回);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(4, 796);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(842, 100);
            this.panel1.TabIndex = 1;
            // 
            // rJ_Button_返回
            // 
            this.rJ_Button_返回.AutoResetState = false;
            this.rJ_Button_返回.BackColor = System.Drawing.Color.Transparent;
            this.rJ_Button_返回.BackgroundColor = System.Drawing.Color.Black;
            this.rJ_Button_返回.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_返回.BorderRadius = 10;
            this.rJ_Button_返回.BorderSize = 0;
            this.rJ_Button_返回.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_返回.Dock = System.Windows.Forms.DockStyle.Right;
            this.rJ_Button_返回.FlatAppearance.BorderSize = 0;
            this.rJ_Button_返回.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_返回.Font = new System.Drawing.Font("微軟正黑體", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Button_返回.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_返回.GUID = "";
            this.rJ_Button_返回.Image_padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.rJ_Button_返回.Location = new System.Drawing.Point(678, 0);
            this.rJ_Button_返回.Name = "rJ_Button_返回";
            this.rJ_Button_返回.ShadowColor = System.Drawing.Color.DimGray;
            this.rJ_Button_返回.ShadowSize = 3;
            this.rJ_Button_返回.ShowLoadingForm = false;
            this.rJ_Button_返回.Size = new System.Drawing.Size(164, 100);
            this.rJ_Button_返回.State = false;
            this.rJ_Button_返回.TabIndex = 0;
            this.rJ_Button_返回.Text = "返回";
            this.rJ_Button_返回.TextColor = System.Drawing.Color.White;
            this.rJ_Button_返回.TextHeight = 0;
            this.rJ_Button_返回.UseVisualStyleBackColor = false;
            // 
            // sqL_DataGridView_庫存查詢
            // 
            this.sqL_DataGridView_庫存查詢.AutoSelectToDeep = false;
            this.sqL_DataGridView_庫存查詢.backColor = System.Drawing.Color.LightGray;
            this.sqL_DataGridView_庫存查詢.BorderColor = System.Drawing.Color.LightGray;
            this.sqL_DataGridView_庫存查詢.BorderRadius = 0;
            this.sqL_DataGridView_庫存查詢.BorderSize = 2;
            this.sqL_DataGridView_庫存查詢.cellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sqL_DataGridView_庫存查詢.cellStylBackColor = System.Drawing.Color.LightGray;
            this.sqL_DataGridView_庫存查詢.cellStyleFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_庫存查詢.cellStylForeColor = System.Drawing.Color.Black;
            this.sqL_DataGridView_庫存查詢.columnHeaderBackColor = System.Drawing.Color.Gray;
            this.sqL_DataGridView_庫存查詢.columnHeaderFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_庫存查詢.columnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.sqL_DataGridView_庫存查詢.columnHeadersHeight = 18;
            this.sqL_DataGridView_庫存查詢.columnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sqL_DataGridView_庫存查詢.DataBaseName = "storehouse_0";
            this.sqL_DataGridView_庫存查詢.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqL_DataGridView_庫存查詢.Font = new System.Drawing.Font("新細明體", 12F);
            this.sqL_DataGridView_庫存查詢.ImageBox = false;
            this.sqL_DataGridView_庫存查詢.Location = new System.Drawing.Point(4, 124);
            this.sqL_DataGridView_庫存查詢.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sqL_DataGridView_庫存查詢.Name = "sqL_DataGridView_庫存查詢";
            this.sqL_DataGridView_庫存查詢.OnlineState = SQLUI.SQL_DataGridView.OnlineEnum.Online;
            this.sqL_DataGridView_庫存查詢.Password = "user82822040";
            this.sqL_DataGridView_庫存查詢.Port = ((uint)(3306u));
            this.sqL_DataGridView_庫存查詢.rowHeaderBackColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_庫存查詢.rowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.sqL_DataGridView_庫存查詢.RowsColor = System.Drawing.SystemColors.ButtonHighlight;
            this.sqL_DataGridView_庫存查詢.RowsHeight = 60;
            this.sqL_DataGridView_庫存查詢.SaveFileName = "SQL_DataGridView";
            this.sqL_DataGridView_庫存查詢.Server = "localhost";
            this.sqL_DataGridView_庫存查詢.Size = new System.Drawing.Size(842, 672);
            this.sqL_DataGridView_庫存查詢.SSLMode = MySql.Data.MySqlClient.MySqlSslMode.None;
            this.sqL_DataGridView_庫存查詢.TabIndex = 157;
            this.sqL_DataGridView_庫存查詢.TableName = "medicine_page_firstclass";
            this.sqL_DataGridView_庫存查詢.UserName = "root";
            this.sqL_DataGridView_庫存查詢.可拖曳欄位寬度 = true;
            this.sqL_DataGridView_庫存查詢.可選擇多列 = true;
            this.sqL_DataGridView_庫存查詢.單格樣式 = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sqL_DataGridView_庫存查詢.自動換行 = true;
            this.sqL_DataGridView_庫存查詢.表單字體 = new System.Drawing.Font("新細明體", 12F);
            this.sqL_DataGridView_庫存查詢.邊框樣式 = System.Windows.Forms.BorderStyle.None;
            this.sqL_DataGridView_庫存查詢.顯示CheckBox = false;
            this.sqL_DataGridView_庫存查詢.顯示首列 = true;
            this.sqL_DataGridView_庫存查詢.顯示首行 = true;
            this.sqL_DataGridView_庫存查詢.首列樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.sqL_DataGridView_庫存查詢.首行樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            // 
            // Dialog_周邊設備庫存顯示
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(850, 900);
            this.Controls.Add(this.sqL_DataGridView_庫存查詢);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rJ_Lable1);
            this.Name = "Dialog_周邊設備庫存顯示";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MyUI.RJ_Lable rJ_Lable1;
        private System.Windows.Forms.Panel panel1;
        private MyUI.RJ_Button rJ_Button_返回;
        private SQLUI.SQL_DataGridView sqL_DataGridView_庫存查詢;
    }
}