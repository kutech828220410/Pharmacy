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
namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {
        public enum enum_藥局_藥品資料
        {
            GUID,
            藥品碼,
            中文名稱,
            藥品名稱,
            藥品學名,
            藥品群組,
            健保碼,
            包裝單位,
            包裝數量,
            最小包裝單位,
            最小包裝數量,
            藥局庫存,
            藥庫庫存,
            總庫存,
            基準量,
            安全庫存,
            藥品條碼1,
            藥品條碼2,
            狀態,
        }

        private void sub_Program_藥局_藥品資料_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_藥局_藥品資料, dBConfigClass.DB_DS01);
            this.sqL_DataGridView_藥局_藥品資料.Init();

            this.plC_UI_Init.Add_Method(sub_Program_藥局_藥品資料);
        }

        private bool flag_藥局_藥品資料 = false;
        private void sub_Program_藥局_藥品資料()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥品資料")
            {
                if (!this.flag_藥局_藥品資料)
                {
                    this.flag_藥局_藥品資料 = true;
                }

            }
            else
            {
                this.flag_藥局_藥品資料 = false;
            }
        }


        private void Function_藥局_藥品資料_檢查表格()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(10000);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_本地_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥局_藥品資料.SQL_GetAllRows(false);

            List<object[]> list_Add = new List<object[]>();
            List<object> list_Delete_SerchValue = new List<object>();
            List<string[]> list_Replace_SerchValue = new List<string[]>();
            List<object[]> list_Replace_Value = new List<object[]>();

            Parallel.ForEach(list_本地藥檔, value =>
            {
                List<object[]> list_藥品資料_buf = new List<object[]>();
                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥局_藥品資料.藥品碼, value[(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                object[] src_value = LINQ.CopyRow(value, new enum_本地_藥品資料(), new enum_藥局_藥品資料());
                if (list_藥品資料_buf.Count > 0)
                {
                    object[] dst_value = LINQ.CopyRow(list_藥品資料_buf[0], new enum_藥局_藥品資料(), new enum_藥局_藥品資料());
                    src_value[(int)enum_藥局_藥品資料.GUID] = dst_value[(int)enum_藥局_藥品資料.GUID];
                    src_value[(int)enum_藥局_藥品資料.藥局庫存] = dst_value[(int)enum_藥局_藥品資料.藥局庫存];
                    src_value[(int)enum_藥局_藥品資料.藥庫庫存] = dst_value[(int)enum_藥局_藥品資料.藥庫庫存];
                    src_value[(int)enum_藥局_藥品資料.總庫存] = dst_value[(int)enum_藥局_藥品資料.總庫存];
                    src_value[(int)enum_藥局_藥品資料.基準量] = dst_value[(int)enum_藥局_藥品資料.基準量];
                    src_value[(int)enum_藥局_藥品資料.安全庫存] = dst_value[(int)enum_藥局_藥品資料.安全庫存];
                    bool flag_IsEqual = src_value.IsEqual(dst_value, (int)enum_藥局_藥品資料.藥局庫存, (int)enum_藥局_藥品資料.藥庫庫存, (int)enum_藥局_藥品資料.總庫存, (int)enum_藥局_藥品資料.基準量, (int)enum_藥局_藥品資料.安全庫存);
                    if (src_value[(int)enum_藥局_藥品資料.藥庫庫存].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_藥局_藥品資料.藥庫庫存] = "0";
                        flag_IsEqual = false;
                    }
                    if (src_value[(int)enum_藥局_藥品資料.藥局庫存].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_藥局_藥品資料.藥局庫存] = "0";
                        flag_IsEqual = false;
                    }
                    if (src_value[(int)enum_藥局_藥品資料.總庫存].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_藥局_藥品資料.總庫存] = "0";
                        flag_IsEqual = false;
                    }
                    if (src_value[(int)enum_藥局_藥品資料.基準量].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_藥局_藥品資料.基準量] = "0";
                        flag_IsEqual = false;
                    }
                    if (src_value[(int)enum_藥局_藥品資料.安全庫存].ObjectToString().StringIsEmpty())
                    {
                        src_value[(int)enum_藥局_藥品資料.安全庫存] = "0";
                        flag_IsEqual = false;
                    }
                    if (!flag_IsEqual)
                    {
                        list_Replace_SerchValue.LockAdd(new string[] { src_value[(int)enum_藥局_藥品資料.GUID].ObjectToString() });
                        list_Replace_Value.LockAdd(src_value);
                    }

                }
                else
                {
                    src_value[(int)enum_藥局_藥品資料.總庫存] = "0";
                    src_value[(int)enum_藥局_藥品資料.藥庫庫存] = "0";
                    src_value[(int)enum_藥局_藥品資料.藥局庫存] = "0";
                    src_value[(int)enum_藥局_藥品資料.安全庫存] = "0";
                    src_value[(int)enum_藥局_藥品資料.基準量] = "0";
                    list_Add.LockAdd(src_value);
                }
            });

            Parallel.ForEach(list_藥品資料, value =>
            {
                List<object[]> list_本地藥檔_buf = list_本地藥檔.GetRows((int)enum_本地_藥品資料.藥品碼, value[(int)enum_藥局_藥品資料.藥品碼].ObjectToString());
                if (list_本地藥檔_buf.Count == 0)
                {
                    list_Delete_SerchValue.LockAdd(value[(int)enum_藥局_藥品資料.GUID]);
                }
            });

            this.sqL_DataGridView_藥局_藥品資料.SQL_DeleteExtra(list_Delete_SerchValue, false);
            this.sqL_DataGridView_藥局_藥品資料.SQL_ReplaceExtra(list_Replace_Value, false);
            this.sqL_DataGridView_藥局_藥品資料.SQL_AddRows(list_Add, false);
            this.sqL_DataGridView_藥局_藥品資料.SQL_GetAllRows(true);
            Console.Write($"更新 藥局_屏東榮總-藥品資料 耗時 : {myTimer.GetTickTime()} ms\n");


        }
        private void Function_藥局_藥品資料_檢查DeviceBasic()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(100000);
            this.List_藥局_DeviceBasic = DeviceBasicClass_藥局.SQL_GetAllDeviceBasic();
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥局_藥品資料.SQL_GetAllRows(false);

            List<DeviceBasic> devices_Add = new List<DeviceBasic>();
            List<DeviceBasic> devices_Replace = new List<DeviceBasic>();

            Parallel.ForEach(list_藥品資料, value =>
            {
                string 藥品碼 = value[(int)enum_藥局_藥品資料.藥品碼].ObjectToString();
                List<DeviceBasic> devices_buf = (from Value in this.List_藥局_DeviceBasic
                                                 where Value.Code == 藥品碼
                                                 select Value).ToList();
                if (devices_buf.Count == 0)
                {
                    DeviceBasic device = new DeviceBasic();
                    device.Code = 藥品碼;
                    devices_Add.LockAdd(device);
                }
            });


            DeviceBasicClass_藥局.SQL_AddDeviceBasic(devices_Add);
            Console.Write($"儲位總量新增時間 ,耗時 :{myTimer.GetTickTime().ToString("0.000")}\n");
        }
    }
}
