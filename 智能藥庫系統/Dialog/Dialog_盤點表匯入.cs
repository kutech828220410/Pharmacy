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
namespace 智能藥庫系統
{
    public partial class Dialog_盤點表匯入 : MyDialog
    {
        [EnumDescription("enum_盤點單")]
        private enum enum_盤點單
        {
            [Description("GUID,VARCHAR,50,PRIMARY")]
            GUID,
            [Description("fileName,VARCHAR,50,None")]
            fileName,
            [Description("名稱,VARCHAR,50,None")]
            名稱,
            [Description("藥品數量,VARCHAR,50,None")]
            藥品數量,
            [Description("上傳完成,VARCHAR,50,None")]
            上傳完成,
        }
        public static bool IsShown = false;
        private MyThread myThread = new MyThread();
        public Dialog_盤點表匯入()
        {
            form.Invoke(new Action(delegate { InitializeComponent(); }));



            this.FormBorderStyle = FormBorderStyle.None;
            stepViewer1.AddSetp("匯入", "批次選擇匯入檔案");
            stepViewer1.AddSetp("上傳", "上傳至伺服器");
            stepViewer1.AddSetp("完成", "表單建立成功");
            stepViewer1.CurrentStep = 1;
            this.TopLevel = true;
            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowDialogEvent += Dialog_盤點表匯入_ShowDialogEvent;
            this.Load += Dialog_盤點表匯入_Load;
            this.FormClosing += Dialog_盤點表匯入_FormClosing;
            this.CancelButton = this.plC_RJ_Button_返回;
            this.plC_RJ_Button_返回.MouseDownEvent += PlC_RJ_Button_返回_MouseDownEvent;
            this.plC_RJ_Button_瀏覽.MouseDownEvent += PlC_RJ_Button_瀏覽_MouseDownEvent;
            this.plC_RJ_Button_上傳.MouseDownEvent += PlC_RJ_Button_上傳_MouseDownEvent;
        }

  

        private void sub_program()
        {
            if (stepViewer1.CurrentStep == 1)
            {
                
            }
            if (stepViewer1.CurrentStep == 2)
            {
                if(plC_RJ_Button_上傳.Enabled == false)
                {
                    this.Invoke(new Action(delegate 
                    {
                        plC_RJ_Button_上傳.Enabled = true;
                    }));
                }
            }
            if (stepViewer1.CurrentStep == 3)
            {
                if (plC_RJ_Button_上傳.Enabled == true)
                {
                    this.Invoke(new Action(delegate
                    {
                        plC_RJ_Button_上傳.Enabled = false;
                    }));
                }
                if (plC_RJ_Button_瀏覽.Enabled == true)
                {
                    this.Invoke(new Action(delegate
                    {
                        plC_RJ_Button_瀏覽.Enabled = false;
                    }));
                }
            }
        }

        #region Event
        private void SqL_DataGridView_盤點單_DataGridRefreshEvent()
        {
            for (int i = 0; i < this.sqL_DataGridView_盤點單.dataGridView.Rows.Count; i++)
            {
                string 上傳完成 = this.sqL_DataGridView_盤點單.dataGridView.Rows[i].Cells[enum_盤點單.上傳完成.GetEnumName()].Value.ObjectToString();

                if (上傳完成 == "Y")
                {
                    this.sqL_DataGridView_盤點單.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                    this.sqL_DataGridView_盤點單.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void Dialog_盤點表匯入_FormClosing(object sender, FormClosingEventArgs e)
        {
            myThread.Abort();
            myThread = null;
            IsShown = false;
        }
        private void Dialog_盤點表匯入_Load(object sender, EventArgs e)
        {
            Table table = new Table(new enum_盤點單());
            this.sqL_DataGridView_盤點單.RowsHeight = 25;
            this.sqL_DataGridView_盤點單.Init(table);
            this.sqL_DataGridView_盤點單.Set_ColumnVisible(false, new enum_盤點單().GetEnumNames());
            this.sqL_DataGridView_盤點單.Set_ColumnWidth(400, DataGridViewContentAlignment.MiddleLeft, enum_盤點單.名稱);
            this.sqL_DataGridView_盤點單.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_盤點單.藥品數量);
            this.sqL_DataGridView_盤點單.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_盤點單.上傳完成);
            this.sqL_DataGridView_盤點單.DataGridRefreshEvent += SqL_DataGridView_盤點單_DataGridRefreshEvent;
            myThread.AutoRun(true);
            myThread.AutoStop(true);
            myThread.SetSleepTime(100);
            myThread.Add_Method(sub_program);
            myThread.Trigger();
            IsShown = true;
        }
        private void Dialog_盤點表匯入_ShowDialogEvent()
        {
            if (IsShown)
            {
                MyDialog.BringDialogToFront(this.Text);
                this.DialogResult = DialogResult.Cancel;
            }
        }
        private void PlC_RJ_Button_上傳_MouseDownEvent(MouseEventArgs mevent)
        {
            LoadingForm.ShowLoadingForm();

            string url = $"{Main_Form.API_Server}/api/inventory/excel_upload";
            List<object[]> list_value = this.sqL_DataGridView_盤點單.GetAllRows();
            for (int i = 0; i < list_value.Count; i++)
            {
                string IC_NAME = list_value[i][(int)enum_盤點單.名稱].ObjectToString();
                string CT = Main_Form._登入者名稱;
                string filename  = list_value[i][(int)enum_盤點單.fileName].ObjectToString();
                DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(filename);
                byte[] bytes = MyOffice.ExcelClass.NPOI_GetBytes(dataTable);
                List<string> names = new List<string>();
                names.Add("IC_NAME");
                names.Add("CT");
                List<string> values = new List<string>();
                values.Add(IC_NAME);
                values.Add(CT);
                string json_out = Basic.Net.WEBApiPost(url, filename, bytes, names, values);
                list_value[i][(int)enum_盤點單.上傳完成] = "Y";
                this.sqL_DataGridView_盤點單.ReplaceExtra(list_value[i], true);
            }
         
            LoadingForm.CloseLoadingForm();
            stepViewer1.Next();
        }
        private void PlC_RJ_Button_瀏覽_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                if (this.openFileDialog_LoadExcel.ShowDialog() == DialogResult.OK)
                {
                    string[] filenames = this.openFileDialog_LoadExcel.FileNames;
                    List<object[]> list_value = new List<object[]>();
                    foreach(string name in filenames)
                    {
                        DataTable dataTable = MyOffice.ExcelClass.NPOI_LoadFile(name);
                        if(dataTable != null)
                        {
                            object[] value = new object[new enum_盤點單().GetLength()];
                            value[(int)enum_盤點單.GUID] = Guid.NewGuid().ToString();
                            value[(int)enum_盤點單.fileName] = name;
                            value[(int)enum_盤點單.名稱] = System.IO.Path.GetFileName(name);
                            value[(int)enum_盤點單.藥品數量] = dataTable.Rows.Count;
                            value[(int)enum_盤點單.上傳完成] = "N";
                            list_value.Add(value);
                        }
                    }
                    this.sqL_DataGridView_盤點單.RefreshGrid(list_value);
                    this.rJ_Lable_狀態.Text = $"已選擇{list_value.Count}筆檔案";
                    stepViewer1.Next();

                }
            }));
        }
        private void PlC_RJ_Button_返回_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
        }
 
 
        #endregion
    }


}
