using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {
        public enum enum_藥品補給系統_訂單資料 : int
        {
            序號,
            訂單編號,
            藥品碼,
            藥品名稱,
            單位,
            供應商全名,
            供應商Email,
            供應商聯絡人,
            供應商電話,
            包裝單位,
            訂購日期,
            訂購時間,
            訂購人,
            訂購院所別,
            訂購數量,
            已入庫數量,
            訂購單價,
            訂購總價,
            前次訂購單價,
            驗收人,
            驗收院所別,
            驗收日期,
            驗收時間,
            驗收單價,
            驗收總價,
            前次驗收單價,
            應驗收日期,
            發票日期,
            發票金額,
            折讓金額,
            效期,
            批號,
            確認驗收,
            備註,
        }
        public enum enum_藥品補給系統_發票資料 : int
        {
            序號,
            訂單編號,
            發票號碼,
            發票日期,
            登錄時間,
            藥品碼,
            藥品名稱,
            數量,
            單價,
            總價,
            折讓金額,
            折讓後單價,
            前次驗收折讓後單價,
            賣方統一編號,
            入庫人,
            效期,
            批號,
            訂購日期,
            已結清,
            一般匯出,
            中榮匯出,
            逾期罰金,
            短效罰金,
            入庫日期,
            備註,
        }
        private void sub_Program_藥品補給系統_Init()
        {

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_藥品補給系統_訂單資料, dBConfigClass.DB_order_server);
            this.sqL_DataGridView_藥品補給系統_訂單資料.Init();

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_藥品補給系統_發票資料, dBConfigClass.DB_order_server);
            this.sqL_DataGridView_藥品補給系統_發票資料.Init();



            this.plC_UI_Init.Add_Method(this.sub_Program_藥品補給系統);
        }


        private void sub_Program_藥品補給系統()
        {

        }
    }
}
