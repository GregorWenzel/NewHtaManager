using HtaManager.Infrastructure;
using HtaManager.Infrastructure.Domain;
using HtaManager.Infrastructure.Mvvm;
using HtaManager.Repository.Endpoint;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Unity;
using Unity.Resolution;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace HtaManager.GUI.EndpointDescriptorSelector
{
    public class EndpointDescriptorSelectorViewModel: Infrastructure.Mvvm.ViewModelBase
    {
        private EndpointDescriptorViewModel oldEndpointDescriptor = null;
        List<EndpointDescriptor> originalEndpointDescriptorList;

        IEndpointRepository repo;

        public DelegateCommand NewEndpointCommand
        {
            get => new DelegateCommand(OnNewEndpointDescriptor);
        }

        public DelegateCommand<RadWindow> CloseWindowCommand
        {
            get => new DelegateCommand<RadWindow>(OnCloseWindow);
        }

        public DelegateCommand<EndpointDescriptorViewModel> DeleteEndpointCommand
        {
            get => new DelegateCommand<EndpointDescriptorViewModel>(OnDeleteEndpointDescriptor);
        }
        
        public DelegateCommand<SelectionChangedEventArgs> EndpointDescriptorChangedCommand
        {
            get => new DelegateCommand<SelectionChangedEventArgs>(OnEndpointDescriptorChanged);
        }

        public DelegateCommand ChildSelectionChangedCommand
        {
            get => new DelegateCommand(OnChildSelectionChanged);
        }

        public DelegateCommand ClearParentCommand
        {
            get => new DelegateCommand(OnClearParent);
        }

        private ObservableCollection<EndpointDescriptorViewModel> endpointDescriptorList;
        public ObservableCollection<EndpointDescriptorViewModel> EndpointDescriptorList
        {
            get => endpointDescriptorList;
            set => SetProperty(ref endpointDescriptorList, value);
        }

        private ObservableCollection<EndpointDescriptorViewModel> flatEndpointDescriptorList;
        public ObservableCollection<EndpointDescriptorViewModel> FlatEndpointDescriptorList
        {
            get => flatEndpointDescriptorList;
            set => SetProperty(ref flatEndpointDescriptorList, value);
        }

        int currentEndpointDescriptorId;

        private EndpointDescriptorViewModel selectedEndpointDescriptor = null;
        public EndpointDescriptorViewModel SelectedEndpointDescriptor
        {
            get => selectedEndpointDescriptor;
            set
            {
                SetProperty(ref selectedEndpointDescriptor, value);

                if (value != null)
                {
                    currentEndpointDescriptorId = SelectedEndpointDescriptor.Id;
                }
                RaisePropertyChanged("SelectedEndpointDescriptorDimension");
            }
        }
       
        private string filterString;
        public string FilterString
        {
            get => filterString;
            set
            {
                SetProperty(ref filterString, value);
                FilterTree();
            }
        }

        private void ConstructFlatList()
        {
            FlatEndpointDescriptorList.Clear();
            foreach (EndpointDescriptorViewModel item in EndpointDescriptorList)
            {
                if (item.Parent == null)
                    FlatEndpointDescriptorList.Add(item);
            }
        }

        public KeyValuePair<EndpointDimensionType, string> SelectedEndpointDescriptorDimension
        {
            get => SelectedEndpointDescriptor != null ? new KeyValuePair<EndpointDimensionType, string>(SelectedEndpointDescriptor.Dimension, EndpointDimensionTypeString.Resolve[SelectedEndpointDescriptor.Dimension]) : new KeyValuePair<EndpointDimensionType, string>(EndpointDimensionType.UNKNOWN, EndpointDimensionTypeString.Resolve[EndpointDimensionType.UNKNOWN]);
            set
            {
                SelectedEndpointDescriptor.Dimension = value.Key;
            }
        }

        private void FilterTree()
        {
            EndpointDescriptorList.Clear();

            foreach (EndpointDescriptor ep in originalEndpointDescriptorList)
            {
                EndpointDescriptorViewModel epv = new EndpointDescriptorViewModel(ep);

                foreach (EndpointDescriptor child in ep.ChildList)
                {
                    if (!IsInFilter(child))
                    {
                        epv.ChildList.Remove(epv.ChildList.First(item => item.Id == child.Id));
                    }
                }

                if (IsInFilter(epv) || epv.ChildList.Count > 0)
                {
                    EndpointDescriptorList.Add(epv);
                }
            }
        }

        private bool IsInFilter(EndpointDescriptorViewModel item)
        {
            if (string.IsNullOrEmpty(FilterString)) return true;

            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(item.Name, FilterString, CompareOptions.IgnoreCase) >= 0 || (item.Abbreviation != null && CultureInfo.CurrentCulture.CompareInfo.IndexOf(item.Abbreviation, FilterString, CompareOptions.IgnoreCase) >= 0) || CultureInfo.CurrentCulture.CompareInfo.IndexOf(item.DisplayName, FilterString, CompareOptions.IgnoreCase) >= 0;
        }

        private bool IsInFilter(EndpointDescriptor item)
        {
            if (string.IsNullOrEmpty(FilterString)) return true;

            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(item.Name, FilterString, CompareOptions.IgnoreCase) >= 0 || (item.Abbreviation != null && CultureInfo.CurrentCulture.CompareInfo.IndexOf(item.Abbreviation, FilterString, CompareOptions.IgnoreCase) >= 0);
        }

        public EndpointDescriptorSelectorViewModel(IUnityContainer container, List<EndpointDescriptor> endpointDescriptorList)
        {
            originalEndpointDescriptorList = endpointDescriptorList;

            repo = container.Resolve<IEndpointRepository>(new ParameterOverride("connectionString", Constants.ConnectionString));

            FlatEndpointDescriptorList = new ObservableCollection<EndpointDescriptorViewModel>();
            EndpointDescriptorList = new ObservableCollection<EndpointDescriptorViewModel>(originalEndpointDescriptorList.Select(item => new EndpointDescriptorViewModel(item)));            
            ConstructFlatList();
            foreach (EndpointDescriptorViewModel endpoint in EndpointDescriptorList)
            {
                endpoint.IsChanged = false;
                foreach (EndpointDescriptorViewModel child in endpoint.ChildList)
                {
                    if (child.ParentId > 0)
                    {
                        child.Parent = EndpointDescriptorList.FirstOrDefault(item => item.Id == child.ParentId);
                        child.Parent.IsChanged = false;
                        child.IsChanged = false;
                    }
                }
            }
        }

        private void OnNewEndpointDescriptor()
        {
            EndpointDescriptorViewModel newEndpointDescriptor = new EndpointDescriptorViewModel();
            EndpointDescriptorList.Add(newEndpointDescriptor);
            FlatEndpointDescriptorList.Add(newEndpointDescriptor);
            newEndpointDescriptor.Id = repo.SaveEndpointDescriptor(newEndpointDescriptor);
            SelectedEndpointDescriptor = newEndpointDescriptor;
        }

        private void OnDeleteEndpointDescriptor(EndpointDescriptorViewModel endpointDescriptor)
        {
            if (endpointDescriptor.ChildList.Count > 0)
            {
                MessageBox.Show("Das gewählte Endpunkt-Template enthält untergeordnete Endpunkte und kann daher nicht gelöscht werden.", "Löschen nicht möglich!", MessageBoxButton.OK);
            }
            else
            {
                if (MessageBox.Show("Das gewählte Endpunkt-Template wird endgültig gelöscht.\r\nSind Sie sicher?.", $"Endpunkt-Template {endpointDescriptor.Name} löschen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    EndpointDescriptorList.Remove(endpointDescriptor);
                }
            }            
        }

        private void OnEndpointDescriptorChanged(SelectionChangedEventArgs sccea)
        {
            if (oldEndpointDescriptor == SelectedEndpointDescriptor || SelectedEndpointDescriptor == null) return;

            UpdateEndpointDescriptors();

            oldEndpointDescriptor = SelectedEndpointDescriptor;
        }

        private void UpdateEndpointDescriptors()
        {
            List<EndpointDescriptorViewModel> changedEndpointDescriptorList = FlatEndpointDescriptorList.Where(item => item.IsChanged).ToList();

            foreach (EndpointDescriptorViewModel endpointDescriptor in changedEndpointDescriptorList)
            {
                endpointDescriptor.Id = repo.SaveEndpointDescriptor(endpointDescriptor);

                int index;
                if (originalEndpointDescriptorList.Any(item => item.Id == endpointDescriptor.Id))
                {
                    index = originalEndpointDescriptorList.FindIndex(item => item.Id == endpointDescriptor.Id);
                    originalEndpointDescriptorList[index] = EndpointDescriptor.FromViewModel(endpointDescriptor);
                }
                else
                {
                    foreach (EndpointDescriptor ep in originalEndpointDescriptorList)
                    {
                        if (ep.ChildList.Any(item => item.Id == endpointDescriptor.Id))
                        {
                            index = ep.ChildList.FindIndex(item => item.Id == endpointDescriptor.Id);
                            ep.ChildList[index] = EndpointDescriptor.FromViewModel(endpointDescriptor);
                        }
                    }
                }                
            }
        }

        private void OnChildSelectionChanged()
        {
            SelectedEndpointDescriptor = FlatEndpointDescriptorList.First(item => item.Id == currentEndpointDescriptorId);

            if (SelectedEndpointDescriptor.Parent == null)
            {
                if (EndpointDescriptorList.Contains(SelectedEndpointDescriptor) == false) EndpointDescriptorList.Add(SelectedEndpointDescriptor);
            }
            else
            {
                if (EndpointDescriptorList.Contains(SelectedEndpointDescriptor)) EndpointDescriptorList.Remove(SelectedEndpointDescriptor);
            }
        }
       
        private void OnClearParent()
        {
            int id = SelectedEndpointDescriptor.Id;
            EndpointDescriptorViewModel parent = SelectedEndpointDescriptor.Parent;
            SelectedEndpointDescriptor.Parent = null;
            SelectedEndpointDescriptor.ParentId = 0;
            EndpointDescriptorList.Add(SelectedEndpointDescriptor);
            FlatEndpointDescriptorList.Add(SelectedEndpointDescriptor);

            if (parent != null)
            {
                parent.ChildList.Remove(SelectedEndpointDescriptor);
            }

            SelectedEndpointDescriptor = EndpointDescriptorList.First(item => item.Id == id);     
            UpdateEndpointDescriptors();
        }

        private void OnCloseWindow(RadWindow parentWindow)
        {
            if (SelectedEndpointDescriptor != null && SelectedEndpointDescriptor.Id != 0 && SelectedEndpointDescriptor.IsChanged)
            {
                repo.SaveEndpointDescriptor(SelectedEndpointDescriptor);

            }

            parentWindow.DialogResult = true;
            parentWindow.Close();
        }
    }
}
