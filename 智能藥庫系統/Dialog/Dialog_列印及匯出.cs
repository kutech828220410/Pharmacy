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
using MyOffice;
using MyPrinterlib;
using SQLUI;
using System.Threading;
namespace 智能藥庫系統
{
    public partial class Dialog_列印及匯出 : Form
    {
        private PrinterClass printerClass = new PrinterClass();
        private string emg_apply_ApiURL = "";
        private SQL_DataGridView sQL_DataGridView;
        public static Form form;
        public new DialogResult ShowDialog()
        {
            if (form == null)
            {
                base.ShowDialog();
            }
            else
            {
                form.Invoke(new Action(delegate
                {
                    base.ShowDialog();
                }));
            }

            return this.DialogResult;
        }

        public Dialog_列印及匯出(SQL_DataGridView sQL_DataGridView, string emg_apply_ApiURL)
        {
            InitializeComponent();
            this.emg_apply_ApiURL = emg_apply_ApiURL;
            this.sQL_DataGridView = sQL_DataGridView;
        }

        private void Dialog_列印及匯出_Load(object sender, EventArgs e)
        {
            this.button_列印.Click += Button_列印_Click;
            this.button_預覽列印.Click += Button_預覽列印_Click;
            this.button_匯出.Click += Button_匯出_Click;
            this.rJ_Button_取消.MouseDownEvent += RJ_Button_取消_MouseDownEvent;

            printerClass.Init();
            printerClass.PrintPageEvent += PrinterClass_PrintPageEvent;
        }



        private List<SheetClass> GetSheetClasses(bool 設定已列印)
        {
            List<object[]> list_value = this.sQL_DataGridView.Get_All_Checked_RowsValues();

            List<class_emg_apply> class_Emg_Applies = new List<class_emg_apply>();
            for (int i = 0; i < list_value.Count; i++)
            {
                class_emg_apply class_Emg_Apply = new class_emg_apply();
                class_Emg_Apply.成本中心 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥局代碼].ObjectToString();
                class_Emg_Apply.藥品碼 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品碼].ObjectToString();
                class_Emg_Apply.藥品名稱 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.藥品名稱].ObjectToString();
                class_Emg_Apply.撥出量 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.異動量].ObjectToString();
                class_Emg_Apply.庫存量 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.庫存].ObjectToString();
                class_Emg_Apply.備註 = list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.備註].ObjectToString();
                class_Emg_Applies.Add(class_Emg_Apply);
            }

            string json_in = class_Emg_Applies.JsonSerializationt(true);
            string json = Basic.Net.WEBApiPostJson($"{emg_apply_ApiURL}", json_in);
            List<SheetClass> sheetClass = json.JsonDeserializet<List<SheetClass>>();

            if (設定已列印)
            {
                for (int i = 0; i < list_value.Count; i++)
                {
                    if (list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] == enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()) continue;
                    list_value[i][(int)enum_藥庫_撥補_藥局_緊急申領.狀態] = enum_藥庫_撥補_藥局_緊急申領_狀態.已列印.GetEnumName();
                }
                this.sQL_DataGridView.SQL_ReplaceExtra(list_value, false);
                this.sQL_DataGridView.ReplaceExtra(list_value, true);
            }
         


            return sheetClass;
        }


        private void saveFileDialogl()
        {
            DialogResult dialogResult = DialogResult.None;
            dialogResult = this.saveFileDialog_SaveExcel.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {

                List<SheetClass> sheetClass = this.GetSheetClasses(false);

                sheetClass.NPOI_SaveFile(this.saveFileDialog_SaveExcel.FileName);
                MyMessageBox.ShowDialog("匯出完成!");
                RJ_Button_取消_MouseDownEvent(null);
            }
        }
        #region Event

        private void Button_匯出_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(saveFileDialogl));

            thread.SetApartmentState(ApartmentState.STA); //重點

            thread.Start();
           
        }
        private void Button_預覽列印_Click(object sender, EventArgs e)
        {

            List<SheetClass> sheetClass = this.GetSheetClasses(false);
            this.GetSheetClasses(true);
            if (printerClass.ShowPreviewDialog(sheetClass, MyPrinterlib.PrinterClass.PageSize.A4) == DialogResult.OK)
            {
               
            }
            RJ_Button_取消_MouseDownEvent(null);
        }
        private void Button_列印_Click(object sender, EventArgs e)
        {
            List<SheetClass> sheetClass = this.GetSheetClasses(true);

            printerClass.Print(sheetClass, PrinterClass.PageSize.A4);
            RJ_Button_取消_MouseDownEvent(null);

        }

        private void RJ_Button_取消_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Close();
            }));
        }
  
        private void PrinterClass_PrintPageEvent(object sender, Graphics g, int width, int height, int page_num)
        {
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            using (Bitmap bitmap = printerClass.GetSheetClass(page_num).GetBitmap(width, height, 0.75, H_Alignment.Center, V_Alignment.Top, 0, 50))
            {
                rectangle.Height = bitmap.Height;
                g.DrawImage(bitmap, rectangle);
            }
        }
        #endregion
    }
}
