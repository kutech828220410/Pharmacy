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

namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        enum enum_藥庫_撥補_藥局_緊急申領_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
        }
        enum enum_藥庫_撥補_藥局_緊急申領
        {
            GUID,
            藥局代碼,
            藥品碼,
            藥品名稱,
            庫存,
            異動量,
            結存量,
            產出時間,
            過帳時間,
            狀態,
            備註,
        }

        private void sub_Program_藥庫_撥補_藥局_緊急申領_Init()
        {
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Init();
            if (!this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_IsTableCreat()) this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_CreateTable();
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.DataGridRefreshEvent += SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRefreshEvent;
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.DataGridRowsChangeRefEvent += SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRowsChangeRefEvent;

            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_顯示資料.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_顯示資料_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_撥補選取資料.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_撥補選取資料_MouseDownEvent;

            this.plC_UI_Init.Add_Method(this.sub_Program_藥庫_撥補_藥局_緊急申領);
        }

     

        private void sub_Program_藥庫_撥補_藥局_緊急申領()
        {

        }

        #region Function

        #endregion
        #region Event
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_顯示資料_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_GetRowsByBetween((int)enum_藥庫_撥補_藥局_緊急申領.產出時間, rJ_DatePicker_藥庫_撥補_藥局_緊急申領_產出日期_起始, rJ_DatePicker_藥庫_撥補_藥局_緊急申領_產出日期_結束, true);
        }
        private void SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_藥庫_撥補_藥局_緊急申領());
        }
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_撥補選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Get_All_Select_RowsValues();

            list_value = (from value in list_value
                          where value[(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() != enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()
                          select value).ToList();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取有效資料!");
            }

            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_交易紀錄_Add = new List<object[]>();
            List<DeviceBasic> deviceBasics_藥庫 = DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
            List<DeviceBasic> deviceBasics_藥庫_replace = new List<DeviceBasic>();
            List<DeviceBasic> deviceBasics_藥庫_buf = new List<DeviceBasic>();

            List<DeviceBasic> deviceBasics_藥局 = DeviceBasicClass_藥局.SQL_GetAllDeviceBasic();
            List<DeviceBasic> deviceBasics_藥局_replace = new List<DeviceBasic>();
            List<DeviceBasic> deviceBasics_藥局_buf = new List<DeviceBasic>();

            if (list_value.Count == 0) return;
            this.Function_從SQL取得儲位到本地資料();
            List<object[]> list_儲位資料 = new List<object[]>();
            List<Storage> storages_藥庫_replace = new List<Storage>();
            List<Storage> storages_藥庫_buf = new List<Storage>();
            string 藥品碼 = "";
            string 藥品名稱 = "";
            int 來源庫存量 = 0;
            int 來源異動量 = 0;
            int 來源結存量 = 0;
            string 來源備註 = "";

            int 輸出庫存量 = 0;
            int 輸出異動量 = 0;
            int 輸出結存量 = 0;
            string 輸出備註 = "";
            string 儲位資訊_GUID = "";
            string 儲位資訊_IP = "";
            string 儲位資訊_效期 = "";
            string 儲位資訊_批號 = "";
            int 儲位資訊_異動量 = 0;
            bool flag_部分撥發 = false;
            Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_value.Count);
            dialog_Prcessbar.State = "開始撥補...";
            for (int i = 0; i < list_value.Count; i++)
            {
                dialog_Prcessbar.Value = i;
                if (list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() == enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName())
                {
                    continue;
                }
                flag_部分撥發 = false;

                藥品碼 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString();
                藥品名稱 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品名稱].ObjectToString();
                deviceBasics_藥局_buf = deviceBasics_藥局.SortByCode(藥品碼);
                if (deviceBasics_藥局.Count == 0) continue;

                來源庫存量 = this.Function_從本地資料取得庫存(藥品碼);
                輸出異動量 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.異動量].ObjectToString().StringToInt32();
                if (來源庫存量 + (輸出異動量 * -1) < 0)
                {
                    來源異動量 = 來源庫存量 * -1;
                    輸出異動量 = 來源庫存量;
                    flag_部分撥發 = true;
                }

                輸出庫存量 = deviceBasics_藥局_buf[0].取得庫存();
                輸出結存量 = 輸出庫存量 + 輸出異動量;
                輸出備註 = "";


                來源異動量 = 輸出異動量 * -1;
                來源結存量 = 來源庫存量 + 來源異動量;
                來源備註 = "";

                list_儲位資料 = Function_取得異動儲位資訊從本地資料(藥品碼, 來源異動量);
                if (list_儲位資料.Count == 0)
                {
                    list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.未建立儲位.GetEnumName();
                    continue;
                }
                //if ((來源結存量) < 0)
                //{
                //    list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.庫存不足.GetEnumName();
                //    continue;
                //}


                for (int k = 0; k < list_儲位資料.Count; k++)
                {
                    儲位資訊_GUID = list_儲位資料[k][(int)enum_儲位資訊.GUID].ObjectToString();
                    儲位資訊_IP = list_儲位資料[k][(int)enum_儲位資訊.IP].ObjectToString();
                    儲位資訊_效期 = list_儲位資料[k][(int)enum_儲位資訊.效期].ObjectToString();
                    儲位資訊_批號 = list_儲位資料[k][(int)enum_儲位資訊.批號].ObjectToString();
                    儲位資訊_異動量 = list_儲位資料[k][(int)enum_儲位資訊.異動量].ObjectToString().StringToInt32();
                    Storage storage_藥庫 = this.List_Pannel35_本地資料.SortByGUID(儲位資訊_GUID);
                    DeviceBasic deviceBasic_藥庫 = this.List_藥庫_DeviceBasic.SortByGUID(儲位資訊_GUID);
                    if (storage_藥庫 == null && deviceBasic_藥庫 == null) continue;
                    if (storage_藥庫 != null)
                    {
                        storage_藥庫.效期庫存異動(儲位資訊_效期, 儲位資訊_異動量);
                        storages_藥庫_replace.Add(storage_藥庫);
                        this.List_Pannel35_本地資料.Add_NewStorage(storage_藥庫);
                    }
                    if (deviceBasic_藥庫 != null)
                    {
                        deviceBasic_藥庫.效期庫存異動(儲位資訊_效期, 儲位資訊_異動量);
                        deviceBasics_藥庫_replace.Add(deviceBasic_藥庫);
                        List_藥庫_DeviceBasic.Add_NewDeviceBasic(deviceBasic_藥庫);
                    }
                    if (deviceBasics_藥局_buf[0] != null)
                    {
                        deviceBasics_藥局_buf[0].效期庫存異動(儲位資訊_效期, 儲位資訊_異動量 * -1);
                        deviceBasics_藥局_replace.Add(deviceBasics_藥局_buf[0]);
                        List_藥局_DeviceBasic.Add_NewDeviceBasic(deviceBasics_藥局_buf[0]);
                    }




                    輸出備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量 * -1}";
                    if (k != list_儲位資料.Count - 1) 輸出備註 += "\n";

                    來源備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量 * 1}";
                    if (k != list_儲位資料.Count - 1) 來源備註 += "\n";
                }
                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.庫存] = 輸出庫存量;
                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.異動量] = 輸出異動量;
                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.結存量] = 輸出結存量;

                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.過帳時間] = DateTime.Now.ToDateTimeString_6();
                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName();
                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.備註] = 輸出備註;
                if (flag_部分撥發)
                {
                    list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.備註] += "[部分撥發]\n";
                }
                list_value_buf.Add(list_value[i]);



                object[] value_src = new object[new enum_交易記錄查詢資料().GetLength()];
                value_src[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                value_src[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_src[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.緊急申領.GetEnumName();
                value_src[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_src[(int)enum_交易記錄查詢資料.庫存量] = 來源庫存量;
                value_src[(int)enum_交易記錄查詢資料.交易量] = 來源異動量;
                value_src[(int)enum_交易記錄查詢資料.結存量] = 來源結存量;
                value_src[(int)enum_交易記錄查詢資料.備註] = 來源備註;
                value_src[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.藥庫.GetEnumName();
                value_src[(int)enum_交易記錄查詢資料.操作人] = this.登入者名稱;
                value_src[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();

                object[] value_out = new object[new enum_交易記錄查詢資料().GetLength()];
                value_out[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                value_out[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_out[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.緊急申領.GetEnumName();
                value_out[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_out[(int)enum_交易記錄查詢資料.庫存量] = 輸出庫存量;
                value_out[(int)enum_交易記錄查詢資料.交易量] = 輸出異動量;
                value_out[(int)enum_交易記錄查詢資料.結存量] = 輸出結存量;
                value_out[(int)enum_交易記錄查詢資料.備註] = 輸出備註;
                value_out[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.屏榮藥局.GetEnumName();
                value_out[(int)enum_交易記錄查詢資料.操作人] = this.登入者名稱;
                value_out[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();

                list_交易紀錄_Add.Add(value_src);
                list_交易紀錄_Add.Add(value_out);

            }

            dialog_Prcessbar.State = "上傳資料...";
            this.storageUI_WT32.SQL_ReplaceStorage(storages_藥庫_replace);
            this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasics_藥庫_replace);
            this.DeviceBasicClass_藥局.SQL_ReplaceDeviceBasic(deviceBasics_藥局_replace);
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_ReplaceExtra(list_value, false);
            this.sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄_Add, false);
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.RefreshGrid(list_value);
            dialog_Prcessbar.Close();
        }
        private void SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].Cells[(int)enum_藥庫_撥補_藥局_緊急申領.狀態].Value.ToString();
                if (狀態 == enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName())
                {
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_藥庫_撥補_藥局_緊急申領_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_藥庫_撥補_藥局_緊急申領_狀態.未建立儲位.GetEnumName())
                {
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }

            }
        }
        #endregion

        private class ICP_藥庫_撥補_藥局_緊急申領 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime0 = x[(int)enum_藥庫_撥補_藥局_緊急申領.產出時間].ToDateTimeString_6().StringToDateTime();
                DateTime datetime1 = y[(int)enum_藥庫_撥補_藥局_緊急申領.產出時間].ToDateTimeString_6().StringToDateTime();
                return datetime0.CompareTo(datetime1);
            }
        }
    }
}
