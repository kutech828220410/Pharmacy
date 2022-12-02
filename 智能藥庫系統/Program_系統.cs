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
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        #region 特殊輸出表
        public enum enum_特殊輸出表
        {
            GUID,
            Name,
            IP,
            Num,
            輸出位置,
            輸出狀態
        }
        enum enum_特殊輸出表_匯出
        {
            IP,
            Name,
            Num,
            輸出位置,
        }
        enum enum_特殊輸出表_匯入
        {
            IP,
            Name,
            Num,
            輸出位置,
        }
        public enum ContextMenuStrip_特殊輸出表
        {
            匯出,
            匯入,
            刷新,
            刪除選取資料,
        }
        #endregion
        public enum enum_補給驗收入庫
        {
            GUID,
            藥品碼,
            數量,
            效期,
            批號,
            驗收時間,
            加入時間,
            狀態,
            來源,
            備註,
            
        }
  
        private void sub_Program_系統_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_寫入報表設定, dBConfigClass.DB_posting_server);
            this.sqL_DataGridView_寫入報表設定.Init();

            this.sqL_DataGridView_特殊輸出表.Init();
            if (!this.sqL_DataGridView_特殊輸出表.SQL_IsTableCreat()) this.sqL_DataGridView_特殊輸出表.SQL_CreateTable();
            this.sqL_DataGridView_特殊輸出表.MouseDown += SqL_DataGridView_特殊輸出表_MouseDown;

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_補給驗收入庫, dBConfigClass.DB_posting_server);           
            this.sqL_DataGridView_補給驗收入庫.Init();
            if (!this.sqL_DataGridView_補給驗收入庫.SQL_IsTableCreat()) this.sqL_DataGridView_補給驗收入庫.SQL_CreateTable();
            

            this.plC_UI_Init.Add_Method(this.sub_Program_系統);
        }

        private void sub_Program_系統()
        {

        }

        #region Fucntion

        #endregion
        #region Event
        private void SqL_DataGridView_特殊輸出表_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) return;
            Dialog_ContextMenuStrip dialog_ContextMenuStrip = new Dialog_ContextMenuStrip(new ContextMenuStrip_特殊輸出表());
            if (dialog_ContextMenuStrip.ShowDialog() != DialogResult.Yes) return;
            if (dialog_ContextMenuStrip.Value == ContextMenuStrip_特殊輸出表.匯出.GetEnumName())
            {
                saveFileDialog_SaveExcel.OverwritePrompt = false;
                if (saveFileDialog_SaveExcel.ShowDialog(this) == DialogResult.OK)
                {
                    DataTable datatable = new DataTable();
                    datatable = sqL_DataGridView_特殊輸出表.GetDataTable();
                    datatable = datatable.ReorderTable(new enum_特殊輸出表_匯出());
                    CSVHelper.SaveFile(datatable, saveFileDialog_SaveExcel.FileName);
                    MyMessageBox.ShowDialog("匯出完成!");
                }
            }
            else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_特殊輸出表.匯入.GetEnumName())
            {
                if (openFileDialog_LoadExcel.ShowDialog(this) == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataTable dataTable = new DataTable();
                    CSVHelper.LoadFile(this.openFileDialog_LoadExcel.FileName, 0, dataTable);
                    DataTable datatable_buf = dataTable.ReorderTable(new enum_特殊輸出表_匯入());
                    if (datatable_buf == null)
                    {
                        MyMessageBox.ShowDialog("匯入檔案,資料錯誤!");
                        return;
                    }
                    List<object[]> list_LoadValue = datatable_buf.DataTableToRowList();
                    List<object[]> list_SQL_Value = this.sqL_DataGridView_特殊輸出表.SQL_GetAllRows(false);
                    List<object[]> list_Add = new List<object[]>();
                    List<object[]> list_Delete_ColumnName = new List<object[]>();
                    List<object[]> list_Delete_SerchValue = new List<object[]>();
                    List<string> list_Replace_SerchValue = new List<string>();
                    List<object[]> list_Replace_Value = new List<object[]>();
                    List<object[]> list_SQL_Value_buf = new List<object[]>();

                    for (int i = 0; i < list_LoadValue.Count; i++)
                    {
                        object[] value_load = list_LoadValue[i];
                        value_load = value_load.CopyRow(new enum_特殊輸出表_匯入(), new enum_特殊輸出表());
                        list_SQL_Value_buf = list_SQL_Value.GetRows((int)enum_特殊輸出表.IP, value_load[(int)enum_特殊輸出表.IP].ObjectToString());
                        value_load[(int)enum_特殊輸出表.輸出狀態] = false.ToString();
                        if (list_SQL_Value_buf.Count > 0)
                        {
                            object[] value_SQL = list_SQL_Value_buf[0];
                            bool flag_Equal = value_load.IsEqual(value_SQL);
                            if (!flag_Equal)
                            {
                                list_Replace_Value.Add(value_load);
                            }
                        }
                        else
                        {
                            value_load[(int)enum_特殊輸出表.GUID] = Guid.NewGuid().ToString();
                            list_Add.Add(value_load);
                        }
                    }
                    this.sqL_DataGridView_特殊輸出表.SQL_AddRows(list_Add, false);
                    this.sqL_DataGridView_特殊輸出表.SQL_ReplaceExtra(list_Replace_Value, false);
                    this.sqL_DataGridView_特殊輸出表.SQL_GetAllRows(true);
                    this.Cursor = Cursors.Default;
                }
                this.Cursor = Cursors.Default;
                MyMessageBox.ShowDialog("匯入完成!");
            }
            else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_特殊輸出表.刪除選取資料.GetEnumName())
            {
                if (MyMessageBox.ShowDialog("是否刪除選取資料", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    List<object[]> list_value = this.sqL_DataGridView_特殊輸出表.Get_All_Select_RowsValues();
                    List<object> list_delete_serchValue = new List<object>();
                    for (int i = 0; i < list_value.Count; i++)
                    {
                        string GUID = list_value[i][(int)enum_特殊輸出表.GUID].ObjectToString();
                        list_delete_serchValue.Add(GUID);
                    }
                    this.sqL_DataGridView_特殊輸出表.SQL_DeleteExtra(list_delete_serchValue, true);
                    this.Cursor = Cursors.Default;
                }
            }
            else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_特殊輸出表.刷新.GetEnumName())
            {
                this.sqL_DataGridView_特殊輸出表.SQL_GetAllRows(true);
            }
        }
        #endregion
    }
}
