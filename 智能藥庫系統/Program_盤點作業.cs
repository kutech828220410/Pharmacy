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
using MyUI;
using Basic;
using H_Pannel_lib;
using SQLUI;
using HIS_DB_Lib;
namespace 智能藥庫系統
{
    public enum enum_盤點狀態
    {
        正在盤點 = 1,
        完成盤點 = 2,
    }
    public enum enum_盤點總表
    {
        GUID,
        inventory_id,
        盤點編號,
        狀態,
        盤點人員,
        盤點開始時間,
        盤點結束時間,
    }
    public enum enum_盤點明細
    {
        GUID,
        inventory_id,
        藥品碼,
        藥品名稱,
        中文名稱,
        單位,
        庫存量,
        盤點量,
        盤點差異,

    }
    public class UpdateDrugApi_Result
    {
        private int errorCode = 0;
        private string _msg = "";
        private string _audit = "";
        private List<dataClass> _data = new List<dataClass>();

        public int ErrorCode { get => errorCode; set => errorCode = value; }
        public string msg { get => _msg; set => _msg = value; }
        public string audit { get => _audit; set => _audit = value; }
        public List<dataClass> data { get => _data; set => _data = value; }

        public class dataClass
        {
            private string _inventory_id = "";
            private string _code = "";
            private string _name = "";
            private string _chinese_name = "";
            private string _package = "";
            private string _barcode = "";
            private string _inventory = "";
            private string _inventory_new = "";

            public string inventory_id { get => _inventory_id; set => _inventory_id = value; }
            public string code { get => _code; set => _code = value; }
            public string name { get => _name; set => _name = value; }
            public string chinese_name { get => _chinese_name; set => _chinese_name = value; }
            public string package { get => _package; set => _package = value; }
            public string barcode { get => _barcode; set => _barcode = value; }
            public string inventory { get => _inventory; set => _inventory = value; }
            public string inventory_new { get => _inventory_new; set => _inventory_new = value; }

        }


    }
    public class InventoryApi
    {
        private int errorCode = 0;
        private string _msg = "";
        private List<string> post = new List<string>();
        private List<dataClass> _data = new List<dataClass>();

        public int ErrorCode { get => errorCode; set => errorCode = value; }
        public string msg { get => _msg; set => _msg = value; }
        public List<string> Post { get => post; set => post = value; }
        public List<dataClass> data { get => _data; set => _data = value; }

        public class dataClass
        {
            private string _inventory_id = "";
            private string _inventory_code = "";
            private string _audit = "";
            private string _user_id = "";
            private string _date_create = "";
            private string _date_finish = "";
            List<detailClass> _detail = new List<detailClass>();

            public string inventory_id { get => _inventory_id; set => _inventory_id = value; }
            public string inventory_code { get => _inventory_code; set => _inventory_code = value; }
            public string audit { get => _audit; set => _audit = value; }
            public string user_id { get => _user_id; set => _user_id = value; }
            public string date_create { get => _date_create; set => _date_create = value; }
            public string date_finish { get => _date_finish; set => _date_finish = value; }
            public List<detailClass> detail { get => _detail; set => _detail = value; }
        }
        public class detailClass
        {
            private string _code = "";
            private string _name = "";
            private string _chinese_name = "";
            private string _package = "";
            private string _barcode = "";
            private string _inventory = "";
            private string _inventory_new = "";

            public string code { get => _code; set => _code = value; }
            public string name { get => _name; set => _name = value; }
            public string chinese_name { get => _chinese_name; set => _chinese_name = value; }
            public string package { get => _package; set => _package = value; }
            public string barcode { get => _barcode; set => _barcode = value; }
            public string inventory { get => _inventory; set => _inventory = value; }
            public string inventory_new { get => _inventory_new; set => _inventory_new = value; }
        }
    }
    public class InventoryApi_Result
    {
        private int errorCode = 0;
        private string _msg = "";
        private dataClass _data = new dataClass();

        public int ErrorCode { get => errorCode; set => errorCode = value; }
        public string msg { get => _msg; set => _msg = value; }
        public dataClass data { get => _data; set => _data = value; }

        public class dataClass
        {
            private string _inventory_id = "";
         
            public string inventory_id { get => _inventory_id; set => _inventory_id = value; }

        }


    }
    public class InsertDrugApi_code_all
    {
        private string _code = "";

        public string code { get => _code; set => _code = value; }

        public InsertDrugApi_code_all(string code)
        {
            this.code = code;
        }
    }
    public partial class Form1 : Form
    {
        private void sub_Program_盤點作業_Init()
        {
            this.plC_RJ_Button_盤點作業_盤點表匯入.MouseDownEvent += PlC_RJ_Button_盤點作業_盤點表匯入_MouseDownEvent;
        }

        private void PlC_RJ_Button_盤點作業_盤點表匯入_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_盤點表匯入 dialog_盤點表匯入 = new Dialog_盤點表匯入();
            dialog_盤點表匯入.ShowDialog();


        }
    }
}
