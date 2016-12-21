using System;

namespace PostcardCore
{
    [Serializable]
    public class PostcardCore_Subject_Info
    {
        private String subjectName;
        private String subjectPerspective;
        private String subjectTopic;

        public PostcardCore_Subject_Info()
        {
        }

        public PostcardCore_Subject_Info(String SubjectName,String SubjectPerspective,String SubjectTopic)
        {
            subjectName = SubjectName;
            subjectPerspective = SubjectPerspective;
            subjectTopic = SubjectTopic;
        }

        public String Subject_Topic
        {
            get { return subjectTopic ?? String.Empty; }
            set { subjectTopic = value; }
        }

        public String Subject_Perspective
        {
            get { return subjectPerspective ?? String.Empty; }
            set { subjectPerspective = value; }
        }

        public String Subject_Name
        {
            get { return subjectName ?? String.Empty; }
            set { subjectName = value; }
        }
    }
}
