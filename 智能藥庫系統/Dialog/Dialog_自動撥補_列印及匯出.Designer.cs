
namespace 智能藥庫系統
{
    partial class Dialog_自動撥補_列印及匯出
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_匯出 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_預覽列印 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_列印 = new System.Windows.Forms.Button();
            this.saveFileDialog_SaveExcel = new System.Windows.Forms.SaveFileDialog();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_匯出);
            this.groupBox3.Location = new System.Drawing.Point(278, 54);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(101, 101);
            this.groupBox3.TabIndex = 26;
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_預覽列印);
            this.groupBox2.Location = new System.Drawing.Point(159, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(101, 101);
            this.groupBox2.TabIndex = 25;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_列印);
            this.groupBox1.Location = new System.Drawing.Point(39, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(101, 101);
            this.groupBox1.TabIndex = 24;
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
            // saveFileDialog_SaveExcel
            // 
            this.saveFileDialog_SaveExcel.DefaultExt = "txt";
            this.saveFileDialog_SaveExcel.Filter = "Excel File (*.xls)|*.xls|txt File (*.txt)|*.txt;";
            // 
            // Dialog_自動撥補_列印及匯出
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(432, 192);
            this.ControlBox = true;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "Dialog_自動撥補_列印及匯出";
            this.Text = "列印及匯出";
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_匯出;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_預覽列印;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_列印;
        private System.Windows.Forms.SaveFileDialog saveFileDialog_SaveExcel;
    }
}