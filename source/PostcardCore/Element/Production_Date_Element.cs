using System.IO;
using System.Text;
using System.Web;
using SobekCM.Core.ApplicationState;
using SobekCM.Core.Configuration;
using SobekCM.Core.Configuration.Localization;
using SobekCM.Core.Users;
using SobekCM.Resource_Object;
using System.Diagnostics;

namespace PostcardCore
{
    public class Production_Date_Element : SobekCM.Library.Citation.Elements.simpleTextBox_Element
    {
        public PostcardCore_Logme pclogme = new PostcardCore_Logme();

        public Production_Date_Element()
            : base("Production Date", "pc_productiondate")
        {
            pclogme.logme("PC_ProductionDate_Element constructor...");
            Repeatable = false;
        }

        public override void Prepare_For_Save(SobekCM_Item Bib, User_Object Current_User)
        {
            pclogme.logme("PC_ProductionDate_Element: Prepare_For_Save...");
            // Do nothing since there is only one value.
        }

        public override void Render_Template_HTML(TextWriter Output, SobekCM_Item Bib, string Skin_Code, bool IsMozilla, StringBuilder PopupFormBuilder, User_Object Current_User, Web_Language_Enum CurrentLanguage, Language_Support_Info Translator, string Base_URL)
        {
            pclogme.logme("PC_ProductionDate_Element: Render_Tempalte_HTML: ...");

            if (Acronym.Length == 0)
            {
                Acronym = "Enter the production date";
            }

            string valueToDisplay = string.Empty;

            PostcardCore_Info postcardInfo = Bib.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

            if (postcardInfo != null)
            {
                pclogme.logme("PC_ProductionDate_Element: Render_Tempalte_HTML: rendering [" + postcardInfo.Production_Date + "].");
                valueToDisplay = postcardInfo.Production_Date;
            }

            render_helper(Output, valueToDisplay, Skin_Code, Current_User, CurrentLanguage, Translator, Base_URL);
        }

        public override void Save_To_Bib(SobekCM_Item Bib)
        {
            pclogme.logme("PC_ProductionDate_Element: Save_To_Bib: [" + Bib.BibID + "_" + Bib.VID + "]...");

            string[] getKeys = HttpContext.Current.Request.Form.AllKeys;

            pclogme.logme("PC_ProductionDate_Element: Save_To_Bib: getKeys has " + getKeys.Length + " keys.");
            pclogme.logme("PC_ProductionDate_Element: Save_To_Bib: html_element_name=[" + html_element_name + "].");

            foreach (string thisKey in getKeys)
            {
                if (thisKey.IndexOf(html_element_name.Replace("_", "")) == 0)
                {
                    pclogme.logme("PC_ProductionDate_Element: Save_To_Bib: Key matched [" + thisKey + "].");

                    PostcardCore_Info postcardInfo = Bib.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

                    string value = HttpContext.Current.Request.Form[thisKey];

                    if (value.Length > 0)
                    {
                        pclogme.logme("PC_ProductionDate_Element: Save_To_Bib: Value's length was > 0 [" + value + "].");

                        if (postcardInfo == null)
                        {
                            pclogme.logme("PC_ProductionDate_Element: Save_To_Bib: postcardInfo was null.");
                            postcardInfo = new PostcardCore_Info();
                            Bib.Add_Metadata_Module("PostcardCore", postcardInfo);
                        }
                        else
                        {
                            pclogme.logme("PC_ProductionDate_Element: Save_To_Bib: postcardInfo was not null.");
                        }

                        postcardInfo.Production_Date = value;
                    }
                    else
                    {
                        if (postcardInfo != null)
                        {
                            postcardInfo.Production_Date = string.Empty;
                        }
                    }

                    return;
                }
                else
                {
                    pclogme.logme("PC_ProductionDate_Element: Save_To_Bib: Key didn't match [" + thisKey + "].");
                }
            }
        }
    }
}