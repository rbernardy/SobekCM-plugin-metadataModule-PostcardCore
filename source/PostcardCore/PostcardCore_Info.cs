using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SobekCM.Resource_Object;

namespace PostcardCore
{
    /// <summary> Stores the PostcardCore-specific data elements for describing postcard materials</summary>
    [Serializable]
    public class PostcardCore_Info : SobekCM.Resource_Object.Metadata_Modules.iMetadata_Module
    {
        //20161221-1423
        //Dublin Core mapping:
        //dc:Title          = image_caption;                                                            publicly viewable label: Image caption
        //dc:Subject        = Subject_topic, Subject_perespective, Subject_name(repeatable fields);     publicly viewable labels: Topics, Perspectives, Names
        //dc:Format         = format;                                                                   publicly viewable label: Format
        //dc:Format         = Color_info;                                                               publicly viewable label: Color information Coverage
        //dc:Date           = production_date;                                                          publicly viewable label: Production date
        //dc:Description    = Postmark_sender_date_time;                                                publicly viewable label: Postmark date
        //dc:Description    = postmark_receiver_datetime;                                               publicly viewable label: Postmark date
        //dc:Description    = postmark_sender_state and postmark_sender_city;                           publicly viewable label: Postmark sent from:
        //dc:Description    = postmark_receiver_state and postcard_receiver_city;                       publicly viewable label: Postmark received at:
        //dc:Description    = Era;                                                                      publicly viewable label: Era
        //dc:Description    = Production_number;                                                        publicly viewable label: Production number

        #region individual private fields
        private String imageCaption;
        private String era;
        private String format;
        private String productionNumber;
        private String productionDate;
        private PostcardCore_PostmarkSent_Info postmarkSent = new PostcardCore_PostmarkSent_Info();
        private PostcardCore_PostmarkReceived_Info postmarkReceived = new PostcardCore_PostmarkReceived_Info();
        private List<PostcardCore_Subject_Info> subjects;
        #endregion

        #region individual fields

        PostcardCore_Logme pclogme = new PostcardCore_Logme();

        public String Image_Caption
        {
            get { return imageCaption ?? String.Empty; }
            set { imageCaption = value; }
        }

        public String Era
        {
            get { return era ?? String.Empty; }
            set { era = value; }
        }
        
        public String Format
        {
            get { return format ?? String.Empty; }
            set { format = value; }
        }

        public String Production_Number
        {
            get { return productionNumber ?? String.Empty; }
            set { productionNumber = value; }
        }

        public String Production_Date
        {
            get { return productionDate ?? String.Empty; }
            set { productionDate = value; }
        }
                
        public bool Postmark_Received_hasData
        {
            get
            {
                return (postmarkReceived!=null && (!String.IsNullOrEmpty(postmarkReceived.Receiver_city) || 
                        !String.IsNullOrEmpty(postmarkReceived.Receiver_state) ||
                        !String.IsNullOrEmpty(postmarkReceived.Receiver_Datetime))
                        );
            }
        }

        public bool Postmark_Sent_hasData
        {
            get
            {
                return (postmarkSent!=null && (!String.IsNullOrEmpty(postmarkSent.Sender_city) ||
                        !String.IsNullOrEmpty(postmarkSent.Sender_state) ||
                        !String.IsNullOrEmpty(postmarkSent.Sender_datetime))
                        );
            }
        }

        public PostcardCore_PostmarkSent_Info Postmark_Sent
        {
            get { return postmarkSent ?? null; }
            set
            {
                if (postmarkSent == null)
                {
                    postmarkSent = new PostcardCore_PostmarkSent_Info(value.Sender_city, value.Sender_state, value.Sender_datetime);
                }
                else
                {
                    postmarkSent.Sender_city = value.Sender_city;
                    postmarkSent.Sender_state = value.Sender_state;
                    postmarkSent.Sender_datetime = value.Sender_datetime;
                }
            }
        }

        public PostcardCore_PostmarkReceived_Info Postmark_Received
        {
            get { return postmarkReceived ?? null; }
            set
            {
                if (postmarkReceived == null)
                {
                    PostcardCore_PostmarkReceived_Info postmarkSent = new PostcardCore_PostmarkReceived_Info(value.Receiver_city, value.Receiver_state, value.Receiver_Datetime);
                }
                else
                {
                    postmarkReceived.Receiver_city = value.Receiver_city;
                    postmarkReceived.Receiver_state = value.Receiver_state;
                    postmarkReceived.Receiver_Datetime = value.Receiver_Datetime;
                }
            }
        }
        #endregion

        #region Subjects properties and methods

        public int Subjects_Count
        {
            get { return subjects == null ? 0 : subjects.Count; }
        }

        public ReadOnlyCollection<PostcardCore_Subject_Info> Subjects
        {
            get
            {
                if (subjects == null)
                {
                    subjects = new List<PostcardCore_Subject_Info>();
                }

                return new ReadOnlyCollection<PostcardCore_Subject_Info>(subjects);
            }
        }

        public void Clear_Subjects()
        {
            pclogme.logme("PostcardCore_Info: Clear_Subjects...");

            if (subjects != null )
            {
                subjects.Clear();
            }
        }

        public void Add_Subject(String name,String perspective,String topic)
        {
            pclogme.logme("PostcardCore_Info: Add_Subjects...");

            if (subjects == null)
            {
                pclogme.logme("PostcardCore_Info: subjects was null.");
                subjects = new List<PostcardCore_Subject_Info>();
            }

            if (!subjects.Contains(new PostcardCore_Subject_Info(name,perspective,topic)))
            {
                pclogme.logme("PostcardCore_Info: subjects was Not null and this subject had not already been added.");
                pclogme.logme("PostcardCore_Info: adding subject: name=[" + name + "], perspective=[" + perspective + "], topic=[" + topic + "].");
                subjects.Add(new PostcardCore_Subject_Info(name,perspective,topic));
                pclogme.logme("PostcardCore_Info: There are now [" + subjects.Count + "](element count) (" + Subjects_Count + ") (method returned count) subjects.");
            }
        }
        
        #endregion
        
        public bool hasData
        {
            get
            {
                return  (!String.IsNullOrEmpty(imageCaption)) || (!String.IsNullOrEmpty(format)) || 
                        (!String.IsNullOrEmpty(productionDate)) || (!String.IsNullOrEmpty(era)) ||
                        (!String.IsNullOrEmpty(productionNumber)) || (postmarkSent != null) || (postmarkReceived != null) ||
                        ((subjects != null) && (Subjects_Count > 0)
                 );
            }
        }

        #region Methods/Properties to implement the iMetadata_Module interface

        public string Module_Name
        {
            get { return "PostcardCore"; }
        }

        public List<KeyValuePair<string, string>> Metadata_Search_Terms
        {
            get
            {
                List<KeyValuePair<String, String>> metadataTerms = new List<KeyValuePair<String, String>>();

                if (!String.IsNullOrEmpty(imageCaption))
                {
                    metadataTerms.Add(new KeyValuePair<String, String>("Image Caption", imageCaption));
                }

                if (!String.IsNullOrEmpty(format))
                {
                    metadataTerms.Add(new KeyValuePair<String, String>("Format", format));
                }

                /*
                if (!String.IsNullOrEmpty(colorInfo))
                {
                    metadataTerms.Add(new KeyValuePair<String, String>("Color Info", colorInfo));
                }
                */

                if (!String.IsNullOrEmpty(productionDate))
                {
                    metadataTerms.Add(new KeyValuePair<String, String>("Production Date", productionDate));
                }
                
                if (!String.IsNullOrEmpty(era))
                {
                    metadataTerms.Add(new KeyValuePair<String, String>("Era", era));
                }

                if (!String.IsNullOrEmpty(productionNumber))
                {
                    metadataTerms.Add(new KeyValuePair<String, String>("Production Number", productionNumber));
                }

                if (subjects != null && Subjects_Count > 0)
                {
                    foreach (PostcardCore_Subject_Info subject in subjects)
                    {
                        metadataTerms.Add(new KeyValuePair<String, String>("Subject Topic", subject.Subject_Name));
                        metadataTerms.Add(new KeyValuePair<String, String>("Subject Perspective", subject.Subject_Perspective));
                        metadataTerms.Add(new KeyValuePair<String, String>("Subject Name", subject.Subject_Name));
                    }
                }

                if (postmarkSent!= null)
                {
                    metadataTerms.Add(new KeyValuePair<String, String>("Postmark Sent Date/time", postmarkSent.Sender_datetime));
                    metadataTerms.Add(new KeyValuePair<String, String>("Postmark Sender City", postmarkSent.Sender_city));
                    metadataTerms.Add(new KeyValuePair<String, String>("Postmark Sender State", postmarkSent.Sender_state));
                }

                if (postmarkReceived!= null)
                {
                    metadataTerms.Add(new KeyValuePair<String, String>("Postmark Received Date/time", postmarkReceived.Receiver_Datetime));
                    metadataTerms.Add(new KeyValuePair<String, String>("Postmark Received City", postmarkReceived.Receiver_city));
                    metadataTerms.Add(new KeyValuePair<String, String>("Postmark Received State", postmarkReceived.Receiver_state));
                }

                return metadataTerms;                    
            }
        }

        public bool Retrieve_Additional_Info_From_Database(int ItemID, string DB_ConnectionString, SobekCM_Item BibObject, out string Error_Message)
        {
            pclogme.logme("PostcardCore_Info: Retrieve_Additional_Info_From_Database...");
            Error_Message = String.Empty;

            return true;
        }

        public bool Save_Additional_Info_To_Database(int ItemID, string DB_ConnectionString, SobekCM_Item BibObject, out string Error_Message)
        {
            pclogme.logme("PostcardCore_Info: Save_Additional_Info_To_Database...");
            Error_Message = String.Empty;

            return true;
        }

        #endregion
    }
}
