using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CommitmentLettersApp
{
    public partial class Encrypt : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)

        {

     
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (!fuConfig.HasFile)
                return;

            //Get config file in memory

            // Create a filemap refering the config file.
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = MapPath($"{txtFolder.Text}\\{fuConfig.FileName}");

            // Retrieve the config file.
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            //Encrypt section

            var c = config.AppSettings;
            if (c!=null)
                c.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");

            var c2 = config.GetSectionGroup("system.net/mailSettings");
            if (c2!=null && c2.Sections.Count > 0)
                c2.Sections[0].SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");

            var c3 = config.ConnectionStrings;
            if (c3 != null)
                c3.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");

            // Save the encrypted section.

            config.AppSettings.SectionInformation.ForceSave = true;

            config.Save(ConfigurationSaveMode.Full);

            GetConfig(fileMap.ExeConfigFilename);
        }

        public void GetConfig(string filepath)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", System.IO.Path.GetFileName(filepath)));
            Response.TransmitFile(filepath);
            Response.End();

        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (!fuConfig.HasFile)
                return;

            //Get config file in memory

            // Create a filemap refering the config file.
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = MapPath($"{txtFolder.Text}\\{fuConfig.FileName}");

            // Retrieve the config file.
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            //WebConfigurationManager.OpenMachineConfiguration("c:\\temp");

            //Configuration config = ConfigurationManager.sa

            //Encrypt section

            config.AppSettings.SectionInformation.UnprotectSection();

            // Save the encrypted section.

            config.AppSettings.SectionInformation.ForceSave = true;

            config.Save(ConfigurationSaveMode.Full);

            GetConfig(fileMap.ExeConfigFilename);

        }
    }
}