using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SobekCM.Resource_Object;
using SobekCM.Resource_Object.METS_Sec_ReaderWriters;
using System.Diagnostics;

namespace PostcardCore
{
    class PostcardCore_METS_dmdSec_ReaderWriter : XML_Writing_Base_Type, iPackage_dmdSec_ReaderWriter
    {
        PostcardCore_Logme pclogme = new PostcardCore_Logme();

        public bool Include_dmdSec(SobekCM_Item METS_Item, Dictionary<string, object> Options)
        {
            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Include_dmdSec...");

            PostcardCore_Info postcardInfo = METS_Item.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

            if (postcardInfo == null)
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Include_dmdSec: postcardInfo is null.");
                return false;
            }
            else
            {
                if (!postcardInfo.hasData)
                {
                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Include_dmdSec: Not null, but no data, not including.");
                    return false;
                }
                else
                {
                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Include_dmdSec: Not null, has data, including.");
                    return true;
                }
            }
        }

        public bool Read_dmdSec(XmlReader Input_XmlReader, SobekCM_Item Return_Package, Dictionary<string, object> Options)
        {
            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: " + Return_Package.BibID + "_" + Return_Package.VID + "...");

            PostcardCore_Info postcardInfo = Return_Package.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

            if (postcardInfo == null)
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: postcardInfo was null, creating and adding...");
                postcardInfo = new PostcardCore_Info();
                Return_Package.Add_Metadata_Module("PostcardCore", postcardInfo);
            }

            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Beginning read loop.");

            do
            {
                if ((Input_XmlReader.NodeType == XmlNodeType.EndElement) && ((Input_XmlReader.Name == "METS:mdWrap") || (Input_XmlReader.Name == "mdWrap")))
                {
                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: first node was endelement type and mdWrap, returning.");
                    return true;
                }

                // get the right division information based on node type
                if (Input_XmlReader.NodeType == XmlNodeType.Element)
                {
                    string termname = Input_XmlReader.Name.ToLower();

                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: original termname=[" + termname + "].");

                    if (termname.IndexOf("postcard:") == 0)
                    {
                        termname = termname.Substring(9);
                    }

                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: trimmed termname=[" + termname + "].\r\n");

                    switch (termname)
                    {
                        case "imagecaption":

                            Input_XmlReader.Read();

                            if (Input_XmlReader.NodeType == XmlNodeType.Text)
                            {
                                if (Input_XmlReader.Value.Length > 0)
                                {
                                    postcardInfo.Image_Caption = Input_XmlReader.Value;
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: read imagecapation=[" + postcardInfo.Image_Caption + "].");
                                }
                            }

                            break;

                        case "format":

                            Input_XmlReader.Read();

                            if (Input_XmlReader.NodeType == XmlNodeType.Text)
                            {
                                if (Input_XmlReader.Value.Length > 0)
                                {
                                    postcardInfo.Format = Input_XmlReader.Value;
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: read format=[" + postcardInfo.Format + "].");
                                }
                            }

                            break;

                        case "productiondate":

                            Input_XmlReader.Read();

                            if (Input_XmlReader.NodeType == XmlNodeType.Text)
                            {
                                if (Input_XmlReader.Value.Length > 0)
                                {
                                    postcardInfo.Production_Date = Input_XmlReader.Value;
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: read productiondate=[" + postcardInfo.Production_Date + "].");
                                }
                            }

                            break;

                        case "era":

                            Input_XmlReader.Read();

                            if (Input_XmlReader.NodeType == XmlNodeType.Text)
                            {
                                if (Input_XmlReader.Value.Length > 0)
                                {
                                    postcardInfo.Era = Input_XmlReader.Value;
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: read era=[" + postcardInfo.Era + "].");
                                }
                            }

                            break;

                        case "productionnumber":

                            Input_XmlReader.Read();

                            if (Input_XmlReader.NodeType == XmlNodeType.Text)
                            {
                                if (Input_XmlReader.Value.Length > 0)
                                {
                                    postcardInfo.Production_Number = Input_XmlReader.Value;
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: read productionnumber=[" + postcardInfo.Production_Number + "].");
                                }
                            }

                            break;

                        case "postmarksent":

                            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: reading postmarksent...");

                            String city = String.Empty;
                            String state = String.Empty;
                            String datetime = String.Empty;

                            if (Input_XmlReader.HasAttributes)
                            {
                                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has attributes.");

                                if (Input_XmlReader.MoveToAttribute("city"))
                                {
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has city attribute.");
                                    Input_XmlReader.ReadAttributeValue();
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Read [" + Input_XmlReader.Value + "].");
                                    city = Input_XmlReader.Value;
                                }

                                if (Input_XmlReader.MoveToAttribute("state"))
                                {
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has state attribute.");
                                    Input_XmlReader.ReadAttributeValue();
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Read [" + Input_XmlReader.Value + "].");
                                    state = Input_XmlReader.Value;
                                }

                                if (Input_XmlReader.MoveToAttribute("datetime"))
                                {
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has datetime attribute.");
                                    Input_XmlReader.ReadAttributeValue();
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Read [" + Input_XmlReader.Value + "].");
                                    datetime = Input_XmlReader.Value;
                                }

                                postcardInfo.Postmark_Sent.Sender_city = city;
                                postcardInfo.Postmark_Sent.Sender_state = state;
                                postcardInfo.Postmark_Sent.Sender_datetime = datetime;

                                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: postmark sent: city=[" + city + "], state=[" + state + "], datetime=[" + datetime + "].");
                            }

                            break;

                        case "postmarkreceived":

                            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: reading postmarkreceived...");

                            city = String.Empty;
                            state = String.Empty;
                            datetime = String.Empty;

                            if (Input_XmlReader.HasAttributes)
                            {
                                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has attributes.");

                                if (Input_XmlReader.MoveToAttribute("city"))
                                {
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has city attribute.");
                                    Input_XmlReader.ReadAttributeValue();
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Read [" + Input_XmlReader.Value + "].");
                                    city = Input_XmlReader.Value;
                                }

                                if (Input_XmlReader.MoveToAttribute("state"))
                                {
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has state attribute.");
                                    Input_XmlReader.ReadAttributeValue();
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Read [" + Input_XmlReader.Value + "].");
                                    state = Input_XmlReader.Value;
                                }

                                if (Input_XmlReader.MoveToAttribute("datetime"))
                                {
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has datetime attribute.");
                                    Input_XmlReader.ReadAttributeValue();
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Read [" + Input_XmlReader.Value + "].");
                                    datetime = Input_XmlReader.Value;
                                }

                                postcardInfo.Postmark_Received.Receiver_city = city;
                                postcardInfo.Postmark_Received.Receiver_state = state;
                                postcardInfo.Postmark_Received.Receiver_Datetime = datetime;

                                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: postmark received: city=[" + city + "], state=[" + state + "], datetime=[" + datetime + "].");

                            }

                            break;

                        case "subject":

                            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: reading subject...");

                            String topic = String.Empty;
                            String perspective = String.Empty;
                            String name = String.Empty;

                            if (Input_XmlReader.HasAttributes)
                            {
                                if (Input_XmlReader.MoveToAttribute("topic"))
                                {
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has topic attribute.");
                                    Input_XmlReader.ReadAttributeValue();
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Read [" + Input_XmlReader.Value + "].");
                                    topic = Input_XmlReader.Value;
                                }

                                if (Input_XmlReader.MoveToAttribute("perspective"))
                                {
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has perspective attribute.");
                                    Input_XmlReader.ReadAttributeValue();
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Read [" + Input_XmlReader.Value + "].");
                                    perspective = Input_XmlReader.Value;
                                }

                                if (Input_XmlReader.MoveToAttribute("name"))
                                {
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: has name attribute.");
                                    Input_XmlReader.ReadAttributeValue();
                                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: Read [" + Input_XmlReader.Value + "].");
                                    name = Input_XmlReader.Value;
                                }

                                postcardInfo.Add_Subject(name, perspective, topic);
                                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: subject: name=[" + name + "], perspective=[" + perspective + "], topic=[" + topic + "].");

                            }

                            break;
                    }
                }
            } while (Input_XmlReader.Read());

            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Read_dmdSec: loop done.");

            return true;
        }

        public string[] Schema_Location(SobekCM_Item METS_Item)
        {
            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Schema_Location...");

            return new String[] { };
        }

        public string[] Schema_Namespace(SobekCM_Item METS_Item)
        {
            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Schema_Namespace...");

            return new string[] { "postcard=\"http://dis.lib.usf.edu/standards/postcard/postcard.xsd\"" };
        }

        public bool Schema_Reference_Required_Package(SobekCM_Item METS_Item)
        {
            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Schema_Reference_Required_Package.");

            PostcardCore_Info postcardInfo = METS_Item.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

            if (postcardInfo == null)
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Schema_Reference_Required_Package: postcardInfo was null.");
                return false;
            }
            else
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Schema_Reference_Required_Package: postcardInfo was NOT null.");

                if (postcardInfo.hasData)
                {
                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Schema_Reference_Required_Package: has data.");
                }
                else
                {
                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Schema_Reference_Required_Package: does NOT have data.");
                }

                return postcardInfo.hasData;
            }
        }

        public bool Write_dmdSec(TextWriter Output_Stream, SobekCM_Item METS_Item, Dictionary<string, object> Options)
        {
            pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: ...");

            PostcardCore_Info postcardInfo = METS_Item.Get_Metadata_Module("PostcardCore") as PostcardCore_Info;

            if (postcardInfo == null)
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: postcardInfo is null.");
                return false;
            }
            else
            {
                if (!postcardInfo.hasData)
                {
                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: Not null, but no data.");
                    return false;
                }
                else
                {
                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: Not null, has data.");
                }
            }

            Output_Stream.WriteLine("<postcard:postcard>");

            if (!String.IsNullOrEmpty(postcardInfo.Image_Caption))
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: writing image caption=[" + postcardInfo.Image_Caption + "].");
                Output_Stream.WriteLine("<postcard:imageCaption>" + Convert_String_To_XML_Safe(postcardInfo.Image_Caption) + "</postcard:imageCaption>");
            }

            if (!String.IsNullOrEmpty(postcardInfo.Format))
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: writing format=[" + postcardInfo.Format + "].");
                Output_Stream.WriteLine("<postcard:format>" + Convert_String_To_XML_Safe(postcardInfo.Format) + "</postcard:format>");
            }

            if (!String.IsNullOrEmpty(postcardInfo.Production_Date))
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: writing production date=[" + postcardInfo.Production_Date + "].");
                Output_Stream.WriteLine("<postcard:productionDate>" + Convert_String_To_XML_Safe(postcardInfo.Production_Date) + "</postcard:productionDate>");
            }

            if (!String.IsNullOrEmpty(postcardInfo.Era))
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: writing era=[" + postcardInfo.Era + "].");
                Output_Stream.WriteLine("<postcard:era>" + Convert_String_To_XML_Safe(postcardInfo.Era) + "</postcard:era>");
            }

            if (!String.IsNullOrEmpty(postcardInfo.Production_Number))
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: writing production number=[" + postcardInfo.Production_Number + "].");
                Output_Stream.WriteLine("<postcard:productionNumber>" + Convert_String_To_XML_Safe(postcardInfo.Production_Number) + "</postcard:productionNumber>");
            }

            if (postcardInfo.Postmark_Sent != null)
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: writing postmark sent data: city=[" + postcardInfo.Postmark_Sent.Sender_city + "], state=[" + postcardInfo.Postmark_Sent.Sender_state + "], datetime=[" + postcardInfo.Postmark_Sent.Sender_datetime + "].");
                Output_Stream.Write("<postcard:postmarkSent ");
                Output_Stream.Write("city =\"" + Convert_String_To_XML_Safe(postcardInfo.Postmark_Sent.Sender_city) + "\" ");
                Output_Stream.Write("state=\"" + Convert_String_To_XML_Safe(postcardInfo.Postmark_Sent.Sender_state) + "\" ");
                Output_Stream.Write("datetime=\"" + Convert_String_To_XML_Safe(postcardInfo.Postmark_Sent.Sender_datetime) + "\" ");
                Output_Stream.Write("/>");
                Output_Stream.WriteLine();
            }

            if (postcardInfo.Postmark_Received != null)
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: writing postmark received: city=[" + postcardInfo.Postmark_Received.Receiver_city + "], state=[" + postcardInfo.Postmark_Received.Receiver_state + "], datetime=[" + postcardInfo.Postmark_Received.Receiver_Datetime + "].");
                Output_Stream.Write("<postcard:postmarkReceived ");
                Output_Stream.Write("city=\"" + Convert_String_To_XML_Safe(postcardInfo.Postmark_Received.Receiver_city) + "\" ");
                Output_Stream.Write("state=\"" + Convert_String_To_XML_Safe(postcardInfo.Postmark_Received.Receiver_state) + "\" ");
                Output_Stream.Write("datetime=\"" + Convert_String_To_XML_Safe(postcardInfo.Postmark_Received.Receiver_Datetime) + "\" ");
                Output_Stream.Write("/>");
                Output_Stream.WriteLine();
            }

            if (postcardInfo.Subjects != null && postcardInfo.Subjects_Count > 0)
            {
                pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: writing [" + postcardInfo.Subjects_Count + "] subjects.");
                int k = 0;

                foreach (PostcardCore_Subject_Info subject in postcardInfo.Subjects)
                {
                    k++;
                    pclogme.logme("PostcardCore_METS_dmdSec_ReaderWriter: Write_dmdSec: writing #" + k + " subject name=[" + subject.Subject_Name + "], perspective=[" + subject.Subject_Perspective + "], topic=[" + subject.Subject_Topic + "].");

                    Output_Stream.Write("<postcard:subject ");

                    if (!String.IsNullOrEmpty(subject.Subject_Name))
                    {
                        Output_Stream.Write("name=\"" + Convert_String_To_XML_Safe(subject.Subject_Name) + "\" ");
                    }
                    else
                    {
                        Output_Stream.Write("name=\"\" ");
                    }

                    if (!String.IsNullOrEmpty(subject.Subject_Perspective))
                    {
                        Output_Stream.Write("perspective=\"" + Convert_String_To_XML_Safe(subject.Subject_Perspective) + "\" ");
                    }
                    else
                    {
                        Output_Stream.Write("perspective=\"\" ");
                    }

                    if (!String.IsNullOrEmpty(subject.Subject_Topic))
                    {
                        Output_Stream.Write("topic=\"" + Convert_String_To_XML_Safe(subject.Subject_Topic) + "\" ");
                    }
                    else
                    {
                        Output_Stream.Write("topic=\"\" ");
                    }

                    Output_Stream.Write("/>\r\n");
                }
            }

            Output_Stream.WriteLine("</postcard:postcard>");

            return true;
        }
    }
}
