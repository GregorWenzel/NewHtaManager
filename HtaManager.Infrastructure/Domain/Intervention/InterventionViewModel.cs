using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class InterventionViewModel : ViewModelBase
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private InterventionType type;
        public InterventionType Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        private List<string> otherNameList;
        public List<string> OtherNameList
        {
            get => otherNameList;
            set => SetProperty(ref otherNameList, value);
        }

        private List<string> studyArmNameList;
        public List<string> StudyArmNameList
        {
            get => studyArmNameList;
            set => SetProperty(ref studyArmNameList, value);
        }

        public InterventionViewModel(Intervention intervention)
        {
            this.Description = intervention.Description;
            this.Name = intervention.Name;
            this.OtherNameList = intervention.OtherNameList;
            this.StudyArmNameList = intervention.StudyArmNameList;
            this.Type = intervention.Type;
        }
    }
}
