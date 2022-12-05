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
using Basic;
using MyUI;
namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {
        private enum enum_Log
        {
            GUID,
            內容,
            時間,
        
        }

        private void sub_Program_Log_Init()
        {
            SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_Log, this.dBConfigClass.DB_Basic);

            this.sqL_DataGridView_Log.Init();
            if (!this.sqL_DataGridView_Log.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_Log.SQL_CreateTable();
            }
            this.sqL_DataGridView_Log.DataGridRefreshEvent += SqL_DataGridView_Log_DataGridRefreshEvent;
            this.sqL_DataGridView_Log.DataGridRowsChangeRefEvent += SqL_DataGridView_Log_DataGridRowsChangeRefEvent;

            this.rJ_ComboBox_Log_藥局代碼.Enter += RJ_ComboBox_Log_藥局代碼_Enter;
            this.rJ_ComboBox_Log_來源報表.Enter += RJ_ComboBox_Log_來源報表_Enter;
            this.rJ_ComboBox_Log_事件內容.Enter += RJ_ComboBox_Log_事件內容_Enter;

            this.plC_RJ_Button_Log_顯示全部.MouseDownEvent += PlC_RJ_Button_Log_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_Log_刪除.MouseDownEvent += PlC_RJ_Button_Log_刪除_MouseDownEvent;
            this.plC_RJ_Button_Log_藥局代碼_搜尋.MouseDownEvent += PlC_RJ_Button_Log_藥局代碼_搜尋_MouseDownEvent;
            this.plC_RJ_Button_Log_藥品碼_搜尋.MouseDownEvent += PlC_RJ_Button_Log_藥品碼_搜尋_MouseDownEvent;
            this.plC_RJ_Button_Log_來源報表_搜尋.MouseDownEvent += PlC_RJ_Button_Log_來源報表_搜尋_MouseDownEvent;
            this.plC_RJ_Button_Log_事件內容_搜尋.MouseDownEvent += PlC_RJ_Button_Log_事件內容_搜尋_MouseDownEvent;

            this.plC_UI_Init.Add_Method(this.sub_Program_Log);
        }

 

        private void sub_Program_Log()
        {

        }
        #region Function
        private void Function_Log_寫入資料(string text)
        {
            object[] value = new object[new enum_Log().GetLength()];
            value[(int)enum_Log.GUID] = Guid.NewGuid().ToString();
            value[(int)enum_Log.內容] = text;
            value[(int)enum_Log.時間] = DateTime.Now.ToDateTimeString_6();
        }
        #endregion
        #region Event
        private void RJ_ComboBox_Log_來源報表_Enter(object sender, EventArgs e)
        {
            List<string> list_來源報表 = new List<string>();
            Task.Run(() =>
            {
                MyTimer myTimer = new MyTimer();
                myTimer.TickStop();
                myTimer.StartTickTime(50000);

                List<string> list_text = new List<string>();

                List<string> list_來源報表_buf = new List<string>();
                List<object[]> list_value = this.sqL_DataGridView_Log.SQL_GetAllRows(false);
                for (int i = 0; i < list_value.Count; i++)
                {
                    list_text.Add(list_value[i][(int)enum_Log.內容].ObjectToString());
                }
                for (int i = 0; i < list_text.Count; i++)
                {
                    list_來源報表_buf = list_text[i].GetTextValues("來源報表");
                    if (list_來源報表_buf.Count == 0) continue;
                    list_來源報表.Add(list_來源報表_buf[0]);
                }
                list_來源報表 = list_來源報表.Distinct().ToList();


                Console.WriteLine($"取得來源報表Log 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
            }).Wait();
            this.rJ_ComboBox_Log_來源報表.DataSource = list_來源報表;
        }
        private void RJ_ComboBox_Log_藥局代碼_Enter(object sender, EventArgs e)
        {
            List<string> list_藥局代碼 = new List<string>();
            Task.Run(() =>
            {
                MyTimer myTimer = new MyTimer();
                myTimer.TickStop();
                myTimer.StartTickTime(50000);

                List<string> list_text = new List<string>();

                List<string> list_藥局代碼_buf = new List<string>();
                List<object[]> list_value = this.sqL_DataGridView_Log.SQL_GetAllRows(false);
                for (int i = 0; i < list_value.Count; i++)
                {
                    list_text.Add(list_value[i][(int)enum_Log.內容].ObjectToString());
                }
                for (int i = 0; i < list_text.Count; i++)
                {
                    list_藥局代碼_buf = list_text[i].GetTextValues("藥局代碼");
                    if (list_藥局代碼_buf.Count == 0) continue;
                    list_藥局代碼.Add(list_藥局代碼_buf[0]);
                }
                list_藥局代碼 = list_藥局代碼.Distinct().ToList();
   

                Console.WriteLine($"取得藥局代碼Log 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
            }).Wait();
            this.rJ_ComboBox_Log_藥局代碼.DataSource = list_藥局代碼;
        }
        private void RJ_ComboBox_Log_事件內容_Enter(object sender, EventArgs e)
        {
            List<string> list_事件內容 = new List<string>();
            Task.Run(() =>
            {
                MyTimer myTimer = new MyTimer();
                myTimer.TickStop();
                myTimer.StartTickTime(50000);

                List<string> list_text = new List<string>();

                List<string> list_事件內容_buf = new List<string>();
                List<object[]> list_value = this.sqL_DataGridView_Log.SQL_GetAllRows(false);
                for (int i = 0; i < list_value.Count; i++)
                {
                    list_text.Add(list_value[i][(int)enum_Log.內容].ObjectToString());
                }
                for (int i = 0; i < list_text.Count; i++)
                {
                    list_事件內容_buf = list_text[i].GetTextValues("事件內容");
                    if (list_事件內容_buf.Count == 0) continue;
                    list_事件內容.Add(list_事件內容_buf[0]);
                }
                list_事件內容 = list_事件內容.Distinct().ToList();


                Console.WriteLine($"取得事件內容Log 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
            }).Wait();
            this.rJ_ComboBox_Log_事件內容.DataSource = list_事件內容;
        }
        private void SqL_DataGridView_Log_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            if (plC_RJ_ChechBox_Log_發生日期.Checked)
            {
                RowsList = RowsList.GetRowsInDate((int)enum_Log.時間, this.rJ_DatePicker_Log_發生日期_起始, this.rJ_DatePicker_Log_發生日期_結束);
            }
        }
        private void SqL_DataGridView_Log_DataGridRefreshEvent()
        {
     
        }
        private void PlC_RJ_Button_Log_藥品碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string serchValue = this.rJ_TextBox_Log_藥品碼.Texts;
            if (serchValue.StringIsEmpty()) return;
            List<object[]> list_value = this.sqL_DataGridView_Log.SQL_GetAllRows(false);
            list_value = (from value in list_value
                          where value[(int)enum_Log.內容].ObjectToString().GetTextValue("藥品碼").Contains(serchValue)
                          select value).ToList();
            this.sqL_DataGridView_Log.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_Log_藥局代碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string serchValue = this.rJ_ComboBox_Log_藥局代碼.Texts;
            if (serchValue.StringIsEmpty()) return;
            List<object[]> list_value = this.sqL_DataGridView_Log.SQL_GetAllRows(false);
            list_value = (from value in list_value
                          where value[(int)enum_Log.內容].ObjectToString().GetTextValue("藥局代碼") == serchValue
                          select value).ToList();
            this.sqL_DataGridView_Log.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_Log_來源報表_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string serchValue = this.rJ_ComboBox_Log_來源報表.Texts;
            if (serchValue.StringIsEmpty()) return;
            List<object[]> list_value = this.sqL_DataGridView_Log.SQL_GetAllRows(false);
            list_value = (from value in list_value
                          where value[(int)enum_Log.內容].ObjectToString().GetTextValue("來源報表") == serchValue
                          select value).ToList();
            this.sqL_DataGridView_Log.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_Log_事件內容_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string serchValue = this.rJ_ComboBox_Log_事件內容.Texts;
            if (serchValue.StringIsEmpty()) return;
            List<object[]> list_value = this.sqL_DataGridView_Log.SQL_GetAllRows(false);
            list_value = (from value in list_value
                          where value[(int)enum_Log.內容].ObjectToString().GetTextValue("事件內容") == serchValue
                          select value).ToList();
            this.sqL_DataGridView_Log.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_Log_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_Log.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_Log_刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_Log.Get_All_Select_RowsValues();
            this.sqL_DataGridView_Log.SQL_DeleteExtra(list_value, true);
        }
        #endregion

        public class LogComparerby : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime1 = x[(int)enum_Log.時間].ToDateTimeString_6().StringToDateTime();
                DateTime datetime2 = y[(int)enum_Log.時間].ToDateTimeString_6().StringToDateTime();
                
                int compare = DateTime.Compare(datetime1, datetime2);
                return compare;

            }
        }
    }
}
