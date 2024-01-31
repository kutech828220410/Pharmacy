using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using MyUI;
using Basic;
using SQLUI;
using HIS_DB_Lib;
using H_Pannel_lib;

namespace drug_consumption
{
    class Program
    {
        enum enum_庫別
        {
            藥庫,
            屏榮藥局,
        }
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
        private enum enum_藥品過消耗帳_來源名稱
        {
            門診,
            急診,
            住院,
            公藥,
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
        enum enum_過帳明細_門診_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
        }
        enum enum_過帳明細_急診_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
        }
        enum enum_過帳明細_住院_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
        }
        enum enum_過帳明細_公藥_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
        }
        private enum enum_過帳明細_門診
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
        private enum enum_過帳明細_急診
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
        private enum enum_過帳明細_住院
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
        private enum enum_過帳明細_公藥
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
        private static System.Threading.Mutex mutex;

        static void Main(string[] args)
        {
            bool isNewInstance;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "drug_consumption", out isNewInstance);

            if (!isNewInstance)
            {
                Console.WriteLine("程式已經在運行中...");
                return;
            }

         
            MyTimerBasic myTimer = new MyTimerBasic();

            try
            {
                List<object[]> list_過賬明細  = Function_藥品過消耗帳_取得所有過帳明細();
                List<object[]> list_過賬明細_buf = new List<object[]>();
                list_過賬明細_buf = (from temp in list_過賬明細
                                 where temp[(int)enum_藥品過消耗帳.狀態].ObjectToString() != enum_過帳明細_門診_狀態.過帳完成.GetEnumName()
                                 select temp).ToList();
                Console.WriteLine($"取得過賬明細,共<{list_過賬明細_buf.Count}>筆,{myTimer.ToString()}");
                Function_藥品過消耗帳_設定過帳完成(list_過賬明細_buf);
                Console.WriteLine($"過賬完成!!!!");
                System.Threading.Thread.Sleep(10000);
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message})");

                //Console.ReadLine(); // 等待使用者按下 Enter
            }
            finally
            {
                mutex.ReleaseMutex(); // 釋放互斥鎖
            }
        }

        static private List<object[]> Function_藥品過消耗帳_取得所有過帳明細()
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic();
            myTimerBasic.StartTickTime(50000);
            SQLControl sQLControl_posting_sd0_opd = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_opd", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_pher = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_pher", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_phr = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_phr", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_public = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_public", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            List<Task> tasks = new List<Task>();

            List<object[]> list_門診 = new List<object[]>();
            List<object[]> list_急診 = new List<object[]>();
            List<object[]> list_住院 = new List<object[]>();
            List<object[]> list_公藥 = new List<object[]>();
            List<object[]> list_門診_buf = new List<object[]>();
            List<object[]> list_急診_buf = new List<object[]>();
            List<object[]> list_住院_buf = new List<object[]>();
            List<object[]> list_公藥_buf = new List<object[]>();

            tasks.Add(Task.Factory.StartNew(new Action(delegate
            {
                MyTimerBasic _myTimerBasic = new MyTimerBasic();
                _myTimerBasic.StartTickTime(50000);
                list_門診 = sQLControl_posting_sd0_opd.GetAllRows(null);
                list_門診_buf = list_門診.CopyRows(new enum_過帳明細_門診(), new enum_藥品過消耗帳());
                for (int i = 0; i < list_門診_buf.Count; i++)
                {
                    list_門診_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.門診.GetEnumName();
                }
                Console.WriteLine($"取得門診所有過帳資料,共<{list_門診.Count}>筆,{_myTimerBasic.ToString()}");
            })));
            tasks.Add(Task.Factory.StartNew(new Action(delegate
            {
                MyTimerBasic _myTimerBasic = new MyTimerBasic();
                _myTimerBasic.StartTickTime(50000);
                list_急診 = sQLControl_posting_sd0_pher.GetAllRows(null);
                list_急診_buf = list_急診.CopyRows(new enum_過帳明細_急診(), new enum_藥品過消耗帳());
                for (int i = 0; i < list_急診_buf.Count; i++)
                {
                    list_急診_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.急診.GetEnumName();
                }
                Console.WriteLine($"取得急診所有過帳資料,共<{list_急診.Count}>筆{_myTimerBasic.ToString()}");
            })));
            tasks.Add(Task.Factory.StartNew(new Action(delegate
            {
                MyTimerBasic _myTimerBasic = new MyTimerBasic();
                _myTimerBasic.StartTickTime(50000);
                list_住院 = sQLControl_posting_sd0_phr.GetAllRows(null);
                list_住院_buf = list_住院.CopyRows(new enum_過帳明細_住院(), new enum_藥品過消耗帳());
                for (int i = 0; i < list_住院_buf.Count; i++)
                {
                    list_住院_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.住院.GetEnumName();
                }
                Console.WriteLine($"取得住院所有過帳資料,共<{list_住院.Count}>筆{_myTimerBasic.ToString()}");
            })));
            tasks.Add(Task.Factory.StartNew(new Action(delegate
            {
                MyTimerBasic _myTimerBasic = new MyTimerBasic();
                _myTimerBasic.StartTickTime(50000);
                list_公藥 = sQLControl_posting_sd0_public.GetAllRows(null);
                list_公藥_buf = list_公藥.CopyRows(new enum_過帳明細_公藥(), new enum_藥品過消耗帳());
                for (int i = 0; i < list_公藥_buf.Count; i++)
                {
                    list_公藥_buf[i][(int)enum_藥品過消耗帳.來源名稱] = enum_藥品過消耗帳_來源名稱.公藥.GetEnumName();
                }
                Console.WriteLine($"取得公藥所有過帳資料,共<{list_公藥.Count}>筆{_myTimerBasic.ToString()}");
            })));
            Task.WhenAll(tasks).Wait();
            List<object[]> list_value = new List<object[]>();


            list_value.LockAdd(list_門診_buf);
            list_value.LockAdd(list_急診_buf);
            list_value.LockAdd(list_住院_buf);
            list_value.LockAdd(list_公藥_buf);

            return list_value;
        }
        static private void Function_藥品過消耗帳_設定過帳完成(List<object[]> list_藥品過消耗帳)
        {
            SQLControl sQLControl_posting_sd0_opd = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_opd", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_pher = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_pher", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_phr = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_phr", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_public = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_public", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_trading = new SQLControl("127.0.0.1", "ds01", "trading", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);

            MyTimer MyTimer_TickTime = new MyTimer();
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(5000000);
            SQL_DataGridView.ConnentionClass DB_DS01 = new SQL_DataGridView.ConnentionClass();
            DB_DS01.IP = "127.0.0.1";
            DB_DS01.DataBaseName = "ds01";
            DB_DS01.UserName = "user";
            DB_DS01.Password = "66437068";
            DB_DS01.Port = 3306;
            DB_DS01.MySqlSslMode = MySql.Data.MySqlClient.MySqlSslMode.None;
            DeviceBasicClass DeviceBasicClass_藥局 = new DeviceBasicClass();
            DeviceBasicClass DeviceBasicClass_藥庫 = new DeviceBasicClass();
            DeviceBasicClass_藥局.Init(DB_DS01, "sd0_device_jsonstring");
            DeviceBasicClass_藥庫.Init(DB_DS01, "firstclass_device_jsonstring");

            List<object[]> list_trading_value = new List<object[]>();
            List<DeviceBasic> deviceBasics = DeviceBasicClass_藥局.SQL_GetAllDeviceBasic();
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
            List<object[]> list_藥品過消耗帳_門急住 = new List<object[]>();

            list_藥品過消耗帳_門急住.LockAdd(list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.門診.GetEnumName()));
            list_藥品過消耗帳_門急住.LockAdd(list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.急診.GetEnumName()));
            list_藥品過消耗帳_門急住.LockAdd(list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.住院.GetEnumName()));

            List<object[]> list_藥品過消耗帳_公藥 = new List<object[]>();
            list_藥品過消耗帳_公藥.LockAdd(list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.公藥.GetEnumName()));
            sub_Function_藥品過消耗帳_設定過帳完成_公藥(list_藥品過消耗帳_公藥);



            Console.WriteLine($"計算過帳內容中...");
            for (int i = 0; i < list_藥品過消耗帳_門急住.Count; i++)
            {
                Console.WriteLine($"{i}/{list_藥品過消耗帳_門急住.Count}");
                //list_value[i] = sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetRows(list_value[i]);
                if (list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.狀態].ObjectToString() == enum_過帳明細_公藥_狀態.過帳完成.GetEnumName())
                {
                    continue;
                }
                string GUID = list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.GUID].ObjectToString();
                string 藥品碼 = list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.藥品碼].ObjectToString();
                string 藥品名稱 = list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.藥品名稱].ObjectToString();
                List<DeviceBasic> deviceBasic_buf = deviceBasics.SortByCode(藥品碼);
                if (deviceBasic_buf.Count == 0)
                {
                    list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.找無此藥品.GetEnumName();
                    list_ReplaceValue.Add(list_藥品過消耗帳_門急住[i]);
                    continue;
                }
                int 已消耗量 = Function_藥品過消耗帳_取得已消耗量(list_藥品過消耗帳_門急住[i]);
                int 需異動量 = list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.異動量].ObjectToString().StringToInt32();
                int 異動量 = 需異動量 - 已消耗量;
                if (異動量 == 0)
                {
                    list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.過帳完成.GetEnumName();
                    list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.過帳時間] = DateTime.Now.ToDateTimeString_6();
                    list_ReplaceValue.Add(list_藥品過消耗帳_門急住[i]);
                    continue;
                }
                int 庫存量 = deviceBasic_buf[0].Inventory.StringToInt32();
                int 結存量 = 庫存量 + 異動量;
                list_儲位資訊 = Function_取得異動儲位資訊(deviceBasic_buf[0], 異動量);
                string 備註 = list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.備註].ObjectToString();
                if (備註.StringIsEmpty() == false) 備註 = $"{備註}\n";
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
                                if (儲位資訊_批號.StringIsEmpty()) 儲位資訊_批號 = "None";
                                備註 += $"[效期]:{儲位資訊_效期},[批號]:{儲位資訊_批號},[數量]:{儲位資訊_異動量}";
                                if (k != list_儲位資訊.Count - 1) 備註 += "\n";
                            }
                            list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.過帳完成.GetEnumName();
                            list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.過帳時間] = DateTime.Now.ToDateTimeString_6();
                            list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.備註] = 備註;

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
                                list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.過帳完成.GetEnumName();
                                list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.過帳時間] = DateTime.Now.ToDateTimeString_6();
                                list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.備註] = 備註;

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
                                list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.無效期可入帳.GetEnumName();
                                list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.備註] = 備註;
                                if (備註.Length > 250)
                                {
                                    list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.備註] = "";
                                }
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
                        list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.過帳時間] = DateTime.Now.ToDateTimeString_6();

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

                        list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.庫存不足.GetEnumName();
                        list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.備註] = 備註;
                    }
                }
                else
                {
                    list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.未建立儲位.GetEnumName();
                    list_藥品過消耗帳_門急住[i][(int)enum_藥品過消耗帳.備註] = 備註;
                }

                deviceBasics_buf = (from value in deviceBasics_Replace
                                    where value.Code == 藥品碼
                                    select value).ToList();
                if (deviceBasics_buf.Count == 0)
                {
                    deviceBasics_Replace.Add(deviceBasic_buf[0]);
                }

                list_ReplaceValue.Add(list_藥品過消耗帳_門急住[i]);
   

            }
            Console.WriteLine($"上傳過帳內容...");
            DeviceBasicClass_藥局.SQL_ReplaceDeviceBasic(deviceBasics_Replace);
            Console.WriteLine($"更新過帳明細...");

            List<object[]> list_藥品過消耗帳_門診 = list_ReplaceValue.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.門診.GetEnumName());
            List<object[]> list_藥品過消耗帳_急診 = list_ReplaceValue.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.急診.GetEnumName());
            List<object[]> list_藥品過消耗帳_住院 = list_ReplaceValue.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.住院.GetEnumName());

            list_藥品過消耗帳_門診 = list_藥品過消耗帳_門診.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_門診());
            list_藥品過消耗帳_急診 = list_藥品過消耗帳_急診.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_急診());
            list_藥品過消耗帳_住院 = list_藥品過消耗帳_住院.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_住院());

            sQLControl_posting_sd0_opd.UpdateByDefulteExtra(null, list_藥品過消耗帳_門診);
            sQLControl_posting_sd0_pher.UpdateByDefulteExtra(null, list_藥品過消耗帳_急診);
            sQLControl_posting_sd0_phr.UpdateByDefulteExtra(null, list_藥品過消耗帳_住院);


            sQLControl_trading.AddRows(null, list_trading_value);

            Console.Write($"藥品過消耗帳(門急住){list_藥品過消耗帳.Count}筆資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
        }
        static private void sub_Function_藥品過消耗帳_設定過帳完成_公藥(List<object[]> list_藥品過消耗帳)
        {
            MyTimer MyTimer_TickTime = new MyTimer();
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(5000000);
            SQLControl sQLControl_posting_sd0_opd = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_opd", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_pher = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_pher", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_phr = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_phr", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_sd0_public = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_public", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_trading = new SQLControl("127.0.0.1", "ds01", "trading", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);

            SQL_DataGridView.ConnentionClass DB_DS01 = new SQL_DataGridView.ConnentionClass();
            DB_DS01.IP = "127.0.0.1";
            DB_DS01.DataBaseName = "ds01";
            DB_DS01.UserName = "user";
            DB_DS01.Password = "66437068";
            DB_DS01.Port = 3306;
            DB_DS01.MySqlSslMode = MySql.Data.MySqlClient.MySqlSslMode.None;
            DeviceBasicClass DeviceBasicClass_藥局 = new DeviceBasicClass();
            DeviceBasicClass DeviceBasicClass_藥庫 = new DeviceBasicClass();
            DeviceBasicClass_藥局.Init(DB_DS01, "sd0_device_jsonstring");
            DeviceBasicClass_藥庫.Init(DB_DS01, "firstclass_device_jsonstring");

            List<object[]> list_trading_value = new List<object[]>();
            List<DeviceBasic> deviceBasics = DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
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

            list_藥品過消耗帳.LockAdd(list_藥品過消耗帳.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.公藥.GetEnumName()));



            Console.WriteLine("(公藥)計算過帳內容中...");
            for (int i = 0; i < list_藥品過消耗帳.Count; i++)
            {
                Console.WriteLine($"(公藥){i}/{list_藥品過消耗帳.Count}");
                //list_value[i] = sqL_DataGridView_批次過帳_公藥_批次過帳明細.SQL_GetRows(list_value[i]);
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
                int 已消耗量 = Function_藥品過消耗帳_取得已消耗量(list_藥品過消耗帳[i]);
                int 需異動量 = list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.異動量].ObjectToString().StringToInt32();
                int 異動量 = 需異動量 - 已消耗量;
                if (異動量 == 0)
                {
                    list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.狀態] = enum_藥品過消耗帳_狀態.過帳完成.GetEnumName();
                    list_藥品過消耗帳[i][(int)enum_藥品過消耗帳.過帳時間] = DateTime.Now.ToDateTimeString_6();
                    list_ReplaceValue.Add(list_藥品過消耗帳[i]);
                    continue;
                }
                int 庫存量 = deviceBasic_buf[0].Inventory.StringToInt32();
                int 結存量 = 庫存量 + 異動量;
                list_儲位資訊 = Function_取得異動儲位資訊(deviceBasic_buf[0], 異動量);
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
                                Function_庫存異動(list_儲位資訊[k]);
                                Function_堆疊資料_取得儲位資訊內容(list_儲位資訊[k], ref 儲位資訊_GUID, ref 儲位資訊_TYPE, ref 儲位資訊_IP, ref 儲位資訊_Num, ref 儲位資訊_效期, ref 儲位資訊_批號, ref 儲位資訊_庫存, ref 儲位資訊_異動量);
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
                            value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.藥庫.GetEnumName();
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
                                value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.藥庫.GetEnumName();
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
                            Function_庫存異動(list_儲位資訊[k]);
                            Function_堆疊資料_取得儲位資訊內容(list_儲位資訊[k], ref 儲位資訊_GUID, ref 儲位資訊_TYPE, ref 儲位資訊_IP, ref 儲位資訊_Num, ref 儲位資訊_效期, ref 儲位資訊_批號, ref 儲位資訊_庫存, ref 儲位資訊_異動量);
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
                        value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.藥庫.GetEnumName();
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
      

            }
            Console.WriteLine($"上傳過帳內容...");
            DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasics_Replace);
            Console.WriteLine($"更新過帳明細...");
            List<object[]> list_藥品過消耗帳_公藥 = list_ReplaceValue.GetRows((int)enum_藥品過消耗帳.來源名稱, enum_藥品過消耗帳_來源名稱.公藥.GetEnumName());


            list_藥品過消耗帳_公藥 = list_藥品過消耗帳_公藥.CopyRows(new enum_藥品過消耗帳(), new enum_過帳明細_公藥());


            sQLControl_posting_sd0_public.UpdateByDefulteExtra(null,list_藥品過消耗帳_公藥);



            sQLControl_trading.AddRows(null, list_trading_value);

            Console.Write($"藥品過消耗帳(公藥){list_藥品過消耗帳_公藥.Count}筆資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
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
            string 備註 = value[(int)enum_藥品過消耗帳.備註].ObjectToString();
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
            List<object[]> list_value = sQLControl.GetRowsByDefult(null ,(int)enum_交易記錄查詢資料.藥品碼, 藥碼);
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
