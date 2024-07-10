using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DrLogyApp
{
    public partial class _TestFTP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (fu1.PostedFile != null && fu1.PostedFile.FileName != "")
            {
                string path = $"{Request.MapPath("Temp")}\\{fu1.FileName}";

                fu1.SaveAs (path);

                Utils.UploadFileToFtp(txtURL.Text, path, "",txtUserName.Text, txtPassword.Text);
            }
        }

        protected void btnArchive_Click(object sender, EventArgs e)
        {

            if (fu1.PostedFile != null && fu1.PostedFile.FileName != "")
            {
                string path = $"{Request.MapPath("Temp")}\\{fu1.FileName}";

                fu1.SaveAs(path);
                var arc = new FileArchive();
                arc.UploadArchive(1, path, 0, new string[] { "TEST", "KAKA" }, new string[] { "123", "ZONA" });

            }
        }
    }
}