﻿using System;
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
using HIS_DB_Lib;
namespace 智能藥庫系統
{
    public class class_emg_apply
    {
        [JsonPropertyName("cost_center")]
        public string 成本中心 { get; set; }
        [JsonPropertyName("code")]
        public string 藥品碼 { get; set; }
        [JsonPropertyName("name")]
        public string 藥品名稱 { get; set; }
        [JsonPropertyName("inventory")]
        public string 庫存量 { get; set; }
        [JsonPropertyName("value")]
        public string 撥出量 { get; set; }
        [JsonPropertyName("note")]
        public string 備註 { get; set; }
    }
    enum enum_藥庫_撥補_藥局_緊急申領_狀態
    {
        等待過帳,
        庫存不足,
        未建立儲位,
        過帳完成,
        找無此藥品,
        備藥中,
        已列印,
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
    public partial class Main_Form : Form
    {
      
       
        private PrinterClass printerClass_緊急申領 = new PrinterClass();
        private void sub_Program_藥庫_撥補_藥局_緊急申領_Init()
        {
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Init();
            if (!this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_IsTableCreat()) this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_CreateTable();
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.DataGridRefreshEvent += SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRefreshEvent;
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.DataGridRowsChangeRefEvent += SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.RowDoubleClickEvent += SqL_DataGridView_藥庫_撥補_藥局_緊急申領_RowDoubleClickEvent;


            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_顯示資料.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_顯示資料_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_撥發.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_撥發_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_備藥中.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_備藥中_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_列印及匯出資料.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_列印及匯出資料_MouseDownEvent;
            this.plC_RJ_Button_藥庫_撥補_藥局_緊急申領_刪除選取資料.MouseDownEvent += PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_刪除選取資料_MouseDownEvent;

            this.comboBox_藥庫_緊急申領_搜尋條件.SelectedIndex = 0;


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
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_刪除選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Get_All_Checked_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取有效資料!");
            }
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_DeleteExtra(list_value, false);
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.DeleteExtra(list_value, true);
            MyMessageBox.ShowDialog($"已刪除資料<{list_value.Count}>筆資料!");

        }
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_顯示資料_MouseDownEvent(MouseEventArgs mevent)
        {
            LoadingForm.ShowLoadingForm();
            try
            {
                string text = "";
                string value = rJ_TextBox_藥庫_緊急申領_搜尋條件.Texts;
                this.Invoke(new Action(delegate
                {
                    text = comboBox_藥庫_緊急申領_搜尋條件.Text;
                }));
                List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_GetRowsByBetween((int)enum_藥庫_撥補_藥局_緊急申領.產出時間, rJ_DatePicker_藥庫_撥補_藥局_緊急申領_產出日期_起始, rJ_DatePicker_藥庫_撥補_藥局_緊急申領_產出日期_結束, false);
                if (text == "全部顯示")
                {

                }
                if (text == "藥碼")
                {
                    if (rJ_RatioButton_藥庫_每日訂單_下訂單_模糊.Checked)
                    {
                        list_value = list_value.GetRowsByLike((int)enum_藥庫_撥補_藥局_緊急申領.藥品碼, value);
                    }
                    if (rJ_RatioButton_藥庫_每日訂單_下訂單_前綴.Checked)
                    {
                        list_value = list_value.GetRowsStartWithByLike((int)enum_藥庫_撥補_藥局_緊急申領.藥品碼, value);
                    }
                }
                if (text == "藥名")
                {
                    if (rJ_RatioButton_藥庫_每日訂單_下訂單_模糊.Checked)
                    {
                        list_value = list_value.GetRowsByLike((int)enum_藥庫_撥補_藥局_緊急申領.藥品名稱, value);
                    }
                    if (rJ_RatioButton_藥庫_每日訂單_下訂單_前綴.Checked)
                    {
                        list_value = list_value.GetRowsStartWithByLike((int)enum_藥庫_撥補_藥局_緊急申領.藥品名稱, value);
                    }
                }
                if (list_value.Count == 0)
                {
                    MyMessageBox.ShowDialog("查無資料");
                    return;
                }

                this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.RefreshGrid(list_value);
            }
            catch
            {

            }
            finally
            {
                LoadingForm.CloseLoadingForm();
            }
        }
        private void SqL_DataGridView_藥庫_撥補_藥局_緊急申領_RowDoubleClickEvent(object[] RowValue)
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();

            if (RowValue[(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() == enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()) return;
            Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel("請輸入申領數量");
            if (dialog_NumPannel.ShowDialog() != DialogResult.Yes) return;
            if (dialog_NumPannel.Value < 0) return;
            RowValue[(int)enum_藥庫_撥補_藥局_緊急申領.異動量] = dialog_NumPannel.Value;
   
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_ReplaceExtra(RowValue, false);
            Console.WriteLine($"SQL 緊急申領,{myTimerBasic.ToString()} ");
        
            this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.ReplaceExtra(RowValue, true);
            Console.WriteLine($"緊急申領,{myTimerBasic.ToString()} ");
        }
        private void SqL_DataGridView_藥庫_撥補_藥局_緊急申領_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            List<object[]> list_藥庫庫存資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥庫庫存資料_buf = new List<object[]>();
            List<object[]> RowsList_buf = new List<object[]>();
            list_藥庫庫存資料 = this.sqL_DataGridView_藥庫_藥品資料.RowsChangeFunction(list_藥庫庫存資料);
            string Code = "";
            for (int i = 0; i < RowsList.Count; i++)
            {
                Code = RowsList[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString();
                list_藥庫庫存資料_buf = list_藥庫庫存資料.GetRows((int)enum_medDrugstore.藥品碼, Code);
            
                if(list_藥庫庫存資料_buf.Count > 0)
                {
                    RowsList[i][(int)enum_藥庫_撥補_藥局_緊急申領.庫存] = list_藥庫庫存資料_buf[0][(int)enum_medDrugstore.藥庫庫存];
                }
                
            }
            
            RowsList.Sort(new ICP_藥庫_撥補_藥局_緊急申領());

            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_過帳完成.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_等待過帳.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.等待過帳.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_未建立儲位.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.未建立儲位.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_已列印.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.已列印.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_找無此藥品.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.找無此藥品.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_庫存不足.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.庫存不足.GetEnumName()));
            if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_顯示設定_備藥中.Checked) RowsList_buf.LockAdd(RowsList.GetRows((int)enum_藥庫_撥補_藥局_緊急申領.狀態, enum_藥庫_撥補_藥局_緊急申領_狀態.備藥中.GetEnumName()));

            RowsList = RowsList_buf;
        }
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_撥發_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (MyMessageBox.ShowDialog("是否確定撥發?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
                List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Get_All_Checked_RowsValues();
                List<object[]> list_value_buf = new List<object[]>();
                list_value = (from value in list_value
                              where value[(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() != enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()
                              select value).ToList();
                if (list_value.Count == 0)
                {
                    MyMessageBox.ShowDialog("未選取有效資料!");
                }

                for (int i = 0; i < list_value.Count; i++)
                {
                    list_value_buf = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_GetRows((int)enum_藥庫_撥補_藥局_緊急申領.GUID, list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.GUID].ObjectToString(), false);
                    if (list_value_buf.Count != 0)
                    {
                        list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = list_value_buf[0][(int)enum_藥庫_撥補_藥局_緊急申領.狀態];
                        this.sqL_DataGridView_藥庫_撥補_自動撥補.ReplaceExtra(list_value_buf[0], false);
                    }
                }
                this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.RefreshGrid();
                list_value_buf = (from value in list_value
                                  where value[(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() != "已列印"
                                  select value).ToList();
                if (list_value_buf.Count != 0)
                {
                    MyMessageBox.ShowDialog("資料都需為[已列印]狀態,重新檢視");
                    return;
                }

                if (plC_CheckBox_藥庫_撥補_藥局_緊急申領_要過帳.Checked || true)
                {
                    string error_msg = "";
                    List<object[]> list_交易紀錄_Add = new List<object[]>();
                    List<DeviceBasic> deviceBasics_藥庫 = DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
                    List<DeviceBasic> deviceBasics_藥庫_replace = new List<DeviceBasic>();
                    List<DeviceBasic> deviceBasics_藥庫_buf = new List<DeviceBasic>();

                    List<DeviceBasic> deviceBasics_藥局 = DeviceBasicClass_藥局.SQL_GetAllDeviceBasic();
                    List<DeviceBasic> deviceBasics_藥局_replace = new List<DeviceBasic>();
                    List<DeviceBasic> deviceBasics_藥局_buf = new List<DeviceBasic>();

                    for (int i = 0; i < deviceBasics_藥局.Count; i++)
                    {
                        deviceBasics_藥局[i].flag_replace = false;
                    }
                    for (int i = 0; i < deviceBasics_藥庫.Count; i++)
                    {
                        deviceBasics_藥庫[i].flag_replace = false;
                    }


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
                    string str = "";
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
                        deviceBasics_藥庫_buf = deviceBasics_藥庫.SortByCode(藥品碼);
                        if (deviceBasics_藥局_buf.Count == 0) continue;

                        來源庫存量 = this.Function_從本地資料取得庫存(藥品碼);
                        int 異動量 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.異動量].ObjectToString().StringToInt32();
                        輸出異動量 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.異動量].ObjectToString().StringToInt32();
                        if (來源庫存量 + (輸出異動量 * -1) < 0)
                        {
                            來源異動量 = 來源庫存量 * -1;
                            輸出異動量 = 來源庫存量;
                            flag_部分撥發 = true;
                            str += $"({藥品碼}){藥品名稱}\n";
                            continue;
                        }

                        輸出庫存量 = deviceBasics_藥局_buf[0].取得庫存();
                        輸出結存量 = 輸出庫存量 + 輸出異動量;
                        輸出備註 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.備註].ObjectToString();


                        來源異動量 = 輸出異動量 * -1;
                        來源結存量 = 來源庫存量 + 來源異動量;
                        來源備註 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.備註].ObjectToString();

                        list_儲位資料 = Function_取得異動儲位資訊從本地資料(藥品碼, 來源異動量);
                     
                        if (list_儲位資料.Count == 0)
                        {
                            if (異動量 > 0)
                            {
                                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.庫存不足.GetEnumName();
                                continue;
                            }

                            //藥局申領量小於零
                            List<DeviceBasic> deviceBasics = this.List_藥庫_DeviceBasic.SortByCode(藥品碼);
                            if(deviceBasics.Count == 0)
                            {
                                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.未建立儲位.GetEnumName();
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
                        //if ((來源結存量) < 0)
                        //{
                        //    list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.庫存不足.GetEnumName();
                        //    continue;
                        //}

                        DeviceBasic deviceBasic_藥庫 = null;
                        for (int k = 0; k < list_儲位資料.Count; k++)
                        {
                            儲位資訊_GUID = list_儲位資料[k][(int)enum_儲位資訊.GUID].ObjectToString();
                            儲位資訊_IP = list_儲位資料[k][(int)enum_儲位資訊.IP].ObjectToString();
                            儲位資訊_效期 = list_儲位資料[k][(int)enum_儲位資訊.效期].ObjectToString();
                            儲位資訊_批號 = list_儲位資料[k][(int)enum_儲位資訊.批號].ObjectToString();
                            儲位資訊_異動量 = list_儲位資料[k][(int)enum_儲位資訊.異動量].ObjectToString().StringToInt32();
                            if (deviceBasics_藥庫_buf.Count == 0) continue;
                            deviceBasic_藥庫 = deviceBasics_藥庫_buf[0];
                            if (deviceBasic_藥庫 == null) continue;

                            if (deviceBasic_藥庫 != null)
                            {
                                deviceBasic_藥庫.效期庫存異動(儲位資訊_效期, 儲位資訊_異動量);
                                deviceBasic_藥庫.flag_replace = true; ;
                                List_藥庫_DeviceBasic.Add_NewDeviceBasic(deviceBasic_藥庫);
                            }
                            if (deviceBasics_藥局_buf[0] != null)
                            {
                                if ((輸出異動量) > 0)
                                {
                                    deviceBasics_藥局_buf[0].效期庫存異動(儲位資訊_效期, 儲位資訊_批號, (儲位資訊_異動量 * -1).ToString());
                                    deviceBasics_藥局_buf[0].flag_replace = true;
                                    List_藥局_DeviceBasic.Add_NewDeviceBasic(deviceBasics_藥局_buf[0]);
                                    輸出備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量 * -1}";
                                    if (k != list_儲位資料.Count - 1) 輸出備註 += "\n";
                                }
                                else
                                {

                                }
                               
                           
                            }

                            來源備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量 * 1}";
                            if (k != list_儲位資料.Count - 1) 來源備註 += "\n";
                        }

                        if ((輸出異動量 ) < 0)
                        {
                            List<string> list_藥局效期 = new List<string>();
                            List<string> list_藥局批號 = new List<string>();
                            List<string> list_藥局異動量 = new List<string>();

                            deviceBasics_藥局_buf[0].庫存異動(輸出異動量 , out list_藥局效期, out list_藥局批號, out list_藥局異動量);
                            deviceBasics_藥局_buf[0].flag_replace = true;
                            List_藥局_DeviceBasic.Add_NewDeviceBasic(deviceBasics_藥局_buf[0]);
                            for(int k = 0; k < list_藥局效期.Count; k++)
                            {
                                輸出備註 += $"[效期]:{list_藥局效期[k]},[批號]:{list_藥局批號[k]},[數量]:{list_藥局異動量[k]}";
                                if (k != list_藥局效期.Count - 1) 輸出備註 += "\n";
                            }
           
                        }
                        else
                        {

                        }

                        if (deviceBasics_藥庫_buf[0].Inventory.StringToInt32() != 來源結存量)
                        {
                            error_msg += $"[藥庫]({藥品碼}){藥品名稱}\n";
                            deviceBasics_藥庫_buf[0].flag_replace = false;
                            continue;
                        }

                        if (deviceBasics_藥局_buf[0].Inventory.StringToInt32() != 輸出結存量)
                        {
                            error_msg += $"[藥局]({藥品碼}){藥品名稱}\n";
                            deviceBasics_藥局_buf[0].flag_replace = false;
                            continue;
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
                    dialog_Prcessbar.Close();

                    for (int i = 0; i < deviceBasics_藥局.Count; i++)
                    {
                        if (deviceBasics_藥局[i].flag_replace)
                        {
                            deviceBasics_藥局_replace.Add(deviceBasics_藥局[i]);
                            deviceBasics_藥局[i].flag_replace = false;
                        }
                    }
                    for (int i = 0; i < deviceBasics_藥庫.Count; i++)
                    {
                        if (deviceBasics_藥庫[i].flag_replace)
                        {
                            deviceBasics_藥庫_replace.Add(deviceBasics_藥庫[i]);
                            deviceBasics_藥庫[i].flag_replace = false;
                        }
                    }

                    this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasics_藥庫_replace);
                    this.DeviceBasicClass_藥局.SQL_ReplaceDeviceBasic(deviceBasics_藥局_replace);
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_ReplaceExtra(list_value, false);
                    this.sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄_Add, false);
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.RefreshGrid(list_value);
                    if (str.StringIsEmpty() == false)
                    {
                        str += "以上藥品,藥庫庫存不足無法撥發,請重新設定核撥量";
                        MyMessageBox.ShowDialog(str);
                    }
                    if (error_msg.StringIsEmpty() == false)
                    {
                        MyMessageBox.ShowDialog($"{error_msg}\n 藥品撥發異常,重新撥發");
                    }
                }
                else
                {
                    for (int i = 0; i < list_value.Count; i++)
                    {
                        string 狀態 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString();
                        if (狀態 != enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()|| 狀態 != enum_藥庫_撥補_藥局_緊急申領_狀態.未建立儲位.GetEnumName()|| 狀態 != enum_藥庫_撥補_藥局_緊急申領_狀態.庫存不足.GetEnumName())
                        {
                            list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.已列印.GetEnumName();
                        }
                       
                    }
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.SQL_ReplaceExtra(list_value, false);
                    this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.ReplaceExtra(list_value, true);
                }
            }));

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
                list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.等待過帳.GetEnumName();
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
      
            }
        }
      
        private void PlC_RJ_Button_藥庫_撥補_藥局_緊急申領_列印及匯出資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<class_emg_apply> class_Emg_Applies = new List<class_emg_apply>();
            List<object[]> list_value = this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領.Get_All_Checked_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取有效資料!");
                return;
            }
            Dialog_緊急申領_列印及匯出 Dialog_緊急申領_列印及匯出 = new Dialog_緊急申領_列印及匯出(this.sqL_DataGridView_藥庫_撥補_藥局_緊急申領, dBConfigClass.Emg_apply_ApiURL);
            Dialog_緊急申領_列印及匯出.ShowDialog();
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
