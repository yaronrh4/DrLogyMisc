﻿using PdfSharp.Pdf.IO;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DrLogy.DrLogyPDFUtils
{
    public class PDFUtils
    {
        private PdfSharp.Pdf.PdfDocument _sharpDoc;
        private Spire.Pdf.PdfDocument _spireDoc;
        private Spire.Pdf.PdfDocument _spireSplittedDoc;

        string _pdfFilename = "";
        string _pagesFolder = "";
        bool _isSplitMode = false;
        bool _modified = false;

        public bool OpenPdf(string fileName, string pagesFolder = "")
        {
            _pdfFilename = fileName;
            if (!string.IsNullOrEmpty(pagesFolder))
            {
                _isSplitMode = true;
                _pagesFolder = pagesFolder;
                _sharpDoc = PdfReader.Open(fileName, PdfDocumentOpenMode.Import);
            }
            else
            {
                _isSplitMode = false;
                _spireDoc = new Spire.Pdf.PdfDocument(fileName);
            }

            splitedPages = new Dictionary<int, bool>();
            _currentPage = -1;
            _modified = false;

            return true;
        }

        public bool ClosePdf()
        {
            if (_spireDoc != null)
                _spireDoc.Close();

            if (_sharpDoc != null)
            {
                if (_modified)
                    if (!_isSplitMode)
                    {
                        _sharpDoc.Save(_pdfFilename);
                    }
                    else
                    {
                        _spireSplittedDoc.SaveToFile(this.Filename);
                        _spireSplittedDoc.Close();
                    }
                _sharpDoc.Close();
            }

            _sharpDoc = null;
            _spireDoc = null;
            _modified = false;
            _currentPage = -1;
            return true;
        }


        Dictionary<int, bool> splitedPages;
        int _currentPage = -1;

        public int GetPagesCount()
        {

            if (!_isSplitMode)
                return _spireDoc.Pages.Count;
            else
            {
                return splitedPages.Count;
            }
        }

                private Spire.Pdf.PdfPageBase GetPage(int page)
        {

            if (!_isSplitMode)
                return _spireDoc.Pages[page - 1];
            else
            {
                if (_currentPage != page)
                {
                    if (_currentPage <= 0 || !splitedPages.ContainsKey(page))
                    {
                        SplitPage($"{_pagesFolder}\\{page}.pdf", page);
                        splitedPages[page] = true;

                        if (_currentPage > 0 && _modified)
                        {
                            _spireSplittedDoc.SaveToFile($"{_pagesFolder}\\{_currentPage}.pdf");
                            _spireSplittedDoc.Close();
                        }

                        _modified = false;
                        _currentPage = page;
                        _spireSplittedDoc = new PdfDocument($"{_pagesFolder}\\{page}.pdf");
                    }
                }
            }
            return _spireSplittedDoc.Pages[0];
        }
        public bool Replace(PDFReplaceOption replaceOption, int startPage, int endPage = 0)
        {
            PdfPageBase curPage = null;

            if (endPage <= 0 || endPage < startPage)
                endPage = startPage;
            for (int pageIndex = startPage; pageIndex <= endPage && pageIndex <= Pages; pageIndex++)
            {
                curPage = GetPage(pageIndex);

                string replace = replaceOption.Replace;
                if (replaceOption.Rotate)
                    replace = DrLogy.DrLogyUtils.Utils.RotateString(replace);

                Color color = replaceOption.Color.IsEmpty ? Color.Black : replaceOption.Color;
                Color backColor = replaceOption.BackColor.IsEmpty ? Color.White : replaceOption.BackColor;

                if (replaceOption.Rec == null || replaceOption.Rec.IsEmpty)
                {

                    PdfTextFindCollection collection = curPage.FindText(replaceOption.Search, TextFindParameter.IgnoreCase);

                    if (collection.Finds != null)
                    {
                        _modified = true;
                        foreach (PdfTextFind find in collection.Finds)
                            find.ApplyRecoverString(replace, backColor, true);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(replaceOption.Replace))
                    {
                        ClearRectable(curPage, replaceOption.Rec, backColor);
                    }
                    else
                    {
                        DrawText(curPage, replaceOption.Rec, replace, new Font(replaceOption.FontName, replaceOption.FontSize, replaceOption.FontStyle), color, backColor);
                    }


                }
            }

            return true;
        }

        private void ClearRectable(PdfPageBase page, RectangleF rec, Color color)
        {
            PdfSolidBrush b = new PdfSolidBrush(color);

            page.Canvas.DrawRectangle(b, rec);
            _modified = true;

        }

        private void DrawText(PdfPageBase page, RectangleF rec, string text, Font font, Color color, Color backColor)
        {

            PdfBrush brush = new PdfSolidBrush(color);
            //var used = doc.UsedFonts[2];

            //RectangleF rec;
            //var font = new PdfFont(PdfFontFamily.Courier, 14, PdfFontStyle.Regular , );
            //page.Canvas.DrawString(text, new PdfTrueTypeFont(font, true), PdfBrushes.Black ,Convert.ToInt32( rec.X), Convert.ToInt32(rec.Y), new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));

            //PdfBrush brush = new PdfSolidBrush(Color.Black);
            //var used = doc.UsedFonts[2];

            //RectangleF rec;
            //var font = new PdfFont(PdfFontFamily.Courier, 14, PdfFontStyle.Regular , );
            ClearRectable(page, rec, backColor);
            page.Canvas.DrawString(text, new PdfTrueTypeFont(new Font(font, font.Style), true), brush, rec.X, rec.Y, new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle));


            _modified = true;
        }

        /// <summary>|
        /// 
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public List<RectangleF> SearchPage(string searchString, int pageStartIndex=1, int pageEndIndex = -1 , bool searchBackwards = false )
        {
            List<RectangleF> res = new List<RectangleF>();

            if (pageEndIndex == -1)
            {
                pageEndIndex = pageStartIndex;
            }

            for (int currPageIndex = pageStartIndex; currPageIndex <=pageEndIndex; currPageIndex++)
            { 
                PdfPageBase curPage = GetPage(currPageIndex);
                PdfTextFindCollection col = curPage.FindText(searchString, TextFindParameter.IgnoreCase);
                if (col.Finds.Count() > 0)
                {
                    for (int k = 0; k < col.Finds.Count(); k++)
                        if (!searchBackwards)
                            res.Add(col.Finds[k].TextBounds[0]);
                        else
                            res.Insert(0, col.Finds[k].TextBounds[0]);
                }
            }

            return res;
        }

        public string ExtractText(RectangleF region, int pageIndex)
        {
            PdfPageBase curPage = GetPage(pageIndex);
            string rc = curPage.ExtractText(region);

            return rc;
        }


        public string Filename
        {
            get
            {
                if (_isSplitMode)
                    return $"{_pagesFolder}\\{_currentPage}.pdf";
                else
                    return this._pdfFilename;
            }
        }


        public int Pages
        {
            get
            {
                if (_sharpDoc != null)
                    return _sharpDoc.Pages.Count;
                else if (_spireDoc != null)
                    return _spireDoc.Pages.Count;
                else
                    return -1;
            }
        }

        public bool SplitPage(string filename, int startPage, int endPage = 0)
        {
            bool rc = true;
            if (endPage <= 0 || endPage < startPage)
                endPage = startPage;

            if (_sharpDoc != null)
            {
                PdfSharp.Pdf.PdfDocument p2 = new PdfSharp.Pdf.PdfDocument();
                for (int pageIndex = startPage - 1; pageIndex <= endPage - 1; pageIndex++)
                    p2.AddPage(_sharpDoc.Pages[pageIndex]);
                p2.Save(filename);
                p2.Close();
            }
            else if (_spireDoc != null)
            {
                _spireDoc.SaveToFile(filename, startPage - 1, endPage - 1, Spire.Pdf.FileFormat.PDF);

            }
            else
                rc = false;
            return rc;
        }




    }
}
