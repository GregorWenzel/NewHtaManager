using HtaManager.GUI.StudyEditor;
using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using Unity;
using Prism.Commands;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace HtaManager.GUI.StudyGrid
{
    public class StudyGridViewModel: RegionViewModelBase
    {
        RadWindow newStudyWindow;
        IUnityContainer container;
        IRegionManager regionManager;

        private ObservableCollection<StudyViewModel> studyList;
        public ObservableCollection<StudyViewModel> StudyList
        {
            get => studyList;
            set => SetProperty(ref studyList, value);
        }

        private StudyViewModel selectedStudy;
        public StudyViewModel SelectedStudy
        {
            get => selectedStudy;
            set => SetProperty(ref selectedStudy, value);
        }

        public DelegateCommand NewStudyCommand
        {
            get => new DelegateCommand(OnNewStudy);
        }

        public DelegateCommand StudyDoubleClickCommand
        {
            get => new DelegateCommand(OnStudyDoubleClick);
        }

        public StudyGridViewModel(IRegionManager regionManager, IUnityContainer container): base(regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;

            StudyList = new ObservableCollection<StudyViewModel>();
        }

        private void OnNewStudy()
        {
            IUnityContainer childContainer = container.CreateChildContainer();

            StudyViewModel newStudy = new Infrastructure.Domain.StudyViewModel();

            OpenStudyWindow(newStudy);
        }

        private void OpenStudyWindow(StudyViewModel study)
        { 
            if (newStudyWindow == null)
            {
                newStudyWindow = new RadWindow();
                newStudyWindow.Width = 1000;
                newStudyWindow.Height = 600;
                newStudyWindow.Loaded += (s, e) => { newStudyWindow.ParentOfType<Window>().ShowInTaskbar = true; };
                newStudyWindow.Content = new StudyEditorView();

                Prism.Regions.RegionManager.SetRegionManager(newStudyWindow, regionManager);
            }
            else
            {
                ((newStudyWindow.Content as StudyEditorView).DataContext as StudyEditorViewModel).SelectedStudy = study;
            }
            newStudyWindow.ShowDialog();

            if (newStudyWindow.DialogResult == true)
            {
                study = ((newStudyWindow.Content as StudyEditorView).DataContext as StudyEditorViewModel).SelectedStudy;
                if (StudyList.Contains(study) == false) StudyList.Add(study);
                SelectedStudy = study;
            }
        }

        private void OnStudyDoubleClick()
        {
            OpenStudyWindow(SelectedStudy);
        }
    }
}
