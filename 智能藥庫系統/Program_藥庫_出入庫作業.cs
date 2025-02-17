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
            [Description("備註,VARCHAR,50,NONE")]
            備註,
        }

        private List<medClass> medClasses_cloud_出入庫作業 = new List<medClass>();
        private void sub_Program_藥庫_出入庫作業_Init()
        {
            ComboBox_藥庫_出入庫作業_藥名.TextChanged += ComboBox_藥庫_出入庫作業_藥名_TextChanged;
            ComboBox_藥庫_出入庫作業_藥名.LostFocus += ComboBox_藥庫_出入庫作業_藥名_LostFocus;
            ComboBox_藥庫_出入庫作業_藥名.Click += ComboBox_藥庫_出入庫作業_藥名_Click;

            ComboBox_藥庫_出入庫作業_效期.LostFocus += ComboBox_藥庫_出入庫作業_效期_LostFocus;
            ComboBox_藥庫_出入庫作業_效期.SelectedIndexChanged += ComboBox_藥庫_出入庫作業_效期_SelectedIndexChanged;

            button_藥庫_出入庫作業_選擇效期.Click += Button_藥庫_出入庫作業_選擇效期_Click;
            button_藥庫_出入庫作業_填入.Click += Button_藥庫_出入庫作業_填入_Click;
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
            this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(200, DataGridViewContentAlignment.MiddleLeft, enum_出入庫作業.備註);
            //this.sqL_DataGridView_藥庫_出入庫作業.Set_ColumnWidth(120, DataGridViewContentAlignment.MiddleCenter, enum_出入庫作業.採購金額繳回);
            this.sqL_DataGridView_藥庫_出入庫作業.MouseDown += SqL_DataGridView_藥庫_出入庫作業_MouseDown;
            this.plC_RJ_Button_藥庫_出入庫作業_新增資料.MouseDownEvent += PlC_RJ_Button_藥庫_出入庫作業_新增資料_MouseDownEvent;
            plC_RJ_Button_藥庫_出入庫作業_確認送出.MouseDownEvent += PlC_RJ_Button_藥庫_出入庫作業_確認送出_MouseDownEvent;
            plC_RJ_Button_藥庫_出入庫作業_刪除資料.MouseDownEvent += PlC_RJ_Button_藥庫_出入庫作業_刪除資料_MouseDownEvent;

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

        private void Button_藥庫_出入庫作業_選擇效期_Click(object sender, EventArgs e)
        {
            if(ComboBox_藥庫_出入庫作業_效期.Items.Count == 0)
            {
                MyMessageBox.ShowDialog("無效期可選擇");
                return;
            }
            rJ_DatePicker_藥庫_出入庫作業_選擇效期.Visible = false;
            ComboBox_藥庫_出入庫作業_效期.Visible = true;

            ComboBox_藥庫_出入庫作業_效期.DroppedDown = true;

        }
        private void PlC_RJ_Button_藥庫_出入庫作業_刪除資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = sqL_DataGridView_藥庫_出入庫作業.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料");
                return;
            }

            if (MyMessageBox.ShowDialog("是否刪除選取資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;

            sqL_DataGridView_藥庫_出入庫作業.DeleteExtra(list_value, true);
        }
        private void PlC_RJ_Button_藥庫_出入庫作業_新增資料_MouseDownEvent(MouseEventArgs mevent)
        {
            //if (ComboBox_藥庫_出入庫作業_效期.GetComboBoxText() == "請選擇效期")
            //{
            //    MyMessageBox.ShowDialog("未選擇效期");
            //    return;
            //}
            if (ComboBox_藥庫_出入庫作業_批號.GetComboBoxText() == "請選擇批號")
            {
                MyMessageBox.ShowDialog("未選擇批號");
                return;
            }
            if (ComboBox_藥庫_出入庫作業_出入庫原因.GetComboBoxText() == "請選擇出入庫原因")
            {
                MyMessageBox.ShowDialog("未選擇原因");
                return;
            }
            if(textBox_藥庫_出入庫作業_數量.Text.StringIsInt32() == false)
            {
                MyMessageBox.ShowDialog("請輸入有效數字");
                return;
            }
            try
            {
                LoadingForm.ShowLoadingForm();
                List<medClass> medclasses_drugStore = medClass.get_ds_drugstore_med(Main_Form.API_Server, "ds01");
                List<medClass> medclasses = (from temp in medclasses_drugStore
                                             where temp.藥品名稱.ToUpper().StartsWith(ComboBox_藥庫_出入庫作業_藥名.GetComboBoxText().ToUpper())
                                             select temp).ToList();
                string 動作 = ((rJ_RatioButton_藥庫_出入庫作業_出庫.Checked) ? "出庫" : "入庫");
                string 藥碼 = medclasses[0].藥品碼;
                string 藥名 = medclasses[0].藥品名稱;
                string 藥庫庫存 = medclasses[0].藥庫庫存;
                string 效期 = rJ_DatePicker_藥庫_出入庫作業_選擇效期.Value.ToDateString();
                string 批號 = ComboBox_藥庫_出入庫作業_批號.GetComboBoxText();
                string 原因 = ComboBox_藥庫_出入庫作業_出入庫原因.GetComboBoxText();
                string 採購金額是否繳回 = "False";
                string 數量 = textBox_藥庫_出入庫作業_數量.Text;
                string 備註 = textBox_藥庫_出入庫作業_備註.Text;
                int temp0 = 數量.StringToInt32();
                List<object[]> list_value = sqL_DataGridView_藥庫_出入庫作業.GetAllRows();
                list_value = (from temp1 in list_value
                              where temp1[(int)enum_出入庫作業.藥碼].ObjectToString() == 藥碼
                              select temp1).ToList();

                for (int i = 0; i < list_value.Count; i++)
                {
                    temp0 += list_value[i][(int)enum_出入庫作業.數量].StringToInt32();
                }

                if (temp0 > 藥庫庫存.StringToInt32())
                {
                    MyMessageBox.ShowDialog($"藥庫庫存[{藥庫庫存}],不足出庫數量");
                    return;
                }    
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
                value[(int)enum_出入庫作業.備註] = 備註;
                sqL_DataGridView_藥庫_出入庫作業.AddRow(value, true);


                this.Invoke(new Action(delegate
                {
                    ComboBox_藥庫_出入庫作業_藥名.TextChanged -= ComboBox_藥庫_出入庫作業_藥名_TextChanged;

                    ComboBox_藥庫_出入庫作業_效期.Text = "請選擇效期";
                    ComboBox_藥庫_出入庫作業_批號.Text = "請選擇批號";
                    ComboBox_藥庫_出入庫作業_出入庫原因.Text = "請選擇出入庫原因";
                    textBox_藥庫_出入庫作業_數量.Text = "";
                    textBox_藥庫_出入庫作業_藥碼.Text = "";
                    ComboBox_藥庫_出入庫作業_藥名.Text = "";
                    ComboBox_藥庫_出入庫作業_效期.Items.Clear();
                    ComboBox_藥庫_出入庫作業_批號.Items.Clear();
                    ComboBox_藥庫_出入庫作業_藥名.Items.Clear();

                    ComboBox_藥庫_出入庫作業_藥名.TextChanged += ComboBox_藥庫_出入庫作業_藥名_TextChanged;

                }));
            }
            catch
            {

            }
            finally
            {
                LoadingForm.CloseLoadingForm();
            }
            
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
                Function_藥庫_驗收入庫_過帳明細_取得資料(medClasses_code[0], ref list_效期, ref list_批號);

                ComboBox_藥庫_出入庫作業_效期.Items.Clear();
                ComboBox_藥庫_出入庫作業_效期.Items.AddRange(list_效期.ToArray());

                ComboBox_藥庫_出入庫作業_批號.Items.Clear();
                ComboBox_藥庫_出入庫作業_批號.Items.AddRange(list_批號.ToArray());
            }
        }
     
        private void ComboBox_藥庫_出入庫作業_效期_LostFocus(object sender, EventArgs e)
        {
            rJ_DatePicker_藥庫_出入庫作業_選擇效期.Visible = true;
            ComboBox_藥庫_出入庫作業_效期.Visible = false;

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
            if (ComboBox_藥庫_出入庫作業_效期.Text.Check_Date_String() == true)
            {
                rJ_DatePicker_藥庫_出入庫作業_選擇效期.Value = ComboBox_藥庫_出入庫作業_效期.Text.StringToDateTime();
                rJ_DatePicker_藥庫_出入庫作業_選擇效期.Visible = true;
                ComboBox_藥庫_出入庫作業_效期.Visible = false;
            }
            if (medClasses_code.Count > 0)
            {
                Function_藥庫_驗收入庫_過帳明細_取得資料(medClasses_code[0], ref list_效期, ref list_批號);

                for (int i = 0; i < list_效期.Count; i++)
                {
                    if (list_效期[i] == ComboBox_藥庫_出入庫作業_效期.Text) ComboBox_藥庫_出入庫作業_批號.Text = list_批號[i];
                }
            }
        
        }
        private void Button_藥庫_出入庫作業_填入_Click(object sender, EventArgs e)
        {
            try
            {
                LoadingForm.ShowLoadingForm();

                if (textBox_藥庫_出入庫作業_藥碼.Text.StringIsEmpty())
                {
                    MyMessageBox.ShowDialog("未輸入資料");
                    return;
                }
                medClass medClass = medClass.get_med_clouds_by_code(API_Server, textBox_藥庫_出入庫作業_藥碼.Text);
                if (medClass == null)
                {
                    MyMessageBox.ShowDialog("查無資料");
                    return;
                }
                if (ComboBox_藥庫_出入庫作業_藥名.Items.Count > 0) ComboBox_藥庫_出入庫作業_藥名.Items.Clear();
                ComboBox_藥庫_出入庫作業_藥名.Items.Add(medClass.藥品名稱);
                ComboBox_藥庫_出入庫作業_藥名.SelectedIndex = 0;
                List<string> list_效期 = new List<string>();
                List<string> list_批號 = new List<string>();
                Function_藥庫_驗收入庫_過帳明細_取得資料(textBox_藥庫_出入庫作業_藥碼.Text, ref list_效期, ref list_批號);

                ComboBox_藥庫_出入庫作業_效期.Items.Clear();
                ComboBox_藥庫_出入庫作業_效期.Items.AddRange(list_效期.ToArray());

                ComboBox_藥庫_出入庫作業_批號.Items.Clear();
                ComboBox_藥庫_出入庫作業_批號.Items.AddRange(list_批號.ToArray());
            }
            catch
            {

            }
            finally
            {
                LoadingForm.CloseLoadingForm();
            }
         
        }

        private void PlC_RJ_Button_藥庫_出入庫作業_確認送出_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = sqL_DataGridView_藥庫_出入庫作業.GetAllRows();

            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未建立出庫資料");
                return;
            }
            if (MyMessageBox.ShowDialog("是否確認送出?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;


            string error_msg = "";
            List<object[]> list_交易紀錄_Add = new List<object[]>();
            List<DeviceBasic> deviceBasics_藥庫 = DeviceBasicClass_藥庫.SQL_GetAllDeviceBasic();
            List<DeviceBasic> deviceBasics_藥庫_replace = new List<DeviceBasic>();
            List<string> list_deviceBasics_藥庫_replace_GUID = new List<string>();
            List<string> list_deviceBasics_藥庫_replace_GUID_buf = new List<string>();
            List<DeviceBasic> deviceBasics_藥庫_buf = new List<DeviceBasic>();

        
            if (list_value.Count == 0) return;
            this.Function_從SQL取得儲位到本地資料();
            List<object[]> list_儲位資料 = new List<object[]>();

            string 藥品碼 = "";
            string 藥品名稱 = "";
            int 來源庫存量 = 0;
            int 來源異動量 = 0;
            int 來源結存量 = 0;
            string 來源備註 = "";
            string 收支原因 = "";

            string 儲位資訊_GUID = "";
            string 儲位資訊_IP = "";
            string 儲位資訊_效期 = "";
            string 儲位資訊_批號 = "";
            int 儲位資訊_異動量 = 0;
            string 出庫_效期 = "";
            string 出庫_批號 = "";

            for (int i = 0; i < deviceBasics_藥庫.Count; i++)
            {
                deviceBasics_藥庫[i].flag_replace = false;
            }


            Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(list_value.Count);
            dialog_Prcessbar.State = "開始...";
            for (int i = 0; i < list_value.Count; i++)
            {
                dialog_Prcessbar.Value = i;
              
                藥品碼 = list_value[i][(int)enum_出入庫作業.藥碼].ObjectToString();
                藥品名稱 = list_value[i][(int)enum_出入庫作業.藥名].ObjectToString();
                deviceBasics_藥庫_buf = deviceBasics_藥庫.SortByCode(藥品碼);
                來源庫存量 = this.Function_從本地資料取得庫存(藥品碼);
                int 異動量 = list_value[i][(int)enum_出入庫作業.數量].ObjectToString().StringToInt32();
                異動量 = 異動量 * -1;
                來源異動量 = 異動量;
                來源結存量 = 來源庫存量 + 來源異動量;
                出庫_效期 = list_value[i][(int)enum_出入庫作業.效期].ObjectToString();
                出庫_批號 = list_value[i][(int)enum_出入庫作業.批號].ObjectToString();
                收支原因 = list_value[i][(int)enum_出入庫作業.原因].ObjectToString();
                list_儲位資料 = Function_取得異動儲位資訊從本地資料(藥品碼, 來源異動量);
                來源備註 = list_value[i][(int)enum_出入庫作業.備註].ObjectToString();
                if (來源備註.StringIsEmpty() == false)
                {
                    來源備註 = $"「{來源備註}」";
                }
                DeviceBasic deviceBasic_藥庫 = null;
                for (int k = 0; k < list_儲位資料.Count; k++)
                {
                    儲位資訊_效期 = list_儲位資料[k][(int)enum_儲位資訊.效期].ObjectToString();
                    儲位資訊_批號 = list_儲位資料[k][(int)enum_儲位資訊.批號].ObjectToString();
                    儲位資訊_異動量 = list_儲位資料[k][(int)enum_儲位資訊.異動量].ObjectToString().StringToInt32();
                    if (deviceBasics_藥庫_buf.Count == 0) continue;
                    deviceBasic_藥庫 = deviceBasics_藥庫_buf[0];
                    if (deviceBasic_藥庫 == null) continue;

                    if (deviceBasic_藥庫 != null)
                    {
                        deviceBasic_藥庫.效期庫存異動(儲位資訊_效期, 儲位資訊_異動量);
                        deviceBasic_藥庫.flag_replace = true; ;
                        List_藥庫_DeviceBasic.Add_NewDeviceBasic(deviceBasic_藥庫);
                    }
                  
                    來源備註 += $"[效期]:{出庫_效期},[批號]:{出庫_批號},[數量]:{儲位資訊_異動量 * 1}";
                    if (k != list_儲位資料.Count - 1) 來源備註 += "\n";
                }
                         
                if (deviceBasics_藥庫_buf[0].Inventory.StringToInt32() != 來源結存量)
                {
                    error_msg += $"[藥庫]({藥品碼}){藥品名稱}\n";
                    deviceBasics_藥庫_buf[0].flag_replace = false;
                    continue;
                }

               


                object[] value_src = new object[new enum_交易記錄查詢資料().GetLength()];
                value_src[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                value_src[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_src[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.出庫作業.GetEnumName();
                value_src[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_src[(int)enum_交易記錄查詢資料.庫存量] = 來源庫存量;
                value_src[(int)enum_交易記錄查詢資料.交易量] = 來源異動量;
                value_src[(int)enum_交易記錄查詢資料.結存量] = 來源結存量;
                value_src[(int)enum_交易記錄查詢資料.備註] = 來源備註;
                value_src[(int)enum_交易記錄查詢資料.庫別] = enum_庫別.藥庫.GetEnumName();
                value_src[(int)enum_交易記錄查詢資料.操作人] = this.登入者名稱;
                value_src[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();
                value_src[(int)enum_交易記錄查詢資料.收支原因] = 收支原因;
                

                list_交易紀錄_Add.Add(value_src);

            }

            dialog_Prcessbar.State = "上傳資料...";
            dialog_Prcessbar.Close();

        
            for (int i = 0; i < deviceBasics_藥庫.Count; i++)
            {
                if (deviceBasics_藥庫[i].flag_replace)
                {
                    deviceBasics_藥庫_replace.Add(deviceBasics_藥庫[i]);
                    deviceBasics_藥庫[i].flag_replace = false;
                }
            }
            sqL_DataGridView_藥庫_出入庫作業.SQL_AddRows(list_value, false);
            this.DeviceBasicClass_藥庫.SQL_ReplaceDeviceBasic(deviceBasics_藥庫_replace);
            this.sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄_Add, false);
            sqL_DataGridView_藥庫_出入庫作業.ClearGrid();
            MyMessageBox.ShowDialog("完成");
        }
    }
}
