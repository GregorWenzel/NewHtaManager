using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class EndpointDescriptor
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string AbbreviationEN { get; set; }
        public List<EndpointDescriptor> ChildList { get; set; }
        public EndpointDimensionType Dimension { get; set; }
        public string DisplayName { get; }
        public bool IsSelectable { get; }
        public string Name { get; set; }
        public string NameEN { get; set; }
        public string NameWithAbbreviation { get; }
        public EndpointDescriptor Parent { get; set; }
        public int ParentId { get; set; }

        public EndpointDescriptor()
        {
            ChildList = new List<EndpointDescriptor>();
        }
    }
}
