using HtaManager.Infrastructure.Domain;
using System.Collections.Generic;

namespace HtaManager.Repository.Endpoint
{
    public interface IEndpointRepository
    {
        bool DeleteEndpointDescriptor(EndpointDescriptor EndpointDescriptor);
        List<EndpointDescriptor> GetEndpointDescriptorList();
        int SaveEndpointDescriptor(EndpointDescriptor EndpointDescriptor);
        int SaveEndpointDescriptor(EndpointDescriptorViewModel EndpointDescriptor);
    }
}