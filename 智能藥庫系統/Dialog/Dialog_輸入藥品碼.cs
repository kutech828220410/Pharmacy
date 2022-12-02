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
    public partial class Dialog_寫入藥品碼 : Form
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
                return this.textBox_藥品碼.Text;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.Enter)
            {
                if (this.textBox_藥品碼.Text == "")
                {
                    Basic.MyMessageBox.ShowDialog("藥品碼不得為空!");
                }
                else
                {
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
                return true;
            }
            else if(keyData == System.Windows.Forms.Keys.Escape)
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            else
            {
                this.textBox_藥品碼.Focus();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public Dialog_寫入藥品碼()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void Dialog_寫入藥品碼_Load(object sender, EventArgs e)
        {
          
            this.TopLevel = true;
            this.TopMost = true;
            
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.textBox_藥品碼.Text == "")
            {
                Basic.MyMessageBox.ShowDialog("藥品碼不得為空!");
            }
            else
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void textBox_藥品碼_TextChanged(object sender, EventArgs e)
        {

        }

        private void Dialog_寫入藥品碼_Shown(object sender, EventArgs e)
        {
            this.textBox_藥品碼.Focus();
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
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.textBox_藥品碼 = new MyUI.RJ_TextBox();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Font = new System.Drawing.Font("新細明體", 20F);
            this.button_OK.Location = new System.Drawing.Point(379, 12);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(97, 71);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Font = new System.Drawing.Font("新細明體", 20F);
            this.button_Cancel.Location = new System.Drawing.Point(264, 12);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(109, 71);
            this.button_Cancel.TabIndex = 8;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // textBox_藥品碼
            // 
            this.textBox_藥品碼.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_藥品碼.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.textBox_藥品碼.BorderFocusColor = System.Drawing.Color.HotPink;
            this.textBox_藥品碼.BorderRadius = 0;
            this.textBox_藥品碼.BorderSize = 2;
            this.textBox_藥品碼.Font = new System.Drawing.Font("新細明體", 20F);
            this.textBox_藥品碼.ForeColor = System.Drawing.Color.DimGray;
            this.textBox_藥品碼.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBox_藥品碼.Location = new System.Drawing.Point(18, 25);
            this.textBox_藥品碼.Multiline = false;
            this.textBox_藥品碼.Name = "textBox_藥品碼";
            this.textBox_藥品碼.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.textBox_藥品碼.PassWordChar = false;
            this.textBox_藥品碼.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.textBox_藥品碼.PlaceholderText = "Enter code...";
            this.textBox_藥品碼.ShowTouchPannel = false;
            this.textBox_藥品碼.Size = new System.Drawing.Size(235, 46);
            this.textBox_藥品碼.TabIndex = 9;
            this.textBox_藥品碼.TextAlgin = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBox_藥品碼.Texts = "";
            this.textBox_藥品碼.UnderlineStyle = false;
            // 
            // Dialog_寫入藥品碼
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(488, 93);
            this.ControlBox = false;
            this.Controls.Add(this.textBox_藥品碼);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Dialog_寫入藥品碼";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Dialog_寫入藥品碼_Load);
            this.Shown += new System.EventHandler(this.Dialog_寫入藥品碼_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        internal MyUI.RJ_TextBox textBox_藥品碼;
    }
}
