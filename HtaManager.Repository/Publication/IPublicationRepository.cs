using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtaManager.Infrastructure.Domain;

namespace HtaManager.Repository
{
    public interface IPublicationRepository
    {
        Publication RequestPublication(string PMID);
    }
}
