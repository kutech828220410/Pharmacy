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
        private enum enum_藥品過消耗帳_匯出
        {
            來源名稱,
            來源報表,
            藥局代碼,
            藥品碼,
            藥品名稱,
            異動量,
            訂購單價,
            訂購總價,
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
            this.sqL_DataGridView_藥品過消耗帳.DataGridRowsChangeRefEvent += SqL_DataGridView_藥品過消耗帳_DataGridRowsChangeRefEvent;

            this.plC_RJ_Button_藥品過消耗帳_顯示今日消耗帳.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_顯示今日消耗帳_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_指定報表日期_顯示.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_指定報表日期_顯示_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_選取資料設定過帳完成.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_選取資料設定過帳完成_MouseDownEvent;
            this.plC_RJ_Button藥品過消耗帳_選取資料等待過帳.MouseDownEvent += PlC_RJ_Button藥品過消耗帳_選取資料等待過帳_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_選取資料忽略過帳.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_選取資料忽略過帳_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_顯示異常過帳.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_顯示異常過帳_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_藥品碼篩選.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_藥品碼篩選_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_藥品名稱篩選.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_藥品名稱篩選_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_顯示全部.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_全部資料匯出.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_全部資料匯出_MouseDownEvent;


            this.plC_UI_Init.Add_Method(this.sub_Program_藥品過消耗帳);
        }

 
        private void sub_Program_藥品過消耗帳()
        {
        }


        #region Fucntion

        private List<object[]> Function_藥品過消耗帳_取得所有過帳明細(DateTime dateTime1 , DateTime dateTime2)
        {
            List<object[]> list_門診 = this.sqL_DataGridView_批次過帳_門診_批次過帳明細.SQL_GetRowsByBetween((int)enum_藥品過消耗帳.報表日期, dateTime1.ToDateString(), dateTime2.ToDateString(), false);
            List<object[]> list_急診 = this.sqL_DataGridView_批次過帳_急診_批次過帳明細.SQL_GetRowsByBetween((int)enum_藥品過消耗帳.報表日期, dateTime1.ToDateString(), dateTime2.ToDateString(), false);
            List<object[]> list_住院 = this.sqL_DataGridView_批次過帳_住院_批次過帳明細.SQL_GetRowsByBetween((int)enum_藥品過消耗帳.報表日期, dateTime1.ToDateString(), dateTime2.ToDateString(), false);
            List<object[]> list_公藥 = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetRowsByBetween((int)enum_藥品過消耗帳.報表日期, dateTime1.ToDateString(), dateTime2.ToDateString(), false);
            List<object[]> list_value = new List<object[]>();

            List<object[]> list_門診_buf = list_門診.CopyRows(new enum_批次過帳_門診_批次過帳明細(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_門診_buf.Count; i++)
            {
                list_門診_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.門診.GetEnumName();
            }
            List<object[]> list_急診_buf = list_急診.CopyRows(new enum_批次過帳_急診_批次過帳明細(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_急診_buf.Count; i++)
            {
                list_急診_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.急診.GetEnumName();
            }
            List<object[]> list_住院_buf = list_住院.CopyRows(new enum_批次過帳_住院_批次過帳明細(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_住院_buf.Count; i++)
            {
                list_住院_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.住院.GetEnumName();
            }
            List<object[]> list_公藥_buf = list_公藥.CopyRows(new enum_批次過帳_公藥_批次過帳明細(), new enum_藥品過消耗帳());
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

        private List<object[]> Function_藥品過消耗帳_取得所有過帳明細()
        {
            List<object[]> list_門診 = this.sqL_DataGridView_批次過帳_門診_批次過帳明細.SQL_GetAllRows(false);
            List<object[]> list_急診 = this.sqL_DataGridView_批次過帳_急診_批次過帳明細.SQL_GetAllRows(false);
            List<object[]> list_住院 = this.sqL_DataGridView_批次過帳_住院_批次過帳明細.SQL_GetAllRows(false);
            List<object[]> list_公藥 = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetAllRows(false);
            List<object[]> list_value = new List<object[]>();

            List<object[]> list_門診_buf = list_門診.CopyRows(new enum_批次過帳_門診_批次過帳明細(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_門診_buf.Count; i++)
            {
                list_門診_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.門診.GetEnumName();
            }
            List<object[]> list_急診_buf = list_急診.CopyRows(new enum_批次過帳_急診_批次過帳明細(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_急診_buf.Count; i++)
            {
                list_急診_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.急診.GetEnumName();
            }
            List<object[]> list_住院_buf = list_住院.CopyRows(new enum_批次過帳_住院_批次過帳明細(), new enum_藥品過消耗帳());
            for (int i = 0; i < list_住院_buf.Count; i++)
            {
                list_住院_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.住院.GetEnumName();
            }
            List<object[]> list_公藥_buf = list_公藥.CopyRows(new enum_批次過帳_公藥_批次過帳明細(), new enum_藥品過消耗帳());
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
                if (list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.過帳完成.GetEnumName())
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
                int 異動量 = list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.異動量].ObjectToString().StringToInt32();
                int 庫存量 = deviceBasic_buf[0].Inventory.StringToInt32();
                int 結存量 = 庫存量 + 異動量;
                list_儲位資訊 = this.Function_取得異動儲位資訊(deviceBasic_buf[0], 異動量);
                string 備註 = "";

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

            list_藥品過消耗帳_門診 = list_藥品過消耗帳_門診.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_門診_批次過帳明細());
            list_藥品過消耗帳_急診 = list_藥品過消耗帳_急診.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_急診_批次過帳明細());
            list_藥品過消耗帳_住院 = list_藥品過消耗帳_住院.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_住院_批次過帳明細());
            list_藥品過消耗帳_公藥 = list_藥品過消耗帳_公藥.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_公藥_批次過帳明細());
            
            this.sqL_DataGridView_批次過帳_門診_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_門診, false);
            this.sqL_DataGridView_批次過帳_急診_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_急診, false);
            this.sqL_DataGridView_批次過帳_住院_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_住院, false);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_公藥, false);


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
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
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
                if (狀態 == enum_藥品過消耗帳_狀態.找無此藥品.GetEnumName())
                {
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Black;
                    this.sqL_DataGridView_藥品過消耗帳.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }
        private void SqL_DataGridView_藥品過消耗帳_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            List<object[]> list_value = new List<object[]>();

            if (checkBox_藥品過消耗帳_門診.Checked)
            {
                list_value.LockAdd(RowsList.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.門診.GetEnumName()));
                List<object[]> list_公藥 = RowsList.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.公藥.GetEnumName());
                list_公藥 = list_公藥.GetRows((int)enum_藥品過消耗帳.藥局代碼, "OPD");
                list_value.LockAdd(list_公藥);
            }
            if (checkBox_藥品過消耗帳_急診.Checked)
            {
                list_value.LockAdd(RowsList.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.急診.GetEnumName()));
                List<object[]> list_公藥 = RowsList.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.公藥.GetEnumName());
                list_公藥 = list_公藥.GetRows((int)enum_藥品過消耗帳.藥局代碼, "PHER");
                list_value.LockAdd(list_公藥);
            }
            if (checkBox_藥品過消耗帳_住院.Checked)
            {
                list_value.LockAdd(RowsList.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.住院.GetEnumName()));
                List<object[]> list_公藥 = RowsList.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.公藥.GetEnumName());
                list_公藥 = list_公藥.GetRows((int)enum_藥品過消耗帳.藥局代碼, "PHR");
                list_value.LockAdd(list_公藥);
            }
       
            RowsList = list_value;
        }
        private void PlC_RJ_Button_藥品過消耗帳_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥品過消耗帳_取得所有過帳明細(rJ_DatePicker_藥品過消耗帳_指定報表日期_起始.Value, rJ_DatePicker_藥品過消耗帳_指定報表日期_結束.Value);

            
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
        private void PlC_RJ_Button_藥品過消耗帳_全部資料匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    List<object[]> list_補給系統藥品資料 = sqL_DataGridView_藥品補給系統_藥品資料.SQL_GetAllRows(false);
                    List<object[]> list_補給系統藥品資料_buf = new List<object[]>();

                    List<object[]> list_藥品過消耗帳_src = this.sqL_DataGridView_藥品過消耗帳.GetAllRows();
                    List<object[]> list_藥品過消耗帳_out = list_藥品過消耗帳_src.CopyRows(new enum_藥品過消耗帳(), new enum_藥品過消耗帳_匯出());
                    for (int i = 0; i < list_藥品過消耗帳_out.Count; i++)
                    {
                        string 藥品碼 = list_藥品過消耗帳_out[i][(int)enum_藥品過消耗帳_匯出.藥品碼].ObjectToString();
                        list_補給系統藥品資料_buf = list_補給系統藥品資料.GetRows((int)enum_藥品補給系統_藥品資料.藥品碼, 藥品碼);
                        if (list_補給系統藥品資料_buf.Count > 0)
                        {
                            int 數量 = list_藥品過消耗帳_out[i][(int)enum_藥品過消耗帳_匯出.異動量].ObjectToString().StringToInt32();
                            數量 *= -1;
                            double 訂購單價 = list_補給系統藥品資料_buf[0][(int)enum_藥品補給系統_藥品資料.最新訂購單價].StringToDouble();
                            double 訂購總價 = 訂購單價 * 數量;
                            if(訂購單價 > 0)
                            {
                                list_藥品過消耗帳_out[i][(int)enum_藥品過消耗帳_匯出.訂購單價] = 訂購單價.ToString("0.000");
                                list_藥品過消耗帳_out[i][(int)enum_藥品過消耗帳_匯出.訂購總價] = 訂購總價.ToString("0.000");
                            }
                        }
                    }
                    DataTable dataTable = list_藥品過消耗帳_out.ToDataTable(new enum_藥品過消耗帳_匯出());
                    dataTable = dataTable.ReorderTable(new enum_藥品過消耗帳_匯出());

                    string Extension = System.IO.Path.GetExtension(this.saveFileDialog_SaveExcel.FileName);
                    if(Extension == ".txt")
                    {
                        CSVHelper.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                    }
                    else if (Extension == ".xls")
                    {
                        MyOffice.ExcelClass.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                    }

                    this.Cursor = Cursors.Default;
                    MyMessageBox.ShowDialog("匯出完成");
                }
            }));
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
        private void PlC_RJ_Button_藥品過消耗帳_選取資料設定過帳完成_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品過消耗帳 = this.sqL_DataGridView_藥品過消耗帳.Get_All_Checked_RowsValues();
            if (list_藥品過消耗帳.Count == 0)
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

            list_藥品過消耗帳_門診 = list_藥品過消耗帳_門診.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_門診_批次過帳明細());
            list_藥品過消耗帳_急診 = list_藥品過消耗帳_急診.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_急診_批次過帳明細());
            list_藥品過消耗帳_住院 = list_藥品過消耗帳_住院.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_住院_批次過帳明細());
            list_藥品過消耗帳_公藥 = list_藥品過消耗帳_公藥.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_公藥_批次過帳明細());

            this.sqL_DataGridView_批次過帳_門診_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_門診, false);
            this.sqL_DataGridView_批次過帳_急診_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_急診, false);
            this.sqL_DataGridView_批次過帳_住院_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_住院, false);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_公藥, false);


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

            list_藥品過消耗帳_門診 = list_藥品過消耗帳_門診.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_門診_批次過帳明細());
            list_藥品過消耗帳_急診 = list_藥品過消耗帳_急診.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_急診_批次過帳明細());
            list_藥品過消耗帳_住院 = list_藥品過消耗帳_住院.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_住院_批次過帳明細());
            list_藥品過消耗帳_公藥 = list_藥品過消耗帳_公藥.CopyRows(new enum_藥品過消耗帳(), new enum_批次過帳_公藥_批次過帳明細());

            this.sqL_DataGridView_批次過帳_門診_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_門診, false);
            this.sqL_DataGridView_批次過帳_急診_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_急診, false);
            this.sqL_DataGridView_批次過帳_住院_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_住院, false);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_ReplaceExtra(list_藥品過消耗帳_公藥, false);


            this.sqL_DataGridView_藥品過消耗帳.ReplaceExtra(list_藥品過消耗帳, true);
        }
        private void PlC_RJ_Button_藥品過消耗帳_藥品碼篩選_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品過消耗帳.GetAllRows();
            list_value = list_value.GetRowsByLike((int)enum_藥品過消耗帳.藥品碼, rJ_TextBox_藥品過消耗帳_藥品碼篩選.Text);
            this.sqL_DataGridView_藥品過消耗帳.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥品過消耗帳_藥品名稱篩選_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品過消耗帳.GetAllRows();
            list_value = list_value.GetRowsByLike((int)enum_藥品過消耗帳.藥品名稱, rJ_TextBox_藥品過消耗帳_藥品名稱篩選.Text);
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
                if (cmp == 0)
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
