using System.IO;
using System.Text;
using SobekCM.Core.ApplicationState;
using SobekCM.Core.Configuration;
using SobekCM.Core.Configuration.Localization;
using SobekCM.Core.Users;
using SobekCM.Resource_Object;
using System.Diagnostics;
using System;
using System.Xml;
using System.Web;

namespace PostcardCore
{
    // pattering after SobekCM_Library/Citation/Elements/nonstandard elements/Abstract_Complex_Element.cs

    public class Postmark_Sent_Element : SobekCM.Library.Citation.Elements.abstract_Element
    {
        public PostcardCore_Logme pclogme = new PostcardCore_Logme();

        private readonly int cols;
        private readonly int colsMozilla;

        public Postmark_Sent_Element()
        {
            pclogme.logme("PC_PostmarkSent_Element: constructor...");
            Rows = 1;
            Repeatable = false;
            Title = "Postmark Sent";
            cols = TEXT_AREA_COLUMNS;
            colsMozilla = MOZILLA_TEXT_AREA_COLUMNS;
            html_element_name = "postmarksent";
            help_page = "postmarksent";
        }

        protected int Rows { get; set; }

        public override void Prepare_For_Save(SobekCM_Item Bib, User_Object Current_User)
        {
            pclogme.logme("PC_PostmarkSent_Element: Prepare_For_Save...");

            PostcardCore_Info postcardInfo = Bib.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

            if (postcardInfo!=null)
            {
                postcardInfo.Postmark_Sent.Sender_city = String.Empty;
                postcardInfo.Postmark_Sent.Sender_state = String.Empty;
                postcardInfo.Postmark_Sent.Sender_datetime = String.Empty;
            }
        }

        public override void Save_To_Bib(SobekCM_Item Bib)
        {
            pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: [" + Bib.BibID + "_" + Bib.VID + "]...");

            PostcardCore_Info postcardInfo = Bib.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;
            string[] getKeys = HttpContext.Current.Request.Form.AllKeys;

            pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib:getKeys has " + getKeys.Length + " keys.");
            pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: html_element_name=[" + html_element_name + "].");
            int i =0;
            String value;

            if (postcardInfo == null)
            {
                pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: postcardInfo was null.");
                postcardInfo = new PostcardCore_Info();
                Bib.Add_Metadata_Module("PostcardCore", postcardInfo);
            }
            else
            {
                pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: postcardInfo was not null.");
            }

            if (postcardInfo.Postmark_Sent==null)
            {
                pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: postcardInfo.Postmark_Sent was null.");
                postcardInfo.Postmark_Sent = new PostcardCore_PostmarkSent_Info();
            }
            else
            {
                pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: postcardInfo.Postmark_Sent was NOT null.");
            }

            foreach (string thisKey in getKeys)
            {
                i++;
                pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: " + i + ". [" + thisKey + "].");

                if (thisKey.StartsWith(html_element_name))
                {
                    pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: Matched key [" + thisKey + "].");

                    value = HttpContext.Current.Request.Form[thisKey];

                    if (thisKey == "postmarksent_sender_city")
                    {
                        if (value.Length > 0)
                        {
                            pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: city=[" + value + "].");
                            postcardInfo.Postmark_Sent.Sender_city = value;
                        }
                        else
                        {
                            postcardInfo.Postmark_Sent.Sender_city = String.Empty;
                        }
                    }
                    else if (thisKey == "postmarksent_sender_state")
                    {
                        if (value.Length > 0)
                        {
                            pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: state=[" + value + "].");
                            postcardInfo.Postmark_Sent.Sender_state = value;
                        }
                        else
                        {
                            postcardInfo.Postmark_Sent.Sender_state = String.Empty;
                        }
                    }
                    else if (thisKey=="postmarksent_sender_datetime")
                    {
                        if (value.Length > 0)
                        {
                            pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: datetime=[" + value + "].");
                            postcardInfo.Postmark_Sent.Sender_datetime = value;
                        }
                        else
                        {
                            postcardInfo.Postmark_Sent.Sender_datetime = String.Empty;
                        }
                    }
                }
                else
                {
                    pclogme.logme("PC_PostmarkSent_Element: Save_To_Bib: key didn't match [" + thisKey + "].");
                }
            }
        }

        public override void Render_Template_HTML(TextWriter Output, SobekCM_Item Bib, string Skin_Code, bool IsMozilla, StringBuilder PopupFormBuilder, User_Object Current_User, Web_Language_Enum CurrentLanguage, Language_Support_Info Translator, string Base_URL)
        {
            pclogme.logme("PostcardCore_PostmarkSent_Element: Render_Template_HTML: ...");

            if (Acronym.Length==0)
            {
                const String DEFAULT_ACRONYM = " Enter your postmark sent info data.";

                switch (CurrentLanguage)
                {
                    case Web_Language_Enum.English:
                        Acronym = DEFAULT_ACRONYM;
                        break;

                    case Web_Language_Enum.Spanish:
                        Acronym = DEFAULT_ACRONYM;
                        break;

                    case Web_Language_Enum.French:
                        Acronym = DEFAULT_ACRONYM;
                        break;

                    default:
                        Acronym = DEFAULT_ACRONYM;
                        break;
                }
            }

            int actual_cols = cols;

            if (IsMozilla)
                actual_cols = colsMozilla;

            String id_name=html_element_name.Replace("_","");

            PostcardCore_Info postcardInfo = Bib.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

            Output.WriteLine(" <!-- " + Title + " Element -->");
            Output.WriteLine(" <tr>");
            Output.WriteLine("  <td style=\"width:" + LEFT_MARGIN + "px\">&nbsp;</td>");

            if (Acronym.Length > 0)
            {
                Output.WriteLine("    <td class=\"metadata_label\"><a href=\"" + Help_URL(Skin_Code, Base_URL) + "\" target=\"_" + html_element_name.ToUpper() + "\"><acronym title=\"" + Acronym + "\">" + Translator.Get_Translation(Title, CurrentLanguage) + ":</acronym></a></td>");
            }
            else
            {
                Output.WriteLine("    <td class=\"metadata_label\"><a href=\"" + Help_URL(Skin_Code, Base_URL) + "\" target=\"_" + html_element_name.ToUpper() + "\">" + Translator.Get_Translation(Title, CurrentLanguage) + ":</a></td>");
            }

            Output.WriteLine("    <td>");
            Output.WriteLine("      <table>");
            Output.WriteLine("        <tr style=\"text-align:left;\">");
            Output.Write("          <td><input name=\"postmarksent_sender_city\" type=\"text\" value=\"");

            if (postcardInfo!=null && postcardInfo.Postmark_Sent_hasData && !String.IsNullOrEmpty(postcardInfo.Postmark_Sent.Sender_city))
            {
                pclogme.logme("PostcardCore_PostmarkSent_Element: Render_Template_HTML: city=[" + postcardInfo.Postmark_Sent.Sender_city + "].");
                Output.Write(postcardInfo.Postmark_Sent.Sender_city);
            }
        
            Output.Write("\"/></td><td>, <input name=\"postmarksent_sender_state\" type=\"text\" value=\"");

            if (postcardInfo!=null && postcardInfo.Postmark_Sent_hasData && !String.IsNullOrEmpty(postcardInfo.Postmark_Sent.Sender_state))
            {
                pclogme.logme("PostcardCore_PostmarkSent_Element: Render_Template_HTML: state=[" + postcardInfo.Postmark_Sent.Sender_state + "].");
                Output.Write(postcardInfo.Postmark_Sent.Sender_state);
            }
           
            Output.WriteLine("\"/></td><td><input name=\"postmarksent_sender_datetime\" type=\"text\" value=\"");

            if (postcardInfo!=null && postcardInfo.Postmark_Sent_hasData && !String.IsNullOrEmpty(postcardInfo.Postmark_Sent.Sender_datetime))
            {
                pclogme.logme("PostcardCore_PostmarkSent_Element: Render_Template_HTML: datetime=[" + postcardInfo.Postmark_Sent.Sender_datetime + "].");
                Output.WriteLine(postcardInfo.Postmark_Sent.Sender_datetime);
            }

            Output.Write("\"/></td></tr>");
            Output.WriteLine(" <tr><td>City</td><td>State</td><td>Date time</td></tr>");
            Output.WriteLine("      </table>");
            Output.WriteLine("    </td>");
            Output.WriteLine("  </tr>");
            Output.WriteLine();
        }

        protected override void Inner_Read_Data(XmlTextReader XMLReader)
        {
            pclogme.logme("PostcardCore_PostmarkSent_Element: Inner_Read_Data...");
            // Do nothing - not yet necessary
        }
    }
}