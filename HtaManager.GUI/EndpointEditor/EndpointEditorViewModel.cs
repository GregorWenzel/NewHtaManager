using HtaManager.GUI.EndpointDescriptorSelector;
using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace HtaManager.GUI.EndpointEditor
{
    public class EndpointEditorViewModel: Infrastructure.Mvvm.ViewModelBase
    {
        private OutcomeMeasureViewModel selectedEndpoint;
        public OutcomeMeasureViewModel SelectedEndpoint
        {
            get => selectedEndpoint;
            set
            {
                SetProperty(ref selectedEndpoint, value);
                EndpointPriority = new KeyValuePair<OutcomePriorityType, string>(SelectedEndpoint.EndpointPriority, OutcomePriorityTypeString.Resolve[SelectedEndpoint.EndpointPriority]);
            }
        }

        public Prism.Commands.DelegateCommand ChangeEndpointDescriptorCommand
        {
            get => new Prism.Commands.DelegateCommand(OnChangeEndpointDescriptor);
        }

        public DelegateCommand<RadWindow> CloseWindowCommand
        {
            get => new DelegateCommand<RadWindow>(OnCloseWindow);
        }

        private object endpointPriority;
        public object EndpointPriority
        {
            get => endpointPriority;
            set
            {
                SetProperty(ref endpointPriority, value);
                SelectedEndpoint.EndpointPriority = ((KeyValuePair<OutcomePriorityType, string>)endpointPriority).Key;
            }
        }

        private void OnChangeEndpointDescriptor()
        {
            RadWindow endpointDescriptorWindow = new RadWindow();
            endpointDescriptorWindow.Width = 800;
            endpointDescriptorWindow.Height = 800;
            endpointDescriptorWindow.Header = "Endpunkt-Template";

            EndpointDescriptorSelectorView endpointDescriptorSelectorView = new EndpointDescriptorSelectorView();
            endpointDescriptorSelectorView.Loaded += (s, e) => { 
                (endpointDescriptorSelectorView.DataContext as EndpointDescriptorSelectorViewModel).SelectedEndpointDescriptor = SelectedEndpoint.EndpointDescriptor;
                endpointDescriptorWindow.ParentOfType<Window>().ShowInTaskbar = true;
            };

            endpointDescriptorWindow.Content = endpointDescriptorSelectorView;
            endpointDescriptorWindow.ShowDialog();

            if (endpointDescriptorWindow.DialogResult == true)
            {
                this.SelectedEndpoint.EndpointDescriptor = (endpointDescriptorSelectorView.DataContext as EndpointDescriptorSelectorViewModel).SelectedEndpointDescriptor;
            }
        }

        private void OnCloseWindow(RadWindow parentWindow)
        {
            parentWindow.DialogResult = true;
            parentWindow.Close();
        }

    }
}
