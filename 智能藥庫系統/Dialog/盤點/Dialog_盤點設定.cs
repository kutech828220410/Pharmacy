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
using MyUI;
using Basic;
using H_Pannel_lib;
using SQLUI;
using HIS_DB_Lib;

namespace 智能藥庫系統
{
    public partial class Dialog_盤點設定 : MyDialog
    {
        public List<string> 預設盤點人 = new List<string>();
        private List<RJ_TextBox> rJ_TextBoxes = new List<RJ_TextBox>();
        public Dialog_盤點設定(params string[] 預設盤點人)
        {
            if(預設盤點人 != null)
            {
                for (int i = 0; i < 預設盤點人.Length; i++)
                {
                    this.預設盤點人.Add(預設盤點人[i]);
                }
            }
          

            InitializeComponent();
            this.CancelButton = this.plC_RJ_Button_返回;
            this.plC_RJ_Button_返回.MouseDownEvent += PlC_RJ_Button_返回_MouseDownEvent;
            this.plC_RJ_Button_確認.MouseDownEvent += PlC_RJ_Button_確認_MouseDownEvent;
            this.Load += Dialog_盤點設定_Load;
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_01);
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_02);
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_03);
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_04);
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_05);
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_06);
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_07);
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_08);
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_09);
            rJ_TextBoxes.Add(rJ_TextBox_預設盤點人_10);
        }
        #region Evnet
        private void Dialog_盤點設定_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < 預設盤點人.Count; i++)
            {
                rJ_TextBoxes[i].Texts = 預設盤點人[i];
            }
        }
        private void PlC_RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            預設盤點人.Clear();
            for (int i = 0; i < rJ_TextBoxes.Count; i++)
            {
                if (rJ_TextBoxes[i].Texts.StringIsEmpty() == false)
                {
                    預設盤點人.Add(rJ_TextBoxes[i].Texts);
                }
            }
            DialogResult = DialogResult.Yes;
            this.Close();  
        }
        private void PlC_RJ_Button_返回_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult = DialogResult.No;
            this.Close();     
        }
        #endregion
    }
}
