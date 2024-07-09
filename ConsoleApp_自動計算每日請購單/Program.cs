using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using H_Pannel_lib;
using HIS_DB_Lib;
using SQLUI;
using MySql.Data;

namespace ConsoleApp_自動計算每日請購單
{
    class Program
    {
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

        public enum enum_每日訂單
        {
            GUID,
            藥品碼,
            今日訂購數量,
            緊急訂購數量,
            訂購時間,
            狀態,
        }

        public enum enum_藥品請購表
        {
            藥碼,
            藥名,
            安全量,
            基準量,
            在途量,
            庫存,
            包裝量,
            報表請購量,
            實際請購量,

        }
        public static string currentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        static public string API_Server = "http://127.0.0.1:4433";
        static public DateTime dateTime = DateTime.Now.AddDays(0);
        static void Main(string[] args)
        {
            Logger.LogAddLine();
            try
            {
                if(IsHspitalHolidays(dateTime))
                {
                    Logger.Log($"今日為假日無須計算請購單");

                    return;
                }
                string filename = "";
                System.Data.DataTable dataTable;
                DateTime dateTime_start = new DateTime();
                DateTime dateTime_end = new DateTime();
                Get_st_end_time(dateTime ,ref dateTime_start, ref dateTime_end);

                Logger.Log($"訂單時間 : {dateTime_start.ToDateTimeString()} ~ {dateTime_end.ToDateTimeString()}");
                Table table_每日訂單 = new Table("order_perday");
                table_每日訂單.Server = "127.0.0.1";
                table_每日訂單.DBName = "ds01_vm";
                table_每日訂單.Username = "user";
                table_每日訂單.Password = "66437068";
                table_每日訂單.Port = "3306";
                SQLControl sQLControl_每日訂單 = new SQLControl(table_每日訂單);


                List<object[]> list_每日訂單 = sQLControl_每日訂單.GetRowsByBetween(null, (int)enum_每日訂單.訂購時間, dateTime_start.ToDateTimeString(), dateTime_end.ToDateTimeString());
                List<object[]> list_每日訂單_buf = new List<object[]>();
                List<object[]> list_每日訂單_add = new List<object[]>();
                List<medClass> medClasses_drugstore_med = medClass.get_ds_drugstore_med(API_Server, "ds01");
                List<medClass> medClasses_drugstore_med_buf = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_drugstore_med = medClasses_drugstore_med.CoverToDictionaryByCode();
                API_OrderClass aPI_OrderClass = Function_取得在途量();
                Logger.Log($"取得藥庫藥品資料共<{medClasses_drugstore_med.Count}>筆");
                Logger.Log($"取得在途藥品共<{aPI_OrderClass.Result.Count}>筆");

                List<object[]> list_藥品請購表 = new List<object[]>();

                for (int i = 0; i < medClasses_drugstore_med.Count; i++)
                {
                    string 藥碼 = medClasses_drugstore_med[i].藥品碼;
                    string 藥名 = medClasses_drugstore_med[i].藥品名稱;
                    int 安全量 = medClasses_drugstore_med[i].安全庫存.StringToInt32();
                    int 基準量 = medClasses_drugstore_med[i].基準量.StringToInt32();
                    int 總庫存 = medClasses_drugstore_med[i].總庫存.StringToInt32();
                    int 包裝量 = medClasses_drugstore_med[i].包裝數量.StringToInt32();
                    int 在途量 = aPI_OrderClass.取得數量(藥碼);
                    int 請購量 = 0;
                    if (安全量 <= 0) continue;
                    if (基準量 <= 0) continue;
                    if (包裝量 <= 0) continue;
                    if ((總庫存 + 在途量) <= 安全量)
                    {
                        object[] value = new object[new enum_藥品請購表().GetLength()];
                        value[(int)enum_藥品請購表.藥碼] = 藥碼;
                        value[(int)enum_藥品請購表.藥名] = 藥名;
                        value[(int)enum_藥品請購表.安全量] = 安全量;
                        value[(int)enum_藥品請購表.基準量] = 基準量;
                        value[(int)enum_藥品請購表.在途量] = 在途量;
                        value[(int)enum_藥品請購表.庫存] = 總庫存;
                        value[(int)enum_藥品請購表.包裝量] = 包裝量;

                        請購量 = 基準量 - (總庫存 + 在途量);
                        if (請購量 % 包裝量 > 0)
                        {
                            請購量 = (請購量 - (請購量 % 包裝量)) + 包裝量;
                        }

                        value[(int)enum_藥品請購表.報表請購量] = 請購量;
                        value[(int)enum_藥品請購表.實際請購量] = 請購量;

                        list_每日訂單_buf = (from temp in list_每日訂單
                                         where temp[(int)enum_每日訂單.藥品碼].ObjectToString() == 藥碼
                                         select temp).ToList();

                        if (list_每日訂單_buf.Count > 0)
                        {
                            value[(int)enum_藥品請購表.實際請購量] = list_每日訂單_buf[0][(int)enum_每日訂單.今日訂購數量].ObjectToString();
                        }
                        list_藥品請購表.Add(value);

                        Logger.Log($"({list_藥品請購表.Count})".StringLength(8) + $"({藥碼}){藥名}".StringLength(50) + $"庫存:{總庫存}".StringLength(14) + $"在途量:{在途量}".StringLength(14) + $"安全量:{安全量}".StringLength(14) + $"基準量:{基準量}".StringLength(14) + $"請購量:{請購量}".StringLength(14)); 
                    }
                    else
                    {

                    }

                }
                dataTable = list_藥品請購表.ToDataTable(new enum_藥品請購表());
                filename = $@"{currentDirectory}\excel\藥品請購表({dateTime_start.ToDateString("")}_{dateTime_end.ToDateString("")}).xlsx";
                MyOffice.ExcelClass.NPOI_SaveFile(dataTable, filename, new int[] { (int)enum_藥品請購表.基準量, (int)enum_藥品請購表.安全量, (int)enum_藥品請購表.在途量, (int)enum_藥品請購表.庫存, (int)enum_藥品請購表.包裝量, (int)enum_藥品請購表.報表請購量, (int)enum_藥品請購表.實際請購量 });
                Logger.Log($"請購藥品共<{list_藥品請購表.Count}>筆");

                for (int i = 0; i < list_藥品請購表.Count; i++)
                {
                    string Code = list_藥品請購表[i][(int)enum_藥品請購表.藥碼].ObjectToString();
                    string 藥名 = list_藥品請購表[i][(int)enum_藥品請購表.藥名].ObjectToString();
                    string 實際請購量 = list_藥品請購表[i][(int)enum_藥品請購表.實際請購量].ObjectToString();

                    list_每日訂單_buf = (from temp in list_每日訂單
                                     where temp[(int)enum_每日訂單.藥品碼].ObjectToString() == Code
                                     select temp).ToList();
                    if(list_每日訂單_buf.Count == 0)
                    {
                        object[] value = new object[new enum_每日訂單().GetLength()];

                        value[(int)enum_每日訂單.GUID] = Guid.NewGuid().ToString();
                        value[(int)enum_每日訂單.藥品碼] = Code;
                        value[(int)enum_每日訂單.今日訂購數量] = 實際請購量;
                        value[(int)enum_每日訂單.緊急訂購數量] = "0";
                        value[(int)enum_每日訂單.訂購時間] = DateTime.Now.ToDateTimeString_6();
                        list_每日訂單_add.Add(value);

                    }
                    else
                    {
                        Logger.Log($"({i})".StringLength(5) + $"({Code}){藥名}".StringLength(50) +$"此藥已請購,數量 : {list_每日訂單_buf[0][(int)enum_每日訂單.今日訂購數量].ObjectToString()}");

                    }
                }
                sQLControl_每日訂單.AddRows(null, list_每日訂單_add);
                Logger.Log($"新增每日訂單共<{list_每日訂單_add.Count}>筆");

            }
            catch (Exception ex)
            {
                Logger.Log($"Exception : {ex.Message}");
            }
            finally
            {
                Logger.LogAddLine();
            }
        
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
        static public void Get_st_end_time(DateTime dateTime ,ref DateTime dateTime_start, ref DateTime dateTime_end)
        {
            int hour = 11;
            int min = 50;


            DateTime dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            //dateNow = new DateTime(DateTime.Now.Year,10,1, 11, 50, 00);
            DateTime dateTime_temp = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, hour, min, 00);
            dateTime_temp = dateTime_temp.AddMinutes(5);



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
        }

        static public API_OrderClass Function_取得在途量()
        {
            API_OrderClass aPI_OrderClass = new API_OrderClass();
            API_OrderClass aPI_OrderClass_out = new API_OrderClass();

            string result = Basic.Net.WEBApiPostJson("https://wac01p.vghks.gov.tw:4430/ITWeb/jaxrs/ItCommon/pinmed_itm", "{\"hid\"   : [\"2A0\"]}");
            if (result.StringIsEmpty())
            {
                Logger.Log($"在途量取得失敗 url : https://wac01p.vghks.gov.tw:4430/ITWeb/jaxrs/ItCommon/pinmed_itm , post : " + "{\"hid\"   : [\"2A0\"]}");
                return aPI_OrderClass;
            }
            aPI_OrderClass = result.JsonDeserializet<API_OrderClass>();
            for (int i = 0; i < aPI_OrderClass.Result.Count; i++)
            {
                aPI_OrderClass.Result[i].code = Function_藥品碼轉換(aPI_OrderClass.Result[i].code);
            }
            for (int i = 0; i < aPI_OrderClass.Result.Count; i++)
            {
                aPI_OrderClass_out.新增數量(aPI_OrderClass.Result[i].code, aPI_OrderClass.Result[i].value.StringToInt32());
            }

            return aPI_OrderClass_out;
        }
        static public string Function_藥品碼轉換(string value)
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
    }
}
