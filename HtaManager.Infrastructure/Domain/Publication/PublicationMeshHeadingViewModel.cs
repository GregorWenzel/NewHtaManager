using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class PublicationMeshHeadingViewModel : ViewModelBase
    {
        private string ui;
        public string UI
        {
            get => ui;
            set => SetProperty(ref ui, value);
        }

        private string text;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        private bool isMajorTopic;
        public bool IsMajorTopic
        {
            get => isMajorTopic;
            set
            {
                SetProperty(ref isMajorTopic, value);
            }
        }
    }
}