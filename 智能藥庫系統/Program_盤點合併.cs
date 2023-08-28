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
    public enum enum_盤點管理_盤點表
    {
        藥碼,
        藥名,
        中文名稱,
        單位,
        庫存量,
        盤點量,
        差異量
    }
    public partial class Form1 : Form
    {
     

  
        private void sub_Program_盤點合併_Init()
        {
            this.plC_RJ_Button_盤點合併_上傳Excel.MouseDownEvent += PlC_RJ_Button_盤點合併_上傳Excel_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_合併盤點單.MouseDownEvent += PlC_RJ_Button_盤點合併_合併盤點單_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_合併處方量.MouseDownEvent += PlC_RJ_Button_盤點合併_合併處方量_MouseDownEvent;
            this.plC_RJ_Button_盤點合併_匯出Excel.MouseDownEvent += PlC_RJ_Button_盤點合併_匯出Excel_MouseDownEvent;
            this.sqL_DataGridView_盤點合併_盤點表.Init();
            plC_UI_Init.Add_Method(sub_Program_盤點合併);
        }

 

        private void sub_Program_盤點合併()
        {

        }
        #region Event
        private void PlC_RJ_Button_盤點合併_上傳Excel_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            if (dataTable == null)
            {
                return;
            }
            dataTable = dataTable.ReorderTable(new enum_盤點管理_盤點表());
           
            List<object[]> list_value_load = dataTable.DataTableToRowList();
            for(int i = 0; i < list_value_load.Count; i++)
            {
                int 庫存量 = list_value_load[i][(int)enum_盤點管理_盤點表.庫存量].StringToInt32();
                int 盤點量 = list_value_load[i][(int)enum_盤點管理_盤點表.盤點量].StringToInt32();
                int 差異量 = 盤點量 - 庫存量;
                list_value_load[i][(int)enum_盤點管理_盤點表.差異量] = 差異量;
            }
         
            sqL_DataGridView_盤點合併_盤點表.RefreshGrid(list_value_load);
        }
        private void PlC_RJ_Button_盤點合併_合併盤點單_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            List<object[]> list_讀取盤點單 = new List<object[]>();
            List<object[]> list_盤點單 = this.sqL_DataGridView_盤點合併_盤點表.GetAllRows();
            List<object[]> list_盤點單_buf = new List<object[]>();
            List<object[]> list_盤點單_result = new List<object[]>();
            this.Invoke(new Action(delegate
            {
                Dialog_合併盤點單 dialog_合併盤點單 = new Dialog_合併盤點單();
                dialogResult = dialog_合併盤點單.ShowDialog();
                list_讀取盤點單 = dialog_合併盤點單.Value;
            }));
            for(int i = 0; i < list_讀取盤點單.Count; i++)
            {
                string 藥碼 = list_讀取盤點單[i][(int)enum_盤點管理_盤點表.藥碼].ObjectToString();
                list_盤點單_buf = list_盤點單.GetRows((int)enum_盤點管理_盤點表.藥碼, 藥碼);
                if (list_盤點單_buf.Count == 0)
                {
                    int 庫存量 = list_讀取盤點單[i][(int)enum_盤點管理_盤點表.庫存量].StringToInt32();
                    int 盤點量 = list_讀取盤點單[i][(int)enum_盤點管理_盤點表.盤點量].StringToInt32();
                    int 差異量 = 盤點量 - 庫存量;
                    list_讀取盤點單[i][(int)enum_盤點管理_盤點表.庫存量] = 庫存量;
                    list_讀取盤點單[i][(int)enum_盤點管理_盤點表.盤點量] = 盤點量;
                    list_讀取盤點單[i][(int)enum_盤點管理_盤點表.差異量] = 差異量;
                    list_盤點單.Add(list_讀取盤點單[i]);
                }
                else
                {
                    if(藥碼 == "50090")
                    {

                    }
                    int 庫存量 = list_盤點單_buf[0][(int)enum_盤點管理_盤點表.庫存量].StringToInt32();
                    int 盤點量 = list_盤點單_buf[0][(int)enum_盤點管理_盤點表.盤點量].StringToInt32() + list_讀取盤點單[i][(int)enum_盤點管理_盤點表.盤點量].StringToInt32();
                    int 差異量 = 盤點量 - 庫存量;
                    list_盤點單_buf[0][(int)enum_盤點管理_盤點表.庫存量] = 庫存量;
                    list_盤點單_buf[0][(int)enum_盤點管理_盤點表.盤點量] = 盤點量;
                    list_盤點單_buf[0][(int)enum_盤點管理_盤點表.差異量] = 差異量;
                }

            }
            this.sqL_DataGridView_盤點合併_盤點表.RefreshGrid(list_盤點單);
        }
        private void PlC_RJ_Button_盤點合併_合併處方量_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            List<object[]> list_處方量 = new List<object[]>();
            List<object[]> list_盤點單 = this.sqL_DataGridView_盤點合併_盤點表.GetAllRows();
            List<object[]> list_盤點單_buf = new List<object[]>();
            this.Invoke(new Action(delegate
            {
                Dialog_盤點處方量 dialog_盤點處方量 = new Dialog_盤點處方量();
                dialogResult = dialog_盤點處方量.ShowDialog();
                list_處方量 = dialog_盤點處方量.Value;
            }));
            for (int i = 0; i < list_處方量.Count; i++)
            {
                string 藥碼 = list_處方量[i][(int)enum_盤點處方量.藥碼].ObjectToString();
                list_盤點單_buf = list_盤點單.GetRows((int)enum_盤點管理_盤點表.藥碼, 藥碼);
                if (list_盤點單_buf.Count == 0)
                {
                    object[] value = new object[new enum_盤點管理_盤點表().GetLength()];
                    int 處方量 = list_處方量[i][(int)enum_盤點處方量.處方量].StringToInt32();
                    int 庫存量 = - 處方量;
                    int 盤點量 = 0;
                    int 差異量 = 盤點量 - 庫存量;
                    value[(int)enum_盤點管理_盤點表.藥碼] = 藥碼;
                    value[(int)enum_盤點管理_盤點表.庫存量] = 庫存量;
                    value[(int)enum_盤點管理_盤點表.盤點量] = 盤點量;
                    value[(int)enum_盤點管理_盤點表.差異量] = 差異量;
                    list_盤點單.Add(value);
                }
                else
                {
                    int 處方量 = list_處方量[i][(int)enum_盤點處方量.處方量].StringToInt32();
                    int 庫存量 = list_盤點單_buf[0][(int)enum_盤點管理_盤點表.庫存量].StringToInt32() - 處方量;
                    int 盤點量 = list_盤點單_buf[0][(int)enum_盤點管理_盤點表.盤點量].StringToInt32();
                    int 差異量 = 盤點量 - 庫存量;
                    list_盤點單_buf[0][(int)enum_盤點管理_盤點表.庫存量] = 庫存量;
                    list_盤點單_buf[0][(int)enum_盤點管理_盤點表.盤點量] = 盤點量;
                    list_盤點單_buf[0][(int)enum_盤點管理_盤點表.差異量] = 差異量;
                }

            }
            this.sqL_DataGridView_盤點合併_盤點表.RefreshGrid(list_盤點單);
        }
        private void PlC_RJ_Button_盤點合併_匯出Excel_MouseDownEvent(MouseEventArgs mevent)
        {
            DataTable dataTable = this.sqL_DataGridView_盤點合併_盤點表.GetDataTable();
            this.Invoke(new Action(delegate
            {
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    MyOffice.ExcelClass.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);

                }
            }));
        }
        #endregion
    }
}
