using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class PublicationAuthor
    { 
        public string FirstName { get; set; }
        public string Initials { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{LastName} {Initials}";
        }
    }
}
