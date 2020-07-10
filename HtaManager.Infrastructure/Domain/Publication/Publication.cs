using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class Publication
    {
        public string AbstractText { get; set; }
        public List<PublicationAuthor> AuthorList { get; set; }
        public string DOI { get; set; }
        public string Issue { get; set; }
        public string JournalFullTitle { get; set; }
        public string JournalShortTitle { get; set; }
        public List<PublicationMeshHeading> MeshHeadingList { get; set; }
        public string NctId { get; set; }
        public string Pagination { get; set; }
        public string PMID { get; set; }
        public PublicationAbstract PublicationAbstract { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Title { get; set; }
        public PublicationType Type { get; set; }
        public string Volume { get; set; }
    }
}
