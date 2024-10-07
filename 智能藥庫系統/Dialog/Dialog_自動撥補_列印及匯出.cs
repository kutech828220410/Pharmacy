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
using HIS_DB_Lib;
using MyUI;
namespace 智能藥庫系統
{
    public partial class Dialog_自動撥補_列印及匯出 : MyDialog
    {
        public List<drugStotreDistributionClass> Value
        {
            get
            {
                return _drugStotreDistributionClasses;
            }
        }
        private PrinterClass printerClass = new PrinterClass();
        private List<drugStotreDistributionClass> _drugStotreDistributionClasses;
        public Dialog_自動撥補_列印及匯出(List<drugStotreDistributionClass> drugStotreDistributionClasses)
        {
            InitializeComponent();

            _drugStotreDistributionClasses = drugStotreDistributionClasses;

            this.Load += Dialog_自動撥補_列印及匯出_Load;
            this.button_列印.Click += Button_列印_Click;
            this.button_預覽列印.Click += Button_預覽列印_Click;
            this.button_匯出.Click += Button_匯出_Click;
        }

        private void Dialog_自動撥補_列印及匯出_Load(object sender, EventArgs e)
        {
            printerClass.Init();
            printerClass.PrintPageEvent += PrinterClass_PrintPageEvent;
        }

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
            this.TopMost = false;
            this.Close();
            if (printerClass.ShowPreviewDialog(sheetClass, MyPrinterlib.PrinterClass.PageSize.A4) == DialogResult.OK)
            {

            }

        }
        private void Button_列印_Click(object sender, EventArgs e)
        {
            List<SheetClass> sheetClass = this.GetSheetClasses(true);

            printerClass.Print(sheetClass, PrinterClass.PageSize.A4);
            this.Close();
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


        private void saveFileDialogl()
        {
            DialogResult dialogResult = DialogResult.None;
            dialogResult = this.saveFileDialog_SaveExcel.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {

                List<SheetClass> sheetClass = this.GetSheetClasses(false);
                for (int i = 0; i < sheetClass.Count; i++)
                {
                    sheetClass[i].Name = $"{i}";
                }
                sheetClass.NPOI_SaveFile(this.saveFileDialog_SaveExcel.FileName);
                MyMessageBox.ShowDialog("匯出完成!");
                this.Close();
            }
        }
        private List<SheetClass> GetSheetClasses(bool 設定已列印)
        {

            List<class_emg_apply> class_Emg_Applies = new List<class_emg_apply>();
            for (int i = 0; i < _drugStotreDistributionClasses.Count; i++)
            {
                class_emg_apply class_Emg_Apply = new class_emg_apply();
                class_Emg_Apply.成本中心 = _drugStotreDistributionClasses[i].目的庫別;
                class_Emg_Apply.藥品碼 = _drugStotreDistributionClasses[i].藥碼;
                class_Emg_Apply.藥品名稱 = _drugStotreDistributionClasses[i].藥名;
                class_Emg_Apply.撥出量 = _drugStotreDistributionClasses[i].實撥量;
                class_Emg_Apply.庫存量 = _drugStotreDistributionClasses[i].來源庫庫存;
                class_Emg_Apply.備註 = _drugStotreDistributionClasses[i].報表名稱;
                class_Emg_Applies.Add(class_Emg_Apply);
            }

            string json_in = class_Emg_Applies.JsonSerializationt(true);
            string json = Basic.Net.WEBApiPostJson($"{Main_Form.dBConfigClass.Emg_apply_ApiURL}", json_in);
            List<SheetClass> sheetClass = json.JsonDeserializet<List<SheetClass>>();

            if (設定已列印)
            {
                for (int i = 0; i < _drugStotreDistributionClasses.Count; i++)
                {
                    if (_drugStotreDistributionClasses[i].狀態 == enum_藥庫_撥補_藥局_緊急申領_狀態.過帳完成.GetEnumName()) continue;
                    _drugStotreDistributionClasses[i].狀態= enum_藥庫_撥補_藥局_緊急申領_狀態.已列印.GetEnumName();
                }

                drugStotreDistributionClass.update_by_guid(Main_Form.API_Server, _drugStotreDistributionClasses);

            }


            return sheetClass;
        }
    }
}
