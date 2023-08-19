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
    public partial class Dialog_效期批號歷史紀錄 : Form
    {
        public enum enum_效期及批號
        {
            效期,
            批號,
        }
        private List<string> _list_效期 = new List<string>();
        private List<string> _list_批號 = new List<string>();

        private string _藥名 = "0";
        public string 藥名
        {

            set
            {
                this.rJ_Lable_藥名.Text = $" 藥名 :{value}";
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
        public Dialog_效期批號歷史紀錄(string 藥名,List<string> list_效期, List<string> list_批號)
        {
            form.Invoke(new Action(delegate
            {
                InitializeComponent();
            }));
            this.rJ_Button_返回.MouseDownEvent += RJ_Button_返回_MouseDownEvent;
            _list_效期 = list_效期;
            _list_批號 = list_批號;
            _藥名 = 藥名;
       
            this.Load += Dialog_效期批號歷史紀錄_Load;
        }

 
        private void Dialog_效期批號歷史紀錄_Load(object sender, EventArgs e)
        {
            this.sqL_DataGridView_效期及批號.Init();
            藥名 = _藥名;
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < _list_效期.Count; i++)
            {
                object[] value = new object[new enum_效期及批號().GetLength()];
                value[(int)enum_效期及批號.效期] = _list_效期[i];
                value[(int)enum_效期及批號.批號] = _list_批號[i];
                list_value.Add(value);

            }

            this.sqL_DataGridView_效期及批號.RefreshGrid(list_value);
        }
        private void RJ_Button_返回_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                this.Close();
            }));
        }

    }
}
