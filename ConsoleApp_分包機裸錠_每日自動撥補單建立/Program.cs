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

namespace ConsoleApp_分包機裸錠_每日自動撥補單建立
{
    class Program
    {
        static string API_Server = "http://127.0.0.1:4433";

        static void Main(string[] args)
        {
            List<drugStotreDistributionClass> drugStotreDistributionClasses = drugStotreDistributionClass.get_by_addedTime(API_Server, DateTime.Now.GetStartDate(), DateTime.Now.GetEndDate());
            List<drugStotreDistributionClass> drugStotreDistributionClasses_buf = new List<drugStotreDistributionClass>();
            List<drugStotreDistributionClass> drugStotreDistributionClasses_add = new List<drugStotreDistributionClass>();
            List<drugStotreDistributionClass> drugStotreDistributionClasses_replace = new List<drugStotreDistributionClass>();
            List<medClass> medClasses_藥庫 = medClass.get_ds_drugstore_med(API_Server, "ds01");
            List<medClass> medClasses_藥庫_buf = new List<medClass>();
            List<medClass> medClasses_藥局 = medClass.get_ds_pharma_med(API_Server, "ds01");
            List<medClass> medClasses_藥局_buf = new List<medClass>();

            medClasses_藥局 = (from temp in medClasses_藥局
                             where temp.類別 == "分包機裸錠"
                             select temp).ToList();

            medClasses_藥局 = (from temp in medClasses_藥局
                             where temp.藥局庫存.StringToInt32() <= temp.安全庫存.StringToInt32()
                             && temp.安全庫存.StringToInt32() > 0
                             select temp).ToList();
            Console.WriteLine("-----------------------------------------------------------------------");
            for (int i = 0; i < medClasses_藥局.Count; i++)
            {


                drugStotreDistributionClass drugStotreDistributionClass = new drugStotreDistributionClass();
                string 藥碼 = medClasses_藥局[i].藥品碼;
                string 藥名 = medClasses_藥局[i].藥品名稱;
                string 包裝單位 = medClasses_藥局[i].包裝單位;
                drugStotreDistributionClasses_buf = (from temp in drugStotreDistributionClasses
                                                     where temp.藥碼 == 藥碼
                                                     select temp).ToList();

                medClasses_藥庫_buf = (from temp0 in medClasses_藥庫
                                     where temp0.藥品碼 == 藥碼
                                     select temp0).ToList();

                int 藥局庫存 = medClasses_藥局[i].藥局庫存.StringToInt32();
                int 藥庫庫存 = medClasses_藥局[i].藥庫庫存.StringToInt32();
                int 基準量 = medClasses_藥局[i].基準量.StringToInt32();
                int 安全量 = medClasses_藥局[i].安全庫存.StringToInt32();
                int 包裝數量 = medClasses_藥局[i].包裝數量.StringToInt32();
                if (包裝數量 < 0) 包裝數量 = 1;

                int 撥發量 = 基準量 - 藥局庫存;
                int 實撥量 = 0;

                if (撥發量 % 包裝數量 != 0)
                {
                    撥發量 = 撥發量 - (撥發量 % 包裝數量);
                    撥發量 = 撥發量 + 包裝數量;               
                }
                實撥量 = 撥發量;

                if (實撥量 > 藥庫庫存)
                {
                    實撥量 = 藥庫庫存;
                    if (實撥量 % 包裝數量 != 0)
                    {
                        實撥量 = 實撥量 - (實撥量 % 包裝數量);
                    }
                    if (實撥量 < 0) 實撥量 = 0;
                }

                string temp_str = $"「({藥碼}) {藥名}」";
                temp_str = temp_str.StringLength(50);

                drugStotreDistributionClass.GUID = Guid.NewGuid().ToString();
                drugStotreDistributionClass.來源庫別 = "藥庫";
                drugStotreDistributionClass.目的庫別 = "藥局";
                drugStotreDistributionClass.藥碼 = 藥碼;
                drugStotreDistributionClass.藥名 = 藥名;
                drugStotreDistributionClass.包裝單位 = 包裝單位;
                drugStotreDistributionClass.包裝量 = 包裝數量.ToString();
                drugStotreDistributionClass.狀態 = "等待過帳";
                drugStotreDistributionClass.來源庫庫存 = medClasses_藥局[i].藥庫庫存;
                drugStotreDistributionClass.目的庫庫存 = medClasses_藥局[i].藥局庫存;
                drugStotreDistributionClass.撥發量 = 撥發量.ToString();
                drugStotreDistributionClass.實撥量 = 實撥量.ToString();
                drugStotreDistributionClass.報表名稱 = "分包機裸錠";
                drugStotreDistributionClass.加入時間 = DateTime.Now;
                drugStotreDistributionClass.報表生成時間 = DateTime.Now;
                List<StockClass> stockClasses = medClasses_藥庫_buf[0].DeviceBasics[0].庫存異動((實撥量 * -1).ToString());
                stockClasses = stockClasses.QtyAbs();
                drugStotreDistributionClass.issuedStocks = stockClasses;
                if (drugStotreDistributionClasses_buf.Count > 0)
                {
                    if (drugStotreDistributionClasses_buf[0].狀態 != "等待過帳") continue;
                    drugStotreDistributionClass.GUID = drugStotreDistributionClasses_buf[0].GUID;
                    drugStotreDistributionClasses_replace.Add(drugStotreDistributionClass);
                    Console.WriteLine($"[修改]({i.ToString("00")}/{medClasses_藥局.Count}) {temp_str} 撥發量 : {撥發量}");

                }
                else
                {
                    drugStotreDistributionClasses_add.Add(drugStotreDistributionClass);
                    Console.WriteLine($"[新增]({i.ToString("00")}/{medClasses_藥局.Count}) {temp_str} 撥發量 : {撥發量}");
                }



            }
            drugStotreDistributionClass.add(API_Server, drugStotreDistributionClasses_add);
            drugStotreDistributionClass.update_by_guid(API_Server, drugStotreDistributionClasses_replace);

            Console.WriteLine($"新增資料共<{drugStotreDistributionClasses_add.Count}>筆");
            Console.WriteLine($"修改資料共<{drugStotreDistributionClasses_replace.Count}>筆");
            Console.WriteLine("-----------------------------------------------------------------------");

            System.Threading.Thread.Sleep(5000);
        }
    }
}
