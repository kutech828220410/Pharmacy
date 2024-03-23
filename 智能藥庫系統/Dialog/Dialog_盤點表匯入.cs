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
using MyUI;
using SQLUI;
namespace 智能藥庫系統
{
    public partial class Dialog_盤點表匯入 : MyDialog
    {
        public static bool IsShown = false;
        public Dialog_盤點表匯入()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            stepViewer1.AddSetp("匯入", "批次選擇匯入檔案");
            stepViewer1.AddSetp("上傳", "上傳至伺服器");
            stepViewer1.AddSetp("完成", "表單建立成功");
            stepViewer1.CurrentStep = 1;
            this.TopLevel = true;
            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowDialogEvent += Dialog_盤點表匯入_ShowDialogEvent;
            this.Load += Dialog_盤點表匯入_Load;
            this.FormClosing += Dialog_盤點表匯入_FormClosing;
            this.CancelButton = this.plC_RJ_Button_返回;
            this.plC_RJ_Button_返回.MouseDownEvent += PlC_RJ_Button_返回_MouseDownEvent;
        }

        private void Dialog_盤點表匯入_ShowDialogEvent()
        {
            if (IsShown)
            {
                MyMessageBox.ShowDialog("視窗已開啟!");
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void PlC_RJ_Button_返回_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
        }
        private void Dialog_盤點表匯入_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsShown = false;
        }
        private void Dialog_盤點表匯入_Load(object sender, EventArgs e)
        {
            IsShown = true;
        }
    }
}
