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
namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {

        private void sub_Program_補給驗收_驗收入庫_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_驗收入庫_補給驗收_發票資料, dBConfigClass.DB_Basic);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Init(this.sqL_DataGridView_藥品補給系統_發票資料);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Set_ColumnVisible(false, new enum_藥品補給系統_發票資料().GetEnumNames());
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Set_ColumnVisible(true, enum_藥品補給系統_發票資料.訂單編號);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Set_ColumnVisible(true, enum_藥品補給系統_發票資料.藥品碼);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Set_ColumnVisible(true, enum_藥品補給系統_發票資料.藥品名稱);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Set_ColumnVisible(true, enum_藥品補給系統_發票資料.數量);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Set_ColumnVisible(true, enum_藥品補給系統_發票資料.效期);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Set_ColumnVisible(true, enum_藥品補給系統_發票資料.批號);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Set_ColumnVisible(true, enum_藥品補給系統_發票資料.訂購日期);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Set_ColumnVisible(true, enum_藥品補給系統_發票資料.登錄時間);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.DataGridRowsChangeRefEvent += SqL_DataGridView_驗收入庫_補給驗收_發票資料_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.DataGridRefreshEvent += SqL_DataGridView_驗收入庫_補給驗收_發票資料_DataGridRefreshEvent;

            this.plC_RJ_Button_驗收入庫_補給驗收_全部顯示.MouseDownEvent += PlC_RJ_Button_驗收入庫_補給驗收_全部顯示_MouseDownEvent;
            this.plC_RJ_Button_驗收入庫_補給驗收_選取資料寫入過帳明細.MouseDownEvent += PlC_RJ_Button_驗收入庫_補給驗收_選取資料寫入過帳明細_MouseDownEvent;
            this.plC_RJ_Button_驗收入庫_補給驗收_選取資料設定未匯出.MouseDownEvent += PlC_RJ_Button_驗收入庫_補給驗收_選取資料設定未匯出_MouseDownEvent;
            this.plC_RJ_Button_驗收入庫_補給驗收_顯示未過帳資料.MouseDownEvent += PlC_RJ_Button_驗收入庫_補給驗收_顯示未過帳資料_MouseDownEvent;
            this.plC_RJ_Button_驗收入庫_補給驗收_藥品碼篩選.MouseDownEvent += PlC_RJ_Button_驗收入庫_補給驗收_藥品碼篩選_MouseDownEvent;
            this.plC_RJ_Button_驗收入庫_補給驗收_藥品名稱篩選.MouseDownEvent += PlC_RJ_Button_驗收入庫_補給驗收_藥品名稱篩選_MouseDownEvent;
            this.plC_RJ_Button_驗收入庫_補給驗收_顯示未入帳發票.MouseDownEvent += PlC_RJ_Button_驗收入庫_補給驗收_顯示未入帳發票_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_補給驗收_驗收入庫);
        }

  

        private bool flag_補給驗收_驗收入庫 = false;
        private void sub_Program_補給驗收_驗收入庫()
        {
       
        }
        #region Function
        private void Function_驗收入庫_補給驗收_寫入過帳明細(List<object[]> list_發票資料)
        {
            List<object[]> list_補給驗收入庫_add = new List<object[]>();
            List<string[]> list_發票資料_serchvalues = new List<string[]>();
            List<object[]> list_發票資料_values = new List<object[]>();
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            for (int i = 0; i < list_發票資料.Count; i++)
            {
                string 藥品碼 = list_發票資料[i][(int)enum_藥品補給系統_發票資料.藥品碼].ObjectToString();
                if (list_發票資料[i][(int)enum_藥品補給系統_發票資料.中榮匯出].ObjectToString() == true.ToString())
                {
                    continue;
                }
                
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥庫_藥品資料.藥品碼, 藥品碼);
                if (list_藥品資料_buf.Count == 0)
                {
                    continue;
                }
                object[] value = new object[new enum_補給驗收入庫().GetLength()];
                value[(int)enum_補給驗收入庫.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_補給驗收入庫.藥品碼] = list_發票資料[i][(int)enum_藥品補給系統_發票資料.藥品碼].ObjectToString();
                if(value[(int)enum_補給驗收入庫.藥品碼].ToString().Length == 5)
                {
                    value[(int)enum_補給驗收入庫.藥品碼] = $"A0000{value[(int)enum_補給驗收入庫.藥品碼].ToString()}";
                }
                value[(int)enum_補給驗收入庫.效期] = list_發票資料[i][(int)enum_藥品補給系統_發票資料.效期].ToDateString();
                value[(int)enum_補給驗收入庫.批號] = list_發票資料[i][(int)enum_藥品補給系統_發票資料.批號].ObjectToString();
                value[(int)enum_補給驗收入庫.數量] = list_發票資料[i][(int)enum_藥品補給系統_發票資料.數量].ObjectToString();
                string 驗收時間 = list_發票資料[i][(int)enum_藥品補給系統_發票資料.登錄時間].ObjectToString();
                if (驗收時間.StringIsEmpty()) 驗收時間 = list_發票資料[i][(int)enum_藥品補給系統_發票資料.登錄時間].ToDateTimeString();
                value[(int)enum_補給驗收入庫.驗收時間] = 驗收時間;
                value[(int)enum_補給驗收入庫.加入時間] = DateTime.Now.ToDateTimeString_6();
                value[(int)enum_補給驗收入庫.狀態] = enum_驗收入庫明細_狀態.等待過帳.GetEnumName();
                value[(int)enum_補給驗收入庫.來源] = "補給驗收";
                value[(int)enum_補給驗收入庫.備註] = list_發票資料[i][(int)enum_藥品補給系統_發票資料.訂單編號].ObjectToString();
                DateTime dateTime = DateTime.Now;
                if (!DateTime.TryParse(驗收時間, out dateTime))
                {
                    continue;
                }
                if (!value[(int)enum_補給驗收入庫.效期].ObjectToString().Check_Date_String())
                {
                    continue;
                }
                list_補給驗收入庫_add.Add(value);

                list_發票資料[i][(int)enum_藥品補給系統_發票資料.中榮匯出] = true.ToString();
                string[] serchvalues = new string[] { list_發票資料[i][(int)enum_藥品補給系統_發票資料.訂單編號].ObjectToString(), list_發票資料[i][(int)enum_藥品補給系統_發票資料.藥品碼].ObjectToString() };
                list_發票資料_values.Add(list_發票資料[i]);
                list_發票資料_serchvalues.Add(serchvalues);
            }

            string[] colnames = new string[] { enum_藥品補給系統_發票資料.訂單編號.GetEnumName(), enum_藥品補給系統_發票資料.藥品碼.GetEnumName() };
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.SQL_ReplaceExtra(colnames, list_發票資料_serchvalues, list_發票資料_values, false);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.ReplaceExtra(colnames, list_發票資料_serchvalues, list_發票資料_values, true);
            this.sqL_DataGridView_補給驗收入庫.SQL_AddRows(list_補給驗收入庫_add, false);
        }
        #endregion
        #region Event
        private void SqL_DataGridView_驗收入庫_補給驗收_發票資料_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.dataGridView.Rows[i].Cells[enum_藥品補給系統_發票資料.中榮匯出.GetEnumName()].Value.ToString();
                if (狀態 == true.ToString())
                {
                    this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }

            }
        }
        private void SqL_DataGridView_驗收入庫_補給驗收_發票資料_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
           // RowsList = RowsList.GetRows((int)enum_藥品補給系統_發票資料.已結清, true.ToString());
            RowsList.Sort(new ICP_驗收入庫_補給驗收());
        }
        private void PlC_RJ_Button_驗收入庫_補給驗收_選取資料設定未匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_發票資料 = this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Get_All_Checked_RowsValues();
            List<string[]> list_發票資料_serchvalues = new List<string[]>();
            List<object[]> list_發票資料_values = new List<object[]>();
            if (list_發票資料.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            for (int i = 0; i < list_發票資料.Count; i++)
            {
                list_發票資料[i][(int)enum_藥品補給系統_發票資料.中榮匯出] = false.ToString();
                string[] serchvalues = new string[] { list_發票資料[i][(int)enum_藥品補給系統_發票資料.訂單編號].ObjectToString(), list_發票資料[i][(int)enum_藥品補給系統_發票資料.藥品碼].ObjectToString() };
                list_發票資料_values.Add(list_發票資料[i]);
                list_發票資料_serchvalues.Add(serchvalues);
            }
            string[] colnames = new string[] { enum_藥品補給系統_發票資料.訂單編號.GetEnumName(), enum_藥品補給系統_發票資料.藥品碼.GetEnumName() };
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.SQL_ReplaceExtra(colnames, list_發票資料_serchvalues, list_發票資料_values, false);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.ReplaceExtra(colnames, list_發票資料_serchvalues, list_發票資料_values, true);
            MyMessageBox.ShowDialog("設定完成!");
        }
        private void PlC_RJ_Button_驗收入庫_補給驗收_選取資料寫入過帳明細_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_發票資料 = this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.Get_All_Checked_RowsValues();       
     
            if (list_發票資料.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            this.Function_驗收入庫_補給驗收_寫入過帳明細(list_發票資料);
            MyMessageBox.ShowDialog("寫入完成!");
        }
        private void PlC_RJ_Button_驗收入庫_補給驗收_全部顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_驗收入庫_補給驗收_顯示未過帳資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.SQL_GetAllRows(false);
            list_value = (from value in list_value
                          where value[(int)enum_藥品補給系統_發票資料.中榮匯出].ObjectToString() != true.ToString()
                          select value).ToList();
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.RefreshGrid(list_value);

        }
        private void PlC_RJ_Button_驗收入庫_補給驗收_藥品名稱篩選_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.GetAllRows();
            list_value = list_value.GetRowsByLike((int)enum_藥品補給系統_發票資料.藥品名稱, this.rJ_TextBox_驗收入庫_補給驗收_藥品名稱篩選.Text);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_驗收入庫_補給驗收_藥品碼篩選_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.GetAllRows();
            list_value = list_value.GetRowsByLike((int)enum_藥品補給系統_發票資料.藥品碼, this.rJ_TextBox_驗收入庫_補給驗收_藥品碼篩選.Text);
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_驗收入庫_補給驗收_顯示未入帳發票_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_發票資料 = this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.SQL_GetAllRows(false);
            List<object[]> list_發票資料_buf = new List<object[]>();
            List<object[]> list_補給驗收資料 = this.sqL_DataGridView_補給驗收入庫.SQL_GetAllRows(false);
            List<object[]> list_補給驗收資料_buf = new List<object[]>();
            for (int i = 0; i < list_發票資料.Count; i++)
            {
                string 訂單編號 = list_發票資料[i][(int)enum_藥品補給系統_發票資料.訂單編號].ObjectToString();
                string 藥品碼 = list_發票資料[i][(int)enum_藥品補給系統_發票資料.藥品碼].ObjectToString();
   
                藥品碼 = $"A0000{藥品碼}";
                list_補給驗收資料_buf = list_補給驗收資料.GetRows((int)enum_補給驗收入庫.備註, 訂單編號);
                list_補給驗收資料_buf = list_補給驗收資料_buf.GetRows((int)enum_補給驗收入庫.藥品碼, 藥品碼);
                if(list_補給驗收資料_buf.Count == 0)
                {
                    list_發票資料_buf.Add(list_發票資料[i]);
                }
            }
            this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.RefreshGrid(list_發票資料_buf);
        }
        #endregion
        private class ICP_驗收入庫_補給驗收 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                DateTime temp0 = x[(int)enum_藥品補給系統_發票資料.登錄時間].ToDateString().StringToDateTime();
                DateTime temp1 = y[(int)enum_藥品補給系統_發票資料.登錄時間].ToDateString().StringToDateTime();
                int cmp = temp0.CompareTo(temp1);
                return cmp;
            }
        }
    }
}
