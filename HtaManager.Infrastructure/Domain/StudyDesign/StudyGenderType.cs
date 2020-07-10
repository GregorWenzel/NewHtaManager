using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum StudyGenderType
    {
        ALL,
        FEMALE,
        MALE,
        OTHER,
        UNKNOWN
    }

    public static class GenderParser
    {
        public static StudyGenderType Parse(string valueString)
        {
            switch (valueString)
            {
                case "All":
                    return StudyGenderType.ALL;
                case "Female":
                    return StudyGenderType.FEMALE;
                case "Male":
                    return StudyGenderType.MALE;
                case "Other":
                    return StudyGenderType.OTHER;
                default:
                    return StudyGenderType.UNKNOWN;
            }
        }
    }

    public static class GenderTypeString
    {
        public static Dictionary<StudyGenderType, string> Resolve = new Dictionary<StudyGenderType, string>
        {
            { StudyGenderType.ALL, "keine Einschränkung" },
            { StudyGenderType.FEMALE, "Frauen" },
            { StudyGenderType.MALE, "Männer" },
            { StudyGenderType.OTHER, "andere" },
            { StudyGenderType.UNKNOWN, "unbekannt" },
        };
    }
}
