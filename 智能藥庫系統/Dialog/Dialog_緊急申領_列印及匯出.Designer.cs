
namespace 智能藥庫系統
{
    partial class Dialog_緊急申領_列印及匯出
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
            this.rJ_Button_取消 = new MyUI.RJ_Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_列印 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_預覽列印 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_匯出 = new System.Windows.Forms.Button();
            this.saveFileDialog_SaveExcel = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // rJ_Button_取消
            // 
            this.rJ_Button_取消.AutoResetState = false;
            this.rJ_Button_取消.BackColor = System.Drawing.Color.White;
            this.rJ_Button_取消.BackgroundColor = System.Drawing.Color.Gray;
            this.rJ_Button_取消.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_取消.BorderRadius = 20;
            this.rJ_Button_取消.BorderSize = 0;
            this.rJ_Button_取消.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_取消.DisenableColor = System.Drawing.Color.Gray;
            this.rJ_Button_取消.FlatAppearance.BorderSize = 0;
            this.rJ_Button_取消.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_取消.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Button_取消.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_取消.GUID = "";
            this.rJ_Button_取消.Image_padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.rJ_Button_取消.Location = new System.Drawing.Point(387, 15);
            this.rJ_Button_取消.Name = "rJ_Button_取消";
            this.rJ_Button_取消.ProhibitionBorderLineWidth = 1;
            this.rJ_Button_取消.ProhibitionLineWidth = 4;
            this.rJ_Button_取消.ProhibitionSymbolSize = 30;
            this.rJ_Button_取消.ShadowColor = System.Drawing.Color.DimGray;
            this.rJ_Button_取消.ShadowSize = 3;
            this.rJ_Button_取消.ShowLoadingForm = false;
            this.rJ_Button_取消.Size = new System.Drawing.Size(133, 98);
            this.rJ_Button_取消.State = false;
            this.rJ_Button_取消.TabIndex = 20;
            this.rJ_Button_取消.Text = "取消";
            this.rJ_Button_取消.TextColor = System.Drawing.Color.White;
            this.rJ_Button_取消.TextHeight = 0;
            this.rJ_Button_取消.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_列印);
            this.groupBox1.Location = new System.Drawing.Point(27, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(101, 101);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "列印";
            // 
            // button_列印
            // 
            this.button_列印.BackgroundImage = global::智能藥庫系統.Properties.Resources.icon_19;
            this.button_列印.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_列印.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_列印.Location = new System.Drawing.Point(3, 18);
            this.button_列印.Name = "button_列印";
            this.button_列印.Size = new System.Drawing.Size(95, 80);
            this.button_列印.TabIndex = 0;
            this.button_列印.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_預覽列印);
            this.groupBox2.Location = new System.Drawing.Point(147, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(101, 101);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "預覽列印";
            // 
            // button_預覽列印
            // 
            this.button_預覽列印.BackgroundImage = global::智能藥庫系統.Properties.Resources._1200px_Document_print_preview_svg;
            this.button_預覽列印.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_預覽列印.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_預覽列印.Location = new System.Drawing.Point(3, 18);
            this.button_預覽列印.Name = "button_預覽列印";
            this.button_預覽列印.Size = new System.Drawing.Size(95, 80);
            this.button_預覽列印.TabIndex = 1;
            this.button_預覽列印.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_匯出);
            this.groupBox3.Location = new System.Drawing.Point(266, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(101, 101);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "匯出";
            // 
            // button_匯出
            // 
            this.button_匯出.BackgroundImage = global::智能藥庫系統.Properties.Resources.export_to_csv_icon_11;
            this.button_匯出.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_匯出.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_匯出.Location = new System.Drawing.Point(3, 18);
            this.button_匯出.Name = "button_匯出";
            this.button_匯出.Size = new System.Drawing.Size(95, 80);
            this.button_匯出.TabIndex = 1;
            this.button_匯出.UseVisualStyleBackColor = true;
            // 
            // saveFileDialog_SaveExcel
            // 
            this.saveFileDialog_SaveExcel.DefaultExt = "txt";
            this.saveFileDialog_SaveExcel.Filter = "Excel File (*.xls)|*.xls|txt File (*.txt)|*.txt;";
            // 
            // Dialog_緊急申領_列印及匯出
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(547, 125);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rJ_Button_取消);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Dialog_緊急申領_列印及匯出";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "列印與匯出";
            this.Load += new System.EventHandler(this.Dialog_緊急申領_列印及匯出_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MyUI.RJ_Button rJ_Button_取消;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_列印;
        private System.Windows.Forms.Button button_預覽列印;
        private System.Windows.Forms.Button button_匯出;
        private System.Windows.Forms.SaveFileDialog saveFileDialog_SaveExcel;
    }
}