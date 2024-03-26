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
namespace 智能藥庫系統
{
   
    public partial class Dialog_盤點單管理 : MyDialog
    {
        public static bool IsShown = false;
        private MyThread myThread = new MyThread();
        private List<Panel> panels = new List<Panel>();
        public Dialog_盤點單管理()
        {
            form.Invoke(new Action(delegate { InitializeComponent(); }));
            this.TopLevel = true;
            this.TopMost = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.CanResize = true;
            this.CancelButton = this.plC_RJ_Button_返回;
            this.Load += Dialog_盤點單管理_Load;
            this.FormClosing += Dialog_盤點單管理_FormClosing;
            this.ShowDialogEvent += Dialog_盤點單管理_ShowDialogEvent;

            this.plC_RJ_Button_返回.MouseDownEvent += PlC_RJ_Button_返回_MouseDownEvent;
            this.plC_RJ_Button_刪除.MouseDownEvent += PlC_RJ_Button_刪除_MouseDownEvent;
            this.dateTimeIntervelPicker_建表日期.SureClick += DateTimeIntervelPicker_建表日期_SureClick;
        }


        #region Function
        static public List<inventoryClass.creat> Fuction_取得盤點單(DateTime start, DateTime end)
        {
            List<inventoryClass.creat> creats = HIS_DB_Lib.inventoryClass.creat_get_by_CT_TIME_ST_END(Main_Form.API_Server, start, end);
            if (creats == null)
            {
                Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm("盤點資訊回傳錯誤", 1500);
                dialog_AlarmForm.ShowDialog();
                return null;
            }
            return creats;
        }
        static public returnData Function_刪除盤點單(string IC_SN)
        {
            returnData returnData = HIS_DB_Lib.inventoryClass.creat_delete_by_IC_SN(Main_Form.API_Server, IC_SN);
            return returnData;
        }
        private void Function_RereshUI(List<inventoryClass.creat> creats)
        {
            this.Invoke(new Action(delegate 
            {
                this.SuspendLayout();
                panels.Clear();
                this.panel_controls.SuspendLayout();
                this.panel_controls.Controls.Clear();
                for (int i = 0; i < creats.Count; i++)
                {
       
                    CheckBox checkBox = new CheckBox();
                    checkBox.AutoSize = true;
                    checkBox.Dock = System.Windows.Forms.DockStyle.Left;
                    checkBox.Location = new System.Drawing.Point(2, 2);
                    checkBox.Name = "checkBox";
                    checkBox.Font = new Font("標楷體", 15);
                    checkBox.Size = new System.Drawing.Size(15, 37);
                    checkBox.TabIndex = 0;
                    checkBox.UseVisualStyleBackColor = true;

                    RJ_Lable rJ_Lable_list_state = new RJ_Lable();
                    rJ_Lable_list_state.BackColor = System.Drawing.Color.White;
                    rJ_Lable_list_state.BackgroundColor = System.Drawing.Color.White;
                    rJ_Lable_list_state.BorderColor = System.Drawing.Color.PaleVioletRed;
                    rJ_Lable_list_state.BorderRadius = 10;
                    rJ_Lable_list_state.BorderSize = 0;
                    rJ_Lable_list_state.Dock = System.Windows.Forms.DockStyle.Left;
                    rJ_Lable_list_state.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    rJ_Lable_list_state.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    rJ_Lable_list_state.ForeColor = System.Drawing.Color.Black;
                    rJ_Lable_list_state.GUID = "";
                    rJ_Lable_list_state.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_state.Location = new System.Drawing.Point(17, 2);
                    rJ_Lable_list_state.Name = "rJ_Lable_list_state";
                    rJ_Lable_list_state.ShadowColor = System.Drawing.Color.DimGray;
                    rJ_Lable_list_state.ShadowSize = 0;
                    rJ_Lable_list_state.Size = new System.Drawing.Size(300, 37);
                    rJ_Lable_list_state.TabIndex = 1;
                    rJ_Lable_list_state.Text = $"{i+1}. 名稱 : {creats[i].盤點名稱.StringLength(30)}";
                    rJ_Lable_list_state.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_state.TextColor = System.Drawing.Color.Black;
                    rJ_Lable_list_state.Click += Panel_inv_list_Click;

                    RJ_Lable rJ_Lable_list_content = new RJ_Lable();
                    rJ_Lable_list_content.BackColor = System.Drawing.Color.White;
                    rJ_Lable_list_content.BackgroundColor = System.Drawing.Color.White;
                    rJ_Lable_list_content.BorderColor = System.Drawing.Color.PaleVioletRed;
                    rJ_Lable_list_content.BorderRadius = 10;
                    rJ_Lable_list_content.BorderSize = 0;
                    rJ_Lable_list_content.Dock = System.Windows.Forms.DockStyle.Fill;
                    rJ_Lable_list_content.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    rJ_Lable_list_content.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    rJ_Lable_list_content.ForeColor = System.Drawing.Color.Black;
                    rJ_Lable_list_content.GUID = "";
                    rJ_Lable_list_content.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_content.Location = new System.Drawing.Point(17, 2);
                    rJ_Lable_list_content.Name = "rJ_Lable_list_content";
                    rJ_Lable_list_content.ShadowColor = System.Drawing.Color.DimGray;
                    rJ_Lable_list_content.ShadowSize = 0;
                    rJ_Lable_list_content.Size = new System.Drawing.Size(400, 37);
                    rJ_Lable_list_content.TabIndex = 1;
                    rJ_Lable_list_content.Text = $"建表時間 : {creats[i].建表時間}    預設盤點人 : {(creats[i].預設盤點人.StringIsEmpty() ? "無" : creats[i].預設盤點人)}";
                    rJ_Lable_list_content.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_content.TextColor = System.Drawing.Color.Black;
                    rJ_Lable_list_content.Click += Panel_inv_list_Click;


                    MyUI.PLC_RJ_Button plC_RJ_Button_content = new PLC_RJ_Button();
                    plC_RJ_Button_content.AutoResetState = false;
                    plC_RJ_Button_content.BackgroundColor = System.Drawing.Color.Black;
                    plC_RJ_Button_content.Bool = false;
                    plC_RJ_Button_content.BorderColor = System.Drawing.Color.PaleVioletRed;
                    plC_RJ_Button_content.BorderRadius = 8;
                    plC_RJ_Button_content.BorderSize = 0;
                    plC_RJ_Button_content.but_press = false;
                    plC_RJ_Button_content.buttonType = MyUI.RJ_Button.ButtonType.Toggle;
                    plC_RJ_Button_content.DisenableColor = System.Drawing.Color.Gray;
                    plC_RJ_Button_content.Dock = System.Windows.Forms.DockStyle.Right;
                    plC_RJ_Button_content.FlatAppearance.BorderSize = 0;
                    plC_RJ_Button_content.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    plC_RJ_Button_content.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    plC_RJ_Button_content.GUID = "";
                    plC_RJ_Button_content.Icon = System.Windows.Forms.MessageBoxIcon.Warning;
                    plC_RJ_Button_content.Image_padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    plC_RJ_Button_content.Location = new System.Drawing.Point(916, 2);
                    plC_RJ_Button_content.Name = "plC_RJ_Button_content";
                    plC_RJ_Button_content.OFF_文字內容 = "明細";
                    plC_RJ_Button_content.OFF_文字字體 = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    plC_RJ_Button_content.OFF_文字顏色 = System.Drawing.Color.White;
                    plC_RJ_Button_content.OFF_背景顏色 = System.Drawing.Color.Black;
                    plC_RJ_Button_content.ON_BorderSize = 5;
                    plC_RJ_Button_content.ON_文字內容 = "明細";
                    plC_RJ_Button_content.ON_文字字體 = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold);
                    plC_RJ_Button_content.ON_文字顏色 = System.Drawing.Color.White;
                    plC_RJ_Button_content.ON_背景顏色 = System.Drawing.Color.Black;
                    plC_RJ_Button_content.ProhibitionBorderLineWidth = 1;
                    plC_RJ_Button_content.ProhibitionLineWidth = 4;
                    plC_RJ_Button_content.ProhibitionSymbolSize = 30;
                    plC_RJ_Button_content.ShadowColor = System.Drawing.Color.DimGray;
                    plC_RJ_Button_content.ShadowSize = 3;
                    plC_RJ_Button_content.ShowLoadingForm = false;
                    plC_RJ_Button_content.Size = new System.Drawing.Size(72, 37);
                    plC_RJ_Button_content.State = false;
                    plC_RJ_Button_content.TabIndex = 2;
                    plC_RJ_Button_content.Text = "明細";
                    plC_RJ_Button_content.TextColor = System.Drawing.Color.White;
                    plC_RJ_Button_content.TextHeight = 0;
                    plC_RJ_Button_content.Texts = "明細";
                    plC_RJ_Button_content.UseVisualStyleBackColor = false;
                    plC_RJ_Button_content.字型鎖住 = false;
                    plC_RJ_Button_content.按鈕型態 = MyUI.PLC_RJ_Button.StatusEnum.保持型;
                    plC_RJ_Button_content.按鍵方式 = MyUI.PLC_RJ_Button.PressEnum.Mouse_左鍵;
                    plC_RJ_Button_content.文字鎖住 = false;
                    plC_RJ_Button_content.背景圖片 = null;
                    plC_RJ_Button_content.讀取位元反向 = false;
                    plC_RJ_Button_content.讀寫鎖住 = false;
                    plC_RJ_Button_content.音效 = true;
                    plC_RJ_Button_content.顯示 = false;
                    plC_RJ_Button_content.顯示狀態 = false;

                    Panel panel_inv_list = new Panel();                  
                    panel_inv_list.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;            
                    panel_inv_list.Padding = new System.Windows.Forms.Padding(2);
                    panel_inv_list.Width = this.panel_controls.Width;
                    panel_inv_list.Height = 50;
                    panel_inv_list.Dock = DockStyle.Top;
                    panel_inv_list.Name = $"{creats[i].盤點單號}";
                    panel_inv_list.TabIndex = creats.Count - i;
                    panel_inv_list.Controls.Add(plC_RJ_Button_content);                   
                    panel_inv_list.Controls.Add(rJ_Lable_list_content);
                    panel_inv_list.Controls.Add(rJ_Lable_list_state);
                    panel_inv_list.Controls.Add(checkBox);
                    panels.Add(panel_inv_list);
                }
                for (int i = panels.Count - 1; i >= 0; i--)
                {
                    this.panel_controls.Controls.Add(panels[i]);
                }
                this.panel_controls.AutoScroll = true;
                this.panel_controls.ResumeLayout(false);
                this.panel_controls.Refresh();
                this.panel_controls.ResumeDrawing();
                this.ResumeLayout(false);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height + 1);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
            }));
           
            
        }
        private void Panel_inv_list_Click(object sender, EventArgs e)
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
                        checkBox.Checked = !checkBox.Checked;
                    }
                }
            }
            
        }


        #endregion
        #region Event
        private void Dialog_盤點單管理_ShowDialogEvent()
        {
            if (IsShown)
            {
                MyDialog.BringDialogToFront(this.Text);
                this.DialogResult = DialogResult.Cancel;
            }
        }
        private void Dialog_盤點單管理_Load(object sender, EventArgs e)
        {
            IsShown = true;
        }
        private void Dialog_盤點單管理_FormClosing(object sender, FormClosingEventArgs e)
        {
            myThread.Abort();
            myThread = null;
            IsShown = false;
        }
        private void PlC_RJ_Button_返回_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Close();
            DialogResult = DialogResult.No;
        }
        private void DateTimeIntervelPicker_建表日期_SureClick(object sender, EventArgs e, DateTime start, DateTime end)
        {
            List<inventoryClass.creat> creats = Fuction_取得盤點單(start, end);
            this.rJ_Lable_狀態.Text = $"已搜尋到{creats.Count}筆資料";

            if (creats == null) return;
            Function_RereshUI(creats);
        }
        private void PlC_RJ_Button_刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            List<Panel> panels_checked = panels;
            List<Panel> panels_retmove = new List<Panel>();
            List<string> list_delete_IC_SN = new List<string>();
            for(int i = 0; i < panels_checked.Count; i++)
            {
               foreach(Control control in panels_checked[i].Controls)
                {
                    if(control is CheckBox)
                    {
                        CheckBox checkBox = (CheckBox)control;
                        if (checkBox.Checked)
                        {
                            list_delete_IC_SN.Add(panels_checked[i].Name);
                            panels_retmove.Add(panels_checked[i]);
                        }
                    }
                }
            }
            if(list_delete_IC_SN.Count == 0)
            {
                Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm("未選取資料", 2000);
                dialog_AlarmForm.ShowDialog();
                return;
            }
            LoadingForm.ShowLoadingForm();

            for(int i = 0; i < list_delete_IC_SN.Count; i++)
            {
                string IC_SN = list_delete_IC_SN[i];
                Function_刪除盤點單(IC_SN);
                panels.Remove(panels_checked[i]);
            }
            Function_RereshUI(new List<inventoryClass.creat>());
            LoadingForm.CloseLoadingForm();
        }
        #endregion
    }
}
