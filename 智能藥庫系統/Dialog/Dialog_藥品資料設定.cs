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
    public partial class Dialog_藥品資料設定 : Form
    {
        private string _藥名 = "0";
        public string 藥名
        {

            set
            {
                this.rJ_Lable_藥名.Text = $" 藥名 :{value}";
            }
        }
        private string _包裝數量 = "0";
        public string 包裝數量
        {
            get
            {
                return this.rJ_TextBox_包裝數量.Texts;
            }
            set
            {
                this.rJ_TextBox_包裝數量.Texts = value;
            }
        }
        private string _基準量 = "0";
        public string 基準量
        {
            get
            {
                return this.rJ_TextBox_基準量.Texts;
            }
            set
            {
                this.rJ_TextBox_基準量.Texts = value;
            }
        }
        private string _安全量 = "0";
        public string 安全量
        {
            get
            {
                return this.rJ_TextBox_安全量.Texts;
            }
            set
            {
                this.rJ_TextBox_安全量.Texts = value;
            }
        }
        private string _儲位名稱 = "0";
        public string 儲位名稱
        {
            get
            {
                return this.rJ_TextBox_儲位名稱.Texts;
            }
            set
            {
                this.rJ_TextBox_儲位名稱.Texts = value;
            }
        }
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

        public Dialog_藥品資料設定(string 藥名,int 安全量, int 基準量 ,int 包裝數量, string 儲位名稱)
        {
            InitializeComponent();
            this.Load += Dialog_藥品資料設定_Load;
            _藥名 = 藥名;
            _安全量 = 安全量.ToString();
            _基準量 = 基準量.ToString();
            _包裝數量 = 包裝數量.ToString();
            _儲位名稱 = 儲位名稱;
        }

        private void Dialog_藥品資料設定_Load(object sender, EventArgs e)
        {
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_Button_取消.MouseDownEvent += RJ_Button_取消_MouseDownEvent;
            this.藥名 = _藥名;
            this.rJ_TextBox_基準量.Texts = _基準量;
            this.rJ_TextBox_安全量.Texts = _安全量;
            this.rJ_TextBox_包裝數量.Texts = _包裝數量;
            this.rJ_TextBox_儲位名稱.Texts = _儲位名稱;
        }

        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                if(基準量.StringIsInt32() == false || 安全量.StringIsInt32() == false || 包裝數量.StringIsInt32() == false)
                {
                    MyMessageBox.ShowDialog("請輸入正確數字!");
                    return;
                }
                if(包裝數量.StringToInt32() <= 0)
                {
                    MyMessageBox.ShowDialog("包裝數量不得小於'0'!");
                    return;
                }

                if (基準量.StringToInt32() < 安全量.StringToInt32())
                {
                    MyMessageBox.ShowDialog("基準量不得小於安全量!");
                    return;
                }
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }));
        }
        private void RJ_Button_取消_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.No;
                this.Close();

            }));
        }
    }
}
