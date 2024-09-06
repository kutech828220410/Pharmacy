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

namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        private List<medClass> medClasses_cloud_出入庫作業 = new List<medClass>();
        private void sub_Program_藥庫_出入庫作業_Init()
        {
            rJ_ComboBox_藥庫_出入庫作業_藥名.TextChanged += ComboBox_藥庫_出入庫作業_藥名_TextChanged;
            this.plC_UI_Init.Add_Method(sub_Program_藥庫_出入庫作業);

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

        private void ComboBox_藥庫_出入庫作業_藥名_TextChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.Text.Length >= 3 || true)
            {
                List<string> medClasses_name = (from temp in medClasses_cloud_出入庫作業
                                             where temp.藥品名稱.ToUpper().StartsWith(comboBox.Text.ToUpper())
                                             select temp.藥品名稱).ToList();
                if (comboBox.Items.Count > 0) comboBox.Items.Clear();
               
                comboBox.Items.AddRange(medClasses_name.ToArray());
                comboBox.SelectionStart = comboBox.Text.Length;
         
                comboBox.DroppedDown = true;
                comboBox.MaxDropDownItems = 15;
                Cursor.Current = Cursors.Default;  // 保持滑鼠游標正常顯示

            }
        }
    }
}
