using System.Drawing;

namespace DrLogy.DrLogyPDFUtils
{
    public class PDFReplaceOption
    {
        public PDFReplaceOption()
        {
        }

        public PDFReplaceOption(string search, string replace)
        {
            Search = search;
            Replace = replace;
        }
        public PDFReplaceOption(float x, float y, float w, float h, string replace = "")
        {
            Search = "";
            Rec = new RectangleF(x, y, w, h);
            Replace = replace;
        }
        public string Search;
        public string Replace;
        public RectangleF Rec = RectangleF.Empty;
        public Color Color = Color.Empty;
        public Color BackColor = Color.Empty;
        public string FontName = "Arial";
        public float FontSize = 14F;
        public FontStyle FontStyle = FontStyle.Regular;
        //public Font ReplaceFont = new Font("Arial", 14F, FontStyle.Regular);
        public bool Rotate = true;
    }
}
