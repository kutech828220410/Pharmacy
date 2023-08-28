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
    public partial class Dialog_補給系統藥品建置 : Form
    {
        public object[] Value;
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
        public Dialog_補給系統藥品建置(object[] value)
        {
            InitializeComponent();
            Value = value;
            this.Load += Dialog_補給系統藥品建置_Load;
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_Button_取消.MouseDownEvent += RJ_Button_取消_MouseDownEvent;
        }

        #region Event
        private void Dialog_補給系統藥品建置_Load(object sender, EventArgs e)
        {
            rJ_Lable_藥名.Text = $"藥名:{Value[(int)enum_藥品補給系統_藥品資料.藥品名稱].ObjectToString()}";
            rJ_TextBox_藥碼.Text = Value[(int)enum_藥品補給系統_藥品資料.藥品碼].ObjectToString();
            rJ_TextBox_包裝單位.Text = Value[(int)enum_藥品補給系統_藥品資料.包裝單位].ObjectToString();
            rJ_TextBox_廠牌.Text = Value[(int)enum_藥品補給系統_藥品資料.廠牌].ObjectToString();
            rJ_TextBox_已訂購數量.Text = Value[(int)enum_藥品補給系統_藥品資料.已訂購數量].ObjectToString();
            rJ_TextBox_已採購總量.Text = Value[(int)enum_藥品補給系統_藥品資料.已採購總量].ObjectToString();
            rJ_TextBox_已訂購總價.Text = Value[(int)enum_藥品補給系統_藥品資料.已訂購總價].ObjectToString();
            rJ_TextBox_已採購總價.Text = Value[(int)enum_藥品補給系統_藥品資料.已採購總價].ObjectToString();
            rJ_TextBox_已採購總量上限.Text = Value[(int)enum_藥品補給系統_藥品資料.已採購總量上限].ObjectToString();
            rJ_TextBox_契約價金.Text = Value[(int)enum_藥品補給系統_藥品資料.契約價金].ObjectToString();
            rJ_TextBox_最新訂購單價.Text = Value[(int)enum_藥品補給系統_藥品資料.最新訂購單價].ObjectToString();
            rJ_TextBox_訂購商.Text = Value[(int)enum_藥品補給系統_藥品資料.訂購商].ObjectToString();
            rJ_TextBox_合約商.Text = Value[(int)enum_藥品補給系統_藥品資料.合約廠商].ObjectToString();
            rJ_TextBox_維護到期日.Text = Value[(int)enum_藥品補給系統_藥品資料.維護到期日].ToDateString();
        }
        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                if(rJ_TextBox_維護到期日.Text.Check_Date_String() == false)
                {
                    MyMessageBox.ShowDialog("請輸入正確日期[維護到期日]!");
                    return;
                }
                Value[(int)enum_藥品補給系統_藥品資料.廠牌] = rJ_TextBox_廠牌.Text;
                Value[(int)enum_藥品補給系統_藥品資料.契約價金] = rJ_TextBox_契約價金.Text;
                Value[(int)enum_藥品補給系統_藥品資料.已採購總量上限] = rJ_TextBox_已採購總量上限.Text;
                Value[(int)enum_藥品補給系統_藥品資料.訂購商] = rJ_TextBox_訂購商.Text;
                Value[(int)enum_藥品補給系統_藥品資料.合約廠商] = rJ_TextBox_合約商.Text;
                Value[(int)enum_藥品補給系統_藥品資料.維護到期日] = rJ_TextBox_維護到期日.Text;
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
      
        #endregion
    }
}
