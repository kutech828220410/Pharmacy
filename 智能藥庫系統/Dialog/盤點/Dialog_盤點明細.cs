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
    public partial class Dialog_盤點明細 : MyDialog
    {
    
        public inventoryClass.creat Creat = new inventoryClass.creat();
        private List<Panel> panels = new List<Panel>();
        private string _IC_SN = "";
        public Dialog_盤點明細(string IC_SN)
        {
            InitializeComponent();
            Reflection.MakeDoubleBuffered(this, true);
            this._IC_SN = IC_SN;
            this.CancelButton = this.plC_RJ_Button_返回;
            this.Load += Dialog_盤點明細_Load;
            this.LoadFinishedEvent += Dialog_盤點明細_LoadFinishedEvent;
            this.plC_RJ_Button_返回.MouseDownEvent += PlC_RJ_Button_返回_MouseDownEvent;
            this.rJ_Button_藥碼搜尋.MouseDownEvent += RJ_Button_藥碼搜尋_MouseDownEvent;
            this.rJ_Button_藥名搜尋.MouseDownEvent += RJ_Button_藥名搜尋_MouseDownEvent;
            this.rJ_TextBox_藥碼搜尋.KeypressEnterButton = this.rJ_Button_藥碼搜尋;
            this.rJ_TextBox_藥名搜尋.KeypressEnterButton = this.rJ_Button_藥名搜尋;
        }





        #region Fucntion
        private void Function_RereshUI(List<inventoryClass.content> contents)
        {

            this.Invoke(new Action(delegate
            {
                this.SuspendLayout();
                panels.Clear();
                this.panel_controls.SuspendLayout();
                this.panel_controls.Controls.Clear();
                this.panel_controls.Visible = false;
                contents.Sort(new ICP_content());
                for (int i = 0; i < contents.Count; i++)
                {
                    string GUID = contents[i].GUID;
                    Panel panel_list = new Panel();
                    RJ_Lable rJ_Lable_content = new RJ_Lable();
                    RJ_Lable rJ_Lable_index = new RJ_Lable();
                    RJ_Lable rJ_Lable_value = new RJ_Lable();
                    PLC_RJ_Button plC_RJ_Button_delete = new PLC_RJ_Button();

                    Color backColor = Color.White;
                    panel_list.Controls.Add(rJ_Lable_value);
                    panel_list.Controls.Add(rJ_Lable_content);
                    panel_list.Controls.Add(rJ_Lable_index);
                    panel_list.Controls.Add(plC_RJ_Button_delete);
                    panel_list.Dock = System.Windows.Forms.DockStyle.Top;
                    panel_list.Location = new System.Drawing.Point(0, 0);
                    panel_list.Size = new System.Drawing.Size(1192, 36);
                    panel_list.TabIndex = 9;
                    panel_list.Visible = true;
                    if(contents[i].Sub_content.Count > 0)
                    {
                        backColor = Color.GreenYellow;
                    }
                    // 
                    // plC_RJ_Button_delete
                    // 
                    plC_RJ_Button_delete.AutoResetState = false;
                    plC_RJ_Button_delete.BackgroundColor = System.Drawing.Color.Red;
                    plC_RJ_Button_delete.Bool = false;
                    plC_RJ_Button_delete.BorderColor = System.Drawing.Color.DarkRed;
                    plC_RJ_Button_delete.BorderRadius = 3;
                    plC_RJ_Button_delete.BorderSize = 0;
                    plC_RJ_Button_delete.but_press = false;
                    plC_RJ_Button_delete.buttonType = MyUI.RJ_Button.ButtonType.Toggle;
                    plC_RJ_Button_delete.DisenableColor = System.Drawing.Color.Gray;
                    plC_RJ_Button_delete.Dock = System.Windows.Forms.DockStyle.Right;
                    plC_RJ_Button_delete.FlatAppearance.BorderSize = 0;
                    plC_RJ_Button_delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    plC_RJ_Button_delete.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold);
                    plC_RJ_Button_delete.GUID = $"{contents[i].GUID}";
                    plC_RJ_Button_delete.Icon = System.Windows.Forms.MessageBoxIcon.Warning;
                    plC_RJ_Button_delete.Image_padding = new System.Windows.Forms.Padding(0);
                    plC_RJ_Button_delete.Location = new System.Drawing.Point(1118, 0);
                    plC_RJ_Button_delete.OFF_文字內容 = "刪除";
                    plC_RJ_Button_delete.OFF_文字字體 = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold);
                    plC_RJ_Button_delete.OFF_文字顏色 = System.Drawing.Color.White;
                    plC_RJ_Button_delete.OFF_背景顏色 = System.Drawing.Color.Red;
                    plC_RJ_Button_delete.ON_BorderSize = 5;
                    plC_RJ_Button_delete.ON_文字內容 = "刪除";
                    plC_RJ_Button_delete.ON_文字字體 = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    plC_RJ_Button_delete.ON_文字顏色 = System.Drawing.Color.White;
                    plC_RJ_Button_delete.ON_背景顏色 = System.Drawing.Color.Red;
                    plC_RJ_Button_delete.ProhibitionBorderLineWidth = 1;
                    plC_RJ_Button_delete.ProhibitionLineWidth = 4;
                    plC_RJ_Button_delete.ProhibitionSymbolSize = 30;
                    plC_RJ_Button_delete.ShadowColor = System.Drawing.Color.DimGray;
                    plC_RJ_Button_delete.ShadowSize = 3;
                    plC_RJ_Button_delete.ShowLoadingForm = false;
                    plC_RJ_Button_delete.Size = new System.Drawing.Size(74, 36);
                    plC_RJ_Button_delete.State = false;
                    plC_RJ_Button_delete.TabIndex = 6;
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
                    plC_RJ_Button_delete.MouseDownEventEx += PlC_RJ_Button_delete_MouseDownEventEx;
                    // 
                    // rJ_Lable_index
                    // 
                    rJ_Lable_index.BackColor = System.Drawing.Color.White;
                    rJ_Lable_index.BackgroundColor = System.Drawing.Color.White;
                    rJ_Lable_index.BorderColor = System.Drawing.Color.Black;
                    rJ_Lable_index.BorderRadius = 0;
                    rJ_Lable_index.BorderSize = 2;
                    rJ_Lable_index.Dock = System.Windows.Forms.DockStyle.Left;
                    rJ_Lable_index.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    rJ_Lable_index.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    rJ_Lable_index.ForeColor = System.Drawing.Color.Transparent;
                    rJ_Lable_index.GUID = "";
                    rJ_Lable_index.Location = new System.Drawing.Point(0, 0);
                    rJ_Lable_index.ShadowColor = System.Drawing.Color.DimGray;
                    rJ_Lable_index.ShadowSize = 0;
                    rJ_Lable_index.Size = new System.Drawing.Size(47, 36);
                    rJ_Lable_index.TabIndex = 7;
                    rJ_Lable_index.Text = $"{i+1}.";
                    rJ_Lable_index.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    rJ_Lable_index.TextColor = System.Drawing.Color.Black;
                    // 
                    // rJ_Lable_content
                    // 
                    rJ_Lable_content.BackColor = backColor;
                    rJ_Lable_content.BackgroundColor = backColor;
                    rJ_Lable_content.BorderColor = System.Drawing.Color.PaleVioletRed;
                    rJ_Lable_content.BorderRadius = 0;
                    rJ_Lable_content.BorderSize = 0;
                    rJ_Lable_content.Dock = System.Windows.Forms.DockStyle.Left;
                    rJ_Lable_content.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    rJ_Lable_content.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    rJ_Lable_content.ForeColor = System.Drawing.Color.Transparent;
                    rJ_Lable_content.GUID = GUID;
                    rJ_Lable_content.Location = new System.Drawing.Point(47, 0);
                    rJ_Lable_content.ShadowColor = System.Drawing.Color.DimGray;
                    rJ_Lable_content.ShadowSize = 0;
                    rJ_Lable_content.Size = new System.Drawing.Size(600, 36);
                    rJ_Lable_content.TabIndex = 8;
                    rJ_Lable_content.Text = $"({contents[i].藥品碼}) {contents[i].藥品名稱}";
                    rJ_Lable_content.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_content.TextColor = System.Drawing.Color.Black;


                    rJ_Lable_value.BackColor = backColor;
                    rJ_Lable_value.BackgroundColor = backColor;
                    rJ_Lable_value.BorderColor = System.Drawing.Color.PaleVioletRed;
                    rJ_Lable_value.BorderRadius = 0;
                    rJ_Lable_value.BorderSize = 0;
                    rJ_Lable_value.Dock = System.Windows.Forms.DockStyle.Fill;
                    rJ_Lable_value.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    rJ_Lable_value.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
                    rJ_Lable_value.ForeColor = System.Drawing.Color.Transparent;
                    rJ_Lable_value.GUID = "";
                    rJ_Lable_value.Location = new System.Drawing.Point(641, 0);
                    rJ_Lable_value.ShadowColor = System.Drawing.Color.DimGray;
                    rJ_Lable_value.ShadowSize = 0;
                    rJ_Lable_value.Size = new System.Drawing.Size(477, 36);
                    rJ_Lable_value.TabIndex = 9;
                    rJ_Lable_value.Text = $"盤點量 : {contents[i].盤點量}";
                    rJ_Lable_value.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    rJ_Lable_value.TextColor = System.Drawing.Color.Black;

                    panels.Add(panel_list);

                }
                for (int i = panels.Count - 1; i >= 0; i--)
                {
                    this.panel_controls.Controls.Add(panels[i]);
                }
                this.panel_controls.AutoScroll = true;
                this.panel_controls.ResumeLayout(false);
        
                this.ResumeLayout(false);
                this.panel_controls.Visible = true;
                //this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height + 1);
                //this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
            }));
        }

    
        #endregion
        #region Event
        private void Dialog_盤點明細_Load(object sender, EventArgs e)
        {
    
        }
        private void Dialog_盤點明細_LoadFinishedEvent(EventArgs e)
        {
            LoadingForm.ShowLoadingForm();

            inventoryClass.creat creat = inventoryClass.creat_get_by_IC_SN(Main_Form.API_Server, _IC_SN);
            if (creat == null)
            {
                Dialog_AlarmForm dialog_AlarmForm = new Dialog_AlarmForm("找無盤點資料", 2000);
                dialog_AlarmForm.ShowDialog();
                this.Close();
                return;
            }
            this.Creat = creat;
            Function_RereshUI(creat.Contents);
            LoadingForm.CloseLoadingForm();
        }
        private void RJ_Button_藥名搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<inventoryClass.content> contents = (from temp in this.Creat.Contents
                                                     where temp.藥品名稱.ToUpper().Contains(rJ_TextBox_藥名搜尋.Texts.ToUpper())
                                                     select temp).ToList();
            Function_RereshUI(contents);
        }
        private void RJ_Button_藥碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<inventoryClass.content> contents = (from temp in this.Creat.Contents
                                                     where temp.藥品碼.ToUpper().Contains(rJ_TextBox_藥碼搜尋.Texts.ToUpper())
                                                     select temp).ToList();
            Function_RereshUI(contents);
        }
        private void PlC_RJ_Button_返回_MouseDownEvent(MouseEventArgs mevent)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        private void PlC_RJ_Button_delete_MouseDownEventEx(RJ_Button rJ_Button, MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("確認刪除?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            LoadingForm.ShowLoadingForm();
            string GUID = rJ_Button.GUID;
            inventoryClass.contents_delete_by_GUID(Main_Form.API_Server, GUID);
            inventoryClass.creat creat = inventoryClass.creat_get_by_IC_SN(Main_Form.API_Server, _IC_SN);
            this.Creat = creat;
            Function_RereshUI(creat.Contents);
            LoadingForm.CloseLoadingForm();
        }
        #endregion
        public class ICP_content : IComparer<inventoryClass.content>
        {
            public int Compare(inventoryClass.content x, inventoryClass.content y)
            {
                // 檢查 x 和 y 的 Sub_content 是否為 null，並取得其 Count
                int xSubContentCount = x.Sub_content != null ? x.Sub_content.Count : 0;
                int ySubContentCount = y.Sub_content != null ? y.Sub_content.Count : 0;

                // 先以 Sub_content.Count 排序
                int result = ySubContentCount.CompareTo(xSubContentCount);
                if (result == 0)
                {
                    // 若 Sub_content.Count 相等，則以藥碼排序
                    result = x.藥品碼.CompareTo(y.藥品碼);
                }
                return result;

            }
        }
       
    }
}
