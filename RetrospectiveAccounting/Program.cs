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
using H_Pannel_lib;
namespace RetrospectiveAccounting
{
    class Program
    {
        public enum enum_儲位資訊
        {
            GUID,
            IP,
            TYPE,
            效期,
            批號,
            庫存,
            異動量,
            Value,
        }
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
        static private SQLControl sQLControl_sd0_public = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_public", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_sd0_opd = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_opd", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_sd0_pher = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_pher", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_sd0_phr = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_phr", "user", "66437068", 3306, MySqlSslMode.None);
        static private SQLControl sQLControl_medicine_cloud = new SQLControl("127.0.0.1", "dbvm", "medicine_page_cloud", "user", "66437068", 3306, MySqlSslMode.None);

        static void Main(string[] args)
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            List<Task> tasks = new List<Task>();
            List<object[]> list_medicine_cloud = new List<object[]>();
            List<object[]> list_sd0_public = new List<object[]>();
            List<object[]> list_sd0_opd = new List<object[]>();
            List<object[]> list_sd0_pher = new List<object[]>();
            List<object[]> list_sd0_phr = new List<object[]>();
            List<object[]> list_消耗帳_藥局 = new List<object[]>();
            List<object[]> list_消耗帳_藥庫 = new List<object[]>();

            string dt_st = new DateTime(2024, 03, 25, 07, 00, 00).ToDateTimeString();
            string dt_end = new DateTime(2024, 03, 25, 07, 59, 00).ToDateTimeString();
            tasks.Add(Task.Run(new Action(delegate
            {
                list_sd0_public = sQLControl_sd0_public.GetRowsByBetween(null, (int)enum_批次過帳_批次過帳明細.產出時間, dt_st, dt_end);
            })));
            tasks.Add(Task.Run(new Action(delegate
            {
                list_sd0_opd = sQLControl_sd0_opd.GetRowsByBetween(null, (int)enum_批次過帳_批次過帳明細.產出時間, dt_st, dt_end);
            })));
            tasks.Add(Task.Run(new Action(delegate
            {
                list_sd0_pher = sQLControl_sd0_pher.GetRowsByBetween(null, (int)enum_批次過帳_批次過帳明細.產出時間, dt_st, dt_end);
            })));
            tasks.Add(Task.Run(new Action(delegate
            {
                list_sd0_phr = sQLControl_sd0_phr.GetRowsByBetween(null, (int)enum_批次過帳_批次過帳明細.產出時間, dt_st, dt_end);
            })));
            Task.WhenAll(tasks).Wait();
            list_消耗帳_藥庫.LockAdd(list_sd0_public);
            list_消耗帳_藥局.LockAdd(list_sd0_opd);
            list_消耗帳_藥局.LockAdd(list_sd0_pher);
            list_消耗帳_藥局.LockAdd(list_sd0_phr);
            Console.WriteLine($"取得消耗帳資料,藥局<{list_消耗帳_藥局.Count}>筆,藥庫<{list_消耗帳_藥庫.Count}>筆,{myTimerBasic}");
            Console.WriteLine($"-------------------------------------------------------------------------");
          
            DeviceBasicClass DeviceBasicClass_藥庫 = new DeviceBasicClass();
       
            DeviceBasicClass_藥庫.Init("127.0.0.1", "ds01", "firstclass_device_jsonstring", "user", "66437068", 3306, MySqlSslMode.None);
            Function_藥局回帳(list_消耗帳_藥局);



            Console.ReadKey();
        }
        static private void Function_藥局回帳(List<object[]> list_藥品過消耗帳_門急住)
        {
          
            List<object[]> list_trading_value = new List<object[]>();
            List<object[]> list_value_buf = new List<object[]>();
            DeviceBasicClass DeviceBasicClass_藥局 = new DeviceBasicClass();
            DeviceBasicClass_藥局.Init("127.0.0.1", "ds01", "sd0_device_jsonstring", "user", "66437068", 3306, MySqlSslMode.None);
            List<string> codes = (from temp in list_藥品過消耗帳_門急住
                                  select temp[(int)enum_批次過帳_批次過帳明細.藥品碼].ObjectToString()).Distinct().ToList();
            List<H_Pannel_lib.StockClass> stockClasses = new List<StockClass>();
            for (int i = 0; i < codes.Count; i++)
            {
                string Code = codes[i];
                list_value_buf = list_藥品過消耗帳_門急住.GetRows((int)enum_批次過帳_批次過帳明細.藥品碼, Code);
                StockClass stockClass = new StockClass();
                stockClass.Code = Code;
              
                if (list_value_buf.Count > 0)
                {
                    int qty = 0;
                    for (int k = 0; k < list_value_buf.Count; k++)
                    {
                        qty += Function_藥品過消耗帳_取得已消耗量(list_value_buf[k]) * -1;
                    }
                    if (qty == 0) continue;
                    stockClass.Name = list_value_buf[0][(int)enum_批次過帳_批次過帳明細.藥品名稱].ObjectToString();
                    stockClass.Qty = (qty).ToString();
                    stockClasses.Add(stockClass);
                }
            }

            for (int i = 0; i < list_藥品過消耗帳_門急住.Count; i++)
            {
                list_藥品過消耗帳_門急住[i][(int)enum_批次過帳_批次過帳明細.狀態] = "過帳完成";
                string 備註 = list_藥品過消耗帳_門急住[i][(int)enum_批次過帳_批次過帳明細.備註].ObjectToString();
                if (備註.StringIsEmpty() == false) 備註 = $"{備註}\n";
                list_藥品過消耗帳_門急住[i][(int)enum_批次過帳_批次過帳明細.備註] = $"{備註}異常回帳";

            }

            List<object[]> list_儲位資訊 = new List<object[]>();
            string 儲位資訊_TYPE = "";
            string 儲位資訊_IP = "";
            string 儲位資訊_Num = "";
            string 儲位資訊_效期 = "";
            string 儲位資訊_批號 = "";
            string 儲位資訊_庫存 = "";
            string 儲位資訊_異動量 = "";
            string 儲位資訊_GUID = "";
            List<DeviceBasic> deviceBasics = DeviceBasicClass_藥局.SQL_GetAllDeviceBasic();
            List<DeviceBasic> deviceBasics_Replace = new List<DeviceBasic>();
            for (int i = 0; i < stockClasses.Count; i++ )
            {
                #region 資料計算
                string 藥品碼 = stockClasses[i].Code;
                string 藥品名稱 = stockClasses[i].Name;
                List<DeviceBasic> deviceBasic_buf = deviceBasics.SortByCode(藥品碼);
                if (deviceBasic_buf.Count == 0)
                {
                    continue;
                }
                int 異動量 = stockClasses[i].Qty.StringToInt32();
                int 庫存量 = deviceBasic_buf[0].Inventory.StringToInt32();
                int 結存量 = 庫存量 + 異動量;
                list_儲位資訊 = Function_取得異動儲位資訊(deviceBasic_buf[0], 異動量);
                if (deviceBasic_buf[0] != null)
                {
                    if (結存量 > 0)
                    {
                        if (庫存量 > 0)
                        {
                            for (int k = 0; k < list_儲位資訊.Count; k++)
                            {
                                Function_庫存異動(list_儲位資訊[k]);
                                Function_堆疊資料_取得儲位資訊內容(list_儲位資訊[k], ref 儲位資訊_GUID, ref 儲位資訊_TYPE, ref 儲位資訊_IP, ref 儲位資訊_Num, ref 儲位資訊_效期, ref 儲位資訊_批號, ref 儲位資訊_庫存, ref 儲位資訊_異動量);
                        
                            }

                            object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                            value_trading[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                            value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                            value_trading[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.盤點調整.GetEnumName();
                            value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                            value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                            value_trading[(int)enum_交易記錄查詢資料.交易量] = 異動量;
                            value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                            value_trading[(int)enum_交易記錄查詢資料.備註] = "異常回帳";
                            value_trading[(int)enum_交易記錄查詢資料.庫別] = "屏榮藥局";
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
                    

                                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                                value_trading[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                                value_trading[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.盤點調整.GetEnumName();
                                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                                value_trading[(int)enum_交易記錄查詢資料.交易量] = 異動量;
                                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                                value_trading[(int)enum_交易記錄查詢資料.備註] = "異常回帳";
                           
                                value_trading[(int)enum_交易記錄查詢資料.庫別] = "屏榮藥局";
                                value_trading[(int)enum_交易記錄查詢資料.操作人] = "系統";
                                value_trading[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();
                                list_trading_value.Add(value_trading);
                            }
                            else
                            {
                            

                            }

                        }
                    }
                    else
                    {
                        異動量 = 0;
                        for (int k = 0; k < list_儲位資訊.Count; k++)
                        {
                            Function_庫存異動(list_儲位資訊[k]);
                            Function_堆疊資料_取得儲位資訊內容(list_儲位資訊[k], ref 儲位資訊_GUID, ref 儲位資訊_TYPE, ref 儲位資訊_IP, ref 儲位資訊_Num, ref 儲位資訊_效期, ref 儲位資訊_批號, ref 儲位資訊_庫存, ref 儲位資訊_異動量);
                         
                            異動量 += 儲位資訊_異動量.StringToInt32();
                        }

                        結存量 = 庫存量 + 異動量;
                        if (庫存量 == 0)
                        {
                            continue;
                        }

                        object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                        value_trading[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                        value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                        value_trading[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.盤點調整.GetEnumName();
                        value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                        value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                        value_trading[(int)enum_交易記錄查詢資料.交易量] = 異動量;
                        value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                        value_trading[(int)enum_交易記錄查詢資料.備註] = "異常回帳";
                        value_trading[(int)enum_交易記錄查詢資料.庫別] = "屏榮藥局";
                        value_trading[(int)enum_交易記錄查詢資料.操作人] = "系統";
                        value_trading[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();
                        list_trading_value.Add(value_trading);

                    }
                }
                else
                {

                }
                #endregion

                List<DeviceBasic> deviceBasics_buf = (from value in deviceBasics_Replace
                                    where value.Code == 藥品碼
                                    select value).ToList();
                if (deviceBasics_buf.Count == 0)
                {
                    deviceBasics_Replace.Add(deviceBasic_buf[0]);
                }
            }

        

            List<object[]> list_藥品過消耗帳_門診 = list_藥品過消耗帳_門急住.GetRows((int)enum_批次過帳_批次過帳明細.藥局代碼, "OPD");
            List<object[]> list_藥品過消耗帳_急診 = list_藥品過消耗帳_門急住.GetRows((int)enum_批次過帳_批次過帳明細.藥局代碼, "PHER");
            List<object[]> list_藥品過消耗帳_住院 = list_藥品過消耗帳_門急住.GetRows((int)enum_批次過帳_批次過帳明細.藥局代碼, "PHR");

            SQLControl sQLControl_posting_sd0_opd = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_opd", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_pher = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_pher", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_phr = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_phr", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_public = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_public", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);


            Console.WriteLine($"上傳過帳內容...");
            DeviceBasicClass_藥局.SQL_ReplaceDeviceBasic(deviceBasics_Replace);
            Console.WriteLine($"更新過帳明細...");

            SQLControl sQLControl_trading = new SQLControl("127.0.0.1", "ds01", "trading", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);

            sQLControl_posting_sd0_opd.UpdateByDefulteExtra(null, list_藥品過消耗帳_門診);
            sQLControl_posting_sd0_pher.UpdateByDefulteExtra(null, list_藥品過消耗帳_急診);
            sQLControl_posting_sd0_phr.UpdateByDefulteExtra(null, list_藥品過消耗帳_住院);

            sQLControl_trading.AddRows(null, list_trading_value);

        }
        static private int Function_藥品過消耗帳_取得已消耗量(object[] value)
        {
            int value_out = 0;
            List<string> 效期 = new List<string>();
            List<string> 數量 = new List<string>();
            Function_藥品過消耗帳_取得效期數量(value, ref 效期, ref 數量);
            for (int i = 0; i < 數量.Count; i++)
            {
                if (數量[i].StringIsInt32())
                {
                    value_out += 數量[i].StringToInt32();
                }
            }
            return value_out;
        }
        static private void Function_藥品過消耗帳_取得效期數量(object[] value, ref List<string> 效期, ref List<string> 數量)
        {
            string 備註 = value[(int)enum_批次過帳_批次過帳明細.備註].ObjectToString();
            備註 = 備註.Replace('\n', ',');
            效期 = 備註.GetTextValues("效期");
            數量 = 備註.GetTextValues("數量");
        }
        static private void Funnction_交易記錄查詢_取得指定藥碼批號期效期(string 藥碼, ref List<string> list_效期, ref List<string> list_批號)
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();

            SQLControl sQLControl = new SQLControl("127.0.0.1", "ds01", "trading", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);

            List<string> list_效期_buf = new List<string>();
            List<string> list_操作時間 = new List<string>();
            if (藥碼.StringIsEmpty()) return;
            List<object[]> list_value = sQLControl.GetRowsByDefult(null, (int)enum_交易記錄查詢資料.藥品碼, 藥碼);
            Console.WriteLine($"搜尋藥碼: {藥碼} , {myTimerBasic.ToString()}");
            string 備註 = "";
            string 操作時間 = "";
            for (int i = 0; i < list_value.Count; i++)
            {
                備註 = list_value[i][(int)enum_交易記錄查詢資料.備註].ObjectToString();
                string[] temp_ary = 備註.Split('\n');
                for (int k = 0; k < temp_ary.Length; k++)
                {
                    string 效期 = temp_ary[k].GetTextValue("效期");
                    string 批號 = temp_ary[k].GetTextValue("批號");
                    操作時間 = list_value[i][(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString();
                    if (效期.StringIsEmpty() == true) continue;
                    list_效期_buf = (from temp in list_效期
                                   where temp == 效期
                                   select temp).ToList();
                    if (list_效期_buf.Count > 0) continue;
                    list_效期.Add(效期);
                    list_批號.Add(批號);
                    list_操作時間.Add(操作時間);
                }
            }
            // 組合效期、批號和操作時間
            List<Tuple<string, string, DateTime>> combinedList = new List<Tuple<string, string, DateTime>>();
            for (int i = 0; i < list_效期.Count; i++)
            {
                combinedList.Add(new Tuple<string, string, DateTime>(list_效期[i], list_批號[i], DateTime.Parse(list_操作時間[i])));
            }

            // 根據操作時間排序
            combinedList.Sort((x, y) => DateTime.Compare(y.Item3, x.Item3));

            // 更新list_效期、list_批號和list_操作時間
            list_效期.Clear();
            list_批號.Clear();
            list_操作時間.Clear();
            foreach (var item in combinedList)
            {
                list_效期.Add(item.Item1);
                list_批號.Add(item.Item2);
                list_操作時間.Add(item.Item3.ToString());
            }
        }

        static private List<object[]> Function_取得異動儲位資訊(DeviceBasic deviceBasic, int 異動量)
        {

            List<object[]> 儲位資訊_buf = new List<object[]>();
            List<object[]> 儲位資訊 = new List<object[]>();

            for (int i = 0; i < deviceBasic.List_Validity_period.Count; i++)
            {
                object[] value = new object[new enum_儲位資訊().GetLength()];
                value[(int)enum_儲位資訊.效期] = deviceBasic.List_Validity_period[i];
                value[(int)enum_儲位資訊.批號] = deviceBasic.List_Lot_number[i];
                value[(int)enum_儲位資訊.庫存] = deviceBasic.List_Inventory[i];
                value[(int)enum_儲位資訊.異動量] = "0";
                value[(int)enum_儲位資訊.Value] = deviceBasic;
                儲位資訊.Add(value);
            }

            儲位資訊 = 儲位資訊.OrderBy(r => DateTime.Parse(r[(int)enum_儲位資訊.效期].ToDateString())).ToList();

            if (異動量 == 0) return 儲位資訊;
            int 使用數量 = 異動量;
            int 庫存數量 = 0;
            int 剩餘庫存數量 = 0;
            for (int i = 0; i < 儲位資訊.Count; i++)
            {
                庫存數量 = 儲位資訊[i][(int)enum_儲位資訊.庫存].ObjectToString().StringToInt32();
                if ((使用數量 < 0 && 庫存數量 > 0) || (使用數量 > 0 && 庫存數量 >= 0))
                {
                    剩餘庫存數量 = 庫存數量 + 使用數量;
                    if (剩餘庫存數量 >= 0)
                    {
                        儲位資訊[i][(int)enum_儲位資訊.異動量] = (使用數量).ToString();
                        儲位資訊_buf.Add(儲位資訊[i]);
                        break;
                    }
                    else
                    {
                        儲位資訊[i][(int)enum_儲位資訊.異動量] = (庫存數量 * -1).ToString();
                        使用數量 = 剩餘庫存數量;
                        儲位資訊_buf.Add(儲位資訊[i]);
                    }
                }
            }

            return 儲位資訊_buf;
        }
        static private void Function_庫存異動(object[] 儲位資訊)
        {
            object Value = 儲位資訊[(int)enum_儲位資訊.Value];
            string 效期 = 儲位資訊[(int)enum_儲位資訊.效期].ObjectToString();
            string 異動量 = 儲位資訊[(int)enum_儲位資訊.異動量].ObjectToString();
            if (Value is DeviceBasic)
            {
                DeviceBasic deviceBasic = (DeviceBasic)Value;
                if (deviceBasic != null)
                {
                    deviceBasic.效期庫存異動(效期, 異動量, false);
                    return;
                }
            }
        }
        static private void Function_堆疊資料_取得儲位資訊內容(object[] value, ref string Device_GUID, ref string TYPE, ref string IP, ref string Num, ref string 效期, ref string 批號, ref string 庫存, ref string 異動量)
        {
            if (value[(int)enum_儲位資訊.Value] is Device)
            {
                Device device = value[(int)enum_儲位資訊.Value] as Device;
                IP = device.IP;
                TYPE = device.DeviceType.GetEnumName();
                Device_GUID = device.GUID;
                Num = device.MasterIndex.ToString();
            }
            IP = value[(int)enum_儲位資訊.IP].ObjectToString();
            TYPE = value[(int)enum_儲位資訊.TYPE].ObjectToString();
            效期 = value[(int)enum_儲位資訊.效期].ObjectToString();
            批號 = value[(int)enum_儲位資訊.批號].ObjectToString();
            庫存 = value[(int)enum_儲位資訊.庫存].ObjectToString();
            異動量 = value[(int)enum_儲位資訊.異動量].ObjectToString();

        }
    }
}
