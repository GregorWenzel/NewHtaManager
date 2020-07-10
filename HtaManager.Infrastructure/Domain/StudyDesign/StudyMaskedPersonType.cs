using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum StudyMaskedPersonType
    {
        PARTICIPANT,
        CARE_PROVIDER,
        INVESTIGATOR,
        OUTCOMES_ASSESSOR,
        UNKNOWN
    }

    public static class MaskedPersonParser
    {
        public static StudyMaskedPersonType Parse(string valueString)
        {
            switch (valueString)
            {
                case "Participant":
                    return StudyMaskedPersonType.PARTICIPANT;
                case "Care Provider":
                    return StudyMaskedPersonType.CARE_PROVIDER;
                case "Investigator":
                    return StudyMaskedPersonType.INVESTIGATOR;
                case "Outcomes Assessor":
                    return StudyMaskedPersonType.OUTCOMES_ASSESSOR;
                default:
                    return StudyMaskedPersonType.UNKNOWN;
            }
        }
    }

    public static class StudyMaskedPersonTypeString
    {
        public static Dictionary<StudyMaskedPersonType, string> Resolve = new Dictionary<StudyMaskedPersonType, string>
        {
            { StudyMaskedPersonType.UNKNOWN, "unbekannt" },
            { StudyMaskedPersonType.PARTICIPANT, "Patienten" },
            { StudyMaskedPersonType.CARE_PROVIDER, "Pflegekräfte" },
            { StudyMaskedPersonType.INVESTIGATOR, "Prüfärzte" },
            { StudyMaskedPersonType.OUTCOMES_ASSESSOR, "Ergebnisbewerter" },
        };
    }
}
