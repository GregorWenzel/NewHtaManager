using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum StudyArmType
    {
        EXPERIMENTAL,
        ACTIVE_COMPARATOR,
        PLACEBO_COMPARATOR,
        SHAM_COMPARATOR,
        NO_INTERVENTION,
        OTHER,
        UNKNOWN
    }

    public static class StudyArmParser
    {
        public static StudyArmType Parse(string valueString)
        {
            switch (valueString)
            {
                case "Experimental":
                    return StudyArmType.EXPERIMENTAL;
                case "Active Comparator":
                    return StudyArmType.ACTIVE_COMPARATOR;
                case "Placebo Comparator":
                    return StudyArmType.PLACEBO_COMPARATOR;
                case "Sham Comparator":
                    return StudyArmType.SHAM_COMPARATOR;
                case "No Intervention":
                    return StudyArmType.NO_INTERVENTION;
                case "Other":
                    return StudyArmType.OTHER;
                default:
                    return StudyArmType.UNKNOWN;
            }
        }
    }

    public static class StudyArmTypeString
    {
        public static Dictionary<StudyArmType, string> Resolve = new Dictionary<StudyArmType, string>
        {
            { StudyArmType.EXPERIMENTAL, "Experimentell" },
            { StudyArmType.ACTIVE_COMPARATOR, "Aktive Kontrolle" },
            { StudyArmType.PLACEBO_COMPARATOR, "Placebo" },
            { StudyArmType.SHAM_COMPARATOR, "Scheinintervention" },
            { StudyArmType.NO_INTERVENTION, "Keine Intervention" },
            { StudyArmType.OTHER, "Andere" },
            { StudyArmType.UNKNOWN, "unbekannt" }
        };
    }
}
