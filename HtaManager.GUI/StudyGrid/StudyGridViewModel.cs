using HtaManager.GUI.StudyEditor;
using HtaManager.Infrastructure.Mvvm;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using Unity;

namespace HtaManager.GUI.StudyGrid
{
    public class StudyGridViewModel: RegionViewModelBase
    {
        IUnityContainer container;

        public Prism.Commands.DelegateCommand NewStudyCommand
        {
            get => new Prism.Commands.DelegateCommand(OnNewStudy);
        }

        public StudyGridViewModel(IRegionManager regionManager, IUnityContainer container): base(regionManager)
        {
            this.container = container;
        }

        private void OnNewStudy()
        {
            RadWindow newStudyWindow = new RadWindow();
            newStudyWindow.Width = 1000;
            newStudyWindow.Height = 600;
            newStudyWindow.Loaded += (s, e) => { newStudyWindow.ParentOfType<Window>().ShowInTaskbar = true; };
            newStudyWindow.Content = new StudyEditorView();
            newStudyWindow.ShowDialog();
        }
    }
}
