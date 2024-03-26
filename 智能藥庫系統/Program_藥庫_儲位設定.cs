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

namespace 智能藥庫系統
{
    public partial class Main_Form : Form
    {
        private void sub_Program_藥庫_儲位設定_Init()
        {
            this.rfiD_UI.Init(dBConfigClass.DB_Basic.DataBaseName, dBConfigClass.DB_Basic.UserName, dBConfigClass.DB_Basic.Password, dBConfigClass.DB_Basic.IP, dBConfigClass.DB_Basic.Port, dBConfigClass.DB_Basic.MySqlSslMode);
            this.plC_UI_Init.Add_Method(sub_Program_藥庫_儲位設定);
        }
        private void sub_Program_藥庫_儲位設定()
        {

        }
    }
}
