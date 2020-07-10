using HtaManager.Infrastructure;
using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.GUI.StudyDetail
{
    public class StudyDetailViewModel: RegionViewModelBase
    {
        private StudyViewModel selectedStudy;
        public StudyViewModel SelectedStudy
        {
            get => selectedStudy;
            set => SetProperty(ref selectedStudy, value);
        }

        public StudyDetailViewModel(IRegionManager regionManager, IEventAggregator eventAggregator): base(regionManager)
        {
            eventAggregator.GetEvent<SelectedStudyChangedEvent>().Subscribe(OnSelectedStudyChanged, true);
            eventAggregator.GetEvent<TranslateStudyEvent>().Subscribe(OnTranslateStudy, true);
        }

        private void OnSelectedStudyChanged(Study study)
        {
            SelectedStudy = new StudyViewModel(study);
        }

        private void OnTranslateStudy()
        {
            if (SelectedStudy is object)
            {
                //
            }
        }
    }
}
