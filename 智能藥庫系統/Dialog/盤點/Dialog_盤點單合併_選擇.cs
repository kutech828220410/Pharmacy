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
    public partial class Dialog_盤點單合併_選擇 : MyDialog
    {
        public List<inventoryClass.creat> Creats = new List<inventoryClass.creat>();
        private List<Panel> panels = new List<Panel>();
        private List<inventoryClass.creat> _creats = new List<inventoryClass.creat>();
        public Dialog_盤點單合併_選擇()
        {
            InitializeComponent();
            this.plC_RJ_Button_確認.MouseDownEvent += PlC_RJ_Button_確認_MouseDownEvent;
            this.plC_RJ_Button_返回.MouseDownEvent += PlC_RJ_Button_返回_MouseDownEvent;
            this.dateTimeIntervelPicker_建表日期.SureClick += DateTimeIntervelPicker_建表日期_SureClick;
            this.Load += Dialog_盤點單合併_選擇_Load;

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
        private void Function_RereshUI(List<inventoryClass.creat> creats)
        {
            this._creats = creats;
            this.Invoke(new Action(delegate
            {
                this.SuspendLayout();
                panels.Clear();
                this.panel_controls.SuspendLayout();
                this.panel_controls.Visible = false;
                this.panel_controls.Controls.Clear();
                if (creats.Count == 0)
                {
                    rJ_Lable_warning.Visible = true;
                }
                else
                {
                    rJ_Lable_warning.Visible = false;
                }
                for (int i = 0; i < creats.Count; i++)
                {
                    RJ_Pannel panel_inv_list = new RJ_Pannel();
                    RJ_Lable rJ_Lable_list_content = new RJ_Lable();
                    RJ_Lable rJ_Lable_list_state = new RJ_Lable();



                    CheckBox checkBox = new CheckBox();
                    // 
                    // panel_inv_list
                    // 
                    panel_inv_list.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    panel_inv_list.Padding = new System.Windows.Forms.Padding(2);
                    panel_inv_list.Width = this.panel_controls.Width;
                    panel_inv_list.Height = 50;
                    panel_inv_list.Dock = DockStyle.Top;
                    panel_inv_list.Name = $"{creats[i].盤點單號}";
                    panel_inv_list.TabIndex = creats.Count - i;
                    panel_inv_list.BorderRadius = 0;
                    panel_inv_list.BorderColor = Color.White;
                    panel_inv_list.BorderSize = 0;
                    panel_inv_list.Controls.Add(rJ_Lable_list_content);
                    panel_inv_list.Controls.Add(rJ_Lable_list_state);
                    panel_inv_list.Controls.Add(checkBox);
                    // 
                    // checkBox
                    // 
                    checkBox.AutoSize = true;
                    checkBox.Dock = System.Windows.Forms.DockStyle.Left;
                    checkBox.Location = new System.Drawing.Point(2, 2);
                    checkBox.Name = $"checkBox";
                    checkBox.Size = new System.Drawing.Size(15, 46);
                    checkBox.TabIndex = 0;
                    checkBox.UseVisualStyleBackColor = true;
                    checkBox.Click += CheckBox_Click;
                    // 
                    // rJ_Lable_list_content
                    // 
                    rJ_Lable_list_content.BackColor = System.Drawing.Color.White;
                    rJ_Lable_list_content.BackgroundColor = System.Drawing.Color.White;
                    rJ_Lable_list_content.BorderColor = System.Drawing.Color.PaleVioletRed;
                    rJ_Lable_list_content.BorderRadius = 0;
                    rJ_Lable_list_content.BorderSize = 0;
                    rJ_Lable_list_content.Dock = System.Windows.Forms.DockStyle.Fill;
                    rJ_Lable_list_content.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    rJ_Lable_list_content.ForeColor = System.Drawing.Color.White;
                    rJ_Lable_list_content.GUID = "";
                    rJ_Lable_list_content.Font = new Font("微軟正黑體", 12, FontStyle.Bold);
                    rJ_Lable_list_content.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_content.Location = new System.Drawing.Point(317, 2);
                    rJ_Lable_list_content.Name = "rJ_Lable_list_content";
                    rJ_Lable_list_content.ShadowColor = System.Drawing.Color.DimGray;
                    rJ_Lable_list_content.ShadowSize = 0;
                    rJ_Lable_list_content.Size = new System.Drawing.Size(843, 46);
                    rJ_Lable_list_content.TabIndex = 8;
                    rJ_Lable_list_content.Text = $"建表時間 : {creats[i].建表時間}    預設盤點人 : {(creats[i].預設盤點人.StringIsEmpty() ? "無" : creats[i].預設盤點人)}";
                    rJ_Lable_list_content.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_content.TextColor = System.Drawing.Color.Black;
                    rJ_Lable_list_content.Click += Panel_inv_list_Click;

              
                    // 
                    // rJ_Lable_list_state
                    // 
                    rJ_Lable_list_state.BackColor = System.Drawing.Color.White;
                    rJ_Lable_list_state.BackgroundColor = System.Drawing.Color.White;
                    rJ_Lable_list_state.BorderColor = System.Drawing.Color.PaleVioletRed;
                    rJ_Lable_list_state.BorderRadius = 0;
                    rJ_Lable_list_state.BorderSize = 0;
                    rJ_Lable_list_state.Dock = System.Windows.Forms.DockStyle.Left;
                    rJ_Lable_list_state.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    rJ_Lable_list_state.ForeColor = System.Drawing.Color.White;
                    rJ_Lable_list_state.GUID = "";
                    rJ_Lable_list_state.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_state.Location = new System.Drawing.Point(17, 2);
                    rJ_Lable_list_state.Name = "rJ_Lable_list_state";
                    rJ_Lable_list_state.Font = new Font("微軟正黑體", 12, FontStyle.Bold);
                    rJ_Lable_list_state.ShadowColor = System.Drawing.Color.DimGray;
                    rJ_Lable_list_state.ShadowSize = 0;
                    rJ_Lable_list_state.Size = new System.Drawing.Size(300, 46);
                    rJ_Lable_list_state.TabIndex = 6;
                    rJ_Lable_list_state.Text = $"{i + 1}. 名稱 : {creats[i].盤點名稱.StringLength(30)}";
                    rJ_Lable_list_state.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_list_state.TextColor = System.Drawing.Color.Black;
                    rJ_Lable_list_state.Click += Panel_inv_list_Click;

                
                    panels.Add(panel_inv_list);
                }
                for (int i = panels.Count - 1; i >= 0; i--)
                {
                    this.panel_controls.Controls.Add(panels[i]);
                }
                this.panel_controls.AutoScroll = true;
                this.panel_controls.ResumeLayout(false);
                this.panel_controls.Visible = true;
                //this.panel_controls.Refresh();
                //this.panel_controls.ResumeDrawing();
                this.ResumeLayout(false);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height + 1);
                this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
            }));


        }

        #endregion
        #region Event
        private void Dialog_盤點單合併_選擇_Load(object sender, EventArgs e)
        {
            this.dateTimeIntervelPicker_建表日期.StartTime = DateTime.Now.AddMonths(-1).GetStartDate();
            this.dateTimeIntervelPicker_建表日期.EndTime = DateTime.Now.AddMonths(0).GetEndDate();
            this.dateTimeIntervelPicker_建表日期.OnSureClick();
        }
        private void DateTimeIntervelPicker_建表日期_SureClick(object sender, EventArgs e, DateTime start, DateTime end)
        {
            List<inventoryClass.creat> creats = Fuction_取得盤點單(start, end);
            this.rJ_Lable_狀態.Text = $"已搜尋到{creats.Count}筆資料";
         
            if (creats == null) return;
            Function_RereshUI(creats);
        }
        private void PlC_RJ_Button_返回_MouseDownEvent(MouseEventArgs mevent)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        private void PlC_RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            for (int i = 0; i < panel_controls.Controls.Count; i++)
            {
                if(panel_controls.Controls[i] is Panel)
                {
                    string IC_SN = panel_controls.Controls[i].Name;
                    Panel panel = (Panel)panel_controls.Controls[i];
                    for (int k = 0; k < panel.Controls.Count; k++)
                    {
                        if (panel.Controls[k].Name == "checkBox")
                        {
                            if(panel.Controls[k] is CheckBox)
                            {
                                CheckBox checkBox = (CheckBox)panel.Controls[k];
                                if (checkBox.Checked)
                                {
                                    List<inventoryClass.creat> creats_buf = (from temp in this._creats
                                                                             where temp.盤點單號 == IC_SN
                                                                             select temp).ToList();
                                    if (creats_buf.Count > 0)
                                    {
                                        Creats.Add(creats_buf[0]);
                                    }
                                }
                            }
                        
                        }
                    }
                }
               
              
      
            }

            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    
        private void CheckBox_Click(object sender, EventArgs e)
        {
            Panel_inv_list_Click(sender, null);
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
                        if (e != null) checkBox.Checked = !checkBox.Checked;

                        if (checkBox.Checked)
                        {
                            for (int k = 0; k < panel.Controls.Count; k++)
                            {
                                if (panel.Controls[k] is RJ_Lable)  ((RJ_Lable)panel.Controls[k]).BackgroundColor = Color.YellowGreen;
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
    }
}
