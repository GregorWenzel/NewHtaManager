using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum StudyPurposeType
    {
        UNKNOWN,
        TREATMENT,
        PREVENTION,
        DIAGNOSTIC,
        SUPPORTIVE_CARE,
        SCREENING,
        HEALTH_SERVICES_RESEARCH,
        BASIC_SCIENCE,
        DEVICE_FEASIBILITY,
        OTHER
    }

    public static class StudyPurposeParser
    {
        public static StudyPurposeType Parse(string valueString)
        {
            switch (valueString)
            {
                case "Treatment":
                    return StudyPurposeType.TREATMENT;
                case "Prevention":
                    return StudyPurposeType.PREVENTION;
                case "Diagnostic":
                    return StudyPurposeType.DIAGNOSTIC;
                case "Supportive Care":
                    return StudyPurposeType.SUPPORTIVE_CARE;
                case "Screening":
                    return StudyPurposeType.SCREENING;
                case "Health Services Research":
                    return StudyPurposeType.HEALTH_SERVICES_RESEARCH;
                case "Basic Science":
                    return StudyPurposeType.BASIC_SCIENCE;
                case "Device Feasibility":
                    return StudyPurposeType.DEVICE_FEASIBILITY;
                case "Other":
                    return StudyPurposeType.OTHER;
                default:
                    return StudyPurposeType.UNKNOWN;
            }
        }
    }

    public static class StudyPurposeTypeString
    {
        public static Dictionary<StudyPurposeType, string> Resolve = new Dictionary<StudyPurposeType, string>
        {
            { StudyPurposeType.UNKNOWN, "unbekannt" },
            { StudyPurposeType.OTHER, "andere" },
            { StudyPurposeType.TREATMENT, "Behandlung" },
            { StudyPurposeType.PREVENTION, "Prävention" },
            { StudyPurposeType.DIAGNOSTIC, "Diagnose" },
            { StudyPurposeType.SUPPORTIVE_CARE, "Unterstützende Behandlung" },
            { StudyPurposeType.SCREENING, "Screening" },
            { StudyPurposeType.HEALTH_SERVICES_RESEARCH, "Forschung im Gesundheitswesen" },
            { StudyPurposeType.BASIC_SCIENCE, "Grundlagenforschung" },
            { StudyPurposeType.DEVICE_FEASIBILITY, "Geräte-Realisierbarkeit" },
        };
    }
}