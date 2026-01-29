//using DocumentFormat.OpenXml.Spreadsheet;
using DrLogy.DrLogyPDFMailerUtils;
using DrLogy.DrLogyPDFUtils;
using DrLogy.DrLogyUtils;
using PDFMailer;
using PDFMailer.Properties;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace PDFMailer
{
    public partial class frmMain : Form
    {
        private string[] DATA_FILENAME = { "PDFMailer.xml", "806Mailer.xml" };

        private int _currType = -1;
        private PDFProcessor _processor = null;

        private enum pdfType
        {
            PdfPerut = 0,
            Pdf806 = 1
        }
        private bool blnCancel;
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();

                fileDialog.Title = "בחרו קובץ";
                fileDialog.Filter = "PDF|*.PDF";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtSource.Text = fileDialog.FileName;
                }
            }

            catch (Exception ex)
            {
                LogError("btnChooseFile_Click", ex);
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            try
            {

                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDestFolder.Text = folderBrowserDialog.SelectedPath;
                }
            }

            catch (Exception ex)
            {
                LogError("btnSelectFolder_Click", ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }

            catch (Exception ex)
            {
                LogError("btnClose_Click", ex);
            }
        }



        private void WriteToLog(string txt, int current = -1)
        {
            try
            {
                txtResults.Text = txt + "\r\n" + txtResults.Text;

                if (current > -0)
                    progressBar1.Value = current;

                Application.DoEvents();
            }
            catch
            {
            }
        }

        private void ClearLog(int size)
        {
            try
            {
                txtResults.Clear();
                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = size;
            }

            catch (Exception ex)
            {
                LogError("ClearLog", ex);
            }
        }



        BindingSource SBind = new BindingSource();


        private void CreateObjectsPerut()
        {
            try
            {
                List<PDFReplaceOption> options = new List<PDFReplaceOption>();

                options.Add(new PDFReplaceOption(480, 193, 96, 30, ""));
                options.Add(new PDFReplaceOption(484, 200, 90, 13, "תשלום מרצים"));

                //options.Add(new PDFReplaceOption("שכר מרצים", "תשלום מרצים"));

                options.Add(new PDFReplaceOption("אינפורמטיבי - שכר מינימום לחודש", "אינפורמטיבי - תשלום מינימום לחודש"));
                options.Add(new PDFReplaceOption("אינפורמטיבי - שכר מינימום לשעה", "אינפורמטיבי - תשלום מינימום לשעה"));
                options.Add(new PDFReplaceOption("שכר שווה כסף", "תשלום שווה כסף"));
                options.Add(new PDFReplaceOption("שכר ל", "תשלום ל"));
                options.Add(new PDFReplaceOption("שכר נטו", "תשלום ל"));

                options.Add(new PDFReplaceOption("תלוש משכורת", "פירוט חשבונית"));
                options.Add(new PDFReplaceOption("קה\"ל", ""));
                options.Add(new PDFReplaceOption("מעביד", ""));
                options.Add(new PDFReplaceOption("מעסיק", ""));
                options.Add(new PDFReplaceOption("קופ\"ג", ""));
                options.Add(new PDFReplaceOption("עבודה", ""));
                options.Add(new PDFReplaceOption("שכר מינימום", ""));
                options.Add(new PDFReplaceOption("וותק", ""));
                options.Add(new PDFReplaceOption("פיצויים", ""));
                options.Add(new PDFReplaceOption("דמי חבר/טיפול", ""));
                options.Add(new PDFReplaceOption("חופש", ""));
                options.Add(new PDFReplaceOption("ניהול העדרויות", ""));
                options.Add(new PDFReplaceOption("צבירת מחלה", ""));
                options.Add(new PDFReplaceOption("תעריף", ""));
                options.Add(new PDFReplaceOption("ימי תקן", ""));
                options.Add(new PDFReplaceOption("שעות תקן", ""));
                options.Add(new PDFReplaceOption("גמל 35 %", ""));
                options.Add(new PDFReplaceOption("ק.השתלמות %", ""));

                options.Add(new PDFReplaceOption("ח ו ד ש י ע ב ו ד ה", "ח ו ד ש י פ ע י ל ו ת"));
                options.Add(new PDFReplaceOption(335, 735, 38, 22));

                MailerOptions mailerOptions = new MailerOptions();
                mailerOptions.IDFieldName = "TEC_ID";
                mailerOptions.EmailFieldName = "TEC_MESSAGE_EMAIL";
                mailerOptions.NameFieldName = "TEC_FNAME + ' ' + TEC_LNAME";
                mailerOptions.KeyFieldName = "TEC_ZEHUT";
                mailerOptions.KeyType = PDFKeyType.Number;
                mailerOptions.TableName = "TBL_TEACHERS";
                mailerOptions.Filter = "tec_status = 1 ";
                mailerOptions.EmptyRectangle = new RectangleF(25, 625, 60, 13);
                mailerOptions.EmptyKeyValue = "0";
                mailerOptions.EmptyKeyType = PDFKeyType.Number;
                mailerOptions.EditOptions = options;
                mailerOptions.KeyRectangle = new RectangleF(256, 137, 78, 15);

                Utils.SerializeObjectUTF(DATA_FILENAME[(int)pdfType.PdfPerut], mailerOptions);
                }

            catch (Exception ex)
            {
                LogError("CreateObjectsPerut", ex);
            }
        }

        private void CreateObjects806()
        {
            try
            {
                List<PDFReplaceOption> options = new List<PDFReplaceOption>();

                options.Add(new PDFReplaceOption(50, 340, 162, 11, ""));

                MailerOptions mailerOptions = new MailerOptions();
                mailerOptions.IDFieldName = "TEC_ID";
                mailerOptions.EmailFieldName = "TEC_MESSAGE_EMAIL";
                mailerOptions.NameFieldName = "TEC_FNAME + ' ' + TEC_LNAME";
                mailerOptions.KeyFieldName = "TEC_ZEHUT";
                mailerOptions.TableName = "TBL_TEACHERS";
                mailerOptions.Filter = "tec_status = 1 ";
                //mailerOptions.EmptyRectangle = new RectangleF(25, 625, 60, 13);
                //mailerOptions.EmptyKeyValue = "0";
                //mailerOptions.EmptyKeyType = PDFKeyType.PDFNumber;
                mailerOptions.EditOptions = options;
                mailerOptions.KeyRectangle = new RectangleF(448, 314, 100, 15);
                mailerOptions.KeyType = PDFKeyType.PDFNumber;
                Utils.SerializeObjectUTF(DATA_FILENAME[(int)pdfType.Pdf806], mailerOptions);
            }

            catch (Exception ex)
            {
                LogError("CreateObjects806", ex);
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDestFolder.Text == "" || txtSource.Text == "")
                {
                    WriteToLog("לא נבחרו שם קובץ או תיקיית יעד");
                    return;
                }
                if (cmbPDFType.SelectedIndex == -1)
                {
                    WriteToLog("לא נבחר סוג קובץ");
                    return;
                }

                int fromPage = 0;
                int toPage = 0;
                int.TryParse(txtFromPage.Text, out fromPage);
                int.TryParse(txtToPage.Text, out toPage);
                EnableDisable(true);

                MailerOptions mailerOptions = null;
                if (cmbPDFType.SelectedIndex == (int)pdfType.PdfPerut)
                {
                    //CreateObjectsPerut();
                    mailerOptions = (MailerOptions)Utils.DeSerializeObjectUTF(DATA_FILENAME[(int)(pdfType.PdfPerut)], typeof(MailerOptions));
                }
                else if (cmbPDFType.SelectedIndex == (int)pdfType.Pdf806)
                {
                    //CreateObjects806();
                    mailerOptions = (MailerOptions)Utils.DeSerializeObjectUTF(DATA_FILENAME[(int)pdfType.Pdf806], typeof(MailerOptions));
                }
                else
                {
                    WriteToLog("שגיאה בסוג הקובץ");
                    return;
                }
                _processor.LogMessageSent += Processor_LogMessageSent;
                _processor.PagesCountChanged += Processor_PagesCountChanged;

                if (cbFixIdNum.Checked)
                    mailerOptions.KeyType = PDFKeyType.PDFNumber;
                else
                    mailerOptions.KeyType = PDFKeyType.Number;

                int arcId = int.Parse(Utils.GetAppSetting("ArchiveId", "0"));
                if (arcId ==0)
                {
                    WriteToLog("לא מוגדר ארכיון בקובץ ההגדרות");
                    return;
                }
                _processor.ProcessPDF(mailerOptions, txtSource.Text, txtDestFolder.Text, GetConnectionString(), txtYear.Text, fromPage, toPage, arcId, new string[] {"FILE_TYPE"}, new string[] {cmbPDFType.Text});

                ShowData();


                if (_processor.Cancel)
                    WriteToLog("הפעולה הופסקה");
                else
                    WriteToLog("הפעולה הסתיימה בהצלחה");

                EnableDisable(false);
            }

            catch (Exception ex)
            {
                LogError("btnProcess_Click", ex);
            }
        }

        private void Processor_PagesCountChanged(int pageNumber)
        {
            try
            {
                ClearLog(pageNumber);
            }

            catch (Exception ex)
            {
                LogError("Processor_PagesCountChanged", ex);
            }
        }

        private void Processor_LogMessageSent(string message, int pageNumber)
        {
            try
            {
                WriteToLog(message, pageNumber);
            }

            catch (Exception ex)
            {
                LogError("Processor_LogMessageSent", ex);
            }
        }

        private void EnableDisable(bool start)
        {
            try
            {
                if (start)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    dataGridView1.DataSource = null;
                }
                else
                    Cursor.Current = Cursors.Default;

                blnCancel = !start;
                btnStop.Enabled = start;
                btnClose.Enabled = !start;
                btnSendMail.Enabled = !start;
                btnSendOneMail.Enabled = !start;
                txtSampleEmail.Enabled = !start;
                btnProcess.Enabled = !start;

                btnDeleteEmpty.Enabled = !start;
                btnSaveLog.Enabled = !start;
            }

            catch (Exception ex)
            {
                LogError("EnableDisable", ex);
            }

        }

        private void ShowData()
        {
            try
            {
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AutoGenerateColumns = false;

                dataGridView1.Columns.Clear();
                DataGridViewColumn c;

                c = new DataGridViewCheckBoxColumn();
                c.Name = "IsSelected";
                c.DataPropertyName = "";
                c.HeaderText = "";
                c.ReadOnly = false;
                dataGridView1.Columns.Add(c);


                c = new DataGridViewCheckBoxColumn();
                c.Name = "MailError";
                c.DataPropertyName = "MailError";
                c.HeaderText = "שגיאה במייל";
                c.ReadOnly = true;
                dataGridView1.Columns.Add(c);


                c = new DataGridViewTextBoxColumn();
                c.DataPropertyName = "PageNumber";
                c.HeaderText = "עמוד";
                c.ReadOnly = true;
                dataGridView1.Columns.Add(c);

                c = new DataGridViewTextBoxColumn();
                c.Name = "FileName";
                c.DataPropertyName = "FileName";
                c.HeaderText = "שם קובץ";
                c.ReadOnly = true;
                dataGridView1.Columns.Add(c);

                c = new DataGridViewTextBoxColumn();
                c.Name = "IdNum";
                c.DataPropertyName = "IdNum";
                c.HeaderText = "ת.ז";
                c.ReadOnly = true;
                dataGridView1.Columns.Add(c);

                c = new DataGridViewTextBoxColumn();
                c.Name = "Name";
                c.DataPropertyName = "Name";
                c.HeaderText = "שם המורה";
                c.ReadOnly = true;
                dataGridView1.Columns.Add(c);

                c = new DataGridViewCheckBoxColumn();
                c.Name = "IsEmpty";
                c.DataPropertyName = "IsEmpty";
                c.HeaderText = "ריק";
                c.ReadOnly = true;
                dataGridView1.Columns.Add(c);

                c = new DataGridViewTextBoxColumn();
                c.Name = "Email";
                c.DataPropertyName = "Email";
                c.HeaderText = "דוא\"ל";
                c.ReadOnly = true;
                dataGridView1.Columns.Add(c);

                c = new DataGridViewTextBoxColumn();
                c.Name = "SentDate";
                c.DataPropertyName = "SentDate";
                c.HeaderText = "תאריך משלוח";
                c.ReadOnly = true;
                dataGridView1.Columns.Add(c);

                SBind.DataSource = _processor.Results;
                dataGridView1.DataSource = SBind;

                CheckAll(true);
            }

            catch (Exception ex)
            {
                LogError("ShowData", ex);
            }
        }

        private string GetConnectionString()
        {
            return (string)Settings.Default.Properties[$"Connection{cmbConnection.SelectedIndex + 1}Value"].DefaultValue;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text += " " + Application.ProductVersion;
                txtDestFolder.Text = Utils.GetAppSetting("DestFolder", "");
                txtSource.Text = Utils.GetAppSetting("Source", "");
                txtYear.Text = Utils.GetAppSetting("Year", "");
                cbFixIdNum.Checked= Utils.GetAppSetting("FixIdNum", "1")=="1";

                SettingsProperty prop = null;
                int i = 1;
                while ((prop = Settings.Default.Properties[$"Connection{i}Name"]) != null)
                {
                    cmbConnection.Items.Add(prop.DefaultValue);
                    i++;
                }
                cmbConnection.SelectedIndex = cmbConnection.FindString(Utils.GetAppSetting("Connection", "0"));
                if (cmbConnection.SelectedIndex == -1)
                    cmbConnection.SelectedIndex = 0;

            }

            catch (Exception ex)
            {
                LogError("frmMain_Load", ex);
            }
        }


        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Utils.SetAppSetting("Connection", cmbConnection.Text);
                Utils.SetAppSetting("DestFolder", txtDestFolder.Text);
                Utils.SetAppSetting("Source", txtSource.Text);
                Utils.SetAppSetting("Year", txtYear.Text);
                Utils.SetAppSetting("FixIdNum", cbFixIdNum.Checked ? "1" : "0");

                SaveMailMessages();
            }

            catch (Exception ex)
            {
                LogError("frmMain_FormClosed", ex);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _processor.Cancel = true;
        }


        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog fileDialog = new SaveFileDialog();

                fileDialog.Title = "בחרו קובץ";
                fileDialog.Filter = "xlsx|*.xlsx";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    try

                    {
                        ExcelCreator ex = new ExcelCreator();
                        ex.DataTableToExcel(_processor.Results.ToDataTable(), 0, fileDialog.FileName);
                        WriteToLog("הלוג נשמר בהצלחה");
                    }
                    catch
                    {
                        WriteToLog("שגיאה בשמירת לוג");
                    }
                }

            }

            catch (Exception ex)
            {
                LogError("btnSaveLog_Click", ex);
            }
        }

        private void btnDeleteEmpty_Click(object sender, EventArgs e)
        {
            try
            {
                _processor.DeleteEmpty();
            }

            catch (Exception ex)
            {
                LogError("btnDeleteEmpty_Click", ex);
            }
        }

        private bool SendEmail(string toAddress, string teacher, string filename)
        {
            try
            {
                string subject = txtMailSubject.Text.Replace("|TEACHER|", teacher);
                string message = txtMailMessage.Text.Replace("|TEACHER|", teacher);
                message = message.Replace("\r\n", "\r");
                message = message.Replace("\n", "\r");
                message = message.Replace("\r", "<br/>");
                message = "<div dir='rtl'>" + message + "</div>";
                string strFrom = Utils.GetAppSetting("MailFrom", "");
                EmailSender e = new EmailSender();
                
                var z = e.SendEmail_aux(toAddress, strFrom, message, subject, new string[] { filename });

                if (z.Code != 0)
                {
                    WriteToLog(z.Message);
                    return false;
                }
                /*if (rc.Code != 0)
                {
                    return false;
                }*/

                return true;
            }

            catch (Exception ex)
            {
                LogError("SendEmail", ex);
                return false;
            }
        }
        private void btnSendOneMail_Click(object sender, EventArgs e)
        {
            try
            {
                {
                    if (!string.IsNullOrEmpty(txtSampleEmail.Text))
                    {
                        foreach (var item in _processor.Results)
                        {
                            item.Email = txtSampleEmail.Text;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                LogError("btnSendOneMail_Click", ex);
            }
        }
        private void CheckAll(bool check)
        {
            try
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    r.Cells[0].Value = check;
                    dataGridView1.RefreshEdit();
                }
            }

            catch (Exception ex)
            {
                LogError("CheckAll", ex);
            }
        }
        private void dataGridView1_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0 && dataGridView1.Rows.Count > 0)
                {
                    bool b = dataGridView1.Rows[0].Cells[0].Value != null && ((bool)dataGridView1.Rows[0].Cells[0].Value);
                    CheckAll(!b);
                }
            }

            catch (Exception ex)
            {
                LogError("dataGridView1_ColumnHeaderMouseDoubleClick", ex);
            }
        }

        private void btnSendMail_Click(object sender, EventArgs e)
        {
            try
            {
                bool errors = false;
                blnCancel = false;
                btnProcess.Enabled = false;
                btnSendMail.Enabled = false;
                btnStop.Enabled = true;
                int cnt = dataGridView1.Rows.Count;
                ClearLog(cnt);

                for (int i = 0; i < cnt && !blnCancel; i++)
                {
                    try
                    {

                        var r = dataGridView1.Rows[i];
                        if (r.Cells["IsSelected"].Value != null && ((bool)r.Cells["IsSelected"].Value) && r.Cells["IsSelected"].Value != null && !((bool)r.Cells["IsEmpty"].Value) && (!string.IsNullOrEmpty(r.Cells["IdNum"].Value.ToString())) && (!string.IsNullOrEmpty(r.Cells["Email"].Value.ToString())))
                        {
                            string email = r.Cells["Email"].Value.ToString();

                            WriteToLog($"שולח מייל ל {email} {i + 1}/{cnt}...", i + 1);
                            {
                                bool b = SendEmail(email, r.Cells["Name"].Value.ToString(), r.Cells["FileName"].Value.ToString());
                                r.Cells["SentDate"].Value = DateTime.Now;

                                if (!b)
                                {
                                    errors = true;
                                    _processor.Results[i].MailError = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errors = true;
                        WriteToLog($"שגיאה בשליחת מייל {ex.Message}");
                        try
                        {
                            _processor.Results[i].MailError = true;
                        }
                        catch
                        {
                        }
                    }
                }

                if (blnCancel)
                {
                    WriteToLog("הפעולה בוטלה");
                }

                if (!errors)
                    WriteToLog("הפעולה בוצעה בהצלחה");
                else
                    WriteToLog("הפעולה הסתיימה עם שגיאות");

                btnProcess.Enabled = true;
                btnSendMail.Enabled = true;
                btnStop.Enabled = false;
            }

            catch (Exception ex)
            {
                LogError("btnSendMail_Click", ex);
            }
        }

        private string GetPublicIPAddress()
        {
            string ip = "";
            try
            {
            using (WebClient client = new WebClient())
            {
                ip = client.DownloadString("https://api.ipify.org").Trim();
            }
            }
            catch (Exception ex)
            {
                ip = "0.0.0.0";
            }

            return ip;
        }

        private string _ip;
        private string IP
        {

            get
            {
                if (string.IsNullOrEmpty (_ip))
                    _ip = GetPublicIPAddress();

                return _ip;
            }

        }

        private void LogError(string funcName, Exception ex)
        {

            WriteToLog($"שגיאה קריטית {funcName} {ex.Message} ip:{this.IP}");
        }
        private void SaveMailMessages()
        {
            try
            {
                if (_currType >= 0)
                {
                    Utils.SetAppSetting($"MailMessage{_currType}", txtMailMessage.Text);
                    Utils.SetAppSetting($"MailSubject{_currType}", txtMailSubject.Text);
                }
            }

            catch (Exception ex)
            {
                LogError("SaveMailMessages", ex);
            }
        }

        private void LoadMailMessages()
        {
            try
            {
                txtMailMessage.Enabled = txtMailSubject.Enabled = true;
                txtMailMessage.Text = Utils.GetAppSetting($"MailMessage{_currType}", "");
                txtMailSubject.Text = Utils.GetAppSetting($"MailSubject{_currType}", "");
            }

            catch (Exception ex)
            {
                LogError("LoadMailMessages", ex);
            }
        }
        private void cmbPDFType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SaveMailMessages();

                _currType = cmbPDFType.SelectedIndex;
                LoadMailMessages();

                txtYear.Enabled = (_currType == 1);
                btnLoadLog.Enabled = true;

                if (cmbPDFType.SelectedIndex == (int)pdfType.PdfPerut)
                    _processor = new PDFProcessor();
                else if (cmbPDFType.SelectedIndex == (int)pdfType.Pdf806)
                    _processor = new PDFProcessor806();
            }
            catch (Exception ex)
            {
                LogError("cmbPDFType_SelectedIndexChanged", ex);
            }
        }

        private void btnLoadLog_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();

                fileDialog.Title = "בחרו קובץ";
                fileDialog.Filter = "xlsx|*.xlsx";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    try

                    {
                        ExcelCreator ex = new ExcelCreator();
                        DataTable dt = ex.ExcelToDatatable(fileDialog.FileName);
                        List<BasePDFProperties> lst; 
                        lst = DtExtension.ConvertDataTable<BasePDFProperties>(dt);
                        _processor.Results = lst;
                        ShowData();
                        EnableDisable(false);
                        //ex.DataTableToExcel(_processor.Results.ToDataTable(), 0, fileDialog.FileName);
                        WriteToLog("הלוג נטען בהצלחה");
                    }
                    catch (Exception ex)
                    {
                        WriteToLog("שגיאה בטעינת לוג");
                    }
                }

            }

            catch (Exception ex)
            {
                LogError("btnLoadLog_Click", ex);
            }
        }
    }
}