using HtaManager.GUI;
using HtaManager.GUI.SplashScreen;
using HtaManager.Infrastructure.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Shell
{
    public class MainShellViewModel: ViewModelBase
    {
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public MainShellViewModel(IRegionManager regionManager)
        {
            Title = "HTA Manager";

            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(SplashScreenView));
        }
    }
}
