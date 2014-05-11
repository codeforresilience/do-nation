using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_Nation
{
    class Notification
    {
        public string id { get; set; }

        public string userid { get; set; }

        public string volunteerid { get; set; }

        public string status { get; set; }

        // PENDING      -       Waiting for user's response
        // APPROVED     -       User's Decision
        // DECLINED     -       User's Decision

        public string postid { get; set; }

        public Boolean isreadbyvolunteer { get; set; }
    }
}
