using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype.Data
{
    public class CandidateInfo
    {
        public int CandidateId { get; set; }

        public String DisplayName { get; set; }
        public Boolean IsIncumbent { get; set; }

        public PartyTypeEnum Party { get; set; }

        public String FirstName { get; set; }

        public String MiddleName { get; set; }

        public String LastName { get; set; }
    }
}
