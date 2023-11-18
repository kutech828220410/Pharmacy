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
using MyUI;
namespace 智能藥庫系統
{
    public partial class Dialog_日期選擇 : Form
    {
        private string title = "";
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

        public DateTime Value
        {
            get
            {
                return dateTimeComList.Value;
            }
        }

        public Dialog_日期選擇(string Title)
        {
            InitializeComponent();
            this.Load += Dialog_日期選擇_Load;
            title = Title;
        }

        private void Dialog_日期選擇_Load(object sender, EventArgs e)
        {
            this.Text = title;
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.dateTimeComList.Height = 50;
        }

        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }));
        }
    }
}
