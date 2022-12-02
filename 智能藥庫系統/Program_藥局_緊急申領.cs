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
using SQLUI;
using MyUI;
using Basic;
using H_Pannel_lib;
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        enum enum_藥局_緊急申領_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
        }
        enum enum_藥局_緊急申領
        {
            GUID,
            藥局代碼,
            藥品碼,
            藥品名稱,
            庫存,
            異動量,
            結存量,
            產出時間,
            過帳時間,
            狀態,
            備註,
        }

        private void sub_Program_藥局_緊急申領_Init()
        {
            this.sqL_DataGridView_藥局_緊急申領.Init();
            if(!this.sqL_DataGridView_藥局_緊急申領.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_藥局_緊急申領.SQL_CreateTable();
            }
            this.sqL_DataGridView_藥局_緊急申領.DataGridRefreshEvent += SqL_DataGridView_藥局_緊急申領_DataGridRefreshEvent;
            this.sqL_DataGridView_藥局_緊急申領.DataGridRowsChangeRefEvent += SqL_DataGridView_藥局_緊急申領_DataGridRowsChangeRefEvent;

            this.sqL_DataGridView_藥局_緊急申領_藥品資料.Init(this.sqL_DataGridView_藥局_藥品資料);
            this.sqL_DataGridView_藥局_緊急申領_藥品資料.Set_ColumnVisible(false, new enum_藥局_藥品資料().GetEnumNames());
            this.sqL_DataGridView_藥局_緊急申領_藥品資料.Set_ColumnVisible(true, enum_藥局_藥品資料.藥品碼, enum_藥局_藥品資料.藥品名稱, enum_藥局_藥品資料.總庫存, enum_藥局_藥品資料.藥局庫存, enum_藥局_藥品資料.藥庫庫存, enum_藥局_藥品資料.包裝單位);
            this.sqL_DataGridView_藥局_緊急申領_藥品資料.DataGridRowsChangeRefEvent += SqL_DataGridView_藥局_緊急申領_藥品資料_DataGridRowsChangeRefEvent;

            this.plC_RJ_Button_藥局_緊急申領_藥品資料_搜尋.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_藥品資料_搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥局_緊急申領_藥品資料_確認申領.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_藥品資料_確認申領_MouseDownEvent;


            this.plC_RJ_Button_藥局_緊急申領_顯示資料.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_顯示資料_MouseDownEvent;
            this.plC_RJ_Button_藥局_緊急申領_取消申領.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_取消申領_MouseDownEvent;


            this.plC_UI_Init.Add_Method(sub_Program_藥局_緊急申領);
        }



        private bool flag_Program_藥局_緊急申領_Init = false;
        private void sub_Program_藥局_緊急申領()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥局" && this.plC_ScreenPage_藥局.PageText == "大武院區"  && this.plC_ScreenPage_藥局.PageText == "緊急申領")
            {
                if (!flag_Program_藥局_緊急申領_Init)
                {
     
                    flag_Program_藥局_緊急申領_Init = true;
                }
            }
            else
            {
                flag_Program_藥局_緊急申領_Init = false;
            }

        }

        #region Function

        #endregion
        #region Event
        private void SqL_DataGridView_藥局_緊急申領_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_藥局_緊急申領());
        }
        private void SqL_DataGridView_藥局_緊急申領_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].Cells[(int)enum_藥局_緊急申領.狀態].Value.ToString();
                if (狀態 == enum_藥局_緊急申領_狀態.過帳完成.GetEnumName())
                {
                    this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_藥局_緊急申領_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_藥局_緊急申領_狀態.未建立儲位.GetEnumName())
                {
                    this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }

            }
        }
        private void SqL_DataGridView_藥局_緊急申領_藥品資料_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_藥局_藥品資料());
        }
        private void PlC_RJ_Button_藥局_緊急申領_藥品資料_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥局_緊急申領_藥品資料.SQL_GetAllRows(false);
            this.sqL_DataGridView_藥局_藥品資料.RowsChangeFunction(list_value);

            if (this.rJ_TextBox_藥局_緊急申領_藥品資料_藥品碼.Texts.StringIsEmpty() == false)
            {
                list_value = list_value.GetRowsByLike((int)enum_藥局_藥品資料.藥品碼, this.rJ_TextBox_藥局_緊急申領_藥品資料_藥品碼.Texts);
            }
            if (this.rJ_TextBox_藥局_緊急申領_藥品資料_藥品名稱.Texts.StringIsEmpty() == false)
            {
                list_value = list_value.GetRowsByLike((int)enum_藥局_藥品資料.藥品名稱, this.rJ_TextBox_藥局_緊急申領_藥品資料_藥品名稱.Texts);
            }
            this.sqL_DataGridView_藥局_緊急申領_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥局_緊急申領_藥品資料_確認申領_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥局_緊急申領_藥品資料.Get_All_Select_RowsValues();
            if(list_藥品資料.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取藥品資料!");
                return;
            }
            int 申領數量 = 0;
            DialogResult dialogResult = DialogResult.None;
            Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
            this.Invoke(new Action(delegate
            {
                dialogResult = dialog_NumPannel.ShowDialog();
                申領數量 = dialog_NumPannel.Value;
            }));
            if (dialogResult != DialogResult.Yes) return;
            if(申領數量 <= 0)
            {
                MyMessageBox.ShowDialog("申領數量不得小於'0'");
                return;
            }
            object[] value = new object[new enum_藥局_緊急申領().GetLength()];
            value[(int)enum_藥局_緊急申領.GUID] = Guid.NewGuid().ToString();
            value[(int)enum_藥局_緊急申領.藥局代碼] = enum_庫別.屏榮藥局.GetEnumName();
            value[(int)enum_藥局_緊急申領.藥品碼] = list_藥品資料[0][(int)enum_藥局_藥品資料.藥品碼].ObjectToString();
            value[(int)enum_藥局_緊急申領.藥品名稱] = list_藥品資料[0][(int)enum_藥局_藥品資料.藥品名稱].ObjectToString();
            value[(int)enum_藥局_緊急申領.庫存] = 0;
            value[(int)enum_藥局_緊急申領.異動量] = 申領數量;
            value[(int)enum_藥局_緊急申領.結存量] = 0;
            value[(int)enum_藥局_緊急申領.產出時間] = DateTime.Now.ToDateTimeString_6();
            value[(int)enum_藥局_緊急申領.過帳時間] = DateTime.MinValue.ToDateTimeString();
            value[(int)enum_藥局_緊急申領.狀態] = enum_藥局_緊急申領_狀態.等待過帳.GetEnumName();
            value[(int)enum_藥局_緊急申領.備註] = "";

            this.sqL_DataGridView_藥局_緊急申領.SQL_AddRow(value, false);
            this.sqL_DataGridView_藥局_緊急申領.AddRow(value, true);
        }
        private void PlC_RJ_Button_藥局_緊急申領_顯示資料_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_藥局_緊急申領.SQL_GetRowsByBetween((int)enum_藥局_緊急申領.產出時間, rJ_DatePicker_藥局_緊急申領_產出日期_起始, rJ_DatePicker_藥局_緊急申領_產出日期_結束, true);
        }
        private void PlC_RJ_Button_藥局_緊急申領_取消申領_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥局_緊急申領.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取申領資料!");
                return;
            }
            
            list_value = (from value in list_value
                          where value[(int)enum_藥局_緊急申領.狀態].ObjectToString() != enum_藥局_緊急申領_狀態.過帳完成.GetEnumName()
                          select value).ToList();

            if (list_value.Count == 0) return;
            if (MyMessageBox.ShowDialog($"確認取消申領選取{list_value.Count}筆資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;

            this.sqL_DataGridView_藥局_緊急申領.SQL_DeleteExtra(list_value, false);
            this.sqL_DataGridView_藥局_緊急申領.DeleteExtra(list_value, true);
        }
        #endregion

        private class ICP_藥局_緊急申領 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime0 = x[(int)enum_藥局_緊急申領.產出時間].ToDateTimeString_6().StringToDateTime();
                DateTime datetime1 = y[(int)enum_藥局_緊急申領.產出時間].ToDateTimeString_6().StringToDateTime();
                return datetime0.CompareTo(datetime1);
            }
        }
    }
}
