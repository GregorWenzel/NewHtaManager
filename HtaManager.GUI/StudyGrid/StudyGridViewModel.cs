﻿using HtaManager.GUI.StudyEditor;
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
        RadWindow newStudyWindow;
        IUnityContainer container;
        IRegionManager regionManager;

        public Prism.Commands.DelegateCommand NewStudyCommand
        {
            get => new Prism.Commands.DelegateCommand(OnNewStudy);
        }

        public StudyGridViewModel(IRegionManager regionManager, IUnityContainer container): base(regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        private void OnNewStudy()
        {
            IUnityContainer childContainer = container.CreateChildContainer();

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
                ((newStudyWindow.Content as StudyEditorView).DataContext as StudyEditorViewModel).SelectedStudy = new Infrastructure.Domain.StudyViewModel();
            }
            newStudyWindow.ShowDialog();
        }
    }
}
