using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum StudyInterventionModelType
    {
        NONE,
        SINGLE_GROUP,
        PARALLEL,
        CROSSOVER,
        FACTORIAL,
        SEQUENTIAL,
        UNKNOWN
    }

    public static class StudyInterventionModelTypeParser
    {
        public static StudyInterventionModelType Parse(string valueString)
        {
            switch (valueString)
            {
                case "Single Group Assignment":
                    return StudyInterventionModelType.SINGLE_GROUP;
                case "Parallel Assignment":
                    return StudyInterventionModelType.PARALLEL;
                case "Crossover Assignment":
                    return StudyInterventionModelType.CROSSOVER;
                case "Factorial Assignment":
                    return StudyInterventionModelType.FACTORIAL;
                case "Sequential Assignment":
                    return StudyInterventionModelType.SEQUENTIAL;
                default:
                    return StudyInterventionModelType.UNKNOWN;
            }
        }
    }

    public static class StudyInterventionModelTypeString
    {
        public static Dictionary<StudyInterventionModelType, string> Resolve = new Dictionary<StudyInterventionModelType, string>
        {
            { StudyInterventionModelType.UNKNOWN, "unbekannt" },
            { StudyInterventionModelType.SINGLE_GROUP, "Einzelgruppe" },
            { StudyInterventionModelType.PARALLEL, "Parallelgruppen" },
            { StudyInterventionModelType.CROSSOVER, "Crossover" },
            { StudyInterventionModelType.FACTORIAL, "faktoriell" },
            { StudyInterventionModelType.SEQUENTIAL, "sequenzielle" }
        };
    }
}