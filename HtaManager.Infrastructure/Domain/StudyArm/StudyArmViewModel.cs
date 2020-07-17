using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class StudyArmViewModel : ViewModelBase
    {
        public int Id { get; set; }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private StudyArmType type;
        public StudyArmType Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        private InterventionType interventionType;
        public InterventionType InterventionType
        {
            get => interventionType;
            set => SetProperty(ref interventionType, value);
        }

        private ObservableCollection<InterventionViewModel> interventionList;
        public ObservableCollection<InterventionViewModel> InterventionList
        {
            get => interventionList;
            set => SetProperty(ref interventionList, value);
        }

        private string interventionName;
        public string InterventionName
        {
            get => interventionName;
            set => SetProperty(ref interventionName, value);
        }

        private string interventionDescription;
        public string InterventionDescription
        {
            get => interventionDescription;
            set => SetProperty(ref interventionDescription, value);
        }

        public string InterventionNameLabel
        {
            get => string.Join(",", InterventionList.Select(item => item.Name));
        }
           
        public string InterventionTypeLabel
        {
            get => InterventionTypeString.Resolve[InterventionType];
        }

        public StudyArmViewModel()
        {
            InterventionList = new ObservableCollection<InterventionViewModel>();
        }

        public StudyArmViewModel(StudyArm arm)
        {
            this.Description = arm.Description;
            this.Id = arm.Id;
            this.InterventionDescription = arm.InterventionDescription;
            this.InterventionList = new ObservableCollection<InterventionViewModel>(arm.InterventionList.Select(item => new InterventionViewModel(item)));
            this.InterventionName = arm.InterventionName;
            this.InterventionType = arm.InterventionType;
            this.Title = arm.Title;
            this.Type = arm.Type;
        }
    }
}
