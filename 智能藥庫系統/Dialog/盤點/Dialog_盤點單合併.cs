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
            this.plC_RJ_Button_下載報表.MouseDownEvent += PlC_RJ_Button_下載報表_MouseDownEvent;           

            dateTimeIntervelPicker_建表日期.SureClick += DateTimeIntervelPicker_建表日期_SureClick;
            this.comboBox_inv_Combinelist.SelectedIndexChanged += ComboBox_inv_Combinelist_SelectedIndexChanged;
        }



        private void Function_RereshUI(inv_combinelistClass Inv_CombinelistClass)
        {
            this.Inv_CombinelistClass = Inv_CombinelistClass;
            this.Invoke(new Action(delegate
            {
                this.panel_controls.Visible = false;
                this.panel_controls.Controls.Clear();
                this.SuspendLayout();
                panels.Clear();
                this.panel_controls.SuspendLayout();
        
                //if (creats.Count == 0)
                //{
                //    rJ_Lable_warning.Visible = true;
                //}
                //else
                //{
                //    rJ_Lable_warning.Visible = false;
                //}
                for (int i = 0; i < Inv_CombinelistClass.Records_Ary.Count; i++)
                {
                    Panel panel_inv_list = new Panel();
                    RJ_Lable rJ_Lable_list_content = new RJ_Lable();
                    PLC_RJ_Button plC_RJ_Button_delete = new PLC_RJ_Button();



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
                    panel_inv_list.Controls.Add(plC_RJ_Button_delete);
                    panel_inv_list.Controls.Add(rJ_Lable_list_content);
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
                  
                    // 
                    // plC_RJ_Button_delete
                    // 
                    plC_RJ_Button_delete.AutoResetState = false;
                    plC_RJ_Button_delete.BackgroundColor = System.Drawing.Color.Red;
                    plC_RJ_Button_delete.Bool = false;
                    plC_RJ_Button_delete.BorderColor = System.Drawing.Color.DarkRed;
                    plC_RJ_Button_delete.BorderRadius = 10;
                    plC_RJ_Button_delete.BorderSize = 0;
                    plC_RJ_Button_delete.but_press = false;
                    plC_RJ_Button_delete.buttonType = MyUI.RJ_Button.ButtonType.Toggle;
                    plC_RJ_Button_delete.DisenableColor = System.Drawing.Color.Gray;
                    plC_RJ_Button_delete.Dock = System.Windows.Forms.DockStyle.Right;
                    plC_RJ_Button_delete.FlatAppearance.BorderSize = 0;
                    plC_RJ_Button_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    plC_RJ_Button_delete.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    plC_RJ_Button_delete.GUID = $"{Inv_CombinelistClass.Records_Ary[i].單號}";
                    plC_RJ_Button_delete.Icon = System.Windows.Forms.MessageBoxIcon.Warning;
                    plC_RJ_Button_delete.Image_padding = new System.Windows.Forms.Padding(0);
                    plC_RJ_Button_delete.Location = new System.Drawing.Point(1253, 2);
                    plC_RJ_Button_delete.Name = "plC_RJ_Button_delete";
                    plC_RJ_Button_delete.OFF_文字內容 = "刪除";
                    plC_RJ_Button_delete.OFF_文字字體 = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    plC_RJ_Button_delete.OFF_文字顏色 = System.Drawing.Color.White;
                    plC_RJ_Button_delete.OFF_背景顏色 = System.Drawing.Color.Red;
                    plC_RJ_Button_delete.ON_BorderSize = 5;
                    plC_RJ_Button_delete.ON_文字內容 = "刪除";
                    plC_RJ_Button_delete.ON_文字字體 = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
                    plC_RJ_Button_delete.ON_文字顏色 = System.Drawing.Color.White;
                    plC_RJ_Button_delete.ON_背景顏色 = System.Drawing.Color.Red;
                    plC_RJ_Button_delete.ProhibitionBorderLineWidth = 1;
                    plC_RJ_Button_delete.ProhibitionLineWidth = 4;
                    plC_RJ_Button_delete.ProhibitionSymbolSize = 30;
                    plC_RJ_Button_delete.ShadowColor = System.Drawing.Color.DimGray;
                    plC_RJ_Button_delete.ShadowSize = 3;
                    plC_RJ_Button_delete.ShowLoadingForm = false;
                    plC_RJ_Button_delete.Size = new System.Drawing.Size(87, 46);
                    plC_RJ_Button_delete.State = false;
                    plC_RJ_Button_delete.TabIndex = 5;
                    plC_RJ_Button_delete.Text = "刪除";
                    plC_RJ_Button_delete.TextColor = System.Drawing.Color.White;
                    plC_RJ_Button_delete.TextHeight = 0;
                    plC_RJ_Button_delete.Texts = "刪除";
                    plC_RJ_Button_delete.UseVisualStyleBackColor = false;
                    plC_RJ_Button_delete.字型鎖住 = false;
                    plC_RJ_Button_delete.按鈕型態 = MyUI.PLC_RJ_Button.StatusEnum.保持型;
                    plC_RJ_Button_delete.按鍵方式 = MyUI.PLC_RJ_Button.PressEnum.Mouse_左鍵;
                    plC_RJ_Button_delete.文字鎖住 = false;
                    plC_RJ_Button_delete.背景圖片 = null;
                    plC_RJ_Button_delete.讀取位元反向 = false;
                    plC_RJ_Button_delete.讀寫鎖住 = false;
                    plC_RJ_Button_delete.音效 = true;
                    plC_RJ_Button_delete.顯示 = false;
                    plC_RJ_Button_delete.顯示狀態 = false;
                    plC_RJ_Button_delete.MouseDownEventEx += PlC_RJ_Button_delete_MouseDownEventEx; ;
                    panels.Add(panel_inv_list);
                }
                for (int i = panels.Count - 1; i >= 0; i--)
                {
                    this.panel_controls.Controls.Add(panels[i]);
                }
          
                this.panel_controls.Visible = true;        
                this.panel_controls.AutoScroll = true;
                this.panel_controls.ResumeLayout(false);
                this.ResumeLayout(false);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height + 1);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
                //this.panel_controls.Refresh();
                //this.panel_controls.ResumeDrawing();

            }));


        }

    
        #region Event
        private void ComboBox_inv_Combinelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.comboBox_inv_Combinelist.Text;
            if (text.StringIsEmpty()) return;
            text = RemoveParentheses(text);
            inv_combinelistClass inv_CombinelistClass = inv_combinelistClass.get_all_inv(Main_Form.API_Server, text);
            if(inv_CombinelistClass == null)
            {
                Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm($"查無資料[{text}]", 1500);
                dialog_AlarmForm.ShowDialog();
                return;
            }
            Function_RereshUI(inv_CombinelistClass);
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
            Function_RereshUI(inv_CombinelistClass);
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
            comboBox_inv_Combinelist.DataSource = list_str;
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

        }
        private void PlC_RJ_Button_設定_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_盤點單合併_設定 dialog_盤點單合併_設定 = new Dialog_盤點單合併_設定();
            dialog_盤點單合併_設定.ShowDialog();
        }
        private void PlC_RJ_Button_delete_MouseDownEventEx(RJ_Button rJ_Button, MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否刪除?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            string text = "";
            this.Invoke(new Action(delegate { text = this.comboBox_inv_Combinelist.Text; }));
            if (text.StringIsEmpty()) return;
            text = RemoveParentheses(text);
            inv_combinelistClass inv_CombinelistClass = inv_combinelistClass.get_all_inv(Main_Form.API_Server, text);
            if (inv_CombinelistClass == null)
            {
                Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm($"查無資料[{text}]", 1500);
                dialog_AlarmForm.ShowDialog();
                return;
            }
            inv_CombinelistClass.DeleteRecord(rJ_Button.GUID);
            inv_combinelistClass.inv_creat_update(Main_Form.API_Server, inv_CombinelistClass);
            Function_RereshUI(inv_CombinelistClass);

        }
   
        #endregion
        static string RemoveParentheses(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"\([^()]*\)", "");
        }
    }
}
