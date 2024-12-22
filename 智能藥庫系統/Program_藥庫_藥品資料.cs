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
using HIS_DB_Lib;
using SQLUI;
namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        public class DrugInventory
        {
            public string DrugCode { get; set; } // 藥碼
            public int Inventory { get; set; }   // 庫存

            public DrugInventory(string drugCode, int inventory)
            {
                DrugCode = drugCode;
                Inventory = inventory;
            }
        }

        private DeviceBasicClass DeviceBasicClass_藥庫 = new DeviceBasicClass();

        public enum enum_medType
        {
            冷藏藥,
            分包機裸錠,
            高價藥櫃,
            少用及易混,
            水劑,
            口服藥,
            針劑,
            外用藥,
            未分類
        }
        public enum enum_開檔狀態
        {
            開檔中,
            關檔中,
            已取消,
            停用中,
        }
        private enum enum_medDrugstore_效期及庫存
        {
            效期,
            批號,
            庫存,
        }
        public enum enum_medDrugstore_匯出
        {
            藥碼,
            藥名,
            總庫存,
            包裝單位,
            包裝數量,
            基準量,
            安全庫存,
            採購單價,
            庫存金額,
            藥庫庫存,
            藥局庫存,
            效期及批號,
        }
        public enum enum_medDrugstore_匯入
        {
            藥碼,
            基準量,
            安全量,
            包裝數量,
        }
        public enum enum_medDrugstore_匯入_安全基準量
        {
            藥碼,
            基準量,
            安全量,
        }
  
        public enum ContextMenuStrip_藥庫_藥品資料
        {
            檢視歷史效期批號,
            藥品設定,
            補給系統藥品建置
        }
        private void sub_Program_藥庫_藥品資料_Init()
        {

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_藥庫_藥品資料, dBConfigClass.DB_Basic);
            string url = $"{API_Server}/api/MED_page/init";
            returnData returnData = new returnData();
            returnData.ServerType = enum_ServerSetting_Type.藥庫.GetEnumName();
            returnData.ServerName = $"{"DS01"}";
            returnData.TableName = "medicine_page_firstclass";
            string json_in = returnData.JsonSerializationt();
            string json = Basic.Net.WEBApiPostJson($"{url}", json_in);
            Table table = json.JsonDeserializet<Table>();
            if (table == null)
            {
                MyMessageBox.ShowDialog($"藥庫藥檔表單建立失敗!! API_Server:{API_Server}");
                return;
            }
            table[enum_medDrugstore.類別.GetEnumName()].TypeName = Table.GetTypeName(Table.OtherType.ENUM, new enum_medType().GetEnumNames());
            //table[enum_medDrugstore.開檔狀態.GetEnumName()].TypeName = Table.GetTypeName(Table.OtherType.ENUM, new enum_開檔狀態().GetEnumNames());
            this.sqL_DataGridView_藥庫_藥品資料.Init(table);

            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnVisible(false, new enum_medDrugstore().GetEnumNames());
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(60, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.藥品碼);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(300, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.中文名稱);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(300, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.藥品名稱);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(300, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.藥品學名);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.包裝單位);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.包裝數量);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.藥局庫存);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.藥庫庫存);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.總庫存);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.基準量);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_medDrugstore.安全庫存);
            //this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleCenter, enum_medDrugstore.類別);
            //this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleCenter, enum_medDrugstore.開檔狀態);

            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnText("藥碼", enum_medDrugstore.藥品碼);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnText("藥名", enum_medDrugstore.藥品名稱);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnText("中文名", enum_medDrugstore.中文名稱);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnText("商品名", enum_medDrugstore.藥品學名);
            this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnText("安全量", enum_medDrugstore.安全庫存);
            //this.sqL_DataGridView_藥庫_藥品資料.Set_ColumnText("表單分類", enum_medDrugstore.類別);

            this.sqL_DataGridView_藥庫_藥品資料.DataGridRefreshEvent += SqL_DataGridView_藥庫_藥品資料_DataGridRefreshEvent;
            this.sqL_DataGridView_藥庫_藥品資料.MouseDown += SqL_DataGridView_藥庫_藥品資料_MouseDown;
            this.sqL_DataGridView_藥庫_藥品資料.ComboBoxSelectedIndexChangedEvent += SqL_DataGridView_藥庫_藥品資料_ComboBoxSelectedIndexChangedEvent;
            this.sqL_DataGridView_藥庫_藥品資料.DataGridRowsChangeEvent += SqL_DataGridView_藥庫_藥品資料_DataGridRowsChangeEvent;
            this.sqL_DataGridView_藥庫_藥品資料.DataGridRefreshEvent += SqL_DataGridView_藥庫_藥品資料_DataGridRefreshEvent;
            this.sqL_DataGridView_藥庫_藥品資料.DataGridRowsChangeRefEvent += SqL_DataGridView_藥庫_藥品資料_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_藥庫_藥品資料.RowDoubleClickEvent += SqL_DataGridView_藥庫_藥品資料_RowDoubleClickEvent;
            this.sqL_DataGridView_藥庫_藥品資料.RowEnterEvent += SqL_DataGridView_藥庫_藥品資料_RowEnterEvent;
            this.sqL_DataGridView_藥庫_藥品資料_效期及庫存.Init();
            this.DeviceBasicClass_藥庫.Init(dBConfigClass.DB_Basic, "firstclass_device_jsonstring");

            this.plC_RJ_Button_藥庫_藥品資料_匯出.MouseDownEvent += PlC_RJ_Button_藥庫_藥品資料_匯出_MouseDownEvent;
   
            this.plC_RJ_Button_藥庫_藥品資料_新增效期.MouseDownEvent += PlC_RJ_Button_藥庫_藥品資料_新增效期_MouseDownEvent;
            this.plC_RJ_Button_藥庫_藥品資料_修正庫存.MouseDownEvent += PlC_RJ_Button_藥庫_藥品資料_修正庫存_MouseDownEvent;
            this.plC_RJ_Button_藥庫_藥品資料_修正批號.MouseDownEvent += PlC_RJ_Button_藥庫_藥品資料_修正批號_MouseDownEvent;
            this.plC_RJ_Button_藥庫_藥品資料_測試清除所有效期資料.MouseDownEvent += PlC_RJ_Button_藥庫_藥品資料_測試清除所有效期資料_MouseDownEvent;
            this.plC_RJ_Button_藥庫_藥品資料_顯示有庫存藥品.MouseDownEvent += PlC_RJ_Button_藥庫_藥品資料_顯示有庫存藥品_MouseDownEvent;
            this.plC_RJ_Button_藥庫_藥品資料_匯入安全基準量.MouseDownEvent += PlC_RJ_Button_藥庫_藥品資料_匯入安全基準量_MouseDownEvent;
            this.plC_RJ_Button_藥庫_藥品資料_搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_藥品資料_搜尋_MouseDownEvent;


            this.comboBox_藥庫_藥品資料_搜尋條件.SelectedIndex = 0;
            this.parentCheckBox_藥庫_藥品資料_表單分類_全選.AddChildCheckBox(checkBox_藥庫_藥品資料_表單分類_冷藏藥);
            this.parentCheckBox_藥庫_藥品資料_表單分類_全選.AddChildCheckBox(checkBox_藥庫_藥品資料_表單分類_分包機裸錠);
            this.parentCheckBox_藥庫_藥品資料_表單分類_全選.AddChildCheckBox(checkBox_藥庫_藥品資料_表單分類_高價藥櫃);
            this.parentCheckBox_藥庫_藥品資料_表單分類_全選.AddChildCheckBox(checkBox_藥庫_藥品資料_表單分類_少用及易混);
            this.parentCheckBox_藥庫_藥品資料_表單分類_全選.AddChildCheckBox(checkBox_藥庫_藥品資料_表單分類_口服藥);
            this.parentCheckBox_藥庫_藥品資料_表單分類_全選.AddChildCheckBox(checkBox_藥庫_藥品資料_表單分類_針劑);
            this.parentCheckBox_藥庫_藥品資料_表單分類_全選.AddChildCheckBox(checkBox_藥庫_藥品資料_表單分類_水劑);
            this.parentCheckBox_藥庫_藥品資料_表單分類_全選.AddChildCheckBox(checkBox_藥庫_藥品資料_表單分類_外用藥);
            this.parentCheckBox_藥庫_藥品資料_表單分類_全選.AddChildCheckBox(checkBox_藥庫_藥品資料_表單分類_未分類);

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_藥品資料);
        }

    

        private bool flag_Program_藥庫_藥品資料_Init = false;
        private void sub_Program_藥庫_藥品資料()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "藥品資料")
            {
                if(!flag_Program_藥庫_藥品資料_Init)
                {
                    this.Function_藥庫_藥品資料_檢查表格();
                    this.Function_藥庫_藥品資料_檢查DeviceBasic();
                    flag_Program_藥庫_藥品資料_Init = true;
                }            
            }
            else
            {
                flag_Program_藥庫_藥品資料_Init = false;
            }
        }
        #region Function
        private void Function_藥庫_藥品資料_檢查表格()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(10000);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_藥品資料_資料維護_本地藥檔.SQL_GetAllRows(false);
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            Console.Write($"取得 藥庫-藥品資料,耗時 : {myTimer.ToString()} ms\n");
            List<object[]> list_Add = new List<object[]>();
            List<object> list_Delete_SerchValue = new List<object>();
            List<string[]> list_Replace_SerchValue = new List<string[]>();
            List<object[]> list_Replace_Value = new List<object[]>();

            Parallel.ForEach(list_本地藥檔, value =>
            {
                List<object[]> list_藥品資料_buf = new List<object[]>();
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_medDrugstore.藥品碼, value[(int)enum_藥品資料_資料維護_本地藥檔.藥品碼].ObjectToString());
                object[] src_value = LINQ.CopyRow(value, new enum_藥品資料_資料維護_本地藥檔(), new enum_medDrugstore());
                if (list_藥品資料_buf.Count > 0)
                {
                    object[] dst_value = LINQ.CopyRow(list_藥品資料_buf[0], new enum_medDrugstore(), new enum_medDrugstore());
                    src_value[(int)enum_medDrugstore.GUID] = dst_value[(int)enum_medDrugstore.GUID];
                    src_value[(int)enum_medDrugstore.藥局庫存] = dst_value[(int)enum_medDrugstore.藥局庫存];
                    src_value[(int)enum_medDrugstore.藥庫庫存] = dst_value[(int)enum_medDrugstore.藥庫庫存];
                    src_value[(int)enum_medDrugstore.總庫存] = dst_value[(int)enum_medDrugstore.總庫存];
                    src_value[(int)enum_medDrugstore.基準量] = dst_value[(int)enum_medDrugstore.基準量];
                    src_value[(int)enum_medDrugstore.安全庫存] = dst_value[(int)enum_medDrugstore.安全庫存];
                    src_value[(int)enum_medDrugstore.包裝數量] = dst_value[(int)enum_medDrugstore.包裝數量];
                    bool flag_IsEqual = src_value.IsEqual(dst_value, (int)enum_medDrugstore.包裝數量, (int)enum_medDrugstore.藥庫庫存, (int)enum_medDrugstore.藥局庫存, (int)enum_medDrugstore.藥庫庫存, (int)enum_medDrugstore.總庫存, (int)enum_medDrugstore.基準量, (int)enum_medDrugstore.安全庫存);
                    if (src_value[(int)enum_medDrugstore.藥庫庫存].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_medDrugstore.藥庫庫存] = "0";
                        flag_IsEqual = false;
                    }
                    if (src_value[(int)enum_medDrugstore.藥局庫存].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_medDrugstore.藥局庫存] = "0";
                        flag_IsEqual = false;
                    }
                    if (src_value[(int)enum_medDrugstore.總庫存].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_medDrugstore.總庫存] = "0";
                        flag_IsEqual = false;
                    }
                    if (src_value[(int)enum_medDrugstore.基準量].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_medDrugstore.基準量] = "0";
                        flag_IsEqual = false;
                    }
                    if (src_value[(int)enum_medDrugstore.安全庫存].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_medDrugstore.安全庫存] = "0";
                        flag_IsEqual = false;
                    }
                    if (!flag_IsEqual)
                    {
                        list_Replace_SerchValue.LockAdd(new string[] { src_value[(int)enum_medDrugstore.GUID].ObjectToString() });
                        list_Replace_Value.LockAdd(src_value);
                    }

                }
                else
                {
                    src_value[(int)enum_medDrugstore.總庫存] = "0";
                    src_value[(int)enum_medDrugstore.藥庫庫存] = "0";
                    src_value[(int)enum_medDrugstore.藥局庫存] = "0";
                    src_value[(int)enum_medDrugstore.安全庫存] = "0";
                    src_value[(int)enum_medDrugstore.基準量] = "0";
                    list_Add.LockAdd(src_value);
                }
            });
            Console.Write($"計算 藥庫-藥品資料,耗時 : {myTimer.ToString()} ms\n");
            Parallel.ForEach(list_藥品資料, value =>
            {
                List<object[]> list_本地藥檔_buf = list_本地藥檔.GetRows((int)enum_藥品資料_資料維護_本地藥檔.藥品碼, value[(int)enum_medDrugstore.藥品碼].ObjectToString());
                if (list_本地藥檔_buf.Count == 0)
                {
                    list_Delete_SerchValue.LockAdd(value[(int)enum_medDrugstore.GUID]);
                }
            });
            Console.Write($"刪除 藥庫-藥品資料 多餘資料{list_Delete_SerchValue.Count}筆,耗時 : {myTimer.ToString()} ms\n");
            this.sqL_DataGridView_藥庫_藥品資料.SQL_DeleteExtra(list_Delete_SerchValue, false);
            this.sqL_DataGridView_藥庫_藥品資料.SQL_ReplaceExtra(list_Replace_Value, false);
            this.sqL_DataGridView_藥庫_藥品資料.SQL_AddRows(list_Add, false);
            Console.Write($"上傳 藥庫-藥品資料 耗時 : {myTimer.ToString()} ms\n");

            //this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            Console.Write($"更新 藥庫-藥品資料Grid 耗時 : {myTimer.ToString()} ms\n");
        }
        private void Function_藥庫_藥品資料_匯入()
        {
            if (this.openFileDialog_LoadExcel.ShowDialog() == DialogResult.OK)
            {
                DataTable dataTable = new DataTable();
                CSVHelper.LoadFile(this.openFileDialog_LoadExcel.FileName, 0, dataTable);
                DataTable datatable_buf = dataTable.ReorderTable(new enum_medDrugstore_匯入());
                if (datatable_buf == null)
                {
                    MyMessageBox.ShowDialog("匯入檔案,資料錯誤!");
                    return;
                }
                List<object[]> list_LoadValue = datatable_buf.DataTableToRowList();

                List<object[]> list_SQL_Value = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
                List<object[]> list_Add = new List<object[]>();
                List<object[]> list_Delete_ColumnName = new List<object[]>();
                List<object[]> list_Delete_SerchValue = new List<object[]>();
                List<string> list_Replace_SerchValue = new List<string>();
                List<object[]> list_Replace_Value = new List<object[]>();
                List<object[]> list_SQL_Value_buf = new List<object[]>();

                for (int i = 0; i < list_LoadValue.Count; i++)
                {
                    object[] value_load = list_LoadValue[i];
                    list_SQL_Value_buf = list_SQL_Value.GetRows((int)enum_medDrugstore.藥品碼, value_load[(int)enum_medDrugstore_匯入.藥碼].ObjectToString());
                    if (list_SQL_Value_buf.Count > 0)
                    {
                        object[] value_SQL = list_SQL_Value_buf[0];
                        value_load = value_load.CopyRow(new enum_medDrugstore_匯入(), new enum_medDrugstore());
                        value_SQL.CopyRow(ref value_load ,new enum_medDrugstore(), new enum_medDrugstore(), (int)enum_medDrugstore.藥庫庫存, (int)enum_medDrugstore.總庫存, (int)enum_medDrugstore.安全庫存, (int)enum_medDrugstore.基準量, (int)enum_medDrugstore.藥品條碼1, (int)enum_medDrugstore.藥品條碼2);
                        bool flag_Equal = value_load.IsEqual(value_SQL);
                        if(!flag_Equal)
                        {
                            list_Replace_SerchValue.Add(value_load[(int)enum_medDrugstore.GUID].ObjectToString());
                            list_Replace_Value.Add(value_load);
                        }
                    }
                }
                this.sqL_DataGridView_藥庫_藥品資料.SQL_ReplaceExtra(enum_medDrugstore.GUID.GetEnumName(), list_Replace_SerchValue, list_Replace_Value, true);
            }
        }
        private void Function_藥庫_藥品資料_檢查DeviceBasic()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(100000);
            this.List_藥庫_DeviceBasic = DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);

            List<DeviceBasic> devices_Add = new List<DeviceBasic>();
            List<DeviceBasic> devices_Replace = new List<DeviceBasic>();

            Parallel.ForEach(list_藥品資料, value =>
            {
                string 藥品碼 = value[(int)enum_medDrugstore.藥品碼].ObjectToString();
                List<DeviceBasic> devices_buf = (from Value in this.List_藥庫_DeviceBasic
                                                 where Value.Code == 藥品碼
                                                 select Value).ToList();
                if (devices_buf.Count == 0)
                {
                    DeviceBasic device = new DeviceBasic();
                    device.Code = 藥品碼;
                    devices_Add.LockAdd(device);
                }
            });


            DeviceBasicClass_藥庫.SQL_AddDeviceBasic(devices_Add);
            Console.Write($"儲位總量新增時間 ,耗時 :{myTimer.GetTickTime().ToString("0.000")}\n");
        }
        private void Function_藥庫_藥品資料_檢視歷史效期批號()
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_藥品資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            string 藥碼 = list_value[0][(int)enum_medDrugstore.藥品碼].ObjectToString();
            string 藥名 = list_value[0][(int)enum_medDrugstore.藥品名稱].ObjectToString();
            List<string> list_效期 = new List<string>();
            List<string> list_批號 = new List<string>();
            Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥碼, ref list_效期, ref list_批號);
            if (list_效期.Count == 0)
            {
                MyMessageBox.ShowDialog("無歷史紀錄!");
                return;
            }

            Dialog_效期批號歷史紀錄 dialog_效期批號歷史紀錄 = new Dialog_效期批號歷史紀錄(藥名, list_效期, list_批號);
            dialog_效期批號歷史紀錄.ShowDialog();
        }
        private void Function_藥庫_藥品資料_藥品設定()
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_藥品資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            string 藥品碼 = list_value[0][(int)enum_medDrugstore.藥品碼].ObjectToString();
            string 藥品名稱 = list_value[0][(int)enum_medDrugstore.藥品名稱].ObjectToString();
            string 安全庫存 = list_value[0][(int)enum_medDrugstore.安全庫存].ObjectToString();
            string 基準量 = list_value[0][(int)enum_medDrugstore.基準量].ObjectToString();
            string 包裝數量 = list_value[0][(int)enum_medDrugstore.包裝數量].ObjectToString();
            string 儲位名稱 = "";

            List<DeviceBasic> list_藥庫_DeviceBasic = DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
            List<DeviceBasic> devices_buf = (from Value in list_藥庫_DeviceBasic
                                             where Value.Code == 藥品碼
                                             select Value).ToList();
            if (devices_buf.Count > 0)
            {
                儲位名稱 = devices_buf[0].StorageName;
            }
            Dialog_藥品資料設定 dialog_藥品資料設定 = new Dialog_藥品資料設定(藥品名稱, 安全庫存.StringToInt32(), 基準量.StringToInt32(), 包裝數量.StringToInt32(), 儲位名稱);
            if (dialog_藥品資料設定.ShowDialog() != DialogResult.Yes) return;

            list_value[0][(int)enum_medDrugstore.安全庫存] = dialog_藥品資料設定.安全量;
            list_value[0][(int)enum_medDrugstore.基準量] = dialog_藥品資料設定.基準量;
            list_value[0][(int)enum_medDrugstore.包裝數量] = dialog_藥品資料設定.包裝數量;
            this.sqL_DataGridView_藥庫_藥品資料.SQL_ReplaceExtra(list_value[0], false);
            this.sqL_DataGridView_藥庫_藥品資料.ReplaceExtra(list_value[0], true);

            if (devices_buf.Count > 0)
            {
                儲位名稱 = dialog_藥品資料設定.儲位名稱;
                devices_buf[0] = DeviceBasicClass_藥庫.SQL_GetDeviceBasic(devices_buf[0]);
                devices_buf[0].StorageName = 儲位名稱;
                DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(devices_buf[0]);
            }
        }
        private void Function_藥庫_藥品資料_補給系統藥品建置()
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_藥品資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            string 藥品碼 = list_value[0][(int)enum_medDrugstore.藥品碼].ObjectToString();
            string 藥品名稱 = list_value[0][(int)enum_medDrugstore.藥品名稱].ObjectToString();
            string 包裝單位 = list_value[0][(int)enum_medDrugstore.包裝單位].ObjectToString();
            List<object[]> list_藥品補給系統_藥品資料 = this.sqL_DataGridView_藥品補給系統_藥品資料.SQL_GetRows((int)enum_藥品補給系統_藥品資料.藥品碼, 藥品碼,false);
            if(list_藥品補給系統_藥品資料.Count == 0)
            {
                object[] value = new object[new enum_藥品補給系統_藥品資料().GetLength()];
                value[(int)enum_藥品補給系統_藥品資料.藥品碼] = 藥品碼;
                value[(int)enum_藥品補給系統_藥品資料.藥品名稱] = 藥品名稱;
                value[(int)enum_藥品補給系統_藥品資料.包裝單位] = 包裝單位;
                value[(int)enum_藥品補給系統_藥品資料.已採購總價] = "0";
                value[(int)enum_藥品補給系統_藥品資料.已採購總量] = "0";
                value[(int)enum_藥品補給系統_藥品資料.已採購總量上限] = "0";
                value[(int)enum_藥品補給系統_藥品資料.已訂購數量] = "0";
                value[(int)enum_藥品補給系統_藥品資料.已訂購總價] = "0";
                value[(int)enum_藥品補給系統_藥品資料.最小包裝數量] = "1";
                value[(int)enum_藥品補給系統_藥品資料.契約價金] = "1";
                value[(int)enum_藥品補給系統_藥品資料.維護到期日] = "2099/12/31";
                Dialog_補給系統藥品建置 dialog_補給系統藥品建置 = new Dialog_補給系統藥品建置(value);
                if (dialog_補給系統藥品建置.ShowDialog() != DialogResult.Yes) return;
                this.sqL_DataGridView_藥品補給系統_藥品資料.SQL_AddRow(value, false);
            }
            else
            {
                object[] value = list_藥品補給系統_藥品資料[0];
                value[(int)enum_藥品補給系統_藥品資料.藥品碼] = 藥品碼;
                value[(int)enum_藥品補給系統_藥品資料.藥品名稱] = 藥品名稱;
                value[(int)enum_藥品補給系統_藥品資料.包裝單位] = 包裝單位;
                Dialog_補給系統藥品建置 dialog_補給系統藥品建置 = new Dialog_補給系統藥品建置(value);
                if (dialog_補給系統藥品建置.ShowDialog() != DialogResult.Yes) return;
                this.sqL_DataGridView_藥品補給系統_藥品資料.SQL_Replace((int)enum_藥品補給系統_藥品資料.藥品碼 , 藥品碼, value, false);
            }
        }
        private List<object[]> Function_藥庫_藥品資料_取得庫存(List<object[]> RowsList)
        {
           return this.Function_藥庫_藥品資料_取得庫存(RowsList , true);
        }
        private List<object[]> Function_藥庫_藥品資料_取得庫存(List<object[]> RowsList ,bool reload)
        {
            List<object[]> RowsList_buf = new List<object[]>();

            Dictionary<object, List<object[]>> keyValuePairs = RowsList.ConvertToDictionary((int)enum_medDrugstore.藥品碼);
            List<Task> tasks = new List<Task>();
            if(reload)
            {
                tasks.Add(Task.Run(new Action(delegate
                {
                    MyTimerBasic myTimerBasic = new MyTimerBasic();
                    myTimerBasic.StartTickTime(5000);
                    this.List_藥庫_DeviceBasic = DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
                    Console.WriteLine($"[SqL_DataGridView_藥庫_藥品資料_DataGridRowsChangeRefEvent] 取得藥庫DeviceBasic,{myTimerBasic}ms");
                })));
                tasks.Add(Task.Run(new Action(delegate
                {
                    MyTimerBasic myTimerBasic = new MyTimerBasic();
                    myTimerBasic.StartTickTime(5000);
                    this.List_藥局_DeviceBasic = DeviceBasicClass_藥局.SQL_GetAllDeviceBasic();
                    Console.WriteLine($"[SqL_DataGridView_藥庫_藥品資料_DataGridRowsChangeRefEvent] 取得藥局DeviceBasic,{myTimerBasic}ms");
                })));
    
                Task.WhenAll(tasks).Wait();

            }

            MyTimerBasic _myTimerBasic = new MyTimerBasic();
            _myTimerBasic.StartTickTime(5000);

            Dictionary<string, List<DeviceBasic>> Dictionary_藥庫 = DeviceBasicMethod.CoverToDictionaryByCode(this.List_藥庫_DeviceBasic);
            Dictionary<string, List<DeviceBasic>> Dictionary_藥局 = DeviceBasicMethod.CoverToDictionaryByCode(this.List_藥局_DeviceBasic);
            Console.WriteLine($"「藥庫藥品資料」轉換成字典({_myTimerBasic}ms)");
            // 提前將資料轉換為整數型別，避免重複字串轉換
            var 藥庫Dictionary = Dictionary_藥庫.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(d => d.Inventory.StringToInt32()).ToList()
            );

            var 藥局Dictionary = Dictionary_藥局.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(d => d.Inventory.StringToInt32()).ToList()
            );


            Console.WriteLine($"「藥庫藥品資料」字典數值轉換({_myTimerBasic}ms)");

            // 單執行緒處理 RowsList
            foreach (var value in RowsList)
            {
                string 藥品碼 = value[(int)enum_medDrugstore.藥品碼].ObjectToString();

                int 總庫存 = 0, 藥庫庫存 = 0, 藥局庫存 = 0;

                // 藥庫計算
                if (藥庫Dictionary.TryGetValue(藥品碼, out var 藥庫Inventories))
                {
                    藥庫庫存 = 藥庫Inventories.Sum();
                    總庫存 += 藥庫庫存;
                }

                // 藥局計算
                if (藥局Dictionary.TryGetValue(藥品碼, out var 藥局Inventories))
                {
                    藥局庫存 = 藥局Inventories.Sum();
                    總庫存 += 藥局庫存;
                }

                // 更新結果
                value[(int)enum_medDrugstore.藥庫庫存] = 藥庫庫存;
                value[(int)enum_medDrugstore.藥局庫存] = 藥局庫存;
                value[(int)enum_medDrugstore.總庫存] = 總庫存;
            }
            Console.WriteLine($"「藥庫藥品資料」庫存加總({_myTimerBasic}ms)");
            return RowsList;
        }
        #endregion
        #region Event
        private bool flag_藥庫_開檔狀態顯示 = false;
        private void SqL_DataGridView_藥庫_藥品資料_ComboBoxSelectedIndexChangedEvent(object sender, string colName, object[] RowValue)
        {
            string 藥碼 = RowValue[(int)enum_medDrugstore.藥品碼].ObjectToString();
            string 類別 = RowValue[(int)enum_medDrugstore.類別].ObjectToString();
            string 開檔狀態 = RowValue[(int)enum_medDrugstore.開檔狀態].ObjectToString();

            Task.Run(new Action(delegate
            {
                medClass medClass = medClass.get_med_clouds_by_code(Main_Form.API_Server, 藥碼);
                if (medClass != null)
                {
                    medClass.類別 = 類別;
                    medClass.開檔狀態 = 開檔狀態;
                    medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClass);
                    this.sqL_DataGridView_藥庫_藥品資料.ReplaceExtra(RowValue, true);
                }

            }));
        }
        private void SqL_DataGridView_藥庫_藥品資料_RowDoubleClickEvent(object[] RowValue)
        {


        }
        private void SqL_DataGridView_藥庫_藥品資料_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                Dialog_ContextMenuStrip dialog_ContextMenuStrip = new Dialog_ContextMenuStrip(new ContextMenuStrip_藥庫_藥品資料().GetEnumNames());
                if (dialog_ContextMenuStrip.ShowDialog() == DialogResult.Yes)
                {
                    if (dialog_ContextMenuStrip.Value == ContextMenuStrip_藥庫_藥品資料.檢視歷史效期批號.GetEnumName())
                    {
                        Function_藥庫_藥品資料_檢視歷史效期批號();
                    }
                    if (dialog_ContextMenuStrip.Value == ContextMenuStrip_藥庫_藥品資料.藥品設定.GetEnumName())
                    {
                        Function_藥庫_藥品資料_藥品設定();
                    }
                    if (dialog_ContextMenuStrip.Value == ContextMenuStrip_藥庫_藥品資料.補給系統藥品建置.GetEnumName())
                    {
                        Function_藥庫_藥品資料_補給系統藥品建置();
                    }
                }
            }
     
        }
        private void SqL_DataGridView_藥庫_藥品資料_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {

            RowsList = Function_藥庫_藥品資料_取得庫存(RowsList);
            List<medClass> medClasses = medClass.get_med_cloud(Main_Form.API_Server);
   
            Dictionary<string, List<medClass>> keyValuePairs_medClasses = medClasses.CoverToDictionaryByCode();


            List<object[]> RowsList_buf = new List<object[]>();
            Parallel.ForEach(RowsList, value =>
            {

                string 藥品碼 = value[(int)enum_medDrugstore.藥品碼].ObjectToString();
                List<medClass> medClasses_buf = medClass.SortDictionaryByCode(keyValuePairs_medClasses, 藥品碼);
                if (medClasses_buf.Count > 0)
                {
                    value[(int)enum_medDrugstore.類別] = medClasses_buf[0].類別;
                    value[(int)enum_medDrugstore.開檔狀態] = medClasses_buf[0].開檔狀態;
                }
                if (checkBox_藥庫_藥品資料_近8個月效期.Checked)
                {
                    bool flag_近效期 = false;
                    int 總庫存 = 0;
                    int 藥庫庫存 = 0;
                    int 藥局庫存 = 0;
                    List<DeviceBasic> deviceBasic_藥庫_buf = this.List_藥庫_DeviceBasic.SortByCode(藥品碼);
                    List<DeviceBasic> deviceBasic_藥局_buf = this.List_藥局_DeviceBasic.SortByCode(藥品碼);

                    for (int i = 0; i < deviceBasic_藥庫_buf.Count; i++)
                    {
                        for (int k = 0; k < deviceBasic_藥庫_buf[i].List_Validity_period.Count; k++)
                        {
                            DateTime dateTime = deviceBasic_藥庫_buf[i].List_Validity_period[k].StringToDateTime();
                            int month = GetMonthsDifference(DateTime.Now, dateTime);
                            if (month <= 8)
                            {
                                flag_近效期 = true;
                            }
                        }
                    }
                    for (int i = 0; i < deviceBasic_藥局_buf.Count; i++)
                    {
                        for (int k = 0; k < deviceBasic_藥局_buf[i].List_Validity_period.Count; k++)
                        {
                            DateTime dateTime = deviceBasic_藥局_buf[i].List_Validity_period[k].StringToDateTime();
                            int month = GetMonthsDifference(DateTime.Now, dateTime);
                            if (month <= 8)
                            {
                                flag_近效期 = true;
                            }
                        }
                    }
                    if (flag_近效期) RowsList_buf.LockAdd(value);
                }
                 
            });
            if (checkBox_藥庫_藥品資料_近8個月效期.Checked) RowsList = RowsList_buf;

            List<object[]> list_buf = RowsList.GetRows((int)enum_medDrugstore.開檔狀態, "未開檔"); 
            if (flag_藥庫_開檔狀態顯示 == false) RowsList = RowsList.GetRows((int)enum_medDrugstore.開檔狀態, "開檔中");
            flag_藥庫_開檔狀態顯示 = false;
            RowsList.Sort(new ICP_藥庫_藥品資料());
        }
        private void SqL_DataGridView_藥庫_藥品資料_DataGridRowsChangeEvent(List<object[]> RowsList)
        {
            
        }
        private void SqL_DataGridView_藥庫_藥品資料_RowEnterEvent(object[] RowValue)
        {
            this.sqL_DataGridView_藥庫_藥品資料_效期及庫存.ClearGrid();
            string 藥品碼 = RowValue[(int)enum_medDrugstore.藥品碼].ObjectToString();
            List<DeviceBasic> deviceBasic_buf = this.List_藥庫_DeviceBasic.SortByCode(藥品碼);
            if (deviceBasic_buf.Count == 0) return;

            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < deviceBasic_buf[0].List_Validity_period.Count; i++)
            {
                object[] value = new object[new enum_medDrugstore_效期及庫存().GetLength()];
                value[(int)enum_medDrugstore_效期及庫存.效期] = deviceBasic_buf[0].List_Validity_period[i];
                value[(int)enum_medDrugstore_效期及庫存.批號] = deviceBasic_buf[0].List_Lot_number[i];
                value[(int)enum_medDrugstore_效期及庫存.庫存] = deviceBasic_buf[0].List_Inventory[i];
                list_value.Add(value);
            }

            this.sqL_DataGridView_藥庫_藥品資料_效期及庫存.RefreshGrid(list_value);
        }
        private void SqL_DataGridView_藥庫_藥品資料_DataGridRefreshEvent()
        {
            int 藥庫庫存 = 0;
            int 藥局庫存 = 0;
            int 庫存 = 0;
            int 基準量 = 0;
            int 安全量 = 0;
            for (int i = 0; i < this.sqL_DataGridView_藥庫_藥品資料.dataGridView.Rows.Count; i++)
            {
                藥庫庫存 = this.sqL_DataGridView_藥庫_藥品資料.dataGridView.Rows[i].Cells[enum_medDrugstore.藥庫庫存.GetEnumName()].Value.ToString().StringToInt32();
                藥局庫存 = this.sqL_DataGridView_藥庫_藥品資料.dataGridView.Rows[i].Cells[enum_medDrugstore.藥局庫存.GetEnumName()].Value.ToString().StringToInt32();
                庫存 = this.sqL_DataGridView_藥庫_藥品資料.dataGridView.Rows[i].Cells[enum_medDrugstore.總庫存.GetEnumName()].Value.ToString().StringToInt32();
                基準量 = this.sqL_DataGridView_藥庫_藥品資料.dataGridView.Rows[i].Cells[enum_medDrugstore.基準量.GetEnumName()].Value.ToString().StringToInt32();
                安全量 = this.sqL_DataGridView_藥庫_藥品資料.dataGridView.Rows[i].Cells[(int)enum_medDrugstore.安全庫存].Value.ToString().StringToInt32();
                if (庫存 <= 安全量)
                {
                    this.sqL_DataGridView_藥庫_藥品資料.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_藥庫_藥品資料.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void PlC_RJ_Button_藥庫_藥品資料_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {

            LoadingForm.ShowLoadingForm();
            try
            {
                string cmb_text = "";
                string serch_text = textBox_藥庫_藥品資料_搜尋.Text;

                this.Invoke(new Action(delegate
                {
                    cmb_text = this.comboBox_藥庫_藥品資料_搜尋條件.Text;
                }));
                if (cmb_text == "藥碼" || cmb_text == "藥名" || cmb_text == "中文名" || cmb_text == "商品名") flag_藥庫_開檔狀態顯示 = true;
                List<object[]> list_value = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
                list_value = this.sqL_DataGridView_藥庫_藥品資料.RowsChangeFunction(list_value);
                List<object[]> list_value_buf = new List<object[]>();

                if (cmb_text == "全部顯示")
                {
                    list_value_buf = list_value;
                }
                else if (cmb_text == "低於安全量")
                {
                    list_value_buf = (from temp in list_value
                                      where temp[(int)enum_medPharmacy.藥庫庫存].StringToInt32() <= temp[(int)enum_medPharmacy.安全庫存].StringToInt32()
                                      && temp[(int)enum_medPharmacy.安全庫存].StringToInt32() != 0
                                      select temp).ToList();
                }
                else
                {
                    if (serch_text.StringIsEmpty() == true)
                    {
                        MyMessageBox.ShowDialog("搜尋內容空白");
                        return;
                    }
                    if (cmb_text == "藥碼")
                    {
                        if (rJ_RatioButton_藥庫_藥品資料_搜尋方式_模糊.Checked) list_value_buf = list_value.GetRowsByLike((int)enum_medPharmacy.藥品碼, serch_text);
                        if (rJ_RatioButton_藥庫_藥品資料_搜尋方式_前綴.Checked) list_value_buf = list_value.GetRowsStartWithByLike((int)enum_medPharmacy.藥品碼, serch_text);
                    }
                    if (cmb_text == "藥名")
                    {
                        if (rJ_RatioButton_藥庫_藥品資料_搜尋方式_模糊.Checked) list_value_buf = list_value.GetRowsByLike((int)enum_medPharmacy.藥品名稱, serch_text);
                        if (rJ_RatioButton_藥庫_藥品資料_搜尋方式_前綴.Checked) list_value_buf = list_value.GetRowsStartWithByLike((int)enum_medPharmacy.藥品名稱, serch_text);
                    }
                    if (cmb_text == "中文名")
                    {
                        if (rJ_RatioButton_藥庫_藥品資料_搜尋方式_模糊.Checked) list_value_buf = list_value.GetRowsByLike((int)enum_medPharmacy.中文名稱, serch_text);
                        if (rJ_RatioButton_藥庫_藥品資料_搜尋方式_前綴.Checked) list_value_buf = list_value.GetRowsStartWithByLike((int)enum_medPharmacy.中文名稱, serch_text);
                    }
                    if (cmb_text == "商品名")
                    {
                        if (rJ_RatioButton_藥庫_藥品資料_搜尋方式_模糊.Checked) list_value_buf = list_value.GetRowsByLike((int)enum_medPharmacy.藥品學名, serch_text);
                        if (rJ_RatioButton_藥庫_藥品資料_搜尋方式_前綴.Checked) list_value_buf = list_value.GetRowsStartWithByLike((int)enum_medPharmacy.藥品學名, serch_text);
                    }
                }

                Dictionary<object, List<object[]>> keyValuePairs_開檔狀態 = list_value_buf.ConvertToDictionary((int)enum_medPharmacy.開檔狀態);
                List<object[]> list_開檔狀態 = new List<object[]>();

                list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary(enum_開檔狀態.開檔中.GetEnumName()));
                list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary(enum_開檔狀態.停用中.GetEnumName()));
                list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary(enum_開檔狀態.已取消.GetEnumName()));
                list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary(enum_開檔狀態.關檔中.GetEnumName()));
                list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary("未開檔"));
                //if (checkBox_藥庫_藥品資料_開檔中.Checked)
                //{

                //    list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary(enum_開檔狀態.開檔中.GetEnumName()));
                //    //list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary(""));

                //}
                //if (checkBox_藥庫_藥品資料_未開檔.Checked)
                //{
                //    list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary(enum_開檔狀態.停用中.GetEnumName()));
                //    list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary(enum_開檔狀態.已取消.GetEnumName()));
                //    list_開檔狀態.LockAdd(keyValuePairs_開檔狀態.SortDictionary(enum_開檔狀態.關檔中.GetEnumName()));
                //}
                list_value_buf = list_開檔狀態;

                Dictionary<object, List<object[]>> keyValuePairs_表單分類 = list_value_buf.ConvertToDictionary((int)enum_medPharmacy.類別);
                List<object[]> list_表單分類 = new List<object[]>();
                if (checkBox_藥庫_藥品資料_表單分類_冷藏藥.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.冷藏藥.GetEnumName()));
                if (checkBox_藥庫_藥品資料_表單分類_分包機裸錠.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.分包機裸錠.GetEnumName()));
                if (checkBox_藥庫_藥品資料_表單分類_高價藥櫃.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.高價藥櫃.GetEnumName()));
                if (checkBox_藥庫_藥品資料_表單分類_少用及易混.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.少用及易混.GetEnumName()));
                if (checkBox_藥庫_藥品資料_表單分類_口服藥.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.口服藥.GetEnumName()));
                if (checkBox_藥庫_藥品資料_表單分類_針劑.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.針劑.GetEnumName()));
                if (checkBox_藥庫_藥品資料_表單分類_水劑.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.水劑.GetEnumName()));
                if (checkBox_藥庫_藥品資料_表單分類_外用藥.Checked) list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.外用藥.GetEnumName()));
                if (checkBox_藥庫_藥品資料_表單分類_未分類.Checked)
                {
                    list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(enum_medType.未分類.GetEnumName()));
                    list_表單分類.LockAdd(keyValuePairs_表單分類.SortDictionary(""));
                }
                list_value_buf = list_表單分類;

                if (list_value_buf.Count == 0)
                {
                    MyMessageBox.ShowDialog("查無資料");
                    return;
                }
                list_value_buf.Sort(new ICP_藥庫_藥品資料());
                this.sqL_DataGridView_藥庫_藥品資料.RefreshGridNoEvent(list_value_buf);
            }
            catch
            {

            }
            finally
            {
                LoadingForm.CloseLoadingForm();
            }
        }

        private void PlC_RJ_Button_藥庫_藥品資料_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.List_藥庫_DeviceBasic = DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
                    this.List_藥局_DeviceBasic = DeviceBasicClass_藥局.SQL_GetAllDeviceBasic();

                    string MedPrice = Basic.Net.WEBApiGet($"{dBConfigClass.MedPrice_ApiURL}");
                    List<class_MedPrice> class_MedPrices = MedPrice.JsonDeserializet<List<class_MedPrice>>();
                    List<class_MedPrice> class_MedPrices_buf = new List<class_MedPrice>();
                    List<object[]> list_value_src = this.sqL_DataGridView_藥庫_藥品資料.GetAllRows();
                    List<object[]> list_value_out = list_value_src.CopyRows(new enum_medDrugstore(), new enum_medDrugstore_匯出());

                    for (int i = 0; i < list_value_out.Count; i++)
                    {
                        list_value_out[i][(int)enum_medDrugstore_匯出.藥碼] = list_value_src[i][(int)enum_medDrugstore.藥品碼];
                        list_value_out[i][(int)enum_medDrugstore_匯出.藥名] = list_value_src[i][(int)enum_medDrugstore.藥品名稱];


                        string 藥品碼 = list_value_out[i][(int)enum_medDrugstore_匯出.藥碼].ObjectToString();
                        if(class_MedPrices != null)
                        {
                            class_MedPrices_buf = (from value in class_MedPrices
                                                   where value.藥品碼 == 藥品碼
                                                   select value).ToList();
                            if (class_MedPrices_buf.Count > 0)
                            {
                                int 數量 = list_value_out[i][(int)enum_medDrugstore_匯出.總庫存].ObjectToString().StringToInt32();
                                double 訂購單價 = class_MedPrices_buf[0].售價.StringToDouble();
                                double 訂購總價 = 訂購單價 * 數量;
                                if (訂購單價 > 0)
                                {
                                    list_value_out[i][(int)enum_medDrugstore_匯出.採購單價] = 訂購單價.ToString("0.000").StringToDouble();
                                    list_value_out[i][(int)enum_medDrugstore_匯出.庫存金額] = 訂購總價.ToString("0.000").StringToDouble();
                                }
                            }
                        }
                    

                        List<DeviceBasic> deviceBasic_藥庫_buf = this.List_藥庫_DeviceBasic.SortByCode(藥品碼);
                        List<DeviceBasic> deviceBasic_藥局_buf = this.List_藥局_DeviceBasic.SortByCode(藥品碼);
                        List<string> list_效期及批號 = new List<string>();
                        for (int m = 0; m < deviceBasic_藥庫_buf.Count; m++)
                        {
                            for (int k = 0; k < deviceBasic_藥庫_buf[m].List_Validity_period.Count; k++)
                            {
                                DateTime dateTime = deviceBasic_藥庫_buf[m].List_Validity_period[k].StringToDateTime();
                                int month = GetMonthsDifference(DateTime.Now, dateTime);
                                if (month <= 8)
                                {
                                    string 效期 = deviceBasic_藥庫_buf[m].List_Validity_period[k];
                                    string 批號 = deviceBasic_藥庫_buf[m].List_Lot_number[k];
                                    if (批號.StringIsEmpty() == true) 批號 = "無";
                                    list_效期及批號.Add($"{效期}({批號})");
                                }
                            }
                        }
                        for (int m = 0; m < deviceBasic_藥局_buf.Count; m++)
                        {
                            for (int k = 0; k < deviceBasic_藥局_buf[m].List_Validity_period.Count; k++)
                            {
                                DateTime dateTime = deviceBasic_藥局_buf[m].List_Validity_period[k].StringToDateTime();
                                int month = GetMonthsDifference(DateTime.Now, dateTime);
                                if (month <= 8)
                                {
                                    string 效期 = deviceBasic_藥局_buf[m].List_Validity_period[k];
                                    string 批號 = deviceBasic_藥局_buf[m].List_Lot_number[k];
                                    if (批號.StringIsEmpty() == true) 批號 = "無";
                                    list_效期及批號.Add($"{效期}({批號})");
                                }
                            }

                        }
                        string 效期及批號 = "";
                        for (int k = 0; k < list_效期及批號.Count; k++)
                        {
                            效期及批號 += list_效期及批號[k];
                            if (k != list_效期及批號.Count - 1) 效期及批號 += ",";
                        }
                        list_value_out[i][(int)enum_medDrugstore_匯出.效期及批號] = 效期及批號;
                    }

                    DataTable dataTable = list_value_out.ToDataTable(new enum_medDrugstore_匯出());
                    dataTable = dataTable.ReorderTable(new enum_medDrugstore_匯出());
                    string Extension = System.IO.Path.GetExtension(this.saveFileDialog_SaveExcel.FileName);
                    if (Extension == ".txt")
                    {
                        CSVHelper.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                    }
                    else if (Extension == ".xlsx" || Extension == ".xls")
                    {
                        MyOffice.ExcelClass.NPOI_SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                    }
                    this.Cursor = Cursors.Default;
                    MyMessageBox.ShowDialog("匯出完成");
                }
            }));
 
        }
        private void PlC_RJ_Button_藥庫_藥品資料_新增效期_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.Get_All_Select_RowsValues();

                if (list_藥品資料.Count == 0)
                {
                    this.Invoke(new Action(delegate
                    {
                        MyMessageBox.ShowDialog("未選擇資料!");
                    }));

                    return;
                }
                string 藥品碼 = list_藥品資料[0][(int)enum_medDrugstore.藥品碼].ObjectToString();
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 藥品名稱 = list_藥品資料[0][(int)enum_medDrugstore.藥品名稱].ObjectToString();
                List<DeviceBasic> deviceBasic_buf = this.List_藥庫_DeviceBasic.SortByCode(藥品碼);
                if (deviceBasic_buf.Count == 0)
                {
                    this.Invoke(new Action(delegate
                    {
                        MyMessageBox.ShowDialog("找無此藥品內容!");
                    }));
                    return;
                }
                string 效期 = "";
                string 批號 = "";
                string 數量 = "";
                Dialog_DateTime dialog_DateTime = new Dialog_DateTime();
                if (dialog_DateTime.ShowDialog() == DialogResult.Yes)
                {
                    效期 = dialog_DateTime.Value.ToDateString();
                }
                else
                {
                    return;
                }
                Dialog_寫入批號 dialog_寫入批號 = new Dialog_寫入批號();
                if (dialog_寫入批號.ShowDialog() == DialogResult.Yes)
                {
                    批號 = dialog_寫入批號.Value;
                }
                else
                {
                    return;
                }
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
                if (dialog_NumPannel.ShowDialog() == DialogResult.Yes)
                {
                    數量 = dialog_NumPannel.Value.ToString();
                }
                else
                {
                    return;
                }

                int 原有庫存 = deviceBasic_buf[0].取得庫存();


                string 庫存量 = deviceBasic_buf[0].取得庫存().ToString();
                deviceBasic_buf[0].效期庫存覆蓋(效期, 批號, 數量);
                int 修正庫存 = deviceBasic_buf[0].取得庫存();
                this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasic_buf[0]);

                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.新增效期.GetEnumName();

                string 交易量 = (修正庫存 - 原有庫存).ToString();
                string 結存量 = deviceBasic_buf[0].取得庫存().ToString();
                string 操作人 = this.登入者名稱;
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}],批號[{批號}]";

                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.藥庫.GetEnumName();
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);


                List<object[]> list_value = new List<object[]>();
                for (int i = 0; i < deviceBasic_buf[0].List_Validity_period.Count; i++)
                {
                    object[] value = new object[new enum_medDrugstore_效期及庫存().GetLength()];
                    value[(int)enum_medDrugstore_效期及庫存.效期] = deviceBasic_buf[0].List_Validity_period[i];
                    value[(int)enum_medDrugstore_效期及庫存.批號] = deviceBasic_buf[0].List_Lot_number[i];
                    value[(int)enum_medDrugstore_效期及庫存.庫存] = deviceBasic_buf[0].List_Inventory[i];
                    list_value.Add(value);
                }
                this.sqL_DataGridView_藥庫_藥品資料_效期及庫存.RefreshGrid(list_value);

            }));
        }
        private void PlC_RJ_Button_藥庫_藥品資料_修正批號_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.Get_All_Select_RowsValues();

                if (list_藥品資料.Count == 0)
                {
                    this.Invoke(new Action(delegate
                    {
                        MyMessageBox.ShowDialog("未選擇資料!");
                    }));

                    return;
                }
                string 藥品碼 = list_藥品資料[0][(int)enum_medDrugstore.藥品碼].ObjectToString();
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 藥品名稱 = list_藥品資料[0][(int)enum_medDrugstore.藥品名稱].ObjectToString();
                List<DeviceBasic> deviceBasic_buf = this.List_藥庫_DeviceBasic.SortByCode(藥品碼);
                if (deviceBasic_buf.Count == 0)
                {
                    this.Invoke(new Action(delegate
                    {
                        MyMessageBox.ShowDialog("找無此藥品內容!");
                    }));
                    return;
                }


                object[] value = sqL_DataGridView_藥庫_藥品資料_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }
                string 效期 = value[(int)enum_medDrugstore_效期及庫存.效期].ObjectToString();
                string 舊批號 = value[(int)enum_medDrugstore_效期及庫存.批號].ObjectToString();
                string 新批號 = "";

                Dialog_寫入批號 dialog_寫入批號 = new Dialog_寫入批號();
                if (dialog_寫入批號.ShowDialog() == DialogResult.Yes)
                {
                    新批號 = dialog_寫入批號.Value;
                }
                else
                {
                    return;
                }


                deviceBasic_buf[0].修正批號(效期, 新批號);
                this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasic_buf[0]);


                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.修正批號.GetEnumName();
                string 交易量 = (0).ToString();
                string 結存量 = 0.ToString();
                string 操作人 = this.登入者名稱;
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}]新批號[{新批號}]";

                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.藥庫.GetEnumName();
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 0.ToString();
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);

                List<object[]> list_value = new List<object[]>();
                for (int i = 0; i < deviceBasic_buf[0].List_Validity_period.Count; i++)
                {
                    object[] value_0 = new object[new enum_medDrugstore_效期及庫存().GetLength()];
                    value_0[(int)enum_medDrugstore_效期及庫存.效期] = deviceBasic_buf[0].List_Validity_period[i];
                    value_0[(int)enum_medDrugstore_效期及庫存.批號] = deviceBasic_buf[0].List_Lot_number[i];
                    value_0[(int)enum_medDrugstore_效期及庫存.庫存] = deviceBasic_buf[0].List_Inventory[i];
                    list_value.Add(value_0);
                }
                this.sqL_DataGridView_藥庫_藥品資料_效期及庫存.RefreshGrid(list_value);
            }));
        }
        private void PlC_RJ_Button_藥庫_藥品資料_修正庫存_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.Get_All_Select_RowsValues();

                if (list_藥品資料.Count == 0)
                {
                    this.Invoke(new Action(delegate
                    {
                        MyMessageBox.ShowDialog("未選擇資料!");
                    }));

                    return;
                }
                string 藥品碼 = list_藥品資料[0][(int)enum_medDrugstore.藥品碼].ObjectToString();
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 藥品名稱 = list_藥品資料[0][(int)enum_medDrugstore.藥品名稱].ObjectToString();
                List<DeviceBasic> deviceBasic_buf = this.List_藥庫_DeviceBasic.SortByCode(藥品碼);
                if (deviceBasic_buf.Count == 0)
                {
                    this.Invoke(new Action(delegate
                    {
                        MyMessageBox.ShowDialog("找無此藥品內容!");
                    }));
                    return;
                }


                object[] value = sqL_DataGridView_藥庫_藥品資料_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }

                string 效期 = value[(int)enum_medDrugstore_效期及庫存.效期].ObjectToString();
                string 批號 = value[(int)enum_medDrugstore_效期及庫存.批號].ObjectToString();
                string 數量 = "";
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
                if (dialog_NumPannel.ShowDialog() == DialogResult.Yes)
                {
                    數量 = dialog_NumPannel.Value.ToString();
                }
                else
                {
                    return;
                }


                int 原有庫存 = deviceBasic_buf[0].取得庫存();
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 庫存量 = deviceBasic_buf[0].Inventory;
                deviceBasic_buf[0].效期庫存覆蓋(效期, 數量);
                int 修正庫存 = deviceBasic_buf[0].取得庫存();
                this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasic_buf[0]);

                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.修正庫存.GetEnumName();
                string 交易量 = (修正庫存 - 原有庫存).ToString();
                string 結存量 = deviceBasic_buf[0].Inventory;
                string 操作人 = this.登入者名稱;
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}],批號[{批號}]";
                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.藥庫.GetEnumName();
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);
                List<object[]> list_value = new List<object[]>();
                for (int i = 0; i < deviceBasic_buf[0].List_Validity_period.Count; i++)
                {
                    object[] value_0 = new object[new enum_medDrugstore_效期及庫存().GetLength()];
                    value_0[(int)enum_medDrugstore_效期及庫存.效期] = deviceBasic_buf[0].List_Validity_period[i];
                    value_0[(int)enum_medDrugstore_效期及庫存.批號] = deviceBasic_buf[0].List_Lot_number[i];
                    value_0[(int)enum_medDrugstore_效期及庫存.庫存] = deviceBasic_buf[0].List_Inventory[i];
                    list_value.Add(value_0);
                }
                this.sqL_DataGridView_藥庫_藥品資料_效期及庫存.RefreshGrid(list_value);
            }));
        }
        private void PlC_RJ_Button_藥庫_藥品資料_顯示有庫存藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            this.sqL_DataGridView_藥庫_藥品資料.RowsChangeFunction(list_value);
            list_value = (from value in list_value
                          where value[(int)enum_medDrugstore.總庫存].StringToInt32() > 0
                          select value).ToList();
            this.sqL_DataGridView_藥庫_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥庫_藥品資料_測試清除所有效期資料_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("確認測試清除所有效期資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            List<DeviceBasic> deviceBasics = this.DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
            Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(deviceBasics.Count);
            dialog_Prcessbar.State = "清除儲位庫存資料";
            for (int i = 0; i < deviceBasics.Count; i++)
            {
                dialog_Prcessbar.Value = i;
                deviceBasics[i].清除所有庫存資料();
            }

            dialog_Prcessbar.State = "上傳儲位資料...";
            this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasics);
            dialog_Prcessbar.Close();
        }
        private void PlC_RJ_Button_藥庫_藥品資料_匯入安全基準量_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (this.openFileDialog_LoadExcel.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = new DataTable();
                    string Extension = System.IO.Path.GetExtension(this.openFileDialog_LoadExcel.FileName);

                    if (Extension == ".txt") CSVHelper.LoadFile(this.openFileDialog_LoadExcel.FileName, 0, dataTable);
                    else if (Extension == ".xls" || Extension == ".xlsx") dataTable = MyOffice.ExcelClass.NPOI_LoadFile(this.openFileDialog_LoadExcel.FileName);
                    DataTable datatable_buf = dataTable.ReorderTable(new enum_medDrugstore_匯入_安全基準量());
                    if (datatable_buf == null)
                    {
                        MyMessageBox.ShowDialog("匯入檔案,資料錯誤!");
                        return;
                    }
                    List<object[]> list_LoadValue = datatable_buf.DataTableToRowList();
                    List<object[]> list_LoadValue_buf = new List<object[]>();
                    List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
                    List<object[]> list_藥品資料_replace = new List<object[]>();
                    for (int i = 0; i < list_藥品資料.Count; i++)
                    {
                        string 藥品碼 = list_藥品資料[i][(int)enum_medDrugstore.藥品碼].ObjectToString();
                        list_LoadValue_buf = list_LoadValue.GetRows((int)enum_medDrugstore_匯入_安全基準量.藥碼, 藥品碼);
                        if(list_LoadValue_buf.Count > 0)
                        {
                            list_藥品資料[i][(int)enum_medDrugstore.安全庫存] = list_LoadValue_buf[0][(int)enum_medDrugstore_匯入_安全基準量.安全量].ObjectToString();
                            list_藥品資料[i][(int)enum_medDrugstore.基準量] = list_LoadValue_buf[0][(int)enum_medDrugstore_匯入_安全基準量.基準量].ObjectToString();
                            list_藥品資料_replace.Add(list_藥品資料[i]);
                        }
                    }
                    this.sqL_DataGridView_藥庫_藥品資料.SQL_ReplaceExtra(list_藥品資料_replace, false);
                    this.sqL_DataGridView_藥庫_藥品資料.ReplaceExtra(list_藥品資料_replace, true);

                    MyMessageBox.ShowDialog($"匯入藥品安全量及基準量成功!共修改<{list_藥品資料_replace.Count}>筆資料");
               
                }
            }));
        }
        private static int GetMonthsDifference(DateTime startDate, DateTime endDate)
        {
            int months = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;
            if (endDate.Day < startDate.Day)
            {
                months--;
            }
            return months;
        }
        #endregion

        private class ICP_藥庫_藥品資料 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string Code0 = x[(int)enum_medDrugstore.藥品碼].ObjectToString();
                string Code1 = y[(int)enum_medDrugstore.藥品碼].ObjectToString();
                return Code0.CompareTo(Code1);
            }
        }

        public static Dictionary<string, List<DrugInventory>> ConvertToDrugInventoryDictionary(List<DeviceBasic> deviceBasics)
        {
            // 創建字典以藥碼為鍵，DrugInventory列表為值
            return deviceBasics
                .GroupBy(device => device.Code) // 按藥碼分組
                .ToDictionary(
                    group => group.Key, // 藥碼作為鍵
                    group => group.Select(device => new DrugInventory(device.Code, device.Inventory.StringToInt32())).ToList()
                );
        }
    }
}
