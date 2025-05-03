using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic;
using MyUI;
using SQLUI;
using HIS_DB_Lib;
using H_Pannel_lib;

namespace 智能藥庫系統
{
    public partial class Dialog_90日內最大消耗量 : MyDialog
    {
        List<object[]> list_90日內最大消耗量 = new List<object[]>();
        public enum enum_90日內最大消耗量
        {
            [Description("GUID,VARCHAR,50,None")]
            GUID,
            [Description("藥碼,VARCHAR,50,None")]
            藥碼,
            [Description("藥名,VARCHAR,50,None")]
            藥名,
            [Description("中文名,VARCHAR,50,None")]
            中文名,
            [Description("消耗量,VARCHAR,50,None")]
            消耗量,
            [Description("最大消耗日期,VARCHAR,50,None")]
            最大消耗日期,
        }
        public Dialog_90日內最大消耗量()
        {
            form.Invoke(new Action(delegate { InitializeComponent(); }));
 
            this.Load += Dialog_90日內最大消耗量_Load;
            this.LoadFinishedEvent += Dialog_90日內最大消耗量_LoadFinishedEvent;
            this.plC_RJ_Button_搜尋.MouseDownEvent += PlC_RJ_Button_搜尋_MouseDownEvent;
        }

  

        private void PlC_RJ_Button_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_90日內最大消耗量_buf = new List<object[]>();

            string serch_text = comboBox_搜尋條件.GetComboBoxText();
            if(rJ_TextBox_搜尋條件.Text.StringIsEmpty())
            {
                MyMessageBox.ShowDialog("請輸入搜尋內容");
                return;
            }
            if (serch_text == "全部顯示")
            {
                list_90日內最大消耗量_buf = list_90日內最大消耗量;
            }
            if (serch_text == "藥碼")
            {
                list_90日內最大消耗量_buf = (from temp in list_90日內最大消耗量
                                      where temp[(int)enum_90日內最大消耗量.藥碼].ObjectToString().ToUpper().StartsWith(rJ_TextBox_搜尋條件.Text.ToUpper())
                                      select temp).ToList();
            }
            if (serch_text == "藥名")
            {
                list_90日內最大消耗量_buf = (from temp in list_90日內最大消耗量
                                      where temp[(int)enum_90日內最大消耗量.藥名].ObjectToString().ToUpper().StartsWith(rJ_TextBox_搜尋條件.Text.ToUpper())
                                      select temp).ToList();
            }
            if(list_90日內最大消耗量_buf.Count == 0)
            {
                MyMessageBox.ShowDialog("查無資料");
            }
            this.sqL_DataGridView_90日內最大消耗量.RefreshGrid(list_90日內最大消耗量_buf);

        }
        private void Dialog_90日內最大消耗量_Load(object sender, EventArgs e)
        {
          
        }
        private void Dialog_90日內最大消耗量_LoadFinishedEvent(EventArgs e)
        {
            comboBox_搜尋條件.SelectedIndex = 0;
            Table table = new Table(new enum_90日內最大消耗量());

            this.sqL_DataGridView_90日內最大消耗量.Init(table);
            this.sqL_DataGridView_90日內最大消耗量.Set_ColumnVisible(false, new enum_90日內最大消耗量().GetEnumNames());
            this.sqL_DataGridView_90日內最大消耗量.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_90日內最大消耗量.藥碼);
            this.sqL_DataGridView_90日內最大消耗量.Set_ColumnWidth(500, DataGridViewContentAlignment.MiddleLeft, enum_90日內最大消耗量.藥名);
            this.sqL_DataGridView_90日內最大消耗量.Set_ColumnWidth(150, DataGridViewContentAlignment.MiddleCenter, enum_90日內最大消耗量.消耗量);
            this.sqL_DataGridView_90日內最大消耗量.Set_ColumnWidth(200, DataGridViewContentAlignment.MiddleCenter, enum_90日內最大消耗量.最大消耗日期);
            LoadingForm.ShowLoadingForm();

            List<object[]> list_過帳明細 = Main_Form.Function_藥品過消耗帳_取得所有過帳明細以產出時間(DateTime.Now.AddDays(-90), DateTime.Now);
  
            Dictionary<object, List<object[]>> keyValuePairs_過帳明細 = list_過帳明細.ConvertToDictionary((int)Main_Form.enum_藥品過消耗帳.藥品碼);
            foreach (string key in keyValuePairs_過帳明細.Keys)
            {
                List<object[]> list_過帳明細_buf = new List<object[]>();
                List<object[]> list_過帳明細_temp = new List<object[]>();
                List<object[]> list_過帳明細_buf_temp = new List<object[]>();

                list_過帳明細_buf = keyValuePairs_過帳明細.SortDictionary(key);
                if (list_過帳明細_buf.Count > 0)
                {
                    object[] value = new object[new enum_90日內最大消耗量().GetLength()];
                    value[(int)enum_90日內最大消耗量.GUID] = list_過帳明細_buf[0][(int)Main_Form.enum_藥品過消耗帳.藥品碼].ObjectToString();
                    value[(int)enum_90日內最大消耗量.藥碼] = list_過帳明細_buf[0][(int)Main_Form.enum_藥品過消耗帳.藥品碼].ObjectToString();
                    value[(int)enum_90日內最大消耗量.藥名] = list_過帳明細_buf[0][(int)Main_Form.enum_藥品過消耗帳.藥品名稱].ObjectToString();
                    double 消耗量 = 0;
                    List<string> list_過帳明細_date = (from temp in list_過帳明細_buf
                                                     select temp[(int)Main_Form.enum_藥品過消耗帳.報表日期].ToDateString()).Distinct().ToList();
                    for (int i = 0; i < list_過帳明細_date.Count; i++)
                    {
                        list_過帳明細_buf_temp = (from temp in list_過帳明細_buf
                                              where temp[(int)Main_Form.enum_藥品過消耗帳.報表日期].ToDateString() == list_過帳明細_date[i]
                                              select temp).ToList();
                        object[] value_temp = new object[new Main_Form.enum_藥品過消耗帳().GetLength()];
                        if(list_過帳明細_buf_temp.Count > 0)
                        {
                            value_temp[(int)Main_Form.enum_藥品過消耗帳.藥品碼] = list_過帳明細_buf_temp[0][(int)Main_Form.enum_藥品過消耗帳.藥品碼].ObjectToString();
                            value_temp[(int)Main_Form.enum_藥品過消耗帳.藥品名稱] = list_過帳明細_buf_temp[0][(int)Main_Form.enum_藥品過消耗帳.藥品名稱].ObjectToString();
                            value_temp[(int)Main_Form.enum_藥品過消耗帳.報表日期] = list_過帳明細_buf_temp[0][(int)Main_Form.enum_藥品過消耗帳.報表日期].ToDateString();
                            value_temp[(int)Main_Form.enum_藥品過消耗帳.異動量] = "0";
                            for (int k = 0; k < list_過帳明細_buf_temp.Count; k++)
                            {
                                value_temp[(int)Main_Form.enum_藥品過消耗帳.異動量] = value_temp[(int)Main_Form.enum_藥品過消耗帳.異動量].StringToDouble() + list_過帳明細_buf_temp[k][(int)Main_Form.enum_藥品過消耗帳.異動量].StringToDouble();
                            }
                            list_過帳明細_temp.Add(value_temp);
                        }
                       
                    }
                 
                    for (int i = 0; i < list_過帳明細_temp.Count; i++)
                    {
                        if (list_過帳明細_temp[i][(int)Main_Form.enum_藥品過消耗帳.異動量].StringToDouble() * -1 > 消耗量 * -1)
                        {
                            消耗量 = list_過帳明細_temp[i][(int)Main_Form.enum_藥品過消耗帳.異動量].StringToDouble();
                            value[(int)enum_90日內最大消耗量.最大消耗日期] = list_過帳明細_temp[i][(int)Main_Form.enum_藥品過消耗帳.報表日期].ToDateString();
                        }
                    }
                    消耗量 = 消耗量 * -1;
                    value[(int)enum_90日內最大消耗量.消耗量] = 消耗量;
                    list_90日內最大消耗量.Add(value);
                }
            }
            list_90日內最大消耗量.Sort(new ICP_90日內最大消耗量());
            this.sqL_DataGridView_90日內最大消耗量.RefreshGrid(list_90日內最大消耗量);


            LoadingForm.CloseLoadingForm();
        }
        private class ICP_90日內最大消耗量 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                int temp0 = x[(int)enum_90日內最大消耗量.消耗量].StringToInt32();
                int temp1 = y[(int)enum_90日內最大消耗量.消耗量].StringToInt32();
                return temp1.CompareTo(temp0);
            }
        }
    }
}
