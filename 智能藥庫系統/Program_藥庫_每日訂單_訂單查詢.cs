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
using HIS_DB_Lib;
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        public enum enum_藥庫_每日訂單_訂單查詢 : int
        {
            GUID,
            藥品碼,
            中文名稱,
            藥品名稱,
            包裝單位,
            今日訂購數量,
            緊急訂購數量,
            訂購時間,
        }

        private void sub_Program_藥庫_每日訂單_訂單查詢_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_每日訂單, dBConfigClass.DB_posting_server);
            this.sqL_DataGridView_每日訂單.Init();
            if (!this.sqL_DataGridView_每日訂單.SQL_IsTableCreat()) this.sqL_DataGridView_每日訂單.SQL_CreateTable();


            this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.Init();
            this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.DataGridRowsChangeRefEvent += SqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料_DataGridRowsChangeRefEvent;

            this.plC_RJ_Button_藥庫_每日訂單_訂購資料_顯示全部.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_訂購資料_顯示全部_MouseDownEvent;
            this.plC_RJ_ButtonrJ_DatePicker_藥庫_每日訂單_訂購資料_訂購時間搜尋.MouseDownEvent += PlC_RJ_ButtonrJ_DatePicker_藥庫_每日訂單_訂購資料_訂購時間搜尋_MouseDownEvent;
            this.plC_RJ_Buttonr_藥庫_每日訂單_訂購資料_藥品碼搜尋.MouseDownEvent += PlC_RJ_Buttonr_藥庫_每日訂單_訂購資料_藥品碼搜尋_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_每日訂單_訂單查詢);
        }

    

        private bool flag_藥庫_每日訂單_訂單查詢 = false;
        private void sub_Program_藥庫_每日訂單_訂單查詢()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "每日訂單")
            {
                if (!this.flag_藥庫_每日訂單_訂單查詢)
                {
                    this.Function_堆疊資料_刪除指定調劑台名稱母資料("藥庫");
                    this.flag_藥庫_每日訂單_訂單查詢 = true;
                }

            }
            else
            {
                this.flag_藥庫_每日訂單_訂單查詢 = false;
            }
        }

        #region Function
        public List<object[]> Function_藥庫_每日訂單_訂單查詢_取得訂單資料()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            Console.WriteLine($"Function_藥庫_每日訂單_訂單查詢_取得訂單資料");

            List<object[]> list_value = new List<object[]>();
            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            Console.WriteLine($"取得藥品資料,耗時{myTimer.ToString()}");
            List<object[]> list_訂單資料 = this.sqL_DataGridView_每日訂單.SQL_GetAllRows(false);
            list_value = list_訂單資料.CopyRows(new enum_每日訂單(), new enum_藥庫_每日訂單_訂單查詢());
            Console.WriteLine($"取得訂單資料,耗時{myTimer.ToString()}");

            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 中文名稱 = "";
            string 包裝單位 = "";
            int 今日訂購數量 = 0;
            int 緊急訂購數量 = 0;

            for (int i = 0; i < list_value.Count; i++)
            {
                今日訂購數量 = list_value[i][(int)enum_藥庫_每日訂單_訂單查詢.今日訂購數量].ObjectToString().StringToInt32();
                緊急訂購數量 = list_value[i][(int)enum_藥庫_每日訂單_訂單查詢.緊急訂購數量].ObjectToString().StringToInt32();
                if (!(今日訂購數量 > 0 || 緊急訂購數量 > 0)) continue;
                藥品碼 = list_value[i][(int)enum_藥庫_每日訂單_訂單查詢.藥品碼].ObjectToString();
                if (藥品碼.Length != 5)
                {
                    continue;
                }
                藥品名稱 = "";
                中文名稱 = "";
                包裝單位 = "";

                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥庫_藥品資料.藥品碼, 藥品碼);
                if(list_藥品資料_buf.Count > 0)
                {
                    藥品名稱 = list_藥品資料_buf[0][(int)enum_藥庫_藥品資料.藥品名稱].ObjectToString();
                    中文名稱 = list_藥品資料_buf[0][(int)enum_藥庫_藥品資料.中文名稱].ObjectToString();
                    包裝單位 = list_藥品資料_buf[0][(int)enum_藥庫_藥品資料.包裝單位].ObjectToString();
                }
                list_value[i][(int)enum_藥庫_每日訂單_訂單查詢.藥品名稱] = 藥品名稱;
                list_value[i][(int)enum_藥庫_每日訂單_訂單查詢.中文名稱] = 中文名稱;
                list_value[i][(int)enum_藥庫_每日訂單_訂單查詢.包裝單位] = 包裝單位;
                list_value_buf.Add(list_value[i]);
            }

            return list_value_buf;
        }
        #endregion
        #region Event
        private void SqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            for(int i = 0; i < RowsList.Count; i++)
            {
                RowsList[i][(int)enum_藥庫_每日訂單_訂單查詢.訂購時間] = RowsList[i][(int)enum_藥庫_每日訂單_訂單查詢.訂購時間].ToDateTimeString();
            }
            RowsList.Sort(new ICP_藥庫_每日訂單_訂單查詢());
        }
        private void PlC_RJ_Button_藥庫_每日訂單_訂購資料_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥庫_每日訂單_訂單查詢_取得訂單資料();
            this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_ButtonrJ_DatePicker_藥庫_每日訂單_訂購資料_訂購時間搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥庫_每日訂單_訂單查詢_取得訂單資料();
            list_value = list_value.GetRowsInDate((int)enum_藥庫_每日訂單_訂單查詢.訂購時間, rJ_DatePicker_藥庫_每日訂單_訂購資料_訂購時間起始, rJ_DatePicker_藥庫_每日訂單_訂購資料_訂購時間結束);
            this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Buttonr_藥庫_每日訂單_訂購資料_藥品碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥庫_每日訂單_訂單查詢_取得訂單資料();
            list_value = list_value.GetRowsByLike((int)enum_藥庫_每日訂單_訂單查詢.藥品碼, rJ_TextBoxr_藥庫_每日訂單_訂購資料_藥品碼搜尋.Text);
            this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.RefreshGrid(list_value);
        }
        #endregion
        private class ICP_藥庫_每日訂單_訂單查詢 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                DateTime temp0 = x[(int)enum_藥庫_每日訂單_訂單查詢.訂購時間].StringToDateTime();
                DateTime temp1 = y[(int)enum_藥庫_每日訂單_訂單查詢.訂購時間].StringToDateTime();
                int cmp = temp0.CompareTo(temp1);
                return cmp;
            }
        }

    }
}
