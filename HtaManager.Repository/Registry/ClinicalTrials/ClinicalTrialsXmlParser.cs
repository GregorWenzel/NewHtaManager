using HtaManager.Infrastructure.Domain;
using HtaManager.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace HtaManager.Repository
{
    public class ClinicalTrialsXmlParser : XmlParser
    {
        List<EndpointDescriptor> endpointDescriptorList;

        public ClinicalTrialsXmlParser(List<EndpointDescriptor> endpointDescriptorList)
        {
            this.endpointDescriptorList = endpointDescriptorList;
        }

        public Study Parse(XmlDocument doc)
        {
            this.doc = doc;

            Study result = new Study();
            ParseIdentificationModule(ref result);
            ParseStatusModule(ref result);
            ParseSponsorModule(ref result);
            ParseDescriptionModule(ref result);
            ParseConditionsModule(ref result);
            ParseDesignModule(ref result);
            ParseEligibilityModule(ref result);
            ParseArmsInterventionsModule(ref result);
            ParseOutocmesModule(ref result);

            return result;
        }

        private void ParseArmsInterventionsModule(ref Study result)
        {
            XmlNode armsInterventionModule = doc.SelectSingleNode("/FullStudiesResponse/FullStudyList/FullStudy/Struct/Struct[@Name='ProtocolSection']/Struct[@Name='ArmsInterventionsModule']");
            XmlNodeList armList = armsInterventionModule.SelectNodes("./List[@Name='ArmGroupList']/Struct");

            foreach (XmlNode arm in armList)
            {
                StudyArm newArm = new StudyArm();
                newArm.Title = ParseSingleNode(arm, "./Field[@Name='ArmGroupLabel']");
                newArm.Description = ParseSingleNode(arm, "./Field[@Name='ArmGroupDescription']");
                newArm.Type = StudyArmParser.Parse(ParseSingleNode(arm, "./Field[@Name='ArmGroupType']"));

                result.StudyArmList.Add(newArm);
            }

            XmlNodeList interventionList = armsInterventionModule.SelectNodes("./List[@Name='InterventionList']/Struct");

            foreach (XmlNode intervention in interventionList)
            {
                Intervention newIntervention = new Intervention();
                newIntervention.Name = ParseSingleNode(intervention, "./Field[@Name='InterventionName']");
                newIntervention.Description = ParseSingleNode(intervention, "./Field[@Name='InterventionDescription']");
                newIntervention.Type = InterventionParser.Parse(ParseSingleNode(intervention, "./Field[@Name='InterventionType']"));

                armList = intervention.SelectNodes("./List[@Name='InterventionArmGroupLabelList']/Field");
                foreach (XmlNode armNode in armList)
                {
                    if (result.StudyArmList.Any(item => item.Title == armNode.InnerText))
                    {
                        StudyArm thisArm = result.StudyArmList.First(item => item.Title == armNode.InnerText);
                        newIntervention.StudyArmNameList.Add(thisArm.Title);
                        thisArm.InterventionList.Add(newIntervention);
                    }
                }
            }

            result.StudyArmList = result.StudyArmList.OrderBy(item => item.Title).ToList();
        }

        private void ParseConditionsModule(ref Study result)
        {
            XmlNode conditionsModule = doc.SelectSingleNode("/FullStudiesResponse/FullStudyList/FullStudy/Struct/Struct[@Name='ProtocolSection']/Struct[@Name='ConditionsModule']");
            XmlNodeList conditionList = conditionsModule.SelectNodes("./List[@Name='ConditionList']/Field");

            foreach (XmlNode condition in conditionList)
            {
                Condition newCondition = new Condition(condition.InnerText);

                result.ConditionList.Add(newCondition);
            }

            XmlNodeList keywordList = conditionsModule.SelectNodes("./List[@Name='KeywordList']/Field");

            foreach (XmlNode keyword in keywordList)
            {
                result.KeywordList.Add(keyword.InnerText);
            }
        }

        private void ParseDescriptionModule(ref Study result)
        {
            XmlNode descriptionModule = doc.SelectSingleNode("/FullStudiesResponse/FullStudyList/FullStudy/Struct/Struct[@Name='ProtocolSection']/Struct[@Name='DescriptionModule']");
            result.BriefDescription = ParseSingleNode(descriptionModule, "./Field[@Name='BriefSummary']");
            result.DetailedDescription = ParseSingleNode(descriptionModule, "./Field[@Name='DetailedDescription']");
        }

        private void ParseDesignModule(ref Study result)
        {
            XmlNode designModule = doc.SelectSingleNode("/FullStudiesResponse/FullStudyList/FullStudy/Struct/Struct[@Name='ProtocolSection']/Struct[@Name='DesignModule']");
            result.Design.Type = ParseSingleNode(designModule, "./Field[@Name='StudyType']");

            if (result.Design.Type == "Observational")
            {
                XmlNodeList observationalModelList = designModule.SelectNodes("./Struct[@Name='DesignInfo']/List[@Name='DesignObservationalModelList']/Field");
                XmlNodeList observationalTimePerspectiveList = designModule.SelectNodes("./Struct[@Name='DesignInfo']/List[@Name='DesignTimePerspectiveList']/Field");

                StudyObservationalModelType[] modelTypeArr = new StudyObservationalModelType[observationalModelList.Count];
                for (int i = 0; i < observationalModelList.Count; i++)
                {
                    XmlNode modelNode = observationalModelList[i];
                    modelTypeArr[i] = StudyObservationalModelParser.Parse(modelNode.InnerText);
                }
                result.Design.ObservationalModel = modelTypeArr.FirstOrDefault();

                StudyTimePerspectiveType[] timeArr = new StudyTimePerspectiveType[observationalTimePerspectiveList.Count];
                for (int i = 0; i < observationalTimePerspectiveList.Count; i++)
                {
                    XmlNode timeperspectiveNode = observationalTimePerspectiveList[i];

                    timeArr[i] = StudyTimePerspectiveParser.Parse(timeperspectiveNode.InnerText);
                }
                result.Design.TimePerspective = timeArr.FirstOrDefault();

                result.Design.InterventionModel = StudyInterventionModelType.NONE;
            }
            else
            {
                result.Design.Allocation = StudyAllocationParser.Parse(ParseSingleNode(designModule, "./Struct[@Name='DesignInfo']/Field[@Name='DesignAllocation']"));
                result.Design.InterventionModel = StudyInterventionModelTypeParser.Parse(ParseSingleNode(designModule, "./Struct[@Name='DesignInfo']/Field[@Name='DesignInterventionModel']"));

                XmlNodeList phaseList = designModule.SelectNodes("./List[@Name='PhaseList']/Field");
                if (phaseList.Count == 0)
                {
                    result.Design.Phase = StudyPhaseType.NONE;
                }
                else if (phaseList.Count == 1)
                {
                    result.Design.Phase = StudyPhaseParser.Parse(phaseList[0].InnerText);
                }
                else
                {
                    StudyPhaseType[] phaseArr = new StudyPhaseType[phaseList.Count];
                    for (int i = 0; i < phaseArr.Length; i++)
                    {
                        XmlNode phase = phaseList[i];
                        phaseArr[i] = StudyPhaseParser.Parse(phase.InnerText);
                    }


                    if (phaseArr.Contains(StudyPhaseType.PHASE_II) && phaseArr.Contains(StudyPhaseType.PHASE_III))
                    {
                        result.Design.Phase = StudyPhaseType.PHASE_II_III;
                    }
                    else if (phaseArr.Contains(StudyPhaseType.PHASE_I) && phaseArr.Contains(StudyPhaseType.PHASE_II))
                    {
                        result.Design.Phase = StudyPhaseType.PHASE_I_II;
                    }
                    else
                    {
                        result.Design.Phase = phaseArr.Max();
                    }
                }
            }

            result.Design.DurationPlanned = ParseSingleNode(designModule, "./Field[@Name='TargetDuration']");            
            result.Design.Purpose = StudyPurposeParser.Parse(ParseSingleNode(designModule, "./Struct[@Name='DesignInfo']/Field[@Name='DesignPrimaryPurpose']"));

            int enrollment = 0;
            Int32.TryParse(ParseSingleNode(designModule, "./Struct[@Name='EnrollmentInfo']/Field[@Name='EnrollmentCount']"), out enrollment);
            result.Design.Enrollment = enrollment;

            XmlNodeList maskingList = designModule.SelectNodes("./Struct[@Name='DesignInfo']/Struct[@Name='DesignMaskingInfo']/List[@Name='DesignWhoMaskedList']/Field");
            foreach (XmlNode masking in maskingList)
            {
                result.Design.MaskedPersonList.Add(MaskedPersonParser.Parse(masking.InnerText));
            }
        }

        private void ParseEligibilityModule(ref Study result)
        {
            XmlNode eligibilityModule = doc.SelectSingleNode("/FullStudiesResponse/FullStudyList/FullStudy/Struct/Struct[@Name='ProtocolSection']/Struct[@Name='EligibilityModule']");
            result.Design.EligibilityText = ParseSingleNode(eligibilityModule, "./Field[@Name='EligibilityCriteria']");
            result.Design.GenderType = GenderParser.Parse(ParseSingleNode(eligibilityModule, "./Field[@Name='Gender']"));
            result.Design.HasHealthyPatients = ParseSingleNode(eligibilityModule, "./Field[@Name='HealthyVolunteers']") == "Yes";
            result.Design.MinAge = ParseSingleNode(eligibilityModule, "./Field[@Name='MinimumAge']");
            result.Design.MaxAge = ParseSingleNode(eligibilityModule, "./Field[@Name='MaximunAge']");
        }

        private void ParseIdentificationModule(ref Study result)
        {
            XmlNode identificationModule = doc.SelectSingleNode("/FullStudiesResponse/FullStudyList/FullStudy/Struct/Struct[@Name='ProtocolSection']/Struct[@Name='IdentificationModule']");
            result.NctId = ParseSingleNode(identificationModule, "./Field[@Name='NCTId']");
            result.ShortTitle = ParseSingleNode(identificationModule, "./Field[@Name='BriefTitle']");
            result.FullTitle = ParseSingleNode(identificationModule, "./Field[@Name='OfficialTitle']");

            string acronym = ParseSingleNode(identificationModule, "./Field[@Name='Acronym']");
            if (!string.IsNullOrEmpty(acronym))
            {
                result.SecondaryIndentifierList.Add(acronym);
            }

            string otherIdentifier = ParseSingleNode(identificationModule, "./Struct[@Name='OrgStudyIdInfo']/Field");
            if (!string.IsNullOrEmpty(otherIdentifier))
            {
                result.SecondaryIndentifierList.Add(otherIdentifier);
            }
        }

        private void ParseOutocmesModule(ref Study result)
        {
            XmlNode outcomesModule = doc.SelectSingleNode("/FullStudiesResponse/FullStudyList/FullStudy/Struct/Struct[@Name='ProtocolSection']/Struct[@Name='OutcomesModule']");

            if (outcomesModule == null) return;

            XmlNodeList outcomeList = outcomesModule.SelectNodes("./List[@Name='PrimaryOutcomeList']/Struct");
            foreach (XmlNode outcome in outcomeList)
            {
                OutcomeMeasure newEndpoint = new OutcomeMeasure();
                newEndpoint.EndpointPriority = OutcomePriorityType.PRIMARY;
                newEndpoint.Description = ParseSingleNode(outcome, "./Field[@Name='PrimaryOutcomeDescription']");
                newEndpoint.TimeFrame = ParseSingleNode(outcome, "./Field[@Name='PrimaryOutcomeTimeFrame']");
                newEndpoint.Name = ParseSingleNode(outcome, "./Field[@Name='PrimaryOutcomeMeasure']");
                newEndpoint.Study = result;
                newEndpoint.EndpointDescriptor = GetEndpointDescriptor(newEndpoint);

                result.EndpointList.Add(newEndpoint);
            }

            outcomeList = outcomesModule.SelectNodes("./List[@Name='SecondaryOutcomeList']/Struct");
            foreach (XmlNode outcome in outcomeList)
            {
                OutcomeMeasure newEndpoint = new OutcomeMeasure();
                newEndpoint.EndpointPriority = OutcomePriorityType.SECONDARY;
                newEndpoint.Description = ParseSingleNode(outcome, "./Field[@Name='SecondaryOutcomeDescription']");
                newEndpoint.TimeFrame = ParseSingleNode(outcome, "./Field[@Name='SecondaryOutcomeTimeFrame']");
                newEndpoint.Name = ParseSingleNode(outcome, "./Field[@Name='SecondaryOutcomeMeasure']");
                newEndpoint.Study = result;
                newEndpoint.EndpointDescriptor = GetEndpointDescriptor(newEndpoint);

                result.EndpointList.Add(newEndpoint);
            }
        }

        private EndpointDescriptor GetEndpointDescriptor(OutcomeMeasure outcome)
        {
            StringBuilder sb = new StringBuilder(outcome.Name.ToLower());
            sb.Replace("diagnosis of a", "");
            sb.Replace("diangosis of", "");
            sb.Replace("diagnosis of the", "");
            string cleanedOutcomeName = Regex.Replace(sb.ToString(), @" ?\(.*?\)", string.Empty);

            string[] forbiddenWordArr = new string[] { "during", "within" };

            foreach (string forbiddenWord in forbiddenWordArr)
            {
                int redundantIndex = cleanedOutcomeName.IndexOf(forbiddenWord);
                if (redundantIndex > 0)
                {
                    cleanedOutcomeName = cleanedOutcomeName.Substring(0, redundantIndex);
                }
            }
                       
            cleanedOutcomeName = cleanedOutcomeName.Trim();

            var result = endpointDescriptorList.FirstOrDefault(item => item.NameEN.ToLower() == outcome.Name.ToLower() || item.ChildList.Any(child => child.NameEN.ToLower() == outcome.Name.ToLower()));
            return null;
        }

        private void ParseSponsorModule(ref Study result)
        {
            XmlNode sponsorModule = doc.SelectSingleNode("/FullStudiesResponse/FullStudyList/FullStudy/Struct/Struct[@Name='ProtocolSection']/Struct[@Name='SponsorCollaboratorsModule']");
        }

        private void ParseStatusModule(ref Study result)
        {
            XmlNode statusModule = doc.SelectSingleNode("/FullStudiesResponse/FullStudyList/FullStudy/Struct/Struct[@Name='ProtocolSection']/Struct[@Name='StatusModule']");
            result.ActualPrimaryCompletionDate = ParseDateTimeNode(statusModule, "./Struct[@Name='PrimaryCompletionDateStruct']/Field[@Name='PrimaryCompletionDate']");
            result.ActualStudyStartDate = ParseDateTimeNode(statusModule, "./Struct[@Name='StartDateStruct']/Field[@Name='StartDate']");
            result.LastUpdatePostedDate = ParseDateTimeNode(statusModule, "./Struct[@Name='LastUpdatePostDateStruct']/Field[@Name='LastUpdatePostDate']");
            //result.FirstResultsSubmittedDate = ParseDateTimeNode(statusModule, "./Struct[@Name='LastUpdatePostDateStruct']/Field[@Name='LastUpdatePostDate']");
            result.FirstSubmittedDate = ParseDateTimeNode(statusModule, "./Struct[@Name='StudyFirstPostDateStruct']/Field[@Name='StudyFirstPostDate']");
        }
    }
}
