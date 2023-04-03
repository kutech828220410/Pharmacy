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
using MyOffice;
using MyPrinterlib;
using NPOI;
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        public class class_emg_apply
        {
            [JsonPropertyName("cost_center")]
            public string 成本中心 { get; set; }
            [JsonPropertyName("code")]
            public string 藥品碼 { get; set; }
            [JsonPropertyName("name")]
            public string 藥品名稱 { get; set; }
            [JsonPropertyName("value")]
            public string 撥出量 { get; set; }


        }
        enum enum_藥庫_撥補_藥局_緊急申領_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
            備藥中,
            撥發完成,
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
        private PrinterClass printerClass_緊急申領 = new PrinterClass();
        private void sub_Program_藥庫_撥補_藥局_緊急申領_Init()
        {
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Init();
            if (!this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_IsTableCreat()) this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_CreateTable();
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.DataGridRefreshEvent += SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRefreshEvent;
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.DataGridRowsChangeRefEvent += SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRowsChangeRefEvent;

            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_顯示資料.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_顯示資料_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_撥發.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_撥發_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_備藥中.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_備藥中_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_列印選擇資料.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_列印選擇資料_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_匯出選擇資料.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_匯出選擇資料_MouseDownEvent;


            this.printerClass_緊急申領.Init();
            this.printerClass_緊急申領.PrintPageEvent += PrinterClass_緊急申領_PrintPageEvent;

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
            List<object[]> list_藥庫庫存資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥庫庫存資料_buf = new List<object[]>();
            List<object[]> RowsList_buf = new List<object[]>();
            list_藥庫庫存資料 = this.sqL_DataGridView_藥庫_藥品資料.RowsChangeFunction(list_藥庫庫存資料);

            for (int i = 0; i < RowsList.Count; i++)
            {
                list_藥庫庫存資料_buf = list_藥庫庫存資料.GetRows((int)enum_藥庫_藥品資料.藥品碼, RowsList[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString());
                if(list_藥庫庫存資料_buf.Count > 0)
                {
                    RowsList[i][(int)enum_藥庫_撥補_藥局_緊急申領.庫存] = list_藥庫庫存資料_buf[0][(int)enum_藥庫_藥品資料.藥庫庫存];
                }
                
            }
            
            RowsList.Sort(new ICP_藥庫_撥補_藥局_緊急申領());

            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_過帳完成.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_等待過帳.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.等待過帳.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_未建立儲位.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.未建立儲位.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_撥發完成.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.撥發完成.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_找無此藥品.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.找無此藥品.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_庫存不足.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.庫存不足.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_備藥中.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.備藥中.GetEnumName()));

            RowsList = RowsList_buf;
        }
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_撥發_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否確定撥發?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Get_All_Checked_RowsValues();

            list_value = (from value in list_value
                          where value[(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() != enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()
                          select value).ToList();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取有效資料!");
            }
            if(plC_CheckBox_藥庫_撥補_藥局_緊急申領_要過帳.Checked)
            {
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
            else
            {
                for(int i = 0; i < list_value.Count; i++)
                {
                    list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.撥發完成.GetEnumName();
                }
                this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_ReplaceExtra(list_value, false);
                this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.ReplaceExtra(list_value, true);
            }
           
        }
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_備藥中_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否確定設定備藥中?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Get_All_Checked_RowsValues();

            list_value = (from value in list_value
                          where value[(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() != enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()
                          select value).ToList();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取有效資料!");
            }
            for (int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.備藥中.GetEnumName();
            }
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_ReplaceExtra(list_value, false);
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.ReplaceExtra(list_value, true);
        }
        private void SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].Cells[enum_藥庫_撥補_藥局_緊急申領.狀態.GetEnumName()].Value.ToString();
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
                if (狀態 == enum_藥庫_撥補_藥局_緊急申領_狀態.備藥中.GetEnumName())
                {
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_藥庫_撥補_藥局_緊急申領_狀態.撥發完成.GetEnumName())
                {
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_列印選擇資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<class_emg_apply> class_Emg_Applies = new List<class_emg_apply>();
            List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Get_All_Checked_RowsValues();
            for (int i = 0; i < list_value.Count; i++)
            {
                class_emg_apply class_Emg_Apply = new class_emg_apply();
                class_Emg_Apply.成本中心 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥局代碼].ObjectToString();
                class_Emg_Apply.藥品碼 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString();
                class_Emg_Apply.藥品名稱 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品名稱].ObjectToString();
                class_Emg_Apply.撥出量 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.異動量].ObjectToString();
                class_Emg_Applies.Add(class_Emg_Apply);
            }

            string json_in = class_Emg_Applies.JsonSerializationt(true);
            string json = Basic.Net.WEBApiPostJson($"{dBConfigClass.Emg_apply_ApiURL}", json_in);
            List<SheetClass> sheetClass = json.JsonDeserializet<List<SheetClass>>();
            if (printerClass_緊急申領.ShowPreviewDialog(sheetClass, MyPrinterlib.PrinterClass.PageSize.A4) == DialogResult.OK)
            {

            }
            for (int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.備藥中.GetEnumName();
            }
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_ReplaceExtra(list_value, false);
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.ReplaceExtra(list_value, true);

        }
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_匯出選擇資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<class_emg_apply> class_Emg_Applies = new List<class_emg_apply>();
            List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Get_All_Checked_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取有效資料!");
                return;
            }
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate { dialogResult = this.saveFileDialog_SaveExcel.ShowDialog(); }));
            if (dialogResult == DialogResult.OK)
            {
                for (int i = 0; i < list_value.Count; i++)
                {
                    class_emg_apply class_Emg_Apply = new class_emg_apply();
                    class_Emg_Apply.成本中心 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥局代碼].ObjectToString();
                    class_Emg_Apply.藥品碼 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString();
                    class_Emg_Apply.藥品名稱 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品名稱].ObjectToString();
                    class_Emg_Apply.撥出量 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.異動量].ObjectToString();
                    class_Emg_Applies.Add(class_Emg_Apply);

                                  
                }
                string json_in = class_Emg_Applies.JsonSerializationt(true);
                string json = Basic.Net.WEBApiPostJson($"{dBConfigClass.Emg_apply_ApiURL}", json_in);
                List<SheetClass> sheetClass = json.JsonDeserializet<List<SheetClass>>();

                sheetClass.NPOI_SaveFile(this.saveFileDialog_SaveExcel.FileName);
                MyMessageBox.ShowDialog("匯出完成!");
            }
        }
        private void PrinterClass_緊急申領_PrintPageEvent(object sender, Graphics g, int width, int height, int page_num)
        {
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            using (Bitmap bitmap = printerClass_緊急申領.GetSheetClass(page_num).GetBitmap(width, height, 0.75, H_Alignment.Center, V_Alignment.Top, 0, 50))
            {
                rectangle.Height = bitmap.Height;
                g.DrawImage(bitmap, rectangle);
            }
        }
        #endregion

        private class ICP_藥庫_撥補_藥局_緊急申領 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string 藥品碼0 = x[(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString();
                string 藥品碼1 = y[(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString();
                int temp = 藥品碼0.CompareTo(藥品碼1);
                if (temp == 0)
                {
                    DateTime datetime0 = x[(int)enum_藥庫_撥補_藥局_緊急申領.產出時間].ToDateTimeString_6().StringToDateTime();
                    DateTime datetime1 = y[(int)enum_藥庫_撥補_藥局_緊急申領.產出時間].ToDateTimeString_6().StringToDateTime();
                    return datetime0.CompareTo(datetime1);
                }
                else
                {
                    return temp;
                }
              
            }
        }
    }
}
