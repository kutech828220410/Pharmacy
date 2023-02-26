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
        private void sub_Program_戰情白板_Init()
        {
            this.plC_RJ_Button_戰情白板_全螢幕顯示.MouseDownEvent += PlC_RJ_Button_戰情白板_全螢幕顯示_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_戰情白板);
        }

        PLC_Device PLC_Device_戰情白板_全螢幕顯示 = new PLC_Device("");

        private bool flag_戰情白板_頁面更新 = false;
        private void sub_Program_戰情白板()
        {
            if (this.plC_ScreenPage_Main.PageText == "戰情白板" && this.plC_ScreenPage_戰情白板.PageText == "顯示畫面")
            {
                if (!this.flag_戰情白板_頁面更新)
                {
                    this.Invoke(new Action(delegate
                    {
                

                    }));
                   
                    this.flag_戰情白板_頁面更新 = true;
                }
                this.sub_Program_戰情白板_檢查離開全螢幕();

                this.Function_戰情白板_繪製時間();
                this.Function_戰情白板_醫院名稱();
                this.Function_戰情白板_標題();
                this.Function_戰情白板_即時公告("");
                this.Function_戰情白板_藥品庫存量及安全量即時資訊();
            }
            else
            {
                this.flag_戰情白板_頁面更新 = false;
            }
        }

        #region PLC_戰情白板_檢查離開全螢幕
        PLC_Device PLC_Device_戰情白板_檢查離開全螢幕 = new PLC_Device("");
        PLC_Device PLC_Device_戰情白板_檢查離開全螢幕_OK = new PLC_Device("");
        MyTimer MyTimer_戰情白板_檢查離開全螢幕_左鍵按下 = new MyTimer();

        MyTimer MyTimer_戰情白板_檢查離開全螢幕_結束延遲 = new MyTimer();
        int cnt_Program_戰情白板_檢查離開全螢幕 = 65534;
        void sub_Program_戰情白板_檢查離開全螢幕()
        {
            PLC_Device_戰情白板_檢查離開全螢幕.Bool = true;
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 65534)
            {
                this.MyTimer_戰情白板_檢查離開全螢幕_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_檢查離開全螢幕.SetComment("PLC_戰情白板_檢查離開全螢幕");
                PLC_Device_戰情白板_檢查離開全螢幕_OK.SetComment("PLC_戰情白板_檢查離開全螢幕_OK");
                PLC_Device_戰情白板_檢查離開全螢幕.Bool = false;
                cnt_Program_戰情白板_檢查離開全螢幕 = 65535;
            }
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 65535) cnt_Program_戰情白板_檢查離開全螢幕 = 1;
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 1) cnt_Program_戰情白板_檢查離開全螢幕_檢查按下(ref cnt_Program_戰情白板_檢查離開全螢幕);
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 2) cnt_Program_戰情白板_檢查離開全螢幕_初始化(ref cnt_Program_戰情白板_檢查離開全螢幕);
            if (cnt_Program_戰情白板_檢查離開全螢幕 == 3) cnt_Program_戰情白板_檢查離開全螢幕 = 65500;
            if (cnt_Program_戰情白板_檢查離開全螢幕 > 1) cnt_Program_戰情白板_檢查離開全螢幕_檢查放開(ref cnt_Program_戰情白板_檢查離開全螢幕);

            if (cnt_Program_戰情白板_檢查離開全螢幕 == 65500)
            {
                this.MyTimer_戰情白板_檢查離開全螢幕_結束延遲.TickStop();
                this.MyTimer_戰情白板_檢查離開全螢幕_結束延遲.StartTickTime(10000);
                PLC_Device_戰情白板_檢查離開全螢幕.Bool = false;
                PLC_Device_戰情白板_檢查離開全螢幕_OK.Bool = false;
                cnt_Program_戰情白板_檢查離開全螢幕 = 65535;
            }
        }
        void cnt_Program_戰情白板_檢查離開全螢幕_檢查按下(ref int cnt)
        {
            if (PLC_Device_戰情白板_檢查離開全螢幕.Bool) cnt++;
        }
        void cnt_Program_戰情白板_檢查離開全螢幕_檢查放開(ref int cnt)
        {
            if (!PLC_Device_戰情白板_檢查離開全螢幕.Bool) cnt = 65500;
        }
        void cnt_Program_戰情白板_檢查離開全螢幕_初始化(ref int cnt)
        {
            if (!PLC_Device_戰情白板_全螢幕顯示.Bool)
            {
                MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.TickStop();
                MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.StartTickTime(2000);
                return;
            }
            if(plC_Button_滑鼠左鍵.Bool)
            {
                if(MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.IsTimeOut())
                {
                    this.Invoke(new Action(delegate
                    {
                        this.panel_戰情白板.Visible = true;
                        this.panel_Main.Visible = true;
                        Basic.Screen.FullScreen(this.FindForm(), 0, false);
                        PLC_Device_戰情白板_全螢幕顯示.Bool = false;
                    }));
                }
            }
            else
            {
                MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.TickStop();
                MyTimer_戰情白板_檢查離開全螢幕_左鍵按下.StartTickTime(2000);
            }
            cnt++;
        
        }



















        #endregion

        #region Function
        private void Function_戰情白板_繪製時間()
        {
            using (Graphics g = this.panel_戰情白板_時間.CreateGraphics())
            {
                using (Bitmap bitmap = new Bitmap(this.panel_戰情白板_時間.Width, this.panel_戰情白板_時間.Height))
                {
                    using(Graphics g_bmp = Graphics.FromImage(bitmap))
                    {
                        Font font = new Font("微軟正黑體", 20, FontStyle.Bold);

                        string str_date = DateTime.Now.ToDateString();
                        string str_time = $"{DateTime.Now.Hour.ToString("00")}:{DateTime.Now.Minute.ToString("00")}:{DateTime.Now.Second.ToString("00")}";
                        Size size_date = TextRenderer.MeasureText(str_date, font);
                        Size size_time = TextRenderer.MeasureText(str_time, font);
                        int x_date = (this.panel_戰情白板_時間.Width - size_date.Width) / 2;
                        int y_date = (this.panel_戰情白板_時間.Height - size_date.Height - size_time.Height) / 2;

                        int x_time = (this.panel_戰情白板_時間.Width - size_time.Width) / 2;
                        int y_time = y_date + size_date.Height;
                        DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.Black, 1, true, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_date, new PointF(x_date, y_date), new Font("微軟正黑體", 20, FontStyle.Bold), Color.Yellow, Color.Black, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_time, new PointF(x_time, y_time), new Font("微軟正黑體", 20, FontStyle.Bold), Color.Yellow, Color.Black, g_bmp, 1, 1);

                        g.DrawImage(bitmap, new PointF(0, 0));
                    }
                }
             

            }
        }
        private void Function_戰情白板_醫院名稱()
        {
            using (Graphics g = this.panel_戰情白板_醫院名稱.CreateGraphics())
            {
                using (Bitmap bitmap = new Bitmap(this.panel_戰情白板_醫院名稱.Width, this.panel_戰情白板_醫院名稱.Height))
                {
                    using (Graphics g_bmp = Graphics.FromImage(bitmap))
                    {
                        Font font_hs_CHT_name = new Font("微軟正黑體", 26, FontStyle.Bold);
                        Font font_hs_EN_name = new Font("微軟正黑體", 16, FontStyle.Bold);

                        string str_hs_CHT_name = "屏東榮民總醫院";
                        Size size_hs_CHT_name = TextRenderer.MeasureText(str_hs_CHT_name, font_hs_CHT_name);

                        string str_hs_EN_name = "Pingtung Veterans General Hospital";
                        Size size_hs_EN_name = TextRenderer.MeasureText(str_hs_EN_name, font_hs_EN_name);

                        int x_hs_CHT_name = (this.panel_戰情白板_醫院名稱.Width - size_hs_EN_name.Width) / 2;
                        int y_hs_CHT_name = (this.panel_戰情白板_時間.Height - size_hs_CHT_name.Height - size_hs_EN_name.Height) / 2;

                        int x_hs_EN_name = x_hs_CHT_name;
                        int y_hs_EN_name = y_hs_CHT_name + size_hs_CHT_name.Height;


                        DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.Black, 1, true, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_hs_CHT_name, bitmap.Width, new PointF(0, y_hs_CHT_name), font_hs_CHT_name, Color.White, g_bmp);
                        DrawingClass.Draw.文字左上繪製(str_hs_EN_name, bitmap.Width, new PointF(0, y_hs_EN_name), font_hs_EN_name, Color.White, g_bmp);



                        g.DrawImage(bitmap, new PointF(0, 0));
                    }
                }


            }
        }
        private void Function_戰情白板_標題()
        {
            using (Graphics g = this.panel_戰情白板_標題.CreateGraphics())
            {
                using (Bitmap bitmap = new Bitmap(this.panel_戰情白板_標題.Width, this.panel_戰情白板_標題.Height))
                {
                    using (Graphics g_bmp = Graphics.FromImage(bitmap))
                    {
                        Font font_hs_CHT_name = new Font("微軟正黑體", 26, FontStyle.Bold);
                        Font font_hs_EN_name = new Font("微軟正黑體", 16, FontStyle.Bold);

                        string str_hs_CHT_name = "藥局戰情整合平台";
                        Size size_hs_CHT_name = TextRenderer.MeasureText(str_hs_CHT_name, font_hs_CHT_name);

                        string str_hs_EN_name = "Pharmacy Information Integration plaform";
                        Size size_hs_EN_name = TextRenderer.MeasureText(str_hs_EN_name, font_hs_EN_name);

                        int x_hs_CHT_name = (this.panel_戰情白板_標題.Width - size_hs_EN_name.Width) / 2;
                        int y_hs_CHT_name = (this.panel_戰情白板_時間.Height - size_hs_CHT_name.Height - size_hs_EN_name.Height) / 2;

                        int x_hs_EN_name = x_hs_CHT_name;
                        int y_hs_EN_name = y_hs_CHT_name + size_hs_CHT_name.Height;


                        DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.Black, 1, true, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_hs_CHT_name, bitmap.Width, new PointF(0, y_hs_CHT_name), font_hs_CHT_name, Color.White, g_bmp);
                        DrawingClass.Draw.文字左上繪製(str_hs_EN_name, bitmap.Width, new PointF(0, y_hs_EN_name), font_hs_EN_name, Color.White, g_bmp);



                        g.DrawImage(bitmap, new PointF(0, 0));
                    }
                }


            }
        }

        private MyTimer MyTimer_戰情白板_即時公告_捲動時間 = new MyTimer();
        int StringCurrent_X;
        private void Function_戰情白板_即時公告(string AlarmString)
        {
            MyTimer_戰情白板_即時公告_捲動時間.StartTickTime(500);
            if (MyTimer_戰情白板_即時公告_捲動時間.IsTimeOut())
            {
                using (Graphics g = this.panel_戰情白板_即時公告.CreateGraphics())
                {
                    using (Bitmap bitmap = new Bitmap(this.panel_戰情白板_即時公告.Width, this.panel_戰情白板_即時公告.Height))
                    {
                        using (Graphics g_bmp = Graphics.FromImage(bitmap))
                        {

                            if(AlarmString.StringIsEmpty())
                            {
                                AlarmString = "無公告";
                            }

                            Font font_AlarmString = new Font("微軟正黑體", 26, FontStyle.Bold);
                            DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.RoyalBlue, 1, true, g_bmp, 1, 1);
                            SizeF SizeOfString = g_bmp.MeasureString(AlarmString, font_AlarmString);
                            int StringEnd_X = (int)(-SizeOfString.Width);
                            float y_name = (this.panel_戰情白板_即時公告.Height - SizeOfString.Height) / 2;
                            if (StringCurrent_X < StringEnd_X) this.StringCurrent_X = bitmap.Width;
                            g_bmp.DrawString(AlarmString, font_AlarmString, new SolidBrush(Color.White), new Point(StringCurrent_X, (int)y_name));
                            StringCurrent_X -= (int)g_bmp.MeasureString("X", font_AlarmString).Width;
                            g.DrawImage(bitmap, new Point(0, 0));
                        }
                    }


                }
            }
            
        }

        private void Function_戰情白板_藥品庫存量及安全量即時資訊()
        {
            using (Graphics g = this.panel_戰情白板_藥品庫存量及安全量即時資訊.CreateGraphics())
            {
                using (Bitmap bitmap = new Bitmap(this.panel_戰情白板_藥品庫存量及安全量即時資訊.Width, this.panel_戰情白板_藥品庫存量及安全量即時資訊.Height))
                {
                    using (Graphics g_bmp = Graphics.FromImage(bitmap))
                    {
                        Font font_hs_CHT_name = new Font("微軟正黑體", 36, FontStyle.Bold);

                        int str_width = 700;
                        string str_hs_CHT_name = "藥品庫存量及安全量即時資訊";
                        Size size_hs_CHT_name = TextRenderer.MeasureText(str_hs_CHT_name, font_hs_CHT_name);

     

                        int x_hs_CHT_name = (this.panel_戰情白板_藥品庫存量及安全量即時資訊.Width - str_width) / 2;
                        int y_hs_CHT_name = (this.panel_戰情白板_時間.Height - size_hs_CHT_name.Height) / 2;

       

                        DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, Color.White, 1, true, g_bmp, 1, 1);
                        DrawingClass.Draw.文字左上繪製(str_hs_CHT_name, str_width, new PointF(x_hs_CHT_name, y_hs_CHT_name), font_hs_CHT_name, Color.Black, g_bmp);



                        g.DrawImage(bitmap, new PointF(0, 0));
                    }
                }


            }
        }
        #endregion

        #region Event
        private void PlC_RJ_Button_戰情白板_全螢幕顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.panel_戰情白板.Visible = false;
                this.panel_Main.Visible = false;
                Basic.Screen.FullScreen(this.FindForm(), 0, true);
                PLC_Device_戰情白板_全螢幕顯示.Bool = true;
            }));
       
        }
        #endregion
    }
}
