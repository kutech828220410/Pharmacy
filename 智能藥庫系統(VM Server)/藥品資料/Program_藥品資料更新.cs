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
        private void sub_Program_藥品資料更新_Init()
        {


            this.plC_UI_Init.Add_Method(sub_Program_藥品資料更新);
        }

        private bool flag_藥品資料更新 = false;
        private void sub_Program_藥品資料更新()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥品資料")
            {
                if (!this.flag_藥品資料更新)
                {
                    this.flag_藥品資料更新 = true;
                }

            }
            else
            {
                this.flag_藥品資料更新 = false;
            }

            this.sub_Program_藥品資料更新一次();
        }

        #region PLC_藥品資料更新一次
        PLC_Device PLC_Device_藥品資料更新一次 = new PLC_Device("");
        PLC_Device PLC_Device_藥品資料更新一次_OK = new PLC_Device("");
        Task Task_藥品資料更新一次;
        MyTimer MyTimer_藥品資料更新一次_結束延遲 = new MyTimer();
        int cnt_Program_藥品資料更新一次 = 65534;
        void sub_Program_藥品資料更新一次()
        {
            PLC_Device_藥品資料更新一次.Bool = true;
            if (cnt_Program_藥品資料更新一次 == 65534)
            {
                this.MyTimer_藥品資料更新一次_結束延遲.StartTickTime(10000);
                PLC_Device_藥品資料更新一次.SetComment("PLC_藥品資料更新一次");
                PLC_Device_藥品資料更新一次_OK.SetComment("PLC_藥品資料更新一次_OK");
                PLC_Device_藥品資料更新一次.Bool = false;
                cnt_Program_藥品資料更新一次 = 65535;
            }
            if (cnt_Program_藥品資料更新一次 == 65535) cnt_Program_藥品資料更新一次 = 1;
            if (cnt_Program_藥品資料更新一次 == 1) cnt_Program_藥品資料更新一次_檢查按下(ref cnt_Program_藥品資料更新一次);
            if (cnt_Program_藥品資料更新一次 == 2) cnt_Program_藥品資料更新一次_初始化(ref cnt_Program_藥品資料更新一次);
            if (cnt_Program_藥品資料更新一次 == 3) cnt_Program_藥品資料更新一次 = 65500;
            if (cnt_Program_藥品資料更新一次 > 1) cnt_Program_藥品資料更新一次_檢查放開(ref cnt_Program_藥品資料更新一次);

            if (cnt_Program_藥品資料更新一次 == 65500)
            {
                this.MyTimer_藥品資料更新一次_結束延遲.TickStop();
                this.MyTimer_藥品資料更新一次_結束延遲.StartTickTime(60 * 60 * 24 * 1000);
                PLC_Device_藥品資料更新一次.Bool = false;
                PLC_Device_藥品資料更新一次_OK.Bool = false;
                cnt_Program_藥品資料更新一次 = 65535;
            }
        }
        void cnt_Program_藥品資料更新一次_檢查按下(ref int cnt)
        {
            if (PLC_Device_藥品資料更新一次.Bool) cnt++;
        }
        void cnt_Program_藥品資料更新一次_檢查放開(ref int cnt)
        {
            if (!PLC_Device_藥品資料更新一次.Bool) cnt = 65500;
        }
        void cnt_Program_藥品資料更新一次_初始化(ref int cnt)
        {
            if (this.MyTimer_藥品資料更新一次_結束延遲.IsTimeOut())
            {
                if (Task_藥品資料更新一次 == null)
                {
                    Task_藥品資料更新一次 = new Task(new Action(delegate
                    {
                        plC_RJ_Button_雲端_藥品資料_更新資料_MouseDownEvent(null);
                        PlC_RJ_Button_本地_藥品資料_更新所有雲端藥品資料_MouseDownEvent(null);
                        Function_藥庫_藥品資料_檢查表格();
                        //Function_藥庫_藥品資料_檢查DeviceBasic();
                        Function_藥局_藥品資料_檢查表格();
                        //Function_藥局_藥品資料_檢查DeviceBasic();
                    }));
                }
                if (Task_藥品資料更新一次.Status == TaskStatus.RanToCompletion)
                {
                    Task_藥品資料更新一次 = new Task(new Action(delegate
                    {
                        plC_RJ_Button_雲端_藥品資料_更新資料_MouseDownEvent(null);
                        PlC_RJ_Button_本地_藥品資料_更新所有雲端藥品資料_MouseDownEvent(null);
                        Function_藥庫_藥品資料_檢查表格();
                        //Function_藥庫_藥品資料_檢查DeviceBasic();
                        Function_藥局_藥品資料_檢查表格();
                        //Function_藥局_藥品資料_檢查DeviceBasic();
                    }));
                }
                if (Task_藥品資料更新一次.Status == TaskStatus.Created)
                {
                    Task_藥品資料更新一次.Start();
                }
                cnt++;
            }
        }







        #endregion
    }
}
