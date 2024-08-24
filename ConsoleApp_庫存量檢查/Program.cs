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

namespace ConsoleApp_庫存量檢查
{
    class Program
    {
        private static string API_Server = $"http://127.0.0.1:4433";
        static void Main(string[] args)
        {
            List<medClass> medClasses_cloud = medClass.get_med_cloud(API_Server);
            medClasses_cloud = (from temp in medClasses_cloud
                                where temp.開檔狀態 == "開檔中"
                                select temp).ToList();

            List<DeviceBasic> deviceBasics_藥庫 = deviceApiClass.GetDeviceBasics(API_Server, "ds01", "藥庫", deviceApiClass.StoreType.藥庫);
            List<DeviceBasic> deviceBasics_藥局 = deviceApiClass.GetDeviceBasics(API_Server, "ds01", "藥庫", deviceApiClass.StoreType.藥局);

    

            Dictionary<string, List<DeviceBasic>> keyValuePairs_deviceBasics_藥庫 = deviceBasics_藥庫.CoverToDictionaryByCode();
            Dictionary<string, List<DeviceBasic>> keyValuePairs_deviceBasics_藥局 = deviceBasics_藥局.CoverToDictionaryByCode();

            Logger.Log($"取得開檔中藥檔共<{medClasses_cloud.Count}>筆");
            Logger.Log($"取得藥庫庫存資料共<{deviceBasics_藥庫.Count}>筆");
            Logger.Log($"取得藥局庫存資料共<{deviceBasics_藥局.Count}>筆");

            Table table = new Table("trading");
            table.Server = "127.0.0.1";
            table.Port = "3306";
            table.Username = "user";
            table.Password = "66437068";
            table.DBName = "ds01";
            List<Task> tasks = new List<Task>();
            SQLControl sQLControl_trading = new SQLControl(table);
            string str_log = "";
            int index = 0;
            for (int i = 0; i < medClasses_cloud.Count; i++)
            {
                string 藥碼 = medClasses_cloud[i].藥品碼;
                tasks.Add(Task.Run(new Action(delegate
                {
                    List<object[]> list_trading = sQLControl_trading.GetRowsByDefult(null, (int)enum_交易記錄查詢資料.藥品碼, 藥碼);
                    List<object[]> list_trading_藥庫 = new List<object[]>();
                    List<object[]> list_trading_藥局 = new List<object[]>();
                    List<DeviceBasic> deviceBasics_藥庫_buf = new List<DeviceBasic>();
                    List<DeviceBasic> deviceBasics_藥局_buf = new List<DeviceBasic>();
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

                        DateTime dateTime = list_trading[0][(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString().StringToDateTime();

                        DateTime fiveMonthsAgo = DateTime.Now.AddMonths(-5);

                        if (dateTime >= fiveMonthsAgo && dateTime <= DateTime.Now)
                        {

                        }
                        else
                        {
                            // dateTime 不在五個月內
                            //msg += $"藥碼 : {藥碼} ,日期不在五個月內";


                            index++;
                            return;
                        }

                        int 藥庫結存 = 0;
                        int 藥局結存 = 0;
                        int 藥庫庫存 = 0;
                        int 藥局庫存 = 0;

                        if (list_trading_藥庫.Count > 0)
                        {
                            藥庫結存 = list_trading_藥庫[0][(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                        }
                        else
                        {
                            msg += " [找無藥庫交易紀錄]";
                        }

                        if (list_trading_藥局.Count > 0)
                        {
                            藥局結存 = list_trading_藥局[0][(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                        }
                        else
                        {
                            msg += " [找無藥局交易紀錄]";
                        }

                        deviceBasics_藥庫_buf = keyValuePairs_deviceBasics_藥庫.SortDictionaryByCode(藥碼);
                        deviceBasics_藥局_buf = keyValuePairs_deviceBasics_藥局.SortDictionaryByCode(藥碼);

                        if (deviceBasics_藥庫_buf.Count > 0)
                        {
                            藥庫庫存 = deviceBasics_藥庫_buf.GetInventory();
                        }
                        else
                        {
                            msg += " [找無藥庫庫存資料]";
                        }

                        if (deviceBasics_藥局_buf.Count > 0)
                        {
                            藥局庫存 = deviceBasics_藥局_buf.GetInventory();
                        }
                        else
                        {
                            msg += " [找無藥局庫存資料]";
                        }
                        bool flag_OK = true;
                        if (藥庫庫存 != 藥庫結存 && 藥庫結存 > 0)
                        {
                            msg += $"【藥庫庫存異常】";
                            flag_OK = false;
                        }
                        if (藥局庫存 != 藥局結存 && 藥局結存 > 0)
                        {
                            msg += $"【藥局庫存異常】";
                            flag_OK = false;
                        }
                        if (flag_OK) msg += $"【庫存正常】";
                        msg += $"藥碼 : {藥碼} ,藥庫庫存:{藥庫庫存},藥庫結存:{藥庫結存}   ,藥局庫存:{藥局庫存},藥局結存:{藥局結存}";
                        str_log += msg;
                        if (flag_OK == false) Console.WriteLine($"({index}).{msg}");
                        index++;
                    }
                })));
            



            }
            Task.WhenAll(tasks).Wait();
            Console.WriteLine($"檢查完成...");
            Console.ReadKey();
        }

        public class ICP_交易記錄查詢 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime1 = x[(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString_6().StringToDateTime();
                DateTime datetime2 = y[(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString_6().StringToDateTime();
                int compare = DateTime.Compare(datetime2, datetime1);
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
