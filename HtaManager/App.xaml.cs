using HtaManager.GUI;
using HtaManager.GUI.SplashScreen;
using HtaManager.GUI.StudyDetail;
using HtaManager.GUI.StudyEditor;
using HtaManager.GUI.StudyGrid;
using HtaManager.GUI.StudySearch;
using HtaManager.Infrastructure;
using HtaManager.Infrastructure.Domain;
using HtaManager.MainShell;
using HtaManager.Repository;
using HtaManager.Repository.Endpoint;
using HtaManager.Repository.Loader;
using HtaManager.Shell;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;
using Unity.Resolution;

namespace HtaManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register(typeof(MainWindow).ToString(), typeof(MainWindowViewModel));
            ViewModelLocationProvider.Register(typeof(SplashScreenView).ToString(), typeof(SplashScreenViewModel));
            ViewModelLocationProvider.Register(typeof(StudyGridView).ToString(), typeof(StudyGridViewModel));
            ViewModelLocationProvider.Register(typeof(StudySearchView).ToString(), typeof(StudySearchViewModel));
            ViewModelLocationProvider.Register(typeof(StudyDetailView).ToString(), typeof(StudyDetailViewModel));
            ViewModelLocationProvider.Register(typeof(StudyEditorView).ToString(), typeof(StudyEditorViewModel));

            containerRegistry.Register<IRegistryRepository, ClinicalTrialsRegistryRepository>("ClinicalTrials");
            containerRegistry.Register<IPublicationRepository, PubmedPublicationRepository>("Pubmed");
            containerRegistry.RegisterSingleton<object, MainLoader>();
            containerRegistry.RegisterSingleton<List<EndpointDescriptor>, List<EndpointDescriptor>>();
            containerRegistry.Register<IEndpointRepository, MySqlEndpointRepository>();

            containerRegistry.Register<object, StudySearchView>("StudySearchView");
            containerRegistry.Register<object, StudyGridView>("StudyGridView");
            containerRegistry.Register<object, ModelSelectionView>("ModelSelectionView");
            containerRegistry.Register<object, InterventionalStudyEditorView>("InterventionalStudyEditorView");
            containerRegistry.Register<object, ObservationalStudyEditorView>("ObservationalStudyEditorView");

            //containerRegistry.Register<object, StudyEditorView>("StudyGridView");

        }

        protected override void OnInitialized()
        {
            StyleManager.ApplicationTheme = new SummerTheme();

            /*
            RadShell shellWindow = Container.Resolve<RadShell>();
            shellWindow.Width = 1600;
            shellWindow.Height = 900;
            shellWindow.Show();
            MainWindow = shellWindow.ParentOfType<Window>();
            MainWindow.ShowInTaskbar = true;

            RegionManager.SetRegionManager(MainWindow, Container.Resolve<IRegionManager>());
            RegionManager.UpdateRegions();
            */

            base.OnInitialized();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<GUIModule>();

            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
