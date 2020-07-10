namespace HtaManager.Infrastructure.Domain
{
    public class OutcomeMeasure
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public EndpointDescriptor EndpointDescriptor { get; set; }
        public OutcomePriorityType EndpointPriority { get; set; }
        public string Name { get; set; }
        public Study Study { get; set; }
        public string TimeFrame { get; set; }
    }
}