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

namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        public enum enum_緊急訂單_下訂單_訂單內容
        {
            GUID,
            藥品碼,
            藥品名稱,
            包裝單位,
            數量,
            單價,
            總價,
            前次訂購單價,
        }

        private void sub_Program_藥庫_緊急訂單_下訂單_Init()
        {
            this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.Init();
            this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.RowDoubleClickEvent += SqL_DataGridView_緊急訂單_下訂單_訂單內容_RowDoubleClickEvent;

            this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.Init(this.sqL_DataGridView_藥品補給系統_藥品資料);
            this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.Set_ColumnVisible(false, new enum_藥品補給系統_藥品資料().GetEnumNames());
            this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.Set_ColumnVisible(true, enum_藥品補給系統_藥品資料.藥品碼);
            this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.Set_ColumnVisible(true, enum_藥品補給系統_藥品資料.藥品名稱);
            this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.Set_ColumnVisible(true, enum_藥品補給系統_藥品資料.包裝單位);
            this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.Set_ColumnVisible(true, enum_藥品補給系統_藥品資料.契約價金);
            this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.Set_ColumnWidth(350, enum_藥品補給系統_藥品資料.藥品名稱);

            this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.Init(this.sqL_DataGridView_藥品補給系統_供應商資料);
            this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.Set_ColumnVisible(false, new enum_藥品補給系統_供應商資料().GetEnumNames());
            this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.Set_ColumnVisible(true, enum_藥品補給系統_供應商資料.全名);
            this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.Set_ColumnVisible(true, enum_藥品補給系統_供應商資料.簡名);
            this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.Set_ColumnVisible(true, enum_藥品補給系統_供應商資料.Email);
            this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.Set_ColumnVisible(true, enum_藥品補給系統_供應商資料.訂單最低總金額);


            this.rJ_TextBox_緊急訂單_下訂單_藥品搜尋_藥品碼.KeyPress += RJ_TextBox_緊急訂單_下訂單_藥品搜尋_藥品碼_KeyPress;
            this.rJ_TextBox_緊急訂單_下訂單_藥品搜尋_藥品名稱.KeyPress += RJ_TextBox_緊急訂單_下訂單_藥品搜尋_藥品名稱_KeyPress;
            this.rJ_TextBox_緊急訂單_下訂單_數量.KeyPress += RJ_TextBox_緊急訂單_下訂單_數量_KeyPress;
            this.rJ_TextBox_緊急訂單_下訂單_數量._TextChanged += RJ_TextBox_緊急訂單_下訂單_數量__TextChanged;
            this.rJ_TextBox_緊急訂單_下訂單_單價.KeyPress += RJ_TextBox_緊急訂單_下訂單_單價_KeyPress;
            this.rJ_TextBox_緊急訂單_下訂單_單價._TextChanged += RJ_TextBox_緊急訂單_下訂單_單價__TextChanged;

            this.plC_RJ_Button_緊急訂單_下訂單_自動產生訂單編號.MouseDownEvent += PlC_RJ_Button_緊急訂單_下訂單_自動產生訂單編號_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_下訂單_藥品搜尋_藥品碼搜尋.MouseDownEvent += PlC_RJ_Button_緊急訂單_下訂單_藥品搜尋_藥品碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_下訂單_藥品搜尋_藥品名稱搜尋.MouseDownEvent += PlC_RJ_Button_緊急訂單_下訂單_藥品搜尋_藥品名稱搜尋_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_下訂單_供應商搜尋_全名搜尋.MouseDownEvent += PlC_RJ_Button_緊急訂單_下訂單_供應商搜尋_全名搜尋_MouseDownEvent;

            this.plC_RJ_Button_緊急訂單_下訂單_藥品搜尋_填入.MouseDownEvent += PlC_RJ_Button_緊急訂單_下訂單_藥品搜尋_填入_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_下訂單_供應商搜尋_填入.MouseDownEvent += PlC_RJ_Button_緊急訂單_下訂單_供應商搜尋_填入_MouseDownEvent;

            this.plC_RJ_Button_緊急訂單_下訂單_新增訂單內容.MouseDownEvent += PlC_RJ_Button_緊急訂單_下訂單_新增訂單內容_MouseDownEvent;
            this.plC_RJ_Button_緊急訂單_下訂單_刪除訂單內容.MouseDownEvent += PlC_RJ_Button_緊急訂單_下訂單_刪除訂單內容_MouseDownEvent;

            this.plC_Button_下訂單_取消作業.btnClick += PlC_Button_下訂單_取消作業_btnClick;

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_緊急訂單_下訂單);
        }



        private bool flag_藥庫_緊急訂單_下訂單 = false;
        private void sub_Program_藥庫_緊急訂單_下訂單()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "緊急訂單" && this.plC_ScreenPage_藥庫_緊急訂單.PageText == "下訂單")
            {
                this.sub_Program_緊急訂單_下訂單_資料確認();
                this.sub_Program_緊急訂單_下訂單_發送Email();
                this.sub_Program_緊急訂單_下訂單_訂單確認();
                if (!this.flag_藥庫_緊急訂單_下訂單)
                {
                    this.flag_藥庫_緊急訂單_下訂單 = true;
                }

            }
            else
            {
                this.flag_藥庫_緊急訂單_下訂單 = false;
            }
        }


        #region PLC_緊急訂單_下訂單_資料確認
        PLC_Device PLC_Device_緊急訂單_下訂單_資料確認 = new PLC_Device("M1000");
        PLC_Device PLC_Device_緊急訂單_下訂單_資料確認_OK = new PLC_Device("M1001");
        PLC_Device PLC_Device_緊急訂單_下訂單_資料確認_Enable = new PLC_Device("M1002");
        MyTimer MyTimer_緊急訂單_下訂單_資料確認_結束延遲 = new MyTimer();
        int cnt_Program_緊急訂單_下訂單_資料確認 = 65534;
        void sub_Program_緊急訂單_下訂單_資料確認()
        {
            PLC_Device_緊急訂單_下訂單_資料確認_Enable.Bool = !PLC_Device_緊急訂單_下訂單_資料確認_OK.Bool;
            if (cnt_Program_緊急訂單_下訂單_資料確認 == 65534)
            {
                PLC_Device_緊急訂單_下訂單_資料確認_OK.Bool = false;
                this.MyTimer_緊急訂單_下訂單_資料確認_結束延遲.StartTickTime(10000);
                PLC_Device_緊急訂單_下訂單_資料確認.SetComment("PLC_緊急訂單_下訂單_資料確認");
                PLC_Device_緊急訂單_下訂單_資料確認_OK.SetComment("PLC_緊急訂單_下訂單_資料確認_OK");
                PLC_Device_緊急訂單_下訂單_資料確認.Bool = false;
                cnt_Program_緊急訂單_下訂單_資料確認 = 65535;
            }
            if (cnt_Program_緊急訂單_下訂單_資料確認 == 65535) cnt_Program_緊急訂單_下訂單_資料確認 = 1;
            if (cnt_Program_緊急訂單_下訂單_資料確認 == 1) cnt_Program_緊急訂單_下訂單_資料確認_檢查按下(ref cnt_Program_緊急訂單_下訂單_資料確認);
            if (cnt_Program_緊急訂單_下訂單_資料確認 == 2) cnt_Program_緊急訂單_下訂單_資料確認_初始化(ref cnt_Program_緊急訂單_下訂單_資料確認);
            if (cnt_Program_緊急訂單_下訂單_資料確認 == 3) cnt_Program_緊急訂單_下訂單_資料確認 = 65500;
            if (cnt_Program_緊急訂單_下訂單_資料確認 > 1) cnt_Program_緊急訂單_下訂單_資料確認_檢查放開(ref cnt_Program_緊急訂單_下訂單_資料確認);

            if (cnt_Program_緊急訂單_下訂單_資料確認 == 65500)
            {
                this.MyTimer_緊急訂單_下訂單_資料確認_結束延遲.TickStop();
                this.MyTimer_緊急訂單_下訂單_資料確認_結束延遲.StartTickTime(10000);
                PLC_Device_緊急訂單_下訂單_資料確認.Bool = false;
                cnt_Program_緊急訂單_下訂單_資料確認 = 65535;
            }
        }
        void cnt_Program_緊急訂單_下訂單_資料確認_檢查按下(ref int cnt)
        {
            if (PLC_Device_緊急訂單_下訂單_資料確認.Bool) cnt++;
        }
        void cnt_Program_緊急訂單_下訂單_資料確認_檢查放開(ref int cnt)
        {
            if (!PLC_Device_緊急訂單_下訂單_資料確認.Bool) cnt = 65500;
        }
        void cnt_Program_緊急訂單_下訂單_資料確認_初始化(ref int cnt)
        {
            List<string> list_error_msg = new List<string>();
            string error_msg = "";
            List<object[]> list_訂單內容 = this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.GetAllRows();
            if (rJ_TextBox_緊急訂單_下訂單_訂單編號.Texts.StringIsEmpty()) list_error_msg.Add($"'訂單編號'空白");
            if (rJ_TextBox_緊急訂單_下訂單_訂購人.Texts.StringIsEmpty()) list_error_msg.Add($"'訂購人'空白");
            if (rJ_TextBox_緊急訂單_下訂單_院所名稱.Texts.StringIsEmpty()) list_error_msg.Add($"'院所名稱'空白");
            if (rJ_TextBox_緊急訂單_下訂單_訂購日期.Texts.StringIsEmpty()) list_error_msg.Add($"'訂購日期'空白");
            if (rJ_TextBox_緊急訂單_下訂單_應驗收日期.Texts.StringIsEmpty()) list_error_msg.Add($"'應驗收日期'空白");
            if (rJ_TextBox_緊急訂單_下訂單_全名.Texts.StringIsEmpty()) list_error_msg.Add($"'全名'空白");
            if (rJ_TextBox_緊急訂單_下訂單_Email.Texts.StringIsEmpty()) list_error_msg.Add($"'Email'空白");
            if (rJ_TextBox_緊急訂單_下訂單_聯絡人.Texts.StringIsEmpty()) list_error_msg.Add($"'聯絡人'空白");           
            if (list_訂單內容.Count == 0) list_error_msg.Add($"未建立訂單內容");

            for(int i = 0; i < list_error_msg.Count; i++)
            {
                error_msg += $"{(i+1).ToString("00")}. {list_error_msg[i]}";
                if(i != list_error_msg.Count -1) error_msg += "\n";
            }
            if(!error_msg.StringIsEmpty())
            {
                MyMessageBox.ShowDialog(error_msg);
                cnt = 65500;
                return;
            }

          
         
            this.Invoke(new Action(delegate 
            {

                textBox_信箱設定_訂單編號.Text = rJ_TextBox_緊急訂單_下訂單_訂單編號.Texts;
                textBox_信箱設定_訂購人.Text = rJ_TextBox_緊急訂單_下訂單_訂購人.Texts;
                textBox_信箱設定_訂購院所別.Text = rJ_TextBox_緊急訂單_下訂單_院所名稱.Texts;
                textBox_信箱設定_訂購日期.Text = rJ_TextBox_緊急訂單_下訂單_訂購日期.Texts;
                textBox_信箱設定_供應商全名.Text = rJ_TextBox_緊急訂單_下訂單_全名.Texts;
                textBox_信箱設定_Email.Text = rJ_TextBox_緊急訂單_下訂單_Email.Texts;
                textBox_信箱設定_聯絡人.Text = rJ_TextBox_緊急訂單_下訂單_聯絡人.Texts;
                textBox_信箱設定_應驗收日期.Text = rJ_TextBox_緊急訂單_下訂單_應驗收日期.Texts;

                this.myEmail_Send_UI.Clear();

                this.myEmail_Send_UI.Adress_From = this.textBox_信箱設定_伺服器參數_發件者.Text;
                this.myEmail_Send_UI.Adress_To = this.rJ_TextBox_緊急訂單_下訂單_Email.Text;
                this.myEmail_Send_UI.Subject = this.Function_信箱設定_ReplaceCode(this.Function_信箱設定_信箱主旨_讀檔());
                this.myEmail_Send_UI.Rtf = this.Function_信箱設定_信箱內容_讀檔();
                this.myEmail_Send_UI.UserName = this.textBox_信箱設定_伺服器參數_UserName.Text;
                this.myEmail_Send_UI.Password = this.textBox_信箱設定_伺服器參數_Password.Text;
                this.myEmail_Send_UI.Host = this.textBox_信箱設定_伺服器參數_Host.Text;
                this.myEmail_Send_UI.Port = this.textBox_信箱設定_伺服器參數_Port.Text;


                string 包裝單位;
                MyEmail.MyEmail_Send_UI.Table_Rtf Table_Rtf = new MyEmail.MyEmail_Send_UI.Table_Rtf(4, list_訂單內容.Count + 1);
                Table_Rtf.AddRow(new string[] { "藥品碼", "名稱", "數量", "包裝單位" });
                for (int i = 0; i < list_訂單內容.Count; i++)
                {
                    Table_Rtf.AddRow(new string[]
                  {
                           list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.藥品碼].ObjectToString(),
                           list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.藥品名稱].ObjectToString(),
                           list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.數量].ObjectToString(),
                           list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.包裝單位].ObjectToString(),
                  });
                }
              
                Table_Rtf.Set_ColunmWidth(0, 1000);
                Table_Rtf.Set_ColunmWidth(1, 4000);
                Table_Rtf.Set_ColunmWidth(2, 1000);
                Table_Rtf.Set_ColunmWidth(3, 1500);


                this.Function_信箱設定_ReplaceCode(this.myEmail_Send_UI, Table_Rtf);
            }));

            this.PLC_Device_緊急訂單_下訂單_資料確認_OK.Bool = true;
            cnt++;
        }



















        #endregion
        #region PLC_緊急訂單_下訂單_發送Email
        PLC_Device PLC_Device_緊急訂單_下訂單_發送Email = new PLC_Device("M1010");
        PLC_Device PLC_Device_緊急訂單_下訂單_發送Email_OK = new PLC_Device("M1011");
        PLC_Device PLC_Device_緊急訂單_下訂單_發送Email_Enable = new PLC_Device("M1012");
        MyTimer MyTimer_緊急訂單_下訂單_發送Email_結束延遲 = new MyTimer();
        int cnt_Program_緊急訂單_下訂單_發送Email = 65534;
        void sub_Program_緊急訂單_下訂單_發送Email()
        {
            PLC_Device_緊急訂單_下訂單_發送Email_Enable.Bool = (PLC_Device_緊急訂單_下訂單_資料確認_OK.Bool && !PLC_Device_緊急訂單_下訂單_發送Email_OK.Bool);
            if (cnt_Program_緊急訂單_下訂單_發送Email == 65534)
            {
                PLC_Device_緊急訂單_下訂單_發送Email_OK.Bool = false;
                this.MyTimer_緊急訂單_下訂單_發送Email_結束延遲.StartTickTime(10000);
                PLC_Device_緊急訂單_下訂單_發送Email.SetComment("PLC_緊急訂單_下訂單_發送Email");
                PLC_Device_緊急訂單_下訂單_發送Email_OK.SetComment("PLC_緊急訂單_下訂單_發送Email_OK");
                PLC_Device_緊急訂單_下訂單_發送Email.Bool = false;
                cnt_Program_緊急訂單_下訂單_發送Email = 65535;
            }
            if (cnt_Program_緊急訂單_下訂單_發送Email == 65535) cnt_Program_緊急訂單_下訂單_發送Email = 1;
            if (cnt_Program_緊急訂單_下訂單_發送Email == 1) cnt_Program_緊急訂單_下訂單_發送Email_檢查按下(ref cnt_Program_緊急訂單_下訂單_發送Email);
            if (cnt_Program_緊急訂單_下訂單_發送Email == 2) cnt_Program_緊急訂單_下訂單_發送Email_初始化(ref cnt_Program_緊急訂單_下訂單_發送Email);
            if (cnt_Program_緊急訂單_下訂單_發送Email == 3) cnt_Program_緊急訂單_下訂單_發送Email_開始發送Email(ref cnt_Program_緊急訂單_下訂單_發送Email);
            if (cnt_Program_緊急訂單_下訂單_發送Email == 4) cnt_Program_緊急訂單_下訂單_發送Email_等待發送完成_Busy(ref cnt_Program_緊急訂單_下訂單_發送Email);
            if (cnt_Program_緊急訂單_下訂單_發送Email == 5) cnt_Program_緊急訂單_下訂單_發送Email_等待發送完成(ref cnt_Program_緊急訂單_下訂單_發送Email);
            if (cnt_Program_緊急訂單_下訂單_發送Email == 6) cnt_Program_緊急訂單_下訂單_發送Email_檢查發送結果(ref cnt_Program_緊急訂單_下訂單_發送Email);
            if (cnt_Program_緊急訂單_下訂單_發送Email == 7) cnt_Program_緊急訂單_下訂單_發送Email = 65500;
            if (cnt_Program_緊急訂單_下訂單_發送Email > 1) cnt_Program_緊急訂單_下訂單_發送Email_檢查放開(ref cnt_Program_緊急訂單_下訂單_發送Email);

            if (cnt_Program_緊急訂單_下訂單_發送Email == 65500)
            {
                this.MyTimer_緊急訂單_下訂單_發送Email_結束延遲.TickStop();
                this.MyTimer_緊急訂單_下訂單_發送Email_結束延遲.StartTickTime(10000);
                PLC_Device_緊急訂單_下訂單_發送Email.Bool = false;
                cnt_Program_緊急訂單_下訂單_發送Email = 65535;
            }
            if(!this.myEmail_Send_UI.Get_Send_Ready())
            {
                if (!rJ_Lable_下訂單_發送中.Visible)
                {
                    this.Invoke(new Action(delegate
                    {
                        rJ_Lable_下訂單_發送中.Visible = true;
                    }));
                }                          
            }
            else
            {
                if(rJ_Lable_下訂單_發送中.Visible)
                {
                    this.Invoke(new Action(delegate
                    {
                        rJ_Lable_下訂單_發送中.Visible = false;
                    }));
                }
            }
        }
        void cnt_Program_緊急訂單_下訂單_發送Email_檢查按下(ref int cnt)
        {
            if (PLC_Device_緊急訂單_下訂單_發送Email.Bool) cnt++;
        }
        void cnt_Program_緊急訂單_下訂單_發送Email_檢查放開(ref int cnt)
        {
            if (!PLC_Device_緊急訂單_下訂單_發送Email.Bool) cnt = 65500;
        }
        void cnt_Program_緊急訂單_下訂單_發送Email_初始化(ref int cnt)
        {
            if(plC_Button_Email不發送.Bool)
            {
                Console.WriteLine($"Email 不發送!");
                this.PLC_Device_緊急訂單_下訂單_發送Email_OK.Bool = true;
                cnt = 65500;
                return;
            }
            cnt++;
        }
        void cnt_Program_緊急訂單_下訂單_發送Email_開始發送Email(ref int cnt)
        {
            if (this.myEmail_Send_UI.Get_Send_Ready())
            {
                Console.WriteLine($"Email 開始發送!");
                this.myEmail_Send_UI.Send_Email();
                cnt++;
            }
        }
        void cnt_Program_緊急訂單_下訂單_發送Email_等待發送完成_Busy(ref int cnt)
        {
            if (!this.myEmail_Send_UI.Get_Send_Ready())
            {
                Console.WriteLine($"Email 正在發送!");
                cnt++;
            }
        }
        void cnt_Program_緊急訂單_下訂單_發送Email_等待發送完成(ref int cnt)
        {
            if (this.myEmail_Send_UI.Get_Send_Ready())
            {
                Console.WriteLine($"Email 發送完畢!");
                cnt++;
            }
        }
        void cnt_Program_緊急訂單_下訂單_發送Email_檢查發送結果(ref int cnt)
        {
            if (this.myEmail_Send_UI.Get_Send_Error())
            {
                MyMessageBox.ShowDialog("發送失敗!");
                this.PLC_Device_緊急訂單_下訂單_發送Email_OK.Bool = false;
                cnt++;
            }
            else
            {
                MyMessageBox.ShowDialog("發送完成!");
                this.PLC_Device_緊急訂單_下訂單_發送Email_OK.Bool = true;
                cnt++;
            }
        }



















        #endregion
        #region PLC_緊急訂單_下訂單_訂單確認
        PLC_Device PLC_Device_緊急訂單_下訂單_訂單確認 = new PLC_Device("M1020");
        PLC_Device PLC_Device_緊急訂單_下訂單_訂單確認_OK = new PLC_Device("M1021");
        PLC_Device PLC_Device_緊急訂單_下訂單_訂單確認_Enable = new PLC_Device("M1022");

        MyTimer MyTimer_緊急訂單_下訂單_訂單確認_結束延遲 = new MyTimer();
        int cnt_Program_緊急訂單_下訂單_訂單確認 = 65534;
        void sub_Program_緊急訂單_下訂單_訂單確認()
        {
            PLC_Device_緊急訂單_下訂單_訂單確認_Enable.Bool = (PLC_Device_緊急訂單_下訂單_發送Email_OK.Bool && !PLC_Device_緊急訂單_下訂單_訂單確認_OK.Bool);
            if (cnt_Program_緊急訂單_下訂單_訂單確認 == 65534)
            {
                this.MyTimer_緊急訂單_下訂單_訂單確認_結束延遲.StartTickTime(10000);
                PLC_Device_緊急訂單_下訂單_訂單確認.SetComment("PLC_緊急訂單_下訂單_訂單確認");
                PLC_Device_緊急訂單_下訂單_訂單確認_OK.SetComment("PLC_緊急訂單_下訂單_訂單確認_OK");
                PLC_Device_緊急訂單_下訂單_訂單確認.Bool = false;
                cnt_Program_緊急訂單_下訂單_訂單確認 = 65535;
            }
            if (cnt_Program_緊急訂單_下訂單_訂單確認 == 65535) cnt_Program_緊急訂單_下訂單_訂單確認 = 1;
            if (cnt_Program_緊急訂單_下訂單_訂單確認 == 1) cnt_Program_緊急訂單_下訂單_訂單確認_檢查按下(ref cnt_Program_緊急訂單_下訂單_訂單確認);
            if (cnt_Program_緊急訂單_下訂單_訂單確認 == 2) cnt_Program_緊急訂單_下訂單_訂單確認_初始化(ref cnt_Program_緊急訂單_下訂單_訂單確認);
            if (cnt_Program_緊急訂單_下訂單_訂單確認 == 3) cnt_Program_緊急訂單_下訂單_訂單確認 = 65500;
            if (cnt_Program_緊急訂單_下訂單_訂單確認 > 1) cnt_Program_緊急訂單_下訂單_訂單確認_檢查放開(ref cnt_Program_緊急訂單_下訂單_訂單確認);

            if (cnt_Program_緊急訂單_下訂單_訂單確認 == 65500)
            {
                this.MyTimer_緊急訂單_下訂單_訂單確認_結束延遲.TickStop();
                this.MyTimer_緊急訂單_下訂單_訂單確認_結束延遲.StartTickTime(10000);
                PLC_Device_緊急訂單_下訂單_訂單確認.Bool = false;
                cnt_Program_緊急訂單_下訂單_訂單確認 = 65535;
            }
        }
        void cnt_Program_緊急訂單_下訂單_訂單確認_檢查按下(ref int cnt)
        {
            if (PLC_Device_緊急訂單_下訂單_訂單確認.Bool) cnt++;
        }
        void cnt_Program_緊急訂單_下訂單_訂單確認_檢查放開(ref int cnt)
        {
            if (!PLC_Device_緊急訂單_下訂單_訂單確認.Bool) cnt = 65500;
        }
        void cnt_Program_緊急訂單_下訂單_訂單確認_初始化(ref int cnt)
        {
            List<object[]> list_訂單內容 = this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.GetAllRows();
            List<object[]> list_value_add = new List<object[]>();
            for(int i = 0; i < list_訂單內容.Count; i++)
            {
                string 藥品碼 = list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.藥品碼].ObjectToString();
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品補給系統_藥品資料.SQL_GetRows((int)enum_藥品補給系統_藥品資料.藥品碼, 藥品碼, false);
                if(list_藥品資料.Count == 0)
                {
                    MyMessageBox.ShowDialog($"找無<{藥品碼}>資料!");
                    cnt = 65500;
                    return;
                }
                object[] value = new object[new enum_藥品補給系統_訂單資料().GetLength()];
                value[(int)enum_藥品補給系統_訂單資料.序號] = i.ToString();
                value[(int)enum_藥品補給系統_訂單資料.訂單編號] = this.rJ_TextBox_緊急訂單_下訂單_訂單編號.Texts;
                value[(int)enum_藥品補給系統_訂單資料.藥品碼] = list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.藥品碼].ObjectToString();
                value[(int)enum_藥品補給系統_訂單資料.藥品名稱] = list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.藥品名稱].ObjectToString();
                value[(int)enum_藥品補給系統_訂單資料.單位] = list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.包裝單位].ObjectToString();
                value[(int)enum_藥品補給系統_訂單資料.供應商全名] = rJ_TextBox_緊急訂單_下訂單_全名.Texts;
                value[(int)enum_藥品補給系統_訂單資料.供應商Email] = rJ_TextBox_緊急訂單_下訂單_Email.Texts;
                value[(int)enum_藥品補給系統_訂單資料.供應商聯絡人] = rJ_TextBox_緊急訂單_下訂單_聯絡人.Texts;
                value[(int)enum_藥品補給系統_訂單資料.供應商電話] = rJ_TextBox_緊急訂單_下訂單_電話.Texts;
                value[(int)enum_藥品補給系統_訂單資料.包裝單位] = list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.包裝單位].ObjectToString();
                value[(int)enum_藥品補給系統_訂單資料.訂購日期] = DateTime.Now.ToDateString();
                value[(int)enum_藥品補給系統_訂單資料.訂購時間] = DateTime.Now.ToDateTimeString_6();
                value[(int)enum_藥品補給系統_訂單資料.訂購人] = rJ_TextBox_緊急訂單_下訂單_訂購人.Texts;
                value[(int)enum_藥品補給系統_訂單資料.訂購院所別] = rJ_TextBox_緊急訂單_下訂單_院所名稱.Texts;
                value[(int)enum_藥品補給系統_訂單資料.訂購數量] = list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.數量].ObjectToString();
                value[(int)enum_藥品補給系統_訂單資料.已入庫數量] = "0";
                value[(int)enum_藥品補給系統_訂單資料.訂購單價] = list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.單價].ObjectToString();
                value[(int)enum_藥品補給系統_訂單資料.訂購總價] = list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.數量].ObjectToString();
                value[(int)enum_藥品補給系統_訂單資料.前次訂購單價] = list_訂單內容[i][(int)enum_緊急訂單_下訂單_訂單內容.前次訂購單價].ObjectToString();
                value[(int)enum_藥品補給系統_訂單資料.驗收人] = "";
                value[(int)enum_藥品補給系統_訂單資料.驗收院所別] = "";
                value[(int)enum_藥品補給系統_訂單資料.驗收日期] = DateTime.MinValue.ToDateString();
                value[(int)enum_藥品補給系統_訂單資料.驗收時間] = DateTime.MinValue.ToDateTimeString_6();
                value[(int)enum_藥品補給系統_訂單資料.驗收單價] = "0";
                value[(int)enum_藥品補給系統_訂單資料.驗收總價] = "0";
                value[(int)enum_藥品補給系統_訂單資料.前次驗收單價] = "0";
                value[(int)enum_藥品補給系統_訂單資料.應驗收日期] = rJ_TextBox_緊急訂單_下訂單_應驗收日期.Texts;
                value[(int)enum_藥品補給系統_訂單資料.發票日期] = DateTime.MinValue.ToDateString();
                value[(int)enum_藥品補給系統_訂單資料.發票金額] = "0";
                value[(int)enum_藥品補給系統_訂單資料.折讓金額] = "0";
                value[(int)enum_藥品補給系統_訂單資料.效期] = DateTime.MinValue.ToDateString();
                value[(int)enum_藥品補給系統_訂單資料.批號] = "";
                value[(int)enum_藥品補給系統_訂單資料.確認驗收] = false.ToString();
                value[(int)enum_藥品補給系統_訂單資料.備註] = "";
                list_藥品資料[0][(int)enum_藥品補給系統_藥品資料.已訂購數量] = (list_藥品資料[0][(int)enum_藥品補給系統_藥品資料.已訂購數量].StringToInt32() + value[(int)enum_藥品補給系統_訂單資料.訂購數量].StringToInt32()).ToString();
                list_藥品資料[0][(int)enum_藥品補給系統_藥品資料.已訂購總價] = (list_藥品資料[0][(int)enum_藥品補給系統_藥品資料.已訂購總價].ObjectToString().StringToInt32() + list_藥品資料[0][(int)enum_緊急訂單_下訂單_訂單內容.總價].StringToInt32()).ToString();
                list_value_add.Add(value);
            }
            this.sqL_DataGridView_藥品補給系統_訂單資料.SQL_AddRows(list_value_add, false);
            MyMessageBox.ShowDialog("訂單成功送出!");
            PLC_Device_緊急訂單_下訂單_訂單確認_OK.Bool = true;
            this.Function_緊急訂單_下訂單_清除全部資料();
            cnt++;
        }



        #endregion

        #region Function
        private int Function_緊急訂單_下訂單_取得略過假日天數(int 天數, DateTime date)
        {
            if (天數 > 1) 天數 = 天數 - 1;
            int DateAdd = 0;
            date = date.AddDays(天數);
            while (true)
            {
                if (!Basic.TypeConvert.IsHolidays(date.AddDays(DateAdd)))
                {
                    break;
                }
                DateAdd++;
            }
            return DateAdd + 天數;
        }
        private void Function_緊急訂單_下訂單_清除全部資料()
        {
            this.Invoke(new Action(delegate
            {
                this.rJ_TextBox_緊急訂單_下訂單_訂單編號.Texts = "";

                this.rJ_TextBox_緊急訂單_下訂單_全名.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_Email.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_聯絡人.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_電話.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_訂購日期.Texts = DateTime.Now.ToDateString();
                this.rJ_TextBox_緊急訂單_下訂單_訂購人.Texts = this.登入者名稱;
                this.rJ_TextBox_緊急訂單_下訂單_院所名稱.Texts = "屏東榮民總醫院";

                this.rJ_TextBox_緊急訂單_下訂單_藥品碼.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_藥品名稱.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_包裝單位.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_單價.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_前次單價.Texts = "";

                this.rJ_TextBox_緊急訂單_下訂單_數量.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_已訂購總價.Text = "";
                this.rJ_TextBox_緊急訂單_下訂單_已訂購總量.Text = "";
                this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.ClearGrid();
                this.myEmail_Send_UI.Clear();


                PLC_Device_緊急訂單_下訂單_資料確認_OK.Bool = false;
                PLC_Device_緊急訂單_下訂單_發送Email_OK.Bool = false;
                PLC_Device_緊急訂單_下訂單_訂單確認_OK.Bool = false;
            }));
   
        }
        #endregion
        #region Event
        private void SqL_DataGridView_緊急訂單_下訂單_訂單內容_RowDoubleClickEvent(object[] RowValue)
        {
            string 藥品碼 = RowValue[(int)enum_緊急訂單_下訂單_訂單內容.藥品碼].ObjectToString();
            string 藥品名稱 = RowValue[(int)enum_緊急訂單_下訂單_訂單內容.藥品名稱].ObjectToString();
            string 包裝單位 = RowValue[(int)enum_緊急訂單_下訂單_訂單內容.包裝單位].ObjectToString();
            string 單價 = RowValue[(int)enum_緊急訂單_下訂單_訂單內容.單價].ObjectToString();
            string 數量 = RowValue[(int)enum_緊急訂單_下訂單_訂單內容.數量].ObjectToString();
            string 總價 = RowValue[(int)enum_緊急訂單_下訂單_訂單內容.總價].ObjectToString();
            string 前次訂購單價 = RowValue[(int)enum_緊急訂單_下訂單_訂單內容.前次訂購單價].ObjectToString();

            this.Invoke(new Action(delegate
            {
                rJ_TextBox_緊急訂單_下訂單_藥品碼.Texts = 藥品碼;
                rJ_TextBox_緊急訂單_下訂單_藥品名稱.Texts = 藥品名稱;
                rJ_TextBox_緊急訂單_下訂單_包裝單位.Texts = 包裝單位;
                rJ_TextBox_緊急訂單_下訂單_單價.Texts = 單價;
                rJ_TextBox_緊急訂單_下訂單_數量.Texts = 數量;
                rJ_TextBox_緊急訂單_下訂單_總價.Texts = 總價;
                rJ_TextBox_緊急訂單_下訂單_前次單價.Texts = 前次訂購單價;
            }));
        }

        private void RJ_TextBox_緊急訂單_下訂單_藥品搜尋_藥品名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_緊急訂單_下訂單_藥品搜尋_藥品名稱搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_緊急訂單_下訂單_藥品搜尋_藥品碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_緊急訂單_下訂單_藥品搜尋_藥品碼搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_緊急訂單_下訂單_數量_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar >= 0x30 && e.KeyChar <= 0x39 || e.KeyChar == (char)Keys.Back)
            {
                return;
            }
            e.Handled = true;
        }
        private void RJ_TextBox_緊急訂單_下訂單_數量__TextChanged(object sender, EventArgs e)
        {
            double 數量 = this.rJ_TextBox_緊急訂單_下訂單_數量.Texts.StringToDouble();
            double 單價 = this.rJ_TextBox_緊急訂單_下訂單_單價.Texts.StringToDouble();
            bool flag_failed = false;
            if (單價 < 0)
            {
                this.rJ_TextBox_緊急訂單_下訂單_單價.Texts = "";
                flag_failed = true;
            }
            if (數量 < 0)
            {
                this.rJ_TextBox_緊急訂單_下訂單_數量.Texts = "";
                flag_failed = true;
            }
            if (flag_failed) return;
            double 總價 = 數量 * 單價;
            this.rJ_TextBox_緊急訂單_下訂單_總價.Texts = 總價.ToString();
        }
        private void RJ_TextBox_緊急訂單_下訂單_單價_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 0x30 && e.KeyChar <= 0x39 || e.KeyChar == (char)Keys.Back || e.KeyChar == 0x2E)
            {
                return;
            }
            
            e.Handled = true;
        }
        private void RJ_TextBox_緊急訂單_下訂單_單價__TextChanged(object sender, EventArgs e)
        {
            double 數量 = this.rJ_TextBox_緊急訂單_下訂單_數量.Texts.StringToDouble();
            double 單價 = this.rJ_TextBox_緊急訂單_下訂單_單價.Texts.StringToDouble();
            bool flag_failed = false;
            if (單價 < 0)
            {
                this.rJ_TextBox_緊急訂單_下訂單_單價.Texts = "";
                flag_failed = true;
            }
            if (數量 < 0)
            {
                this.rJ_TextBox_緊急訂單_下訂單_數量.Texts = "";
                flag_failed = true;
            }
            if (flag_failed) return;
            if (double.TryParse(this.rJ_TextBox_緊急訂單_下訂單_單價.Texts, out 單價))
            {
                
            }

            double 總價 = 數量 * 單價;
            this.rJ_TextBox_緊急訂單_下訂單_總價.Texts = 總價.ToString();
        }


        private void PlC_RJ_Button_緊急訂單_下訂單_藥品搜尋_藥品名稱搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string text = rJ_TextBox_緊急訂單_下訂單_藥品搜尋_藥品名稱.Text;
            List<object[]> list_value = this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品補給系統_藥品資料.藥品名稱, text);
            this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_緊急訂單_下訂單_藥品搜尋_藥品碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string text = rJ_TextBox_緊急訂單_下訂單_藥品搜尋_藥品碼.Text;
            List<object[]> list_value = this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品補給系統_藥品資料.藥品碼, text);
            this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_緊急訂單_下訂單_供應商搜尋_全名搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string text = rJ_TextBox_緊急訂單_下訂單_供應商搜尋_全名.Text;
            List<object[]> list_value = this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品補給系統_供應商資料.全名, text);
            this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_緊急訂單_下訂單_藥品搜尋_填入_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_緊急訂單_下訂單_藥品搜尋.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            string 藥品碼 = list_value[0][(int)enum_藥品補給系統_藥品資料.藥品碼].ObjectToString();
            string 藥品名稱 = list_value[0][(int)enum_藥品補給系統_藥品資料.藥品名稱].ObjectToString();
            string 包裝單位 = list_value[0][(int)enum_藥品補給系統_藥品資料.包裝單位].ObjectToString();
            string 最新訂購單價 = list_value[0][(int)enum_藥品補給系統_藥品資料.最新訂購單價].ObjectToString();
            string 已訂購數量 = list_value[0][(int)enum_藥品補給系統_藥品資料.已訂購數量].ObjectToString();
            string 已訂購總價 = list_value[0][(int)enum_藥品補給系統_藥品資料.已訂購總價].ObjectToString();
            string 訂購商 = list_value[0][(int)enum_藥品補給系統_藥品資料.訂購商].ObjectToString();
            string 契約價金 = list_value[0][(int)enum_藥品補給系統_藥品資料.契約價金].ObjectToString();
            string 維護到期日 = list_value[0][(int)enum_藥品補給系統_藥品資料.維護到期日].ToDateString();

            if (已訂購總價.StringToInt32() < 0) 已訂購總價 = "0";
            if (已訂購數量.StringToInt32() < 0) 已訂購數量 = "0";

            if (!維護到期日.StringIsEmpty())
            {
                if (維護到期日.Get_DateTINY() <= DateTime.Now.ToDateString().Get_DateTINY())
                {
                    MyMessageBox.ShowDialog($"<{藥品名稱}> 維護到期日到達!");
                    return;
                }
            }
            this.Invoke(new Action(delegate
            {
                this.rJ_TextBox_緊急訂單_下訂單_藥品碼.Texts = 藥品碼;
                this.rJ_TextBox_緊急訂單_下訂單_藥品名稱.Texts = 藥品名稱;
                this.rJ_TextBox_緊急訂單_下訂單_包裝單位.Texts = 包裝單位;
                this.rJ_TextBox_緊急訂單_下訂單_單價.Texts = 契約價金;
                this.rJ_TextBox_緊急訂單_下訂單_前次單價.Texts = 最新訂購單價;

                this.rJ_TextBox_緊急訂單_下訂單_數量.Texts = "";
                this.rJ_TextBox_緊急訂單_下訂單_已訂購總價.Text = 已訂購總價;
                this.rJ_TextBox_緊急訂單_下訂單_已訂購總量.Text = 已訂購總價;

            }));

            List<object[]> list_供應商資料 = this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.SQL_GetAllRows(false);
            list_供應商資料 = list_供應商資料.GetRowsByLike((int)enum_藥品補給系統_供應商資料.簡名, 訂購商);
            this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.RefreshGrid(list_供應商資料);

        }
        private void PlC_RJ_Button_緊急訂單_下訂單_供應商搜尋_填入_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_緊急訂單_下訂單_供應商搜尋.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            string 全名 = list_value[0][(int)enum_藥品補給系統_供應商資料.全名].ObjectToString();
            string Email = list_value[0][(int)enum_藥品補給系統_供應商資料.Email].ObjectToString();
            string 聯絡人 = list_value[0][(int)enum_藥品補給系統_供應商資料.聯絡人].ObjectToString();
            string 電話 = list_value[0][(int)enum_藥品補給系統_供應商資料.電話].ObjectToString();

            List<object[]> list_參數資料 = this.sqL_DataGridView_藥品補給系統_參數資料.SQL_GetAllRows(false);
            list_參數資料 = list_參數資料.GetRows((int)enum_藥品補給系統_參數資料.名稱, enum_藥品補給系統_參數名稱.PLC_驗收期限.GetEnumName());
            this.Invoke(new Action(delegate
            {
                this.PlC_RJ_Button_緊急訂單_下訂單_自動產生訂單編號_MouseDownEvent(null);
                this.rJ_TextBox_緊急訂單_下訂單_全名.Texts = 全名;
                this.rJ_TextBox_緊急訂單_下訂單_Email.Texts = Email;
                this.rJ_TextBox_緊急訂單_下訂單_聯絡人.Texts = 聯絡人;
                this.rJ_TextBox_緊急訂單_下訂單_電話.Texts = 電話;
                this.rJ_TextBox_緊急訂單_下訂單_訂購日期.Texts = DateTime.Now.ToDateString();
                this.rJ_TextBox_緊急訂單_下訂單_訂購人.Texts = this.登入者名稱;
                this.rJ_TextBox_緊急訂單_下訂單_院所名稱.Texts = "屏東榮民總醫院";
                if (list_參數資料.Count > 0)
                {
                    int value = list_參數資料[0][(int)enum_藥品補給系統_參數資料.數值].ObjectToString().StringToInt32();
                    if (value > 0)
                    {
                        value = Function_緊急訂單_下訂單_取得略過假日天數(value, DateTime.Now);
                        this.rJ_TextBox_緊急訂單_下訂單_應驗收日期.Texts = DateTime.Now.AddDays(value).ToDateString();
                    }
                }
            }));


        }
        private void PlC_RJ_Button_緊急訂單_下訂單_自動產生訂單編號_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.rJ_TextBox_緊急訂單_下訂單_訂單編號.Text = "EM" + DateTime.Now.Get_DateTimeTINY().ToString();
            }));
 

        }

        private void PlC_RJ_Button_緊急訂單_下訂單_新增訂單內容_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_訂單內容 = this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.GetAllRows();
            List<object[]> list_訂單內容_buf = new List<object[]>();
            string 藥品碼 = rJ_TextBox_緊急訂單_下訂單_藥品碼.Texts;
            string 藥品名稱 = rJ_TextBox_緊急訂單_下訂單_藥品名稱.Texts;
            string 包裝單位 = rJ_TextBox_緊急訂單_下訂單_包裝單位.Texts;
            string 單價 = rJ_TextBox_緊急訂單_下訂單_單價.Texts;
            string 數量 = rJ_TextBox_緊急訂單_下訂單_數量.Texts;
            string 總價 = rJ_TextBox_緊急訂單_下訂單_總價.Texts;
            string 前次訂購單價 = rJ_TextBox_緊急訂單_下訂單_前次單價.Texts;
            string error_msg = "";
            if (藥品碼.StringIsEmpty()) error_msg += $"◎ '藥品碼'空白\n";
            if (單價.StringIsEmpty()) error_msg += $"◎ '單價'空白\n";
            if (數量.StringIsEmpty()) error_msg += $"◎ '數量'空白\n";
            if (總價.StringIsEmpty()) error_msg += $"◎ '總價'空白\n";
            if(!error_msg.StringIsEmpty())
            {
                MyMessageBox.ShowDialog($"{error_msg}");
                return;
            }
            List<object[]> list_供應商資料 = this.sqL_DataGridView_藥品補給系統_供應商資料.SQL_GetRows(enum_藥品補給系統_供應商資料.全名.GetEnumName(), this.rJ_TextBox_緊急訂單_下訂單_全名.Text, false);
            if (list_供應商資料.Count > 0)
            {
                double 最低訂購金額 = list_供應商資料[0][(int)enum_藥品補給系統_供應商資料.訂單最低總金額].ObjectToString().StringToDouble();
                if (最低訂購金額 > 0)
                {
                    if (總價.StringToDouble() < 最低訂購金額)
                    {
                        MyMessageBox.ShowDialog(string.Format("未超過最低訂購額,還缺少{0},請增加訂購金額!", (最低訂購金額 - 總價.StringToDouble()).ToString("0.00")));
                        return;
                    }
                }
            }
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品補給系統_藥品資料.SQL_GetRows(enum_藥品補給系統_藥品資料.藥品碼.GetEnumName(), this.rJ_TextBox_緊急訂單_下訂單_藥品碼.Text, false);
            if (list_藥品資料.Count > 0)
            {
                int 最小包裝數量 = list_藥品資料[0][(int)enum_藥品補給系統_藥品資料.最小包裝數量].ObjectToString().StringToInt32();
                if (最小包裝數量 > 0)
                {
                    if (數量.StringToInt32() % 最小包裝數量 != 0)
                    {
                        MyMessageBox.ShowDialog(string.Format("藥品最小包裝單位為 {0} ", (最小包裝數量).ToString()));
                        return;
                    }
                }
            }



            list_訂單內容_buf = list_訂單內容.GetRows((int)enum_緊急訂單_下訂單_訂單內容.藥品碼, 藥品碼);

            if(list_訂單內容_buf.Count == 0)
            {
                object[] value = new object[new enum_緊急訂單_下訂單_訂單內容().GetLength()];
                value[(int)enum_緊急訂單_下訂單_訂單內容.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_緊急訂單_下訂單_訂單內容.藥品碼] = 藥品碼;
                value[(int)enum_緊急訂單_下訂單_訂單內容.藥品名稱] = 藥品名稱;
                value[(int)enum_緊急訂單_下訂單_訂單內容.包裝單位] = 包裝單位;
                value[(int)enum_緊急訂單_下訂單_訂單內容.單價] = 單價;
                value[(int)enum_緊急訂單_下訂單_訂單內容.數量] = 數量;
                value[(int)enum_緊急訂單_下訂單_訂單內容.總價] = 總價;
                value[(int)enum_緊急訂單_下訂單_訂單內容.前次訂購單價] = 前次訂購單價;
                this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.AddRow(value , true);
            }
            else
            {
                object[] value = list_訂單內容_buf[0];
                value[(int)enum_緊急訂單_下訂單_訂單內容.藥品碼] = 藥品碼;
                value[(int)enum_緊急訂單_下訂單_訂單內容.藥品名稱] = 藥品名稱;
                value[(int)enum_緊急訂單_下訂單_訂單內容.包裝單位] = 包裝單位;
                value[(int)enum_緊急訂單_下訂單_訂單內容.單價] = 單價;
                value[(int)enum_緊急訂單_下訂單_訂單內容.數量] = 數量;
                value[(int)enum_緊急訂單_下訂單_訂單內容.總價] = 總價;
                value[(int)enum_緊急訂單_下訂單_訂單內容.前次訂購單價] = 前次訂購單價;
                this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.ReplaceExtra(value, true);
            }
            this.Invoke(new Action(delegate
            {
                rJ_TextBox_緊急訂單_下訂單_藥品碼.Texts = "";
                rJ_TextBox_緊急訂單_下訂單_藥品名稱.Texts = "";
                rJ_TextBox_緊急訂單_下訂單_包裝單位.Texts = "";
                rJ_TextBox_緊急訂單_下訂單_單價.Texts = "";
                rJ_TextBox_緊急訂單_下訂單_數量.Texts = "";
                rJ_TextBox_緊急訂單_下訂單_總價.Texts = "";
                rJ_TextBox_緊急訂單_下訂單_前次單價.Texts = "";

            }));
        }
        private void PlC_RJ_Button_緊急訂單_下訂單_刪除訂單內容_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_訂單內容 = this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.Get_All_Select_RowsValues();
            if(list_訂單內容.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            this.sqL_DataGridView_緊急訂單_下訂單_訂單內容.DeleteExtra(list_訂單內容[0], true);
        }
        private void PlC_Button_下訂單_取消作業_btnClick(object sender, EventArgs e)
        {
            if (MyMessageBox.ShowDialog("是否清除此筆訂單內容?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            this.Function_緊急訂單_下訂單_清除全部資料();
        }

        #endregion
    }
}
