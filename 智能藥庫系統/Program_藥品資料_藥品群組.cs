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

namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        public enum enum_藥品資料_藥品群組
        {
            GUID,
            群組序號,
            群組名稱,
        }

        private void sub_Program_藥品資料_藥品群組_Init()
        {
            this.sqL_DataGridView_藥品資料_藥品群組.Init();
            if (!this.sqL_DataGridView_藥品資料_藥品群組.SQL_IsTableCreat()) this.sqL_DataGridView_藥品資料_藥品群組.SQL_CreateTable();
            Function_藥品資料_藥品群組_初始化表單();
            this.sqL_DataGridView_藥品資料_藥品群組.DataGridRowsChangeEvent += SqL_DataGridView_藥品資料_藥品群組_DataGridRowsChangeEvent;
            this.sqL_DataGridView_藥品資料_藥品群組.RowEnterEvent += SqL_DataGridView_藥品資料_藥品群組_RowEnterEvent;
            this.sqL_DataGridView_藥品資料_藥品群組.SQL_GetAllRows(false);

            this.rJ_TextBox_藥品資料_藥品群組_群組名稱.KeyPress += RJ_TextBox_藥品資料_藥品群組_群組名稱_KeyPress;
            this.plC_RJ_Button_藥品資料_藥品群組_寫入.MouseDownEvent += PlC_RJ_Button_藥品資料_藥品群組_寫入_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_藥品資料_藥品群組);
        }



        private bool flag_藥品資料_藥品群組_頁面更新 = false;
        private void sub_Program_藥品資料_藥品群組()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥品資料" && this.plC_ScreenPage_藥品資料.PageText == "藥品群組")
            {
                if (!this.flag_藥品資料_藥品群組_頁面更新)
                {
                    this.sqL_DataGridView_藥品資料_藥品群組.SQL_GetAllRows(true);
                    this.flag_藥品資料_藥品群組_頁面更新 = true;
                }
            }
            else
            {
                this.flag_藥品資料_藥品群組_頁面更新 = false;
            }
        }

        #region Function
        public class Icp_藥品資料_藥品群組 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                int index_0 = x[(int)enum_藥品資料_藥品群組.群組序號].ObjectToString().StringToInt32();
                int index_1 = y[(int)enum_藥品資料_藥品群組.群組序號].ObjectToString().StringToInt32();
                return index_0.CompareTo(index_1);
            }
        }
        private void Function_藥品資料_藥品群組_初始化表單()
        {
            List<object[]> list_value = sqL_DataGridView_藥品資料_藥品群組.SQL_GetAllRows(false);
            List<object[]> list_value_buf = new List<object[]>();
            List<object[]> list_Add = new List<object[]>();
            List<string[]> list_Replace_SerchValue = new List<string[]>();
            List<object[]> list_Replace_Value = new List<object[]>();
            List<object[]> list_Delete_ColumnName = new List<object[]>();
            List<object[]> list_Delete_SerchValue = new List<object[]>();
            for (int i = 0; i < list_value.Count; i++)
            {
                int index = list_value[i][(int)enum_藥品資料_藥品群組.群組序號].StringToInt32();
                if (index <= 0 || index > 20)
                {
                    list_Delete_ColumnName.Add(new string[] { enum_藥品資料_藥品群組.GUID.GetEnumName() });
                    list_Delete_SerchValue.Add(new string[] { list_value[i][(int)enum_藥品資料_藥品群組.GUID].ObjectToString() });
                }
            }
            for (int i = 1; i <= 20; i++)
            {
                list_value_buf = list_value.GetRows((int)enum_藥品資料_藥品群組.群組序號, i.ToString("00"));
                if (list_value_buf.Count == 0)
                {
                    object[] value = new object[new enum_藥品資料_藥品群組().GetEnumNames().Length];
                    value[(int)enum_藥品資料_藥品群組.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_藥品資料_藥品群組.群組序號] = i.ToString("00");
                    list_Add.Add(value);
                }
            }
            sqL_DataGridView_藥品資料_藥品群組.SQL_DeleteExtra(list_Delete_ColumnName, list_Delete_SerchValue, false);
            sqL_DataGridView_藥品資料_藥品群組.SQL_AddRows(list_Add, false);

        }
        private void Finction_藥品資料_藥品群組_序號轉名稱(List<object[]> RowsList, int Enum)
        {
            List<object[]> list_藥品資料_藥品群組 = sqL_DataGridView_藥品資料_藥品群組.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_藥品群組_buf = new List<object[]>();
            string 群組序號 = "";
            for (int i = 0; i < RowsList.Count; i++)
            {
                群組序號 = RowsList[i][Enum].ObjectToString();
                list_藥品資料_藥品群組_buf = list_藥品資料_藥品群組.GetRows((int)enum_藥品資料_藥品群組.群組序號, 群組序號);
                if (list_藥品資料_藥品群組_buf.Count > 0)
                {
                    RowsList[i][Enum] = list_藥品資料_藥品群組_buf[0][(int)enum_藥品資料_藥品群組.群組名稱];
                }
            }
        }
        private void Finction_藥品資料_藥品群組_名稱轉序號(List<object[]> RowsList, int Enum)
        {
            List<object[]> list_藥品資料_藥品群組 = sqL_DataGridView_藥品資料_藥品群組.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_藥品群組_buf = new List<object[]>();
            string 群組名稱 = "";
            for (int i = 0; i < RowsList.Count; i++)
            {
                群組名稱 = RowsList[i][Enum].ObjectToString();
                list_藥品資料_藥品群組_buf = list_藥品資料_藥品群組.GetRows((int)enum_藥品資料_藥品群組.群組名稱, 群組名稱);
                if (list_藥品資料_藥品群組_buf.Count > 0)
                {
                    RowsList[i][Enum] = list_藥品資料_藥品群組_buf[0][(int)enum_藥品資料_藥品群組.群組序號];
                }
            }
        }
        private string[] Function_藥品資料_藥品群組_取得選單()
        {
            List<string> list_data = new List<string>();
            List<object[]> list_藥品資料_藥品群組 = sqL_DataGridView_藥品資料_藥品群組.SQL_GetAllRows(false);
            list_藥品資料_藥品群組.Sort(new Icp_藥品資料_藥品群組());
            string 序號 = "";
            string 名稱 = "";
            for (int i = 0; i < list_藥品資料_藥品群組.Count; i++)
            {
                序號 = list_藥品資料_藥品群組[i][(int)enum_藥品資料_藥品群組.群組序號].ObjectToString();
                名稱 = list_藥品資料_藥品群組[i][(int)enum_藥品資料_藥品群組.群組名稱].ObjectToString();
                list_data.Add($"{序號}. {名稱}");
            }
            return list_data.ToArray();
        }
        #endregion
        #region Event
        private void SqL_DataGridView_藥品資料_藥品群組_RowEnterEvent(object[] RowValue)
        {
            int index = this.rJ_TextBox_藥品資料_藥品群組_群組序號.Text.StringToInt32();
            if (index > 0)
            {
                List<object[]> list_value = this.sqL_DataGridView_藥品資料_藥品群組.SQL_GetRows(enum_藥品資料_藥品群組.群組序號.GetEnumName(), index.ToString("00"), false);
                if (list_value.Count > 0)
                {
                    string GUID = list_value[0][(int)enum_藥品資料_藥品群組.GUID].ObjectToString();
                    object[] value = new object[new enum_藥品資料_藥品群組().GetEnumNames().Length];
                    value[(int)enum_藥品資料_藥品群組.GUID] = GUID;
                    value[(int)enum_藥品資料_藥品群組.群組序號] = index.ToString("00");
                    value[(int)enum_藥品資料_藥品群組.群組名稱] = this.rJ_TextBox_藥品資料_藥品群組_群組名稱.Text;
                    this.sqL_DataGridView_藥品資料_藥品群組.SQL_Replace(enum_藥品資料_藥品群組.GUID.GetEnumName(), GUID, value, false);
                }

            }
            rJ_TextBox_藥品資料_藥品群組_群組序號.Text = RowValue[(int)enum_藥品資料_藥品群組.群組序號].ObjectToString();
            rJ_TextBox_藥品資料_藥品群組_群組名稱.Text = RowValue[(int)enum_藥品資料_藥品群組.群組名稱].ObjectToString();
        }
        private void SqL_DataGridView_藥品資料_藥品群組_DataGridRowsChangeEvent(List<object[]> RowsList)
        {
            RowsList.Sort(new Icp_藥品資料_藥品群組());
        }
        private void RJ_TextBox_藥品資料_藥品群組_群組名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_藥品資料_藥品群組_寫入_MouseDownEvent(null);
            }
        }
        private void PlC_RJ_Button_藥品資料_藥品群組_寫入_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品資料_藥品群組.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取資料!");
                }));
                return;
            }
            list_value[0][(int)enum_藥品資料_藥品群組.群組名稱] = this.rJ_TextBox_藥品資料_藥品群組_群組名稱.Texts;
            this.sqL_DataGridView_藥品資料_藥品群組.SQL_ReplaceExtra(list_value[0], true);
        } 
        #endregion
    }
}
