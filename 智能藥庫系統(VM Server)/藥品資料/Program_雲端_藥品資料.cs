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
using SQLUI;
using Basic;
using MyUI;
using IBM.Data.DB2;
namespace 智能藥庫系統_VM_Server_
{
    public partial class Form1 : Form
    {
        public enum enum_雲端_藥品資料_DB2
        {
            藥品碼,
            藥品名稱,
            藥品學名,
            中文名稱,
            包裝單位,
            包裝數量,
        }
        public enum enum_雲端_藥品資料
        {
            GUID,
            藥品碼,
            中文名稱,
            藥品名稱,
            藥品學名,
            健保碼,
            包裝單位,
            包裝數量,
            最小包裝單位,
            最小包裝數量,
            藥品條碼1,
            藥品條碼2,
            警訊藥品,
            管制級別,
            類別
        }
        public enum enum_雲端_藥品資料_匯出
        {
            藥品碼,
            中文名稱,
            藥品名稱,
            藥品學名,
            健保碼,
            包裝單位,
            包裝數量,
            最小包裝單位,
            最小包裝數量,
            藥品條碼1,
            藥品條碼2,
        }
        public enum enum_雲端_藥品資料_匯入
        {
            藥品碼,
            中文名稱,
            藥品名稱,
            藥品學名,
            健保碼,
            包裝單位,
            包裝數量,
            最小包裝單位,
            最小包裝數量,
            藥品條碼1,
            藥品條碼2,
        }
        private void sub_Program_雲端_藥品資料_Init()
        {
            SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_雲端_藥品資料, this.dBConfigClass.DB_Medicine_page);

            this.sqL_DataGridView_雲端_藥品資料.Init();
            if (!this.sqL_DataGridView_雲端_藥品資料.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_雲端_藥品資料.SQL_CreateTable();
            }
            this.plC_RJ_Button_雲端_藥品資料_更新資料.MouseDownEvent += plC_RJ_Button_雲端_藥品資料_更新資料_MouseDownEvent;
            this.plC_RJ_Button_雲端_藥品資料_檢查重複藥碼.MouseDownEvent += plC_RJ_Button_雲端_藥品資料_檢查重複藥碼_MouseDownEvent;
            this.plC_RJ_Button_雲端_藥品資料_顯示全部.MouseDownEvent += plC_RJ_Button_雲端_藥品資料_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_雲端_藥品資料_匯入.MouseDownEvent += plC_RJ_Button_雲端_藥品資料_匯入_MouseDownEvent;
            this.plC_RJ_Button_雲端_藥品資料_匯出.MouseDownEvent += plC_RJ_Button_雲端_藥品資料_匯出_MouseDownEvent;

            this.plC_UI_Init.Add_Method(this.sub_Program_雲端_藥品資料);
        }

        private void sub_Program_雲端_藥品資料()
        {
           
        }
        
        #region Event
        private void plC_RJ_Button_雲端_藥品資料_檢查重複藥碼_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);

            String MyDb2ConnectionString = "server=DBGW1.VGHKS.GOV.TW:50000;database=DBDSNP;uid=APUD07;pwd=UD07AP;";
            IBM.Data.DB2.DB2Connection MyDb2Connection = new IBM.Data.DB2.DB2Connection(MyDb2ConnectionString);
            Console.Write($"開啟DB2連線....");
            MyDb2Connection.Open();
            Console.WriteLine($"DB2連線成功!耗時{myTimer.ToString()}ms");
            IBM.Data.DB2.DB2Command MyDB2Command = MyDb2Connection.CreateCommand();
            MyDB2Command.CommandText = "SELECT A.UDCONVER ,A.UDUNFORM, A.UDRPNAME,A.UDSCNAME,A.UDSTOKNO,A.UDCHTNAM,A.UDDRGNO,B.UDPRDNAM FROM UD.UDDRGVWA A LEFT OUTER JOIN UD.UDPRDPF B ON A.UDDRGNO = B.UDDRGNO AND A.HID = B.HID WHERE A.HID = '2A0' WITH UR";

            var reader = MyDB2Command.ExecuteReader();
            Console.WriteLine($"取得DB2資料!耗時{myTimer.ToString()}ms");
            int FieldCount = reader.FieldCount;
            List<object[]> obj_temp_array = new List<object[]>();
            List<object[]> obj_temp_array_buf = new List<object[]>();
            List<object[]> obj_temp_array_2X = new List<object[]>();
            List<object[]> obj_temp_array_2X_buf = new List<object[]>();
            List<object> obj_temp = new List<object>();
            string UDSTOKNO = "";
            string UDDRGNO = "";
            while (reader.Read())
            {
                obj_temp.Clear();
                UDSTOKNO = reader["UDSTOKNO"].ToString().Trim();

                if (UDSTOKNO.Length >= 5)
                {
                    UDSTOKNO = UDSTOKNO.Substring(UDSTOKNO.Length - 5);
                }
                else
                {
                    UDSTOKNO = "";
                }
                UDDRGNO = reader["UDDRGNO"].ToString().Trim();
                if (UDDRGNO != "")
                {

                    obj_temp.Add(UDDRGNO); 
                    obj_temp.Add(reader["UDRPNAME"].ToString().Trim());
                    obj_temp.Add(reader["UDPRDNAM"].ToString().Trim());
                    obj_temp.Add(reader["UDCHTNAM"].ToString().Trim());
                    obj_temp.Add(reader["UDUNFORM"].ToString().Trim());
                    obj_temp.Add(reader["UDCONVER"].ToString().Trim());


                    obj_temp_array.Add(obj_temp.ToArray());
                }
            }
            MyDb2Connection.Close();
            Console.WriteLine($"關閉連線!取得資料共{obj_temp_array.Count}筆,耗時{myTimer.ToString()}ms");

            for (int i = 0; i < obj_temp_array.Count; i++)
            {
                string 藥品碼 = obj_temp_array[i][(int)enum_雲端_藥品資料_DB2.藥品碼].ObjectToString();
                obj_temp_array_buf = obj_temp_array.GetRows((int)enum_雲端_藥品資料_DB2.藥品碼, 藥品碼);
                obj_temp_array_2X_buf = obj_temp_array_2X.GetRows((int)enum_雲端_藥品資料_DB2.藥品碼, 藥品碼);
                if (obj_temp_array_buf.Count >= 2 && obj_temp_array_2X_buf.Count == 0)
                {
                    for (int k = 0; k < obj_temp_array_buf.Count; k++)
                    {
                        obj_temp_array_2X.Add(obj_temp_array_buf[k]);
                    }
                }
            }
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.saveFileDialog_SaveExcel.ShowDialog();
            }));
            if (dialogResult == DialogResult.OK)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.WaitCursor;
                }));
                DataTable dataTable = obj_temp_array_2X.ToDataTable(new enum_雲端_藥品資料_DB2());
                dataTable = dataTable.ReorderTable(new enum_雲端_藥品資料_DB2());
                CSVHelper.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));

            }
        }
        private void plC_RJ_Button_雲端_藥品資料_更新資料_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            try
            {
                String MyDb2ConnectionString = "server=DBGW1.VGHKS.GOV.TW:50000;database=DBDSNP;uid=APUD07;pwd=UD07AP;";
                IBM.Data.DB2.DB2Connection MyDb2Connection = new IBM.Data.DB2.DB2Connection(MyDb2ConnectionString);
                Console.Write($"開啟DB2連線....");
                MyDb2Connection.Open();
                Console.WriteLine($"DB2連線成功!耗時{myTimer.ToString()}ms");
                IBM.Data.DB2.DB2Command MyDB2Command = MyDb2Connection.CreateCommand();
                MyDB2Command.CommandText = "SELECT A.UDCONVER ,A.UDUNFORM, A.UDRPNAME,A.UDSCNAME,A.UDSTOKNO,A.UDCHTNAM,A.UDDRGNO,B.UDPRDNAM FROM UD.UDDRGVWA A LEFT OUTER JOIN UD.UDPRDPF B ON A.UDDRGNO = B.UDDRGNO AND A.HID = B.HID WHERE A.HID = '2A0' WITH UR";

                var reader = MyDB2Command.ExecuteReader();
                Console.WriteLine($"取得DB2資料!耗時{myTimer.ToString()}ms");
                int FieldCount = reader.FieldCount;
                List<object[]> obj_temp_array = new List<object[]>();
                List<object[]> obj_temp_array_buf = new List<object[]>();
                List<object[]> obj_temp_array_result = new List<object[]>();
                List<object> obj_temp = new List<object>();
                string UDSTOKNO = "";
                string UDDRGNO = "";

                while (reader.Read())
                {
                    obj_temp.Clear();
                    UDSTOKNO = reader["UDSTOKNO"].ToString().Trim();

                    if (UDSTOKNO.Length >= 5)
                    {
                        UDSTOKNO = UDSTOKNO.Substring(UDSTOKNO.Length - 5);
                    }
                    else
                    {
                        UDSTOKNO = "";
                    }
                    UDDRGNO = reader["UDDRGNO"].ToString().Trim();
                    if (UDDRGNO != "")
                    {

                        obj_temp.Add(UDDRGNO);
                        obj_temp.Add(reader["UDRPNAME"].ToString().Trim());
                        obj_temp.Add(reader["UDPRDNAM"].ToString().Trim());
                        obj_temp.Add(reader["UDCHTNAM"].ToString().Trim());
                        obj_temp.Add(reader["UDUNFORM"].ToString().Trim());
                        obj_temp.Add(reader["UDCONVER"].ToString().Trim());


                        obj_temp_array.Add(obj_temp.ToArray());
                    }
                }
                MyDb2Connection.Close();
                Console.WriteLine($"關閉連線!取得資料共{obj_temp_array.Count}筆,耗時{myTimer.ToString()}ms");

                for (int i = 0; i < obj_temp_array.Count; i++)
                {
                    string 藥品碼 = obj_temp_array[i][(int)enum_雲端_藥品資料_DB2.藥品碼].ObjectToString();
                    obj_temp_array_buf = obj_temp_array_result.GetRows((int)enum_雲端_藥品資料_DB2.藥品碼, 藥品碼);
                    if (obj_temp_array_buf.Count == 0)
                    {
                        obj_temp_array_result.Add(obj_temp_array[i]);
                    }
                }


                List<object[]> list_藥品資料 = this.sqL_DataGridView_雲端_藥品資料.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                List<object[]> list_藥品資料_add = new List<object[]>();
                List<object[]> list_藥品資料_replace = new List<object[]>();

                Console.WriteLine($"取得藥品資料!耗時{myTimer.ToString()}ms");

                for (int i = 0; i < obj_temp_array_result.Count; i++)
                {
                    string 藥品碼 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.藥品碼].ObjectToString();
                    string 藥品名稱 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.藥品名稱].ObjectToString();
                    string 藥品學名 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.藥品學名].ObjectToString();
                    string 中文名稱 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.中文名稱].ObjectToString();
                    string 包裝單位 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.包裝單位].ObjectToString();
                    string 包裝數量 = obj_temp_array_result[i][(int)enum_雲端_藥品資料_DB2.包裝數量].ObjectToString();
                    string 最小包裝數量 = "1";
                    if (!包裝數量.StringIsInt32())
                    {
                        if (!包裝數量.StringIsEmpty())
                        {
                            包裝數量 = 包裝數量.Substring(0, 包裝數量.Length - 1);
                        }
                        else
                        {
                            包裝數量 = "1";
                        }
                    }
                    list_藥品資料_buf = list_藥品資料.GetRows((int)enum_雲端_藥品資料.藥品碼, 藥品碼);
                    if (list_藥品資料_buf.Count == 0)
                    {
                        object[] value = new object[new enum_雲端_藥品資料().GetLength()];
                        value[(int)enum_雲端_藥品資料.GUID] = Guid.NewGuid().ToString();
                        value[(int)enum_雲端_藥品資料.藥品碼] = 藥品碼;
                        value[(int)enum_雲端_藥品資料.藥品名稱] = 藥品名稱;
                        value[(int)enum_雲端_藥品資料.藥品學名] = 藥品學名;
                        value[(int)enum_雲端_藥品資料.中文名稱] = 中文名稱;
                        value[(int)enum_雲端_藥品資料.包裝單位] = 包裝單位;
                        value[(int)enum_雲端_藥品資料.包裝數量] = 包裝數量;
                        value[(int)enum_雲端_藥品資料.最小包裝數量] = 最小包裝數量;

                        list_藥品資料_add.Add(value);
                    }
                    else if (list_藥品資料_buf.Count == 1)
                    {
                        bool replace = false;
                        if (list_藥品資料_buf[0][(int)enum_雲端_藥品資料.藥品名稱].ObjectToString() != 藥品名稱)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端_藥品資料.藥品學名].ObjectToString() != 藥品學名)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端_藥品資料.中文名稱].ObjectToString() != 中文名稱)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端_藥品資料.包裝單位].ObjectToString() != 包裝單位)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端_藥品資料.包裝數量].ObjectToString() != 包裝數量)
                        {
                            replace = true;
                        }
                        if (list_藥品資料_buf[0][(int)enum_雲端_藥品資料.最小包裝數量].ObjectToString() != 最小包裝數量)
                        {
                            replace = true;
                        }
                        list_藥品資料_buf[0][(int)enum_雲端_藥品資料.藥品名稱] = 藥品名稱;
                        list_藥品資料_buf[0][(int)enum_雲端_藥品資料.藥品學名] = 藥品學名;
                        list_藥品資料_buf[0][(int)enum_雲端_藥品資料.中文名稱] = 中文名稱;
                        list_藥品資料_buf[0][(int)enum_雲端_藥品資料.包裝單位] = 包裝單位;
                        list_藥品資料_buf[0][(int)enum_雲端_藥品資料.包裝數量] = 包裝數量;
                        list_藥品資料_buf[0][(int)enum_雲端_藥品資料.最小包裝數量] = 最小包裝數量;

                        if (replace)
                        {
                            list_藥品資料_replace.Add(list_藥品資料_buf[0]);
                        }
                    }

                }
                Console.WriteLine($"檢查藥品資料!耗時{myTimer.ToString()}ms");
                this.sqL_DataGridView_雲端_藥品資料.SQL_AddRows(list_藥品資料_add, false);
                Console.WriteLine($"新增藥品資料!共{list_藥品資料_add.Count}筆,耗時{myTimer.ToString()}ms");

                this.sqL_DataGridView_雲端_藥品資料.SQL_ReplaceExtra(list_藥品資料_replace, false);
                Console.WriteLine($"修正藥品資料!共{list_藥品資料_replace.Count}筆,耗時{myTimer.ToString()}ms");
            }
            catch
            {

            }
           

        }
        private void plC_RJ_Button_雲端_藥品資料_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            this.sqL_DataGridView_雲端_藥品資料.SQL_GetAllRows(true);
            Console.WriteLine($"取得藥品資料 耗時 :{myTimer.GetTickTime().ToString("0.000")}");
        }
        private void plC_RJ_Button_雲端_藥品資料_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.saveFileDialog_SaveExcel.ShowDialog();
            }));
            if (dialogResult == DialogResult.OK)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.WaitCursor;
                }));
                DataTable dataTable = this.sqL_DataGridView_雲端_藥品資料.GetDataTable();
                dataTable = dataTable.ReorderTable(new enum_雲端_藥品資料_匯出());
                CSVHelper.SaveFile(dataTable, this.saveFileDialog_SaveExcel.FileName);
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                }));

            }
        }
        private void plC_RJ_Button_雲端_藥品資料_匯入_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult == DialogResult.OK)
            {
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.WaitCursor;
                }));

                DataTable dataTable = new DataTable();
                CSVHelper.LoadFile(this.openFileDialog_LoadExcel.FileName, 0, dataTable);
                DataTable datatable_buf = dataTable.ReorderTable(new enum_雲端_藥品資料_匯入());
                if (datatable_buf == null)
                {
                    this.Invoke(new Action(delegate
                    {
                        MyMessageBox.ShowDialog("匯入檔案,資料錯誤!");
                        this.Cursor = Cursors.Default;
                        return;
                    }));

                }
                List<object[]> list_LoadValue = datatable_buf.DataTableToRowList();
                List<object[]> list_SQL_Value = this.sqL_DataGridView_雲端_藥品資料.SQL_GetAllRows(false);
                List<object[]> list_Add = new List<object[]>();
                List<object[]> list_Add_buf = new List<object[]>();
                List<object[]> list_Delete_ColumnName = new List<object[]>();
                List<object[]> list_Delete_SerchValue = new List<object[]>();
                List<string> list_Replace_SerchValue = new List<string>();
                List<object[]> list_Replace_Value = new List<object[]>();
                List<object[]> list_SQL_Value_buf = new List<object[]>();
                for (int i = 0; i < list_LoadValue.Count; i++)
                {
                    object[] value_load = list_LoadValue[i];
                    value_load = value_load.CopyRow(new enum_雲端_藥品資料_匯入(), new enum_雲端_藥品資料());
                    value_load[(int)enum_雲端_藥品資料.藥品碼] = this.Function_藥品碼檢查(value_load[(int)enum_雲端_藥品資料.藥品碼].ObjectToString());
                    list_SQL_Value_buf = list_SQL_Value.GetRows((int)enum_雲端_藥品資料.藥品碼, value_load[(int)enum_雲端_藥品資料.藥品碼].ObjectToString());
                    if (list_SQL_Value_buf.Count > 0)
                    {
                        object[] value_SQL = list_SQL_Value_buf[0];
                        value_load[(int)enum_雲端_藥品資料.GUID] = value_SQL[(int)enum_雲端_藥品資料.GUID];
                        bool flag_Equal = value_load.IsEqual(value_SQL);
                        if (!flag_Equal)
                        {
                            list_Replace_SerchValue.Add(value_load[(int)enum_雲端_藥品資料.GUID].ObjectToString());
                            list_Replace_Value.Add(value_load);
                        }
                    }
                    else
                    {
                        value_load[(int)enum_雲端_藥品資料.GUID] = Guid.NewGuid().ToString();
                        list_Add_buf = list_Add.GetRows((int)enum_雲端_藥品資料.藥品碼, value_load[(int)enum_雲端_藥品資料.藥品碼].ObjectToString());
                        if (list_Add_buf.Count == 0)
                        {
                            list_Add.Add(value_load);
                        }

                    }
                }
                this.sqL_DataGridView_雲端_藥品資料.SQL_AddRows(list_Add, false);
                this.sqL_DataGridView_雲端_藥品資料.SQL_ReplaceExtra(enum_雲端_藥品資料.GUID.GetEnumName(), list_Replace_SerchValue, list_Replace_Value, false);

                this.sqL_DataGridView_雲端_藥品資料.SQL_GetAllRows(true);
                this.Invoke(new Action(delegate
                {
                    this.Cursor = Cursors.Default;
                    MyMessageBox.ShowDialog("匯入完成!");
                }));
            }
        }
        #endregion
    }
}
