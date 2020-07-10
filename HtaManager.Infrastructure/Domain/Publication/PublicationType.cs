using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public enum PublicationType
    {
        UNKNOWN,
        ABSTRACT,
        META_ANALYSIS,
        REVIEW,
        SINGLE_STUDY,
        SYSTEMATIC_REVIEW,
        SYSTEMATIC_REVIEW_MA
    }

    public static class PublicationTypeString
    {
        public static Dictionary<PublicationType, string> Resolve = new Dictionary<PublicationType, string>
        {
            { PublicationType.UNKNOWN, "unbekannt" },
            { PublicationType.ABSTRACT, "Abstract" },
            { PublicationType.META_ANALYSIS, "Meta-Analyse" },
            { PublicationType.REVIEW, "Review" },
            { PublicationType.SINGLE_STUDY, "Einzelstudie" },
            { PublicationType.SYSTEMATIC_REVIEW, "Systematischer Review" },
            { PublicationType.SYSTEMATIC_REVIEW_MA, "Systematischer Review und Meta-Analyse" },

        };
    }
}
