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
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {


        public enum enum_藥庫_儲位設定_區域儲位
        {
            GUID,
            IP,
            Port,
            Num,
            序號,
            名稱,
        }

        private void sub_Program_藥庫_儲位設定_區域儲位_Init()
        {
            this.sqL_DataGridView_貨架區域儲位列表.Init();
            if (!this.sqL_DataGridView_貨架區域儲位列表.SQL_IsTableCreat()) this.sqL_DataGridView_貨架區域儲位列表.SQL_CreateTable();
            this.plC_RJ_Button_藥庫_儲位設定_區域儲位_寫入.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_區域儲位_寫入_MouseDownEvent;
            this.sqL_DataGridView_貨架區域儲位列表.RowEnterEvent += SqL_DataGridView_貨架區域儲位列表_RowEnterEvent;
            this.sqL_DataGridView_貨架區域儲位列表.DataGridRowsChangeRefEvent += SqL_DataGridView_貨架區域儲位列表_DataGridRowsChangeRefEvent;

            this.plC_RJ_Button_藥庫_儲位設定_區域儲位_亮燈.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_區域儲位_亮燈_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_區域儲位_滅燈.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_區域儲位_滅燈_MouseDownEvent;

            this.Function_藥庫_儲位設定_區域儲位_檢查表格合理性();

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_儲位設定_區域儲位);
        }

    

        private bool flag_藥庫_儲位設定_區域儲位 = false;
        private void sub_Program_藥庫_儲位設定_區域儲位()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "儲位設定" && this.plC_ScreenPage_藥庫_儲位設定.PageText == "區域儲位")
            {
                if (!this.flag_藥庫_儲位設定_區域儲位)
                {
                    this.Function_藥庫_儲位設定_區域儲位_檢查表格合理性();
                    this.flag_藥庫_儲位設定_區域儲位 = true;
                }
            }
            else
            {
                this.flag_藥庫_儲位設定_區域儲位 = false;
            }
        }

        #region Function
        private void Function_藥庫_儲位設定_區域儲位_檢查表格合理性()
        {
            int 貨架數量 = myConfigClass.貨架數量;
            List<object[]> list_value = this.sqL_DataGridView_貨架區域儲位列表.SQL_GetAllRows(false);
            List<object[]> list_value_add = new List<object[]>();
            List<object[]> list_value_delete = new List<object[]>();

            list_value.Sort(new ICP_貨架區域儲位列表());

            while (true)
            {
                if (list_value.Count >= 貨架數量) break;
                object[] value = new object[new enum_藥庫_儲位設定_區域儲位().GetLength()];
                value[(int)enum_藥庫_儲位設定_區域儲位.GUID] = Guid.NewGuid().ToString();
                list_value.Add(value);
                list_value_add.Add(value);
            }
            this.sqL_DataGridView_貨架區域儲位列表.SQL_AddRows(list_value_add, false);

            while (true)
            {
                if (list_value.Count <= 貨架數量) break;
                object[] value = list_value[list_value.Count - 1];
                list_value_delete.Add(value);
                list_value.RemoveByGUID(value);           
            }
            this.sqL_DataGridView_貨架區域儲位列表.SQL_DeleteExtra(list_value_delete ,false);

          
            for (int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_藥庫_儲位設定_區域儲位.序號] = (i + 1).ToString();
            }
            this.sqL_DataGridView_貨架區域儲位列表.SQL_ReplaceExtra(list_value, true);
        }
        private string[] Function_藥庫_儲位設定_區域儲位_取得選單()
        {
            List<string> list_data = new List<string>();
            List<object[]> list_貨架區域儲位列表 = sqL_DataGridView_貨架區域儲位列表.SQL_GetAllRows(false);
            list_貨架區域儲位列表.Sort(new ICP_貨架區域儲位列表());
            string 序號 = "";
            string 名稱 = "";
            for (int i = 0; i < list_貨架區域儲位列表.Count; i++)
            {
                序號 = list_貨架區域儲位列表[i][(int)enum_藥庫_儲位設定_區域儲位.序號].ObjectToString();
                名稱 = list_貨架區域儲位列表[i][(int)enum_藥庫_儲位設定_區域儲位.名稱].ObjectToString();
                list_data.Add($"{序號}. {名稱}");
            }
            return list_data.ToArray();
        }
        #endregion
        #region Event
        private void SqL_DataGridView_貨架區域儲位列表_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_貨架區域儲位列表());
        }
        private void SqL_DataGridView_貨架區域儲位列表_RowEnterEvent(object[] RowValue)
        {
            this.rJ_TextBox_藥庫_儲位設定_區域儲位_序號.Texts = RowValue[(int)enum_藥庫_儲位設定_區域儲位.序號].ObjectToString();
            this.rJ_TextBox_藥庫_儲位設定_區域儲位_名稱.Texts = RowValue[(int)enum_藥庫_儲位設定_區域儲位.名稱].ObjectToString();
            this.rJ_TextBox_藥庫_儲位設定_區域儲位_IP.Texts = RowValue[(int)enum_藥庫_儲位設定_區域儲位.IP].ObjectToString();
            this.rJ_TextBox_藥庫_儲位設定_區域儲位_Port.Texts = RowValue[(int)enum_藥庫_儲位設定_區域儲位.Port].ObjectToString();
            this.rJ_TextBox_藥庫_儲位設定_區域儲位_PIN_Num.Texts = RowValue[(int)enum_藥庫_儲位設定_區域儲位.Num].ObjectToString();


        }
        private void PlC_RJ_Button_藥庫_儲位設定_區域儲位_寫入_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_貨架區域儲位列表.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取資料!");
                }));
                return;
            }
            list_value[0][(int)enum_藥庫_儲位設定_區域儲位.名稱] = this.rJ_TextBox_藥庫_儲位設定_區域儲位_名稱.Texts;
            list_value[0][(int)enum_藥庫_儲位設定_區域儲位.IP] = this.rJ_TextBox_藥庫_儲位設定_區域儲位_IP.Texts;
            list_value[0][(int)enum_藥庫_儲位設定_區域儲位.Port] = this.rJ_TextBox_藥庫_儲位設定_區域儲位_Port.Texts;
            list_value[0][(int)enum_藥庫_儲位設定_區域儲位.Num] = this.rJ_TextBox_藥庫_儲位設定_區域儲位_PIN_Num.Texts;

            this.sqL_DataGridView_貨架區域儲位列表.SQL_ReplaceExtra(list_value, true);

        }
        private void PlC_RJ_Button_藥庫_儲位設定_區域儲位_亮燈_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_貨架區域儲位列表.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }

            string IP = list_value[0][(int)enum_藥庫_儲位設定_區域儲位.IP].ObjectToString();
            int Port = list_value[0][(int)enum_藥庫_儲位設定_區域儲位.Port].ObjectToString().StringToInt32();
            int Num = list_value[0][(int)enum_藥庫_儲位設定_區域儲位.Num].ObjectToString().StringToInt32();

            this.rfiD_UI.Set_OutputPIN(IP, Port, Num, true);


        }
        private void PlC_RJ_Button_藥庫_儲位設定_區域儲位_滅燈_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_貨架區域儲位列表.SQL_GetAllRows(false);


            List<Task> taskList = new List<Task>();
            for (int i = 0; i < list_value.Count; i++)
            {
                string IP = list_value[i][(int)enum_藥庫_儲位設定_區域儲位.IP].ObjectToString();
                int Port = list_value[i][(int)enum_藥庫_儲位設定_區域儲位.Port].ObjectToString().StringToInt32();
                int Num = list_value[i][(int)enum_藥庫_儲位設定_區域儲位.Num].ObjectToString().StringToInt32();

                taskList.Add(Task.Run(() =>
                {
                    this.rfiD_UI.Set_OutputPIN(IP, Port, Num, false);
                }));
            }
            Task.WhenAll(taskList);
        }
        #endregion

        private class ICP_貨架區域儲位列表 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                int temp0 = x[(int)enum_藥庫_儲位設定_區域儲位.序號].ObjectToString().StringToInt32();
                int temp1 = y[(int)enum_藥庫_儲位設定_區域儲位.序號].ObjectToString().StringToInt32();
                if (temp0 == -1) temp0 = 9999;
                if (temp1 == -1) temp1 = 9999;
                return temp0.CompareTo(temp1);

            }
        }

    }
}
