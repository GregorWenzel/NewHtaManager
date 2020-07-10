using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public interface IEndpointDescriptor
    {
        int Id { get; set; }
        string Abbreviation { get; set; }
        string AbbreviationEN { get; set; }
        List<IEndpointDescriptor> ChildList { get; set; }
        EndpointDimensionType Dimension { get; set; }
        string DisplayName { get; }
        bool IsSelectable { get; }
        string Name { get; set; }
        string NameEN { get; set; }
        string NameWithAbbreviation { get; }
        IEndpointDescriptor Parent { get; set; }
        int ParentId { get; set; }
    }
}
