using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_Nation
{
    class Post
    {
        public string id { get; set; }

        public string availabletime { get; set; }

        public string userid { get; set; }

        public string volunteerid { get; set; }

        public string specialinstruction { get; set; }

        public DateTime dateposted { get; set; }

        public DateTime datepickedup { get; set; }

        public string status { get; set; }
        // NEW                       -       newly posted
        // PENDING FOR APPROVAL      -       waiting for donator's approval
        // WAITING FOR PICKUP        -       donator accepted volunteer's request
        // DELIVERED                 -       volunteer received it

        public string address { get; set; }
    }
}
