using System.Collections.Generic;

namespace econoomic_planer_X.ResourceSet
{
    public class ExternalTradingResources
    {
        public int ID { get; set; }

        public virtual List<ExternatlTradingResource> TradingResourceList { get; set; }

        public ExternalTradingResources()
        {
        }

        public ExternalTradingResources Init()
        {
            TradingResourceList = new List<ExternatlTradingResource>();
            return this;
        }

        public int Count()
        {
            return TradingResourceList.Count;
        }

        internal void Add(ExternatlTradingResource tradingResounce)
        {
            TradingResourceList.Add(tradingResounce);
        }

        internal void Add(List<ExternatlTradingResource> tradingResounces)
        {
            foreach (ExternatlTradingResource tr in tradingResounces)
            {
                TradingResourceList.Add(tr);
            }
        }
    }
}
