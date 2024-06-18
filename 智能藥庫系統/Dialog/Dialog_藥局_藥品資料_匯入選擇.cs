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
using SQLUI;
using HIS_DB_Lib;
using H_Pannel_lib;

namespace 智能藥庫系統
{
    public partial class Dialog_藥局_藥品資料_匯入選擇 : MyDialog
    {
        public enum enum_表單分類_匯入
        {
            藥碼,
            安全量,
            基準量,
        }
        public enum enum_包裝量_匯入
        {
            藥碼,
            包裝量,
        }
        public Dialog_藥局_藥品資料_匯入選擇()
        {
            InitializeComponent();
            this.plC_RJ_Button_冷藏藥.MouseDownEvent += PlC_RJ_Button_冷藏藥_MouseDownEvent;
            this.plC_RJ_Button_分包機裸錠.MouseDownEvent += PlC_RJ_Button_分包機裸錠_MouseDownEvent;
            this.plC_RJ_Button_高價藥櫃.MouseDownEvent += PlC_RJ_Button_高價藥櫃_MouseDownEvent;
            this.plC_RJ_Button_少用及易混.MouseDownEvent += PlC_RJ_Button_少用及易混_MouseDownEvent;
            this.plC_RJ_Button_水劑.MouseDownEvent += PlC_RJ_Button_水劑_MouseDownEvent;
            this.plC_RJ_Button_口服藥.MouseDownEvent += PlC_RJ_Button_口服藥_MouseDownEvent;
            this.plC_RJ_Button_針劑.MouseDownEvent += PlC_RJ_Button_針劑_MouseDownEvent;
            this.plC_RJ_Button_外用藥.MouseDownEvent += PlC_RJ_Button_外用藥_MouseDownEvent;
            this.plC_RJ_Button_基準安全量全部歸零.MouseDownEvent += PlC_RJ_Button_基準安全量全部歸零_MouseDownEvent;

            this.plC_RJ_Button_包裝量_匯入.MouseDownEvent += PlC_RJ_Button_包裝量_匯入_MouseDownEvent;
            this.plC_RJ_Button_包裝量重置.MouseDownEvent += PlC_RJ_Button_包裝量重置_MouseDownEvent;
        }

        private void PlC_RJ_Button_包裝量重置_MouseDownEvent(MouseEventArgs mevent)
        {
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            if (MyMessageBox.ShowDialog("※注意※ 藥局包裝量會全部重置,是否繼續?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            try
            {


                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<medClass> medClasses_cloud_replace = new List<medClass>();

                for (int i = 0; i < medClasses_pharma.Count; i++)
                {

                    medClasses_pharma[i].包裝數量 = "1";
                    medClasses_pharma_replace.Add(medClasses_pharma[i]);

                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                MyMessageBox.ShowDialog($"更新<{medClasses_pharma_replace.Count}>筆資料成功!");
            }
        }
        private void PlC_RJ_Button_包裝量_匯入_MouseDownEvent(MouseEventArgs mevent)
        {
            string fileName = "";
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            List<object[]> list_包裝量_error = new List<object[]>();

            try
            {
                   
                this.Invoke(new Action(delegate
                {
                    if (this.openFileDialog_LoadExcel.ShowDialog() != DialogResult.OK) return;
                    fileName = this.openFileDialog_LoadExcel.FileName;
                }));

                if (fileName.StringIsEmpty()) return;
                DataTable dataTable_src = MyOffice.ExcelClass.NPOI_LoadFile(fileName);
                if (dataTable_src == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                DataTable dataTable = dataTable_src.ReorderTable(new enum_包裝量_匯入());
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                Dictionary<string, List<medClass>> keyValuePairs_med_pharma = medClasses_pharma.CoverToDictionaryByCode();
                List<object[]> list_包裝量 = dataTable.DataTableToRowList();
                for (int i = 0; i < list_包裝量.Count; i++)
                {
                    string 藥碼 = list_包裝量[i][(int)enum_包裝量_匯入.藥碼].ObjectToString();
                    string 包裝量 = list_包裝量[i][(int)enum_包裝量_匯入.包裝量].ObjectToString();

                    if (包裝量.StringIsInt32() == false)
                    {
                        list_包裝量_error.Add(list_包裝量[i]);
                        continue;
                    }



                    List<medClass> medClasses_pharma_buf = keyValuePairs_med_pharma.SortDictionaryByCode(藥碼);
                    if (medClasses_pharma_buf.Count == 0)
                    {
                        list_包裝量_error.Add(list_包裝量[i]);
                        continue;
                    }


                    medClasses_pharma_buf[0].包裝數量 = 包裝量;
                    medClasses_pharma_replace.Add(medClasses_pharma_buf[0]);
                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                if (fileName.StringIsEmpty() == false) MyMessageBox.ShowDialog($"匯入<{medClasses_pharma_replace.Count}>筆資料成功!<{list_包裝量_error.Count}>筆未成功匯入");
            }
          

        }

        private void PlC_RJ_Button_基準安全量全部歸零_MouseDownEvent(MouseEventArgs mevent)
        {
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            if (MyMessageBox.ShowDialog("※注意※ 藥局基準安全量會全數歸零,是否繼續?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            try
            {


                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<medClass> medClasses_cloud_replace = new List<medClass>();

                for (int i = 0; i < medClasses_pharma.Count; i++)
                {


                    medClasses_pharma[i].安全庫存 = "0";
                    medClasses_pharma[i].基準量 = "0";
                    medClasses_pharma_replace.Add(medClasses_pharma[i]);

                    List<medClass> medClasses_cloud_buf = keyValuePairs_med_cloud.SortDictionaryByCode(medClasses_pharma[i].藥品碼);
                    if (medClasses_cloud_buf.Count > 0)
                    {
                        medClasses_cloud_buf[0].類別 = Main_Form.enum_medType.未分類.GetEnumName();
                        medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                    }

                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                MyMessageBox.ShowDialog($"更新<{medClasses_pharma_replace.Count}>筆資料成功!");
            }
        }
        private void PlC_RJ_Button_外用藥_MouseDownEvent(MouseEventArgs mevent)
        {
            string fileName = "";
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            List<medClass> medClasses_cloud_replace = new List<medClass>();
            List<object[]> list_error = new List<object[]>();

            try
            {

                this.Invoke(new Action(delegate
                {
                    if (this.openFileDialog_LoadExcel.ShowDialog() != DialogResult.OK) return;
                    fileName = this.openFileDialog_LoadExcel.FileName;
                }));

                if (fileName.StringIsEmpty()) return;
                DataTable dataTable_src = MyOffice.ExcelClass.NPOI_LoadFile(fileName);
                if (dataTable_src == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                DataTable dataTable = dataTable_src.ReorderTable(new enum_表單分類_匯入());
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                medClasses_cloud_replace = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_med_pharma = medClasses_pharma.CoverToDictionaryByCode();
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<object[]> list_表單分類 = dataTable.DataTableToRowList();
                for (int i = 0; i < list_表單分類.Count; i++)
                {
                    string 藥碼 = list_表單分類[i][(int)enum_表單分類_匯入.藥碼].ObjectToString();
                    string 安全量 = list_表單分類[i][(int)enum_表單分類_匯入.安全量].ObjectToString();
                    string 基準量 = list_表單分類[i][(int)enum_表單分類_匯入.基準量].ObjectToString();

                    if (安全量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (基準量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    安全量 = Math.Round(安全量.StringToDouble()).ToString();
                    基準量 = Math.Round(基準量.StringToDouble()).ToString();


                    List<medClass> medClasses_pharma_buf = keyValuePairs_med_pharma.SortDictionaryByCode(藥碼);
                    List<medClass> medClasses_cloud_buf = keyValuePairs_med_cloud.SortDictionaryByCode(藥碼);
                    if (medClasses_pharma_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (medClasses_cloud_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }

                    medClasses_pharma_buf[0].安全庫存 = 安全量;
                    medClasses_pharma_buf[0].基準量 = 基準量;
                    medClasses_cloud_buf[0].類別 = Main_Form.enum_medType.外用藥.GetEnumName();
                    medClasses_cloud_buf[0].開檔狀態 = Main_Form.enum_開檔狀態.開檔中.GetEnumName();
                    medClasses_pharma_replace.Add(medClasses_pharma_buf[0]);
                    medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                if (fileName.StringIsEmpty() == false) MyMessageBox.ShowDialog($"匯入<{medClasses_pharma_replace.Count}>筆資料成功!<{list_error.Count}>筆未成功匯入");
            }
        }
        private void PlC_RJ_Button_針劑_MouseDownEvent(MouseEventArgs mevent)
        {
            string fileName = "";
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            List<medClass> medClasses_cloud_replace = new List<medClass>();
            List<object[]> list_error = new List<object[]>();

            try
            {

                this.Invoke(new Action(delegate
                {
                    if (this.openFileDialog_LoadExcel.ShowDialog() != DialogResult.OK) return;
                    fileName = this.openFileDialog_LoadExcel.FileName;
                }));

                if (fileName.StringIsEmpty()) return;
                DataTable dataTable_src = MyOffice.ExcelClass.NPOI_LoadFile(fileName);
                if (dataTable_src == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                DataTable dataTable = dataTable_src.ReorderTable(new enum_表單分類_匯入());
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                medClasses_cloud_replace = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_med_pharma = medClasses_pharma.CoverToDictionaryByCode();
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<object[]> list_表單分類 = dataTable.DataTableToRowList();
                for (int i = 0; i < list_表單分類.Count; i++)
                {
                    string 藥碼 = list_表單分類[i][(int)enum_表單分類_匯入.藥碼].ObjectToString();
                    string 安全量 = list_表單分類[i][(int)enum_表單分類_匯入.安全量].ObjectToString();
                    string 基準量 = list_表單分類[i][(int)enum_表單分類_匯入.基準量].ObjectToString();

                    if (安全量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (基準量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    安全量 = Math.Round(安全量.StringToDouble()).ToString();
                    基準量 = Math.Round(基準量.StringToDouble()).ToString();


                    List<medClass> medClasses_pharma_buf = keyValuePairs_med_pharma.SortDictionaryByCode(藥碼);
                    List<medClass> medClasses_cloud_buf = keyValuePairs_med_cloud.SortDictionaryByCode(藥碼);
                    if (medClasses_pharma_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (medClasses_cloud_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }

                    medClasses_pharma_buf[0].安全庫存 = 安全量;
                    medClasses_pharma_buf[0].基準量 = 基準量;
                    medClasses_cloud_buf[0].類別 = Main_Form.enum_medType.針劑.GetEnumName();
                    medClasses_cloud_buf[0].開檔狀態 = Main_Form.enum_開檔狀態.開檔中.GetEnumName();
                    medClasses_pharma_replace.Add(medClasses_pharma_buf[0]);
                    medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                if (fileName.StringIsEmpty() == false) MyMessageBox.ShowDialog($"匯入<{medClasses_pharma_replace.Count}>筆資料成功!<{list_error.Count}>筆未成功匯入");
            }
        }
        private void PlC_RJ_Button_口服藥_MouseDownEvent(MouseEventArgs mevent)
        {
            string fileName = "";
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            List<medClass> medClasses_cloud_replace = new List<medClass>();
            List<object[]> list_error = new List<object[]>();

            try
            {

                this.Invoke(new Action(delegate
                {
                    if (this.openFileDialog_LoadExcel.ShowDialog() != DialogResult.OK) return;
                    fileName = this.openFileDialog_LoadExcel.FileName;
                }));

                if (fileName.StringIsEmpty()) return;
                DataTable dataTable_src = MyOffice.ExcelClass.NPOI_LoadFile(fileName);
                if (dataTable_src == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                DataTable dataTable = dataTable_src.ReorderTable(new enum_表單分類_匯入());
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                medClasses_cloud_replace = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_med_pharma = medClasses_pharma.CoverToDictionaryByCode();
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<object[]> list_表單分類 = dataTable.DataTableToRowList();
                for (int i = 0; i < list_表單分類.Count; i++)
                {
                    string 藥碼 = list_表單分類[i][(int)enum_表單分類_匯入.藥碼].ObjectToString();
                    string 安全量 = list_表單分類[i][(int)enum_表單分類_匯入.安全量].ObjectToString();
                    string 基準量 = list_表單分類[i][(int)enum_表單分類_匯入.基準量].ObjectToString();

                    if (安全量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (基準量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    安全量 = Math.Round(安全量.StringToDouble()).ToString();
                    基準量 = Math.Round(基準量.StringToDouble()).ToString();


                    List<medClass> medClasses_pharma_buf = keyValuePairs_med_pharma.SortDictionaryByCode(藥碼);
                    List<medClass> medClasses_cloud_buf = keyValuePairs_med_cloud.SortDictionaryByCode(藥碼);
                    if (medClasses_pharma_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (medClasses_cloud_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }

                    medClasses_pharma_buf[0].安全庫存 = 安全量;
                    medClasses_pharma_buf[0].基準量 = 基準量;
                    medClasses_cloud_buf[0].類別 = Main_Form.enum_medType.口服藥.GetEnumName();
                    medClasses_cloud_buf[0].開檔狀態 = Main_Form.enum_開檔狀態.開檔中.GetEnumName();
                    medClasses_pharma_replace.Add(medClasses_pharma_buf[0]);
                    medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                if (fileName.StringIsEmpty() == false) MyMessageBox.ShowDialog($"匯入<{medClasses_pharma_replace.Count}>筆資料成功!<{list_error.Count}>筆未成功匯入");
            }
        }
        private void PlC_RJ_Button_水劑_MouseDownEvent(MouseEventArgs mevent)
        {
            string fileName = "";
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            List<medClass> medClasses_cloud_replace = new List<medClass>();
            List<object[]> list_error = new List<object[]>();

            try
            {

                this.Invoke(new Action(delegate
                {
                    if (this.openFileDialog_LoadExcel.ShowDialog() != DialogResult.OK) return;
                    fileName = this.openFileDialog_LoadExcel.FileName;
                }));

                if (fileName.StringIsEmpty()) return;
                DataTable dataTable_src = MyOffice.ExcelClass.NPOI_LoadFile(fileName);
                if (dataTable_src == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                DataTable dataTable = dataTable_src.ReorderTable(new enum_表單分類_匯入());
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                medClasses_cloud_replace = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_med_pharma = medClasses_pharma.CoverToDictionaryByCode();
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<object[]> list_表單分類 = dataTable.DataTableToRowList();
                for (int i = 0; i < list_表單分類.Count; i++)
                {
                    string 藥碼 = list_表單分類[i][(int)enum_表單分類_匯入.藥碼].ObjectToString();
                    string 安全量 = list_表單分類[i][(int)enum_表單分類_匯入.安全量].ObjectToString();
                    string 基準量 = list_表單分類[i][(int)enum_表單分類_匯入.基準量].ObjectToString();

                    if (安全量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (基準量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    安全量 = Math.Round(安全量.StringToDouble()).ToString();
                    基準量 = Math.Round(基準量.StringToDouble()).ToString();


                    List<medClass> medClasses_pharma_buf = keyValuePairs_med_pharma.SortDictionaryByCode(藥碼);
                    List<medClass> medClasses_cloud_buf = keyValuePairs_med_cloud.SortDictionaryByCode(藥碼);
                    if (medClasses_pharma_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (medClasses_cloud_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }

                    medClasses_pharma_buf[0].安全庫存 = 安全量;
                    medClasses_pharma_buf[0].基準量 = 基準量;
                    medClasses_cloud_buf[0].類別 = Main_Form.enum_medType.水劑.GetEnumName();
                    medClasses_cloud_buf[0].開檔狀態 = Main_Form.enum_開檔狀態.開檔中.GetEnumName();
                    medClasses_pharma_replace.Add(medClasses_pharma_buf[0]);
                    medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                if (fileName.StringIsEmpty() == false) MyMessageBox.ShowDialog($"匯入<{medClasses_pharma_replace.Count}>筆資料成功!<{list_error.Count}>筆未成功匯入");
            }
        }
        private void PlC_RJ_Button_少用及易混_MouseDownEvent(MouseEventArgs mevent)
        {
            string fileName = "";
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            List<medClass> medClasses_cloud_replace = new List<medClass>();
            List<object[]> list_error = new List<object[]>();

            try
            {

                this.Invoke(new Action(delegate
                {
                    if (this.openFileDialog_LoadExcel.ShowDialog() != DialogResult.OK) return;
                    fileName = this.openFileDialog_LoadExcel.FileName;
                }));

                if (fileName.StringIsEmpty()) return;
                DataTable dataTable_src = MyOffice.ExcelClass.NPOI_LoadFile(fileName);
                if (dataTable_src == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                DataTable dataTable = dataTable_src.ReorderTable(new enum_表單分類_匯入());
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                medClasses_cloud_replace = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_med_pharma = medClasses_pharma.CoverToDictionaryByCode();
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<object[]> list_表單分類 = dataTable.DataTableToRowList();
                for (int i = 0; i < list_表單分類.Count; i++)
                {
                    string 藥碼 = list_表單分類[i][(int)enum_表單分類_匯入.藥碼].ObjectToString();
                    string 安全量 = list_表單分類[i][(int)enum_表單分類_匯入.安全量].ObjectToString();
                    string 基準量 = list_表單分類[i][(int)enum_表單分類_匯入.基準量].ObjectToString();

                    if (安全量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (基準量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    安全量 = Math.Round(安全量.StringToDouble()).ToString();
                    基準量 = Math.Round(基準量.StringToDouble()).ToString();


                    List<medClass> medClasses_pharma_buf = keyValuePairs_med_pharma.SortDictionaryByCode(藥碼);
                    List<medClass> medClasses_cloud_buf = keyValuePairs_med_cloud.SortDictionaryByCode(藥碼);
                    if (medClasses_pharma_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (medClasses_cloud_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }

                    medClasses_pharma_buf[0].安全庫存 = 安全量;
                    medClasses_pharma_buf[0].基準量 = 基準量;
                    medClasses_cloud_buf[0].類別 = Main_Form.enum_medType.少用及易混.GetEnumName();
                    medClasses_cloud_buf[0].開檔狀態 = Main_Form.enum_開檔狀態.開檔中.GetEnumName();
                    medClasses_pharma_replace.Add(medClasses_pharma_buf[0]);
                    medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                if (fileName.StringIsEmpty() == false) MyMessageBox.ShowDialog($"匯入<{medClasses_pharma_replace.Count}>筆資料成功!<{list_error.Count}>筆未成功匯入");
            }
        }
        private void PlC_RJ_Button_高價藥櫃_MouseDownEvent(MouseEventArgs mevent)
        {
            string fileName = "";
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            List<medClass> medClasses_cloud_replace = new List<medClass>();
            List<object[]> list_error = new List<object[]>();

            try
            {

                this.Invoke(new Action(delegate
                {
                    if (this.openFileDialog_LoadExcel.ShowDialog() != DialogResult.OK) return;
                    fileName = this.openFileDialog_LoadExcel.FileName;
                }));

                if (fileName.StringIsEmpty()) return;
                DataTable dataTable_src = MyOffice.ExcelClass.NPOI_LoadFile(fileName);
                if (dataTable_src == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                DataTable dataTable = dataTable_src.ReorderTable(new enum_表單分類_匯入());
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                medClasses_cloud_replace = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_med_pharma = medClasses_pharma.CoverToDictionaryByCode();
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<object[]> list_表單分類 = dataTable.DataTableToRowList();
                for (int i = 0; i < list_表單分類.Count; i++)
                {
                    string 藥碼 = list_表單分類[i][(int)enum_表單分類_匯入.藥碼].ObjectToString();
                    string 安全量 = list_表單分類[i][(int)enum_表單分類_匯入.安全量].ObjectToString();
                    string 基準量 = list_表單分類[i][(int)enum_表單分類_匯入.基準量].ObjectToString();

                    if (安全量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (基準量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    安全量 = Math.Round(安全量.StringToDouble()).ToString();
                    基準量 = Math.Round(基準量.StringToDouble()).ToString();


                    List<medClass> medClasses_pharma_buf = keyValuePairs_med_pharma.SortDictionaryByCode(藥碼);
                    List<medClass> medClasses_cloud_buf = keyValuePairs_med_cloud.SortDictionaryByCode(藥碼);
                    if (medClasses_pharma_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (medClasses_cloud_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }

                    medClasses_pharma_buf[0].安全庫存 = 安全量;
                    medClasses_pharma_buf[0].基準量 = 基準量;
                    medClasses_cloud_buf[0].類別 = Main_Form.enum_medType.高價藥櫃.GetEnumName();
                    medClasses_cloud_buf[0].開檔狀態 = Main_Form.enum_開檔狀態.開檔中.GetEnumName();
                    medClasses_pharma_replace.Add(medClasses_pharma_buf[0]);
                    medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                if (fileName.StringIsEmpty() == false) MyMessageBox.ShowDialog($"匯入<{medClasses_pharma_replace.Count}>筆資料成功!<{list_error.Count}>筆未成功匯入");
            }
        }
        private void PlC_RJ_Button_分包機裸錠_MouseDownEvent(MouseEventArgs mevent)
        {
            string fileName = "";
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            List<medClass> medClasses_cloud_replace = new List<medClass>();
            List<object[]> list_error = new List<object[]>();

            try
            {

                this.Invoke(new Action(delegate
                {
                    if (this.openFileDialog_LoadExcel.ShowDialog() != DialogResult.OK) return;
                    fileName = this.openFileDialog_LoadExcel.FileName;
                }));

                if (fileName.StringIsEmpty()) return;
                DataTable dataTable_src = MyOffice.ExcelClass.NPOI_LoadFile(fileName);
                if (dataTable_src == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                DataTable dataTable = dataTable_src.ReorderTable(new enum_表單分類_匯入());
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                medClasses_cloud_replace = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_med_pharma = medClasses_pharma.CoverToDictionaryByCode();
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<object[]> list_表單分類 = dataTable.DataTableToRowList();
                for (int i = 0; i < list_表單分類.Count; i++)
                {
                    string 藥碼 = list_表單分類[i][(int)enum_表單分類_匯入.藥碼].ObjectToString();
                    string 安全量 = list_表單分類[i][(int)enum_表單分類_匯入.安全量].ObjectToString();
                    string 基準量 = list_表單分類[i][(int)enum_表單分類_匯入.基準量].ObjectToString();

                    if (安全量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (基準量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    安全量 = Math.Round(安全量.StringToDouble()).ToString();
                    基準量 = Math.Round(基準量.StringToDouble()).ToString();


                    List<medClass> medClasses_pharma_buf = keyValuePairs_med_pharma.SortDictionaryByCode(藥碼);
                    List<medClass> medClasses_cloud_buf = keyValuePairs_med_cloud.SortDictionaryByCode(藥碼);
                    if (medClasses_pharma_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (medClasses_cloud_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }

                    medClasses_pharma_buf[0].安全庫存 = 安全量;
                    medClasses_pharma_buf[0].基準量 = 基準量;
                    medClasses_cloud_buf[0].類別 = Main_Form.enum_medType.分包機裸錠.GetEnumName();
                    medClasses_cloud_buf[0].開檔狀態 = Main_Form.enum_開檔狀態.開檔中.GetEnumName();
                    medClasses_pharma_replace.Add(medClasses_pharma_buf[0]);
                    medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                if (fileName.StringIsEmpty() == false) MyMessageBox.ShowDialog($"匯入<{medClasses_pharma_replace.Count}>筆資料成功!<{list_error.Count}>筆未成功匯入");
            }
        }
        private void PlC_RJ_Button_冷藏藥_MouseDownEvent(MouseEventArgs mevent)
        {
            string fileName = "";
            List<medClass> medClasses_pharma_replace = new List<medClass>();
            List<medClass> medClasses_cloud_replace = new List<medClass>();
            List<object[]> list_error = new List<object[]>();
            try
            {

                this.Invoke(new Action(delegate
                {
                    if (this.openFileDialog_LoadExcel.ShowDialog() != DialogResult.OK) return;
                    fileName = this.openFileDialog_LoadExcel.FileName;
                }));

                if (fileName.StringIsEmpty()) return;
                DataTable dataTable_src = MyOffice.ExcelClass.NPOI_LoadFile(fileName);
                if (dataTable_src == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                DataTable dataTable = dataTable_src.ReorderTable(new enum_表單分類_匯入());
                if (dataTable == null)
                {
                    MyMessageBox.ShowDialog("匯入失敗");
                    return;
                }
                LoadingForm.ShowLoadingForm();
                List<medClass> medClasses_pharma = medClass.get_ds_pharma_med(Main_Form.API_Server, "ds01");
                medClasses_pharma_replace = new List<medClass>();
                List<medClass> medClasses_cloud = medClass.get_med_cloud(Main_Form.API_Server);
                medClasses_cloud_replace = new List<medClass>();
                Dictionary<string, List<medClass>> keyValuePairs_med_pharma = medClasses_pharma.CoverToDictionaryByCode();
                Dictionary<string, List<medClass>> keyValuePairs_med_cloud = medClasses_cloud.CoverToDictionaryByCode();
                List<object[]> list_表單分類 = dataTable.DataTableToRowList();
                for (int i = 0; i < list_表單分類.Count; i++)
                {
                    string 藥碼 = list_表單分類[i][(int)enum_表單分類_匯入.藥碼].ObjectToString();
                    string 安全量 = list_表單分類[i][(int)enum_表單分類_匯入.安全量].ObjectToString();
                    string 基準量 = list_表單分類[i][(int)enum_表單分類_匯入.基準量].ObjectToString();



                    if (安全量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (基準量.StringIsDouble() == false)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    安全量 = Math.Round(安全量.StringToDouble()).ToString();
                    基準量 = Math.Round(基準量.StringToDouble()).ToString();


                    List<medClass> medClasses_pharma_buf = keyValuePairs_med_pharma.SortDictionaryByCode(藥碼);
                    List<medClass> medClasses_cloud_buf = keyValuePairs_med_cloud.SortDictionaryByCode(藥碼);
                    if (medClasses_pharma_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }
                    if (medClasses_cloud_buf.Count == 0)
                    {
                        list_error.Add(list_表單分類[i]);
                        continue;
                    }

                    medClasses_pharma_buf[0].安全庫存 = 安全量;
                    medClasses_pharma_buf[0].基準量 = 基準量;
                    medClasses_cloud_buf[0].類別 = Main_Form.enum_medType.冷藏藥.GetEnumName();
                    medClasses_cloud_buf[0].開檔狀態 = Main_Form.enum_開檔狀態.開檔中.GetEnumName();
                    medClasses_pharma_replace.Add(medClasses_pharma_buf[0]);
                    medClasses_cloud_replace.Add(medClasses_cloud_buf[0]);
                }
                medClass.update_ds_pharma_by_guid(Main_Form.API_Server, "ds01", medClasses_pharma_replace);
                medClass.update_med_clouds_by_guid(Main_Form.API_Server, medClasses_cloud_replace);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
            }
            finally
            {
                LoadingForm.CloseLoadingForm();
                if (fileName.StringIsEmpty() == false) MyMessageBox.ShowDialog($"匯入<{medClasses_pharma_replace.Count}>筆資料成功!<{list_error.Count}>筆未成功匯入");
            }


        }
    }
}
