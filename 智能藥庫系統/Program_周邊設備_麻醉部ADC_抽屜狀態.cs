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
    public partial class Main_Form : Form
    {
        private List<RJ_Lable[]> rJ_Lables_周邊設備_麻醉部ADC_庫存查詢 = new List<RJ_Lable[]>();
       

        private void sub_Program_周邊設備_麻醉部ADC_庫存查詢_Init()
        {

            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存查詢_API測試.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存查詢_API測試_MouseDownEvent;

            List<RJ_Lable> rJ_Lables_1 = new List<RJ_Lable>();
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_1_1);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_1_2);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_1_3);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_1_4);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_1_5);
            rJ_Lables_1.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_1_6);
            List<RJ_Lable> rJ_Lables_2 = new List<RJ_Lable>();
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_2_1);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_2_2);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_2_3);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_2_4);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_2_5);
            rJ_Lables_2.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_2_6);
            List<RJ_Lable> rJ_Lables_3 = new List<RJ_Lable>();
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_3_1);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_3_2);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_3_3);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_3_4);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_3_5);
            rJ_Lables_3.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_3_6);
            List<RJ_Lable> rJ_Lables_4 = new List<RJ_Lable>();
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_4_1);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_4_2);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_4_3);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_4_4);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_4_5);
            rJ_Lables_4.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_4_6);
            List<RJ_Lable> rJ_Lables_5 = new List<RJ_Lable>();
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_5_1);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_5_2);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_5_3);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_5_4);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_5_5);
            rJ_Lables_5.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_5_6);
            List<RJ_Lable> rJ_Lables_6 = new List<RJ_Lable>();
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_6_1);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_6_2);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_6_3);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_6_4);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_6_5);
            rJ_Lables_6.Add(this.rJ_Lable_周邊設備_麻醉部ADC_庫存查詢_6_6);

            rJ_Lables_周邊設備_麻醉部ADC_庫存查詢.Add(rJ_Lables_1.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_庫存查詢.Add(rJ_Lables_2.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_庫存查詢.Add(rJ_Lables_3.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_庫存查詢.Add(rJ_Lables_4.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_庫存查詢.Add(rJ_Lables_5.ToArray());
            rJ_Lables_周邊設備_麻醉部ADC_庫存查詢.Add(rJ_Lables_6.ToArray());

            this.plC_RJ_Button_周邊設備_麻醉部ADC_庫存查詢_更新資料.MouseDownEvent += PlC_RJ_Button_周邊設備_麻醉部ADC_庫存查詢_更新資料_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_周邊設備_麻醉部ADC_庫存查詢);
        }

   

        private bool flag_Program_周邊設備_麻醉部ADC_庫存查詢_Init = false;
        private void sub_Program_周邊設備_麻醉部ADC_庫存查詢()
        { 
            if (this.plC_ScreenPage_Main.PageText == "周邊設備" && this.plC_ScreenPage_周邊設備.PageText == "麻醉部ADC" && this.plC_ScreenPage_周邊設備_麻醉部ADC.PageText == "庫存查詢")
            {
                if (!flag_Program_周邊設備_麻醉部ADC_庫存查詢_Init)
                {
                    PlC_RJ_Button_周邊設備_麻醉部ADC_庫存查詢_更新資料_MouseDownEvent(null);
                    flag_Program_周邊設備_麻醉部ADC_庫存查詢_Init = true;
                }
            }
            else
            {
                flag_Program_周邊設備_麻醉部ADC_庫存查詢_Init = false;
            }

        }

        #region Function

        #endregion
        #region Event
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存查詢_更新資料_MouseDownEvent(MouseEventArgs mevent)
        {
            PlC_RJ_Button_周邊設備_麻醉部ADC_庫存查詢_API測試_MouseDownEvent(null);
        }
        private void PlC_RJ_Button_周邊設備_麻醉部ADC_庫存查詢_API測試_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string result = Basic.Net.WEBApiGet("http://10.18.28.17/api/medicine_page/storage_list");

            m_returnData m_returnData = result.JsonDeserializet<m_returnData>();
            this.Invoke(new Action(delegate 
            {
                for (int i = 0; i < m_returnData.Data.Count; i++)
                {
                    string text = "";
                    string[] 位置 = m_returnData.Data[i].位置.Split('-');
                    int X = 位置[0].StringToInt32() - 1;
                    int Y = 位置[1].StringToInt32() - 1;
                    text += $"藥碼:{m_returnData.Data[i].藥品碼} 位置:{m_returnData.Data[i].位置}\n";
                    text += $"藥名:{m_returnData.Data[i].藥品名稱}\n";
                    text += $"庫存: [{m_returnData.Data[i].庫存}]\n";
                    text += $"最小單位: {m_returnData.Data[i].最小包裝量}\n";
                    text += $"最大存量: {m_returnData.Data[i].可放置盒數}\n";
                    rJ_Lables_周邊設備_麻醉部ADC_庫存查詢[X][Y].Text = text;
                    rJ_Lables_周邊設備_麻醉部ADC_庫存查詢[X][Y].Font = new Font("微軟正黑體", 12, FontStyle.Bold);

                    if(m_returnData.Data[i].藥品碼.StringIsEmpty())
                    {
                        rJ_Lables_周邊設備_麻醉部ADC_庫存查詢[X][Y].BackgroundColor = Color.Silver;
                        continue;
                    }
                    if (m_returnData.Data[i].庫存.StringToInt32() == 0)
                    {
                        rJ_Lables_周邊設備_麻醉部ADC_庫存查詢[X][Y].BackgroundColor = Color.DarkOrange;
                        continue;
                    }
                    rJ_Lables_周邊設備_麻醉部ADC_庫存查詢[X][Y].BackgroundColor = Color.Green;
                }
                
            }));


            Console.WriteLine(result);
        }
        #endregion


    }
}
