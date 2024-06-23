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

namespace ConsoleApp_ABC消耗量計算
{
    class Program
    {
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
        private enum enum_ABC報表
        {
            藥碼,
            藥名,
            消耗量,
            成本價,
            總金額,
            基準量,
            安全量,
            ABC類,
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
        public static string currentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        static void Main(string[] args)
        {
            SQLControl sQLControl_posting_門診 = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_opd", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_急診 = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_pher", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_住院 = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_phr", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);
            SQLControl sQLControl_posting_公藥 = new SQLControl("127.0.0.1", "ds01_vm", "posting_sd0_public", "user", "66437068", 3306, MySql.Data.MySqlClient.MySqlSslMode.None);

            List<object[]> list_門診消耗帳 = new List<object[]>();
            List<object[]> list_急診消耗帳 = new List<object[]>();
            List<object[]> list_住院消耗帳 = new List<object[]>();
            List<object[]> list_公藥消耗帳 = new List<object[]>();
            List<object[]> list_消耗帳 = new List<object[]>();
            DateTime dateTime_st = DateTime.Now.GetFirstDayOfMonth().AddDays(-1).AddDays(-90);
            DateTime dateTime_end = DateTime.Now.GetFirstDayOfMonth().AddDays(-1).AddDays(0);
            List<Task> tasks = new List<Task>();
            Logger.LogAddLine();
            tasks.Add(Task.Run(new Action(delegate 
            {
                list_門診消耗帳 = sQLControl_posting_門診.GetRowsByBetween(null, (int)enum_過帳明細.報表日期, dateTime_st.ToDateTimeString(), dateTime_end.ToDateTimeString());
                Logger.Log($"取得門診消耗帳,({dateTime_st.ToDateTimeString()}-{dateTime_end.ToDateTimeString()})共<{list_門診消耗帳.Count}>筆");
            })));
            tasks.Add(Task.Run(new Action(delegate
            {
                list_急診消耗帳 = sQLControl_posting_急診.GetRowsByBetween(null, (int)enum_過帳明細.報表日期, dateTime_st.ToDateTimeString(), dateTime_end.ToDateTimeString());
                Logger.Log($"取得急診消耗帳,({dateTime_st.ToDateTimeString()}-{dateTime_end.ToDateTimeString()})共<{list_急診消耗帳.Count}>筆");
            })));
            tasks.Add(Task.Run(new Action(delegate
            {
                list_住院消耗帳 = sQLControl_posting_住院.GetRowsByBetween(null, (int)enum_過帳明細.報表日期, dateTime_st.ToDateTimeString(), dateTime_end.ToDateTimeString());
                Logger.Log($"取得住院消耗帳,({dateTime_st.ToDateTimeString()}-{dateTime_end.ToDateTimeString()})共<{list_住院消耗帳.Count}>筆");
            })));
            tasks.Add(Task.Run(new Action(delegate
            {
                list_公藥消耗帳 = sQLControl_posting_公藥.GetRowsByBetween(null, (int)enum_過帳明細.報表日期, dateTime_st.ToDateTimeString(), dateTime_end.ToDateTimeString());
                Logger.Log($"取得公藥消耗帳,({dateTime_st.ToDateTimeString()}-{dateTime_end.ToDateTimeString()})共<{list_公藥消耗帳.Count}>筆");
            })));

            Task.WhenAll(tasks).Wait();


            list_消耗帳.LockAdd(list_門診消耗帳);
            list_消耗帳.LockAdd(list_急診消耗帳);
            list_消耗帳.LockAdd(list_住院消耗帳);
            list_消耗帳.LockAdd(list_公藥消耗帳);

            string MedPrice = Basic.Net.WEBApiGet($"https://10.18.1.146:4434/api/MedPrice");
            List<class_MedPrice> class_MedPrices = MedPrice.JsonDeserializet<List<class_MedPrice>>();
            List<class_MedPrice> class_MedPrices_buf = new List<class_MedPrice>();

            Dictionary<object, List<object[]>> keyValuePairs_消耗帳 = list_消耗帳.ConvertToDictionary((int)enum_過帳明細.藥品碼);
            Logger.Log($"({dateTime_st.ToDateTimeString()}-{dateTime_end.ToDateTimeString()})共有<{list_消耗帳.Count}>筆消耗帳,<{keyValuePairs_消耗帳.Keys.Count}>種藥品");
            List<object[]> list_ABC = new List<object[]>();
            int index = 0;
            foreach (string key in keyValuePairs_消耗帳.Keys)
            {
              
                string 藥碼 = key;
                string 藥名 = "";
              
                if (keyValuePairs_消耗帳.ContainsKey(藥碼))
                {
                
                    int 消耗量 = 0;
                    List<object[]> list_temp = keyValuePairs_消耗帳[藥碼];
                    for (int i = 0; i < list_temp.Count; i++)
                    {
                        if (藥名.StringIsEmpty()) 藥名 = list_temp[i][(int)enum_過帳明細.藥品名稱].ObjectToString();
                        int temp = list_temp[i][(int)enum_過帳明細.異動量].StringToInt32();
                        消耗量 += temp;
                    }
                    消耗量 = 消耗量 * -1;
                    Console.WriteLine($"({index}/{keyValuePairs_消耗帳.Keys.Count}) - ({藥碼}){藥名}".StringLength(80) + "消耗帳計算...");

                    class_MedPrices_buf = (from temp in class_MedPrices
                                           where temp.藥品碼 == 藥碼
                                           select temp).ToList();
                    if (class_MedPrices_buf.Count == 0)
                    {
                        Logger.Log($"藥碼 : {藥碼} ,找無售價資料");
                        index++;
                        continue;

                    }

                    double 成本價 = class_MedPrices_buf[0].成本價.StringToDouble();
                    double 總金額 = 消耗量 * 成本價;

                    object[] value = new object[new enum_ABC報表().GetLength()];
                    value[(int)enum_ABC報表.藥碼] = 藥碼;
                    value[(int)enum_ABC報表.藥名] = 藥名;
                    value[(int)enum_ABC報表.消耗量] = 消耗量;
                    value[(int)enum_ABC報表.成本價] = 成本價;
                    value[(int)enum_ABC報表.總金額] = 總金額;
                    list_ABC.Add(value);
                    index++;
                }

          

            }
            list_ABC.Sort(new ICP_ABC報表_總金額_降序());
            int A_num = (list_ABC.Count / 100) * 10;
            int B_num = (list_ABC.Count / 100) * 30;
            int C_num = (list_ABC.Count / 100) * 100;

            int A_total = 0;
            int B_total = 0;
            int C_total = 0;
            for (int i = 0; i < list_ABC.Count; i++)
            {
                double 基準量 = 0;
                double 安全量 = 0;
                double 消耗量 = 0;
                if (i <= A_num)
                {
                    消耗量 = list_ABC[i][(int)enum_ABC報表.消耗量].ObjectToString().StringToDouble();
                    基準量 = (消耗量 / 90) * 16;
                    安全量 = 基準量 / 2;
                    基準量 = Math.Round(基準量);
                    安全量 = Math.Round(安全量);
                    list_ABC[i][(int)enum_ABC報表.基準量] = 基準量;
                    list_ABC[i][(int)enum_ABC報表.安全量] = 安全量;
                    list_ABC[i][(int)enum_ABC報表.ABC類] = "A";
                    A_total++;
                }
                else if (i > A_num && i <= B_num)
                {
                    消耗量 = list_ABC[i][(int)enum_ABC報表.消耗量].ObjectToString().StringToDouble();
                    基準量 = (消耗量 / 90) * 32;
                    安全量 = 基準量 / 2;
                    基準量 = Math.Round(基準量);
                    安全量 = Math.Round(安全量);
                    list_ABC[i][(int)enum_ABC報表.基準量] = 基準量;
                    list_ABC[i][(int)enum_ABC報表.安全量] = 安全量;
                    list_ABC[i][(int)enum_ABC報表.ABC類] = "B";
                    B_total++;
                }
                else
                {
                    消耗量 = list_ABC[i][(int)enum_ABC報表.消耗量].ObjectToString().StringToDouble();
                    基準量 = (消耗量 / 90) * 38;
                    安全量 = 基準量 / 2;
                    基準量 = Math.Round(基準量);
                    安全量 = Math.Round(安全量);
                    list_ABC[i][(int)enum_ABC報表.基準量] = 基準量;
                    list_ABC[i][(int)enum_ABC報表.安全量] = 安全量;
                    list_ABC[i][(int)enum_ABC報表.ABC類] = "C";
                    C_total++;
                }
            }
            Logger.Log($"ABC類分類完成,全部<{list_ABC.Count}>種藥品,A類共<{A_total}>種藥品,B類共<{B_total}>種藥品,C類共<{C_total}>種藥品");
            System.Data.DataTable dataTable = list_ABC.ToDataTable(new enum_ABC報表());
            string filename = $@"{currentDirectory}\abc_excel\ABC_tabel({dateTime_st.ToDateString("")}_{dateTime_end.ToDateString("")}).xlsx";
            MyOffice.ExcelClass.NPOI_SaveFile(dataTable, filename, new int[] { (int)enum_ABC報表.基準量, (int)enum_ABC報表.安全量, (int)enum_ABC報表.成本價, (int)enum_ABC報表.消耗量, (int)enum_ABC報表.總金額 });
            Logger.LogAddLine();
        }

        public class ICP_ABC報表_總金額_降序 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                double temp0 = x[(int)enum_ABC報表.總金額].ObjectToString().StringToDouble();
                double temp1 = y[(int)enum_ABC報表.總金額].ObjectToString().StringToDouble();

                return temp1.CompareTo(temp0);

            }
        }
    }
}
