using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum OutcomePriorityType
    {
        UNKNOWN,
        PRIMARY,
        SECONDARY,
        EXPLORATORY,
        OTHER
    }

    public static class OutcomePriorityTypeString
    {
        public static Dictionary<OutcomePriorityType, string> Resolve = new Dictionary<OutcomePriorityType, string>
        {
            { OutcomePriorityType.UNKNOWN, "unbekannt" },
            { OutcomePriorityType.PRIMARY, "primär" },
            { OutcomePriorityType.SECONDARY, "sekundär" },
            { OutcomePriorityType.EXPLORATORY, "explorativ" },
            { OutcomePriorityType.OTHER, "andere" }

        };
    }
}