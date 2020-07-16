using System.Collections.Generic;

namespace econoomic_planer_X.ResourceSet
{
    public class ExternalTradingResources
    {
        public int ID { get; set; }

        public virtual List<ExternalTradingResource> TradingResourceList { get; set; }

        public ExternalTradingResources()
        {
        }

        public ExternalTradingResources Init()
        {
            TradingResourceList = new List<ExternalTradingResource>();
            return this;
        }

        public int Count()
        {
            return TradingResourceList.Count;
        }

        internal void Add(ExternalTradingResource tradingResounce)
        {
            TradingResourceList.Add(tradingResounce);
        }

        internal void Add(List<ExternalTradingResource> tradingResounces)
        {
            foreach (ExternalTradingResource tr in tradingResounces)
            {
                TradingResourceList.Add(tr);
            }
        }
    }
}
