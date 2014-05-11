using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_Nation
{
    class OrganizationKeyList
    {
        public string id { get; set; }

        public string organizationid { get; set; }

        public string volunteerid { get; set; }

        public string status { get; set; }
        // PENDING
        // APPROVED
        // DECLINED

        public Boolean isreadbyvolunteer { get; set; }
    }
}
