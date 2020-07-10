using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    class OutcomeMeasureViewModel : ViewModelBase
    {
        public int Id {get;set;}

        public EndpointDescriptor EndpointDescriptor { get; set; }
        public Study Study { get; set; }

        private string name;
        public string Name
        {
            get => Name;
            set => SetProperty(ref name, value);
        }

        private string timeFrame;
        public string TimeFrame
        {
            get => timeFrame;
            set => SetProperty(ref timeFrame, value);
        }

        private OutcomePriorityType endpointPriority;
        public OutcomePriorityType EndpointPriority
        {
            get => endpointPriority;
            set => SetProperty(ref endpointPriority, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
    }
}
