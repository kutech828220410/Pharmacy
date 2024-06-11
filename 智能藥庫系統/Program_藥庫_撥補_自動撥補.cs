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
using HIS_DB_Lib;
using SQLUI;
namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        enum enum_藥庫_撥補_自動撥補_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
            無效期可入帳,
            忽略過帳,
        }
        enum enum_藥庫_撥補_自動撥補
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
        private void sub_Program_藥庫_撥補_自動撥補_Init()
        {
            Table table = drugStotreDistributionClass.init(API_Server);

            this.sqL_DataGridView_藥庫_撥補_自動撥補.Init(table);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnVisible(false, new enum_drugStotreDistribution().GetEnumNames());
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(60, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.藥碼);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(250, DataGridViewContentAlignment.MiddleLeft, enum_drugStotreDistribution.藥名);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(50, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.目的庫別);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(70, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.包裝單位);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(50, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.包裝量);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(70, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.來源庫庫存);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(70, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.目的庫庫存);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(50, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.撥發量);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(50, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.實撥量);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(70, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.來源庫結存);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(70, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.目的庫結存);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(60, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.撥發人員);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.加入時間);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.撥發時間);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.報表名稱);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleCenter, enum_drugStotreDistribution.狀態);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnWidth(230, DataGridViewContentAlignment.MiddleLeft, enum_drugStotreDistribution.備註);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnSortMode(DataGridViewColumnSortMode.Automatic, enum_drugStotreDistribution.藥碼);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.DataGridRowsChangeRefEvent += SqL_DataGridView_藥庫_撥補_自動撥補_DataGridRowsChangeRefEvent;

            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnText("藥庫庫存", enum_drugStotreDistribution.來源庫庫存);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnText("藥局庫存", enum_drugStotreDistribution.目的庫庫存);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnText("藥庫結存", enum_drugStotreDistribution.來源庫結存);
            this.sqL_DataGridView_藥庫_撥補_自動撥補.Set_ColumnText("藥局結存", enum_drugStotreDistribution.目的庫結存);

            this.plC_RJ_Button_藥庫_撥補_搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_列印及匯出.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_列印及匯出_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_核撥.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_核撥_MouseDownEvent;

            this.parentCheckBox_藥局_撥補_表單分類_全選.AddChildCheckBox(checkBox_藥局_撥補_表單分類_冷藏藥);
            this.parentCheckBox_藥局_撥補_表單分類_全選.AddChildCheckBox(checkBox_藥局_撥補_表單分類_分包機裸錠);
            this.parentCheckBox_藥局_撥補_表單分類_全選.AddChildCheckBox(checkBox_藥局_撥補_表單分類_高價藥櫃);
            this.parentCheckBox_藥局_撥補_表單分類_全選.AddChildCheckBox(checkBox_藥局_撥補_表單分類_少用及易混);
            this.parentCheckBox_藥局_撥補_表單分類_全選.AddChildCheckBox(checkBox_藥局_撥補_表單分類_口服藥);
            this.parentCheckBox_藥局_撥補_表單分類_全選.AddChildCheckBox(checkBox_藥局_撥補_表單分類_針劑);
            this.parentCheckBox_藥局_撥補_表單分類_全選.AddChildCheckBox(checkBox_藥局_撥補_表單分類_外用藥);
            this.parentCheckBox_藥局_撥補_表單分類_全選.AddChildCheckBox(checkBox_藥局_撥補_表單分類_未分類);

            this.parentCheckBox_藥局_撥補_狀態條件_全選.AddChildCheckBox(plC_CheckBox_藥局_撥補_狀態條件_等待過帳);
            this.parentCheckBox_藥局_撥補_狀態條件_全選.AddChildCheckBox(plC_CheckBox_藥局_撥補_狀態條件_過帳完成);
            this.parentCheckBox_藥局_撥補_狀態條件_全選.AddChildCheckBox(plC_CheckBox_藥局_撥補_狀態條件_已列印);
            this.parentCheckBox_藥局_撥補_狀態條件_全選.AddChildCheckBox(plC_CheckBox_藥局_撥補_狀態條件_庫存不足);


            dateTimeIntervelPicker_藥庫_撥補_搜尋條件_報表時間.SetDateTime(DateTime.Now.GetStartDate(), DateTime.Now.GetEndDate());
            comboBox_藥庫_撥補_搜尋條件.SelectedIndex = 0;

            this.plC_UI_Init.Add_Method(this.sub_Program_藥庫_撥補_自動撥補);
        }



        private void sub_Program_藥庫_撥補_自動撥補()
        {

        }
        #region Function
        private void Function_藥庫_撥補_藥局_開始撥補(List<object[]> list_value)
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
                if (list_value[i][(int)enum_藥庫_撥補_自動撥補.狀態].ObjectToString() == enum_藥庫_撥補_自動撥補_狀態.過帳完成.GetEnumName())
                {
                    continue;
                }
                flag_部分撥發 = false;

                藥品碼 = list_value[i][(int)enum_藥庫_撥補_自動撥補.藥品碼].ObjectToString();
                藥品名稱 = list_value[i][(int)enum_藥庫_撥補_自動撥補.藥品名稱].ObjectToString();
                deviceBasics_藥局_buf = deviceBasics_藥局.SortByCode(藥品碼);
                if (deviceBasics_藥局.Count == 0) continue;

                來源庫存量 = this.Function_從本地資料取得庫存(藥品碼);
                輸出異動量 = list_value[i][(int)enum_藥庫_撥補_自動撥補.異動量].ObjectToString().StringToInt32();
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
                //if (list_儲位資料.Count == 0)
                //{
                //    list_value[i][(int)enum_藥庫_撥補_自動撥補.狀態] = enum_藥庫_撥補_自動撥補_狀態.庫存不足.GetEnumName();
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
                        storages_藥庫_replace.Add_NewStorage(storage_藥庫);
                        this.List_Pannel35_本地資料.Add_NewStorage(storage_藥庫);
                    }
                    if (deviceBasic_藥庫 != null)
                    {
                        deviceBasic_藥庫.效期庫存異動(儲位資訊_效期, 儲位資訊_異動量);
                        deviceBasics_藥庫_replace.Add_NewDeviceBasic(deviceBasic_藥庫);
                        List_藥庫_DeviceBasic.Add_NewDeviceBasic(deviceBasic_藥庫);
                    }
                    if (deviceBasics_藥局_buf[0] != null)
                    {
                        deviceBasics_藥局_buf[0].效期庫存異動(儲位資訊_效期, 儲位資訊_異動量 * -1);
                        deviceBasics_藥局_replace.Add_NewDeviceBasic(deviceBasics_藥局_buf[0]);
                        List_藥局_DeviceBasic.Add_NewDeviceBasic(deviceBasics_藥局_buf[0]);
                    }




                    輸出備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量 * -1}";
                    if (k != list_儲位資料.Count - 1) 輸出備註 += "\n";

                    來源備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量 * 1}";
                    if (k != list_儲位資料.Count - 1) 來源備註 += "\n";
                }
                list_value[i][(int)enum_藥庫_撥補_自動撥補.庫存] = 輸出庫存量;
                list_value[i][(int)enum_藥庫_撥補_自動撥補.異動量] = 輸出異動量;
                list_value[i][(int)enum_藥庫_撥補_自動撥補.結存量] = 輸出結存量;

                list_value[i][(int)enum_藥庫_撥補_自動撥補.過帳時間] = DateTime.Now.ToDateTimeString_6();
                list_value[i][(int)enum_藥庫_撥補_自動撥補.狀態] = enum_藥庫_撥補_自動撥補_狀態.過帳完成.GetEnumName();
                list_value[i][(int)enum_藥庫_撥補_自動撥補.備註] = 輸出備註;
                if (flag_部分撥發)
                {
                    list_value[i][(int)enum_藥庫_撥補_自動撥補.備註] += "\n[部分撥發]";
                }
                list_value_buf.Add(list_value[i]);



                object[] value_src = new object[new enum_交易記錄查詢資料().GetLength()];
                value_src[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                value_src[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_src[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.自動撥補.GetEnumName();
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
                value_out[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.自動撥補.GetEnumName();
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
            //this.sqL_DataGridView_藥庫_撥補_自動撥補.SQL_ReplaceExtra(list_value, false);
            this.sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄_Add, false);
            //this.sqL_DataGridView_藥庫_撥補_自動撥補.RefreshGrid(list_value);
            dialog_Prcessbar.Close();
        }
        #endregion
        #region Event
 
        private void SqL_DataGridView_藥庫_撥補_自動撥補_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            for (int i = 0; i < RowsList.Count; i++)
            {
                if (RowsList[i][(int)enum_drugStotreDistribution.撥發人員].ObjectToString().StringIsEmpty()) RowsList[i][(int)enum_drugStotreDistribution.撥發人員] = "-";
                if (RowsList[i][(int)enum_drugStotreDistribution.撥發單位].ObjectToString().StringIsEmpty()) RowsList[i][(int)enum_drugStotreDistribution.撥發單位] = "-";
                if (RowsList[i][(int)enum_drugStotreDistribution.實撥量].ObjectToString().StringIsEmpty()) RowsList[i][(int)enum_drugStotreDistribution.實撥量] = "-";
                if (RowsList[i][(int)enum_drugStotreDistribution.來源庫結存].ObjectToString().StringIsEmpty()) RowsList[i][(int)enum_drugStotreDistribution.來源庫結存] = "-";
                if (RowsList[i][(int)enum_drugStotreDistribution.目的庫結存].ObjectToString().StringIsEmpty()) RowsList[i][(int)enum_drugStotreDistribution.目的庫結存] = "-";
            }
        }
        private void PlC_RJ_Button_藥庫_撥補_核撥_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (MyMessageBox.ShowDialog("是否確定撥發?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
                List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_自動撥補.Get_All_Checked_RowsValues();

                list_value = (from value in list_value
                              where value[(int)enum_drugStotreDistribution.狀態].ObjectToString() != "過帳完成"
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
                    if (list_value[i][(int)enum_drugStotreDistribution.狀態].ObjectToString() == "過帳完成")
                    {
                        continue;
                    }
                    flag_部分撥發 = false;

                    藥品碼 = list_value[i][(int)enum_drugStotreDistribution.藥碼].ObjectToString();
                    藥品名稱 = list_value[i][(int)enum_drugStotreDistribution.藥名].ObjectToString();
                    deviceBasics_藥局_buf = deviceBasics_藥局.SortByCode(藥品碼);
                    if (deviceBasics_藥局_buf.Count == 0) continue;

                    來源庫存量 = this.Function_從本地資料取得庫存(藥品碼);
                    int 異動量 = list_value[i][(int)enum_drugStotreDistribution.實撥量].ObjectToString().StringToInt32();
                    輸出異動量 = list_value[i][(int)enum_drugStotreDistribution.實撥量].ObjectToString().StringToInt32();
                    if (來源庫存量 + (輸出異動量 * -1) < 0)
                    {
                        來源異動量 = 來源庫存量 * -1;
                        輸出異動量 = 來源庫存量;
                        flag_部分撥發 = true;
                    }

                    輸出庫存量 = deviceBasics_藥局_buf[0].取得庫存();
                    輸出結存量 = 輸出庫存量 + 輸出異動量;
                    輸出備註 = list_value[i][(int)enum_drugStotreDistribution.備註].ObjectToString();


                    來源異動量 = 輸出異動量 * -1;
                    來源結存量 = 來源庫存量 + 來源異動量;
                    來源備註 = list_value[i][(int)enum_drugStotreDistribution.備註].ObjectToString();

                    list_儲位資料 = Function_取得異動儲位資訊從本地資料(藥品碼, 來源異動量);

                    if (list_儲位資料.Count == 0)
                    {
                        if (異動量 > 0)
                        {
                            list_value[i][(int)enum_drugStotreDistribution.狀態] = "庫存不足";
                            continue;
                        }

                        //藥局申領量小於零
                        List<DeviceBasic> deviceBasics = this.List_藥庫_DeviceBasic.SortByCode(藥品碼);
                        if (deviceBasics.Count == 0)
                        {
                            list_value[i][(int)enum_drugStotreDistribution.狀態] = "未建立儲位";
                            continue;
                        }
                        List<string> list_效期 = new List<string>();
                        List<string> list_批號 = new List<string>();
                        Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥品碼, ref list_效期, ref list_批號);
                        if (list_效期.Count != 0)
                        {
                            deviceBasics[0].新增效期(list_效期[0], list_批號[0], "00");
                        }
                        else
                        {
                            deviceBasics[0].新增效期("2000/12/31", "系統代入", "00");
                        }

                        list_儲位資料 = Function_取得異動儲位資訊從本地資料(藥品碼, 來源異動量);

                    }

                    for (int k = 0; k < list_儲位資料.Count; k++)
                    {
                        儲位資訊_GUID = list_儲位資料[k][(int)enum_儲位資訊.GUID].ObjectToString();
                        儲位資訊_IP = list_儲位資料[k][(int)enum_儲位資訊.IP].ObjectToString();
                        儲位資訊_效期 = list_儲位資料[k][(int)enum_儲位資訊.效期].ObjectToString();
                        儲位資訊_批號 = list_儲位資料[k][(int)enum_儲位資訊.批號].ObjectToString();
                        儲位資訊_異動量 = list_儲位資料[k][(int)enum_儲位資訊.異動量].ObjectToString().StringToInt32();
                        DeviceBasic deviceBasic_藥庫 = this.List_藥庫_DeviceBasic.SortByGUID(儲位資訊_GUID);
                        if (deviceBasic_藥庫 == null) continue;

                        if (deviceBasic_藥庫 != null)
                        {

                            deviceBasic_藥庫.效期庫存異動(儲位資訊_效期, 儲位資訊_異動量);
                            deviceBasics_藥庫_replace.Add(deviceBasic_藥庫);
                            List_藥庫_DeviceBasic.Add_NewDeviceBasic(deviceBasic_藥庫);
                        }
                        if (deviceBasics_藥局_buf[0] != null)
                        {
                            if ((輸出異動量) > 0)
                            {
                                deviceBasics_藥局_buf[0].效期庫存異動(儲位資訊_效期, 儲位資訊_批號, (儲位資訊_異動量 * -1).ToString());
                                deviceBasics_藥局_replace.Add(deviceBasics_藥局_buf[0]);
                                List_藥局_DeviceBasic.Add_NewDeviceBasic(deviceBasics_藥局_buf[0]);
                                輸出備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量 * -1}";
                                if (k != list_儲位資料.Count - 1) 輸出備註 += "\n";
                            }
                        }

                        來源備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量 * 1}";
                        if (k != list_儲位資料.Count - 1) 來源備註 += "\n";
                    }

                    if ((輸出異動量) <= 0)
                    {
                        List<string> list_藥局效期 = new List<string>();
                        List<string> list_藥局批號 = new List<string>();
                        List<string> list_藥局異動量 = new List<string>();

                        deviceBasics_藥局_buf[0].庫存異動(輸出異動量, out list_藥局效期, out list_藥局批號, out list_藥局異動量);
                        deviceBasics_藥局_replace.Add(deviceBasics_藥局_buf[0]);
                        List_藥局_DeviceBasic.Add_NewDeviceBasic(deviceBasics_藥局_buf[0]);
                        for (int k = 0; k < list_藥局效期.Count; k++)
                        {
                            輸出備註 += $"[效期]:{list_藥局效期[k]},[批號]:{list_藥局批號[k]},[數量]:{list_藥局異動量[k]}";
                            if (k != list_藥局效期.Count - 1) 輸出備註 += "\n";
                        }

                    }
                    else
                    {

                    }
                    list_value[i][(int)enum_drugStotreDistribution.目的庫庫存] = 輸出庫存量;
                    list_value[i][(int)enum_drugStotreDistribution.實撥量] = 輸出異動量;
                    list_value[i][(int)enum_drugStotreDistribution.目的庫結存] = 輸出結存量;
                    list_value[i][(int)enum_drugStotreDistribution.來源庫庫存] = 來源庫存量;
                    list_value[i][(int)enum_drugStotreDistribution.來源庫結存] = 來源結存量;

                    list_value[i][(int)enum_drugStotreDistribution.撥發人員] = this.登入者名稱;

                    list_value[i][(int)enum_drugStotreDistribution.撥發時間] = DateTime.Now.ToDateTimeString_6();
                    list_value[i][(int)enum_drugStotreDistribution.狀態] = "過帳完成";
                    list_value[i][(int)enum_drugStotreDistribution.備註] = 輸出備註;
                    if (flag_部分撥發)
                    {
                        list_value[i][(int)enum_drugStotreDistribution.備註] += "[部分撥發]\n";
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
                dialog_Prcessbar.Close();
                this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasics_藥庫_replace);
                this.DeviceBasicClass_藥局.SQL_ReplaceDeviceBasic(deviceBasics_藥局_replace);

                List<drugStotreDistributionClass> drugStotreDistributionClasses = list_value.SQLToClass<drugStotreDistributionClass, enum_drugStotreDistribution>();
                drugStotreDistributionClass.update_by_guid(Main_Form.API_Server, drugStotreDistributionClasses);

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄_Add, false);
                this.sqL_DataGridView_藥庫_撥補_自動撥補.RefreshGrid(list_value);
            }));
        }
        private void PlC_RJ_Button_藥庫_撥補_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            try
            {
                string text = rJ_TextBox_藥庫_撥補_搜尋條件.Text;
                LoadingForm.ShowLoadingForm();
                DateTime dateTime_st = dateTimeIntervelPicker_藥庫_撥補_搜尋條件_報表時間.StartTime;
                DateTime dateTime_end = dateTimeIntervelPicker_藥庫_撥補_搜尋條件_報表時間.EndTime;
                List<drugStotreDistributionClass> drugStotreDistributionClasses = drugStotreDistributionClass.get_by_addedTime(API_Server, dateTime_st, dateTime_end);
                List<object[]> list_vale = drugStotreDistributionClasses.ClassToSQL<drugStotreDistributionClass, enum_drugStotreDistribution>();
                List<object[]> list_value_buf = new List<object[]>();
                string cmb_text = "";
     
                this.Invoke(new Action(delegate
                {
                    cmb_text = comboBox_藥庫_撥補_搜尋條件.Text;
                }));
                if (cmb_text == "全部顯示")
                {

                }
                else
                {
                    if(text.StringIsEmpty())
                    {
                        MyMessageBox.ShowDialog("未輸入搜尋條件");
                        return;
                    }
                    if (cmb_text == "藥碼")
                    {
                        if(rJ_RatioButton_藥庫_撥補_搜尋條件_模糊.Checked)
                        {
                            list_vale = list_vale.GetRowsByLike((int)enum_drugStotreDistribution.藥碼, text);
                        }
                        if (rJ_RatioButton_藥庫_撥補_搜尋條件_前綴.Checked)
                        {
                            list_vale = list_vale.GetRowsStartWithByLike((int)enum_drugStotreDistribution.藥碼, text);
                        }                    
                    }
                    if (cmb_text == "藥名")
                    {
                        if (rJ_RatioButton_藥庫_撥補_搜尋條件_模糊.Checked)
                        {
                            list_vale = list_vale.GetRowsByLike((int)enum_drugStotreDistribution.藥名, text);
                        }
                        if (rJ_RatioButton_藥庫_撥補_搜尋條件_前綴.Checked)
                        {
                            list_vale = list_vale.GetRowsStartWithByLike((int)enum_drugStotreDistribution.藥名, text);
                        }
                    }
           
                }
                list_value_buf = list_vale;

                Dictionary<object, List<object[]>> keyValuePairs_表單分類 = list_value_buf.ConvertToDictionary((int)enum_drugStotreDistribution.報表名稱);
                List<object[]> list_表單分類 = new List<object[]>();
                if (checkBox_藥局_撥補_表單分類_冷藏藥.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.冷藏藥.GetEnumName()));
                if (checkBox_藥局_撥補_表單分類_分包機裸錠.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.分包機裸錠.GetEnumName()));
                if (checkBox_藥局_撥補_表單分類_高價藥櫃.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.高價藥櫃.GetEnumName()));
                if (checkBox_藥局_撥補_表單分類_少用及易混.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.少用及易混.GetEnumName()));
                if (checkBox_藥局_撥補_表單分類_口服藥.Checked)
                {
                    List<object[]> list_口服藥 = (from temp in list_value_buf
                                               where temp[(int)enum_drugStotreDistribution.報表名稱].ObjectToString().Contains("口服藥")
                                               select temp).ToList();
                    list_表單分類.LockAdd(list_口服藥);
                }
                if (checkBox_藥局_撥補_表單分類_針劑.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.針劑.GetEnumName()));
                if (checkBox_藥局_撥補_表單分類_外用藥.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.外用藥.GetEnumName()));
                if (checkBox_藥局_撥補_表單分類_未分類.Checked)
                {
                    list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.未分類.GetEnumName()));
                    list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(""));
                }
                list_value_buf = list_表單分類;

                Dictionary<object, List<object[]>> keyValuePairs_狀態條件 = list_value_buf.ConvertToDictionary((int)enum_drugStotreDistribution.狀態);
                List<object[]> list_狀態條件 = new List<object[]>();
                if (plC_CheckBox_藥局_撥補_狀態條件_等待過帳.Checked) list_狀態條件.LockAdd(keyValuePairs_狀態條件.SortDictionary("等待過帳"));
                if (plC_CheckBox_藥局_撥補_狀態條件_過帳完成.Checked) list_狀態條件.LockAdd(keyValuePairs_狀態條件.SortDictionary("過帳完成"));
                if (plC_CheckBox_藥局_撥補_狀態條件_已列印.Checked) list_狀態條件.LockAdd(keyValuePairs_狀態條件.SortDictionary("已列印"));
                if (plC_CheckBox_藥局_撥補_狀態條件_庫存不足.Checked) list_狀態條件.LockAdd(keyValuePairs_狀態條件.SortDictionary("庫存不足"));
                list_value_buf = list_狀態條件;


                drugStotreDistributionClasses = list_value_buf.SQLToClass<drugStotreDistributionClass, enum_drugStotreDistribution>();
                drugStotreDistributionClasses.Sort(new drugStotreDistributionClass.ICP_By_addedTime());
                list_value_buf = drugStotreDistributionClasses.ClassToSQL<drugStotreDistributionClass, enum_drugStotreDistribution>();
                sqL_DataGridView_藥庫_撥補_自動撥補.RefreshGrid(list_value_buf);

                if (list_value_buf.Count == 0)
                {
                    MyMessageBox.ShowDialog("查無資料");
                    return;
                }

        
            }
            catch
            {

            }
            finally
            {
                LoadingForm.CloseLoadingForm();
            }

        }
        private void PlC_RJ_Button_藥庫_撥補_列印及匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_自動撥補.Get_All_Checked_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料");
                return;
            }
            List<drugStotreDistributionClass> drugStotreDistributionClasses = list_value.SQLToClass<drugStotreDistributionClass,enum_drugStotreDistribution>(); 
            Dialog_自動撥補_列印及匯出 dialog_自動撥補_列印及匯出 = new Dialog_自動撥補_列印及匯出(drugStotreDistributionClasses);
            dialog_自動撥補_列印及匯出.ShowDialog();

            list_value = dialog_自動撥補_列印及匯出.Value.ClassToSQL<drugStotreDistributionClass, enum_drugStotreDistribution>();
            this.sqL_DataGridView_藥庫_撥補_自動撥補.ReplaceExtra(list_value , true);
        }
        #endregion
    }
}
