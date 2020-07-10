using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class StudyArm
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<Intervention> InterventionList { get; set; }
        public string InterventionDescription { get; set; }
        public string InterventionName { get; set; }
        public string Title { get; set; }
        public StudyArmType Type { get; set; }
        public InterventionType InterventionType { get; set; }

        public StudyArm()
        {
            InterventionList = new List<Intervention>();
        }
    }
}
