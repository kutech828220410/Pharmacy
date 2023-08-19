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
        public enum enum_本地_藥品資料
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
            藥品條碼1,
            藥品條碼2,
        }
  
        private void sub_Program_本地_藥品資料_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_本地_藥品資料, dBConfigClass.DB_DS01);
            this.sqL_DataGridView_本地_藥品資料.Init();

            this.plC_RJ_Button_本地_藥品資料_更新所有雲端藥品資料.MouseDownEvent += PlC_RJ_Button_本地_藥品資料_更新所有雲端藥品資料_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_本地_藥品資料);
        }

     

        private bool flag_本地_藥品資料 = false;
        private void sub_Program_本地_藥品資料()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥品資料")
            {
                if (!this.flag_本地_藥品資料)
                {
                    this.flag_本地_藥品資料 = true;
                }

            }
            else
            {
                this.flag_本地_藥品資料 = false;
            }
        }

        #region Function
        private List<object[]> Function_本地_藥品資料_列出DC藥品()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端_藥品資料_old.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_本地_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_value = this.Function_本地_藥品資料_列出DC藥品(list_雲端藥檔, list_本地藥檔);
            return list_value;
        }
        private List<object[]> Function_本地_藥品資料_列出DC藥品(List<object[]> list_雲端藥檔, List<object[]> list_本地藥檔)
        {

            List<object[]> list_value = new List<object[]>();

            Parallel.ForEach(list_本地藥檔, value =>
            {
                List<object[]> list_雲端藥檔_buf = new List<object[]>();
                List<object[]> list_本地藥檔_buf = new List<object[]>();

                list_雲端藥檔_buf = list_雲端藥檔.GetRows((int)enum_雲端_藥品資料_old.藥品碼, value[(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_雲端藥檔_buf.Count == 0)
                {
                    list_value.LockAdd(value);
                }
            });


            return list_value;
        }
        private List<object[]> Function_本地_藥品資料_列出異動藥品()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端_藥品資料_old.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_本地_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_value = this.Function_本地_藥品資料_列出異動藥品(list_雲端藥檔, list_本地藥檔);
            return list_value;
        }
        private List<object[]> Function_本地_藥品資料_列出異動藥品(List<object[]> list_雲端藥檔, List<object[]> list_本地藥檔)
        {

            bool flag_IsEqual = false;
            List<object[]> list_本地藥檔_buf = new List<object[]>();
            Parallel.ForEach(list_本地藥檔, value =>
            {
                List<object[]> list_雲端藥檔_buf = new List<object[]>();

                list_雲端藥檔_buf = list_雲端藥檔.GetRows((int)enum_雲端_藥品資料_old.藥品碼, value[(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_雲端藥檔_buf.Count != 0)
                {
                    object[] value_dst = LINQ.CopyRow(list_雲端藥檔_buf[0], new enum_雲端_藥品資料_old(), new enum_本地_藥品資料());
                    flag_IsEqual = value.IsEqual(value_dst, (int)enum_本地_藥品資料.GUID, (int)enum_本地_藥品資料.藥品群組);
                    if (!flag_IsEqual)
                    {
                        value[(int)enum_本地_藥品資料.藥品名稱] = list_雲端藥檔_buf[0][(int)enum_雲端_藥品資料_old.藥品名稱];
                        value[(int)enum_本地_藥品資料.藥品學名] = list_雲端藥檔_buf[0][(int)enum_雲端_藥品資料_old.藥品學名];

                        list_本地藥檔_buf.LockAdd(value);
                    }
                }
            });

            return list_本地藥檔_buf;
        }
        private List<object[]> Function_本地_藥品資料_列出新增藥品()
        {
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端_藥品資料_old.SQL_GetAllRows(false);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_本地_藥品資料.SQL_GetAllRows(false);
            List<object[]> list_value = this.Function_本地_藥品資料_列出新增藥品(list_雲端藥檔, list_本地藥檔);
            return list_value;

        }
        private List<object[]> Function_本地_藥品資料_列出新增藥品(List<object[]> list_雲端藥檔, List<object[]> list_本地藥檔)
        {

            List<object[]> list_雲端藥檔_新增藥品 = new List<object[]>();
            List<object[]> list_本地藥檔_新增藥品 = new List<object[]>();

            Parallel.ForEach(list_雲端藥檔, value =>
            {
                if (value == null)
                {

                }
                List<object[]> list_雲端藥檔_buf = new List<object[]>();
                List<object[]> list_本地藥檔_buf = new List<object[]>();
                list_本地藥檔_buf = list_本地藥檔.GetRows((int)enum_本地_藥品資料.藥品碼, value[(int)enum_雲端_藥品資料_old.藥品碼].ObjectToString());
                if (list_本地藥檔_buf.Count == 0)
                {

                    list_雲端藥檔_新增藥品.LockAdd(value);
                }

            });


            for (int i = 0; i < list_雲端藥檔_新增藥品.Count; i++)
            {
                object[] value_dst = LINQ.CopyRow(list_雲端藥檔_新增藥品[i], new enum_雲端_藥品資料_old(), new enum_本地_藥品資料());
                list_本地藥檔_新增藥品.Add(value_dst);
            }
            return list_本地藥檔_新增藥品;

        }
        #endregion
        #region Event
        private void PlC_RJ_Button_本地_藥品資料_更新所有雲端藥品資料_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            List<object[]> list_雲端藥檔 = this.sqL_DataGridView_雲端_藥品資料_old.SQL_GetAllRows(false);
            Console.WriteLine($"取得雲端藥檔 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            List<object[]> list_本地藥檔 = this.sqL_DataGridView_本地_藥品資料.SQL_GetAllRows(false);
            Console.WriteLine($"取得本地藥檔 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
            List<object[]> list_新增藥品 = this.Function_本地_藥品資料_列出新增藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_異動藥品 = this.Function_本地_藥品資料_列出異動藥品(list_雲端藥檔, list_本地藥檔);
            List<object[]> list_DC藥品 = this.Function_本地_藥品資料_列出DC藥品(list_雲端藥檔, list_本地藥檔);

            List<object[]> list_新增藥品_buf = new List<object[]>();
            List<object[]> list_異動藥品_buf = new List<object[]>();
            List<object[]> list_DC藥品_buf = new List<object[]>();

            List<object[]> list_AddValue_buf = new List<object[]>();
            List<object[]> list_ReplaceValue_buf = new List<object[]>();
            List<object[]> list_Delete_buf = new List<object[]>();

            List<object[]> list_value = list_雲端藥檔;
            for (int i = 0; i < list_value.Count; i++)
            {
                list_新增藥品_buf = list_新增藥品.GetRows((int)enum_本地_藥品資料.藥品碼, list_value[i][(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_新增藥品_buf.Count == 1)
                {
                    list_AddValue_buf.Add(list_新增藥品_buf[0]);
                    continue;
                }
                list_異動藥品_buf = list_異動藥品.GetRows((int)enum_本地_藥品資料.藥品碼, list_value[i][(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_異動藥品_buf.Count > 0)
                {
                    list_異動藥品_buf[0][(int)enum_本地_藥品資料.GUID] = list_value[i][(int)enum_本地_藥品資料.GUID];
                    list_ReplaceValue_buf.Add(list_異動藥品_buf[0]);
                    continue;
                }

                list_DC藥品_buf = list_DC藥品.GetRows((int)enum_本地_藥品資料.藥品碼, list_value[i][(int)enum_本地_藥品資料.藥品碼].ObjectToString());
                if (list_DC藥品_buf.Count > 0)
                {
                    list_Delete_buf.Add(list_DC藥品_buf[0]);
                    continue;
                }
            }
            for (int i = 0; i < list_AddValue_buf.Count; i++)
            {
                list_AddValue_buf[i][(int)enum_本地_藥品資料.GUID] = Guid.NewGuid().ToString();
            }
            this.sqL_DataGridView_本地_藥品資料.SQL_AddRows(list_AddValue_buf, false);
            this.sqL_DataGridView_本地_藥品資料.SQL_ReplaceExtra(list_ReplaceValue_buf, false);
            this.sqL_DataGridView_本地_藥品資料.SQL_DeleteExtra(list_Delete_buf, false);
        }
        #endregion
    }
}
