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
        public enum enum_藥庫_每日訂單_下訂單 : int
        {
            GUID,
            藥品碼,
            中文名稱,
            藥品名稱,            
            包裝單位,
            包裝數量,
            藥庫庫存,
            總庫存,      
            安全庫存,
            基準量,
            今日訂購數量,
            在途量,
            未消耗量,
        }
        public enum enum_每日訂單
        {
            GUID,
            藥品碼,
            今日訂購數量,
            緊急訂購數量,
            訂購時間,
            狀態,
        }

        private API_OrderClass API_OrderClass_每日訂單_訂購數量 = new API_OrderClass();
        private API_OrderClass API_OrderClass_每日訂單_今日訂購數量_Buf = new API_OrderClass();
        private API_OrderClass API_OrderClass_每日訂單_緊急訂購數量_Buf = new API_OrderClass();
        private API_OrderClass API_OrderClass_每日訂單_在途量_Buf = new API_OrderClass();
        private List<object[]> list_每日訂單_消耗帳 = new List<object[]>();

       [Serializable]
        public class API_OrderClass
        {
            private List<resultClass> _result = new List<resultClass>();
            [JsonPropertyName("Result")]
            public List<resultClass> Result { get => _result; set => _result = value; }

            [Serializable]
            public class resultClass
            {

                private string _code;
                private string _value;
                private string _orderDateTime;
                private string _success;

                public string code { get => _code; set => _code = value; }
                public string value { get => _value; set => _value = value; }
                public string success { get => _success; set => _success = value; }
                public string orderDateTime { get => _orderDateTime; set => _orderDateTime = value; }

                public resultClass(string code ,string value, string orderDateTime)
                {
                    this.code = code;
                    this.value = value;
                    this.orderDateTime = orderDateTime;
                }
            }
            public void 清除全部()
            {
                Result.Clear();
            }
            public void 新增數量(string code, int num)
            {
                this.新增數量(code, num, "");
            }
            public void 新增數量(string code, int num, string orderDateTime)
            {
                if (num < 0) return;

                List<resultClass> Result_buf = new List<resultClass>();
                Result_buf = (from value in Result
                              where value.code == code
                              select value).ToList();
                if (Result_buf.Count > 0)
                {
                    if (num == 0)
                    {
                        Result.Remove(Result_buf[0]);
                        return;
                    }
                    Result_buf[0].value = (num + Result_buf[0].value.StringToInt32()).ToString();
                }
                else
                {
                    if (num == 0) return;
                    this.Result.Add(new resultClass(code, num.ToString(), orderDateTime));
                }
            }
        
            public void 新增藥品(string code)
            {
                this.新增藥品(code, 0);
            }
            public void 新增藥品(string code , int num)
            {
                
                List<resultClass> Result_buf = new List<resultClass>();
                Result_buf = (from value in Result
                              where value.code == code
                              select value).ToList();
                if(Result_buf.Count > 0)
                {
                    //if(num == 0)
                    //{
                    //    Result.Remove(Result_buf[0]);
                    //    return;
                    //}
                    Result_buf[0].value = num.ToString();
                }
                else
                {
                    //if (num == 0) return;
                    this.Result.Add(new resultClass(code, num.ToString(), ""));
                }
            }
            public int 取得數量(string code)
            {
                List<resultClass> Result_buf = new List<resultClass>();
                Result_buf = (from value in Result
                              where value.code == code
                              select value).ToList();
                if (Result_buf.Count > 0)
                {
                    return Result_buf[0].value.StringToInt32();
                }
                return 0;
            }
        }
        private void sub_Program_藥庫_每日訂單_下訂單_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_每日訂單, dBConfigClass.DB_posting_server);
            this.sqL_DataGridView_每日訂單.Init();
            if (!this.sqL_DataGridView_每日訂單.SQL_IsTableCreat()) this.sqL_DataGridView_每日訂單.SQL_CreateTable();


            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.Init();
            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.RowDoubleClickEvent += SqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料_RowDoubleClickEvent;
            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.DataGridRowsChangeRefEvent += SqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料_DataGridRowsChangeRefEvent;

            this.rJ_TextBox_藥庫_每日訂單_下訂單_藥品碼.KeyPress += RJ_TextBox_藥庫_每日訂單_下訂單_藥品碼_KeyPress;
            this.rJ_TextBox_藥庫_每日訂單_下訂單_藥品名稱.KeyPress += RJ_TextBox_藥庫_每日訂單_下訂單_藥品名稱_KeyPress;


            this.plC_RJ_Button_藥庫_每日訂單_下訂單_顯示全部.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_顯示在途藥品.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_顯示在途藥品_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_顯示已訂購藥品.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_顯示已訂購藥品_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_清除所有線上訂單.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_清除所有線上訂單_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_顯示低於安全量.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_顯示低於安全量_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_清除選取藥品訂單.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_清除選取藥品訂單_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_選取藥品補足基準量.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_選取藥品補足基準量_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_藥品碼搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_藥品碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_藥品名稱搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_藥品名稱搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_送出線上訂單.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_送出線上訂單_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_下訂單_檢查訂單是否送出.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_下訂單_檢查訂單是否送出_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_每日訂單_下訂單);
        }

   

        private bool flag_藥庫_每日訂單_下訂單 = false;
        private bool flag_藥庫_每日訂單_資料Init = false;
        private void sub_Program_藥庫_每日訂單_下訂單()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "每日訂單")
            {
                if (!this.flag_藥庫_每日訂單_下訂單)
                {
                 

                    this.Function_堆疊資料_刪除指定調劑台名稱母資料("藥庫");
                    this.flag_藥庫_每日訂單_下訂單 = true;
                }

            }
            else
            {
                flag_藥庫_每日訂單_資料Init = false;
                this.flag_藥庫_每日訂單_下訂單 = false;
            }
        }

        #region Function
        public List<object[]> Function_藥庫_每日訂單_下訂單_取得藥品資料()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            List<Task> tasks = new List<Task>();

            if (flag_藥庫_每日訂單_資料Init == false)
            {
                tasks.Add(Task.Run(new Action(delegate
                {
                    API_OrderClass_每日訂單_緊急訂購數量_Buf = this.Function_藥庫_每日訂單_下訂單_取得緊急訂購數量();
                })));
                tasks.Add(Task.Run(new Action(delegate
                {
                    API_OrderClass_每日訂單_在途量_Buf = this.Function_藥庫_每日訂單_下訂單_取得在途量();
                })));
                tasks.Add(Task.Run(new Action(delegate
                {
                    list_每日訂單_消耗帳 = Function_藥品過消耗帳_取得未過帳明細();
                })));
                Task.WhenAll(tasks).Wait();


                flag_藥庫_每日訂單_資料Init = true;
            }
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            Console.WriteLine($"取得藥品資料,耗時{myTimer.ToString()}");
            List<object[]> list_藥品資料_每日訂單 = new List<object[]>();
            list_藥品資料 = Function_藥庫_藥品資料_取得庫存(list_藥品資料);
            list_藥品資料_每日訂單 = list_藥品資料.CopyRows(new enum_medDrugstore(), new enum_藥庫_每日訂單_下訂單());
            Console.WriteLine($"轉換藥品資料成每日訂單藥品資料,耗時{myTimer.ToString()}");

            tasks.Clear();
            tasks.Add(Task.Run(new Action(delegate
            {
                //取得今日訂購數量
                //API_OrderClass_每日訂單_今日訂購數量_Buf = this.Function_藥庫_每日訂單_下訂單_取得今日訂購數量();
                for (int i = 0; i < list_藥品資料_每日訂單.Count; i++)
                {
                    list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.今日訂購數量] = "0";
                    string Code = list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
                    List<API_OrderClass.resultClass> resultClasses = (from value in API_OrderClass_每日訂單_今日訂購數量_Buf.Result
                                                                      where value.code == Code
                                                                      select value).ToList();
                    if (resultClasses.Count > 0)
                    {
                        list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.今日訂購數量] = resultClasses[0].value;
                    }
                }
                Console.WriteLine($"取得今日訂購數量資料,耗時{myTimer.ToString()}");
            })));
            tasks.Add(Task.Run(new Action(delegate
            {
                //取得緊急訂購數量
                //for (int i = 0; i < list_藥品資料_每日訂單.Count; i++)
                //{
                //    list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.緊急訂購數量] = "0";
                //    string Code = list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
                //    List<API_OrderClass.resultClass> resultClasses = (from value in API_OrderClass_每日訂單_緊急訂購數量_Buf.Result
                //                                                      where value.code == Code
                //                                                      select value).ToList();
                //    if (resultClasses.Count > 0)
                //    {
                //        list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.緊急訂購數量] = resultClasses[0].value;
                //    }
                //}
                Console.WriteLine($"取得緊急訂購數量資料,耗時{myTimer.ToString()}");
            })));
            tasks.Add(Task.Run(new Action(delegate
            {

                //取得在途量
                for (int i = 0; i < list_藥品資料_每日訂單.Count; i++)
                {
                    list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.在途量] = "0";
                    string Code = list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
                    List<API_OrderClass.resultClass> resultClasses = (from value in API_OrderClass_每日訂單_在途量_Buf.Result
                                                                      where value.code == Code
                                                                      select value).ToList();
                    if (resultClasses.Count > 0)
                    {
                        list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.在途量] = resultClasses[0].value;
                    }
                }
                Console.WriteLine($"取得在途量資料,耗時{myTimer.ToString()}");
            })));
            tasks.Add(Task.Run(new Action(delegate
            {
                string Code = "";
                int 未消耗量 = 0;
                List<object[]> list_消耗帳_buf = new List<object[]>();
                for (int i = 0; i < list_藥品資料_每日訂單.Count; i++)
                {
                    未消耗量 = 0;
                    Code = list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
                    list_消耗帳_buf = list_每日訂單_消耗帳.GetRows((int)enum_藥品過消耗帳.藥品碼, Code);
                    for (int k = 0; k < list_消耗帳_buf.Count; k++)
                    {
                        未消耗量 += list_消耗帳_buf[k][(int)enum_藥品過消耗帳.異動量].StringToInt32();
                    }
                    未消耗量 *= -1;
                    list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.未消耗量] = 未消耗量;
                }
            })));

            Task.WhenAll(tasks).Wait();

           
         

            return list_藥品資料_每日訂單;
        }
        public List<object[]> Function_藥庫_每日訂單_下訂單_取得每日訂單資料()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);

            List<object[]> list_value = new List<object[]>();
            List<object[]> list_寫入報表設定 = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
            list_寫入報表設定 = list_寫入報表設定.GetRows((int)enum_寫入報表設定.檔名, "每日訂單送出");
            if (list_寫入報表設定.Count == 0) return list_value;
            int hour = list_寫入報表設定[0][(int)enum_寫入報表設定.更新每日].ObjectToString().Substring(0, 2).StringToInt32();
            int min = list_寫入報表設定[0][(int)enum_寫入報表設定.更新每日].ObjectToString().Substring(2, 2).StringToInt32();

            DateTime dateTime_start;
            DateTime dateTime_end;

            DateTime dateTime_basic_start = DateTime.Now;
            DateTime dateTime_basic_end = dateTime_basic_start.AddDays(1);
            bool isholiday = false;
            while (true)
            {
                if (!Basic.TypeConvert.IsHspitalHolidays(dateTime_basic_start))
                {
                    break;
                }
                dateTime_basic_start = dateTime_basic_start.AddDays(-1);
                isholiday = true;
            }
          
            if (dateTime_basic_start.IsNewDay(hour, min) || isholiday)
            {
                dateTime_start = $"{dateTime_basic_start.ToDateString()} {hour}:{min}:00".StringToDateTime();
                dateTime_end = $"{dateTime_basic_end.ToDateString()} {hour}:{min}:00".StringToDateTime();
            }
            else
            {
                dateTime_end = $"{dateTime_basic_start.ToDateString()} {hour}:{min}:00".StringToDateTime();
                dateTime_start = dateTime_end.AddDays(-1);
            }
            while (true)
            {
                if (!Basic.TypeConvert.IsHspitalHolidays(dateTime_basic_end))
                {
                    break;
                }
                dateTime_basic_end = dateTime_basic_end.AddDays(1);
            }

            list_value = this.sqL_DataGridView_每日訂單.SQL_GetRowsByBetween((int)enum_每日訂單.訂購時間, dateTime_start, dateTime_end, false);
            //list_value = list_value.GetRowsInDate((int)enum_每日訂單.訂購時間, dateTime_start, dateTime_end);
            Console.WriteLine($"取得[每日訂單],耗時{myTimer.ToString()}");
            return list_value;
        }
        public API_OrderClass Function_藥庫_每日訂單_下訂單_取得今日訂購數量()
        {
            API_OrderClass aPI_OrderClass = new API_OrderClass();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            List<object[]> list_value = this.Function_藥庫_每日訂單_下訂單_取得每日訂單資料();



            for (int i = 0; i < list_value.Count; i++)
            {
                string 藥品碼 = list_value[i][(int)enum_每日訂單.藥品碼].ObjectToString();
                string 數量 = list_value[i][(int)enum_每日訂單.今日訂購數量].ObjectToString();
                string 訂購日期 = list_value[i][(int)enum_每日訂單.訂購時間].ToDateTimeString();
                if (!數量.StringIsInt32()) continue;
                if (!訂購日期.Check_Date_String()) continue;
                aPI_OrderClass.新增數量(藥品碼, 數量.StringToInt32(), 訂購日期);

            }
            return aPI_OrderClass;
        }
        public API_OrderClass Function_藥庫_每日訂單_下訂單_取得緊急訂購數量()
        {
            API_OrderClass aPI_OrderClass = new API_OrderClass();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);

            List<object[]> list_value = new List<object[]>();
            List<object[]> list_寫入報表設定 = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
            list_寫入報表設定 = list_寫入報表設定.GetRows((int)enum_寫入報表設定.檔名, "每日訂單送出");
            if (list_寫入報表設定.Count == 0) return aPI_OrderClass;
            int hour = list_寫入報表設定[0][(int)enum_寫入報表設定.更新每日].ObjectToString().Substring(0, 2).StringToInt32();
            int min = list_寫入報表設定[0][(int)enum_寫入報表設定.更新每日].ObjectToString().Substring(2, 2).StringToInt32();

            DateTime dateTime_start;
            DateTime dateTime_end;

            DateTime dateTime_basic_start = DateTime.Now;
            DateTime dateTime_basic_end = DateTime.Now.AddDays(1);
            bool isholiday = false;
            while (true)
            {
                if (!Basic.TypeConvert.IsHspitalHolidays(dateTime_basic_start))
                {
                    break;
                }
                dateTime_basic_start = dateTime_basic_start.AddDays(-1);
                isholiday = true;
            }
           
            if (dateTime_basic_start.IsNewDay(hour, min) || isholiday)
            {
                dateTime_start = $"{dateTime_basic_start.ToDateString()} {hour}:{min}:00".StringToDateTime();
                dateTime_end = $"{dateTime_basic_end.ToDateString()} {hour}:{min}:00".StringToDateTime();
            }
            else
            {
                dateTime_end = $"{dateTime_basic_start.ToDateString()} {hour}:{min}:00".StringToDateTime();
                dateTime_start = dateTime_end.AddDays(-1);
            }
            while (true)
            {
                if (!Basic.TypeConvert.IsHspitalHolidays(dateTime_basic_end))
                {
                    break;
                }
                dateTime_basic_end = dateTime_basic_end.AddDays(1);
            }

            List<object[]> list_訂單資料 = this.sqL_DataGridView_藥品補給系統_訂單資料.SQL_GetRowsByBetween((int)enum_藥品補給系統_訂單資料.訂購時間, dateTime_start, dateTime_end, false);
            list_訂單資料 = list_訂單資料.GetRowsByLike((int)enum_藥品補給系統_訂單資料.訂單編號, "EM");
            for (int i = 0; i < list_訂單資料.Count; i++)
            {
                string 藥品碼 = list_訂單資料[i][(int)enum_藥品補給系統_訂單資料.藥品碼].ObjectToString();
                string 數量 = list_訂單資料[i][(int)enum_藥品補給系統_訂單資料.訂購數量].ObjectToString();
                if (!數量.StringIsInt32()) continue;
                aPI_OrderClass.新增數量(藥品碼, 數量.StringToInt32());

            }
            return aPI_OrderClass;
        }
        public API_OrderClass Function_藥庫_每日訂單_下訂單_取得已訂購數量()
        {
            API_OrderClass aPI_OrderClass_今日訂購數量 = this.Function_藥庫_每日訂單_下訂單_取得今日訂購數量();
            API_OrderClass aPI_OrderClass_緊急訂購數量 = this.Function_藥庫_每日訂單_下訂單_取得緊急訂購數量();
            API_OrderClass aPI_OrderClass = new API_OrderClass();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            for (int i = 0; i < aPI_OrderClass_今日訂購數量.Result.Count; i++)
            {
                aPI_OrderClass.新增數量(aPI_OrderClass_今日訂購數量.Result[i].code, aPI_OrderClass_今日訂購數量.Result[i].value.StringToInt32());
            }
            for (int i = 0; i < aPI_OrderClass_緊急訂購數量.Result.Count; i++)
            {
                aPI_OrderClass.新增數量(aPI_OrderClass_緊急訂購數量.Result[i].code, aPI_OrderClass_緊急訂購數量.Result[i].value.StringToInt32());
            }
            return aPI_OrderClass_緊急訂購數量;
        }
        public API_OrderClass Function_藥庫_每日訂單_下訂單_取得線上已訂購數量()
        {
            API_OrderClass aPI_OrderClass = new API_OrderClass();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string serverfilepath = @"HTS81P.ptvgh.gov.tw\MIS\DG";
            string serverfilename = "itinvd0304.txt";
            string localfilepath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}";
            string localfilename = "itinvd0304.txt";
            string username = "hsonds01";
            string password = "KuT1Ch@75511";
            bool flag = Basic.FileIO.ServerFileCopy(serverfilepath, serverfilename, localfilepath, localfilename, username, password);
            Console.WriteLine($"取得Fileserver檔案 {(flag ? "sucess" : "fail")} ! ,耗時{myTimer.ToString()}");
            List<string> load_texts = Basic.MyFileStream.LoadFile($"{localfilepath}{localfilename}");
            for (int i = 0; i < load_texts.Count; i++)
            {
                string 藥品碼 = "";
                string 數量 = "";
                string 訂購日期 = "";
                this.Function_藥庫_每日訂單_下訂單_已訂購字串轉換(load_texts[i], ref 藥品碼, ref 數量, ref 訂購日期);
                if (訂購日期 != DateTime.Now.ToDateString()) continue;
                aPI_OrderClass.Result.Add(new API_OrderClass.resultClass(藥品碼, 數量, 訂購日期));

            }
            return aPI_OrderClass;
        }
        public API_OrderClass Function_藥庫_每日訂單_下訂單_取得在途量()
        {
            API_OrderClass aPI_OrderClass = new API_OrderClass();
            API_OrderClass aPI_OrderClass_out = new API_OrderClass();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string result = Basic.Net.WEBApiPostJson("https://wac01p.vghks.gov.tw:4430/ITWeb/jaxrs/ItCommon/pinmed_itm", "{\"hid\"   : [\"2A0\"]}");
            if(result.StringIsEmpty())
            {
                MyMessageBox.ShowDialog("在途量API呼叫失敗!");
                return aPI_OrderClass;
            }
            aPI_OrderClass = result.JsonDeserializet<API_OrderClass>();
            for (int i = 0; i < aPI_OrderClass.Result.Count; i++)
            {
                aPI_OrderClass.Result[i].code = Function_藥庫_每日訂單_下訂單_藥品碼轉換(aPI_OrderClass.Result[i].code);
            }
            for (int i = 0; i < aPI_OrderClass.Result.Count; i++)
            {
                aPI_OrderClass_out.新增數量(aPI_OrderClass.Result[i].code, aPI_OrderClass.Result[i].value.StringToInt32());
            }

            return aPI_OrderClass_out;
        }

        public void Function_藥庫_每日訂單_下訂單_更新藥品資料表單()
        {
            List<object[]> list_藥品資料 = Function_藥庫_每日訂單_下訂單_取得藥品資料();
            List<object[]> list_value = sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.GetAllRows();
            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_value_add = new List<object[]>();
            for (int i = 0; i < list_value.Count; i++)
            {
                list_value_buf = list_藥品資料.GetRows((int)enum_藥庫_每日訂單_下訂單.GUID, list_value[i][(int)enum_藥庫_每日訂單_下訂單.GUID].ObjectToString());
                if(list_value_buf.Count > 0)
                {
                    list_value_add.Add(list_value_buf[0]);
                }
            }
            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.RefreshGrid(list_value_add);
        }
        public string Function_藥庫_每日訂單_下訂單_藥品碼轉換(string value)
        {
            if (value.Length == 10)
            {
                value = value.Substring(value.Length - 5, 5);
            }
            else if (value.Length == 12)
            {
                value = value.Substring(value.Length - 7, 5);
            }
            return value;
        }
        public void Function_藥庫_每日訂單_下訂單_已訂購字串轉換(string text, ref string 藥品碼, ref string 數量, ref string 訂購日期)
        {
            藥品碼 = text.Substring(0, 15).Trim();
            數量 = text.Substring(15, 6).Trim();
            訂購日期 = text.Substring(21, 8).Trim();

            藥品碼 = this.Function_藥庫_每日訂單_下訂單_藥品碼轉換(藥品碼);
            訂購日期 = $"{訂購日期.Substring(0, 4)}/{訂購日期.Substring(4, 2)}/{訂購日期.Substring(6, 2)}";
            訂購日期 = 訂購日期.StringToDateTime().ToDateString();
        }
        public string Function_藥庫_每日訂單_下訂單_已訂購字串轉換(string 藥品碼, string 數量, string 訂購日期)
        {
            藥品碼 = $"A0000{藥品碼}";
            while (true)
            {
                if (藥品碼.Length >= 15) break;
                藥品碼 = 藥品碼 + " ";
            }
            while (true)
            {
                if (數量.Length >= 6) break;
                數量 = " " + 數量;
            }
            訂購日期 = 訂購日期.Replace("/", "");
            訂購日期 = 訂購日期.Replace("-", "");
            string text = $"{藥品碼}{數量}{訂購日期}2A0";
            return text;
        }
        public bool Function_藥庫_每日訂單_下訂單_訂單寫入FileServer(List<string> list_texts)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);

            string serverfilepath = @"HTS81P.ptvgh.gov.tw\MIS\DG";
            string serverfilename = "itinvd0304.txt";
            string localfilepath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}";
            string localfilename = "itinvd0304.txt";
            string username = "hsonds01";
            string password = "KuT1Ch@75511";

            bool flag = Basic.MyFileStream.SaveFile($"{localfilepath}{localfilename}", list_texts); 
            Console.WriteLine($"存至本地 {(flag ? "sucess" : "fail")} ! ,耗時{myTimer.ToString()}");
            if (!flag) return false;
            flag = Basic.FileIO.CopyToServer(serverfilepath, serverfilename, localfilepath, localfilename, username, password);
            Console.WriteLine($"上傳檔案至Fileserver {(flag ? "sucess" : "fail")} ! ,耗時{myTimer.ToString()}");
            if (!flag) return false;
            return true;
        }
        public void Function_藥庫_每日訂單_下訂單_今日訂購數量更新(API_OrderClass aPI_OrderClass)
        {
            List<string> _藥品碼 = new List<string>();
            List<int> _數量 = new List<int>();
            for(int i = 0; i < aPI_OrderClass.Result.Count; i++)
            {
                _藥品碼.Add(aPI_OrderClass.Result[i].code);
                _數量.Add(aPI_OrderClass.Result[i].value.StringToInt32());
            }
            this.Function_藥庫_每日訂單_下訂單_今日訂購數量更新(_藥品碼, _數量);
        }
        public void Function_藥庫_每日訂單_下訂單_今日訂購數量更新(string 藥品碼, int 數量)
        {
            List<string> _藥品碼 = new List<string>();
            List<int> _數量 = new List<int>();
            _藥品碼.Add(藥品碼);
            _數量.Add(數量);
            this.Function_藥庫_每日訂單_下訂單_今日訂購數量更新(_藥品碼, _數量);
        }
        public void Function_藥庫_每日訂單_下訂單_今日訂購數量更新(List<string> 藥品碼, List<int> 數量)
        {
            List<object[]> list_value = this.Function_藥庫_每日訂單_下訂單_取得每日訂單資料();
            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_value_add = new List<object[]>();
            List<object[]> list_value_replace = new List<object[]>();
            List<object[]> list_value_delete = new List<object[]>();
            for (int i = 0; i < 藥品碼.Count; i++)
            {
                list_value_buf = list_value.GetRows((int)enum_每日訂單.藥品碼, 藥品碼[i]);
                if (list_value_buf.Count == 0)
                {
                    object[] value = new object[new enum_每日訂單().GetLength()];
                    value[(int)enum_每日訂單.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_每日訂單.藥品碼] = 藥品碼[i];
                    value[(int)enum_每日訂單.今日訂購數量] = 數量[i];
                    value[(int)enum_每日訂單.緊急訂購數量] = "0";
                    value[(int)enum_每日訂單.訂購時間] = DateTime.Now.ToDateTimeString_6();
                    list_value_add.Add(value);

                }
                else if (list_value_buf.Count > 1)
                {
                    for(int k = 0; k < list_value_buf.Count; k++)
                    {
                        list_value_delete.Add(list_value_buf[k]);
                    }
                    object[] value = list_value_buf[0].DeepClone();
                    value[(int)enum_每日訂單.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_每日訂單.藥品碼] = 藥品碼[i];
                    value[(int)enum_每日訂單.今日訂購數量] = 數量[i];
                    value[(int)enum_每日訂單.緊急訂購數量] = "0";
                    value[(int)enum_每日訂單.訂購時間] = DateTime.Now.ToDateTimeString_6();
                    list_value_add.Add(value);
                }
                else
                {
                    object[] value = list_value_buf[0];
                    value[(int)enum_每日訂單.今日訂購數量] = 數量[i];
                    list_value_replace.Add(value);
                }
            }
            this.sqL_DataGridView_每日訂單.SQL_DeleteExtra(list_value_delete, false);
            this.sqL_DataGridView_每日訂單.SQL_AddRows(list_value_add, false);
            this.sqL_DataGridView_每日訂單.SQL_ReplaceExtra(list_value_replace, false);
        }

        public void Function_藥庫_每日訂單_下訂單_藥品補足基準量(List<object[]> list_value)
        {
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            API_OrderClass aPI_OrderClass = Function_藥庫_每日訂單_下訂單_取得已訂購數量();
            API_OrderClass aPI_OrderClass_今日訂購數量 = Function_藥庫_每日訂單_下訂單_取得今日訂購數量();

            for (int i = 0; i < list_value.Count; i++)
            {
                string code = list_value[i][(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_medDrugstore.藥品碼, code);

                int 包裝數量 = 0;
                if (list_藥品資料_buf.Count > 0)
                {
                    包裝數量 = list_藥品資料_buf[0][(int)enum_medDrugstore.包裝數量].StringToInt32();
                }
                else
                {
                    continue;
                }
                int 安全量 = list_value[i][(int)enum_藥庫_每日訂單_下訂單.安全庫存].ObjectToString().StringToInt32();
                //int 緊急訂購數量 = list_value[i][(int)enum_藥庫_每日訂單_下訂單.緊急訂購數量].ObjectToString().StringToInt32();
                int 基準量 = list_value[i][(int)enum_藥庫_每日訂單_下訂單.基準量].ObjectToString().StringToInt32();
                int 總庫存 = list_value[i][(int)enum_藥庫_每日訂單_下訂單.總庫存].ObjectToString().StringToInt32();
                int 在途量 = list_value[i][(int)enum_藥庫_每日訂單_下訂單.在途量].ObjectToString().StringToInt32();
                int 訂購量 = 0;
                if (總庫存 > 安全量) continue;
                if (安全量 <= 0) continue;
                if (基準量 <= 0) continue;
                if (總庫存 < 0) continue;
                if (基準量 <= 安全量) continue;
                訂購量 = 基準量 - (總庫存 + 在途量);
                if (訂購量 <= 0) continue;
                if (包裝數量 > 0)
                {
                    int temp0 = 訂購量 % 包裝數量;
                    int temp1 = 訂購量 / 包裝數量;
                    if (temp0 > 0)
                    {
                        temp1++;
                    }
                    訂購量 = 包裝數量 * temp1;
                }
                aPI_OrderClass_今日訂購數量.新增藥品(code, 訂購量);
            }


            this.Function_藥庫_每日訂單_下訂單_今日訂購數量更新(aPI_OrderClass_今日訂購數量);
            this.Function_藥庫_每日訂單_下訂單_更新藥品資料表單();
        }
        #endregion
        #region Event
        private void RJ_TextBox_藥庫_每日訂單_下訂單_藥品名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_藥庫_每日訂單_下訂單_藥品名稱搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_藥庫_每日訂單_下訂單_藥品碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_藥庫_每日訂單_下訂單_藥品碼搜尋_MouseDownEvent(null);
            }
        }
        private void SqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料_RowDoubleClickEvent(object[] RowValue)
        {
            List<Task> tasks = new List<Task>();
            API_OrderClass aPI_OrderClass = new API_OrderClass();
            tasks.Add(Task.Run(new Action(delegate
            {
                API_OrderClass_每日訂單_今日訂購數量_Buf = Function_藥庫_每日訂單_下訂單_取得今日訂購數量();   

            })));
            string Code = RowValue[(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
            Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
            if (dialog_NumPannel.ShowDialog() != DialogResult.Yes) return;

            Task.WhenAll(tasks).Wait();
            Task.Run(new Action(delegate
            {
                API_OrderClass_每日訂單_今日訂購數量_Buf.新增藥品(Code, dialog_NumPannel.Value);

                this.Function_藥庫_每日訂單_下訂單_今日訂購數量更新(API_OrderClass_每日訂單_今日訂購數量_Buf);
                this.Function_藥庫_每日訂單_下訂單_更新藥品資料表單();
            }));





        }
        private void SqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            List<object[]> rowsList = RowsList;
            List<Task> tasks = new List<Task>();

            RowsList = (from temp in RowsList
                        where temp[(int)enum_藥庫_每日訂單_下訂單.安全庫存].StringToInt32() > 0
                        select temp).ToList();

            //tasks.Add(Task.Run(new Action(delegate
            //{
            //    string Code = "";
            //    int 未消耗量 = 0;
            //    List<object[]> list_消耗帳 = Function_藥品過消耗帳_取得未過帳明細();
            //    List<object[]> list_消耗帳_buf = new List<object[]>();
            //    for(int i = 0; i < rowsList.Count; i ++)
            //    {
            //        未消耗量 = 0;
            //        Code = rowsList[i][(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
            //        list_消耗帳_buf = list_消耗帳.GetRows((int)enum_藥品過消耗帳.藥品碼, Code);
            //        for (int k = 0; k < list_消耗帳_buf.Count; k++)
            //        {
            //            未消耗量 +=  list_消耗帳_buf[k][(int)enum_藥品過消耗帳.異動量].StringToInt32();
            //        }
            //        未消耗量 *= -1;
            //        rowsList[i][(int)enum_藥庫_每日訂單_下訂單.未消耗量] = 未消耗量;
            //    }
            //})));

            Task.WhenAll(tasks).Wait();
            RowsList.Sort(new ICP_藥庫_每日訂單_下訂單());
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.RefreshGrid(this.Function_藥庫_每日訂單_下訂單_取得藥品資料());
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_顯示在途藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);

            string result = Basic.Net.WEBApiPostJson("https://wac01p.vghks.gov.tw:4430/ITWeb/jaxrs/ItCommon/pinmed_itm", "{\"hid\"   : [\"2A0\"]}");

            API_OrderClass api_在途量 = Function_藥庫_每日訂單_下訂單_取得在途量();
   
            List<object[]> list_藥品資料_每日訂單 = Function_藥庫_每日訂單_下訂單_取得藥品資料();
            List<object[]> list_藥品資料_每日訂單_buf = new List<object[]>();
            List<object[]> list_藥品資料_每日訂單_add = new List<object[]>();
            for (int i = 0; i < api_在途量.Result.Count; i++)
            {
                list_藥品資料_每日訂單_buf = list_藥品資料_每日訂單.GetRows((int)enum_藥庫_每日訂單_下訂單.藥品碼, api_在途量.Result[i].code);
                if(list_藥品資料_每日訂單_buf.Count > 0)
                {
                    list_藥品資料_每日訂單_buf[0][(int)enum_藥庫_每日訂單_下訂單.在途量] = api_在途量.Result[i].value;
                    list_藥品資料_每日訂單_add.Add(list_藥品資料_每日訂單_buf[0]);
                }
            }

            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.RefreshGrid(list_藥品資料_每日訂單_add);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_顯示已訂購藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            List<object[]> list_藥品資料_每日訂單 = Function_藥庫_每日訂單_下訂單_取得藥品資料();
            List<object[]> list_藥品資料_每日訂單_buf = new List<object[]>();
            List<object[]> list_藥品資料_每日訂單_add = new List<object[]>();
            API_OrderClass aPI_OrderClas_今日訂購數量 = Function_藥庫_每日訂單_下訂單_取得今日訂購數量();
            API_OrderClass aPI_OrderClas_緊急訂購數量 = Function_藥庫_每日訂單_下訂單_取得緊急訂購數量();
            for (int i = 0; i < aPI_OrderClas_今日訂購數量.Result.Count; i++)
            {

                list_藥品資料_每日訂單_buf = list_藥品資料_每日訂單.GetRows((int)enum_藥庫_每日訂單_下訂單.藥品碼, aPI_OrderClas_今日訂購數量.Result[i].code);
                if (list_藥品資料_每日訂單_buf.Count > 0)
                {
                    list_藥品資料_每日訂單_buf[0][(int)enum_藥庫_每日訂單_下訂單.今日訂購數量] = aPI_OrderClas_今日訂購數量.Result[i].value;
                    list_藥品資料_每日訂單_add.Add(list_藥品資料_每日訂單_buf[0]);
                }
            }
            //for (int i = 0; i < aPI_OrderClas_緊急訂購數量.Result.Count; i++)
            //{

            //    list_藥品資料_每日訂單_buf = list_藥品資料_每日訂單.GetRows((int)enum_藥庫_每日訂單_下訂單.藥品碼, aPI_OrderClas_緊急訂購數量.Result[i].code);
            //    if (list_藥品資料_每日訂單_buf.Count > 0)
            //    {
            //        list_藥品資料_每日訂單_buf[0][(int)enum_藥庫_每日訂單_下訂單.緊急訂購數量] = aPI_OrderClas_緊急訂購數量.Result[i].value;
            //        list_藥品資料_每日訂單_add.Add(list_藥品資料_每日訂單_buf[0]);
            //    }
            //}
            list_藥品資料_每日訂單_add = list_藥品資料_每日訂單_add.Distinct(new Distinct_藥庫_每日訂單_下訂單()).ToList();

            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.RefreshGrid(list_藥品資料_每日訂單_add);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_顯示低於安全量_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品資料_每日訂單 = Function_藥庫_每日訂單_下訂單_取得藥品資料();
            List<object[]> list_藥品資料_每日訂單_add = new List<object[]>();
            for (int i = 0; i < list_藥品資料_每日訂單.Count; i++)
            {
                int 安全量 = list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.安全庫存].ObjectToString().StringToInt32();
                int 基準量 = list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.基準量].ObjectToString().StringToInt32();
                int 總庫存 = list_藥品資料_每日訂單[i][(int)enum_藥庫_每日訂單_下訂單.總庫存].ObjectToString().StringToInt32();
                int 訂購量 = 0;
                if (總庫存 > 安全量) continue;
                if (安全量 <= 0) continue;
                if (基準量 <= 0) continue;
                if (總庫存 < 0) continue;
                if (基準量 <= 安全量) continue;
                訂購量 = 基準量 - 總庫存;
                if (訂購量 <= 0) continue;
                list_藥品資料_每日訂單_add.Add(list_藥品資料_每日訂單[i]);
            }
            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.RefreshGrid(list_藥品資料_每日訂單_add);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_清除所有線上訂單_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string serverfilepath = @"HTS81P.ptvgh.gov.tw\MIS\DG";
            string serverfilename = "itinvd0304.txt";
            string localfilepath = @"C:\Users\HS1T16\Desktop\";
            string localfilename = "itinvd0304.txt";
            string username = "hsonds01";
            string password = "KuT1Ch@75511";

            List<string> load_texts = new List<string>();
            bool flag = Basic.MyFileStream.SaveFile($"{localfilepath}{localfilename}", load_texts);
            Console.WriteLine($"存至本地 {(flag ? "sucess" : "fail")} ! ,耗時{myTimer.ToString()}");
            flag = Basic.FileIO.CopyToServer(serverfilepath, serverfilename, localfilepath, localfilename, username, password);
            Console.WriteLine($"上傳檔案至Fileserver {(flag ? "sucess" : "fail")} ! ,耗時{myTimer.ToString()}");
            MyMessageBox.ShowDialog("清除完成!");
            API_OrderClass_每日訂單_訂購數量.清除全部();
            this.Function_藥庫_每日訂單_下訂單_更新藥品資料表單();
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_送出線上訂單_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string serverfilepath = @"HTS81P.ptvgh.gov.tw\MIS\DG";
            string serverfilename = "itinvd0304.txt";
            string localfilepath = @"C:\Users\HS1T16\Desktop\";
            string localfilename = "itinvd0304.txt";
            string username = "hsonds01";
            string password = "KuT1Ch@75511";

            API_OrderClass aPI_OrderClass_今日訂購數量 = Function_藥庫_每日訂單_下訂單_取得今日訂購數量();
            API_OrderClass aPI_OrderClass_緊急訂購數量 = Function_藥庫_每日訂單_下訂單_取得緊急訂購數量();
            API_OrderClass aPI_OrderClass = new API_OrderClass();
            for (int i = 0; i < aPI_OrderClass_今日訂購數量.Result.Count; i++)
            {
                string 藥品碼 = aPI_OrderClass_今日訂購數量.Result[i].code;
                string 數量 = aPI_OrderClass_今日訂購數量.Result[i].value;
                aPI_OrderClass.新增數量(藥品碼, 數量.StringToInt32());
            }
            for (int i = 0; i < aPI_OrderClass_緊急訂購數量.Result.Count; i++)
            {
                string 藥品碼 = aPI_OrderClass_緊急訂購數量.Result[i].code;
                string 數量 = aPI_OrderClass_緊急訂購數量.Result[i].value;
                aPI_OrderClass.新增數量(藥品碼, 數量.StringToInt32());
            }
            List<string> list_texts = new List<string>();
            for (int i = 0; i < aPI_OrderClass.Result.Count; i++)
            {
                string 藥品碼 = aPI_OrderClass.Result[i].code;
                string 數量 = aPI_OrderClass.Result[i].value;
                string 訂購日期 = DateTime.Now.ToDateString();
                string text = this.Function_藥庫_每日訂單_下訂單_已訂購字串轉換(藥品碼, 數量, 訂購日期);
                if (數量.StringToInt32() <= 0) continue;
                list_texts.Add(text);
            }
            bool flag = Basic.MyFileStream.SaveFile($"{localfilepath}{localfilename}", list_texts);
            Console.WriteLine($"存至本地 {(flag ? "sucess" : "fail")} ! ,耗時{myTimer.ToString()}");
            flag = Basic.FileIO.CopyToServer(serverfilepath, serverfilename, localfilepath, localfilename, username, password);
            Console.WriteLine($"上傳檔案至Fileserver {(flag ? "sucess" : "fail")} ! ,耗時{myTimer.ToString()}");

            MyMessageBox.ShowDialog(flag? "訂單送出完成!" : "訂單送出失敗!");
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_清除選取藥品訂單_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否清除訂單?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            List<object[]> list_value = this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.Get_All_Checked_RowsValues();
            List<object[]> list_每日訂單 = this.sqL_DataGridView_每日訂單.SQL_GetAllRows(false);
            List<object[]> list_每日訂單_buf = new List<object[]>();
            List<object[]> list_value_delete = new List<object[]>();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }


            List<object[]> list_寫入報表設定 = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
            list_寫入報表設定 = list_寫入報表設定.GetRows((int)enum_寫入報表設定.檔名, "每日訂單送出");
            if (list_寫入報表設定.Count == 0) return;
            int hour = list_寫入報表設定[0][(int)enum_寫入報表設定.更新每日].ObjectToString().Substring(0, 2).StringToInt32();
            int min = list_寫入報表設定[0][(int)enum_寫入報表設定.更新每日].ObjectToString().Substring(2, 2).StringToInt32();

            DateTime dateTime_start;
            DateTime dateTime_end;

            DateTime dateTime_basic = DateTime.Now;
            while (true)
            {
                if (!Basic.TypeConvert.IsHolidays(dateTime_basic))
                {
                    break;
                }
                dateTime_basic.AddDays(-1);
            }

            if (dateTime_basic.IsNewDay(hour, min))
            {
                dateTime_start = $"{dateTime_basic.ToDateString()} {hour}:{min}:00".StringToDateTime();
                dateTime_end = dateTime_start.AddDays(1);
            }
            else
            {
                dateTime_end = $"{dateTime_basic.ToDateString()} {hour}:{min}:00".StringToDateTime();
                dateTime_start = dateTime_end.AddDays(-1);
            }

            for (int i = 0; i < list_value.Count; i++)
            {
                string code = list_value[i][(int)enum_每日訂單.藥品碼].ObjectToString();
                list_每日訂單_buf = list_每日訂單.GetRows((int)enum_每日訂單.藥品碼, code);
                list_每日訂單_buf = list_每日訂單_buf.GetRowsInDate((int)enum_每日訂單.訂購時間, dateTime_start, dateTime_end);
                if(list_每日訂單_buf.Count > 0)
                {
                    list_value_delete.Add(list_每日訂單_buf[0]);
                }
            }
            this.sqL_DataGridView_每日訂單.SQL_DeleteExtra(list_value_delete, false);
            this.Function_藥庫_每日訂單_下訂單_更新藥品資料表單();
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_選取藥品補足基準量_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否補足基準量?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            List<object[]> list_value = this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.Get_All_Checked_RowsValues();      
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            this.Function_藥庫_每日訂單_下訂單_藥品補足基準量(list_value);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_藥品名稱搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string text = rJ_TextBox_藥庫_每日訂單_下訂單_藥品名稱.Text;
            List<object[]> list_value = this.Function_藥庫_每日訂單_下訂單_取得藥品資料();
            list_value = list_value.GetRowsByLike((int)enum_藥庫_每日訂單_下訂單.藥品名稱, text);
            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_藥品碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string text = rJ_TextBox_藥庫_每日訂單_下訂單_藥品碼.Text;
            List<object[]> list_value = this.Function_藥庫_每日訂單_下訂單_取得藥品資料();
            list_value = list_value.GetRowsByLike((int)enum_藥庫_每日訂單_下訂單.藥品碼, text);
            this.sqL_DataGridView_藥庫_每日訂單_下訂單_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_下訂單_檢查訂單是否送出_MouseDownEvent(MouseEventArgs mevent)
        {
            string dateTime = Basic.Net.WEBApiGet("https://127.0.0.1:4433/api/test/OrderLastWriteTime");

            MyMessageBox.ShowDialog($"最後送出時間 {dateTime}");
        }
        #endregion
        private class ICP_藥庫_每日訂單_下訂單 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string Code0 = x[(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
                string Code1 = y[(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
                return Code0.CompareTo(Code1);
            }
        }
        public class Distinct_藥庫_每日訂單_下訂單 : IEqualityComparer<object[]>
        {
            public bool Equals(object[] x, object[] y)
            {
                return x[(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString() == y[(int)enum_藥庫_每日訂單_下訂單.藥品碼].ObjectToString();
            }

            public int GetHashCode(object[] obj)
            {
                return 1;
            }
        }
    }
}
