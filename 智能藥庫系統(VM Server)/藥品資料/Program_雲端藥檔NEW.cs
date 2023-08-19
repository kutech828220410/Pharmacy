using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using SQLUI;
using Basic;
using MyUI;
using IBM.Data.DB2;
using HIS_DB_Lib;
namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {
        private string API_URL = "http://10.18.1.146:4433";
        private void sub_Program_雲端藥檔NEW_Init()
        {

            string url = $"{API_URL}/api/MED_page/init";
            returnData returnData = new returnData();
            returnData.ServerType = enum_ServerSetting_Type.調劑台.GetEnumName();
            returnData.ServerName = $"{"口服1"}";
            returnData.TableName = "medicine_page_cloud";
            string json_in = returnData.JsonSerializationt();
            string json = Basic.Net.WEBApiPostJson($"{url}", json_in);
            Table table = json.JsonDeserializet<Table>();
            if (table == null)
            {
                MyMessageBox.ShowDialog($"雲端藥檔表單建立失敗!! Api_URL:{API_URL}");
                return;
            }
            SQLUI.SQL_DataGridView.ConnentionClass connentionClass = new SQL_DataGridView.ConnentionClass();
            connentionClass.DataBaseName = "dbvm_new";
            connentionClass.IP = "10.18.1.146";
            connentionClass.Password = "66437068";
            connentionClass.UserName = "user";
            connentionClass.Port = 3306;
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_雲端藥檔NEW, connentionClass);

            this.sqL_DataGridView_雲端藥檔NEW.Init(table);
            this.sqL_DataGridView_雲端藥檔NEW.Set_ColumnVisible(true, new enum_雲端藥檔().GetEnumNames());

            this.plC_RJ_Button_雲端藥檔NEW_顯示全部.MouseDownEvent += PlC_RJ_Button_雲端藥檔NEW_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_雲端藥檔NEW_更新資料.MouseDownEvent += PlC_RJ_Button_雲端藥檔NEW_更新資料_MouseDownEvent;

            this.plC_UI_Init.Add_Method(this.sub_Program_雲端藥檔NEW);
        } 

        private void sub_Program_雲端藥檔NEW()
        {

        }


        private void PlC_RJ_Button_雲端藥檔NEW_更新資料_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            try
            {
                String MyDb2ConnectionString = "server=DBGW1.VGHKS.GOV.TW:50000;database=DBDSNP;uid=APUD07;pwd=UD07AP;";
                IBM.Data.DB2.DB2Connection MyDb2Connection = new IBM.Data.DB2.DB2Connection(MyDb2ConnectionString);
                Console.Write($"開啟DB2連線....");
                MyDb2Connection.Open();
                Console.WriteLine($"DB2連線成功!耗時{myTimer.ToString()}ms");
                IBM.Data.DB2.DB2Command MyDB2Command = MyDb2Connection.CreateCommand();
                MyDB2Command.CommandText = "SELECT A.UDCONVER ,A.UDUNFORM, A.UDRPNAME,A.UDSCNAME,A.UDSTOKNO,A.UDCHTNAM,A.UDDRGNO,B.UDPRDNAM FROM UD.UDDRGVWA A LEFT OUTER JOIN UD.UDPRDPF B ON A.UDDRGNO = B.UDDRGNO AND A.HID = B.HID WHERE A.HID = '2A0' WITH UR";

                var reader = MyDB2Command.ExecuteReader();
                Console.WriteLine($"取得DB2資料!耗時{myTimer.ToString()}ms");
                int FieldCount = reader.FieldCount;
                List<object[]> obj_temp_array = new List<object[]>();
                List<object[]> obj_temp_array_buf = new List<object[]>();
                List<object[]> obj_temp_array_result = new List<object[]>();
                List<object> obj_temp = new List<object>();
                string UDSTOKNO = "";
                string UDDRGNO = "";

                while (reader.Read())
                {
                    obj_temp.Clear();
                    UDSTOKNO = reader["UDSTOKNO"].ToString().Trim();

                    if (UDSTOKNO.Length >= 5)
                    {
                        UDSTOKNO = UDSTOKNO.Substring(UDSTOKNO.Length - 5);
                    }
                    else
                    {
                        UDSTOKNO = "";
                    }
                    UDDRGNO = reader["UDDRGNO"].ToString().Trim();
                    if (UDDRGNO != "")
                    {

                        obj_temp.Add(UDDRGNO);
                        obj_temp.Add(reader["UDRPNAME"].ToString().Trim());
                        obj_temp.Add(reader["UDPRDNAM"].ToString().Trim());
                        obj_temp.Add(reader["UDCHTNAM"].ToString().Trim());
                        obj_temp.Add(reader["UDUNFORM"].ToString().Trim());
                        obj_temp.Add(reader["UDCONVER"].ToString().Trim());


                        obj_temp_array.Add(obj_temp.ToArray());
                    }
                }
                MyDb2Connection.Close();
                Console.WriteLine($"關閉連線!取得資料共{obj_temp_array.Count}筆,耗時{myTimer.ToString()}ms");

                for (int i = 0; i < obj_temp_array.Count; i++)
                {
                    string 藥品碼 = obj_temp_array[i][(int)enum_雲端_藥品資料_DB2.藥品碼].ObjectToString();
                    obj_temp_array_buf = obj_temp_array_result.GetRows((int)enum_雲端_藥品資料_DB2.藥品碼, 藥品碼);
                    if (obj_temp_array_buf.Count == 0)
                    {
                        obj_temp_array_result.Add(obj_temp_array[i]);
                    }
                }


                List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端藥檔NEW.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                List<object[]> list_藥品資料_add = new List<object[]>();
                List<object[]> list_藥品資料_replace = new List<object[]>();

                Console.WriteLine($"取得藥品資料!耗時{myTimer.ToString()}ms");

                for (int i = 0; i < obj_temp_array_result.Count; i++)
                {
                    string 藥品碼 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.藥品碼].ObjectToString();
                    string 藥品名稱 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.藥品名稱].ObjectToString();
                    string 藥品學名 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.藥品學名].ObjectToString();
                    string 中文名稱 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.中文名稱].ObjectToString();
                    string 包裝單位 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.包裝單位].ObjectToString();
                    string 包裝數量 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.包裝數量].ObjectToString();
                    string 最小包裝數量 = "1";
                    if (!包裝數量.StringIsInt32())
                    {
                        if (!包裝數量.StringIsEmpty())
                        {
                            包裝數量 = 包裝數量.Substring(0, 包裝數量.Length - 1);
                        }
                        else
                        {
                            包裝數量 = "1";
                        }
                    }
                    list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端藥檔.藥品碼, 藥品碼);
                    if (list_藥品資料_buf.Count == 0)
                    {
                        object[] value = new object[new enum_雲端藥檔().GetLength()];
                        value[(int)enum_雲端藥檔.GUID] = Guid.NewGuid().ToString();
                        value[(int)enum_雲端藥檔.藥品碼] = 藥品碼;
                        value[(int)enum_雲端藥檔.藥品名稱] = 藥品名稱;
                        value[(int)enum_雲端藥檔.藥品學名] = 藥品學名;
                        value[(int)enum_雲端藥檔.中文名稱] = 中文名稱;
                        value[(int)enum_雲端藥檔.包裝單位] = 包裝單位;
                        value[(int)enum_雲端藥檔.包裝數量] = 包裝數量;
                        value[(int)enum_雲端藥檔.最小包裝數量] = 最小包裝數量;

                        list_藥品資料_add.Add(value);
                    }
                    else if (list_藥品資料_buf.Count == 1)
                    {
                        bool replace = false;
                        if (list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品名稱].ObjectToString() != 藥品名稱)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品學名].ObjectToString() != 藥品學名)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端藥檔.中文名稱].ObjectToString() != 中文名稱)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端藥檔.包裝單位].ObjectToString() != 包裝單位)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端藥檔.包裝數量].ObjectToString() != 包裝數量)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端藥檔.最小包裝數量].ObjectToString() != 最小包裝數量)
                        {
                            replace = true;
                        }
                        list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品名稱] = 藥品名稱;
                        list_藥品資料_buf[0][(int)enum_雲端藥檔.藥品學名] = 藥品學名;
                        list_藥品資料_buf[0][(int)enum_雲端藥檔.中文名稱] = 中文名稱;
                        list_藥品資料_buf[0][(int)enum_雲端藥檔.包裝單位] = 包裝單位;
                        list_藥品資料_buf[0][(int)enum_雲端藥檔.包裝數量] = 包裝數量;
                        list_藥品資料_buf[0][(int)enum_雲端藥檔.最小包裝數量] = 最小包裝數量;

                        if (replace)
                        {
                            list_藥品資料_replace.Add(list_藥品資料_buf[0]);
                        }
                    }

                }
                Console.WriteLine($"檢查藥品資料!耗時{myTimer.ToString()}ms");
                this.sqL_DataGridView_雲端藥檔NEW.SQL_AddRows(list_藥品資料_add, false);
                Console.WriteLine($"新增藥品資料!共{list_藥品資料_add.Count}筆,耗時{myTimer.ToString()}ms");

                this.sqL_DataGridView_雲端藥檔NEW.SQL_ReplaceExtra(list_藥品資料_replace, false);
                Console.WriteLine($"修正藥品資料!共{list_藥品資料_replace.Count}筆,耗時{myTimer.ToString()}ms");
            }
            catch
            {

            }

        }
        private void PlC_RJ_Button_雲端藥檔NEW_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_雲端藥檔NEW.SQL_GetAllRows(true);
        }
    }
}
