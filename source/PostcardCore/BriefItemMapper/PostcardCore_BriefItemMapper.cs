using System;
using SobekCM.Core.BriefItem;
using SobekCM.Resource_Object;
using System.Diagnostics;

namespace PostcardCore
{
    public class PostcardCore_BriefItemMapper : IBriefItemMapper
    {
        public bool MapToBriefItem(SobekCM_Item Original, BriefItemInfo New)
        {
            PostcardCore_Logme pclogme = new PostcardCore_Logme();
            pclogme.logme("PostcardCore_BriefItemMapper...");
         
            PostcardCore_Info postcardInfo = Original.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

            if ((postcardInfo != null) && (postcardInfo.hasData))
            {
                pclogme.logme("PostcardCore_BriefItemMapper: postcardInfo was not null and has data...");

                if (!String.IsNullOrEmpty(postcardInfo.Image_Caption))
                {
                    New.Add_Description("Image Caption", postcardInfo.Image_Caption);
                }

                if (!String.IsNullOrEmpty(postcardInfo.Format))
                {
                    New.Add_Description("Format", postcardInfo.Format);
                }

                if (!String.IsNullOrEmpty(postcardInfo.Production_Date))
                {
                    New.Add_Description("Production Date", postcardInfo.Production_Date);
                }

                if (!String.IsNullOrEmpty(postcardInfo.Era))
                {
                    New.Add_Description("Era", postcardInfo.Era);
                }

                if (!String.IsNullOrEmpty(postcardInfo.Production_Number))
                {
                    New.Add_Description("Production Number", postcardInfo.Production_Number);
                }

                String myterm = String.Empty;

                if (postcardInfo.Postmark_Sent_hasData)
                {
                    pclogme.logme("PostcardCore_BriefItemMapper: postmark sent has data ...");

                    if (!String.IsNullOrEmpty(postcardInfo.Postmark_Sent.Sender_city))
                    {
                        myterm = postcardInfo.Postmark_Sent.Sender_city;

                        if (!String.IsNullOrEmpty(postcardInfo.Postmark_Sent.Sender_state))
                        {
                            myterm += ", " + postcardInfo.Postmark_Sent.Sender_state;
                        }

                        if (!String.IsNullOrEmpty(postcardInfo.Postmark_Sent.Sender_datetime))
                        {
                            myterm += " (" + postcardInfo.Postmark_Sent.Sender_datetime + ")";
                        }

                        New.Add_Description("Postmark Sent", myterm);
                    }
                }

                if (postcardInfo.Postmark_Received_hasData)
                {
                    pclogme.logme("PostcardCore_BriefItemMapper: postmark received has data...");

                    if (!String.IsNullOrEmpty(postcardInfo.Postmark_Received.Receiver_city))
                    {
                        myterm = postcardInfo.Postmark_Received.Receiver_city;

                        if (!String.IsNullOrEmpty(postcardInfo.Postmark_Received.Receiver_state))
                        {
                            myterm += ", " + postcardInfo.Postmark_Received.Receiver_state;
                        }

                        if (!String.IsNullOrEmpty(postcardInfo.Postmark_Received.Receiver_Datetime))
                        {
                            myterm += " (" + postcardInfo.Postmark_Received.Receiver_Datetime + ")";
                        }

                        New.Add_Description("Postmark Received", myterm);
                    }
                }

                if (postcardInfo.Subjects_Count>0)
                {
                    pclogme.logme("PostcardCore_BriefItemMapper: there are subjects...");

                    foreach (PostcardCore_Subject_Info subject in postcardInfo.Subjects)
                    {
                        if (!String.IsNullOrEmpty(subject.Subject_Name))
                        {
                            myterm = subject.Subject_Name;
                        }

                        if (!String.IsNullOrEmpty(subject.Subject_Perspective))
                        {
                            myterm += " viewed from " + subject.Subject_Perspective;
                        }
                        
                        if (!String.IsNullOrEmpty(subject.Subject_Topic))
                        {
                            myterm +=" (" + subject.Subject_Topic +")";
                        }

                        New.Add_Description("Subject", myterm);                      
                    }
                }
            }

            return true;
        }
    }
}
