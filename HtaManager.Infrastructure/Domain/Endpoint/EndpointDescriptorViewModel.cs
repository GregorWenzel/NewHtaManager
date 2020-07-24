using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class EndpointDescriptorViewModel : ViewModelBase
    {
        public bool IsChanged { get; set; }

        public int Id { get; set; }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                IsChanged = true;
            }
        }

        public int ParentId { get; set; }

        private string abbreviation;
        public string Abbreviation
        {
            get => abbreviation;
            set
            {
                SetProperty(ref abbreviation, value);
                IsChanged = true;
            }
        }

        private ObservableCollection<EndpointDescriptorViewModel> childList;
        public ObservableCollection<EndpointDescriptorViewModel> ChildList
        {
            get => childList;
            set => SetProperty(ref childList, value);
        }

        private EndpointDimensionType dimension;
        public EndpointDimensionType Dimension
        {
            get => dimension;
            set
            {
                SetProperty(ref dimension, value);
                IsChanged = true;
            }
        }

        private string nameEN;
        public string NameEN
        {
            get => nameEN;
            set
            {
                SetProperty(ref nameEN, value);
                IsChanged = true;
            }
        }

        private string abbreviationEN;
        public string AbbreviationEN
        {
            get => abbreviationEN;
            set
            {
                SetProperty(ref abbreviationEN, value);
                IsChanged = true;
            }
        }

        public EndpointDescriptorViewModel OldParent;

        private EndpointDescriptorViewModel parent;
        public EndpointDescriptorViewModel Parent
        {
            get => parent;
            set
            {
                if (OldParent != null)
                {
                    OldParent.ChildList.Remove(this);
                }
                OldParent = Parent;
                parent = value;

                if (Parent != null && Parent.ChildList.Contains(this) == false)
                {
                    Parent.ChildList.Add(this);
                }

                IsChanged = true;
            }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get => isExpanded;
            set => SetProperty(ref isExpanded, value);
        }

        public string DisplayName
        {
            get
            {
                if (Parent is object)
                {
                    return $"{Parent.DisplayName} - " + (!string.IsNullOrEmpty(Abbreviation) ? Abbreviation : Name);
                }
                else
                {
                    if (ChildList.Count == 0)
                    {
                        return NameWithAbbreviation;
                    }
                    else
                    {
                        return !string.IsNullOrEmpty(Abbreviation) ? Abbreviation : Name;
                    }
                }
            }
        }

        public string NameWithAbbreviation
        {
            get => $"{Name}" + (!string.IsNullOrEmpty(Abbreviation) ? $" ({Abbreviation})" : "");
        }

        public bool IsSelectable
        {
            get => ChildList.Count == 0;
        }

        public EndpointDescriptorViewModel()
        {
            ChildList = new ObservableCollection<EndpointDescriptorViewModel>();
        }

        public EndpointDescriptorViewModel(EndpointDescriptor endpointDescriptor)
        {
            if (endpointDescriptor != null)
            {
                this.Id = endpointDescriptor.Id;
                this.Abbreviation = endpointDescriptor.Abbreviation;
                this.AbbreviationEN = endpointDescriptor.AbbreviationEN;
                this.ChildList = new ObservableCollection<EndpointDescriptorViewModel>(endpointDescriptor.ChildList.Select(item => new EndpointDescriptorViewModel(item)));
                this.Dimension = endpointDescriptor.Dimension;
                this.Id = endpointDescriptor.Id;
                this.Name = endpointDescriptor.Name;
                this.NameEN = endpointDescriptor.NameEN;
                this.ParentId = endpointDescriptor.ParentId;               
            }
            else
            {
                ChildList = new ObservableCollection<EndpointDescriptorViewModel>();
            }

            this.ChildList.CollectionChanged += (s, e) => { IsChanged = true; };
        }
    }
}
