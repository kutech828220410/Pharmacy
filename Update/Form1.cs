using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Update
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if (Basic.MyMessageBox.ShowDialog("是否執行系統更新?下載完成,系統將會關閉!", "Update", Basic.MyMessageBox.enum_BoxType.Asterisk, Basic.MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
            {
                if (this.ftp_DounloadUI1.DownloadFile())
                {
                    if (this.ftp_DounloadUI1.SaveFile())
                    {
                        this.ftp_DounloadUI1.RunFile(this.FindForm());
                    }
                    else
                    {
                        Basic.MyMessageBox.ShowDialog("安裝檔存檔失敗!");
                    }
                }
                else
                {
                    Basic.MyMessageBox.ShowDialog("下載失敗!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
