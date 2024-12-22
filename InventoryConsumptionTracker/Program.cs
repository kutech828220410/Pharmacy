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
using SQLUI;
using HIS_DB_Lib;
using System.IO;
using System.Reflection;
namespace InventoryConsumptionTracker
{
    class Program
    {
        public enum enum_批次過帳_批次過帳明細
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
        public enum enum_匯出
        {
            藥碼,
            藥名,
            消耗量,
            起始日期,
            結束日期,
            成本價,
            健保價,
            ATC,
            期初藥局庫存,
            期末藥局庫存,
            期初藥庫庫存,
            期末藥庫庫存,
        }
        public class class_MedPrice
        {
            public string 藥品碼 { get; set; }
            public string 售價 { get; set; }
            public string 成本價 { get; set; }
            public string 最近一次售價 { get; set; }
            public string 最近一次成本價 { get; set; }
            public string ATC { get; set; }

        }
        static public string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        static private SQLControl sQLControl_sd0_public = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_public", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_sd0_opd = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_opd", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_sd0_pher = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_pher", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_sd0_phr = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_phr", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_medicine_cloud = new SQLControl("127.0.0.1", "dbvm", "medicine_page_cloud", "user", "66437068", 3306, MySqlSslMode.None);

     

        static void Main(string[] args)
        {
            int month = 9;
            DateTime dateTime = new DateTime(DateTime.Now.Year, month, 1);
            DateTime dateTime_st = dateTime.GetStartDate();
            DateTime dateTime_end = dateTime.AddMonths(3).AddDays(-1).GetEndDate();

            string e_msg = "\n";
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            try
            {
           


                List<Task> tasks = new List<Task>();
                List<object[]> list_medicine_cloud = new List<object[]>();
                List<object[]> list_sd0_public = new List<object[]>();
                List<object[]> list_sd0_opd = new List<object[]>();
                List<object[]> list_sd0_pher = new List<object[]>();
                List<object[]> list_sd0_phr = new List<object[]>();
                List<object[]> list_消耗帳 = new List<object[]>();
                tasks.Add(Task.Run(new Action(delegate
                {
                    List<medClass> medClasses_cloud = medClass.get_med_cloud("http://127.0.0.1:4433");
                    medClasses_cloud = (from temp in medClasses_cloud
                                        where temp.開檔狀態 == "開檔中"
                                        select temp).ToList();
                    list_medicine_cloud = medClasses_cloud.ClassToSQL<medClass , enum_雲端藥檔>();
                })));
                tasks.Add(Task.Run(new Action(delegate
                {
                    list_sd0_public = sQLControl_sd0_public.GetRowsByBetween(null, (int)enum_批次過帳_批次過帳明細.報表日期, dateTime_st.ToDateTimeString(), dateTime_end.ToDateTimeString());
                })));
                tasks.Add(Task.Run(new Action(delegate
                {
                    list_sd0_opd = sQLControl_sd0_opd.GetRowsByBetween(null, (int)enum_批次過帳_批次過帳明細.報表日期, dateTime_st.ToDateTimeString(), dateTime_end.ToDateTimeString());
                })));
                tasks.Add(Task.Run(new Action(delegate
                {
                    list_sd0_pher = sQLControl_sd0_pher.GetRowsByBetween(null, (int)enum_批次過帳_批次過帳明細.報表日期, dateTime_st.ToDateTimeString(), dateTime_end.ToDateTimeString());
                })));
                tasks.Add(Task.Run(new Action(delegate
                {
                    list_sd0_phr = sQLControl_sd0_phr.GetRowsByBetween(null, (int)enum_批次過帳_批次過帳明細.報表日期, dateTime_st.ToDateTimeString(), dateTime_end.ToDateTimeString());
                })));
                Task.WhenAll(tasks).Wait();
                string MedPrice = Basic.Net.WEBApiGet($"https://10.18.1.146:4434/api/MedPrice");
                List<class_MedPrice> class_MedPrices = MedPrice.JsonDeserializet<List<class_MedPrice>>();
                List<class_MedPrice> class_MedPrices_buf = new List<class_MedPrice>();


                list_消耗帳.LockAdd(list_sd0_public);
                list_消耗帳.LockAdd(list_sd0_opd);
                list_消耗帳.LockAdd(list_sd0_pher);
                list_消耗帳.LockAdd(list_sd0_phr);
                Console.WriteLine($"取得消耗帳資料及藥檔完成,消耗帳<{list_消耗帳.Count}>筆,{myTimerBasic}");
                Console.WriteLine($"-------------------------------------------------------------------------");
                List<object[]> list_匯出資料 = new List<object[]>();
                Dictionary<string, List<object[]>> keyValuePairs_消耗帳 = ConvertToDictionary(list_消耗帳, (int)enum_批次過帳_批次過帳明細.藥品碼);
                for(int i = 0; i < list_medicine_cloud.Count; i++)
                {
                    string 藥品碼 = list_medicine_cloud[i][(int)enum_雲端藥檔.藥品碼].ObjectToString();
                    string 藥名 = list_medicine_cloud[i][(int)enum_雲端藥檔.藥品名稱].ObjectToString();
                    int 消耗量 = 0;
                    string 起始日期 = "";
                    string 結束日期 = "";
                    string 健保價 = "0";
                    string 售價 = "0";
                    string 成本價 = "0";
                    string ATC = "";
                    object[] value = new object[new enum_匯出().GetLength()];
                    if (keyValuePairs_消耗帳.ContainsKey(藥品碼))
                    {
                        List<object[]> list_消耗帳_buf = keyValuePairs_消耗帳[藥品碼];
                        if (list_消耗帳_buf.Count == 0) continue;
                        list_消耗帳_buf.Sort(new ICP_消耗帳());
                        for (int k = 0; k < list_消耗帳_buf.Count; k++)
                        {
                            消耗量 += list_消耗帳_buf[k][(int)enum_批次過帳_批次過帳明細.異動量].StringToInt32();
                        }
                        消耗量 *= -1;
                        起始日期 = list_消耗帳_buf[0][(int)enum_批次過帳_批次過帳明細.報表日期].ToDateString();
                        結束日期 = list_消耗帳_buf[list_消耗帳_buf.Count - 1][(int)enum_批次過帳_批次過帳明細.報表日期].ToDateString();

                        class_MedPrices_buf = (from temp in class_MedPrices
                                               where temp.藥品碼 == 藥品碼
                                               select temp).ToList();
                        if(class_MedPrices_buf.Count > 0)
                        {
                            售價 = class_MedPrices_buf[0].售價;
                            成本價 = class_MedPrices_buf[0].成本價;
                            ATC = class_MedPrices_buf[0].ATC;
                        }

                        value[(int)enum_匯出.藥碼] = 藥品碼;
                        value[(int)enum_匯出.藥名] = 藥名;
                        value[(int)enum_匯出.消耗量] = 消耗量;
                        value[(int)enum_匯出.起始日期] = 起始日期;
                        value[(int)enum_匯出.結束日期] = 結束日期;
                        value[(int)enum_匯出.健保價] = 健保價;
                        value[(int)enum_匯出.成本價] = 成本價;
                        value[(int)enum_匯出.ATC] = ATC;

                        list_匯出資料.Add(value);
                        Console.WriteLine($"藥碼:{藥品碼} ,藥名:{藥名} ,消耗量:{消耗量} ,日期:{起始日期} ~ {結束日期}");
                    }
                }
                Table table = new Table("trading");
                table.Server = "127.0.0.1";
                table.Port = "3306";
                table.Username = "user";
                table.Password = "66437068";
                table.DBName = "ds01";
                tasks.Clear();
                SQLControl sQLControl_trading = new SQLControl(table);
                int index = 0;
                for (int i = 0; i < list_匯出資料.Count; i++)
                {
                    object[] value = list_匯出資料[i];
                    string 藥碼 = list_匯出資料[i][(int)enum_匯出.藥碼].ObjectToString() ;
                    tasks.Add(Task.Run(new Action(delegate
                    {
                        string[] col_names = new string[] {"藥品碼","操作時間" };
                        List<object[]> list_trading = sQLControl_trading.GetRowsByDefult(null, (int)enum_交易記錄查詢資料.藥品碼, 藥碼);
                        list_trading = list_trading.GetRowsInDateEx((int)enum_交易記錄查詢資料.操作時間, dateTime_st, dateTime_end);

                        int 期初藥局庫存 = 0;
                        int 期初藥庫庫存 = 0;
                        int 期末藥局庫存 = 0;
                        int 期末藥庫庫存 = 0;

                        List<object[]> list_trading_藥庫 = new List<object[]>();
                        List<object[]> list_trading_藥局 = new List<object[]>();
            
                        string msg = "";

                        msg = "";
                        if (list_trading.Count > 0)
                        {
                            list_trading.Sort(new ICP_交易記錄查詢());
                            list_trading_藥庫 = list_trading.GetRows((int)enum_交易記錄查詢資料.庫別, "藥庫");
                            list_trading_藥局 = (from temp in list_trading
                                               where temp[(int)enum_交易記錄查詢資料.庫別].ObjectToString().Contains("藥局")
                                               select temp).ToList();
                            list_trading_藥庫.Sort(new ICP_交易記錄查詢());
                            list_trading_藥局.Sort(new ICP_交易記錄查詢());



                      

                            if (list_trading_藥庫.Count > 0)
                            {
                                期初藥庫庫存 = list_trading_藥庫[0][(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                                期末藥庫庫存 = list_trading_藥庫[list_trading_藥庫.Count -1][(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                            }
                            else
                            {
                                msg += " [找無藥庫交易紀錄]";
                            }

                            if (list_trading_藥局.Count > 0)
                            {
                                期初藥局庫存 = list_trading_藥局[0][(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                                期末藥局庫存 = list_trading_藥局[list_trading_藥局.Count - 1][(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                            }
                            else
                            {
                                msg += " [找無藥局交易紀錄]";
                            }
                            Console.Write($"({index})...");
                            index++;
                        }
                        value[(int)enum_匯出.期初藥局庫存] = 期初藥局庫存;
                        value[(int)enum_匯出.期末藥局庫存] = 期末藥局庫存;
                        value[(int)enum_匯出.期初藥庫庫存] = 期初藥庫庫存;
                        value[(int)enum_匯出.期末藥庫庫存] = 期末藥庫庫存;
                    })));

               


                }
                Task.WhenAll(tasks).Wait();
                list_匯出資料.Sort(new ICP_匯出_藥品碼排序());

                Console.WriteLine($"整理消耗帳完成,{myTimerBasic}");
                Console.WriteLine($"-------------------------------------------------------------------------");

                System.Data.DataTable dataTable = list_匯出資料.ToDataTable(new enum_匯出());
                MyOffice.ExcelClass.NPOI_SaveFile(dataTable, $"{currentDirectory}\\期初期末彙總_{dateTime_st.ToDateTinyString()}_{dateTime_end.ToDateTinyString()}.xls",new int[]{ (int)enum_匯出.消耗量 , (int)enum_匯出.健保價, (int)enum_匯出.成本價,
                    (int)enum_匯出.期初藥局庫存, (int)enum_匯出.期初藥庫庫存,   (int)enum_匯出.期末藥局庫存, (int)enum_匯出.期末藥庫庫存 });
                Console.WriteLine($"存檔完成,{myTimerBasic}");
                Console.WriteLine($"-------------------------------------------------------------------------");
                System.Threading.Thread.Sleep(2000);

            }
            catch (Exception e)
            {
                e_msg += $"Exception {e.Message}";
            }
            finally
            {
                Logger.LogAddLine("InventoryConsumptionTracker");
                Logger.Log("InventoryConsumptionTracker", $"{e_msg}");
                Logger.LogAddLine("InventoryConsumptionTracker");
            }
        }

        static public Dictionary<string, List<object[]>> ConvertToDictionary(List<object[]> list_value, int index)
        {
            Dictionary<string, List<object[]>> dictionary = new Dictionary<string, List<object[]>>();

            foreach (var item in list_value)
            {
                string _key = item[index].ObjectToString();

                // 如果字典中已經存在該索引鍵，則將值添加到對應的列表中
                if (dictionary.ContainsKey(_key))
                {
                    dictionary[_key].Add(item);
                }
                // 否則創建一個新的列表並添加值
                else
                {
                    List<object[]> values = new List<object[]> { item };
                    dictionary[_key] = values;
                }
            }

            return dictionary;
        }
        public class ICP_消耗帳 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime1 = x[(int)enum_批次過帳_批次過帳明細.報表日期].ToDateTimeString_6().StringToDateTime();
                DateTime datetime2 = y[(int)enum_批次過帳_批次過帳明細.報表日期].ToDateTimeString_6().StringToDateTime();
                int compare = DateTime.Compare(datetime1, datetime2);
                return compare;

            }
        }
        private class ICP_匯出_藥品碼排序 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string Code0 = x[(int)enum_匯出.藥碼].ObjectToString();
                string Code1 = y[(int)enum_匯出.藥碼].ObjectToString();
                return Code0.CompareTo(Code1);
            }
        }


        public class ICP_交易記錄查詢 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime1 = x[(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString_6().StringToDateTime();
                DateTime datetime2 = y[(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString_6().StringToDateTime();
                int compare = DateTime.Compare(datetime1, datetime2);
                if (compare != 0) return compare;
                int 結存量1 = x[(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                int 結存量2 = y[(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                string 動作1 = x[(int)enum_交易記錄查詢資料.動作].ObjectToString();
                string 動作2 = x[(int)enum_交易記錄查詢資料.動作].ObjectToString();

                string 庫別1 = x[(int)enum_交易記錄查詢資料.庫別].ObjectToString();
                string 庫別2 = x[(int)enum_交易記錄查詢資料.庫別].ObjectToString();

                if (動作1 == "自動撥補" && 動作2 == "自動撥補")
                {
                    if (庫別1.Contains("藥局") && 庫別2.Contains("藥局"))
                    {
                        if (結存量1 > 結存量2)
                        {
                            return -1;
                        }
                        else if (結存量1 < 結存量2)
                        {
                            return 1;
                        }
                        else if (結存量1 == 結存量2) return 0;
                    }
                }

                if (結存量1 > 結存量2)
                {
                    return 1;
                }
                else if (結存量1 < 結存量2)
                {
                    return -1;
                }
                else if (結存量1 == 結存量2) return 0;

                return 0;

            }
        }
    }
}
