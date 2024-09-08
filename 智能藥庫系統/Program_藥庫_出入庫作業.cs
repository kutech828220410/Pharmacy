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
using HIS_DB_Lib;
using SQLUI;
namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        public enum ContextMenuStrip_出入庫作業
        {
            刪除,
        }

        public enum enum_出入庫作業
        {
            [Description("GUID,VARCHAR,50,PRIMARY")]
            GUID,
            [Description("動作,VARCHAR,50,NONE")]
            動作,
            [Description("藥碼,VARCHAR,50,NONE")]
            藥碼,
            [Description("藥名,VARCHAR,50,NONE")]
            藥名,
            [Description("藥庫庫存,VARCHAR,50,NONE")]
            藥庫庫存,
            [Description("數量,VARCHAR,50,NONE")]
            數量,
            [Description("效期,VARCHAR,50,NONE")]
            效期,
            [Description("批號,VARCHAR,50,NONE")]
            批號,
            [Description("原因,VARCHAR,50,NONE")]
            原因,
            [Description("採購金額繳回,VARCHAR,50,NONE")]
            採購金額繳回,
                      
        }

        private List<medClass> medClasses_cloud_出入庫作業 = new List<medClass>();
        private void sub_Program_藥庫_出入庫作業_Init()
        {
            ComboBox_藥庫_出入庫作業_藥名.TextChanged += ComboBox_藥庫_出入庫作業_藥名_TextChanged;
            ComboBox_藥庫_出入庫作業_藥名.LostFocus += ComboBox_藥庫_出入庫作業_藥名_LostFocus;
            ComboBox_藥庫_出入庫作業_藥名.Click += ComboBox_藥庫_出入庫作業_藥名_Click;

            ComboBox_藥庫_出入庫作業_效期.LostFocus += ComboBox_藥庫_出入庫作業_效期_LostFocus;
            ComboBox_藥庫_出入庫作業_效期.SelectedIndexChanged += ComboBox_藥庫_出入庫作業_效期_SelectedIndexChanged;

            Table table = new Table(new enum_出入庫作業());
            sqL_DataGridView_藥庫_出入庫作業.Init(table);
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnVisible(false, new enum_出入庫作業().GetEnumNames());
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_出入庫作業.動作);
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(80, DataGridViewContentAlignment.MiddleLeft, enum_出入庫作業.藥碼);
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(500, DataGridViewContentAlignment.MiddleLeft, enum_出入庫作業.藥名);
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleCenter, enum_出入庫作業.藥庫庫存);
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleCenter, enum_出入庫作業.數量);
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleLeft, enum_出入庫作業.效期);
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleLeft, enum_出入庫作業.批號);
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(200, DataGridViewContentAlignment.MiddleLeft, enum_出入庫作業.原因);
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleCenter, enum_出入庫作業.採購金額繳回);
            this.sqL_DataGridView_藥庫_出入庫作業.MouseDown += SqL_DataGridView_藥庫_出入庫作業_MouseDown;
            this.plC_RJ_Button_藥庫_出入庫作業_新增資料.MouseDownEvent += PlC_RJ_Button_藥庫_出入庫作業_新增資料_MouseDownEvent;

            this.plC_UI_Init.Add_Method(sub_Program_藥庫_出入庫作業);

        }

        private void SqL_DataGridView_藥庫_出入庫作業_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
       
                Dialog_ContextMenuStrip dialog_ContextMenuStrip = new Dialog_ContextMenuStrip(new ContextMenuStrip_出入庫作業().GetEnumNames());
                dialog_ContextMenuStrip.TitleText = "";
                if (dialog_ContextMenuStrip.ShowDialog() == DialogResult.Yes)
                {
                }
            }
        }

        private bool flag_藥庫_出入庫作業 = false;
        private void sub_Program_藥庫_出入庫作業()
        {
            if (this.plC_ScreenPage_Main.PageText == "藥庫" && this.plC_ScreenPage_藥庫.PageText == "出入庫作業")
            {
                if (!this.flag_藥庫_出入庫作業)
                {
                    medClasses_cloud_出入庫作業 = medClass.get_med_cloud(Main_Form.API_Server);
                    this.flag_藥庫_出入庫作業 = true;
                }

            }
            else
            {
                this.flag_藥庫_出入庫作業 = false;
            }
        }

        private void PlC_RJ_Button_藥庫_出入庫作業_新增資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<medClass> medclasses_drugStore = medClass.get_ds_drugstore_med(Main_Form.API_Server, "ds01"); 
            List<medClass> medclasses = (from temp in medclasses_drugStore
                                         where temp.藥品名稱.ToUpper().StartsWith(ComboBox_藥庫_出入庫作業_藥名.GetComboBoxText().ToUpper())
                                            select temp).ToList();
            string 動作 = ((rJ_RatioButton_藥庫_出入庫作業_出庫.Checked) ? "出庫" : "入庫");
            string 藥碼 = medclasses[0].藥品碼;
            string 藥名 = medclasses[0].藥品名稱;
            string 藥庫庫存 = medclasses[0].藥庫庫存;
            string 效期 = ComboBox_藥庫_出入庫作業_效期.GetComboBoxText();
            string 批號 = ComboBox_藥庫_出入庫作業_批號.GetComboBoxText();
            string 原因 = ComboBox_藥庫_出入庫作業_出入庫原因.GetComboBoxText();
            string 採購金額是否繳回 = ComboBox_藥庫_出入庫作業_採購金額是否繳回.GetComboBoxText();
            string 數量 = textBox_藥庫_出入庫作業_數量.Text;

            object[] value = new object[new enum_出入庫作業().GetLength()];
            value[(int)enum_出入庫作業.GUID] = Guid.NewGuid().ToString();
            value[(int)enum_出入庫作業.藥碼] = 藥碼;
            value[(int)enum_出入庫作業.藥名] = 藥名;
            value[(int)enum_出入庫作業.效期] = 效期;
            value[(int)enum_出入庫作業.批號] = 批號;
            value[(int)enum_出入庫作業.原因] = 原因;
            value[(int)enum_出入庫作業.採購金額繳回] = 採購金額是否繳回;
            value[(int)enum_出入庫作業.藥庫庫存] = 藥庫庫存;
            value[(int)enum_出入庫作業.數量] = 數量;
            value[(int)enum_出入庫作業.動作] = 動作;

            sqL_DataGridView_藥庫_出入庫作業.AddRow(value, true);
        }
        private void ComboBox_藥庫_出入庫作業_藥名_Click(object sender, EventArgs e)
        {
            ComboBox_藥庫_出入庫作業_藥名_TextChanged(sender, e);
        }
        private void ComboBox_藥庫_出入庫作業_藥名_TextChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.Text.Length >= 3 || true)
            {
                // 根據輸入過濾項目
                List<string> medClasses_name = (from temp in medClasses_cloud_出入庫作業
                                                where temp.藥品名稱.ToUpper().StartsWith(comboBox.Text.ToUpper())
                                                select temp.藥品名稱).ToList();

                // 清空現有的項目
                if (comboBox.Items.Count > 0) comboBox.Items.Clear();
                // 新增過濾後的項目
                comboBox.Items.AddRange(medClasses_name.ToArray());
                // 設置 MaxDropDownItems 必須在顯示下拉選單前設定
                comboBox.MaxDropDownItems = 15;
                // 設置下拉框的高度
                comboBox.DropDownHeight = comboBox.ItemHeight * 15;
                // 確保輸入框的游標保持在文字最後
                comboBox.SelectionStart = comboBox.Text.Length;
                // 顯示下拉選單
                comboBox.DroppedDown = true;
                // 保持滑鼠游標正常顯示
                Cursor.Current = Cursors.Default;
            }
        }

        private void ComboBox_藥庫_出入庫作業_藥名_LostFocus(object sender, EventArgs e)
        {
            if (ComboBox_藥庫_出入庫作業_藥名.Text == "") return;
            List<string> medClasses_name = (from temp in medClasses_cloud_出入庫作業
                                            where temp.藥品名稱.ToUpper().StartsWith(ComboBox_藥庫_出入庫作業_藥名.Text.ToUpper())
                                            select temp.藥品名稱).ToList();
            if(medClasses_name.Count == 0)
            {
                MyMessageBox.ShowDialog("找無此藥品,請重新選擇");
                ComboBox_藥庫_出入庫作業_藥名.Text = "";
                return;
            }
            ComboBox comboBox = (ComboBox)sender;
            List<string> list_效期 = new List<string>();
            List<string> list_批號 = new List<string>();
            List<string> medClasses_code = (from temp in medClasses_cloud_出入庫作業
                                            where temp.藥品名稱.ToUpper().StartsWith(comboBox.Text.ToUpper())
                                            select temp.藥品碼).ToList();
            if (medClasses_code.Count > 0)
            {
                Funnction_交易記錄查詢_取得指定藥碼批號期效期(medClasses_code[0], ref list_效期, ref list_批號);

                ComboBox_藥庫_出入庫作業_效期.Items.Clear();
                ComboBox_藥庫_出入庫作業_效期.Items.AddRange(list_效期.ToArray());

                ComboBox_藥庫_出入庫作業_批號.Items.Clear();
                ComboBox_藥庫_出入庫作業_批號.Items.AddRange(list_批號.ToArray());
            }
        }
     
        private void ComboBox_藥庫_出入庫作業_效期_LostFocus(object sender, EventArgs e)
        {
            if (ComboBox_藥庫_出入庫作業_效期.Text == "請選擇效期") return;
            if (ComboBox_藥庫_出入庫作業_效期.Text.Check_Date_String() == false)
            {
                ComboBox_藥庫_出入庫作業_效期.Text = "請選擇效期";
                MyMessageBox.ShowDialog("效期須為日期格式");
             
                return;
            }
        
        }
        private void ComboBox_藥庫_出入庫作業_效期_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<string> list_效期 = new List<string>();
            List<string> list_批號 = new List<string>();
            List<string> medClasses_code = (from temp in medClasses_cloud_出入庫作業
                                            where temp.藥品名稱.ToUpper().StartsWith(ComboBox_藥庫_出入庫作業_藥名.Text.ToUpper())
                                            select temp.藥品碼).ToList();

            if (medClasses_code.Count > 0)
            {
                Funnction_交易記錄查詢_取得指定藥碼批號期效期(medClasses_code[0], ref list_效期, ref list_批號);

                for (int i = 0; i < list_效期.Count; i++)
                {
                    if (list_效期[i] == ComboBox_藥庫_出入庫作業_效期.Text) ComboBox_藥庫_出入庫作業_批號.Text = list_批號[i];
                }
            }
        }

    }
}
