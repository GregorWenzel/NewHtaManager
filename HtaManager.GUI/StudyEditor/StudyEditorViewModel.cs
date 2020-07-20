using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using HtaManager.Repository;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HtaManager.GUI.StudyEditor
{
    public class StudyEditorViewModel: ViewModelBase
    {
        IUnityContainer container;
        IRegionManager regionManager;

        public Prism.Commands.DelegateCommand LoadNctCommand
        {
            get => new Prism.Commands.DelegateCommand(OnLoadNct);
        }

        public Prism.Commands.DelegateCommand SetInterventionalStudyCommand
        {
            get => new Prism.Commands.DelegateCommand(OnSetInterventionalStudy);
        }

        public Prism.Commands.DelegateCommand SetObservationalStudyCommand
        {
            get => new Prism.Commands.DelegateCommand(OnSetObservationalStudy);
        }

        public Prism.Commands.DelegateCommand BackToModelSelectionCommand
        {
            get => new Prism.Commands.DelegateCommand(OnBackToModelSelection);
        }
        
        private StudyViewModel selectedStudy;
        public StudyViewModel SelectedStudy
        {
            get => selectedStudy;
            set
            {
                SetProperty(ref selectedStudy, value);
                allocation = new KeyValuePair<StudyAllocationType, string>(SelectedStudy.Design.Allocation, StudyAllocationTypeString.Resolve[SelectedStudy.Design.Allocation]);
                purpose = new KeyValuePair<StudyPurposeType, string>(SelectedStudy.Design.Purpose, StudyPurposeTypeString.Resolve[SelectedStudy.Design.Purpose]);

                RaisePropertyChanged("Allocation");
                RaisePropertyChanged("Purpose");
                RaisePropertyChanged("AreOutcomeAssessorsBlinded");
                RaisePropertyChanged("AreCareProvidersBlinded");
                RaisePropertyChanged("AreInvestigatorsBlinded");
                RaisePropertyChanged("ArePatientsBlinded");
            }
        }

        private object allocation;
        public object Allocation
        {
            get => allocation;
            set
            {
                SetProperty(ref allocation, value);
                SelectedStudy.Design.Allocation = ((KeyValuePair<StudyAllocationType, string>)allocation).Key;
            }
        }

        private object purpose;
        public object Purpose
        {
            get => purpose;
            set
            {
                SetProperty(ref purpose, value);
                SelectedStudy.Design.Purpose = ((KeyValuePair<StudyPurposeType, string>)purpose).Key;
            }
        }

        private object timePerspective;
        public object TimePerspective
        {
            get => timePerspective;
            set
            {
                SetProperty(ref timePerspective, value);
                SelectedStudy.Design.TimePerspective = ((KeyValuePair<StudyTimePerspectiveType, string>)timePerspective).Key;
            }
        }

        private object observationalModel;
        public object ObservationalModel
        {
            get => observationalModel;
            set
            {
                SetProperty(ref observationalModel, value);
                SelectedStudy.Design.ObservationalModel = ((KeyValuePair<StudyObservationalModelType, string>)observationalModel).Key;
            }
        }

        public bool ArePatientsBlinded
        {
            get => SelectedStudy.Design.MaskedPersonList.Contains(StudyMaskedPersonType.PARTICIPANT);
            set
            {
                if (value == true)
                {
                    if (!SelectedStudy.Design.MaskedPersonList.Contains(StudyMaskedPersonType.PARTICIPANT))
                    {
                        SelectedStudy.Design.MaskedPersonList.Add(StudyMaskedPersonType.PARTICIPANT);
                    }
                }
                else
                {
                    SelectedStudy.Design.MaskedPersonList.Remove(StudyMaskedPersonType.PARTICIPANT);
                }
            }
        }

        public bool AreInvestigatorsBlinded
        {
            get => SelectedStudy.Design.MaskedPersonList.Contains(StudyMaskedPersonType.INVESTIGATOR);
            set
            {
                if (value == true)
                {
                    if (!SelectedStudy.Design.MaskedPersonList.Contains(StudyMaskedPersonType.INVESTIGATOR))
                    {
                        SelectedStudy.Design.MaskedPersonList.Add(StudyMaskedPersonType.INVESTIGATOR);
                    }
                }
                else
                {
                    SelectedStudy.Design.MaskedPersonList.Remove(StudyMaskedPersonType.INVESTIGATOR);
                }
            }
        }

        public bool AreCareProvidersBlinded
        {
            get => SelectedStudy.Design.MaskedPersonList.Contains(StudyMaskedPersonType.CARE_PROVIDER);
            set
            {
                if (value == true)
                {
                    if (!SelectedStudy.Design.MaskedPersonList.Contains(StudyMaskedPersonType.CARE_PROVIDER))
                    {
                        SelectedStudy.Design.MaskedPersonList.Add(StudyMaskedPersonType.CARE_PROVIDER);
                    }
                }
                else
                {
                    SelectedStudy.Design.MaskedPersonList.Remove(StudyMaskedPersonType.CARE_PROVIDER);
                }
            }
        }

        public bool AreOutcomeAssessorsBlinded
        {
            get => SelectedStudy.Design.MaskedPersonList.Contains(StudyMaskedPersonType.OUTCOMES_ASSESSOR);
            set
            {
                if (value == true)
                {
                    if (!SelectedStudy.Design.MaskedPersonList.Contains(StudyMaskedPersonType.OUTCOMES_ASSESSOR))
                    {
                        SelectedStudy.Design.MaskedPersonList.Add(StudyMaskedPersonType.OUTCOMES_ASSESSOR);
                    }
                }
                else
                {
                    SelectedStudy.Design.MaskedPersonList.Remove(StudyMaskedPersonType.OUTCOMES_ASSESSOR);
                }
            }
        }

        private bool isBlindingUnknown;
        public bool IsBlindingUnknown
        {
            get => isBlindingUnknown;
            set
            {
                SelectedStudy.Design.MaskedPersonList.Remove(StudyMaskedPersonType.OUTCOMES_ASSESSOR);
                SelectedStudy.Design.MaskedPersonList.Remove(StudyMaskedPersonType.CARE_PROVIDER);
                SelectedStudy.Design.MaskedPersonList.Remove(StudyMaskedPersonType.INVESTIGATOR);
                SelectedStudy.Design.MaskedPersonList.Remove(StudyMaskedPersonType.PARTICIPANT);

                RaisePropertyChanged("AreOutcomeAssessorsBlinded");
                RaisePropertyChanged("AreCareProvidersBlinded");
                RaisePropertyChanged("AreInvestigatorsBlinded");
                RaisePropertyChanged("ArePatientsBlinded");

                SetProperty(ref isBlindingUnknown, value);
            }
        }

        public StudyEditorViewModel(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;

            SelectedStudy = new StudyViewModel();
            regionManager.RegisterViewWithRegion(RegionNames.StudyDesignRegion, typeof(ModelSelectionView));
        }

        private void OnLoadNct()
        {
            SelectedStudy = new StudyViewModel(container.Resolve<IRegistryRepository>("ClinicalTrials").RequestStudy(SelectedStudy.NctId));
        }

        private void OnSetInterventionalStudy()
        {
            regionManager.RequestNavigate(RegionNames.StudyDesignRegion, "InterventionalStudyEditorView");
        }

        private void OnSetObservationalStudy()
        {
            regionManager.RequestNavigate(RegionNames.StudyDesignRegion, "ObservationalStudyEditorView");
        }


        private void OnBackToModelSelection()
        {
            regionManager.RequestNavigate(RegionNames.StudyDesignRegion, "ModelSelectionView");
        }

        

    }
}
