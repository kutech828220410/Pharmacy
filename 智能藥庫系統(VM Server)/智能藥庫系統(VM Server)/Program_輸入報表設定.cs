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
namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {
        private enum enum_寫入報表設定_類別
        {
            OPD_消耗帳,
            PHR_消耗帳,
            PHER_消耗帳,
            公藥_消耗帳,
            其他,
        }
        private enum enum_寫入報表設定
        {
            GUID,
            編號,
            檔名,
            檔案位置,
            類別,
            更新每日,
            更新每週,
            更新每月,
            備註內容,
            out_db_server,
            out_db_name,
            out_db_username,
            out_db_password,
            out_db_port,
            fileServer_username,
            fileServer_password,
        }
        public enum enum_寫入報表設定_匯出
        {
            檔名,
            檔案位置,
            類別,
            更新每日,
            更新每週,
            更新每月,
            out_db_server,
            out_db_name,
            out_db_username,
            out_db_password,
            out_db_port,
            fileServer_username,
            fileServer_password,
        }
        public enum enum_寫入報表設定_匯入
        {
            檔名,
            檔案位置,
            類別,
            更新每日,
            更新每週,
            更新每月,
            out_db_server,
            out_db_name,
            out_db_username,
            out_db_password,
            out_db_port,
            fileServer_username,
            fileServer_password,
        }
        private List<CheckBox> list_寫入報表設定_CheckBox_每週 = new List<CheckBox>();
        private List<CheckBox> list_寫入報表設定_CheckBox_每月 = new List<CheckBox>();

        private void sub_Program_寫入報表設定_Init()
        {
            SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_寫入報表設定, this.dBConfigClass.DB_Basic);

            this.list_寫入報表設定_CheckBox_每週.Add(checkBox_寫入報表設定_週期選擇_週日);
            this.list_寫入報表設定_CheckBox_每週.Add(checkBox_寫入報表設定_週期選擇_週一);
            this.list_寫入報表設定_CheckBox_每週.Add(checkBox_寫入報表設定_週期選擇_週二);
            this.list_寫入報表設定_CheckBox_每週.Add(checkBox_寫入報表設定_週期選擇_週三);
            this.list_寫入報表設定_CheckBox_每週.Add(checkBox_寫入報表設定_週期選擇_週四);
            this.list_寫入報表設定_CheckBox_每週.Add(checkBox_寫入報表設定_週期選擇_週五);
            this.list_寫入報表設定_CheckBox_每週.Add(checkBox_寫入報表設定_週期選擇_週六);
          

            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_01);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_02);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_03);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_04);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_05);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_06);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_07);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_08);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_09);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_10);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_11);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_12);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_13);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_14);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_15);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_16);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_17);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_18);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_19);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_20);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_21);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_22);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_23);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_24);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_25);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_26);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_27);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_28);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_29);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_30);
            this.list_寫入報表設定_CheckBox_每月.Add(checkBox_寫入報表設定_週期選擇_31);

            for (int i = 0; i < this.list_寫入報表設定_CheckBox_每週.Count; i++)
            {
                this.list_寫入報表設定_CheckBox_每週[i].CheckedChanged += CheckBox_寫入報表設定_週期選擇_每週_CheckedChanged;
            }
            for (int i = 0; i < this.list_寫入報表設定_CheckBox_每月.Count; i++)
            {
                this.list_寫入報表設定_CheckBox_每月[i].CheckedChanged += CheckBox_寫入報表設定_週期選擇_每月_CheckedChanged;
            }

            this.sqL_DataGridView_寫入報表設定.Init();
            if(!this.sqL_DataGridView_寫入報表設定.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_寫入報表設定.SQL_CreateTable();
            }
            this.sqL_DataGridView_寫入報表設定.RowEnterEvent += SqL_DataGridView_寫入報表設定_RowEnterEvent;
            this.sqL_DataGridView_寫入報表設定.RowDoubleClickEvent += SqL_DataGridView_寫入報表設定_RowDoubleClickEvent;
            this.sqL_DataGridView_寫入報表設定.DataGridRefreshEvent += SqL_DataGridView_寫入報表設定_DataGridRefreshEvent;
            this.sqL_DataGridView_寫入報表設定.DataGridRowsChangeRefEvent += SqL_DataGridView_寫入報表設定_DataGridRowsChangeRefEvent;

            this.rJ_ComboBox_寫入報表設定_類別.DataSource = new enum_寫入報表設定_類別().GetEnumNames();

            this.plC_RJ_Button_寫入報表設定_登錄.MouseDownEvent += PlC_RJ_Button_寫入報表設定_登錄_MouseDownEvent;
            this.plC_RJ_Button_寫入報表設定_瀏覽.MouseDownEvent += PlC_RJ_Button_寫入報表設定_瀏覽_MouseDownEvent;
            this.plC_RJ_Button_寫入報表設定_刪除.MouseDownEvent += PlC_RJ_Button_寫入報表設定_刪除_MouseDownEvent;
            this.plC_RJ_Button_寫入報表設定_清除內容.MouseDownEvent += PlC_RJ_Button_寫入報表設定_清除內容_MouseDownEvent;
            this.plC_RJ_Button_寫入報表設定_匯出.MouseDownEvent += PlC_RJ_Button_寫入報表設定_匯出_MouseDownEvent;
            this.plC_RJ_Button_寫入報表設定_匯入.MouseDownEvent += PlC_RJ_Button_寫入報表設定_匯入_MouseDownEvent;
            this.plC_RJ_Button_寫入報表設定_重新排序.MouseDownEvent += PlC_RJ_Button_寫入報表設定_重新排序_MouseDownEvent;
            this.plC_RJ_Button_寫入報表設定_向上排序.MouseDownEvent += PlC_RJ_Button_寫入報表設定_向上排序_MouseDownEvent;
            this.plC_RJ_Button_寫入報表設定_向下排序.MouseDownEvent += PlC_RJ_Button_寫入報表設定_向下排序_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_寫入報表設定);
        }

        private bool flag_Program_寫入報表設定 = false;
        private void sub_Program_寫入報表設定()
        {
            if(this.plC_ScreenPage_Main.PageText == "寫入報表設定")
            {
                if(this.flag_Program_寫入報表設定 == false)
                {
                    this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(true);
                    this.PlC_RJ_Button_寫入報表設定_清除內容_MouseDownEvent(null);
                    this.flag_Program_寫入報表設定 = true;
                }
            }
            else
            {
                this.flag_Program_寫入報表設定 = false;
            }
        }
        #region Function
        private string Function_寫入報表設定_取得每日更新時間()
        {
            return $"{rJ_ComboBox_寫入報表設定_週期選擇_Hour.Texts}{rJ_ComboBox_寫入報表設定_週期選擇_Min.Texts}";
        }
        private void Function_寫入報表設定_寫入每日更新時間(string str)
        {
            if (str.Length != 4) return;
            string str0 = str.Substring(0, 2);
            string str1 = str.Substring(2, 2);
            rJ_ComboBox_寫入報表設定_週期選擇_Hour.Texts = str0;
            rJ_ComboBox_寫入報表設定_週期選擇_Min.Texts = str1;
        }
        private int Function_寫入報表設定_取得每週更新時間()
        {
            int value = 0;
            for(int i = 0; i < this.list_寫入報表設定_CheckBox_每週.Count; i++)
            {
                value += this.list_寫入報表設定_CheckBox_每週[i].Checked ? 1 << i : 0;
            }         
            return value;
        }
        private void Function_寫入報表設定_寫入每週更新時間(int value)
        {
            this.Invoke(new Action(delegate
            {
                for (int i = 0; i < this.list_寫入報表設定_CheckBox_每週.Count; i++)
                {
                    this.list_寫入報表設定_CheckBox_每週[i].Checked = ((value >> i) % 2) == 1;
                }              
            }));
        }
        private int Function_寫入報表設定_取得每月更新時間()
        {
            int value = 0;
            for (int i = 0; i < this.list_寫入報表設定_CheckBox_每月.Count; i++)
            {
                value += this.list_寫入報表設定_CheckBox_每月[i].Checked ? 1 << i : 0;
            }
            return value;
        }
        private void Function_寫入報表設定_寫入每月更新時間(int value)
        {
            this.Invoke(new Action(delegate
            {
                for (int i = 0; i < this.list_寫入報表設定_CheckBox_每月.Count; i++)
                {
                    this.list_寫入報表設定_CheckBox_每月[i].Checked = ((value >> i) % 2) == 1;
                }
            }));
        }

        #endregion
        #region Event
        private void SqL_DataGridView_寫入報表設定_RowDoubleClickEvent(object[] RowValue)
        {
            SqL_DataGridView_寫入報表設定_RowEnterEvent(RowValue);
        }
        private void SqL_DataGridView_寫入報表設定_RowEnterEvent(object[] RowValue)
        {
            this.rJ_TextBox_寫入報表設定_檔名.Texts = RowValue[(int)enum_寫入報表設定.檔名].ObjectToString();
            this.rJ_TextBox_寫入報表設定_檔案位置.Texts = RowValue[(int)enum_寫入報表設定.檔案位置].ObjectToString();
            this.rJ_ComboBox_寫入報表設定_類別.Texts = RowValue[(int)enum_寫入報表設定.類別].ObjectToString();
            this.rJ_TextBox_寫入報表設定_DB_Server.Texts = RowValue[(int)enum_寫入報表設定.out_db_server].ObjectToString();
            this.rJ_TextBox_寫入報表設定_DB_Name.Texts = RowValue[(int)enum_寫入報表設定.out_db_name].ObjectToString();
            this.rJ_TextBox_寫入報表設定_DB_UserName.Texts = RowValue[(int)enum_寫入報表設定.out_db_username].ObjectToString();
            this.rJ_TextBox_寫入報表設定_DB_Password.Texts = RowValue[(int)enum_寫入報表設定.out_db_password].ObjectToString();
            this.rJ_TextBox_寫入報表設定_DB_Port.Texts = RowValue[(int)enum_寫入報表設定.out_db_port].ObjectToString();
            this.rJ_TextBox_寫入報表設定_備註.Texts = RowValue[(int)enum_寫入報表設定.備註內容].ObjectToString();
            this.rJ_TextBox_寫入報表設定_FileServer_UserName.Texts = RowValue[(int)enum_寫入報表設定.fileServer_username].ObjectToString();
            this.rJ_TextBox_寫入報表設定_FileServer_Password.Texts = RowValue[(int)enum_寫入報表設定.fileServer_password].ObjectToString();
            this.Function_寫入報表設定_寫入每日更新時間(RowValue[(int)enum_寫入報表設定.更新每日].ObjectToString());
            this.Function_寫入報表設定_寫入每週更新時間(RowValue[(int)enum_寫入報表設定.更新每週].ObjectToString().StringToInt32());
            this.Function_寫入報表設定_寫入每月更新時間(RowValue[(int)enum_寫入報表設定.更新每月].ObjectToString().StringToInt32());
        }
        private void SqL_DataGridView_寫入報表設定_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_寫入報表設定());
        }
        private void SqL_DataGridView_寫入報表設定_DataGridRefreshEvent()
        {

        }
        private void PlC_RJ_Button_寫入報表設定_重新排序_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = MyMessageBox.ShowDialog("確認全部重新排序?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel);
            }));
            if (dialogResult != DialogResult.Yes) return;
            List<object[]> list_value = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);

            for (int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_寫入報表設定.編號] = (i + 1).ToString();
            }

            this.sqL_DataGridView_寫入報表設定.SQL_ReplaceExtra(list_value, true);
        }
        private void PlC_RJ_Button_寫入報表設定_登錄_MouseDownEvent(MouseEventArgs mevent)
        {

            string 檔名 = this.rJ_TextBox_寫入報表設定_檔名.Texts;
            string 檔案位置 = this.rJ_TextBox_寫入報表設定_檔案位置.Texts;
            string 類別 = this.rJ_ComboBox_寫入報表設定_類別.Texts;
            string db_server = this.rJ_TextBox_寫入報表設定_DB_Server.Texts;
            string db_name = this.rJ_TextBox_寫入報表設定_DB_Name.Texts;
            string db_username = this.rJ_TextBox_寫入報表設定_DB_UserName.Texts;
            string db_password = this.rJ_TextBox_寫入報表設定_DB_Password.Texts;
            string db_port = this.rJ_TextBox_寫入報表設定_DB_Port.Texts;
            string 備註 = this.rJ_TextBox_寫入報表設定_備註.Texts;

            string fileServer_username = this.rJ_TextBox_寫入報表設定_FileServer_UserName.Texts;
            string fileServer_password = this.rJ_TextBox_寫入報表設定_FileServer_Password.Texts;

            string str_error = "";
            if (檔名.StringIsEmpty()) str_error += $"'檔名'不得為空白!\n";
            if (檔案位置.StringIsEmpty()) str_error += $"'檔案位置'不得為空白!\n";
            //if (db_server.StringIsEmpty()) str_error += $"'db_server'不得為空白!\n";
            //if (db_name.StringIsEmpty()) str_error += $"'db_name'不得為空白!\n";
            //if (db_username.StringIsEmpty()) str_error += $"'db_username'不得為空白!\n";
            //if (db_password.StringIsEmpty()) str_error += $"'db_password'不得為空白!\n";
            //if (db_port.StringIsEmpty()) str_error += $"'db_port'不得為空白!\n";
            //if(db_port.StringToInt32() < 0) str_error += $"'db_port'為非法字元!\n";

            if(!str_error.StringIsEmpty())
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog(str_error);
                }));
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
            List<object[]> list_value_buf = new List<object[]>();
            list_value_buf = list_value.GetRows((int)enum_寫入報表設定.檔名, 檔名);
            if(list_value_buf.Count == 0)
            {
                object[] value = new object[new enum_寫入報表設定().GetLength()];
                value[(int)enum_寫入報表設定.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_寫入報表設定.編號] = (list_value.Count +1);
                value[(int)enum_寫入報表設定.檔名] = 檔名;
                value[(int)enum_寫入報表設定.檔案位置] = 檔案位置;
                value[(int)enum_寫入報表設定.類別] = 類別;
                value[(int)enum_寫入報表設定.更新每日] = this.Function_寫入報表設定_取得每日更新時間();
                value[(int)enum_寫入報表設定.更新每週] = this.Function_寫入報表設定_取得每週更新時間();
                value[(int)enum_寫入報表設定.更新每月] = this.Function_寫入報表設定_取得每月更新時間();
                value[(int)enum_寫入報表設定.備註內容] = 備註;
                value[(int)enum_寫入報表設定.out_db_server] = db_server;
                value[(int)enum_寫入報表設定.out_db_name] = db_name;
                value[(int)enum_寫入報表設定.out_db_username] = db_username;
                value[(int)enum_寫入報表設定.out_db_password] = db_password;
                value[(int)enum_寫入報表設定.out_db_port] = db_port;
                value[(int)enum_寫入報表設定.fileServer_username] = fileServer_username;
                value[(int)enum_寫入報表設定.fileServer_password] = fileServer_password;

                this.sqL_DataGridView_寫入報表設定.SQL_AddRow(value, true);

            }
            else
            {
                object[] value = list_value_buf[0];
                value[(int)enum_寫入報表設定.檔名] = 檔名;
                value[(int)enum_寫入報表設定.檔案位置] = 檔案位置;
                value[(int)enum_寫入報表設定.類別] = 類別;
                value[(int)enum_寫入報表設定.更新每日] = this.Function_寫入報表設定_取得每日更新時間();
                value[(int)enum_寫入報表設定.更新每週] = this.Function_寫入報表設定_取得每週更新時間();
                value[(int)enum_寫入報表設定.更新每月] = this.Function_寫入報表設定_取得每月更新時間();
                value[(int)enum_寫入報表設定.備註內容] = 備註;
                value[(int)enum_寫入報表設定.out_db_server] = db_server;
                value[(int)enum_寫入報表設定.out_db_name] = db_name;
                value[(int)enum_寫入報表設定.out_db_username] = db_username;
                value[(int)enum_寫入報表設定.out_db_password] = db_password;
                value[(int)enum_寫入報表設定.out_db_port] = db_port;
                value[(int)enum_寫入報表設定.fileServer_username] = fileServer_username;
                value[(int)enum_寫入報表設定.fileServer_password] = fileServer_password;

                this.sqL_DataGridView_寫入報表設定.SQL_ReplaceExtra(value, true);
            }
        }
        private void PlC_RJ_Button_寫入報表設定_瀏覽_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog.ShowDialog();
                if (dialogResult != DialogResult.OK) return;
                string fileName = this.openFileDialog.FileName.GetFileName();
                string filePath = this.openFileDialog.FileName.GetFilePath();

                this.rJ_TextBox_寫入報表設定_檔名.Texts = fileName;
                this.rJ_TextBox_寫入報表設定_檔案位置.Texts = filePath;
            }));   
        }
        private void PlC_RJ_Button_寫入報表設定_刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_寫入報表設定.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取資料!");
                }));
                return;
            }
            this.Invoke(new Action(delegate
            {
                if (MyMessageBox.ShowDialog("確認刪除選取資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
                this.sqL_DataGridView_寫入報表設定.SQL_DeleteExtra(list_value, true);
            }));

        }
        private void PlC_RJ_Button_寫入報表設定_清除內容_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                object[] RowValue = new object[new enum_寫入報表設定().GetLength()];
                this.rJ_TextBox_寫入報表設定_檔名.Texts = RowValue[(int)enum_寫入報表設定.檔名].ObjectToString();
                this.rJ_TextBox_寫入報表設定_檔案位置.Texts = RowValue[(int)enum_寫入報表設定.檔案位置].ObjectToString();
                this.rJ_TextBox_寫入報表設定_DB_Server.Texts = RowValue[(int)enum_寫入報表設定.out_db_server].ObjectToString();
                this.rJ_TextBox_寫入報表設定_DB_Name.Texts = RowValue[(int)enum_寫入報表設定.out_db_name].ObjectToString();
                this.rJ_TextBox_寫入報表設定_DB_UserName.Texts = RowValue[(int)enum_寫入報表設定.out_db_username].ObjectToString();
                this.rJ_TextBox_寫入報表設定_DB_Password.Texts = RowValue[(int)enum_寫入報表設定.out_db_password].ObjectToString();
                this.rJ_TextBox_寫入報表設定_DB_Port.Texts = RowValue[(int)enum_寫入報表設定.out_db_port].ObjectToString();
                this.rJ_TextBox_寫入報表設定_備註.Texts = RowValue[(int)enum_寫入報表設定.備註內容].ObjectToString();
                this.rJ_TextBox_寫入報表設定_FileServer_UserName.Texts = RowValue[(int)enum_寫入報表設定.fileServer_username].ObjectToString();
                this.rJ_TextBox_寫入報表設定_FileServer_Password.Texts = RowValue[(int)enum_寫入報表設定.fileServer_password].ObjectToString();

                this.rJ_ComboBox_寫入報表設定_週期選擇_Hour.SelectedIndex = 0;
                this.rJ_ComboBox_寫入報表設定_週期選擇_Min.SelectedIndex = 0;
            }));
           
        }
        private void PlC_RJ_Button_寫入報表設定_匯入_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult == DialogResult.OK)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.WaitCursor;
                }));

                DataTable dataTable = new DataTable();
                CSVHelper.LoadFile(this.openFileDialog_LoadExcel.FileName, 0, dataTable);
                DataTable datatable_buf = dataTable.ReorderTable(new enum_寫入報表設定_匯入());
                if (datatable_buf == null)
                {
                    this.Invoke(new Action(delegate
                    {
                        MyMessageBox.ShowDialog("匯入檔案,資料錯誤!");
                        this.Cursor = Cursors.Default;
                        return;
                    }));

                }
                List<object[]> list_LoadValue = datatable_buf.DataTableToRowList();
                List<object[]> list_SQL_Value = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
                List<object[]> list_Add = new List<object[]>();
                List<object[]> list_Add_buf = new List<object[]>();
                List<object[]> list_Delete_ColumnName = new List<object[]>();
                List<object[]> list_Delete_SerchValue = new List<object[]>();
                List<string> list_Replace_SerchValue = new List<string>();
                List<object[]> list_Replace_Value = new List<object[]>();
                List<object[]> list_SQL_Value_buf = new List<object[]>();
                for (int i = 0; i < list_LoadValue.Count; i++)
                {
                    object[] value_load = list_LoadValue[i];
                    value_load = value_load.CopyRow(new enum_寫入報表設定_匯入(), new enum_寫入報表設定());
                    list_SQL_Value_buf = list_SQL_Value.GetRows((int)enum_寫入報表設定.檔名, value_load[(int)enum_寫入報表設定.檔名].ObjectToString());
                    if (list_SQL_Value_buf.Count > 0)
                    {
                        object[] value_SQL = list_SQL_Value_buf[0];
                        value_load[(int)enum_寫入報表設定.GUID] = value_SQL[(int)enum_寫入報表設定.GUID];
                        value_load[(int)enum_寫入報表設定.編號] = value_SQL[(int)enum_寫入報表設定.編號];
                        value_load[(int)enum_寫入報表設定.備註內容] = value_SQL[(int)enum_寫入報表設定.備註內容];
                        bool flag_Equal = value_load.IsEqual(value_SQL, (int)enum_寫入報表設定.備註內容, (int)enum_寫入報表設定.編號);
                        if (!flag_Equal)
                        {
                            list_Replace_SerchValue.Add(value_load[(int)enum_寫入報表設定.GUID].ObjectToString());
                            list_Replace_Value.Add(value_load);
                        }
                    }
                    else
                    {
                        value_load[(int)enum_寫入報表設定.GUID] = Guid.NewGuid().ToString();
                        list_Add_buf = list_Add.GetRows((int)enum_寫入報表設定.檔名, value_load[(int)enum_寫入報表設定.檔名].ObjectToString());
                        if (list_Add_buf.Count == 0)
                        {
                            list_Add.Add(value_load);
                        }

                    }
                }
                this.sqL_DataGridView_寫入報表設定.SQL_AddRows(list_Add, false);
                this.sqL_DataGridView_寫入報表設定.SQL_ReplaceExtra(enum_寫入報表設定.GUID.GetEnumName(), list_Replace_SerchValue, list_Replace_Value, false);

                this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(true);
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                    MyMessageBox.ShowDialog("匯入完成!");
                }));
            }
        }
        private void PlC_RJ_Button_寫入報表設定_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.saveFileDialog_SaveExcel.ShowDialog();
            }));
            if (dialogResult == DialogResult.OK)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.WaitCursor;
                }));
                DataTable dataTable = this.sqL_DataGridView_寫入報表設定.GetDataTable();
                dataTable = dataTable.ReorderTable(new enum_寫入報表設定_匯出());
                CSVHelper.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));

            }
        }
        private void PlC_RJ_Button_寫入報表設定_向上排序_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_all_selected_values = this.sqL_DataGridView_寫入報表設定.Get_All_Select_RowsValues();
            if(list_all_selected_values.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取資料!");
                }));
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
            list_value.Sort(new ICP_寫入報表設定());

            int num = list_all_selected_values[0][(int)enum_寫入報表設定.編號].ObjectToString().StringToInt32();
            if (num == 1) return;

            List<object[]> current_value = list_value.GetRows((int)enum_寫入報表設定.編號, (num - 0).ToString());
            List<object[]> change_value = list_value.GetRows((int)enum_寫入報表設定.編號, (num - 1).ToString());
            if (current_value.Count == 0 || change_value.Count == 0) return;
            current_value[0][(int)enum_寫入報表設定.編號] = (num - 1).ToString();
            change_value[0][(int)enum_寫入報表設定.編號] = (num - 0).ToString();
            this.sqL_DataGridView_寫入報表設定.SQL_ReplaceExtra(list_value, true);
            this.sqL_DataGridView_寫入報表設定.SetSelectRow(this.sqL_DataGridView_寫入報表設定.GetSelectRow() - 1);
        }
        private void PlC_RJ_Button_寫入報表設定_向下排序_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_all_selected_values = this.sqL_DataGridView_寫入報表設定.Get_All_Select_RowsValues();
            if (list_all_selected_values.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取資料!");
                }));
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
            list_value.Sort(new ICP_寫入報表設定());

            int num = list_all_selected_values[0][(int)enum_寫入報表設定.編號].ObjectToString().StringToInt32();
            if (num == list_value.Count) return;

            List<object[]> current_value = list_value.GetRows((int)enum_寫入報表設定.編號, (num + 0).ToString());
            List<object[]> change_value = list_value.GetRows((int)enum_寫入報表設定.編號, (num + 1).ToString());
            if (current_value.Count == 0 || change_value.Count == 0) return;
            current_value[0][(int)enum_寫入報表設定.編號] = (num + 1).ToString();
            change_value[0][(int)enum_寫入報表設定.編號] = (num + 0).ToString();
            this.sqL_DataGridView_寫入報表設定.SQL_ReplaceExtra(list_value, true);
            this.sqL_DataGridView_寫入報表設定.SetSelectRow(this.sqL_DataGridView_寫入報表設定.GetSelectRow() + 1);
        }
        private void CheckBox_寫入報表設定_週期選擇_每週_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.list_寫入報表設定_CheckBox_每月.Count; i++)
            {
                this.list_寫入報表設定_CheckBox_每月[i].Checked = false;
            }
        }
        private void CheckBox_寫入報表設定_週期選擇_每月_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.list_寫入報表設定_CheckBox_每週.Count; i++)
            {
                this.list_寫入報表設定_CheckBox_每週[i].Checked = false;
            }
        }

        #endregion
        private class ICP_寫入報表設定 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                int temp0 = x[(int)enum_寫入報表設定.編號].ObjectToString().StringToInt32();
                int temp1 = y[(int)enum_寫入報表設定.編號].ObjectToString().StringToInt32();
                if (temp0 == -1) temp0 = 9999;
                if (temp1 == -1) temp1 = 9999;
                return temp0.CompareTo(temp1);

            }
        }
    }
}
