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
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        private void sub_Program_盤點作業_新增盤點_Init()
        {
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.Init(this.sqL_DataGridView_藥庫_藥品資料);
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.Set_ColumnVisible(false, new enum_藥庫_藥品資料().GetEnumNames());
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.Set_ColumnVisible(true, enum_藥庫_藥品資料.藥品碼, enum_藥庫_藥品資料.藥品名稱, enum_藥庫_藥品資料.藥品群組, enum_藥庫_藥品資料.中文名稱, enum_藥庫_藥品資料.包裝單位);
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.DataGridRefreshEvent += SqL_DataGridView_盤點作業_新增盤點_藥品資料_DataGridRefreshEvent;
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.DataGridRowsChangeRefEvent += SqL_DataGridView_盤點作業_新增盤點_藥品資料_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.CheckedChangedEvent += SqL_DataGridView_盤點作業_新增盤點_藥品資料_CheckedChangedEvent;
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.RowDoubleClickEvent += SqL_DataGridView_盤點作業_新增盤點_藥品資料_RowDoubleClickEvent;

            this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.Init(this.sqL_DataGridView_藥庫_藥品資料);
            this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.Set_ColumnVisible(false, new enum_藥庫_藥品資料().GetEnumNames());
            this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.Set_ColumnVisible(true, enum_藥庫_藥品資料.藥品碼, enum_藥庫_藥品資料.藥品名稱, enum_藥庫_藥品資料.藥品群組, enum_藥庫_藥品資料.中文名稱, enum_藥庫_藥品資料.包裝單位);
            this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.DataGridRefreshEvent += SqL_DataGridView_盤點作業_新增盤點_已選擇藥品_DataGridRefreshEvent;


            this.plC_RJ_ComboBox_盤點作業_新增盤點_藥品資料_藥品群組.Enter += PlC_RJ_ComboBox_盤點作業_新增盤點_藥品資料_藥品群組_Enter;
            this.plC_RJ_Button_盤點作業_新增盤點_藥品資料_顯示全部.MouseDownEvent += PlC_RJ_Button_盤點作業_新增盤點_藥品資料_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_盤點作業_新增盤點_藥品資料_藥品名稱_搜尋.MouseDownEvent += PlC_RJ_Button_盤點作業_新增盤點_藥品資料_藥品名稱_搜尋_MouseDownEvent;
            this.plC_RJ_Button_盤點作業_新增盤點_藥品資料_藥品碼_搜尋.MouseDownEvent += PlC_RJ_Button_盤點作業_新增盤點_藥品資料_藥品碼_搜尋_MouseDownEvent;
            this.plC_RJ_Button_盤點作業_新增盤點_藥品資料_藥品群組_搜尋.MouseDownEvent += PlC_RJ_Button_盤點作業_新增盤點_藥品資料_藥品群組_搜尋_MouseDownEvent;
            this.plC_RJ_Button_盤點作業_新增盤點_藥品資料_選擇藥品.MouseDownEvent += PlC_RJ_Button_盤點作業_新增盤點_藥品資料_選擇藥品_MouseDownEvent;

            this.plC_RJ_Button_盤點作業_新增盤點_已選擇藥品_取消選擇.MouseDownEvent += PlC_RJ_Button_盤點作業_新增盤點_已選擇藥品_取消選擇_MouseDownEvent;
            this.plC_RJ_Button_盤點作業_新增盤點_已選擇藥品_開始盤點.MouseDownEvent += PlC_RJ_Button_盤點作業_新增盤點_已選擇藥品_開始盤點_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_盤點作業_新增盤點);
        }

        private bool flag_Program_盤點作業_新增盤點_Init = false;
        private void sub_Program_盤點作業_新增盤點()
        {
            if (this.plC_ScreenPage_Main.PageText == "盤點作業" && this.plC_ScreenPage_盤點作業.PageText == "新增盤點")
            {
                if (!flag_Program_盤點作業_新增盤點_Init)
                {
                    this.PlC_RJ_ComboBox_盤點作業_新增盤點_藥品資料_藥品群組_Enter(null, null);
                    flag_Program_盤點作業_新增盤點_Init = true;
                }
            }
            else
            {
                flag_Program_盤點作業_新增盤點_Init = false;
            }
        }

        #region Function
        private List<string> Function_盤點作業_新增盤點_取得正在盤點藥碼()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(5000);
            List<string> list_code = new List<string>();
            string json = Basic.Net.WEBApiGet(@"https://10.18.1.146/backend/InventoryApi");
            List<object[]> list_value = new List<object[]>();
            InventoryApi inventoryApi = json.JsonDeserializet<InventoryApi>();
            if (inventoryApi == null) return list_code;
            for (int i = 0; i < inventoryApi.data.Count; i++)
            {
                InventoryApi.dataClass dataClass = inventoryApi.data[i];
                if((enum_盤點狀態)(dataClass.audit.StringToInt32()) == enum_盤點狀態.正在盤點)
                {
                    for (int k = 0; k < dataClass.detail.Count; k++)
                    {
                        list_code.Add(dataClass.detail[k].code);
                    }
                }
            }      
            Console.WriteLine($"{Basic.Reflection.GetMethodName()},共{list_code.Count}筆‧耗時:{myTimer.ToString()}");
            return list_code;
        }
        #endregion
        #region Event
        private void PlC_RJ_Button_盤點作業_新增盤點_藥品資料_選擇藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取藥品!");
                return;
            }
            SqL_DataGridView_盤點作業_新增盤點_藥品資料_CheckedChangedEvent(list_value, -1);
        }
        private void PlC_RJ_ComboBox_盤點作業_新增盤點_藥品資料_藥品群組_Enter(object sender, EventArgs e)
        {
            plC_RJ_ComboBox_盤點作業_新增盤點_藥品資料_藥品群組.SetDataSource(Function_藥品資料_藥品群組_取得選單());
        }
        private void SqL_DataGridView_盤點作業_新增盤點_藥品資料_DataGridRefreshEvent()
        {
            List<string> list_code = this.Function_盤點作業_新增盤點_取得正在盤點藥碼();
            List<string> list_code_buf = new List<string>();
            List<object[]> list_已選擇藥品 = this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.GetAllRows();
            List<object[]> list_已選擇藥品_buf = new List<object[]>();
            for (int i = 0; i < this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.dataGridView.Rows.Count; i++)
            {
                string Code = this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.dataGridView.Rows[i].Cells[enum_藥庫_藥品資料.藥品碼.GetEnumName()].Value.ToString();
                list_code_buf = (from value in list_code
                                 where value == Code
                                 select value).ToList();
                list_已選擇藥品_buf = list_已選擇藥品.GetRows((int)enum_藥庫_藥品資料.藥品碼, Code);

                if (list_code_buf.Count > 0)
                {
                    this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Black;
                    this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                }
                if (list_已選擇藥品_buf.Count > 0)
                {
                    this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.Set_Checked(i, true);
                }
            }
        }
        private void SqL_DataGridView_盤點作業_新增盤點_已選擇藥品_DataGridRefreshEvent()
        {
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.RefreshGrid();
        }
        private void SqL_DataGridView_盤點作業_新增盤點_藥品資料_CheckedChangedEvent(List<object[]> RowsList, int index)
        {
            List<string> list_code = this.Function_盤點作業_新增盤點_取得正在盤點藥碼();
            List<object[]> RowsList_buf = new List<object[]>();
            for (int i = 0; i < list_code.Count; i++)
            {
                RowsList_buf = RowsList.GetRows((int)enum_藥庫_藥品資料.藥品碼, list_code[i]);
                if (RowsList_buf.Count > 0)
                {
                    RowsList.RemoveByGUID(RowsList_buf[0]);
                }
            }
            List<object[]> list_value = this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.GetAllRows();
            List<object[]> list_value_buf = new List<object[]>();
            for (int i = 0; i < RowsList.Count; i++)
            {
                string Code = RowsList[i][(int)enum_藥庫_藥品資料.藥品碼].ObjectToString();
                list_value_buf = list_value.GetRows((int)enum_藥庫_藥品資料.藥品碼, Code);
                if (list_value_buf.Count == 0)
                {
                    list_value.Add(RowsList[i]);
                }
            }
            this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.RefreshGrid(list_value);
        }
        private void SqL_DataGridView_盤點作業_新增盤點_藥品資料_RowDoubleClickEvent(object[] RowValue)
        {

            List<string> list_code = this.Function_盤點作業_新增盤點_取得正在盤點藥碼();
            List<object[]> RowsList_buf = new List<object[]>();
            for (int i = 0; i < list_code.Count; i++)
            {
                if(RowValue[(int)enum_藥庫_藥品資料.藥品碼].ObjectToString() == list_code[i])
                {
                    return;
                }
            }
            List<object[]> list_value = this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.GetAllRows();
            List<object[]> list_value_buf = new List<object[]>();
            string Code = RowValue[(int)enum_藥庫_藥品資料.藥品碼].ObjectToString();
            list_value_buf = list_value.GetRows((int)enum_藥庫_藥品資料.藥品碼, Code);
            if (list_value_buf.Count == 0)
            {
                list_value.Add(RowValue);
            }
            this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.RefreshGrid(list_value);
        }
        private void SqL_DataGridView_盤點作業_新增盤點_藥品資料_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            Finction_藥品資料_藥品群組_序號轉名稱(RowsList, (int)enum_藥庫_藥品資料.藥品群組);
        }
        private void PlC_RJ_Button_盤點作業_新增盤點_藥品資料_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_盤點作業_新增盤點_藥品資料_藥品碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥庫_藥品資料.藥品碼, rJ_TextBox_盤點作業_新增盤點_藥品資料_藥品碼.Text);
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_盤點作業_新增盤點_藥品資料_藥品名稱_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥庫_藥品資料.藥品名稱, rJ_TextBox_盤點作業_新增盤點_藥品資料_藥品名稱.Text);
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_盤點作業_新增盤點_藥品資料_藥品群組_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.SQL_GetAllRows(false);

            string[] strArray = myConvert.分解分隔號字串(plC_RJ_ComboBox_盤點作業_新增盤點_藥品資料_藥品群組.Texts, ".");
            if (strArray.Length <= 0) return;
            int 群組序號 = strArray[0].StringToInt32();
            if (群組序號 >= 1 && 群組序號 <= 20)
            {
                list_value = list_value.GetRows((int)enum_藥庫_藥品資料.藥品群組, 群組序號.ToString("00"));
            }
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.RefreshGrid(list_value);
        }

        private void PlC_RJ_Button_盤點作業_新增盤點_已選擇藥品_取消選擇_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.Get_All_Select_RowsValues();
            List<object[]> list_all_value = this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.GetAllRows();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            for (int i = 0; i < list_value.Count; i++)
            {
                list_all_value.RemoveByGUID(list_value[i]);
            }
            this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.RefreshGrid(list_all_value);
        }
        private void PlC_RJ_Button_盤點作業_新增盤點_已選擇藥品_開始盤點_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.GetAllRows();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            List<string> names = new List<string>();
            List<string> values = new List<string>();
            names.Add("user_id");
            values.Add(this.rJ_TextBox_登入者ID.Text);
            names.Add("code_all");
            List<InsertDrugApi_code_all> list_codeall = new List<InsertDrugApi_code_all>();
            for (int i = 0; i < list_value.Count; i++)
            {        
                list_codeall.Add(new InsertDrugApi_code_all(list_value[i][(int)enum_藥庫_藥品資料.藥品碼].ObjectToString()));
            }
        
            string json = list_codeall.JsonSerializationt();
            values.Add(json);

            string result = Basic.Net.WEBApiPost("https://10.18.1.146/backend/InsertDrugApi", names, values);
            this.sqL_DataGridView_盤點作業_新增盤點_藥品資料.ClearGrid();
            this.sqL_DataGridView_盤點作業_新增盤點_已選擇藥品.ClearGrid();

            InventoryApi_Result inventoryApi_Result = result.JsonDeserializet<InventoryApi_Result>();
            if (inventoryApi_Result == null) return;
            if(inventoryApi_Result.msg == "新增成功")
            {
                MyMessageBox.ShowDialog("新增盤點資料成功!");
            }
            else
            {
                MyMessageBox.ShowDialog("新增盤點資料失敗!");
            }
        }
        #endregion

   
    }
}
