using HtaManager.GUI.StudyArmEditor;
using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using HtaManager.Repository;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Unity;

namespace HtaManager.GUI.StudyEditor
{
    public class StudyEditorViewModel: Infrastructure.Mvvm.ViewModelBase
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

        public Prism.Commands.DelegateCommand<MouseButtonEventArgs> MouseDoubleClickCommand
        {
            get => new Prism.Commands.DelegateCommand<MouseButtonEventArgs>(OnMouseDoubleClicked);        
        }
        public Prism.Commands.DelegateCommand AddStudyArm
        {
            get => new Prism.Commands.DelegateCommand(OnAddStudyArm);
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

        private object interventionalModel;
        public object InterventionalModel
        {
            get => interventionalModel;
            set
            {
                SetProperty(ref interventionalModel, value);
                SelectedStudy.Design.InterventionModel = ((KeyValuePair<StudyInterventionModelType, string>)interventionalModel).Key;
            }
        }

        private object studyPhase;
        public object StudyPhase
        {
            get => studyPhase;
            set
            {
                SetProperty(ref studyPhase, value);
                SelectedStudy.Design.Phase = ((KeyValuePair<StudyPhaseType, string>)studyPhase).Key;
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
            if (!regionManager.Regions.ContainsRegionWithName(RegionNames.StudyDesignRegion))
            {
                regionManager.RegisterViewWithRegion(RegionNames.StudyDesignRegion, typeof(ModelSelectionView));
            }
        }

        private void OnLoadNct()
        {
            SelectedStudy = new StudyViewModel(container.Resolve<IRegistryRepository>("ClinicalTrials").RequestStudy(SelectedStudy.NctId));
            Purpose = new KeyValuePair<StudyPurposeType, string>(SelectedStudy.Design.Purpose, StudyPurposeTypeString.Resolve[SelectedStudy.Design.Purpose]);

            if (SelectedStudy.Design.Type == "Observational")
            {
                regionManager.RequestNavigate(RegionNames.StudyDesignRegion, "ObservationalStudyEditorView");
                ObservationalModel = new KeyValuePair<StudyObservationalModelType, string>(SelectedStudy.Design.ObservationalModel, StudyObservationalModelTypeString.Resolve[SelectedStudy.Design.ObservationalModel]);
                TimePerspective = new KeyValuePair<StudyTimePerspectiveType, string>(SelectedStudy.Design.TimePerspective, StudyTimePerspectiveTypeString.Resolve[SelectedStudy.Design.TimePerspective]);
            }
            else
            {
                regionManager.RequestNavigate(RegionNames.StudyDesignRegion, "InterventionalStudyEditorView");
                InterventionalModel = new KeyValuePair<StudyInterventionModelType, string>(SelectedStudy.Design.InterventionModel, StudyInterventionModelTypeString.Resolve[SelectedStudy.Design.InterventionModel]);
                Allocation = new KeyValuePair<StudyAllocationType, string>(SelectedStudy.Design.Allocation, StudyAllocationTypeString.Resolve[SelectedStudy.Design.Allocation]);
                StudyPhase = new KeyValuePair<StudyPhaseType, string>(SelectedStudy.Design.Phase, StudyPhaseTypeString.Resolve[SelectedStudy.Design.Phase]);
            }
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

        private void OnMouseDoubleClicked(MouseButtonEventArgs obj)
        {
            StudyArmViewModel studyArm = (obj.Source as RadGridView).SelectedItem as StudyArmViewModel;

            OpenStudyArmEditor(studyArm);
        }

        private bool? OpenStudyArmEditor(StudyArmViewModel studyArm)
        { 
            RadWindow studyArmEditorWindow = new RadWindow();
            studyArmEditorWindow.Header = "Studienarm-Editor";

            StudyArmEditorView studyArmEditorView = new StudyArmEditorView();
            
            StudyArmEditorViewModel studyArmEditorViewModel = new StudyArmEditorViewModel();
            studyArmEditorViewModel.SelectedStudyArm = studyArm;

            studyArmEditorView.DataContext = studyArmEditorViewModel;

            studyArmEditorWindow.Content = studyArmEditorView;

            return studyArmEditorWindow.ShowDialog();
        }

        private void OnAddStudyArm()
        {
            StudyArmViewModel newStudyArm = new StudyArmViewModel();

            if (OpenStudyArmEditor(newStudyArm) == true)
            {
                SelectedStudy.StudyArmList.Add(newStudyArm);
                newStudyArm.Update();
            }
        }
    }
}
