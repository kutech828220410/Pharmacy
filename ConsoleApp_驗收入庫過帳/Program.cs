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
namespace ConsoleApp_驗收入庫過帳
{
    class Program
    {
        public enum enum_驗收入庫明細
        {
            GUID,
            請購單號,
            驗收單號,
            藥品碼,
            藥品名稱,
            包裝單位,
            數量,
            效期,
            批號,
            驗收時間,
            加入時間,
            狀態,
            來源,
            備註,
        }
        public enum enum_驗收入庫明細_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
            無效期可入帳,
            忽略過帳,
        }
        public enum enum_補給驗收入庫
        {
            GUID,
            請購單號,
            驗收單號,
            藥品碼,
            數量,
            效期,
            批號,
            驗收時間,
            加入時間,
            狀態,
            來源,
            備註,
        }
        static string API_Server = "http://127.0.0.1:4433";
        static SQL_DataGridView.ConnentionClass DB_Basic = new SQL_DataGridView.ConnentionClass();
        static SQL_DataGridView.ConnentionClass DB_Medicine_Cloud = new SQL_DataGridView.ConnentionClass();
        static SQL_DataGridView.ConnentionClass DB_DS01 = new SQL_DataGridView.ConnentionClass();
        
        static void Main(string[] args)
        {
            Logger.Log($"------------------開始執行------------------");
            try
            {
                bool isNewInstance;
                string applicationName = Assembly.GetExecutingAssembly().GetName().Name; // 获取程序集名称
                using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, applicationName, out isNewInstance))
                {
                    if (isNewInstance)
                    {
                        Console.WriteLine("This is a new instance of the application.");
                        Console.WriteLine($"ApplicationName: [{applicationName}]");

                        List<HIS_DB_Lib.ServerSettingClass> serverSettingClasses = ServerSettingClassMethod.WebApiGet($"{API_Server}/api/serversetting");
                        HIS_DB_Lib.ServerSettingClass serverSettingClass;
                        serverSettingClass = serverSettingClasses.MyFind("ds01", enum_ServerSetting_Type.藥庫, enum_ServerSetting_藥庫.VM端);
                        if (serverSettingClass != null)
                        {

                            DB_Basic.IP = serverSettingClass.Server;
                            DB_Basic.Port = (uint)(serverSettingClass.Port.StringToInt32());
                            DB_Basic.DataBaseName = serverSettingClass.DBName;
                            DB_Basic.UserName = serverSettingClass.User;
                            DB_Basic.Password = serverSettingClass.Password;
                        }
                        serverSettingClass = serverSettingClasses.MyFind("Main", enum_ServerSetting_Type.網頁, "藥檔資料");
                        if (serverSettingClass != null)
                        {

                            DB_Medicine_Cloud.IP = serverSettingClass.Server;
                            DB_Medicine_Cloud.Port = (uint)(serverSettingClass.Port.StringToInt32());
                            DB_Medicine_Cloud.DataBaseName = serverSettingClass.DBName;
                            DB_Medicine_Cloud.UserName = serverSettingClass.User;
                            DB_Medicine_Cloud.Password = serverSettingClass.Password;
                        }
                        serverSettingClass = serverSettingClasses.MyFind("ds01", enum_ServerSetting_Type.藥庫, "一般資料");
                        if (serverSettingClass != null)
                        {

                            DB_DS01.IP = serverSettingClass.Server;
                            DB_DS01.Port = (uint)(serverSettingClass.Port.StringToInt32());
                            DB_DS01.DataBaseName = serverSettingClass.DBName;
                            DB_DS01.UserName = serverSettingClass.User;
                            DB_DS01.Password = serverSettingClass.Password;
                        }

                        SQLControl sQLControl_補給驗收入庫 = new SQLControl(DB_Basic.IP, DB_Basic.DataBaseName, DB_Basic.UserName, DB_Basic.Password, DB_Basic.Port);
                        sQLControl_補給驗收入庫.TableName = "acceptance_med";
                        List<object[]> list_補給驗收入庫 = sQLControl_補給驗收入庫.GetRowsByDefult(null, (int)enum_補給驗收入庫.狀態, enum_驗收入庫明細_狀態.等待過帳.GetEnumName());
                        List<object[]> list_驗收明細 = Function_驗收入庫明細_取得資料(list_補給驗收入庫);
                        Function_驗收入庫明細_選取資料過帳(list_驗收明細);
                    }
                    else
                    {
                        Console.WriteLine("An instance of the application is already running.");
                        Console.WriteLine("exit application 3...");
                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine("exit application 2...");
                        System.Threading.Thread.Sleep(1000);
                        Console.WriteLine("exit application 1...");
                        System.Threading.Thread.Sleep(1000);
                        return;
                    }
                }
            }
            catch(Exception e)
            {
                Logger.Log($"Exception : {e.Message}");
                Console.WriteLine($"Exception : {e.Message}");
                Console.ReadKey();
            }
            finally
            {
                Logger.Log($"------------------程序結束------------------");
            }
     
           
        }
        static private List<object[]> Function_驗收入庫明細_取得資料(List<object[]> list_補給驗收入庫)
        {
            SQLControl sQLControl_藥庫_藥品資料 = new SQLControl(DB_Medicine_Cloud.IP, DB_Medicine_Cloud.DataBaseName, DB_Medicine_Cloud.UserName, DB_Medicine_Cloud.Password, DB_Medicine_Cloud.Port);
            sQLControl_藥庫_藥品資料.TableName = "medicine_page_cloud";
            List<object[]> list_補給驗收入庫_buf = new List<object[]>();
            List<object[]> list_藥品資料 = sQLControl_藥庫_藥品資料.GetAllRows(null);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            List<object[]> list_藥庫_驗收入庫 = new List<object[]>();
            List<object[]> list_藥庫_驗收入庫_error = new List<object[]>();
            for (int i = 0; i < list_補給驗收入庫.Count; i++)
            {
                object[] value = list_補給驗收入庫[i].CopyRow(new enum_補給驗收入庫(), new enum_驗收入庫明細());
                string 藥品碼 = value[(int)enum_驗收入庫明細.藥品碼].ObjectToString();
                string 料號 = value[(int)enum_驗收入庫明細.藥品碼].ObjectToString();
                DateTime dateTime = value[(int)enum_驗收入庫明細.驗收時間].StringToDateTime();
                list_補給驗收入庫_buf = list_補給驗收入庫.GetRowsInDate((int)enum_補給驗收入庫.驗收時間, dateTime);
                list_補給驗收入庫_buf = list_補給驗收入庫_buf.GetRows((int)enum_驗收入庫明細.藥品碼, 藥品碼);
                if (list_補給驗收入庫_buf.Count == 0)
                {

                }
                else if (list_補給驗收入庫_buf.Count >= 2)
                {

                }
                if (list_補給驗收入庫_buf[0][(int)enum_補給驗收入庫.來源].ObjectToString() == "院內系統")
                {

                }
                if (藥品碼.Length == 10)
                {
                    藥品碼 = 藥品碼.Substring(藥品碼.Length - 5, 5);
                    value[(int)enum_驗收入庫明細.藥品碼] = 藥品碼;
                }
                else if (藥品碼.Length == 12)
                {
                    藥品碼 = 藥品碼.Substring(藥品碼.Length - 7, 5);
                    value[(int)enum_驗收入庫明細.藥品碼] = 藥品碼;
                    list_藥庫_驗收入庫_error.Add(value);
                }
                list_藥庫_驗收入庫.Add(value);
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥庫_藥品資料.藥品碼, 藥品碼);
                if (list_藥品資料_buf.Count == 0)
                {

                    continue;
                }
                value[(int)enum_驗收入庫明細.藥品名稱] = list_藥品資料_buf[0][(int)enum_藥庫_藥品資料.藥品名稱];
                value[(int)enum_驗收入庫明細.包裝單位] = list_藥品資料_buf[0][(int)enum_藥庫_藥品資料.包裝單位];

            }
            list_藥庫_驗收入庫.Sort(new ICP_驗收入庫_過帳明細());
            list_藥庫_驗收入庫_error.Sort(new ICP_驗收入庫_過帳明細());
            return list_藥庫_驗收入庫;
        }
        static private void Function_驗收入庫明細_選取資料過帳(List<object[]> list_驗收入庫明細)
        {
            try
            {
                List<object[]> list_交易紀錄_add = new List<object[]>();
                List<object[]> list_驗收入庫明細_replace = new List<object[]>();
                List<object[]> list_補給驗收入庫_buf = new List<object[]>();
                List<object[]> list_補給驗收入庫_replace = new List<object[]>();

                DeviceBasicClass DeviceBasicClass_藥庫 = new DeviceBasicClass();
                DeviceBasicClass_藥庫.Init("127.0.0.1", "ds01", "firstclass_device_jsonstring", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
                SQLControl sQLControl_補給驗收入庫 = new SQLControl(DB_Basic.IP, DB_Basic.DataBaseName, DB_Basic.UserName, DB_Basic.Password, DB_Basic.Port);
                sQLControl_補給驗收入庫.TableName = "acceptance_med";
                SQLControl sQLControl_交易紀錄 = new SQLControl(DB_DS01.IP, DB_DS01.DataBaseName, DB_DS01.UserName, DB_DS01.Password, DB_DS01.Port);
                sQLControl_交易紀錄.TableName = "trading";
                List<DeviceBasic> deviceBasics = DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
                List<DeviceBasic> deviceBasics_replace = new List<DeviceBasic>();
                Logger.Log($"整理驗收資料共<{list_驗收入庫明細.Count}>筆...");
                for (int i = 0; i < list_驗收入庫明細.Count; i++)
                {
                    Console.WriteLine($"整理驗收資料({i}/{list_驗收入庫明細.Count})");
                    bool flag_continue = true;
                    if (list_驗收入庫明細[i][(int)enum_驗收入庫明細.狀態].ObjectToString() == enum_驗收入庫明細_狀態.等待過帳.GetEnumName()) flag_continue = false;
                    if (list_驗收入庫明細[i][(int)enum_驗收入庫明細.狀態].ObjectToString() == enum_驗收入庫明細_狀態.未建立儲位.GetEnumName()) flag_continue = false;
                    if (list_驗收入庫明細[i][(int)enum_驗收入庫明細.狀態].ObjectToString() == enum_驗收入庫明細_狀態.庫存不足.GetEnumName()) flag_continue = false;
                    if (flag_continue) continue;
                    string 藥品碼 = list_驗收入庫明細[i][(int)enum_驗收入庫明細.藥品碼].ObjectToString();
                    string 效期 = list_驗收入庫明細[i][(int)enum_驗收入庫明細.效期].ObjectToString();
                    if (效期.StringIsEmpty()) 效期 = list_驗收入庫明細[i][(int)enum_驗收入庫明細.效期].ToDateString();
                    string 批號 = list_驗收入庫明細[i][(int)enum_驗收入庫明細.批號].ObjectToString();
                    string 數量 = list_驗收入庫明細[i][(int)enum_驗收入庫明細.數量].ObjectToString();

                    object[] value = list_驗收入庫明細[i].CopyRow(new enum_驗收入庫明細(), new enum_補給驗收入庫());
                    value[(int)enum_驗收入庫明細.藥品碼] = $"A0000{藥品碼}";
                    if (value[(int)enum_驗收入庫明細.藥品碼].ObjectToString().Length >= 50) continue;
                    List<DeviceBasic> deviceBasics_buf = deviceBasics.SortByCode(藥品碼);

                    if (deviceBasics_buf.Count > 0)
                    {
                        string 庫存量 = deviceBasics_buf[0].Inventory.ToString();

                        deviceBasics_buf[0].效期庫存異動(效期, 批號, 數量);
                        deviceBasics_replace.Add(deviceBasics_buf[0]);
                        value[(int)enum_補給驗收入庫.狀態] = enum_驗收入庫明細_狀態.過帳完成.GetEnumName();
                        list_驗收入庫明細[i][(int)enum_驗收入庫明細.狀態] = enum_驗收入庫明細_狀態.過帳完成.GetEnumName();
                        list_驗收入庫明細_replace.Add(list_驗收入庫明細[i]);



                        string GUID = Guid.NewGuid().ToString();
                        string 動作 = enum_交易記錄查詢動作.驗收入庫.GetEnumName();
                        string 藥品名稱 = list_驗收入庫明細[i][(int)enum_驗收入庫明細.藥品名稱].ObjectToString();
                        string 交易量 = 數量;
                        string 結存量 = deviceBasics_buf[0].Inventory.ToString();
                        string 操作人 = "系統";
                        string 操作時間 = DateTime.Now.ToDateTimeString_6();
                        string 開方時間 = DateTime.Now.ToDateTimeString_6();
                        string 備註 = $"效期[{效期}],批號[{批號}]";
                        object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                        value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                        value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                        value_trading[(int)enum_交易記錄查詢資料.庫別] = "藥庫";
                        value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                        value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                        value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                        value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                        value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                        value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                        value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                        value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                        list_交易紀錄_add.Add(value_trading);
                    }
                    else
                    {
                        value[(int)enum_補給驗收入庫.狀態] = enum_驗收入庫明細_狀態.未建立儲位.GetEnumName();
                        list_驗收入庫明細[i][(int)enum_驗收入庫明細.狀態] = enum_驗收入庫明細_狀態.未建立儲位.GetEnumName();
                        list_驗收入庫明細_replace.Add(list_驗收入庫明細[i]);
                    }
                    list_補給驗收入庫_replace.Add(value);
                }
                Logger.Log($"上傳補給驗收入庫資料 <{list_補給驗收入庫_replace.Count}>筆...");
                Console.WriteLine($"上傳補給驗收入庫資料 <{list_補給驗收入庫_replace.Count}>筆...");
                sQLControl_補給驗收入庫.UpdateByDefulteExtra(null, list_補給驗收入庫_replace);
                Logger.Log($"上傳交易紀錄資料 <{list_交易紀錄_add.Count}>筆...");
                Console.WriteLine($"上傳交易紀錄資料 <{list_交易紀錄_add.Count}>筆...");
                sQLControl_交易紀錄.AddRows(null, list_交易紀錄_add);
                Logger.Log($"藥庫儲位庫存更動 <{deviceBasics_replace.Count}>筆...");
                Console.WriteLine($"藥庫儲位庫存更動 <{deviceBasics_replace.Count}>筆...");
                DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasics_replace);

            }
            catch(Exception e)
            {
                Logger.Log($"Exception : {e.Message}");
                Console.WriteLine($"Exception : {e.Message}");
                Console.ReadKey();
            }
           


        }
        private class ICP_驗收入庫_過帳明細 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                DateTime temp0 = x[(int)enum_驗收入庫明細.加入時間].ToDateTimeString().StringToDateTime();
                DateTime temp1 = y[(int)enum_驗收入庫明細.加入時間].ToDateTimeString().StringToDateTime();
                int cmp = temp0.CompareTo(temp1);
                return cmp;
            }
        }
    }
}
