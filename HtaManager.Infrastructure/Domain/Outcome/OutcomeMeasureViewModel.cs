using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class OutcomeMeasureViewModel : ViewModelBase
    {
        public int Id {get;set;}

        private EndpointDescriptorViewModel endpointDescriptor;
        public EndpointDescriptorViewModel EndpointDescriptor
        {
            get => endpointDescriptor;
            set => SetProperty(ref endpointDescriptor, value);
        }

        public Study Study { get; set; }

        private string name;
        public string Name
        {
            get => name;
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

        public OutcomeMeasureViewModel()
        {

        }

        public OutcomeMeasureViewModel(OutcomeMeasure outcomeMeasure)
        {
            this.Description = outcomeMeasure.Description;
            this.EndpointDescriptor = new EndpointDescriptorViewModel(outcomeMeasure.EndpointDescriptor);
            this.EndpointPriority = outcomeMeasure.EndpointPriority;
            this.Id = outcomeMeasure.Id;
            this.Name = outcomeMeasure.Name;
            this.Study = outcomeMeasure.Study;
            this.TimeFrame = outcomeMeasure.TimeFrame;
        }
    }
}
