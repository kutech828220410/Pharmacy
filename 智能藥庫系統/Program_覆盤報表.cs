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

    public enum enum_覆盤報表_覆盤量_匯入
    {
        藥碼,
        盤點量,
    }
    public partial class Form1 : Form
    {
        private void sub_Program_覆盤報表_Init()
        {
            this.plC_RJ_Button_覆盤報表_上傳Excel.MouseDownEvent += PlC_RJ_Button_覆盤報表_上傳Excel_MouseDownEvent;
            this.plC_RJ_Button_覆盤報表_匯入覆盤量.MouseDownEvent += PlC_RJ_Button_覆盤報表_匯入覆盤量_MouseDownEvent;
            this.plC_RJ_Button_覆盤報表_匯出Excel.MouseDownEvent += PlC_RJ_Button_覆盤報表_匯出Excel_MouseDownEvent;
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
            table.AddColumnList("覆盤量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("結存金額", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("誤差量", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("誤差金額", Table.StringType.VARCHAR, Table.IndexType.None);

            this.sqL_DataGridView_覆點報表.Init(table);
            this.sqL_DataGridView_覆點報表.Set_ColumnVisible(false, new enum_盤點定盤_Excel().GetEnumNames());
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "藥碼");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(400, DataGridViewContentAlignment.MiddleLeft, "藥名");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "單位");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "單價");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "庫存量");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "庫存金額");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "盤點量");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "庫存差異量");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "異動後結存量");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "消耗量");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "覆盤量");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "結存金額");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "誤差量");
            this.sqL_DataGridView_覆點報表.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, "誤差金額");

            plC_UI_Init.Add_Method(sub_Program_覆點報表);
        }

   

        private void sub_Program_覆點報表()
        {

        }



        #region Function
        private void Function_覆點報表_載入單價()
        {

            string MedPrice = Basic.Net.WEBApiGet($"{dBConfigClass.MedPrice_ApiURL}");
            List<class_MedPrice> class_MedPrices = MedPrice.JsonDeserializet<List<class_MedPrice>>();
            List<class_MedPrice> class_MedPrices_buf = new List<class_MedPrice>();
            List<object[]> list_value = this.sqL_DataGridView_覆點報表.GetAllRows();
            for (int i = 0; i < list_value.Count; i++)
            {
                string 藥碼 = list_value[i][(int)enum_盤點定盤_Excel.藥碼].ObjectToString();
                class_MedPrices_buf = (from temp in class_MedPrices
                                       where temp.藥品碼 == 藥碼
                                       select temp).ToList();
                if (class_MedPrices_buf.Count > 0)
                {
                    list_value[i][(int)enum_盤點定盤_Excel.單價] = class_MedPrices_buf[0].成本價;
                }
            }
            this.sqL_DataGridView_覆點報表.RefreshGrid(list_value);
        }
        #endregion
        #region Event
        private void PlC_RJ_Button_覆盤報表_上傳Excel_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;
            DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);

            dataTable = dataTable.ReorderTable(new enum_盤點覆盤_Excel());

            if (dataTable == null)
            {
                return;
            }
            List<object[]> list_value_load = dataTable.DataTableToRowList();

            sqL_DataGridView_覆點報表.RefreshGrid(list_value_load);


            Function_覆點報表_載入單價();
        }
        private void PlC_RJ_Button_覆盤報表_匯出Excel_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_覆點報表.GetAllRows();
            this.Invoke(new Action(delegate
            {
                DataTable dataTable = list_value.ToDataTable(new enum_盤點覆盤_Excel());
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    MyOffice.ExcelClass.NPOI_SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);

                }
            }));
            MyMessageBox.ShowDialog("完成!");
        }
        private void PlC_RJ_Button_覆盤報表_匯入覆盤量_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;
            DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            dataTable = dataTable.ReorderTable(new enum_覆盤報表_覆盤量_匯入());

            if (dataTable == null)
            {
                return;
            }
            string 藥碼 = "";
            List<object[]> list_覆點報表 = sqL_DataGridView_覆點報表.GetAllRows();
            List<object[]> list_覆點報表_buf = new List<object[]>();
            List<object[]> list_覆盤報表_覆盤量_匯入 = dataTable.DataTableToRowList();
            for (int i = 0; i < list_覆盤報表_覆盤量_匯入.Count; i++)
            {
                藥碼 = list_覆盤報表_覆盤量_匯入[i][(int)enum_覆盤報表_覆盤量_匯入.藥碼].ObjectToString();

                list_覆點報表_buf = list_覆點報表.GetRows((int)enum_盤點覆盤_Excel.藥碼, 藥碼);
                if (list_覆點報表_buf.Count > 0)
                {
                    object[] value = list_覆點報表_buf[0];
                    value[(int)enum_盤點覆盤_Excel.覆盤量] = list_覆盤報表_覆盤量_匯入[i][(int)enum_覆盤報表_覆盤量_匯入.盤點量].ObjectToString();
                }
                else
                {
                    object[] value = new object[new enum_盤點覆盤_Excel().GetLength()];
                    value[(int)enum_盤點覆盤_Excel.藥碼] = list_覆盤報表_覆盤量_匯入[i][(int)enum_覆盤報表_覆盤量_匯入.藥碼].ObjectToString();
                    value[(int)enum_盤點覆盤_Excel.覆盤量] = list_覆盤報表_覆盤量_匯入[i][(int)enum_覆盤報表_覆盤量_匯入.盤點量].ObjectToString();

                    list_覆點報表.Add(value);
                }
            }
            for (int i = 0; i < list_覆點報表.Count; i++)
            {
                if(list_覆點報表[i][(int)enum_盤點覆盤_Excel.覆盤量].ObjectToString().StringIsEmpty())
                {
                    list_覆點報表[i][(int)enum_盤點覆盤_Excel.覆盤量] = "0";
                }
            }
            sqL_DataGridView_覆點報表.RefreshGrid(list_覆點報表);
        }
        #endregion

    }
}
