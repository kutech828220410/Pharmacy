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
    public partial class Dialog_盤點單合併 : MyDialog
    {
        private List<Panel> panels = new List<Panel>();
        public inv_combinelistClass Inv_CombinelistClass = new inv_combinelistClass();
        public stockRecord StockRecord = new stockRecord();
        public DateTime DateTime_st = new DateTime();
        public DateTime DateTime_end = new DateTime();

        public static bool IsShown = false;
        static public Dialog_盤點單合併 myDialog;
        static public Dialog_盤點單合併 GetForm()
        {
            if(myDialog != null)
            {
                return myDialog;
            }
            else
            {
                myDialog  = new Dialog_盤點單合併();
                return myDialog;
            }
        }
        public Dialog_盤點單合併()
        {
            InitializeComponent();

            this.TopLevel = true;
            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = true;
            this.CanResize = true;
            this.Load += Dialog_盤點單合併_Load;
            this.ShowDialogEvent += Dialog_盤點單合併_ShowDialogEvent;
            this.FormClosing += Dialog_盤點單合併_FormClosing;
            this.plC_RJ_Button_返回.MouseDownEvent += PlC_RJ_Button_返回_MouseDownEvent;
            this.plC_RJ_Button_新建.MouseDownEvent += PlC_RJ_Button_新建_MouseDownEvent;
            this.plC_RJ_Button_設定.MouseDownEvent += PlC_RJ_Button_設定_MouseDownEvent;
            this.plC_RJ_Button_選擇.MouseDownEvent += PlC_RJ_Button_選擇_MouseDownEvent;
            this.plC_RJ_Button_刪除.MouseDownEvent += PlC_RJ_Button_刪除_MouseDownEvent;
            this.plC_RJ_Button_下載報表.MouseDownEvent += PlC_RJ_Button_下載報表_MouseDownEvent;
            this.plC_RJ_Button_刪除選取資料.MouseDownEvent += PlC_RJ_Button_刪除選取資料_MouseDownEvent;

            dateTimeIntervelPicker_建表日期.SureClick += DateTimeIntervelPicker_建表日期_SureClick;
            this.comboBox_inv_Combinelist.SelectedIndexChanged += ComboBox_inv_Combinelist_SelectedIndexChanged;
        }

        private void Function_RereshUI()
        {
            string text = "";
            this.Invoke(new Action(delegate { text = this.comboBox_inv_Combinelist.Text; }));
            if (text.StringIsEmpty()) return;
            text = RemoveParentheses(text);

            inv_combinelistClass inv_CombinelistClass = inv_combinelistClass.get_all_inv(Main_Form.API_Server, text);
            DateTime_st = inv_CombinelistClass.消耗量起始時間.StringToDateTime();
            DateTime_end = inv_CombinelistClass.消耗量結束時間.StringToDateTime();
            if (inv_CombinelistClass == null)
            {
                return;
            }
            stockRecord stockRecord = stockRecord.POST_get_record_by_guid(Main_Form.API_Server, inv_CombinelistClass.StockRecord_GUID, inv_CombinelistClass.StockRecord_ServerName, inv_CombinelistClass.StockRecord_ServerType);

            if (inv_CombinelistClass == null)
            {
                Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm($"查無資料[{text}]", 1500);
                dialog_AlarmForm.ShowDialog();
                return;
            }
   
            
            Function_RereshUI(inv_CombinelistClass, stockRecord);
        }
        private void Function_RereshUI(inv_combinelistClass Inv_CombinelistClass , stockRecord stockRecord)
        {
            this.Inv_CombinelistClass = Inv_CombinelistClass;
            this.StockRecord = stockRecord;
            this.Invoke(new Action(delegate
            {
                this.panel_controls.Visible = false;
                this.panel_controls.Controls.Clear();
                this.SuspendLayout();
                panels.Clear();
                this.panel_controls.SuspendLayout();

                for (int i = 0; i < Inv_CombinelistClass.Records_Ary.Count; i++)
                {
                    Panel panel_inv_list = new Panel();
                    RJ_Lable rJ_Lable_list_content = new RJ_Lable();
                    CheckBox checkBox = new CheckBox();

                    checkBox.AutoSize = true;
                    checkBox.Dock = System.Windows.Forms.DockStyle.Left;
                    checkBox.Location = new System.Drawing.Point(2, 2);
                    checkBox.Name = "checkBox";
                    checkBox.Size = new System.Drawing.Size(15, 46);
                    checkBox.TabIndex = 11;
                    checkBox.UseVisualStyleBackColor = true;
                    // 
                    // panel_inv_list
                    // 
                    panel_inv_list.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    panel_inv_list.Padding = new System.Windows.Forms.Padding(2);
                    panel_inv_list.Width = this.panel_controls.Width;
                    panel_inv_list.Height = 50;
                    panel_inv_list.Dock = DockStyle.Top;
                    panel_inv_list.Name = $"{Inv_CombinelistClass.Records_Ary[i].單號}";
                    panel_inv_list.TabIndex = Inv_CombinelistClass.Records_Ary.Count - i;
                    panel_inv_list.Controls.Add(rJ_Lable_list_content);
                    panel_inv_list.Controls.Add(checkBox);
                    rJ_Lable_list_content.BackColor = System.Drawing.Color.White;
                    rJ_Lable_list_content.BackgroundColor = System.Drawing.Color.White;
                    rJ_Lable_list_content.BorderColor = System.Drawing.Color.White;
                    rJ_Lable_list_content.BorderRadius = 0;
                    rJ_Lable_list_content.BorderSize = 0;
                    rJ_Lable_list_content.Dock = System.Windows.Forms.DockStyle.Fill;
                    rJ_Lable_list_content.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    rJ_Lable_list_content.ForeColor = System.Drawing.Color.White;
                    rJ_Lable_list_content.GUID = "";
                    rJ_Lable_list_content.Text = $"{i+1} ({Inv_CombinelistClass.Records_Ary[i].單號}) {Inv_CombinelistClass.Records_Ary[i].名稱}";
                    rJ_Lable_list_content.Font = new Font("微軟正黑體", 14, FontStyle.Bold);
                    rJ_Lable_list_content.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_content.Location = new System.Drawing.Point(302, 2);
                    rJ_Lable_list_content.Name = "rJ_Lable_list_content";
                    rJ_Lable_list_content.ShadowColor = System.Drawing.Color.DimGray;
                    rJ_Lable_list_content.ShadowSize = 0;
                    rJ_Lable_list_content.Size = new System.Drawing.Size(951, 46);
                    rJ_Lable_list_content.TabIndex = 10;
                    rJ_Lable_list_content.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_content.TextColor = System.Drawing.Color.Black;
                    rJ_Lable_list_content.Click += RJ_Lable_list_content_Click;


                    panels.Add(panel_inv_list);
                }
                for (int i = panels.Count - 1; i >= 0; i--)
                {
                    this.panel_controls.Controls.Add(panels[i]);
                }
                if (this.StockRecord == null)
                {
                    this.label_盤點單設定.Text = $"庫存截點 : ------------    消耗量日期 : ----------------------";
                }
                else
                {
                    this.label_盤點單設定.Text = $"庫存截點 : {this.StockRecord.加入時間}    消耗量日期 : {this.DateTime_st.ToDateTimeString()} - {this.DateTime_end.ToDateTimeString()}";
                }
                this.panel_controls.Visible = true;        
                this.panel_controls.AutoScroll = true;
                this.panel_controls.ResumeLayout(false);
                this.ResumeLayout(false);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height + 1);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);

            }));


        }
      
        #region Event

        private void ComboBox_inv_Combinelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            Function_RereshUI();
        }
        private void PlC_RJ_Button_下載報表_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                string text = "";
                text = this.comboBox_inv_Combinelist.Text;
                text = RemoveParentheses(text);
                if (this.saveFileDialog_SaveExcel.ShowDialog() == DialogResult.OK)
                {
                    string fileName = this.saveFileDialog_SaveExcel.FileName;
                    LoadingForm.ShowLoadingForm();
                    byte[] bytes = inv_combinelistClass.get_full_inv_Excel_by_SN(Main_Form.API_Server, text , "料號");
                    bytes.SaveFileStream(fileName);
                    LoadingForm.CloseLoadingForm();
                    Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm("匯出完成", 1000, Color.Green);
                    dialog_AlarmForm.ShowDialog();
                }
            }));
        
        }
        private void PlC_RJ_Button_選擇_MouseDownEvent(MouseEventArgs mevent)
        {

            string text = "";
            this.Invoke(new Action(delegate { text = this.comboBox_inv_Combinelist.Text; }));
            if (text.StringIsEmpty()) return;
            text = RemoveParentheses(text);
            Dialog_盤點單合併_選擇 dialog_盤點單合併_選擇 = new Dialog_盤點單合併_選擇();
            dialog_盤點單合併_選擇.ShowDialog();
            inv_combinelistClass inv_CombinelistClass = inv_combinelistClass.get_all_inv(Main_Form.API_Server, text);
            if (inv_CombinelistClass == null)
            {
                Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm($"查無資料[{text}]", 1500);
                dialog_AlarmForm.ShowDialog();
                return;
            }
            List<inventoryClass.creat> creats = dialog_盤點單合併_選擇.Creats;
            for (int i = 0; i < creats.Count; i++)
            {
                inv_CombinelistClass.AddRecord(creats[i]);
            }
            inv_combinelistClass.inv_creat_update(Main_Form.API_Server, inv_CombinelistClass);
            Function_RereshUI();
        }
        private void Dialog_盤點單合併_FormClosing(object sender, FormClosingEventArgs e)
        {
            myDialog = null;
            IsShown = false;
        }
        private void Dialog_盤點單合併_ShowDialogEvent()
        {
            if (IsShown)
            {
                this.Invoke(new Action(delegate 
                {
                    if (this.FormBorderStyle == FormBorderStyle.None)
                    {
                        myDialog.WindowState = FormWindowState.Normal;
                        //myDialog.Show();
                        myDialog.BringToFront();
                    }  
                    this.DialogResult = DialogResult.Cancel;
                }));
     
            }
        }
        private void Dialog_盤點單合併_Load(object sender, EventArgs e)
        {
            dateTimeIntervelPicker_建表日期.StartTime = DateTime.Now.GetStartDate().AddMonths(-1);
            dateTimeIntervelPicker_建表日期.EndTime = DateTime.Now.GetEndDate().AddMonths(0);
            dateTimeIntervelPicker_建表日期.OnSureClick();
            IsShown = true;
          
        }
        private void DateTimeIntervelPicker_建表日期_SureClick(object sender, EventArgs e, DateTime start, DateTime end)
        {
            DateTime dateTime_st = dateTimeIntervelPicker_建表日期.StartTime;
            DateTime dateTime_end = dateTimeIntervelPicker_建表日期.EndTime;
            List<inv_combinelistClass> inv_CombinelistClasses = inv_combinelistClass.get_all_inv($"{Main_Form.API_Server}", dateTime_st, dateTime_end);
            List<string> list_str = new List<string>();
            for (int i = 0; i < inv_CombinelistClasses.Count; i++)
            {
                string str = $"{inv_CombinelistClasses[i].合併單號}({inv_CombinelistClasses[i].合併單名稱})";
                list_str.Add(str);
            }
            this.Invoke(new Action(delegate 
            {
                comboBox_inv_Combinelist.Items.Clear();
                for (int i = 0; i < list_str.Count; i++)
                {
                    comboBox_inv_Combinelist.Items.Add(list_str[i]);
                }
                comboBox_inv_Combinelist.SelectedIndex = 0;
            }));
        
        }
        private void PlC_RJ_Button_返回_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Close();
            DialogResult = DialogResult.No;
        }
        private void PlC_RJ_Button_新建_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_AlarmForm dialog_AlarmForm;
            Dialog_新建合併盤點單 dialog_新建合併盤點單 = new Dialog_新建合併盤點單();
            dialog_新建合併盤點單.ShowDialog();
            if (dialog_新建合併盤點單.DialogResult != DialogResult.Yes) return;

            string name = dialog_新建合併盤點單.Value;
            inv_combinelistClass inv_CombinelistClass = new inv_combinelistClass();
            inv_CombinelistClass.合併單名稱 = name;
            inv_CombinelistClass = inv_combinelistClass.inv_creat_update(Main_Form.API_Server, inv_CombinelistClass);
            if(inv_CombinelistClass == null)
            {
                dialog_AlarmForm = new Dialog_AlarmForm("新建失敗", 1500);
                dialog_AlarmForm.ShowDialog();
            }
            dialog_AlarmForm = new Dialog_AlarmForm("新建成功", 1500, Color.Green);
            dialog_AlarmForm.ShowDialog();
            dateTimeIntervelPicker_建表日期.OnSureClick();

        }
        private void PlC_RJ_Button_設定_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_盤點單合併_設定 dialog_盤點單合併_設定 = new Dialog_盤點單合併_設定();
            if (dialog_盤點單合併_設定.ShowDialog() != DialogResult.Yes) return;
            this.StockRecord = dialog_盤點單合併_設定.StockRecord;
            inv_combinelistClass.inv_stockrecord_update_by_GUID(Main_Form.API_Server, Inv_CombinelistClass.GUID, dialog_盤點單合併_設定.StockRecord_GUID, dialog_盤點單合併_設定.StockRecord_ServerName, dialog_盤點單合併_設定.StockRecord_ServerType);
            DateTime_st = dialog_盤點單合併_設定.DateTime_st;
            DateTime_end = dialog_盤點單合併_設定.DateTime_end;
            inv_combinelistClass.inv_consumption_time_update_by_GUID(Main_Form.API_Server, Inv_CombinelistClass.GUID, dialog_盤點單合併_設定.DateTime_st, dialog_盤點單合併_設定.DateTime_end);
            Function_RereshUI();
        }
        private void PlC_RJ_Button_刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("確認刪除?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            string text = "";
            this.Invoke(new Action(delegate { text = this.comboBox_inv_Combinelist.Text; }));
            if (text.StringIsEmpty()) return;
            text = RemoveParentheses(text);
            inv_combinelistClass.inv_delete_by_SN(Main_Form.API_Server, text);
            dateTimeIntervelPicker_建表日期.OnSureClick();

        }
        private void PlC_RJ_Button_刪除選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<string> list_delete_IC_SN = new List<string>();
            List<Panel> panels_retmove = new List<Panel>();
            for (int i = 0; i < panels.Count; i++)
            {
                foreach (Control control in panels[i].Controls)
                {
                    if (control is CheckBox)
                    {
                        CheckBox checkBox = (CheckBox)control;
                        if (checkBox.Checked)
                        {
                            list_delete_IC_SN.Add(panels[i].Name);
                            panels_retmove.Add(panels[i]);
                        }
                    }
                }
            }
            if (list_delete_IC_SN.Count == 0)
            {
                Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm("未選取資料", 2000);
                dialog_AlarmForm.ShowDialog();
                return;
            }
            if (MyMessageBox.ShowDialog("確認刪除?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            LoadingForm.ShowLoadingForm();

            for (int i = 0; i < list_delete_IC_SN.Count; i++)
            {
                string IC_SN = list_delete_IC_SN[i];
                Inv_CombinelistClass.DeleteRecord(IC_SN);
                inv_combinelistClass.inv_creat_update(Main_Form.API_Server, Inv_CombinelistClass);        
            }

            Function_RereshUI();
            LoadingForm.CloseLoadingForm();
        }
        private void RJ_Lable_list_content_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            if (control.Parent is Panel)
            {
                Panel panel = (Panel)control.Parent;
                for (int i = 0; i < panel.Controls.Count; i++)
                {
                    if (panel.Controls[i].Name == "checkBox")
                    {
                        CheckBox checkBox = (CheckBox)panel.Controls[i];
                        if (e != null) checkBox.Checked = !checkBox.Checked;

                        if (checkBox.Checked)
                        {
                            for (int k = 0; k < panel.Controls.Count; k++)
                            {
                                if (panel.Controls[k] is RJ_Lable) ((RJ_Lable)panel.Controls[k]).BackgroundColor = Color.YellowGreen;
                            }
                        }
                        else
                        {
                            for (int k = 0; k < panel.Controls.Count; k++)
                            {
                                if (panel.Controls[k] is RJ_Lable) ((RJ_Lable)panel.Controls[k]).BackgroundColor = Color.White;
                            }
                        }
                    }
                }
            }
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
