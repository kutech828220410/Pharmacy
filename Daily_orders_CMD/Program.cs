using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MyUI;
using Basic;
using H_Pannel_lib;
using SQLUI;

namespace Daily_orders_CMD
{
    public enum enum_藥庫_每日訂單 : int
    {
        GUID,
        藥品碼,
        中文名稱,
        藥品名稱,
        包裝單位,
        藥庫庫存,
        總庫存,
        安全庫存,
        基準量,
        今日訂購數量,
        緊急訂購數量,
        在途量,

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
    public enum enum_讀取每日訂單
    {
        藥品碼,
        請購數量,
    }
    public enum enum_過帳狀態列表
    {
        GUID,
        編號,
        檔名,
        檔案位置,
        類別,
        報表日期,
        產生排程時間,
        排程作業時間,
        備註內容,
        狀態,

    }
    public enum enum_過帳狀態
    {
        已產生排程,
        排程已作業,
    }
    public enum enum_寫入報表設定_類別
    {
        OPD_消耗帳,
        PHR_消耗帳,
        PHER_消耗帳,
        公藥_消耗帳,
        其他,
    }
  
    public enum enum_寫入報表設定
    {
        GUID,
        編號,
        檔名,
        檔案位置,
        類別,
        更新每日,
        更新每週,
        更新每月,
        備註內容,
        out_db_server,
        out_db_name,
        out_db_username,
        out_db_password,
        out_db_port,
        fileServer_username,
        fileServer_password,
    }
    class Program
    {
        private API_OrderClass API_OrderClass_每日訂單_訂購數量 = new API_OrderClass();
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

                public resultClass(string code, string value, string orderDateTime)
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
            public void 新增藥品(string code, int num)
            {

                List<resultClass> Result_buf = new List<resultClass>();
                Result_buf = (from value in Result
                              where value.code == code
                              select value).ToList();
                if (Result_buf.Count > 0)
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
        //
        static private SQLControl sQLControl_每日訂單 = new SQLControl("10.18.1.146", "ds01_vm", "order_perday", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_過帳狀態 = new SQLControl("10.18.1.146", "ds01_vm", "posting_state", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_寫入報表設定 = new SQLControl("10.18.1.146", "ds01_vm", "posting_table_setting", "user", "66437068", 3306, MySqlSslMode.None);


        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    if (送出線上訂單())
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
          
    
          //  Console.ReadLine(); // 等待用户按下 Enter 键
        }

        static bool 送出線上訂單()
        {
            if (IsHspitalHolidays(DateTime.Now)) return true;
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string serverfilepath = @"C:\MIS\DG\";
            string serverfilename = "itinvd0304.txt";
            string localfilepath = @"C:\Users\hsonds01\Desktop\";
            string localfilename = "itinvd0304.txt";
            string username = "hsonds01";
            string password = "KuT1Ch@75511";

            API_OrderClass aPI_OrderClass_今日訂購數量 = Function_藥庫_每日訂單_取得今日訂購數量();

            API_OrderClass aPI_OrderClass = new API_OrderClass();
            for (int i = 0; i < aPI_OrderClass_今日訂購數量.Result.Count; i++)
            {
                string 藥品碼 = aPI_OrderClass_今日訂購數量.Result[i].code;
                string 數量 = aPI_OrderClass_今日訂購數量.Result[i].value;
                aPI_OrderClass.新增數量(藥品碼, 數量.StringToInt32());
            }
     
            List<string> list_texts = new List<string>();
            for (int i = 0; i < aPI_OrderClass.Result.Count; i++)
            {
                string 藥品碼 = aPI_OrderClass.Result[i].code;
                string 數量 = aPI_OrderClass.Result[i].value;
                string 訂購日期 = DateTime.Now.ToDateString();
             
                string text = Function_藥庫_每日訂單_已訂購字串轉換(藥品碼, 數量, 訂購日期);
                if (藥品碼.Length != 5) continue;
                if (數量.StringToInt32() <= 0) continue;
                list_texts.Add(text);
            }
            bool flag = Basic.MyFileStream.SaveFile($"{localfilepath}{localfilename}", list_texts);
            //flag = Basic.MyFileStream.SaveFile($"C:\\{localfilename}", list_texts);
            Console.WriteLine($"存至本地 {(flag ? "sucess" : "fail")} ! ,耗時{myTimer.ToString()}");
            flag = Basic.MyFileStream.SaveFile($"{serverfilepath}{serverfilename}", list_texts);
            return flag;
        }
        static public API_OrderClass Function_藥庫_每日訂單_取得今日訂購數量()
        {
            API_OrderClass aPI_OrderClass = new API_OrderClass();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            List<object[]> list_value = Function_藥庫_每日訂單_取得每日訂單資料();



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
        static public string Function_藥庫_每日訂單_藥品碼轉換(string value)
        {
            if (value.Length == 10)
            {
                value = value.Substring(5, 5);
            }
            return value;
        }
        static public void Function_藥庫_每日訂單_已訂購字串轉換(string text, ref string 藥品碼, ref string 數量, ref string 訂購日期)
        {
            藥品碼 = text.Substring(0, 15).Trim();
            數量 = text.Substring(15, 6).Trim();
            訂購日期 = text.Substring(21, 8).Trim();

            藥品碼 = Function_藥庫_每日訂單_藥品碼轉換(藥品碼);
            訂購日期 = $"{訂購日期.Substring(0, 4)}/{訂購日期.Substring(4, 2)}/{訂購日期.Substring(6, 2)}";
            訂購日期 = 訂購日期.StringToDateTime().ToDateString();
        }
        static public string Function_藥庫_每日訂單_已訂購字串轉換(string 藥品碼, string 數量, string 訂購日期)
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
        static public List<object[]> Function_藥庫_每日訂單_取得每日訂單資料()
        {
            List<object[]> list_value = new List<object[]>();
            List<object[]> list_寫入報表設定 = sQLControl_寫入報表設定.GetAllRows(null);
            list_寫入報表設定 = list_寫入報表設定.GetRows((int)enum_寫入報表設定.檔名, "每日訂單送出");
            if (list_寫入報表設定.Count == 0) return list_value;
            int hour = list_寫入報表設定[0][(int)enum_寫入報表設定.更新每日].ObjectToString().Substring(0, 2).StringToInt32();
            int min = list_寫入報表設定[0][(int)enum_寫入報表設定.更新每日].ObjectToString().Substring(2, 2).StringToInt32();

            DateTime dateNow =  new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);


            //dateNow = new DateTime(DateTime.Now.Year,10,1, 11, 50, 00);
            DateTime dateTime_temp = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, hour, min, 00);
            dateTime_temp = dateTime_temp.AddMinutes(5);


            DateTime dateTime_start;
            DateTime dateTime_end;

            DateTime dateTime_basic_start = dateNow;
            DateTime dateTime_basic_end = dateNow.AddDays(1);
            bool isholiday = false;
            if (!dateTime_basic_start.IsNewDay(dateTime_temp.Hour, dateTime_temp.Minute))
            {
                if (!IsHspitalHolidays(dateTime_basic_start))
                {
                    if (IsHspitalHolidays(dateTime_basic_start.AddDays(-1)))
                    {
                        dateTime_basic_start = dateTime_basic_start.AddDays(-1);
                    }
                }

            }
            while (true)
            {
                if (!IsHspitalHolidays(dateTime_basic_start))
                {
                    break;
                }
                dateTime_basic_start = dateTime_basic_start.AddDays(-1);
                isholiday = true;
            }

            if (dateTime_basic_start.IsNewDay(dateTime_temp.Hour, dateTime_temp.Minute) || isholiday)
            {
                dateTime_start = $"{dateTime_basic_start.ToDateString()} {hour}:{min}:00".StringToDateTime();
                dateTime_end = $"{dateTime_basic_start.AddDays(1).ToDateString()} {hour}:{min}:00".StringToDateTime();
            }
            else
            {
                dateTime_end = $"{dateTime_basic_start.ToDateString()} {hour}:{min}:00".StringToDateTime();
                dateTime_start = dateTime_end.AddDays(-1);
            }
            while (true)
            {
                if (!IsHspitalHolidays(dateTime_end))
                {
                    break;
                }
                dateTime_end = dateTime_end.AddDays(1);
            }
            list_value = sQLControl_每日訂單.GetAllRows(null);
            list_value = list_value.GetRowsInDate((int)enum_每日訂單.訂購時間, dateTime_start, dateTime_end);
            return list_value;
        }
        public static bool IsHspitalHolidays(DateTime date)
        {
            if (date.ToString("MM/dd").Equals("09/23"))
            {
                return false;
            }
            // 週休二日
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            // 國定假日(國曆)
            if (date.ToString("MM/dd").Equals("01/01"))
            {
                return true;
            }
            if (date.ToString("MM/dd").Equals("02/28"))
            {
                return true;
            }
            if (date.ToString("MM/dd").Equals("04/04"))
            {
                return true;
            }
            if (date.ToString("MM/dd").Equals("04/05"))
            {
                return true;
            }

            if (date.ToString("MM/dd").Equals("10/10"))
            {
                return true;
            }

            // 國定假日(農曆)
            System.Globalization.TaiwanLunisolarCalendar TaiwanLunisolarCalendar = new System.Globalization.TaiwanLunisolarCalendar();
            string LeapDate = string.Format("{0}/{1}", TaiwanLunisolarCalendar.GetMonth(date), TaiwanLunisolarCalendar.GetDayOfMonth(date));
            if (LeapDate == "12/30")
            {
                return true;
            }
            if (LeapDate == ("1/1"))
            {
                return true;
            }
            if (LeapDate == ("1/2"))
            {
                return true;
            }
            if (LeapDate == ("1/3"))
            {
                return true;
            }
            if (LeapDate == ("1/4"))
            {
                return true;
            }
            if (LeapDate == ("1/5"))
            {
                return true;
            }
            if (LeapDate == ("5/5"))
            {
                return true;
            }
            if (LeapDate == ("8/15"))
            {
                return true;
            }

            return false;
        }
    }
}
