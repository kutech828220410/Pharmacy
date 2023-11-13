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
using System.Reflection;
using MyUI;
using Basic;
using MySql.Data.MySqlClient;
using SQLUI;
using H_Pannel_lib;
using System.Net.Http;
using System.IO;
using HIS_DB_Lib;

[assembly: AssemblyVersion("1.0.51.13")]
[assembly: AssemblyFileVersion("1.0.51.13")]
namespace 智能藥庫系統
{

    public partial class Form1 : Form
    {
        private string Api_URL = "http://127.0.0.1:4435";
        private string FormText = "";
        private MyTimer MyTimer_TickTime = new MyTimer();
        private MyConvert myConvert = new MyConvert();
        private Stopwatch stopwatch = new Stopwatch();
        private const string DBConfigFileName = "DBConfig.txt";
        private const string MyConfigFileName = "MyConfig.txt";
        public MyConfigClass myConfigClass = new MyConfigClass();
        public DBConfigClass dBConfigClass = new DBConfigClass();
        private PLC_Device PLC_Device_主機模式 = new PLC_Device("S1050");
        private PLC_Device PLC_Device_滑鼠左鍵按下 = new PLC_Device("S4600");
        private PLC_Device PLC_Device_M8013 = new PLC_Device("M8013");
        private PLC_Device PLC_Device_主頁面頁碼 = new PLC_Device("D0");
        public static string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public class DBConfigClass
        {
            private SQL_DataGridView.ConnentionClass dB_Basic = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_person_page = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_Medicine_Cloud = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_posting_server = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_order_server = new SQL_DataGridView.ConnentionClass();

            private string emg_apply_ApiURL = "";
            private string medPrice_ApiURL = "";


            public SQL_DataGridView.ConnentionClass DB_Basic { get => dB_Basic; set => dB_Basic = value; }
            public SQL_DataGridView.ConnentionClass DB_person_page { get => dB_person_page; set => dB_person_page = value; }
            public SQL_DataGridView.ConnentionClass DB_Medicine_Cloud { get => dB_Medicine_Cloud; set => dB_Medicine_Cloud = value; }
            public SQL_DataGridView.ConnentionClass DB_posting_server { get => dB_posting_server; set => dB_posting_server = value; }
            public SQL_DataGridView.ConnentionClass DB_order_server { get => dB_order_server; set => dB_order_server = value; }
            public string Emg_apply_ApiURL { get => emg_apply_ApiURL; set => emg_apply_ApiURL = value; }
            public string MedPrice_ApiURL { get => medPrice_ApiURL; set => medPrice_ApiURL = value; }
        }
        public class MyConfigClass
        {
            private string fTP_Server = "";
            private string fTP_username = "";
            private string fTP_password = "";
            private int _貨架數量 = 8;
            private bool _主機模式 = false;

            public string FTP_Server { get => fTP_Server; set => fTP_Server = value; }
            public int 貨架數量 { get => _貨架數量; set => _貨架數量 = value; }
            public bool 主機模式 { get => _主機模式; set => _主機模式 = value; }
            public string FTP_username { get => fTP_username; set => fTP_username = value; }
            public string FTP_password { get => fTP_password; set => fTP_password = value; }
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void LoadDBConfig()
        {
            string jsonstr = MyFileStream.LoadFileAllText($"{currentDirectory}/{DBConfigFileName}");
            if (jsonstr.StringIsEmpty())
            {
                jsonstr = Basic.Net.JsonSerializationt<DBConfigClass>(new DBConfigClass() ,true);
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
        private void LoadMyConfig()
        {
            string jsonstr = MyFileStream.LoadFileAllText($"{currentDirectory}/{MyConfigFileName}");
            if (jsonstr.StringIsEmpty())
            {
                jsonstr = Basic.Net.JsonSerializationt<MyConfigClass>(new MyConfigClass(), true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{MyConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{MyConfigFileName}檔案失敗!");
                }
                MyMessageBox.ShowDialog($"未建立參數文件!請至子目錄設定{MyConfigFileName}");
                Application.Exit();
            }
            else
            {
                myConfigClass = Basic.Net.JsonDeserializet<MyConfigClass>(jsonstr);

                jsonstr = Basic.Net.JsonSerializationt<MyConfigClass>(myConfigClass, true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{MyConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{MyConfigFileName}檔案失敗!");
                }

            }

            //this.ftp_DounloadUI1.FTP_Server = myConfigClass.FTP_Server;
        }
        private void ApiServerSetting(string Name)
        {
            string json_result = Basic.Net.WEBApiGet($"{Api_URL}/api/ServerSetting");
            if (json_result.StringIsEmpty())
            {
                MyMessageBox.ShowDialog("API Server 連結失敗!");
                return;
            }
            Console.WriteLine(json_result);
            returnData returnData = json_result.JsonDeserializet<returnData>();
            List<HIS_DB_Lib.ServerSettingClass> serverSettingClasses = returnData.Data.ObjToListClass<ServerSettingClass>();
            HIS_DB_Lib.ServerSettingClass serverSettingClass;
            string ServerName = Name;
            serverSettingClass = serverSettingClasses.MyFind(Name, enum_ServerSetting_Type.藥庫, enum_ServerSetting_藥庫.藥檔資料);
            if (serverSettingClass != null)
            {
                dBConfigClass.DB_Medicine_Cloud.IP = serverSettingClass.Server;
                dBConfigClass.DB_Medicine_Cloud.Port = (uint)(serverSettingClass.Port.StringToInt32());
                dBConfigClass.DB_Medicine_Cloud.DataBaseName = serverSettingClass.DBName;
                dBConfigClass.DB_Medicine_Cloud.UserName = serverSettingClass.User;
                dBConfigClass.DB_Medicine_Cloud.Password = serverSettingClass.Password;
            }
            serverSettingClass = serverSettingClasses.MyFind(Name, enum_ServerSetting_Type.藥庫, enum_ServerSetting_藥庫.本地端);
            if (serverSettingClass != null)
            {
                dBConfigClass.DB_Basic.IP = serverSettingClass.Server;
                dBConfigClass.DB_Basic.Port = (uint)(serverSettingClass.Port.StringToInt32());
                dBConfigClass.DB_Basic.DataBaseName = serverSettingClass.DBName;
                dBConfigClass.DB_Basic.UserName = serverSettingClass.User;
                dBConfigClass.DB_Basic.Password = serverSettingClass.Password;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
         

            if (this.DesignMode == false)
            {
                MyMessageBox.form = this.FindForm();
                this.LoadMyConfig();
                this.LoadDBConfig();
                this.ftp_DounloadUI.FTP_Server = myConfigClass.FTP_Server;
                this.ftp_DounloadUI.Username = myConfigClass.FTP_username;
                this.ftp_DounloadUI.Password = myConfigClass.FTP_password;
                //string updateVersion = this.ftp_DounloadUI.GetFileVersion();
                //if (this.ftp_DounloadUI.CheckUpdate(this.ProductVersion, updateVersion))
                //{
                //    this.Invoke(new Action(delegate { this.Update(); }));
                //}


                this.Text += "Ver" + this.ProductVersion;
                this.FormText = this.Text;
                this.WindowState = FormWindowState.Maximized;
                this.plC_RJ_Button_測試.MouseDownEvent += PlC_RJ_Button_測試_MouseDownEvent;
                MyMessageBox.音效 = false;
                Dialog_Prcessbar.form = this.FindForm();
                Dialog_寫入批號.form = this.FindForm();
                Dialog_寫入效期.form = this.FindForm();
                Dialog_輸入備註.form = this.FindForm();
                Dialog_寫入藥品碼.form = this.FindForm();
                Dialog_更換密碼.form = this.FindForm();
                Dialog_藥品資料設定.form = this.FindForm();
                Dialog_效期批號歷史紀錄.form = this.FindForm();
                Dialog_補給系統藥品建置.form = this.FindForm();
                Dialog_日期選擇.form = this.FindForm();
                ApiServerSetting("DS01");
                this.plC_UI_Init.Run(this.FindForm(), this.lowerMachine_Panel1);
                this.plC_UI_Init.UI_Finished_Event += PlC_UI_Init_UI_Finished_Event;
            }
        }
    
        private void PlC_UI_Init_UI_Finished_Event()
        {
            PLC_UI_Init.Set_PLC_ScreenPage(panel_Main, this.plC_ScreenPage_Main);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥庫, this.plC_ScreenPage_藥庫);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥庫_儲位設定, this.plC_ScreenPage_藥庫_儲位設定);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥庫_撥補, this.plC_ScreenPage_藥庫_撥補);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥庫_撥補_藥局, this.plC_ScreenPage_藥庫_撥補_藥局);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥庫_緊急訂單, this.plC_ScreenPage_藥庫_緊急訂單);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥庫_驗收入庫, this.plC_ScreenPage_藥庫_驗收入庫);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥庫_每日訂單, this.plC_ScreenPage_藥庫_每日訂單);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥品資料, this.plC_ScreenPage_藥品資料);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_系統, this.plC_ScreenPage_系統);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_系統_Pannel設定, this.plC_ScreenPage_系統_Pannel設定);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_批次過帳, this.plC_ScreenPage_批次過帳);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥局_屏東榮總, this.plC_ScreenPage_藥局_屏東榮總);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_人員資料, this.plC_ScreenPage_人員資料);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_盤點作業, this.plC_ScreenPage_盤點作業);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_周邊設備, this.plC_ScreenPage_周邊設備);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_周邊設備_麻醉部ADC, this.plC_ScreenPage_周邊設備_麻醉部ADC);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_周邊設備_ICU_ADC_01, this.plC_ScreenPage_周邊設備_ICU_ADC_01);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_戰情白板, this.plC_ScreenPage_戰情白板);           

            SQLUI.SQL_DataGridView.SQL_Set_Properties(dBConfigClass.DB_Basic.DataBaseName, dBConfigClass.DB_Basic.UserName, dBConfigClass.DB_Basic.Password, dBConfigClass.DB_Basic.IP, dBConfigClass.DB_Basic.Port, dBConfigClass.DB_Basic.MySqlSslMode, this.FindForm());

            //this.PLC_Device_主機模式.Bool = myConfigClass.主機模式;

            this.sub_Program_藥品補給系統_Init();

            this.sub_Program_系統_Init();
            this.sub_Program_人員資料_Init();
            this.sub_Program_盤點作業_定盤_Init();
            this.sub_Program_盤點合併_Init();
            this.sub_Program_登入畫面_Init();
            this.sub_Program_交易紀錄查詢_Init();
            this.sub_Program_藥品資料_資料維護_Init();
            this.sub_Program_藥品資料_藥品群組_Init();
            this.sub_Program_藥庫_藥品資料_Init();
            this.sub_Program_藥庫_儲位設定_Init();
            this.sub_Program_藥庫_儲位設定_區域儲位_Init();
            this.sub_Program_藥庫_儲位設定_Pannel35_Init();
            this.sub_Program_藥庫_入庫_Init();
            this.sub_Program_藥庫_每日訂單_下訂單_Init();
            this.sub_Program_藥庫_每日訂單_訂單查詢_Init();
            this.sub_Program_藥庫_每日訂單_檢視驗收入庫量_Init();


            this.sub_Program_藥庫_撥補_藥局_自動撥補_Init();
            this.sub_Program_藥庫_撥補_藥局_緊急申領_Init();
            this.sub_Program_藥庫_驗收入庫_過帳明細_Init();
            this.sub_Program_藥庫_驗收入庫_補給驗收_Init();

            this.sub_Program_藥庫_緊急訂單_下訂單_Init();
            this.sub_Program_藥庫_緊急訂單_信箱設定_Init();
            this.sub_Program_藥庫_緊急訂單_訂單列表_Init();

            this.sub_Program_批次過帳_門診_Init();
            this.sub_Program_批次過帳_急診_Init();
            this.sub_Program_批次過帳_住院_Init();
            this.sub_Program_批次過帳_公藥_Init();
            this.sub_Program_藥品過消耗帳_Init();


            this.sub_Program_藥局_藥品資料_Init();
            this.sub_Program_藥局_緊急申領_Init();


            this.sun_Program_堆疊資料_Init();
            this.sub_Program_工程模式_Init();

            this.sub_Program_寫入報表設定_Init();

            this.sub_Program_周邊設備_麻醉部ADC_庫存_Init();
            this.sub_Program_周邊設備_麻醉部ADC_抽屜狀態_Init(); 
            this.sub_Program_周邊設備_ICU_ADC_01_庫存_Init();
            this.sub_Program_周邊設備_ICU_ADC_01_抽屜狀態_Init();
            this.sub_Program_戰情白板_Init();


            this.Function_堆疊資料_刪除指定調劑台名稱母資料("藥庫");
            this.WindowState = FormWindowState.Maximized;
            Basic.Keyboard.Hook.KeyDown += Hook_KeyDown;
            Basic.Keyboard.Hook.MouseDown += Hook_MouseDown;
            Basic.Keyboard.Hook.MouseUp += Hook_MouseUp;
        }

      

        MyTimer myTimer_登出計時 = new MyTimer();
        private void Hook_MouseDown(int nCode, int mouse_x, int mouse_y)
        {
            if(this.CanFocus)
            {
                this.myTimer_登出計時.TickStop();
                this.myTimer_登出計時.StartTickTime(600000);
            }
            this.PLC_Device_滑鼠左鍵按下.Bool = true;
        }

        private void Hook_KeyDown(int nCode, IntPtr wParam, Keys Keys)
        {
             if (this.CanFocus)
            {
                this.myTimer_登出計時.TickStop();
                this.myTimer_登出計時.StartTickTime(600000);
            }
        }
        private void Hook_MouseUp(int nCode, int mouse_x, int mouse_y)
        {
            this.PLC_Device_滑鼠左鍵按下.Bool = false;
        }
        private class medclass
        {
            private string Code = "";
            private string Inventory_new = "";

            public string code { get => Code; set => Code = value; }
            public string inventory_new { get => Inventory_new; set => Inventory_new = value; }

            public medclass(string code , string inventory_new)
            {
                this.code = code;
                this.inventory_new = inventory_new;
            }
        }
        
        private void RJ_Button1_MouseDownEvent(MouseEventArgs mevent)
        {
            List<string> list_file_text = MyFileStream.LoadFile($@"C:\藥庫帳務資料表\DIMU2OB1.txt");
            for (int i = 0; i < list_file_text.Count; i++)
            {
                int len = list_file_text[i].Length;
                if (len == 28)
                {
                    string 藥品碼 = list_file_text[i].Substring(0, 5);
                    string 藥局代碼 = list_file_text[i].Substring(5, 4);
                    藥局代碼 = 藥局代碼.Replace(" ", "");
                    string Year = list_file_text[i].Substring(9, 4);
                    string Month = list_file_text[i].Substring(13, 2);
                    string Day = list_file_text[i].Substring(15, 2);
                    string Hour = list_file_text[i].Substring(17, 2);
                    string Min = list_file_text[i].Substring(19, 2);
                    string Date = $"{Year}/{Month}/{Day} {Hour}:{Min}:00";
                    int 異動量 = list_file_text[i].Substring(21, 7).StringToInt32();
                }
            }
           
        }
        private void PlC_RJ_Button_測試_MouseDownEvent(MouseEventArgs mevent)
        {
            bool flag = Basic.FileIO.ServerFileCopy(@"HTS81P.ptvgh.gov.tw\MIS\DG", "itinvd0304.txt", @"C:\Users\HS1T16\Desktop\", "test.txt", "hsonds01", "KuT1Ch@75511");
            Console.WriteLine($"ServerFileCopy {(flag? "sucess" : "fail")} !");
        }
        private void timer_init_Tick(object sender, EventArgs e)
        {
            if (this.plC_UI_Init.Init_Finish)
            {

                this.timer_init.Enabled = false;
            }
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

        private void Update()
        {
            //try
            //{
            //    if (this.ftp_DounloadUI.DownloadFile())
            //    {
            //        if (this.ftp_DounloadUI.SaveFile())
            //        {
            //            this.ftp_DounloadUI.RunFile(this.FindForm());
            //        }
            //        else
            //        {
            //            Basic.MyMessageBox.ShowDialog("安裝檔存檔失敗!");
            //        }
            //    }
            //    else
            //    {
            //        Basic.MyMessageBox.ShowDialog("下載失敗!");
            //    }
            //}
            //catch
            //{
            //    Application.Exit();
            //}
      

        }

    }
}
