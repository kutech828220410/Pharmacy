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
    public enum enum_盤點合併_庫存差異量_匯入
    {
        藥碼,
        數量,
    }
    public enum enum_盤點合併_盤點總表
    {
        藥碼,
        藥名,
        單價,
        藥局庫存,
        藥局盤點量,
        藥庫庫存,
        藥庫盤點量,
    }
    public class class_MedPrice
    {
        public string 藥品碼 { get; set; }
        public string 售價 { get; set; }
        public string 成本價 { get; set; }
        public string 最近一次售價 { get; set; }
        public string 最近一次成本價 { get; set; }

    }

    public partial class Form1 : Form
    {
     

  
        private void sub_Program_盤點合併_Init()
        {
            this.plC_RJ_Button_盤點合併_上傳Excel.MouseDownEvent += PlC_RJ_Button_盤點合併_上傳Excel_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_庫存帶入.MouseDownEvent += PlC_RJ_Button_盤點合併_庫存帶入_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_計算庫存差異量.MouseDownEvent += PlC_RJ_Button_盤點合併_計算庫存差異量_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_計算誤差量.MouseDownEvent += PlC_RJ_Button_盤點合併_計算誤差量_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_匯出Excel.MouseDownEvent += PlC_RJ_Button_盤點合併_匯出Excel_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_載入單價.MouseDownEvent += PlC_RJ_Button_盤點合併_載入單價_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_計算消耗量.MouseDownEvent += PlC_RJ_Button_盤點合併_計算消耗量_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_載入庫存差異量.MouseDownEvent += PlC_RJ_Button_盤點合併_載入庫存差異量_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_製作盤點總表.MouseDownEvent += PlC_RJ_Button_盤點合併_製作盤點總表_MouseDownEvent;

            Table table = new Table("");
            table.AddColumnList("藥碼", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("料號", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("藥名", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("單位", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("單價", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("庫存量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("庫存金額", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("盤點量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("庫存差異量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("異動後結存量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("消耗量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("結存金額", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("誤差量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("誤差金額", Table.StringType.VARCHAR, Table.IndexType.None);
            this.sqL_DataGridView_盤點合併_盤點表.Init(table);
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnVisible(false, new enum_盤點定盤_Excel().GetEnumNames());
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "藥碼");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(400, DataGridViewContentAlignment.MiddleLeft, "藥名");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "單位");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "單價");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "庫存量");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "庫存金額");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "盤點量");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "庫存差異量");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "異動後結存量");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "消耗量");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "結存金額");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "誤差量");
            this.sqL_DataGridView_盤點合併_盤點表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "誤差金額");


            plC_UI_Init.Add_Method(sub_Program_盤點合併);
        }

        private void PlC_RJ_Button_盤點合併_製作盤點總表_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;

            DataTable dataTable_藥庫 = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            dataTable_藥庫 = dataTable_藥庫.ReorderTable(new enum_盤點定盤_Excel());
            List<object[]> list_藥庫 = dataTable_藥庫.DataTableToRowList();
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;
            DataTable dataTable_藥局 = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            dataTable_藥局 = dataTable_藥局.ReorderTable(new enum_盤點定盤_Excel());
            List<object[]> list_藥局 = dataTable_藥局.DataTableToRowList();

            List<object[]> list_value = new List<object[]>();
            List<object[]> list_value_buf = new List<object[]>();
            for(int i = 0; i < list_藥庫.Count; i++)
            {
                string 藥碼 = list_藥庫[i][(int)enum_盤點定盤_Excel.藥碼].ObjectToString();
                string 藥名 = list_藥庫[i][(int)enum_盤點定盤_Excel.藥名].ObjectToString();
                string 盤點量 = list_藥庫[i][(int)enum_盤點定盤_Excel.異動後結存量].ObjectToString();
                string 庫存量 = list_藥庫[i][(int)enum_盤點定盤_Excel.庫存量].ObjectToString();
                list_value_buf = list_value.GetRows((int)enum_盤點定盤_Excel.藥碼, 藥碼);
                if (list_value_buf.Count == 0)
                {
                    object[] value = new object[new enum_盤點合併_盤點總表().GetLength()];
                    value[(int)enum_盤點合併_盤點總表.藥碼] = 藥碼;
                    value[(int)enum_盤點合併_盤點總表.藥名] = 藥名;
                    value[(int)enum_盤點合併_盤點總表.藥庫盤點量] = 盤點量;
                    value[(int)enum_盤點合併_盤點總表.藥庫庫存] = 庫存量;
                    value[(int)enum_盤點合併_盤點總表.藥局盤點量] = "0";
                    value[(int)enum_盤點合併_盤點總表.藥局庫存] = "0";
                    list_value.Add(value);
                }
                else
                {
                    object[] value = list_value_buf[0];
                    value[(int)enum_盤點合併_盤點總表.藥碼] = 藥碼;
                    value[(int)enum_盤點合併_盤點總表.藥名] = 藥名;
                    value[(int)enum_盤點合併_盤點總表.藥庫盤點量] = 盤點量;
                    value[(int)enum_盤點合併_盤點總表.藥庫庫存] = 庫存量;
                }
            }
            for (int i = 0; i < list_藥局.Count; i++)
            {
                string 藥碼 = list_藥局[i][(int)enum_盤點定盤_Excel.藥碼].ObjectToString();
                string 藥名 = list_藥局[i][(int)enum_盤點定盤_Excel.藥名].ObjectToString();
                string 盤點量 = list_藥局[i][(int)enum_盤點定盤_Excel.異動後結存量].ObjectToString();
                string 庫存量 = list_藥局[i][(int)enum_盤點定盤_Excel.庫存量].ObjectToString();
                list_value_buf = list_value.GetRows((int)enum_盤點定盤_Excel.藥碼, 藥碼);
                if (list_value_buf.Count == 0)
                {
                    object[] value = new object[new enum_盤點合併_盤點總表().GetLength()];
                    value[(int)enum_盤點合併_盤點總表.藥碼] = 藥碼;
                    value[(int)enum_盤點合併_盤點總表.藥名] = 藥名;
                    value[(int)enum_盤點合併_盤點總表.藥局盤點量] = 盤點量;
                    value[(int)enum_盤點合併_盤點總表.藥局庫存] = 庫存量;
                    value[(int)enum_盤點合併_盤點總表.藥庫盤點量] = "0";
                    value[(int)enum_盤點合併_盤點總表.藥庫庫存] = "0";
                    list_value.Add(value);
                }
                else
                {
                    object[] value = list_value_buf[0];
                    value[(int)enum_盤點合併_盤點總表.藥碼] = 藥碼;
                    value[(int)enum_盤點合併_盤點總表.藥名] = 藥名;
                    value[(int)enum_盤點合併_盤點總表.藥局盤點量] = 盤點量;
                    value[(int)enum_盤點合併_盤點總表.藥局庫存] = 庫存量;
                }
            }
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;

            string jsonstr = MyFileStream.LoadFileAllText($"{this.openFileDialog_LoadExcel.FileName}");
            List<class_MedPrice> class_MedPrices = jsonstr.JsonDeserializet<List<class_MedPrice>>();
            List<class_MedPrice> class_MedPrices_buf = new List<class_MedPrice>();
            for (int i = 0; i < list_value.Count; i++)
            {
                string 藥碼 = list_value[i][(int)enum_盤點合併_盤點總表.藥碼].ObjectToString();
                class_MedPrices_buf = (from temp in class_MedPrices
                                       where temp.藥品碼 == 藥碼
                                       select temp).ToList();
                if (class_MedPrices_buf.Count > 0)
                {
                    list_value[i][(int)enum_盤點合併_盤點總表.單價] = class_MedPrices_buf[0].成本價;
                }
            }


            this.Invoke(new Action(delegate
            {
                DataTable dataTable = list_value.ToDataTable(new enum_盤點合併_盤點總表());
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    MyOffice.ExcelClass.NPOI_SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);

                }
            }));
            MyMessageBox.ShowDialog("完成!");
        }

        private void sub_Program_盤點合併()
        {

        }
        #region Event
        private void PlC_RJ_Button_盤點合併_載入庫存差異量_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;
            DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            dataTable = dataTable.ReorderTable(new enum_盤點合併_庫存差異量_匯入());
            if (dataTable == null)
            {
                return;
            }
            List<object[]> list_value_load = dataTable.DataTableToRowList();
            List<object[]> list_value_load_add = new List<object[]>();
            List<object[]> list_value = sqL_DataGridView_盤點合併_盤點表.GetAllRows();
            List<object[]> list_value_buf = new List<object[]>();
            for(int i = 0; i < list_value_load.Count; i++)
            {
                string 藥碼 = list_value_load[i][(int)enum_盤點合併_庫存差異量_匯入.藥碼].ObjectToString();
                string 數量 = list_value_load[i][(int)enum_盤點合併_庫存差異量_匯入.數量].ObjectToString();
                list_value_buf = list_value.GetRows((int)enum_盤點定盤_Excel.藥碼, 藥碼);
                if(list_value_buf.Count > 0)
                {
                    list_value_buf[0][(int)enum_盤點定盤_Excel.庫存量] = 數量;
                    //string str = list_value_buf[0][(int)enum_盤點定盤_Excel.庫存差異量].ObjectToString();
                    //if (str.StringIsInt32() == false) str = "0";
                    //str = (str.StringToInt32() + 數量.StringToInt32()).ToString();
                    //list_value_buf[0][(int)enum_盤點定盤_Excel.庫存差異量] = str;
                }
                else
                {
                    object[] value = new object[new enum_盤點定盤_Excel().GetLength()];
                    value[(int)enum_盤點定盤_Excel.藥碼] = 藥碼;
                    value[(int)enum_盤點定盤_Excel.庫存量] = 數量;
                    value[(int)enum_盤點定盤_Excel.異動後結存量] = 數量;
                    list_value_load_add.Add(value);
                }
            }
            for(int i = 0; i < list_value_load_add.Count; i++)
            {
                list_value.Add(list_value_load_add[i]);
            }
            sqL_DataGridView_盤點合併_盤點表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_盤點合併_計算消耗量_MouseDownEvent(MouseEventArgs mevent)
        {
    
            DateTime dateTime_st = rJ_DatePicker_盤點合併_計算庫存差異量.Value;
    
            dateTime_st = new DateTime(2023, 10, 27, 00, 00, 00);
            DateTime dateTime_end = rJ_DatePicker_盤點合併_計算庫存差異量.Value;
            dateTime_end = new DateTime(dateTime_end.Year, 01, 05, 23, 59, 59);
            List<object[]> list_藥品消耗帳 = Function_藥品過消耗帳_取得所有過帳明細(dateTime_st, dateTime_end);
            List<object[]> list_藥品消耗帳_buf = new List<object[]>();
            List<object[]> list_value = this.sqL_DataGridView_盤點合併_盤點表.GetAllRows();
            for (int i = 0; i < list_value.Count; i++)
            {
                string 藥碼 = list_value[i][(int)enum_盤點定盤_Excel.藥碼].ObjectToString();
                list_藥品消耗帳_buf = list_藥品消耗帳.GetRows((int)enum_藥品過消耗帳.藥品碼, 藥碼);
                list_value[i][(int)enum_盤點定盤_Excel.消耗量] = "0";
                if (list_藥品消耗帳_buf.Count > 0)
                {
                    int temp = 0;
                    for (int k = 0; k < list_藥品消耗帳_buf.Count; k++)
                    {
                        temp += list_藥品消耗帳_buf[k][(int)enum_藥品過消耗帳.異動量].StringToInt32();
                    }
                    list_value[i][(int)enum_盤點定盤_Excel.消耗量] = (temp * -1).ToString();
                }
            }
            this.sqL_DataGridView_盤點合併_盤點表.RefreshGrid(list_value);
            MyMessageBox.ShowDialog("完成!");
        }
        private void PlC_RJ_Button_盤點合併_上傳Excel_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;
            DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            dataTable = dataTable.ReorderTable(new enum_盤點定盤_Excel());
            if (dataTable == null)
            {
                return;
            }
            List<object[]> list_value_load = dataTable.DataTableToRowList();
            for (int i = 0; i < list_value_load.Count; i++)
            {

            }
            sqL_DataGridView_盤點合併_盤點表.RefreshGrid(list_value_load);
        }
        private void PlC_RJ_Button_盤點合併_載入單價_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;

            string jsonstr = MyFileStream.LoadFileAllText($"{this.openFileDialog_LoadExcel.FileName}");
            List<class_MedPrice> class_MedPrices = jsonstr.JsonDeserializet<List<class_MedPrice>>();
            List<class_MedPrice> class_MedPrices_buf = new List<class_MedPrice>();
            List<object[]> list_value = this.sqL_DataGridView_盤點合併_盤點表.GetAllRows();
            for (int i = 0; i < list_value.Count; i++)
            {
                string 藥碼 = list_value[i][(int)enum_盤點定盤_Excel.藥碼].ObjectToString();
                class_MedPrices_buf = (from temp in class_MedPrices
                                       where temp.藥品碼 == 藥碼
                                       select temp).ToList();
                if(class_MedPrices_buf.Count > 0)
                {
                    list_value[i][(int)enum_盤點定盤_Excel.單價] = class_MedPrices_buf[0].成本價;
                }
            }
            this.sqL_DataGridView_盤點合併_盤點表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_盤點合併_庫存帶入_MouseDownEvent(MouseEventArgs mevent)
        {
            string 庫別 = "";
            this.Invoke(new Action(delegate 
            {
                庫別 = comboBox_盤點合併_庫別選擇.Text;
            }));
            if(庫別 != "藥庫" && 庫別 != "藥局")
            {
                MyMessageBox.ShowDialog("請選擇庫別!");
                return;
            }
            Function_從SQL取得儲位到本地資料();
            List<object[]> list_value = this.sqL_DataGridView_盤點合併_盤點表.GetAllRows();
            for (int i = 0; i < list_value.Count; i++)
            {
                string 藥碼 = list_value[i][(int)enum_盤點定盤_Excel.藥碼].ObjectToString();
                if (庫別 == "藥庫")
                {
                    List<DeviceBasic> deviceBasics = this.List_藥庫_DeviceBasic.SortByCode(藥碼);
                    if(deviceBasics.Count > 0)
                    {
                        list_value[i][(int)enum_盤點定盤_Excel.庫存量] = deviceBasics[0].Inventory;
                    }
                }
                else if (庫別 == "藥局")
                {
                    List<DeviceBasic> deviceBasics = this.List_藥局_DeviceBasic.SortByCode(藥碼);
                    if (deviceBasics.Count > 0)
                    {
                        list_value[i][(int)enum_盤點定盤_Excel.庫存量] = deviceBasics[0].Inventory;
                    }
                }
            }
            this.sqL_DataGridView_盤點合併_盤點表.RefreshGrid(list_value);
            MyMessageBox.ShowDialog("完成!");
        }
   
        private void PlC_RJ_Button_盤點合併_匯出Excel_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_盤點合併_盤點表.GetAllRows();
            this.Invoke(new Action(delegate
            {
                DataTable dataTable = list_value.ToDataTable(new enum_盤點定盤_Excel());
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    MyOffice.ExcelClass.NPOI_SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);

                }
            }));
            MyMessageBox.ShowDialog("完成!");

        }
        private void PlC_RJ_Button_盤點合併_計算誤差量_MouseDownEvent(MouseEventArgs mevent)
        {

        }
        private void PlC_RJ_Button_盤點合併_計算庫存差異量_MouseDownEvent(MouseEventArgs mevent)
        {
            string 庫別 = "";
            this.Invoke(new Action(delegate
            {
                庫別 = comboBox_盤點合併_庫別選擇.Text;
            }));
            if (庫別 != "藥庫" && 庫別 != "藥局")
            {
                MyMessageBox.ShowDialog("請選擇庫別!");
                return;
            }
            DateTime dateTime_st = rJ_DatePicker_盤點合併_計算庫存差異量.Value;
            dateTime_st = new DateTime(dateTime_st.Year, dateTime_st.Month, dateTime_st.Day, 17, 00, 00);
            DateTime dateTime_end = rJ_DatePicker_盤點合併_計算庫存差異量.Value;
            dateTime_end = new DateTime(dateTime_st.Year, dateTime_st.Month, dateTime_st.Day, 23, 59, 59);
            List<object[]> list_交易紀錄 = sqL_DataGridView_交易記錄查詢.SQL_GetRowsByBetween((int)enum_交易記錄查詢資料.開方時間, dateTime_st, dateTime_end, false);
            List<object[]> list_交易紀錄_buf = new List<object[]>();
            List<object[]> list_value = this.sqL_DataGridView_盤點合併_盤點表.GetAllRows();
            for (int i = 0; i < list_value.Count; i++)
            {
                string 藥碼 = list_value[i][(int)enum_盤點定盤_Excel.藥碼].ObjectToString();
                list_交易紀錄_buf = list_交易紀錄.GetRows((int)enum_交易記錄查詢資料.藥品碼, 藥碼);
                list_交易紀錄_buf = list_交易紀錄_buf.GetRows((int)enum_交易記錄查詢資料.庫別, 庫別);
                list_value[i][(int)enum_盤點定盤_Excel.庫存差異量] = "0";
                if (list_交易紀錄_buf.Count > 0)
                {
                    int temp = 0;
                    for(int  k = 0; k < list_交易紀錄_buf.Count; k++)
                    {
                        temp += list_交易紀錄_buf[k][(int)enum_交易記錄查詢資料.交易量].StringToInt32();
                    }
                    list_value[i][(int)enum_盤點定盤_Excel.庫存差異量] = temp.ToString();
                }
            }
            this.sqL_DataGridView_盤點合併_盤點表.RefreshGrid(list_value);
            MyMessageBox.ShowDialog("完成!");
        }
        #endregion
    }
}
