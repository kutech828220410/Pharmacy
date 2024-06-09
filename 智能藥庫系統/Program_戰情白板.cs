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
        public enum enum_戰情白板_公告
        {
            GUID,
            排序,
            內容,
            登錄時間,
        }
        public enum enum_戰情白板
        {
            藥碼,
            藥名,
            中文名,
            狀態,
            單位,
            總庫存,
            藥庫庫存,
            藥局庫存,
            安全量,
            基準量,
            狀態文字
        }
        public enum enum_戰情白板_自選藥品
        {
            GUID,
            藥碼,
            藥名,
            單位,
        }
        public enum enum_戰情白板_狀態文字
        {
            高於安全量,
            低於基準量,
            低於安全量
        }
        private void sub_Program_戰情白板_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_戰情白板_公告, dBConfigClass.DB_Basic);
            this.sqL_DataGridView_戰情白板_公告.Init();
            if (!this.sqL_DataGridView_戰情白板_公告.SQL_IsTableCreat()) this.sqL_DataGridView_戰情白板_公告.SQL_CreateTable();
            this.sqL_DataGridView_戰情白板_公告.RowEnterEvent += SqL_DataGridView_戰情白板_公告_RowEnterEvent;
            this.sqL_DataGridView_戰情白板_公告.DataGridRowsChangeRefEvent += SqL_DataGridView_戰情白板_公告_DataGridRowsChangeRefEvent;

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_戰情白板_自選藥品_選取內容, dBConfigClass.DB_Basic);
            this.sqL_DataGridView_戰情白板_自選藥品_選取內容.Init();
            if (!this.sqL_DataGridView_戰情白板_自選藥品_選取內容.SQL_IsTableCreat()) this.sqL_DataGridView_戰情白板_自選藥品_選取內容.SQL_CreateTable();

            this.sqL_DataGridView_戰情白板_自選藥品.Init();
            this.sqL_DataGridView_戰情白板_自選藥品.DataGridRefreshEvent += SqL_DataGridView_戰情白板_自選藥品_DataGridRefreshEvent;
            this.sqL_DataGridView_戰情白板_自選藥品.DataGridRowsChangeRefEvent += SqL_DataGridView_戰情白板_自選藥品_DataGridRowsChangeRefEvent;

            this.sqL_DataGridView_戰情白板_一般藥品.Init();
            this.sqL_DataGridView_戰情白板_一般藥品.DataGridRefreshEvent += SqL_DataGridView_戰情白板_一般藥品_DataGridRefreshEvent;

            this.sqL_DataGridView_戰情白板_自選藥品_藥品資料.Init(this.sqL_DataGridView_藥庫_藥品資料);
            this.sqL_DataGridView_戰情白板_自選藥品_藥品資料.Set_ColumnVisible(false, new enum_medDrugstore().GetEnumNames());
            this.sqL_DataGridView_戰情白板_自選藥品_藥品資料.Set_ColumnVisible(true, enum_medDrugstore.藥品碼, enum_medDrugstore.藥品名稱, enum_medDrugstore.中文名稱, enum_medDrugstore.包裝單位);

            this.plC_RJ_Button_戰情白板_全螢幕顯示.MouseDownEvent += PlC_RJ_Button_戰情白板_全螢幕顯示_MouseDownEvent;
            this.plC_RJ_Button_戰情白板_刷新一般藥品.MouseDownEvent += PlC_RJ_Button_戰情白板_刷新一般藥品_MouseDownEvent;
            this.plC_RJ_Button_戰情白板_刷新自選藥品.MouseDownEvent += PlC_RJ_Button_戰情白板_刷新自選藥品_MouseDownEvent;
            this.plC_RJ_Button_戰情白板_刷新公告內容.MouseDownEvent += PlC_RJ_Button_戰情白板_刷新公告內容_MouseDownEvent;
            this.plC_RJ_Button_戰情白板_檢查緊急申領.MouseDownEvent += PlC_RJ_Button_戰情白板_檢查緊急申領_MouseDownEvent;


            this.plC_RJ_Button_戰情白板_公告_確認.MouseDownEvent += PlC_RJ_Button_戰情白板_公告_確認_MouseDownEvent;
            this.plC_RJ_Button_戰情白板_公告_刷新.MouseDownEvent += PlC_RJ_Button_戰情白板_公告_刷新_MouseDownEvent;

          
            this.plC_RJ_Button_戰情白板_自選藥品_藥品資料_藥品名稱搜尋.MouseDownEvent += PlC_RJ_Button_戰情白板_自選藥品_藥品資料_藥品名稱搜尋_MouseDownEvent;
            this.plC_RJ_Button_戰情白板_自選藥品_藥品資料_藥品碼搜尋.MouseDownEvent += PlC_RJ_Button_戰情白板_自選藥品_藥品資料_藥品碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_戰情白板_自選藥品_藥品資料_填入資料.MouseDownEvent += PlC_RJ_Button_戰情白板_自選藥品_藥品資料_填入資料_MouseDownEvent;

            this.plC_RJ_Button_戰情白板_自選藥品_更新.MouseDownEvent += PlC_RJ_Button_戰情白板_自選藥品_更新_MouseDownEvent;
            this.plC_RJ_Button_戰情白板_自選藥品_刪除已選擇藥品.MouseDownEvent += PlC_RJ_Button_戰情白板_自選藥品_刪除已選擇藥品_MouseDownEvent;

            this.Function_戰情白板_公告_確認表格合理性();
            this.plC_UI_Init.Add_Method(sub_Program_戰情白板);
        }

      

        PLC_Device PLC_Device_戰情白板_全螢幕顯示 = new PLC_Device("");
        private string 戰情白板_公告內容 = "";
        private bool flag_戰情白板_顯示畫面_頁面更新 = false;
        private bool flag_戰情白板_設定_頁面更新 = false;
        private void sub_Program_戰情白板()
        {
            if (this.plC_ScreenPage_Main.PageText == "戰情白板" && this.plC_ScreenPage_戰情白板.PageText == "顯示畫面")
            {
                if (!this.flag_戰情白板_顯示畫面_頁面更新)
                {
                    this.Invoke(new Action(delegate
                    {
                

                    }));

                    this.PlC_RJ_Button_戰情白板_刷新公告內容_MouseDownEvent(null);
                    this.flag_戰情白板_顯示畫面_頁面更新 = true;
                }
                this.sub_Program_戰情白板_檢查離開全螢幕();
                this.sub_Program_戰情白板_刷新一般藥品();
                this.sub_Program_戰情白板_刷新自選藥品();
                this.sub_Program_戰情白板_刷新公告內容();
                this.sub_Program_戰情白板_檢查緊急申領();

                this.Function_戰情白板_繪製時間();
                this.Function_戰情白板_醫院名稱();
                this.Function_戰情白板_標題();
                this.Function_戰情白板_即時公告(戰情白板_公告內容);
                this.Function_戰情白板_藥品庫存量及安全量即時資訊();

                rJ_ProgressBar_戰情白版_一般藥品刷新條.Maximum = (int)MyTimer_戰情白板_刷新一般藥品_結束延遲.GetTickTime();
                if ((int)this.MyTimer_戰情白板_刷新一般藥品_結束延遲.GetTickTime() < rJ_ProgressBar_戰情白版_一般藥品刷新條.Maximum)
                {
                    rJ_ProgressBar_戰情白版_一般藥品刷新條.Value = (int)this.MyTimer_戰情白板_刷新一般藥品_結束延遲.GetTickTime();
                }

                rJ_ProgressBar_戰情白版_自選藥品刷新條.Maximum = (int)MyTimer_戰情白板_刷新自選藥品_結束延遲.GetTickTime();
                if ((int)this.MyTimer_戰情白板_刷新自選藥品_結束延遲.GetTickTime() < rJ_ProgressBar_戰情白版_自選藥品刷新條.Maximum)
                {
                    rJ_ProgressBar_戰情白版_自選藥品刷新條.Value = (int)this.MyTimer_戰情白板_刷新自選藥品_結束延遲.GetTickTime();
                }
            }
            else
            {
                this.flag_戰情白板_顯示畫面_頁面更新 = false;
            }

            if (this.plC_ScreenPage_Main.PageText == "戰情白板" && this.plC_ScreenPage_戰情白板.PageText == "設定")
            {
                if (!this.flag_戰情白板_設定_頁面更新)
                {
                    this.Invoke(new Action(delegate
                    {


                    }));

                    this.PlC_RJ_Button_戰情白板_公告_刷新_MouseDownEvent(null);
                    this.PlC_RJ_Button_戰情白板_自選藥品_更新_MouseDownEvent(null);
                    this.flag_戰情白板_設定_頁面更新 = true;
                }
            
            }
            else
            {
                this.flag_戰情白板_設定_頁面更新 = false;
            }
        }

        #region PLC_戰情白板_檢查離開全螢幕
        PLC_Device PLC_Device_戰情白板_檢查離開全螢幕 = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_檢查離開全螢幕_OK = new PLC_Device("");
        MyTimer MyTimer_戰情白板_檢查離開全螢幕_左鍵按下 = new MyTimer();

        MyTimer MyTimer_戰情白板_檢查離開全螢幕_結束延遲 = new MyTimer();
        int cnt_Program_戰情白板_檢查離開全螢幕 = 65534;
        void sub_Program_戰情白板_檢查離開全螢幕()
        {
            PLC_Device_戰情白板_檢查離開全螢幕.Bool = true;
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 65534)
            {
                this.MyTimer_戰情白板_檢查離開全螢幕_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_檢查離開全螢幕.SetComment("PLC_戰情白板_檢查離開全螢幕");
                PLC_Device_戰情白板_檢查離開全螢幕_OK.SetComment("PLC_戰情白板_檢查離開全螢幕_OK");
                PLC_Device_戰情白板_檢查離開全螢幕.Bool = false;
                cnt_Program_戰情白板_檢查離開全螢幕 = 65535;
            }
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 65535) cnt_Program_戰情白板_檢查離開全螢幕 = 1;
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 1) cnt_Program_戰情白板_檢查離開全螢幕_檢查按下(ref cnt_Program_戰情白板_檢查離開全螢幕);
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 2) cnt_Program_戰情白板_檢查離開全螢幕_初始化(ref cnt_Program_戰情白板_檢查離開全螢幕);
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 3) cnt_Program_戰情白板_檢查離開全螢幕 = 65500;
            if (cnt_Program_戰情白板_檢查離開全螢幕 > 1) cnt_Program_戰情白板_檢查離開全螢幕_檢查放開(ref cnt_Program_戰情白板_檢查離開全螢幕);

            if (cnt_Program_戰情白板_檢查離開全螢幕 == 65500)
            {
                this.MyTimer_戰情白板_檢查離開全螢幕_結束延遲.TickStop();
                this.MyTimer_戰情白板_檢查離開全螢幕_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_檢查離開全螢幕.Bool = false;
                PLC_Device_戰情白板_檢查離開全螢幕_OK.Bool = false;
                cnt_Program_戰情白板_檢查離開全螢幕 = 65535;
            }
        }
        void cnt_Program_戰情白板_檢查離開全螢幕_檢查按下(ref int cnt)
        {
            if (PLC_Device_戰情白板_檢查離開全螢幕.Bool) cnt++;
        }
        void cnt_Program_戰情白板_檢查離開全螢幕_檢查放開(ref int cnt)
        {
            if (!PLC_Device_戰情白板_檢查離開全螢幕.Bool) cnt = 65500;
        }
        void cnt_Program_戰情白板_檢查離開全螢幕_初始化(ref int cnt)
        {
            if (!PLC_Device_戰情白板_全螢幕顯示.Bool)
            {
                MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.TickStop();
                MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.StartTickTime(2000);
                return;
            }
            if(plC_Button_滑鼠左鍵.Bool)
            {
                if(MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.IsTimeOut())
                {
                    this.Invoke(new Action(delegate
                    {
                        this.panel_戰情白板.Visible = true;
                        this.panel_Main.Visible = true;
                        Basic.Screen.FullScreen(this.FindForm(), 0, false);
                        PLC_Device_戰情白板_全螢幕顯示.Bool = false;
                    }));
                }
            }
            else
            {
                MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.TickStop();
                MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.StartTickTime(2000);
            }
            cnt++;
        
        }



















        #endregion
        #region PLC_戰情白板_刷新一般藥品
        PLC_Device PLC_Device_戰情白板_刷新一般藥品 = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_刷新一般藥品_OK = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_刷新一般藥品_每頁列數 = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_刷新一般藥品_現在列數 = new PLC_Device("");
        Task Task_戰情白板_刷新一般藥品;
        MyTimer MyTimer_戰情白板_刷新一般藥品_結束延遲 = new MyTimer();
        int cnt_Program_戰情白板_刷新一般藥品 = 65534;
        void sub_Program_戰情白板_刷新一般藥品()
        {
            PLC_Device_戰情白板_刷新一般藥品.Bool = true;
            PLC_Device_戰情白板_刷新一般藥品_每頁列數.Value = 10;
            if (cnt_Program_戰情白板_刷新一般藥品 == 65534)
            {
                this.MyTimer_戰情白板_刷新一般藥品_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_刷新一般藥品_每頁列數.Value = 10;
                PLC_Device_戰情白板_刷新一般藥品.SetComment("PLC_戰情白板_刷新一般藥品");
                PLC_Device_戰情白板_刷新一般藥品_OK.SetComment("PLC_戰情白板_刷新一般藥品_OK");
                PLC_Device_戰情白板_刷新一般藥品.Bool = false;
                cnt_Program_戰情白板_刷新一般藥品 = 65535;
            }
            if (cnt_Program_戰情白板_刷新一般藥品 == 65535) cnt_Program_戰情白板_刷新一般藥品 = 1;
            if (cnt_Program_戰情白板_刷新一般藥品 == 1) cnt_Program_戰情白板_刷新一般藥品_檢查按下(ref cnt_Program_戰情白板_刷新一般藥品);
            if (cnt_Program_戰情白板_刷新一般藥品 == 2) cnt_Program_戰情白板_刷新一般藥品_初始化(ref cnt_Program_戰情白板_刷新一般藥品);
            if (cnt_Program_戰情白板_刷新一般藥品 == 3) cnt_Program_戰情白板_刷新一般藥品 = 65500;
            if (cnt_Program_戰情白板_刷新一般藥品 > 1) cnt_Program_戰情白板_刷新一般藥品_檢查放開(ref cnt_Program_戰情白板_刷新一般藥品);

            if (cnt_Program_戰情白板_刷新一般藥品 == 65500)
            {
                this.MyTimer_戰情白板_刷新一般藥品_結束延遲.TickStop();
                this.MyTimer_戰情白板_刷新一般藥品_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_刷新一般藥品.Bool = false;
                PLC_Device_戰情白板_刷新一般藥品_OK.Bool = false;
                cnt_Program_戰情白板_刷新一般藥品 = 65535;
            }
        }
        void cnt_Program_戰情白板_刷新一般藥品_檢查按下(ref int cnt)
        {
            if (PLC_Device_戰情白板_刷新一般藥品.Bool) cnt++;
        }
        void cnt_Program_戰情白板_刷新一般藥品_檢查放開(ref int cnt)
        {
            if (!PLC_Device_戰情白板_刷新一般藥品.Bool) cnt = 65500;
        }
        void cnt_Program_戰情白板_刷新一般藥品_初始化(ref int cnt)
        {
            if (this.MyTimer_戰情白板_刷新一般藥品_結束延遲.IsTimeOut())
            {
                if (Task_戰情白板_刷新一般藥品 == null)
                {
                    Task_戰情白板_刷新一般藥品 = new Task(new Action(delegate { this.PlC_RJ_Button_戰情白板_刷新一般藥品_MouseDownEvent(null); }));
                }
                if (Task_戰情白板_刷新一般藥品.Status == TaskStatus.RanToCompletion)
                {
                    Task_戰情白板_刷新一般藥品 = new Task(new Action(delegate { this.PlC_RJ_Button_戰情白板_刷新一般藥品_MouseDownEvent(null); }));
                }
                if (Task_戰情白板_刷新一般藥品.Status == TaskStatus.Created)
                {
                    Task_戰情白板_刷新一般藥品.Start();
                }
                cnt++;
            }
        }







        #endregion
        #region PLC_戰情白板_刷新自選藥品
        PLC_Device PLC_Device_戰情白板_刷新自選藥品 = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_刷新自選藥品_OK = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_刷新自選藥品_每頁列數 = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_刷新自選藥品_現在列數 = new PLC_Device("");
        Task Task_戰情白板_刷新自選藥品;
        MyTimer MyTimer_戰情白板_刷新自選藥品_結束延遲 = new MyTimer();
        int cnt_Program_戰情白板_刷新自選藥品 = 65534;
        void sub_Program_戰情白板_刷新自選藥品()
        {
            PLC_Device_戰情白板_刷新自選藥品.Bool = true;
            PLC_Device_戰情白板_刷新自選藥品_每頁列數.Value = 5;
            if (cnt_Program_戰情白板_刷新自選藥品 == 65534)
            {
                this.MyTimer_戰情白板_刷新自選藥品_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_刷新自選藥品_每頁列數.Value = 10;
                PLC_Device_戰情白板_刷新自選藥品.SetComment("PLC_戰情白板_刷新自選藥品");
                PLC_Device_戰情白板_刷新自選藥品_OK.SetComment("PLC_戰情白板_刷新自選藥品_OK");
                PLC_Device_戰情白板_刷新自選藥品.Bool = false;
                cnt_Program_戰情白板_刷新自選藥品 = 65535;
            }
            if (cnt_Program_戰情白板_刷新自選藥品 == 65535) cnt_Program_戰情白板_刷新自選藥品 = 1;
            if (cnt_Program_戰情白板_刷新自選藥品 == 1) cnt_Program_戰情白板_刷新自選藥品_檢查按下(ref cnt_Program_戰情白板_刷新自選藥品);
            if (cnt_Program_戰情白板_刷新自選藥品 == 2) cnt_Program_戰情白板_刷新自選藥品_初始化(ref cnt_Program_戰情白板_刷新自選藥品);
            if (cnt_Program_戰情白板_刷新自選藥品 == 3) cnt_Program_戰情白板_刷新自選藥品 = 65500;
            if (cnt_Program_戰情白板_刷新自選藥品 > 1) cnt_Program_戰情白板_刷新自選藥品_檢查放開(ref cnt_Program_戰情白板_刷新自選藥品);

            if (cnt_Program_戰情白板_刷新自選藥品 == 65500)
            {
                this.MyTimer_戰情白板_刷新自選藥品_結束延遲.TickStop();
                this.MyTimer_戰情白板_刷新自選藥品_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_刷新自選藥品.Bool = false;
                PLC_Device_戰情白板_刷新自選藥品_OK.Bool = false;
                cnt_Program_戰情白板_刷新自選藥品 = 65535;
            }
        }
        void cnt_Program_戰情白板_刷新自選藥品_檢查按下(ref int cnt)
        {
            if (PLC_Device_戰情白板_刷新自選藥品.Bool) cnt++;
        }
        void cnt_Program_戰情白板_刷新自選藥品_檢查放開(ref int cnt)
        {
            if (!PLC_Device_戰情白板_刷新自選藥品.Bool) cnt = 65500;
        }
        void cnt_Program_戰情白板_刷新自選藥品_初始化(ref int cnt)
        {
            if (this.MyTimer_戰情白板_刷新自選藥品_結束延遲.IsTimeOut())
            {
                if (Task_戰情白板_刷新自選藥品 == null)
                {
                    Task_戰情白板_刷新自選藥品 = new Task(new Action(delegate { this.PlC_RJ_Button_戰情白板_刷新自選藥品_MouseDownEvent(null); }));
                }
                if (Task_戰情白板_刷新自選藥品.Status == TaskStatus.RanToCompletion)
                {
                    Task_戰情白板_刷新自選藥品 = new Task(new Action(delegate { this.PlC_RJ_Button_戰情白板_刷新自選藥品_MouseDownEvent(null); }));
                }
                if (Task_戰情白板_刷新自選藥品.Status == TaskStatus.Created)
                {
                    Task_戰情白板_刷新自選藥品.Start();
                }
                cnt++;
            }
        }







        #endregion
        #region PLC_戰情白板_刷新公告內容
        PLC_Device PLC_Device_戰情白板_刷新公告內容 = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_刷新公告內容_OK = new PLC_Device("");
        Task Task_戰情白板_刷新公告內容;
        MyTimer MyTimer_戰情白板_刷新公告內容_結束延遲 = new MyTimer();
        int cnt_Program_戰情白板_刷新公告內容 = 65534;
        void sub_Program_戰情白板_刷新公告內容()
        {
            PLC_Device_戰情白板_刷新公告內容.Bool = true;
            if (cnt_Program_戰情白板_刷新公告內容 == 65534)
            {
                this.MyTimer_戰情白板_刷新公告內容_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_刷新公告內容.SetComment("PLC_戰情白板_刷新公告內容");
                PLC_Device_戰情白板_刷新公告內容_OK.SetComment("PLC_戰情白板_刷新公告內容_OK");
                PLC_Device_戰情白板_刷新公告內容.Bool = false;
                cnt_Program_戰情白板_刷新公告內容 = 65535;
            }
            if (cnt_Program_戰情白板_刷新公告內容 == 65535) cnt_Program_戰情白板_刷新公告內容 = 1;
            if (cnt_Program_戰情白板_刷新公告內容 == 1) cnt_Program_戰情白板_刷新公告內容_檢查按下(ref cnt_Program_戰情白板_刷新公告內容);
            if (cnt_Program_戰情白板_刷新公告內容 == 2) cnt_Program_戰情白板_刷新公告內容_初始化(ref cnt_Program_戰情白板_刷新公告內容);
            if (cnt_Program_戰情白板_刷新公告內容 == 3) cnt_Program_戰情白板_刷新公告內容 = 65500;
            if (cnt_Program_戰情白板_刷新公告內容 > 1) cnt_Program_戰情白板_刷新公告內容_檢查放開(ref cnt_Program_戰情白板_刷新公告內容);

            if (cnt_Program_戰情白板_刷新公告內容 == 65500)
            {
                this.MyTimer_戰情白板_刷新公告內容_結束延遲.TickStop();
                this.MyTimer_戰情白板_刷新公告內容_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_刷新公告內容.Bool = false;
                PLC_Device_戰情白板_刷新公告內容_OK.Bool = false;
                cnt_Program_戰情白板_刷新公告內容 = 65535;
            }
        }
        void cnt_Program_戰情白板_刷新公告內容_檢查按下(ref int cnt)
        {
            if (PLC_Device_戰情白板_刷新公告內容.Bool) cnt++;
        }
        void cnt_Program_戰情白板_刷新公告內容_檢查放開(ref int cnt)
        {
            if (!PLC_Device_戰情白板_刷新公告內容.Bool) cnt = 65500;
        }
        void cnt_Program_戰情白板_刷新公告內容_初始化(ref int cnt)
        {
            if (this.MyTimer_戰情白板_刷新公告內容_結束延遲.IsTimeOut())
            {
                if (Task_戰情白板_刷新公告內容 == null)
                {
                    Task_戰情白板_刷新公告內容 = new Task(new Action(delegate { PlC_RJ_Button_戰情白板_刷新公告內容_MouseDownEvent(null); }));
                }
                if (Task_戰情白板_刷新公告內容.Status == TaskStatus.RanToCompletion)
                {
                    Task_戰情白板_刷新公告內容 = new Task(new Action(delegate { PlC_RJ_Button_戰情白板_刷新公告內容_MouseDownEvent(null); }));
                }
                if (Task_戰情白板_刷新公告內容.Status == TaskStatus.Created)
                {
                    Task_戰情白板_刷新公告內容.Start();
                }
                cnt++;
            }
        }







        #endregion
        #region PLC_戰情白板_檢查緊急申領
        List<object[]> list_檢查緊急申領_已警示 = new List<object[]>();
        PLC_Device PLC_Device_戰情白板_檢查緊急申領 = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_檢查緊急申領_OK = new PLC_Device("");
        Task Task_戰情白板_檢查緊急申領;
        MyTimer MyTimer_戰情白板_檢查緊急申領_結束延遲 = new MyTimer();
        int cnt_Program_戰情白板_檢查緊急申領 = 65534;
        void sub_Program_戰情白板_檢查緊急申領()
        {
            PLC_Device_戰情白板_檢查緊急申領.Bool = true;
            if (cnt_Program_戰情白板_檢查緊急申領 == 65534)
            {
                this.MyTimer_戰情白板_檢查緊急申領_結束延遲.StartTickTime(3000);
                PLC_Device_戰情白板_檢查緊急申領.SetComment("PLC_戰情白板_檢查緊急申領");
                PLC_Device_戰情白板_檢查緊急申領_OK.SetComment("PLC_戰情白板_檢查緊急申領_OK");
                PLC_Device_戰情白板_檢查緊急申領.Bool = false;
                cnt_Program_戰情白板_檢查緊急申領 = 65535;
            }
            if (cnt_Program_戰情白板_檢查緊急申領 == 65535) cnt_Program_戰情白板_檢查緊急申領 = 1;
            if (cnt_Program_戰情白板_檢查緊急申領 == 1) cnt_Program_戰情白板_檢查緊急申領_檢查按下(ref cnt_Program_戰情白板_檢查緊急申領);
            if (cnt_Program_戰情白板_檢查緊急申領 == 2) cnt_Program_戰情白板_檢查緊急申領_初始化(ref cnt_Program_戰情白板_檢查緊急申領);
            if (cnt_Program_戰情白板_檢查緊急申領 == 3) cnt_Program_戰情白板_檢查緊急申領 = 65500;
            if (cnt_Program_戰情白板_檢查緊急申領 > 1) cnt_Program_戰情白板_檢查緊急申領_檢查放開(ref cnt_Program_戰情白板_檢查緊急申領);

            if (cnt_Program_戰情白板_檢查緊急申領 == 65500)
            {
                this.MyTimer_戰情白板_檢查緊急申領_結束延遲.TickStop();
                this.MyTimer_戰情白板_檢查緊急申領_結束延遲.StartTickTime(3000);
                PLC_Device_戰情白板_檢查緊急申領.Bool = false;
                PLC_Device_戰情白板_檢查緊急申領_OK.Bool = false;
                cnt_Program_戰情白板_檢查緊急申領 = 65535;
            }
        }
        void cnt_Program_戰情白板_檢查緊急申領_檢查按下(ref int cnt)
        {
            if (PLC_Device_戰情白板_檢查緊急申領.Bool) cnt++;
        }
        void cnt_Program_戰情白板_檢查緊急申領_檢查放開(ref int cnt)
        {
            if (!PLC_Device_戰情白板_檢查緊急申領.Bool) cnt = 65500;
        }
        void cnt_Program_戰情白板_檢查緊急申領_初始化(ref int cnt)
        {
            if (this.MyTimer_戰情白板_檢查緊急申領_結束延遲.IsTimeOut())
            {
                if (Task_戰情白板_檢查緊急申領 == null)
                {
                    Task_戰情白板_檢查緊急申領 = new Task(new Action(delegate { PlC_RJ_Button_戰情白板_檢查緊急申領_MouseDownEvent(null); }));
                }
                if (Task_戰情白板_檢查緊急申領.Status == TaskStatus.RanToCompletion)
                {
                    Task_戰情白板_檢查緊急申領 = new Task(new Action(delegate { PlC_RJ_Button_戰情白板_檢查緊急申領_MouseDownEvent(null); }));
                }
                if (Task_戰情白板_檢查緊急申領.Status == TaskStatus.Created)
                {
                    Task_戰情白板_檢查緊急申領.Start();
                }
                cnt++;
            }
        }







        #endregion
        #region Function
        private void Function_戰情白板_繪製時間()
        {
            using (Graphics g = this.panel_戰情白板_時間.CreateGraphics())
            {
                int width = this.panel_戰情白板_時間.Width;
                int height = this.panel_戰情白板_時間.Height;
                if (width == 0 || height == 0) return;
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using(Graphics g_bmp = Graphics.FromImage(bitmap))
                    {
                        Font font = new Font("微軟正黑體", 20, FontStyle.Bold);

                        string str_date = DateTime.Now.ToDateString();
                        string str_time = $"{DateTime.Now.Hour.ToString("00")}:{DateTime.Now.Minute.ToString("00")}:{DateTime.Now.Second.ToString("00")}";
                        Size size_date = TextRenderer.MeasureText(str_date, font);
                        Size size_time = TextRenderer.MeasureText(str_time, font);
                        int x_date = (this.panel_戰情白板_時間.Width - size_date.Width) / 2;
                        int y_date = (this.panel_戰情白板_時間.Height - size_date.Height - size_time.Height) / 2;

                        int x_time = (this.panel_戰情白板_時間.Width - size_time.Width) / 2;
                        int y_time = y_date + size_date.Height;
                        DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.Black, 1, true, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_date, new PointF(x_date, y_date), new Font("微軟正黑體", 20, FontStyle.Bold), Color.Yellow, Color.Black, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_time, new PointF(x_time, y_time), new Font("微軟正黑體", 20, FontStyle.Bold), Color.Yellow, Color.Black, g_bmp, 1, 1);

                        g.DrawImage(bitmap, new PointF(0, 0));
                    }
                }
             

            }
        }
        private void Function_戰情白板_醫院名稱()
        {
            using (Graphics g = this.panel_戰情白板_醫院名稱.CreateGraphics())
            {
                int width = this.panel_戰情白板_醫院名稱.Width;
                int height = this.panel_戰情白板_醫院名稱.Height;
                if (width == 0 || height == 0) return;
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics g_bmp = Graphics.FromImage(bitmap))
                    {
                        Font font_hs_CHT_name = new Font("微軟正黑體", 26, FontStyle.Bold);
                        Font font_hs_EN_name = new Font("微軟正黑體", 16, FontStyle.Bold);

                        string str_hs_CHT_name = "屏東榮民總醫院";
                        Size size_hs_CHT_name = TextRenderer.MeasureText(str_hs_CHT_name, font_hs_CHT_name);

                        string str_hs_EN_name = "Pingtung Veterans General Hospital";
                        Size size_hs_EN_name = TextRenderer.MeasureText(str_hs_EN_name, font_hs_EN_name);

                        int x_hs_CHT_name = (this.panel_戰情白板_醫院名稱.Width - size_hs_EN_name.Width) / 2;
                        int y_hs_CHT_name = (this.panel_戰情白板_時間.Height - size_hs_CHT_name.Height - size_hs_EN_name.Height) / 2;

                        int x_hs_EN_name = x_hs_CHT_name;
                        int y_hs_EN_name = y_hs_CHT_name + size_hs_CHT_name.Height;


                        DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.Black, 1, true, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_hs_CHT_name, bitmap.Width, new PointF(0, y_hs_CHT_name), font_hs_CHT_name, Color.White, g_bmp);
                        DrawingClass.Draw.文字左上繪製(str_hs_EN_name, bitmap.Width, new PointF(0, y_hs_EN_name), font_hs_EN_name, Color.White, g_bmp);



                        g.DrawImage(bitmap, new PointF(0, 0));
                    }
                }


            }
        }
        private void Function_戰情白板_標題()
        {
            using (Graphics g = this.panel_戰情白板_標題.CreateGraphics())
            {
                int width = this.panel_戰情白板_標題.Width;
                int height = this.panel_戰情白板_標題.Height;
                if (width == 0 || height == 0) return;
                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics g_bmp = Graphics.FromImage(bitmap))
                    {
                        Font font_hs_CHT_name = new Font("微軟正黑體", 26, FontStyle.Bold);
                        Font font_hs_EN_name = new Font("微軟正黑體", 16, FontStyle.Bold);

                        string str_hs_CHT_name = "藥局戰情整合平台";
                        Size size_hs_CHT_name = TextRenderer.MeasureText(str_hs_CHT_name, font_hs_CHT_name);

                        string str_hs_EN_name = "Pharmacy Information Integration plaform";
                        Size size_hs_EN_name = TextRenderer.MeasureText(str_hs_EN_name, font_hs_EN_name);

                        int x_hs_CHT_name = (this.panel_戰情白板_標題.Width - size_hs_EN_name.Width) / 2;
                        int y_hs_CHT_name = (this.panel_戰情白板_時間.Height - size_hs_CHT_name.Height - size_hs_EN_name.Height) / 2;

                        int x_hs_EN_name = x_hs_CHT_name;
                        int y_hs_EN_name = y_hs_CHT_name + size_hs_CHT_name.Height;


                        DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.Black, 1, true, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_hs_CHT_name, bitmap.Width, new PointF(0, y_hs_CHT_name), font_hs_CHT_name, Color.White, g_bmp);
                        DrawingClass.Draw.文字左上繪製(str_hs_EN_name, bitmap.Width, new PointF(0, y_hs_EN_name), font_hs_EN_name, Color.White, g_bmp);



                        g.DrawImage(bitmap, new PointF(0, 0));
                    }
                }


            }
        }
        private void Function_戰情白板_公告_確認表格合理性()
        {
            List<object[]> list_value = this.sqL_DataGridView_戰情白板_公告.SQL_GetAllRows(false);
            List<object[]> list_value_add = new List<object[]>();
            if (list_value.Count != 10)
            {
                this.sqL_DataGridView_戰情白板_公告.SQL_CreateTable();
                for (int i = 0; i < 10; i++)
                {
                    object[] value = new object[new enum_戰情白板_公告().GetLength()];
                    value[(int)enum_戰情白板_公告.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_戰情白板_公告.排序] = i.ToString();
                    value[(int)enum_戰情白板_公告.內容] = "";
                    value[(int)enum_戰情白板_公告.登錄時間] = DateTime.Now.ToDateTimeString_6();
                    list_value_add.Add(value);
                }
            }
            this.sqL_DataGridView_戰情白板_公告.SQL_AddRows(list_value_add, false);
        }
        private MyTimer MyTimer_戰情白板_即時公告_捲動時間 = new MyTimer();
        int StringCurrent_X;
        private void Function_戰情白板_即時公告(string AlarmString)
        {
            MyTimer_戰情白板_即時公告_捲動時間.StartTickTime(500);
            if (MyTimer_戰情白板_即時公告_捲動時間.IsTimeOut())
            {
                using (Graphics g = this.panel_戰情白板_即時公告.CreateGraphics())
                {
                    int width = this.panel_戰情白板_即時公告.Width;
                    int height = this.panel_戰情白板_即時公告.Height;
                    if (width == 0 || height == 0) return;
                    using (Bitmap bitmap = new Bitmap(width, height))
                    {
                        using (Graphics g_bmp = Graphics.FromImage(bitmap))
                        {

                            if(AlarmString.StringIsEmpty())
                            {
                                AlarmString = "無公告";
                            }

                            Font font_AlarmString = new Font("微軟正黑體", 26, FontStyle.Bold);
                            DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.RoyalBlue, 1, true, g_bmp, 1, 1);
                            SizeF SizeOfString = g_bmp.MeasureString(AlarmString, font_AlarmString);
                            int StringEnd_X = (int)(-SizeOfString.Width);
                            float y_name = (this.panel_戰情白板_即時公告.Height - SizeOfString.Height) / 2;
                            if (StringCurrent_X < StringEnd_X) this.StringCurrent_X = bitmap.Width;
                            g_bmp.DrawString(AlarmString, font_AlarmString, new SolidBrush(Color.White), new Point(StringCurrent_X, (int)y_name));
                            StringCurrent_X -= (int)g_bmp.MeasureString("X", font_AlarmString).Width;
                            g.DrawImage(bitmap, new Point(0, 0));
                        }
                    }


                }
            }
            
        }

        private void Function_戰情白板_藥品庫存量及安全量即時資訊()
        {
            using (Graphics g = this.panel_戰情白板_藥品庫存量及安全量即時資訊.CreateGraphics())
            {
                int width = this.panel_戰情白板_藥品庫存量及安全量即時資訊.Width;
                int height = this.panel_戰情白板_藥品庫存量及安全量即時資訊.Height;
                if (width == 0 || height == 0) return;

                using (Bitmap bitmap = new Bitmap(width, height))
                {
                    using (Graphics g_bmp = Graphics.FromImage(bitmap))
                    {
                        Font font_hs_CHT_name = new Font("微軟正黑體", 36, FontStyle.Bold);

                        int str_width = 700;
                        string str_hs_CHT_name = "藥品庫存量及安全量即時資訊";
                        Size size_hs_CHT_name = TextRenderer.MeasureText(str_hs_CHT_name, font_hs_CHT_name);

     

                        int x_hs_CHT_name = (this.panel_戰情白板_藥品庫存量及安全量即時資訊.Width - str_width) / 2;
                        int y_hs_CHT_name = (this.panel_戰情白板_時間.Height - size_hs_CHT_name.Height) / 2;

       

                        DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.White, 1, true, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_hs_CHT_name, str_width, new PointF(x_hs_CHT_name, y_hs_CHT_name), font_hs_CHT_name, Color.Black, g_bmp);



                        g.DrawImage(bitmap, new PointF(0, 0));
                    }
                }


            }
        }
        #endregion

        #region Event
        private void SqL_DataGridView_戰情白板_自選藥品_DataGridRefreshEvent()
        {
            for (int i = 0; i < this.sqL_DataGridView_戰情白板_自選藥品.dataGridView.Rows.Count; i++)
            {
                this.sqL_DataGridView_戰情白板_自選藥品.dataGridView.Rows[i].DefaultCellStyle.BackColor = ((i % 2 == 1) ? Color.Black : Color.RoyalBlue);
                this.sqL_DataGridView_戰情白板_自選藥品.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                string 狀態文字 = this.sqL_DataGridView_戰情白板_自選藥品.dataGridView.Rows[i].Cells[enum_戰情白板.狀態文字.GetEnumName()].Value.ObjectToString();
                if (狀態文字 == enum_戰情白板_狀態文字.低於安全量.GetEnumName())
                {
                    this.sqL_DataGridView_戰情白板_自選藥品.dataGridView.Rows[i].Cells[enum_戰情白板.狀態.GetEnumName()].Value = 智能藥庫系統.Properties.Resources.Red_alarm; ;
                }
                else if (狀態文字 == enum_戰情白板_狀態文字.低於基準量.GetEnumName())
                {
                    this.sqL_DataGridView_戰情白板_自選藥品.dataGridView.Rows[i].Cells[enum_戰情白板.狀態.GetEnumName()].Value = 智能藥庫系統.Properties.Resources.Blue_alarm; ;
                }
                else if (狀態文字 == enum_戰情白板_狀態文字.高於安全量.GetEnumName())
                {
                    this.sqL_DataGridView_戰情白板_自選藥品.dataGridView.Rows[i].Cells[enum_戰情白板.狀態.GetEnumName()].Value = 智能藥庫系統.Properties.Resources.Green_alarm; ;
                }
            }
        }
        private void SqL_DataGridView_戰情白板_自選藥品_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_戰情白板_公告());
        }
        private void SqL_DataGridView_戰情白板_公告_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_戰情白板_公告());
        }
        private void SqL_DataGridView_戰情白板_公告_RowEnterEvent(object[] RowValue)
        {
            rJ_TextBox_戰情白板_公告_內容.Text = RowValue[(int)enum_戰情白板_公告.內容].ObjectToString();
        }
        private void SqL_DataGridView_戰情白板_一般藥品_DataGridRefreshEvent()
        {
            for (int i = 0; i < this.sqL_DataGridView_戰情白板_一般藥品.dataGridView.Rows.Count; i++)
            {
                this.sqL_DataGridView_戰情白板_一般藥品.dataGridView.Rows[i].DefaultCellStyle.BackColor = ((i % 2 == 1) ? Color.Black : Color.RoyalBlue);
                this.sqL_DataGridView_戰情白板_一般藥品.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                string 狀態文字 = this.sqL_DataGridView_戰情白板_一般藥品.dataGridView.Rows[i].Cells[enum_戰情白板.狀態文字.GetEnumName()].Value.ObjectToString();
                if(狀態文字 == enum_戰情白板_狀態文字.低於安全量.GetEnumName())
                {
                    this.sqL_DataGridView_戰情白板_一般藥品.dataGridView.Rows[i].Cells[enum_戰情白板.狀態.GetEnumName()].Value = 智能藥庫系統.Properties.Resources.Red_alarm; ;
                }
                else if (狀態文字 == enum_戰情白板_狀態文字.低於基準量.GetEnumName())
                {
                    this.sqL_DataGridView_戰情白板_一般藥品.dataGridView.Rows[i].Cells[enum_戰情白板.狀態.GetEnumName()].Value = 智能藥庫系統.Properties.Resources.Blue_alarm; ;
                }
                else if (狀態文字 == enum_戰情白板_狀態文字.高於安全量.GetEnumName())
                {
                    this.sqL_DataGridView_戰情白板_一般藥品.dataGridView.Rows[i].Cells[enum_戰情白板.狀態.GetEnumName()].Value = 智能藥庫系統.Properties.Resources.Green_alarm; ;
                }
            }
        }
        private void PlC_RJ_Button_戰情白板_全螢幕顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.panel_戰情白板.Visible = false;
                this.panel_Main.Visible = false;
                Basic.Screen.FullScreen(this.FindForm(), 0, true);
                PLC_Device_戰情白板_全螢幕顯示.Bool = true;
            }));
       
        }
        private void PlC_RJ_Button_戰情白板_刷新一般藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            List<object[]> list_藥庫_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            Console.WriteLine($"戰情白板_刷新一般藥品>>取得藥庫藥品資料 ,耗時 {myTimer.ToString()}ms");
            this.sqL_DataGridView_藥庫_藥品資料.RowsChangeFunction(list_藥庫_藥品資料);
            Console.WriteLine($"戰情白板_刷新一般藥品>>更新藥庫藥品資料 ,耗時 {myTimer.ToString()}ms");
            List<object[]> list_戰情白板_一般藥品 = new List<object[]>();
            List<object[]> list_戰情白板_一般藥品_buf = new List<object[]>();

            list_藥庫_藥品資料.Sort(new ICP_藥庫_藥品資料());
        

            Parallel.ForEach(list_藥庫_藥品資料, value_temp =>
            {
                object[] value = new object[new enum_戰情白板().GetLength()];
                value[(int)enum_戰情白板.藥碼] =value_temp[(int)enum_medDrugstore.藥品碼];
                value[(int)enum_戰情白板.藥名] =value_temp[(int)enum_medDrugstore.藥品名稱];
                value[(int)enum_戰情白板.中文名] =value_temp[(int)enum_medDrugstore.中文名稱];
                value[(int)enum_戰情白板.單位] =value_temp[(int)enum_medDrugstore.包裝單位];
                value[(int)enum_戰情白板.總庫存] =value_temp[(int)enum_medDrugstore.總庫存];
                value[(int)enum_戰情白板.藥局庫存] =value_temp[(int)enum_medDrugstore.藥局庫存];
                value[(int)enum_戰情白板.藥庫庫存] =value_temp[(int)enum_medDrugstore.藥庫庫存];
                value[(int)enum_戰情白板.安全量] =value_temp[(int)enum_medDrugstore.安全庫存];
                value[(int)enum_戰情白板.基準量] =value_temp[(int)enum_medDrugstore.基準量];

                int 總庫存 = value[(int)enum_戰情白板.總庫存].StringToInt32();
                int 安全量 = value[(int)enum_戰情白板.安全量].StringToInt32();
                int 基準量 = value[(int)enum_戰情白板.基準量].StringToInt32();
                if (總庫存 < 安全量)
                {
                   value[(int)enum_戰情白板.狀態文字] = enum_戰情白板_狀態文字.低於安全量.GetEnumName();
                }
                else if (總庫存 < 基準量)
                {
                   value[(int)enum_戰情白板.狀態文字] = enum_戰情白板_狀態文字.低於基準量.GetEnumName();
                }
                else
                {
                    value[(int)enum_戰情白板.狀態文字] = enum_戰情白板_狀態文字.高於安全量.GetEnumName();
                }
                list_戰情白板_一般藥品_buf.LockAdd(value);
            });

            if(!plC_RJ_ChechBox_戰情白板_一般藥品_高於安全量要顯示.Checked)
            {
                list_戰情白板_一般藥品_buf.RemoveRow((int)enum_戰情白板.狀態文字, enum_戰情白板_狀態文字.高於安全量.GetEnumName());
            }
            if (!plC_RJ_ChechBox_戰情白板_一般藥品_低於基準量要顯示.Checked)
            {
                list_戰情白板_一般藥品_buf.RemoveRow((int)enum_戰情白板.狀態文字, enum_戰情白板_狀態文字.低於基準量.GetEnumName());
            }
            if (!plC_RJ_ChechBox_戰情白板_一般藥品_低於安全量要顯示.Checked)
            {
                list_戰情白板_一般藥品_buf.RemoveRow((int)enum_戰情白板.狀態文字, enum_戰情白板_狀態文字.低於安全量.GetEnumName());
            }

            int startnum = PLC_Device_戰情白板_刷新一般藥品_現在列數.Value;
            int endnum = PLC_Device_戰情白板_刷新一般藥品_現在列數.Value + PLC_Device_戰情白板_刷新一般藥品_每頁列數.Value;
            bool flag_recount = false;
            if (PLC_Device_戰情白板_刷新一般藥品_現在列數.Value + PLC_Device_戰情白板_刷新一般藥品_每頁列數.Value >= list_戰情白板_一般藥品_buf.Count)
            {
                flag_recount = true;
                endnum = list_戰情白板_一般藥品_buf.Count;
            }

            list_戰情白板_一般藥品_buf.Sort(new ICP_戰情白板());
            for (int i = startnum; i < (endnum); i++)
            {
                list_戰情白板_一般藥品.Add(list_戰情白板_一般藥品_buf[i]);
            }
            
            PLC_Device_戰情白板_刷新一般藥品_現在列數.Value += PLC_Device_戰情白板_刷新一般藥品_每頁列數.Value;
            if (flag_recount) PLC_Device_戰情白板_刷新一般藥品_現在列數.Value = 0;
            Console.WriteLine($"戰情白板_刷新一般藥品>>藥庫藥品資料轉換 ,耗時 {myTimer.ToString()}ms");
            this.sqL_DataGridView_戰情白板_一般藥品.RefreshGrid(list_戰情白板_一般藥品);
            Console.WriteLine($"戰情白板_刷新一般藥品>>刷新面板 ,耗時 {myTimer.ToString()}ms");

        }
        private void PlC_RJ_Button_戰情白板_刷新自選藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            List<object[]> list_藥庫_藥品資料_src = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥庫_藥品資料_buf = new List<object[]>();
            List<object[]> list_藥庫_藥品資料 = new List<object[]>();
            Console.WriteLine($"戰情白板_刷新自選藥品>>取得藥庫藥品資料 ,耗時 {myTimer.ToString()}ms");
            this.sqL_DataGridView_藥庫_藥品資料.RowsChangeFunction(list_藥庫_藥品資料);
            Console.WriteLine($"戰情白板_刷新自選藥品>>更新藥庫藥品資料 ,耗時 {myTimer.ToString()}ms");

            List<object[]> list_戰情白板_自選藥品資料 = this.sqL_DataGridView_戰情白板_自選藥品_選取內容.SQL_GetAllRows(false);


            for (int i = 0; i < list_戰情白板_自選藥品資料.Count; i++)
            {
                string 藥品碼 = list_戰情白板_自選藥品資料[i][(int)enum_戰情白板_自選藥品.藥碼].ObjectToString();
                list_藥庫_藥品資料_buf = list_藥庫_藥品資料_src.GetRows((int)enum_medDrugstore.藥品碼, 藥品碼);
                if(list_藥庫_藥品資料_buf.Count > 0)
                {
                    list_藥庫_藥品資料.Add(list_藥庫_藥品資料_buf[0]);
                }

            }

            List<object[]> list_戰情白板_自選藥品 = new List<object[]>();
            List<object[]> list_戰情白板_自選藥品_buf = new List<object[]>();

            list_藥庫_藥品資料.Sort(new ICP_藥庫_藥品資料());


            Parallel.ForEach(list_藥庫_藥品資料, value_temp =>
            {
                object[] value = new object[new enum_戰情白板().GetLength()];
                value[(int)enum_戰情白板.藥碼] = value_temp[(int)enum_medDrugstore.藥品碼];
                value[(int)enum_戰情白板.藥名] = value_temp[(int)enum_medDrugstore.藥品名稱];
                value[(int)enum_戰情白板.中文名] = value_temp[(int)enum_medDrugstore.中文名稱];
                value[(int)enum_戰情白板.單位] = value_temp[(int)enum_medDrugstore.包裝單位];
                value[(int)enum_戰情白板.總庫存] = value_temp[(int)enum_medDrugstore.總庫存];
                value[(int)enum_戰情白板.藥局庫存] = value_temp[(int)enum_medDrugstore.藥局庫存];
                value[(int)enum_戰情白板.藥庫庫存] = value_temp[(int)enum_medDrugstore.藥庫庫存];
                value[(int)enum_戰情白板.安全量] = value_temp[(int)enum_medDrugstore.安全庫存];
                value[(int)enum_戰情白板.基準量] = value_temp[(int)enum_medDrugstore.基準量];

                int 總庫存 = value[(int)enum_戰情白板.總庫存].StringToInt32();
                int 安全量 = value[(int)enum_戰情白板.安全量].StringToInt32();
                int 基準量 = value[(int)enum_戰情白板.基準量].StringToInt32();
                if (總庫存 < 安全量)
                {
                    value[(int)enum_戰情白板.狀態文字] = enum_戰情白板_狀態文字.低於安全量.GetEnumName();
                }
                else if (總庫存 < 基準量)
                {
                    value[(int)enum_戰情白板.狀態文字] = enum_戰情白板_狀態文字.低於基準量.GetEnumName();
                }
                else
                {
                    value[(int)enum_戰情白板.狀態文字] = enum_戰情白板_狀態文字.高於安全量.GetEnumName();
                }
                list_戰情白板_自選藥品_buf.LockAdd(value);
            });

            if (!plC_RJ_ChechBox_戰情白板_自選藥品_高於安全量要顯示.Checked)
            {
                list_戰情白板_自選藥品_buf.RemoveRow((int)enum_戰情白板.狀態文字, enum_戰情白板_狀態文字.高於安全量.GetEnumName());
            }
            if (!plC_RJ_ChechBox_戰情白板_自選藥品_低於基準量要顯示.Checked)
            {
                list_戰情白板_自選藥品_buf.RemoveRow((int)enum_戰情白板.狀態文字, enum_戰情白板_狀態文字.低於基準量.GetEnumName());
            }
            if (!plC_RJ_ChechBox_戰情白板_自選藥品_低於安全量要顯示.Checked)
            {
                list_戰情白板_自選藥品_buf.RemoveRow((int)enum_戰情白板.狀態文字, enum_戰情白板_狀態文字.低於安全量.GetEnumName());
            }

            int startnum = PLC_Device_戰情白板_刷新自選藥品_現在列數.Value;
            int endnum = PLC_Device_戰情白板_刷新自選藥品_現在列數.Value + PLC_Device_戰情白板_刷新自選藥品_每頁列數.Value;
            bool flag_recount = false;
            if (PLC_Device_戰情白板_刷新自選藥品_現在列數.Value + PLC_Device_戰情白板_刷新自選藥品_每頁列數.Value >= list_戰情白板_自選藥品_buf.Count)
            {
                flag_recount = true;
                endnum = list_戰情白板_自選藥品_buf.Count;
            }

            list_戰情白板_自選藥品_buf.Sort(new ICP_戰情白板());
            for (int i = startnum; i < (endnum); i++)
            {
                list_戰情白板_自選藥品.Add(list_戰情白板_自選藥品_buf[i]);
            }

            PLC_Device_戰情白板_刷新自選藥品_現在列數.Value += PLC_Device_戰情白板_刷新自選藥品_每頁列數.Value;
            if (flag_recount) PLC_Device_戰情白板_刷新自選藥品_現在列數.Value = 0;
            Console.WriteLine($"戰情白板_刷新自選藥品>>藥庫藥品資料轉換 ,耗時 {myTimer.ToString()}ms");
            this.sqL_DataGridView_戰情白板_自選藥品.RefreshGrid(list_戰情白板_自選藥品);
            Console.WriteLine($"戰情白板_刷新自選藥品>>刷新面板 ,耗時 {myTimer.ToString()}ms");
        }
        private void PlC_RJ_Button_戰情白板_刷新公告內容_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_戰情白板_公告.SQL_GetAllRows(false);
            List<string> list_str_result = new List<string>();
            string str_result = "";
            for (int i = 0; i < list_value.Count; i++)
            {
                string str = list_value[i][(int)enum_戰情白板_公告.內容].ObjectToString();
                if(!str.StringIsEmpty())
                {
                    list_str_result.Add(str);
                }
            }
            for (int i = 0; i < list_str_result.Count; i++)
            {
                if (i != 0) str_result += "         ";
                str_result += list_str_result[i];
              
            }
            戰情白板_公告內容 = str_result;
        }
        private void PlC_RJ_Button_戰情白板_檢查緊急申領_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_檢查緊急申領_已警示_buf = new List<object[]>();
            List<object[]> list_緊急申領 = this.sqL_DataGridView_藥局_緊急申領.SQL_GetRowsByBetween((int)enum_藥局_緊急申領.產出時間, DateTime.Now, false);
            list_緊急申領 = list_緊急申領.GetRows((int)enum_藥局_緊急申領.狀態, enum_藥局_緊急申領_狀態.等待過帳.GetEnumName());
            //list_緊急申領 = (from value in list_緊急申領
            //             where value[(int)enum_藥局_緊急申領.狀態].ObjectToString() != enum_藥局_緊急申領_狀態.過帳完成.GetEnumName()
            //             select value).ToList();

            if (list_緊急申領.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    rJ_Lable_戰情白版_新申領藥品.Visible = false;
                }));
                return;
            }
            this.Invoke(new Action(delegate
            {
                rJ_Lable_戰情白版_新申領藥品.Visible = true;
                rJ_Lable_戰情白版_新申領藥品.BackColor = Color.Red;
                rJ_Lable_戰情白版_新申領藥品.ForeColor = Color.White;
            }));
            bool flag_Alarm = false;
            for (int i = 0; i < list_緊急申領.Count; i++)
            {
                string GUID = list_緊急申領[i][(int)enum_藥局_緊急申領.GUID].ObjectToString();
                list_檢查緊急申領_已警示_buf = list_檢查緊急申領_已警示.GetRows((int)enum_藥局_緊急申領.GUID, GUID);
                if(list_檢查緊急申領_已警示_buf.Count > 0)
                {
                    continue;
                }
                flag_Alarm = true;
                list_檢查緊急申領_已警示.Add(list_緊急申領[i]);
            }
            if (!flag_Alarm) return;
            MyTimer myTimer_break = new MyTimer();
            myTimer_break.StartTickTime(10000);

            MyTimer myTimer = new MyTimer();
         
            int cnt = 0;
            while(true)
            {
                if (myTimer_break.IsTimeOut()) break;
                if(cnt == 0)
                {
                    this.Invoke(new Action(delegate
                    {
                        Basic.Voice.GoogleSpeaker("有新申領請求", $@"speak.mp3");
                    }));
      
                    myTimer.StartTickTime(2000);
                    this.Invoke(new Action(delegate
                    {
                        rJ_Lable_戰情白版_新申領藥品.BackColor = Color.Red;
                        rJ_Lable_戰情白版_新申領藥品.ForeColor = Color.White;
                    }));               
                    cnt++;
                }
                if (cnt == 1)
                {
                    if (myTimer.IsTimeOut())
                    {
                        myTimer.StartTickTime(2000);
                        this.Invoke(new Action(delegate
                        {
                            rJ_Lable_戰情白版_新申領藥品.BackColor = Color.White;
                            rJ_Lable_戰情白版_新申領藥品.ForeColor = Color.Black;
                        }));
                        cnt++;
                    }
                }
                if(cnt == 2)
                {
                    if (myTimer.IsTimeOut())
                    {
                        cnt++;
                    }                     
                }
                if (cnt == 3)
                {
                    cnt = 0;
                }
                System.Threading.Thread.Sleep(10);
            }
            this.Invoke(new Action(delegate
            {
                rJ_Lable_戰情白版_新申領藥品.Visible = false;
            }));
        }
        private void PlC_RJ_Button_戰情白板_公告_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_戰情白板_公告.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取公告欄位!");
                return;
            }
            list_value[0][(int)enum_戰情白板_公告.內容] = rJ_TextBox_戰情白板_公告_內容.Text;
            list_value[0][(int)enum_戰情白板_公告.登錄時間] = DateTime.Now.ToDateTimeString_6();
            this.sqL_DataGridView_戰情白板_公告.SQL_ReplaceExtra(list_value[0], false);
            this.sqL_DataGridView_戰情白板_公告.ReplaceExtra(list_value[0], true);

        }
        private void PlC_RJ_Button_戰情白板_公告_刷新_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_戰情白板_公告.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_戰情白板_自選藥品_藥品資料_藥品碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_戰情白板_自選藥品_藥品資料.SQL_GetAllRows(false);          
            list_value = list_value.GetRowsByLike((int)enum_medDrugstore.藥品碼, this.rJ_TextBox_戰情白板_自選藥品_藥品資料_藥品碼.Text);
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
                return;
            }
            this.sqL_DataGridView_戰情白板_自選藥品_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_戰情白板_自選藥品_藥品資料_藥品名稱搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_戰情白板_自選藥品_藥品資料.SQL_GetAllRows(false);           
            list_value = list_value.GetRowsByLike((int)enum_medDrugstore.藥品名稱, this.rJ_TextBox_戰情白板_自選藥品_藥品資料_藥品名稱.Text);
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
                return;
            }
            this.sqL_DataGridView_戰情白板_自選藥品_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_戰情白板_自選藥品_藥品資料_填入資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_戰情白板_自選藥品_藥品資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取藥品!");
                return;
            }
            List<object[]> list_自選藥品 = this.sqL_DataGridView_戰情白板_自選藥品_選取內容.SQL_GetAllRows(false);
            List<object[]> list_自選藥品_buf = new List<object[]>();
            string 藥品碼 = list_value[0][(int)enum_medDrugstore.藥品碼].ObjectToString();
            string 藥品名稱 = list_value[0][(int)enum_medDrugstore.藥品名稱].ObjectToString();
            string 包裝單位 = list_value[0][(int)enum_medDrugstore.包裝單位].ObjectToString();
            list_自選藥品_buf = list_自選藥品.GetRows((int)enum_戰情白板_自選藥品.藥碼, 藥品碼);
            if (list_自選藥品_buf.Count == 0)
            {
                object[] value = new object[new enum_戰情白板_自選藥品().GetLength()];
                value[(int)enum_戰情白板_自選藥品.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_戰情白板_自選藥品.藥碼] = 藥品碼;
                value[(int)enum_戰情白板_自選藥品.藥名] = 藥品名稱;
                value[(int)enum_戰情白板_自選藥品.單位] = 包裝單位;
                this.sqL_DataGridView_戰情白板_自選藥品_選取內容.SQL_AddRow(value, false);
                this.sqL_DataGridView_戰情白板_自選藥品_選取內容.AddRow(value, true);
            }
            else
            {
                object[] value = list_自選藥品_buf[0];
                value[(int)enum_戰情白板_自選藥品.藥碼] = 藥品碼;
                value[(int)enum_戰情白板_自選藥品.藥名] = 藥品名稱;
                value[(int)enum_戰情白板_自選藥品.單位] = 包裝單位;
                this.sqL_DataGridView_戰情白板_自選藥品_選取內容.SQL_ReplaceExtra(value, false);
                this.sqL_DataGridView_戰情白板_自選藥品_選取內容.ReplaceExtra(value, true);
            }
        }
        private void PlC_RJ_Button_戰情白板_自選藥品_更新_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_戰情白板_自選藥品_選取內容.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_戰情白板_自選藥品_刪除已選擇藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_戰情白板_自選藥品_選取內容.Get_All_Checked_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取藥品!");
                return;
            }
            this.sqL_DataGridView_戰情白板_自選藥品_選取內容.SQL_DeleteExtra(list_value, false);
            this.sqL_DataGridView_戰情白板_自選藥品_選取內容.DeleteExtra(list_value, true);

            MyMessageBox.ShowDialog("刪除完畢!");
        }
        #endregion
        private class ICP_戰情白板 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                int temp = (x[(int)enum_戰情白板.狀態文字].ObjectToString().CompareTo(y[(int)enum_戰情白板.狀態文字].ObjectToString()));
                if (temp == 0)
                {
                    string Code0 = x[(int)enum_戰情白板.藥碼].ObjectToString();
                    string Code1 = y[(int)enum_戰情白板.藥碼].ObjectToString();
                    return Code0.CompareTo(Code1);
                }
                return temp;
            }
        }
        private class ICP_戰情白板_公告 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                int temp = (x[(int)enum_戰情白板_公告.排序].ObjectToString().CompareTo(y[(int)enum_戰情白板_公告.排序].ObjectToString()));

                return temp;
            }
        }
    }
}
