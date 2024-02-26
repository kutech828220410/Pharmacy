using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyUI;
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using MySQL_Login;
using HIS_DB_Lib;
using SQLUI;
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {

        public enum enum_人員資料_匯出
        {
            ID,
            姓名,
            性別,
            密碼,
            單位,
            卡號,
            一維條碼,
        }
        public enum enum_人員資料_匯入
        {
            ID,
            姓名,
            性別,
            密碼,
            單位,
            卡號,
            一維條碼,
        }
        public enum ContextMenuStrip_人員資料
        {
            [Description("S39014")]
            匯出,
            [Description("S39014")]
            匯入,
            [Description("S39014")]
            匯出選取資料,
            [Description("S39014")]
            登錄資料,
            [Description("S39014")]
            刪除選取資料,
        }

        private List<PLC_Device> List_PLC_Device_權限管理 = new List<PLC_Device>();
        private List<LoginDataWebAPI.Class_login_data> List_class_Login_Data = new List<LoginDataWebAPI.Class_login_data>();
        private List<LoginDataWebAPI.Class_login_data_index> List_class_Login_Data_index = new List<LoginDataWebAPI.Class_login_data_index>();

        private void sub_Program_人員資料_Init()
        {

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_人員資料, dBConfigClass.DB_person_page);

            this.loginUI.Set_login_data_DB(dBConfigClass.DB_person_page);
            this.loginUI.Set_login_data_index_DB(dBConfigClass.DB_person_page);
            this.loginUI.Init();

            string url = $"{Api_URL}/api/person_page/init";
            returnData returnData = new returnData();
            returnData.ServerType = enum_ServerSetting_Type.藥庫.GetEnumName();
            returnData.ServerName = $"{"DS01"}";
            string json_in = returnData.JsonSerializationt();
            string json = Basic.Net.WEBApiPostJson($"{url}", json_in);
            Table table = json.JsonDeserializet<Table>();
            if (table == null)
            {
                MyMessageBox.ShowDialog($"人員資料表單建立失敗!! Api_URL:{Api_URL}");
                return;
            }
            this.sqL_DataGridView_人員資料.Init(table);
            this.sqL_DataGridView_人員資料.Set_ColumnVisible(false, new enum_人員資料().GetEnumNames());
            this.sqL_DataGridView_人員資料.Set_ColumnWidth(150, DataGridViewContentAlignment.MiddleLeft, enum_人員資料.ID);
            this.sqL_DataGridView_人員資料.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_人員資料.姓名);
            this.sqL_DataGridView_人員資料.Set_ColumnWidth(60, DataGridViewContentAlignment.MiddleCenter, enum_人員資料.性別);
            this.sqL_DataGridView_人員資料.Set_ColumnWidth(300, DataGridViewContentAlignment.MiddleLeft, enum_人員資料.單位);
            this.sqL_DataGridView_人員資料.Set_ColumnWidth(200, DataGridViewContentAlignment.MiddleLeft, enum_人員資料.藥師證字號);
            this.sqL_DataGridView_人員資料.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleCenter, enum_人員資料.權限等級);
            this.sqL_DataGridView_人員資料.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleCenter, enum_人員資料.顏色);
            this.sqL_DataGridView_人員資料.Set_ColumnWidth(200, DataGridViewContentAlignment.MiddleLeft, enum_人員資料.卡號);
            this.sqL_DataGridView_人員資料.Set_ColumnWidth(200, DataGridViewContentAlignment.MiddleLeft, enum_人員資料.一維條碼);

            this.loginUI.Set_login_data_DB(dBConfigClass.DB_person_page);
            this.loginUI.Set_login_data_index_DB(dBConfigClass.DB_person_page);
            this.loginUI.Init();


            this.sqL_DataGridView_人員資料.DataGridRefreshEvent += SqL_DataGridView_人員資料_DataGridRefreshEvent;
            this.sqL_DataGridView_人員資料.RowEnterEvent += SqL_DataGridView_人員資料_RowEnterEvent;
            this.sqL_DataGridView_人員資料.RowDoubleClickEvent += SqL_DataGridView_人員資料_RowDoubleClickEvent;
            this.sqL_DataGridView_人員資料.MouseDown += SqL_DataGridView_人員資料_MouseDown;
            this.sqL_DataGridView_人員資料.SQL_GetAllRows(true);

            this.plC_Button_權限設定_設定至Server.MouseDownEvent += PlC_Button_權限設定_設定至Server_MouseDownEvent;
            this.plC_RJ_ComboBox_權限管理_權限等級.OnSelectedIndexChanged += PlC_RJ_ComboBox_權限管理_權限等級_OnSelectedIndexChanged;
            this.plC_RJ_ComboBox_權限管理_權限等級.Items.Clear();
            int level_num = (int)this.loginUI.Level_num;
            for (int i = 1; i <= level_num; i++)
            {
                this.plC_RJ_ComboBox_權限管理_權限等級.Items.Add(i.ToString("00"));
            }
            for (int i = 0; i < 256; i++) this.List_PLC_Device_權限管理.Add(new PLC_Device($"S{39000 + i}"));

            this.button_人員資料_顏色選擇.Click += button_人員資料_顏色選擇_Click;
            this.plC_RJ_Button_人員資料_匯出.MouseDownEvent += PlC_RJ_Button_人員資料_匯出_MouseDownEvent;
            this.plC_RJ_Button_人員資料_匯入.MouseDownEvent += PlC_RJ_Button_人員資料_匯入_MouseDownEvent;
            this.plC_RJ_Button_人員資料_登錄.MouseDownEvent += PlC_RJ_Button_人員資料_登錄_MouseDownEvent;
            this.plC_RJ_Button_人員資料_刪除.MouseDownEvent += PlC_RJ_Button_人員資料_刪除_MouseDownEvent;
            this.plC_RJ_Button_人員資料_清除內容.MouseDownEvent += PlC_RJ_Button_人員資料_清除內容_MouseDownEvent;


            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_交易紀錄頁面, 87);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_批次過帳頁面, 70);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_周邊設備頁面, 77);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_工程模式頁面, 82);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_效期庫存異動, 84);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_戰情白版設定, 78);

            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_藥品資料_頁面顯示, 79);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_藥品資料_資料更動, 79);

            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_藥庫_頁面顯示, 71);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_藥庫_儲位設定, 81);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_藥庫_訂單管理, 88);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_藥庫_撥補功能, 89);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_藥庫_驗收入庫管理, 90);

            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_藥局_頁面顯示, 72);

            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_人員資料_頁面顯示, 80);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_人員資料_資料更動, 85);
            this.loginIndex_Pannel.Set_CheckBox(this.checkBox_權限管理_人員資料_登入權限資料, 86);

            this.plC_UI_Init.Add_Method(this.sub_Program_人員資料);
        }



        bool flag_人員資料_資料維護_頁面更新 = false;
        bool flag_人員資料_權限管理_頁面更新 = false;
        private void sub_Program_人員資料()
        {
            if (this.plC_ScreenPage_Main.PageText == "人員資料" && this.plC_ScreenPage_人員資料.PageText == "資料維護")
            {
                if (!this.flag_人員資料_資料維護_頁面更新)
                {


                    this.plC_RJ_ComboBox_權限管理_權限等級.SetValue(0);
                    this.sqL_DataGridView_人員資料.SQL_GetAllRows(true);



                    this.flag_人員資料_資料維護_頁面更新 = true;
                }
            }
            else
            {
                this.flag_人員資料_資料維護_頁面更新 = false;
            }
            if (this.plC_ScreenPage_Main.PageText == "人員資料" && this.plC_ScreenPage_人員資料.PageText == "權限管理")
            {
                if (!this.flag_人員資料_權限管理_頁面更新)
                {
                    this.List_class_Login_Data = this.loginUI.Get_login_data();
                    this.List_class_Login_Data_index = this.loginUI.Get_login_data_index();
                    this.loginIndex_Pannel.Set_Login_Data_Index(this.List_class_Login_Data_index, enum_login_data_type.藥庫);
                    this.loginIndex_Pannel.Set_Login_Data(this.List_class_Login_Data[0]);

                    this.Invoke(new Action(delegate
                    {
                        PLC_Device pLC_Device = new PLC_Device("S39014");
                        this.comboBox_人員資料_權限等級.Enabled = pLC_Device.Bool;
                    }));


                    this.flag_人員資料_權限管理_頁面更新 = true;
                }
            }
            else
            {
                this.flag_人員資料_權限管理_頁面更新 = false;
            }
            this.sub_Program_人員資料_接收設備資料();
        }
        #region PLC_人員資料_接收設備資料
        PLC_Device PLC_Device_人員資料_接收設備資料 = new PLC_Device("");
        int cnt_Program_人員資料_接收設備資料 = 65534;
        void sub_Program_人員資料_接收設備資料()
        {
            if (this.plC_ScreenPage_Main.PageText == "人員資料")
            {
                PLC_Device_人員資料_接收設備資料.Bool = true;
            }
            else
            {
                PLC_Device_人員資料_接收設備資料.Bool = false;
            }
            if (cnt_Program_人員資料_接收設備資料 == 65534)
            {
                PLC_Device_人員資料_接收設備資料.Bool = false;
                cnt_Program_人員資料_接收設備資料 = 65535;
            }
            if (cnt_Program_人員資料_接收設備資料 == 65535) cnt_Program_人員資料_接收設備資料 = 1;
            if (cnt_Program_人員資料_接收設備資料 == 1) cnt_Program_人員資料_接收設備資料_檢查按下(ref cnt_Program_人員資料_接收設備資料);
            if (cnt_Program_人員資料_接收設備資料 == 2) cnt_Program_人員資料_接收設備資料_初始化(ref cnt_Program_人員資料_接收設備資料);
            if (cnt_Program_人員資料_接收設備資料 == 3) cnt_Program_人員資料_接收設備資料 = 65500;
            if (cnt_Program_人員資料_接收設備資料 > 1) cnt_Program_人員資料_接收設備資料_檢查放開(ref cnt_Program_人員資料_接收設備資料);

            if (cnt_Program_人員資料_接收設備資料 == 65500)
            {
                PLC_Device_人員資料_接收設備資料.Bool = false;
                cnt_Program_人員資料_接收設備資料 = 65535;
            }
        }
        void cnt_Program_人員資料_接收設備資料_檢查按下(ref int cnt)
        {
            if ((PLC_Device_人員資料_接收設備資料.Bool == true)) cnt++;
        }
        void cnt_Program_人員資料_接收設備資料_檢查放開(ref int cnt)
        {
            if (!(PLC_Device_人員資料_接收設備資料.Bool == false)) cnt = 65500;
        }
        void cnt_Program_人員資料_接收設備資料_初始化(ref int cnt)
        {
            List<RFID_FX600lib.RFID_FX600_UI.RFID_Device> list_RFID_Devices = this.rfiD_FX600_UI.Get_RFID();
            if (list_RFID_Devices.Count > 0)
            {
                this.Invoke(new Action(delegate
                {
                    this.rJ_TextBox_人員資料_卡號.Text = list_RFID_Devices[0].UID;
                }));

            }

            cnt++;
        }


















        #endregion

        #region Function
        private string Function_人員資料_檢查內容(object[] value)
        {
            string str_error = "";
            List<string> list_error = new List<string>();
            if (value[(int)enum_人員資料.姓名].ObjectToString().StringIsEmpty())
            {
                list_error.Add("'姓名'欄位不得空白!");
            }
            if (value[(int)enum_人員資料.ID].ObjectToString().StringIsEmpty())
            {
                list_error.Add("'ID'欄位不得空白!");
            }
            if (value[(int)enum_人員資料.密碼].ObjectToString().StringIsEmpty())
            {
                list_error.Add("'密碼'欄位不得空白!");
            }
            for (int i = 0; i < list_error.Count; i++)
            {
                str_error += $"{(i + 1).ToString("00")}. {list_error[i]}";
                if (i != list_error.Count - 1) str_error += "\n";
            }
            return str_error;
        }
        private void Function_人員資料_清除內容()
        {
            this.Invoke(new Action(delegate
            {
                this.rJ_TextBox_人員資料_ID.Text = "";
                this.rJ_TextBox_人員資料_姓名.Text = "";
                this.rJ_TextBox_人員資料_密碼.Text = "";
                this.rJ_TextBox_人員資料_單位.Text = "";
                this.rJ_TextBox_人員資料_卡號.Text = "";
                this.textBox_人員資料_顏色.Text = colorDialog.Color.ToColorString();
                this.comboBox_人員資料_權限等級.Text = "";
                this.rJ_TextBox_人員資料_一維條碼.Text = "";
                this.rJ_TextBox_人員資料_識別圖案.Text = "";
            }));

        }
        private void Function_人員資料_登錄資料()
        {
            string 性別 = rJ_RatioButton_人員資料_男.Checked ? "男" : "女";
            List<object[]> list_value = this.sqL_DataGridView_人員資料.SQL_GetAllRows(false);
            List<object[]> list_value_buf = new List<object[]>();
            list_value_buf = list_value.GetRows((int)enum_人員資料.ID, rJ_TextBox_人員資料_ID.Text);
            object[] value = new object[new enum_人員資料().GetLength()];
            if (list_value_buf.Count == 0)
            {
                value[(int)enum_人員資料.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_人員資料.ID] = this.rJ_TextBox_人員資料_ID.Text;
                value[(int)enum_人員資料.姓名] = this.rJ_TextBox_人員資料_姓名.Text;
                value[(int)enum_人員資料.性別] = 性別;
                value[(int)enum_人員資料.密碼] = this.rJ_TextBox_人員資料_密碼.Text;
                value[(int)enum_人員資料.單位] = this.rJ_TextBox_人員資料_單位.Text;
                value[(int)enum_人員資料.卡號] = this.rJ_TextBox_人員資料_卡號.Text;
                value[(int)enum_人員資料.權限等級] = this.comboBox_人員資料_權限等級.Text;
                value[(int)enum_人員資料.顏色] = this.textBox_人員資料_顏色.Text;
                value[(int)enum_人員資料.一維條碼] = this.rJ_TextBox_人員資料_一維條碼.Text;
                value[(int)enum_人員資料.識別圖案] = this.rJ_TextBox_人員資料_識別圖案.Text;
                string str_error = this.Function_人員資料_檢查內容(value);
                if (!str_error.StringIsEmpty())
                {
                    MyMessageBox.ShowDialog(str_error);
                    return;
                }
                this.sqL_DataGridView_人員資料.SQL_AddRow(value, true);
            }
            else
            {
                if (MyMessageBox.ShowDialog("此ID已註冊,是否覆寫?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
                {
                    value = list_value_buf[0];
                    value[(int)enum_人員資料.ID] = this.rJ_TextBox_人員資料_ID.Text;
                    value[(int)enum_人員資料.姓名] = this.rJ_TextBox_人員資料_姓名.Text;
                    value[(int)enum_人員資料.性別] = 性別;
                    value[(int)enum_人員資料.密碼] = this.rJ_TextBox_人員資料_密碼.Text;
                    value[(int)enum_人員資料.單位] = this.rJ_TextBox_人員資料_單位.Text;
                    value[(int)enum_人員資料.卡號] = this.rJ_TextBox_人員資料_卡號.Text;
                    value[(int)enum_人員資料.權限等級] = this.comboBox_人員資料_權限等級.Text;
                    value[(int)enum_人員資料.顏色] = this.textBox_人員資料_顏色.Text;
                    value[(int)enum_人員資料.一維條碼] = this.rJ_TextBox_人員資料_一維條碼.Text;
                    value[(int)enum_人員資料.識別圖案] = this.rJ_TextBox_人員資料_識別圖案.Text;
                    string str_error = this.Function_人員資料_檢查內容(value);
                    if (!str_error.StringIsEmpty())
                    {
                        MyMessageBox.ShowDialog(str_error);
                        return;
                    }
                    this.sqL_DataGridView_人員資料.SQL_ReplaceExtra(value, true);
                }
            }

            Function_人員資料_清除內容();
        }
        private void Function_人員資料_匯出()
        {
            saveFileDialog_SaveExcel.OverwritePrompt = false;
            if (saveFileDialog_SaveExcel.ShowDialog(this) == DialogResult.OK)
            {
                DataTable datatable = new DataTable();
                datatable = sqL_DataGridView_人員資料.GetDataTable();
                datatable = datatable.ReorderTable(new enum_人員資料_匯出());
                string Extension = System.IO.Path.GetExtension(this.saveFileDialog_SaveExcel.FileName);
                if (Extension == ".txt")
                {
                    CSVHelper.SaveFile(datatable, this.saveFileDialog_SaveExcel.FileName);
                    MyMessageBox.ShowDialog("匯出完成!");
                }
                else if (Extension == ".xls")
                {
                    MyOffice.ExcelClass.NPOI_SaveFile(datatable, this.saveFileDialog_SaveExcel.FileName);
                    MyMessageBox.ShowDialog("匯出完成!");
                }
            }
        }
        private void Function_人員資料_匯入()
        {
            if (openFileDialog_LoadExcel.ShowDialog(this) == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable dataTable = new DataTable();
                string Extension = System.IO.Path.GetExtension(this.openFileDialog_LoadExcel.FileName);

                if (Extension == ".txt")
                {
                    CSVHelper.LoadFile(this.openFileDialog_LoadExcel.FileName, 0, dataTable);
                }
                else if (Extension == ".xls")
                {
                    dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
                }
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗,請檢查是否檔案開啟中!");
                    this.Cursor = Cursors.Default;
                    return;
                }
                DataTable datatable_buf = dataTable.ReorderTable(new enum_人員資料_匯入());
                if (datatable_buf == null)
                {
                    MyMessageBox.ShowDialog("匯入檔案,資料錯誤!");
                    return;
                }
                List<object[]> list_LoadValue = datatable_buf.DataTableToRowList();
                List<object[]> list_SQL_Value = this.sqL_DataGridView_人員資料.SQL_GetAllRows(false);
                List<object[]> list_Add = new List<object[]>();
                List<object[]> list_Delete_ColumnName = new List<object[]>();
                List<object[]> list_Delete_SerchValue = new List<object[]>();
                List<string> list_Replace_SerchValue = new List<string>();
                List<object[]> list_Replace_Value = new List<object[]>();
                List<object[]> list_SQL_Value_buf = new List<object[]>();

                for (int i = 0; i < list_LoadValue.Count; i++)
                {
                    object[] value_load = list_LoadValue[i];
                    value_load = value_load.CopyRow(new enum_人員資料_匯入(), new enum_人員資料());
                    if (!Function_人員資料_檢查內容(value_load).StringIsEmpty()) continue;

                    string 性別 = value_load[(int)enum_人員資料.性別].ObjectToString();
                    string 權限等級 = value_load[(int)enum_人員資料.權限等級].ObjectToString();
                    if (!(性別 == "男" || 性別 == "女")) 性別 = "男";
                    if (權限等級.StringToInt32() <= 0 || 權限等級.StringToInt32() > 20) 權限等級 = "01";
                    value_load[(int)enum_人員資料.性別] = 性別;
                    value_load[(int)enum_人員資料.權限等級] = 權限等級;

                    list_SQL_Value_buf = list_SQL_Value.GetRows((int)enum_人員資料.ID, value_load[(int)enum_人員資料.ID].ObjectToString());
                    if (list_SQL_Value_buf.Count > 0)
                    {
                        object[] value_SQL = list_SQL_Value_buf[0];
                        value_load[(int)enum_人員資料.GUID] = value_SQL[(int)enum_人員資料.GUID];
                        value_load[(int)enum_人員資料.權限等級] = value_SQL[(int)enum_人員資料.權限等級];
                        bool flag_Equal = value_load.IsEqual(value_SQL);
                        if (!flag_Equal)
                        {
                            list_Replace_SerchValue.Add(value_load[(int)enum_人員資料.GUID].ObjectToString());
                            list_Replace_Value.Add(value_load);
                        }
                    }
                    else
                    {
                        value_load[(int)enum_人員資料.GUID] = Guid.NewGuid().ToString();
                        list_Add.Add(value_load);
                    }
                }
                this.sqL_DataGridView_人員資料.SQL_AddRows(list_Add, false);
                this.sqL_DataGridView_人員資料.SQL_ReplaceExtra(enum_人員資料.GUID.GetEnumName(), list_Replace_SerchValue, list_Replace_Value, false);
                this.sqL_DataGridView_人員資料.SQL_GetAllRows(true);
                this.Cursor = Cursors.Default;
                MyMessageBox.ShowDialog("匯入完成!");
            }
            this.Cursor = Cursors.Default;

        }

        private void Function_登入權限資料_取得權限(int level)
        {
            LoginDataWebAPI.Class_login_data class_Login_Data = this.loginUI.Get_login_data(level);
            if (class_Login_Data != null)
            {
                for (int i = 0; i < class_Login_Data.data.Count; i++)
                {
                    this.List_PLC_Device_權限管理[i].Bool = class_Login_Data.data[i];
                }
            }
        }
        private void Function_登入權限資料_最高權限()
        {
            for (int i = 0; i < 256; i++)
            {
                this.List_PLC_Device_權限管理[i].Bool = true;
            }
            PLC_Device_最高權限.Bool = true;
        }
        private void Function_登入權限資料_清除權限()
        {
            for (int i = 0; i < 256; i++)
            {
                this.List_PLC_Device_權限管理[i].Bool = false;
            }
            PLC_Device_最高權限.Bool = false;
        }
        #endregion
        #region Event
        private void SqL_DataGridView_人員資料_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Dialog_ContextMenuStrip dialog_ContextMenuStrip = new Dialog_ContextMenuStrip(new ContextMenuStrip_人員資料());
                if (dialog_ContextMenuStrip.ShowDialog() == DialogResult.Yes)
                {
                    if (dialog_ContextMenuStrip.Value == ContextMenuStrip_人員資料.匯入.GetEnumName())
                    {
                        Function_人員資料_匯入();
                    }
                    else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_人員資料.匯出.GetEnumName())
                    {
                        Function_人員資料_匯出();
                    }
                    else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_人員資料.匯出選取資料.GetEnumName())
                    {
                        saveFileDialog_SaveExcel.OverwritePrompt = false;
                        if (saveFileDialog_SaveExcel.ShowDialog(this) == DialogResult.OK)
                        {
                            DataTable datatable = new DataTable();
                            datatable = sqL_DataGridView_人員資料.GetSelectRowsDataTable();
                            datatable = datatable.ReorderTable(new enum_人員資料_匯出());
                            CSVHelper.SaveFile(datatable, saveFileDialog_SaveExcel.FileName);
                            MyMessageBox.ShowDialog("匯出完成!");
                        }
                    }
                    else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_人員資料.刪除選取資料.GetEnumName())
                    {
                        DialogResult Result = MyMessageBox.ShowDialog("是否刪除選取欄位資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel);
                        if (Result == System.Windows.Forms.DialogResult.Yes)
                        {
                            List<object[]> list_value = this.sqL_DataGridView_人員資料.Get_All_Select_RowsValues();
                            this.sqL_DataGridView_人員資料.SQL_DeleteExtra(list_value, true);
                        }
                    }
                    else if (dialog_ContextMenuStrip.Value == ContextMenuStrip_人員資料.登錄資料.GetEnumName())
                    {
                        Function_人員資料_登錄資料();
                    }
                }
            }
        }
        private void button_人員資料_顏色選擇_Click(object sender, EventArgs e)
        {
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_人員資料_顏色.Text = this.colorDialog.Color.ToColorString();
                textBox_人員資料_顏色.BackColor = textBox_人員資料_顏色.Text.ToColor();
            }
        }
        private void SqL_DataGridView_人員資料_DataGridRefreshEvent()
        {
            for (int i = 0; i < this.sqL_DataGridView_人員資料.dataGridView.Rows.Count; i++)
            {
                Color color = this.sqL_DataGridView_人員資料.dataGridView.Rows[i].Cells[enum_人員資料.顏色.GetEnumName()].Value.ObjectToString().ToColor();
                this.sqL_DataGridView_人員資料.dataGridView.Rows[i].Cells[enum_人員資料.顏色.GetEnumName()].Style.BackColor = color;
                this.sqL_DataGridView_人員資料.dataGridView.Rows[i].Cells[enum_人員資料.顏色.GetEnumName()].Style.ForeColor = color;
            }
        }
        private void SqL_DataGridView_人員資料_RowEnterEvent(object[] RowValue)
        {
            //rJ_TextBox_人員資料_ID.Text = RowValue[(int)enum_人員資料.ID].ObjectToString();
            //rJ_TextBox_人員資料_姓名.Text = RowValue[(int)enum_人員資料.姓名].ObjectToString();
            //rJ_TextBox_人員資料_密碼.Text = RowValue[(int)enum_人員資料.密碼].ObjectToString();
            //rJ_TextBox_人員資料_單位.Text = RowValue[(int)enum_人員資料.單位].ObjectToString();
            //comboBox_人員資料_權限等級.Text = RowValue[(int)enum_人員資料.權限等級].ObjectToString();
            //textBox_人員資料_顏色.Text = RowValue[(int)enum_人員資料.顏色].ObjectToString();
            //textBox_人員資料_顏色.BackColor = textBox_人員資料_顏色.Text.ToColor();
            //rJ_TextBox_人員資料_卡號.Text = RowValue[(int)enum_人員資料.卡號].ObjectToString();
            //rJ_TextBox_人員資料_一維條碼.Text = RowValue[(int)enum_人員資料.一維條碼].ObjectToString();
            //rJ_TextBox_人員資料_識別圖案.Text = RowValue[(int)enum_人員資料.識別圖案].ObjectToString();


            //string 性別 = RowValue[(int)enum_人員資料.性別].ObjectToString();
            //if (性別 == "男") rJ_RatioButton_人員資料_男.Checked = true;
            //else rJ_RatioButton_人員資料_女.Checked = true;
        }
        private void SqL_DataGridView_人員資料_RowDoubleClickEvent(object[] RowValue)
        {
            rJ_TextBox_人員資料_ID.Text = RowValue[(int)enum_人員資料.ID].ObjectToString();
            rJ_TextBox_人員資料_姓名.Text = RowValue[(int)enum_人員資料.姓名].ObjectToString();
            rJ_TextBox_人員資料_密碼.Text = RowValue[(int)enum_人員資料.密碼].ObjectToString();
            rJ_TextBox_人員資料_單位.Text = RowValue[(int)enum_人員資料.單位].ObjectToString();
            comboBox_人員資料_權限等級.Text = RowValue[(int)enum_人員資料.權限等級].ObjectToString();
            textBox_人員資料_顏色.Text = RowValue[(int)enum_人員資料.顏色].ObjectToString();
            textBox_人員資料_顏色.BackColor = textBox_人員資料_顏色.Text.ToColor();
            rJ_TextBox_人員資料_卡號.Text = RowValue[(int)enum_人員資料.卡號].ObjectToString();
            rJ_TextBox_人員資料_一維條碼.Text = RowValue[(int)enum_人員資料.一維條碼].ObjectToString();
            rJ_TextBox_人員資料_識別圖案.Text = RowValue[(int)enum_人員資料.識別圖案].ObjectToString();


            string 性別 = RowValue[(int)enum_人員資料.性別].ObjectToString();
            if (性別 == "男") rJ_RatioButton_人員資料_男.Checked = true;
            else rJ_RatioButton_人員資料_女.Checked = true;
        }

        private void PlC_RJ_ComboBox_權限管理_權限等級_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int level = plC_RJ_ComboBox_權限管理_權限等級.Text.StringToInt32();
            if (level > 0)
            {
                List<LoginDataWebAPI.Class_login_data> List_class_Login_Data_buf = new List<LoginDataWebAPI.Class_login_data>();
                List_class_Login_Data_buf = (from value in List_class_Login_Data
                                             where value.level.StringToInt32() == level
                                             select value).ToList();
                if (List_class_Login_Data_buf.Count > 0)
                {
                    this.loginIndex_Pannel.Set_Login_Data(List_class_Login_Data_buf[0]);
                }
            }
        }
        private void PlC_Button_權限設定_設定至Server_MouseDownEvent(MouseEventArgs mevent)
        {
            for (int i = 0; i < List_class_Login_Data.Count; i++)
            {
                this.loginUI.Set_login_data(List_class_Login_Data[i]);
            }
            this.Invoke(new Action(delegate
            {
                MyMessageBox.ShowDialog("權限更動完成!");
            }));
        }
        private void PlC_RJ_Button_人員資料_刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                DialogResult Result = MyMessageBox.ShowDialog("是否刪除選取欄位資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel);
                if (Result == System.Windows.Forms.DialogResult.Yes)
                {
                    List<object[]> list_value = this.sqL_DataGridView_人員資料.Get_All_Select_RowsValues();
                    this.sqL_DataGridView_人員資料.SQL_DeleteExtra(list_value, true);
                }
            }));
        }
        private void PlC_RJ_Button_人員資料_登錄_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Function_人員資料_登錄資料();
            }));
        }
        private void PlC_RJ_Button_人員資料_匯入_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Function_人員資料_匯入();
            }));
        }
        private void PlC_RJ_Button_人員資料_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Function_人員資料_匯出();
            }));
        }
        private void PlC_RJ_Button_人員資料_清除內容_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Function_人員資料_清除內容();
        }
        #endregion
    }
}
