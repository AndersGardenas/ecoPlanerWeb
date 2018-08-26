using System;
using System.Collections.Generic;

namespace econoomic_planer_X.ResourceSet
{
    public class TradingResources
    {
        public Guid ID { get; set; }
        public virtual List<TradingResource> tradingResources { get; set; }

        public TradingResources() {
            tradingResources = new List<TradingResource>();
        }

        public void Add(TradingResource tradingResource) {
            tradingResources.Add(tradingResource);
        }

        internal int Count() {
            return tradingResources.Count;
        }
    }
}
