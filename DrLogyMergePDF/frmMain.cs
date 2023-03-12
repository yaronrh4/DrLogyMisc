using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DrLogyMergePDF
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //private static void Merge(PdfDocument outputPDFDocument, string pdfFile)
        //{
        //    PdfDocument inputPDFDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);
        //    outputPDFDocument.Version = inputPDFDocument.Version;
        //    foreach (PdfPage page in inputPDFDocument.Pages)
        //    {
        //        outputPDFDocument.AddPage(page);
        //    }
        //}


        //private void button1_Click(object sender, EventArgs e)
        //{

        //    string dir = @"C:\tmp\pdf\";

        //    //files in files folder and named like: TP031041 TP031041 TP031337 TP031337_1
        //    File.SetAttributes(dir, FileAttributes.Normal);
        //    string[] files = Directory.GetFiles(dir, "*.pdf");
        //    IEnumerable<IGrouping<string, string>> groups = files.GroupBy(n => n.Split('.')[0].Split('_')[0]);


        //    foreach (var items in groups)
        //    {
        //        // Console.WriteLine(items.Key);
        //        PdfDocument outputPDFDocument = new PdfDocument();
        //        foreach (var pdfFile in items)
        //        {y
        //            Merge(outputPDFDocument, pdfFile);
        //        }
        //        outputPDFDocument.Save(Path.GetDirectoryName(items.Key) + @"\Merge\" + Path.GetFileNameWithoutExtension(items.Key) + ".pdf");
        //    }
        //    //Console.ReadKey();

        //}

        private void MergePdfs(string dest, string[] files)
        {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfMerger merger = new PdfMerger(pdf);

            foreach (string filename in files)
            {
                //Add pages from the first document
                PdfDocument sourcePdf = new PdfDocument(new PdfReader(filename));
                merger.Merge(sourcePdf, 1, sourcePdf.GetNumberOfPages());
                sourcePdf.Close();
            }

            pdf.Close();

        }

        private void AddLog(string msg, bool isError = false)
        {
            Color c = Color.Black;
            if (richTextBox1.Text != "")
                msg = "\r\n" + msg;

            if (isError)
                c = Color.Red;

            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;

            richTextBox1.SelectionColor = c;
            richTextBox1.AppendText(msg);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
        }

        private void ClearLog()
        {
            richTextBox1.Clear();
        }

        private string FixFileDoubleSpaces (string fileName)
        {
            string fileNameWithoutSpaces = fileName.Replace("  ", " ");
            string rc = "";
            if (fileName != fileNameWithoutSpaces)
            {
                System.IO.File.Move(fileName, fileNameWithoutSpaces);
                rc = fileNameWithoutSpaces;
                string s=rc ;
                while ( s !="")
                {
                    rc = s;
                    s = FixFileDoubleSpaces(rc);
                }
            }

            return rc;
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            bool OK = true;
            ClearLog();

            try
            {
                if (txtSource.Text == "")
                {
                    AddLog("יש לבחור ספרית מקור" , true);
                    richTextBox1.ForeColor = Color.Red;
                    return;
                }

                if (txtDest.Text == "")
                {
                    AddLog("יש לבחור ספרית יעד", true);
                    return;
                }

                if (!Directory.Exists(txtSource.Text))
                {
                    AddLog("ספרית מקור לא קיימת", true);
                    return;
                }
                if (!Directory.Exists(txtDest.Text))
                {
                    AddLog("ספרית יעד לא קיימת", true);
                    return;
                }

                if (txtSource.Text == txtDest.Text)
                {
                    AddLog("ספריות המקור והיעד חייבות להיות שונות" , true);
                    return;

                }

                string srcdir = txtSource.Text;
                string outputdir = txtDest.Text;
                Properties.Settings.Default.SrcFolder = txtSource.Text;
                Properties.Settings.Default.DestFolder = txtDest.Text;
                Properties.Settings.Default.ClearDest = chkClearDest.Checked;

                Properties.Settings.Default.TxtStart = FilenameInfo.TxtStart =  txtStart.Text;
                Properties.Settings.Default.TxtEnd = FilenameInfo.TxtEnd =  txtEnd.Text;
                Properties.Settings.Default.TxtAddToFile = txtAddToFile.Text;

                Properties.Settings.Default.Save();

                //files in files folder and named like: TP031041 TP031041 TP031337 TP031337_1
                File.SetAttributes(srcdir, FileAttributes.Normal);
                string[] files = Directory.GetFiles(srcdir, "*.pdf");
                for (int j = 0; j < files.Count(); j++)
                {
                    string tmp = FixFileDoubleSpaces(files[j]);
                    if (tmp != "")
                        files[j] = tmp;
                }
                Array.Sort(files, StringComparer.InvariantCulture);
                List<string> fileList = files.ToList();

                Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();

                //בדיקה של שם קובץ
                //for (int i = 0; i < files.Count(); i++)
                //{
                //    string fileName = Path.GetFileNameWithoutExtension(files[i]);
                //    List<string> groupFiles = new List<string>();
                //    groupFiles.Add(files[i]);
                //    string nextFile = "";
                //    while (i + 1 < files.Count() && (nextFile = Path.GetFileNameWithoutExtension(files[i + 1])).StartsWith(fileName))
                //    {
                //        groupFiles.Add(files[i + 1]);
                //        i++;
                //    }
                //    groups[fileName] = groupFiles;
                //}

                //הוספת קבוצות לחשבוניות
                for (int i = 0; i < fileList.Count(); i++)
                {
                    string fileName = Path.GetFileNameWithoutExtension(fileList[i]);

                    if (FilenameInfo.IsInvoice(fileName))
                    {
                        List<string> groupFiles = new List<string>();
                        groupFiles.Add(fileList[i]);
                        fileList.Remove(fileList[i]);
                        i--;
                        groups[fileName] = groupFiles;
                    }
                }

                //בדיקות התאמה
                for (int i = 0; i < fileList.Count(); i++)
                {
                    string fileName = Path.GetFileNameWithoutExtension(fileList[i]);
                    string group = "";
                    FilenameInfo fInfo = new FilenameInfo(fileName);

                    //בדיקה על פי התאמה של קובץ
                    foreach (var items in groups)
                    {
                        if (group == "")
                        {
                            if (items.Key.Substring(0, items.Key.Length - 1).Trim() == fileName)
                            {
                                group = items.Key;
                            }
                        }
                    }

                    //בדיקה על פי התאמה של שם סטודנט, התאמה וחודש
                    if (group == "" && fInfo.IsValid)
                    {
                        foreach (var items in groups)
                        {
                            FilenameInfo gInfo = new FilenameInfo(items.Key);
                            if (gInfo.IsValid && gInfo.BeforeText == fInfo.BeforeText && gInfo.AfterText == fInfo.AfterText)
                            {
                                group = items.Key;
                            }
                        }
                    }

                    if (group != "")
                    {
                        groups[group].Add(fileList[i]);
                        fileList.Remove(fileList[i]);
                        i--;
                    }

                }

                if (chkClearDest.Checked)
                {
                    AddLog("מנקה תיקית יעד " + outputdir);

                    System.IO.DirectoryInfo di = new DirectoryInfo(outputdir);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }

                //בדיקת חשבוניות ללא דוח
                foreach (var items in groups)
                {
                    if (items.Value.Count == 1)
                    {
                        AddLog("שגיאה - חשבונית ללא דוחות " + Path.GetFileName (items.Key) , true);
                        OK = false;
                    }
                }

                if (fileList.Count > 0)
                {
                    AddLog(string.Format("שגיאה - קיימים {0} קבצים ללא חשבונית ", fileList.Count) , true);
                    OK = false;

                    for (int i = 0; i < fileList.Count; i++)
                    {
                        AddLog(string.Format("קובץ ללא חשבונית {0} ", Path.GetFileName( fileList[i])), true);
                    }
                }

                foreach (var items in groups)
                {
                    AddLog(string.Format(" ממזג קבוצה של {0} קבצים לתוך {1}", items.Value.Count, items.Key));

                    //Creates a string array of source files to be merged
                    string[] source = items.Value.ToArray();
                    AddLog("" + String.Join("\r\n", source));


                    //Merge PDF documents
                    string mergedFileName = items.Key.Substring(0, items.Key.Length - 1).Trim() + txtAddToFile.Text;
                    //Save the document

                    MergePdfs(outputdir + "\\" + mergedFileName + ".pdf", source);


                }

                if (OK)
                {
                    AddLog("הסתיים בהצלחה");
                }
                else
                    AddLog("הסתיים עם שגיאות" , true);
            }
            catch ( Exception ex)
            {
                AddLog(string.Format ("שגיאה: {0}",ex.Message), true);
                OK = false;
            }

            //Console.ReadKey();

        }
        
        private void FrmMain_Load(object sender, EventArgs e)
        {
            txtSource.Text = Properties.Settings.Default.SrcFolder;
            txtDest.Text = Properties.Settings.Default.DestFolder;
            chkClearDest.Checked = Properties.Settings.Default.ClearDest;

            txtStart.Text = Properties.Settings.Default.TxtStart;
            txtEnd.Text = Properties.Settings.Default.TxtEnd;
            txtAddToFile.Text = Properties.Settings.Default.TxtAddToFile;
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(Owner) == DialogResult.OK)
                txtSource.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btnBrowseDest_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(Owner) == DialogResult.OK)
                txtDest.Text = folderBrowserDialog1.SelectedPath;
        }

    }
}
