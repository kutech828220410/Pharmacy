using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyUI;
using SQLUI;
using Basic;
namespace 智能藥庫系統
{
    public partial class Dialog_周邊設備庫存顯示 : MyDialog
    {
        private string _IP;
        private enum enum_庫存查詢
        {
            GUID,
            藥碼,
            藥名,
            庫存
        }
        public Dialog_周邊設備庫存顯示(string IP)
        {
            InitializeComponent();
            this._IP = IP;
            this.Load += Dialog_周邊設備庫存顯示_Load;
            this.LoadFinishedEvent += Dialog_周邊設備庫存顯示_LoadFinishedEvent;
            this.rJ_Button_返回.MouseDownEvent += RJ_Button_返回_MouseDownEvent;
        }

        private void Dialog_周邊設備庫存顯示_Load(object sender, EventArgs e)
        {
            Table table = new Table("");
            table.AddColumnList("GUID", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("藥碼", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("藥名", Table.StringType.VARCHAR, Table.IndexType.None);
            table.AddColumnList("庫存", Table.StringType.VARCHAR, Table.IndexType.None);
            sqL_DataGridView_庫存查詢.Init(table);
            sqL_DataGridView_庫存查詢.Set_ColumnVisible(false, new enum_庫存查詢().GetEnumNames());
            sqL_DataGridView_庫存查詢.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_庫存查詢.藥碼);
            sqL_DataGridView_庫存查詢.Set_ColumnWidth(600, DataGridViewContentAlignment.MiddleLeft, enum_庫存查詢.藥名);
            sqL_DataGridView_庫存查詢.Set_ColumnWidth(100, DataGridViewContentAlignment.MiddleLeft, enum_庫存查詢.庫存);
        }

        private void RJ_Button_返回_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Close();
            }));
        }
        private void Dialog_周邊設備庫存顯示_LoadFinishedEvent(EventArgs e)
        {
            LoadingForm.ShowLoadingForm();
            List<object[]> list_庫存查詢 = Main_Form.Function_周邊設備_庫存查詢(_IP);
            list_庫存查詢 = list_庫存查詢.CopyRows(new Main_Form.enum_周邊設備_庫存_庫存查詢(), new enum_庫存查詢());
            sqL_DataGridView_庫存查詢.RefreshGrid(list_庫存查詢);
            LoadingForm.CloseLoadingForm();
        }
    }
}
