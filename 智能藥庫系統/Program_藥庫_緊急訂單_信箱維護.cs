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
        public enum enum_信箱設定_RTF存檔參數
        {
            使用者ID,
            參數名稱,           
        }
        public string[] 信箱設定_Code_Data
        {
            get
            {
                string[] _信箱設定_Code_Data = new string[50];
                _信箱設定_Code_Data[(int)enum_信箱設定_Code.訂單編號] = textBox_信箱設定_訂單編號.Text;
                _信箱設定_Code_Data[(int)enum_信箱設定_Code.訂購人] = textBox_信箱設定_訂購人.Text;
                _信箱設定_Code_Data[(int)enum_信箱設定_Code.訂購院所別] = textBox_信箱設定_訂購院所別.Text;
                _信箱設定_Code_Data[(int)enum_信箱設定_Code.訂購日期] = textBox_信箱設定_訂購日期.Text;


                _信箱設定_Code_Data[(int)enum_信箱設定_Code.供應商全名] = textBox_信箱設定_供應商全名.Text;
                _信箱設定_Code_Data[(int)enum_信箱設定_Code.包裝單位] = textBox_信箱設定_包裝單位.Text;
                _信箱設定_Code_Data[(int)enum_信箱設定_Code.Email] = textBox_信箱設定_Email.Text;
                _信箱設定_Code_Data[(int)enum_信箱設定_Code.聯絡人] = textBox_信箱設定_聯絡人.Text;

                _信箱設定_Code_Data[(int)enum_信箱設定_Code.應驗收日期] = textBox_信箱設定_應驗收日期.Text;
                return _信箱設定_Code_Data;
            }
            set
            {
                this.Invoke(new Action(delegate
                {
                    textBox_信箱設定_訂單編號.Text = value[(int)enum_信箱設定_Code.訂單編號];
                    textBox_信箱設定_訂購人.Text = value[(int)enum_信箱設定_Code.訂購人];
                    textBox_信箱設定_訂購院所別.Text = value[(int)enum_信箱設定_Code.訂購院所別];
                    textBox_信箱設定_訂購日期.Text = value[(int)enum_信箱設定_Code.訂購日期];


                    textBox_信箱設定_供應商全名.Text = value[(int)enum_信箱設定_Code.供應商全名];
                    textBox_信箱設定_包裝單位.Text = value[(int)enum_信箱設定_Code.包裝單位];
                    textBox_信箱設定_Email.Text = value[(int)enum_信箱設定_Code.Email];
                    textBox_信箱設定_聯絡人.Text = value[(int)enum_信箱設定_Code.聯絡人];

                    textBox_信箱設定_應驗收日期.Text = value[(int)enum_信箱設定_Code.應驗收日期];
                }));
            }
        }
        public enum enum_信箱設定_Code
        {
            訂單編號 = 0,
            訂購人 = 1,
            訂購院所別 = 2,
            訂購日期 = 3,

            藥品內容表格 = 10,

            供應商全名 = 20,
            包裝單位 = 21,
            Email = 22,
            聯絡人 = 23,
            應驗收日期 = 30,

        }
        private void sub_Program_藥庫_緊急訂單_信箱設定_Init()
        {

            this.Function_緊急訂單_信箱設定_伺服器參數讀檔();

            this.checkBox_信箱設定_伺服器參數_顯示字元.CheckedChanged += CheckBox_信箱設定_伺服器參數_顯示字元_CheckedChanged;
            this.plC_RJ_Button_信箱設定_存檔.MouseDownEvent += PlC_RJ_Button_信箱設定_存檔_MouseDownEvent;
            this.plC_RJ_Button_信箱設定_讀檔.MouseDownEvent += PlC_RJ_Button_信箱設定_讀檔_MouseDownEvent;
            this.plC_RJ_Button_信箱設定_伺服器參數_存檔.MouseDownEvent += PlC_RJ_Button_信箱設定_伺服器參數_存檔_MouseDownEvent;
            this.plC_RJ_Button_信箱設定_伺服器參數_讀檔.MouseDownEvent += PlC_RJ_Button_信箱設定_伺服器參數_讀檔_MouseDownEvent;
            this.plC_RJ_Button_信箱設定_讀取測試值.MouseDownEvent += PlC_RJ_Button_信箱設定_讀取測試值_MouseDownEvent;
            this.plC_RJ_Button_信箱設定_預覽.MouseDownEvent += PlC_RJ_Button_信箱設定_預覽_MouseDownEvent;
            this.plC_RJ_Button_信箱設定_伺服器參數_寫入測試郵件.MouseDownEvent += PlC_RJ_Button_信箱設定_伺服器參數_寫入測試郵件_MouseDownEvent;


            this.plC_UI_Init.Add_Method(sub_Program_藥庫_緊急訂單_信箱設定);
        }

  

        private bool flag_藥庫_緊急訂單_信箱設定 = false;
        private void sub_Program_藥庫_緊急訂單_信箱設定()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "緊急訂單" && this.plC_ScreenPage_藥庫_緊急訂單.PageText == "信箱設定")
            {
                if (!this.flag_藥庫_緊急訂單_信箱設定)
                {
                    myEmail_Send_UI_信箱設定_文本.Subject = Function_信箱設定_信箱主旨_讀檔();
                    myEmail_Send_UI_信箱設定_文本.Rtf = Function_信箱設定_信箱內容_讀檔();

                    this.flag_藥庫_緊急訂單_信箱設定 = true;
                }

            }
            else
            {
                this.flag_藥庫_緊急訂單_信箱設定 = false;
            }
        }

        #region Function
        public string Function_信箱設定_GetCode(int index)
        {
            return "{" + index.ToString() + "}";
        }
        public string Function_信箱設定_ReplaceCode(string value)
        {
            this.Invoke(new Action(delegate
            {
                Array emun_values = Enum.GetValues(typeof(enum_信箱設定_Code));
                foreach (int i in Enum.GetValues(typeof(enum_信箱設定_Code)))
                {
                    value = value.Replace(this.Function_信箱設定_GetCode(i), 信箱設定_Code_Data[i]);
                }
            }));
         
            return value;
        }
        public void Function_信箱設定_ReplaceCode(MyEmail.MyEmail_Send_UI MyEmail_Send_UI, MyEmail.MyEmail_Send_UI.Table_Rtf Table_Rtf)
        {
            this.Invoke(new Action(delegate
            {
                Array emun_values = Enum.GetValues(typeof(enum_信箱設定_Code));
                foreach (int i in Enum.GetValues(typeof(enum_信箱設定_Code)))
                {
                    if (i != (int)enum_信箱設定_Code.藥品內容表格)
                    {
                        MyEmail_Send_UI.Replace(this.Function_信箱設定_GetCode(i), 信箱設定_Code_Data[i]);
                    }
                    else
                    {
                        MyEmail_Send_UI.Replace_RTF(this.Function_信箱設定_GetCode(i), Table_Rtf.Get_Table_RTF());
                    }
                }
            }));
        
        }
        private string Function_信箱設定_信箱主旨_讀檔()
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品補給系統_參數資料.SQL_GetAllRows(false);
            List<object[]> list_value_buf = new List<object[]>();

            object[] titles = new object[new enum_信箱設定_RTF存檔參數().GetLength()];
            titles[(int)enum_信箱設定_RTF存檔參數.使用者ID] = this.登入者ID;
            titles[(int)enum_信箱設定_RTF存檔參數.參數名稱] = "信箱主旨";
            string 名稱 = Basic.TypeConvert.SetTextValue(new enum_信箱設定_RTF存檔參數().GetEnumNames(), titles);
            list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, 名稱);
            if (list_value_buf.Count > 0)
            {
                object[] value = list_value_buf[0];
                value[(int)enum_藥品補給系統_參數資料.名稱] = 名稱;
                return value[(int)enum_藥品補給系統_參數資料.數值].ObjectToString();
            }
            return "";
        }
        private string Function_信箱設定_信箱內容_讀檔()
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品補給系統_參數資料.SQL_GetAllRows(false);
            List<object[]> list_value_buf = new List<object[]>();

            object[] titles = new object[new enum_信箱設定_RTF存檔參數().GetLength()];
            titles[(int)enum_信箱設定_RTF存檔參數.使用者ID] = this.登入者ID;
            titles[(int)enum_信箱設定_RTF存檔參數.參數名稱] = "信箱內容";
            string 名稱 = Basic.TypeConvert.SetTextValue(new enum_信箱設定_RTF存檔參數().GetEnumNames(), titles);
            list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, 名稱);
            if (list_value_buf.Count > 0)
            {
                object[] value = list_value_buf[0];
                value[(int)enum_藥品補給系統_參數資料.名稱] = 名稱;
                return value[(int)enum_藥品補給系統_參數資料.數值].ObjectToString();
            }
            return "";
        }
        private void Function_信箱設定_信箱主旨_存檔()
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品補給系統_參數資料.SQL_GetAllRows(false);
            List<object[]> list_value_buf = new List<object[]>();

            object[] titles = new object[new enum_信箱設定_RTF存檔參數().GetLength()];
            titles[(int)enum_信箱設定_RTF存檔參數.使用者ID] = this.登入者ID;
            titles[(int)enum_信箱設定_RTF存檔參數.參數名稱] = "信箱主旨";
            string 名稱 = Basic.TypeConvert.SetTextValue(new enum_信箱設定_RTF存檔參數().GetEnumNames(), titles);
            list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, 名稱);
            this.Invoke(new Action(delegate
            {
                if (list_value_buf.Count == 0)
                {
                    object[] value = new object[new enum_藥品補給系統_參數資料().GetLength()];
                    value[(int)enum_藥品補給系統_參數資料.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_藥品補給系統_參數資料.名稱] = 名稱;
                    value[(int)enum_藥品補給系統_參數資料.數值] = myEmail_Send_UI_信箱設定_文本.Subject;
                    sqL_DataGridView_藥品補給系統_參數資料.SQL_AddRow(value, false);
                }
                else
                {
                    object[] value = list_value_buf[0];
                    value[(int)enum_藥品補給系統_參數資料.名稱] = 名稱;
                    value[(int)enum_藥品補給系統_參數資料.數值] = myEmail_Send_UI_信箱設定_文本.Subject;
                    sqL_DataGridView_藥品補給系統_參數資料.SQL_Replace(enum_藥品補給系統_參數資料.名稱.GetEnumName(), 名稱, value, false);
                }
            }));


        }
        private void Function_信箱設定_信箱內容_存檔()
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品補給系統_參數資料.SQL_GetAllRows(false);
            List<object[]> list_value_buf = new List<object[]>();

            object[] titles = new object[new enum_信箱設定_RTF存檔參數().GetLength()];
            titles[(int)enum_信箱設定_RTF存檔參數.使用者ID] = this.登入者ID;
            titles[(int)enum_信箱設定_RTF存檔參數.參數名稱] = "信箱內容";
            string 名稱 = Basic.TypeConvert.SetTextValue(new enum_信箱設定_RTF存檔參數().GetEnumNames(), titles);
            list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, 名稱);
            this.Invoke(new Action(delegate
            {
                if (list_value_buf.Count == 0)
                {
                    object[] value = new object[new enum_藥品補給系統_參數資料().GetLength()];
                    value[(int)enum_藥品補給系統_參數資料.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_藥品補給系統_參數資料.名稱] = 名稱;
                    value[(int)enum_藥品補給系統_參數資料.數值] = myEmail_Send_UI_信箱設定_文本.Rtf;
                    sqL_DataGridView_藥品補給系統_參數資料.SQL_AddRow(value, false);
                }
                else
                {
                    object[] value = list_value_buf[0];
                    value[(int)enum_藥品補給系統_參數資料.名稱] = 名稱;
                    value[(int)enum_藥品補給系統_參數資料.數值] = myEmail_Send_UI_信箱設定_文本.Rtf;
                    sqL_DataGridView_藥品補給系統_參數資料.SQL_Replace(enum_藥品補給系統_參數資料.名稱.GetEnumName(), 名稱, value, false);
                }
            }));


        }
        private void Function_緊急訂單_信箱設定_伺服器參數讀檔()
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品補給系統_參數資料.SQL_GetAllRows(false);
            List<object[]> list_value_buf = new List<object[]>();

            this.Invoke(new Action(delegate
            {
                list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_UserName.GetEnumName());
                if (list_value_buf.Count > 0)
                {
                    this.textBox_信箱設定_伺服器參數_UserName.Text = list_value_buf[0][(int)enum_藥品補給系統_參數資料.數值].ObjectToString();
                }
                list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_Password.GetEnumName());
                if (list_value_buf.Count > 0)
                {
                    this.textBox_信箱設定_伺服器參數_Password.Text = list_value_buf[0][(int)enum_藥品補給系統_參數資料.數值].ObjectToString();
                }
                list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_Host.GetEnumName());
                if (list_value_buf.Count > 0)
                {
                    this.textBox_信箱設定_伺服器參數_Host.Text = list_value_buf[0][(int)enum_藥品補給系統_參數資料.數值].ObjectToString();
                }
                list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_Port.GetEnumName());
                if (list_value_buf.Count > 0)
                {
                    this.textBox_信箱設定_伺服器參數_Port.Text = list_value_buf[0][(int)enum_藥品補給系統_參數資料.數值].ObjectToString();
                }
                list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_發件者.GetEnumName());
                if (list_value_buf.Count > 0)
                {
                    this.textBox_信箱設定_伺服器參數_發件者.Text = list_value_buf[0][(int)enum_藥品補給系統_參數資料.數值].ObjectToString();
                }
            }));

        }
        private void Function_緊急訂單_信箱設定_伺服器參數存檔()
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品補給系統_參數資料.SQL_GetAllRows(false);
            List<object[]> list_value_buf = new List<object[]>();

            object[] value;

            list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_UserName.GetEnumName());
            if (list_value_buf.Count > 0)
            {
                value = list_value_buf[0];
                value[(int)enum_藥品補給系統_參數資料.名稱] = enum_藥品補給系統_參數名稱.伺服器參數_UserName.GetEnumName();
                value[(int)enum_藥品補給系統_參數資料.數值] = this.textBox_信箱設定_伺服器參數_UserName.Text;
                this.sqL_DataGridView_藥品補給系統_參數資料.SQL_ReplaceExtra(value, false);
            }
            list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_Password.GetEnumName());
            if (list_value_buf.Count > 0)
            {
                value = list_value_buf[0];
                value[(int)enum_藥品補給系統_參數資料.名稱] = enum_藥品補給系統_參數名稱.伺服器參數_Password.GetEnumName();
                value[(int)enum_藥品補給系統_參數資料.數值] = this.textBox_信箱設定_伺服器參數_Password.Text;
                this.sqL_DataGridView_藥品補給系統_參數資料.SQL_ReplaceExtra(value, false);
            }
            list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_Host.GetEnumName());
            if (list_value_buf.Count > 0)
            {
                value = list_value_buf[0];
                value[(int)enum_藥品補給系統_參數資料.名稱] = enum_藥品補給系統_參數名稱.伺服器參數_Host.GetEnumName();
                value[(int)enum_藥品補給系統_參數資料.數值] = this.textBox_信箱設定_伺服器參數_Host.Text;
                this.sqL_DataGridView_藥品補給系統_參數資料.SQL_ReplaceExtra(value, false);
            }
            list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_Port.GetEnumName());
            if (list_value_buf.Count > 0)
            {
                value = list_value_buf[0];
                value[(int)enum_藥品補給系統_參數資料.名稱] = enum_藥品補給系統_參數名稱.伺服器參數_Port.GetEnumName();
                value[(int)enum_藥品補給系統_參數資料.數值] = this.textBox_信箱設定_伺服器參數_Port.Text;
                this.sqL_DataGridView_藥品補給系統_參數資料.SQL_ReplaceExtra(value, false);
            }
            list_value_buf = list_value.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.伺服器參數_發件者.GetEnumName());
            if (list_value_buf.Count > 0)
            {
                value = list_value_buf[0];
                value[(int)enum_藥品補給系統_參數資料.名稱] = enum_藥品補給系統_參數名稱.伺服器參數_發件者.GetEnumName();
                value[(int)enum_藥品補給系統_參數資料.數值] = this.textBox_信箱設定_伺服器參數_發件者.Text;
                this.sqL_DataGridView_藥品補給系統_參數資料.SQL_ReplaceExtra(value, false);
            }

        }
        #endregion
        #region Event
        private void CheckBox_信箱設定_伺服器參數_顯示字元_CheckedChanged(object sender, EventArgs e)
        {
            bool flag_ok = !this.checkBox_信箱設定_伺服器參數_顯示字元.Checked;
            this.textBox_信箱設定_伺服器參數_Password.PasswordChar = flag_ok? '*': new char();
        }
        private void PlC_RJ_Button_信箱設定_存檔_MouseDownEvent(MouseEventArgs mevent)
        {
            Function_信箱設定_信箱主旨_存檔();
            Function_信箱設定_信箱內容_存檔();
            MyMessageBox.ShowDialog("存檔完成!");
        }
        private void PlC_RJ_Button_信箱設定_讀檔_MouseDownEvent(MouseEventArgs mevent)
        {
            myEmail_Send_UI_信箱設定_文本.Subject = Function_信箱設定_信箱主旨_讀檔();
            myEmail_Send_UI_信箱設定_文本.Rtf = Function_信箱設定_信箱內容_讀檔();
            MyMessageBox.ShowDialog("讀檔完成!");
        }
        private void PlC_RJ_Button_信箱設定_伺服器參數_存檔_MouseDownEvent(MouseEventArgs mevent)
        {
            Function_緊急訂單_信箱設定_伺服器參數存檔();
            MyMessageBox.ShowDialog("存檔完成!");
        }
        private void PlC_RJ_Button_信箱設定_伺服器參數_讀檔_MouseDownEvent(MouseEventArgs mevent)
        {
            Function_緊急訂單_信箱設定_伺服器參數讀檔();
            MyMessageBox.ShowDialog("讀檔完成!");
        }
        private void PlC_RJ_Button_信箱設定_讀取測試值_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                DateTime date = DateTime.Now;
                string[] 信箱設定_Code_Data_buf = new string[50];
                信箱設定_Code_Data_buf[(int)enum_信箱設定_Code.訂單編號] = "SN12345678";
                信箱設定_Code_Data_buf[(int)enum_信箱設定_Code.訂購人] = "王曉明";
                信箱設定_Code_Data_buf[(int)enum_信箱設定_Code.訂購院所別] = "XXX院所";
                信箱設定_Code_Data_buf[(int)enum_信箱設定_Code.訂購日期] = date.Date.ToShortDateString();

                信箱設定_Code_Data_buf[(int)enum_信箱設定_Code.供應商全名] = "供應商XXX有限公司";
                信箱設定_Code_Data_buf[(int)enum_信箱設定_Code.包裝單位] = "PCS";
                信箱設定_Code_Data_buf[(int)enum_信箱設定_Code.Email] = "test@yahoo.com.tw";
                信箱設定_Code_Data_buf[(int)enum_信箱設定_Code.聯絡人] = "章大同";

                信箱設定_Code_Data_buf[(int)enum_信箱設定_Code.應驗收日期] = date.Date.AddDays(10).ToShortDateString();


                this.信箱設定_Code_Data = 信箱設定_Code_Data_buf;
            }));
           
        }
        private void PlC_RJ_Button_信箱設定_預覽_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.myEmail_Send_UI_信箱設定_預覽.Subject = this.Function_信箱設定_ReplaceCode(this.myEmail_Send_UI_信箱設定_文本.Subject);
                this.myEmail_Send_UI_信箱設定_預覽.Rtf = this.myEmail_Send_UI_信箱設定_文本.Rtf;

                string 包裝單位 = Function_信箱設定_ReplaceCode("{21}");
                MyEmail.MyEmail_Send_UI.Table_Rtf Table_Rtf = new MyEmail.MyEmail_Send_UI.Table_Rtf(4, 4);
                Table_Rtf.AddRow(new string[] { "藥品碼", "名稱", "數量", "包裝單位" });
                Table_Rtf.AddRow(new string[] { "02721", "藥品001", "5", 包裝單位 });
                Table_Rtf.AddRow(new string[] { "06788", "藥品002", "3", 包裝單位 });
                Table_Rtf.AddRow(new string[] { "09823", "藥品003", "15", 包裝單位 });
                Table_Rtf.Set_ColunmWidth(0, 1000);
                Table_Rtf.Set_ColunmWidth(1, 4000);
                Table_Rtf.Set_ColunmWidth(2, 1000);
                Table_Rtf.Set_ColunmWidth(3, 1500);
                this.Function_信箱設定_ReplaceCode(this.myEmail_Send_UI_信箱設定_預覽, Table_Rtf);
            }));

        }
        private void PlC_RJ_Button_信箱設定_伺服器參數_寫入測試郵件_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                textBox_信箱設定_伺服器參數_UserName.Text = "Hson_Evan@outlook.com";
                textBox_信箱設定_伺服器參數_Password.Text = "s19910404";
                textBox_信箱設定_伺服器參數_Host.Text = "smtp-mail.outlook.com";
                textBox_信箱設定_伺服器參數_Port.Text = "587";
                textBox_信箱設定_伺服器參數_發件者.Text = "Hson_Evan@outlook.com";
                this.myEmail_Send_UI.EnableSsl = true;

            }));
        }
        #endregion

    }
}
