using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum StudyTimePerspectiveType
    {
        UNKNOWN,
        RETROSPECTIVE,
        PROSPECTIVE,
        CROSS_SECTIONAL,
        OTHER,
    }

    public static class StudyTimePerspectiveParser
    {
        public static StudyTimePerspectiveType Parse(string valueString)
        {
            switch (valueString)
            {
                case "Retrospective":
                    return StudyTimePerspectiveType.RETROSPECTIVE;
                case "Prospective":
                    return StudyTimePerspectiveType.PROSPECTIVE;
                case "Cross-Sectional":
                    return StudyTimePerspectiveType.CROSS_SECTIONAL;
                case "Other":
                    return StudyTimePerspectiveType.OTHER;
                default:
                    return StudyTimePerspectiveType.UNKNOWN;
            }
        }
    }

    public static class StudyTimePerspectiveTypeString
    {
        public static Dictionary<StudyTimePerspectiveType, string> Resolve = new Dictionary<StudyTimePerspectiveType, string>
        {
            { StudyTimePerspectiveType.UNKNOWN, "unbekannt" },
            { StudyTimePerspectiveType.RETROSPECTIVE, "retrospektiv" },
            { StudyTimePerspectiveType.PROSPECTIVE, "prospektiv" },
            { StudyTimePerspectiveType.CROSS_SECTIONAL, "Querschnitt" },
            { StudyTimePerspectiveType.OTHER, "andere" },
        };
    }
}

