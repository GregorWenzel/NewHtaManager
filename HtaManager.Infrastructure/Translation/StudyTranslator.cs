using HtaManager.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Translation
{
    public static class StudyTranslator
    {
        public static Study Translate(Study study)
        {
            Study result = new Study();
            result.Id = study.Id;
            result.ActualPrimaryCompletionDate = study.ActualPrimaryCompletionDate;
            result.ActualStudyStartDate = study.ActualStudyStartDate;
            result.ConditionList = new List<Condition>();

            foreach (Condition condition in study.ConditionList)
            {
                Condition newCondition = new Condition(Translator.Translate(condition.Name));
                newCondition.Icd10Item = condition.Icd10Item;
                newCondition.Id = condition.Id;
                result.ConditionList.Add(newCondition);
            }

            result.Design = new StudyDesign();
            result.Design.FollowUpDuration = Translator.Translate(study.Design.FollowUpDuration);
            result.Design.MaskingDescription = Translator.Translate(study.Design.MaskingDescription);
            result.Design.ModelDescription = Translator.Translate(study.Design.ModelDescription);
            result.Design.InterventionModel = study.Design.InterventionModel;
            result.Design.ObservationalModelList = study.Design.ObservationalModelList;
            result.Design.TimePerspectiveList = study.Design.TimePerspectiveList;

            result.Design.EligibilityText = Translator.Translate(study.Design.EligibilityText);
            result.EndpointList = new List<OutcomeMeasure>();
            foreach (OutcomeMeasure measure in study.EndpointList)
            {
                OutcomeMeasure newMeasure = new OutcomeMeasure();
                newMeasure.Description = Translator.Translate(measure.Description);
                newMeasure.EndpointPriority = measure.EndpointPriority;
                newMeasure.EndpointDescriptor = measure.EndpointDescriptor;
                newMeasure.Id = measure.Id;
                newMeasure.Name = Translator.Translate(measure.Name);
                newMeasure.TimeFrame = Translator.Translate(measure.TimeFrame);
                result.EndpointList.Add(newMeasure);
            }

            result.FirstResultsSubmittedDate = study.FirstResultsSubmittedDate;
            result.FirstSubmittedDate = study.FirstResultsSubmittedDate;
            result.FullTitle = Translator.Translate(study.FullTitle);
            result.KeywordList = new List<string>();
            foreach (string keyWord in study.KeywordList)
            {
                result.KeywordList.Add(Translator.Translate(keyWord));
            }
            result.LastUpdatePostedDate = study.LastUpdatePostedDate;
            result.NctId = study.NctId;
            result.PublicationList = study.PublicationList;
            result.SecondaryIndentifierList = study.SecondaryIndentifierList;
            result.ShortTitle = Translator.Translate(study.ShortTitle);
            result.StudyArmList = new List<StudyArm>();
            foreach (StudyArm arm in study.StudyArmList)
            {
                StudyArm newArm = new StudyArm();
                newArm.Id = arm.Id;
                newArm.Description = Translator.Translate(arm.Description);
                newArm.InterventionDescription = Translator.Translate(arm.InterventionDescription);
                newArm.InterventionName = Translator.Translate(arm.InterventionName);
                newArm.Title = Translator.Translate(arm.Title);
                newArm.InterventionType = arm.InterventionType;
                newArm.Type = arm.Type;
                newArm.InterventionList = new List<Intervention>();
                foreach (Intervention intervention in arm.InterventionList)
                {
                    Intervention newIntervention = new Intervention();
                    newIntervention.Id = intervention.Id;
                    newIntervention.Description = Translator.Translate(intervention.Description);
                    newIntervention.Name = Translator.Translate(intervention.Name);
                    newIntervention.OtherNameList = intervention.OtherNameList;
                    newIntervention.Type = intervention.Type;
                    newIntervention.StudyArmNameList = intervention.StudyArmNameList;
                    newArm.InterventionList.Add(newIntervention);
                }
                result.StudyArmList.Add(newArm);
            }

            result.OriginalLanguageStudy = study;
            return result;
        }
    }
}
