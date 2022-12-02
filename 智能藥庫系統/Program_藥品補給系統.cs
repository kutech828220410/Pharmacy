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

namespace 智能藥庫系統
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
        public enum enum_藥品補給系統_藥品資料
        {
            藥品碼,
            合約項次,
            藥品名稱,
            藥品學名,
            品項,
            廠牌,
            健保碼,
            健保價,
            庫存,
            基本量,
            消耗量,
            安全庫存,
            已訂購數量,
            包裝單位,
            藥品條碼,
            已訂購總價,
            已採購總價,
            已採購總量,
            已採購總量上限,
            契約價金,
            最新訂購單價,
            訂購商,
            合約廠商,
            維護到期日,
            最小包裝數量,
        }
        public enum enum_藥品補給系統_供應商資料 : int
        {
            序號,
            全名,
            簡名,
            類別,
            電話,
            聯絡人,
            Email,
            地址,
            統一編號,
            訂單最低總金額,
            備註,
        }
        public enum enum_藥品補給系統_參數資料 : int
        {
            GUID ,名稱, 數值
        }
        public enum enum_藥品補給系統_參數名稱 : int
        {
            PLC_設定編號,
            伺服器參數_UserName, 伺服器參數_Password, 伺服器參數_Host, 伺服器參數_Port, 伺服器參數_發件者,
            PLC_驗收期限, PLC_超時訂單警報_黃, PLC_超時訂單警報_紅, PLC_自動登出時間_分, PLC_自動登出時間_秒,
            PLC_期初庫存金額,
            短效罰金計算,
            逾時罰金計算,

        }

        private void sub_Program_藥品補給系統_Init()
        {

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_藥品補給系統_藥品資料, dBConfigClass.DB_order_server);
            this.sqL_DataGridView_藥品補給系統_藥品資料.Init();

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_藥品補給系統_供應商資料, dBConfigClass.DB_order_server);
            this.sqL_DataGridView_藥品補給系統_供應商資料.Init();

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_藥品補給系統_參數資料, dBConfigClass.DB_order_server);
            this.sqL_DataGridView_藥品補給系統_參數資料.Init();

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_藥品補給系統_訂單資料, dBConfigClass.DB_order_server);
            this.sqL_DataGridView_藥品補給系統_訂單資料.Init();

            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_藥品補給系統_發票資料, dBConfigClass.DB_order_server);
            this.sqL_DataGridView_藥品補給系統_發票資料.Init();

            this.plC_RJ_Button_藥品補給系統_藥品資料_全部顯示.MouseDownEvent += PlC_RJ_Button_藥品補給系統_藥品資料_全部顯示_MouseDownEvent;
            this.plC_RJ_Button_藥品補給系統_供應商資料_全部顯示.MouseDownEvent += PlC_RJ_Button_藥品補給系統_供應商資料_全部顯示_MouseDownEvent;


            this.plC_UI_Init.Add_Method(this.sub_Program_藥品補給系統);
        }

   
        private void sub_Program_藥品補給系統()
        {

        }

        #region Event
        private void PlC_RJ_Button_藥品補給系統_藥品資料_全部顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_藥品補給系統_藥品資料.SQL_GetAllRows(true);
        }
        private void PlC_RJ_Button_藥品補給系統_供應商資料_全部顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_藥品補給系統_供應商資料.SQL_GetAllRows(true);
        }
        #endregion
    }
}
