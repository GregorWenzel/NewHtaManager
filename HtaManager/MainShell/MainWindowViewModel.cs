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
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(SplashScreenView));
        }
    }
}
