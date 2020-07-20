using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class StudyDesignViewModel : ViewModelBase
    {
        private StudyPurposeType purpose;
        public StudyPurposeType Purpose
        {
            get => purpose;
            set => SetProperty(ref purpose, value);
        }

        private StudyPhaseType phase;
        public StudyPhaseType Phase
        {
            get => phase;
            set => SetProperty(ref phase, value);
        }

        private string eligibilityText;
        public string EligibilityText
        {
            get => eligibilityText;
            set => SetProperty(ref eligibilityText, value);
        }

        public string PhaseString
        {
            get => StudyPhaseTypeTypeString.Resolve[Phase];                
        }

        private bool hasHealthyPatient;
        public bool HasHealthyPatients
        {
            get => hasHealthyPatient;
            set => SetProperty(ref hasHealthyPatient, value);
        }

        private string minAge;
        public string MinAge
        {
            get => minAge;
            set => SetProperty(ref minAge, value);
        }

        private string maxAge;
        public string MaxAge
        {
            get => maxAge;
            set => SetProperty(ref maxAge, value);
        }

        private StudyGenderType genderType;
        public StudyGenderType GenderType
        {
            get => genderType;
            set => SetProperty(ref genderType, value);
        }

        private string type;
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        public string ModelString
        {
            get
            {
                if (InterventionModel != StudyInterventionModelType.NONE)
                {
                    return InterventionStudyModelTypeString.Resolve[InterventionModel];
                }
                else
                {
                    return StudyObservationalModelTypeString.Resolve[ObservationalModel] + " (" + StudyTimePerspectiveTypeString.Resolve[TimePerspective] + ")";
                }
            }
        }

        public string PurposeString
        {
            get => StudyPurposeTypeString.Resolve[Purpose];
        }

        public string RandomizationString
        {
            get => StudyAllocationTypeString.Resolve[Allocation];
        }

        public string MaskingString
        {
            get
            {
                int maskCount = MaskedPersonList.Count;

                switch (maskCount)
                {
                    case 0:
                        return "keine";
                    case 1:
                        return "einfach";
                    case 2:
                        return "doppelt";
                    case 3:
                        return "dreifach";
                    case 4:
                        return "vierfach";
                    default:
                        return "unbenkannt";
                }
            }
        }


        private ObservableCollection<StudyMaskedPersonType> maskedPersonList;
        public ObservableCollection<StudyMaskedPersonType> MaskedPersonList
        {
            get => maskedPersonList;
            set => SetProperty(ref maskedPersonList, value);
        }

        private StudyAllocationType allocation;
        public StudyAllocationType Allocation
        {
            get => allocation;
            set => SetProperty(ref allocation, value);
        }

        private string modelDescription;
        public string ModelDescription
        {
            get => modelDescription;
            set => SetProperty(ref modelDescription, value);
        }

        private StudyInterventionModelType interventionModel;
        public StudyInterventionModelType InterventionModel
        {
            get => interventionModel;
            set => SetProperty(ref interventionModel, value);
        }

        private string maskingDescription;
        public string MaskingDescription
        {
            get => maskingDescription;
            set => SetProperty(ref maskingDescription, value);
        }

        private int enrollment;
        public int Enrollment
        {
            get => enrollment;
            set => SetProperty(ref enrollment, value);
        }

        private StudyObservationalModelType observationalModel;
        public StudyObservationalModelType ObservationalModel
        {
            get => observationalModel;
            set => SetProperty(ref observationalModel, value);
        }

        private StudyTimePerspectiveType timePerspective;
        public StudyTimePerspectiveType TimePerspective
        {
            get => timePerspective;
            set => SetProperty(ref timePerspective, value);
        }

        private string followUpDuration;
        public string FollowUpDuration
        {
            get => followUpDuration;
            set => SetProperty(ref followUpDuration, value);
        }

        public string PopulationString
        {
            get => Enrollment != 0 ? $"Population (N={Enrollment})" : "Population";
        }

        public StudyDesignViewModel()
        {
            Allocation = StudyAllocationType.NONE;
            InterventionModel = StudyInterventionModelType.NONE;
            MaskedPersonList = new ObservableCollection<StudyMaskedPersonType>();
            ObservationalModel = StudyObservationalModelType.UNKNOWN;
            Phase = StudyPhaseType.UNKNOWN;
            Purpose = StudyPurposeType.OTHER;
        }

        public StudyDesignViewModel(StudyDesign design)
        {
            Allocation = StudyAllocationType.NONE;
            InterventionModel = StudyInterventionModelType.NONE;
            Purpose = StudyPurposeType.OTHER;

            this.Allocation = design.Allocation;
            this.EligibilityText = design.EligibilityText;
            this.Enrollment = design.Enrollment;
            this.FollowUpDuration = design.FollowUpDuration;
            this.GenderType = design.GenderType;
            this.HasHealthyPatients = design.HasHealthyPatients;
            this.InterventionModel = design.InterventionModel;
            this.MaskedPersonList = new ObservableCollection<StudyMaskedPersonType>(design.MaskedPersonList);
            this.MaxAge = design.MaxAge;
            this.MinAge = design.MinAge;
            this.ModelDescription = design.ModelDescription;
            this.ObservationalModel = design.ObservationalModel;
            this.Phase = design.Phase;
            this.Purpose = design.Purpose;
            this.TimePerspective = design.TimePerspective;
            this.Type = design.Type;
        }
    }
}