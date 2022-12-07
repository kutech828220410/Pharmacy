using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 智能藥庫系統
{
    public partial class Dialog_輸入備註 : Form
    {
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private string value = "";
        public string Value
        {
            get
            {
                return this.rJ_TextBox_備註.Texts;
            }
            set
            {
                this.rJ_TextBox_備註.Texts = value;
            }
        }
        public Dialog_輸入備註()
        {
            InitializeComponent();
        }

        private void Dialog_輸入備註_Load(object sender, EventArgs e)
        {
            this.Value = value;
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_Button_取消.MouseDownEvent += RJ_Button_取消_MouseDownEvent;
        }
        private void RJ_Button_取消_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
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
