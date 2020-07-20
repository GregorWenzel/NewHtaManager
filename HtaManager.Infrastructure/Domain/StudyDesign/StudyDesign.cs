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
        public StudyObservationalModelType ObservationalModel { get; set; }
        public StudyPhaseType Phase { get; set; }
        public string PhaseString { get; }
        public StudyPurposeType Purpose { get; set; }
        public string PurposeString { get; }
        public string RandomizationString { get; }
        public StudyTimePerspectiveType TimePerspective { get; set; }
        public string Type { get; set; }

        public StudyDesign()
        {
            MaskedPersonList = new List<StudyMaskedPersonType>();
            ObservationalModel = StudyObservationalModelType.UNKNOWN;
            InterventionModel = StudyInterventionModelType.UNKNOWN;
            Phase = StudyPhaseType.UNKNOWN;
            TimePerspective = StudyTimePerspectiveType.UNKNOWN;
        }
    }
}