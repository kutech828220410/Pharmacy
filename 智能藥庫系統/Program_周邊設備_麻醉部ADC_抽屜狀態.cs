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
using MyUI;
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using H_Pannel_lib;

namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        private List<RJ_Lable[]> rJ_Lables_周邊設備_麻醉部ADC_抽屜狀態 = new List<RJ_Lable[]>();
        private class returnData
        {
            private string result = "";
            private int code = 0;
            private List<class_儲位總庫存表> data = new List<class_儲位總庫存表>();

            public string Result { get => result; set => result = value; }
            public int Code { get => code; set => code = value; }
            public List<class_儲位總庫存表> Data { get => data; set => data = value; }
        }


        public class class_儲位總庫存表
        {
            [JsonPropertyName("IP")]
            public string IP { get; set; }
            [JsonPropertyName("storage_name")]
            public string 儲位名稱 { get; set; }
            [JsonPropertyName("Code")]
            public string 藥品碼 { get; set; }
            [JsonPropertyName("Neme")]
            public string 藥品名稱 { get; set; }
            [JsonPropertyName("min_package")]
            public string 最小包裝量 { get; set; }
            [JsonPropertyName("package")]
            public string 單位 { get; set; }
            [JsonPropertyName("inventory")]
            public string 庫存 { get; set; }
            [JsonPropertyName("storage_type")]
            public string 儲位型式 { get; set; }
            [JsonPropertyName("max_inventory")]
            public string 可放置盒數 { get; set; }
            [JsonPropertyName("position")]
            public string 位置 { get; set; }

        }

        private void sub_Program_周邊設備_麻醉部ADC_抽屜狀態_Init()
        {

            this.plC_RJ_Button_周邊設備_麻醉部ADC_抽屜狀態_API測試.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_抽屜狀態_API測試_MouseDownEvent;

            List<RJ_Lable> rJ_Lables_1 = new List<RJ_Lable>();
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_1_1);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_1_2);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_1_3);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_1_4);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_1_5);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_1_6);
            List<RJ_Lable> rJ_Lables_2 = new List<RJ_Lable>();
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_2_1);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_2_2);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_2_3);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_2_4);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_2_5);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_2_6);
            List<RJ_Lable> rJ_Lables_3 = new List<RJ_Lable>();
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_3_1);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_3_2);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_3_3);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_3_4);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_3_5);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_3_6);
            List<RJ_Lable> rJ_Lables_4 = new List<RJ_Lable>();
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_4_1);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_4_2);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_4_3);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_4_4);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_4_5);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_4_6);
            List<RJ_Lable> rJ_Lables_5 = new List<RJ_Lable>();
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_5_1);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_5_2);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_5_3);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_5_4);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_5_5);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_5_6);
            List<RJ_Lable> rJ_Lables_6 = new List<RJ_Lable>();
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_6_1);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_6_2);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_6_3);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_6_4);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_6_5);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_抽屜狀態_6_6);

            rJ_Lables_周邊設備_麻醉部ADC_抽屜狀態.Add(rJ_Lables_1.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_抽屜狀態.Add(rJ_Lables_2.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_抽屜狀態.Add(rJ_Lables_3.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_抽屜狀態.Add(rJ_Lables_4.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_抽屜狀態.Add(rJ_Lables_5.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_抽屜狀態.Add(rJ_Lables_6.ToArray());

            this.plC_RJ_Button_周邊設備_麻醉部ADC_抽屜狀態_更新資料.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_抽屜狀態_更新資料_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_周邊設備_麻醉部ADC_抽屜狀態);
        }

   

        private bool flag_Program_周邊設備_麻醉部ADC_抽屜狀態_Init = false;
        private void sub_Program_周邊設備_麻醉部ADC_抽屜狀態()
        { 
            if (this.plC_ScreenPage_Main.PageText == "周邊設備" && this.plC_ScreenPage_周邊設備.PageText == "麻醉部ADC" && this.plC_ScreenPage_周邊設備_麻醉部ADC.PageText == "抽屜狀態")
            {
                if (!flag_Program_周邊設備_麻醉部ADC_抽屜狀態_Init)
                {
                    PlC_RJ_Button_周邊設備_麻醉部ADC_抽屜狀態_更新資料_MouseDownEvent(null);
                    flag_Program_周邊設備_麻醉部ADC_抽屜狀態_Init = true;
                }
            }
            else
            {
                flag_Program_周邊設備_麻醉部ADC_抽屜狀態_Init = false;
            }

        }

        #region Function

        #endregion
        #region Event
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_抽屜狀態_更新資料_MouseDownEvent(MouseEventArgs mevent)
        {
            PlC_RJ_Button_周邊設備_麻醉部ADC_抽屜狀態_API測試_MouseDownEvent(null);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_抽屜狀態_API測試_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string result = Basic.Net.WEBApiGet("http://10.18.28.17/api/medicine_page/storage_list");

            returnData returnData = result.JsonDeserializet<returnData>();
            this.Invoke(new Action(delegate 
            {
                for (int i = 0; i < returnData.Data.Count; i++)
                {
                    string text = "";
                    string[] 位置 = returnData.Data[i].位置.Split('-');
                    int X = 位置[0].StringToInt32() - 1;
                    int Y = 位置[1].StringToInt32() - 1;
                    text += $"藥碼:{returnData.Data[i].藥品碼} 位置:{returnData.Data[i].位置}\n";
                    text += $"藥名:{returnData.Data[i].藥品名稱}\n";
                    text += $"庫存: [{returnData.Data[i].庫存}]\n";
                    text += $"最小單位: {returnData.Data[i].最小包裝量}\n";
                    text += $"可放置盒數: {returnData.Data[i].可放置盒數}\n";
                    rJ_Lables_周邊設備_麻醉部ADC_抽屜狀態[X][Y].Text = text;
                    rJ_Lables_周邊設備_麻醉部ADC_抽屜狀態[X][Y].Font = new Font("微軟正黑體", 12, FontStyle.Bold);
                }
                
            }));


            Console.WriteLine(result);
        }
        #endregion


    }
}
