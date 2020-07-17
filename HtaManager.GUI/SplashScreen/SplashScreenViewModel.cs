using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using HtaManager.Repository.Loader;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HtaManager.GUI.SplashScreen
{
    public class SplashScreenViewModel : RegionViewModelBase
    {
        readonly MainLoader loader;
        readonly IRegionManager regionManager;

        private int maxProgress;
        public int MaxProgress
        {
            get => maxProgress;
            set => SetProperty(ref maxProgress, value);
        }

        private int progress;
        public int Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }

        public DelegateCommand InitializedCommand
        {
            get => new DelegateCommand(OnInitailzed);
        }

        public SplashScreenViewModel(IRegionManager regionManager, MainLoader loader, IUnityContainer container): base(regionManager)
        {
            this.regionManager = regionManager;
            this.loader = loader;            
            loader.LoadProgressed += Loader_LoadProgressed;
            MaxProgress = loader.StepCount;
        }

        private void Loader_LoadProgressed(object sender, EventArgs e)
        {
            Progress += 1;  

            if (Progress == MaxProgress)
            {
                regionManager.RequestNavigate(RegionNames.ContentRegion, "StudyGridView");
            }
        }

        private void OnInitailzed()
        {
            loader.Load();
        }
    }
}
