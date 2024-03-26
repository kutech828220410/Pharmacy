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
using HIS_DB_Lib;

namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        enum enum_過帳明細_公藥_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
            無效期可入帳,
            忽略過帳,
        }

        public enum enum_批次過帳_公藥_批次過帳明細
        {
            GUID,
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

        private void sub_Program_批次過帳_公藥_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_批次過帳_公藥_批次過帳明細, dBConfigClass.DB_posting_server);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.Init();
            if (!this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_CreateTable();
            }
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.DataGridRefreshEvent += SqL_DataGridView_批次過帳_公藥_批次過帳明細_DataGridRefreshEvent;
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.DataGridRowsChangeRefEvent += SqL_DataGridView_批次過帳_公藥_批次過帳明細_DataGridRowsChangeRefEvent;

            this.rJ_ComboBox_批次過帳_公藥_狀態.DataSource = new enum_過帳明細_公藥_狀態().GetEnumNames();

            this.plC_RJ_Button_批次過帳_公藥_全部顯示.MouseDownEvent += PlC_RJ_Button_批次過帳_公藥_全部顯示_MouseDownEvent;
            this.plC_RJ_Button_批次過帳_公藥_狀態_搜尋.MouseDownEvent += PlC_RJ_Button_批次過帳_公藥_狀態_搜尋_MouseDownEvent;
            this.plC_RJ_Button_批次過帳_公藥_選取資料過帳.MouseDownEvent += PlC_RJ_Button_批次過帳_公藥_選取資料過帳_MouseDownEvent;
            this.plC_RJ_Button_批次過帳_公藥_藥品碼_搜尋.MouseDownEvent += PlC_RJ_Button_批次過帳_公藥_藥品碼_搜尋_MouseDownEvent;
            this.plC_RJ_Button_批次過帳_公藥_藥品名稱_搜尋.MouseDownEvent += PlC_RJ_Button_批次過帳_公藥_藥品名稱_搜尋_MouseDownEvent;
            this.plC_RJ_Button_批次過帳_公藥_選取資料忽略過帳.MouseDownEvent += PlC_RJ_Button_批次過帳_公藥_選取資料忽略過帳_MouseDownEvent;
            this.plC_RJ_Button_批次過帳_公藥_選取資料等待過帳.MouseDownEvent += PlC_RJ_Button_批次過帳_公藥_選取資料等待過帳_MouseDownEvent;
            this.plC_UI_Init.Add_Method(sub_Program_批次過帳_公藥);
        }

        private void sub_Program_批次過帳_公藥()
        {

        }


        #region Event
        private void SqL_DataGridView_批次過帳_公藥_批次過帳明細_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            if (plC_RJ_ChechBox_批次過帳_公藥_報表日期.Checked)
            {
                RowsList = RowsList.GetRowsInDate((int)enum_批次過帳_公藥_批次過帳明細.報表日期, this.dateTimePicke_批次過帳_公藥_報表日期_起始, this.dateTimePicke_批次過帳_公藥_報表日期_結束);
            }

            if (plC_RJ_ChechBox_批次過帳_公藥_過帳日期.Checked)
            {
                RowsList = RowsList.GetRowsInDate((int)enum_批次過帳_公藥_批次過帳明細.過帳時間, this.dateTimePicke_批次過帳_公藥_產出日期_起始, this.dateTimePicke_批次過帳_公藥_產出日期_結束);
            }
            RowsList.Sort(new ICP_批次過帳_公藥_批次過帳明細());
        }
        private void SqL_DataGridView_批次過帳_公藥_批次過帳明細_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows[i].Cells[(int)enum_批次過帳_公藥_批次過帳明細.狀態 + 1].Value.ToString();
                if (狀態 == enum_過帳明細_公藥_狀態.過帳完成.GetEnumName())
                {
                    this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_過帳明細_公藥_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_過帳明細_公藥_狀態.未建立儲位.GetEnumName())
                {
                    this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_過帳明細_公藥_狀態.無效期可入帳.GetEnumName())
                {
                    this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.HotPink;
                    this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void PlC_RJ_Button_批次過帳_公藥_全部顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            if (plC_RJ_ChechBox_批次過帳_公藥_產出日期.Checked)
            {
                this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetRowsByBetween((int)enum_批次過帳_公藥_批次過帳明細.產出時間, this.dateTimePicke_批次過帳_公藥_產出日期_起始, this.dateTimePicke_批次過帳_公藥_產出日期_結束, true);
            }
            else
            {
                this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetAllRows(true);
            }

        }
        private void PlC_RJ_Button_批次過帳_公藥_狀態_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_批次過帳_公藥_批次過帳明細.狀態, this.rJ_ComboBox_批次過帳_公藥_狀態.Texts);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_批次過帳_公藥_藥品碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.rJ_TextBox_批次過帳_公藥_藥品碼.Texts.StringIsEmpty()) return;
            List<object[]> list_value = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_批次過帳_公藥_批次過帳明細.藥品碼, rJ_TextBox_批次過帳_公藥_藥品碼.Texts);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_批次過帳_公藥_藥品名稱_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.rJ_TextBox_批次過帳_公藥_藥品名稱.Texts.StringIsEmpty()) return;
            List<object[]> list_value = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_批次過帳_公藥_批次過帳明細.藥品名稱, rJ_TextBox_批次過帳_公藥_藥品名稱.Texts);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_批次過帳_公藥_選取資料過帳_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(5000000);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_LockTable();

            List<object[]> list_value = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.Get_All_Checked_RowsValues();
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
            if (list_value.Count == 0) return;

            Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_value.Count);
            dialog_Prcessbar.State = "計算過帳內容中...";
            for (int i = 0; i < list_value.Count; i++)
            {
                dialog_Prcessbar.Value = i;
                //list_value[i] = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetRows(list_value[i]);
                if (list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.狀態].ObjectToString() == enum_過帳明細_公藥_狀態.過帳完成.GetEnumName())
                {
                    continue;
                }
                string GUID = list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.GUID].ObjectToString();
                string 藥品碼 = list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.藥品碼].ObjectToString();
                string 藥品名稱 = list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.藥品名稱].ObjectToString();
                List<DeviceBasic> deviceBasic_buf = deviceBasics.SortByCode(藥品碼);
                if (deviceBasic_buf.Count == 0) continue;
                int 異動量 = list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.異動量].ObjectToString().StringToInt32();
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
                            list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.狀態] = enum_過帳明細_公藥_狀態.過帳完成.GetEnumName();
                            list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.過帳時間] = DateTime.Now.ToDateTimeString_6();
                            list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.備註] = 備註;

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
                            value_trading[(int)enum_交易記錄查詢資料.操作人] = this.登入者名稱;
                            value_trading[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();
                            list_trading_value.Add(value_trading);
                        }
                        else
                        {
                            list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.狀態] = enum_過帳明細_公藥_狀態.無效期可入帳.GetEnumName();
                            list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.備註] = 備註;
                        }
                    }
                    else
                    {
                        list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.狀態] = enum_過帳明細_公藥_狀態.庫存不足.GetEnumName();
                        list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.備註] = 備註;
                    }
                }
                else
                {
                    list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.狀態] = enum_過帳明細_公藥_狀態.未建立儲位.GetEnumName();
                    list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.備註] = 備註;
                }
                deviceBasics_buf = (from value in deviceBasics_Replace
                                    where value.Code == 藥品碼
                                    select value).ToList();
                if (deviceBasics_buf.Count == 0)
                {
                    deviceBasics_Replace.Add(deviceBasic_buf[0]);
                }

                list_ReplaceValue.Add(list_value[i]);
                if (dialog_Prcessbar.DialogResult == DialogResult.No)
                {
                    return;
                }

            }
            dialog_Prcessbar.State = "上傳過帳內容...";
            this.DeviceBasicClass_藥局.SQL_ReplaceDeviceBasic(deviceBasics_Replace);
            dialog_Prcessbar.State = "更新過帳明細...";
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_ReplaceExtra(list_ReplaceValue, false);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.ReplaceExtra(list_ReplaceValue, true);
            this.sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_trading_value, false);

            dialog_Prcessbar.Close();
            dialog_Prcessbar.Dispose();
            Console.Write($"公藥批次過帳{list_value.Count}筆資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_UnLockTable();
        }
        private void PlC_RJ_Button_批次過帳_公藥_選取資料忽略過帳_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否將選取資料設定為[忽略過帳]?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(5000000);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_LockTable();

            List<object[]> list_value = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.Get_All_Checked_RowsValues();

            List<object[]> list_ReplaceValue = new List<object[]>();

            if (list_value.Count == 0) return;

            Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_value.Count);
            dialog_Prcessbar.State = "計算過帳內容中...";
            for (int i = 0; i < list_value.Count; i++)
            {
                dialog_Prcessbar.Value = i;
                //list_value[i] = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetRows(list_value[i]);
                if (list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.狀態].ObjectToString() == enum_過帳明細_公藥_狀態.過帳完成.GetEnumName())
                {
                    continue;
                }
                list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.狀態] = enum_過帳明細_公藥_狀態.忽略過帳.GetEnumName();

                list_ReplaceValue.Add(list_value[i]);
                if (dialog_Prcessbar.DialogResult == DialogResult.No)
                {
                    return;
                }

            }
            dialog_Prcessbar.State = "更新過帳明細...";
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_ReplaceExtra(list_ReplaceValue, false);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.ReplaceExtra(list_ReplaceValue, true);

            dialog_Prcessbar.Close();
            dialog_Prcessbar.Dispose();
            Console.Write($"公藥批次過帳{list_value.Count}筆資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_UnLockTable();
        }
        private void PlC_RJ_Button_批次過帳_公藥_選取資料等待過帳_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否將選取資料設定為[等待過帳]?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(5000000);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_LockTable();

            List<object[]> list_value = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.Get_All_Checked_RowsValues();

            List<object[]> list_ReplaceValue = new List<object[]>();

            if (list_value.Count == 0) return;

            Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_value.Count);
            dialog_Prcessbar.State = "計算過帳內容中...";
            for (int i = 0; i < list_value.Count; i++)
            {
                dialog_Prcessbar.Value = i;
                //list_value[i] = this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetRows(list_value[i]);

                list_value[i][(int)enum_批次過帳_公藥_批次過帳明細.狀態] = enum_過帳明細_公藥_狀態.等待過帳.GetEnumName();

                list_ReplaceValue.Add(list_value[i]);
                if (dialog_Prcessbar.DialogResult == DialogResult.No)
                {
                    return;
                }

            }
            dialog_Prcessbar.State = "更新過帳明細...";
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_ReplaceExtra(list_ReplaceValue, false);
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.ReplaceExtra(list_ReplaceValue, true);

            dialog_Prcessbar.Close();
            dialog_Prcessbar.Dispose();
            Console.Write($"公藥批次過帳{list_value.Count}筆資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            this.sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_UnLockTable();
        }
        #endregion

        private class ICP_批次過帳_公藥_批次過帳明細 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                DateTime temp0 = x[(int)enum_批次過帳_公藥_批次過帳明細.報表日期].ToDateString().StringToDateTime();
                DateTime temp1 = y[(int)enum_批次過帳_公藥_批次過帳明細.報表日期].ToDateString().StringToDateTime();
                int cmp = temp0.CompareTo(temp1);
                return cmp;
            }
        }
    }
}
