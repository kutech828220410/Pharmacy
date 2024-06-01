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
        public enum enum_補給驗收入庫
        {
            GUID,
            請購單號,
            驗收單號,
            藥品碼,
            數量,
            效期,
            批號,
            驗收時間,
            加入時間,
            狀態,
            來源,
            備註,
        }

        private void sub_Program_補給驗收入庫_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_補給驗收入庫, dBConfigClass.DB_Basic);
            this.sqL_DataGridView_補給驗收入庫.Init();


            this.plC_UI_Init.Add_Method(sub_Program_補給驗收入庫);
        }

     
        private bool flag_補給驗收入庫 = false;
        private void sub_Program_補給驗收入庫()
        {
            sub_Program_檢查補給驗收入庫();
        }

        #region PLC_檢查補給驗收入庫
        Task Task_檢查補給驗收入庫;
        PLC_Device PLC_Device_檢查補給驗收入庫 = new PLC_Device("");
        PLC_Device PLC_Device_檢查補給驗收入庫_OK = new PLC_Device("");
        MyTimer MyTimer_檢查補給驗收入庫_結束延遲 = new MyTimer();
        int cnt_Program_檢查補給驗收入庫 = 65534;
        void sub_Program_檢查補給驗收入庫()
        {
            PLC_Device_檢查補給驗收入庫.Bool = true;
            if (cnt_Program_檢查補給驗收入庫 == 65534)
            {
                this.MyTimer_檢查補給驗收入庫_結束延遲.StartTickTime(10000);
                PLC_Device_檢查補給驗收入庫.SetComment("PLC_檢查補給驗收入庫");
                PLC_Device_檢查補給驗收入庫_OK.SetComment("PLC_檢查補給驗收入庫_OK");
                PLC_Device_檢查補給驗收入庫.Bool = false;
                cnt_Program_檢查補給驗收入庫 = 65535;
            }
            if (cnt_Program_檢查補給驗收入庫 == 65535) cnt_Program_檢查補給驗收入庫 = 1;
            if (cnt_Program_檢查補給驗收入庫 == 1) cnt_Program_檢查補給驗收入庫_檢查按下(ref cnt_Program_檢查補給驗收入庫);
            if (cnt_Program_檢查補給驗收入庫 == 2) cnt_Program_檢查補給驗收入庫_初始化(ref cnt_Program_檢查補給驗收入庫);
            if (cnt_Program_檢查補給驗收入庫 == 3) cnt_Program_檢查補給驗收入庫 = 65500;
            if (cnt_Program_檢查補給驗收入庫 > 1) cnt_Program_檢查補給驗收入庫_檢查放開(ref cnt_Program_檢查補給驗收入庫);

            if (cnt_Program_檢查補給驗收入庫 == 65500)
            {
                this.MyTimer_檢查補給驗收入庫_結束延遲.TickStop();
                this.MyTimer_檢查補給驗收入庫_結束延遲.StartTickTime(60000);
                PLC_Device_檢查補給驗收入庫.Bool = false;
                PLC_Device_檢查補給驗收入庫_OK.Bool = false;
                cnt_Program_檢查補給驗收入庫 = 65535;
            }
        }
        void cnt_Program_檢查補給驗收入庫_檢查按下(ref int cnt)
        {
            if (PLC_Device_檢查補給驗收入庫.Bool) cnt++;
        }
        void cnt_Program_檢查補給驗收入庫_檢查放開(ref int cnt)
        {
            if (!PLC_Device_檢查補給驗收入庫.Bool) cnt = 65500;
        }
        void cnt_Program_檢查補給驗收入庫_初始化(ref int cnt)
        {
            if (this.MyTimer_檢查補給驗收入庫_結束延遲.IsTimeOut())
            {
                List<object[]> list_過帳狀態 = this.sqL_DataGridView_過帳狀態列表.SQL_GetAllRows(false);
                List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端_藥品資料_old.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                List<object[]> list_過帳明細_Add = new List<object[]>();
                list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.類別, enum_寫入報表設定_類別.其他.GetEnumName());
                list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.狀態, enum_過帳狀態.已產生排程.GetEnumName());
                list_過帳狀態 = list_過帳狀態.GetRows((int)enum_過帳狀態列表.檔名, "驗收入庫(一)");
                if (list_過帳狀態.Count > 0)
                {
                    //List<object[]> list_發票資料 = this.sqL_DataGridView_驗收入庫_補給驗收_發票資料.SQL_GetAllRows(false);

                    //if (Task_檢查補給驗收入庫 == null)
                    //{
                    
                    //    Task_檢查補給驗收入庫 = new Task(new Action(delegate
                    //    {                           
                    //        this.Function_驗收入庫_補給驗收_寫入過帳明細(list_發票資料);
                    //        List<object[]> list_驗收入庫明細 = this.Function_驗收入庫明細_取得資料();
                    //        this.Function_驗收入庫明細_選取資料過帳(list_驗收入庫明細);
                    //    }));
                    //}
                    //if (Task_檢查補給驗收入庫.Status == TaskStatus.RanToCompletion)
                    //{
                    //    Task_檢查補給驗收入庫 = new Task(new Action(delegate
                    //    {
                    //        this.Function_驗收入庫_補給驗收_寫入過帳明細(list_發票資料);
                    //        List<object[]> list_驗收入庫明細 = this.Function_驗收入庫明細_取得資料();
                    //        this.Function_驗收入庫明細_選取資料過帳(list_驗收入庫明細);
                    //    }));
                    //}
                    //if (Task_檢查補給驗收入庫.Status == TaskStatus.Faulted)
                    //{
                    //    Task_檢查補給驗收入庫 = new Task(new Action(delegate
                    //    {
                    //        this.Function_驗收入庫_補給驗收_寫入過帳明細(list_發票資料);
                    //        List<object[]> list_驗收入庫明細 = this.Function_驗收入庫明細_取得資料();
                    //        this.Function_驗收入庫明細_選取資料過帳(list_驗收入庫明細);
                    //    }));
                    //}
                    //if (Task_檢查補給驗收入庫.Status == TaskStatus.Created)
                    //{
                    //    Task_檢查補給驗收入庫.Start();
                    //}
                    list_過帳狀態[0][(int)enum_過帳狀態列表.排程作業時間] = DateTime.Now.ToDateTimeString_6();
                    list_過帳狀態[0][(int)enum_過帳狀態列表.狀態] = enum_過帳狀態.排程已作業.GetEnumName();
                    this.sqL_DataGridView_過帳狀態列表.SQL_ReplaceExtra(list_過帳狀態[0], false);
                }


                cnt++;
            }
        }







        #endregion
    }
}
