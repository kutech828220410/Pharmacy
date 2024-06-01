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

namespace ConsoleApp_門診批次帳讀取
{
    class Program
    {
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
        private enum enum_寫入報表設定_類別
        {
            OPD_消耗帳,
            PHR_消耗帳,
            PHER_消耗帳,
            公藥_消耗帳,
            其他,
        }
        public enum enum_過帳狀態
        {
            已產生排程,
            排程已作業,
        }
      
        public enum enum_過帳明細_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
        }

        private enum enum_過帳明細
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
        private enum enum_寫入報表設定
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

        static DateTime dateTime = DateTime.Now.AddDays(0);
        static string API_Server = "http://127.0.0.1:4433";
        static SQL_DataGridView.ConnentionClass DB_Basic = new SQL_DataGridView.ConnentionClass();
        static SQL_DataGridView.ConnentionClass DB_Medicine_Cloud = new SQL_DataGridView.ConnentionClass();
        static SQL_DataGridView.ConnentionClass DB_DS01 = new SQL_DataGridView.ConnentionClass();
        static string filePath = @"C:\MIS\DG";
        static string[] fileNames = new string[] { "DIMR2OF1_[Date].TXT", "DIMR2OF2_[Date].TXT", "DIMU2OB1_[Date].TXT", "DIMU2OB2_[Date].TXT", "DIMU1OB1_[Date].TXT", "DIMU1OB2_[Date].TXT" };//消耗帳1-6
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
                        SQLControl sQLControl_藥庫_藥品資料 = new SQLControl(DB_Medicine_Cloud.IP, DB_Medicine_Cloud.DataBaseName, DB_Medicine_Cloud.UserName, DB_Medicine_Cloud.Password, DB_Medicine_Cloud.Port);
                        sQLControl_藥庫_藥品資料.TableName = "medicine_page_cloud";
                        List<object[]> list_藥品資料 = sQLControl_藥庫_藥品資料.GetAllRows(null);
                        List<medClass> medClasses = list_藥品資料.SQLToClass<medClass, enum_雲端藥檔>();
                        List<medClass> medClasses_buf = new List<medClass>();
                        Dictionary<string, List<medClass>> keyValuePairs_medClass = medClass.CoverToDictionaryByCode(medClasses);


                        SQLControl sQLControl_過帳狀態 = new SQLControl(DB_Basic.IP, DB_Basic.DataBaseName, DB_Basic.UserName, DB_Basic.Password, DB_Basic.Port);
                        sQLControl_過帳狀態.TableName = "posting_state";

                        SQLControl sQLControl_過帳明細 = new SQLControl(DB_Basic.IP, DB_Basic.DataBaseName, DB_Basic.UserName, DB_Basic.Password, DB_Basic.Port);
                        sQLControl_過帳明細.TableName = "posting_sd0_opd";

                        SQLControl sQLControl_寫入報表設定 = new SQLControl(DB_Basic.IP, DB_Basic.DataBaseName, DB_Basic.UserName, DB_Basic.Password, DB_Basic.Port);
                        sQLControl_寫入報表設定.TableName = "posting_table_setting";

                        List<object[]> list_過帳狀態 = sQLControl_過帳狀態.GetAllRows(null);
                        List<object[]> list_報表列表 = sQLControl_寫入報表設定.GetAllRows(null);
                        List<object[]> list_報表列表_buf = new List<object[]>();

                        List<object[]> list_過帳明細_Add = new List<object[]>();
                        List<object[]> list_過帳狀態_buf = new List<object[]>();
                        List<object[]> list_過帳狀態_Add = new List<object[]>();


                        Logger.Log($"取得過帳狀態 <{list_過帳狀態.Count}>筆...");
                        Console.WriteLine($"取得過帳狀態 <{list_過帳狀態.Count}>筆...");

                        for (int i = 0; i < fileNames.Length; i++)
                        {
                            list_過帳明細_Add.Clear();
                            list_過帳狀態_Add.Clear();

                            string date_temp = $"{dateTime.Year.ToString("0000")}{dateTime.Month.ToString("00")}{dateTime.Day.ToString("00")}";
                            string fileName_temp = fileNames[i].Replace("[Date]", date_temp);
                            string filename = $@"{filePath}\{fileName_temp}";
                            list_過帳狀態_buf = (from temp in list_過帳狀態
                                             where temp[(int)enum_過帳狀態列表.檔名].ObjectToString() == fileName_temp
                                             select temp).ToList();

                            list_報表列表_buf = list_報表列表.GetRows((int)enum_過帳狀態列表.檔名, fileNames[i]);

                            Logger.Log($"整理檔案[{fileName_temp}] ({i}/{fileNames.Length})");
                            Console.WriteLine($"整理檔案[{fileName_temp}] ({i}/{fileNames.Length})");

                            if (list_報表列表_buf.Count == 0)
                            {
                                Logger.Log($"報表列表找無資料[{fileName_temp}] ({i}/{fileNames.Length})");
                                Console.WriteLine($"報表列表找無資料[{fileName_temp}] ({i}/{fileNames.Length})");
                                continue;
                            }
                            if (list_過帳狀態_buf.Count > 0)
                            {
                                Logger.Log($"報表已寫入過[{fileName_temp}] ({i}/{fileNames.Length})");
                                Console.WriteLine($"報表已寫入過[{fileName_temp}] ({i}/{fileNames.Length})");
                                continue;
                            }

                            string 來源報表 = fileName_temp;
                            string 藥品碼 = "";
                            string 藥品名稱 = "";
                            string 藥局代碼 = "";
                            string 報表日期 = dateTime.ToDateString();
                            string 報表產出日期 = "";
                            string 異動量 = "";

                            List<string> list_text = MyFileStream.LoadFile(filename);

                            if (list_text == null)
                            {
                                Logger.Log($"解析失敗[{fileName_temp}]");
                                Console.WriteLine($"解析失敗[{fileName_temp}]");
                                continue;
                            }


                            for (int k = 0; k < list_text.Count; k++)
                            {

                                Console.WriteLine($"過帳明細解析[{fileName_temp}] ({k}/{list_text.Count})");
                                object[] value = new object[new enum_過帳明細().GetLength()];
                                Function_過帳明細_解析TXT(list_text[k], ref 藥品碼, ref 藥局代碼, ref 報表產出日期, ref 異動量);
                                medClasses_buf = medClass.SortDictionaryByCode(keyValuePairs_medClass, 藥品碼);
                                if (medClasses_buf.Count > 0)
                                {
                                    藥品名稱 = medClasses_buf[0].藥品名稱;
                                }

                                value[(int)enum_過帳明細.GUID] = Guid.NewGuid().ToString();
                                value[(int)enum_過帳明細.來源報表] = 來源報表;
                                value[(int)enum_過帳明細.藥品碼] = 藥品碼;
                                value[(int)enum_過帳明細.藥品名稱] = 藥品名稱;
                                value[(int)enum_過帳明細.藥局代碼] = 藥局代碼;
                                value[(int)enum_過帳明細.報表日期] = 報表日期;
                                value[(int)enum_過帳明細.異動量] = 異動量.StringToInt32() * -1;
                                value[(int)enum_過帳明細.產出時間] = DateTime.Now.ToDateTimeString_6();
                                value[(int)enum_過帳明細.過帳時間] = DateTime.MinValue.ToDateTimeString_6();
                                value[(int)enum_過帳明細.狀態] = enum_過帳明細_狀態.等待過帳.GetEnumName();
                                value[(int)enum_過帳明細.備註] = "";

                                list_過帳明細_Add.Add(value);
                            }
                            if (list_報表列表_buf.Count > 0)
                            {
                                string 編號 = list_報表列表_buf[0][(int)enum_寫入報表設定.編號].ObjectToString();
                                string 檔名 = list_報表列表_buf[0][(int)enum_寫入報表設定.檔名].ObjectToString();
                                string 檔案位置 = list_報表列表_buf[0][(int)enum_寫入報表設定.檔案位置].ObjectToString();
                                string 類別 = list_報表列表_buf[0][(int)enum_寫入報表設定.類別].ObjectToString();
                                string 更新每日 = list_報表列表_buf[0][(int)enum_寫入報表設定.更新每日].ObjectToString();
                                string 備註內容 = list_報表列表_buf[0][(int)enum_寫入報表設定.備註內容].ObjectToString();
                                int 每週更新週期 = list_報表列表_buf[0][(int)enum_寫入報表設定.更新每週].ObjectToString().StringToInt32();

                                object[] value = new object[new enum_過帳狀態列表().GetLength()];
                                value[(int)enum_過帳狀態列表.GUID] = Guid.NewGuid().ToString();
                                value[(int)enum_過帳狀態列表.編號] = 編號;
                                value[(int)enum_過帳狀態列表.檔名] = fileName_temp;
                                value[(int)enum_過帳狀態列表.檔案位置] = 檔案位置;
                                value[(int)enum_過帳狀態列表.類別] = 類別;
                                value[(int)enum_過帳狀態列表.報表日期] = dateTime.ToDateString();
                                value[(int)enum_過帳狀態列表.產生排程時間] = DateTime.Now.ToDateTimeString_6();
                                value[(int)enum_過帳狀態列表.排程作業時間] = DateTime.Now.ToDateTimeString_6();
                                value[(int)enum_過帳狀態列表.狀態] = enum_過帳狀態.排程已作業.GetEnumName();
                                value[(int)enum_過帳狀態列表.備註內容] = 備註內容;
                                list_過帳狀態_Add.Add(value);

                            }
                            Logger.Log($"新增過帳狀態[{fileName_temp}] <{list_過帳狀態_Add.Count}>筆...");
                            Console.WriteLine($"新增過帳狀態[{fileName_temp}] <{list_過帳狀態_Add.Count}>筆...");
                            sQLControl_過帳狀態.AddRows(null, list_過帳狀態_Add);

                            Logger.Log($"新增過帳明細[{fileName_temp}] <{list_過帳明細_Add.Count}>筆...");
                            Console.WriteLine($"新增過帳明細[{fileName_temp}] <{list_過帳明細_Add.Count}>筆...");
                            sQLControl_過帳明細.AddRows(null, list_過帳明細_Add);
                        

                        }
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
            catch (Exception e)
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

        static private void Function_過帳明細_解析TXT(string text, ref string 藥品碼, ref string 藥局代碼, ref string Date, ref string 異動量)
        {
            int len = text.Length;
            if (len == 28)
            {
                藥品碼 = text.Substring(0, 5);
                藥局代碼 = text.Substring(5, 4).Replace(" ", "");
                string Year = text.Substring(9, 4);
                string Month = text.Substring(13, 2);
                string Day = text.Substring(15, 2);
                string Hour = text.Substring(17, 2);
                string Min = text.Substring(19, 2);
                Date = $"{Year}/{Month}/{Day} {Hour}:{Min}:00";
                異動量 = text.Substring(21, 7).Replace(" ", "");
            }
        }
    }
}
