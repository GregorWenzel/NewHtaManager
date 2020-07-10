﻿using HtaManager.Infrastructure.Domain;
using HtaManager.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HtaManager.Repository
{
    public class ClinicalTrialsXmlParser : XmlParser
    {

        public ClinicalTrialsXmlParser()
        {
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

                armList = intervention.SelectNodes("./List[@Name='InterventionArmGroupLabelList']");
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

            result.StudyArmList = result.StudyArmList.OrderBy(item => item.Type).ToList();
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

                for (int i=0; i<observationalModelList.Count; i++)
                {
                    XmlNode modelNode = observationalModelList[i];
                    XmlNode timeperspectiveNode = observationalTimePerspectiveList[i];

                    result.Design.ObservationalModelList.Add(StudyObservationalModelParser.Parse(modelNode.InnerText));
                    result.Design.TimePerspectiveList.Add(StudyTimePerspectiveParser.Parse(timeperspectiveNode.InnerText));
                }

                result.Design.InterventionModel = StudyInterventionModelType.NONE;
            }
            else
            {
                result.Design.Allocation = StudyAllocationParser.Parse(ParseSingleNode(designModule, "./Struct[@Name='DesignInfo']/Field[@Name='DesignAllocation']"));
                result.Design.InterventionModel = StudyInterventionModelTypeParser.Parse(ParseSingleNode(designModule, "./Struct[@Name='DesignInfo']/Field[@Name='DesignInterventionModel']"));
            }

            XmlNodeList phaseList = designModule.SelectNodes("./List[@Name='PhaseList']/Field");
            foreach (XmlNode phase in phaseList)
            {
                result.Design.PhaseList.Add(StudyPhaseParser.Parse(phase.InnerText));
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

            XmlNodeList outcomeList = outcomesModule.SelectNodes("./List[@Name='PrimaryOutcomeList']/Struct");
            foreach (XmlNode outcome in outcomeList)
            {
                OutcomeMeasure newEndpoint = new OutcomeMeasure();
                newEndpoint.EndpointPriority = OutcomePriorityType.PRIMARY;
                newEndpoint.Description = ParseSingleNode(outcome, "./Field[@Name='PrimaryOutcomeDescription']");
                newEndpoint.TimeFrame = ParseSingleNode(outcome, "./Field[@Name='PrimaryOutcomeTimeFrame']");
                newEndpoint.Name = ParseSingleNode(outcome, "./Field[@Name='PrimaryOutcomeMeasure']");
                newEndpoint.Study = result;
                //newEndpoint.EndpointType = new EndpointTypeParser().Parse(newEndpoint);
            }

            outcomeList = outcomesModule.SelectNodes("./List[@Name='SecondaryOutcomeList']/Struct");
            foreach (XmlNode outcome in outcomeList)
            {
                OutcomeMeasure newEndpoint = new OutcomeMeasure();
                newEndpoint.EndpointPriority = OutcomePriorityType.PRIMARY;
                newEndpoint.Description = ParseSingleNode(outcome, "./Field[@Name='SecondaryOutcomeDescription']");
                newEndpoint.TimeFrame = ParseSingleNode(outcome, "./Field[@Name='SecondaryOutcomeTimeFrame']");
                newEndpoint.Name = ParseSingleNode(outcome, "./Field[@Name='SecondaryOutcomeMeasure']");
                newEndpoint.Study = result;
                // newEndpoint.EndpointType = container.Resolve<IGlobals>(). new EndpointTypeParser().Parse(newEndpoint);
            }
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
