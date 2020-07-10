using HtaManager.Infrastructure;
using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using HtaManager.Repository;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace HtaManager.GUI.StudySearch
{
    public class StudySearchViewModel : RegionViewModelBase
    {
        IEventAggregator eventAggregator;
        IUnityContainer container;

        private string pmidSearchString;
        public string PmidSearchString
        {
            get => pmidSearchString;
            set => SetProperty(ref pmidSearchString, value);
        }

        private string nctSearchString;
        public string NctSearchString
        {
            get => nctSearchString;
            set => SetProperty(ref nctSearchString, value);
        }

        public DelegateCommand SearchByNctCommand
        {
            get => new DelegateCommand(OnNctSearch);
        }

        public DelegateCommand SearchByPmidCommand
        {
            get => new DelegateCommand(OnPmidSearch);
        }

        public DelegateCommand TranslateCommand
        {
            get => new DelegateCommand(OnTranslate);
        }

        public StudySearchViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IUnityContainer container) : base(regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.container = container;
        }

        private void OnNctSearch()
        {
            Study study = container.Resolve<IRegistryRepository>("ClinicalTrials").RequestStudy(NctSearchString);

            eventAggregator.GetEvent<SelectedStudyChangedEvent>().Publish(study);
        }


        private void OnPmidSearch()
        {
            Publication publication = container.Resolve<IPublicationRepository>("Pubmed").RequestPublication(PmidSearchString) as Publication;

            Study study;
            if (!string.IsNullOrEmpty(publication.NctId))
            {
                study = container.Resolve<IRegistryRepository>("ClinicalTrials").RequestStudy(publication.NctId);
            }
            else
            {
                study = new Study();
            }

            study.PublicationList.Add(publication);
            eventAggregator.GetEvent<SelectedStudyChangedEvent>().Publish(study);
        }

        private void OnTranslate()
        {
            eventAggregator.GetEvent<TranslateStudyEvent>().Publish();
        }
    }
}
