using HtaManager.GUI.EndpointEditor;
using HtaManager.GUI.StudyArmEditor;
using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using HtaManager.Repository;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Unity;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace HtaManager.GUI.StudyEditor
{
    public class StudyEditorViewModel: Infrastructure.Mvvm.ViewModelBase
    {
        IUnityContainer container;
        IRegionManager regionManager;

        public DelegateCommand LoadNctCommand
        {
            get => new DelegateCommand(OnLoadNct);
        }

        public DelegateCommand SetInterventionalStudyCommand
        {
            get => new DelegateCommand(OnSetInterventionalStudy);
        }

        public DelegateCommand SetObservationalStudyCommand
        {
            get => new DelegateCommand(OnSetObservationalStudy);
        }

        public DelegateCommand BackToModelSelectionCommand
        {
            get => new DelegateCommand(OnBackToModelSelection);
        }

        public DelegateCommand<MouseButtonEventArgs> ShowStudyArmCommand
        {
            get => new DelegateCommand<MouseButtonEventArgs>(OnShowStudyArm);        
        }

        public DelegateCommand<MouseButtonEventArgs> ShowEndpointCommand
        {
            get => new DelegateCommand<MouseButtonEventArgs>(OnShowEndpoint);
        }

        public DelegateCommand AddStudyArm
        {
            get => new DelegateCommand(OnAddStudyArm);
        }

        public DelegateCommand<RadWindow> CloseWindowCommand
        {
            get => new DelegateCommand<RadWindow>(OnCloseWindow);
        }

        private StudyViewModel selectedStudy;
        public StudyViewModel SelectedStudy
        {
            get => selectedStudy;
            set
            {
                SetProperty(ref selectedStudy, value);
                UpdateView();
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
            UpdateView();
        }

        private void UpdateView()
        { 
            Purpose = new KeyValuePair<StudyPurposeType, string>(SelectedStudy.Design.Purpose, StudyPurposeTypeString.Resolve[SelectedStudy.Design.Purpose]);

            if (SelectedStudy.Design.Type == "Observational")
            {
                regionManager.RequestNavigate(RegionNames.StudyDesignRegion, "ObservationalStudyEditorView");
                ObservationalModel = new KeyValuePair<StudyObservationalModelType, string>(SelectedStudy.Design.ObservationalModel, StudyObservationalModelTypeString.Resolve[SelectedStudy.Design.ObservationalModel]);
                TimePerspective = new KeyValuePair<StudyTimePerspectiveType, string>(SelectedStudy.Design.TimePerspective, StudyTimePerspectiveTypeString.Resolve[SelectedStudy.Design.TimePerspective]);
            }
            else if (SelectedStudy.Design.Type == "Interventional")
            {
                regionManager.RequestNavigate(RegionNames.StudyDesignRegion, "InterventionalStudyEditorView");
                InterventionalModel = new KeyValuePair<StudyInterventionModelType, string>(SelectedStudy.Design.InterventionModel, StudyInterventionModelTypeString.Resolve[SelectedStudy.Design.InterventionModel]);
                Allocation = new KeyValuePair<StudyAllocationType, string>(SelectedStudy.Design.Allocation, StudyAllocationTypeString.Resolve[SelectedStudy.Design.Allocation]);
                StudyPhase = new KeyValuePair<StudyPhaseType, string>(SelectedStudy.Design.Phase, StudyPhaseTypeString.Resolve[SelectedStudy.Design.Phase]);
            }
            else
            {
                OnBackToModelSelection();
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

        private void OnShowStudyArm(MouseButtonEventArgs obj)
        {
            StudyArmViewModel studyArm = (obj.Source as RadGridView).SelectedItem as StudyArmViewModel;

            OpenStudyArmEditor(studyArm);
        }

        private void OnShowEndpoint(MouseButtonEventArgs obj)
        {
            OutcomeMeasureViewModel endpoint = (obj.Source as RadListBox).SelectedItem as OutcomeMeasureViewModel;
            RadWindow endpointEditorWindow = new RadWindow();            
            EndpointEditorView endpointEditorView = new EndpointEditorView();
            EndpointEditorViewModel endpointEditorViewModel = new EndpointEditorViewModel();
            endpointEditorViewModel.SelectedEndpoint = endpoint;
            endpointEditorView.DataContext = endpointEditorViewModel;
            endpointEditorWindow.Content = endpointEditorView;
            endpointEditorWindow.Loaded += (s, e) =>
            {
                endpointEditorWindow.Width = 800;
                endpointEditorWindow.Height = 800;
                endpointEditorWindow.Header = "Endpunkt-Editor";
                endpointEditorWindow.ParentOfType<Window>().ShowInTaskbar = true;
            };
            endpointEditorWindow.ShowDialog();
        }

        private bool? OpenStudyArmEditor(StudyArmViewModel studyArm)
        { 
            RadWindow studyArmEditorWindow = new RadWindow();
            studyArmEditorWindow.Header = "Studienarm-Editor";
            StudyArmEditorView studyArmEditorView = new StudyArmEditorView();
            StudyArmEditor.StudyArmEditorViewModel studyArmEditorViewModel = new StudyArmEditor.StudyArmEditorViewModel();
            studyArmEditorViewModel.SelectedStudyArm = studyArm;
            studyArmEditorView.DataContext = studyArmEditorViewModel;
            studyArmEditorWindow.Content = studyArmEditorView;
            studyArmEditorWindow.Loaded += (s, e) => { 
                studyArmEditorWindow.ParentOfType<Window>().ShowInTaskbar = true; 
            };
            studyArmEditorWindow.ShowDialog();

            return studyArmEditorWindow.DialogResult;
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

        private void OnCloseWindow(RadWindow parentWindow)
        {
            parentWindow.DialogResult = true;
            parentWindow.Close();
        }
    }
}
