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
using System.Text.RegularExpressions;
namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {
        private string 登入者名稱 = "系統";
        private PLC_Device PLC_Device_最高權限 = new PLC_Device("S4077");

        private MyConvert myConvert = new MyConvert();
        private Stopwatch stopwatch = new Stopwatch();
        private const string DBConfigFileName = "DBConfig.txt";
        public DBConfigClass dBConfigClass = new DBConfigClass();
        public class DBConfigClass
        {
            private SQL_DataGridView.ConnentionClass dB_Basic = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_order_server = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_DS01 = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_Medicine_page = new SQL_DataGridView.ConnentionClass();

            public SQL_DataGridView.ConnentionClass DB_Basic { get => dB_Basic; set => dB_Basic = value; }
            public SQL_DataGridView.ConnentionClass DB_Medicine_page { get => dB_Medicine_page; set => dB_Medicine_page = value; }
            public SQL_DataGridView.ConnentionClass DB_order_server { get => dB_order_server; set => dB_order_server = value; }
            public SQL_DataGridView.ConnentionClass DB_DS01 { get => dB_DS01; set => dB_DS01 = value; }
        }
        private void LoadDBConfig()
        {
            string jsonstr = MyFileStream.LoadFileAllText($".//{DBConfigFileName}");
            if (jsonstr.StringIsEmpty())
            {
                jsonstr = Basic.Net.JsonSerializationt<DBConfigClass>(new DBConfigClass(), true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{DBConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{DBConfigFileName}檔案失敗!");
                }
                MyMessageBox.ShowDialog($"未建立參數文件!請至子目錄設定{DBConfigFileName}");
                Application.Exit();
            }
            else
            {
                dBConfigClass = Basic.Net.JsonDeserializet<DBConfigClass>(jsonstr);

                jsonstr = Basic.Net.JsonSerializationt<DBConfigClass>(dBConfigClass, true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{DBConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{DBConfigFileName}檔案失敗!");
                }

            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.LoadDBConfig();
                this.plC_UI_Init.Run(this.FindForm(), this.lowerMachine_Panel);
                MyMessageBox.音效 = false;
                MyMessageBox.form = this.FindForm();
               
                this.plC_RJ_Button_測試.MouseDownEvent += PlC_RJ_Button_測試_MouseDownEvent;
                this.plC_UI_Init.UI_Finished_Event += PlC_UI_Init_UI_Finished_Event;
            } 
        }

      

        private void PlC_UI_Init_UI_Finished_Event()
        {
            if(!this.DesignMode)
            {
                this.PLC_Device_最高權限.Bool = true;

                PLC_UI_Init.Set_PLC_ScreenPage(this.panel_Main, this.plC_ScreenPage_Main);
                PLC_UI_Init.Set_PLC_ScreenPage(this.panel_過帳明細, this.plC_ScreenPage_過帳明細);
                PLC_UI_Init.Set_PLC_ScreenPage(this.panel_藥品資料, this.plC_ScreenPage_藥品資料);
                PLC_UI_Init.Set_PLC_ScreenPage(this.panel_驗收入庫, this.plC_ScreenPage_驗收入庫);

                this.Function_Init();

                this.sub_Program_雲端_藥品資料_Init();
                this.sub_Program_本地_藥品資料_Init();
                this.sub_Program_藥局_藥品資料_Init();
                this.sub_Program_藥庫_藥品資料_Init();
                this.sub_Program_藥品資料更新_Init();

                this.sub_Program_寫入報表設定_Init();
                this.sub_Program_過帳狀態查詢_Init();
                this.sub_Program_過帳明細_門診_Init();
                this.sub_Program_過帳明細_急診_Init();
                this.sub_Program_過帳明細_住院_Init();
                this.sub_Program_過帳明細_公藥_Init();

                this.sub_Program_藥品過消耗帳_Init();

                this.sub_Program_藥品補給系統_Init();
                this.sub_Program_藥庫_每日訂單_Init();
                this.sub_Program_藥庫_藥品資料_Init();

                this.sub_Program_交易紀錄查詢_Init();
                this.sub_Program_補給驗收入庫_Init();
                this.sub_Program_驗收入庫明細_Init();
                this.sub_Program_補給驗收_驗收入庫_Init();


                this.sub_Program_Log_Init();

                this.sub_Program_藥庫_撥補_藥局_自動撥補_Init();

            }
 
        }

        private void PlC_RJ_Button_測試_MouseDownEvent(MouseEventArgs mevent)
        {
            bool flag = Basic.FileIO.ServerFileCopy(@"HTS81P.ptvgh.gov.tw\MIS\DG", "itinvd0304.txt", @"C:\Users\hsonds01\Desktop\", "test.txt", "hsonds01", "KuT1Ch@75511");
            MyMessageBox.ShowDialog($"{flag}");
        }
        #region PLC_Method
        PLC_Device PLC_Device_Method = new PLC_Device("");
        PLC_Device PLC_Device_Method_OK = new PLC_Device("");
        Task Task_Method;
        MyTimer MyTimer_Method_結束延遲 = new MyTimer();
        int cnt_Program_Method = 65534;
        void sub_Program_Method()
        {
            if (cnt_Program_Method == 65534)
            {
                this.MyTimer_Method_結束延遲.StartTickTime(10000);
                PLC_Device_Method.SetComment("PLC_Method");
                PLC_Device_Method_OK.SetComment("PLC_Method_OK");
                PLC_Device_Method.Bool = false;
                cnt_Program_Method = 65535;
            }
            if (cnt_Program_Method == 65535) cnt_Program_Method = 1;
            if (cnt_Program_Method == 1) cnt_Program_Method_檢查按下(ref cnt_Program_Method);
            if (cnt_Program_Method == 2) cnt_Program_Method_初始化(ref cnt_Program_Method);
            if (cnt_Program_Method == 3) cnt_Program_Method = 65500;
            if (cnt_Program_Method > 1) cnt_Program_Method_檢查放開(ref cnt_Program_Method);

            if (cnt_Program_Method == 65500)
            {
                this.MyTimer_Method_結束延遲.TickStop();
                this.MyTimer_Method_結束延遲.StartTickTime(10000);
                PLC_Device_Method.Bool = false;
                PLC_Device_Method_OK.Bool = false;
                cnt_Program_Method = 65535;
            }
        }
        void cnt_Program_Method_檢查按下(ref int cnt)
        {
            if (PLC_Device_Method.Bool) cnt++;
        }
        void cnt_Program_Method_檢查放開(ref int cnt)
        {
            if (!PLC_Device_Method.Bool) cnt = 65500;
        }
        void cnt_Program_Method_初始化(ref int cnt)
        {
            if (this.MyTimer_Method_結束延遲.IsTimeOut())
            {
                if (Task_Method == null)
                {
                    Task_Method = new Task(new Action(delegate { }));
                }
                if (Task_Method.Status == TaskStatus.RanToCompletion)
                {
                    Task_Method = new Task(new Action(delegate { }));
                }
                if (Task_Method.Status == TaskStatus.Created)
                {
                    Task_Method.Start();
                }
                cnt++;
            }
        }







        #endregion
    }
}
