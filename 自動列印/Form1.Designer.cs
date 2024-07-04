
namespace 自動列印
{
    partial class Main_Form
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_申領_狀態 = new System.Windows.Forms.Label();
            this.radioButton_循環列印 = new System.Windows.Forms.RadioButton();
            this.radioButton_單次列印 = new System.Windows.Forms.RadioButton();
            this.button_列印作業開始 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_列印作業開始);
            this.panel1.Controls.Add(this.radioButton_單次列印);
            this.panel1.Controls.Add(this.radioButton_循環列印);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1150, 72);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_申領_狀態);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(10, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1150, 126);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "申領";
            // 
            // label_申領_狀態
            // 
            this.label_申領_狀態.AutoSize = true;
            this.label_申領_狀態.Font = new System.Drawing.Font("新細明體", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_申領_狀態.Location = new System.Drawing.Point(43, 47);
            this.label_申領_狀態.Name = "label_申領_狀態";
            this.label_申領_狀態.Size = new System.Drawing.Size(567, 27);
            this.label_申領_狀態.TabIndex = 0;
            this.label_申領_狀態.Text = "狀態 : ------------------------------------------------------------";
            // 
            // radioButton_循環列印
            // 
            this.radioButton_循環列印.AutoSize = true;
            this.radioButton_循環列印.Checked = true;
            this.radioButton_循環列印.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_循環列印.Location = new System.Drawing.Point(27, 27);
            this.radioButton_循環列印.Name = "radioButton_循環列印";
            this.radioButton_循環列印.Size = new System.Drawing.Size(90, 20);
            this.radioButton_循環列印.TabIndex = 1;
            this.radioButton_循環列印.Text = "循環列印";
            this.radioButton_循環列印.UseVisualStyleBackColor = true;
            // 
            // radioButton_單次列印
            // 
            this.radioButton_單次列印.AutoSize = true;
            this.radioButton_單次列印.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioButton_單次列印.Location = new System.Drawing.Point(123, 27);
            this.radioButton_單次列印.Name = "radioButton_單次列印";
            this.radioButton_單次列印.Size = new System.Drawing.Size(90, 20);
            this.radioButton_單次列印.TabIndex = 2;
            this.radioButton_單次列印.Text = "單次列印";
            this.radioButton_單次列印.UseVisualStyleBackColor = true;
            // 
            // button_列印作業開始
            // 
            this.button_列印作業開始.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_列印作業開始.Location = new System.Drawing.Point(234, 10);
            this.button_列印作業開始.Name = "button_列印作業開始";
            this.button_列印作業開始.Size = new System.Drawing.Size(153, 55);
            this.button_列印作業開始.TabIndex = 3;
            this.button_列印作業開始.Text = "列印作業開始";
            this.button_列印作業開始.UseVisualStyleBackColor = true;
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 767);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "Main_Form";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自動列印系統";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton_循環列印;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_申領_狀態;
        private System.Windows.Forms.Button button_列印作業開始;
        private System.Windows.Forms.RadioButton radioButton_單次列印;
    }
}

