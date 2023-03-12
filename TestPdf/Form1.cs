using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestPdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int Cnt=0;
        private void button1_Click(object sender, EventArgs e)
        {
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument("c:\\_a\\0.pdf");
            var page = doc.Pages[0];
            PdfTextFindCollection collection = page.FindText("0.0", TextFindParameter.IgnoreCase);
            //string newText = "aaa";
            PdfBrush brush = new PdfSolidBrush(Color.LightGoldenrodYellow);
            var used = doc.UsedFonts[2];

            //RectangleF rec;
            //var font = new PdfFont(PdfFontFamily.Courier, 14, PdfFontStyle.Regular);
            int z = 0;
            foreach (PdfTextFind find in collection.Finds)
            {
                z++;
                find.ApplyRecoverString(z.ToString(), Color.White, true);

                //var a = find.Bounds;
                //page.Canvas.DrawRectangle(PdfBrushes.White, a);
                //  page.Canvas.DrawString(newText, font, brush, a);
            }

            doc.SaveToFile("c:\\_a\\zyaron.pdf");

            //Spire.Doc.Document doc = new Spire.Doc.Document();
            //doc.LoadFromFile("c:\\_a\\p.docx");
            //doc.SaveToFile("c:\\_a\\moshe.docx");

            //TextSelection sel1 = doc.FindString("שם החברה", false, true);
            //TextSelection sel2 = doc.FindString("תוכנת השכר המובילה במדינה", false, true);
            //TextRange range1 = sel1.GetAsOneRange();
            //TextRange range2 = sel2.GetAsOneRange();

            //Spire.Doc.Document doc2 = new Spire.Doc.Document();
            //doc2.Sections.Add(range1.OwnerParagraph.Owner.Owner.Owner.Owner.Owner.Clone());
            //doc2.Sections.Add(range1.OwnerParagraph.Owner.Owner.Owner.Owner.Owner.NextSibling.Clone());
            //doc2.Sections.Add(range1.OwnerParagraph.Owner.Owner.Owner.Owner.Owner.NextSibling.NextSibling.Clone());

            //doc2.SaveToFile("c:\\_a\\goodes.docx");

            ////var app = new Microsoft.Office.Interop.Word.Application();
            ////app.Visible = true;
            ////var doc = app.Documents.Open(@"c:\\_a\\s.docx");
            ////int i = 0;
            ////foreach (var page in doc.Pages())
            ////{
            ////    i++;
            ////    page.Copy();
            ////    var doc2 = app.Documents.Add();
            ////    doc2.Range().Paste();

            ////    doc2.SaveAs2("c:\\_a\\z" + i.ToString()+".docx");
            ////}


        }

        private string RotateString(string str)
        {
            string rc = "";

            for (int i = str.Length - 1; i >= 0; i--)
            {
                rc += str[i];
            }

            return rc;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument(txtSource.Text);

            string idNum = null;

            var page = doc.Pages[0];
            PdfTextFindCollection collection = page.FindText("0.00", TextFindParameter.IgnoreCase);
            int z = 0;
            foreach (PdfTextFind find in collection.Finds)
            {
                z++;
                //find.ApplyRecoverString(z.ToString());
                var b = find.TextBounds[0];
                if (b.X == 62.25 && b.Y == 625.95)
                {
                    MessageBox.Show("EMPTY");
                    return;
                }
            }

            //MessageBox.Show("not empty");
            string search = "553"; //201553575
            PdfTextFindCollection coll2 = page.FindText(search, TextFindParameter.IgnoreCase);
            int found = coll2.Finds.Count();
            //            string text = page.ExtractText(collection.Finds[0].TextBounds[0]);

            PdfBrush brush = new PdfSolidBrush(Color.LightGoldenrodYellow);
            //page.Canvas.DrawRectangle(PdfBrushes.Yellow, new RectangleF(254, 141, 75, 13));\

            string tmp = page.ExtractText(new RectangleF(484, 193, 90, 13));
            ReplaceString(doc, tmp, "");
            idNum = page.ExtractText(collection.Finds[0].TextBounds[0]);

            ReplaceString(doc, "שכר מרצים", "תשלום מרצים");

            ReplaceString(doc, "אינפורמטיבי - שכר מינימום לחודש", "אינפורמטיבי - תשלום מינימום לחודש");
            ReplaceString(doc, "אינפורמטיבי - שכר מינימום לשעה", "אינפורמטיבי - תשלום מינימום לשעה");
            ReplaceString(doc, "שכר שווה כסף", "תשלום שווה כסף");
            ReplaceString(doc, "שכר ל", "תשלום ל");
            ReplaceString(doc, "שכר נטו", "תשלום ל");

            ReplaceString(doc, "תלוש משכורת", "פירוט חשבונית");
            ReplaceString(doc, "קה\"ל", "");
            ReplaceString(doc, "מעביד", "");
            ReplaceString(doc, "מעסיק", "");
            ReplaceString(doc, "קופ\"ג", "");
            ReplaceString(doc, "עבודה", "");
            ReplaceString(doc, "שכר מינימום", "");
            ReplaceString(doc, "וותק", "");
            ReplaceString(doc, "פיצויים", "");
            ReplaceString(doc, "דמי חבר/טיפול", "");
            ReplaceString(doc, "חופש", "");
            ReplaceString(doc, "ניהול העדרויות", "");
            ReplaceString(doc, "צבירת מחלה", "");
            ReplaceString(doc, "תעריף", "");
            ReplaceString(doc, "ימי תקן", "");
            ReplaceString(doc, "שעות תקן", "");
            ReplaceString(doc, "גמל 35 %", "");
            ReplaceString(doc, "ק.השתלמות %", "");


            ReplaceString(doc, "ח ו ד ש י ע ב ו ד ה", "ח ו ד ש י פ ע י ל ו ת");

            ClearRectacle(doc, 335, 735, 38, 22);
            //ReplaceString(doc, "", "");

            doc.SaveToFile(txtDest.Text);

            System.Diagnostics.Process.Start(txtDest.Text);
        }

        private void ReplaceString(Spire.Pdf.PdfDocument doc, string search, string replace)
        {
            PdfTextFindCollection collection = doc.Pages[0].FindText(search, TextFindParameter.IgnoreCase);

            foreach (PdfTextFind find in collection.Finds)
            {
                find.ApplyRecoverString(RotateString(replace), Color.White, true);
            }
        }

        private void ClearRectacle(Spire.Pdf.PdfDocument doc, int x, int y, int w, int h)
        {
            doc.Pages[0].Canvas.DrawRectangle(PdfBrushes.White, new RectangleF(x, y, w, h));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PdfSharp.Pdf.PdfDocument document = PdfReader.Open(txtDest.Text, PdfDocumentOpenMode.Modify);

            var page = document.Pages[0];

            // Draw the text
            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Arial", 10, XFontStyle.BoldItalic);

            //484, 193, 90, 13
            // Draw the text
            gfx.DrawString(RotateString("תשלום מרצים"), font, XBrushes.Black,
            new XRect(484, 193, 90, 13), XStringFormats.Center);
            document.Save(txtDest.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxX.Text = GetSetting("textBoxX", textBoxX.Text);
            textBoxY.Text = GetSetting("textBoxY", textBoxY.Text);
            textBoxW.Text = GetSetting("textBoxW", textBoxW.Text);
            textBoxH.Text = GetSetting("textBoxH", textBoxH.Text);
            txtHighlightFile.Text = GetSetting("txtHighlightFile", txtHighlightFile.Text);

        }

        private void SetSetting(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }


        private string GetSetting(string key, string def)
        {
            string val = "";

            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                val = appSettings[key];
            }
            catch (ConfigurationErrorsException)
            {
                //Console.WriteLine("Error reading app settings");
            }
            if (string.IsNullOrEmpty(val))
                val = def;

            return val;
        }





        private void btnHighlight_Click(object sender, EventArgs e)
        {
            try
            {
                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument(txtHighlightFile.Text);

                bool saved = false;
                string filename = "";
                while (!saved && Cnt < 30)
                {
                    try
                    {
                        Cnt++;
                        filename = $"{System.IO.Path.GetDirectoryName(txtHighlightFile.Text)}\\{System.IO.Path.GetFileNameWithoutExtension(txtHighlightFile.Text)}__{Cnt}.pdf";
                        doc.SaveToFile(filename);
                        saved = true;
                    }
                    catch
                    {

                    }
                }
                if (!saved)
                    txtHighlightSearch.Text = "שגיאה בשמירת קובץ";

                else
                {
                    txtHighlightSearch.Text = "";
                    var rec = new RectangleF(float.Parse(textBoxX.Text), float.Parse(textBoxY.Text), float.Parse(textBoxW.Text), float.Parse(textBoxH.Text));
                    doc.Pages[0].Canvas.DrawRectangle(PdfBrushes.Yellow, rec);
                    string txt = doc.Pages[0].ExtractText(rec);

                    txtHighlightSearch.Text = txt;
                    doc.SaveToFile(filename);
                    System.Diagnostics.Process.Start(filename);

                }
            }
            catch (Exception ex)
            {
                txtHighlightSearch.Text = ex.Message; 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Spire.Pdf.PdfDocument document = new Spire.Pdf.PdfDocument(txtDest.Text);

            var page = document.Pages[0];

            // Draw the text
            // Get an XGraphics object for drawing
            //XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            //XFont font = new XFont("Arial", 10, XFontStyle.BoldItalic);

            //484, 193, 90, 13
            // Draw the text

            PdfBrush brush = new PdfSolidBrush(Color.Black);
            //var used = doc.UsedFonts[2];

            //RectangleF rec;
            //var font = new PdfFont(PdfFontFamily.Courier, 14, PdfFontStyle.Regular , );
            document.Pages[0].Canvas.DrawString(RotateString("תשלום מרצים"), new PdfTrueTypeFont(new System.Drawing.Font("Arial", 14F), true), new PdfSolidBrush(Color.Black), 484, 193, new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));

            //new XRect(484, 193, 90, 13), XStringFormats.Center);
            document.SaveToFile(txtDest.Text, Spire.Pdf.FileFormat.PDF);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetSetting("textBoxX", textBoxX.Text);
            SetSetting("textBoxY", textBoxY.Text);
            SetSetting("textBoxW", textBoxW.Text);
            SetSetting("textBoxH", textBoxH.Text);
            SetSetting("txtHighlightFile", txtHighlightFile.Text);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Clipboard.SetText($"{textBoxX.Text},{textBoxY.Text},{textBoxW.Text},{textBoxH.Text}");
            txtHighlightSearch.Text = "OK";
        }

        private void btnResetIndex_Click(object sender, EventArgs e)
        {
            Cnt = 0;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Title = "בחרו קובץ";
            d.Filter = "pdf|*.pdf";

            if (d.ShowDialog () == DialogResult.OK )
            {
                txtHighlightFile.Text = d.FileName;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument(txtHighlightFile.Text);

                bool saved = false;
                string filename = "";
                while (!saved && Cnt < 30)
                {
                    try
                    {
                        Cnt++;
                        filename = $"{System.IO.Path.GetDirectoryName(txtHighlightFile.Text)}\\{System.IO.Path.GetFileNameWithoutExtension(txtHighlightFile.Text)}__{Cnt}.pdf";
                        doc.SaveToFile(filename);
                        saved = true;
                    }
                    catch
                    {

                    }
                }
                if (!saved)
                    txtHighlightSearch.Text = "שגיאה בשמירת קובץ";

                else
                {
                    var page = doc.Pages[0];
                    PdfTextFindCollection col = page.FindText(txtSearch.Text, TextFindParameter.IgnoreCase);

                    if (col.Finds.Count() > 0)
                    {
                        RectangleF rec=  col.Finds[0].TextBounds[0];

                        textBoxX.Text = rec.X.ToString();
                        textBoxY.Text = rec.Y.ToString();
                        textBoxH.Text= rec.Height.ToString();
                        textBoxW.Text = rec.Width.ToString();
                    }

                    doc.SaveToFile(filename);
                    System.Diagnostics.Process.Start(filename);

                }
            }
            catch (Exception ex)
            {
                txtHighlightSearch.Text = ex.Message;
            }
        }
    }
}


