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
using HIS_DB_Lib;
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
            備藥中,
            撥發完成,
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
            if (!this.sqL_DataGridView_藥局_緊急申領.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_藥局_緊急申領.SQL_CreateTable();
            }
            this.sqL_DataGridView_藥局_緊急申領_資料查詢.Init();
            this.sqL_DataGridView_藥局_緊急申領.DataGridRefreshEvent += SqL_DataGridView_藥局_緊急申領_DataGridRefreshEvent;
            this.sqL_DataGridView_藥局_緊急申領.DataGridRowsChangeRefEvent += SqL_DataGridView_藥局_緊急申領_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_藥局_緊急申領.CellValidatingEvent += SqL_DataGridView_藥局_緊急申領_CellValidatingEvent;
            this.sqL_DataGridView_藥局_緊急申領.RowEndEditEvent += SqL_DataGridView_藥局_緊急申領_RowEndEditEvent;

            this.sqL_DataGridView_藥局_緊急申領_藥品資料.Init(this.sqL_DataGridView_藥局_藥品資料);
            this.sqL_DataGridView_藥局_緊急申領_藥品資料.Set_ColumnVisible(false, new enum_藥局_藥品資料().GetEnumNames());
            this.sqL_DataGridView_藥局_緊急申領_藥品資料.Set_ColumnVisible(true, enum_藥局_藥品資料.藥品碼, enum_藥局_藥品資料.藥品名稱, enum_藥局_藥品資料.總庫存, enum_藥局_藥品資料.藥局庫存, enum_藥局_藥品資料.藥庫庫存, enum_藥局_藥品資料.包裝單位, enum_藥局_藥品資料.包裝數量);
            this.sqL_DataGridView_藥局_緊急申領_藥品資料.DataGridRowsChangeRefEvent += SqL_DataGridView_藥局_緊急申領_藥品資料_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_藥局_緊急申領_藥品資料.RowDoubleClickEvent += SqL_DataGridView_藥局_緊急申領_藥品資料_RowDoubleClickEvent;

            this.plC_RJ_Button_藥局_緊急申領_藥品資料_搜尋.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_藥品資料_搜尋_MouseDownEvent;

            this.plC_RJ_Button_藥局_緊急申領_確認申領.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_確認申領_MouseDownEvent;
            this.plC_RJ_Button_藥局_緊急申領_顯示資料.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_顯示資料_MouseDownEvent;
            this.plC_RJ_Button_藥局_緊急申領_取消申領.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_取消申領_MouseDownEvent;
            this.plC_RJ_Button_藥局_緊急申領_刪除選取資料.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_刪除選取資料_MouseDownEvent;
            this.plC_RJ_Button_藥局_緊急申領_資料查詢_藥碼.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_資料查詢_藥碼_MouseDownEvent;
            this.plC_RJ_Button_藥局_緊急申領_資料查詢_申領單位.MouseDownEvent += PlC_RJ_Button_藥局_緊急申領_資料查詢_申領單位_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_藥局_緊急申領);
        }

 

        private bool flag_Program_藥局_緊急申領_Init = false;
        private void sub_Program_藥局_緊急申領()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥局" && this.plC_ScreenPage_藥局.PageText == "大武院區" && this.plC_ScreenPage_藥局.PageText == "緊急申領")
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
        private void SqL_DataGridView_藥局_緊急申領_藥品資料_RowDoubleClickEvent(object[] RowValue)
        {
            string 申領單位 = comboBox_藥局_緊急申領_申領單位.Text;
            if(申領單位.StringIsEmpty())
            {
                MyMessageBox.ShowDialog("未選擇申領單位!");
                return;
            }
            string 藥品碼 = RowValue[(int)enum_藥局_藥品資料.藥品碼].ObjectToString();
            string 藥品名稱 = RowValue[(int)enum_藥局_藥品資料.藥品名稱].ObjectToString();

            List<object[]> list_SQL緊急申領資料 = this.sqL_DataGridView_藥局_緊急申領.SQL_GetRowsByBetween((int)enum_藥局_緊急申領.產出時間, DateTime.Now, false);
            List<object[]> list_SQL緊急申領資料_buf = new List<object[]>();
            list_SQL緊急申領資料_buf = list_SQL緊急申領資料.GetRows((int)enum_藥局_緊急申領.藥品碼, 藥品碼);
            if (list_SQL緊急申領資料_buf.Count > 0)
            {
                int temp = 0;
                for (int i = 0; i < list_SQL緊急申領資料_buf.Count; i++)
                {
                    temp += list_SQL緊急申領資料_buf[0][(int)enum_藥局_緊急申領.異動量].StringToInt32();
                }
                string 異動量 = temp.ToString();
                MyMessageBox.ShowDialog($"<{藥品名稱}> ,今日已申領數量<{異動量}>");
            }
            List<object[]> list_緊急申領資料 = this.sqL_DataGridView_藥局_緊急申領.GetAllRows();
            List<object[]> list_緊急申領資料_buf = new List<object[]>();
            list_緊急申領資料_buf = list_緊急申領資料.GetRows((int)enum_藥局_緊急申領.藥品碼, 藥品碼);
            if (list_緊急申領資料_buf.Count == 0)
            {
                object[] value = new object[new enum_藥局_緊急申領().GetLength()];
                value[(int)enum_藥局_緊急申領.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_藥局_緊急申領.藥局代碼] = enum_庫別.屏榮藥局.GetEnumName();
                value[(int)enum_藥局_緊急申領.藥品碼] = 藥品碼;
                value[(int)enum_藥局_緊急申領.藥品名稱] = 藥品名稱;
                value[(int)enum_藥局_緊急申領.庫存] = 0;
                value[(int)enum_藥局_緊急申領.異動量] = "0";
                value[(int)enum_藥局_緊急申領.結存量] = 0;
                value[(int)enum_藥局_緊急申領.產出時間] = DateTime.Now.ToDateTimeString_6();
                value[(int)enum_藥局_緊急申領.過帳時間] = DateTime.MinValue.ToDateTimeString();
                value[(int)enum_藥局_緊急申領.狀態] = enum_藥局_緊急申領_狀態.等待過帳.GetEnumName();
                value[(int)enum_藥局_緊急申領.備註] = $"申領單位:{申領單位},申領人:{登入者名稱}\n";
                this.sqL_DataGridView_藥局_緊急申領.AddRow(value, true);
            }
            else
            {
                object[] value = new object[new enum_藥局_緊急申領().GetLength()];
                value[(int)enum_藥局_緊急申領.GUID] = list_緊急申領資料_buf[0][(int)enum_藥局_緊急申領.GUID].ObjectToString();
                value[(int)enum_藥局_緊急申領.藥局代碼] = enum_庫別.屏榮藥局.GetEnumName();
                value[(int)enum_藥局_緊急申領.藥品碼] = 藥品碼;
                value[(int)enum_藥局_緊急申領.藥品名稱] = 藥品名稱;
                value[(int)enum_藥局_緊急申領.庫存] = 0;
                value[(int)enum_藥局_緊急申領.異動量] = "0";
                value[(int)enum_藥局_緊急申領.結存量] = 0;
                value[(int)enum_藥局_緊急申領.產出時間] = DateTime.Now.ToDateTimeString_6();
                value[(int)enum_藥局_緊急申領.過帳時間] = DateTime.MinValue.ToDateTimeString();
                value[(int)enum_藥局_緊急申領.狀態] = enum_藥局_緊急申領_狀態.等待過帳.GetEnumName();
                value[(int)enum_藥局_緊急申領.備註] = $"申領單位:{申領單位},申領人:{登入者名稱}\n";
                this.sqL_DataGridView_藥局_緊急申領.ReplaceExtra(value, true);
            }
        }
        private void SqL_DataGridView_藥局_緊急申領_CellValidatingEvent(object[] RowValue, int rowIndex, int colIndex, string value, DataGridViewCellValidatingEventArgs e)
        {
            string 藥碼 = RowValue[(int)enum_藥局_緊急申領.藥品碼].ObjectToString();
            string 異動量 = value;
            //if (異動量.StringToInt32() < 0)
            //{
            //    MyMessageBox.ShowDialog("請輸入正確數字(大於'0')!");
            //    e.Cancel = true;
            //}
            List<object[]> list_藥庫_藥品資料 = sqL_DataGridView_藥庫_藥品資料.SQL_GetRows((int)enum_藥庫_藥品資料.藥品碼, 藥碼, false);
            if (list_藥庫_藥品資料.Count > 0)
            {
                string 包裝數量 = list_藥庫_藥品資料[0][(int)enum_藥庫_藥品資料.包裝數量].ObjectToString();
                if (包裝數量.StringIsInt32() == true)
                {
                    int temp = 包裝數量.StringToInt32();
                    if (異動量.StringToInt32() % temp != 0)
                    {
                        MyMessageBox.ShowDialog($"【提醒】藥品包裝量為<{temp}>,此非可整除包裝量");
       
                    }
                    if (異動量.StringToInt32() > 0)
                    {
                        this.sqL_DataGridView_藥庫_藥品資料.RowsChangeFunction(list_藥庫_藥品資料);
                        if (list_藥庫_藥品資料.Count > 0)
                        {
                            int 藥庫庫存 = list_藥庫_藥品資料[0][(int)enum_藥庫_藥品資料.藥庫庫存].StringToInt32();
                            List<object[]> list_緊急申領 = this.sqL_DataGridView_藥局_緊急申領_資料查詢.SQL_GetRows((int)enum_藥局_緊急申領.藥品碼, 藥碼, false);
                            list_緊急申領 = list_緊急申領.GetRowsInDate((int)enum_藥局_緊急申領.產出時間, DateTime.Now);
                            int 申領數量 = 0;
                            for (int i = 0; i < list_緊急申領.Count; i++)
                            {
                                申領數量 += list_緊急申領[i][(int)enum_藥局_緊急申領.異動量].ObjectToString().StringToInt32();
                            }
                            if ((申領數量 + 異動量.StringToInt32()) > 藥庫庫存)
                            {
                                MyMessageBox.ShowDialog($"今日已申領數量<{申領數量}>,藥庫庫存<{藥庫庫存}>,短缺<{(藥庫庫存 - (申領數量 + 異動量.StringToInt32())) * -1}>無法申領!");
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                    if (異動量.StringToInt32() < 0)
                    {
                        List<object[]> list_value = this.sqL_DataGridView_藥局_緊急申領_藥品資料.SQL_GetRows((int)enum_藥局_藥品資料.藥品碼, 藥碼, false);
                        this.sqL_DataGridView_藥局_藥品資料.RowsChangeFunction(list_value);
                        if(list_value.Count > 0)
                        {
                            int 藥局庫存 = list_value[0][(int)enum_藥局_藥品資料.藥局庫存].StringToInt32();
                            if(異動量.StringToInt32() + 藥局庫存 < 0)
                            {
                                MyMessageBox.ShowDialog($"藥局庫存<{藥局庫存}>,不夠退回藥庫數量!");
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void SqL_DataGridView_藥局_緊急申領_RowEndEditEvent(object[] RowValue, int rowIndex, int colIndex, string value)
        {
            this.sqL_DataGridView_藥局_緊急申領.ReplaceExtra(RowValue, true);
       
        }
        private void SqL_DataGridView_藥局_緊急申領_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_藥局_緊急申領());
        }
        private void SqL_DataGridView_藥局_緊急申領_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].Cells[enum_藥局_緊急申領.狀態.GetEnumName()].Value.ToString();
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
                if (狀態 == enum_藥局_緊急申領_狀態.備藥中.GetEnumName())
                {
                    this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_藥局_緊急申領_狀態.撥發完成.GetEnumName())
                {
                    this.sqL_DataGridView_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
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
                list_value = list_value.GetRowsStartWithByLike((int)enum_藥局_藥品資料.藥品碼, this.rJ_TextBox_藥局_緊急申領_藥品資料_藥品碼.Texts);
            }
            if (this.rJ_TextBox_藥局_緊急申領_藥品資料_藥品名稱.Texts.StringIsEmpty() == false)
            {
                list_value = list_value.GetRowsStartWithByLike((int)enum_藥局_藥品資料.藥品名稱, this.rJ_TextBox_藥局_緊急申領_藥品資料_藥品名稱.Texts);
            }
            this.sqL_DataGridView_藥局_緊急申領_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥局_緊急申領_刪除選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_緊急申領資料 = this.sqL_DataGridView_藥局_緊急申領.Get_All_Select_RowsValues();
            this.sqL_DataGridView_藥局_緊急申領.DeleteExtra(list_緊急申領資料, true);
        }
        private void PlC_RJ_Button_藥局_緊急申領_確認申領_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_緊急申領資料 = this.sqL_DataGridView_藥局_緊急申領.GetAllRows();
            this.sqL_DataGridView_藥局_緊急申領.SQL_AddRows(list_緊急申領資料, false);
            this.sqL_DataGridView_藥局_緊急申領.ClearGrid();
            MyMessageBox.ShowDialog("申領完成!");
            //List<object[]> list_藥品資料 = this.sqL_DataGridView_藥局_緊急申領_藥品資料.Get_All_Select_RowsValues();
            //if (list_藥品資料.Count == 0)
            //{
            //    MyMessageBox.ShowDialog("未選取藥品資料!");
            //    return;
            //}
            //int 申領數量 = 0;
            //DialogResult dialogResult = DialogResult.None;
            //Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
            //this.Invoke(new Action(delegate
            //{
            //    dialogResult = dialog_NumPannel.ShowDialog();
            //    申領數量 = dialog_NumPannel.Value;
            //}));
            //if (dialogResult != DialogResult.Yes) return;
            //if (申領數量 <= 0)
            //{
            //    MyMessageBox.ShowDialog("申領數量不得小於'0'");
            //    return;
            //}
            //object[] value = new object[new enum_藥局_緊急申領().GetLength()];
            //value[(int)enum_藥局_緊急申領.GUID] = Guid.NewGuid().ToString();
            //value[(int)enum_藥局_緊急申領.藥局代碼] = enum_庫別.屏榮藥局.GetEnumName();
            //value[(int)enum_藥局_緊急申領.藥品碼] = list_藥品資料[0][(int)enum_藥局_藥品資料.藥品碼].ObjectToString();
            //value[(int)enum_藥局_緊急申領.藥品名稱] = list_藥品資料[0][(int)enum_藥局_藥品資料.藥品名稱].ObjectToString();
            //value[(int)enum_藥局_緊急申領.庫存] = 0;
            //value[(int)enum_藥局_緊急申領.異動量] = 申領數量;
            //value[(int)enum_藥局_緊急申領.結存量] = 0;
            //value[(int)enum_藥局_緊急申領.產出時間] = DateTime.Now.ToDateTimeString_6();
            //value[(int)enum_藥局_緊急申領.過帳時間] = DateTime.MinValue.ToDateTimeString();
            //value[(int)enum_藥局_緊急申領.狀態] = enum_藥局_緊急申領_狀態.等待過帳.GetEnumName();
            //value[(int)enum_藥局_緊急申領.備註] = "";

            //this.sqL_DataGridView_藥局_緊急申領.SQL_AddRow(value, false);
            //this.sqL_DataGridView_藥局_緊急申領.AddRow(value, true);
        }
        private void PlC_RJ_Button_藥局_緊急申領_顯示資料_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_藥局_緊急申領.SQL_GetRowsByBetween((int)enum_藥局_緊急申領.產出時間, rJ_DatePicker_藥局_緊急申領_產出日期_起始, rJ_DatePicker_藥局_緊急申領_產出日期_結束, true);
        }
        private void PlC_RJ_Button_藥局_緊急申領_取消申領_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥局_緊急申領.Get_All_Checked_RowsValues();
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
        private void PlC_RJ_Button_藥局_緊急申領_資料查詢_申領單位_MouseDownEvent(MouseEventArgs mevent)
        {
            string 申領單位 = "";
            this.Invoke(new Action(delegate 
            {
                申領單位 = comboBox_藥局_緊急申領_資料查詢_申領單位.Text;
     
            }));
            if (申領單位.StringIsEmpty())
            {
                MyMessageBox.ShowDialog("請選擇申領單位!");
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_藥局_緊急申領_資料查詢.SQL_GetRowsByBetween((int)enum_藥局_緊急申領.產出時間, rJ_DatePicker_藥局_緊急申領_產出日期_起始, rJ_DatePicker_藥局_緊急申領_產出日期_結束, false);
            List<object[]> list_value_buf = new List<object[]>();
        
            for (int i = 0; i < list_value.Count; i++)
            {
                string 備註 = list_value[i][(int)enum_藥局_緊急申領.備註].ObjectToString();
                if (備註.Contains(申領單位)) list_value_buf.Add(list_value[i]);
            }
            if (list_value_buf.Count == 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
                return;
            }
            this.sqL_DataGridView_藥局_緊急申領_資料查詢.RefreshGrid(list_value_buf);
        }
        private void PlC_RJ_Button_藥局_緊急申領_資料查詢_藥碼_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥局_緊急申領_資料查詢.SQL_GetRowsByBetween((int)enum_藥局_緊急申領.產出時間, rJ_DatePicker_藥局_緊急申領_產出日期_起始, rJ_DatePicker_藥局_緊急申領_產出日期_結束, false);
            List<object[]> list_value_buf = new List<object[]>();
            if(rJ_TextBox_藥局_緊急申領_資料查詢_藥碼.Texts.StringIsEmpty())
            {
                MyMessageBox.ShowDialog("藥碼空白!");
                return;
            }
            list_value_buf = list_value.GetRows((int)enum_藥局_緊急申領.藥品碼, rJ_TextBox_藥局_緊急申領_資料查詢_藥碼.Texts);
            if (list_value_buf.Count == 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
                return;
            }
            this.sqL_DataGridView_藥局_緊急申領_資料查詢.RefreshGrid(list_value_buf);
        }
        #endregion

        private class ICP_藥局_緊急申領 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string 藥品碼0 = x[(int)enum_藥局_緊急申領.藥品碼].ObjectToString();
                string 藥品碼1 = y[(int)enum_藥局_緊急申領.藥品碼].ObjectToString();
                int temp = 藥品碼0.CompareTo(藥品碼1);
                if (temp == 0)
                {
                    DateTime datetime0 = x[(int)enum_藥局_緊急申領.產出時間].ToDateTimeString_6().StringToDateTime();
                    DateTime datetime1 = y[(int)enum_藥局_緊急申領.產出時間].ToDateTimeString_6().StringToDateTime();
                    return datetime0.CompareTo(datetime1);
                }
                else
                {
                    return temp;
                }
            }
        }
    }
}
