using DrLogy.DrLogyUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace DrLogyApp
{
    /// <summary>
    /// Summary description for OldAppServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OldAppServices : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetTeacherAppUrl(int teacherId)
        {

            string token = Utils.EncodeId(teacherId);
            string appUrl = ConfigurationManager.AppSettings["AppUrl"];
            string url = Utils.ReplaceTemplateString(ConfigurationManager.AppSettings["AppButtonTeacherLink"].ToString(), new string[] { "appurl", "token" }, new string[] { appUrl, token });

            return url;
        }
    }
}
