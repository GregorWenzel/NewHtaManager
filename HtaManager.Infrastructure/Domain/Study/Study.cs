using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class Study
    {
        public int Id { get; set; }
        public DateTime ActualPrimaryCompletionDate { get; set; }
        public DateTime ActualStudyStartDate { get; set; }
        public List<Condition> ConditionList { get; set; }
        public StudyDesign Design { get; set; }
        public List<OutcomeMeasure> EndpointList { get; set; }
        public DateTime FirstResultsSubmittedDate { get; set; }
        public DateTime FirstSubmittedDate { get; set; }
        public string FullTitle { get; set; }
        public List<string> KeywordList { get; set; }
        public DateTime LastUpdatePostedDate { get; set; }
        public string NctId { get; set; }
        public List<string> SecondaryIndentifierList { get; set; }
        public string ShortTitle { get; set; }
        public List<StudyArm> StudyArmList { get; set; }
        public List<Publication> PublicationList { get; set; }
        public Study OriginalLanguageStudy { get; set; }
        public string BriefDescription { get; set; }
        public string DetailedDescription { get; set; }

        public Study()
        {
            Design = new StudyDesign();
            PublicationList = new List<Publication>();
            StudyArmList = new List<StudyArm>();
            KeywordList = new List<string>();
            EndpointList = new List<OutcomeMeasure>();
            ConditionList = new List<Condition>();
            SecondaryIndentifierList = new List<string>();
        }
    }
}
