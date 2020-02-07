using System.Collections.Generic;

namespace econoomic_planer_X.ResourceSet
{
    public class ExternalTradingResources
    {
        public int ID { get; set; }

        public virtual List<ExternatlTradingResource> Values { get; set; }

        public ExternalTradingResources()
        {
        }

        public ExternalTradingResources Init()
        {
            Values = new List<ExternatlTradingResource>();
            return this;
        }
    }
}
