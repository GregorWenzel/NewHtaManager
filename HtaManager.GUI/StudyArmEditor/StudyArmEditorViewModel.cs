using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Telerik.Windows.Controls;

namespace HtaManager.GUI.StudyArmEditor
{
    public class StudyArmEditorViewModel: Infrastructure.Mvvm.ViewModelBase
    {
        private StudyArmViewModel selectedStudyArm;
        public StudyArmViewModel SelectedStudyArm
        {
            get => selectedStudyArm;
            set
            {
                SetProperty(ref selectedStudyArm, value);
                StudyArmType = new KeyValuePair<StudyArmType, string>(SelectedStudyArm.Type, StudyArmTypeString.Resolve[SelectedStudyArm.Type]);
            }
        }
        public DelegateCommand<RadWindow> CloseWindowCommand
        {
            get => new DelegateCommand<RadWindow>(OnCloseWindow);
        }

        public DelegateCommand<RadGridView> AddInterventionCommand
        {
            get => new DelegateCommand<RadGridView>(OnAddIntervention);
        }

        public DelegateCommand<RadGridView> RemoveInterventionCommand
        {
            get => new DelegateCommand<RadGridView>(OnRemoveIntervention);
        }


        private object studyArmType;
        public object StudyArmType
        {
            get => studyArmType;
            set
            {
                SetProperty(ref studyArmType, value);
                SelectedStudyArm.Type = ((KeyValuePair<StudyArmType, string>)studyArmType).Key;
            }
        }

        public StudyArmEditorViewModel()
        {

        }

        private void OnCloseWindow(RadWindow parentWindow)
        {
            parentWindow.DialogResult = true;
            parentWindow.Close();
        }

        private void OnAddIntervention(RadGridView gridView)
        {
            (gridView.ItemsSource as ObservableCollection<InterventionViewModel>).Add(new InterventionViewModel());
        }

        private void OnRemoveIntervention(RadGridView gridView)
        {
            (gridView.ItemsSource as ObservableCollection<InterventionViewModel>).Remove(gridView.SelectedItem as InterventionViewModel);
        }
    }
}
