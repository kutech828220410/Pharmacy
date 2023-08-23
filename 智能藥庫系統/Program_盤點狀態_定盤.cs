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
using SQLUI;
using MyUI;
using Basic;
using H_Pannel_lib;
using HIS_DB_Lib;
using MyOffice;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        public enum enum_定盤_盤點明細
        {
            GUID,
            藥碼,
            藥名,
            庫存值,
            異動量,
            盤點量,         
            效期及批號,
        }
        private void sub_Program_盤點作業_定盤_Init()
        {

            this.sqL_DataGridView_定盤_盤點明細.Init();

            this.plC_RJ_Button_定盤_盤點明細_上傳Excel.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_上傳Excel_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_重置作業.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_重置作業_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_計算定盤結果.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_計算定盤結果_MouseDownEvent;
            this.plC_RJ_Button_定盤_盤點明細_確認更動庫存值.MouseDownEvent += PlC_RJ_Button_定盤_盤點明細_確認更動庫存值_MouseDownEvent;
            this.PlC_RJ_Button_定盤_盤點明細_重置作業_MouseDownEvent(null);
            this.plC_UI_Init.Add_Method(sub_Program_盤點作業_定盤);
        }

      

        private bool flag_Program_盤點作業_定盤_Init = false;
        private void sub_Program_盤點作業_定盤()
        {
            if (this.plC_ScreenPage_Main.PageText == "盤點作業" && this.plC_ScreenPage_盤點作業.PageText == "定盤")
            {
                if (!flag_Program_盤點作業_定盤_Init)
                {
                    flag_Program_盤點作業_定盤_Init = true;
                }
            }
            else
            {
                flag_Program_盤點作業_定盤_Init = false;
            }
        }

        #region Function

        #endregion
        #region Event
        private void PlC_RJ_Button_定盤_盤點明細_重置作業_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                sqL_DataGridView_定盤_盤點明細.ClearGrid();
                plC_RJ_Button_定盤_盤點明細_上傳Excel.Enabled = true;
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = false;
                comboBox_定盤_盤點明細_庫別.Enabled = false;
                plC_RJ_Button_定盤_盤點明細_確認更動庫存值.Enabled = false;
            }));
        }
        private void PlC_RJ_Button_定盤_盤點明細_上傳Excel_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Cursor = Cursors.WaitCursor;
            }));

            DialogResult dialogResult = DialogResult.None;

            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));
                return;
            }
            string json = Net.UploadFileToApi(this.openFileDialog_LoadExcel.FileName, $"{Api_URL}/api/inventory/excel_upload", "");
            returnData returnData = json.JsonDeserializet<returnData>();
            Console.WriteLine(json);
            if (returnData == null)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));
                MyMessageBox.ShowDialog("上傳文件失敗,請檢查格式是否正確");
                return;
            }
            if (returnData.Code != 200)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));
                MyMessageBox.ShowDialog($"{returnData.Result}");
                return;
            }
            inventoryClass.creat creat = returnData.Data.ObjToClass<inventoryClass.creat>();
            if (creat == null)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));
                MyMessageBox.ShowDialog("取得資料為空白");
                return;
            }
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < creat.Contents.Count; i++)
            {
                object[] value = new object[new enum_定盤_盤點明細().GetLength()];
                value[(int)enum_定盤_盤點明細.GUID] = creat.Contents[i].GUID;
                value[(int)enum_定盤_盤點明細.藥碼] = creat.Contents[i].藥品碼;
                value[(int)enum_定盤_盤點明細.藥名] = creat.Contents[i].藥品名稱;
                value[(int)enum_定盤_盤點明細.盤點量] = creat.Contents[i].盤點量;

                list_value.Add(value);
            }
            this.sqL_DataGridView_定盤_盤點明細.RefreshGrid(list_value);
            this.Invoke(new Action(delegate
            {
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = true;
                comboBox_定盤_盤點明細_庫別.Enabled = true;
                plC_RJ_Button_定盤_盤點明細_確認更動庫存值.Enabled = false;
            }));
            this.Invoke(new Action(delegate
            {
                this.Cursor = Cursors.Default;
            }));
        }
        private void PlC_RJ_Button_定盤_盤點明細_計算定盤結果_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
            }));
           
            string 庫別 = "";
            this.Invoke(new Action(delegate
            {
                庫別 = comboBox_定盤_盤點明細_庫別.Text;
            }));
            if (庫別.StringIsEmpty() == true)
            {
                MyMessageBox.ShowDialog("未選擇庫別");
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));
              
                return;
            }
            Function_從SQL取得儲位到本地資料();
            if (庫別 == "藥庫")
            {
      
                List<object[]> list_盤點明細 = this.sqL_DataGridView_定盤_盤點明細.GetAllRows();
                Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_盤點明細.Count);
                dialog_Prcessbar.State = "盤點明細計算...";
                for (int i = 0; i < list_盤點明細.Count; i++)
                {
                    dialog_Prcessbar.Value = i;
                    string 備註 = "";
                    string 藥碼 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥碼].ObjectToString();
                    int 盤點量 = list_盤點明細[i][(int)enum_定盤_盤點明細.盤點量].StringToInt32();
                    int 庫存值 = 0;
                    int 異動量 = 0;
                    List<DeviceBasic> deviceBasics = this.List_藥庫_DeviceBasic.SortByCode(藥碼);
                    if (deviceBasics.Count == 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.庫存值] = "異常";
                        continue;
                    }
                    庫存值 = deviceBasics[0].Inventory.StringToInt32();
                    異動量 = 盤點量 - 庫存值;
                    list_盤點明細[i][(int)enum_定盤_盤點明細.庫存值] = 庫存值.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.異動量] = 異動量.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] = "";
                    List<string> list_效期 = new List<string>();
                    List<string> list_批號 = new List<string>();
                    List<string> list_異動量 = new List<string>();
                    if (異動量 == 0)
                    {
                        continue;
                    }
                    if (庫存值 == 0 || 異動量 > 0)
                    {
                    
                        Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥碼, ref list_效期, ref list_批號);
                        if (list_效期.Count != 0)
                        {
                            if (list_批號[0].StringIsEmpty() == true) list_批號[0] = "無";
                            備註 += $"[效期]:{list_效期[0]},[批號]:{list_批號[0]}";
                            deviceBasics[0].效期庫存異動(list_效期[0], list_批號[0], 異動量.ToString());
                        }
                        else
                        {
                            備註 += $"[效期]:{"2000/12/31"},[批號]:{"系統代入"}";
                            deviceBasics[0].效期庫存異動("2000/12/31", "系統代入", 異動量.ToString());
                        }
                    }
                    else
                    {
                        deviceBasics[0].庫存異動(異動量, out list_效期, out list_批號, out list_異動量);
                        for (int k = 0; k < list_效期.Count; k++)
                        {
                            備註 += $"[效期]:{list_效期[k]},[批號]:{list_批號[k]}";
                            if (k != list_效期.Count - 1) 備註 += "\n";
                        }
                    }
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] += 備註;
                }
                dialog_Prcessbar.Close();
                dialog_Prcessbar.Dispose();
                this.sqL_DataGridView_定盤_盤點明細.RefreshGrid(list_盤點明細);
            }
            if (庫別 == "藥局")
            {
                List<object[]> list_盤點明細 = this.sqL_DataGridView_定盤_盤點明細.GetAllRows();
                Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_盤點明細.Count);
                dialog_Prcessbar.State = "盤點明細計算...";
                for (int i = 0; i < list_盤點明細.Count; i++)
                {
                    dialog_Prcessbar.Value = i;
                    string 備註 = "";
                    string 藥碼 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥碼].ObjectToString();
                    int 盤點量 = list_盤點明細[i][(int)enum_定盤_盤點明細.盤點量].StringToInt32();
                    int 庫存值 = 0;
                    int 異動量 = 0;
                    List<DeviceBasic> deviceBasics = this.List_藥局_DeviceBasic.SortByCode(藥碼);
                    if (deviceBasics.Count == 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.庫存值] = "異常";
                        continue;
                    }
                    庫存值 = deviceBasics[0].Inventory.StringToInt32();
                    異動量 = 盤點量 - 庫存值;
                    list_盤點明細[i][(int)enum_定盤_盤點明細.庫存值] = 庫存值.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.異動量] = 異動量.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] = "";
                    List<string> list_效期 = new List<string>();
                    List<string> list_批號 = new List<string>();
                    List<string> list_異動量 = new List<string>();
                    if (異動量 == 0)
                    {
                        continue;
                    }
                    if (庫存值 == 0 || 異動量 > 0)
                    {

                        Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥碼, ref list_效期, ref list_批號);
                        if (list_效期.Count != 0)
                        {
                            if (list_批號[0].StringIsEmpty() == true) list_批號[0] = "無";
                            備註 += $"[效期]:{list_效期[0]},[批號]:{list_批號[0]}";
                            deviceBasics[0].效期庫存異動(list_效期[0], list_批號[0], 異動量.ToString());
                        }
                        else
                        {
                            備註 += $"[效期]:{"2000/12/31"},[批號]:{"系統代入"}";
                            deviceBasics[0].效期庫存異動("2000/12/31", "系統代入", 異動量.ToString());
                        }
                    }
                    else
                    {
                        deviceBasics[0].庫存異動(異動量, out list_效期, out list_批號, out list_異動量);
                        for (int k = 0; k < list_效期.Count; k++)
                        {
                            備註 += $"[效期]:{list_效期[k]},[批號]:{list_批號[k]}";
                            if (k != list_效期.Count - 1) 備註 += "\n";
                        }
                    }
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] += 備註;
                }
                dialog_Prcessbar.Close();
                dialog_Prcessbar.Dispose();
                this.sqL_DataGridView_定盤_盤點明細.RefreshGrid(list_盤點明細);
            }
            this.Invoke(new Action(delegate
            {
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = true;
                comboBox_定盤_盤點明細_庫別.Enabled = false;
                plC_RJ_Button_定盤_盤點明細_確認更動庫存值.Enabled = true;
                this.Cursor = Cursors.Default;
            }));
        }
        private void PlC_RJ_Button_定盤_盤點明細_確認更動庫存值_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
            }));

            string 庫別 = "";
            this.Invoke(new Action(delegate
            {
                庫別 = comboBox_定盤_盤點明細_庫別.Text;
            }));
            if (庫別.StringIsEmpty() == true)
            {
                MyMessageBox.ShowDialog("未選擇庫別");
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));

                return;
            }
            List<object[]> list_盤點明細 = this.sqL_DataGridView_定盤_盤點明細.GetAllRows();
            List<DeviceBasic> deviceBasics_replace = new List<DeviceBasic>();
            List<object[]> list_交易紀錄_Add = new List<object[]>();
            Function_從SQL取得儲位到本地資料();
            if (庫別 == "藥庫")
            {
                Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_盤點明細.Count);
                dialog_Prcessbar.State = "盤點明細計算...";
                for (int i = 0; i < list_盤點明細.Count; i++)
                {
                    dialog_Prcessbar.Value = i;
                    string 備註 = "";
                    string 藥碼 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥碼].ObjectToString();
                    int 盤點量 = list_盤點明細[i][(int)enum_定盤_盤點明細.盤點量].StringToInt32();
                    int 庫存值 = 0;
                    int 異動量 = 0;
                    List<DeviceBasic> deviceBasics = this.List_藥庫_DeviceBasic.SortByCode(藥碼);
                    if (deviceBasics.Count == 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.庫存值] = "異常";
                        continue;
                    }
                    庫存值 = deviceBasics[0].Inventory.StringToInt32();
                    異動量 = 盤點量 - 庫存值;
                    list_盤點明細[i][(int)enum_定盤_盤點明細.庫存值] = 庫存值.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.異動量] = 異動量.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] = "";
                    List<string> list_效期 = new List<string>();
                    List<string> list_批號 = new List<string>();
                    List<string> list_異動量 = new List<string>();
                    if (異動量 == 0)
                    {
                        continue;
                    }
                    if (庫存值 == 0 || 異動量 > 0)
                    {

                        Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥碼, ref list_效期, ref list_批號);
                        if (list_效期.Count != 0)
                        {
                            if (list_批號[0].StringIsEmpty() == true) list_批號[0] = "無";
                            備註 += $"[效期]:{list_效期[0]},[批號]:{list_批號[0]}";
                            deviceBasics[0].效期庫存異動(list_效期[0], list_批號[0], 異動量.ToString());
                        }
                        else
                        {
                            備註 += $"[效期]:{"2000/12/31"},[批號]:{"系統代入"}";
                            deviceBasics[0].效期庫存異動("2000/12/31", "系統代入", 異動量.ToString());
                        }
                    }
                    else
                    {
                        deviceBasics[0].庫存異動(異動量, out list_效期, out list_批號, out list_異動量);
                        for (int k = 0; k < list_效期.Count; k++)
                        {
                            備註 += $"[效期]:{list_效期[k]},[批號]:{list_批號[k]}";
                            if (k != list_效期.Count - 1) 備註 += "\n";
                        }
                    }
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] += 備註;

                    transactionsClass transactionsClass = new transactionsClass();
                    transactionsClass.GUID = Guid.NewGuid().ToString();
                    transactionsClass.動作 = enum_交易記錄查詢動作.盤存盈虧.GetEnumName();
                    transactionsClass.庫存量 = 庫存值.ToString();
                    transactionsClass.交易量 = 異動量.ToString();
                    transactionsClass.結存量 = 盤點量.ToString();
                    transactionsClass.庫別 = "藥庫";
                    transactionsClass.操作人 = 登入者名稱;
                    transactionsClass.操作時間 = DateTime.Now.ToDateTimeString_6();
                    transactionsClass.備註 = 備註;
                    object[] trading_value = transactionsClass.ClassToSQL<transactionsClass , enum_交易記錄查詢資料>();

                    list_交易紀錄_Add.Add(trading_value);
                    deviceBasics_replace.Add(deviceBasics[0]);
                }
                if (deviceBasics_replace.Count > 0) this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasics_replace);
                if (list_交易紀錄_Add.Count > 0) sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄_Add, false);
                dialog_Prcessbar.Close();
                dialog_Prcessbar.Dispose();

            }
            if (庫別 == "藥局")
            {
                Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_盤點明細.Count);
                dialog_Prcessbar.State = "盤點明細計算...";
                for (int i = 0; i < list_盤點明細.Count; i++)
                {
                    dialog_Prcessbar.Value = i;
                    string 備註 = "";
                    string 藥碼 = list_盤點明細[i][(int)enum_定盤_盤點明細.藥碼].ObjectToString();
                    int 盤點量 = list_盤點明細[i][(int)enum_定盤_盤點明細.盤點量].StringToInt32();
                    int 庫存值 = 0;
                    int 異動量 = 0;
                    List<DeviceBasic> deviceBasics = this.List_藥局_DeviceBasic.SortByCode(藥碼);
                    if (deviceBasics.Count == 0)
                    {
                        list_盤點明細[i][(int)enum_定盤_盤點明細.庫存值] = "異常";
                        continue;
                    }
                    庫存值 = deviceBasics[0].Inventory.StringToInt32();
                    異動量 = 盤點量 - 庫存值;
                    list_盤點明細[i][(int)enum_定盤_盤點明細.庫存值] = 庫存值.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.異動量] = 異動量.ToString();
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] = "";
                    List<string> list_效期 = new List<string>();
                    List<string> list_批號 = new List<string>();
                    List<string> list_異動量 = new List<string>();
                    if (異動量 == 0)
                    {
                        continue;
                    }
                    if (庫存值 == 0 || 異動量 > 0)
                    {

                        Funnction_交易記錄查詢_取得指定藥碼批號期效期(藥碼, ref list_效期, ref list_批號);
                        if (list_效期.Count != 0)
                        {
                            if (list_批號[0].StringIsEmpty() == true) list_批號[0] = "無";
                            備註 += $"[效期]:{list_效期[0]},[批號]:{list_批號[0]}";
                            deviceBasics[0].效期庫存異動(list_效期[0], list_批號[0], 異動量.ToString());
                        }
                        else
                        {
                            備註 += $"[效期]:{"2000/12/31"},[批號]:{"系統代入"}";
                            deviceBasics[0].效期庫存異動("2000/12/31", "系統代入", 異動量.ToString());
                        }
                    }
                    else
                    {
                        deviceBasics[0].庫存異動(異動量, out list_效期, out list_批號, out list_異動量);
                        for (int k = 0; k < list_效期.Count; k++)
                        {
                            備註 += $"[效期]:{list_效期[k]},[批號]:{list_批號[k]}";
                            if (k != list_效期.Count - 1) 備註 += "\n";
                        }
                    }
                    list_盤點明細[i][(int)enum_定盤_盤點明細.效期及批號] += 備註;

                    transactionsClass transactionsClass = new transactionsClass();
                    transactionsClass.GUID = Guid.NewGuid().ToString();
                    transactionsClass.動作 = enum_交易記錄查詢動作.盤存盈虧.GetEnumName();
                    transactionsClass.庫存量 = 庫存值.ToString();
                    transactionsClass.交易量 = 異動量.ToString();
                    transactionsClass.結存量 = 盤點量.ToString();
                    transactionsClass.庫別 = "藥局";
                    transactionsClass.操作人 = 登入者名稱;
                    transactionsClass.操作時間 = DateTime.Now.ToDateTimeString_6();
                    transactionsClass.備註 = 備註;
                    object[] trading_value = transactionsClass.ClassToSQL<transactionsClass, enum_交易記錄查詢資料>();

                    list_交易紀錄_Add.Add(trading_value);
                    deviceBasics_replace.Add(deviceBasics[0]);
                }
                if (deviceBasics_replace.Count > 0) this.DeviceBasicClass_藥局.SQL_ReplaceDeviceBasic(deviceBasics_replace);
                if (list_交易紀錄_Add.Count > 0) sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄_Add, false);
                dialog_Prcessbar.Close();
                dialog_Prcessbar.Dispose();

            }
            this.sqL_DataGridView_定盤_盤點明細.RefreshGrid(list_盤點明細);
            this.Invoke(new Action(delegate
            {
                plC_RJ_Button_定盤_盤點明細_計算定盤結果.Enabled = false;
                comboBox_定盤_盤點明細_庫別.Enabled = false;
                this.PlC_RJ_Button_定盤_盤點明細_重置作業_MouseDownEvent(null);
                this.Cursor = Cursors.Default;
                MyMessageBox.ShowDialog("定盤庫存調整完成!");
            }));
        }
        #endregion


    }
}
