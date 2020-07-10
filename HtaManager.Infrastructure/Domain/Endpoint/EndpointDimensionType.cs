using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum EndpointDimensionType
    {
        UNKNOWN,
        MORTALITY,
        MORBIDITY,
        QOL,
        SAFETY,
        OTHER
    }

    public static class EndpointDimensionTypeString
    {
        public static Dictionary<EndpointDimensionType, string> Resolve = new Dictionary<EndpointDimensionType, string>
        {
            { EndpointDimensionType.UNKNOWN, "Keine" },
            { EndpointDimensionType.MORTALITY, "Mortalität" },
            { EndpointDimensionType.MORBIDITY, "Morbidität" },
            { EndpointDimensionType.QOL, "Lebensqualität" },
            { EndpointDimensionType.SAFETY, "Nebenwirkungen" },
            { EndpointDimensionType.OTHER, "Andere" }

        };
    }
}
