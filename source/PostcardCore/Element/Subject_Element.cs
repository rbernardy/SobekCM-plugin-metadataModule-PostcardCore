using System;
using System.IO;
using System.Text;
using System.Xml;
using SobekCM.Core.ApplicationState;
using SobekCM.Core.Configuration;
using SobekCM.Core.Configuration.Localization;
using SobekCM.Core.Users;
using SobekCM.Resource_Object;
using System.Diagnostics;
using System.Web;

namespace PostcardCore
{
    public class Subject_Element : SobekCM.Library.Citation.Elements.abstract_Element
    {
        private readonly int cols;
        private readonly int colsMozilla;

        public PostcardCore_Logme pclogme = new PostcardCore_Logme();

        public Subject_Element()
        {
            pclogme.logme("PC_Subject_Element: constructor...");
            Repeatable = true;
            Title = "Subject";
            cols = TEXT_AREA_COLUMNS;
            colsMozilla = MOZILLA_TEXT_AREA_COLUMNS;
            html_element_name = "subject";
            help_page = "subject";
        }

        protected int Rows { get; set; }

        public override void Prepare_For_Save(SobekCM_Item Bib, User_Object Current_User)
        {
            pclogme.logme("PC_Subject_Element: Prepare_For_Save: [" + Bib.BibID + "_" + Bib.VID + "]...");

            PostcardCore_Info postcardInfo = Bib.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

            if (postcardInfo != null)
            {
                postcardInfo.Clear_Subjects();
            }
        }

        public override void Render_Template_HTML(TextWriter Output, SobekCM_Item Bib, string Skin_Code, bool IsMozilla, StringBuilder PopupFormBuilder, User_Object Current_User, Web_Language_Enum CurrentLanguage, Language_Support_Info Translator, string Base_URL)
        {
            pclogme.logme("PC_Subject_Element: Render_Template_HTML: ...");

            if (Acronym.Length == 0)
            {
                const String DEFAULT_ACRONYM = " Enter your subject data.";

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

            String id_name = html_element_name.Replace("_", "");

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
            Output.WriteLine("      <table id=\"pc_subjects\">");

            int idx = 0;

            if (postcardInfo==null || postcardInfo.Subjects_Count == 0)
            {
                pclogme.logme("PC_Subject_Element: Render_Template_HTML: subject element - there were no subjects.");
                Output.WriteLine("       <tr style=\"text-align:left;\" id=\"pc_subject1\">");
                Output.WriteLine("          <td><input name=\"subject_name1\" type=\"text\"/><br/>Name</td>");
                Output.WriteLine("          <td><input name=\"subject_perspective1\" type=\"text\"/><br/>Perspective</td>");
                Output.WriteLine("          <td><input name=\"subject_topic1\" type=\"text\"/><br/>Topic</td></tr>");
            }
            else
            {
                pclogme.logme("PC_Subject_Element: Render_Template_HTML: subject element - there were subjects [" + postcardInfo.Subjects_Count + "].");

                foreach (PostcardCore.PostcardCore_Subject_Info subject in postcardInfo.Subjects)
                {
                    idx++;

                    Output.WriteLine("        <tr style=\"text-align:left;\" id=\"pc_subject" + idx + "\">");
                    Output.Write("              <td><input name=\"subject_name" + idx + "\" type=\"text\" value=\"");

                    if (postcardInfo != null && !String.IsNullOrEmpty(subject.Subject_Name))
                    {
                        pclogme.logme("PC_Subject_Element: Render_Template_HTML: name=[" + subject.Subject_Name + "].");
                        Output.Write(subject.Subject_Name);
                    }

                    Output.Write("\"/><br/>Name</td><td><input name=\"subject_perspective" + idx + "\" type=\"text\" value=\"");

                    if (postcardInfo != null && !String.IsNullOrEmpty(subject.Subject_Perspective))
                    {
                        pclogme.logme("PC_Subject_Element: Render_Template_HTML: perspective=[" + subject.Subject_Perspective + "].");
                        Output.Write(subject.Subject_Perspective);
                    }

                    Output.WriteLine("\"/><br/>Perspective</td><td><input name=\"subject_topic" + idx + "\" type=\"text\" value=\"");

                    if (postcardInfo != null && !String.IsNullOrEmpty(subject.Subject_Topic))
                    {
                        pclogme.logme("PC_Subject_Element: Render_Template_HTML: topic=[" + subject.Subject_Topic + "].");
                        Output.WriteLine(subject.Subject_Topic);
                    }

                    Output.Write("\"/><br/>Topic</td></tr>");
                }
            }

            Output.WriteLine("      </table>");
            Output.WriteLine("    </td>");

            Output.WriteLine("          <td style=\"vertical-align:bottom\">");

            if (Repeatable)
            {
                int pcSubjectCount = 0;

                if (postcardInfo!= null)
                {
                    pcSubjectCount = postcardInfo.Subjects_Count;
                }

                Output.WriteLine("            <a id=\"pcsubjectadd\" title=\"" + Translator.Get_Translation("Click to add a new subject", CurrentLanguage) + ".\" href=\"" + Base_URL + "l/technical/javascriptrequired\" onmousedown=\"return add_pc_subject(" + pcSubjectCount + ");\"><img class=\"repeat_button\" src=\"" + REPEAT_BUTTON_URL + "\" /></a>");
            }

            Output.WriteLine("            <a target=\"_" + html_element_name.ToUpper() + "\"  title=\"" + Translator.Get_Translation("Get help.", CurrentLanguage) + "\" href=\"" + Help_URL(Skin_Code, Base_URL) + "\" ><img class=\"help_button\" src=\"" + HELP_BUTTON_URL + "\" /></a>");

            Output.WriteLine("          </td>");

            Output.WriteLine("  </tr>");
            Output.WriteLine();
        }

        public override void Save_To_Bib(SobekCM_Item Bib)
        {
            pclogme.logme("PC_Subject_Element: Save_To_Bib: [" + Bib.BibID + "_" + Bib.VID + "]...");

            String subject_name;
            String subject_perspective;
            String subject_topic;

            PostcardCore_Info postcardInfo = Bib.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;
            string[] getKeys = HttpContext.Current.Request.Form.AllKeys;

            pclogme.logme("PC_Subject_Element: Save_To_Bib: getKeys has " + getKeys.Length + " keys.");
            pclogme.logme("PC_Subject_Element: Save_To_Bib: html_element_name=[" + html_element_name + "].");
            int i = 0;
 
            if (postcardInfo == null)
            {
                pclogme.logme("PC_Subject_Element: Save_To_Bib: postcardInfo was null.");
                postcardInfo = new PostcardCore_Info();
                Bib.Add_Metadata_Module("PostcardCore", postcardInfo);
            }
            else
            {
                pclogme.logme("PC_Subject_Element: Save_To_Bib: postcardInfo was not null.");
            }

            if (postcardInfo.Subjects_Count==0)
            {
                pclogme.logme("PC_Subject_Element: Save_To_Bib: postcardInfo.Subjects - none");
            }
            else
            {
                pclogme.logme("PC_Subject_Element: Save_To_Bib: postcardInfo.Subjects - count=" + postcardInfo.Subjects_Count);
            }

            int count_subjects = 0;

            pclogme.logme("PC_Subject_Element: Save_To_Bib: The keys.");

            foreach (String thisKey in getKeys)
            {
                pclogme.logme("PC_Subject_Element: Save_To_Bib: " + thisKey + " =[" + HttpContext.Current.Request.Form[thisKey] + "].");

                if (thisKey.StartsWith("subject_name"))
                {
                    count_subjects++;
                }
            }

            pclogme.logme("PC_Subject_Element: Save_To_Bib: There are " + count_subjects + " subjects.");
            postcardInfo.Clear_Subjects();

            for (int j=0; j < count_subjects; j++)
            {
                i++;
                
                subject_name = HttpContext.Current.Request.Form["subject_name" + i] ?? String.Empty;
                subject_perspective = HttpContext.Current.Request.Form["subject_perspective" + i] ?? String.Empty;
                subject_topic = HttpContext.Current.Request.Form["subject_topic" + i] ?? String.Empty;

                pclogme.logme("PC_Subject_Element: Save_To_Bib: " + i + ". name=[" + subject_name + "], perspective=[" + subject_perspective + "], topic=[" + subject_topic + "].");

                if (!String.IsNullOrEmpty(subject_name) || !String.IsNullOrEmpty(subject_perspective) || !String.IsNullOrEmpty(subject_topic))
                {
                    pclogme.logme("PC_Subject_Element: Save_To_Bib: one of the #" + i + ",s subject terms had a value.");
                    pclogme.logme("PC_Subject_Element: Save_To_Bib: Adding name=[" + subject_name + "], perspective=[" + subject_perspective + "], topic=[" + subject_topic + "].");
                    postcardInfo.Add_Subject(subject_name, subject_perspective, subject_topic);
                }
                else
                {
                    pclogme.logme("PC_Subject_Element: Save_To_Bib: None of the subject terms had a value.");
                }
            }

            pclogme.logme("PC_Subject_Element: Save_To_Bib: End of subject save to bib.");
        }

        protected override void Inner_Read_Data(XmlTextReader XMLReader)
        {
            pclogme.logme("PostcardCore_Subject_Element: Inner_Read_Data...");
            // Do nothing - not yet necessary
        }
    }
}
