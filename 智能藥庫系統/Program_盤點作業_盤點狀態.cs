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

      

        private void sub_Program_盤點作業_盤點狀態_Init()
        {
            this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.Init();
            this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.DataGridRefreshEvent += SqL_DataGridView_盤點作業_盤點狀態_盤點總表_DataGridRefreshEvent;
            this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.RowEnterEvent += SqL_DataGridView_盤點作業_盤點狀態_盤點總表_RowEnterEvent;

            this.sqL_DataGridView_盤點作業_盤點狀態_盤點明細.Init();

            this.plC_RJ_Button_盤點作業_盤點狀態_取得API.MouseDownEvent += PlC_RJ_Button_盤點作業_盤點狀態_取得API_MouseDownEvent;

            plC_UI_Init.Add_Method(sub_Program_盤點作業_盤點狀態);
        }




        private bool flag_Program_盤點作業_盤點狀態_Init = false;
        private void sub_Program_盤點作業_盤點狀態()
        {
            if (this.plC_ScreenPage_Main.PageText == "盤點作業" && this.plC_ScreenPage_盤點作業.PageText == "盤點狀態")
            {
                if(!flag_Program_盤點作業_盤點狀態_Init)
                {
                    this.PlC_RJ_Button_盤點作業_盤點狀態_取得API_MouseDownEvent(null);
                    flag_Program_盤點作業_盤點狀態_Init = true;
                }
            }
            else
            {
                flag_Program_盤點作業_盤點狀態_Init = false;
            }
        }


        #region Event
        private void SqL_DataGridView_盤點作業_盤點狀態_盤點總表_DataGridRefreshEvent()
        {
            string 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.dataGridView.Rows[i].Cells[(int)enum_盤點總表.狀態].Value.ToString();
                if (狀態 == enum_盤點狀態.正在盤點.GetEnumName())
                {
                    this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_盤點狀態.完成盤點.GetEnumName())
                {
                    this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void SqL_DataGridView_盤點作業_盤點狀態_盤點總表_RowEnterEvent(object[] RowValue)
        {
            string inventory_id = RowValue[(int)enum_盤點明細.inventory_id].ObjectToString();
            List<string> names = new List<string>();
            List<string> values = new List<string>();
            names.Add("inventory_id");
            values.Add(inventory_id);
            string result = Basic.Net.WEBApiPost("https://10.18.1.146/backend/ResultApi", names, values);
            UpdateDrugApi_Result resultApi = result.JsonDeserializet<UpdateDrugApi_Result>();

            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < resultApi.data.Count; i++)
            {
                UpdateDrugApi_Result.dataClass dataClass = resultApi.data[i];

                object[] value = new object[new enum_盤點明細().GetLength()];
                value[(int)enum_盤點明細.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_盤點明細.inventory_id] = dataClass.inventory_id;
                value[(int)enum_盤點明細.藥品碼] = dataClass.code;
                value[(int)enum_盤點明細.藥品名稱] = dataClass.name;
                value[(int)enum_盤點明細.中文名稱] = dataClass.chinese_name;
                value[(int)enum_盤點明細.單位] = dataClass.package;
                value[(int)enum_盤點明細.庫存量] = dataClass.inventory;
                value[(int)enum_盤點明細.盤點量] = dataClass.inventory_new;
                value[(int)enum_盤點明細.盤點差異] = (dataClass.inventory.StringToInt32() - dataClass.inventory_new.StringToInt32()).ToString();
                list_value.Add(value);
            }
            this.sqL_DataGridView_盤點作業_盤點狀態_盤點明細.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_盤點作業_盤點狀態_取得API_MouseDownEvent(MouseEventArgs mevent)
        {
            string json = Basic.Net.WEBApiGet(@"https://10.18.1.146/backend/InventoryApi");
            List<object[]> list_value = new List<object[]>();
            InventoryApi inventoryApi = json.JsonDeserializet<InventoryApi>();
            if (inventoryApi == null) return;
            for (int i = 0; i < inventoryApi.data.Count; i++)
            {
                InventoryApi.dataClass dataClass = inventoryApi.data[i];

                object[] value = new object[new enum_盤點總表().GetLength()];
                value[(int)enum_盤點總表.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_盤點總表.inventory_id] = dataClass.inventory_id;
                value[(int)enum_盤點總表.盤點編號] = dataClass.inventory_code;
                value[(int)enum_盤點總表.狀態] = (enum_盤點狀態)(dataClass.audit.StringToInt32());
                value[(int)enum_盤點總表.盤點人員] = dataClass.user_id;
                value[(int)enum_盤點總表.盤點開始時間] = dataClass.date_create;
                value[(int)enum_盤點總表.盤點結束時間] = dataClass.date_finish;
                list_value.Add(value);
            }
            list_value.Sort(new ICP_盤點狀態());
            this.sqL_DataGridView_盤點作業_盤點狀態_盤點總表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button1_MouseDownEvent(MouseEventArgs mevent)
        {
            //if (!Basic.Net.is_Chrome())
            //{
            //    return;
            //}
            //System.Diagnostics.Process.Start("chrome.exe", "https://10.18.1.146/backend/Login");


            List<string> names = new List<string>();
            List<string> values = new List<string>();
            List<medclass> medclasses = new List<medclass>();
            names.Add("inventory_id");
            values.Add("24");

            names.Add("audit");
            values.Add("1");

            medclasses.Add(new medclass("21229", "200"));
            names.Add("code_all");
            values.Add(medclasses.JsonSerializationt());

            string result = Basic.Net.WEBApiPost("https://10.18.1.146/backend/UpdateDrugApi", names, values);
            UpdateDrugApi_Result resultApi = result.JsonDeserializet<UpdateDrugApi_Result>();
        }
        #endregion

        public class ICP_盤點狀態 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime1 = x[(int)enum_盤點總表.盤點開始時間].StringToDateTime();
                DateTime datetime2 = y[(int)enum_盤點總表.盤點開始時間].StringToDateTime();
                int compare = DateTime.Compare(datetime2, datetime1);
                return compare;

            }
        }
      
    }
}
