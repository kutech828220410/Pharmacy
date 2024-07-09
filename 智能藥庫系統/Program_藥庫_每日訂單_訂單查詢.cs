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
    public partial class Main_Form : Form
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
        public enum enum_藥庫_每日訂單_訂單查詢_匯出 : int
        {
            藥品碼,
            中文名稱,
            藥品名稱,
            包裝單位,
            今日訂購數量,
            緊急訂購數量,
            採購單價,
            採購總價,
            訂購時間,
        }
        private void sub_Program_藥庫_每日訂單_訂單查詢_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_每日訂單, dBConfigClass.DB_posting_server);
            this.sqL_DataGridView_每日訂單.Init();
            if (!this.sqL_DataGridView_每日訂單.SQL_IsTableCreat()) this.sqL_DataGridView_每日訂單.SQL_CreateTable();


            this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.Init();
            this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.DataGridRowsChangeRefEvent += SqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料_DataGridRowsChangeRefEvent;

            this.plC_RJ_Button_藥庫_每日訂單_訂購資料_匯出.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_訂購資料_匯出_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_訂購資料_刪除.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_訂購資料_刪除_MouseDownEvent;
            this.plC_RJ_Button_藥庫_訂單查詢_搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_訂單查詢_搜尋_MouseDownEvent;
            this.comboBox_藥庫_訂單查詢_搜尋條件.SelectedIndex = 0;
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
        public List<object[]> Function_藥庫_每日訂單_訂單查詢_取得訂單資料(DateTime dateTime_st , DateTime dateTime_end)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            Console.WriteLine($"Function_藥庫_每日訂單_訂單查詢_取得訂單資料 : {dateTime_st.ToDateTimeString()}-{dateTime_end.ToDateTimeString()}");

            List<object[]> list_value = new List<object[]>();
            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            Console.WriteLine($"取得藥品資料,耗時{myTimer.ToString()}");
            List<object[]> list_訂單資料 = this.sqL_DataGridView_每日訂單.SQL_GetRowsByBetween((int)enum_每日訂單.訂購時間, dateTime_st, dateTime_end, false);
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

                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_medDrugstore.藥品碼, 藥品碼);
                if (list_藥品資料_buf.Count > 0)
                {
                    藥品名稱 = list_藥品資料_buf[0][(int)enum_medDrugstore.藥品名稱].ObjectToString();
                    中文名稱 = list_藥品資料_buf[0][(int)enum_medDrugstore.中文名稱].ObjectToString();
                    包裝單位 = list_藥品資料_buf[0][(int)enum_medDrugstore.包裝單位].ObjectToString();
                }
                list_value[i][(int)enum_藥庫_每日訂單_訂單查詢.藥品名稱] = 藥品名稱;
                list_value[i][(int)enum_藥庫_每日訂單_訂單查詢.中文名稱] = 中文名稱;
                list_value[i][(int)enum_藥庫_每日訂單_訂單查詢.包裝單位] = 包裝單位;
                list_value_buf.Add(list_value[i]);
            }

            return list_value_buf;
        }
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

                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_medDrugstore.藥品碼, 藥品碼);
                if(list_藥品資料_buf.Count > 0)
                {
                    藥品名稱 = list_藥品資料_buf[0][(int)enum_medDrugstore.藥品名稱].ObjectToString();
                    中文名稱 = list_藥品資料_buf[0][(int)enum_medDrugstore.中文名稱].ObjectToString();
                    包裝單位 = list_藥品資料_buf[0][(int)enum_medDrugstore.包裝單位].ObjectToString();
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
        private void PlC_RJ_Button_藥庫_訂單查詢_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            LoadingForm.ShowLoadingForm();
            try
            {
                string text = "";
                this.Invoke(new Action(delegate
                {
                    text = this.comboBox_藥庫_訂單查詢_搜尋條件.Text;
                }));
                string value = this.rJ_TextBox_藥庫_訂單查詢_搜尋條件.Texts;

                int hour = 11;
                int min = 50;

                
                DateTime dateNow = rJ_DatePicker_藥庫_每日訂單_請購時間起始.Value;
                dateNow = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 11, 40, DateTime.Now.Second);
                DateTime dateTime_temp = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, hour, min, 00);
                dateTime_temp = dateTime_temp.AddMinutes(5);


                DateTime dateTime_start;
                DateTime dateTime_end;

                DateTime dateTime_basic_start = dateNow;
                DateTime dateTime_basic_end = dateNow.AddDays(1);
                bool isholiday = false;
                if (!dateTime_basic_start.IsNewDay(dateTime_temp.Hour, dateTime_temp.Minute))
                {
                    if (!Basic.TypeConvert.IsHspitalHolidays(dateTime_basic_start))
                    {
                        if (Basic.TypeConvert.IsHspitalHolidays(dateTime_basic_start.AddDays(-1)))
                        {
                            dateTime_basic_start = dateTime_basic_start.AddDays(-1);
                        }
                    }

                }
                while (true)
                {
                    if (!Basic.TypeConvert.IsHspitalHolidays(dateTime_basic_start))
                    {
                        break;
                    }
                    dateTime_basic_start = dateTime_basic_start.AddDays(-1);
                    isholiday = true;
                }

                if (dateTime_basic_start.IsNewDay(dateTime_temp.Hour, dateTime_temp.Minute) || isholiday)
                {
                    dateTime_start = $"{dateTime_basic_start.ToDateString()} {hour}:{min}:00".StringToDateTime();
                    dateTime_end = $"{dateTime_basic_start.AddDays(1).ToDateString()} {hour}:{min}:00".StringToDateTime();
                }
                else
                {
                    dateTime_end = $"{dateTime_basic_start.ToDateString()} {hour}:{min}:00".StringToDateTime();
                    dateTime_start = dateTime_end.AddDays(-1);
                }
                while (true)
                {
                    if (!Basic.TypeConvert.IsHspitalHolidays(dateTime_end))
                    {
                        break;
                    }
                    dateTime_end = dateTime_end.AddDays(1);
                }

                List<object[]> list_value = this.Function_藥庫_每日訂單_訂單查詢_取得訂單資料(dateTime_start, dateTime_end);
                if (text == "全部顯示")
                {

                }
                if (text == "藥碼")
                {
                    if (rJ_RatioButton_藥庫_訂單查詢_模糊.Checked)
                    {
                        list_value = list_value.GetRowsByLike((int)enum_藥庫_每日訂單_訂單查詢.藥品碼, value);
                    }
                    if (rJ_RatioButton_藥庫_訂單查詢_前綴.Checked)
                    {
                        list_value = list_value.GetRowsStartWithByLike((int)enum_藥庫_每日訂單_訂單查詢.藥品碼, value);
                    }
                }
                if (text == "藥名")
                {
                    if (rJ_RatioButton_藥庫_訂單查詢_模糊.Checked)
                    {
                        list_value = list_value.GetRowsByLike((int)enum_藥庫_每日訂單_訂單查詢.藥品名稱, value);
                    }
                    if (rJ_RatioButton_藥庫_訂單查詢_前綴.Checked)
                    {
                        list_value = list_value.GetRowsStartWithByLike((int)enum_藥庫_每日訂單_訂單查詢.藥品名稱, value);
                    }
                }
                if (text == "中文名")
                {
                    if (rJ_RatioButton_藥庫_訂單查詢_模糊.Checked)
                    {
                        list_value = list_value.GetRowsByLike((int)enum_藥庫_每日訂單_訂單查詢.中文名稱, value);
                    }
                    if (rJ_RatioButton_藥庫_訂單查詢_前綴.Checked)
                    {
                        list_value = list_value.GetRowsStartWithByLike((int)enum_藥庫_每日訂單_訂單查詢.中文名稱, value);
                    }
                }
                if (list_value.Count == 0)
                {
                    MyMessageBox.ShowDialog("查無資料");
                    return;
                }

                this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.RefreshGrid(list_value);
            }
            catch
            {

            }
            finally
            {
                LoadingForm.CloseLoadingForm();
            }
          
        }
        private void PlC_RJ_Button_藥庫_每日訂單_訂購資料_刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            if (MyMessageBox.ShowDialog($"是否刪除選取<{list_value.Count}>筆資料", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            List<object[]> list_訂單資料 = list_value.CopyRows(new enum_藥庫_每日訂單_訂單查詢(), new enum_每日訂單());

            this.sqL_DataGridView_每日訂單.SQL_DeleteExtra(list_訂單資料, false);
            this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.DeleteExtra(list_value, true);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_訂購資料_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
           

            this.Invoke(new Action(delegate
            {
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;


                    string MedPrice = Basic.Net.WEBApiGet($"{dBConfigClass.MedPrice_ApiURL}");
                    List<class_MedPrice> class_MedPrices = MedPrice.JsonDeserializet<List<class_MedPrice>>();
                    List<class_MedPrice> class_MedPrices_buf = new List<class_MedPrice>();


                    List<object[]> list_訂單資料_src = this.sqL_DataGridView_藥庫_每日訂單_訂單查詢_訂單資料.GetAllRows();
                    List<object[]> list_訂單資料_out = list_訂單資料_src.CopyRows(new enum_藥庫_每日訂單_訂單查詢(), new enum_藥庫_每日訂單_訂單查詢_匯出());


                    for (int i = 0; i < list_訂單資料_out.Count; i++)
                    {
                        string 藥品碼 = list_訂單資料_out[i][(int)enum_藥庫_每日訂單_訂單查詢_匯出.藥品碼].ObjectToString();
                        class_MedPrices_buf = (from value in class_MedPrices
                                               where value.藥品碼 == 藥品碼
                                               select value).ToList();
                        if (class_MedPrices_buf.Count > 0)
                        {
                            int 數量 = 0;
                            int 今日訂購數量 = list_訂單資料_out[i][(int)enum_藥庫_每日訂單_訂單查詢_匯出.今日訂購數量].ObjectToString().StringToInt32();
                            int 緊急訂購數量 = list_訂單資料_out[i][(int)enum_藥庫_每日訂單_訂單查詢_匯出.緊急訂購數量].ObjectToString().StringToInt32();
                            if (今日訂購數量 > 0) 數量 += 今日訂購數量;
                            if (緊急訂購數量 > 0) 數量 += 緊急訂購數量;
                            double 訂購單價 = class_MedPrices_buf[0].售價.StringToDouble();
                            double 訂購總價 = 訂購單價 * 數量;
                            if (訂購單價 > 0)
                            {
                                list_訂單資料_out[i][(int)enum_藥庫_每日訂單_訂單查詢_匯出.採購單價] = 訂購單價.ToString("0.000").StringToDouble();
                                list_訂單資料_out[i][(int)enum_藥庫_每日訂單_訂單查詢_匯出.採購總價] = 訂購總價.ToString("0.000").StringToDouble();
                            }
                        }
                    }
                    DataTable dataTable = list_訂單資料_out.ToDataTable(new enum_藥庫_每日訂單_訂單查詢_匯出());
                    dataTable = dataTable.ReorderTable(new enum_藥庫_每日訂單_訂單查詢_匯出());

                    string Extension = System.IO.Path.GetExtension(this.saveFileDialog_SaveExcel.FileName);
                    if (Extension == ".txt")
                    {
                        CSVHelper.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                    }
                    else if (Extension == ".xls" || Extension == ".xlsx")
                    {
                        MyOffice.ExcelClass.NPOI_SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName, (int)enum_藥庫_每日訂單_訂單查詢_匯出.今日訂購數量
                            , (int)enum_藥庫_每日訂單_訂單查詢_匯出.採購單價, (int)enum_藥庫_每日訂單_訂單查詢_匯出.採購總價, (int)enum_藥庫_每日訂單_訂單查詢_匯出.緊急訂購數量);
                    }

                    this.Cursor = Cursors.Default;
                    MyMessageBox.ShowDialog("匯出完成");
                }
            }));
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
