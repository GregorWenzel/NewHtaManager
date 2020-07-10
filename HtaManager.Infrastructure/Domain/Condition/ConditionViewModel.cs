using HtaManager.Infrastructure.Mvvm;
using Icd10Selector.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtaManager.Infrastructure.Domain
{
    public class ConditionViewModel : ViewModelBase
    {
        private int id;
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string name;
        public new string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private Icd10ItemBasic icd10Item;
        public Icd10ItemBasic Icd10Item
        {
            get => icd10Item;
            set => SetProperty(ref icd10Item, value);
        }

        public ConditionViewModel(string name)
        {
            this.Name = name;
        }

        public ConditionViewModel(Condition condition)
        {
            this.Icd10Item = condition.Icd10Item;
            this.Id = condition.Id;
            this.Name = condition.Name;
        }
    }
}
