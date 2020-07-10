using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class PublicationAbstract
    {
        public List<PublicationAbstractItem> AbstractItemList;

        public PublicationAbstract()
        {
            AbstractItemList = new List<PublicationAbstractItem>();
        }
    }
}
