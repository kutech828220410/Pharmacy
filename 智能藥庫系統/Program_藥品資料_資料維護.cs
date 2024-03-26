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
using System.Text.RegularExpressions;
using HIS_DB_Lib;
using SQLUI;
namespace 智能藥庫系統
{

    public partial class Main_Form : Form
    {
        //public enum enum_雲端藥檔
        //{
        //    GUID,
        //    藥品碼,
        //    中文名稱,
        //    藥品名稱,
        //    藥品學名,
        //    健保碼,
        //    包裝單位,
        //    包裝數量,
        //    最小包裝單位,
        //    最小包裝數量,
        //    藥品條碼1,
        //    藥品條碼2,
        //    警訊藥品,
        //    管制級別,
        //    類別
        //}
        public enum enum_雲端藥檔_匯出
        {
            藥品碼,
            中文名稱,
            藥品名稱,
            藥品學名,
            健保碼,
            包裝單位,
            包裝數量,
            最小包裝單位,
            最小包裝數量,
            藥品條碼1,
            藥品條碼2,
        }
        public enum enum_雲端藥檔_匯入
        {
            藥品碼,
            中文名稱,
            藥品名稱,
            藥品學名,
            健保碼,
            包裝單位,
            包裝數量,
            最小包裝單位,
            最小包裝數量,
            藥品條碼1,
            藥品條碼2,
        }
        public enum ContextMenuStrip_藥品資料_資料維護_本地藥檔
        {         
            列出DC藥品,
            列出異動藥品,
            搜尋選取藥品,
            刪除選取藥品,
            藥品群組設定,
        }
        public enum enum_藥品資料_資料維護_本地藥檔
        {
            GUID,
            藥品碼,
            中文名稱,
            藥品名稱,
            藥品學名,
            藥品群組,
            健保碼,
            包裝單位,
            包裝數量,
            最小包裝單位,
            最小包裝數量,
            藥品條碼1,
            藥品條碼2,
        }

  
        private void sub_Program_藥品資料_資料維護_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_雲端藥檔, dBConfigClass.DB_Medicine_Cloud);
            string url = $"{API_Server}/api/MED_page/init";
            returnData returnData = new returnData();
            returnData.ServerType = enum_ServerSetting_Type.藥庫.GetEnumName();
            returnData.ServerName = $"{"DS01"}";
            returnData.TableName = "medicine_page_cloud";
            string json_in = returnData.JsonSerializationt();
            string json = Basic.Net.WEBApiPostJson($"{url}", json_in);
            Table table = json.JsonDeserializet<Table>();
            if (table == null)
            {
                MyMessageBox.ShowDialog($"雲端藥檔表單建立失敗!! API_Server:{API_Server}");
                return;
            }
            this.sqL_DataGridView_雲端藥檔.Init(table);
            this.sqL_DataGridView_雲端藥檔.Set_ColumnVisible(true, new enum_雲端藥檔().GetEnumNames());
            this.sqL_DataGridView_雲端藥檔.DataGridRefreshEvent += SqL_DataGridView_雲端藥檔_DataGridRefreshEvent;

            this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.Init();
            if (!this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_CreateTable();
            }
            this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.DataGridRowsChangeEvent += SqL_DataGridView_藥品資料_資料維護_本地藥檔_DataGridRowsChangeEvent;
            this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.DataGridRefreshEvent += SqL_DataGridView_藥品資料_資料維護_本地藥檔_DataGridRefreshEvent;       

            this.plC_RJ_Button_雲端藥檔_搜尋.MouseDownEvent += PlC_RJ_Button_雲端藥檔_搜尋_MouseDownEvent;
            this.plC_RJ_Button_雲端藥檔_選取資料刪除.MouseDownEvent += PlC_RJ_Button_雲端藥檔_選取資料刪除_MouseDownEvent;
            this.plC_RJ_Button_雲端藥檔_匯入.MouseDownEvent += PlC_RJ_Button_雲端藥檔_匯入_MouseDownEvent;
            this.plC_RJ_Button_雲端藥檔_匯出.MouseDownEvent += PlC_RJ_Button_雲端藥檔_匯出_MouseDownEvent;
            this.plC_RJ_Button_雲端藥檔_全部顯示.MouseDownEvent += PlC_RJ_Button_雲端藥檔_全部顯示_MouseDownEvent;
            this.plC_RJ_Button_雲端藥檔_藥品條碼1設定.MouseDownEvent += PlC_RJ_Button_雲端藥檔_藥品條碼1設定_MouseDownEvent;
            this.plC_RJ_Button_雲端藥檔_藥品條碼2設定.MouseDownEvent += PlC_RJ_Button_雲端藥檔_藥品條碼2設定_MouseDownEvent;

            this.plC_RJ_Button_藥品資料_資料維護_本地藥檔_搜尋.MouseDownEvent += PlC_RJ_Button_藥品資料_資料維護_本地藥檔_搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥品資料_資料維護_本地藥檔_全部顯示.MouseDownEvent += PlC_RJ_Button_藥品資料_資料維護_本地藥檔_全部顯示_MouseDownEvent;
            this.plC_RJ_Button_藥品資料_資料維護_本地藥檔_選取資料刪除.MouseDownEvent += PlC_RJ_Button_藥品資料_資料維護_本地藥檔_選取資料刪除_MouseDownEvent;
            this.plC_RJ_Button_藥品資料_資料維護_本地藥檔_修正全部藥品.MouseDownEvent += PlC_RJ_Button_藥品資料_資料維護_本地藥檔_修正全部藥品_MouseDownEvent;
            this.plC_RJ_Button_藥品資料_資料維護_本地藥檔_藥品群組設定.MouseDownEvent += PlC_RJ_Button_藥品資料_資料維護_本地藥檔_藥品群組設定_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_藥品資料_資料維護);
        }

        private bool flag_藥品資料_藥品群組_資料維護 = false;
        private void sub_Program_藥品資料_資料維護()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥品資料" && this.plC_ScreenPage_藥品資料.PageText == "資料維護")
            {
                if (!this.flag_藥品資料_藥品群組_資料維護)
                {
                    this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(true);
                    this.flag_藥品資料_藥品群組_資料維護 = true;
                }
            }
            else
            {
                this.flag_藥品資料_藥品群組_資料維護 = false;
            }
        }
        #region Function

        #region 雲端藥檔
        private List<object[]> Function_雲端藥檔_列出異動藥品()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            List<object[]> list_value = this.Function_藥品資料_資料維護_本地藥檔_列出異動藥品(list_雲端藥檔, list_本地藥檔);
            return list_value;
        }
        private List<object[]> Function_雲端藥檔_列出異動藥品(List<object[]> list_雲端藥檔, List<object[]> list_本地藥檔)
        {
            List<object[]> list_雲端藥檔_buf = new List<object[]>();
            List<object[]> list_本地藥檔_buf = new List<object[]>();
            bool flag_IsEqual = false;
            for (int i = 0; i < list_本地藥檔.Count; i++)
            {
                list_雲端藥檔_buf = list_雲端藥檔.GetRows((int)enum_雲端藥檔.藥品碼, list_本地藥檔[i][(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if (list_雲端藥檔_buf.Count != 0)
                {
                    object[] value_dst = LINQ.CopyRow(list_本地藥檔[i], new enum_藥品資料_資料維護_本地藥檔(), new enum_藥品資料_資料維護_本地藥檔());
                    value_dst = LINQ.CopyRow(list_雲端藥檔_buf[0], new enum_雲端藥檔(), new enum_藥品資料_資料維護_本地藥檔());
                    flag_IsEqual = list_本地藥檔[i].IsEqual(value_dst, (int)enum_藥品資料_資料維護_本地藥檔.GUID, (int)enum_藥品資料_資料維護_本地藥檔.藥品群組);
                    if (!flag_IsEqual)
                    {
                        list_本地藥檔_buf.Add(value_dst);
                    }
                }
            }
            return list_本地藥檔_buf;
        }
        private List<object[]> Function_雲端藥檔_列出新增藥品()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            List<object[]> list_雲端藥檔_buf = new List<object[]>();
            List<object[]> list_本地藥檔_buf = new List<object[]>();
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < list_雲端藥檔.Count; i++)
            {
                list_本地藥檔_buf = list_本地藥檔.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, list_雲端藥檔[i][(int)enum_雲端藥檔.藥品碼].ObjectToString());
                if (list_本地藥檔_buf.Count == 0)
                {
                    list_value.Add(list_雲端藥檔[i]);
                }
            }
    
            return list_value;

        }
        #endregion
        #region 本地藥檔
        private List<object[]> Function_藥品資料_資料維護_本地藥檔_列出DC藥品()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            List<object[]> list_value = this.Function_藥品資料_資料維護_本地藥檔_列出DC藥品(list_雲端藥檔 , list_本地藥檔);
            return list_value;
        }
        private List<object[]> Function_藥品資料_資料維護_本地藥檔_列出DC藥品(List<object[]> list_雲端藥檔 , List<object[]> list_本地藥檔)
        {
       
            List<object[]> list_value = new List<object[]>();

            Parallel.ForEach(list_本地藥檔, value =>
            {
                List<object[]> list_雲端藥檔_buf = new List<object[]>();
                List<object[]> list_本地藥檔_buf = new List<object[]>();

                list_雲端藥檔_buf = list_雲端藥檔.GetRows((int)enum_雲端藥檔.藥品碼, value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if (list_雲端藥檔_buf.Count == 0)
                {
                    list_value.LockAdd(value);
                }
            });

        
            return list_value;
        }
        private List<object[]> Function_藥品資料_資料維護_本地藥檔_列出異動藥品()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            List<object[]> list_value = this.Function_藥品資料_資料維護_本地藥檔_列出異動藥品(list_雲端藥檔, list_本地藥檔);
            return list_value;
        }
        private List<object[]> Function_藥品資料_資料維護_本地藥檔_列出異動藥品(List<object[]> list_雲端藥檔, List<object[]> list_本地藥檔)
        {
    
            bool flag_IsEqual = false;
            List<object[]> list_本地藥檔_buf = new List<object[]>();
            Parallel.ForEach(list_本地藥檔, value =>
            {
                List<object[]> list_雲端藥檔_buf = new List<object[]>();
           
                list_雲端藥檔_buf = list_雲端藥檔.GetRows((int)enum_雲端藥檔.藥品碼, value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if (list_雲端藥檔_buf.Count != 0)
                {
                    object[] value_dst = LINQ.CopyRow(list_雲端藥檔_buf[0], new enum_雲端藥檔(), new enum_藥品資料_資料維護_本地藥檔());
                    flag_IsEqual = value.IsEqual(value_dst, (int)enum_藥品資料_資料維護_本地藥檔.GUID, (int)enum_藥品資料_資料維護_本地藥檔.藥品群組);
                    if (!flag_IsEqual)
                    {
                        value[(int)enum_藥品資料_資料維護_本地藥檔.藥品名稱] = list_雲端藥檔_buf[0][(int)enum_雲端藥檔.藥品名稱];
                        value[(int)enum_藥品資料_資料維護_本地藥檔.藥品學名] = list_雲端藥檔_buf[0][(int)enum_雲端藥檔.藥品學名];

                        list_本地藥檔_buf.LockAdd(value);
                    }
                }
            });
       
            return list_本地藥檔_buf;
        }
        private List<object[]> Function_藥品資料_資料維護_本地藥檔_列出新增藥品()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            List<object[]> list_value = this.Function_藥品資料_資料維護_本地藥檔_列出新增藥品(list_雲端藥檔, list_本地藥檔);
            return list_value;

        }
        private List<object[]> Function_藥品資料_資料維護_本地藥檔_列出新增藥品(List<object[]> list_雲端藥檔, List<object[]> list_本地藥檔)
        {
          
            List<object[]> list_雲端藥檔_新增藥品 = new List<object[]>();
            List<object[]> list_本地藥檔_新增藥品 = new List<object[]>();

            Parallel.ForEach(list_雲端藥檔, value =>
            {
                if (value == null)
                {

                }
                List<object[]> list_雲端藥檔_buf = new List<object[]>();
                List<object[]> list_本地藥檔_buf = new List<object[]>();
                list_本地藥檔_buf = list_本地藥檔.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, value[(int)enum_雲端藥檔.藥品碼].ObjectToString());
                if (list_本地藥檔_buf.Count == 0)
                {

                    list_雲端藥檔_新增藥品.LockAdd(value);
                }

            });

       
            for (int i = 0; i < list_雲端藥檔_新增藥品.Count; i++)
            {
                object[] value_dst = LINQ.CopyRow(list_雲端藥檔_新增藥品[i], new enum_雲端藥檔(), new enum_藥品資料_資料維護_本地藥檔());
                list_本地藥檔_新增藥品.Add(value_dst);
            }
            return list_本地藥檔_新增藥品;

        }
        private void Function_藥品資料_資料維護_本地藥檔_搜尋選取藥品()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.Get_All_Select_RowsValues();
            List<object[]> list_value = new List<object[]>();
            List<object[]> list_雲端藥檔_buf = new List<object[]>();
            List<object[]> list_本地藥檔_buf = new List<object[]>();

            for (int i = 0; i < list_本地藥檔.Count; i++)
            {
                list_雲端藥檔_buf = list_雲端藥檔.GetRows((int)enum_雲端藥檔.藥品碼, list_本地藥檔[i][(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if (list_雲端藥檔_buf.Count > 0)
                {
                    list_value.Add(list_雲端藥檔_buf[0]);
                }
            }
            sqL_DataGridView_雲端藥檔.RefreshGrid(list_value);
        }
        private DialogResult Function_藥品資料_資料維護_本地藥檔_藥品群組設定()
        {
            DialogResult dialogResult = DialogResult.None;
            string value = "";
            this.Invoke(new Action(delegate 
            {
                Dialog_ContextMenuStrip dialog_ContextMenuStrip = new Dialog_ContextMenuStrip(Function_藥品資料_藥品群組_取得選單());
                dialog_ContextMenuStrip.TitleText = "藥品群組設定";
                dialog_ContextMenuStrip.ControlsTextAlign = ContentAlignment.MiddleLeft;
                dialog_ContextMenuStrip.ControlsHeight = 40;
                dialogResult = dialog_ContextMenuStrip.ShowDialog();
                value = dialog_ContextMenuStrip.Value;
            }));

            if (dialogResult == DialogResult.Yes)
            {
                string[] strArray = myConvert.分解分隔號字串(value, ".");
                if (strArray.Length == 2)
                {
                    int 群組序號 = strArray[0].StringToInt32();
                    if (群組序號 >= 1 && 群組序號 <= 20)
                    {
                        List<object[]> list_Replace_Value = new List<object[]>();
                        List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.Get_All_Select_RowsValues();
                        for (int i = 0; i < list_本地藥檔.Count; i++)
                        {
                            list_本地藥檔[i][(int)enum_藥品資料_資料維護_本地藥檔.藥品群組] = 群組序號.ToString("00");
                            list_Replace_Value.Add(list_本地藥檔[i]);
                        }
                        this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_ReplaceExtra(list_Replace_Value, true);
                    }
                }

            }
            return dialogResult;
        }
        #endregion
        #endregion
        #region Event

        #region 雲端藥檔
        private void SqL_DataGridView_雲端藥檔_DataGridRefreshEvent()
        {
            List<object[]> list_新增藥品 = this.Function_雲端藥檔_列出新增藥品();
            List<object[]> list_新增藥品_buf = new List<object[]>();
            string 藥品碼 = "";
            for (int i = 0; i < this.sqL_DataGridView_雲端藥檔.dataGridView.Rows.Count; i++)
            {
                藥品碼 = this.sqL_DataGridView_雲端藥檔.dataGridView.Rows[i].Cells[(int)enum_雲端藥檔.藥品碼].Value.ObjectToString();
      
                list_新增藥品_buf = list_新增藥品.GetRows((int)enum_雲端藥檔.藥品碼, 藥品碼);
                if (list_新增藥品_buf.Count > 0)
                {
                    this.sqL_DataGridView_雲端藥檔.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_雲端藥檔.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }

            }
        }
        private void sqL_DataGridView_雲端藥檔_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                
            }
        }
        private void PlC_RJ_Button_雲端藥檔_選取資料刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (MyMessageBox.ShowDialog("是否刪除選取資料", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    List<object[]> list_value = this.sqL_DataGridView_雲端藥檔.Get_All_Select_RowsValues();
                    List<object> list_delete_serchValue = new List<object>();
                    for (int i = 0; i < list_value.Count; i++)
                    {
                        string GUID = list_value[i][(int)enum_雲端藥檔.GUID].ObjectToString();
                        list_delete_serchValue.Add(GUID);
                    }
                    this.sqL_DataGridView_雲端藥檔.SQL_DeleteExtra(enum_雲端藥檔.GUID.GetEnumName(), list_delete_serchValue, true);
                    this.Cursor = Cursors.Default;
                }
            }));
   
        }
        private void PlC_RJ_Button_雲端藥檔_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.saveFileDialog_SaveExcel.ShowDialog();
            }));
            if (dialogResult == DialogResult.OK)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.WaitCursor;
                }));
                DataTable dataTable = this.sqL_DataGridView_雲端藥檔.GetDataTable();
                dataTable = dataTable.ReorderTable(new enum_雲端藥檔_匯出());
                CSVHelper.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));
         
            }
        }
        private void PlC_RJ_Button_雲端藥檔_匯入_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult == DialogResult.OK)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.WaitCursor;
                }));
         
                DataTable dataTable = new DataTable();
                CSVHelper.LoadFile(this.openFileDialog_LoadExcel.FileName, 0, dataTable);
                DataTable datatable_buf = dataTable.ReorderTable(new enum_雲端藥檔_匯入());
                if (datatable_buf == null)
                {
                    this.Invoke(new Action(delegate
                    {
                        MyMessageBox.ShowDialog("匯入檔案,資料錯誤!");
                        this.Cursor = Cursors.Default;
                        
                    }));
                    return;
                }
                List<object[]> list_LoadValue = datatable_buf.DataTableToRowList();
                List<object[]> list_SQL_Value = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
                List<object[]> list_Add = new List<object[]>();
                List<object[]> list_Add_buf = new List<object[]>();
                List<object[]> list_Delete_ColumnName = new List<object[]>();
                List<object[]> list_Delete_SerchValue = new List<object[]>();
                List<string> list_Replace_SerchValue = new List<string>();
                List<object[]> list_Replace_Value = new List<object[]>();
                List<object[]> list_SQL_Value_buf = new List<object[]>();
                for (int i = 0; i < list_LoadValue.Count; i++)
                {
                    object[] value_load = list_LoadValue[i];
                    value_load = value_load.CopyRow(new enum_雲端藥檔_匯入(), new enum_雲端藥檔());
                    value_load[(int)enum_雲端藥檔.藥品碼] = this.Function_藥品碼檢查(value_load[(int)enum_雲端藥檔.藥品碼].ObjectToString());
                    list_SQL_Value_buf = list_SQL_Value.GetRows((int)enum_雲端藥檔.藥品碼, value_load[(int)enum_雲端藥檔.藥品碼].ObjectToString());
                    if (list_SQL_Value_buf.Count > 0)
                    {
                        object[] value_SQL = list_SQL_Value_buf[0];
                        value_load[(int)enum_雲端藥檔.GUID] = value_SQL[(int)enum_雲端藥檔.GUID];
                        value_load[(int)enum_雲端藥檔.包裝數量] = Regex.Replace(value_load[(int)enum_雲端藥檔.包裝數量].ObjectToString(), "[^0-9]", "");
                        bool flag_Equal = value_load.IsEqual(value_SQL);
                        if (!flag_Equal)
                        {
                            list_Replace_SerchValue.Add(value_load[(int)enum_雲端藥檔.GUID].ObjectToString());
                            list_Replace_Value.Add(value_load);
                        }
                    }
                    else
                    {
                        value_load[(int)enum_雲端藥檔.GUID] = Guid.NewGuid().ToString();
                        value_load[(int)enum_雲端藥檔.包裝數量] = Regex.Replace(value_load[(int)enum_雲端藥檔.包裝數量].ObjectToString(), "[^0-9]", "");
                        list_Add_buf = list_Add.GetRows((int)enum_雲端藥檔.藥品碼, value_load[(int)enum_雲端藥檔.藥品碼].ObjectToString());

                        if (list_Add_buf.Count == 0)
                        {
                            list_Add.Add(value_load);
                        }
                       
                    }
                }
                this.sqL_DataGridView_雲端藥檔.SQL_AddRows(list_Add, false);
                this.sqL_DataGridView_雲端藥檔.SQL_ReplaceExtra(enum_雲端藥檔.GUID.GetEnumName(), list_Replace_SerchValue, list_Replace_Value, false);
         
                this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(true);
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                    MyMessageBox.ShowDialog("匯入完成!");
                }));
            }
        }
        private void PlC_RJ_Button_雲端藥檔_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            if (!this.rJ_TextBox_雲端藥檔_藥品碼.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_雲端藥檔.藥品碼, this.rJ_TextBox_雲端藥檔_藥品碼.Text);
            }
            if (!this.rJ_TextBox_雲端藥檔_中文名稱.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_雲端藥檔.中文名稱, this.rJ_TextBox_雲端藥檔_中文名稱.Text);
            }
            if (!this.rJ_TextBox_雲端藥檔_中文名稱.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_雲端藥檔.中文名稱, this.rJ_TextBox_雲端藥檔_中文名稱.Text);
            }
            if (!this.rJ_TextBox_雲端藥檔_藥品名稱.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_雲端藥檔.藥品名稱, this.rJ_TextBox_雲端藥檔_藥品名稱.Text);
            }
            if (!this.rJ_TextBox_雲端藥檔_藥品學名.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_雲端藥檔.藥品學名, this.rJ_TextBox_雲端藥檔_藥品學名.Text);
            }
         
            this.sqL_DataGridView_雲端藥檔.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_雲端藥檔_選取資料寫入本地端_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.Get_All_Select_RowsValues();
            List<object[]> list_value_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            List<object[]> list_value_本地藥檔_buf = new List<object[]>();
            List<object[]> list_Add = new List<object[]>();
            List<object[]> list_Delete_ColumnName = new List<object[]>();
            List<object[]> list_Delete_SerchValue = new List<object[]>();
            string Replace_ColumnName = enum_藥品資料_資料維護_本地藥檔.GUID.GetEnumName();
            List<string> list_Replace_SerchValue = new List<string>();
            List<object[]> list_Replace_Value = new List<object[]>();
            string 藥品碼 = "";
            for (int i = 0; i < list_value_雲端藥檔.Count; i++)
            {
                object[] value = new object[new enum_藥品資料_資料維護_本地藥檔().GetEnumNames().Length];
                value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.藥品碼];
                value[(int)enum_藥品資料_資料維護_本地藥檔.中文名稱] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.中文名稱];
                value[(int)enum_藥品資料_資料維護_本地藥檔.藥品名稱] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.藥品名稱];
                value[(int)enum_藥品資料_資料維護_本地藥檔.藥品學名] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.藥品學名];
                value[(int)enum_藥品資料_資料維護_本地藥檔.健保碼] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.健保碼];
                value[(int)enum_藥品資料_資料維護_本地藥檔.包裝單位] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.包裝單位];
                value[(int)enum_藥品資料_資料維護_本地藥檔.包裝數量] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.包裝數量];
                value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.藥品碼];
                value[(int)enum_藥品資料_資料維護_本地藥檔.最小包裝單位] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.最小包裝單位];
                value[(int)enum_藥品資料_資料維護_本地藥檔.最小包裝數量] = list_value_雲端藥檔[i][(int)enum_雲端藥檔.最小包裝數量];

                藥品碼 = value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString();
                list_value_本地藥檔_buf = list_value_本地藥檔.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, 藥品碼);
                if (list_value_本地藥檔_buf.Count == 0)
                {
                    value[(int)enum_藥品資料_資料維護_本地藥檔.GUID] = Guid.NewGuid().ToString();
                    list_Add.Add(value);
                }
                else
                {
                    bool flag_replace = false;
                    for (int k = 0; k < new enum_藥品資料_資料維護_本地藥檔().GetEnumNames().Length; k++)
                    {
                        if (k == (int)enum_藥品資料_資料維護_本地藥檔.GUID) continue;
                        if (list_value_本地藥檔_buf[0][k].ObjectToString() != value[k].ObjectToString())
                        {
                            flag_replace = true;
                            break;
                        }
                    }
                    if (flag_replace)
                    {
                        value[(int)enum_藥品資料_資料維護_本地藥檔.GUID] = list_value_本地藥檔_buf[i][(int)enum_藥品資料_資料維護_本地藥檔.GUID];
                        list_Replace_SerchValue.Add(value[(int)enum_藥品資料_資料維護_本地藥檔.GUID].ObjectToString());
                        list_Replace_Value.Add(value);
                    }
                }
            }
            sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_AddRows(list_Add, false);
            sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_ReplaceExtra(Replace_ColumnName, list_Replace_SerchValue, list_Replace_Value, false);
            sqL_DataGridView_藥品資料_資料維護_本地藥檔.ClearGrid();

            this.PlC_RJ_Button_雲端藥檔_列出新增藥品_MouseDownEvent(null);
        }
        private void PlC_RJ_Button_雲端藥檔_列出新增藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_雲端藥檔_列出新增藥品();
            sqL_DataGridView_雲端藥檔.RefreshGrid(list_value);
            
        }
        private void PlC_RJ_Button_雲端藥檔_列出異動藥品_MouseDownEvent(MouseEventArgs mevent)
        {

        }
        private void PlC_RJ_Button_雲端藥檔_全部顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            sqL_DataGridView_雲端藥檔.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_雲端藥檔_藥品條碼1設定_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_雲端藥檔.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取資料!");
                }));
                return;
            }
            this.Invoke(new Action(delegate
            {
                Dialog_TextBox dialog_TextBox = new Dialog_TextBox("藥品條碼1");
                if (dialog_TextBox.ShowDialog() == DialogResult.Yes)
                {
                    list_value[0][(int)enum_雲端藥檔.藥品條碼1] = dialog_TextBox.Value;
                    this.sqL_DataGridView_雲端藥檔.SQL_Replace(list_value[0], true);
                }
            }));


        }
        private void PlC_RJ_Button_雲端藥檔_藥品條碼2設定_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_雲端藥檔.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取資料!");
                }));
                return;
            }
            this.Invoke(new Action(delegate
            {
                Dialog_TextBox dialog_TextBox = new Dialog_TextBox("藥品條碼2");
                if (dialog_TextBox.ShowDialog() == DialogResult.Yes)
                {
                    list_value[0][(int)enum_雲端藥檔.藥品條碼2] = dialog_TextBox.Value;
                    this.sqL_DataGridView_雲端藥檔.SQL_Replace(list_value[0], true);
                }
            }));
        }
        #endregion
        #region 本地藥檔
        private void plC_RJ_ComboBox_藥品資料_資料維護_本地藥檔_Enter(object sender, EventArgs e)
        {
            plC_RJ_ComboBox_藥品資料_資料維護_本地藥檔_藥品群組.SetDataSource(this.Function_藥品資料_藥品群組_取得選單());
        }
        private void SqL_DataGridView_藥品資料_資料維護_本地藥檔_DataGridRowsChangeEvent(List<object[]> RowsList)
        {
            this.Finction_藥品資料_藥品群組_序號轉名稱(RowsList, (int)enum_藥品資料_資料維護_本地藥檔.藥品群組);
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            List<object[]> list_新增藥品 = this.Function_藥品資料_資料維護_本地藥檔_列出新增藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_異動藥品 = this.Function_藥品資料_資料維護_本地藥檔_列出異動藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_DC藥品 = this.Function_藥品資料_資料維護_本地藥檔_列出DC藥品(list_雲端藥檔, list_本地藥檔);

            if (list_新增藥品.Count > 0)
            {
                for (int i = 0; i < list_新增藥品.Count; i++)
                {
                    if (RowsList.RemoveByGUID(list_新增藥品[i]) > 0 || true)
                    {
                        RowsList.Insert(0, list_新增藥品[i]);
                    }             
                }
            }
            if (list_異動藥品.Count > 0)
            {
                for (int i = 0; i < list_異動藥品.Count; i++)
                {
                    if (RowsList.RemoveByGUID(list_異動藥品[i]) > 0)
                    {
                        RowsList.Insert(0, list_異動藥品[i]);
                    }
                }
            }
            if (list_DC藥品.Count > 0)
            {
                for (int i = 0; i < list_DC藥品.Count; i++)
                {
                    if (RowsList.RemoveByGUID(list_DC藥品[i]) > 0)
                    {
                        RowsList.Insert(0, list_DC藥品[i]);
                    }                
                }            
            }
 
        }
        private void SqL_DataGridView_藥品資料_資料維護_本地藥檔_DataGridRefreshEvent()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);

            List<object[]> list_新增藥品 = this.Function_藥品資料_資料維護_本地藥檔_列出新增藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_異動藥品 = this.Function_藥品資料_資料維護_本地藥檔_列出異動藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_DC藥品 = this.Function_藥品資料_資料維護_本地藥檔_列出DC藥品(list_雲端藥檔, list_本地藥檔);

  

            bool flag_IsEqual = false;
            List<DataGridViewRow> dataGridViewRows = new List<DataGridViewRow>();
            for (int i = 0; i < this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows.Count; i++)
            {
                dataGridViewRows.LockAdd(this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows[i]);
            }
            Parallel.ForEach(dataGridViewRows, row =>
            {
                List<object[]> list_新增藥品_buf = new List<object[]>();
                List<object[]> list_異動藥品_buf = new List<object[]>();
                List<object[]> list_DC藥品_buf = new List<object[]>();
                bool flag = false;
                object[] value = new object[row.Cells.Count];
                for (int k = 0; k < value.Length; k++)
                {
                    value[k] = row.Cells[k].Value.ObjectToString();
                }
                list_新增藥品_buf = list_新增藥品.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if (list_新增藥品_buf.Count > 0 && !flag)
                {
                    row.DefaultCellStyle.BackColor = Color.Lime;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    flag = true;
                }

                list_異動藥品_buf = list_異動藥品.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if (list_異動藥品_buf.Count > 0 && !flag)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    flag = true;
                }

                list_DC藥品_buf = list_DC藥品.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if (list_DC藥品_buf.Count > 0 && !flag)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    flag = true;
                }
            });
            //for (int i = 0; i < this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows.Count; i++)
            //{
            //    object[] value = new object[this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows[i].Cells.Count];
            //    for (int k = 0; k < value.Length; k++)
            //    {
            //        value[k] = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows[i].Cells[k].Value.ObjectToString();
            //    }

            //    list_新增藥品_buf = list_新增藥品.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
            //    if (list_新增藥品_buf.Count > 0)
            //    {
            //        this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
            //        this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            //        continue;
            //    }

            //    list_異動藥品_buf = list_異動藥品.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
            //    if (list_異動藥品_buf.Count > 0)
            //    {
            //        this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
            //        this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            //        continue;
            //    }

            //    list_DC藥品_buf = list_DC藥品.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
            //    if (list_DC藥品_buf.Count > 0)
            //    {
            //        this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            //        this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
            //        continue;
            //    }
            //}
        }
        private void sqL_DataGridView_藥品資料_資料維護_本地藥檔_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Dialog_ContextMenuStrip dialog_ContextMenuStrip = new Dialog_ContextMenuStrip(new ContextMenuStrip_藥品資料_資料維護_本地藥檔().GetEnumNames());
                dialog_ContextMenuStrip.TitleText = "本地藥檔功能";

                if (dialog_ContextMenuStrip.ShowDialog() == DialogResult.Yes)
                {
                    if (dialog_ContextMenuStrip.Value == ContextMenuStrip_藥品資料_資料維護_本地藥檔.列出DC藥品.GetEnumName())
                    {
                        sqL_DataGridView_藥品資料_資料維護_本地藥檔.RefreshGrid(this.Function_藥品資料_資料維護_本地藥檔_列出DC藥品());
                    }
                    else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_藥品資料_資料維護_本地藥檔.列出異動藥品.GetEnumName())
                    {
                        List<object[]> list_本地藥檔 = this.Function_藥品資料_資料維護_本地藥檔_列出異動藥品();
                        List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
                        List<object[]> list_雲端藥檔_buf = new List<object[]>();
                        List<object[]> list_value = new List<object[]>();
                        for (int i = 0; i < list_本地藥檔.Count; i++)
                        {
                            list_雲端藥檔_buf = list_雲端藥檔.GetRows((int)enum_雲端藥檔.藥品碼, list_本地藥檔[i][(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                            if (list_雲端藥檔_buf.Count > 0) list_value.Add(list_雲端藥檔_buf[0]);
                        }

                        sqL_DataGridView_藥品資料_資料維護_本地藥檔.RefreshGrid(list_本地藥檔);
                        sqL_DataGridView_雲端藥檔.RefreshGrid(list_value);
                    }
                    else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_藥品資料_資料維護_本地藥檔.搜尋選取藥品.GetEnumName())
                    {
                        this.Function_藥品資料_資料維護_本地藥檔_搜尋選取藥品();
                    }
                    else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_藥品資料_資料維護_本地藥檔.刪除選取藥品.GetEnumName())
                    {
                        this.PlC_RJ_Button_藥品資料_資料維護_本地藥檔_選取資料刪除_MouseDownEvent(null);
                    }
                    else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_藥品資料_資料維護_本地藥檔.藥品群組設定.GetEnumName())
                    {

                 
                    }
                }
            }
        }
        private void PlC_RJ_Button_藥品資料_資料維護_本地藥檔_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
           
            List<object[]> list_value = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            if (!this.rJ_TextBox_藥品資料_資料維護_本地藥檔_藥品碼.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, this.rJ_TextBox_藥品資料_資料維護_本地藥檔_藥品碼.Text);
            }
            if (!this.rJ_TextBox_藥品資料_資料維護_本地藥檔_中文名稱.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_藥品資料_資料維護_本地藥檔.中文名稱, this.rJ_TextBox_藥品資料_資料維護_本地藥檔_中文名稱.Text);
            }
            if (!this.rJ_TextBox_藥品資料_資料維護_本地藥檔_中文名稱.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_藥品資料_資料維護_本地藥檔.中文名稱, this.rJ_TextBox_藥品資料_資料維護_本地藥檔_中文名稱.Text);
            }
            if (!this.rJ_TextBox_藥品資料_資料維護_本地藥檔_藥品名稱.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_藥品資料_資料維護_本地藥檔.藥品名稱, this.rJ_TextBox_藥品資料_資料維護_本地藥檔_藥品名稱.Text);
            }
            if (!this.rJ_TextBox_藥品資料_資料維護_本地藥檔_藥品學名.Text.StringIsEmpty())
            {
                list_value = list_value.GetRowsByLike((int)enum_藥品資料_資料維護_本地藥檔.藥品學名, this.rJ_TextBox_藥品資料_資料維護_本地藥檔_藥品學名.Text);
            }
            if (plC_RJ_ChechBox_藥品資料_資料維護_本地藥檔_藥品群組.Checked)
            {
                string[] strArray = myConvert.分解分隔號字串(plC_RJ_ComboBox_藥品資料_資料維護_本地藥檔_藥品群組.Texts, ".");
                int 群組序號 = strArray[0].StringToInt32();
                if (群組序號 >= 1 && 群組序號 <= 20)
                {
                    list_value = list_value.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品群組, 群組序號.ToString("00"));
                }
            }
            this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥品資料_資料維護_本地藥檔_全部顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_藥品資料_資料維護_本地藥檔_選取資料刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate 
            {
                dialogResult = MyMessageBox.ShowDialog("是否刪除選取資料", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel);
            }));
            if (dialogResult == DialogResult.Yes)
            {
                List<object[]> list_value = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.Get_All_Select_RowsValues();
                List<object> list_delete_serchValue = new List<object>();
                for (int i = 0; i < list_value.Count; i++)
                {
                    string GUID = list_value[i][(int)enum_藥品資料_資料維護_本地藥檔.GUID].ObjectToString();
                    list_delete_serchValue.Add(GUID);
                }
                this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_DeleteExtra(enum_藥品資料_資料維護_本地藥檔.GUID.GetEnumName(), list_delete_serchValue, true);
            }
        }
        private void PlC_RJ_Button_藥品資料_資料維護_本地藥檔_修正全部藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);         
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端藥檔.SQL_GetAllRows(false);
            Console.WriteLine($"取得雲端藥檔 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            Console.WriteLine($"取得本地藥檔 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
            List<object[]> list_新增藥品 = this.Function_藥品資料_資料維護_本地藥檔_列出新增藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_異動藥品 = this.Function_藥品資料_資料維護_本地藥檔_列出異動藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_DC藥品 = this.Function_藥品資料_資料維護_本地藥檔_列出DC藥品(list_雲端藥檔, list_本地藥檔);

            List<object[]> list_新增藥品_buf = new List<object[]>();
            List<object[]> list_異動藥品_buf = new List<object[]>();
            List<object[]> list_DC藥品_buf = new List<object[]>();

            List<object[]> list_AddValue_buf = new List<object[]>();
            List<object[]> list_ReplaceValue_buf = new List<object[]>();
            List<object[]> list_Delete_buf = new List<object[]>();

            List<object[]> list_value = list_本地藥檔;
            for (int i = 0; i < list_value.Count; i++)
            {
                list_新增藥品_buf = list_新增藥品.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, list_value[i][(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if(list_新增藥品_buf.Count == 1)
                {
                    list_AddValue_buf.Add(list_新增藥品_buf[0]);
                    continue;
                }
                list_異動藥品_buf = list_異動藥品.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, list_value[i][(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if (list_異動藥品_buf.Count > 0)
                {
                    list_異動藥品_buf[0][(int)enum_藥品資料_資料維護_本地藥檔.GUID] = list_value[i][(int)enum_藥品資料_資料維護_本地藥檔.GUID];
                    list_ReplaceValue_buf.Add(list_異動藥品_buf[0]);
                    continue;
                }

                list_DC藥品_buf = list_DC藥品.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, list_value[i][(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                if (list_DC藥品_buf.Count > 0)
                {
                    list_Delete_buf.Add(list_DC藥品_buf[0]);
                    continue;
                }
            }
            for (int i = 0; i < list_AddValue_buf.Count; i++)
            {
                list_AddValue_buf[i][(int)enum_藥品資料_資料維護_本地藥檔.GUID] = Guid.NewGuid().ToString();
            }
            this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_AddRows(list_AddValue_buf, false);
            this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_ReplaceExtra(list_ReplaceValue_buf, false);
            this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_DeleteExtra(list_Delete_buf, false);
            this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(true);

        }
        private void PlC_RJ_Button_藥品資料_資料維護_本地藥檔_藥品群組設定_MouseDownEvent(MouseEventArgs mevent)
        {
            Function_藥品資料_資料維護_本地藥檔_藥品群組設定();
        }

        #endregion
        #endregion
    }


}
