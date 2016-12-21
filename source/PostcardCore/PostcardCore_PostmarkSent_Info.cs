using System;

namespace PostcardCore
{
    [Serializable]
    public class PostcardCore_PostmarkSent_Info
    {
        private String postmark_sender_city;
        private String postmark_sender_state;
        private String postmark_sender_datetime;

        public PostcardCore_PostmarkSent_Info()
        {
        }

        public PostcardCore_PostmarkSent_Info(String Sender_city,String Sender_state, String Sender_datetime)
        {
            postmark_sender_city = Sender_city;
            postmark_sender_state = Sender_state;
            postmark_sender_datetime = Sender_datetime;
        }

        public String Sender_city
        {
            get { return postmark_sender_city ?? String.Empty; }
            set { postmark_sender_city = value; }
        }

        public String Sender_state
        {
            get { return postmark_sender_state ?? String.Empty; }
            set { postmark_sender_state = value; }
        }

        public String Sender_datetime
        {
            get { return postmark_sender_datetime ?? String.Empty; }
            set { postmark_sender_datetime = value; }
        }
    }
}