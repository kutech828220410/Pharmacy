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
        static private string API_Server = "http://127.0.0.1:4433";

        static void Main(string[] args)
        {
            MyTimerBasic myTimer = new MyTimerBasic();
            myTimer.StartTickTime(50000);
            Logger.LogAddLine();
            try
            {
                List<medClass> medClasses_temp = new List<medClass>();
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
                        medClasses_temp.Add(medClass);
                    }
                    index++;
                }
                MyDb2Connection.Close();

                for (int i = 0; i < medClasses_temp.Count; i++)
                {
                    medClasses_cloud_buf = keyValuePairs_cloud.SortDictionaryByCode(medClasses_temp[i].藥品碼);
                    if (medClasses_cloud_buf.Count > 0)
                    {
                        bool flag_replace = false;
                        if (medClasses_cloud_buf[0].藥品碼 != medClasses_temp[i].藥品碼) flag_replace = true;
                        if (medClasses_cloud_buf[0].藥品名稱 != medClasses_temp[i].藥品名稱) flag_replace = true;
                        if (medClasses_cloud_buf[0].藥品學名 != medClasses_temp[i].藥品學名) flag_replace = true;
                        if (medClasses_cloud_buf[0].中文名稱 != medClasses_temp[i].中文名稱) flag_replace = true;
                        if (medClasses_cloud_buf[0].包裝單位 != medClasses_temp[i].包裝單位) flag_replace = true;

                        medClasses_cloud_buf[0].藥品碼 = medClasses_temp[i].藥品碼;
                        medClasses_cloud_buf[0].藥品名稱 = medClasses_temp[i].藥品名稱;
                        medClasses_cloud_buf[0].藥品學名 = medClasses_temp[i].藥品學名;
                        medClasses_cloud_buf[0].中文名稱 = medClasses_temp[i].中文名稱;
                        medClasses_cloud_buf[0].包裝單位 = medClasses_temp[i].包裝單位;
                       if(flag_replace) medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                    }
                    else
                    {
                        medClasses_temp[i].GUID = Guid.NewGuid().ToString();
                        medClasses_cloud_add.Add(medClasses_temp[i]);
                    }
                }
                if (medClasses_cloud_add.Count > 0) medClass.add_med_clouds(API_Server, medClasses_cloud_add);
                if (medClasses_cloud_replace.Count > 0) medClass.update_med_clouds_by_guid(API_Server, medClasses_cloud_replace);
                Logger.Log($"雲端藥檔新增<{medClasses_cloud_add.Count}>筆,修改<{medClasses_cloud_replace.Count}>筆");

                Console.ReadKey();
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
    }
}
