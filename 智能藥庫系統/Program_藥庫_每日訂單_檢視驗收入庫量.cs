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
        public enum enum_藥庫_每日訂單_檢視驗收入庫量
        {
            GUID,
            藥碼,
            中文名稱,
            藥名,
            已驗收量,
            在途量,
            已訂購量,
            補償量,
            差異量,
        }

        private void sub_Program_藥庫_每日訂單_檢視驗收入庫量_Init()
        {

            this.sqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量.Init();
            this.sqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量.CellValidatingEvent += SqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量_CellValidatingEvent;
            this.sqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量.RowEndEditEvent += SqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量_RowEndEditEvent;

            this.plC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_顯示全部.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_輸入補償量.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_輸入補償量_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_藥碼搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_藥碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_藥名搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_藥名搜尋_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_每日訂單_檢視驗收入庫量);
        }

     

        private bool flag_藥庫_每日訂單_檢視驗收入庫量 = false;
        private void sub_Program_藥庫_每日訂單_檢視驗收入庫量()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "每日訂單")
            {
                if (!this.flag_藥庫_每日訂單_檢視驗收入庫量)
                {
                    this.flag_藥庫_每日訂單_檢視驗收入庫量 = true;
                }

            }
            else
            {
                this.flag_藥庫_每日訂單_檢視驗收入庫量 = false;
            }
        }

        #region Function
        private List<object[]> Function_藥庫_每日訂單_檢視驗收入庫量_取得資料()
        {
            List<object[]> list_藥庫_驗收入庫_過帳明細 = this.Function_藥庫_驗收入庫_過帳明細_取得資料();
            list_藥庫_驗收入庫_過帳明細 = list_藥庫_驗收入庫_過帳明細.GetRows((int)enum_藥庫_驗收入庫_過帳明細.來源, "院內系統");
            List<object[]> list_藥庫_驗收入庫_過帳明細_buf = new List<object[]>();
            List<object[]> list_藥庫_每日訂單_訂單查詢 = this.Function_藥庫_每日訂單_訂單查詢_取得訂單資料();
            List<object[]> list_藥庫_每日訂單_訂單查詢_buf = new List<object[]>();
            List<object[]> list_藥庫_每日訂單_檢視驗收入庫量 = new List<object[]>();
            List<object[]> list_藥庫_每日訂單_檢視驗收入庫量_buf = new List<object[]>();

            List<object[]> list_SQL_檢視驗收入庫量 = this.sqL_DataGridView_檢視驗收入庫量.SQL_GetAllRows(false);
            List<object[]> list_SQL_檢視驗收入庫量_buf = new List<object[]>();

            API_OrderClass aPI_OrderClass = this.Function_藥庫_每日訂單_下訂單_取得在途量();
            List<API_OrderClass.resultClass> resultClass = new List<API_OrderClass.resultClass>();
            List<string> Code_LINQ = (from value in list_藥庫_每日訂單_訂單查詢
                                      select value[(int)enum_藥庫_每日訂單_訂單查詢.藥品碼].ObjectToString()).Distinct().ToList();

            for (int i = 0; i < Code_LINQ.Count; i++)
            {
                list_藥庫_每日訂單_訂單查詢_buf = list_藥庫_每日訂單_訂單查詢.GetRows((int)enum_藥庫_每日訂單_訂單查詢.藥品碼, Code_LINQ[i]);
                list_藥庫_驗收入庫_過帳明細_buf = list_藥庫_驗收入庫_過帳明細.GetRows((int)enum_藥庫_驗收入庫_過帳明細.藥品碼, Code_LINQ[i]);
                list_SQL_檢視驗收入庫量_buf = list_SQL_檢視驗收入庫量.GetRows((int)enum_檢視驗收入庫量.藥碼, Code_LINQ[i]);
                resultClass = (from value in aPI_OrderClass.Result
                               where value.code == Code_LINQ[i]
                               select value).ToList();
                if (list_藥庫_每日訂單_訂單查詢_buf.Count > 0)
                {
                    string 藥碼 = list_藥庫_每日訂單_訂單查詢_buf[0][(int)enum_藥庫_每日訂單_訂單查詢.藥品碼].ObjectToString();
                    string 中文名稱 = list_藥庫_每日訂單_訂單查詢_buf[0][(int)enum_藥庫_每日訂單_訂單查詢.中文名稱].ObjectToString();
                    string 藥名 = list_藥庫_每日訂單_訂單查詢_buf[0][(int)enum_藥庫_每日訂單_訂單查詢.藥品名稱].ObjectToString();
                    int 已驗收量 = 0;
                    int 在途量 = 0;
                    int 已訂購量 = 0;
                    int 補償量 = 0;
                    int 差異量 = 0;
                    for (int k = 0; k < list_藥庫_每日訂單_訂單查詢_buf.Count; k++)
                    {
                        int 已訂購量_temp = list_藥庫_每日訂單_訂單查詢_buf[k][(int)enum_藥庫_每日訂單_訂單查詢.今日訂購數量].StringToInt32();
                        if (已訂購量_temp > 0)
                        {
                            已訂購量 = 已訂購量 + 已訂購量_temp;
                        }
                    }
                    for (int k = 0; k < list_藥庫_驗收入庫_過帳明細_buf.Count; k++)
                    {
                        int 已驗收量_temp = list_藥庫_驗收入庫_過帳明細_buf[k][(int)enum_藥庫_驗收入庫_過帳明細.數量].StringToInt32();
                        if (已驗收量_temp > 0)
                        {
                            已驗收量 = 已驗收量 + 已驗收量_temp;
                        }
                    }
                    for (int k = 0; k < list_SQL_檢視驗收入庫量_buf.Count; k++)
                    {
              
                        int 補償量_temp = list_SQL_檢視驗收入庫量_buf[k][(int)enum_檢視驗收入庫量.補償量].StringToInt32();
                        if (list_SQL_檢視驗收入庫量_buf[k][(int)enum_檢視驗收入庫量.補償量].ObjectToString().StringIsInt32())
                        {
                            補償量 = 補償量 + 補償量_temp;
                        }
                        break;
                    }
                    for (int k = 0; k < resultClass.Count; k++)
                    {
                        int 在途量_temp = resultClass[k].value.StringToInt32();
                        if (在途量_temp > 0)
                        {
                            在途量 = 在途量 + 在途量_temp;
                        }
                    }
                    差異量 = 已訂購量 - (已驗收量 + 在途量) + 補償量;
                    object[] value = new object[new enum_藥庫_每日訂單_檢視驗收入庫量().GetLength()];
                    value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.藥碼] = 藥碼;
                    value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.中文名稱] = 中文名稱;
                    value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.藥名] = 藥名;
                    value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.已驗收量] = 已驗收量;
                    value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.在途量] = 在途量;
                    value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.已訂購量] = 已訂購量;
                    value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.補償量] = 補償量;
                    value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.差異量] = 差異量;
                    list_藥庫_每日訂單_檢視驗收入庫量.Add(value);
                }

            }
            List<object[]> list_藥庫_每日訂單_檢視驗收入庫量_數量正常 = (from value in list_藥庫_每日訂單_檢視驗收入庫量
                                                        where value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.差異量].StringToInt32() == 0
                                                        select value).ToList();
            List<object[]> list_藥庫_每日訂單_檢視驗收入庫量_數量異常 = (from value in list_藥庫_每日訂單_檢視驗收入庫量
                                                        where value[(int)enum_藥庫_每日訂單_檢視驗收入庫量.差異量].StringToInt32() != 0
                                                        select value).ToList();
            if(checkBox_藥庫_每日訂單_檢視驗收入庫量_數量正常.Checked)
            {
                list_藥庫_每日訂單_檢視驗收入庫量_buf.LockAdd(list_藥庫_每日訂單_檢視驗收入庫量_數量正常);
            }
            if (checkBox_藥庫_每日訂單_檢視驗收入庫量_數量異常.Checked)
            {
                list_藥庫_每日訂單_檢視驗收入庫量_buf.LockAdd(list_藥庫_每日訂單_檢視驗收入庫量_數量異常);
            }
       
            list_藥庫_每日訂單_檢視驗收入庫量_buf.Sort(new ICP_藥庫_每日訂單_檢視驗收入庫量());
            return list_藥庫_每日訂單_檢視驗收入庫量_buf;
        }
        #endregion
        #region Event
        private void SqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量_RowEndEditEvent(object[] RowValue, int rowIndex, int colIndex, string valueS)
        {
            List<object[]> list_檢視驗收入庫量 = new List<object[]>();
            list_檢視驗收入庫量.Add(RowValue);
            List<object[]> list_SQL_檢視驗收入庫量 = this.sqL_DataGridView_檢視驗收入庫量.SQL_GetAllRows(false);
            List<object[]> list_SQL_檢視驗收入庫量_buf = new List<object[]>();
            List<object[]> list_SQL_檢視驗收入庫量_add = new List<object[]>();
            List<object[]> list_SQL_檢視驗收入庫量_replace = new List<object[]>();
            for (int i = 0; i < list_檢視驗收入庫量.Count; i++)
            {
                string 藥碼 = list_檢視驗收入庫量[i][(int)enum_藥庫_每日訂單_檢視驗收入庫量.藥碼].ObjectToString();
                int 補償量_src = list_檢視驗收入庫量[i][(int)enum_藥庫_每日訂單_檢視驗收入庫量.補償量].StringToInt32();
                int 差異量_src = list_檢視驗收入庫量[i][(int)enum_藥庫_每日訂單_檢視驗收入庫量.差異量].StringToInt32();
                int 在途量_src = list_檢視驗收入庫量[i][(int)enum_藥庫_每日訂單_檢視驗收入庫量.在途量].StringToInt32();
                int 已訂購量_src = list_檢視驗收入庫量[i][(int)enum_藥庫_每日訂單_檢視驗收入庫量.已訂購量].StringToInt32();
                int 已驗收量_src = list_檢視驗收入庫量[i][(int)enum_藥庫_每日訂單_檢視驗收入庫量.已驗收量].StringToInt32();

                list_SQL_檢視驗收入庫量_buf = list_SQL_檢視驗收入庫量.GetRows((int)enum_檢視驗收入庫量.藥碼, 藥碼);
                if (list_SQL_檢視驗收入庫量_buf.Count == 0)
                {
                    object[] value = new object[new enum_檢視驗收入庫量().GetLength()];
                    value[(int)enum_檢視驗收入庫量.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_檢視驗收入庫量.藥碼] = 藥碼;
                    value[(int)enum_檢視驗收入庫量.補償量] = 補償量_src;
                    list_SQL_檢視驗收入庫量_add.Add(value);
                }
                else
                {
                    object[] value = list_SQL_檢視驗收入庫量_buf[0];
                    value[(int)enum_檢視驗收入庫量.藥碼] = 藥碼;
                    if (value[(int)enum_檢視驗收入庫量.補償量].StringToInt32() != 補償量_src)
                    {
                        value[(int)enum_檢視驗收入庫量.補償量] = 補償量_src;
                        list_SQL_檢視驗收入庫量_replace.Add(value);
                    }

                }

                差異量_src = 已訂購量_src - (已驗收量_src + 在途量_src ) + 補償量_src;

                list_檢視驗收入庫量[i][(int)enum_藥庫_每日訂單_檢視驗收入庫量.差異量] = 差異量_src;
            }
            
            this.sqL_DataGridView_檢視驗收入庫量.SQL_AddRows(list_SQL_檢視驗收入庫量_add, false);
            this.sqL_DataGridView_檢視驗收入庫量.SQL_ReplaceExtra(list_SQL_檢視驗收入庫量_replace, false);
            this.sqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量.ReplaceExtra(list_檢視驗收入庫量, true);
        }
        private void SqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量_CellValidatingEvent(object[] RowValue, int rowIndex, int colIndex, string value, DataGridViewCellValidatingEventArgs e)
        {
            string 異動量 = value;
            if (異動量.StringIsInt32()==false)
            {
                MyMessageBox.ShowDialog("請輸入正確數字!");
                e.Cancel = true;
            }
        }
        private void PlC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥庫_每日訂單_檢視驗收入庫量 = Function_藥庫_每日訂單_檢視驗收入庫量_取得資料();
            this.sqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量.RefreshGrid(list_藥庫_每日訂單_檢視驗收入庫量);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_藥名搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if(rJ_TextBox_藥庫_每日訂單_檢視驗收入庫量_藥碼搜尋.Text.StringIsEmpty())
            {
                MyMessageBox.ShowDialog("請輸入搜尋內容!");
                return;
            }
            List<object[]> list_藥庫_每日訂單_檢視驗收入庫量 = Function_藥庫_每日訂單_檢視驗收入庫量_取得資料();

            list_藥庫_每日訂單_檢視驗收入庫量 = list_藥庫_每日訂單_檢視驗收入庫量.GetRowsStartWithByLike((int)enum_藥庫_每日訂單_檢視驗收入庫量.藥名, rJ_TextBox_藥庫_每日訂單_檢視驗收入庫量_藥名搜尋.Text);
            if(list_藥庫_每日訂單_檢視驗收入庫量.Count == 0)
            {
                MyMessageBox.ShowDialog("查無內容!");
                return;
            }
            this.sqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量.RefreshGrid(list_藥庫_每日訂單_檢視驗收入庫量);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_藥碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_藥庫_每日訂單_檢視驗收入庫量_藥碼搜尋.Text.StringIsEmpty())
            {
                MyMessageBox.ShowDialog("請輸入搜尋內容!");
                return;
            }
            List<object[]> list_藥庫_每日訂單_檢視驗收入庫量 = Function_藥庫_每日訂單_檢視驗收入庫量_取得資料();

            list_藥庫_每日訂單_檢視驗收入庫量 = list_藥庫_每日訂單_檢視驗收入庫量.GetRows((int)enum_藥庫_每日訂單_檢視驗收入庫量.藥碼, rJ_TextBox_藥庫_每日訂單_檢視驗收入庫量_藥碼搜尋.Text);
            if (list_藥庫_每日訂單_檢視驗收入庫量.Count == 0)
            {
                MyMessageBox.ShowDialog("查無內容!");
                return;
            }
            this.sqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量.RefreshGrid(list_藥庫_每日訂單_檢視驗收入庫量);
        }
        private void PlC_RJ_Button_藥庫_每日訂單_檢視驗收入庫量_輸入補償量_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_檢視驗收入庫量 = this.sqL_DataGridView_藥庫_每日訂單_檢視驗收入庫量.GetAllRows();
            List<object[]> list_SQL_檢視驗收入庫量 = this.sqL_DataGridView_檢視驗收入庫量.SQL_GetAllRows(false);
            List<object[]> list_SQL_檢視驗收入庫量_buf = new List<object[]>();
            List<object[]> list_SQL_檢視驗收入庫量_add = new List<object[]>();
            List<object[]> list_SQL_檢視驗收入庫量_replace = new List<object[]>();
            for (int i = 0; i < list_檢視驗收入庫量.Count; i++)
            {
                string 藥碼 = list_檢視驗收入庫量[i][(int)enum_藥庫_每日訂單_檢視驗收入庫量.藥碼].ObjectToString();
                string 補償量_src = list_檢視驗收入庫量[i][(int)enum_藥庫_每日訂單_檢視驗收入庫量.補償量].ObjectToString();
         
                list_SQL_檢視驗收入庫量_buf = list_SQL_檢視驗收入庫量.GetRows((int)enum_檢視驗收入庫量.藥碼, 藥碼);
                if (list_SQL_檢視驗收入庫量_buf.Count == 0)
                {
                    object[] value = new object[new enum_檢視驗收入庫量().GetLength()];
                    value[(int)enum_檢視驗收入庫量.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_檢視驗收入庫量.藥碼] = 藥碼;
                    value[(int)enum_檢視驗收入庫量.補償量] = 補償量_src;
                    list_SQL_檢視驗收入庫量_add.Add(value);
                }
                else
                {
                    object[] value = list_SQL_檢視驗收入庫量_buf[0];
                    value[(int)enum_檢視驗收入庫量.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_檢視驗收入庫量.藥碼] = 藥碼;
                    if(value[(int)enum_檢視驗收入庫量.補償量].ObjectToString() != 補償量_src)
                    {
                        value[(int)enum_檢視驗收入庫量.補償量] = 補償量_src;
                        list_SQL_檢視驗收入庫量_replace.Add(value);
                    }
             
                }
            }
            this.sqL_DataGridView_檢視驗收入庫量.SQL_AddRows(list_SQL_檢視驗收入庫量_add, false);
            this.sqL_DataGridView_檢視驗收入庫量.SQL_ReplaceExtra(list_SQL_檢視驗收入庫量_replace, false);

            MyMessageBox.ShowDialog("完成!");
        }
        #endregion

        private class ICP_藥庫_每日訂單_檢視驗收入庫量 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                if (x == null && y == null)
                {
                    return 0; // 如果 x 和 y 都为 null，则认为它们相等
                }
                else if (x == null)
                {
                    return -1; // 如果 x 为 null，而 y 不为 null，则将 x 排在 y 前面
                }
                else if (y == null)
                {
                    return 1; // 如果 y 为 null，而 x 不为 null，则将 y 排在 x 前面
                }
                int temp0 = x[(int)enum_藥庫_每日訂單_檢視驗收入庫量.差異量].StringToInt32();
                int temp1 = y[(int)enum_藥庫_每日訂單_檢視驗收入庫量.差異量].StringToInt32();
                if (temp0 == 0 && temp1 == 0)
                {
                    return 0; // 如果差異量都为0，则认为它们相等
                }
                else if (temp0 < 0 && temp1 >= 0)
                {
                    return -1; // 如果 x 差異量小于0，y 差異量大于等于0，则将 x 排在 y 前面
                }
                else if (temp0 >= 0 && temp1 < 0)
                {
                    return 1; // 如果 x 差異量大于等于0，y 差異量小于0，则将 y 排在 x 前面
                }
                else if (temp0 < 0 && temp1 < 0)
                {
                    return temp0.CompareTo(temp1); // 如果 x 和 y 的差異量都小于0，则按照差異量进行比较
                }
                else
                {
                    return temp1.CompareTo(temp0); // 其他情况下，按照差異量进行比较（正数排在负数前面）
                }

            }
        }
    }
}
