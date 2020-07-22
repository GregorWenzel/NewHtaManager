using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum StudyPhaseType
    {
        UNKNOWN,
        NONE,
        EARLY_PHASE_I,
        PHASE_I,
        PHASE_I_II,
        PHASE_II,
        PHASE_II_III,
        PHASE_III,
        PHASE_IV
    }

    public static class StudyPhaseParser
    {
        public static StudyPhaseType Parse(string valueString)
        {
            switch (valueString)
            {
                case "Not Applicable":
                case "N/A":
                    return StudyPhaseType.NONE;
                case "Phase 1":
                    return StudyPhaseType.PHASE_I;
                case "Phase 1/Phase 2":
                    return StudyPhaseType.PHASE_I_II;
                case "Phase 2":
                    return StudyPhaseType.PHASE_II;
                case "Phase 2/Phase 3":
                    return StudyPhaseType.PHASE_II_III;
                case "Phase 3":
                    return StudyPhaseType.PHASE_III;
                case "Phase 4":
                    return StudyPhaseType.PHASE_IV;
                default:
                    return StudyPhaseType.UNKNOWN;
            }
        }
    }

    public static class StudyPhaseTypeString
    {
        public static Dictionary<StudyPhaseType, string> Resolve = new Dictionary<StudyPhaseType, string>
        {
            { StudyPhaseType.UNKNOWN, "unbekannt" },
            { StudyPhaseType.NONE, "keine" },
            { StudyPhaseType.EARLY_PHASE_I, "frühe Phase I" },
            { StudyPhaseType.PHASE_I, "Phase I" },
            { StudyPhaseType.PHASE_I_II, "Phase I/II" },
            { StudyPhaseType.PHASE_II, "Phase II" },
            { StudyPhaseType.PHASE_II_III, "Phase II/III" },
            { StudyPhaseType.PHASE_III, "Phase III" },
            { StudyPhaseType.PHASE_IV, "Phase IV" }
        };
    }
}