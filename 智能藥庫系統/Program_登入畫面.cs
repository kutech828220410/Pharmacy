﻿using System;
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
using HIS_DB_Lib;
namespace 智能藥庫系統
{

    public partial class Main_Form : Form
    {
        readonly private string Admin_ID = "admin";
        readonly private string Admoin_Password = "66437068";
        private bool flag_登入畫面_頁面更新 = false;

        private PLC_Device PLC_Device_已登入 = new PLC_Device("S4000");
        private PLC_Device PLC_Device_未登入 = new PLC_Device("S4001");
        private PLC_Device PLC_Device_最高權限 = new PLC_Device("S4077");

        static public string _登入者名稱 = "";
        public string 登入者名稱
        {
            get
            {
                return _登入者名稱;
            }
            set
            {
                _登入者名稱 = value;
                this.rJ_TextBox_登入者姓名.Texts = value;
                this.rJ_Lable_登入畫面_登出顯示_姓名.Text = value;
            }
        }
        static public string _登入者ID = "";
        public string 登入者ID
        {
            get
            {
                return _登入者ID;
            }
            set
            {
                _登入者ID = value;
                this.rJ_TextBox_登入者ID.Texts = value;
            }
        }
        private string 登入者顏色
        {
            get
            {
                return this.rJ_TextBox_登入者顏色.Text;
            }
            set
            {
                Color color = value.ToColor();
                this.rJ_TextBox_登入者顏色.ForeColor = color;
                this.rJ_TextBox_登入者顏色.BackColor = color;
                this.rJ_TextBox_登入者顏色.Text = color.ToColorString();
            }
        }

        private void sub_Program_登入畫面_Init()
        {
            plC_RJ_Button_登入畫面_登入.MouseDownEvent += PlC_RJ_Button_登入畫面_登入_MouseDownEvent;
            plC_RJ_Button_登入畫面_登出.MouseDownEvent += PlC_RJ_Button_登入畫面_登出_MouseDownEvent;
            plC_RJ_Button_登入畫面_更換密碼.MouseDownEvent += PlC_RJ_Button_登入畫面_更換密碼_MouseDownEvent;
            this.plC_RJ_Button_登入畫面_登出顯示_登出.MouseDownEvent += PlC_RJ_Button_登入畫面_登出顯示_登出_MouseDownEvent;


            textBox_登入畫面_帳號.KeyPress += TextBox_登入畫面_帳號_KeyPress;
            textBox_登入畫面_密碼.KeyPress += TextBox_登入畫面_密碼_KeyPress;

            this.Function_登出();

            this.plC_UI_Init.Add_Method(this.sub_Program_登入畫面);


        }

  

        private void sub_Program_登入畫面()
        {
            PLC_Device_未登入.Bool = !PLC_Device_已登入.Bool;
            if (this.plC_ScreenPage_Main.PageText == "登入畫面")
            {
                if (!this.flag_登入畫面_頁面更新)
                {
                    this.Invoke(new Action(delegate
                    {
                        this.textBox_登入畫面_帳號.Text = "";
                        this.textBox_登入畫面_密碼.Text = "";
                    }));
                    this.flag_登入畫面_頁面更新 = true;
                }
            }
            else
            {
                this.flag_登入畫面_頁面更新 = false;
            }
            if(!PLC_Device_已登入.Bool)
            {
                this.myTimer_登出計時.TickStop();
                this.myTimer_登出計時.StartTickTime(600000);
            }
            else
            {
                if (this.myTimer_登出計時.IsTimeOut())
                {
                    Function_登出();
                }
            }
            rJ_ProgressBar_閒置登出時間.Maximum = 600000;
            if ((int)this.myTimer_登出計時.GetTickTime() < rJ_ProgressBar_閒置登出時間.Maximum)
            {
                rJ_ProgressBar_閒置登出時間.Value = (int)this.myTimer_登出計時.GetTickTime();
            }
            if (PLC_Device_已登入.Bool)
            {
                if(plC_RJ_GroupBox_登入畫面_登入顯示.Visible)
                {
                    this.Invoke(new Action(delegate
                    {
                        plC_RJ_GroupBox_登入畫面_登入顯示.Visible = false;
                    }));
                }
                if(!rJ_Pannel_登入畫面_登出顯示.Visible)
                {
                    this.Invoke(new Action(delegate
                    {
                        rJ_Pannel_登入畫面_登出顯示.Visible = true;
                    }));
                }
            }
            else
            {
                if (!plC_RJ_GroupBox_登入畫面_登入顯示.Visible)
                {
                    this.Invoke(new Action(delegate
                    {
                        plC_RJ_GroupBox_登入畫面_登入顯示.Visible = true;
                    }));
                }
                if (rJ_Pannel_登入畫面_登出顯示.Visible)
                {
                    this.Invoke(new Action(delegate
                    {
                        rJ_Pannel_登入畫面_登出顯示.Visible = false;
                    }));
                }
            }

            this.sub_Program_登入畫面_RFID登入();
        }
        #region PLC_登入畫面_RFID登入
        PLC_Device PLC_Device_登入畫面_RFID登入 = new PLC_Device("");
        int cnt_Program_登入畫面_RFID登入 = 65534;
        void sub_Program_登入畫面_RFID登入()
        {
            if (this.plC_ScreenPage_Main.PageText == "登入畫面")
            {
                PLC_Device_登入畫面_RFID登入.Bool = true;
            }
            else
            {
                PLC_Device_登入畫面_RFID登入.Bool = false;
            }
            if (cnt_Program_登入畫面_RFID登入 == 65534)
            {
                PLC_Device_登入畫面_RFID登入.SetComment("PLC_登入畫面_RFID登入");
                PLC_Device_登入畫面_RFID登入.Bool = false;
                cnt_Program_登入畫面_RFID登入 = 65535;
            }
            if (cnt_Program_登入畫面_RFID登入 == 65535) cnt_Program_登入畫面_RFID登入 = 1;
            if (cnt_Program_登入畫面_RFID登入 == 1) cnt_Program_登入畫面_RFID登入_檢查按下(ref cnt_Program_登入畫面_RFID登入);
            if (cnt_Program_登入畫面_RFID登入 == 2) cnt_Program_登入畫面_RFID登入_初始化(ref cnt_Program_登入畫面_RFID登入);
            if (cnt_Program_登入畫面_RFID登入 == 3) cnt_Program_登入畫面_RFID登入_檢查權限登入(ref cnt_Program_登入畫面_RFID登入);
            if (cnt_Program_登入畫面_RFID登入 == 4) cnt_Program_登入畫面_RFID登入_讀取RFID(ref cnt_Program_登入畫面_RFID登入);
            if (cnt_Program_登入畫面_RFID登入 == 5) cnt_Program_登入畫面_RFID登入_開始登入(ref cnt_Program_登入畫面_RFID登入);
            if (cnt_Program_登入畫面_RFID登入 == 6) cnt_Program_登入畫面_RFID登入_等待登入完成(ref cnt_Program_登入畫面_RFID登入);
            if (cnt_Program_登入畫面_RFID登入 == 7) cnt_Program_登入畫面_RFID登入 = 65500;
            if (cnt_Program_登入畫面_RFID登入 > 1) cnt_Program_登入畫面_RFID登入_檢查放開(ref cnt_Program_登入畫面_RFID登入);

            if (cnt_Program_登入畫面_RFID登入 == 65500)
            {
                PLC_Device_登入畫面_RFID登入.Bool = false;
                cnt_Program_登入畫面_RFID登入 = 65535;
            }
        }
        void cnt_Program_登入畫面_RFID登入_檢查按下(ref int cnt)
        {
            if (PLC_Device_登入畫面_RFID登入.Bool) cnt++;
        }
        void cnt_Program_登入畫面_RFID登入_檢查放開(ref int cnt)
        {
            if (!PLC_Device_登入畫面_RFID登入.Bool) cnt = 65500;
        }
        void cnt_Program_登入畫面_RFID登入_初始化(ref int cnt)
        {

            cnt++;
        }
        void cnt_Program_登入畫面_RFID登入_檢查權限登入(ref int cnt)
        {
            if (!this.PLC_Device_已登入.Bool)
            {
                cnt++;
                return;
            }
            else
            {
                cnt = 65500;
                return;
            }

        }
        void cnt_Program_登入畫面_RFID登入_讀取RFID(ref int cnt)
        {
            string RFID = "0";
            List<RFID_FX600lib.RFID_FX600_UI.RFID_Device> list_RFID = this.rfiD_FX600_UI.Get_RFID();

      
            if (list_RFID.Count != 0)
            {
                if (list_RFID[0].UID.StringToInt32() != 0)
                {
                    RFID = list_RFID[0].UID;
                }
            }
            if (RFID.StringToInt32() == 0 || RFID.StringIsEmpty())
            {
                cnt = 65500;
                return;
            }

            List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), RFID, false);
            if (list_人員資料.Count > 0)
            {
                this.Invoke(new Action(delegate
                {
                    this.textBox_登入畫面_帳號.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    this.textBox_登入畫面_密碼.Text = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                }));
                Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.RFID登入, list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString(), "登入畫面");
            }
            else
            {
                MyMessageBox.ShowDialog(string.Format("查無此卡帳號! {0}", RFID));
                cnt = 65500;
                return;
            }

            cnt++;
        }
        void cnt_Program_登入畫面_RFID登入_開始登入(ref int cnt)
        {
            Function_登入();
            cnt++;
        }
        void cnt_Program_登入畫面_RFID登入_等待登入完成(ref int cnt)
        {
            cnt++;
        }
        #endregion

        #region Function
        private bool Function_登入()
        {
            bool flag = false;

            this.Invoke(new Action(delegate
            {
                if (this.textBox_登入畫面_帳號.Text.ToUpper() == Admin_ID.ToUpper())
                {
                    if (this.textBox_登入畫面_密碼.Text.ToUpper() == Admoin_Password.ToUpper())
                    {
                        this.登入者名稱 = "最高管理權限";
                        this.登入者ID = "admin";
                        this.登入者顏色 = "";
                        this.textBox_登入畫面_帳號.Text = "";
                        this.textBox_登入畫面_密碼.Text = "";
                        this.Function_登入權限資料_最高權限();
                        this.PLC_Device_已登入.Bool = true;
                        this.Text = $"{this.FormText}         [登入者名稱 : {登入者名稱}] [登入者ID : {登入者ID}]";
                        this.rJ_Lable_登入畫面_登出顯示_姓名.Text = 登入者名稱;
                        flag = true;
                        Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.密碼登入, this.登入者名稱, "登入畫面");
                        return;
                    }
                }
          

                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetAllRows(false);
                List<object[]> list_人員資料_buf = new List<object[]>();
                string ID = this.textBox_登入畫面_帳號.Text;
                string password = textBox_登入畫面_密碼.Text;
                list_人員資料_buf = list_人員資料.GetRows((int)enum_人員資料.ID, ID);
                if (list_人員資料_buf.Count > 0)
                {
                    if (password != list_人員資料_buf[0][(int)enum_人員資料.密碼].ObjectToString())
                    {
                        MyMessageBox.ShowDialog("密碼錯誤");
                        flag = false;
                        return;
                    }
                    this.登入者名稱 = list_人員資料_buf[0][(int)enum_人員資料.姓名].ObjectToString();
                    this.登入者ID = list_人員資料_buf[0][(int)enum_人員資料.ID].ObjectToString();
                    this.登入者顏色 = list_人員資料_buf[0][(int)enum_人員資料.顏色].ObjectToString();
                    int level = list_人員資料_buf[0][(int)enum_人員資料.權限等級].StringToInt32();
                    this.Function_登入權限資料_取得權限(level);
                    this.textBox_登入畫面_帳號.Text = "";
                    this.textBox_登入畫面_密碼.Text = "";
                    this.PLC_Device_已登入.Bool = true;
                    this.Text = $"{this.FormText}         [登入者名稱 : {登入者名稱}] [登入者ID : {登入者ID}]";
                    flag = true;
                    Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.密碼登入, this.登入者名稱, "登入畫面");
                    return;
                }
                else
                {
                    MyMessageBox.ShowDialog("查無此帳號");
                    flag = false;
                    return;
                }
                flag = false;
            }));
            return flag;
        }
        private void Function_登出()
        {
            Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.登出, this.登入者名稱, "登入畫面");
            this.Invoke(new Action(delegate
            {
                this.登入者名稱 = "";
                this.登入者ID = "";
                this.登入者顏色 = "";
                this.textBox_登入畫面_帳號.Text = "";
                this.textBox_登入畫面_密碼.Text = "";
                this.PLC_Device_已登入.Bool = false;
                this.Function_登入權限資料_清除權限();
                this.PLC_Device_最高權限.Bool = false;
                this.Text = $"{this.FormText}";
            }));
            this.PLC_Device_主頁面頁碼.Value = 0;
        }
        #endregion
        #region Event
        private void TextBox_登入畫面_帳號_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox_登入畫面_密碼.Focus();
            }
        }
        private void TextBox_登入畫面_密碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (!this.Function_登入())
                {                
                    textBox_登入畫面_帳號.Focus();
                    return;
                }
                else
                {
                }
            }
        }
        private void PlC_RJ_Button_登入畫面_登入_MouseDownEvent(MouseEventArgs mevent)
        {
          
            Function_登入();
        }
        private void PlC_RJ_Button_登入畫面_登出_MouseDownEvent(MouseEventArgs mevent)
        {
            Function_登出();
        }
        private void PlC_RJ_Button_登入畫面_登出顯示_登出_MouseDownEvent(MouseEventArgs mevent)
        {
            Function_登出();
        }
        private void PlC_RJ_Button_登入畫面_更換密碼_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_登入者ID.Text.StringIsEmpty()) return;
            Dialog_更換密碼 dialog_更換密碼 = new Dialog_更換密碼();
            if (dialog_更換密碼.ShowDialog() != DialogResult.Yes) return;
            List<object[]> list_value = this.sqL_DataGridView_人員資料.SQL_GetRows((int)enum_人員資料.ID, 登入者ID, false);
            if (list_value.Count == 0) return;

            list_value[0][(int)enum_人員資料.密碼] = dialog_更換密碼.Value;

            this.sqL_DataGridView_人員資料.SQL_ReplaceExtra(list_value[0], false);

            MyMessageBox.ShowDialog("密碼修改成功!請重新登入‧");

            this.Function_登出();

        }
        #endregion
    }
}
