using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using MyUI;
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using H_Pannel_lib;

namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        [Serializable]
        public class API_medicine_page_ADC
        {
            private string _code;
            private string _chinese_name;
            private string _name;
            private string _package;
            private string _inventory;
            private string _min_package;

            public string code { get => _code; set => _code = value; }
            public string chinese_name { get => _chinese_name; set => _chinese_name = value; }
            public string name { get => _name; set => _name = value; }
            public string package { get => _package; set => _package = value; }
            public string inventory { get => _inventory; set => _inventory = value; }
            public string min_package { get => _min_package; set => _min_package = value; }
        }
        [Serializable]
        public class API_trading_ADC
        {
            private string _action;
            private string _code;
            private string _name;
            private string _inventory;
            private string _room;
            private string _value;
            private string _balance;
            private string _operator;
            private string _operating_time;

            public string code { get => _code; set => _code = value; }
            public string name { get => _name; set => _name = value; }
            public string inventory { get => _inventory; set => _inventory = value; }
            public string room { get => _room; set => _room = value; }
            public string value { get => _value; set => _value = value; }
            public string balance { get => _balance; set => _balance = value; }

            [JsonPropertyName("operator")]
            public string Operator { get => _operator; set => _operator = value; }
            [JsonPropertyName("operating_time")]
            public string Operating_time { get => _operating_time; set => _operating_time = value; }
            public string Action { get => _action; set => _action = value; }
        }
        private class m_returnData
        {
            private string result = "";
            private int code = 0;
            private List<class_儲位總庫存表> data = new List<class_儲位總庫存表>();

            public string Result { get => result; set => result = value; }
            public int Code { get => code; set => code = value; }
            public List<class_儲位總庫存表> Data { get => data; set => data = value; }
        }


        public class class_儲位總庫存表
        {
            [JsonPropertyName("IP")]
            public string IP { get; set; }
            [JsonPropertyName("storage_name")]
            public string 儲位名稱 { get; set; }
            [JsonPropertyName("Code")]
            public string 藥品碼 { get; set; }
            [JsonPropertyName("Neme")]
            public string 藥品名稱 { get; set; }
            [JsonPropertyName("min_package")]
            public string 最小包裝量 { get; set; }
            [JsonPropertyName("package")]
            public string 單位 { get; set; }
            [JsonPropertyName("inventory")]
            public string 庫存 { get; set; }
            [JsonPropertyName("storage_type")]
            public string 儲位型式 { get; set; }
            [JsonPropertyName("max_inventory")]
            public string 可放置盒數 { get; set; }
            [JsonPropertyName("position")]
            public string 位置 { get; set; }

        }
        public enum enum_周邊設備_庫存_交易記錄查詢動作
        {
            掃碼領藥,
            手輸領藥,
            批次領藥,
            掃碼退藥,
            手輸退藥,
            重複領藥,
            人臉識別登入,
            RFID登入,
            密碼登入,
            登出,
            操作工程模式,
            效期庫存異動,
            入庫,
            實瓶繳回,
            空瓶繳回,
            退藥回收,
            None,
        }
        public enum enum_周邊設備_庫存_庫存查詢
        {
            藥碼,
            藥名,
            中文名稱,
            單位,
            庫存,
        }
        public enum enum_周邊設備_庫存_交易紀錄查詢
        {
            動作,
            藥碼,
            藥名,
            Room,
            庫存,
            交易量,
            結存量,
            操作人,
            操作時間,
        }
    }
}
