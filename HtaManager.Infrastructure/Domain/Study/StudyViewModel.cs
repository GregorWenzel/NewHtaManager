using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class StudyViewModel: ViewModelBase
    {
        private string nctId;
        public string NctId
        {
            get => nctId;
            set => SetProperty(ref nctId, value);
        }

        private ObservableCollection<string> secondaryIdentifierList;
        public ObservableCollection<string> SecondaryIndentifierList
        {
            get => secondaryIdentifierList;
            set => SetProperty(ref secondaryIdentifierList, value);
        }

        private string shortTitle;
        public string ShortTitle
        {
            get => shortTitle;
            set => SetProperty(ref shortTitle, value);
        }

        private string fullTitle;
        public string FullTitle
        {
            get => fullTitle;
            set => SetProperty(ref fullTitle, value);
        }

        private ObservableCollection<string> keywordList;
        public ObservableCollection<string> KeywordList
        {
            get => keywordList;
            set => SetProperty(ref keywordList, value);
        }

        private ObservableCollection<ConditionViewModel> conditionList;

        public ObservableCollection<ConditionViewModel> ConditionList
        {
            get => conditionList;
            set => SetProperty(ref conditionList, value);
        }

        private DateTime firstSubmittedDate;
        public DateTime FirstSubmittedDate
        {
            get => firstSubmittedDate;
            set => SetProperty(ref firstSubmittedDate, value);
        }

        private DateTime firstResultsSubmittedDate;
        public DateTime FirstResultsSubmittedDate
        {
            get => firstResultsSubmittedDate;
            set => SetProperty(ref firstResultsSubmittedDate, value);
        }

        private DateTime lastUpdatePostedDate;
        public DateTime LastUpdatePostedDate
        {
            get => lastUpdatePostedDate;
            set => SetProperty(ref lastUpdatePostedDate, value);
        }

        private DateTime actualStudyStartDate;
        public DateTime ActualStudyStartDate
        {
            get => actualStudyStartDate;
            set => SetProperty(ref actualStudyStartDate, value);
        }

        private DateTime actualPrimaryCompletionDate;
        public DateTime ActualPrimaryCompletionDate
        {
            get => actualPrimaryCompletionDate;
            set => SetProperty(ref actualPrimaryCompletionDate, value);
        }

        private string durationPlanned;
        public string DurationPlanned
        {
            get => durationPlanned;
            set => SetProperty(ref durationPlanned, value);
        }

        private ObservableCollection<OutcomeMeasure> endpointList;
        public ObservableCollection<OutcomeMeasure> EndpointList
        {
            get => endpointList;
            set => SetProperty(ref endpointList, value);
        }

        private StudyDesignViewModel design;
        public StudyDesignViewModel Design
        {
            get => design;
            set => SetProperty(ref design, value);
        }

        private ObservableCollection<StudyArmViewModel> studyArmList;
        public ObservableCollection<StudyArmViewModel> StudyArmList
        {
            get => studyArmList;
            set => SetProperty(ref studyArmList, value);
        }

        private string briefDescription;
        public string BriefDescription
        {
            get => briefDescription;
            set => SetProperty(ref briefDescription, value);
        }

        private string detailedDescription;
        public string DetailedDescription
        {
            get => detailedDescription;
            set => SetProperty(ref detailedDescription, value);
        }

        public string RegisterText
        {
            get
            {
                if (!string.IsNullOrEmpty(NctId))
                {
                    return $"{NctId}: {FullTitle}";
                }
                else
                {
                    return "";
                }
            }
        }

        public string InterventionText
        {
            get
            {
                List<string> result = new List<string>();
                foreach (StudyArmViewModel arm in StudyArmList)
                {
                    result.Add($"{arm.Title} ({string.Join(", ", arm.InterventionList.Select(item => item.Name))})");
                }

                return string.Join("\r\nvs.\r\n", result);
            }
        }

        public string EndpointText
        {
            get
            {
                List<OutcomeMeasure> resultList = EndpointList.OrderBy(item => item.EndpointPriority).GroupBy(item => item.EndpointDescriptor.Id).Select(item2 => item2.OrderBy(item3 => item3.EndpointDescriptor.Id).First()).ToList();

                return string.Join("\r\n", resultList.Select(item => $"{item.EndpointDescriptor.DisplayName} - {OutcomePriorityTypeString.Resolve[item.EndpointPriority]}"));
            }
        }
       
        private ObservableCollection<PublicationViewModel> publicationList;
        public ObservableCollection<PublicationViewModel> PublicationList
        {
            get => publicationList;
            set => SetProperty(ref publicationList, value);
        }

        public string PublicationTitle
        {
            get
            {
                if (PublicationList.Count == 0) return FullTitle;
                else return PublicationList.Last().Citation;
            }
        }

        public StudyViewModel()
        {
            ConditionList = new ObservableCollection<ConditionViewModel>();
            Design = new StudyDesignViewModel();
            EndpointList = new ObservableCollection<OutcomeMeasure>();
            KeywordList = new ObservableCollection<string>();
            PublicationList = new ObservableCollection<PublicationViewModel>();
            SecondaryIndentifierList = new ObservableCollection<string>();
            StudyArmList = new ObservableCollection<StudyArmViewModel>();
        }

        public StudyViewModel(Study study)
        {
            this.ActualPrimaryCompletionDate = study.ActualPrimaryCompletionDate;
            this.ActualStudyStartDate = study.ActualPrimaryCompletionDate;
            this.ConditionList = new ObservableCollection<ConditionViewModel>(study.ConditionList.Select(item => new ConditionViewModel(item)));
            this.Design = new StudyDesignViewModel(study.Design);
            this.Design.EligibilityText = study.Design.EligibilityText;
            this.EndpointList = new ObservableCollection<OutcomeMeasure>(study.EndpointList);
            this.FirstResultsSubmittedDate = study.FirstResultsSubmittedDate;
            this.FirstSubmittedDate = study.FirstSubmittedDate;
            this.FullTitle = study.FullTitle;
            this.KeywordList = new ObservableCollection<string>(study.KeywordList);
            this.LastUpdatePostedDate = study.LastUpdatePostedDate;
            this.NctId = study.NctId;
            this.PublicationList = new ObservableCollection<PublicationViewModel>(study.PublicationList.Select(item => new PublicationViewModel(item)));
            this.SecondaryIndentifierList = new ObservableCollection<string>(study.SecondaryIndentifierList);
            this.ShortTitle = study.ShortTitle;
            this.StudyArmList = new ObservableCollection<StudyArmViewModel>(study.StudyArmList.Select(item => new StudyArmViewModel(item)));
        }
    }
}
