using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class Intervention
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<string> OtherNameList { get; set; }
        public List<string> StudyArmNameList { get; set; }
        public InterventionType Type { get; set; }

        public Intervention()
        {
            OtherNameList = new List<string>();
            StudyArmNameList = new List<string>();
        }
    }
}