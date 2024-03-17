using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using MyUI;
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using H_Pannel_lib;

namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {



        private void sub_Program_周邊設備_麻醉部ADC_庫存_Init()
        {


            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_顯示全部.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥碼搜尋.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥名搜尋.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥名搜尋_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_匯出.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_匯出_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存顯示.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存顯示_MouseDownEvent;

            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.Init();
            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.DataGridRowsChangeRefEvent += SqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_DataGridRowsChangeRefEvent;
            this.plC_UI_Init.Add_Method(sub_Program_周邊設備_麻醉部ADC_庫存);
        }

      

        private bool flag_Program_周邊設備_麻醉部ADC_庫存_Init = false;
        private void sub_Program_周邊設備_麻醉部ADC_庫存()
        {
            if (this.plC_ScreenPage_Main.PageText == "周邊設備" && this.plC_ScreenPage_周邊設備.PageText == "麻醉部ADC" && this.plC_ScreenPage_周邊設備_麻醉部ADC.PageText == "庫存")
            {
                if (!flag_Program_周邊設備_麻醉部ADC_庫存_Init)
                {

                    flag_Program_周邊設備_麻醉部ADC_庫存_Init = true;
                }
            }
            else
            {
                flag_Program_周邊設備_麻醉部ADC_庫存_Init = false;
            }

        }

        #region Function
       
        private List<object[]> Function_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_取得資料()
        {
            List<API_trading_ADC> list_API_trading_ADC = new List<API_trading_ADC>();
            List<object[]> list_values = new List<object[]>();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string result = Basic.Net.WEBApiGet("http://10.18.28.17/api/trading");

            list_API_trading_ADC = result.JsonDeserializet<List<API_trading_ADC>>();
            for (int i = 0; i < list_API_trading_ADC.Count; i++)
            {
                object[] values = new object[new enum_周邊設備_庫存_交易紀錄查詢().GetLength()];
                values[(int)enum_周邊設備_庫存_交易紀錄查詢.動作] = list_API_trading_ADC[i].Action;
                values[(int)enum_周邊設備_庫存_交易紀錄查詢.藥碼] = list_API_trading_ADC[i].code;
                values[(int)enum_周邊設備_庫存_交易紀錄查詢.藥名] = list_API_trading_ADC[i].name;
                values[(int)enum_周邊設備_庫存_交易紀錄查詢.Room] = list_API_trading_ADC[i].room;
                values[(int)enum_周邊設備_庫存_交易紀錄查詢.庫存] = list_API_trading_ADC[i].inventory;
                values[(int)enum_周邊設備_庫存_交易紀錄查詢.交易量] = list_API_trading_ADC[i].value;
                values[(int)enum_周邊設備_庫存_交易紀錄查詢.結存量] = list_API_trading_ADC[i].balance;
                values[(int)enum_周邊設備_庫存_交易紀錄查詢.操作人] = list_API_trading_ADC[i].Operator;
                values[(int)enum_周邊設備_庫存_交易紀錄查詢.操作時間] = list_API_trading_ADC[i].Operating_time;

                list_values.Add(values);
            }
            //list_values.RemoveRow((int)enum_周邊設備_庫存_交易紀錄查詢.藥碼, "");
            //list_values.RemoveRow((int)enum_周邊設備_庫存_交易紀錄查詢.動作, enum_周邊設備_庫存_交易記錄查詢動作.實瓶繳回.GetEnumName());
            list_values.Sort(new ICP_周邊設備_麻醉部ADC_庫存_交易記錄查詢());
            return list_values;
        }
        #endregion
        #region Event
        private void SqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            List<object[]> list_value = new List<object[]>();
            if (checkBox_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_退藥.Checked)
            {
                list_value.LockAdd(RowsList.GetRows((int)enum_周邊設備_庫存_交易紀錄查詢.動作, enum_周邊設備_庫存_交易記錄查詢動作.實瓶繳回.GetEnumName()));
                list_value.LockAdd(RowsList.GetRows((int)enum_周邊設備_庫存_交易紀錄查詢.動作, enum_周邊設備_庫存_交易記錄查詢動作.空瓶繳回.GetEnumName()));
                list_value.LockAdd(RowsList.GetRows((int)enum_周邊設備_庫存_交易紀錄查詢.動作, enum_周邊設備_庫存_交易記錄查詢動作.退藥回收.GetEnumName()));
            }
            if (checkBox_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_領藥.Checked)
            {
                list_value.LockAdd(RowsList.GetRows((int)enum_周邊設備_庫存_交易紀錄查詢.動作, enum_周邊設備_庫存_交易記錄查詢動作.手輸領藥.GetEnumName()));
                list_value.LockAdd(RowsList.GetRows((int)enum_周邊設備_庫存_交易紀錄查詢.動作, enum_周邊設備_庫存_交易記錄查詢動作.掃碼領藥.GetEnumName()));

            }
            if (checkBox_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_入庫.Checked)
            {
                list_value.LockAdd(RowsList.GetRows((int)enum_周邊設備_庫存_交易紀錄查詢.動作, enum_周邊設備_庫存_交易記錄查詢動作.入庫.GetEnumName()));

            }
            list_value = list_value.GetRowsInDate((int)enum_周邊設備_庫存_交易紀錄查詢.操作時間, rJ_DatePicker_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_日期範圍_起始.Value, rJ_DatePicker_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_日期範圍_結束.Value);
            list_value.Sort(new ICP_周邊設備_麻醉部ADC_庫存_交易記錄查詢());
            for (int i = 0; i < list_value.Count; i++)
            {
                if (list_value[i][(int)enum_周邊設備_庫存_交易紀錄查詢.交易量].ObjectToString().StringIsInt32())
                {
                    list_value[i][(int)enum_周邊設備_庫存_交易紀錄查詢.交易量] = list_value[i][(int)enum_周邊設備_庫存_交易紀錄查詢.交易量].ObjectToString().StringToInt32() * -1;
                }
            }
            RowsList = list_value;
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_周邊設備庫存顯示 dialog_周邊設備庫存顯示 = new Dialog_周邊設備庫存顯示("10.18.28.17");
            dialog_周邊設備庫存顯示.ShowDialog();
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_values = this.Function_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_取得資料();


            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.RefreshGrid(list_values);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥名搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_取得資料();
            string name = this.rJ_TextBox_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥名.Text;
            if (name.StringIsEmpty()) return;
            list_value = list_value.GetRowsByLike((int)enum_周邊設備_庫存_交易紀錄查詢.藥名, name);
            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_取得資料();
            string code = this.rJ_TextBox_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥碼.Text;
            if (code.StringIsEmpty()) return;
            list_value = list_value.GetRowsByLike((int)enum_周邊設備_庫存_交易紀錄查詢.藥碼, code);
            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataTable dataTable = this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.GetDataTable();
                    dataTable = dataTable.ReorderTable(new enum_周邊設備_庫存_交易紀錄查詢());
                    string Extension = System.IO.Path.GetExtension(this.saveFileDialog_SaveExcel.FileName);
                    if (Extension == ".txt")
                    {
                        CSVHelper.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                    }
                    else if (Extension == ".xlsx")
                    {
                        MyOffice.ExcelClass.NPOI_SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                    }
                    this.Cursor = Cursors.Default;
                    MyMessageBox.ShowDialog("匯出完成");
                }
            }));
        }
        #endregion

        public class ICP_周邊設備_麻醉部ADC_庫存_交易記錄查詢 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime1 = x[(int)enum_周邊設備_庫存_交易紀錄查詢.操作時間].StringToDateTime();
                DateTime datetime2 = y[(int)enum_周邊設備_庫存_交易紀錄查詢.操作時間].StringToDateTime();
                int compare = DateTime.Compare(datetime1, datetime2);
                return compare;


            }
        }
    }
}
