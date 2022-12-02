using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic;
namespace 智能藥庫系統
{
    public partial class Dialog_寫入效期 : Form
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
        public string Value
        {
            get
            {
                return 效期;
            }
        }
        public string 效期
        {
            get
            {
                string Year = this.touchch_TextBox_Year.Text;
                string Month = this.touchch_TextBox_Month.Text;
                string Day = this.touchch_TextBox_Day.Text;
                return string.Format("{0}/{1}/{2}", Year, Month, Day).ToDateString("/");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.Enter)
            {
                string Year = this.touchch_TextBox_Year.Text;
                string Month = this.touchch_TextBox_Month.Text;
                string Day = this.touchch_TextBox_Day.Text;
                if (Basic.TypeConvert.Check_Date_String(效期))
                {
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }          
                else
                {
                    Basic.MyMessageBox.ShowDialog("寫入日期不合法!");
                    this.touchch_TextBox_Year.Text = "";
                    this.touchch_TextBox_Month.Text = "";
                    this.touchch_TextBox_Day.Text = "";
                    this.touchch_TextBox_Year.Focus();
                }
                return true;
            }
            else if (keyData == System.Windows.Forms.Keys.Escape)
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            else
            {

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public Dialog_寫入效期()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void touchch_TextBox_Year_TextChanged(object sender, EventArgs e)
        {
            if (this.touchch_TextBox_Year.Text.Length >= 4) this.touchch_TextBox_Month.Focus();
        }
        private void touchch_TextBox_Month_TextChanged(object sender, EventArgs e)
        {
            if (this.touchch_TextBox_Month.Text.Length >= 2) this.touchch_TextBox_Day.Focus();
        }
        private void touchch_TextBox_Day_TextChanged(object sender, EventArgs e)
        {

        }
        private void touchch_TextBox_Year_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(this.touchch_TextBox_Year.Text.Length >= 4)
            {
                e.Handled = false;
                return;
            }
            if (((int)e.KeyChar <= 57 && (int)e.KeyChar >= 48) || (int)e.KeyChar == 8) // 8 > BackSpace
            {

                e.Handled = false;
            }
            else e.Handled = true;
        }
        private void touchch_TextBox_Month_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar <= 57 && (int)e.KeyChar >= 48) || (int)e.KeyChar == 8) // 8 > BackSpace
            {

                e.Handled = false;
            }
            else e.Handled = true;
        }
        private void touchch_TextBox_Day_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar <= 57 && (int)e.KeyChar >= 48) || (int)e.KeyChar == 8) // 8 > BackSpace
            {

                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            string Year = this.touchch_TextBox_Year.Text;
            string Month = this.touchch_TextBox_Month.Text;
            string Day = this.touchch_TextBox_Day.Text;

            if (Basic.TypeConvert.Check_Date_String(效期))
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                Basic.MyMessageBox.ShowDialog("寫入日期不合法!");
                this.touchch_TextBox_Year.Text = "";
                this.touchch_TextBox_Month.Text = "";
                this.touchch_TextBox_Day.Text = "";
                this.touchch_TextBox_Year.Focus();
            }
           
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void Dialog_寫入效期_Load(object sender, EventArgs e)
        {
            this.TopLevel = true;
            this.TopMost = true;
          
        }

        private void Dialog_寫入效期_Shown(object sender, EventArgs e)
        {
            this.touchch_TextBox_Year.Focus();
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.touchch_TextBox_Year = new MyUI.RJ_TextBox();
            this.touchch_TextBox_Month = new MyUI.RJ_TextBox();
            this.touchch_TextBox_Day = new MyUI.RJ_TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 20F);
            this.label1.Location = new System.Drawing.Point(107, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "/";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 20F);
            this.label2.Location = new System.Drawing.Point(230, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 27);
            this.label2.TabIndex = 4;
            this.label2.Text = "/";
            // 
            // button_OK
            // 
            this.button_OK.Font = new System.Drawing.Font("新細明體", 20F);
            this.button_OK.Location = new System.Drawing.Point(467, 10);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(81, 58);
            this.button_OK.TabIndex = 6;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Font = new System.Drawing.Font("新細明體", 20F);
            this.button_Cancel.Location = new System.Drawing.Point(352, 9);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(109, 58);
            this.button_Cancel.TabIndex = 7;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // touchch_TextBox_Year
            // 
            this.touchch_TextBox_Year.BackColor = System.Drawing.SystemColors.Window;
            this.touchch_TextBox_Year.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.touchch_TextBox_Year.BorderFocusColor = System.Drawing.Color.HotPink;
            this.touchch_TextBox_Year.BorderRadius = 0;
            this.touchch_TextBox_Year.BorderSize = 2;
            this.touchch_TextBox_Year.Font = new System.Drawing.Font("新細明體", 20F);
            this.touchch_TextBox_Year.ForeColor = System.Drawing.Color.DimGray;
            this.touchch_TextBox_Year.Location = new System.Drawing.Point(12, 16);
            this.touchch_TextBox_Year.Multiline = false;
            this.touchch_TextBox_Year.Name = "touchch_TextBox_Year";
            this.touchch_TextBox_Year.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.touchch_TextBox_Year.PassWordChar = false;
            this.touchch_TextBox_Year.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.touchch_TextBox_Year.PlaceholderText = "year";
            this.touchch_TextBox_Year.ShowTouchPannel = false;
            this.touchch_TextBox_Year.Size = new System.Drawing.Size(86, 46);
            this.touchch_TextBox_Year.TabIndex = 8;
            this.touchch_TextBox_Year.TextAlgin = System.Windows.Forms.HorizontalAlignment.Left;
            this.touchch_TextBox_Year.Texts = "";
            this.touchch_TextBox_Year.UnderlineStyle = false;
            this.touchch_TextBox_Year._TextChanged += new System.EventHandler(this.touchch_TextBox_Year_TextChanged);
            // 
            // touchch_TextBox_Month
            // 
            this.touchch_TextBox_Month.BackColor = System.Drawing.SystemColors.Window;
            this.touchch_TextBox_Month.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.touchch_TextBox_Month.BorderFocusColor = System.Drawing.Color.HotPink;
            this.touchch_TextBox_Month.BorderRadius = 0;
            this.touchch_TextBox_Month.BorderSize = 2;
            this.touchch_TextBox_Month.Font = new System.Drawing.Font("新細明體", 20F);
            this.touchch_TextBox_Month.ForeColor = System.Drawing.Color.DimGray;
            this.touchch_TextBox_Month.Location = new System.Drawing.Point(135, 16);
            this.touchch_TextBox_Month.Multiline = false;
            this.touchch_TextBox_Month.Name = "touchch_TextBox_Month";
            this.touchch_TextBox_Month.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.touchch_TextBox_Month.PassWordChar = false;
            this.touchch_TextBox_Month.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.touchch_TextBox_Month.PlaceholderText = "month";
            this.touchch_TextBox_Month.ShowTouchPannel = false;
            this.touchch_TextBox_Month.Size = new System.Drawing.Size(86, 46);
            this.touchch_TextBox_Month.TabIndex = 9;
            this.touchch_TextBox_Month.TextAlgin = System.Windows.Forms.HorizontalAlignment.Left;
            this.touchch_TextBox_Month.Texts = "";
            this.touchch_TextBox_Month.UnderlineStyle = false;
            this.touchch_TextBox_Month._TextChanged += new System.EventHandler(this.touchch_TextBox_Month_TextChanged);
            // 
            // touchch_TextBox_Day
            // 
            this.touchch_TextBox_Day.BackColor = System.Drawing.SystemColors.Window;
            this.touchch_TextBox_Day.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.touchch_TextBox_Day.BorderFocusColor = System.Drawing.Color.HotPink;
            this.touchch_TextBox_Day.BorderRadius = 0;
            this.touchch_TextBox_Day.BorderSize = 2;
            this.touchch_TextBox_Day.Font = new System.Drawing.Font("新細明體", 20F);
            this.touchch_TextBox_Day.ForeColor = System.Drawing.Color.DimGray;
            this.touchch_TextBox_Day.Location = new System.Drawing.Point(258, 16);
            this.touchch_TextBox_Day.Multiline = false;
            this.touchch_TextBox_Day.Name = "touchch_TextBox_Day";
            this.touchch_TextBox_Day.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.touchch_TextBox_Day.PassWordChar = false;
            this.touchch_TextBox_Day.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.touchch_TextBox_Day.PlaceholderText = "day";
            this.touchch_TextBox_Day.ShowTouchPannel = false;
            this.touchch_TextBox_Day.Size = new System.Drawing.Size(86, 46);
            this.touchch_TextBox_Day.TabIndex = 10;
            this.touchch_TextBox_Day.TextAlgin = System.Windows.Forms.HorizontalAlignment.Left;
            this.touchch_TextBox_Day.Texts = "";
            this.touchch_TextBox_Day.UnderlineStyle = false;
            this.touchch_TextBox_Day._TextChanged += new System.EventHandler(this.touchch_TextBox_Day_TextChanged);
            // 
            // Dialog_寫入效期
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(560, 77);
            this.ControlBox = false;
            this.Controls.Add(this.touchch_TextBox_Day);
            this.Controls.Add(this.touchch_TextBox_Month);
            this.Controls.Add(this.touchch_TextBox_Year);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Dialog_寫入效期";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Dialog_寫入效期_Load);
            this.Shown += new System.EventHandler(this.Dialog_寫入效期_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private MyUI.RJ_TextBox touchch_TextBox_Year;
        private MyUI.RJ_TextBox touchch_TextBox_Month;
        private MyUI.RJ_TextBox touchch_TextBox_Day;
    }
}
