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
        }
        public Dialog_90日內最大消耗量()
        {
            form.Invoke(new Action(delegate { InitializeComponent(); }));
 
            this.Load += Dialog_90日內最大消耗量_Load;
        }

        private void Dialog_90日內最大消耗量_Load(object sender, EventArgs e)
        {
            Table table = new Table(new enum_90日內最大消耗量());
            
            this.sqL_DataGridView_90日內最大消耗量.Init(table);
            this.sqL_DataGridView_90日內最大消耗量.Set_ColumnVisible(false, new enum_90日內最大消耗量().GetEnumNames());
            this.sqL_DataGridView_90日內最大消耗量.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_90日內最大消耗量.藥碼);
            this.sqL_DataGridView_90日內最大消耗量.Set_ColumnWidth(500, DataGridViewContentAlignment.MiddleLeft, enum_90日內最大消耗量.藥名);
            this.sqL_DataGridView_90日內最大消耗量.Set_ColumnWidth(150, DataGridViewContentAlignment.MiddleCenter, enum_90日內最大消耗量.消耗量);
            LoadingForm.ShowLoadingForm();

            List<object[]> list_90日內最大消耗量 = new List<object[]>();
            List<object[]> list_過帳明細 = Main_Form.Function_藥品過消耗帳_取得所有過帳明細以產出時間(DateTime.Now.AddDays(-90), DateTime.Now);
            List<object[]> list_過帳明細_buf = new List<object[]>();
            Dictionary<object, List<object[]>> keyValuePairs_過帳明細 = list_過帳明細.ConvertToDictionary((int)Main_Form.enum_藥品過消耗帳.藥品碼);
            foreach (string key in keyValuePairs_過帳明細.Keys)
            {
                list_過帳明細_buf = keyValuePairs_過帳明細.SortDictionary(key);
                if (list_過帳明細_buf.Count > 0)
                {
                    object[] value = new object[new enum_90日內最大消耗量().GetLength()];
                    value[(int)enum_90日內最大消耗量.GUID] = list_過帳明細_buf[0][(int)Main_Form.enum_藥品過消耗帳.藥品碼].ObjectToString();
                    value[(int)enum_90日內最大消耗量.藥碼] = list_過帳明細_buf[0][(int)Main_Form.enum_藥品過消耗帳.藥品碼].ObjectToString();
                    value[(int)enum_90日內最大消耗量.藥名] = list_過帳明細_buf[0][(int)Main_Form.enum_藥品過消耗帳.藥品名稱].ObjectToString();
                    int 消耗量 = 0;

                    for (int i = 0; i < list_過帳明細_buf.Count; i++)
                    {
                        消耗量 += list_過帳明細_buf[i][(int)Main_Form.enum_藥品過消耗帳.異動量].StringToInt32();
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
