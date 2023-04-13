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
using MyOffice;
using MyPrinterlib;
using SQLUI;
using System.Threading;

namespace 智能藥庫系統
{
    public partial class Dialog_更換密碼 : Form
    {
        public string Value
        {
            get
            {
                return textBox_新密碼確認.Text;
            }
        }

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

        public Dialog_更換密碼()
        {
            InitializeComponent();
        }

        private void Dialog_更換密碼_Load(object sender, EventArgs e)
        {
            this.rJ_Button_取消.MouseDownEvent += RJ_Button_取消_MouseDownEvent;
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
        }

        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                if(textBox_新密碼輸入.Text.StringIsEmpty()|| textBox_新密碼確認.Text.StringIsEmpty())
                {
                    MyMessageBox.ShowDialog("密碼不得為空白!");
                    return;
                }
                if (textBox_新密碼輸入.Text != textBox_新密碼確認.Text)
                {
                    MyMessageBox.ShowDialog("密碼確認不一致!");
                    return;
                }
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }));
        }
        private void RJ_Button_取消_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
        }
    }
}
