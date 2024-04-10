using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MyUI;
using Basic;
using H_Pannel_lib;
using SQLUI;
using HIS_DB_Lib;

namespace 智能藥庫系統
{
    public partial class Dialog_新建合併盤點單 : MyDialog
    {
        private string 盤點單名稱 = "";
        public string Value
        {
            get
            {
                return 盤點單名稱;
            }
            set
            {
                盤點單名稱 = value;
            }
        }
        public Dialog_新建合併盤點單()
        {
            InitializeComponent();
            this.rJ_Button_取消.MouseDownEvent += RJ_Button_取消_MouseDownEvent;
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_TextBox_盤點單名稱.KeypressEnterButton = this.rJ_Button_確認;
        }

        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            if(this.rJ_TextBox_盤點單名稱.Texts.StringIsEmpty())
            {
                Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm("名稱空白", 1000, 0, -150);
                dialog_AlarmForm.ShowDialog();
                return;
            }

            this.Value = this.rJ_TextBox_盤點單名稱.Texts;
            this.Close();
            DialogResult = DialogResult.Yes;
        }
        private void RJ_Button_取消_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Close();
            DialogResult = DialogResult.No;
        }
    }
}
