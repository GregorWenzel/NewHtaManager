using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum StudyAllocationType
    {
        NONE,
        RANDOMIZED,
        NONRANDOMIZED,
        UNKNOWN
    }

    public static class StudyAllocationParser
    {
        public static StudyAllocationType Parse(string valueString)
        {
            switch (valueString)
            {
                case "Not Applicable":
                case "N/A":
                    return StudyAllocationType.NONE;
                case "Randomized":
                    return StudyAllocationType.RANDOMIZED;
                case "Non-Randomized":
                    return StudyAllocationType.NONRANDOMIZED;
                default:
                    return StudyAllocationType.UNKNOWN;
            }
        }
    }

    public static class StudyAllocationTypeString
    {
        public static Dictionary<StudyAllocationType, string> Resolve = new Dictionary<StudyAllocationType, string>
        {
            { StudyAllocationType.UNKNOWN, "unbekannt" },
            { StudyAllocationType.NONE, "keine" },
            { StudyAllocationType.RANDOMIZED, "randomisiert" },
            { StudyAllocationType.NONRANDOMIZED, "nicht randomisiert" },
        };
    }
}
