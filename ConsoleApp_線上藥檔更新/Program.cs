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

namespace ConsoleApp_線上藥檔更新
{
    class Program
    {
        public enum enum_本地_藥品資料
        {
            GUID,
            藥品碼,
            中文名稱,
            藥品名稱,
            藥品學名,
            藥品群組,
            健保碼,
            包裝單位,
            包裝數量,
            最小包裝單位,
            最小包裝數量,
            藥品條碼1,
            藥品條碼2,
        }


        static private string API_Server = "http://127.0.0.1:4433";

        static void Main(string[] args)
        {
            MyTimerBasic myTimer = new MyTimerBasic();
            myTimer.StartTickTime(50000);
            Logger.LogAddLine();
            try
            {
                List<medClass> medClasses_temp = new List<medClass>();
                List<medClass> medClasses_temp_buf = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(API_Server);
                Dictionary<string, List<medClass>> keyValuePairs_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<medClass> medClasses_cloud_replace = new List<medClass>();
                List<medClass> medClasses_cloud_add = new List<medClass>();
                List<medClass> medClasses_cloud_buf = new List<medClass>();
                String MyDb2ConnectionString = "server=DBGW1.VGHKS.GOV.TW:50000;database=DBDSNP;uid=APUD07;pwd=UD07AP;";
                IBM.Data.DB2.DB2Connection MyDb2Connection = new IBM.Data.DB2.DB2Connection(MyDb2ConnectionString);
                Logger.Log($"開啟DB2連線,({MyDb2ConnectionString})");
                MyDb2Connection.Open();
                Logger.Log($"DB2連線成功,耗時{myTimer.ToString()}");
                IBM.Data.DB2.DB2Command MyDB2Command = MyDb2Connection.CreateCommand();
                //MyDB2Command.CommandText = "SELECT A.UDCONVER ,A.UDUNFORM, A.UDRPNAME,A.UDSCNAME,A.UDSTOKNO,A.UDCHTNAM,A.UDDRGNO,B.UDPRDNAM FROM UD.UDDRGVWA A LEFT OUTER JOIN UD.UDPRDPF B ON A.UDDRGNO = B.UDDRGNO AND A.HID = B.HID WHERE A.HID = '2A0' WITH UR";
                MyDB2Command.CommandText = "SELECT * FROM UD.UDPRDPF P JOIN UD.UDDRGVWA V ON P.UDDRGNO = V.UDDRGNO AND P.HID = V.HID WHERE P.HID = '2A0'";

                var reader = MyDB2Command.ExecuteReader();
                DataTable schemaTable = reader.GetSchemaTable();
                foreach (DataRow row in schemaTable.Rows)
                {
                    string columnName = row["ColumnName"].ToString();
                    Console.WriteLine(columnName);
                }
                Logger.Log($"取得DB2資料,耗時{myTimer.ToString()}");
                int index = 0;
                while (reader.Read())
                {
                    string 藥碼 = "";
                    藥碼 = reader["UDSTOKNO"].ToString().Trim();
                    if (藥碼.Length >= 5)
                    {
                        藥碼 = 藥碼.Substring(藥碼.Length - 5);
                    }
                    else
                    {
                        藥碼 = "";
                    }
                    藥碼 = reader["UDDRGNO"].ToString().Trim();
                    string test = reader["UDCTCCTL"].ToString().Trim(); 
                    Console.WriteLine($"({index}) 取得藥品資料({藥碼})");
            
                    if (藥碼.StringIsEmpty() == false)
                    {
                        medClass medClass = new medClass();
                        medClass.藥品碼 = 藥碼;
                        medClass.藥品名稱 = reader["UDRPNAME"].ToString().Trim();
                        medClass.藥品學名 = reader["UDPRDNAM"].ToString().Trim();
                        medClass.中文名稱 = reader["UDCHTNAM"].ToString().Trim();
                        medClass.包裝單位 = reader["UDUNFORM"].ToString().Trim();
                        medClass.包裝數量 = reader["UDCONVER"].ToString().Trim();

                        string UDABSCTL = reader["UDABSCTL"].ToString().Trim();
                        string AROUTFLA = reader["AROUTFLA"].ToString().Trim();

                        if (UDABSCTL == "Y" && AROUTFLA == "Y")
                        {
                            medClass.開檔狀態 = "未開檔";
                        }
                        else
                        {
                            medClass.開檔狀態 = "開檔中";
                        }

                        string UDSTOCK = reader["UDSTOCK"].ToString().Trim();
                        if (UDSTOCK == "1" || UDSTOCK == "2" || UDSTOCK == "3" || UDSTOCK == "4")
                        {
                            medClass.管制級別 = UDSTOCK;
                        }
                        else
                        {
                            medClass.管制級別 = "";
                        }
                        medClasses_temp.Add(medClass);
                    }
                    index++;
                }
                MyDb2Connection.Close();
                Dictionary<string, List<medClass>> keyValuePairs_temp = medClasses_temp.CoverToDictionaryByCode();

                for (int i = 0; i < medClasses_temp.Count; i++)
                {
                    medClasses_cloud_buf = keyValuePairs_cloud.SortDictionaryByCode(medClasses_temp[i].藥品碼);
                    if (medClasses_cloud_buf.Count > 0)
                    {
                        if (medClasses_temp[i].藥品碼 == "04330")
                        {
                            medClasses_temp[i].開檔狀態 = "開檔中";
                        }
                        if (medClasses_temp[i].藥品碼 == "21371")
                        {
                            medClasses_temp[i].開檔狀態 = "開檔中";
                        }
                        bool flag_replace = false;
                        if (medClasses_cloud_buf[0].藥品碼 != medClasses_temp[i].藥品碼) flag_replace = true;
                        if (medClasses_cloud_buf[0].藥品名稱 != medClasses_temp[i].藥品名稱) flag_replace = true;
                        if (medClasses_cloud_buf[0].藥品學名 != medClasses_temp[i].藥品學名) flag_replace = true;
                        if (medClasses_cloud_buf[0].中文名稱 != medClasses_temp[i].中文名稱) flag_replace = true;
                        if (medClasses_cloud_buf[0].包裝單位 != medClasses_temp[i].包裝單位) flag_replace = true;
                        if (medClasses_cloud_buf[0].管制級別 != medClasses_temp[i].管制級別) flag_replace = true;
                        if (medClasses_cloud_buf[0].開檔狀態 != medClasses_temp[i].開檔狀態) flag_replace = true;

                        medClasses_cloud_buf[0].藥品碼 = medClasses_temp[i].藥品碼;
                        medClasses_cloud_buf[0].藥品名稱 = medClasses_temp[i].藥品名稱;
                        medClasses_cloud_buf[0].藥品學名 = medClasses_temp[i].藥品學名;
                        medClasses_cloud_buf[0].中文名稱 = medClasses_temp[i].中文名稱;
                        medClasses_cloud_buf[0].包裝單位 = medClasses_temp[i].包裝單位;
                        medClasses_cloud_buf[0].管制級別 = medClasses_temp[i].管制級別;
                        medClasses_cloud_buf[0].開檔狀態 = medClasses_temp[i].開檔狀態;
                        if (flag_replace)
                        {
                            Logger.Log($"(更新) ({medClasses_temp[i].藥品碼}){ medClasses_temp[i].藥品名稱}");
                            medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                        }
                    }
                    else
                    {
                        Logger.Log($"(新增) ({medClasses_temp[i].藥品碼}){ medClasses_temp[i].藥品名稱}");
                        medClasses_temp[i].GUID = Guid.NewGuid().ToString();
                        medClasses_cloud_add.Add(medClasses_temp[i]);
                    }
                }
                index = 0;
                for (int i = 0; i < medClasses_cloud.Count; i++)
                {
                    medClasses_temp_buf = keyValuePairs_temp.SortDictionaryByCode(medClasses_cloud[i].藥品碼);
                    if (medClasses_temp_buf.Count == 0)
                    {
                        medClasses_cloud[i].開檔狀態 = "未開檔";
                        Logger.Log($"{index + 1}. (未開檔) ({medClasses_cloud[i].藥品碼}){ medClasses_cloud[i].藥品名稱}");
                        medClasses_cloud_replace.Add(medClasses_cloud[i]);
                        index++;

                    }
                }


                if (medClasses_cloud_add.Count > 0) medClass.add_med_clouds(API_Server, medClasses_cloud_add);
                if (medClasses_cloud_replace.Count > 0) medClass.update_med_clouds_by_guid(API_Server, medClasses_cloud_replace);
                Logger.Log($"雲端藥檔新增<{medClasses_cloud_add.Count}>筆,修改<{medClasses_cloud_replace.Count}>筆");
                Function_本地藥檔更新();
            }
            catch(Exception ex)
            {
                Logger.Log($"Exception : {ex.Message}");
                Console.ReadKey();
            }
            finally
            {
                Logger.LogAddLine();
            }



        }
        static private List<object[]> Function_本地_藥品資料_列出DC藥品(List<object[]> list_雲端藥檔, List<object[]> list_本地藥檔)
        {

            List<object[]> list_value = new List<object[]>();

            Parallel.ForEach(list_本地藥檔, value =>
            {
                List<object[]> list_雲端藥檔_buf = new List<object[]>();
                List<object[]> list_本地藥檔_buf = new List<object[]>();

                list_雲端藥檔_buf = list_雲端藥檔.GetRows((int)enum_雲端藥檔.藥品碼, value[(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_雲端藥檔_buf.Count == 0)
                {
                    list_value.LockAdd(value);
                }
            });


            return list_value;
        }
        static private List<object[]> Function_本地_藥品資料_列出異動藥品(List<object[]> list_雲端藥檔, List<object[]> list_本地藥檔)
        {

            List<object[]> list_本地藥檔_buf = new List<object[]>();
            Parallel.ForEach(list_本地藥檔, value =>
            {
                List<object[]> list_雲端藥檔_buf = new List<object[]>();

                list_雲端藥檔_buf = list_雲端藥檔.GetRows((int)enum_雲端藥檔.藥品碼, value[(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_雲端藥檔_buf.Count != 0)
                {

                    bool flag_IsEqual = true;
                    object[] value_dst = LINQ.CopyRow(list_雲端藥檔_buf[0], new enum_雲端藥檔(), new enum_本地_藥品資料());
                    string src_藥品名稱 = value[(int)enum_本地_藥品資料.藥品名稱].ObjectToString();
                    string src_中文名稱 = value[(int)enum_本地_藥品資料.中文名稱].ObjectToString();
                    string src_藥品學名 = value[(int)enum_本地_藥品資料.藥品學名].ObjectToString();
                    string src_包裝單位 = value[(int)enum_本地_藥品資料.包裝單位].ObjectToString();

                    string dst_藥品名稱 = value_dst[(int)enum_本地_藥品資料.藥品名稱].ObjectToString();
                    string dst_中文名稱 = value_dst[(int)enum_本地_藥品資料.中文名稱].ObjectToString();
                    string dst_藥品學名 = value_dst[(int)enum_本地_藥品資料.藥品學名].ObjectToString();
                    string dst_包裝單位 = value_dst[(int)enum_本地_藥品資料.包裝單位].ObjectToString();

                    if (src_藥品名稱 != dst_藥品名稱) flag_IsEqual = false;
                    if (src_中文名稱 != dst_中文名稱) flag_IsEqual = false;
                    if (src_藥品學名 != dst_藥品學名) flag_IsEqual = false;
                    if (src_包裝單位 != dst_包裝單位) flag_IsEqual = false;
                    if (!flag_IsEqual)
                    {
                        value[(int)enum_本地_藥品資料.藥品名稱] = list_雲端藥檔_buf[0][(int)enum_雲端藥檔.藥品名稱];
                        value[(int)enum_本地_藥品資料.中文名稱] = list_雲端藥檔_buf[0][(int)enum_雲端藥檔.中文名稱];
                        value[(int)enum_本地_藥品資料.藥品學名] = list_雲端藥檔_buf[0][(int)enum_雲端藥檔.藥品學名];
                        value[(int)enum_本地_藥品資料.包裝單位] = list_雲端藥檔_buf[0][(int)enum_雲端藥檔.包裝單位];

                        list_本地藥檔_buf.LockAdd(value);
                    }
                }
            });

            return list_本地藥檔_buf;
        }
        static private List<object[]> Function_本地_藥品資料_列出新增藥品(List<object[]> list_雲端藥檔, List<object[]> list_本地藥檔)
        {

            List<object[]> list_雲端藥檔_新增藥品 = new List<object[]>();
            List<object[]> list_本地藥檔_新增藥品 = new List<object[]>();

            Parallel.ForEach(list_雲端藥檔, value =>
            {
                if (value == null)
                {

                }
                List<object[]> list_雲端藥檔_buf = new List<object[]>();
                List<object[]> list_本地藥檔_buf = new List<object[]>();
                list_本地藥檔_buf = list_本地藥檔.GetRows((int)enum_本地_藥品資料.藥品碼, value[(int)enum_雲端藥檔.藥品碼].ObjectToString());
                if (list_本地藥檔_buf.Count == 0)
                {

                    list_雲端藥檔_新增藥品.LockAdd(value);
                }

            });


            for (int i = 0; i < list_雲端藥檔_新增藥品.Count; i++)
            {
                object[] value_dst = LINQ.CopyRow(list_雲端藥檔_新增藥品[i], new enum_雲端藥檔(), new enum_本地_藥品資料());
                list_本地藥檔_新增藥品.Add(value_dst);
            }
            return list_本地藥檔_新增藥品;

        }
        static void Function_本地藥檔更新()
        {
            Table table = new Table("medicine_page_local");
            table.Server = "127.0.0.1";
            table.Username = "user";
            table.Password = "66437068";
            table.DBName = "ds01";
            table.Port = "3306";
            SQLControl sQLControl_本地_藥品資料 = new SQLControl(table);

            List<medClass> medClasses_cloud = medClass.get_med_cloud(API_Server);

            List<object[]> list_雲端藥檔 = medClasses_cloud.ClassToSQL<medClass, enum_雲端藥檔>();
            List<object[]> list_本地藥檔 = sQLControl_本地_藥品資料.GetAllRows(null);
            List<object[]> list_新增藥品 = Function_本地_藥品資料_列出新增藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_異動藥品 = Function_本地_藥品資料_列出異動藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_DC藥品 = Function_本地_藥品資料_列出DC藥品(list_雲端藥檔, list_本地藥檔);

            List<object[]> list_新增藥品_buf = new List<object[]>();
            List<object[]> list_異動藥品_buf = new List<object[]>();
            List<object[]> list_DC藥品_buf = new List<object[]>();

            List<object[]> list_AddValue_buf = new List<object[]>();
            List<object[]> list_ReplaceValue_buf = new List<object[]>();
            List<object[]> list_Delete_buf = new List<object[]>();

            List<object[]> list_value = list_雲端藥檔;

            for (int i = 0; i < list_value.Count; i++)
            {
                string Code = list_value[i][(int)enum_本地_藥品資料.藥品碼].ObjectToString();
      
                list_新增藥品_buf = list_新增藥品.GetRows((int)enum_本地_藥品資料.藥品碼, list_value[i][(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_新增藥品_buf.Count == 1)
                {
                    list_AddValue_buf.Add(list_新增藥品_buf[0]);
                    continue;
                }
                list_異動藥品_buf = list_異動藥品.GetRows((int)enum_本地_藥品資料.藥品碼, list_value[i][(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_異動藥品_buf.Count > 0)
                {
                    list_ReplaceValue_buf.Add(list_異動藥品_buf[0]);
                    continue;
                }

                list_DC藥品_buf = list_DC藥品.GetRows((int)enum_本地_藥品資料.藥品碼, list_value[i][(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_DC藥品_buf.Count > 0)
                {
                    list_Delete_buf.Add(list_DC藥品_buf[0]);
                    continue;
                }
            }
            for (int i = 0; i < list_AddValue_buf.Count; i++)
            {
                list_AddValue_buf[i][(int)enum_本地_藥品資料.GUID] = Guid.NewGuid().ToString();
            }
            sQLControl_本地_藥品資料.AddRows(null ,list_AddValue_buf);
            sQLControl_本地_藥品資料.UpdateByDefulteExtra(null, list_ReplaceValue_buf);
            sQLControl_本地_藥品資料.DeleteExtra(null, list_Delete_buf);
        }
    }
}
