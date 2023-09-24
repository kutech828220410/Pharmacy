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
        private enum enum_藥品過消耗帳_匯出
        {
            藥碼,
            來源,
            藥名,
            消耗量,
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
            this.plC_RJ_Button_藥品過消耗帳_異常過帳設定過帳完成.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_異常過帳設定過帳完成_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_無效期可入帳.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_無效期可入帳_MouseDownEvent;
            this.plC_RJ_Button_藥品過消耗帳_匯出藥局消耗帳_匯出.MouseDownEvent += PlC_RJ_Button_藥品過消耗帳_匯出藥局消耗帳_匯出_MouseDownEvent;
            this.plC_UI_Init.Add_Method(this.sub_Program_藥品過消耗帳);
        }

  

        private void sub_Program_藥品過消耗帳()
        {
            sub_Program_檢查藥品過消耗帳();
            sub_Program_檢查異常消耗帳過帳();
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
                this.MyTimer_檢查藥品過消耗帳_結束延遲.StartTickTime(300000);
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
                List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端_藥品資料_old.SQL_GetAllRows(false);
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

        #region PLC_檢查異常消耗帳過帳
        Task Task_檢查異常消耗帳過帳;
        PLC_Device PLC_Device_檢查異常消耗帳過帳 = new PLC_Device("");
        PLC_Device PLC_Device_檢查異常消耗帳過帳_OK = new PLC_Device("");
        MyTimer MyTimer_檢查異常消耗帳過帳_結束延遲 = new MyTimer();
        int cnt_Program_檢查異常消耗帳過帳 = 65534;
        void sub_Program_檢查異常消耗帳過帳()
        {
            PLC_Device_檢查異常消耗帳過帳.Bool = true;
            if (cnt_Program_檢查異常消耗帳過帳 == 65534)
            {
                this.MyTimer_檢查異常消耗帳過帳_結束延遲.StartTickTime(10000);
                PLC_Device_檢查異常消耗帳過帳.SetComment("PLC_檢查異常消耗帳過帳");
                PLC_Device_檢查異常消耗帳過帳_OK.SetComment("PLC_檢查異常消耗帳過帳_OK");
                PLC_Device_檢查異常消耗帳過帳.Bool = false;
                cnt_Program_檢查異常消耗帳過帳 = 65535;
            }
            if (cnt_Program_檢查異常消耗帳過帳 == 65535) cnt_Program_檢查異常消耗帳過帳 = 1;
            if (cnt_Program_檢查異常消耗帳過帳 == 1) cnt_Program_檢查異常消耗帳過帳_檢查按下(ref cnt_Program_檢查異常消耗帳過帳);
            if (cnt_Program_檢查異常消耗帳過帳 == 2) cnt_Program_檢查異常消耗帳過帳_初始化(ref cnt_Program_檢查異常消耗帳過帳);
            if (cnt_Program_檢查異常消耗帳過帳 == 3) cnt_Program_檢查異常消耗帳過帳 = 65500;
            if (cnt_Program_檢查異常消耗帳過帳 > 1) cnt_Program_檢查異常消耗帳過帳_檢查放開(ref cnt_Program_檢查異常消耗帳過帳);

            if (cnt_Program_檢查異常消耗帳過帳 == 65500)
            {
                this.MyTimer_檢查異常消耗帳過帳_結束延遲.TickStop();
                this.MyTimer_檢查異常消耗帳過帳_結束延遲.StartTickTime(300000);
                PLC_Device_檢查異常消耗帳過帳.Bool = false;
                PLC_Device_檢查異常消耗帳過帳_OK.Bool = false;
                cnt_Program_檢查異常消耗帳過帳 = 65535;
            }
        }
        void cnt_Program_檢查異常消耗帳過帳_檢查按下(ref int cnt)
        {
            if (PLC_Device_檢查異常消耗帳過帳.Bool) cnt++;
        }
        void cnt_Program_檢查異常消耗帳過帳_檢查放開(ref int cnt)
        {
            if (!PLC_Device_檢查異常消耗帳過帳.Bool) cnt = 65500;
        }
        void cnt_Program_檢查異常消耗帳過帳_初始化(ref int cnt)
        {
            if (this.MyTimer_檢查異常消耗帳過帳_結束延遲.IsTimeOut())
            {
                List<object[]> list_過帳狀態 = this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(false);
                List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端_藥品資料_old.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                List<object[]> list_過帳明細_Add = new List<object[]>();
                list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.類別, enum_寫入報表設定_類別.其他.GetEnumName());
                list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.狀態, enum_過帳狀態.已產生排程.GetEnumName());
                list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.檔名, "異常消耗帳過帳");
                if (list_過帳狀態.Count > 0)
                {
                    if (Task_檢查異常消耗帳過帳 == null)
                    {
                        Task_檢查異常消耗帳過帳 = new Task(new Action(delegate { this.PlC_RJ_Button_藥品過消耗帳_異常過帳設定過帳完成_MouseDownEvent(null); }));
                    }
                    if (Task_檢查異常消耗帳過帳.Status == TaskStatus.RanToCompletion)
                    {
                        Task_檢查異常消耗帳過帳 = new Task(new Action(delegate { this.PlC_RJ_Button_藥品過消耗帳_異常過帳設定過帳完成_MouseDownEvent(null); }));
                    }
                    if (Task_檢查異常消耗帳過帳.Status == TaskStatus.Created)
                    {
                        Task_檢查異常消耗帳過帳.Start();
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
        private List<object[]> Function_藥品過消耗帳_取得所有過帳明細(string 藥品碼)
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            myTimerBasic.StartTickTime(50000);
            List<object[]> list_門診 = this.sqL_DataGridView_過帳明細_門診.SQL_GetRows((int)enum_過帳明細_門診.藥品碼, 藥品碼, false);
            Console.WriteLine($"取得門診所有過帳資料,{myTimerBasic.ToString()}");
            List<object[]> list_急診 = this.sqL_DataGridView_過帳明細_急診.SQL_GetRows((int)enum_過帳明細_急診.藥品碼, 藥品碼, false);
            Console.WriteLine($"取得急診所有過帳資料,{myTimerBasic.ToString()}");
            List<object[]> list_住院 = this.sqL_DataGridView_過帳明細_住院.SQL_GetRows((int)enum_過帳明細_住院.藥品碼, 藥品碼, false);
            Console.WriteLine($"取得住院所有過帳資料,{myTimerBasic.ToString()}");
            List<object[]> list_公藥 = this.sqL_DataGridView_過帳明細_公藥.SQL_GetRows((int)enum_過帳明細_公藥.藥品碼, 藥品碼, false);
            Console.WriteLine($"取得公藥所有過帳資料,{myTimerBasic.ToString()}");
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
        private List<object[]> Function_藥品過消耗帳_取得所有過帳明細(DateTime st_time , DateTime end_time)
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            myTimerBasic.StartTickTime(50000);
            List<object[]> list_門診 = this.sqL_DataGridView_過帳明細_門診.SQL_GetRowsByBetween((int)enum_過帳明細_門診.報表日期, st_time, end_time, false);
            Console.WriteLine($"取得門診過帳資料 {st_time} - {end_time},{myTimerBasic.ToString()}");
            List<object[]> list_急診 = this.sqL_DataGridView_過帳明細_急診.SQL_GetRowsByBetween((int)enum_過帳明細_急診.報表日期, st_time, end_time, false);
            Console.WriteLine($"取得急診過帳資料 {st_time} - {end_time},{myTimerBasic.ToString()}");
            List<object[]> list_住院 = this.sqL_DataGridView_過帳明細_住院.SQL_GetRowsByBetween((int)enum_過帳明細_住院.報表日期, st_time, end_time, false);
            Console.WriteLine($"取得住院過帳資料 {st_time} - {end_time},{myTimerBasic.ToString()}");
            List<object[]> list_公藥 = this.sqL_DataGridView_過帳明細_公藥.SQL_GetRowsByBetween((int)enum_過帳明細_公藥.報表日期, st_time, end_time, false);
            Console.WriteLine($"取得公藥過帳資料 {st_time} - {end_time},{myTimerBasic.ToString()}");
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
        private List<object[]> Function_藥品過消耗帳_取得所有過帳明細()
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            myTimerBasic.StartTickTime(50000);
            List<object[]> list_門診 = this.sqL_DataGridView_過帳明細_門診.SQL_GetAllRows(false);
            Console.WriteLine($"取得門診所有過帳資料,{myTimerBasic.ToString()}");
            List<object[]> list_急診 = this.sqL_DataGridView_過帳明細_急診.SQL_GetAllRows(false);
            Console.WriteLine($"取得急診所有過帳資料,{myTimerBasic.ToString()}");
            List<object[]> list_住院 = this.sqL_DataGridView_過帳明細_住院.SQL_GetAllRows(false);
            Console.WriteLine($"取得住院所有過帳資料,{myTimerBasic.ToString()}");
            List<object[]> list_公藥 = this.sqL_DataGridView_過帳明細_公藥.SQL_GetAllRows(false);
            Console.WriteLine($"取得公藥所有過帳資料,{myTimerBasic.ToString()}");
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
                    list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.過帳完成.GetEnumName();
                    list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.過帳時間] = DateTime.Now.ToDateTimeString_6();
                    list_ReplaceValue.Add(list_藥品過消耗帳[i]);
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
                            List<string> list_效期 = new List<string>();
                            List<string> list_批號 = new List<string>();

                            Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥品碼, ref list_效期, ref list_批號);
                            if (list_效期.Count > 0)
                            {
                                儲位資訊_效期 = list_效期[0];
                                儲位資訊_批號 = list_批號[0];
                                儲位資訊_異動量 = 異動量.ToString();
                                庫存量 = deviceBasic_buf[0].Inventory.StringToInt32();
                                結存量 = 庫存量 + 異動量;
                                deviceBasic_buf[0].效期庫存異動(儲位資訊_效期, 儲位資訊_批號, 儲位資訊_異動量, false);
                                備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量}";
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
                                if (備註.Length > 250)
                                {
                                    value_trading[(int)enum_交易記錄查詢資料.備註] = "";
                                }
                                value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.屏榮藥局.GetEnumName();
                                value_trading[(int)enum_交易記錄查詢資料.操作人] = "系統";
                                value_trading[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();
                                list_trading_value.Add(value_trading);
                            }
                            else
                            {
                                list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.無效期可入帳.GetEnumName();
                                list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.備註] = 備註;
                                if (備註.Length > 250)
                                {
                                    list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.備註] = "";
                                }
                            }
                    
                        }
                    }
                    else
                    {
                        異動量 = 0;
                        for (int k = 0; k < list_儲位資訊.Count; k++)
                        {
                            this.Function_庫存異動(list_儲位資訊[k]);
                            this.Function_堆疊資料_取得儲位資訊內容(list_儲位資訊[k], ref 儲位資訊_GUID, ref 儲位資訊_TYPE, ref 儲位資訊_IP, ref 儲位資訊_Num, ref 儲位資訊_效期, ref 儲位資訊_批號, ref 儲位資訊_庫存, ref 儲位資訊_異動量);
                            if (儲位資訊_批號.StringIsEmpty()) 儲位資訊_批號 = "None";
                            備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量}";
                            if (k != list_儲位資訊.Count - 1) 備註 += "\n";
                            異動量 += 儲位資訊_異動量.StringToInt32();
                        }
             
                        結存量 = 庫存量 + 異動量;
                        if (庫存量 == 0)
                        {
                            continue;
                        }
                        list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.過帳時間] = DateTime.Now.ToDateTimeString_6();

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
        private void PlC_RJ_Button_藥品過消耗帳_無效期可入帳_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥品過消耗帳_取得所有過帳明細();
            list_value = (from value in list_value
                          where value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.無效期可入帳.GetEnumName()
                          select value).ToList();
            list_value.Sort(new ICP_藥品過消耗帳());
            this.sqL_DataGridView_藥品過消耗帳.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥品過消耗帳_異常過帳設定過帳完成_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_藥品過消耗帳_取得所有過帳明細();
            list_value = (from value in list_value
                          where (value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.庫存不足.GetEnumName())
                          || value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.找無此藥品.GetEnumName()
                           || value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.未建立儲位.GetEnumName()
                            || value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.忽略過帳.GetEnumName()
                             || value[(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_藥品過消耗帳_狀態.無效期可入帳.GetEnumName()
                          select value).ToList();
            this.Function_藥品過消耗帳_設定過帳完成(list_value);
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
            List<object[]> list_value = Function_藥品過消耗帳_取得所有過帳明細(rJ_TextBox_藥品過消耗帳_藥品碼篩選.Text);

            list_value.Sort(new ICP_藥品過消耗帳());

            this.sqL_DataGridView_藥品過消耗帳.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥品過消耗帳_測試_MouseDownEvent(MouseEventArgs mevent)
        {
            string 備註 = "";
            備註 += "[效期]:2024/09/27,[批號]:250D54E,[數量]:-3\n";
            備註 += "[效期]:2024/10/02,[批號]:250D56E,[數量]:-15";
            string[] ary_space = 備註.Split('\n');
            for (int i = 0; i < ary_space.Length; i++)
            {
                
            }
        }
        private void PlC_RJ_Button_藥品過消耗帳_匯出藥局消耗帳_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (comboBox_藥品過消耗帳_匯出藥局消耗帳_藥局選擇.SelectedIndex == -1) comboBox_藥品過消耗帳_匯出藥局消耗帳_藥局選擇.SelectedIndex = 0;
                DateTime st_datetime = $"{rJ_DatePicker_藥品過消耗帳_匯出藥局消耗帳_起始時間.Value.ToDateString()} 00:00:00".StringToDateTime();
                DateTime end_datetime = $"{rJ_DatePicker_藥品過消耗帳_匯出藥局消耗帳_結束時間.Value.ToDateString()} 23:59:59".StringToDateTime();
                string 藥局名稱 = comboBox_藥品過消耗帳_匯出藥局消耗帳_藥局選擇.Text;
                List<object[]> list_value = Function_藥品過消耗帳_取得所有過帳明細(st_datetime, end_datetime);
                List<object[]> list_value_門診 = new List<object[]>();
                List<object[]> list_value_急診 = new List<object[]>();
                List<object[]> list_value_住院 = new List<object[]>();
                List<object[]> list_匯出資料_門診 = new List<object[]>();
                List<object[]> list_匯出資料_門診_buf = new List<object[]>();
                List<object[]> list_匯出資料_急診 = new List<object[]>();
                List<object[]> list_匯出資料_急診_buf = new List<object[]>();
                List<object[]> list_匯出資料_住院 = new List<object[]>();
                List<object[]> list_匯出資料_住院_buf = new List<object[]>();

                list_value_門診 = list_value.GetRowsByLike((int)enum_藥品過消耗帳.藥局代碼, "OPD");
                for (int i = 0; i < list_value_門診.Count; i++)
                {
                    string 藥碼 = list_value_門診[i][(int)enum_藥品過消耗帳.藥品碼].ObjectToString();
                    string 藥名 = list_value_門診[i][(int)enum_藥品過消耗帳.藥品名稱].ObjectToString();
                    string 來源名稱 = list_value_門診[i][(int)enum_藥品過消耗帳.來源名稱].ObjectToString();
                    藥名 = 藥名.Replace(",", " ");
                    int 消耗量_temp = list_value_門診[i][(int)enum_藥品過消耗帳.異動量].StringToInt32();
                    list_匯出資料_門診_buf = list_匯出資料_門診.GetRows((int)enum_藥品過消耗帳_匯出.藥碼, 藥碼);
                    if (list_匯出資料_門診_buf.Count == 0)
                    {
                        object[] value = new object[new enum_藥品過消耗帳_匯出().GetLength()];
                        value[(int)enum_藥品過消耗帳_匯出.藥碼] = 藥碼;
                        value[(int)enum_藥品過消耗帳_匯出.藥名] = 藥名;
                        string 來源 = value[(int)enum_藥品過消耗帳_匯出.來源].ObjectToString();
                        if (來源.Contains(來源名稱) == false)
                        {
                            if (來源.StringIsEmpty())
                            {
                                來源 = $"{來源名稱}";
                            }
                            else
                            {
                                來源 = $"{來源};{來源名稱}";
                            }
                        }
                        value[(int)enum_藥品過消耗帳_匯出.來源] = 來源;
                        value[(int)enum_藥品過消耗帳_匯出.消耗量] = 消耗量_temp.ToString();
                        list_匯出資料_門診.Add(value);
                    }
                    else
                    {
                        object[] value = list_匯出資料_門診_buf[0];
                        int temp = value[(int)enum_藥品過消耗帳_匯出.消耗量].StringToInt32();
                        value[(int)enum_藥品過消耗帳_匯出.消耗量] = (消耗量_temp + temp).ToString();
                        string 來源 = value[(int)enum_藥品過消耗帳_匯出.來源].ObjectToString();
                        if (來源.Contains(來源名稱) == false)
                        {
                            if (來源.StringIsEmpty())
                            {
                                來源 = $"{來源名稱}";
                            }
                            else
                            {
                                來源 = $"{來源};{來源名稱}";
                            }
                        }
                        value[(int)enum_藥品過消耗帳_匯出.來源] = 來源;
                    }
                }
                for (int i = 0; i < list_匯出資料_門診.Count; i++)
                {
                    object[] value = list_匯出資料_門診[i];
                    int temp = value[(int)enum_藥品過消耗帳_匯出.消耗量].StringToInt32();
                    value[(int)enum_藥品過消耗帳_匯出.消耗量] = (temp * -1).ToString();
                }


                list_value_急診 = list_value.GetRowsByLike((int)enum_藥品過消耗帳.藥局代碼, "PHER");
                for (int i = 0; i < list_value_急診.Count; i++)
                {
                    string 藥碼 = list_value_急診[i][(int)enum_藥品過消耗帳.藥品碼].ObjectToString();
                    string 藥名 = list_value_急診[i][(int)enum_藥品過消耗帳.藥品名稱].ObjectToString();
                    string 來源名稱 = list_value_急診[i][(int)enum_藥品過消耗帳.來源名稱].ObjectToString();
                    藥名 = 藥名.Replace(",", " ");
                    int 消耗量_temp = list_value_急診[i][(int)enum_藥品過消耗帳.異動量].StringToInt32();
                    list_匯出資料_急診_buf = list_匯出資料_急診.GetRows((int)enum_藥品過消耗帳_匯出.藥碼, 藥碼);
                    if (list_匯出資料_急診_buf.Count == 0)
                    {
                        object[] value = new object[new enum_藥品過消耗帳_匯出().GetLength()];
                        value[(int)enum_藥品過消耗帳_匯出.藥碼] = 藥碼;
                        value[(int)enum_藥品過消耗帳_匯出.藥名] = 藥名;
                        value[(int)enum_藥品過消耗帳_匯出.消耗量] = 消耗量_temp.ToString();
                        string 來源 = value[(int)enum_藥品過消耗帳_匯出.來源].ObjectToString();
                        if (來源.Contains(來源名稱) == false)
                        {
                            if (來源.StringIsEmpty())
                            {
                                來源 = $"{來源名稱}";
                            }
                            else
                            {
                                來源 = $"{來源};{來源名稱}";
                            }
                        }
                        value[(int)enum_藥品過消耗帳_匯出.來源] = 來源;
                        list_匯出資料_急診.Add(value);
                    }
                    else
                    {
                        object[] value = list_匯出資料_急診_buf[0];
                        int temp = value[(int)enum_藥品過消耗帳_匯出.消耗量].StringToInt32();
                        value[(int)enum_藥品過消耗帳_匯出.消耗量] = (消耗量_temp + temp).ToString();
                        string 來源 = value[(int)enum_藥品過消耗帳_匯出.來源].ObjectToString();
                        if (來源.Contains(來源名稱) == false)
                        {
                            if (來源.StringIsEmpty())
                            {
                                來源 = $"{來源名稱}";
                            }
                            else
                            {
                                來源 = $"{來源};{來源名稱}";
                            }
                        }
                        value[(int)enum_藥品過消耗帳_匯出.來源] = 來源;
                    }
                }
                for (int i = 0; i < list_匯出資料_急診.Count; i++)
                {
                    object[] value = list_匯出資料_急診[i];
                    int temp = value[(int)enum_藥品過消耗帳_匯出.消耗量].StringToInt32();
                    value[(int)enum_藥品過消耗帳_匯出.消耗量] = (temp * -1).ToString();
                }


                list_value_住院 = list_value.GetRowsByLike((int)enum_藥品過消耗帳.藥局代碼, "PHR");
                for (int i = 0; i < list_value_住院.Count; i++)
                {
                    string 藥碼 = list_value_住院[i][(int)enum_藥品過消耗帳.藥品碼].ObjectToString();
                    string 藥名 = list_value_住院[i][(int)enum_藥品過消耗帳.藥品名稱].ObjectToString();
                    string 來源名稱 = list_value_住院[i][(int)enum_藥品過消耗帳.來源名稱].ObjectToString();
                    string 藥局代碼 = list_value_住院[i][(int)enum_藥品過消耗帳.藥局代碼].ObjectToString();
                    藥名 = 藥名.Replace(",", " ");
                    int 消耗量_temp = list_value_住院[i][(int)enum_藥品過消耗帳.異動量].StringToInt32();
                    list_匯出資料_住院_buf = list_匯出資料_住院.GetRows((int)enum_藥品過消耗帳_匯出.藥碼, 藥碼);
                    if (list_匯出資料_住院_buf.Count == 0)
                    {
                        object[] value = new object[new enum_藥品過消耗帳_匯出().GetLength()];
                        value[(int)enum_藥品過消耗帳_匯出.藥碼] = 藥碼;
                        value[(int)enum_藥品過消耗帳_匯出.藥名] = 藥名;
                        value[(int)enum_藥品過消耗帳_匯出.消耗量] = 消耗量_temp.ToString();
                        string 來源 = value[(int)enum_藥品過消耗帳_匯出.來源].ObjectToString();
                        if(來源名稱 != "住院" && 藥局代碼 != "PHRM")
                        {

                        }
                        if (來源.Contains(來源名稱) == false)
                        {
                            if (來源.StringIsEmpty())
                            {
                                來源 = $"{來源名稱}";
                            }
                            else
                            {
                                來源 = $"{來源};{來源名稱}";
                            }
                        }
                        value[(int)enum_藥品過消耗帳_匯出.來源] = 來源;
                        list_匯出資料_住院.Add(value);
                    }
                    else
                    {
                        object[] value = list_匯出資料_住院_buf[0];
                        int temp = value[(int)enum_藥品過消耗帳_匯出.消耗量].StringToInt32();
                        value[(int)enum_藥品過消耗帳_匯出.消耗量] = (消耗量_temp + temp).ToString();
                        string 來源 = value[(int)enum_藥品過消耗帳_匯出.來源].ObjectToString();
                        if (來源.Contains(來源名稱) == false)
                        {
                            if (來源.StringIsEmpty())
                            {
                                來源 = $"{來源名稱}";
                            }
                            else
                            {
                                來源 = $"{來源};{來源名稱}";
                            }
                        }
                        value[(int)enum_藥品過消耗帳_匯出.來源] = 來源;
                    }
                }
                for (int i = 0; i < list_匯出資料_住院.Count; i++)
                {
                    object[] value = list_匯出資料_住院[i];
                    int temp = value[(int)enum_藥品過消耗帳_匯出.消耗量].StringToInt32();
                    value[(int)enum_藥品過消耗帳_匯出.消耗量] = (temp * -1).ToString();
                }

                Console.WriteLine($"共有 門診<{list_匯出資料_門診.Count}>筆資料");
                Console.WriteLine($"共有 急診<{list_匯出資料_急診.Count}>筆資料");
                Console.WriteLine($"共有 住院<{list_匯出資料_住院.Count}>筆資料");

                if (saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    string filepath = Basic.FileIO.GetFilePath(saveFileDialog_SaveExcel.FileName);
                    Basic.CSVHelper.SaveFile(list_匯出資料_門診.ToDataTable(new enum_藥品過消耗帳_匯出()), $"{filepath}//{st_datetime.ToDateTinyString()}-{end_datetime.ToDateTinyString()}(門診).csv", ',');
                    Basic.CSVHelper.SaveFile(list_匯出資料_急診.ToDataTable(new enum_藥品過消耗帳_匯出()), $"{filepath}//{st_datetime.ToDateTinyString()}-{end_datetime.ToDateTinyString()}(急診).csv", ',');
                    Basic.CSVHelper.SaveFile(list_匯出資料_住院.ToDataTable(new enum_藥品過消耗帳_匯出()), $"{filepath}//{st_datetime.ToDateTinyString()}-{end_datetime.ToDateTinyString()}(住院).csv", ',');
                    MyMessageBox.ShowDialog("匯出完成");
                }
            }));
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
