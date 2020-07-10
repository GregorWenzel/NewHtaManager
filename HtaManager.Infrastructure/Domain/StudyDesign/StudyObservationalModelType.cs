using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum StudyObservationalModelType
    {
        COHORT,
        CASE_CONTROL,
        CASE_ONLY,
        CASE_CROSSOVER,
        ECOLOGIC,
        FAMILY_BASED,
        OTHER,
        UNKNOWN
    }

    public static class StudyObservationalModelParser
    {
        public static StudyObservationalModelType Parse(string valueString)
        {
            switch (valueString)
            {
                case "Cohort":
                    return StudyObservationalModelType.COHORT;
                case "Case Control":
                    return StudyObservationalModelType.CASE_CONTROL;
                case "Case Only":
                    return StudyObservationalModelType.CASE_ONLY;
                case "Case Crossover":
                    return StudyObservationalModelType.CASE_CROSSOVER;
                case "Ecologic":
                    return StudyObservationalModelType.ECOLOGIC;
                case "Family-Based":
                    return StudyObservationalModelType.FAMILY_BASED;
                case "Other":
                    return StudyObservationalModelType.OTHER;
                default:
                    return StudyObservationalModelType.UNKNOWN;
            }
        }
    }

    public static class StudyObservationalModelTypeString
    {
        public static Dictionary<StudyObservationalModelType, string> Resolve = new Dictionary<StudyObservationalModelType, string>
        {
            { StudyObservationalModelType.UNKNOWN, "[unbekannt] " },
            { StudyObservationalModelType.COHORT, "Kohorte" },
            { StudyObservationalModelType.CASE_CONTROL, "Fall-Kontroll" },
            { StudyObservationalModelType.CASE_ONLY, "Einzelfall" },
            { StudyObservationalModelType.CASE_CROSSOVER, "Fall-Crossover" },
            { StudyObservationalModelType.ECOLOGIC, "Ökologische" },
            { StudyObservationalModelType.FAMILY_BASED, "Familienbasiert" },
            { StudyObservationalModelType.OTHER, "andere" },
        };
    }
}

