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
                
        public static EndpointDescriptor FromViewModel(EndpointDescriptorViewModel endpointDescriptor)
        {
            EndpointDescriptor result = new EndpointDescriptor();

            if (endpointDescriptor != null)
            {
                result.Id = endpointDescriptor.Id;
                result.Abbreviation = endpointDescriptor.Abbreviation;
                result.AbbreviationEN = endpointDescriptor.AbbreviationEN;
                result.ChildList = new List<EndpointDescriptor>(endpointDescriptor.ChildList.Select(item => EndpointDescriptor.FromViewModel(item)));
                result.Dimension = endpointDescriptor.Dimension;
                result.Id = endpointDescriptor.Id;
                result.Name = endpointDescriptor.Name;
                result.NameEN = endpointDescriptor.NameEN;
                result.ParentId = endpointDescriptor.ParentId;
            }

            return result;
        }
    }
}
