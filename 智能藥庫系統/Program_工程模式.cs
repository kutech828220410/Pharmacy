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
    enum enum_入庫原因維護
    {
        GUID,
        內容,
        登錄日期,
    }
    public partial class Main_Form : Form
    {
        MyThread MyThread_檢查Pannel35_主畫面;
        MyThread MyThread_檢查Pannel35_數字鍵盤頁面;
        MyThread MyThread_檢查Pannel35_區域亮燈;
        private void sub_Program_工程模式_Init()
        {
            this.sqL_DataGridView_入庫原因維護.Init();
            if (!this.sqL_DataGridView_入庫原因維護.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_入庫原因維護.SQL_CreateTable();
            }
            this.sqL_DataGridView_入庫原因維護.RowEnterEvent += SqL_DataGridView_入庫原因維護_RowEnterEvent;

            this.plC_RJ_Button_入庫原因維護_新增.MouseDownEvent += PlC_RJ_Button_入庫原因維護_新增_MouseDownEvent;
            this.plC_RJ_Button_入庫原因維護_刪除.MouseDownEvent += PlC_RJ_Button_入庫原因維護_刪除_MouseDownEvent;

            this.MyThread_檢查Pannel35_主畫面 = new MyThread();
            this.MyThread_檢查Pannel35_主畫面.AutoStop(true);
            this.MyThread_檢查Pannel35_主畫面.AutoRun(true);
            this.MyThread_檢查Pannel35_主畫面.SetSleepTime(100);
            this.MyThread_檢查Pannel35_主畫面.Add_Method(sub_Program_檢查Pannel35_主畫面);
            this.MyThread_檢查Pannel35_主畫面.Trigger();

            this.MyThread_檢查Pannel35_數字鍵盤頁面 = new MyThread();
            this.MyThread_檢查Pannel35_數字鍵盤頁面.AutoStop(true);
            this.MyThread_檢查Pannel35_數字鍵盤頁面.AutoRun(true);
            this.MyThread_檢查Pannel35_數字鍵盤頁面.SetSleepTime(100);
            this.MyThread_檢查Pannel35_數字鍵盤頁面.Add_Method(sub_Program_檢查Pannel35_數字鍵盤頁面);
            this.MyThread_檢查Pannel35_數字鍵盤頁面.Trigger();

            this.MyThread_檢查Pannel35_區域亮燈 = new MyThread();
            this.MyThread_檢查Pannel35_區域亮燈.AutoStop(true);
            this.MyThread_檢查Pannel35_區域亮燈.AutoRun(true);
            this.MyThread_檢查Pannel35_區域亮燈.SetSleepTime(500);
            this.MyThread_檢查Pannel35_區域亮燈.Add_Method(sub_Program_檢查Pannel35_區域亮燈);
            this.MyThread_檢查Pannel35_區域亮燈.Trigger();

            this.plC_UI_Init.Add_Method(sub_Program_工程模式);
        }

 

        private void sub_Program_工程模式()
        {
           
        }

        #region PLC_檢查Pannel35_主畫面
        PLC_Device PLC_Device_檢查Pannel35_主畫面 = new PLC_Device("");
        PLC_Device PLC_Device_檢查Pannel35_主畫面_OK = new PLC_Device("");
        int cnt_Program_檢查Pannel35_主畫面 = 65534;
        void sub_Program_檢查Pannel35_主畫面()
        {
            if (PLC_Device_主機模式.Bool) PLC_Device_檢查Pannel35_主畫面.Bool = true;
            if (cnt_Program_檢查Pannel35_主畫面 == 65534)
            {
                PLC_Device_檢查Pannel35_主畫面.SetComment("PLC_檢查Pannel35_主畫面");
                PLC_Device_檢查Pannel35_主畫面_OK.SetComment("PLC_檢查Pannel35_主畫面_OK");
                PLC_Device_檢查Pannel35_主畫面.Bool = false;
                cnt_Program_檢查Pannel35_主畫面 = 65535;
            }
            if (cnt_Program_檢查Pannel35_主畫面 == 65535) cnt_Program_檢查Pannel35_主畫面 = 1;
            if (cnt_Program_檢查Pannel35_主畫面 == 1) cnt_Program_檢查Pannel35_主畫面_檢查按下(ref cnt_Program_檢查Pannel35_主畫面);
            if (cnt_Program_檢查Pannel35_主畫面 == 2) cnt_Program_檢查Pannel35_主畫面_初始化(ref cnt_Program_檢查Pannel35_主畫面);
            if (cnt_Program_檢查Pannel35_主畫面 == 3) cnt_Program_檢查Pannel35_主畫面 = 65500;
            if (cnt_Program_檢查Pannel35_主畫面 > 1) cnt_Program_檢查Pannel35_主畫面_檢查放開(ref cnt_Program_檢查Pannel35_主畫面);

            if (cnt_Program_檢查Pannel35_主畫面 == 65500)
            {
                PLC_Device_檢查Pannel35_主畫面.Bool = false;
                PLC_Device_檢查Pannel35_主畫面_OK.Bool = false;
                cnt_Program_檢查Pannel35_主畫面 = 65535;
            }
        }
        void cnt_Program_檢查Pannel35_主畫面_檢查按下(ref int cnt)
        {
            if (PLC_Device_檢查Pannel35_主畫面.Bool) cnt++;
        }
        void cnt_Program_檢查Pannel35_主畫面_檢查放開(ref int cnt)
        {
            if (!PLC_Device_檢查Pannel35_主畫面.Bool) cnt = 65500;
        }
        void cnt_Program_檢查Pannel35_主畫面_初始化(ref int cnt)
        {
            
            try
            {
                List<StorageUI_WT32.UDP_READ> uDP_READs = this.storageUI_WT32.GerAllUDP_READ();
                List<string> list_refresh_ip = new List<string>();
                for (int i = 0; i < uDP_READs.Count; i++)
                {
                    if (uDP_READs[i].Screen_Page == (int)StorageUI_WT32.enum_Page.主頁面 && uDP_READs[i].ScreenPage_Init == true)
                    {
                        list_refresh_ip.Add(uDP_READs[i].IP);
                    }
                }
                List<Task> taskList = new List<Task>();
                List<Storage> storages = this.List_Pannel35_本地資料;
                for (int i = 0; i < list_refresh_ip.Count; i++)
                {
                    List<Storage> storages_buf = (from value in storages
                                                  where value.IP == list_refresh_ip[i]
                                                  select value).ToList();
                    if (storages_buf.Count > 0)
                    {
                        taskList.Add(Task.Run(() =>
                        {
                            storages_buf[0].Code_Title = "藥碼";
                            storages_buf[0].Name_Title = "藥名(學名)";
                            storages_buf[0].ChineseName_Title = "中文名";
                            storages_buf[0].Scientific_Name_Title = "商品名";
                            storages_buf[0].StorageName_Title = "None";
                            this.storageUI_WT32.Set_DrawPannelJEPG(storages_buf[0]);
                        }));
                    }

                }

                Task allTask = Task.WhenAll(taskList);
                allTask.Wait();
            }
            catch
            {

            }

            cnt++;
        }


        #endregion
        #region PLC_檢查Pannel35_數字鍵盤頁面
        PLC_Device PLC_Device_檢查Pannel35_數字鍵盤頁面 = new PLC_Device("");
        PLC_Device PLC_Device_檢查Pannel35_數字鍵盤頁面_OK = new PLC_Device("");
        int cnt_Program_檢查Pannel35_數字鍵盤頁面 = 65534;
        void sub_Program_檢查Pannel35_數字鍵盤頁面()
        {
            if (PLC_Device_主機模式.Bool) PLC_Device_檢查Pannel35_數字鍵盤頁面.Bool = true;
            if (cnt_Program_檢查Pannel35_數字鍵盤頁面 == 65534)
            {
                PLC_Device_檢查Pannel35_數字鍵盤頁面.SetComment("PLC_檢查Pannel35_數字鍵盤頁面");
                PLC_Device_檢查Pannel35_數字鍵盤頁面_OK.SetComment("PLC_檢查Pannel35_數字鍵盤頁面_OK");
                PLC_Device_檢查Pannel35_數字鍵盤頁面.Bool = false;
                cnt_Program_檢查Pannel35_數字鍵盤頁面 = 65535;
            }
            if (cnt_Program_檢查Pannel35_數字鍵盤頁面 == 65535) cnt_Program_檢查Pannel35_數字鍵盤頁面 = 1;
            if (cnt_Program_檢查Pannel35_數字鍵盤頁面 == 1) cnt_Program_檢查Pannel35_數字鍵盤頁面_檢查按下(ref cnt_Program_檢查Pannel35_數字鍵盤頁面);
            if (cnt_Program_檢查Pannel35_數字鍵盤頁面 == 2) cnt_Program_檢查Pannel35_數字鍵盤頁面_初始化(ref cnt_Program_檢查Pannel35_數字鍵盤頁面);
            if (cnt_Program_檢查Pannel35_數字鍵盤頁面 == 3) cnt_Program_檢查Pannel35_數字鍵盤頁面 = 65500;
            if (cnt_Program_檢查Pannel35_數字鍵盤頁面 > 1) cnt_Program_檢查Pannel35_數字鍵盤頁面_檢查放開(ref cnt_Program_檢查Pannel35_數字鍵盤頁面);

            if (cnt_Program_檢查Pannel35_數字鍵盤頁面 == 65500)
            {
                PLC_Device_檢查Pannel35_數字鍵盤頁面.Bool = false;
                PLC_Device_檢查Pannel35_數字鍵盤頁面_OK.Bool = false;
                cnt_Program_檢查Pannel35_數字鍵盤頁面 = 65535;
            }
        }
        void cnt_Program_檢查Pannel35_數字鍵盤頁面_檢查按下(ref int cnt)
        {
            if (PLC_Device_檢查Pannel35_數字鍵盤頁面.Bool) cnt++;
        }
        void cnt_Program_檢查Pannel35_數字鍵盤頁面_檢查放開(ref int cnt)
        {
            if (!PLC_Device_檢查Pannel35_數字鍵盤頁面.Bool) cnt = 65500;
        }
        void cnt_Program_檢查Pannel35_數字鍵盤頁面_初始化(ref int cnt)
        {
            List<StorageUI_WT32.UDP_READ> uDP_READs = this.storageUI_WT32.GerAllUDP_READ();
            List<string> list_refresh_ip = new List<string>();
            for (int i = 0; i < uDP_READs.Count; i++)
            {
                if (uDP_READs[i].Screen_Page == (int)StorageUI_WT32.enum_Page.數字鍵盤頁面 && uDP_READs[i].ScreenPage_Init == true)
                {
                    list_refresh_ip.Add(uDP_READs[i].IP);
                }
            }
            List<Task> taskList = new List<Task>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            try
            {
                for (int i = 0; i < list_refresh_ip.Count; i++)
                {
                    List<Storage> storages_buf = (from value in storages
                                                  where value.IP == list_refresh_ip[i]
                                                  select value).ToList();
                    if (storages_buf.Count > 0)
                    {
                        taskList.Add(Task.Run(() =>
                        {
                            Font code_font = new Font("微軟正黑體", 14, FontStyle.Bold);
                            string code_text = storages_buf[0].GetValue(Device.ValueName.藥品碼, Device.ValueType.StringValue).ObjectToString();
                            Size code_size = DrawingClass.Draw.MeasureText(code_text, code_font);

                            Font name_font = new Font("微軟正黑體", 14, FontStyle.Bold);
                            string name_text = storages_buf[0].GetValue(Device.ValueName.藥品名稱, Device.ValueType.StringValue).ObjectToString();
                            Size name_size = DrawingClass.Draw.MeasureText(name_text, name_font);

                            Font inventory_font = new Font("微軟正黑體", 20, FontStyle.Bold);
                            string inventory_text = storages_buf[0].GetValue(Device.ValueName.庫存, Device.ValueType.StringValue).ObjectToString();
                            Size inventory_size = DrawingClass.Draw.MeasureText(inventory_text, inventory_font);


                            this.storageUI_WT32.Set_TextEx(storages_buf[0], code_text, 5, 5, code_font, Color.Black, Color.White, 1, Color.Black);
                            this.storageUI_WT32.Set_TextEx(storages_buf[0], name_text, 5, 5 + name_size.Height, name_font, Color.Black, Color.White, 1, Color.Black);

                            this.storageUI_WT32.Set_TextEx(storages_buf[0], inventory_text, this.storageUI_WT32.Panel_Width - inventory_size.Width - 10, 5, inventory_font, Color.Red, Color.Yellow, 1, Color.Red);

                            this.storageUI_WT32.Set_ScreenPageInit(storages_buf[0], false);
                        }));
                    }

                }

                Task allTask = Task.WhenAll(taskList);
                allTask.Wait();
            }
            catch
            {

            }
           
            cnt++;
        }


        #endregion
        #region PLC_檢查Pannel35_區域亮燈
        PLC_Device PLC_Device_檢查Pannel35_區域亮燈 = new PLC_Device("");
        PLC_Device PLC_Device_檢查Pannel35_區域亮燈_OK = new PLC_Device("");
        int cnt_Program_檢查Pannel35_區域亮燈 = 65534;
        void sub_Program_檢查Pannel35_區域亮燈()
        {
            if (PLC_Device_主機模式.Bool) PLC_Device_檢查Pannel35_區域亮燈.Bool = true;
            if (cnt_Program_檢查Pannel35_區域亮燈 == 65534)
            {
                PLC_Device_檢查Pannel35_區域亮燈.SetComment("PLC_檢查Pannel35_區域亮燈");
                PLC_Device_檢查Pannel35_區域亮燈_OK.SetComment("PLC_檢查Pannel35_區域亮燈_OK");
                PLC_Device_檢查Pannel35_區域亮燈.Bool = false;
                cnt_Program_檢查Pannel35_區域亮燈 = 65535;
            }
            if (cnt_Program_檢查Pannel35_區域亮燈 == 65535) cnt_Program_檢查Pannel35_區域亮燈 = 1;
            if (cnt_Program_檢查Pannel35_區域亮燈 == 1) cnt_Program_檢查Pannel35_區域亮燈_檢查按下(ref cnt_Program_檢查Pannel35_區域亮燈);
            if (cnt_Program_檢查Pannel35_區域亮燈 == 2) cnt_Program_檢查Pannel35_區域亮燈_初始化(ref cnt_Program_檢查Pannel35_區域亮燈);
            if (cnt_Program_檢查Pannel35_區域亮燈 == 3) cnt_Program_檢查Pannel35_區域亮燈 = 65500;
            if (cnt_Program_檢查Pannel35_區域亮燈 > 1) cnt_Program_檢查Pannel35_區域亮燈_檢查放開(ref cnt_Program_檢查Pannel35_區域亮燈);

            if (cnt_Program_檢查Pannel35_區域亮燈 == 65500)
            {
                PLC_Device_檢查Pannel35_區域亮燈.Bool = false;
                PLC_Device_檢查Pannel35_區域亮燈_OK.Bool = false;
                cnt_Program_檢查Pannel35_區域亮燈 = 65535;
            }
        }
        void cnt_Program_檢查Pannel35_區域亮燈_檢查按下(ref int cnt)
        {
            if (PLC_Device_檢查Pannel35_區域亮燈.Bool) cnt++;
        }
        void cnt_Program_檢查Pannel35_區域亮燈_檢查放開(ref int cnt)
        {
            if (!PLC_Device_檢查Pannel35_區域亮燈.Bool) cnt = 65500;
        }
        void cnt_Program_檢查Pannel35_區域亮燈_初始化(ref int cnt)
        {
            List<StorageUI_WT32.UDP_READ> uDP_READs = this.storageUI_WT32.GerAllUDP_READ();
            List<object[]> list_貨架區域儲位列表 = this.sqL_DataGridView_貨架區域儲位列表.SQL_GetAllRows(false);
            List<object[]> list_貨架區域儲位列表_buf = new List<object[]>();
            List<string> list_master_guid_on = new List<string>();
            List<string> list_master_guid_on_buf = new List<string>();
            List<string> list_master_guid_off = new List<string>();
            List<string> list_master_guid_off_buf = new List<string>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            for (int i = 0; i < uDP_READs.Count; i++)
            {
                if (uDP_READs[i].WS2812_State)
                {
                    Storage storage = storages.SortByIP(uDP_READs[i].IP);
                    if (storage == null) continue;
                    string master_guid = storage.Master_GUID;
                    list_master_guid_on_buf = (from value in list_master_guid_on
                                            where value == master_guid
                                            select value).ToList();
                    if(list_master_guid_on_buf.Count == 0)
                    {
                        list_master_guid_on.LockAdd(master_guid);
                    }
                }
            }
            for (int i = 0; i < list_貨架區域儲位列表.Count; i++)
            {
                string guid = list_貨架區域儲位列表[i][(int)enum_藥庫_儲位設定_區域儲位.GUID].ObjectToString();
                list_master_guid_on_buf = (from value in list_master_guid_on
                                           where value == guid
                                           select value).ToList();
                if(list_master_guid_on_buf.Count == 0)
                {
                    list_master_guid_off.LockAdd(guid);
                }

            }
   
            try
            {
                List<Task> taskList_on = new List<Task>();
                for (int i = 0; i < list_master_guid_on.Count; i++)
                {
                    list_貨架區域儲位列表_buf = list_貨架區域儲位列表.GetRows((int)enum_藥庫_儲位設定_區域儲位.GUID, list_master_guid_on[i]);
                    if (list_貨架區域儲位列表_buf.Count > 0)
                    {
                        string IP = list_貨架區域儲位列表_buf[0][(int)enum_藥庫_儲位設定_區域儲位.IP].ObjectToString();
                        int Port = list_貨架區域儲位列表_buf[0][(int)enum_藥庫_儲位設定_區域儲位.Port].ObjectToString().StringToInt32();
                        int Num = list_貨架區域儲位列表_buf[0][(int)enum_藥庫_儲位設定_區域儲位.Num].ObjectToString().StringToInt32();
                        taskList_on.Add(Task.Run(() =>
                        {
                            if(!this.rfiD_UI.Get_IO_Output(IP, Port, Num))
                            {
                                this.rfiD_UI.Set_OutputPIN(IP, Port, Num, true);
                            }

                        }));
                    }

                }           
                Task.WhenAll(taskList_on).Wait();
            }
            catch
            {

            }
            System.Threading.Thread.Sleep(100);
            try
            {
                List<Task> taskList_off = new List<Task>();
                for (int i = 0; i < list_master_guid_off.Count; i++)
                {
                    list_貨架區域儲位列表_buf = list_貨架區域儲位列表.GetRows((int)enum_藥庫_儲位設定_區域儲位.GUID, list_master_guid_off[i]);
                    if (list_貨架區域儲位列表_buf.Count > 0)
                    {
                        string IP = list_貨架區域儲位列表_buf[0][(int)enum_藥庫_儲位設定_區域儲位.IP].ObjectToString();
                        int Port = list_貨架區域儲位列表_buf[0][(int)enum_藥庫_儲位設定_區域儲位.Port].ObjectToString().StringToInt32();
                        int Num = list_貨架區域儲位列表_buf[0][(int)enum_藥庫_儲位設定_區域儲位.Num].ObjectToString().StringToInt32();
                        if (IP.StringIsEmpty()) continue;
                        taskList_off.Add(Task.Run(() =>
                        {
                            if (this.rfiD_UI.Get_IO_Output(IP, Port, Num))
                            {
                                this.rfiD_UI.Set_OutputPIN(IP, Port, Num, false);
                            }

                        }));
                    }

                }
                Task.WhenAll(taskList_off).Wait();

            }
            catch
            {

            }

           
            cnt++;
        }


        #endregion


        #region Event
        private void SqL_DataGridView_入庫原因維護_RowEnterEvent(object[] RowValue)
        {
            string 內容 = RowValue[(int)enum_入庫原因維護.內容].ObjectToString();
            this.rJ_TextBox_入庫原因維護_內容.Texts = 內容;
        }
        private void PlC_RJ_Button_入庫原因維護_新增_MouseDownEvent(MouseEventArgs mevent)
        {
            string 內容 = rJ_TextBox_入庫原因維護_內容.Text;
            List<object[]> list_value = this.sqL_DataGridView_入庫原因維護.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_入庫原因維護.內容, 內容);
            if(list_value.Count > 0)
            {
                MyMessageBox.ShowDialog("已有相同內容資料!");
                return;
            }
            object[] value = new object[new enum_入庫原因維護().GetLength()];
            value[(int)enum_入庫原因維護.GUID] = Guid.NewGuid().ToString();
            value[(int)enum_入庫原因維護.內容] = 內容;
            value[(int)enum_入庫原因維護.登錄日期] = DateTime.Now.ToDateTimeString_6();

            this.sqL_DataGridView_入庫原因維護.SQL_AddRow(value, true);

        }
        private void PlC_RJ_Button_入庫原因維護_刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_入庫原因維護.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            this.sqL_DataGridView_入庫原因維護.SQL_DeleteExtra(list_value, true);
        }

        #endregion
    }
}
