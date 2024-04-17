using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic;
using MyUI;
using SQLUI;
using HIS_DB_Lib;
using HIS_WebApi;
using MyOffice;


namespace 智能藥庫系統
{
    public partial class Dialog_盤點單合併_設定 : MyDialog
    {
        private List<stockRecord> stockRecords = new List<stockRecord>();
        public stockRecord StockRecord = new stockRecord();
        public DateTime DateTime_st = new DateTime();
        public DateTime DateTime_end = new DateTime();

        public string StockRecord_GUID = "";
        public string StockRecord_ServerName = "";
        public string StockRecord_ServerType = "";
        private List<Panel> panels = new List<Panel>();
        private List<RJ_RatioButton> rJ_RatioButtons = new List<RJ_RatioButton>();
        public Dialog_盤點單合併_設定()
        {
            InitializeComponent();
            Basic.Reflection.MakeDoubleBuffered(this, true);
            this.comboBox_庫別.SelectedIndex = 0;
            this.Load += Dialog_盤點單合併_設定_Load;
            this.LoadFinishedEvent += Dialog_盤點單合併_設定_LoadFinishedEvent;
            this.plC_RJ_Button_確認.MouseDownEvent += PlC_RJ_Button_確認_MouseDownEvent;

            this.rJ_DatePicker_盤點日.ValueChanged += RJ_DatePicker_盤點日_ValueChanged;
            this.comboBox_庫別.SelectedIndexChanged += ComboBox_庫別_SelectedIndexChanged;
        }

        #region Function

        private void Function_RefreshUI(List<stockRecord> stockRecords)
        {
            this.Invoke(new Action(delegate
            {
                this.SuspendLayout();
                panels.Clear();
                this.panel_盤點日選擇_controls.SuspendLayout();
                this.panel_盤點日選擇_controls.Visible = false;
                this.panel_盤點日選擇_controls.Controls.Clear();
         
                for (int i = 0; i < stockRecords.Count; i++)
                {
                    PLC_RJ_Pannel plC_RJ_Pannel_盤點日選擇 = new PLC_RJ_Pannel();
                    RJ_RatioButton rJ_RatioButton_盤點日選擇 = new RJ_RatioButton();
                    plC_RJ_Pannel_盤點日選擇.BackColor = System.Drawing.Color.White;
                    plC_RJ_Pannel_盤點日選擇.BackgroundColor = System.Drawing.Color.Transparent;
                    plC_RJ_Pannel_盤點日選擇.BorderColor = System.Drawing.Color.SkyBlue;
                    plC_RJ_Pannel_盤點日選擇.BorderRadius = 0;
                    plC_RJ_Pannel_盤點日選擇.BorderSize = 0;
                    plC_RJ_Pannel_盤點日選擇.Controls.Add(rJ_RatioButton_盤點日選擇);
                    plC_RJ_Pannel_盤點日選擇.Dock = System.Windows.Forms.DockStyle.Top;
                    plC_RJ_Pannel_盤點日選擇.ForeColor = System.Drawing.Color.White;
                    plC_RJ_Pannel_盤點日選擇.IsSelected = false;
                    plC_RJ_Pannel_盤點日選擇.Location = new System.Drawing.Point(0, 0);
                    plC_RJ_Pannel_盤點日選擇.Name = "plC_RJ_Pannel_盤點日選擇";
                    plC_RJ_Pannel_盤點日選擇.Padding = new System.Windows.Forms.Padding(3);
                    plC_RJ_Pannel_盤點日選擇.ShadowColor = System.Drawing.Color.DimGray;
                    plC_RJ_Pannel_盤點日選擇.ShadowSize = 0;
                    plC_RJ_Pannel_盤點日選擇.Size = new System.Drawing.Size(516, 45);
                    plC_RJ_Pannel_盤點日選擇.TabIndex = 0;
       
                    rJ_RatioButton_盤點日選擇.AutoSize = true;
                    rJ_RatioButton_盤點日選擇.CheckColor = System.Drawing.Color.Green;
                    rJ_RatioButton_盤點日選擇.Dock = System.Windows.Forms.DockStyle.Fill;
                    rJ_RatioButton_盤點日選擇.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    rJ_RatioButton_盤點日選擇.ForeColor = System.Drawing.Color.Black;
                    rJ_RatioButton_盤點日選擇.Location = new System.Drawing.Point(3, 3);
                    rJ_RatioButton_盤點日選擇.MinimumSize = new System.Drawing.Size(0, 21);
                    rJ_RatioButton_盤點日選擇.Name = "rJ_RatioButton_盤點日選擇";
                    rJ_RatioButton_盤點日選擇.Size = new System.Drawing.Size(249, 39);
                    rJ_RatioButton_盤點日選擇.TabIndex = 0;
                    rJ_RatioButton_盤點日選擇.TabStop = true;
                    rJ_RatioButton_盤點日選擇.GUID = $"{stockRecords[i].GUID}";
                    rJ_RatioButton_盤點日選擇.Text = $"({stockRecords[i].庫別}){stockRecords[i].加入時間}";
                    rJ_RatioButton_盤點日選擇.UncheckColor = System.Drawing.Color.Gray;
                    rJ_RatioButton_盤點日選擇.UseVisualStyleBackColor = true;
                    rJ_RatioButton_盤點日選擇.CheckedChanged += RJ_RatioButton_盤點日選擇_CheckedChanged;
                    rJ_RatioButton_盤點日選擇.Click += RJ_RatioButton_盤點日選擇_Click;
                    rJ_RatioButtons.Add(rJ_RatioButton_盤點日選擇);
                    panels.Add(plC_RJ_Pannel_盤點日選擇);
                }
                for (int i = panels.Count - 1; i >= 0; i--)
                {
                    this.panel_盤點日選擇_controls.Controls.Add(panels[i]);
                }
                this.panel_盤點日選擇_controls.AutoScroll = true;
                this.panel_盤點日選擇_controls.ResumeLayout(false);
                this.panel_盤點日選擇_controls.Visible = true;
                //this.panel_controls.Refresh();
                //this.panel_controls.ResumeDrawing();
                this.ResumeLayout(false);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height + 1);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
            }));

        }

  


        #endregion
        #region Event 

        async private void Dialog_盤點單合併_設定_LoadFinishedEvent(EventArgs e)
        {
            this.Refresh();

            await Task.Run(new Action(delegate
            {
                LoadingForm.ShowLoadingFormInvoke();
                stockRecords = stockRecord.POST_get_all_record_simple(Main_Form.API_Server);

                stockRecords = (from temp in stockRecords
                                where temp.加入時間.StringToDateTime().IsInDate(DateTime.Now.GetStartDate(), DateTime.Now.GetEndDate())
                                where temp.庫別 == "藥庫"
                                select temp).ToList();
                Console.WriteLine($"搜尋到<{stockRecords.Count}>筆,庫存紀錄");
                Function_RefreshUI(stockRecords);
                LoadingForm.CloseLoadingFormInvoke();
            }));
        }
        private void RJ_RatioButton_盤點日選擇_Click(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            for (int i = 0; i < rJ_RatioButtons.Count; i++)
            {
                if (radioButton.Text == rJ_RatioButtons[i].Text)
                {
                    rJ_RatioButtons[i].Checked = true;
                }
                else
                {
                    rJ_RatioButtons[i].Checked = false;
                }
            }
        }
        private void RJ_RatioButton_盤點日選擇_CheckedChanged(object sender, EventArgs e)
        {
         
        }
        private void Dialog_盤點單合併_設定_Load(object sender, EventArgs e)
        {
        
        }
        private void RJ_DatePicker_盤點日_ValueChanged(object sender, EventArgs e)
        {
            LoadingForm.ShowLoadingFormInvoke();
            List<stockRecord> stockRecords = stockRecord.POST_get_all_record_simple(Main_Form.API_Server);

            stockRecords = (from temp in stockRecords
                            where temp.加入時間.StringToDateTime().IsInDate(rJ_DatePicker_盤點日.Value.GetStartDate(), rJ_DatePicker_盤點日.Value.GetEndDate())
                            where temp.庫別 == comboBox_庫別.Text
                            select temp).ToList();
            Console.WriteLine($"搜尋到<{stockRecords.Count}>筆,庫存紀錄");
            Function_RefreshUI(stockRecords);
            LoadingForm.CloseLoadingFormInvoke();
        }
        private void ComboBox_庫別_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadingForm.ShowLoadingFormInvoke();
            List<stockRecord> stockRecords = stockRecord.POST_get_all_record_simple(Main_Form.API_Server);

            stockRecords = (from temp in stockRecords
                            where temp.加入時間.StringToDateTime().IsInDate(rJ_DatePicker_盤點日.Value.GetStartDate(), rJ_DatePicker_盤點日.Value.GetEndDate())
                            where temp.庫別 == comboBox_庫別.Text
                            select temp).ToList();
            Console.WriteLine($"搜尋到<{stockRecords.Count}>筆,庫存紀錄");
            Function_RefreshUI(stockRecords);
            LoadingForm.CloseLoadingFormInvoke();
        }
        private void PlC_RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            for (int i = 0; i < rJ_RatioButtons.Count; i++)
            {
                if (rJ_RatioButtons[i].Checked)
                {           
                    this.StockRecord_GUID = rJ_RatioButtons[i].GUID;
                    this.StockRecord_ServerName = "ds01";
                    this.StockRecord_ServerType = "藥庫";
                    List<stockRecord> stockRecords_buf = (from temp in stockRecords
                                                          where temp.GUID == this.StockRecord_GUID
                                                          select temp).ToList();
                    if(stockRecords_buf.Count != 0)
                    {
                        StockRecord = stockRecords_buf[0];
                    }
                }
                DateTime_st = dateTimeIntervelPicker_消耗量起始及結束日期.StartTime;
                DateTime_end = dateTimeIntervelPicker_消耗量起始及結束日期.EndTime;
            }
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        #endregion
        static string RemoveParentheses(string input)
        {
            string pattern = @"^([^\(]+)";
            string result = "";
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(input, pattern);

            if (match.Success)
            {
                result = match.Groups[1].Value;
            }

            return result;
        }
    }
}
