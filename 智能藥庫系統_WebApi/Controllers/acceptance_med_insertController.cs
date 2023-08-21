﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SQLUI;
using Basic;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Configuration;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace 智慧藥庫系統_WebApi
{


    [Route("api/[controller]")]
    [ApiController]
    public class acceptance_med_insertController
    {
        enum enum_狀態
        {
            等待過帳,
            庫存不足,
            未建立儲位,
            過帳完成,
            找無此藥品,
            無效期可入帳,
        }

        static private string DataBaseName = ConfigurationManager.AppSettings["acceptance_med_database"];
        static private string UserName = ConfigurationManager.AppSettings["user"];
        static private string Password = ConfigurationManager.AppSettings["password"];
        static private string IP = ConfigurationManager.AppSettings["acceptance_med_IP"];
        static private uint Port = (uint)ConfigurationManager.AppSettings["port"].StringToInt32();
        static private MySqlSslMode SSLMode = MySqlSslMode.None;
        private SQLControl sQLControl_acceptance_med = new SQLControl(IP, DataBaseName, "acceptance_med", UserName, Password, Port, SSLMode);
        public class class_acceptance_med_data
        {
            [JsonPropertyName("PRN_SN")]
            public string 請購單號 { get; set; }
            [JsonPropertyName("ACP_SN")]
            public string 驗收單號 { get; set; }
            [JsonPropertyName("code")]
            public string 藥品碼 { get; set; }
            [JsonPropertyName("value")]
            public string 數量 { get; set; }
            [JsonPropertyName("validity_period")]
            public string 效期 { get; set; }
            [JsonPropertyName("lot_number")]
            public string 批號 { get; set; }
            [JsonPropertyName("acceptance_date")]
            public string 驗收時間 { get; set; }
            [JsonPropertyName("OP_TIME")]
            public string 加入時間 { get; set; }
            [JsonPropertyName("state")]
            public string 狀態 { get; set; }

        }
        public enum enum_acceptance_med
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
        public enum enum_acceptance_med_test
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
        private readonly Logger _logger;

        public acceptance_med_insertController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _logger = new Logger(env);
        }

        [Route("test")]
        [HttpGet]
        public string Get_test()
        {
            sQLControl_acceptance_med = new SQLControl(IP, DataBaseName, "acceptance_med_test", UserName, Password, Port, SSLMode);
            List<object[]> list_value = sQLControl_acceptance_med.GetAllRows(null);
            List<class_acceptance_med_data> list_class_acceptance_med_data = new List<class_acceptance_med_data>();

            list_value.Sort(new ICP_acceptance_med());

            for (int i = 0; i < list_value.Count; i++)
            {
                if (list_value[i][(int)enum_acceptance_med_test.來源].ObjectToString() != "測試資料") continue;
                class_acceptance_med_data _class_acceptance_med_data = new class_acceptance_med_data();
                _class_acceptance_med_data.請購單號 = list_value[i][(int)enum_acceptance_med_test.請購單號].ObjectToString();
                _class_acceptance_med_data.驗收單號 = list_value[i][(int)enum_acceptance_med_test.驗收單號].ObjectToString();
                _class_acceptance_med_data.藥品碼 = list_value[i][(int)enum_acceptance_med_test.藥品碼].ObjectToString();
                _class_acceptance_med_data.數量 = list_value[i][(int)enum_acceptance_med_test.數量].ObjectToString();
                _class_acceptance_med_data.效期 = list_value[i][(int)enum_acceptance_med_test.效期].ToDateString();
                _class_acceptance_med_data.批號 = list_value[i][(int)enum_acceptance_med_test.批號].ObjectToString();
                _class_acceptance_med_data.驗收時間 = list_value[i][(int)enum_acceptance_med_test.驗收時間].ToDateTimeString();
                _class_acceptance_med_data.加入時間 = list_value[i][(int)enum_acceptance_med_test.加入時間].ToDateTimeString();
                _class_acceptance_med_data.狀態 = list_value[i][(int)enum_acceptance_med_test.狀態].ObjectToString();
                list_class_acceptance_med_data.Add(_class_acceptance_med_data);
            }


            string jsonString = list_class_acceptance_med_data.JsonSerializationt(true);
            return jsonString;
        }
        [Route("test")]
        [HttpPost]
        public string Post_test([FromBody] class_acceptance_med_data data)
        {
            string str_out = "";
            if (data == null)
            {
                return "無資料可更動";
            }
            sQLControl_acceptance_med = new SQLControl(IP, DataBaseName, "acceptance_med_test", UserName, Password, Port, SSLMode);
            List<object[]> list_value = sQLControl_acceptance_med.GetAllRows(null);
            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_value_add = new List<object[]>();
            List<object[]> list_value_replace = new List<object[]>();
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                string jsonString = JsonSerializer.Serialize<class_acceptance_med_data>(data, options);
            }
            catch
            {
                return "JsonSerializer fail!";
            }

            //if (data.GUID.StringIsEmpty()) continue;
            if (data.藥品碼.StringIsEmpty()) return "資料錯誤";
            if (!data.數量.StringIsInt32()) return "資料錯誤";
            if (!data.效期.Check_Date_String()) return "資料錯誤";
            if (!data.驗收時間.Check_Date_String()) return "資料錯誤";
            list_value_buf = list_value.GetRows((int)enum_acceptance_med_test.藥品碼, data.藥品碼);
            list_value_buf = list_value_buf.GetRows((int)enum_acceptance_med_test.批號, data.批號);
            list_value_buf = (from value in list_value_buf
                              where value[(int)enum_acceptance_med_test.效期].ToDateString() == data.效期.StringToDateTime().ToDateString()
                              select value).ToList();
            list_value_buf = (from value in list_value_buf
                              where (DateTime)value[(int)enum_acceptance_med_test.加入時間] == data.加入時間.StringToDateTime()
                              select value).ToList();
            list_value_buf = (from value in list_value_buf
                              where value[(int)enum_acceptance_med_test.來源].ObjectToString() == "測試資料"
                              select value).ToList();

            if (list_value_buf.Count == 0)
            {
                object[] value = new object[new enum_acceptance_med_test().GetLength()];
                value[(int)enum_acceptance_med_test.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_acceptance_med_test.請購單號] = data.請購單號;
                value[(int)enum_acceptance_med_test.驗收單號] = data.驗收單號;
                value[(int)enum_acceptance_med_test.藥品碼] = data.藥品碼;
                value[(int)enum_acceptance_med_test.數量] = data.數量;
                value[(int)enum_acceptance_med_test.效期] = data.效期;
                value[(int)enum_acceptance_med_test.批號] = data.批號;
                value[(int)enum_acceptance_med_test.驗收時間] = data.驗收時間;
                DateTime dateTime = data.加入時間.StringToDateTime();
                value[(int)enum_acceptance_med_test.加入時間] = dateTime.ToDateTimeString_6();
                value[(int)enum_acceptance_med_test.狀態] = enum_狀態.等待過帳.GetEnumName();
                value[(int)enum_acceptance_med_test.來源] = "測試資料";
                value[(int)enum_acceptance_med_test.備註] = "";
                list_value_add.LockAdd(value);
            }
            else
            {
                string 狀態 = list_value_buf[0][(int)enum_acceptance_med_test.狀態].ObjectToString();
                if (狀態 != enum_狀態.過帳完成.GetEnumName())
                {
                    object[] value = new object[new enum_acceptance_med_test().GetLength()];
                    value[(int)enum_acceptance_med_test.GUID] = list_value_buf[0][(int)enum_acceptance_med_test.GUID];
                    value[(int)enum_acceptance_med_test.請購單號] = data.請購單號;
                    value[(int)enum_acceptance_med_test.驗收單號] = data.驗收單號;
                    value[(int)enum_acceptance_med_test.藥品碼] = data.藥品碼;
                    value[(int)enum_acceptance_med_test.數量] = data.數量;
                    value[(int)enum_acceptance_med_test.效期] = data.效期;
                    value[(int)enum_acceptance_med_test.批號] = data.批號;
                    value[(int)enum_acceptance_med_test.驗收時間] = data.驗收時間;
                    DateTime dateTime = data.加入時間.StringToDateTime();
                    value[(int)enum_acceptance_med_test.加入時間] = dateTime.ToDateTimeString_6();
                    value[(int)enum_acceptance_med_test.狀態] = list_value_buf[0][(int)enum_acceptance_med_test.狀態];
                    value[(int)enum_acceptance_med_test.來源] = "測試資料";
                    value[(int)enum_acceptance_med_test.備註] = "";

                    list_value_replace.LockAdd(value);
                }

            }
            sQLControl_acceptance_med.AddRows(null, list_value_add);
            sQLControl_acceptance_med.UpdateByDefulteExtra(null, list_value_replace);
            string result = "==========call acceptance_med begin==========\n";
            result += $"新增<{list_value_add.Count}筆,修改{list_value_replace.Count}>筆資料\n";
            result += $"jsonRecive:{data.JsonSerializationt()}\n";
            result += "==========call acceptance_med end==========\n";
            _logger.WriteLog("acceptance_med_test", result);
            return result;
        }

        [HttpGet]
        public string Get()
        {
            sQLControl_acceptance_med = new SQLControl(IP, DataBaseName, "acceptance_med", UserName, Password, Port, SSLMode);
            List<object[]> list_value = sQLControl_acceptance_med.GetAllRows(null);
            List<class_acceptance_med_data> list_class_acceptance_med_data = new List<class_acceptance_med_data>();
            List<object[]> list_value_replace = new List<object[]>();
            list_value.Sort(new ICP_acceptance_med());

            for (int i = 0; i < list_value.Count; i++)
            {
                //if (list_value[i][(int)enum_acceptance_med.來源].ObjectToString() == "測試資料")
                //{
                //    list_value[i][(int)enum_acceptance_med.來源] = "院內系統";
                //    list_value_replace.Add(list_value[i]);
                //    continue;
                //}
                class_acceptance_med_data _class_acceptance_med_data = new class_acceptance_med_data();
                _class_acceptance_med_data.請購單號 = list_value[i][(int)enum_acceptance_med.請購單號].ObjectToString();
                _class_acceptance_med_data.驗收單號 = list_value[i][(int)enum_acceptance_med.驗收單號].ObjectToString();
                _class_acceptance_med_data.藥品碼 = list_value[i][(int)enum_acceptance_med.藥品碼].ObjectToString();
                _class_acceptance_med_data.數量 = list_value[i][(int)enum_acceptance_med.數量].ObjectToString();
                _class_acceptance_med_data.效期 = list_value[i][(int)enum_acceptance_med.效期].ToDateString();
                _class_acceptance_med_data.批號 = list_value[i][(int)enum_acceptance_med.批號].ObjectToString();
                _class_acceptance_med_data.驗收時間 = list_value[i][(int)enum_acceptance_med.驗收時間].ToDateTimeString();
                _class_acceptance_med_data.加入時間 = list_value[i][(int)enum_acceptance_med.加入時間].ToDateTimeString();
                _class_acceptance_med_data.狀態 = list_value[i][(int)enum_acceptance_med.狀態].ObjectToString();
                list_class_acceptance_med_data.Add(_class_acceptance_med_data);
            }

            //sQLControl_acceptance_med.UpdateByDefulteExtra(null, list_value_replace);
            string jsonString = list_class_acceptance_med_data.JsonSerializationt(true);
            return jsonString;
        }
        [HttpPost]
        public string Post([FromBody] class_acceptance_med_data data)
        {
            string str_out = "";
            if (data == null)
            {
                return "無資料可更動";
            }
            sQLControl_acceptance_med = new SQLControl(IP, DataBaseName, "acceptance_med", UserName, Password, Port, SSLMode);
            List<object[]> list_value = sQLControl_acceptance_med.GetAllRows(null);
            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_value_add = new List<object[]>();
            List<object[]> list_value_replace = new List<object[]>();
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                string jsonString = JsonSerializer.Serialize<class_acceptance_med_data>(data, options);
            }
            catch
            {
                return "JsonSerializer fail!";
            }

            //if (data.GUID.StringIsEmpty()) continue;
            if (data.藥品碼.StringIsEmpty()) return "資料錯誤";
            if (!data.數量.StringIsInt32()) return "資料錯誤";
            if (!data.效期.Check_Date_String()) return "資料錯誤";
            if (!data.驗收時間.Check_Date_String()) return "資料錯誤";
            list_value_buf = list_value.GetRows((int)enum_acceptance_med.藥品碼, data.藥品碼);
            list_value_buf = list_value_buf.GetRows((int)enum_acceptance_med.批號, data.批號);
            list_value_buf = (from value in list_value_buf
                              where value[(int)enum_acceptance_med.效期].ToDateString() == data.效期.StringToDateTime().ToDateString()
                              select value).ToList();
            list_value_buf = (from value in list_value_buf
                              where (DateTime)value[(int)enum_acceptance_med.加入時間] == data.加入時間.StringToDateTime()
                              select value).ToList();
            list_value_buf = (from value in list_value_buf
                              where value[(int)enum_acceptance_med.來源].ObjectToString() == "院內系統"
                              select value).ToList();

            if (list_value_buf.Count == 0)
            {
                object[] value = new object[new enum_acceptance_med().GetLength()];
                value[(int)enum_acceptance_med.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_acceptance_med.請購單號] = data.請購單號;
                value[(int)enum_acceptance_med.驗收單號] = data.驗收單號;
                value[(int)enum_acceptance_med.藥品碼] = data.藥品碼;
                value[(int)enum_acceptance_med.數量] = data.數量;
                value[(int)enum_acceptance_med.效期] = data.效期;
                value[(int)enum_acceptance_med.批號] = data.批號;
                value[(int)enum_acceptance_med.驗收時間] = data.驗收時間;
                DateTime dateTime = data.加入時間.StringToDateTime();
                value[(int)enum_acceptance_med.加入時間] = dateTime.ToDateTimeString_6();
                value[(int)enum_acceptance_med.狀態] = enum_狀態.等待過帳.GetEnumName();
                value[(int)enum_acceptance_med.來源] = "院內系統";
                value[(int)enum_acceptance_med.備註] = "";
                list_value_add.LockAdd(value);
            }
            else
            {
                string 狀態 = list_value_buf[0][(int)enum_acceptance_med.狀態].ObjectToString();
                if (狀態 != enum_狀態.過帳完成.GetEnumName())
                {
                    object[] value = new object[new enum_acceptance_med().GetLength()];
                    value[(int)enum_acceptance_med.GUID] = list_value_buf[0][(int)enum_acceptance_med.GUID];
                    value[(int)enum_acceptance_med.請購單號] = data.請購單號;
                    value[(int)enum_acceptance_med.驗收單號] = data.驗收單號;
                    value[(int)enum_acceptance_med.藥品碼] = data.藥品碼;
                    value[(int)enum_acceptance_med.數量] = data.數量;
                    value[(int)enum_acceptance_med.效期] = data.效期;
                    value[(int)enum_acceptance_med.批號] = data.批號;
                    value[(int)enum_acceptance_med.驗收時間] = data.驗收時間;
                    DateTime dateTime = data.加入時間.StringToDateTime();
                    value[(int)enum_acceptance_med.加入時間] = dateTime.ToDateTimeString_6();
                    value[(int)enum_acceptance_med.狀態] = list_value_buf[0][(int)enum_acceptance_med.狀態];
                    value[(int)enum_acceptance_med.來源] = "院內系統";
                    value[(int)enum_acceptance_med.備註] = "";

                    list_value_replace.LockAdd(value);
                }

            }
            sQLControl_acceptance_med.AddRows(null, list_value_add);
            sQLControl_acceptance_med.UpdateByDefulteExtra(null, list_value_replace);

            string result = "==========call acceptance_med begin==========\n";
            result += $"新增<{list_value_add.Count}筆,修改{list_value_replace.Count}>筆資料\n";
            result += $"jsonRecive:{data.JsonSerializationt()}\n";
            result += "==========call acceptance_med end==========\n";
            _logger.WriteLog("acceptance_med", result);

            return $"新增<{list_value_add.Count}筆,修改{list_value_replace.Count}>筆資料/n{data.JsonSerializationt()}";
        }
  
        private class ICP_acceptance_med : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                int date0 = x[(int)enum_acceptance_med.驗收時間].ToDateString().Get_DateTINY();
                int date1 = y[(int)enum_acceptance_med.驗收時間].ToDateString().Get_DateTINY();
                return date0.CompareTo(date1);
            }
        }

    }
}
