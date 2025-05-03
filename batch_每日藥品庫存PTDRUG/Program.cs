using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic;
using HIS_DB_Lib;
namespace batch_每日藥品庫存PTDRUG
{
    class Program
    {
        public static string API_Server = "http://10.18.1.146:4433";
        static returnData get_all_record_menu()
        {
            returnData returnData = new returnData();
            returnData.ServerName = "ds01";
            returnData.ServerType = "藥庫";
            string json_in = returnData.JsonSerializationt();
            string json_out = Basic.Net.WEBApiPostJson($"{API_Server}/api/stockRecord/get_all_record_menu", json_in);
            return json_out.JsonDeserializet<returnData>();
        }
        static returnData get_record_by_guid(string guid)
        {
            returnData returnData = new returnData();
            returnData.ServerName = "ds01";
            returnData.ServerType = "藥庫";
            returnData.ValueAry.Add(guid);
            string json_in = returnData.JsonSerializationt();
            string json_out = Basic.Net.WEBApiPostJson($"{API_Server}/api/stockRecord/get_record_by_guid", json_in);
            return json_out.JsonDeserializet<returnData>();
        }
        static void Main(string[] args)
        {
            DateTime dateTime = DateTime.Now;     
            List<stockRecord> stockRecords = get_all_record_menu().Data.ObjToClass<List<stockRecord>>();
            List<stockRecord> stockRecords_buf = new List<stockRecord>();
            stockRecord stockRecord_drugStorage = new stockRecord();
            stockRecord stockRecord_pharma = new stockRecord();

            stockRecords_buf = (from temp in stockRecords
                                where temp.加入時間.StringToDateTime() >= dateTime.GetStartDate() && temp.加入時間.StringToDateTime() <= dateTime.GetEndDate()
                                where temp.庫別 == "藥庫"
                                select temp).ToList();
            if(stockRecords_buf.Count == 0)
            {
                Logger.Log("找無藥庫紀錄庫存資訊");
                return;
            }
            stockRecord_drugStorage = stockRecords_buf[0];


            stockRecords_buf = (from temp in stockRecords
                                where temp.加入時間.StringToDateTime() >= dateTime.GetStartDate() && temp.加入時間.StringToDateTime() <= dateTime.GetEndDate()
                                where temp.庫別 == "藥局"
                                select temp).ToList();
            if (stockRecords_buf.Count == 0)
            {
                Logger.Log("找無藥局紀錄庫存資訊");
                return;
            }
            stockRecord_pharma = stockRecords_buf[0];

            stockRecord_drugStorage = get_record_by_guid(stockRecord_drugStorage.GUID).Data.ObjToClass<stockRecord>();
            stockRecord_pharma = get_record_by_guid(stockRecord_pharma.GUID).Data.ObjToClass<stockRecord>();

            // 建立 Dictionary 以藥碼為鍵，整併兩邊庫存
            Dictionary<string, (stockRecord_content drugStorage, stockRecord_content pharma)> mergedStock = new Dictionary<string, (stockRecord_content, stockRecord_content)>();

            // 加入藥庫庫存資訊
            foreach (var item in stockRecord_drugStorage.Contents)
            {
                if (!mergedStock.ContainsKey(item.藥碼))
                    mergedStock[item.藥碼] = (item, null);
                else
                    mergedStock[item.藥碼] = (item, mergedStock[item.藥碼].pharma);
            }

            // 加入藥局庫存資訊
            foreach (var item in stockRecord_pharma.Contents)
            {
                if (!mergedStock.ContainsKey(item.藥碼))
                    mergedStock[item.藥碼] = (null, item);
                else
                    mergedStock[item.藥碼] = (mergedStock[item.藥碼].drugStorage, item);
            }

            // 輸出合併結果
            foreach (var kv in mergedStock)
            {
                string code = kv.Key;
                string name = kv.Value.drugStorage?.藥名 ?? kv.Value.pharma?.藥名 ?? "";
                string stock_drugStorage = kv.Value.drugStorage?.庫存 ?? "0";
                string stock_pharma = kv.Value.pharma?.庫存 ?? "0";
                Logger.Log($"藥碼:{code}, 藥名:{name}, 藥庫庫存:{stock_drugStorage}, 藥局庫存:{stock_pharma}");
            }

            string csv_path = $@"C:\MIS\DG\PTDRUG_{DateTime.Now:yyyyMMdd}.csv";
            List<string> csv_lines = new List<string>();
            csv_lines.Add("DRUGCODE,QTY");

            // 合併並計算總庫存
            foreach (var kv in mergedStock)
            {
                string code = kv.Key;
                int qty_drugStorage = kv.Value.drugStorage?.庫存.StringIsInt32() == true ? kv.Value.drugStorage.庫存.StringToInt32() : 0;
                int qty_pharma = kv.Value.pharma?.庫存.StringIsInt32() == true ? kv.Value.pharma.庫存.StringToInt32() : 0;
                int total_qty = qty_drugStorage + qty_pharma;

                csv_lines.Add($"{code},{total_qty}");
            }

            // 寫入CSV檔案
            System.IO.File.WriteAllLines(csv_path, csv_lines, Encoding.UTF8);
            Logger.Log($"✅ 合併庫存CSV已輸出: {csv_path}");

        }
    }
}
