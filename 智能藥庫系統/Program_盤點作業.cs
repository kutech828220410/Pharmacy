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
    
    public partial class Main_Form : Form
    {
        private void sub_Program_盤點作業_Init()
        {
            this.plC_RJ_Button_盤點作業_盤點表匯入.MouseDownEvent += PlC_RJ_Button_盤點作業_盤點表匯入_MouseDownEvent;
            this.plC_RJ_Button_盤點作業_盤點單管理.MouseDownEvent += PlC_RJ_Button_盤點作業_盤點單管理_MouseDownEvent;
            this.plC_RJ_Button_盤點作業_盤點單合併.MouseDownEvent += PlC_RJ_Button_盤點作業_盤點單合併_MouseDownEvent;
        }

        private void PlC_RJ_Button_盤點作業_盤點單合併_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_盤點單合併 dialog_盤點單合併 = Dialog_盤點單合併.GetForm();
            dialog_盤點單合併.ShowDialog();
        }
        private void PlC_RJ_Button_盤點作業_盤點單管理_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_盤點單管理 dialog_盤點單管理 = new Dialog_盤點單管理();
            dialog_盤點單管理.ShowDialog();
        }
        private void PlC_RJ_Button_盤點作業_盤點表匯入_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_盤點表匯入 dialog_盤點表匯入 = new Dialog_盤點表匯入();
            dialog_盤點表匯入.ShowDialog();


        }
    }
}
