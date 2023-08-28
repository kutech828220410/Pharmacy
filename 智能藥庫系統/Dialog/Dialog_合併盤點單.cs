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
    public partial class Dialog_合併盤點單 : Form
    {
        public List<object[]> Value = new List<object[]>();
        public static Form form;
        public new DialogResult ShowDialog()
        {
            if (form == null)
            {
                base.ShowDialog();
            }
            else
            {
                form.Invoke(new Action(delegate
                {
                    base.ShowDialog();
                }));
            }

            return this.DialogResult;
        }
        public Dialog_合併盤點單()
        {
            InitializeComponent();
            this.Load += Dialog_合併盤點單_Load;
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
        }

      

        private void Dialog_合併盤點單_Load(object sender, EventArgs e)
        {
            this.plC_RJ_Button_上傳Excel.MouseDownEvent += PlC_RJ_Button_上傳Excel_MouseDownEvent;
            sqL_DataGridView_盤點表.Init();
        }

        #region Event
        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Close();
            }));
        }
        private void PlC_RJ_Button_上傳Excel_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
            dataTable = dataTable.ReorderTable(new enum_盤點管理_盤點表());
            if (dataTable == null)
            {
                return;
            }
            List<object[]> list_value_load = dataTable.DataTableToRowList();
            for (int i = 0; i < list_value_load.Count; i++)
            {
                int 庫存量 = list_value_load[i][(int)enum_盤點管理_盤點表.庫存量].StringToInt32();
                int 盤點量 = list_value_load[i][(int)enum_盤點管理_盤點表.盤點量].StringToInt32();
                int 差異量 = 盤點量 - 庫存量;
                list_value_load[i][(int)enum_盤點管理_盤點表.差異量] = 差異量;
            }
            this.Value = list_value_load;
            sqL_DataGridView_盤點表.RefreshGrid(list_value_load);
        }
        #endregion
    }
}
