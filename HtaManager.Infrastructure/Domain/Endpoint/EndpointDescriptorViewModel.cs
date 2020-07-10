using HtaManager.Infrastructure.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class EndpointDescriptorViewModel : ViewModelBase
    {
        private int id;
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public int ParentId { get; set; }

        private string abbreviation;
        public string Abbreviation
        {
            get => abbreviation;
            set => SetProperty(ref abbreviation, value);
        }

        private List<EndpointDescriptorViewModel> childList;
        public List<EndpointDescriptorViewModel> ChildList
        {
            get => childList;
            set => SetProperty(ref childList, value);
        }

        private EndpointDimensionType dimension;
        public EndpointDimensionType Dimension
        {
            get => dimension;
            set => SetProperty(ref dimension, value);
        }

        private string nameEN;
        public string NameEN
        {
            get => nameEN;
            set => SetProperty(ref nameEN, value);
        }

        private string abbreviationEN;
        public string AbbreviationEN
        {
            get => abbreviationEN;
            set => SetProperty(ref abbreviationEN, value);
        }

        public EndpointDescriptorViewModel Parent { get; set; }

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
            ChildList = new List<EndpointDescriptorViewModel>();
        }
    }
}
