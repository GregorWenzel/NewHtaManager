using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum InterventionType
    {
        DRUG,
        DEVICE,
        PROCEDURE,
        RADIATION,
        BEHAVIORAL,
        GENETIC,
        DIETARY,
        DIAGNOSTIC_TEST,
        BIOLOGICAL,
        OTHER,
        UNKNOWN
    }

    public static class InterventionParser
    {
        public static InterventionType Parse(string valueString)
        {
            switch (valueString)
            {
                case "Drug":
                    return InterventionType.DRUG;
                case "Device":
                    return InterventionType.DEVICE;
                case "Procedure":
                    return InterventionType.PROCEDURE;
                case "Radiation":
                    return InterventionType.RADIATION;
                case "Behavioral":
                    return InterventionType.BEHAVIORAL;
                case "Genetic":
                    return InterventionType.GENETIC;
                case "Dietary":
                    return InterventionType.DIETARY;
                case "Diagnostic Test":
                    return InterventionType.DIAGNOSTIC_TEST;
                case "Biological":
                    return InterventionType.BIOLOGICAL;
                case "Other":
                    return InterventionType.OTHER;
                default:
                    return InterventionType.UNKNOWN;
            }
        }
    }

    public static class InterventionTypeString
    {
        public static Dictionary<InterventionType, string> Resolve = new Dictionary<InterventionType, string>
        {
            { InterventionType.DRUG, "Arzneimittel" },
            { InterventionType.DEVICE, "Gerät" },
            { InterventionType.PROCEDURE, "Eingriff" },
            { InterventionType.RADIATION, "Bestrahlung" },
            { InterventionType.BEHAVIORAL, "Verhalten" },
            { InterventionType.GENETIC, "Genetisch" },
            { InterventionType.DIETARY, "Dieätetisch" },
            { InterventionType.DIAGNOSTIC_TEST, "Diagnostischer Test" },
            { InterventionType.BIOLOGICAL, "Biologikum" },
            { InterventionType.OTHER, "Andere" },
            { InterventionType.UNKNOWN, "unbekannt" },
        };
    }
}
