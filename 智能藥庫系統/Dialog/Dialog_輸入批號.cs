using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 智能藥庫系統
{
    public partial class Dialog_寫入批號 : Form
    {
        public static Form form;
        public new DialogResult ShowDialog()
        {
            if (form == null)
            {
                base.ShowDialog();
            }
            else
            {
                form.Invoke(new Action(delegate
                {
                    base.ShowDialog();
                }));
            }

            return this.DialogResult;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.Enter)
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
                return true;
            }
            else if (keyData == System.Windows.Forms.Keys.Escape)
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            else
            {
                this.rJ_TextBox_批號.Focus();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private string value = "";
        public string Value
        {
            get
            {
                return this.rJ_TextBox_批號.Texts;
            }
            set
            {
                this.rJ_TextBox_批號.Texts = value;
            }
        }
        public Dialog_寫入批號()
        {
            InitializeComponent();
        }
        public Dialog_寫入批號(string value)
        {
            InitializeComponent();
            this.value = value;
           
        }

        private void Dialog_寫入批號_Load(object sender, EventArgs e)
        {
            this.Value = value;
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_Button_取消.MouseDownEvent += RJ_Button_取消_MouseDownEvent;
        }
        private void RJ_Button_取消_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
        }
        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }));
        }
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
            this.rJ_TextBox_批號 = new MyUI.RJ_TextBox();
            this.rJ_Button_取消 = new MyUI.RJ_Button();
            this.rJ_Button_確認 = new MyUI.RJ_Button();
            this.rJ_Lable1 = new MyUI.RJ_Lable();
            this.SuspendLayout();
            // 
            // rJ_TextBox_批號
            // 
            this.rJ_TextBox_批號.BackColor = System.Drawing.SystemColors.Window;
            this.rJ_TextBox_批號.BorderColor = System.Drawing.Color.RoyalBlue;
            this.rJ_TextBox_批號.BorderFocusColor = System.Drawing.Color.HotPink;
            this.rJ_TextBox_批號.BorderRadius = 0;
            this.rJ_TextBox_批號.BorderSize = 2;
            this.rJ_TextBox_批號.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_TextBox_批號.ForeColor = System.Drawing.Color.DimGray;
            this.rJ_TextBox_批號.Location = new System.Drawing.Point(64, 13);
            this.rJ_TextBox_批號.Multiline = false;
            this.rJ_TextBox_批號.Name = "rJ_TextBox_批號";
            this.rJ_TextBox_批號.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.rJ_TextBox_批號.PassWordChar = false;
            this.rJ_TextBox_批號.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.rJ_TextBox_批號.PlaceholderText = "寫入批號....";
            this.rJ_TextBox_批號.ShowTouchPannel = false;
            this.rJ_TextBox_批號.Size = new System.Drawing.Size(198, 42);
            this.rJ_TextBox_批號.TabIndex = 0;
            this.rJ_TextBox_批號.TextAlgin = System.Windows.Forms.HorizontalAlignment.Left;
            this.rJ_TextBox_批號.Texts = "";
            this.rJ_TextBox_批號.UnderlineStyle = false;
            // 
            // rJ_Button_取消
            // 
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
            this.rJ_Button_取消.Location = new System.Drawing.Point(354, 16);
            this.rJ_Button_取消.Name = "rJ_Button_取消";
            this.rJ_Button_取消.Size = new System.Drawing.Size(80, 37);
            this.rJ_Button_取消.State = false;
            this.rJ_Button_取消.TabIndex = 19;
            this.rJ_Button_取消.Text = "取消";
            this.rJ_Button_取消.TextColor = System.Drawing.Color.White;
            this.rJ_Button_取消.UseVisualStyleBackColor = false;
            // 
            // rJ_Button_確認
            // 
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
            this.rJ_Button_確認.Location = new System.Drawing.Point(268, 16);
            this.rJ_Button_確認.Name = "rJ_Button_確認";
            this.rJ_Button_確認.Size = new System.Drawing.Size(80, 37);
            this.rJ_Button_確認.State = false;
            this.rJ_Button_確認.TabIndex = 28;
            this.rJ_Button_確認.Text = "確認";
            this.rJ_Button_確認.TextColor = System.Drawing.Color.White;
            this.rJ_Button_確認.UseVisualStyleBackColor = false;
            // 
            // rJ_Lable1
            // 
            this.rJ_Lable1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.rJ_Lable1.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.rJ_Lable1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Lable1.BorderRadius = 12;
            this.rJ_Lable1.BorderSize = 0;
            this.rJ_Lable1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Lable1.ForeColor = System.Drawing.Color.White;
            this.rJ_Lable1.Location = new System.Drawing.Point(2, 14);
            this.rJ_Lable1.Name = "rJ_Lable1";
            this.rJ_Lable1.Size = new System.Drawing.Size(56, 40);
            this.rJ_Lable1.TabIndex = 29;
            this.rJ_Lable1.Text = "批號";
            this.rJ_Lable1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_Lable1.TextColor = System.Drawing.Color.White;
            // 
            // Dialog_寫入批號
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(441, 68);
            this.ControlBox = false;
            this.Controls.Add(this.rJ_Lable1);
            this.Controls.Add(this.rJ_Button_確認);
            this.Controls.Add(this.rJ_Button_取消);
            this.Controls.Add(this.rJ_TextBox_批號);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Dialog_寫入批號";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Dialog_寫入批號_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MyUI.RJ_TextBox rJ_TextBox_批號;
        private MyUI.RJ_Button rJ_Button_取消;
        private MyUI.RJ_Button rJ_Button_確認;
        private MyUI.RJ_Lable rJ_Lable1;

    }
}
