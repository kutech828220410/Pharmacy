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
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {

        private Storage WT32_Storage_Copy;
        private enum enum_藥庫_儲位設定_Pannel35_效期及庫存
        {
            效期,
            批號,
            庫存,
        }
        private enum enum_藥庫_儲位設定_Pannel35_儲位資料
        {
            GUID,
            IP,
            儲位名稱,
            藥品碼,
            藥品名稱,
            藥品學名,
            藥品中文名稱,
            包裝單位,
            藥品條碼1,
            藥品條碼2,
            庫存,
            區域儲位,
        }
        private void sub_Program_藥庫_儲位設定_Pannel35_Init()
        {
            this.storageUI_WT32.Init(dBConfigClass.DB_Basic);
            this.pannel35_Pannel.Init(this.storageUI_WT32.List_UDP_Local);
            this.pannel35_Pannel.EditFinishedEvent += Pannel35_Pannel_EditFinishedEvent;
            this.Function_從SQL取得儲位到本地資料();

            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Init();

            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.Init();

            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_藥品資料.Init(this.sqL_DataGridView_藥庫_藥品資料);
            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_藥品資料.Set_ColumnVisible(false, new enum_藥庫_藥品資料().GetEnumNames());
            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_藥品資料.Set_ColumnVisible(true, enum_藥庫_藥品資料.藥品碼, enum_藥庫_藥品資料.藥品名稱, enum_藥庫_藥品資料.中文名稱, enum_藥庫_藥品資料.包裝單位);

            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.RowEnterEvent += SqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料_RowEnterEvent;

            this.plC_RJ_ComboBox_藥庫_儲位設定_Pannel35_區域儲位_搜尋.Enter += PlC_RJ_ComboBox_藥庫_儲位設定_Pannel35_區域儲位_搜尋_Enter;
            this.rJ_TextBox_藥庫_儲位設定_Pannel35_儲位名稱.KeyPress += RJ_TextBox_藥庫_儲位設定_Pannel35_儲位名稱_KeyPress;



            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_藥品搜尋_藥品名稱_搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品搜尋_藥品名稱_搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_藥品搜尋_藥品碼_搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品搜尋_藥品碼_搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_藥品搜尋_填入資料.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品搜尋_填入資料_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_新增效期.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_新增效期_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_修正庫存.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_修正庫存_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_修正批號.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_修正批號_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_輸入.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_輸入_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_複製格式.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_複製格式_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_貼上格式.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_貼上格式_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_區域儲位設定.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_區域儲位設定_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_藥品碼_搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品碼_搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_藥品名稱_搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品名稱_搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_區域儲位_搜尋.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_區域儲位_搜尋_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_顯示全部.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_更新畫面.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_更新畫面_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_面板亮燈.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_面板亮燈_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_清除燈號.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_清除燈號_MouseDownEvent;
            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_清除儲位.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_清除儲位_MouseDownEvent;


            this.plC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_測試初始化.MouseDownEvent += PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_測試初始化_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_儲位設定_Pannel35);
        }



        private bool flag_藥庫_儲位設定_Pannel35 = false;
        private void sub_Program_藥庫_儲位設定_Pannel35()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "儲位設定" && this.plC_ScreenPage_藥庫_儲位設定.PageText == "Pannel35")
            {
                if (!this.flag_藥庫_儲位設定_Pannel35)
                {
                    this.PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool = true;
                    this.flag_藥庫_儲位設定_Pannel35 = true;
                }
            }
            else
            {
                this.flag_藥庫_儲位設定_Pannel35 = false;
            }

            this.sub_Program_藥庫_儲位設定_Pannel35_資料更新();
        }

        #region PLC_藥庫_儲位設定_Pannel35_資料更新
        PLC_Device PLC_Device_藥庫_儲位設定_Pannel35_資料更新 = new PLC_Device("");
        int cnt_Program_藥庫_儲位設定_Pannel35_資料更新 = 65534;
        void sub_Program_藥庫_儲位設定_Pannel35_資料更新()
        {
            if (cnt_Program_藥庫_儲位設定_Pannel35_資料更新 == 65534)
            {
                PLC_Device_藥庫_儲位設定_Pannel35_資料更新.SetComment("PLC_藥庫_儲位設定_Pannel35_資料更新");
                PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool = false;
                cnt_Program_藥庫_儲位設定_Pannel35_資料更新 = 65535;
            }
            if (cnt_Program_藥庫_儲位設定_Pannel35_資料更新 == 65535) cnt_Program_藥庫_儲位設定_Pannel35_資料更新 = 1;
            if (cnt_Program_藥庫_儲位設定_Pannel35_資料更新 == 1) cnt_Program_藥庫_儲位設定_Pannel35_資料更新_檢查按下(ref cnt_Program_藥庫_儲位設定_Pannel35_資料更新);
            if (cnt_Program_藥庫_儲位設定_Pannel35_資料更新 == 2) cnt_Program_藥庫_儲位設定_Pannel35_資料更新_初始化(ref cnt_Program_藥庫_儲位設定_Pannel35_資料更新);
            if (cnt_Program_藥庫_儲位設定_Pannel35_資料更新 == 3) cnt_Program_藥庫_儲位設定_Pannel35_資料更新_更新藥檔(ref cnt_Program_藥庫_儲位設定_Pannel35_資料更新);
            if (cnt_Program_藥庫_儲位設定_Pannel35_資料更新 == 4) cnt_Program_藥庫_儲位設定_Pannel35_資料更新_更新面板資料(ref cnt_Program_藥庫_儲位設定_Pannel35_資料更新);
            if (cnt_Program_藥庫_儲位設定_Pannel35_資料更新 == 5) cnt_Program_藥庫_儲位設定_Pannel35_資料更新 = 65500;
            if (cnt_Program_藥庫_儲位設定_Pannel35_資料更新 > 1) cnt_Program_藥庫_儲位設定_Pannel35_資料更新_檢查放開(ref cnt_Program_藥庫_儲位設定_Pannel35_資料更新);

            if (cnt_Program_藥庫_儲位設定_Pannel35_資料更新 == 65500)
            {
                PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool = false;
                cnt_Program_藥庫_儲位設定_Pannel35_資料更新 = 65535;
            }
        }
        void cnt_Program_藥庫_儲位設定_Pannel35_資料更新_檢查按下(ref int cnt)
        {
            if (PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool) cnt++;
        }
        void cnt_Program_藥庫_儲位設定_Pannel35_資料更新_檢查放開(ref int cnt)
        {
            if (!PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool) cnt = 65500;
        }
        void cnt_Program_藥庫_儲位設定_Pannel35_資料更新_初始化(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);
            List_Pannel35_本地資料 = this.storageUI_WT32.SQL_GetAllStorage();
            Console.Write($"儲位管理Pannel35:從SQL取得資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        void cnt_Program_藥庫_儲位設定_Pannel35_資料更新_更新藥檔(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);

            List<object[]> list_藥品資料_藥檔資料 = this.sqL_DataGridView_藥庫_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_藥檔資料_buf = new List<object[]>();
            List<Storage> list_replaceValue = new List<Storage>();
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 藥品中文名稱 = "";
            string 藥品學名 = "";
            string BarCode = "";
            string 包裝單位 = "";
            string 最小包裝單位 = "";
            string 最小包裝數量 = "";


            string 藥品碼_buf = "";
            string 藥品名稱_buf = "";
            string 藥品中文名稱_buf = "";
            string 藥品學名_buf = "";
            string BarCode_buf = "";
            string 包裝單位_buf = "";
            string 最小包裝單位_buf = "";
            string 最小包裝數量_buf = "";
            for (int i = 0; i < this.List_Pannel35_本地資料.Count; i++)
            {
                if (this.List_Pannel35_本地資料 == null) continue;
                string IP = this.List_Pannel35_本地資料[i].IP;
                Storage storage = List_Pannel35_本地資料[i];
                bool Is_Replace = false;
                藥品碼 = storage.GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                if (藥品碼.StringIsEmpty()) continue;
                list_藥品資料_藥檔資料_buf = list_藥品資料_藥檔資料.GetRows((int)enum_藥庫_藥品資料.藥品碼, 藥品碼);
                if (list_藥品資料_藥檔資料_buf.Count == 0)
                {
                    storage.Clear();
                    Is_Replace = true;
                }
                else
                {
                    藥品碼_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥庫_藥品資料.藥品碼].ObjectToString();
                    藥品名稱_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥庫_藥品資料.藥品名稱].ObjectToString();
                    藥品中文名稱_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥庫_藥品資料.中文名稱].ObjectToString();
                    藥品學名_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥庫_藥品資料.藥品學名].ObjectToString();
                    BarCode_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥庫_藥品資料.藥品條碼1].ObjectToString();
                    包裝單位_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥庫_藥品資料.包裝單位].ObjectToString();
                    最小包裝單位_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥庫_藥品資料.最小包裝單位].ObjectToString();
                    最小包裝數量_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥庫_藥品資料.最小包裝數量].ObjectToString();

                    if(BarCode_buf.StringIsEmpty())
                    {
                        BarCode_buf = "None";
                    }

                    藥品碼 = storage.GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                    藥品名稱 = storage.GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                    藥品中文名稱 = storage.GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString();
                    藥品學名 = storage.GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString();
                    BarCode = storage.GetValue(Device.ValueName.BarCode, Device.ValueType.Value).ObjectToString();
                    包裝單位 = storage.GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
                    最小包裝單位 = storage.GetValue(Device.ValueName.最小包裝單位, Device.ValueType.Value).ObjectToString();
                    最小包裝數量 = storage.GetValue(Device.ValueName.最小包裝單位數量, Device.ValueType.Value).ObjectToString();


                    if (藥品碼 != 藥品碼_buf) Is_Replace = true;
                    if (藥品名稱 != 藥品名稱_buf) Is_Replace = true;
                    if (藥品學名 != 藥品學名_buf) Is_Replace = true;
                    if (BarCode != BarCode_buf) Is_Replace = true;
                    if (包裝單位 != 包裝單位_buf) Is_Replace = true;
                    if (藥品中文名稱_buf != 藥品中文名稱) Is_Replace = true;
                    if (最小包裝單位_buf != 最小包裝單位) Is_Replace = true;
                    if (最小包裝數量_buf != 最小包裝數量) Is_Replace = true;

                    storage.SetValue(Device.ValueName.藥品碼, Device.ValueType.Value, 藥品碼_buf);
                    storage.SetValue(Device.ValueName.藥品名稱, Device.ValueType.Value, 藥品名稱_buf);
                    storage.SetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value, 藥品中文名稱_buf);
                    storage.SetValue(Device.ValueName.藥品學名, Device.ValueType.Value, 藥品學名_buf);
                    storage.SetValue(Device.ValueName.BarCode, Device.ValueType.Value, BarCode_buf);
                    storage.SetValue(Device.ValueName.包裝單位, Device.ValueType.Value, 包裝單位_buf);
                    storage.SetValue(Device.ValueName.最小包裝單位, Device.ValueType.Value, 最小包裝單位_buf);
                    storage.SetValue(Device.ValueName.最小包裝單位數量, Device.ValueType.Value, 最小包裝數量_buf);


                }
                if (Is_Replace)
                {
                    list_replaceValue.Add(this.List_Pannel35_本地資料[i]);
                }
            }

            this.storageUI_WT32.SQL_ReplaceStorage(list_replaceValue);
            Console.Write($"儲位管理Pannel35:更新藥檔完成 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        void cnt_Program_藥庫_儲位設定_Pannel35_資料更新_更新面板資料(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);
            List<object[]> list_value = new List<object[]>();
            List<object[]> list_區域儲位 = sqL_DataGridView_貨架區域儲位列表.SQL_GetAllRows(false);
            List<object[]> list_區域儲位_buf = new List<object[]>();

            for (int i = 0; i < this.List_Pannel35_本地資料.Count; i++)
            {
                object[] value = new object[new enum_藥庫_儲位設定_Pannel35_儲位資料().GetLength()];
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.GUID] = List_Pannel35_本地資料[i].GUID;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.IP, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.儲位名稱] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品碼] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品名稱] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品學名] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品中文名稱] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.包裝單位] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品條碼1] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.BarCode, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.庫存] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();

                string Master_GUID = List_Pannel35_本地資料[i].Master_GUID;

                list_區域儲位_buf = list_區域儲位.GetRows((int)enum_藥庫_儲位設定_區域儲位.GUID, Master_GUID);
                if(list_區域儲位_buf.Count > 0)
                {
                    value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.區域儲位] = list_區域儲位_buf[0][(int)enum_藥庫_儲位設定_區域儲位.名稱].ObjectToString();
                }

                list_value.Add(value);
            }
            list_value.Sort(new ICP_藥庫_儲位設定_Pannel35_抽屜列表());
            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.RefreshGrid(list_value);
            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.On_RowEnter();
            Console.Write($"儲位管理Pannel35:更新儲位資料完成 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }

        #endregion

        #region Function
    
        #endregion
        #region Event
        private void SqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料_RowEnterEvent(object[] RowValue)
        {
            string IP = RowValue[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString();
            Storage storage = this.List_Pannel35_本地資料.SortByIP(IP);
            if (storage == null) return;
            this.pannel35_Pannel.Set_Stroage(storage);
            this.rJ_TextBox_藥庫_儲位設定_Pannel35_儲位名稱.Texts = (string)storage.GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value);
            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.ClearGrid();
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < storage.List_Validity_period.Count; i++)
            {
                object[] value = new object[new enum_藥庫_儲位設定_Pannel35_效期及庫存().GetLength()];
                value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.效期] = storage.List_Validity_period[i];
                value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.批號] = storage.List_Lot_number[i];
                value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.庫存] = storage.List_Inventory[i];
                list_value.Add(value);
            }

            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.RefreshGrid(list_value);

        }
        private void PlC_RJ_ComboBox_藥庫_儲位設定_Pannel35_區域儲位_搜尋_Enter(object sender, EventArgs e)
        {
            this.plC_RJ_ComboBox_藥庫_儲位設定_Pannel35_區域儲位_搜尋.SetDataSource(this.Function_藥庫_儲位設定_區域儲位_取得選單());
        }
        private void RJ_TextBox_藥庫_儲位設定_Pannel35_儲位名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_輸入_MouseDownEvent(null);
            }
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品搜尋_藥品碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if(rJ_TextBox_藥庫_儲位設定_Pannel35_藥品搜尋_藥品碼.Texts.StringIsEmpty())
            {
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_藥品資料.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥庫_藥品資料.藥品碼, rJ_TextBox_藥庫_儲位設定_Pannel35_藥品搜尋_藥品碼.Texts);

            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品搜尋_藥品名稱_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_藥庫_儲位設定_Pannel35_藥品搜尋_藥品名稱.Texts.StringIsEmpty())
            {
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_藥品資料.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥庫_藥品資料.藥品名稱, rJ_TextBox_藥庫_儲位設定_Pannel35_藥品搜尋_藥品名稱.Texts);

            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_藥品資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品搜尋_填入資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_藥品資料.Get_All_Select_RowsValues();
            List<object[]> list_儲位資料 = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if(list_藥品資料.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取藥品資料!");
                }));
                return;
            }
            if (list_儲位資料.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取儲位資料!");
                }));
                return;
            }
            string IP = list_儲位資料[0][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString();
            Storage storage = this.storageUI_WT32.SQL_GetStorage(IP);
            if (storage == null) return;
            storage.Code = list_藥品資料[0][(int)enum_藥庫_藥品資料.藥品碼].ObjectToString();
            this.storageUI_WT32.SQL_ReplaceStorage(storage);
            this.PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool = true;
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_新增效期_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                Storage storage = this.pannel35_Pannel.CurrentStorage;
                if (storage == null)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
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

                int 原有庫存 = storage.取得庫存();
                string 藥品碼 = storage.Code;
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 庫存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                storage.效期庫存覆蓋(效期, 批號, 數量);
                int 修正庫存 = storage.取得庫存();
                this.storageUI_WT32.SQL_ReplaceStorage(storage);

                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.新增效期.GetEnumName();
                string 藥品名稱 = storage.Name;
                string 交易量 = (修正庫存 - 原有庫存).ToString();
                string 結存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                string 操作人 = this.登入者名稱;  
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}],批號[{批號}]";

                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.藥庫;
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);

                List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.GetRows((int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP, storage.IP, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.庫存] = storage.取得庫存();
                this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Replace((int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP, storage.IP, list_value[0], true);

                this.sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < storage.List_Validity_period.Count; i++)
                {
                    object[] value = new object[new enum_藥庫_儲位設定_Pannel35_效期及庫存().GetLength()];
                    value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.效期] = storage.List_Validity_period[i];
                    value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.批號] = storage.List_Lot_number[i];
                    value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.庫存] = storage.List_Inventory[i];
                    list_value.Add(value);
                }
                this.sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
                this.storageUI_WT32.Set_ToPage(storage, StorageUI_WT32.enum_Page.主頁面, true);
            }));
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_修正批號_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                Storage storage = this.pannel35_Pannel.CurrentStorage;
                if (storage == null)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                object[] value = sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }
                string 效期 = value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.效期].ObjectToString();
                string 舊批號 = value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.批號].ObjectToString();
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


                storage.修正批號(效期, 新批號);
                this.List_Pannel35_本地資料.Add_NewStorage(storage);
                this.storageUI_WT32.SQL_ReplaceStorage(storage);


                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.修正批號.GetEnumName();
                string 藥品名稱 = storage.Name;
                string 交易量 = (0).ToString();
                string 結存量 = 0.ToString();
                string 操作人 = this.登入者名稱;
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}]新批號[{新批號}]";

                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = "";
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 0.ToString();
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);

                List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.GetRows((int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP, storage.IP, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.庫存] = storage.取得庫存();
                this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Replace((int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP, storage.IP, list_value[0], true);

                this.sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < storage.List_Validity_period.Count; i++)
                {
                    value = new object[new enum_藥庫_儲位設定_Pannel35_效期及庫存().GetLength()];
                    value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.效期] = storage.List_Validity_period[i];
                    value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.批號] = storage.List_Lot_number[i];
                    value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.庫存] = storage.List_Inventory[i];
                    list_value.Add(value);
                }
                this.sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
                this.storageUI_WT32.Set_ToPage(storage, StorageUI_WT32.enum_Page.主頁面, true);
            }));
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_修正庫存_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                Storage storage = this.pannel35_Pannel.CurrentStorage;
                if (storage == null)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                object[] value = sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }
                string 效期 = value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.效期].ObjectToString();
                string 批號 = value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.批號].ObjectToString();
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


                int 原有庫存 = storage.取得庫存();
                string 藥品碼 = storage.Code;
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 庫存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                storage.效期庫存覆蓋(效期, 數量);
                int 修正庫存 = storage.取得庫存();
                this.storageUI_WT32.SQL_ReplaceStorage(storage);
                this.List_Pannel35_本地資料.Add_NewStorage(storage);

                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.修正庫存.GetEnumName();
                string 藥品名稱 = storage.Name;
                string 交易量 = (修正庫存 - 原有庫存).ToString();
                string 結存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                string 操作人 = this.登入者名稱;
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}],批號[{批號}]";
                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);

                List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.GetRows((int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP, storage.IP, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.庫存] = storage.取得庫存();
                this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Replace((int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP, storage.IP, list_value[0], true);

                this.sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < storage.List_Validity_period.Count; i++)
                {
                    value = new object[new enum_藥庫_儲位設定_Pannel35_效期及庫存().GetLength()];
                    value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.效期] = storage.List_Validity_period[i];
                    value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.批號] = storage.List_Lot_number[i];
                    value[(int)enum_藥庫_儲位設定_Pannel35_效期及庫存.庫存] = storage.List_Inventory[i];
                    list_value.Add(value);
                }

                this.sqL_DataGridView_藥庫_儲位設定_Pannel35_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
                this.storageUI_WT32.Set_ToPage(storage, StorageUI_WT32.enum_Page.主頁面, true);
            }));
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_輸入_MouseDownEvent(MouseEventArgs mevent)
        {
            Storage storage = this.pannel35_Pannel.CurrentStorage;
            if (storage == null)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            storage.SetValue(Device.ValueName.儲位名稱, Device.ValueType.Value, this.rJ_TextBox_藥庫_儲位設定_Pannel35_儲位名稱.Texts);
            this.storageUI_WT32.SQL_ReplaceStorage(storage);
            this.pannel35_Pannel.Set_Stroage(storage);

            this.PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool = true;
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_貼上格式_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.WT32_Storage_Copy == null)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未複製儲位格式!");
                }));
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            List<Storage> storages = this.storageUI_WT32.SQL_GetAllStorage();
            List<Storage> storages_buf = new List<Storage>();
            List<Storage> storages_replaceValue = new List<Storage>();
            for (int i = 0; i < list_value.Count; i++)
            {
                string IP = list_value[i][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString();
                storages_buf = (from value in storages
                                where value.IP == IP
                                select value).ToList();
                if(storages_buf.Count > 0)
                {
                    storages_replaceValue.Add(storages_buf[0]);
                }
            }
            for (int i = 0; i < storages_replaceValue.Count; i++)
            {
                storages_replaceValue[i].PasteFormat(this.WT32_Storage_Copy);
                this.List_Pannel35_本地資料.Add_NewStorage(storages_replaceValue[i]);
            }

            this.storageUI_WT32.SQL_ReplaceStorage(storages_replaceValue);
        
            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.On_RowEnter();
            this.Invoke(new Action(delegate
            {
                MyMessageBox.ShowDialog("貼上完成!");
            }));
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_複製格式_MouseDownEvent(MouseEventArgs mevent)
        {
            Storage storage = this.pannel35_Pannel.CurrentStorage;
            if (storage == null)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            this.WT32_Storage_Copy = storage.DeepClone();
            this.Invoke(new Action(delegate
            {
                MyMessageBox.ShowDialog("複製完成!");
            }));
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_區域儲位設定_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_Replace_Value = new List<object[]>();
            List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Get_All_Select_RowsValues();
            List<object[]> list_區域儲位 = sqL_DataGridView_貨架區域儲位列表.SQL_GetAllRows(false);
            List<object[]> list_區域儲位_buf = new List<object[]>();
            if (list_value.Count ==0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位");
                }));
                return;
            }

            List<Storage> storages = this.storageUI_WT32.SQL_GetAllStorage();
            List<Storage> storages_buf = new List<Storage>();
            List<Storage> storages_replace = new List<Storage>();
            for (int i = 0; i < list_value.Count; i++)
            {
                storages_buf = (from temp in storages
                                where temp.IP == list_value[i][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString()
                                select temp).ToList();
                if(storages_buf.Count > 0)
                {
                    storages_replace.Add(storages_buf[0]);
                }
            }

            DialogResult dialogResult = DialogResult.None;
            string value = "";
            this.Invoke(new Action(delegate
            {
                Dialog_ContextMenuStrip dialog_ContextMenuStrip = new Dialog_ContextMenuStrip(Function_藥庫_儲位設定_區域儲位_取得選單());
                dialog_ContextMenuStrip.TitleText = "區域儲位設定";
                dialog_ContextMenuStrip.ControlsTextAlign = ContentAlignment.MiddleLeft;
                dialog_ContextMenuStrip.ControlsHeight = 40;
                dialogResult = dialog_ContextMenuStrip.ShowDialog();
                value = dialog_ContextMenuStrip.Value;
            }));

            if (dialogResult == DialogResult.Yes)
            {
                string[] strArray = myConvert.分解分隔號字串(value, ".");
                if (strArray.Length == 2)
                {
                    int 序號 = strArray[0].StringToInt32();
                    if (序號 <= 0) return;
                    string Master_GUID = "";
                    list_區域儲位_buf = list_區域儲位.GetRows((int)enum_藥庫_儲位設定_區域儲位.序號, 序號.ToString());
                    if (list_區域儲位_buf.Count > 0)
                    {
                        Master_GUID = list_區域儲位_buf[0][(int)enum_藥庫_儲位設定_區域儲位.GUID].ObjectToString();
                        for (int i = 0; i < storages_replace.Count; i++)
                        {
                            storages_replace[i].Master_GUID = Master_GUID;
                        }
                        this.storageUI_WT32.SQL_ReplaceStorage(storages_replace);
                    }
                }
                this.Function_設定雲端資料更新();
                PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool = true;
            }
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_區域儲位_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (plC_RJ_ComboBox_藥庫_儲位設定_Pannel35_區域儲位_搜尋.Texts.StringIsEmpty())
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("'區域儲位'搜尋欄位空白!");
                }));
                return;
            }
            List<object[]> list_value = new List<object[]>();
            List<object[]> list_區域儲位 = sqL_DataGridView_貨架區域儲位列表.SQL_GetAllRows(false);
            List<object[]> list_區域儲位_buf = new List<object[]>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            List<Storage> storages_buf = new List<Storage>();

            string[] strArray = myConvert.分解分隔號字串(plC_RJ_ComboBox_藥庫_儲位設定_Pannel35_區域儲位_搜尋.Texts, ".");
            if (strArray.Length != 2) return;
            int 序號 = strArray[0].StringToInt32();
            string Master_GUID = "";
            if (序號 <= 0) return;
            list_區域儲位_buf = list_區域儲位.GetRows((int)enum_藥庫_儲位設定_區域儲位.序號, 序號.ToString());
            if (list_區域儲位_buf.Count == 0) return;
            Master_GUID = list_區域儲位_buf[0][(int)enum_藥庫_儲位設定_區域儲位.GUID].ObjectToString();

            storages_buf = (from value in storages
                            where value.Master_GUID == Master_GUID
                            select value).ToList();
            for (int i = 0; i < storages_buf.Count; i++)
            {
                object[] value = new object[new enum_藥庫_儲位設定_Pannel35_儲位資料().GetLength()];
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.GUID] = storages_buf[i].GUID;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP] = storages_buf[i].GetValue(Device.ValueName.IP, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.儲位名稱] = storages_buf[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品碼] = storages_buf[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品名稱] = storages_buf[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品學名] = storages_buf[i].GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品中文名稱] = storages_buf[i].GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.包裝單位] = storages_buf[i].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品條碼1] = storages_buf[i].GetValue(Device.ValueName.BarCode, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.庫存] = storages_buf[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();

                Master_GUID = storages_buf[i].Master_GUID;

                list_區域儲位_buf = list_區域儲位.GetRows((int)enum_藥庫_儲位設定_區域儲位.GUID, Master_GUID);
                if (list_區域儲位_buf.Count > 0)
                {
                    value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.區域儲位] = list_區域儲位_buf[0][(int)enum_藥庫_儲位設定_區域儲位.名稱].ObjectToString();
                }

                list_value.Add(value);
            }
            list_value.Sort(new ICP_藥庫_儲位設定_Pannel35_抽屜列表());
            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.RefreshGrid(list_value);

        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品名稱_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_藥庫_儲位設定_Pannel35_藥品名稱_搜尋.Texts.StringIsEmpty())
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("'藥品名稱'搜尋欄位空白!");
                }));
                return;
            }
            List<object[]> list_value = new List<object[]>();
            List<object[]> list_區域儲位 = sqL_DataGridView_貨架區域儲位列表.SQL_GetAllRows(false);
            List<object[]> list_區域儲位_buf = new List<object[]>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            List<Storage> storages_buf = new List<Storage>();
            storages_buf = (from value in storages
                            where value.Code.ToUpper().Contains(rJ_TextBox_藥庫_儲位設定_Pannel35_藥品名稱_搜尋.Texts.ToUpper())
                            select value).ToList();
            for (int i = 0; i < storages_buf.Count; i++)
            {
                object[] value = new object[new enum_藥庫_儲位設定_Pannel35_儲位資料().GetLength()];
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.GUID] = storages_buf[i].GUID;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP] = storages_buf[i].GetValue(Device.ValueName.IP, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.儲位名稱] = storages_buf[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品碼] = storages_buf[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品名稱] = storages_buf[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品學名] = storages_buf[i].GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品中文名稱] = storages_buf[i].GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.包裝單位] = storages_buf[i].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品條碼1] = storages_buf[i].GetValue(Device.ValueName.BarCode, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.庫存] = storages_buf[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();

                string Master_GUID = storages_buf[i].Master_GUID;

                list_區域儲位_buf = list_區域儲位.GetRows((int)enum_藥庫_儲位設定_區域儲位.GUID, Master_GUID);
                if (list_區域儲位_buf.Count > 0)
                {
                    value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.區域儲位] = list_區域儲位_buf[0][(int)enum_藥庫_儲位設定_區域儲位.名稱].ObjectToString();
                }

                list_value.Add(value);
            }
            list_value.Sort(new ICP_藥庫_儲位設定_Pannel35_抽屜列表());
            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_藥品碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if(rJ_TextBox_藥庫_儲位設定_Pannel35_藥品碼_搜尋.Texts.StringIsEmpty())
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("'藥品碼'搜尋欄位空白!");
                }));
                return;
            }
            List<object[]> list_value = new List<object[]>();
            List<object[]> list_區域儲位 = sqL_DataGridView_貨架區域儲位列表.SQL_GetAllRows(false);
            List<object[]> list_區域儲位_buf = new List<object[]>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            List<Storage> storages_buf = new List<Storage>();
            storages_buf = (from value in storages
                            where value.Code.ToUpper().Contains(rJ_TextBox_藥庫_儲位設定_Pannel35_藥品碼_搜尋.Texts.ToUpper())
                            select value).ToList();
            for (int i = 0; i < storages_buf.Count; i++)
            {
                object[] value = new object[new enum_藥庫_儲位設定_Pannel35_儲位資料().GetLength()];
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.GUID] = storages_buf[i].GUID;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP] = storages_buf[i].GetValue(Device.ValueName.IP, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.儲位名稱] = storages_buf[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品碼] = storages_buf[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品名稱] = storages_buf[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品學名] = storages_buf[i].GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品中文名稱] = storages_buf[i].GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.包裝單位] = storages_buf[i].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.藥品條碼1] = storages_buf[i].GetValue(Device.ValueName.BarCode, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.庫存] = storages_buf[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();

                string Master_GUID = storages_buf[i].Master_GUID;

                list_區域儲位_buf = list_區域儲位.GetRows((int)enum_藥庫_儲位設定_區域儲位.GUID, Master_GUID);
                if (list_區域儲位_buf.Count > 0)
                {
                    value[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.區域儲位] = list_區域儲位_buf[0][(int)enum_藥庫_儲位設定_區域儲位.名稱].ObjectToString();
                }

                list_value.Add(value);
            }
            list_value.Sort(new ICP_藥庫_儲位設定_Pannel35_抽屜列表());
            this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.RefreshGrid(list_value);

        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool = true;
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_更新畫面_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            List<Task> taskList = new List<Task>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            for (int i = 0; i < list_value.Count; i++)
            {
                List<Storage> storages_buf = (from value in storages
                                              where value.IP == list_value[i][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString()
                                              select value).ToList();
                if (storages_buf.Count > 0)
                {
                    H_Pannel_lib.Communication.UDP_TimeOut = 50000;
                    taskList.Add(Task.Run(() =>
                    {
                        this.storageUI_WT32.Set_DrawPannelJEPG(storages_buf[0]);
                    }));
                }

            }

            Task.WaitAll(taskList.ToArray());


        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_面板亮燈_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            List<Task> taskList = new List<Task>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            try
            {
                for (int i = 0; i < list_value.Count; i++)
                {
                    List<Storage> storages_buf = (from value in storages
                                                  where value.IP == list_value[i][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString()
                                                  select value).ToList();
                    if (storages_buf.Count > 0)
                    {
                        taskList.Add(Task.Run(() =>
                        {
                            Color color = Color.Black;
                            if (radioButton_藥庫_儲位設定_Pannel35_儲位名稱_面板亮燈_紅.Checked) color = Color.Red;
                            if (radioButton_藥庫_儲位設定_Pannel35_儲位名稱_面板亮燈_綠.Checked) color = Color.Lime;
                            if (radioButton_藥庫_儲位設定_Pannel35_儲位名稱_面板亮燈_藍.Checked) color = Color.Blue;
                            if (radioButton_藥庫_儲位設定_Pannel35_儲位名稱_面板亮燈_白.Checked) color = Color.White;

                            this.storageUI_WT32.Set_WS2812_Blink(storages_buf[0], 500, color);
                        }));
                    }

                }

                Task.WaitAll(taskList.ToArray());
            }
            catch
            {

            }
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_清除燈號_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            try
            {
                List<Task> taskList = new List<Task>();
                List<Storage> storages = this.List_Pannel35_本地資料;
                for (int i = 0; i < list_value.Count; i++)
                {
                    List<Storage> storages_buf = (from value in storages
                                                  where value.IP == list_value[i][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString()
                                                  select value).ToList();
                    if (storages_buf.Count > 0)
                    {
                        taskList.Add(Task.Run(() =>
                        {
                            Color color = Color.Black;
                            this.storageUI_WT32.Set_WS2812_Blink(storages_buf[0], 0, color);
                        }));
                    }

                }

                Task.WaitAll(taskList.ToArray());
            }
            catch
            {

            }
          
        }
        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_清除儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取儲位!");
                return;
            }
            if (MyMessageBox.ShowDialog("是否清除所選取儲位?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
            {
                string IP = "";
                for (int i = 0; i < list_value.Count; i++)
                {
                    IP = list_value[i][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString();
                    Storage storage = this.List_Pannel35_本地資料.SortByIP(IP);
                    if (storage == null) return;
                    storage.ClearStorage();
                    this.storageUI_WT32.SQL_ReplaceStorage(storage);
                }
            }
            this.PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool = true;
            while (true)
            {
                if (this.PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool == false) break;
                System.Threading.Thread.Sleep(10);
            }
        }

        private void PlC_RJ_Button_藥庫_儲位設定_Pannel35_儲位名稱_測試初始化_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_藥品資料.SQL_GetAllRows(false);
            list_藥品資料 = (from value in list_藥品資料
                         where value[(int)enum_藥庫_藥品資料.基準量].ObjectToString().StringToInt32() > 0
                         where value[(int)enum_藥庫_藥品資料.安全庫存].ObjectToString().StringToInt32() > 0
                         select value).ToList();
            List<object[]> list_儲位資料 = this.sqL_DataGridView_藥庫_儲位設定_Pannel35_儲位資料.GetAllRows();
            list_儲位資料.Sort(new ICP_藥庫_儲位設定_Pannel35_抽屜列表());
            List<Storage> storages_Add = new List<Storage>();

            Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_儲位資料.Count);
            string IP = "";
            dialog_Prcessbar.State = "更新儲位資料...";
            for (int i = 0; i < list_儲位資料.Count; i++)
            {
                dialog_Prcessbar.Value = i;
                IP = list_儲位資料[i][(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString();
                Storage storage = this.List_Pannel35_本地資料.SortByIP(IP);
                if (storage == null) continue;
                if (i >= list_藥品資料.Count) continue;
                storage.清除所有庫存資料();
                storage.Code = list_藥品資料[i][(int)enum_藥庫_藥品資料.藥品碼].ObjectToString();
                storage.效期庫存覆蓋("1990/01/01", "10");
                storage.效期庫存覆蓋(DateTime.Now.ToDateString(), "100000");
                storages_Add.Add(storage);
            }
            dialog_Prcessbar.State = "上傳儲位資料...";
            this.storageUI_WT32.SQL_ReplaceStorage(storages_Add);
            this.PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool = true;
            dialog_Prcessbar.State = "更新儲位資料...";
            while (true)
            {
                if (this.PLC_Device_藥庫_儲位設定_Pannel35_資料更新.Bool == false) break;
                System.Threading.Thread.Sleep(10);
            }
          
            dialog_Prcessbar.Close();
        }

        private void Pannel35_Pannel_EditFinishedEvent(H_Pannel_lib.Storage storage)
        {
            this.storageUI_WT32.SQL_ReplaceStorage(storage);
            this.List_Pannel35_本地資料.Add_NewStorage(storage);
        }
        #endregion

        private class ICP_藥庫_儲位設定_Pannel35_抽屜列表 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string IP_0 = x[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString();
                string IP_1 = y[(int)enum_藥庫_儲位設定_Pannel35_儲位資料.IP].ObjectToString();
                string[] IP_0_Array = IP_0.Split('.');
                string[] IP_1_Array = IP_1.Split('.');
                IP_0 = "";
                IP_1 = "";
                for (int i = 0; i < 4; i++)
                {
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];

                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];

                    IP_0 += IP_0_Array[i];
                    IP_1 += IP_1_Array[i];
                }
                int cmp = IP_0_Array[2].CompareTo(IP_1_Array[2]);
                if (cmp > 0)
                {
                    return 1;
                }
                else if (cmp < 0)
                {
                    return -1;
                }
                else if (cmp == 0)
                {
                    cmp = IP_0_Array[3].CompareTo(IP_1_Array[3]);
                    if (cmp > 0)
                    {
                        return 1;
                    }
                    else if (cmp < 0)
                    {
                        return -1;
                    }
                    else if (cmp == 0)
                    {
                        return 0;
                    }
                }

                return 0;

            }
        }
    }
}
