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
using Basic;
using MyUI;
namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
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

        private enum enum_過帳明細_公藥
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

        private void sub_Program_過帳明細_公藥_Init()
        {
            SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_過帳明細_公藥, this.dBConfigClass.DB_Basic);
            this.sqL_DataGridView_過帳明細_公藥.Init();
            if (!this.sqL_DataGridView_過帳明細_公藥.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_過帳明細_公藥.SQL_CreateTable();
            }

            this.sqL_DataGridView_過帳明細_公藥.DataGridRowsChangeRefEvent += SqL_DataGridView_過帳明細_公藥_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_過帳明細_公藥.DataGridRefreshEvent += SqL_DataGridView_過帳明細_公藥_DataGridRefreshEvent;

            this.rJ_ComboBox_過帳明細_公藥_狀態.DataSource = new enum_過帳明細_公藥_狀態().GetEnumNames();

            this.plC_RJ_Button_過帳明細_公藥_檢查過帳狀態.MouseDownEvent += PlC_RJ_Button_過帳明細_公藥_檢查過帳狀態_MouseDownEvent;
            this.plC_RJ_Button_過帳明細_公藥_顯示全部.MouseDownEvent += PlC_RJ_Button_過帳明細_公藥_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_過帳明細_公藥_刪除.MouseDownEvent += PlC_RJ_Button_過帳明細_公藥_刪除_MouseDownEvent;
            this.plC_RJ_Button_過帳明細_公藥_搜尋.MouseDownEvent += PlC_RJ_Button_過帳明細_公藥_搜尋_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_過帳明細_公藥);
        }

        private void sub_Program_過帳明細_公藥()
        {
            this.sub_Program_檢查過帳明細_公藥();
        }
        #region PLC_檢查過帳明細_公藥
        Task Task_檢查過帳明細_公藥;

        PLC_Device PLC_Device_檢查過帳明細_公藥 = new PLC_Device("");
        PLC_Device PLC_Device_檢查過帳明細_公藥_OK = new PLC_Device("");
        MyTimer MyTimer_檢查過帳明細_公藥_結束延遲 = new MyTimer();
        int cnt_Program_檢查過帳明細_公藥 = 65534;
        void sub_Program_檢查過帳明細_公藥()
        {
            PLC_Device_檢查過帳明細_公藥.Bool = true;
            if (cnt_Program_檢查過帳明細_公藥 == 65534)
            {
                this.MyTimer_檢查過帳明細_公藥_結束延遲.StartTickTime(10000);
                PLC_Device_檢查過帳明細_公藥.SetComment("PLC_檢查過帳明細_公藥");
                PLC_Device_檢查過帳明細_公藥_OK.SetComment("PLC_檢查過帳明細_公藥_OK");
                PLC_Device_檢查過帳明細_公藥.Bool = false;
                cnt_Program_檢查過帳明細_公藥 = 65535;
            }
            if (cnt_Program_檢查過帳明細_公藥 == 65535) cnt_Program_檢查過帳明細_公藥 = 1;
            if (cnt_Program_檢查過帳明細_公藥 == 1) cnt_Program_檢查過帳明細_公藥_檢查按下(ref cnt_Program_檢查過帳明細_公藥);
            if (cnt_Program_檢查過帳明細_公藥 == 2) cnt_Program_檢查過帳明細_公藥_初始化(ref cnt_Program_檢查過帳明細_公藥);
            if (cnt_Program_檢查過帳明細_公藥 == 3) cnt_Program_檢查過帳明細_公藥 = 65500;
            if (cnt_Program_檢查過帳明細_公藥 > 1) cnt_Program_檢查過帳明細_公藥_檢查放開(ref cnt_Program_檢查過帳明細_公藥);

            if (cnt_Program_檢查過帳明細_公藥 == 65500)
            {
                this.MyTimer_檢查過帳明細_公藥_結束延遲.TickStop();
                this.MyTimer_檢查過帳明細_公藥_結束延遲.StartTickTime(60000);
                PLC_Device_檢查過帳明細_公藥.Bool = false;
                PLC_Device_檢查過帳明細_公藥_OK.Bool = false;
                cnt_Program_檢查過帳明細_公藥 = 65535;
            }
        }
        void cnt_Program_檢查過帳明細_公藥_檢查按下(ref int cnt)
        {
            if (PLC_Device_檢查過帳明細_公藥.Bool) cnt++;
        }
        void cnt_Program_檢查過帳明細_公藥_檢查放開(ref int cnt)
        {
            if (!PLC_Device_檢查過帳明細_公藥.Bool) cnt = 65500;
        }
        void cnt_Program_檢查過帳明細_公藥_初始化(ref int cnt)
        {
            if (this.MyTimer_檢查過帳明細_公藥_結束延遲.IsTimeOut())
            {
                if (Task_檢查過帳明細_公藥 == null)
                {
                    Task_檢查過帳明細_公藥 = new Task(new Action(delegate { this.PlC_RJ_Button_過帳明細_公藥_檢查過帳狀態_MouseDownEvent(null); }));
                }
                if (Task_檢查過帳明細_公藥.Status == TaskStatus.RanToCompletion)
                {
                    Task_檢查過帳明細_公藥 = new Task(new Action(delegate { this.PlC_RJ_Button_過帳明細_公藥_檢查過帳狀態_MouseDownEvent(null); }));
                }
                if (Task_檢查過帳明細_公藥.Status == TaskStatus.Created)
                {
                    Task_檢查過帳明細_公藥.Start();
                }

                cnt++;
            }
        }







        #endregion

        #region Event
        private void SqL_DataGridView_過帳明細_公藥_DataGridRefreshEvent()
        {

        }
        private void SqL_DataGridView_過帳明細_公藥_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            if (plC_RJ_ChechBox_過帳明細_公藥_報表日期.Checked)
            {
                RowsList = RowsList.GetRowsInDate((int)enum_過帳明細_公藥.報表日期, this.dateTimePicke_過帳狀態列表_公藥_報表日期_起始, this.dateTimePicke_過帳狀態列表_公藥_報表日期_結束);
            }
            if (plC_RJ_ChechBox_過帳明細_公藥_產出日期.Checked)
            {
                RowsList = RowsList.GetRowsInDate((int)enum_過帳明細_公藥.產出時間, this.dateTimePicke_過帳狀態列表_公藥_產出日期_起始, this.dateTimePicke_過帳狀態列表_公藥_產出日期_結束);
            }
            if (plC_RJ_ChechBox_過帳明細_公藥_過帳日期.Checked)
            {
                RowsList = RowsList.GetRowsInDate((int)enum_過帳明細_公藥.過帳時間, this.dateTimePicke_過帳狀態列表_公藥_產出日期_起始, this.dateTimePicke_過帳狀態列表_公藥_產出日期_結束);
            }
            RowsList.Sort(new ICP_過帳明細_公藥());
        }

        private void PlC_RJ_Button_過帳明細_公藥_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.rJ_ComboBox_過帳明細_公藥_狀態.Texts.StringIsEmpty()) return;
            List<object[]> list_value = this.sqL_DataGridView_過帳明細_公藥.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_過帳明細_公藥.狀態, this.rJ_ComboBox_過帳明細_公藥_狀態.Texts);
            this.sqL_DataGridView_過帳明細_公藥.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_過帳明細_公藥_檢查過帳狀態_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);

            List<object[]> list_寫入報表設定 = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
            List<object[]> list_過帳狀態 = this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(false);
            List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            List<object[]> list_過帳明細_Add = new List<object[]>();
            list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.類別, enum_寫入報表設定_類別.公藥_消耗帳.GetEnumName());
            list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.狀態, enum_過帳狀態.已產生排程.GetEnumName());
            for (int i = 0; i < list_過帳狀態.Count; i++)
            {
                string filename = $@"{list_過帳狀態[i][(int)enum_過帳狀態列表.檔案位置]}\{list_過帳狀態[i][(int)enum_過帳狀態列表.檔名]}";

                List<string> list_text = MyFileStream.LoadFile(filename);
                if (list_text == null) continue;
                string 來源報表 = list_過帳狀態[i][(int)enum_過帳狀態列表.檔名].ObjectToString();
                string 藥品碼 = "";
                string 藥品名稱 = "";
                string 藥局代碼 = "";
                string 報表日期 = list_過帳狀態[i][(int)enum_過帳狀態列表.報表日期].ToDateString();
                string 報表產出日期 = "";
                string 異動量 = "";
                string 產出時間 = "";
                string 過帳時間 = "";
                string 已過帳 = "";
                list_過帳狀態[i][(int)enum_過帳狀態列表.狀態] = enum_過帳狀態.排程已作業.GetEnumName();
                list_過帳狀態[i][(int)enum_過帳狀態列表.排程作業時間] = DateTime.Now.ToDateTimeString_6();

                for (int k = 0; k < list_text.Count; k++)
                {

                    object[] value = new object[new enum_過帳明細_公藥().GetLength()];
                    this.Function_過帳明細_公藥_解析TXT(list_text[k], ref 藥品碼, ref 藥局代碼, ref 報表產出日期, ref 異動量);

                    if (異動量.StringIsInt32() == false)
                    {
                        object[] log = new object[new enum_Log().GetLength()];
                        log[(int)enum_Log.GUID] = Guid.NewGuid().ToString();
                        log[(int)enum_Log.內容] = $"[事件內容]:異動量異常 '{異動量}',[來源報表]:{來源報表},[藥品碼]:{藥品碼},[藥局代碼]:{藥局代碼},[報表日期]:{報表日期}";
                        log[(int)enum_Log.時間] = DateTime.Now.ToDateTimeString_6();
                        this.sqL_DataGridView_Log.SQL_AddRow(log, false);
                        continue;
                    }


                    value[(int)enum_過帳明細_公藥.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_過帳明細_公藥.來源報表] = 來源報表;
                    value[(int)enum_過帳明細_公藥.藥品碼] = 藥品碼;
                    value[(int)enum_過帳明細_公藥.藥局代碼] = 藥局代碼;
                    value[(int)enum_過帳明細_公藥.報表日期] = 報表日期;
                    value[(int)enum_過帳明細_公藥.異動量] = 異動量.StringToInt32() * -1;
                    value[(int)enum_過帳明細_公藥.產出時間] = DateTime.Now.ToDateTimeString_6();
                    value[(int)enum_過帳明細_公藥.過帳時間] = DateTime.MinValue.ToDateTimeString_6();
                    value[(int)enum_過帳明細_公藥.狀態] = enum_過帳明細_公藥_狀態.等待過帳.GetEnumName();

                    list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端_藥品資料.藥品碼, 藥品碼);
                    if (list_藥品資料_buf.Count == 0)
                    {
                        object[] log = new object[new enum_Log().GetLength()];
                        log[(int)enum_Log.GUID] = Guid.NewGuid().ToString();
                        log[(int)enum_Log.內容] = $"[事件內容]:找無此藥品,[來源報表]:{來源報表},[藥品碼]:{藥品碼},[藥局代碼]:{藥局代碼},[報表日期]:{報表日期}";
                        log[(int)enum_Log.時間] = DateTime.Now.ToDateTimeString_6();
                        this.sqL_DataGridView_Log.SQL_AddRow(log, false);
                        value[(int)enum_過帳明細_公藥.狀態] = enum_過帳明細_公藥_狀態.找無此藥品.GetEnumName();
                    }
                    else
                    {
                        value[(int)enum_過帳明細_公藥.藥品名稱] = list_藥品資料_buf[0][(int)enum_雲端_藥品資料.藥品名稱];
                    }

                    list_過帳明細_Add.Add(value);
                }

            }
            this.sqL_DataGridView_過帳狀態列表.SQL_ReplaceExtra(list_過帳狀態, false);
            this.sqL_DataGridView_過帳明細_公藥.SQL_AddRows(list_過帳明細_Add, false);

            Console.WriteLine($"檢查過帳明細-公藥 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
        }
        private void PlC_RJ_Button_過帳明細_公藥_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_過帳明細_公藥.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_過帳明細_公藥_刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_過帳明細_公藥.Get_All_Select_RowsValues();
            this.sqL_DataGridView_過帳明細_公藥.SQL_DeleteExtra(list_value, true);
        }

        #endregion

        private void Function_過帳明細_公藥_解析TXT(string text, ref string 藥品碼, ref string 藥局代碼, ref string Date, ref string 異動量)
        {
            int len = text.Length;
            if (len == 28)
            {
                藥品碼 = text.Substring(0, 5);
                藥局代碼 = text.Substring(5, 4).Replace(" ", "");
                string Year = text.Substring(9, 4);
                string Month = text.Substring(13, 2);
                string Day = text.Substring(15, 2);
                string Hour = text.Substring(17, 2);
                string Min = text.Substring(19, 2);
                Date = $"{Year}/{Month}/{Day} {Hour}:{Min}:00";
                異動量 = text.Substring(21, 7).Replace(" ", "");
            }
        }

        private class ICP_過帳明細_公藥 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                DateTime temp0 = x[(int)enum_過帳明細_公藥.報表日期].ToDateString().StringToDateTime();
                DateTime temp1 = y[(int)enum_過帳明細_公藥.報表日期].ToDateString().StringToDateTime();
                int cmp = temp0.CompareTo(temp1);
                return cmp;
            }
        }
    }
}
