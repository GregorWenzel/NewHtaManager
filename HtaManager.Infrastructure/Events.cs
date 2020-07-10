using HtaManager.Infrastructure.Domain;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure
{
    public class EndpointTypeListUpdated : PubSubEvent { }
    public class NewEndpointTypeEvent : PubSubEvent { }
    public class SelectedEndpointTypeChangedEvent : PubSubEvent<EndpointDescriptor> { }
    public class SelectedStudyChangedEvent : PubSubEvent<Study> { }
    public class TranslateStudyEvent : PubSubEvent { }
}
