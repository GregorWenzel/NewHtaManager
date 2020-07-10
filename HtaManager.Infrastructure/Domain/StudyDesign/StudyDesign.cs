using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HtaManager.Infrastructure.Domain
{
    public class StudyDesign
    {
        public StudyAllocationType Allocation { get; set; }
        public string DurationPlanned { get; set; }
        public string EligibilityText { get; set; }
        public int Enrollment { get; set; }
        public string FollowUpDuration { get; set; }
        public StudyGenderType GenderType { get; set; }
        public bool HasHealthyPatients { get; set; }
        public StudyInterventionModelType InterventionModel { get; set; }
        public List<StudyMaskedPersonType> MaskedPersonList { get; set; }
        public string MaskingDescription { get; set; }
        public string MaskingString { get; }
        public string MaxAge { get; set; }
        public string MinAge { get; set; }
        public string ModelDescription { get; set; }
        public string ModelString { get; }
        public List<StudyObservationalModelType> ObservationalModelList { get; set; }
        public List<StudyPhaseType> PhaseList { get; set; }
        public string PhaseString { get; }
        public StudyPurposeType Purpose { get; set; }
        public string PurposeString { get; }
        public string RandomizationString { get; }
        public List<StudyTimePerspectiveType> TimePerspectiveList { get; set; }
        public string Type { get; set; }

        public StudyDesign()
        {
            MaskedPersonList = new List<StudyMaskedPersonType>();
            ObservationalModelList = new List<StudyObservationalModelType>();
            PhaseList = new List<StudyPhaseType>();
            TimePerspectiveList = new List<StudyTimePerspectiveType>();
        }
    }
}