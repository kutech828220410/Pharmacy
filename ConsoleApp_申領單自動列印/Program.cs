using System;
using System.Collections.Generic;
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
using MyOffice;
using MyPrinterlib;

namespace ConsoleApp_申領單自動列印
{
    public class class_emg_apply
    {
        [JsonPropertyName("cost_center")]
        public string 成本中心 { get; set; }
        [JsonPropertyName("code")]
        public string 藥品碼 { get; set; }
        [JsonPropertyName("name")]
        public string 藥品名稱 { get; set; }
        [JsonPropertyName("inventory")]
        public string 庫存量 { get; set; }
        [JsonPropertyName("value")]
        public string 撥出量 { get; set; }
        [JsonPropertyName("note")]
        public string 備註 { get; set; }
    }
    enum enum_藥庫_撥補_藥局_緊急申領
    {
        GUID,
        藥局代碼,
        藥品碼,
        藥品名稱,
        庫存,
        異動量,
        結存量,
        產出時間,
        過帳時間,
        狀態,
        備註,
    }
    class Program
    {
        static private PrinterClass printerClass = new PrinterClass();
        static private string emg_apply_ApiURL = "https://10.18.1.146:4434/api/excel/emg_apply/";
        static private string API_Server = "http://10.18.1.146:4433";
        static void Main(string[] args)
        {
            try
            {
                printerClass.Init();
                printerClass.PrintPageEvent += PrinterClass_PrintPageEvent;

                Table table = new Table("emg_application_sd0_opd");
                table.Server = "10.18.1.146";
                table.Username = "user";
                table.Password = "66437068";
                table.DBName = "ds01";
                table.Port = "3306";
                SQLControl sQLControl_緊急申領 = new SQLControl(table);
                Logger.LogAddLine();
                List<object[]> list_緊急申領 = sQLControl_緊急申領.GetRowsByBetween(null, (int)enum_藥庫_撥補_藥局_緊急申領.產出時間, DateTime.Now.GetStartDate().ToDateTimeString(), DateTime.Now.GetEndDate().ToDateTimeString());
                Logger.Log($"共取得申領資料共<{list_緊急申領.Count}>筆 ({DateTime.Now.GetStartDate().ToDateTimeString()}~{ DateTime.Now.GetEndDate().ToDateTimeString()})");
                list_緊急申領 = (from temp in list_緊急申領
                             where temp[(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() == "等待過帳"
                             select temp).ToList();
                Logger.Log($"其中[等待過帳]申領資料共<{list_緊急申領.Count}>筆 ({DateTime.Now.GetStartDate().ToDateTimeString()}~{ DateTime.Now.GetEndDate().ToDateTimeString()})");
                List<medClass> medClasses_ds = medClass.get_ds_drugstore_med(API_Server ,"ds01");
                List<medClass> medClasses_ds_buf = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_medClasses_ds = medClasses_ds.CoverToDictionaryByCode();


                List<class_emg_apply> class_Emg_Applies = new List<class_emg_apply>();
                for (int i = 0; i < list_緊急申領.Count; i++)
                {
                    class_emg_apply class_Emg_Apply = new class_emg_apply();
                    class_Emg_Apply.成本中心 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥局代碼].ObjectToString();
                    class_Emg_Apply.藥品碼 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString();
                    class_Emg_Apply.藥品名稱 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品名稱].ObjectToString();
                    class_Emg_Apply.撥出量 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.異動量].ObjectToString();
                    class_Emg_Apply.庫存量 = "0";
                    medClasses_ds_buf = keyValuePairs_medClasses_ds.SortDictionaryByCode(class_Emg_Apply.藥品碼);
                    if (medClasses_ds_buf.Count > 0)
                    {
                        class_Emg_Apply.庫存量 = medClasses_ds_buf[0].藥庫庫存;
                    }


                    class_Emg_Apply.備註 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.備註].ObjectToString();
                    class_Emg_Applies.Add(class_Emg_Apply);
                }

                string json_in = class_Emg_Applies.JsonSerializationt(true);
                string json = Basic.Net.WEBApiPostJson($"{emg_apply_ApiURL}", json_in);
                List<SheetClass> sheetClass = json.JsonDeserializet<List<SheetClass>>();

                Logger.Log($"取得資料表共<{sheetClass.Count}>張");
                if (sheetClass.Count > 0)
                {
                    printerClass.PrinterName = "藥庫";
                    printerClass.Print(sheetClass, PrinterClass.PageSize.A4);
                }
                for (int i = 0; i < list_緊急申領.Count; i++)
                {
                    if (list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() == "過帳完成") continue;
                    list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = "已列印";
                }
                sQLControl_緊急申領.UpdateByDefulteExtra(null, list_緊急申領);
                Logger.Log($"列印完成");
             
            }
            catch(Exception ex)
            {
                Logger.Log($"Exception : {ex.Message}");
            }
            finally
            {
                Logger.LogAddLine();
            }
         
        }

        static private void PrinterClass_PrintPageEvent(object sender, Graphics g, int width, int height, int page_num)
        {
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            using (Bitmap bitmap = printerClass.GetSheetClass(page_num).GetBitmap(width, height, 0.75, H_Alignment.Center, V_Alignment.Top, 0, 50))
            {
                rectangle.Height = bitmap.Height;
                g.DrawImage(bitmap, rectangle);
            }
        }
    }
}
