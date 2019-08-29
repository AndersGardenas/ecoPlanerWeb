using System;
using System.Collections.Generic;

namespace econoomic_planer_X.ResourceSet
{
    public class TradingResources
    {
        public virtual ResourceTypes.ResourceType ResourceType { get; set; }
        public virtual List<TradingResource> tradingResources { get; set; }

        public TradingResources()
        {
        }

        public TradingResources(ResourceTypes.ResourceType ResourceType)
        {
            tradingResources = new List<TradingResource>();
            this.ResourceType = ResourceType;
        }

        public void Add(TradingResource tradingResource)
        {
            tradingResources.Add(tradingResource);
        }

        internal int Count()
        {
            return tradingResources.Count;
        }
    }
}
