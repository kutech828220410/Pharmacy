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
using MyOffice;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        public enum enum_定盤_盤點明細
        {
            GUID,
            藥碼,
            藥名,
            庫存量,
            庫存差異量,
            盤點量,
            異動量,                  
            效期及批號,
        }
        public enum enum_定盤_盤點明細_匯出
        {
            藥碼,
            藥名,
            庫存量,
            消耗量,
            盤點量,
            異動量,
            效期及批號,
        }
        public enum enum_定盤_盤點明細_匯入
        {
            藥碼,
            藥名,
            庫存量,
            消耗量,
            盤點量,
            異動量,
        }
        private void sub_Program_盤點作業_定盤_Init()
        {
            Table table = new Table("");
            table.AddColumnList("GUID", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("藥碼", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("藥名", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("庫存量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("庫存差異量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("盤點量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("異動量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("效期及批號", Table.StringType.VARCHAR, Table.IndexType.None);
            this.sqL_DataGridView_定盤_盤點明細.Init(table);

            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnVisible(false, new enum_定盤_盤點明細().GetEnumNames());
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_定盤_盤點明細.藥碼);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnWidth(400, DataGridViewContentAlignment.MiddleLeft, enum_定盤_盤點明細.藥名);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_定盤_盤點明細.庫存量);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_定盤_盤點明細.庫存差異量);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_定盤_盤點明細.盤點量);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_定盤_盤點明細.異動量);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnWidth(400, DataGridViewContentAlignment.MiddleLeft, enum_定盤_盤點明細.效期及批號);

            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnSortMode(DataGridViewColumnSortMode.Automatic, enum_定盤_盤點明細.藥碼);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnSortMode(DataGridViewColumnSortMode.Automatic, enum_定盤_盤點明細.庫存量);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnSortMode(DataGridViewColumnSortMode.Automatic, enum_定盤_盤點明細.盤點量);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnSortMode(DataGridViewColumnSortMode.Automatic, enum_定盤_盤點明細.庫存差異量);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnSortMode(DataGridViewColumnSortMode.Automatic, enum_定盤_盤點明細.異動量);
            this.sqL_DataGridView_定盤_盤點明細.Set_ColumnSortMode(DataGridViewColumnSortMode.Automatic, enum_定盤_盤點明細.效期及批號);

            this.sqL_DataGridView_定盤_盤點明細.DataGridRefreshEvent += SqL_DataGridView_定盤_盤點明細_DataGridRefreshEvent;

            this.plC_RJ_Button_定盤_盤點明細_匯入庫存量.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_匯入庫存量_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_匯入消耗量.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_匯入消耗量_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_匯入盤點量.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_匯入盤點量_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_重置作業.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_重置作業_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_計算定盤結果.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_計算定盤結果_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_確認更動庫存量.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_確認更動庫存量_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_匯出.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_匯出_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_取得藥局庫存量.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_取得藥局庫存量_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_取得藥庫庫存量.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_取得藥庫庫存量_MouseDownEvent;

            this.PlC_RJ_Button_定盤_盤點明細_重置作業_MouseDownEvent(null);
            this.plC_UI_Init.Add_Method(sub_Program_盤點作業_定盤);
        }

    

        private bool flag_Program_盤點作業_定盤_Init = false;
        private void sub_Program_盤點作業_定盤()
        {
            if (this.plC_ScreenPage_Main.PageText == "盤點作業" && this.plC_ScreenPage_盤點作業.PageText == "定盤")
            {
                if (!flag_Program_盤點作業_定盤_Init)
                {
                    flag_Program_盤點作業_定盤_Init = true;
                }
            }
            else
            {
                flag_Program_盤點作業_定盤_Init = false;
            }
        }

        #region Function

        #endregion
        #region Event
        private void SqL_DataGridView_定盤_盤點明細_DataGridRefreshEvent()
        {
            for (int i = 0; i < this.sqL_DataGridView_定盤_盤點明細.dataGridView.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = this.sqL_DataGridView_定盤_盤點明細.dataGridView.Rows[i].Cells;
                string 庫存量 = cells[enum_定盤_盤點明細.庫存量.GetEnumName()].Value.ToString();
                string 消耗量 = cells[enum_定盤_盤點明細.庫存差異量.GetEnumName()].Value.ToString();
                string 異動量 = cells[enum_定盤_盤點明細.異動量.GetEnumName()].Value.ToString();
                string 盤點量 = cells[enum_定盤_盤點明細.盤點量.GetEnumName()].Value.ToString();

                if (庫存量.StringIsEmpty())
                {
                    cells[enum_定盤_盤點明細.庫存量.GetEnumName()].Value = "-";
                }
                if (消耗量.StringIsEmpty())
                {
                    cells[enum_定盤_盤點明細.庫存差異量.GetEnumName()].Value = "-";
                }
                if (異動量.StringIsEmpty())
                {
                    cells[enum_定盤_盤點明細.異動量.GetEnumName()].Value = "-";
                }
                if (盤點量.StringIsEmpty())
                {
                    cells[enum_定盤_盤點明細.盤點量.GetEnumName()].Value = "-";
                }
            }
        }
        private void PlC_RJ_Button_定盤_盤點明細_重置作業_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                sqL_DataGridView_定盤_盤點明細.ClearGrid();
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = false;
                comboBox_定盤_盤點明細_庫別.Enabled = false;
                plC_RJ_Button_定盤_盤點明細_確認更動庫存量.Enabled = false;
            }));
        }
        private void PlC_RJ_Button_定盤_盤點明細_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataTable dataTable = this.sqL_DataGridView_定盤_盤點明細.GetDataTable();
                    dataTable = dataTable.ReorderTable(new enum_定盤_盤點明細_匯出());
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
        private void PlC_RJ_Button_定盤_盤點明細_匯入庫存量_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;

            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;

            DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            if (dataTable == null)
            {
                MyMessageBox.ShowDialog("讀取失敗!");
                return;
            }
            dataTable = dataTable.ReorderTable(new string[] { enum_定盤_盤點明細.藥碼.GetEnumName(), enum_定盤_盤點明細.庫存量.GetEnumName() });
            if (dataTable == null)
            {
                return;
            }
            List<object[]> list_value = new List<object[]>();

            List<object[]> list_value_匯入 = dataTable.DataTableToRowList();
            List<object[]> list_value_匯入_buf = new List<object[]>();
            List<string> Codes = (from temp in list_value_匯入
                                  select temp[0].ObjectToString()).Distinct().ToList();

            List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();

            for(int i = 0; i < Codes.Count; i++)
            {
                string 藥碼 = Codes[i];
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端藥檔.藥品碼, 藥碼);
                if (list_藥品資料_buf.Count == 0) continue;
                string 藥名 = list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品名稱].ObjectToString();

                object[] value = new object[new enum_定盤_盤點明細().GetLength()];
                value[(int)enum_定盤_盤點明細.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_定盤_盤點明細.藥碼] = 藥碼;
                value[(int)enum_定盤_盤點明細.藥名] = 藥名;

                list_value_匯入_buf = list_value_匯入.GetRows(0, 藥碼);
                int 庫存量 = 0;
                for(int k = 0; k < list_value_匯入_buf.Count; k++)
                {
                    if(list_value_匯入_buf[k][1].ObjectToString().StringIsInt32())
                    {
                        庫存量 += list_value_匯入_buf[k][1].StringToInt32();
                    }
                }
                value[(int)enum_定盤_盤點明細.庫存量] = 庫存量;
                list_value.Add(value);
            }
            this.sqL_DataGridView_定盤_盤點明細.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_定盤_盤點明細_匯入消耗量_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;

            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;

            DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            if (dataTable == null)
            {
                MyMessageBox.ShowDialog("讀取失敗!");
                return;
            }
            dataTable = dataTable.ReorderTable(new string[] { enum_定盤_盤點明細.藥碼.GetEnumName(), enum_定盤_盤點明細.庫存差異量.GetEnumName() });
            if (dataTable == null)
            {
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_定盤_盤點明細.GetAllRows();
            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_value_replace = new List<object[]>();
            List<object[]> list_value_add = new List<object[]>();

            for(int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_定盤_盤點明細.庫存差異量] = "0";
            }
            List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();

            List<object[]> list_value_匯入 = dataTable.DataTableToRowList();
            List<object[]> list_value_匯入_buf = new List<object[]>();
            List<string> Codes = (from temp in list_value_匯入
                                  select temp[0].ObjectToString()).Distinct().ToList();
            for(int i = 0; i < Codes.Count; i++)
            {
                string 藥碼 = Codes[i];
                int 消耗量 = 0;
                list_value_匯入_buf = list_value_匯入.GetRows(0, 藥碼);
                for(int k = 0; k < list_value_匯入_buf.Count; k++)
                {
                    if(list_value_匯入_buf[k][1].ObjectToString().StringIsInt32())
                    {
                        消耗量 += list_value_匯入_buf[k][1].StringToInt32();
                    }
                   
                }

                list_value_buf = list_value.GetRows((int)enum_定盤_盤點明細.藥碼, 藥碼);
                if(list_value_buf.Count > 0)
                {
                    list_value_buf[0][(int)enum_定盤_盤點明細.庫存差異量] = 消耗量;
                    list_value_replace.Add(list_value_buf[0]);
                }
                else
                {
                    list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端藥檔.藥品碼, 藥碼);
                    if (list_藥品資料_buf.Count > 0)
                    {
                        string 藥名 = list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品名稱].ObjectToString();
                        object[] value = new object[new enum_定盤_盤點明細().GetLength()];

                        value[(int)enum_定盤_盤點明細.GUID] = Guid.NewGuid().ToString();
                        value[(int)enum_定盤_盤點明細.藥碼] = 藥碼;
                        value[(int)enum_定盤_盤點明細.藥名] = 藥名;
                        value[(int)enum_定盤_盤點明細.庫存量] = "0";
                        value[(int)enum_定盤_盤點明細.庫存差異量] = 消耗量;
                        value[(int)enum_定盤_盤點明細.異動量] = "0";
                        value[(int)enum_定盤_盤點明細.盤點量] = "0";

                        list_value_add.Add(value);
                    }
                }
            }

            this.sqL_DataGridView_定盤_盤點明細.ReplaceExtra(list_value_replace, true);
            this.sqL_DataGridView_定盤_盤點明細.AddRows(list_value_add, true);


        }
        private void PlC_RJ_Button_定盤_盤點明細_匯入盤點量_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;

            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;

            DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            if (dataTable == null)
            {
                MyMessageBox.ShowDialog("讀取失敗!");
                return;
            }
            dataTable = dataTable.ReorderTable(new string[] { enum_定盤_盤點明細.藥碼.GetEnumName(), enum_定盤_盤點明細.盤點量.GetEnumName() });
            if (dataTable == null)
            {
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_定盤_盤點明細.GetAllRows();
            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_value_replace = new List<object[]>();
            List<object[]> list_value_add = new List<object[]>();

            for (int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_定盤_盤點明細.盤點量] = "0";
            }
            List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();

            List<object[]> list_value_匯入 = dataTable.DataTableToRowList();
            List<object[]> list_value_匯入_buf = new List<object[]>();
            List<string> Codes = (from temp in list_value_匯入
                                  select temp[0].ObjectToString()).Distinct().ToList();
            for (int i = 0; i < Codes.Count; i++)
            {
                string 藥碼 = Codes[i];
                int 盤點量 = 0;
                list_value_匯入_buf = list_value_匯入.GetRows(0, 藥碼);
                for (int k = 0; k < list_value_匯入_buf.Count; k++)
                {
                    盤點量 += list_value_匯入_buf[k][1].StringToInt32();
                }

                list_value_buf = list_value.GetRows((int)enum_定盤_盤點明細.藥碼, 藥碼);
                if (list_value_buf.Count > 0)
                {
                    list_value_buf[0][(int)enum_定盤_盤點明細.盤點量] = 盤點量;
                    if (list_value_buf[0][(int)enum_定盤_盤點明細.庫存差異量].ObjectToString().StringIsInt32() == false)
                    {
                        list_value_buf[0][(int)enum_定盤_盤點明細.庫存差異量] = "0";
                    }
                    list_value_replace.Add(list_value_buf[0]);
                }
                else
                {
                    list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端藥檔.藥品碼, 藥碼);
                    if (list_藥品資料_buf.Count > 0)
                    {
                        string 藥名 = list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品名稱].ObjectToString();
                        object[] value = new object[new enum_定盤_盤點明細().GetLength()];

                        value[(int)enum_定盤_盤點明細.GUID] = Guid.NewGuid().ToString();
                        value[(int)enum_定盤_盤點明細.藥碼] = 藥碼;
                        value[(int)enum_定盤_盤點明細.藥名] = 藥名;
                        value[(int)enum_定盤_盤點明細.庫存量] = "0";
                        value[(int)enum_定盤_盤點明細.盤點量] = 盤點量;
                        value[(int)enum_定盤_盤點明細.庫存差異量] = "0";

                        list_value_add.Add(value);
                    }
                }
            }

            this.sqL_DataGridView_定盤_盤點明細.ReplaceExtra(list_value_replace, true);
            this.sqL_DataGridView_定盤_盤點明細.AddRows(list_value_add, true);
            this.Invoke(new Action(delegate 
            {
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = true;
                comboBox_定盤_盤點明細_庫別.Enabled = true;
            }));
  
        }
        private void PlC_RJ_Button_定盤_盤點明細_取得藥庫庫存量_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_盤點明細 = new List<object[]>();
            List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            Function_從SQL取得儲位到本地資料();
            for (int i = 0; i < this.List_藥庫_DeviceBasic.Count; i++)
            {
                object[] value = new object[new enum_定盤_盤點明細().GetLength()];
                value[(int)enum_定盤_盤點明細.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_定盤_盤點明細.藥碼] = this.List_藥庫_DeviceBasic[i].Code;
                value[(int)enum_定盤_盤點明細.藥名] = this.List_藥庫_DeviceBasic[i].Name;
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端藥檔.藥品碼, this.List_藥庫_DeviceBasic[i].Code);
                if (list_藥品資料_buf.Count > 0)
                {
                    value[(int)enum_定盤_盤點明細.藥名] = list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品名稱].ObjectToString();
                }
                value[(int)enum_定盤_盤點明細.庫存量] = this.List_藥庫_DeviceBasic[i].Inventory;
                value[(int)enum_定盤_盤點明細.異動量] = "0";
                value[(int)enum_定盤_盤點明細.盤點量] = this.List_藥庫_DeviceBasic[i].Inventory;

                list_盤點明細.Add(value);
            }
            this.sqL_DataGridView_定盤_盤點明細.RefreshGrid(list_盤點明細);
            this.Invoke(new Action(delegate
            {
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = true;
                comboBox_定盤_盤點明細_庫別.Enabled = true;
            }));
        }
        private void PlC_RJ_Button_定盤_盤點明細_取得藥局庫存量_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_盤點明細 = new List<object[]>();
            List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            Function_從SQL取得儲位到本地資料();
            for (int i = 0; i < this.List_藥局_DeviceBasic.Count; i++)
            {
                object[] value = new object[new enum_定盤_盤點明細().GetLength()];
                value[(int)enum_定盤_盤點明細.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_定盤_盤點明細.藥碼] = this.List_藥局_DeviceBasic[i].Code;
                value[(int)enum_定盤_盤點明細.藥名] = this.List_藥局_DeviceBasic[i].Name;
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端藥檔.藥品碼, this.List_藥庫_DeviceBasic[i].Code);
                if (list_藥品資料_buf.Count > 0)
                {
                    value[(int)enum_定盤_盤點明細.藥名] = list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品名稱].ObjectToString();
                }
                value[(int)enum_定盤_盤點明細.庫存量] = this.List_藥局_DeviceBasic[i].Inventory;
                value[(int)enum_定盤_盤點明細.異動量] = "0";
                value[(int)enum_定盤_盤點明細.盤點量] = this.List_藥局_DeviceBasic[i].Inventory;

                list_盤點明細.Add(value);
            }
            this.sqL_DataGridView_定盤_盤點明細.RefreshGrid(list_盤點明細);
            this.Invoke(new Action(delegate
            {
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = true;
                comboBox_定盤_盤點明細_庫別.Enabled = true;
            }));
        }
        private void PlC_RJ_Button_定盤_盤點明細_計算定盤結果_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
            }));
           
            string 庫別 = "";
            this.Invoke(new Action(delegate
            {
                庫別 = comboBox_定盤_盤點明細_庫別.Text;
            }));
            if (庫別.StringIsEmpty() == true)
            {
                MyMessageBox.ShowDialog("未選擇庫別");
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));
              
                return;
            }
            Function_從SQL取得儲位到本地資料();
            List<object[]> list_盤點明細 = this.sqL_DataGridView_定盤_盤點明細.GetAllRows();
            if (庫別 == "藥庫")
            {
      
                Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_盤點明細.Count);
                dialog_Prcessbar.State = "盤點明細計算...";
                for (int i = 0; i < list_盤點明細.Count; i++)
                {
                    dialog_Prcessbar.Value = i;
                    string 備註 = "";
                    string 藥碼 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥碼].ObjectToString();
                    int 盤點量 = list_盤點明細[i][(int)enum_定盤_盤點明細.盤點量].StringToInt32();               
                    int 消耗量 = list_盤點明細[i][(int)enum_定盤_盤點明細.庫存差異量].StringToInt32();
                    int 庫存量 = list_盤點明細[i][(int)enum_定盤_盤點明細.庫存量].StringToInt32();
                    if (盤點量 < 0) 盤點量 = 0;
                    if (消耗量 < 0) 消耗量 = 0;
                    if (庫存量 < 0) 庫存量 = 0;
                    int 異動量 = 0;
                    List<DeviceBasic> deviceBasics = this.List_藥庫_DeviceBasic.SortByCode(藥碼);
                    if (deviceBasics.Count == 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.庫存量] = "異常";
                        continue;
                    }
                    異動量 = (盤點量 - 庫存量) + 消耗量;
                    list_盤點明細[i][(int)enum_定盤_盤點明細.異動量] = 異動量.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] = "";
                    List<string> list_效期 = new List<string>();
                    List<string> list_批號 = new List<string>();
                    List<string> list_異動量 = new List<string>();
                    if (異動量 == 0)
                    {
                        if (消耗量 != 0)
                        {

                        }
                        continue;
                    }
                    if (庫存量 == 0 || 異動量 > 0)
                    {
                    
                        Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥碼, ref list_效期, ref list_批號);
                        if (list_效期.Count != 0)
                        {
                            if (list_批號[0].StringIsEmpty() == true) list_批號[0] = "無";
                            備註 += $"[效期]:{list_效期[0]},[批號]:{list_批號[0]}";
                            deviceBasics[0].效期庫存異動(list_效期[0], list_批號[0], 異動量.ToString());
                        }
                        else
                        {
                            備註 += $"[效期]:{"2000/12/31"},[批號]:{"系統代入"}";
                            deviceBasics[0].效期庫存異動("2000/12/31", "系統代入", 異動量.ToString());
                        }
                    }
                    else
                    {
                        deviceBasics[0].庫存異動(異動量, out list_效期, out list_批號, out list_異動量);
                        for (int k = 0; k < list_效期.Count; k++)
                        {
                            備註 += $"[效期]:{list_效期[k]},[批號]:{list_批號[k]}";
                            if (k != list_效期.Count - 1) 備註 += "\n";
                        }
                    }
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] += 備註;
                }
                dialog_Prcessbar.Close();
                dialog_Prcessbar.Dispose();
            }
            if (庫別 == "藥局")
            {
               
                Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_盤點明細.Count);
                dialog_Prcessbar.State = "盤點明細計算...";
                for (int i = 0; i < list_盤點明細.Count; i++)
                {
                    dialog_Prcessbar.Value = i;
                    string 備註 = "";
                    string 藥碼 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥碼].ObjectToString();
                    int 盤點量 = list_盤點明細[i][(int)enum_定盤_盤點明細.盤點量].StringToInt32();
                    int 消耗量 = list_盤點明細[i][(int)enum_定盤_盤點明細.庫存差異量].StringToInt32();
                    int 庫存量 = list_盤點明細[i][(int)enum_定盤_盤點明細.庫存量].StringToInt32();
                    if (盤點量 < 0) 盤點量 = 0;
                    if (消耗量 < 0) 消耗量 = 0;
                    if (庫存量 < 0) 庫存量 = 0;
                    int 異動量 = 0;
                    List<DeviceBasic> deviceBasics = this.List_藥局_DeviceBasic.SortByCode(藥碼);
                    if (deviceBasics.Count == 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.庫存量] = "異常";
                        continue;
                    }
                    異動量 = (盤點量 - 庫存量) + 消耗量;
                    list_盤點明細[i][(int)enum_定盤_盤點明細.異動量] = 異動量.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] = "";
                    List<string> list_效期 = new List<string>();
                    List<string> list_批號 = new List<string>();
                    List<string> list_異動量 = new List<string>();
                    if (異動量 == 0)
                    {
                        if(消耗量 != 0)
                        {

                        }
                        continue;
                    }
                    if (庫存量 == 0 || 異動量 > 0)
                    {

                        Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥碼, ref list_效期, ref list_批號);
                        if (list_效期.Count != 0)
                        {
                            if (list_批號[0].StringIsEmpty() == true) list_批號[0] = "無";
                            備註 += $"[效期]:{list_效期[0]},[批號]:{list_批號[0]}";
                            deviceBasics[0].效期庫存異動(list_效期[0], list_批號[0], 異動量.ToString());
                        }
                        else
                        {
                            備註 += $"[效期]:{"2000/12/31"},[批號]:{"系統代入"}";
                            deviceBasics[0].效期庫存異動("2000/12/31", "系統代入", 異動量.ToString());
                        }
                    }
                    else
                    {
                        deviceBasics[0].庫存異動(異動量, out list_效期, out list_批號, out list_異動量);
                        for (int k = 0; k < list_效期.Count; k++)
                        {
                            備註 += $"[效期]:{list_效期[k]},[批號]:{list_批號[k]}";
                            if (k != list_效期.Count - 1) 備註 += "\n";
                        }
                    }
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] += 備註;
                }
                dialog_Prcessbar.Close();
                dialog_Prcessbar.Dispose();
                
               
            }
            list_盤點明細 = (from temp in list_盤點明細
                         where temp[(int)enum_定盤_盤點明細.異動量].ObjectToString() != "0"
                         select temp).ToList();
            this.sqL_DataGridView_定盤_盤點明細.RefreshGrid(list_盤點明細);
            this.Invoke(new Action(delegate
            {
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = true;
                comboBox_定盤_盤點明細_庫別.Enabled = false;
                plC_RJ_Button_定盤_盤點明細_確認更動庫存量.Enabled = true;
                this.Cursor = Cursors.Default;
            }));
        }
        private void PlC_RJ_Button_定盤_盤點明細_確認更動庫存量_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
            }));

            string 庫別 = "";
            this.Invoke(new Action(delegate
            {
                庫別 = comboBox_定盤_盤點明細_庫別.Text;
            }));
            if (庫別.StringIsEmpty() == true)
            {
                MyMessageBox.ShowDialog("未選擇庫別");
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));

                return;
            }
            List<object[]> list_盤點明細 = this.sqL_DataGridView_定盤_盤點明細.GetAllRows();
            List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            List<DeviceBasic> deviceBasics_replace = new List<DeviceBasic>();
            List<object[]> list_交易紀錄_Add = new List<object[]>();
            Function_從SQL取得儲位到本地資料();
            if (庫別 == "藥庫")
            {
                Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_盤點明細.Count);
                dialog_Prcessbar.State = "盤點明細計算...";
                for (int i = 0; i < list_盤點明細.Count; i++)
                {
                    dialog_Prcessbar.Value = i;
                    string 備註 = "";
                    string 藥碼 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥碼].ObjectToString();
                    list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端藥檔.藥品碼, 藥碼);
                    if (list_藥品資料_buf.Count > 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.藥名] = list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品名稱].ObjectToString();
                    }
                    string 藥名 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥名].ObjectToString();
                  
                    int 盤點量 = list_盤點明細[i][(int)enum_定盤_盤點明細.盤點量].StringToInt32();
                    int 消耗量 = list_盤點明細[i][(int)enum_定盤_盤點明細.庫存差異量].StringToInt32();
                    int 庫存量 = list_盤點明細[i][(int)enum_定盤_盤點明細.庫存量].StringToInt32();
                    int 異動量 = 0;
                    List<DeviceBasic> deviceBasics = this.List_藥庫_DeviceBasic.SortByCode(藥碼);
                    if (deviceBasics.Count == 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.庫存量] = "異常";
                        continue;
                    }
                    int 現有庫存量 = deviceBasics[0].Inventory.StringToInt32();
                    異動量 = (盤點量 - 庫存量) + 消耗量;
                    list_盤點明細[i][(int)enum_定盤_盤點明細.異動量] = 異動量.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] = "";
                    List<string> list_效期 = new List<string>();
                    List<string> list_批號 = new List<string>();
                    List<string> list_異動量 = new List<string>();
                    if (異動量 == 0)
                    {
                        //continue;
                    }
                    if (庫存量 == 0 || 異動量 > 0)
                    {

                        Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥碼, ref list_效期, ref list_批號);
                        if (list_效期.Count != 0)
                        {
                            if (list_批號[0].StringIsEmpty() == true) list_批號[0] = "無";
                            備註 += $"[效期]:{list_效期[0]},[批號]:{list_批號[0]}";
                            deviceBasics[0].效期庫存異動(list_效期[0], list_批號[0], 異動量.ToString());
                        }
                        else
                        {
                            備註 += $"[效期]:{"2000/12/31"},[批號]:{"系統代入"}";
                            deviceBasics[0].效期庫存異動("2000/12/31", "系統代入", 異動量.ToString());
                        }
                    }
                    else
                    {
                        deviceBasics[0].庫存異動(異動量, out list_效期, out list_批號, out list_異動量);
                        for (int k = 0; k < list_效期.Count; k++)
                        {
                            備註 += $"[效期]:{list_效期[k]},[批號]:{list_批號[k]}";
                            if (k != list_效期.Count - 1) 備註 += "\n";
                        }
                    }
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] += 備註;

                    transactionsClass transactionsClass = new transactionsClass();
                    transactionsClass.GUID = Guid.NewGuid().ToString();
                    transactionsClass.動作 = enum_交易記錄查詢動作.盤存盈虧.GetEnumName();
                    transactionsClass.藥品碼 = 藥碼;
                    transactionsClass.藥品名稱 = 藥名.ToString();
                    transactionsClass.庫存量 = 現有庫存量.ToString();
                    transactionsClass.交易量 = 異動量.ToString();
                    transactionsClass.結存量 = (現有庫存量 + 異動量).ToString();
                    transactionsClass.庫別 = "藥庫";
                    transactionsClass.操作人 = 登入者名稱;
                    transactionsClass.操作時間 = DateTime.Now.ToDateTimeString_6();
                    transactionsClass.備註 = $"{備註}";
                    object[] trading_value = transactionsClass.ClassToSQL<transactionsClass , enum_交易記錄查詢資料>();

                    list_交易紀錄_Add.Add(trading_value);
                    deviceBasics_replace.Add(deviceBasics[0]);
                }
                if (deviceBasics_replace.Count > 0) this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasics_replace);
                if (list_交易紀錄_Add.Count > 0) sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄_Add, false);
                dialog_Prcessbar.Close();
                dialog_Prcessbar.Dispose();

            }
            if (庫別 == "藥局")
            {
                Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_盤點明細.Count);
                dialog_Prcessbar.State = "盤點明細計算...";
                for (int i = 0; i < list_盤點明細.Count; i++)
                {
                    dialog_Prcessbar.Value = i;
                    string 備註 = "";
                    string 藥碼 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥碼].ObjectToString();
                    list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端藥檔.藥品碼, 藥碼);
                    if (list_藥品資料_buf.Count > 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.藥名] = list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品名稱].ObjectToString();
                    }
                    string 藥名 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥名].ObjectToString();                
                    int 盤點量 = list_盤點明細[i][(int)enum_定盤_盤點明細.盤點量].StringToInt32();
                    int 消耗量 = list_盤點明細[i][(int)enum_定盤_盤點明細.庫存差異量].StringToInt32();
                    int 庫存量 = list_盤點明細[i][(int)enum_定盤_盤點明細.庫存量].StringToInt32();
                    int 異動量 = 0;
                    List<DeviceBasic> deviceBasics = this.List_藥局_DeviceBasic.SortByCode(藥碼);
                    if (deviceBasics.Count == 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.庫存量] = "異常";
                        continue;
                    }
                    int 現有庫存量 = deviceBasics[0].Inventory.StringToInt32();
                    異動量 = (盤點量 - 庫存量) + 消耗量;
                    list_盤點明細[i][(int)enum_定盤_盤點明細.異動量] = 異動量.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] = "";
                    List<string> list_效期 = new List<string>();
                    List<string> list_批號 = new List<string>();
                    List<string> list_異動量 = new List<string>();
                    if (異動量 == 0)
                    {
                        //continue;
                    }
                    if (庫存量 == 0 || 異動量 > 0)
                    {

                        Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥碼, ref list_效期, ref list_批號);
                        if (list_效期.Count != 0)
                        {
                            if (list_批號[0].StringIsEmpty() == true) list_批號[0] = "無";
                            備註 += $"[效期]:{list_效期[0]},[批號]:{list_批號[0]}";
                            deviceBasics[0].效期庫存異動(list_效期[0], list_批號[0], 異動量.ToString());
                        }
                        else
                        {
                            備註 += $"[效期]:{"2000/12/31"},[批號]:{"系統代入"}";
                            deviceBasics[0].效期庫存異動("2000/12/31", "系統代入", 異動量.ToString());
                        }
                    }
                    else
                    {
                        deviceBasics[0].庫存異動(異動量, out list_效期, out list_批號, out list_異動量);
                        for (int k = 0; k < list_效期.Count; k++)
                        {
                            備註 += $"[效期]:{list_效期[k]},[批號]:{list_批號[k]}";
                            if (k != list_效期.Count - 1) 備註 += "\n";
                        }
                    }
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] += 備註;

                    transactionsClass transactionsClass = new transactionsClass();
                    transactionsClass.GUID = Guid.NewGuid().ToString();
                    transactionsClass.動作 = enum_交易記錄查詢動作.盤存盈虧.GetEnumName();
                    transactionsClass.藥品碼 = 藥碼;
                    transactionsClass.藥品名稱 = 藥名.ToString();
                    transactionsClass.庫存量 = 現有庫存量.ToString();
                    transactionsClass.交易量 = 異動量.ToString();
                    transactionsClass.結存量 = (現有庫存量 + 異動量).ToString();
                    transactionsClass.庫別 = "藥局";
                    transactionsClass.操作人 = 登入者名稱;
                    transactionsClass.操作時間 = DateTime.Now.ToDateTimeString_6();
                    transactionsClass.備註 = $"{備註}";
                    object[] trading_value = transactionsClass.ClassToSQL<transactionsClass, enum_交易記錄查詢資料>();

                    list_交易紀錄_Add.Add(trading_value);
                    deviceBasics_replace.Add(deviceBasics[0]);
                }
                if (deviceBasics_replace.Count > 0) this.DeviceBasicClass_藥局.SQL_ReplaceDeviceBasic(deviceBasics_replace);
                if (list_交易紀錄_Add.Count > 0) sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄_Add, false);
                dialog_Prcessbar.Close();
                dialog_Prcessbar.Dispose();

            }
            this.sqL_DataGridView_定盤_盤點明細.RefreshGrid(list_盤點明細);
            this.Invoke(new Action(delegate
            {
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = false;
                comboBox_定盤_盤點明細_庫別.Enabled = false;
                this.PlC_RJ_Button_定盤_盤點明細_重置作業_MouseDownEvent(null);
                this.Cursor = Cursors.Default;
                MyMessageBox.ShowDialog("定盤庫存調整完成!");
            }));
        }
        #endregion


    }
}
