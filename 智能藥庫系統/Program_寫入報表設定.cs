using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MyUI;
using Basic;
using H_Pannel_lib;
namespace 智能藥庫系統
{
    public partial class Form1 : Form
    {
        private enum enum_寫入報表設定_類別
        {
            OPD_消耗帳,
            PHR_消耗帳,
            PHER_消耗帳,
            公藥_消耗帳,
            其他,
        }
        private enum enum_寫入報表設定
        {
            GUID,
            編號,
            檔名,
            檔案位置,
            類別,
            更新每日,
            更新每週,
            更新每月,
            備註內容,
            out_db_server,
            out_db_name,
            out_db_username,
            out_db_password,
            out_db_port,
            fileServer_username,
            fileServer_password,
        }

        private void sub_Program_寫入報表設定_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_寫入報表設定, dBConfigClass.DB_posting_server);
            this.sqL_DataGridView_寫入報表設定.Init();



            this.plC_UI_Init.Add_Method(this.sub_Program_寫入報表設定);
        }

        private void sub_Program_寫入報表設定()
        {

        }
    }
}
