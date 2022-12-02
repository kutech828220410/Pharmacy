using Basic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SQLUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using H_Pannel_lib;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace 智慧調劑台管理系統_WebApi
{
    public class class_medicine_page_firstclass_data
    {
        [JsonPropertyName("code")]
        public string 藥品碼 { get; set; }
        [JsonPropertyName("chinese_name")]
        public string 藥品中文名稱 { get; set; }
        [JsonPropertyName("name")]
        public string 藥品名稱 { get; set; }
        [JsonPropertyName("scientific_name")]
        public string 藥品學名 { get; set; }
        [JsonPropertyName("group")]
        public string 藥品群組 { get; set; }
        [JsonPropertyName("health_insurance_code")]
        public string 健保碼 { get; set; }
        [JsonPropertyName("barcode")]
        public string 藥品條碼 { get; set; }
        [JsonPropertyName("package")]
        public string 包裝單位 { get; set; }
        [JsonPropertyName("inventory")]
        public string 庫存 { get; set; }
        [JsonPropertyName("safe_inventory")]
        public string 安全庫存 { get; set; }
        [JsonPropertyName("isWarnning")]
        public string 警訊藥品 { get; set; }
    }
    public enum enum_medicine_page_firstclass
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
        庫存,
        基準量,
        安全庫存,
        藥品條碼1,
        藥品條碼2,
        狀態,
    }
    [Route("api/[controller]")]
    [ApiController]
    public class medicine_pageController : ControllerBase
    {

        static private string DataBaseName = ConfigurationManager.AppSettings["database"];
        static private string UserName = ConfigurationManager.AppSettings["user"];
        static private string Password =  ConfigurationManager.AppSettings["password"];
        static private string IP = ConfigurationManager.AppSettings["IP"];
        static private uint Port = (uint)ConfigurationManager.AppSettings["port"].StringToInt32();
        static private MySqlSslMode SSLMode = MySqlSslMode.None;

        private SQLControl sQLControl_WT32_serialize = new SQLControl(IP, DataBaseName, "wt32_jsonstring", UserName, Password, Port, SSLMode);
        private SQLControl sQLControl_sd0_phar_serialize = new SQLControl(IP, DataBaseName, "sd0_phar_device_jsonstring", UserName, Password, Port, SSLMode);

        private SQLControl sQLControl_medicine_page_firstclass = new SQLControl(IP, DataBaseName, "medicine_page_firstclass", UserName,Password, Port, SSLMode);
        private SQLControl sQLControl_medicine_group = new SQLControl(IP, DataBaseName, "medicine_group", UserName, Password, Port, SSLMode);


        #region storage_type
        [Route("storage_type")]
        [HttpGet()]
        public string Get_storage_type()
        {
            string jsonString = "";
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };

            string[] enums = new DeviceType().GetEnumNames();
            List<string> list_str = new List<string>();
            for (int i = 0; i < enums.Length; i++)
            {
                list_str.Add(enums[i]);
            }
            jsonString = JsonSerializer.Serialize<List<string>>(list_str, options);
            return jsonString;
        }
        #endregion
        #region storage_list
        public class class_儲位總庫存表
        {
            [JsonPropertyName("storage_name")]
            public string 儲位名稱 { get; set; }
            [JsonPropertyName("Code")]
            public string 藥品碼 { get; set; }
            [JsonPropertyName("Neme")]
            public string 藥品名稱 { get; set; }
            [JsonPropertyName("package")]
            public string 單位 { get; set; }
            [JsonPropertyName("inventory")]
            public string 庫存 { get; set; }
            [JsonPropertyName("storage_type")]
            public string 儲位型式 { get; set; }

        }
        public enum enum_儲位總庫存表 : int
        {
            儲位名稱,
            藥品碼,
            藥品名稱,
            單位,
            庫存,
            儲位型式,
            IP,
        }

        [Route("storage_list")]
        [HttpGet()]
        public string Get_storage_list(string? code, string? name)
        {

            string jsonString = "";
            this.Function_讀取儲位();
            List<object[]> list_藥品資料_儲位總庫存表_value = new List<object[]>();
            List<object[]> list_藥品資料 = this.sQLControl_medicine_page_firstclass.GetAllRows(null);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            for (int i = 0; i < devices.Count; i++)
            {
                object[] value = new object[new enum_儲位總庫存表().GetLength()];

                value[(int)enum_儲位總庫存表.儲位名稱] = devices[i].StorageName;
                value[(int)enum_儲位總庫存表.藥品碼] = devices[i].Code;
                value[(int)enum_儲位總庫存表.庫存] = devices[i].Inventory;
                value[(int)enum_儲位總庫存表.儲位型式] = devices[i].DeviceType.GetEnumName();
                value[(int)enum_儲位總庫存表.IP] = devices[i].IP;
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_medicine_page_firstclass.藥品碼, devices[i].Code);
                if (list_藥品資料_buf.Count == 0) continue;
                value[(int)enum_儲位總庫存表.藥品名稱] = list_藥品資料_buf[0][(int)enum_medicine_page_firstclass.藥品名稱].ObjectToString();
                value[(int)enum_儲位總庫存表.單位] = list_藥品資料_buf[0][(int)enum_medicine_page_firstclass.包裝單位].ObjectToString();
     
                list_藥品資料_儲位總庫存表_value.Add(value);
            }

            if (code != null)
            {
                list_藥品資料_儲位總庫存表_value = list_藥品資料_儲位總庫存表_value.GetRows((int)enum_儲位效期表.藥品碼, code);
            }
            if (name != null)
            {
                list_藥品資料_儲位總庫存表_value = list_藥品資料_儲位總庫存表_value.GetRowsByLike((int)enum_儲位效期表.藥品名稱, name);
            }

            list_藥品資料_儲位總庫存表_value = list_藥品資料_儲位總庫存表_value.OrderBy(r => r[(int)enum_儲位總庫存表.儲位名稱].ObjectToString()).ToList();
            List<class_儲位總庫存表> list_out_value = new List<class_儲位總庫存表>();
            for (int i = 0; i < list_藥品資料_儲位總庫存表_value.Count; i++)
            {
                class_儲位總庫存表 Class_儲位總庫存表 = new class_儲位總庫存表();
                Class_儲位總庫存表.儲位名稱 = list_藥品資料_儲位總庫存表_value[i][(int)enum_儲位總庫存表.儲位名稱].ObjectToString();
                Class_儲位總庫存表.藥品碼 = list_藥品資料_儲位總庫存表_value[i][(int)enum_儲位總庫存表.藥品碼].ObjectToString();
                Class_儲位總庫存表.藥品名稱 = list_藥品資料_儲位總庫存表_value[i][(int)enum_儲位總庫存表.藥品名稱].ObjectToString();
                Class_儲位總庫存表.單位 = list_藥品資料_儲位總庫存表_value[i][(int)enum_儲位總庫存表.單位].ObjectToString();
                Class_儲位總庫存表.庫存 = list_藥品資料_儲位總庫存表_value[i][(int)enum_儲位總庫存表.庫存].ObjectToString();
                Class_儲位總庫存表.儲位型式 = list_藥品資料_儲位總庫存表_value[i][(int)enum_儲位總庫存表.儲位型式].ObjectToString();

                list_out_value.Add(Class_儲位總庫存表);

            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            jsonString = JsonSerializer.Serialize<List<class_儲位總庫存表>>(list_out_value, options);

            return jsonString;
        }
        #endregion
        #region storage_validity_date_list
        public class class_儲位效期表
        {
            [JsonPropertyName("storage_name")]
            public string 儲位名稱 { get; set; }
            [JsonPropertyName("Code")]
            public string 藥品碼 { get; set; }
            [JsonPropertyName("Neme")]
            public string 藥品名稱 { get; set; }
            [JsonPropertyName("package")]
            public string 單位 { get; set; }
            [JsonPropertyName("validity_date")]
            public string 效期 { get; set; }
            [JsonPropertyName("inventory")]
            public string 庫存 { get; set; }
            [JsonPropertyName("storage_type")]
            public string 儲位型式 { get; set; }

        }


        public enum enum_儲位效期表 : int
        {
            儲位名稱,
            藥品碼,
            藥品名稱,
            單位,
            效期,
            庫存,
            儲位型式,
            IP,
        }
       
        [Route("storage_validity_date_list")]
        [HttpGet()]
        public string Get_storage_validity_date_list(string? code, string? name)
        {
 
            string jsonString = "";
            this.Function_讀取儲位();
            List<object[]> list_藥品資料_儲位效期表_value = new List<object[]>();
            List<object[]> list_藥品資料 = this.sQLControl_medicine_page_firstclass.GetAllRows(null);
            List<object[]> list_藥品資料_buf = new List<object[]>();

            for (int i = 0; i < devices.Count; i++)
            {
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_medicine_page_firstclass.藥品碼, devices[i].Code);
                if (list_藥品資料_buf.Count == 0) continue;

                for (int k = 0; k < devices[i].List_Validity_period.Count; k++)
                {
                    object[] value = new object[new enum_儲位效期表().GetLength()];

                    value[(int)enum_儲位效期表.儲位名稱] = devices[i].StorageName;
                    value[(int)enum_儲位效期表.藥品碼] = devices[i].Code;
                    value[(int)enum_儲位效期表.庫存] = devices[i].Inventory;
                    value[(int)enum_儲位效期表.儲位型式] = devices[i].DeviceType.GetEnumName();
                    value[(int)enum_儲位效期表.IP] = devices[i].IP;
                    value[(int)enum_儲位效期表.庫存] = devices[i].List_Inventory[k].ToString();
                    value[(int)enum_儲位效期表.效期] = devices[i].List_Validity_period[k];
                    value[(int)enum_儲位效期表.藥品名稱] = list_藥品資料_buf[0][(int)enum_medicine_page_firstclass.藥品名稱].ObjectToString();
                    value[(int)enum_儲位效期表.單位] = list_藥品資料_buf[0][(int)enum_medicine_page_firstclass.包裝單位].ObjectToString();

           
                    list_藥品資料_儲位效期表_value.Add(value);
                }
            }




            if (code != null)
            {
                list_藥品資料_儲位效期表_value = list_藥品資料_儲位效期表_value.GetRows((int)enum_儲位效期表.藥品碼, code);
            }
            if (name != null)
            {
                list_藥品資料_儲位效期表_value = list_藥品資料_儲位效期表_value.GetRowsByLike((int)enum_儲位效期表.藥品名稱, name);
            }

            list_藥品資料_儲位效期表_value = list_藥品資料_儲位效期表_value.OrderBy(r => r[(int)enum_儲位效期表.儲位名稱].ObjectToString()).ToList();
            List<class_儲位效期表> list_out_value = new List<class_儲位效期表>();

            for (int i = 0; i < list_藥品資料_儲位效期表_value.Count; i++)
            {
                class_儲位效期表 Class_儲位效期表 = new class_儲位效期表();
                Class_儲位效期表.儲位名稱 = list_藥品資料_儲位效期表_value[i][(int)enum_儲位效期表.儲位名稱].ObjectToString();
                Class_儲位效期表.藥品碼 = list_藥品資料_儲位效期表_value[i][(int)enum_儲位效期表.藥品碼].ObjectToString();
                Class_儲位效期表.藥品名稱 = list_藥品資料_儲位效期表_value[i][(int)enum_儲位效期表.藥品名稱].ObjectToString();
                Class_儲位效期表.單位 = list_藥品資料_儲位效期表_value[i][(int)enum_儲位效期表.單位].ObjectToString();
                Class_儲位效期表.庫存 = list_藥品資料_儲位效期表_value[i][(int)enum_儲位效期表.庫存].ObjectToString();
                Class_儲位效期表.效期 = list_藥品資料_儲位效期表_value[i][(int)enum_儲位效期表.效期].ObjectToString();
                Class_儲位效期表.儲位型式 = list_藥品資料_儲位效期表_value[i][(int)enum_儲位效期表.儲位型式].ObjectToString();

                list_out_value.Add(Class_儲位效期表);

            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            jsonString = JsonSerializer.Serialize<List<class_儲位效期表>>(list_out_value, options);

            return jsonString;
        }
        #endregion
        #region medicine_group
        public class Class_medicine_group
        {
            [JsonPropertyName("GUID")]
            public string GUID { get; set; }
            [JsonPropertyName("SN")]
            public string 群組序號 { get; set; }
            [JsonPropertyName("Neme")]
            public string 群組名稱 { get; set; }


        }
        public enum enum_medicine_group : int
        {
            GUID,
            群組序號,
            群組名稱,
        }
        [Route("medicine_group")]
        [HttpGet()]
        public string Get_medicine_group(string? sn, string? name)
        {
            List<object[]> list_value = sQLControl_medicine_group.GetAllRows(null);
            List<Class_medicine_group> list_out_value = new List<Class_medicine_group>();
            if (sn.StringIsEmpty() == false)
            {
                list_value.GetRows((int)enum_medicine_group.群組序號, sn);
            }
            if (name.StringIsEmpty() == false)
            {
                list_value.GetRowsByLike((int)enum_medicine_group.群組名稱, name);
            }
            for (int i = 0; i < list_value.Count; i++)
            {
                Class_medicine_group class_Medicine_Group = new Class_medicine_group();
                class_Medicine_Group.GUID = list_value[i][(int)enum_medicine_group.GUID].ObjectToString();
                class_Medicine_Group.群組序號 = list_value[i][(int)enum_medicine_group.群組序號].ObjectToString();
                class_Medicine_Group.群組名稱 = list_value[i][(int)enum_medicine_group.群組名稱].ObjectToString();
                list_out_value.Add(class_Medicine_Group);
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            list_out_value.Sort(new Icp_medicine_group());
            
            string jsonString = JsonSerializer.Serialize<List<Class_medicine_group>>(list_out_value, options);
            return jsonString;
        }
        [Route("medicine_group")]
        [HttpPost]
        public string Post_medicine_group([FromBody] Class_medicine_group data)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                string jsonString = JsonSerializer.Serialize<Class_medicine_group>(data, options);
            }
            catch
            {
                return "-1";
            }

            if (sQLControl_medicine_group.GetRowsByDefult(null, enum_medicine_group.GUID.GetEnumName(), data.GUID).Count > 0)
            {
                object[] value = new object[new enum_medicine_group().GetLength()];
                value[(int)enum_medicine_group.GUID] = data.GUID;
                value[(int)enum_medicine_group.群組名稱] = data.群組名稱;
                value[(int)enum_medicine_group.群組序號] = data.群組序號;
                sQLControl_medicine_group.UpdateByDefult(null, enum_medicine_group.GUID.GetEnumName(), data.GUID, value);
                return "200";
            }
            return "-5";

        }

        public class Icp_medicine_group : IComparer<Class_medicine_group>
        {
            public int Compare(Class_medicine_group x, Class_medicine_group y)
            {
                return x.群組序號.CompareTo(y.群組序號);
            }
        }
        #endregion

        // GET: api/<medicine_page_firstclassController>
        [HttpGet]
        public string Get(string code, string? package, string? type)
        {
            List<object[]> list_value = sQLControl_medicine_page_firstclass.GetAllRows(null);
            List<object[]> list_group = sQLControl_medicine_group.GetAllRows(null);
            List<object[]> list_group_buf = new List<object[]>();
            List<class_medicine_page_firstclass_data> list_out_value = new List<class_medicine_page_firstclass_data>();
 

            if (list_value.Count > 0)
            {             
                this.Function_讀取儲位();

                if (!code.StringIsEmpty())
                {
                    list_value = list_value.GetRows((int)enum_medicine_page_firstclass.藥品碼, code);
                }
                if (!package.StringIsEmpty())
                {
                    list_value = list_value.GetRows((int)enum_medicine_page_firstclass.包裝單位, package);
                }
                if (!type.StringIsEmpty())
                {
                    list_value = list_value.GetRows((int)enum_medicine_page_firstclass.藥品群組, type);
                }
          
                for (int i = 0; i < list_value.Count; i++)
                {
                    class_medicine_page_firstclass_data class_medicine_page_firstclass_Data = new class_medicine_page_firstclass_data();
                    class_medicine_page_firstclass_Data.藥品碼 = list_value[i][(int)enum_medicine_page_firstclass.藥品碼].ObjectToString();
                    class_medicine_page_firstclass_Data.藥品中文名稱 = list_value[i][(int)enum_medicine_page_firstclass.中文名稱].ObjectToString();
                    class_medicine_page_firstclass_Data.藥品名稱 = list_value[i][(int)enum_medicine_page_firstclass.藥品名稱].ObjectToString();
                    class_medicine_page_firstclass_Data.藥品學名 = list_value[i][(int)enum_medicine_page_firstclass.藥品學名].ObjectToString();
                    class_medicine_page_firstclass_Data.健保碼 = list_value[i][(int)enum_medicine_page_firstclass.健保碼].ObjectToString();
                    class_medicine_page_firstclass_Data.藥品條碼 = list_value[i][(int)enum_medicine_page_firstclass.藥品條碼1].ObjectToString();
                    class_medicine_page_firstclass_Data.包裝單位 = list_value[i][(int)enum_medicine_page_firstclass.包裝單位].ObjectToString();
                    class_medicine_page_firstclass_Data.庫存 = list_value[i][(int)enum_medicine_page_firstclass.庫存].ObjectToString();
                    class_medicine_page_firstclass_Data.安全庫存 = list_value[i][(int)enum_medicine_page_firstclass.安全庫存].ObjectToString();
                    class_medicine_page_firstclass_Data.警訊藥品 = false.ToString();

                    string 藥品群組Index = list_value[i][(int)enum_medicine_page_firstclass.藥品群組].ObjectToString();
                    list_group_buf = list_group.GetRows((int)enum_medicine_group.群組序號, 藥品群組Index);
                    if(list_group_buf.Count > 0)
                    {
                        class_medicine_page_firstclass_Data.藥品群組 = list_group_buf[0][(int)enum_medicine_group.群組名稱].ObjectToString();
                    }                   
                    class_medicine_page_firstclass_Data.庫存 = this.Function_取得儲位庫存(class_medicine_page_firstclass_Data.藥品碼).ToString();
                    list_out_value.Add(class_medicine_page_firstclass_Data);
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                string jsonString = JsonSerializer.Serialize<List<class_medicine_page_firstclass_data>>(list_out_value, options);


                return jsonString;
            }
            return null;
        }
        // POST api/<medicine_page_firstclassController>
        [HttpPost]
        public string Post([FromBody] class_medicine_page_firstclass_data data)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                string jsonString = JsonSerializer.Serialize<class_medicine_page_firstclass_data>(data, options);
            }
            catch
            {
                return "-1";
            }

            if (sQLControl_medicine_page_firstclass.GetRowsByDefult(null, enum_medicine_page_firstclass.藥品碼.GetEnumName(), data.藥品碼).Count > 0)
            {
                return "-2";
            }
            else
            {
                List<object[]> list_group = sQLControl_medicine_group.GetAllRows(null);
                list_group = list_group.GetRows((int)enum_medicine_group.群組名稱, data.藥品群組);
                if (list_group.Count > 0)
                {
                    data.藥品群組 = list_group[0][(int)enum_medicine_group.群組序號].ObjectToString();
                }
                else
                {
                    data.藥品群組 = "";
                }    

                object[] value = new object[new enum_medicine_page_firstclass().GetLength()];
                value[(int)enum_medicine_page_firstclass.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_medicine_page_firstclass.藥品碼] = data.藥品碼;
                value[(int)enum_medicine_page_firstclass.中文名稱] = data.藥品中文名稱;
                value[(int)enum_medicine_page_firstclass.藥品名稱] = data.藥品名稱;
                value[(int)enum_medicine_page_firstclass.藥品學名] = data.藥品學名;
                value[(int)enum_medicine_page_firstclass.藥品群組] = data.藥品群組;
                value[(int)enum_medicine_page_firstclass.健保碼] = data.健保碼;
                value[(int)enum_medicine_page_firstclass.包裝單位] = data.包裝單位;
                value[(int)enum_medicine_page_firstclass.藥品條碼1] = data.藥品條碼;
                value[(int)enum_medicine_page_firstclass.庫存] = data.庫存;
                value[(int)enum_medicine_page_firstclass.安全庫存] = data.安全庫存;

                sQLControl_medicine_page_firstclass.AddRow(null, value);
                return "200";
            }

        }
        // PUT api/<medicine_page_firstclassController>/5
        [HttpPut]
        public string Put([FromBody] class_medicine_page_firstclass_data data)
        {

            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                string jsonString = JsonSerializer.Serialize<class_medicine_page_firstclass_data>(data, options);
            }
            catch
            {
                return "-1";
            }

            List<object[]> list_value = sQLControl_medicine_page_firstclass.GetRowsByDefult(null, enum_medicine_page_firstclass.藥品碼.GetEnumName(), data.藥品碼);
            if (!(list_value.Count > 0))
            {
                return "-3";
            }
            else
            {
                object[] value = list_value[0];

                List<object[]> list_group = sQLControl_medicine_group.GetAllRows(null);
                list_group = list_group.GetRows((int)enum_medicine_group.群組名稱, data.藥品群組);
                if (list_group.Count > 0)
                {
                    data.藥品群組 = list_group[0][(int)enum_medicine_group.群組序號].ObjectToString();
                }
                else
                {
                    data.藥品群組 = "";
                }

                value[(int)enum_medicine_page_firstclass.藥品碼] = data.藥品碼;
                value[(int)enum_medicine_page_firstclass.中文名稱] = data.藥品中文名稱;
                value[(int)enum_medicine_page_firstclass.藥品名稱] = data.藥品名稱;
                value[(int)enum_medicine_page_firstclass.藥品學名] = data.藥品學名;
                value[(int)enum_medicine_page_firstclass.藥品群組] = data.藥品群組;
                value[(int)enum_medicine_page_firstclass.健保碼] = data.健保碼;
                value[(int)enum_medicine_page_firstclass.包裝單位] = data.包裝單位;
                value[(int)enum_medicine_page_firstclass.藥品條碼1] = data.藥品條碼;
                value[(int)enum_medicine_page_firstclass.庫存] = data.庫存;
                value[(int)enum_medicine_page_firstclass.安全庫存] = data.安全庫存;

                sQLControl_medicine_page_firstclass.UpdateByDefult(null, enum_medicine_page_firstclass.藥品碼.GetEnumName(), data.藥品碼, value);

                return "200";
            }


        }
        // DELETE api/<medicine_page_firstclassController>/5
        [HttpDelete]
        public string Delete([FromBody] class_medicine_page_firstclass_data data)
        {

            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                string jsonString = JsonSerializer.Serialize<class_medicine_page_firstclass_data>(data, options);
            }
            catch
            {
                return "-1";
            }
            if (!(sQLControl_medicine_page_firstclass.GetRowsByDefult(null, enum_medicine_page_firstclass.藥品碼.GetEnumName(), data.藥品碼).Count > 0))
            {
                return "-2";
            }
            else
            {
                sQLControl_medicine_page_firstclass.DeleteByDefult(null, enum_medicine_page_firstclass.藥品碼.GetEnumName(), data.藥品碼);
                return "200";
            }
        }


        #region Function
        List<DeviceBasic> devices = new List<DeviceBasic>();

        private void Function_讀取儲位()
        {

            List<object[]> list_WT32 = sQLControl_WT32_serialize.GetAllRows(null);
            List<object[]> list_sd0_phar = sQLControl_sd0_phar_serialize.GetAllRows(null);

            List<DeviceBasic> deviceBasics = DeviceBasicMethod.SQL_GetAllDeviceBasic(sQLControl_sd0_phar_serialize);
            List<Storage> pannel35s = StorageMethod.SQL_GetAllStorage(sQLControl_WT32_serialize);
  

            List<DeviceBasic> devices_pannel35s = pannel35s.GetAllDeviceBasic();


            for (int i = 0; i < devices_pannel35s.Count; i++)
            {
                if (devices_pannel35s[i].Code.StringIsEmpty() != true)
                {
                    this.devices.Add(devices_pannel35s[i]);
                }
            }
            for (int i = 0; i < deviceBasics.Count; i++)
            {
                if (deviceBasics[i].Code.StringIsEmpty() != true)
                {
                    this.devices.Add(deviceBasics[i]);
                }
            }   
        }
        private int Function_取得儲位庫存(string 藥品碼)
        {
            int 庫存 = 0;
            List<DeviceBasic> deviceBasics = (from value in devices
                                              where value.Code == 藥品碼
                                              select value).ToList();
            for (int k = 0; k < deviceBasics.Count; k++)
            {
                庫存 += deviceBasics[k].Inventory.StringToInt32();
            }
            return 庫存;
        }
        #endregion
    }
}
