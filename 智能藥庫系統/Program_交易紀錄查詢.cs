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
    enum enum_庫別
    {
        藥庫,
        屏榮藥局,
    }

    public partial class Main_Form : Form
    {
        public enum enum_待調整匯出
        {
            藥碼,
            藥名,
            藥局庫存,
            盤點量,
            調整量,
            
        }
        public enum enum_藥局盤點量匯入
        {
            藥碼,
            盤點量,
        }


        private void sub_Program_交易紀錄查詢_Init()
        {
            string url = $"{API_Server}/api/transactions/init";
            returnData returnData = new returnData();
            returnData.ServerType = enum_ServerSetting_Type.藥庫.GetEnumName();
            returnData.ServerName = $"{"DS01"}";
            string json_in = returnData.JsonSerializationt();
            string json = Basic.Net.WEBApiPostJson($"{url}", json_in);
            Table table = json.JsonDeserializet<Table>();
            if (table == null)
            {
                MyMessageBox.ShowDialog($"交易紀錄表單建立失敗!! API_Server:{API_Server}");
                return;
            }
            this.sqL_DataGridView_交易記錄查詢.Init(table);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnVisible(false, new enum_交易記錄查詢資料().GetEnumNames());

            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleCenter, enum_交易記錄查詢資料.動作);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleCenter, enum_交易記錄查詢資料.庫別);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_交易記錄查詢資料.藥品碼);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(250, DataGridViewContentAlignment.MiddleLeft, enum_交易記錄查詢資料.藥品名稱);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(70, DataGridViewContentAlignment.MiddleRight, enum_交易記錄查詢資料.庫存量);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(70, DataGridViewContentAlignment.MiddleRight, enum_交易記錄查詢資料.交易量);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(70, DataGridViewContentAlignment.MiddleRight, enum_交易記錄查詢資料.結存量);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleCenter, enum_交易記錄查詢資料.操作人);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(180, DataGridViewContentAlignment.MiddleCenter, enum_交易記錄查詢資料.操作時間);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnWidth(400, DataGridViewContentAlignment.MiddleLeft, enum_交易記錄查詢資料.備註);

            this.sqL_DataGridView_交易記錄查詢.Set_ColumnSortMode(DataGridViewColumnSortMode.Automatic, enum_交易記錄查詢資料.藥品碼);
            this.sqL_DataGridView_交易記錄查詢.Set_ColumnSortMode(DataGridViewColumnSortMode.Automatic, enum_交易記錄查詢資料.交易量);

            this.sqL_DataGridView_交易記錄查詢.DataGridRefreshEvent += SqL_DataGridView_交易記錄查詢_DataGridRefreshEvent;
            this.sqL_DataGridView_交易記錄查詢.DataGridRowsChangeRefEvent += SqL_DataGridView_交易記錄查詢_DataGridRowsChangeRefEvent;

            this.plC_RJ_Button_交易紀錄查詢_全部顯示.MouseDownEvent += PlC_RJ_Button_交易紀錄查詢_全部顯示_MouseDownEvent;
            this.plC_RJ_Button_交易紀錄查詢_刪除選取資料.MouseDownEvent += PlC_RJ_Button_交易紀錄查詢_刪除選取資料_MouseDownEvent;
            this.plC_RJ_Button_交易紀錄查詢_測試.MouseDownEvent += PlC_RJ_Button_交易紀錄查詢_測試_MouseDownEvent;


            this.plC_UI_Init.Add_Method(this.sub_Program_交易紀錄查詢);
        }

     

        private bool flag_交易紀錄查詢_頁面更新 = false;
        private bool flag_交易紀錄查詢_頁面更新_init = false;
        private void sub_Program_交易紀錄查詢()
        {
            if (this.plC_ScreenPage_Main.PageText == "交易紀錄查詢")
            {
                if (!this.flag_交易紀錄查詢_頁面更新)
                {
                    this.Invoke(new Action(delegate
                    {
                        if(flag_交易紀錄查詢_頁面更新_init == false)
                        {
                            this.dateTimePicker_交易記錄查詢_操作時間_起始.Value = DateTime.Now.AddMonths(-1);
                            this.dateTimePicker_交易記錄查詢_操作時間_結束.Value = DateTime.Now;
                        }
         

                    }));
                    flag_交易紀錄查詢_頁面更新_init = true;
                    this.flag_交易紀錄查詢_頁面更新 = true;
                }
            }
            else
            {
                this.flag_交易紀錄查詢_頁面更新 = false;
            }
        }

        #region Function
        private void Funnction_交易記錄查詢_取得指定藥碼批號期效期(string 藥碼, ref List<string> list_效期, ref List<string> list_批號)
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            List<string> list_效期_buf = new List<string>();
            List<string> list_操作時間 = new List<string>();
            if (藥碼.StringIsEmpty()) return;
            List<object[]> list_value = this.sqL_DataGridView_交易記錄查詢.SQL_GetRows((int)enum_交易記錄查詢資料.藥品碼, 藥碼, false);
            Console.WriteLine($"搜尋藥碼: {藥碼} , {myTimerBasic.ToString()}");
            string 備註 = "";
            string 操作時間 = "";
            for (int i = 0; i < list_value.Count; i++)
            {
                備註 = list_value[i][(int)enum_交易記錄查詢資料.備註].ObjectToString();
                string[] temp_ary = 備註.Split('\n');
                for (int k = 0; k < temp_ary.Length; k++)
                {
                    string 效期 = temp_ary[k].GetTextValue("效期");
                    string 批號 = temp_ary[k].GetTextValue("批號");
                    操作時間 = list_value[i][(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString();
                    if (效期.StringIsEmpty() == true) continue;
                    list_效期_buf = (from temp in list_效期
                                   where temp == 效期
                                   select temp).ToList();
                    if (list_效期_buf.Count > 0) continue;
                    list_效期.Add(效期);
                    list_批號.Add(批號);
                    list_操作時間.Add(操作時間);
                }
            }
            // 組合效期、批號和操作時間
            List<Tuple<string, string, DateTime>> combinedList = new List<Tuple<string, string, DateTime>>();
            for (int i = 0; i < list_效期.Count; i++)
            {
                combinedList.Add(new Tuple<string, string, DateTime>(list_效期[i], list_批號[i], DateTime.Parse(list_操作時間[i])));
            }

            // 根據操作時間排序
            combinedList.Sort((x, y) => DateTime.Compare(y.Item3, x.Item3));

            // 更新list_效期、list_批號和list_操作時間
            list_效期.Clear();
            list_批號.Clear();
            list_操作時間.Clear();
            foreach (var item in combinedList)
            {
                list_效期.Add(item.Item1);
                list_批號.Add(item.Item2);
                list_操作時間.Add(item.Item3.ToString());
            }
        }
        private void Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作 enum_交易記錄查詢動作, string 操作人, string 備註)
        {
            if (操作人.StringIsEmpty()) return;
            string GUID = Guid.NewGuid().ToString();
            string 動作 = enum_交易記錄查詢動作.GetEnumName();
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 庫存量 = "";
            string 交易量 = "";
            string 結存量 = "";

            string 操作時間 = DateTime.Now.ToDateTimeString_6();
            string 開方時間 = DateTime.Now.ToDateTimeString_6();
            object[] value = new object[new enum_交易記錄查詢資料().GetLength()];
            value[(int)enum_交易記錄查詢資料.GUID] = GUID;
            value[(int)enum_交易記錄查詢資料.動作] = 動作;
            value[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
            value[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
            value[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
            value[(int)enum_交易記錄查詢資料.交易量] = 交易量;
            value[(int)enum_交易記錄查詢資料.結存量] = 結存量;
            value[(int)enum_交易記錄查詢資料.操作人] = 操作人;
            value[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
            value[(int)enum_交易記錄查詢資料.備註] = 備註;

            this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value, false);
        }
        #endregion

        #region Event
        private void SqL_DataGridView_交易記錄查詢_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_交易記錄查詢());
        }
        private void SqL_DataGridView_交易記錄查詢_DataGridRefreshEvent()
        {
            string 動作 = "";
            for (int i = 0; i < this.sqL_DataGridView_交易記錄查詢.dataGridView.Rows.Count; i++)
            {
                動作 = this.sqL_DataGridView_交易記錄查詢.dataGridView.Rows[i].Cells[(int)enum_交易記錄查詢資料.動作].Value.ToString();
                if (動作 == enum_交易記錄查詢動作.人臉識別登入.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.一維碼登入.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.密碼登入.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.登出.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.新增效期.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.修正庫存.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.修正批號.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.操作工程模式.GetEnumName()
                    )
                {
                    this.sqL_DataGridView_交易記錄查詢.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Pink;
                    this.sqL_DataGridView_交易記錄查詢.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void PlC_RJ_Button_交易紀錄查詢_全部顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            bool flag_限制兩個月搜尋條件 = true;
            MyTimerBasic myTimerBasic = new MyTimerBasic(500000);
            if (textBox_交易記錄查詢_藥品碼.Text.StringIsEmpty() == false) flag_限制兩個月搜尋條件 = false;
            if (textBox_交易記錄查詢_藥品名稱.Text.StringIsEmpty() == false) flag_限制兩個月搜尋條件 = false;


            DateTime start = dateTimePicker_交易記錄查詢_操作時間_起始.Value;

            DateTime end = dateTimePicker_交易記錄查詢_操作時間_結束.Value;

            TimeSpan ts = end.Subtract(start); //兩時間天數相減

            double dayCount = ts.Days; //相距天數

            if (dayCount > 60 && flag_限制兩個月搜尋條件)
            {
                MyMessageBox.ShowDialog("搜尋時間範圍大於兩個月,請縮短搜尋範圍!");
                return;
            }

            List<object[]> list_value = new List<object[]>();

            if (textBox_交易記錄查詢_藥品碼.Text.StringIsEmpty() == true)
            {
                list_value = this.sqL_DataGridView_交易記錄查詢.SQL_GetRowsByBetween((int)enum_交易記錄查詢資料.操作時間, dateTimePicker_交易記錄查詢_操作時間_起始, dateTimePicker_交易記錄查詢_操作時間_結束, false);
            }
            else
            {
                list_value = this.sqL_DataGridView_交易記錄查詢.SQL_GetRows((int)enum_交易記錄查詢資料.藥品碼, textBox_交易記錄查詢_藥品碼.Text,false);
                list_value = list_value.GetRowsInDate((int)enum_交易記錄查詢資料.操作時間, dateTimePicker_交易記錄查詢_操作時間_起始, dateTimePicker_交易記錄查詢_操作時間_結束);
            }
            Console.WriteLine($"完成搜尋 , {myTimerBasic.ToString()}");
            myTimerBasic.TickStop();
            myTimerBasic.StartTickTime(50000);

            List<List<object[]>> list_list_value_buf = new List<List<object[]>>();
            List<object[]> list_value_buf = new List<object[]>();

            if(rJ_RatioButton_交易記錄查詢_庫別_藥局.Checked)
            {
                list_value = list_value.GetRowsByLike((int)enum_交易記錄查詢資料.庫別, "藥局");
            }
            if (rJ_RatioButton_交易記錄查詢_庫別_藥庫.Checked)
            {
                list_value = list_value.GetRows((int)enum_交易記錄查詢資料.庫別, enum_庫別.藥庫.GetEnumName());
            }
            if(plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_盤存盈虧.Checked)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.盤存盈虧.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.盤點調整.GetEnumName()));

            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_批次過帳.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.批次過帳.GetEnumName()));
            }   
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_入庫作業.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.入庫作業.GetEnumName()));
            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_自動撥補.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.自動撥補.GetEnumName()));
            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_緊急申領.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.緊急申領.GetEnumName()));
            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_驗收入庫.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.驗收入庫.GetEnumName()));
            }

            
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_效期庫存異動.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.新增效期.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.修正批號.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.修正庫存.GetEnumName()));

            }
   
            if (lC_RJ_ChechBox_交易紀錄查詢_搜尋條件_登入及登出.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.人臉識別登入.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.RFID登入.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.一維碼登入.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.密碼登入.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.登出.GetEnumName()));
            }
            for (int i = 0; i < list_list_value_buf.Count; i++)
            {
                foreach (object[] value in list_list_value_buf[i])
                {
                    list_value_buf.Add(value);
                }
            }
            if (textBox_交易記錄查詢_藥品碼.Text.StringIsEmpty() == false) list_value_buf = list_value_buf.GetRowsByLike((int)enum_交易記錄查詢資料.藥品碼, textBox_交易記錄查詢_藥品碼.Text);
            if (textBox_交易記錄查詢_藥品名稱.Text.StringIsEmpty() == false) list_value_buf = list_value_buf.GetRowsByLike((int)enum_交易記錄查詢資料.藥品名稱, textBox_交易記錄查詢_藥品名稱.Text);
            if (textBox_交易記錄查詢_操作人.Text.StringIsEmpty() == false) list_value_buf = list_value_buf.GetRows((int)enum_交易記錄查詢資料.操作人, textBox_交易記錄查詢_操作人.Text);

            Console.WriteLine($"完成分類 , {myTimerBasic.ToString()}");
            myTimerBasic.TickStop();
            myTimerBasic.StartTickTime(50000);
            this.sqL_DataGridView_交易記錄查詢.RefreshGrid(list_value_buf);
            Console.WriteLine($"完成顯示 , {myTimerBasic.ToString()}");
            if (list_value_buf.Count == 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
            }
        }
        private void PlC_RJ_Button_交易紀錄查詢_刪除選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_交易記錄查詢.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            if (MyMessageBox.ShowDialog("是否刪除選取資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            this.sqL_DataGridView_交易記錄查詢.SQL_DeleteExtra(list_value, false);
            this.sqL_DataGridView_交易記錄查詢.DeleteExtra(list_value, true);
            MyMessageBox.ShowDialog("刪除完成!");
        }
        private void PlC_RJ_Button_交易紀錄查詢_測試_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_匯出資料 = new List<object[]>();
            List<object[]> list_匯出資料_buf = new List<object[]>();
            List<object[]> list_藥庫盤點量匯入 = new List<object[]>();
            List<object[]> list_藥庫盤點量匯入_buf = new List<object[]>();
            this.Invoke(new Action(delegate
            {
                if (this.openFileDialog_LoadExcel.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = MyOffice.ExcelClass.LoadFile(this.openFileDialog_LoadExcel.FileName);
                    if (dataTable == null)
                    {
                        MyMessageBox.ShowDialog("讀取失敗!");
                        return;
                    }
                    dataTable = dataTable.ReorderTable(new enum_藥局盤點量匯入());
                    if (dataTable == null)
                    {
                        MyMessageBox.ShowDialog("讀取失敗!");
                        return;
                    }
                    list_藥庫盤點量匯入 = dataTable.DataTableToRowList();
                }
            }));

            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥局_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            this.sqL_DataGridView_藥局_藥品資料.RowsChangeFunction(list_藥品資料);
           
            for(int i = 0; i < list_藥庫盤點量匯入.Count; i++)
            {
                if (list_藥庫盤點量匯入[i][(int)enum_藥局盤點量匯入.盤點量].StringToInt32() != 0) continue;
                string 藥碼 = list_藥庫盤點量匯入[i][(int)enum_藥局盤點量匯入.藥碼].ObjectToString();
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥局_藥品資料.藥品碼, 藥碼);
                string 藥名 = list_藥品資料_buf[0][(int)enum_藥局_藥品資料.藥品名稱].ObjectToString();
                int 藥局庫存 = list_藥品資料_buf[0][(int)enum_藥局_藥品資料.藥局庫存].StringToInt32();
                object[] value = new object[new enum_待調整匯出().GetLength()];
                value[(int)enum_待調整匯出.藥碼] = 藥碼;
                value[(int)enum_待調整匯出.藥名] = 藥名;
                value[(int)enum_待調整匯出.盤點量] = list_藥庫盤點量匯入[i][(int)enum_藥局盤點量匯入.盤點量];
                value[(int)enum_待調整匯出.調整量] = -藥局庫存;
                value[(int)enum_待調整匯出.藥局庫存] = 藥局庫存;
                list_匯出資料.Add(value);
            }

            this.Invoke(new Action(delegate
            {
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = list_匯出資料.ToDataTable(new enum_待調整匯出());
                    dataTable = dataTable.ReorderTable(new enum_待調整匯出());

                    string Extension = System.IO.Path.GetExtension(this.saveFileDialog_SaveExcel.FileName);
                    if (Extension == ".txt")
                    {
                        CSVHelper.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                    }
                    else if (Extension == ".xls" || Extension == ".xlsx")
                    {
                        MyOffice.ExcelClass.NPOI_SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                    }

                    MyMessageBox.ShowDialog("匯出完成");

                }

            }));

            //this.sqL_DataGridView_交易記錄查詢.RefreshGrid(list_value_buf);
            MyMessageBox.ShowDialog("完成");
        }
        #endregion

        public class ICP_交易記錄查詢 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime1 = x[(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString_6().StringToDateTime();
                DateTime datetime2 = y[(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString_6().StringToDateTime();
                int compare = DateTime.Compare(datetime1, datetime2);
                if (compare != 0) return compare;
                int 結存量1 = x[(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                int 結存量2 = y[(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                if (結存量1 > 結存量2)
                {
                    return -1;
                }
                else if (結存量1 < 結存量2)
                {
                    return 1;
                }
                else if (結存量1 == 結存量2) return 0;
                return 0;

            }
        }
    }
}
