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
using System.Collections;

namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        private void sub_Program_藥庫_緊急訂單_訂單列表_Init()
        {

            this.sqL_DataGridView_訂單管理_訂單列表.Init(this.sqL_DataGridView_藥品補給系統_訂單資料);
            this.sqL_DataGridView_訂單管理_訂單列表.DataGridRowsChangeRefEvent += SqL_DataGridView_訂單管理_訂單列表_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_訂單管理_訂單列表.DataGridRefreshEvent += SqL_DataGridView_訂單管理_訂單列表_DataGridRefreshEvent;
            this.sqL_DataGridView_訂單管理_訂單列表.RowEnterEvent += SqL_DataGridView_訂單管理_訂單列表_RowEnterEvent;
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(false, new enum_藥品補給系統_訂單資料().GetEnumNames());
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.訂單編號);
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.供應商全名);
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.訂購日期);
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.訂購時間);
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.訂購人);
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.驗收院所別);
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.驗收人);
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.驗收日期);
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.應驗收日期);
            this.sqL_DataGridView_訂單管理_訂單列表.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.備註);

            this.sqL_DataGridView_訂單管理_訂單內容.Init(this.sqL_DataGridView_藥品補給系統_訂單資料);
            this.sqL_DataGridView_訂單管理_訂單內容.DataGridRefreshEvent += SqL_DataGridView_訂單管理_訂單內容_DataGridRefreshEvent;
            this.sqL_DataGridView_訂單管理_訂單內容.Set_ColumnVisible(false, new enum_藥品補給系統_訂單資料().GetEnumNames());
            this.sqL_DataGridView_訂單管理_訂單內容.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.藥品碼);
            this.sqL_DataGridView_訂單管理_訂單內容.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.藥品名稱);
            this.sqL_DataGridView_訂單管理_訂單內容.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.訂購數量);
            this.sqL_DataGridView_訂單管理_訂單內容.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.已入庫數量);
            this.sqL_DataGridView_訂單管理_訂單內容.Set_ColumnVisible(true, enum_藥品補給系統_訂單資料.訂購總價);

            this.sqL_DataGridView_訂單管理_發票內容.Init(this.sqL_DataGridView_藥品補給系統_發票資料);
            this.sqL_DataGridView_訂單管理_發票內容.DataGridRefreshEvent += SqL_DataGridView_訂單管理_發票內容_DataGridRefreshEvent;
            this.sqL_DataGridView_訂單管理_發票內容.Set_ColumnVisible(true, new enum_藥品補給系統_發票資料().GetEnumNames());
            this.sqL_DataGridView_訂單管理_發票內容.Set_ColumnVisible(false, enum_藥品補給系統_發票資料.序號);
            this.sqL_DataGridView_訂單管理_發票內容.Set_ColumnVisible(false, enum_藥品補給系統_發票資料.一般匯出);
            this.sqL_DataGridView_訂單管理_發票內容.Set_ColumnVisible(false, enum_藥品補給系統_發票資料.中榮匯出);
            this.sqL_DataGridView_訂單管理_發票內容.Set_ColumnVisible(false, enum_藥品補給系統_發票資料.賣方統一編號);
            this.sqL_DataGridView_訂單管理_發票內容.Set_ColumnVisible(false, enum_藥品補給系統_發票資料.前次驗收折讓後單價);
            this.sqL_DataGridView_訂單管理_發票內容.Set_ColumnVisible(false, enum_藥品補給系統_發票資料.已結清);
            this.sqL_DataGridView_訂單管理_發票內容.Set_ColumnVisible(false, enum_藥品補給系統_發票資料.訂單編號);

            this.plC_RJ_Button_緊急訂單_訂單管理_顯示全部.MouseDownEvent += PlC_RJ_Button_緊急訂單_訂單管理_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_訂單管理_訂單編號搜尋.MouseDownEvent += PlC_RJ_Button_緊急訂單_訂單管理_訂單編號搜尋_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_訂單管理_藥品碼搜尋.MouseDownEvent += PlC_RJ_Button_緊急訂單_訂單管理_藥品碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_訂單管理_藥品名稱搜尋.MouseDownEvent += PlC_RJ_Button_緊急訂單_訂單管理_藥品名稱搜尋_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_訂單管理_供應商名稱搜尋.MouseDownEvent += PlC_RJ_Button_緊急訂單_訂單管理_供應商名稱搜尋_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_訂單管理_訂購時間搜尋.MouseDownEvent += PlC_RJ_Button_緊急訂單_訂單管理_訂購時間搜尋_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_訂單管理_刪除訂單.MouseDownEvent += PlC_RJ_Button_緊急訂單_訂單管理_刪除訂單_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_訂單管理_修正備註.MouseDownEvent += PlC_RJ_Button_緊急訂單_訂單管理_修正備註_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_訂單管理_修正訂購時間.MouseDownEvent += PlC_RJ_Button_緊急訂單_訂單管理_修正訂購時間_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_緊急訂單_訂單列表);
        }

    

        private bool flag_藥庫_緊急訂單_訂單列表 = false;
        private void sub_Program_藥庫_緊急訂單_訂單列表()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "緊急訂單" && this.plC_ScreenPage_藥庫_緊急訂單.PageText == "訂單列表")
            {
                if (!this.flag_藥庫_緊急訂單_訂單列表)
                {


                    this.flag_藥庫_緊急訂單_訂單列表 = true;
                }

            }
            else
            {
                this.flag_藥庫_緊急訂單_訂單列表 = false;
            }
        }

        #region Function

        #endregion
        #region Event
        private void SqL_DataGridView_訂單管理_訂單列表_DataGridRefreshEvent()
        {
            List<object[]> list_訂單列表 = this.sqL_DataGridView_訂單管理_訂單列表.SQL_GetAllRows(false);
            List<object[]> list_訂單列表_buf = new List<object[]>();
            List<object[]> list_訂單列表_Distinct = list_訂單列表.Distinct(new Distinct_訂單管理_訂單列表()).ToList();
            for(int i = 0; i < list_訂單列表_Distinct.Count; i++)
            {
                string 訂單編號 = list_訂單列表_Distinct[i][(int)enum_藥品補給系統_訂單資料.訂單編號].ObjectToString();
                list_訂單列表_buf = list_訂單列表.GetRows((int)enum_藥品補給系統_訂單資料.訂單編號, 訂單編號);

            }
            for (int i = 0; i < this.sqL_DataGridView_訂單管理_訂單列表.dataGridView.Rows.Count; i++)
            {
                string 訂單編號 = this.sqL_DataGridView_訂單管理_訂單列表.dataGridView.Rows[i].Cells[enum_藥品補給系統_訂單資料.訂單編號.GetEnumName()].Value.ObjectToString();
                list_訂單列表_buf = list_訂單列表.GetRows((int)enum_藥品補給系統_訂單資料.訂單編號, 訂單編號);
                bool flag_check = true;
                if(list_訂單列表_buf.Count > 0)
                {
                    for (int k = 0; k < list_訂單列表_buf.Count; k++)
                    {
                        string 確認驗收 = list_訂單列表_buf[k][(int)enum_藥品補給系統_訂單資料.確認驗收].ObjectToString();
                        if (確認驗收 == false.ToString()) flag_check = false;
                    }
                    if(flag_check)
                    {
                        this.sqL_DataGridView_訂單管理_訂單列表.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                        this.sqL_DataGridView_訂單管理_訂單列表.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }     
            }
        }
        private void SqL_DataGridView_訂單管理_訂單內容_DataGridRefreshEvent()
        {
           
        }
        private void SqL_DataGridView_訂單管理_發票內容_DataGridRefreshEvent()
        {
            List<object[]> list_發票內容 = this.sqL_DataGridView_訂單管理_發票內容.SQL_GetAllRows(false);
            List<object[]> list_發票內容_buf = new List<object[]>();

            for (int i = 0; i < this.sqL_DataGridView_訂單管理_發票內容.dataGridView.Rows.Count; i++)
            {
                string 已結清 = this.sqL_DataGridView_訂單管理_發票內容.dataGridView.Rows[i].Cells[enum_藥品補給系統_發票資料.已結清.GetEnumName()].Value.ObjectToString();
                if (已結清 == true.ToString())
                {
                    this.sqL_DataGridView_訂單管理_發票內容.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_訂單管理_發票內容.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }         
        private void SqL_DataGridView_訂單管理_訂單列表_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList = RowsList.Distinct(new Distinct_訂單管理_訂單列表()).ToList();
            if(radioButton_緊急訂單_訂單管理_驗收條件_已驗收.Checked)
            {
                RowsList = RowsList.GetRows((int)enum_藥品補給系統_訂單資料.確認驗收, true.ToString());
            }
            else if(radioButton_緊急訂單_訂單管理_驗收條件_未驗收.Checked)
            {
                RowsList = RowsList.GetRows((int)enum_藥品補給系統_訂單資料.確認驗收, false.ToString());
            }
            if(radioButton_緊急訂單_訂單管理_類別_一般.Checked)
            {
                RowsList = RowsList.GetRowsByLike((int)enum_藥品補給系統_訂單資料.訂單編號, "OD");
            }
            else if (radioButton_緊急訂單_訂單管理_類別_緊急.Checked)
            {
                RowsList = RowsList.GetRowsByLike((int)enum_藥品補給系統_訂單資料.訂單編號, "EM");
            }
        }
        private void SqL_DataGridView_訂單管理_訂單列表_RowEnterEvent(object[] RowValue)
        {
            string 訂單編號 = RowValue[(int)enum_藥品補給系統_訂單資料.訂單編號].ObjectToString();
            List<object[]> list_訂單列表 = this.sqL_DataGridView_訂單管理_訂單列表.SQL_GetRows(enum_藥品補給系統_訂單資料.訂單編號.GetEnumName(), 訂單編號, false);
            this.sqL_DataGridView_訂單管理_訂單內容.RefreshGrid(list_訂單列表);

            List<object[]> list_發票內容 = this.sqL_DataGridView_訂單管理_發票內容.SQL_GetRows(enum_藥品補給系統_發票資料.訂單編號.GetEnumName(), 訂單編號, false);
            this.sqL_DataGridView_訂單管理_發票內容.RefreshGrid(list_發票內容);
        }
       
        private void PlC_RJ_Button_緊急訂單_訂單管理_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);

            List<object[]> list_value = this.sqL_DataGridView_訂單管理_訂單列表.SQL_GetAllRows(false);
            Console.WriteLine($"取得訂單列表 ,耗時:{myTimer.ToString()} {DateTime.Now.ToDateTimeString()}");
            this.sqL_DataGridView_訂單管理_訂單列表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_緊急訂單_訂單管理_訂購時間搜尋_MouseDownEvent(MouseEventArgs mevent)
        {

            List<object[]> list_value = this.sqL_DataGridView_訂單管理_訂單列表.SQL_GetRowsByBetween((int)enum_藥品補給系統_訂單資料.訂購日期, rJ_DatePicker_緊急訂單_訂單管理_訂購時間起始, rJ_DatePicker_緊急訂單_訂單管理_訂購時間結束, true);
        }
        private void PlC_RJ_Button_緊急訂單_訂單管理_供應商名稱搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_訂單管理_訂單列表.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品補給系統_訂單資料.供應商全名, rJ_TextBox_緊急訂單_訂單管理_供應商名稱.Texts);
            this.sqL_DataGridView_訂單管理_訂單列表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_緊急訂單_訂單管理_藥品名稱搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_訂單管理_訂單列表.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品補給系統_訂單資料.藥品名稱, rJ_TextBox_緊急訂單_訂單管理_藥品名稱.Texts);
            this.sqL_DataGridView_訂單管理_訂單列表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_緊急訂單_訂單管理_藥品碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_訂單管理_訂單列表.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品補給系統_訂單資料.藥品碼, rJ_TextBox_緊急訂單_訂單管理_藥品碼.Texts);
            this.sqL_DataGridView_訂單管理_訂單列表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_緊急訂單_訂單管理_訂單編號搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_訂單管理_訂單列表.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品補給系統_訂單資料.訂單編號, rJ_TextBox_緊急訂單_訂單管理_訂單編號.Texts);
            this.sqL_DataGridView_訂單管理_訂單列表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_緊急訂單_訂單管理_刪除訂單_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_訂單列表 = this.sqL_DataGridView_訂單管理_訂單列表.Get_All_Select_RowsValues();
            if(list_訂單列表.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            string 訂單編號 = list_訂單列表[0][(int)enum_藥品補給系統_訂單資料.訂單編號].ObjectToString();
            if (MyMessageBox.ShowDialog($"是否刪除<{訂單編號}>所有資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;

            this.sqL_DataGridView_訂單管理_訂單列表.SQL_Delete("訂單編號", 訂單編號, false);
            this.sqL_DataGridView_訂單管理_訂單列表.Delete("訂單編號", 訂單編號, true);
            this.sqL_DataGridView_訂單管理_發票內容.SQL_Delete("訂單編號", 訂單編號, false);
            this.sqL_DataGridView_訂單管理_發票內容.Delete("訂單編號", 訂單編號, true);

        }
        private void PlC_RJ_Button_緊急訂單_訂單管理_修正備註_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_訂單列表 = this.sqL_DataGridView_訂單管理_訂單列表.Get_All_Select_RowsValues();
            if (list_訂單列表.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            Dialog_輸入備註 dialog_輸入備註 = new Dialog_輸入備註();
            if (dialog_輸入備註.ShowDialog() != DialogResult.Yes) return;
            string 訂單編號 = list_訂單列表[0][(int)enum_藥品補給系統_訂單資料.訂單編號].ObjectToString();
            string 序號 = list_訂單列表[0][(int)enum_藥品補給系統_訂單資料.序號].ObjectToString();
            string 藥品碼 = list_訂單列表[0][(int)enum_藥品補給系統_訂單資料.藥品碼].ObjectToString();
            list_訂單列表[0][(int)enum_藥品補給系統_訂單資料.備註] = dialog_輸入備註.Value;
            string[] serch_cols = new string[] { "訂單編號", "序號", "藥品碼" };
            string[] serch_values = new string[] { 訂單編號, 序號, 藥品碼 };
            this.sqL_DataGridView_訂單管理_訂單列表.SQL_Replace(serch_cols, serch_values, list_訂單列表[0], false);
            this.sqL_DataGridView_訂單管理_訂單列表.Replace(serch_cols, serch_values, list_訂單列表[0], true);
        }
        private void PlC_RJ_Button_緊急訂單_訂單管理_修正訂購時間_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_訂單列表 = this.sqL_DataGridView_訂單管理_訂單列表.Get_All_Select_RowsValues();
            if (list_訂單列表.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            string 訂單編號 = list_訂單列表[0][(int)enum_藥品補給系統_訂單資料.訂單編號].ObjectToString();
            string 序號 = list_訂單列表[0][(int)enum_藥品補給系統_訂單資料.序號].ObjectToString();
            string 藥品碼 = list_訂單列表[0][(int)enum_藥品補給系統_訂單資料.藥品碼].ObjectToString();
            string[] serch_cols = new string[] { "訂單編號", "序號", "藥品碼" };
            string[] serch_values = new string[] { 訂單編號, 序號, 藥品碼 };
            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 30, 00);
            dateTime = dateTime.AddDays(-1);
            list_訂單列表[0][(int)enum_藥品補給系統_訂單資料.訂購時間] = dateTime.ToDateTimeString();
            this.sqL_DataGridView_訂單管理_訂單列表.SQL_Replace(serch_cols, serch_values, list_訂單列表[0], false);
            this.sqL_DataGridView_訂單管理_訂單列表.Replace(serch_cols, serch_values, list_訂單列表[0], true);
        }
        #endregion
        public class Distinct_訂單管理_訂單列表 : IEqualityComparer<object[]>
        {
            public bool Equals(object[] x, object[] y)
            {
                return x[(int)enum_藥品補給系統_訂單資料.訂單編號].ObjectToString() == y[(int)enum_藥品補給系統_訂單資料.訂單編號].ObjectToString();
            }

            public int GetHashCode(object[] obj)
            {
                return 1;
            }
        }

    }
    
}
