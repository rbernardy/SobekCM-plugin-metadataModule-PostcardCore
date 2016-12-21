using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostcardCore
{
    [Serializable]
    public class PostcardCore_PostmarkReceived_Info
    {
        public PostcardCore_Logme pclogme = new PostcardCore_Logme();

        private String postmark_receiver_city;
        private String postmark_receiver_state;
        private String postmark_receiver_datetime;

        public PostcardCore_PostmarkReceived_Info()
        {
            postmark_receiver_city = String.Empty;
            postmark_receiver_state = String.Empty;
            postmark_receiver_datetime = String.Empty;
        }

        public PostcardCore_PostmarkReceived_Info(String Receiver_city, String Receiver_state, String Receiver_datetime)
        {
            postmark_receiver_city = Receiver_city;
            postmark_receiver_state = Receiver_state;
            postmark_receiver_datetime = Receiver_datetime;
        }

        public String Receiver_city
        {
            get { return postmark_receiver_city ?? String.Empty; }
            set { postmark_receiver_city = value; }
        }

        public String Receiver_state
        {
            get { return postmark_receiver_state ?? String.Empty; }
            set { postmark_receiver_state = value; }
        }

        public String Receiver_Datetime
        {
            get { return postmark_receiver_datetime ?? String.Empty; }
            set { postmark_receiver_datetime = value; }
        }
    }
}