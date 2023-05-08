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
    public partial class Form1 : Form
    {
        public enum enum_周邊設備_麻醉部ADC_庫存_交易記錄查詢動作
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
            None,
        }
        private enum enum_周邊設備_麻醉部ADC_庫存_庫存查詢
        {
            藥碼,
            藥名,
            中文名稱,
            單位,
            庫存,
        }
        private enum enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢
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
        [Serializable]
        public class API_medicine_page_ADC
        {
            private string _code;
            private string _chinese_name;
            private string _name;
            private string _package;
            private string _inventory;

            public string code { get => _code; set => _code = value; }
            public string chinese_name { get => _chinese_name; set => _chinese_name = value; }
            public string name { get => _name; set => _name = value; }
            public string package { get => _package; set => _package = value; }
            public string inventory { get => _inventory; set => _inventory = value; }
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

        private void sub_Program_周邊設備_麻醉部ADC_庫存_Init()
        {
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_API測試.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_API測試_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_顯示全部.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_藥碼搜尋.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_藥碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_藥名搜尋.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_藥名搜尋_MouseDownEvent;

            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_API測試.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_API測試_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_顯示全部.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥碼搜尋.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥名搜尋.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥名搜尋_MouseDownEvent;

            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_庫存查詢.Init();
            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.Init();

            this.plC_UI_Init.Add_Method(sub_Program_周邊設備_麻醉部ADC_庫存);
        }

     

        private bool flag_Program_周邊設備_麻醉部ADC_庫存_Init = false;
        private void sub_Program_周邊設備_麻醉部ADC_庫存()
        { 
            if (this.plC_ScreenPage_Main.PageText == "周邊設備" && this.plC_ScreenPage_周邊設備.PageText == "麻醉部ADC" && this.plC_ScreenPage_周邊設備_麻醉部ADC.PageText == "庫存")
            {
                if (!flag_Program_周邊設備_麻醉部ADC_庫存_Init)
                {

                    flag_Program_周邊設備_麻醉部ADC_庫存_Init = true;
                }
            }
            else
            {
                flag_Program_周邊設備_麻醉部ADC_庫存_Init = false;
            }

        }

        #region Function
        private List<object[]> Function_周邊設備_麻醉部ADC_庫存_庫存查詢_取得資料()
        {
            List<API_medicine_page_ADC> list_API_medicine_page_ADC = new List<API_medicine_page_ADC>();
            List<object[]> list_values = new List<object[]>();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string result = Basic.Net.WEBApiGet("http://10.18.28.17/api/medicine_page");

            list_API_medicine_page_ADC = result.JsonDeserializet<List<API_medicine_page_ADC>>();

            for (int i = 0; i < list_API_medicine_page_ADC.Count; i++)
            {
                object[] values = new object[new enum_周邊設備_麻醉部ADC_庫存_庫存查詢().GetLength()];
                values[(int)enum_周邊設備_麻醉部ADC_庫存_庫存查詢.藥碼] = list_API_medicine_page_ADC[i].code;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_庫存查詢.藥名] = list_API_medicine_page_ADC[i].name;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_庫存查詢.中文名稱] = list_API_medicine_page_ADC[i].chinese_name;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_庫存查詢.單位] = list_API_medicine_page_ADC[i].package;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_庫存查詢.庫存] = list_API_medicine_page_ADC[i].inventory;
                list_values.Add(values);
            }

            return list_values;
        }
        private List<object[]> Function_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_取得資料()
        {
            List<API_trading_ADC> list_API_trading_ADC = new List<API_trading_ADC>();
            List<object[]> list_values = new List<object[]>();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string result = Basic.Net.WEBApiGet("http://10.18.28.17/api/trading");

            list_API_trading_ADC = result.JsonDeserializet<List<API_trading_ADC>>();
            for (int i = 0; i < list_API_trading_ADC.Count; i++)
            {
                object[] values = new object[new enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢().GetLength()];
                values[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.動作] = list_API_trading_ADC[i].Action;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.藥碼] = list_API_trading_ADC[i].code;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.藥名] = list_API_trading_ADC[i].name;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.Room] = list_API_trading_ADC[i].room;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.庫存] = list_API_trading_ADC[i].inventory;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.交易量] = list_API_trading_ADC[i].value;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.結存量] = list_API_trading_ADC[i].balance;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.操作人] = list_API_trading_ADC[i].Operator;
                values[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.操作時間] = list_API_trading_ADC[i].Operating_time;

                list_values.Add(values);
            }
            list_values.RemoveRow((int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.藥碼, "");
            list_values.RemoveRow((int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.動作, enum_周邊設備_麻醉部ADC_庫存_交易記錄查詢動作.實瓶繳回.GetEnumName());
            list_values.Sort(new ICP_周邊設備_麻醉部ADC_庫存_交易記錄查詢());
            return list_values;
        }
        #endregion
        #region Event
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_API測試_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Function_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_取得資料();
        }

        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_周邊設備_麻醉部ADC_庫存_庫存查詢_取得資料();

            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_庫存查詢.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_藥碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_周邊設備_麻醉部ADC_庫存_庫存查詢_取得資料();
            string code = this.rJ_TextBox_周邊設備_麻醉部ADC_庫存_庫存查詢_藥碼.Text;
            if (code.StringIsEmpty()) return;
            list_value = list_value.GetRowsByLike((int)enum_周邊設備_麻醉部ADC_庫存_庫存查詢.藥碼, code);
            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_庫存查詢.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_藥名搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_周邊設備_麻醉部ADC_庫存_庫存查詢_取得資料();
            string name = this.rJ_TextBox_周邊設備_麻醉部ADC_庫存_庫存查詢_藥名.Text;
            if (name.StringIsEmpty()) return;
            list_value = list_value.GetRowsByLike((int)enum_周邊設備_麻醉部ADC_庫存_庫存查詢.藥名, name);
            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_庫存查詢.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_庫存查詢_API測試_MouseDownEvent(MouseEventArgs mevent)
        {
            List<API_medicine_page_ADC> list_API_medicine_page_ADC = new List<API_medicine_page_ADC>();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string result = Basic.Net.WEBApiGet("http://10.18.28.17//api/medicine_page");

            list_API_medicine_page_ADC = result.JsonDeserializet<List<API_medicine_page_ADC>>();
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_values = this.Function_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_取得資料();
            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.RefreshGrid(list_values);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥名搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_取得資料();
            string name = this.rJ_TextBox_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥名.Text;
            if (name.StringIsEmpty()) return;
            list_value = list_value.GetRowsByLike((int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.藥名, name);
            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_取得資料();
            string code = this.rJ_TextBox_周邊設備_麻醉部ADC_庫存_交易紀錄查詢_藥碼.Text;
            if (code.StringIsEmpty()) return;
            list_value = list_value.GetRowsByLike((int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.藥碼, code);
            this.sqL_DataGridView_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.RefreshGrid(list_value);
        }
        #endregion

        public class ICP_周邊設備_麻醉部ADC_庫存_交易記錄查詢 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime1 = x[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.操作時間].StringToDateTime();
                DateTime datetime2 = y[(int)enum_周邊設備_麻醉部ADC_庫存_交易紀錄查詢.操作時間].StringToDateTime();
                int compare = DateTime.Compare(datetime1, datetime2);
                return compare;
            

            }
        }
    }
}
