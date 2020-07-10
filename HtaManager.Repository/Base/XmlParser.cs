using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HtaManager.Repository.Base
{
    public class XmlParser
    {
        protected XmlDocument doc;

        public string ParseSingleNode(string Path)
        {
            XmlNode node = doc.DocumentElement.SelectSingleNode(Path);

            if (node is object)
            {
                return node.InnerText;
            }
            else
            {
                return "";
            }
        }

        public DateTime ParseDateTimeNode(XmlNode node, string Path)
        {
            DateTime result = new DateTime();
            if (node.SelectSingleNode(Path) is object)
            {
                DateTime.TryParse(node.SelectSingleNode(Path).InnerText, out result);
            }

            return result;
        }

        public string ParseSingleNode(XmlNode node, string Path)
        {
            if (node.SelectSingleNode(Path) is object)
            {
                return node.SelectSingleNode(Path).InnerText;
            }
            else
            {
                return "";
            }
        }
    }
}