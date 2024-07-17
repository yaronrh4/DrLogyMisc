using DrLogy.DrLogyPDFMailerUtils;
using DrLogy.DrLogyPDFUtils;
using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFMailer
{
    public class PDFProcessor806 : PDFProcessor
    {
        protected override BasePDFProperties ProcessPDFPage(MailerOptions options, string connectionString, string parameter, int pageNumber , string[] extraArchiveKeys , string[] extraArchiveValues)
        {
            BasePDFProperties props = base.ProcessPDFPage(options, connectionString, parameter,pageNumber ,extraArchiveKeys ,extraArchiveValues);
            DrLogy.DrLogyUtils.DbUtils.ConStr = connectionString;
            string sql = $"SELECT TEACMONTH_MONTH FROM TEACHER_MONTH_SUMS WHERE TEACMONTH_TEACHER = {props.Id} AND TEACMONTH_YEAR = {parameter} AND ISNULL (TEACMONTH_SALARY,0) > 0";
            System.Data.DataTable dt = DrLogy.DrLogyUtils.DbUtils.GetSQLData(sql, null, null, CommandType.Text);

            string v = "";
            for (int i= 12 ; i >= 1; i--)
            {
                var r= dt.Select($"TEACMONTH_MONTH = {i}");
                if (r.Length > 0)
                {
                    v += "V ";
                }
                else
                    v += "  ";
            }
            if (v.Trim()=="")
            {
                props.IsEmpty = true;
            }

            PDFReplaceOption rep = new PDFReplaceOption();
            rep.Rec = options.EditOptions[0].Rec;
            rep.Rec.X -= 2;
            rep.Rec.Y += 5;
            rep.Rec.Height -= 5;
            rep.Replace = v;
            rep.FontName = "Courier New";
            rep.FontStyle = System.Drawing.FontStyle.Bold;
            rep.FontSize = 11;
            _utils.Replace(rep ,pageNumber);

            return props;
        }
    }
}
