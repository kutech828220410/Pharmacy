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
using System.IO;
using SQLUI;
using Basic;
using MyUI;
namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {
        public enum enum_過帳狀態列表
        {
            GUID,
            編號,
            檔名,
            檔案位置,
            類別,
            報表日期,
            產生排程時間,
            排程作業時間,
            備註內容,
            狀態,
   
        }
        public enum enum_過帳狀態
        {
            已產生排程,
            排程已作業,
        }

        private void sub_Program_過帳狀態查詢_Init()
        {
            SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_過帳狀態列表, this.dBConfigClass.DB_Basic);

            this.sqL_DataGridView_過帳狀態列表.Init();
            if(!this.sqL_DataGridView_過帳狀態列表.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_過帳狀態列表.SQL_CreateTable();
            }
            this.sqL_DataGridView_過帳狀態列表.DataGridRowsChangeRefEvent += SqL_DataGridView_過帳狀態列表_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_過帳狀態列表.DataGridRefreshEvent += SqL_DataGridView_過帳狀態列表_DataGridRefreshEvent;

            this.rJ_ComboBox_過帳狀態列表_檔名.Enter += RJ_ComboBox_過帳狀態列表_檔名_Enter;
            this.rJ_ComboBox_過帳狀態列表_類別.DataSource = new enum_寫入報表設定_類別().GetEnumNames();

            this.plC_RJ_Button_過帳狀態列表_顯示全部.MouseDownEvent += PlC_RJ_Button_過帳狀態列表_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_過帳狀態列表_刪除選取資料.MouseDownEvent += PlC_RJ_Button_過帳狀態列表_刪除選取資料_MouseDownEvent;
            this.plC_RJ_Button_過帳狀態列表_檢查過帳狀態.MouseDownEvent += PlC_RJ_Button_過帳狀態列表_檢查過帳狀態_MouseDownEvent;
            this.plC_RJ_Button_過帳狀態列表_檔名_搜尋.MouseDownEvent += PlC_RJ_Button_過帳狀態列表_檔名_搜尋_MouseDownEvent;
            this.plC_RJ_Button_過帳狀態列表_類別_搜尋.MouseDownEvent += PlC_RJ_Button_過帳狀態列表_類別_搜尋_MouseDownEvent;
            this.plC_RJ_Button1_重新產生排程_產生排程.MouseDownEvent += PlC_RJ_Button1_重新產生排程_產生排程_MouseDownEvent;
            this.plC_RJ_Button_過帳狀態列表_搜尋.MouseDownEvent += PlC_RJ_Button_過帳狀態列表_搜尋_MouseDownEvent;
            this.plC_RJ_Button_過帳狀態列表_選取資料設定為已產生排程.MouseDownEvent += PlC_RJ_Button_過帳狀態列表_選取資料設定為已產生排程_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_過帳狀態查詢);

        }
  
        private void sub_Program_過帳狀態查詢()
        {
            //this.sub_Program_檢查過帳狀態();
        }

        #region PLC_檢查過帳狀態
        PLC_Device PLC_Device_檢查過帳狀態 = new PLC_Device("");
        PLC_Device PLC_Device_檢查過帳狀態_OK = new PLC_Device("");
        MyTimer MyTimer_檢查過帳狀態_結束延遲 = new MyTimer();
        int cnt_Program_檢查過帳狀態 = 65534;
        void sub_Program_檢查過帳狀態()
        {
            PLC_Device_檢查過帳狀態.Bool = true;
            if (cnt_Program_檢查過帳狀態 == 65534)
            {
                this.MyTimer_檢查過帳狀態_結束延遲.StartTickTime(10000);
                PLC_Device_檢查過帳狀態.SetComment("PLC_檢查過帳狀態");
                PLC_Device_檢查過帳狀態_OK.SetComment("PLC_檢查過帳狀態_OK");
                PLC_Device_檢查過帳狀態.Bool = false;
                cnt_Program_檢查過帳狀態 = 65535;
            }
            if (cnt_Program_檢查過帳狀態 == 65535) cnt_Program_檢查過帳狀態 = 1;
            if (cnt_Program_檢查過帳狀態 == 1) cnt_Program_檢查過帳狀態_檢查按下(ref cnt_Program_檢查過帳狀態);
            if (cnt_Program_檢查過帳狀態 == 2) cnt_Program_檢查過帳狀態_初始化(ref cnt_Program_檢查過帳狀態);
            if (cnt_Program_檢查過帳狀態 == 3) cnt_Program_檢查過帳狀態 = 65500;
            if (cnt_Program_檢查過帳狀態 > 1) cnt_Program_檢查過帳狀態_檢查放開(ref cnt_Program_檢查過帳狀態);

            if (cnt_Program_檢查過帳狀態 == 65500)
            {
                this.MyTimer_檢查過帳狀態_結束延遲.TickStop();
                this.MyTimer_檢查過帳狀態_結束延遲.StartTickTime(10000);
                PLC_Device_檢查過帳狀態.Bool = false;
                PLC_Device_檢查過帳狀態_OK.Bool = false;
                cnt_Program_檢查過帳狀態 = 65535;
            }
        }
        void cnt_Program_檢查過帳狀態_檢查按下(ref int cnt)
        {
            if (PLC_Device_檢查過帳狀態.Bool) cnt++;
        }
        void cnt_Program_檢查過帳狀態_檢查放開(ref int cnt)
        {
            if (!PLC_Device_檢查過帳狀態.Bool) cnt = 65500;
        }
        void cnt_Program_檢查過帳狀態_初始化(ref int cnt)
        {
            if(this.MyTimer_檢查過帳狀態_結束延遲.IsTimeOut())
            {
                if (Task_Method == null)
                {
                    Task_Method = new Task(new Action(delegate { this.PlC_RJ_Button_過帳狀態列表_檢查過帳狀態_MouseDownEvent(null); }));
                }
                if (Task_Method.Status == TaskStatus.RanToCompletion)
                {
                    Task_Method = new Task(new Action(delegate { this.PlC_RJ_Button_過帳狀態列表_檢查過帳狀態_MouseDownEvent(null); }));
                }
                if (Task_Method.Status == TaskStatus.Created)
                {
                    Task_Method.Start();
                }

                cnt++;
            }
        }

        #endregion
    
        #region Event
        private void SqL_DataGridView_過帳狀態列表_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_過帳狀態列表.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_過帳狀態列表.dataGridView.Rows[i].Cells[(int)enum_過帳狀態列表.狀態].Value.ToString();
                if (狀態 == enum_過帳狀態.排程已作業.GetEnumName())
                {
                    this.sqL_DataGridView_過帳狀態列表.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_過帳狀態列表.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }

       
            }
        }
        private void SqL_DataGridView_過帳狀態列表_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            if(plC_RJ_ChechBox_過帳狀態列表_產生排程時間.Checked)
            {
                RowsList = RowsList.GetRowsInDate((int)enum_過帳狀態列表.產生排程時間, this.dateTimePicke_過帳狀態列表_產生排程時間_起始, this.dateTimePicke_過帳狀態列表_產生排程時間_結束);
            }
            RowsList.Sort(new ICP_過帳狀態列表());
        }
        private void PlC_RJ_Button1_重新產生排程_產生排程_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            DateTime dateTime = rJ_DatePicker_重新產生排程_排程日期.Value;

            List<object[]> list_報表列表 = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
            List<object[]> list_過帳狀態列表 = this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(false);
            List<object[]> list_過帳狀態列表_buf = new List<object[]>();
            List<object[]> list_過帳狀態列表_Add = new List<object[]>();
            List<object[]> list_過帳狀態列表_Replace = new List<object[]>();
            List<object[]> list_過帳狀態列表_Delete = new List<object[]>();
            for (int i = 0; i < list_報表列表.Count; i++)
            {
                string 編號 = list_報表列表[i][(int)enum_寫入報表設定.編號].ObjectToString();
                string 檔名 = list_報表列表[i][(int)enum_寫入報表設定.檔名].ObjectToString();
                string 檔案位置 = list_報表列表[i][(int)enum_寫入報表設定.檔案位置].ObjectToString();
                string 類別 = list_報表列表[i][(int)enum_寫入報表設定.類別].ObjectToString();
                string 備註內容 = list_報表列表[i][(int)enum_寫入報表設定.備註內容].ObjectToString();
                int 每週更新週期 = list_報表列表[i][(int)enum_寫入報表設定.更新每週].ObjectToString().StringToInt32();
                bool IsUpdate = myConvert.Int32GetBit(每週更新週期, (int)DateTime.Now.DayOfWeek);
                if (IsUpdate)
                {
                    檔名 = 檔名.Replace("[Date]", $"{dateTime.Year}{dateTime.Month.ToString("00")}{dateTime.Day.ToString("00")}");
                    list_過帳狀態列表_buf = list_過帳狀態列表.GetRowsInDate((int)enum_過帳狀態列表.產生排程時間, dateTime);
                    list_過帳狀態列表_buf = list_過帳狀態列表_buf.GetRows((int)enum_過帳狀態列表.檔名, 檔名);

                    string filename = $@"{檔案位置}{檔名}";
                    List<string> list_text = new List<string>();
                    try
                    {
                        list_text = MyFileStream.LoadFile(filename);
                    }
                    catch
                    {
                        continue;
                    }
                   
                    if (list_text == null) continue;

                    if (list_過帳狀態列表_buf.Count == 0)
                    {
                      
                        object[] value = new object[new enum_過帳狀態列表().GetLength()];
                        value[(int)enum_過帳狀態列表.GUID] = Guid.NewGuid().ToString();
                        value[(int)enum_過帳狀態列表.編號] = 編號;

                        value[(int)enum_過帳狀態列表.報表日期] = dateTime.ToDateString();
                        value[(int)enum_過帳狀態列表.檔名] = 檔名;
                        value[(int)enum_過帳狀態列表.檔案位置] = 檔案位置;
                        value[(int)enum_過帳狀態列表.類別] = 類別;
                        value[(int)enum_過帳狀態列表.產生排程時間] = DateTime.Now.ToDateTimeString_6();
                        value[(int)enum_過帳狀態列表.排程作業時間] = DateTime.MinValue.ToDateTimeString_6();
                        value[(int)enum_過帳狀態列表.狀態] = enum_過帳狀態.已產生排程.GetEnumName();
                        value[(int)enum_過帳狀態列表.備註內容] = 備註內容;
                        list_過帳狀態列表_Add.Add(value);
                    }

                }

            }
            this.sqL_DataGridView_過帳狀態列表.SQL_AddRows(list_過帳狀態列表_Add, false);
            Console.WriteLine($"檢查過帳狀態 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
        }
        private void PlC_RJ_Button_過帳狀態列表_檢查過帳狀態_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            DateTime dateTime = DateTime.Now.AddDays(0);

            List<object[]> list_報表列表 = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(false);
            List<object[]> list_過帳狀態列表 = this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(false);
            List<object[]> list_過帳狀態列表_buf = new List<object[]>();
            List<object[]> list_過帳狀態列表_Add = new List<object[]>();
            List<object[]> list_過帳狀態列表_Replace = new List<object[]>();
            List<object[]> list_過帳狀態列表_Delete = new List<object[]>();
            for (int i = 0; i < list_報表列表.Count; i++)
            {
                string 編號 = list_報表列表[i][(int)enum_寫入報表設定.編號].ObjectToString();
                string 檔名 = list_報表列表[i][(int)enum_寫入報表設定.檔名].ObjectToString();
                string 檔案位置 = list_報表列表[i][(int)enum_寫入報表設定.檔案位置].ObjectToString();
                string 類別 = list_報表列表[i][(int)enum_寫入報表設定.類別].ObjectToString();
                string 更新每日 = list_報表列表[i][(int)enum_寫入報表設定.更新每日].ObjectToString();
                string 備註內容 = list_報表列表[i][(int)enum_寫入報表設定.備註內容].ObjectToString();
                int 每週更新週期 = list_報表列表[i][(int)enum_寫入報表設定.更新每週].ObjectToString().StringToInt32();
                bool IsUpdate = myConvert.Int32GetBit(每週更新週期, (int)DateTime.Now.DayOfWeek);
                if (IsUpdate)
                {
                    if(類別 != enum_寫入報表設定_類別.其他.GetEnumName())
                    {
                        檔名 = 檔名.Replace("[Date]", $"{dateTime.Year}{dateTime.Month.ToString("00")}{dateTime.Day.ToString("00")}");

                        string filename = $@"{檔案位置}{檔名}";

                        List<string> list_text = MyFileStream.LoadFile(filename);
                        if (list_text == null) continue;

                        list_過帳狀態列表_buf = list_過帳狀態列表.GetRowsInDate((int)enum_過帳狀態列表.產生排程時間, dateTime);
                        list_過帳狀態列表_buf = list_過帳狀態列表_buf.GetRows((int)enum_過帳狀態列表.檔名, 檔名);

                        if (list_過帳狀態列表_buf.Count == 0)
                        {
                            if (更新每日.Length == 4)
                            {
                                string str_hour = 更新每日.Substring(0, 2);
                                string str_min = 更新每日.Substring(2, 2);
                                DateTime dateTime1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, str_hour.StringToInt32(), str_min.StringToInt32(), 0);
                                DateTime dateTime2 = dateTime1.AddMinutes(10);
                                bool IsInDate = DateTime.Now.IsInDate(dateTime1, dateTime2);
                                if (IsInDate)
                                {
                                    object[] value = new object[new enum_過帳狀態列表().GetLength()];
                                    value[(int)enum_過帳狀態列表.GUID] = Guid.NewGuid().ToString();
                                    value[(int)enum_過帳狀態列表.編號] = 編號;

                                    value[(int)enum_過帳狀態列表.檔名] = 檔名;
                                    value[(int)enum_過帳狀態列表.檔案位置] = 檔案位置;
                                    value[(int)enum_過帳狀態列表.類別] = 類別;
                                    value[(int)enum_過帳狀態列表.報表日期] = dateTime.ToDateString();
                                    value[(int)enum_過帳狀態列表.產生排程時間] = DateTime.Now.ToDateTimeString_6();
                                    value[(int)enum_過帳狀態列表.排程作業時間] = DateTime.MinValue.ToDateTimeString_6();
                                    value[(int)enum_過帳狀態列表.狀態] = enum_過帳狀態.已產生排程.GetEnumName();
                                    value[(int)enum_過帳狀態列表.備註內容] = 備註內容;
                                    list_過帳狀態列表_Add.Add(value);
                                }

                            }

                            //object[] value = new object[new enum_過帳狀態列表().GetLength()];
                            //value[(int)enum_過帳狀態列表.GUID] = Guid.NewGuid().ToString();
                            //value[(int)enum_過帳狀態列表.編號] = 編號;

                            //value[(int)enum_過帳狀態列表.檔名] = 檔名;
                            //value[(int)enum_過帳狀態列表.檔案位置] = 檔案位置;
                            //value[(int)enum_過帳狀態列表.類別] = 類別;
                            //value[(int)enum_過帳狀態列表.報表日期] = dateTime.ToDateString();
                            //value[(int)enum_過帳狀態列表.產生排程時間] = DateTime.Now.ToDateTimeString_6();
                            //value[(int)enum_過帳狀態列表.排程作業時間] = DateTime.MinValue.ToDateTimeString_6();
                            //value[(int)enum_過帳狀態列表.狀態] = enum_過帳狀態.已產生排程.GetEnumName();
                            //value[(int)enum_過帳狀態列表.備註內容] = 備註內容;
                            //list_過帳狀態列表_Add.Add(value);
                        }
                    }
                    else
                    {
                        list_過帳狀態列表_buf = list_過帳狀態列表.GetRowsInDate((int)enum_過帳狀態列表.產生排程時間, dateTime);
                        list_過帳狀態列表_buf = list_過帳狀態列表_buf.GetRows((int)enum_過帳狀態列表.檔名, 檔名);

                        if (list_過帳狀態列表_buf.Count == 0)
                        {
                            if(更新每日.Length == 4)
                            {
                                string str_hour = 更新每日.Substring(0, 2);
                                string str_min = 更新每日.Substring(2, 2);
                                DateTime dateTime1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, str_hour.StringToInt32(), str_min.StringToInt32(), 0);
                                DateTime dateTime2 = dateTime1.AddMinutes(10);
                                bool IsInDate = DateTime.Now.IsInDate(dateTime1, dateTime2);
                                if(IsInDate)
                                {
                                    object[] value = new object[new enum_過帳狀態列表().GetLength()];
                                    value[(int)enum_過帳狀態列表.GUID] = Guid.NewGuid().ToString();
                                    value[(int)enum_過帳狀態列表.編號] = 編號;
                                    value[(int)enum_過帳狀態列表.檔名] = 檔名;
                                    value[(int)enum_過帳狀態列表.檔案位置] = 檔案位置;
                                    value[(int)enum_過帳狀態列表.類別] = 類別;
                                    value[(int)enum_過帳狀態列表.報表日期] = dateTime.ToDateString();
                                    value[(int)enum_過帳狀態列表.產生排程時間] = dateTime.ToDateTimeString_6();
                                    value[(int)enum_過帳狀態列表.排程作業時間] = DateTime.MinValue.ToDateTimeString_6();
                                    value[(int)enum_過帳狀態列表.狀態] = enum_過帳狀態.已產生排程.GetEnumName();
                                    value[(int)enum_過帳狀態列表.備註內容] = 備註內容;
                                    list_過帳狀態列表_Add.Add(value);
                                }                      
                            }
                           
                        }
                    }
                   

                }

            }
            this.sqL_DataGridView_過帳狀態列表.SQL_AddRows(list_過帳狀態列表_Add, false);
            Console.WriteLine($"檢查過帳狀態 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
        }
        private void PlC_RJ_Button_過帳狀態列表_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_過帳狀態列表_刪除選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_過帳狀態列表.Get_All_Select_RowsValues();

            DialogResult dialogResult = DialogResult.None;

            this.Invoke(new Action(delegate
            {
                dialogResult = MyMessageBox.ShowDialog("是否刪除選取資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel);
            }));
            if (dialogResult != DialogResult.Yes) return;
            this.sqL_DataGridView_過帳狀態列表.SQL_DeleteExtra(list_value, false);
            this.sqL_DataGridView_過帳狀態列表.DeleteExtra(list_value, true);
        }
        private void RJ_ComboBox_過帳狀態列表_檔名_Enter(object sender, EventArgs e)
        {
            List<object[]> list_value = this.sqL_DataGridView_寫入報表設定.SQL_GetAllRows(true);
            List<string> list_text = (from value in list_value
                                      select value[(int)enum_寫入報表設定.檔名].ObjectToString()).Distinct().ToList();
            this.rJ_ComboBox_過帳狀態列表_檔名.DataSource = list_text;
        }
        private void PlC_RJ_Button_過帳狀態列表_類別_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_過帳狀態列表.類別, this.rJ_ComboBox_過帳狀態列表_類別.Texts);
            this.sqL_DataGridView_過帳狀態列表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_過帳狀態列表_檔名_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_過帳狀態列表.檔名, this.rJ_ComboBox_過帳狀態列表_檔名.Texts);
            this.sqL_DataGridView_過帳狀態列表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_過帳狀態列表_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(false);
            list_value = list_value.GetRowsInDate((int)enum_過帳狀態列表.報表日期, dateTimePicke_過帳狀態列表_報表日期.Value);
            this.sqL_DataGridView_過帳狀態列表.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_過帳狀態列表_選取資料設定為已產生排程_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否將選取資料設定為[已產生排程]?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            List<object[]> list_value = this.sqL_DataGridView_過帳狀態列表.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            for(int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_過帳狀態列表.狀態] = enum_過帳狀態.已產生排程.GetEnumName();
            }
            this.sqL_DataGridView_過帳狀態列表.SQL_ReplaceExtra(list_value, false);
            this.sqL_DataGridView_過帳狀態列表.ReplaceExtra(list_value, true);
        }
        #endregion

        private class ICP_過帳狀態列表 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                int temp0 = x[(int)enum_過帳狀態列表.編號].ObjectToString().StringToInt32();
                int temp1 = y[(int)enum_過帳狀態列表.編號].ObjectToString().StringToInt32();
                string date01 = x[(int)enum_過帳狀態列表.報表日期].ToDateTimeString_6();
                string date02 = y[(int)enum_過帳狀態列表.報表日期].ToDateTimeString_6();
                int cmp = date01.CompareTo(date02);
                if (cmp != 0) return cmp;
                else
                {
                    return temp0.CompareTo(temp1);
                }
             
            }
        }
    }
}
