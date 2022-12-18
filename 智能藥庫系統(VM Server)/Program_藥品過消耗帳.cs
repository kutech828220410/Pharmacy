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

namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {
     
        private enum enum_藥品過消耗帳_來源名稱
        {
            門診,
            急診,
            住院,
            公藥,
        }
        private enum enum_藥品過消耗帳
        {
            GUID,
            來源名稱,
            來源報表,
            藥局代碼,
            藥品碼,
            藥品名稱,
            異動量,
            報表日期,
            產出時間,
            過帳時間,
            狀態,
            備註,           
        }
        enum enum_藥品過消耗帳_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
            無效期可入帳,
            忽略過帳,
        }
        private void sub_Program_藥品過消耗帳_Init()
        {
          

            this.sqL_DataGridView_藥品過消耗帳.Init();
            this.sqL_DataGridView_藥品過消耗帳.DataGridRefreshEvent += SqL_DataGridView_藥品過消耗帳_DataGridRefreshEvent;


            this.plC_RJ_Button_藥品過消耗帳_顯示今日消耗帳.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_顯示今日消耗帳_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_指定報表日期_顯示.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_指定報表日期_顯示_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_選取資料設定過帳完成.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_選取資料設定過帳完成_MouseDownEvent;
            this.plC_RJ_Button藥品過消耗帳_選取資料等待過帳.MouseDownEvent += PlC_RJ_Button藥品過消耗帳_選取資料等待過帳_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_選取資料忽略過帳.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_選取資料忽略過帳_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_顯示所有資料.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_顯示所有資料_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_藥品碼篩選.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_藥品碼篩選_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_顯示異常過帳.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_顯示異常過帳_MouseDownEvent;

            this.plC_UI_Init.Add_Method(this.sub_Program_藥品過消耗帳);
        }

     

        private void sub_Program_藥品過消耗帳()
        {
            sub_Program_檢查藥品過消耗帳();
        }


        #region PLC_檢查藥品過消耗帳
        Task Task_檢查藥品過消耗帳;
        PLC_Device PLC_Device_檢查藥品過消耗帳 = new PLC_Device("");
        PLC_Device PLC_Device_檢查藥品過消耗帳_OK = new PLC_Device("");
        MyTimer MyTimer_檢查藥品過消耗帳_結束延遲 = new MyTimer();
        int cnt_Program_檢查藥品過消耗帳 = 65534;
        void sub_Program_檢查藥品過消耗帳()
        {
            PLC_Device_檢查藥品過消耗帳.Bool = true;
            if (cnt_Program_檢查藥品過消耗帳 == 65534)
            {
                this.MyTimer_檢查藥品過消耗帳_結束延遲.StartTickTime(10000);
                PLC_Device_檢查藥品過消耗帳.SetComment("PLC_檢查藥品過消耗帳");
                PLC_Device_檢查藥品過消耗帳_OK.SetComment("PLC_檢查藥品過消耗帳_OK");
                PLC_Device_檢查藥品過消耗帳.Bool = false;
                cnt_Program_檢查藥品過消耗帳 = 65535;
            }
            if (cnt_Program_檢查藥品過消耗帳 == 65535) cnt_Program_檢查藥品過消耗帳 = 1;
            if (cnt_Program_檢查藥品過消耗帳 == 1) cnt_Program_檢查藥品過消耗帳_檢查按下(ref cnt_Program_檢查藥品過消耗帳);
            if (cnt_Program_檢查藥品過消耗帳 == 2) cnt_Program_檢查藥品過消耗帳_初始化(ref cnt_Program_檢查藥品過消耗帳);
            if (cnt_Program_檢查藥品過消耗帳 == 3) cnt_Program_檢查藥品過消耗帳 = 65500;
            if (cnt_Program_檢查藥品過消耗帳 > 1) cnt_Program_檢查藥品過消耗帳_檢查放開(ref cnt_Program_檢查藥品過消耗帳);

            if (cnt_Program_檢查藥品過消耗帳 == 65500)
            {
                this.MyTimer_檢查藥品過消耗帳_結束延遲.TickStop();
                this.MyTimer_檢查藥品過消耗帳_結束延遲.StartTickTime(60000);
                PLC_Device_檢查藥品過消耗帳.Bool = false;
                PLC_Device_檢查藥品過消耗帳_OK.Bool = false;
                cnt_Program_檢查藥品過消耗帳 = 65535;
            }
        }
        void cnt_Program_檢查藥品過消耗帳_檢查按下(ref int cnt)
        {
            if (PLC_Device_檢查藥品過消耗帳.Bool) cnt++;
        }
        void cnt_Program_檢查藥品過消耗帳_檢查放開(ref int cnt)
        {
            if (!PLC_Device_檢查藥品過消耗帳.Bool) cnt = 65500;
        }
        void cnt_Program_檢查藥品過消耗帳_初始化(ref int cnt)
        {
            if (this.MyTimer_檢查藥品過消耗帳_結束延遲.IsTimeOut())
            {
                List<object[]> list_過帳狀態 = this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(false);
                List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端_藥品資料.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                List<object[]> list_過帳明細_Add = new List<object[]>();
                list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.類別, enum_寫入報表設定_類別.其他.GetEnumName());
                list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.狀態, enum_過帳狀態.已產生排程.GetEnumName());
                list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.檔名, "藥庫消耗帳過帳");
                if (list_過帳狀態.Count > 0)
                {
                    if (Task_檢查藥品過消耗帳 == null)
                    {
                        Task_檢查藥品過消耗帳 = new Task(new Action(delegate { this.Function_藥品過消耗帳_指定日期過帳(DateTime.Now); }));
                    }
                    if (Task_檢查藥品過消耗帳.Status == TaskStatus.RanToCompletion)
                    {
                        Task_檢查藥品過消耗帳 = new Task(new Action(delegate { this.Function_藥品過消耗帳_指定日期過帳(DateTime.Now); }));
                    }
                    if (Task_檢查藥品過消耗帳.Status == TaskStatus.Created)
                    {
                        Task_檢查藥品過消耗帳.Start();
                    }
                    list_過帳狀態[0][(int)enum_過帳狀態列表.排程作業時間] = DateTime.Now.ToDateTimeString_6();
                    list_過帳狀態[0][(int)enum_過帳狀態列表.狀態] = enum_過帳狀態.排程已作業.GetEnumName();
                    this.sqL_DataGridView_過帳狀態列表.SQL_ReplaceExtra(list_過帳狀態[0], false);
                }


                cnt++;
            }
        }







        #endregion
        #region Fucntion
        private List<object[]> Function_藥品過消耗帳_取得所有過帳明細()
        {
            List<object[]> list_門診 = this.sqL_DataGridView_過帳明細_門診.SQL_GetAllRows(false);
            List<object[]> list_急診 = this.sqL_DataGridView_過帳明細_急診.SQL_GetAllRows(false);
            List<object[]> list_住院 = this.sqL_DataGridView_過帳明細_住院.SQL_GetAllRows(false);
            List<object[]> list_公藥 = this.sqL_DataGridView_過帳明細_公藥.SQL_GetAllRows(false);
            List<object[]> list_value = new List<object[]>();

            List<object[]> list_門診_buf = list_門診.CopyRows(new enum_過帳明細_門診(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_門診_buf.Count; i++)
            {
                list_門診_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.門診.GetEnumName();
            }
            List<object[]> list_急診_buf = list_急診.CopyRows(new enum_過帳明細_急診(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_急診_buf.Count; i++)
            {
                list_急診_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.急診.GetEnumName();
            }
            List<object[]> list_住院_buf = list_住院.CopyRows(new enum_過帳明細_住院(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_住院_buf.Count; i++)
            {
                list_住院_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.住院.GetEnumName();
            }
            List<object[]> list_公藥_buf = list_公藥.CopyRows(new enum_過帳明細_公藥(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_公藥_buf.Count; i++)
            {
                list_公藥_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.公藥.GetEnumName();
            }


            list_value.LockAdd(list_門診_buf);
            list_value.LockAdd(list_急診_buf);
            list_value.LockAdd(list_住院_buf);
            list_value.LockAdd(list_公藥_buf);

            return list_value;
        }
        private int Function_藥品過消耗帳_取得已消耗量(object[] value)
        {
            int value_out = 0;
            List<string> 效期 = new List<string>();
            List<string> 數量 = new List<string>();
            this.Function_藥品過消耗帳_取得效期數量(value, ref 效期, ref 數量);
            for(int i = 0; i < 數量.Count; i++)
            {
                if(數量[i].StringIsInt32())
                {
                    value_out += 數量[i].StringToInt32();
                }
            }
            return value_out;
        }
        private void Function_藥品過消耗帳_取得效期數量(object[] value, ref List<string> 效期, ref List<string> 數量)
        {
            string 備註 = value[(int)enum_藥品過消耗帳.備註].ObjectToString();
            備註 = 備註.Replace('\n', ',');
            效期 = 備註.GetTextValues("效期");
            數量 = 備註.GetTextValues("數量");
        }
        private void Function_藥品過消耗帳_設定過帳完成(List<object[]> list_藥品過消耗帳)
        {
            MyTimer MyTimer_TickTime = new MyTimer();
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(5000000);


            List<object[]> list_trading_value = new List<object[]>();
            List<DeviceBasic> deviceBasics = this.DeviceBasicClass_藥局.SQL_GetAllDeviceBasic();
            List<DeviceBasic> deviceBasics_buf = new List<DeviceBasic>();
            List<DeviceBasic> deviceBasics_Replace = new List<DeviceBasic>();
            List<object[]> list_ReplaceValue = new List<object[]>();
            List<object[]> list_儲位資訊 = new List<object[]>();
            string 儲位資訊_TYPE = "";
            string 儲位資訊_IP = "";
            string 儲位資訊_Num = "";
            string 儲位資訊_效期 = "";
            string 儲位資訊_批號 = "";
            string 儲位資訊_庫存 = "";
            string 儲位資訊_異動量 = "";
            string 儲位資訊_GUID = "";

            Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_藥品過消耗帳.Count);
            dialog_Prcessbar.State = "計算過帳內容中...";
            for (int i = 0; i < list_藥品過消耗帳.Count; i++)
            {
                dialog_Prcessbar.Value = i;
                //list_value[i] = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetRows(list_value[i]);
                if (list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_過帳明細_公藥_狀態.過帳完成.GetEnumName())
                {
                    continue;
                }
                string GUID = list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.GUID].ObjectToString();
                string 藥品碼 = list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.藥品碼].ObjectToString();
                string 藥品名稱 = list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.藥品名稱].ObjectToString();
                List<DeviceBasic> deviceBasic_buf = deviceBasics.SortByCode(藥品碼);
                if (deviceBasic_buf.Count == 0)
                {
                    list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.找無此藥品.GetEnumName();
                    list_ReplaceValue.Add(list_藥品過消耗帳[i]);
                    continue;
                }
                int 已消耗量 = this.Function_藥品過消耗帳_取得已消耗量(list_藥品過消耗帳[i]);
                int 需異動量 = list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.異動量].ObjectToString().StringToInt32();
                int 異動量 = 需異動量 - 已消耗量;
                if(異動量 == 0)
                {
                    continue;
                }
                int 庫存量 = deviceBasic_buf[0].Inventory.StringToInt32();
                int 結存量 = 庫存量 + 異動量;
                list_儲位資訊 = this.Function_取得異動儲位資訊(deviceBasic_buf[0], 異動量);
                string 備註 = list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.備註].ObjectToString();
                if (備註.StringIsEmpty() == false) 備註 = $"{備註}\n";
                if (deviceBasic_buf[0] != null)
                {
                    if (結存量 > 0)
                    {
                        if (庫存量 > 0)
                        {
                            for (int k = 0; k < list_儲位資訊.Count; k++)
                            {
                                this.Function_庫存異動(list_儲位資訊[k]);
                                this.Function_堆疊資料_取得儲位資訊內容(list_儲位資訊[k], ref 儲位資訊_GUID, ref 儲位資訊_TYPE, ref 儲位資訊_IP, ref 儲位資訊_Num, ref 儲位資訊_效期, ref 儲位資訊_批號, ref 儲位資訊_庫存, ref 儲位資訊_異動量);
                                if (儲位資訊_批號.StringIsEmpty()) 儲位資訊_批號 = "None";
                                備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量}";
                                if (k != list_儲位資訊.Count - 1) 備註 += "\n";
                            }
                            list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.過帳完成.GetEnumName();
                            list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.過帳時間] = DateTime.Now.ToDateTimeString_6();
                            list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.備註] = 備註;

                            object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                            value_trading[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                            value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                            value_trading[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.批次過帳.GetEnumName();
                            value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                            value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                            value_trading[(int)enum_交易記錄查詢資料.交易量] = 異動量;
                            value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                            value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;
                            value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.屏榮藥局.GetEnumName();
                            value_trading[(int)enum_交易記錄查詢資料.操作人] = "系統";
                            value_trading[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();
                            list_trading_value.Add(value_trading);
                        }
                        else
                        {
                            list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.無效期可入帳.GetEnumName();
                            list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.備註] = 備註;
                        }
                    }
                    else
                    {
                        //異動量 = 0;
                        //for (int k = 0; k < list_儲位資訊.Count; k++)
                        //{
                        //    this.Function_庫存異動(list_儲位資訊[k]);
                        //    this.Function_堆疊資料_取得儲位資訊內容(list_儲位資訊[k], ref 儲位資訊_GUID, ref 儲位資訊_TYPE, ref 儲位資訊_IP, ref 儲位資訊_Num, ref 儲位資訊_效期, ref 儲位資訊_批號, ref 儲位資訊_庫存, ref 儲位資訊_異動量);
                        //    if (儲位資訊_批號.StringIsEmpty()) 儲位資訊_批號 = "None";
                        //    備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量}";
                        //    if (k != list_儲位資訊.Count - 1) 備註 += "\n";
                        //    異動量 += 儲位資訊_異動量.StringToInt32();
                        //}
                        //結存量 = 庫存量 + 異動量;
                        //if(庫存量 == 0)
                        //{
                        //    continue;
                        //}
                        //list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.過帳時間] = DateTime.Now.ToDateTimeString_6();

                        //object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                        //value_trading[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                        //value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                        //value_trading[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.批次過帳.GetEnumName();
                        //value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                        //value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                        //value_trading[(int)enum_交易記錄查詢資料.交易量] = 異動量;
                        //value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                        //value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;
                        //value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.屏榮藥局.GetEnumName();
                        //value_trading[(int)enum_交易記錄查詢資料.操作人] = "系統";
                        //value_trading[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();
                        //list_trading_value.Add(value_trading);

                        list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.庫存不足.GetEnumName();
                        list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.備註] = 備註;
                    }
                }
                else
                {
                    list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.未建立儲位.GetEnumName();
                    list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.備註] = 備註;
                }
                deviceBasics_buf = (from value in deviceBasics_Replace
                                    where value.Code == 藥品碼
                                    select value).ToList();
                if (deviceBasics_buf.Count == 0)
                {
                    deviceBasics_Replace.Add(deviceBasic_buf[0]);
                }

                list_ReplaceValue.Add(list_藥品過消耗帳[i]);
                if (dialog_Prcessbar.DialogResult == DialogResult.No)
                {
                    return;
                }

            }
            dialog_Prcessbar.State = "上傳過帳內容...";
            this.DeviceBasicClass_藥局.SQL_ReplaceDeviceBasic(deviceBasics_Replace);
            dialog_Prcessbar.State = "更新過帳明細...";

            List<object[]> list_藥品過消耗帳_門診 = list_ReplaceValue.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.門診.GetEnumName());
            List<object[]> list_藥品過消耗帳_急診 = list_ReplaceValue.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.急診.GetEnumName());
            List<object[]> list_藥品過消耗帳_住院 = list_ReplaceValue.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.住院.GetEnumName());
            List<object[]> list_藥品過消耗帳_公藥 = list_ReplaceValue.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.公藥.GetEnumName());

            list_藥品過消耗帳_門診 = list_藥品過消耗帳_門診.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_門診());
            list_藥品過消耗帳_急診 = list_藥品過消耗帳_急診.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_急診());
            list_藥品過消耗帳_住院 = list_藥品過消耗帳_住院.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_住院());
            list_藥品過消耗帳_公藥 = list_藥品過消耗帳_公藥.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_公藥());

            this.sqL_DataGridView_過帳明細_門診.SQL_ReplaceExtra(list_藥品過消耗帳_門診, false);
            this.sqL_DataGridView_過帳明細_急診.SQL_ReplaceExtra(list_藥品過消耗帳_急診, false);
            this.sqL_DataGridView_過帳明細_住院.SQL_ReplaceExtra(list_藥品過消耗帳_住院, false);
            this.sqL_DataGridView_過帳明細_公藥.SQL_ReplaceExtra(list_藥品過消耗帳_公藥, false);


            this.sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_trading_value, false);
            this.sqL_DataGridView_藥品過消耗帳.ReplaceExtra(list_ReplaceValue, true);
            dialog_Prcessbar.Close();
            dialog_Prcessbar.Dispose();
            Console.Write($"藥品過消耗帳{list_藥品過消耗帳.Count}筆資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
        }
        private void Function_藥品過消耗帳_指定日期過帳(DateTime dateTime)
        {
            List<object[]> list_藥品過消耗帳 = this.Function_藥品過消耗帳_取得所有過帳明細();
            list_藥品過消耗帳.GetRowsInDate((int)enum_藥品過消耗帳.報表日期, dateTime);
            this.Function_藥品過消耗帳_設定過帳完成(list_藥品過消耗帳);
        }
        #endregion
        #region Event
        private void SqL_DataGridView_藥品過消耗帳_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].Cells[enum_藥品過消耗帳.狀態.GetEnumName()].Value.ToString();
                if (狀態 == enum_藥品過消耗帳_狀態.過帳完成.GetEnumName())
                {
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_藥品過消耗帳_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_藥品過消耗帳_狀態.未建立儲位.GetEnumName())
                {
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_藥品過消耗帳_狀態.無效期可入帳.GetEnumName())
                {
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.HotPink;
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void PlC_RJ_Button_藥品過消耗帳_顯示異常過帳_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥品過消耗帳_取得所有過帳明細();
            list_value = (from value in list_value
                          where (value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.庫存不足.GetEnumName())
                          || value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.找無此藥品.GetEnumName()
                           || value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.未建立儲位.GetEnumName()
                            || value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.忽略過帳.GetEnumName()
                             || value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.無效期可入帳.GetEnumName()
                          select value).ToList();
            list_value.Sort(new ICP_藥品過消耗帳());
            this.sqL_DataGridView_藥品過消耗帳.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥品過消耗帳_顯示今日消耗帳_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥品過消耗帳_取得所有過帳明細();

            list_value = list_value.GetRowsInDate((int)enum_藥品過消耗帳.報表日期, DateTime.Now);
            list_value.Sort(new ICP_藥品過消耗帳());
            this.sqL_DataGridView_藥品過消耗帳.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥品過消耗帳_指定報表日期_顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥品過消耗帳_取得所有過帳明細();

            list_value = list_value.GetRowsInDate((int)enum_藥品過消耗帳.報表日期, this.rJ_DatePicker_藥品過消耗帳_指定報表日期.Value);
            list_value.Sort(new ICP_藥品過消耗帳());
            this.sqL_DataGridView_藥品過消耗帳.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥品過消耗帳_選取資料設定過帳完成_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品過消耗帳 = this.sqL_DataGridView_藥品過消耗帳.Get_All_Checked_RowsValues();
         
            if(list_藥品過消耗帳.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }

            this.Function_藥品過消耗帳_設定過帳完成(list_藥品過消耗帳);


        }
        private void PlC_RJ_Button藥品過消耗帳_選取資料等待過帳_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品過消耗帳 = this.sqL_DataGridView_藥品過消耗帳.Get_All_Checked_RowsValues();
            if (list_藥品過消耗帳.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            for (int i = 0; i < list_藥品過消耗帳.Count; i++)
            {
                list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.等待過帳.GetEnumName();
                list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.備註] = "";
            }
            List<object[]> list_藥品過消耗帳_門診 = list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.門診.GetEnumName());
            List<object[]> list_藥品過消耗帳_急診 = list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.急診.GetEnumName());
            List<object[]> list_藥品過消耗帳_住院 = list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.住院.GetEnumName());
            List<object[]> list_藥品過消耗帳_公藥 = list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.公藥.GetEnumName());

            list_藥品過消耗帳_門診 = list_藥品過消耗帳_門診.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_門診());
            list_藥品過消耗帳_急診 = list_藥品過消耗帳_急診.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_急診());
            list_藥品過消耗帳_住院 = list_藥品過消耗帳_住院.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_住院());
            list_藥品過消耗帳_公藥 = list_藥品過消耗帳_公藥.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_公藥());

            this.sqL_DataGridView_過帳明細_門診.SQL_ReplaceExtra(list_藥品過消耗帳_門診, false);
            this.sqL_DataGridView_過帳明細_急診.SQL_ReplaceExtra(list_藥品過消耗帳_急診, false);
            this.sqL_DataGridView_過帳明細_住院.SQL_ReplaceExtra(list_藥品過消耗帳_住院, false);
            this.sqL_DataGridView_過帳明細_公藥.SQL_ReplaceExtra(list_藥品過消耗帳_公藥, false);


            this.sqL_DataGridView_藥品過消耗帳.ReplaceExtra(list_藥品過消耗帳, true);
        }
        private void PlC_RJ_Button_藥品過消耗帳_選取資料忽略過帳_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品過消耗帳 = this.sqL_DataGridView_藥品過消耗帳.Get_All_Checked_RowsValues();
            if (list_藥品過消耗帳.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            for (int i = 0; i < list_藥品過消耗帳.Count; i++)
            {
                list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.忽略過帳.GetEnumName();
                list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.備註] = "";
            }
            List<object[]> list_藥品過消耗帳_門診 = list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.門診.GetEnumName());
            List<object[]> list_藥品過消耗帳_急診 = list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.急診.GetEnumName());
            List<object[]> list_藥品過消耗帳_住院 = list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.住院.GetEnumName());
            List<object[]> list_藥品過消耗帳_公藥 = list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.公藥.GetEnumName());

            list_藥品過消耗帳_門診 = list_藥品過消耗帳_門診.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_門診());
            list_藥品過消耗帳_急診 = list_藥品過消耗帳_急診.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_急診());
            list_藥品過消耗帳_住院 = list_藥品過消耗帳_住院.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_住院());
            list_藥品過消耗帳_公藥 = list_藥品過消耗帳_公藥.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_公藥());

            this.sqL_DataGridView_過帳明細_門診.SQL_ReplaceExtra(list_藥品過消耗帳_門診, false);
            this.sqL_DataGridView_過帳明細_急診.SQL_ReplaceExtra(list_藥品過消耗帳_急診, false);
            this.sqL_DataGridView_過帳明細_住院.SQL_ReplaceExtra(list_藥品過消耗帳_住院, false);
            this.sqL_DataGridView_過帳明細_公藥.SQL_ReplaceExtra(list_藥品過消耗帳_公藥, false);


            this.sqL_DataGridView_藥品過消耗帳.ReplaceExtra(list_藥品過消耗帳, true);
        }
        private void PlC_RJ_Button_藥品過消耗帳_顯示所有資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥品過消耗帳_取得所有過帳明細();
            this.sqL_DataGridView_藥品過消耗帳.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥品過消耗帳_藥品碼篩選_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品過消耗帳.GetAllRows();

            list_value = list_value.GetRows((int)enum_藥品過消耗帳.藥品碼, rJ_TextBox_藥品過消耗帳_藥品碼篩選.Text);
            list_value.Sort(new ICP_藥品過消耗帳());

            this.sqL_DataGridView_藥品過消耗帳.RefreshGrid(list_value);
        }
        #endregion
        private class ICP_藥品過消耗帳 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                DateTime temp0 = x[(int)enum_藥品過消耗帳.報表日期].ToDateString().StringToDateTime();
                DateTime temp1 = y[(int)enum_藥品過消耗帳.報表日期].ToDateString().StringToDateTime();
                int cmp = temp0.CompareTo(temp1);
                if(cmp == 0)
                {
                    string str0 = x[(int)enum_藥品過消耗帳.來源名稱].ObjectToString();
                    string str1 = y[(int)enum_藥品過消耗帳.來源名稱].ObjectToString();

                    return str0.CompareTo(str1);
                }
                return cmp;
            }
        }
    }
}
