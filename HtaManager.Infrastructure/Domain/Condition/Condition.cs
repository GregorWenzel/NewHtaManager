using Icd10Selector.Domain;

namespace HtaManager.Infrastructure.Domain
{
    public class Condition
    {
        public Icd10ItemBasic Icd10Item { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public Condition(string name)
        {
            this.Name = name;
        }
    }
}