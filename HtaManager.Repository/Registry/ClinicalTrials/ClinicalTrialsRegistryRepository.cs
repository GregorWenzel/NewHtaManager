using HtaManager.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Unity;

namespace HtaManager.Repository
{
    public class ClinicalTrialsRegistryRepository : IRegistryRepository
    {
        IUnityContainer container;

        public ClinicalTrialsRegistryRepository(IUnityContainer container)
        {
            this.container = container;
        }

        public Study RequestStudy(string Identifier)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string url = @"https://clinicaltrials.gov/api/query/full_studies?expr=" + Identifier + "&max_rnk=1&fmt=XML";

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string xmlString = client.DownloadString(url);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            ClinicalTrialsXmlParser parser = new ClinicalTrialsXmlParser(container.Resolve<List<EndpointDescriptor>>());
            Study study = parser.Parse(doc);

            return study;
        }
    }
}