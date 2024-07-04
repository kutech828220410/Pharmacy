﻿using System;
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
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using HIS_DB_Lib;
using SQLUI;
using MySql.Data;
using MyOffice;
using MyPrinterlib;

namespace 自動列印
{
    public partial class Main_Form : Form
    {
        static private PrinterClass printerClass = new PrinterClass();
        static private string emg_apply_ApiURL = "https://10.18.1.146:4434/api/excel/emg_apply/";
        static private string API_Server = "http://10.18.1.146:4433";
        public class class_emg_apply
        {
            [JsonPropertyName("cost_center")]
            public string 成本中心 { get; set; }
            [JsonPropertyName("code")]
            public string 藥品碼 { get; set; }
            [JsonPropertyName("name")]
            public string 藥品名稱 { get; set; }
            [JsonPropertyName("inventory")]
            public string 庫存量 { get; set; }
            [JsonPropertyName("value")]
            public string 撥出量 { get; set; }
            [JsonPropertyName("note")]
            public string 備註 { get; set; }
        }
        enum enum_藥庫_撥補_藥局_緊急申領
        {
            GUID,
            藥局代碼,
            藥品碼,
            藥品名稱,
            庫存,
            異動量,
            結存量,
            產出時間,
            過帳時間,
            狀態,
            備註,
        }
        private MyThread myThread_列印;
        public Main_Form()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.button_列印作業開始.Click += Button_列印作業開始_Click;
        }

        private void Button_列印作業開始_Click(object sender, EventArgs e)
        {
            this.button_列印作業開始.Enabled = false;
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            printerClass.Init();
            printerClass.PrintPageEvent += PrinterClass_PrintPageEvent;

            myThread_列印 = new MyThread();
            myThread_列印.SetSleepTime(3000);
            myThread_列印.AutoRun(true);
            myThread_列印.AutoStop(true);
            myThread_列印.Add_Method(sub_program_列印);
            myThread_列印.Trigger();

          
        }

        private void sub_program_列印()
        {
            if (this.button_列印作業開始.Enabled == false)
            {
                Funtion_申領列印();
                if(radioButton_單次列印.Checked)
                {
                    this.Invoke(new Action(delegate 
                    {
                        this.button_列印作業開始.Enabled = true;
                    }));
                }
            }
              
        }

        private void Funtion_申領列印()
        {
            try
            {

                Table table = new Table("emg_application_sd0_opd");
                table.Server = "10.18.1.146";
                table.Username = "user";
                table.Password = "66437068";
                table.DBName = "ds01";
                table.Port = "3306";
                SQLControl sQLControl_緊急申領 = new SQLControl(table);
                Logger.LogAddLine();
                List<object[]> list_緊急申領 = sQLControl_緊急申領.GetRowsByBetween(null, (int)enum_藥庫_撥補_藥局_緊急申領.產出時間, DateTime.Now.GetStartDate().ToDateTimeString(), DateTime.Now.GetEndDate().ToDateTimeString());
                Logger.Log($"共取得申領資料共<{list_緊急申領.Count}>筆 ({DateTime.Now.GetStartDate().ToDateTimeString()}~{ DateTime.Now.GetEndDate().ToDateTimeString()})");
                this.Invoke(new Action(delegate
                {
                    this.label_申領_狀態.Text = $"共取得申領資料共<{list_緊急申領.Count}>筆 ({DateTime.Now.GetStartDate().ToDateTimeString()}~{ DateTime.Now.GetEndDate().ToDateTimeString()})";
                }));
                list_緊急申領 = (from temp in list_緊急申領
                             where temp[(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() == "等待過帳"
                             select temp).ToList();
                Logger.Log($"其中[等待過帳]申領資料共<{list_緊急申領.Count}>筆 ({DateTime.Now.GetStartDate().ToDateTimeString()}~{ DateTime.Now.GetEndDate().ToDateTimeString()})");
                this.Invoke(new Action(delegate
                {
                    this.label_申領_狀態.Text = $"其中[等待過帳]申領資料共<{list_緊急申領.Count}>筆 ({DateTime.Now.GetStartDate().ToDateTimeString()}~{ DateTime.Now.GetEndDate().ToDateTimeString()})";
                }));
                List<class_emg_apply> class_Emg_Applies = new List<class_emg_apply>();
                for (int i = 0; i < list_緊急申領.Count; i++)
                {
                    class_emg_apply class_Emg_Apply = new class_emg_apply();
                    class_Emg_Apply.成本中心 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥局代碼].ObjectToString();
                    class_Emg_Apply.藥品碼 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString();
                    class_Emg_Apply.藥品名稱 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品名稱].ObjectToString();
                    class_Emg_Apply.撥出量 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.異動量].ObjectToString();
                    class_Emg_Apply.庫存量 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.庫存].ObjectToString();
                    class_Emg_Apply.備註 = list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.備註].ObjectToString();
                    class_Emg_Applies.Add(class_Emg_Apply);
                }

                string json_in = class_Emg_Applies.JsonSerializationt(true);
                string json = Basic.Net.WEBApiPostJson($"{emg_apply_ApiURL}", json_in);
                List<SheetClass> sheetClass = json.JsonDeserializet<List<SheetClass>>();

                Logger.Log($"取得資料表共<{sheetClass.Count}>張");
                if(sheetClass.Count > 0)
                {
                    printerClass.Print(sheetClass, PrinterClass.PageSize.A4);


                }
                for (int i = 0; i < list_緊急申領.Count; i++)
                {
                    if (list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態].ObjectToString() == "過帳完成") continue;
                    list_緊急申領[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = "已列印";
                }
                sQLControl_緊急申領.UpdateByDefulteExtra(null, list_緊急申領);
                Logger.Log($"列印完成");
                this.Invoke(new Action(delegate
                {
                    this.label_申領_狀態.Text = $"列印完成,共<{sheetClass.Count}>張";
                }));

            }
            catch (Exception ex)
            {
                Logger.Log($"Exception : {ex.Message}");
            }
            finally
            {
                Logger.LogAddLine();
            }
        }
        private void PrinterClass_PrintPageEvent(object sender, Graphics g, int width, int height, int page_num)
        {
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            using (Bitmap bitmap = printerClass.GetSheetClass(page_num).GetBitmap(width, height, 0.75, H_Alignment.Center, V_Alignment.Top, 0, 50))
            {
                rectangle.Height = bitmap.Height;
                g.DrawImage(bitmap, rectangle);
            }
        }
    }
}
