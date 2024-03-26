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

namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        public enum enum_藥庫_入庫 : int
        {
            GUID,
            IP,
            編號,
            藥品碼,
            藥品名稱,
            中文名稱,
            儲位名稱,
            庫存,
            Value,
        }

        private void sub_Program_藥庫_入庫_Init()
        {

            this.sqL_DataGridView_藥庫_入庫_入庫狀態.Init(this.sqL_DataGridView_堆疊母資料);
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.Set_ColumnVisible(false, new enum_堆疊母資料().GetEnumNames());
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.Set_ColumnVisible(true, enum_堆疊母資料.藥品碼, enum_堆疊母資料.藥品名稱, enum_堆疊母資料.庫存量, enum_堆疊母資料.總異動量, enum_堆疊母資料.結存量, enum_堆疊母資料.效期, enum_堆疊母資料.狀態);
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.Set_ColumnWidth(80, enum_堆疊母資料.藥品碼);
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.Set_ColumnWidth(200, enum_堆疊母資料.藥品名稱);
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.Set_ColumnWidth(60, enum_堆疊母資料.總異動量);
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.Set_ColumnWidth(60, enum_堆疊母資料.結存量);
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.Set_ColumnWidth(100, enum_堆疊母資料.效期);
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.Set_ColumnWidth(100, enum_堆疊母資料.狀態);
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.DataGridRefreshEvent += SqL_DataGridView_藥庫_入庫_入庫狀態_DataGridRefreshEvent;
            this.sqL_DataGridView_藥庫_入庫_入庫狀態.DataGridRowsChangeRefEvent += SqL_DataGridView_藥庫_入庫_入庫狀態_DataGridRowsChangeRefEvent;

            this.sqL_DataGridView_藥庫_入庫_儲位搜尋.Init();
 
            this.plC_RJ_Button_藥庫_入庫_顯示全部.MouseDownEvent += PlC_RJ_Button_藥庫_入庫_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_藥庫_入庫_選擇儲位.MouseDownEvent += PlC_RJ_Button_藥庫_入庫_選擇儲位_MouseDownEvent;
            this.plC_RJ_Button_藥庫_入庫_入庫狀態_清除所有資料.MouseDownEvent += PlC_RJ_Button_藥庫_入庫_入庫狀態_清除所有資料_MouseDownEvent;
            this.plC_RJ_Button_藥庫_入庫_入庫狀態_清除選取資料.MouseDownEvent += PlC_RJ_Button_藥庫_入庫_入庫狀態_清除選取資料_MouseDownEvent;
            this.plC_RJ_Button_藥庫_入庫_藥品碼搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_入庫_藥品碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_入庫_藥品名稱搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_入庫_藥品名稱搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_入庫_藥品條碼搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_入庫_藥品條碼搜尋_MouseDownEvent;

            this.rJ_ComboBox_藥庫_入庫_入庫原因.Enter += RJ_ComboBox_藥庫_入庫_入庫原因_Enter;

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_入庫);
        }

  

        private bool flag_藥庫_入庫 = false;
        private void sub_Program_藥庫_入庫()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "入庫")
            {
                if (!this.flag_藥庫_入庫)
                {
                    this.Function_堆疊資料_刪除指定調劑台名稱母資料("藥庫");
                    this.flag_藥庫_入庫 = true;
                }
               
            }
            else
            {
                this.flag_藥庫_入庫 = false;
            }
            this.sub_Program_藥庫_入庫_狀態更新();
        }

        #region Function
        private List<object[]> Function_藥庫_入庫_取得儲位資訊()
        {
            List<object[]> list_儲位資訊 = new List<object[]>();
            List<Device> devices = this.Function_從SQL取得所有儲位();

            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString().StringIsEmpty()) continue;
                object[] value = new object[new enum_藥庫_入庫().GetLength()];
                value[(int)enum_藥庫_入庫.GUID] = devices[i].GUID;
                value[(int)enum_藥庫_入庫.IP] = devices[i].GetValue(Device.ValueName.IP, Device.ValueType.Value).ObjectToString();
                value[(int)enum_藥庫_入庫.藥品碼] = devices[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                value[(int)enum_藥庫_入庫.藥品名稱] = devices[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                value[(int)enum_藥庫_入庫.中文名稱] = devices[i].GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString();
                value[(int)enum_藥庫_入庫.儲位名稱] = devices[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                value[(int)enum_藥庫_入庫.庫存] = devices[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
                value[(int)enum_藥庫_入庫.Value] = devices[i];
                list_儲位資訊.Add(value);
            }
            list_儲位資訊.Sort(new ICP_藥庫_入庫_儲位資訊());
            for (int i = 0; i < list_儲位資訊.Count; i++)
            {
                list_儲位資訊[i][(int)enum_藥庫_入庫.編號] = (i + 1).ToString();
            }
            return list_儲位資訊;
        }
        #endregion

        #region PLC_藥庫_入庫_狀態更新
        PLC_Device PLC_Device_藥庫_入庫_狀態更新 = new PLC_Device("");
        PLC_Device PLC_Device_藥庫_入庫_狀態更新_OK = new PLC_Device("");
        MyTimer MyTimer_藥庫_入庫_狀態更新_刷新時間 = new MyTimer();
        int cnt_Program_藥庫_入庫_狀態更新 = 65534;
        void sub_Program_藥庫_入庫_狀態更新()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "入庫")
            {
                PLC_Device_藥庫_入庫_狀態更新.Bool = true;
            }
            else
            {
                PLC_Device_藥庫_入庫_狀態更新.Bool = false;
            }
            if (cnt_Program_藥庫_入庫_狀態更新 == 65534)
            {
                PLC_Device_藥庫_入庫_狀態更新.SetComment("PLC_藥庫_入庫_狀態更新");
                PLC_Device_藥庫_入庫_狀態更新_OK.SetComment("PLC_藥庫_入庫_狀態更新_OK");
                PLC_Device_藥庫_入庫_狀態更新.Bool = false;
                cnt_Program_藥庫_入庫_狀態更新 = 65535;
            }
            if (cnt_Program_藥庫_入庫_狀態更新 == 65535) cnt_Program_藥庫_入庫_狀態更新 = 1;
            if (cnt_Program_藥庫_入庫_狀態更新 == 1) cnt_Program_藥庫_入庫_狀態更新_檢查按下(ref cnt_Program_藥庫_入庫_狀態更新);
            if (cnt_Program_藥庫_入庫_狀態更新 == 2) cnt_Program_藥庫_入庫_狀態更新_初始化(ref cnt_Program_藥庫_入庫_狀態更新);
            if (cnt_Program_藥庫_入庫_狀態更新 == 3) cnt_Program_藥庫_入庫_狀態更新 = 65500;
            if (cnt_Program_藥庫_入庫_狀態更新 > 1) cnt_Program_藥庫_入庫_狀態更新_檢查放開(ref cnt_Program_藥庫_入庫_狀態更新);

            if (cnt_Program_藥庫_入庫_狀態更新 == 65500)
            {
                PLC_Device_藥庫_入庫_狀態更新.Bool = false;
                PLC_Device_藥庫_入庫_狀態更新_OK.Bool = false;
                cnt_Program_藥庫_入庫_狀態更新 = 65535;
            }
        }
        void cnt_Program_藥庫_入庫_狀態更新_檢查按下(ref int cnt)
        {
            if (PLC_Device_藥庫_入庫_狀態更新.Bool) cnt++;
        }
        void cnt_Program_藥庫_入庫_狀態更新_檢查放開(ref int cnt)
        {
            if (!PLC_Device_藥庫_入庫_狀態更新.Bool) cnt = 65500;
        }
        void cnt_Program_藥庫_入庫_狀態更新_初始化(ref int cnt)
        {
            MyTimer_藥庫_入庫_狀態更新_刷新時間.StartTickTime(200);
            if (MyTimer_藥庫_入庫_狀態更新_刷新時間.IsTimeOut())
            {
                List<object[]> list_value = sqL_DataGridView_藥庫_入庫_入庫狀態.SQL_GetAllRows(true);
            }

            cnt++;
        }



        #endregion
        #region Event
        private void SqL_DataGridView_藥庫_入庫_入庫狀態_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new Icp_堆疊母資料_index排序());
        }
        private void SqL_DataGridView_藥庫_入庫_入庫狀態_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_藥庫_入庫_入庫狀態.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_藥庫_入庫_入庫狀態.dataGridView.Rows[i].Cells[(int)enum_堆疊母資料.狀態].Value.ToString();
                if (狀態 == enum_堆疊母資料_狀態.等待作業.GetEnumName())
                {
                    this.sqL_DataGridView_藥庫_入庫_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_藥庫_入庫_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_堆疊母資料_狀態.入賬完成.GetEnumName())
                {
                    this.sqL_DataGridView_藥庫_入庫_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    this.sqL_DataGridView_藥庫_入庫_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_堆疊母資料_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_藥庫_入庫_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_藥庫_入庫_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void RJ_ComboBox_藥庫_入庫_入庫原因_Enter(object sender, EventArgs e)
        {
            this.rJ_ComboBox_藥庫_入庫_入庫原因.SetDataSource(this.sqL_DataGridView_入庫原因維護.SQL_GetAllRows(false).GetSelectColumnValue((int)enum_入庫原因維護.內容));
        }
        private void PlC_RJ_Button_藥庫_入庫_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_儲位資訊 = this.Function_藥庫_入庫_取得儲位資訊();
            this.sqL_DataGridView_藥庫_入庫_儲位搜尋.RefreshGrid(list_儲位資訊);
        }
        private void PlC_RJ_Button_藥庫_入庫_選擇儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_儲位資訊 = this.sqL_DataGridView_藥庫_入庫_儲位搜尋.Get_All_Select_RowsValues();

            if (list_儲位資訊.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取儲位!");
                }));
                return;
            }
            object device_object = list_儲位資訊[0][(int)enum_藥庫_入庫.Value];
            if (!(device_object is Device)) return;
            Device device = device_object as Device;
 
            string 寫入效期 = "";
            string 寫入批號 = "";
            string 寫入數量 = "0";
            Dialog_DateTime dialog_DateTime = new Dialog_DateTime();
            if (dialog_DateTime.ShowDialog() == DialogResult.Yes)
            {
                寫入效期 = dialog_DateTime.Value.ToDateString();
            }
            else
            {
                return;
            }
            if (device.取得庫存(寫入效期) == -1)
            {
                Dialog_寫入批號 dialog_寫入批號 = new Dialog_寫入批號();
                if (dialog_寫入批號.ShowDialog() == DialogResult.Yes)
                {
                    寫入批號 = dialog_寫入批號.Value;
                }
                else
                {
                    return;
                }
            }

            //Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
            //if (dialog_NumPannel.ShowDialog() == DialogResult.Yes)
            //{
            //    寫入數量 = dialog_NumPannel.Value.ToString();
            //}
            //else
            //{
            //    return;
            //}

            Color color = 登入者顏色.ToColor();
            if (color == Color.Black) 登入者顏色 = Color.White.ToColorString();
            string GUID = Guid.NewGuid().ToString();
            string 調劑台名稱 = "藥庫";
            enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.入庫作業;
            string 藥品碼 = device.Code;
            string 藥品名稱 = device.Name;
            string 藥袋序號 = "";
            string 單位 = device.Package;
            string 病歷號 = "";
            string 病人姓名 = "";
            string 開方時間 = DateTime.Now.ToDateTimeString_6();
            string IP = device.IP;
            string 操作人 = 登入者名稱;
            string 顏色 = 登入者顏色;
            int 總異動量 = 寫入數量.StringToInt32();
            string 效期 = 寫入效期;
            string 批號 = 寫入批號;
            string 備註 = $"[原因]:{this.rJ_ComboBox_藥庫_入庫_入庫原因.Text} ,";
            this.Function_堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, IP, 操作人, 顏色, 總異動量, 效期, 批號, 備註);
        }
        private void PlC_RJ_Button_藥庫_入庫_入庫狀態_清除選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_入庫_入庫狀態.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取資料!");
                }));
                return;
            }
            for (int i = 0; i < list_value.Count; i++)
            {
                string GUID = list_value[i][(int)enum_堆疊母資料.GUID].ObjectToString();
                this.Function_堆疊資料_刪除母資料(GUID);
            }
        }
        private void PlC_RJ_Button_藥庫_入庫_入庫狀態_清除所有資料_MouseDownEvent(MouseEventArgs mevent)
        {
            string 庫別 = "藥庫";
            this.Function_堆疊資料_刪除指定調劑台名稱母資料(庫別);
        }
        private void PlC_RJ_Button_藥庫_入庫_藥品碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if(this.rJ_TextBox_藥庫_入庫_藥品碼搜尋.Texts.StringIsEmpty())
            {
                return;
            }
            List<object[]> list_儲位資訊 = this.Function_藥庫_入庫_取得儲位資訊();
            list_儲位資訊 = list_儲位資訊.GetRowsByLike((int)enum_藥庫_入庫.藥品碼, this.rJ_TextBox_藥庫_入庫_藥品碼搜尋.Texts);
            list_儲位資訊.Sort(new ICP_藥庫_入庫_儲位資訊());
            for (int i = 0; i < list_儲位資訊.Count; i++)
            {
                list_儲位資訊[i][(int)enum_藥庫_入庫.編號] = (i + 1).ToString();
            }
            this.sqL_DataGridView_藥庫_入庫_儲位搜尋.RefreshGrid(list_儲位資訊);
        }
        private void PlC_RJ_Button_藥庫_入庫_藥品名稱搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.rJ_TextBox_藥庫_入庫_藥品名稱搜尋.Texts.StringIsEmpty())
            {
                return;
            }
            List<object[]> list_儲位資訊 = this.Function_藥庫_入庫_取得儲位資訊();
            list_儲位資訊 = list_儲位資訊.GetRowsByLike((int)enum_藥庫_入庫.藥品名稱, this.rJ_TextBox_藥庫_入庫_藥品名稱搜尋.Texts);
            list_儲位資訊.Sort(new ICP_藥庫_入庫_儲位資訊());
            for (int i = 0; i < list_儲位資訊.Count; i++)
            {
                list_儲位資訊[i][(int)enum_藥庫_入庫.編號] = (i + 1).ToString();
            }
            this.sqL_DataGridView_藥庫_入庫_儲位搜尋.RefreshGrid(list_儲位資訊);
        }
        private void PlC_RJ_Button_藥庫_入庫_藥品條碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品資訊 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資訊_buf01 = new List<object[]>();
            List<object[]> list_藥品資訊_buf02 = new List<object[]>();
            if (this.rJ_TextBox_藥庫_入庫_藥品條碼搜尋.Texts.StringIsEmpty())
            {
                return;
            }
            list_藥品資訊_buf01 = list_藥品資訊.GetRows((int)enum_藥庫_藥品資料.藥品條碼1, this.rJ_TextBox_藥庫_入庫_藥品條碼搜尋.Texts);
            list_藥品資訊_buf02 = list_藥品資訊.GetRows((int)enum_藥庫_藥品資料.藥品條碼2, this.rJ_TextBox_藥庫_入庫_藥品條碼搜尋.Texts);
            if (list_藥品資訊_buf01.Count == 0 && list_藥品資訊_buf02.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("找無此藥品條碼藥品!");
                }));
                return;
            }
            List<object[]> list_儲位資訊 = this.Function_藥庫_入庫_取得儲位資訊();
            List<object[]> list_儲位資訊_buf = new List<object[]>();
            if (list_藥品資訊_buf01.Count > 0)
            {
                list_儲位資訊_buf = list_儲位資訊.GetRows((int)enum_藥庫_入庫.藥品碼, list_藥品資訊_buf01[0][(int)enum_藥庫_藥品資料.藥品碼].ObjectToString());
                if(list_儲位資訊_buf.Count > 0)
                {
                    list_儲位資訊_buf.Sort(new ICP_藥庫_入庫_儲位資訊());
                    for (int i = 0; i < list_儲位資訊_buf.Count; i++)
                    {
                        list_儲位資訊_buf[i][(int)enum_藥庫_入庫.編號] = (i + 1).ToString();
                    }
                    this.sqL_DataGridView_藥庫_入庫_儲位搜尋.RefreshGrid(list_儲位資訊_buf);
                    return;
                }         
            }
            if (list_藥品資訊_buf02.Count > 0)
            {
                list_儲位資訊_buf = list_儲位資訊.GetRows((int)enum_藥庫_入庫.藥品碼, list_藥品資訊_buf02[0][(int)enum_藥庫_藥品資料.藥品碼].ObjectToString());
                if (list_儲位資訊_buf.Count > 0)
                {
                    list_儲位資訊_buf.Sort(new ICP_藥庫_入庫_儲位資訊());
                    for (int i = 0; i < list_儲位資訊_buf.Count; i++)
                    {
                        list_儲位資訊_buf[i][(int)enum_藥庫_入庫.編號] = (i + 1).ToString();
                    }
                    this.sqL_DataGridView_藥庫_入庫_儲位搜尋.RefreshGrid(list_儲位資訊_buf);
                    return;
                }
            }
        }


        #endregion

        private class ICP_藥庫_入庫_儲位資訊 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string str01 = x[(int)enum_藥庫_入庫.藥品碼].ObjectToString();
                string str02 = y[(int)enum_藥庫_入庫.藥品碼].ObjectToString();
                int cmp = str01.CompareTo(str02);
                if(cmp == 0)
                {
                    string IP_0 = x[(int)enum_藥庫_入庫.IP].ObjectToString();
                    string IP_1 = y[(int)enum_藥庫_入庫.IP].ObjectToString();
                    string[] IP_0_Array = IP_0.Split('.');
                    string[] IP_1_Array = IP_1.Split('.');
                    IP_0 = "";
                    IP_1 = "";
                    for (int i = 0; i < 4; i++)
                    {
                        if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                        if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                        if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];

                        if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                        if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                        if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];

                        IP_0 += IP_0_Array[i];
                        IP_1 += IP_1_Array[i];
                    }
                    cmp = IP_0_Array[2].CompareTo(IP_1_Array[2]);
                    if (cmp > 0)
                    {
                        return 1;
                    }
                    else if (cmp < 0)
                    {
                        return -1;
                    }
                    else if (cmp == 0)
                    {
                        cmp = IP_0_Array[3].CompareTo(IP_1_Array[3]);
                        if (cmp > 0)
                        {
                            return 1;
                        }
                        else if (cmp < 0)
                        {
                            return -1;
                        }
                        else if (cmp == 0)
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return cmp;
                }
                return 0;

            }
        }
    }
}
